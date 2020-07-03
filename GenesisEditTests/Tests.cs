﻿using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenesisEdit;
using System.Drawing.Imaging;
using System.IO;
using GenesisEdit.FIleHandler;
using GenesisEdit.Compiler;
using GenesisEdit.Compiler.Macros;
using System.Collections.Generic;

namespace GenesisEditTests
{
	[TestClass]
	public class Tests
	{
		[TestMethod]
		public void INITests()
		{
			INIFile f = new INIFile();
			f.AddSection(new INISection("Test1", new System.Collections.Generic.Dictionary<string, string>()
			{
				{ "AAAA", "Test" },
				{ "BBBB", "1337" }
			}));
			f.AddSection(new INISection("Test2", new System.Collections.Generic.Dictionary<string, string>()
			{
				{ "CCCC", "IDK what to put here this is a test" },
				{ "DDDD", "asd.py" }
			}));
			Console.WriteLine("WRITE:");
			Console.WriteLine(f.ToString());
			const string iniPath = @"C:\tmp\iniFile test.ini";
			File.WriteAllText(iniPath, f.ToString());
			Console.WriteLine("READ:");
			Console.WriteLine(INIFile.FromFile(new FileStream(iniPath, FileMode.Open)).ToString());

		}

		[TestMethod]
		public void ColorTest()
		{
			TextWriter oldOut = Console.Out;
			const string bmp = @"C:\tmp\fromWORD.png";
			Console.SetOut(new StreamWriter(@"C:\tmp\fromWORD.txt"));
			using (Bitmap b = new Bitmap(256, 256, PixelFormat.Format32bppArgb))
			{
				for (int i = 0; i <= ushort.MaxValue; i++)
				{
					ushort v = (ushort)i;
					int x = i % 256;
					int y = i / 256;
					Color c = Utils.FromUShort(v);
					Console.WriteLine($"{v,4:X} | {x,2:X},{y,2:X} -> {c,40} (Transparency: {(v & 0b1000000000000000) >> 15})");
					b.SetPixel(x, y, c);
				}
				b.Save(bmp);
			}
			Console.SetOut(new StreamWriter(@"C:\tmp\toWORD.txt"));
			using (Bitmap b = new Bitmap(bmp))
			{
				for (int y = 0; y < b.Height; y++)
				{
					for (int x = 0; x < b.Width; x++)
					{
						Console.WriteLine($"{Utils.FromColor(b.GetPixel(x, y)):X4}");
					}
				}
			}
			Console.SetOut(oldOut);
		}

		[TestMethod]
		public void GetRealVarNameTest()
		{
			Dictionary<string, string> cases = new Dictionary<string, string>()
			{
				{ "0xCAFE", "#$CAFE" },
				{ "1337", "#1337" },
				{ "*A0", "(A0)" },
				{ "A7", "A7" },
				{ "Some_Var", "%Some_Var%" }
			};
			bool fail = false;
			foreach (var kv in cases)
			{
				string v = Compiler.GetRealVariableName(kv.Key);
				if (!v.Equals(kv.Value))
				{
					Console.WriteLine($"FAILED: Got {v}, Expected {kv.Value}");
					fail = true;
				}
				Console.WriteLine($"SUCCESS: {v}");
			}
			if (fail)
			{
				throw new Exception();
			}
		}

		[TestMethod]
		public void GetArgsTest()
		{
			//Test if GetArgs removes whitespace properly
			if (new IfStatementMacro().GetArgs("%IF    A   ==   B%").Length != 3)
			{
				throw new Exception();
			}
		}

		[TestMethod]
		public void MacroTests()
		{
			Console.WriteLine("IF MACRO:");
			IfStatementMacro if_m = new IfStatementMacro();
			Console.WriteLine(if_m.Compile(
@"%IF PY == *A0%
	MOVE.W	#69,D0
	%IF PX == *D0%
		MOVE.W #$1337, D1
	%ENDIF%
%ENDIF%
%IF AAAA == *A7%
	MOVE.W #$1337, D1
%ENDIF%
MOVE.L	#42,D2"));
			SpriteMacro s_m = new SpriteMacro();
			Console.WriteLine("\nSPRITE MACRO:");
			Console.WriteLine(s_m.Compile("%SPRITE PLAYER X = D0%"));
		}
	}
}