﻿using GenesisEdit.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpriteData = System.Tuple<string, string>;
using CharCompactData = System.Tuple<bool, bool, int>;
using GenesisEdit.Forms;

namespace GenesisEdit.Compiler
{
	using static ProgressHelper;
	internal static class ImageToGenesisConverter
	{
		public static ushort[] PAL_BG1 { get; private set; }
		public static ushort[] PAL_BG2 { get; private set; }
		public static List<ushort[,]> ALL_CHARS = new List<ushort[,]>();
		public static int TOTAL_BG_CHARS { get; private set; }  = 0;
		public static int TOTAL_SP_CHARS { get; private set; }  = 0;
		public static int TOTAL_CHARS { get => TOTAL_BG_CHARS + TOTAL_SP_CHARS + 1; } //include blank char

		public static void Reset()
		{
			ALL_CHARS.Clear();
			TOTAL_BG_CHARS = 0;
			TOTAL_SP_CHARS = 0;
			PAL_BG1 = null;
			PAL_BG2 = null;
		}

		//Takes in a bitmap and returns the assembly version of it
		//Item1 = The image data
		//Ex: DC.B $DE,$AD,$BE,$EF...
		//Item2 = The image layout (null if bgMode = false)
		//Ex: DC.W $0003,$0004,$0001,$0001
		//Item3 = The start index of the image
		public static SpriteData CompileImage(Bitmap b, bool bgMode)
		{
			b = b ?? throw new ArgumentNullException(nameof(b));

			bool valid = Utils.ValidateImage(b, out Color[] pallete);
			if (!valid)
			{
				throw new ArgumentException("Inavlid image");
			}

			List<byte[,]> chars = GetChars(b, out string layout, !bgMode).Select(c => GetImageBytes(c, pallete)).ToList();

			if (bgMode)
			{
				if (TOTAL_BG_CHARS == 0)
				{
					if (PAL_BG1 == null)
					{
						PAL_BG1 = pallete.Select(c => Utils.FromColor(c)).ToArray();
					}
					else
					{
						throw new Exception("Failed to compile bg pallete 1! This should NOT happen!");
					}
				}
				else
				{
					if (PAL_BG2 == null)
					{
						PAL_BG2 = pallete.Select(c => Utils.FromColor(c)).ToArray();
					}
					else
					{
						throw new Exception("You can only compile 2 BG's!");
					}
				}
				TOTAL_BG_CHARS += chars.Count;
			}
			else
			{
				TOTAL_SP_CHARS += chars.Count;
			}

			//If there are more than 255 characters (including blank char)
			if (TOTAL_CHARS > 0xFF)
			{
				throw new InvalidOperationException("Too many characters!");
			}

			string data = string.Empty;

			for (int i = 0; i < chars.Count; i++)
			{
				for (int y = 0; y < 8; y++)
				{
					data += "\t\tDC.B\t";
					//2 pixels/byte
					for (int x = 0; x < 8; x += 2)
					{
						data += $"${chars[i][x, y]:X2}";
						if (x != 6)
						{
							data += ", ";
						}
					}
					data += Environment.NewLine;
				}
				data += Environment.NewLine;
			}

			return new SpriteData(data, layout);
		}

		public static byte[,] GetImageBytes(Bitmap b, Color[] pallete)
		{
			b = b ?? throw new ArgumentNullException(nameof(b));

			byte[,] imageBytes = new byte[b.Width, b.Height];
			Utils.ForeachPixel(b, (x, y) =>
			{
				//skip odd pixels on x
				if (x % 2 == 1)
				{
					return;
				}
				byte p1 = (byte)pallete.ToList().IndexOf(b.GetPixel(x, y));
				byte p2 = (byte)pallete.ToList().IndexOf(b.GetPixel(x + 1, y));
				imageBytes[x, y] = (byte)(p1 << 4 | p2);
			});

			return imageBytes;
		}

		public static List<Bitmap> GetChars(Bitmap b, out string layout, bool spriteMode = true)
		{
			Utils.Log("Getting characters");
			b = b ?? throw new ArgumentNullException(nameof(b));
			layout = string.Empty;

			List<Bitmap> chars = new List<Bitmap>();
			//Sprite:
			//0 3 6
			//1 4 7
			//2 5 8
			//BG:
			//0 1 2
			//3 4 5
			//6 7 8

			Utils.Log("Splitting");
			ACTIONS += (b.Width * b.Height) / 64;
			for (int y = 0; y < b.Height; y += 8)
			{
				for (int x = 0; x < b.Width; x += 8)
				{
					UpdateProgress($"Splitting: {chars.Count} chars found!");
					Bitmap c = new Bitmap(8, 8, PixelFormat.Format32bppArgb);
					Utils.ForeachPixel(c, (cx, cy) => c.SetPixel(cx, cy, b.GetPixel(x + cx, y + cy)));
					chars.Add(c);
					PROGRESS++;
				}
			}
			if (spriteMode)
			{
				UpdateProgress("Converting to sprite format");
				Utils.Log("Converting to sprite format");
				//The first row are multiples of 3
				//the second row are multiples of 3 + 1
				//this means if we sort it by the index
				//mod the width we get the correct indexes
				//EX: Enumerable.Range(0, 9).OrderBy(v => v % 3) -> { 0, 3, 6, 1, 4, 7, 2, 5, 8 }
				//Then we use the indexes to reorder the list
				int[] indexes = Enumerable.Range(0, chars.Count).OrderBy(i => i % (b.Width / 8)).ToArray();
				Bitmap[] temp = new Bitmap[chars.Count];
				for (int i = 0; i < chars.Count; i++)
				{
					UpdateProgress($"Transforming {i + 1} -> {indexes[i] + 1}");
					temp[i] = chars[indexes[i]];
					PROGRESS++;
				}
				layout = null; //sprites dont need this
				return temp.ToList();
			}

			Utils.Log($"Compacting {chars.Count} chars...");

			//If we have reached here then we need to 'collapse'
			//the characters to save memory
			//$00XX = no flip
			//$10XX = v flip
			//$08XX = h flip
			//$18XX = v & h flip

			Utils.Log("Seaching for matches");

			List<CharCompactData> compacted = new List<CharCompactData>();
			UpdateProgress("Compacting");
			ACTIONS += chars.Count;
			for (int i = 0; i < chars.Count; i++)
			{
				UpdateProgress($"Compacting char {i + 1}");
				if (i == 0)
				{
					compacted.Add(new CharCompactData(false, false, 0));
					continue;
				}
				int match = i;
				bool vFlip = false;
				bool hFlip = false;
				for (int j = 0; j < chars.Count; j++)
				{
					//Skip if same char or if first char
					if (j == i)
					{
						continue;
					}
					int m = DoesMatch(chars[i], chars[j]);
					switch (m)
					{
						case 0: //no match, continue searching
							continue;
						case 1: //no flip
							break;
						case 2:
							vFlip = true;
							break;
						case 3:
							hFlip = true;
							break;
						case 4:
							vFlip = hFlip = true;
							break;
					}
					Utils.Log("Found match!");
					match = j;
					break;
				}
				Utils.Log($"Char {i} matches {match}. HF: {hFlip} VF: {vFlip}");
				compacted.Add(new CharCompactData(hFlip, vFlip, match));
				PROGRESS++;
			}
			//Get all chars that arent indexed in the compacted
			IEnumerable<int> toRemove = Enumerable.Range(0, chars.Count).Where(index => !compacted.Select(cd => cd.Item3).Contains(index)).Select(index => index);
			ACTIONS += toRemove.Count();
			UpdateProgress("Collapsing");
			Utils.Log($"Removing {toRemove.Count()} duplicate chars");
			for (int i = 0; i < toRemove.Count(); i++)
			{
				int rem = toRemove.ToArray()[i];
				chars.RemoveAt(rem);
				//subtract one from the indexes that are more than rem
				//If you dont create a new Enumerable it resets in the
				//next loop iteration? wierd but ok
				toRemove = toRemove.ToArray().Select(v => v > rem ? v - 1 : v);
				//now apply the same concept to the comacted data
				compacted = compacted.Select(cd => new CharCompactData(cd.Item1, cd.Item2, cd.Item3 > rem ? cd.Item3 - 1 : cd.Item3)).ToList();
				UpdateProgress($"Collapsing: {toRemove.Count()} left to remove");
				PROGRESS++;
			}

			Utils.Log($"Done {chars.Count} chars left!");

			UpdateProgress("Adding chars");

			int h = b.Height / 8;
			int w = b.Width / 8;

			for (int y = 0; y < h; y++)
			{
				layout += "\t\tDC.W\t";
				for (int x = 0; x < w; x++)
				{
					int i = (x % w) + (y * w);
					layout += $"${(compacted[i].Item2 ? "1" : "0")}{(compacted[i].Item1 ? "8" : "0")}{(compacted[i].Item3 + 1):X2}";
					//if not last
					if (x != w - 1)
					{
						layout += ", ";
					}
				}
				layout += Environment.NewLine;
			}
			PROGRESS++;
			return chars;
		}

		//Expand a bitmap to fit VDP plane
		public static Bitmap Expand(Bitmap b)
		{
			b = b ?? throw new ArgumentNullException(nameof(b));
			Bitmap temp = new Bitmap(64 * 8, 32 * 8);
			for (int y = 0; y < temp.Height; y++)
			{
				for (int x = 0; x < temp.Width; x++)
				{
					temp.SetPixel(x, y, b.GetPixel(x % b.Width, y % b.Height));
				}
			}
			return temp;
		}

		//0 = no match
		//1 = no flip
		//2 = v flip
		//3 = h flip
		//4 = v & h flip
		private static int DoesMatch(Bitmap c1, Bitmap c2)
		{
			Bitmap temp;
			List<RotateFlipType> types = new List<RotateFlipType>()
			{
				RotateFlipType.RotateNoneFlipNone,
				RotateFlipType.RotateNoneFlipY,
				RotateFlipType.RotateNoneFlipX,
				RotateFlipType.RotateNoneFlipXY,
			};
			foreach (RotateFlipType rft in types)
			{
				temp = (Bitmap)c1.Clone();
				temp.RotateFlip(rft);
				if (DoCharsMatch(temp, c2)) //if match found
				{
					return types.IndexOf(rft) + 1;
				}
			}
			return 0;
		}

		public static bool DoCharsMatch(Bitmap a, Bitmap b)
		{
			List<ushort> aBytes = new List<ushort>();
			List<ushort> bBytes = new List<ushort>();
			Utils.ForeachPixel(a, (x, y) => aBytes.Add(Utils.FromColor(a.GetPixel(x, y))));
			Utils.ForeachPixel(b, (x, y) => bBytes.Add(Utils.FromColor(b.GetPixel(x, y))));
			if (aBytes.Count != bBytes.Count)
			{
				return false;
			}
			for (int i = 0; i < aBytes.Count; i++)
			{
				if (aBytes[i] != bBytes[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
