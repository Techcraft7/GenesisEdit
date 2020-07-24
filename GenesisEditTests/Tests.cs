using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenesisEdit;
using System.Drawing.Imaging;
using System.IO;
using GenesisEdit.FIleHandler;
using GenesisEdit.Compiler;
using GenesisEdit.Compiler.Macros;
using System.Collections.Generic;
using Resources = GenesisEdit.Resources;

namespace GenesisEditTests
{
	/// <summary>
	/// Tests. Please create the directory C:\tmp\ or these will not work
	/// </summary>
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
				throw new Exception("Test failed!");
			}
		}

		[TestMethod]
		public void MacroTests()
		{
			Console.WriteLine("IF MACRO:\n");
			IfStatementMacro if_m = new IfStatementMacro();
			string if_m_code =
@"%IF PY <= *A0%
	;INSIDE PY <= *A0
	MOVE.W #69,D0
	;------
	%IF PX == D0%
		;INSIDE PX == D0
		MOVE.W	#$1337,D1
		;------
	%ENDIF%
%ENDIF%
%IF AAAA > A7%
	;INSIDE AAA > A7
	MOVE.W	#$1337,D1
	;------
%ELSE%
	;INSIDE ELSE OF AAA > A7
	MOVE.W	#$CAFE,D2
	;------
%ENDIF%
MOVE.L	#42,D2";
			Console.WriteLine($"\nOUTPUT: {{\n{if_m.Compile(if_m_code)}\n}}\n");
			SpriteMacro s_m = new SpriteMacro();
			Console.WriteLine("\nSPRITE MACRO:");
			Console.WriteLine(s_m.Compile("%SPRITE Player X *= *A0 WS%"));

			IfModeMacro ifm_m = new IfModeMacro();
			Console.WriteLine("\nIF MODE MACRO:");
			_ = ifm_m.Compile("%IFMODE UNSIGNED%");
			Console.WriteLine("Should be in unsigned mode:");
			Console.WriteLine(IfStatementMacro.SignedMode ? "Nope" : "YAY!");
			_ = ifm_m.Compile("%IFMODE SIGNED%");
			Console.WriteLine("Should be in signed mode:");
			Console.WriteLine(IfStatementMacro.SignedMode ? "YAY!" : "Nope");
		}

		[TestMethod]
		public void ImageValidatorTest()
		{
			//Check if image validator works
			Bitmap b = new Bitmap(1, 1);
			if (Utils.ValidateImage(b))
			{
				throw new Exception("Failed!");
			}
		}

		[TestMethod]
		public void ImageConvertTest()
		{
			const string path = @"C:\tmp\convtest.png";
			File.WriteAllBytes(path, Convert.FromBase64String(Resources.CONV_TEST));
			Bitmap b = ImageToGenesisConverter.Expand(new Bitmap(path));
			//var spSD = ImageToGenesisConverter.CompileImage(b, false);
			var bgSD = ImageToGenesisConverter.CompileImage(b, true);
			//Console.WriteLine($"SP Data: ");
			//Console.WriteLine(spSD.Item1);
			Console.WriteLine($"BG Data: ");
			Console.WriteLine(bgSD.Item1);
			Console.WriteLine($"BG Layout: ");
			Console.WriteLine(bgSD.Item2);
		}

		[TestMethod]
		public void PaletteTests()
		{
			Console.WriteLine(Utils.FormatPalettes(new ushort[4, 16]));
		}
	}
}
