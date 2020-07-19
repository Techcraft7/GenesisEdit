using GenesisEdit.Compiler.Macros;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenesisEdit.Compiler
{
	internal class Compiler
	{
		private const string FILE_NAME = "CODE";
		private const string SYSTEM_NAME = "SYSTEM.S";

		//Regexes
		private static readonly Regex REGISTER_REGEX = new Regex("(\\*?A[0-7])|(D[0-7])");
		private static readonly Regex NUMBER_REGEX = new Regex("(0x[0-9A-F]+)|([0-9]+)", RegexOptions.IgnoreCase);
		//Assembler process things
		private static readonly Tuple<string, string> ASM_CMD = new Tuple<string, string>("asm68k.exe", "/p /i /w /ov+ /oos+ /oop+ /oow+ /ooz+ /ooaq+ /oosq+ /oomq+ /ow+ %NAME%.S,%NAME%.SGROM,%NAME%");
		private static readonly Process ASSEMBLER;
		private static string ASM_STDOUT_BUF;
		//Macros to compile
		private static readonly Macro[] MACROS = new Macro[] { new IfStatementMacro(), new SpriteMacro(), new IfModeMacro() };

		//Put block macros first, then non-blocks after
		static Compiler()
		{
			MACROS = MACROS.OrderByDescending(m => Convert.ToBoolean(m.GetType().IsSubclassOf(typeof(BlockMacro)))).ToArray();
			ASSEMBLER = new Process()
			{
				StartInfo = new ProcessStartInfo()
				{
					FileName = ASM_CMD.Item1,
					Arguments = ASM_CMD.Item2.Replace("%NAME%", FILE_NAME),
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true
				}
			};
		}

		public static void Compile(List<GenesisEvent> events, List<Variable> variables, List<Sprite> sprites, ROMInfo rom, Bitmap bg1, Bitmap bg2)
		{
			//Null checks
			Utils.ThrowIfAnyNull(events, variables, sprites, rom, bg1/*, bg2*/);
			//Reset
			ImageToGenesisConverter.Reset();
			//Compile
			foreach (object obj in new object[] { events, variables, sprites })
			{
				if (obj == null)
				{
					throw new ArgumentNullException();
				}
			}
			try
			{
				long ms = DateTime.Now.Ticks / 10000;
				Utils.Log("Compiling");
				Dictionary<EventType, GenesisEvent[]> toCompile = new Dictionary<EventType, GenesisEvent[]>();
				foreach (EventType type in Enum.GetValues(typeof(EventType)))
				{
					toCompile.Add(type, events.Where(e => e.Type.Equals(type)).ToArray());
				}
				if (toCompile.TryGetValue(EventType.ON_PRESS, out GenesisEvent[] ges))
				{
					IEnumerable<GenesisEvent> vs = ges.Where(ge => ge.Button.Equals(Button.NONE));
					if (vs.Count() != 0)
					{
						throw new CompilerException($"Please specify a button for the following events:\n\t{string.Join("\n\t", vs.Select(e => e.Name))}");
					}
				}
				if (toCompile.Select(kv => kv.Value.Select(ge => ge.Name)).Any(l => l.Any(v => !Utils.IsValidIdentifier(v))))
				{
					throw new CompilerException("One or more event names were invalid!");
				}
				Dictionary<EventType, string[]> compiled = toCompile.ToDictionary(kv => kv.Key, kv => kv.Value.Select(e => e.Compile(variables)).ToArray());

				Utils.Log($"Compiling Sprites");
				IEnumerable<Sprite> invalidSprites = sprites.Where(s => !s.Validate());
				if (invalidSprites.Count() > 0)
				{
					throw new CompilerException($"The following sprites are invalid: {string.Join(", ", sprites.Select(s => s.Name))}");
				}
				var compiledSprites = sprites.Select(s => ImageToGenesisConverter.CompileImage(s.Texture, false)).ToList();
				Utils.Log($"Compiling BG's");
				Utils.Log("Expanding BG's");
				bg1 = ImageToGenesisConverter.Expand(bg1);
				bg2 = ImageToGenesisConverter.Expand(bg2);

				var compiledBGs = new List<Bitmap>() { bg1, bg2 }.Select(bg => ImageToGenesisConverter.CompileImage(bg, true)).ToList();

				Utils.Log($"Compiling Palettes");



				IEnumerable<ushort> bgPal = Utils.GetColors(bg1).Concat(Utils.GetColors(bg2)).Distinct();
				if (bgPal.Count() > 15)
				{
					throw new CompilerException($"Due to limitations, both backgrounds combined can only have 16 colors in total (1st color is always transparent)");
				}

				ushort[,] palettes = new ushort[4, 16];


				for (int c = 0; c < Math.Min(bgPal.Count(), 16); c++)
				{
					palettes[0, c] = bgPal.ToArray()[c];
				}


				Utils.Log($"Filling in templates");

				Utils.Log($"Filling in Code Template");
				string codeS = Encoding.UTF8.GetString(Convert.FromBase64String(Resources.CODE_TEMPLATE));

				Utils.Log($"Filling in events");
				foreach (EventType et in (EventType[])Enum.GetValues(typeof(EventType)))
				{
					if (!toCompile.ContainsKey(et))
					{
						continue;
					}
					if (et.Equals(EventType.ON_PRESS))
					{
						IEnumerable<Tuple<Button, string>> codeForButtons = toCompile[EventType.ON_PRESS].Select(ge => new Tuple<Button, string>(ge.Button, ge.Compile(variables)));
						Dictionary<Button, List<string>> sorted = new Dictionary<Button, List<string>>();
						foreach (var kv in codeForButtons)
						{
							if (sorted.ContainsKey(kv.Item1))
							{
								sorted[kv.Item1].Add(kv.Item2);
							}
							else
							{
								sorted.Add(kv.Item1, new List<string>() { kv.Item2 });
							}
						}
						foreach (var kv in sorted)
						{
							string buf = string.Empty;
							foreach (string code in kv.Value)
							{
								buf += code + Environment.NewLine;
							}
							codeS = codeS.Replace(Utils.EVENT_REPLACERS[et].Replace("{0}", kv.Key.ToString().ToUpper()), buf);
						}
					}
					else
					{
						string buf = string.Empty;
						foreach (GenesisEvent[] gesToCompile in toCompile.Values)
						{
							foreach (GenesisEvent ge in gesToCompile)
							{
								buf += ge.Compile(variables) + Environment.NewLine;
							}
						}
						if (et.Equals(EventType.ON_USER_INIT))
						{
							buf += Utils.InitSpriteVars(sprites);
						}
						codeS = codeS.Replace(Utils.EVENT_REPLACERS[et], buf);
					}
				}
				List<string> compiledVars = variables.Select(v => v.Compile(null)).ToList();
				compiledVars.AddRange(sprites.Select(s => string.Join(Environment.NewLine, s.GetVariables())));
				Utils.Log("Defining sprite variables");
				sprites.ForEach(s => compiledVars.AddRange(s.GetVariables()));
				Utils.Log("Filling in other data");
				codeS = Utils.ReplaceAll(codeS, new Dictionary<string, string>()
				{
					{ "%GE_NUM_BG_CHARS%", ImageToGenesisConverter.TOTAL_BG_CHARS.ToString() },
					{ "%GE_NUM_SPRITE_CHARS%", ImageToGenesisConverter.TOTAL_SP_CHARS.ToString() },
					{ "%GE_USERVAR%", string.Join(Environment.NewLine, compiledVars) },
					{ "%GE_SPRITES%", "SPRITEGFX:" +  Environment.NewLine + string.Join(Environment.NewLine, compiledSprites.Select(cs => cs.Item1)) },
					{ "%GE_BG%", "MAPGFX:" +  Environment.NewLine + string.Join(Environment.NewLine, compiledBGs.Select(cs => cs.Item1).Distinct()) },
					//BG2 not supported yet!
					{ "%GE_BG_LAYOUT%", $"CHARGFX:{Environment.NewLine}{compiledBGs.First().Item2}"/*{Environment.NewLine}CHARGFX2:{Environment.NewLine}{compiledBGs.Last().Item2}"*/ },
					{ "%GE_PALETTES%", Utils.FormatPalettes(palettes) }
				});

				Utils.Log($"Filling in System Template");
				string systemS = Encoding.UTF8.GetString(Convert.FromBase64String(Resources.SYSTEM_CODE));
				systemS = Utils.ReplaceAll(systemS, new Dictionary<string, string>()
				{
					{ "%GE_ROM_TITLE__%", rom.Title },
					{ "%GE_ROM_DATE___%", rom.AuthorAndDate },
					{ "%GE_ROM_SUBTITLE_1_____________________________%", rom.Subtitle },
					{ "%GE_ROM_SUBTITLE_2_____________________________%", rom.Subtitle2 },
					{ "%GE_ROM_PROD#%", rom.ProductNo }
				});

				Utils.Log($"Writing to file");
				string[] systemSLines = Utils.GetLines(systemS);
				string[] codeSLines = Utils.GetLines(codeS);
				for (int i = 0; i < codeSLines.Length; i++)
				{
					string line = codeSLines[i];
					if (Utils.IsFullMatch(new Regex("%[\\w_0-9#]+%", RegexOptions.IgnoreCase), line))
					{
						codeSLines[i] = string.Empty;
					}
				}
				//codeSLines = codeSLines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
				//systemSLines = systemSLines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
				File.WriteAllLines(FILE_NAME + ".S", codeSLines);
				File.WriteAllLines(SYSTEM_NAME, systemSLines);

				Utils.Log($"Assembling");
				RunAssembler();
				Utils.Log($"Assembler exited with code {ASSEMBLER.ExitCode}");
				CheckAssemblerOutput();

				Utils.Log($"Done!");
				long ms2 = DateTime.Now.Ticks / 10000;
				Utils.Log($"Compiler finished in {((double)ms2 - ms) / 1000D} seconds");
			}
			catch (CompilerException e)
			{
				_ = MessageBox.Show(e.Message);
			}
			catch (Exception e)
			{
				_ = MessageBox.Show($"Unexpected error while compiling:\n{e.GetType().Name} - {e.Message}\n{e.StackTrace}");
			}
		}

		private static void CheckAssemblerOutput()
		{
			Console.WriteLine("Output:");
			Console.WriteLine(ASM_STDOUT_BUF);
			string[] lines = Utils.GetLines(ASM_STDOUT_BUF);
			const string ERROR_IDENTIFIER = ": Error :";
			if (lines.Any(l => l.Contains(ERROR_IDENTIFIER)) || ASSEMBLER.ExitCode != 0)
			{
				throw new CompilerException($"Assembler failed!\n{string.Join("\n", lines.Where(l => l.Contains(ERROR_IDENTIFIER)))}");
			}
		}

		private static void RunAssembler()
		{
			Task t = Task.Run(() =>
			{
				Thread.CurrentThread.Name = "Assembler Thread";
				Utils.Log("Assembling", Thread.CurrentThread.Name);
				ASM_STDOUT_BUF = string.Empty;
				ASSEMBLER.Start();
				Utils.Log("Waiting for assembler to finish...", Thread.CurrentThread.Name);
				ASSEMBLER.WaitForExit();
				Utils.Log("Done!", Thread.CurrentThread.Name);
			});
			//Wait for task to complete without freezing UI
			while (!t.IsCompleted)
			{
				try
				{
					ASM_STDOUT_BUF += ASSEMBLER.StandardOutput.ReadToEnd();
				}
				catch
				{
					//Nothing
				}
				MainWindow.INSTACE.Invoke(new Action(() => Application.DoEvents()));
			}
		}

		public static string CompileMacros(string code)
		{
			Utils.Log("Compiling macros");
			string output = code;
			foreach (Macro m in MACROS)
			{
				Utils.Log($"Compiling macro: {m.GetType().Name}");
				output = m.Compile(output);
			}
			return output;
		}

		public static string ReplaceVars(string code, List<Variable> vars)
		{
			Utils.Log("Replacing variables");
			string output = code;
			Dictionary<string, string> replacers = vars.ToDictionary(v => $"%{v.Name}%", v => $"USER_{v.Name}");
			Utils.Log($"Searching for {replacers.Count} variables to replace");
			foreach (KeyValuePair<string, string> kv in replacers)
			{
				output = output.Replace(kv.Key, kv.Value);
			}
			Utils.Log($"Done!");
			return output;
		}

		//This method counts the maximum number of nested blocks in a string
		public static int CountBlocks(string code, bool countFirst = true)
		{
			int maxBlocks = 0;
			int blocks = 0;
			string[] lines = code.Replace("\r", string.Empty).Split('\n');
			foreach (BlockMacro bm in MACROS.Where(m => m.GetType().IsSubclassOf(typeof(BlockMacro))))
			{
				blocks = 0;
				for (int i = countFirst ? 0 : 1; i < lines.Length - (countFirst ? 0 : 1); i++)
				{
					string line = Utils.RemovePadding(lines[i]);
					if (line.StartsWith("%") && line.EndsWith("%") && line.Length > 1)
					{
						//If the part after the % starts with the prefix
						if (line.Substring(1).ToUpper().StartsWith(bm.GetPrefix().ToUpper()))
						{
							blocks++;
						}
						else if (line.Substring(1).ToUpper().StartsWith(bm.GetSuffix().ToUpper()))
						{
							if (blocks == 0)
							{
								throw new CompilerException("Block was not opened");
							}
							blocks--;
						}
						if (blocks > maxBlocks)
						{
							maxBlocks = blocks;
						}
					}
				}
				if (blocks != 0)
				{
					throw new CompilerException("Block was not closed");
				}
			}
			return maxBlocks;
		}

		public static bool IsValidValue(string s)
		{
			s = Utils.RemovePadding(s);
			return Utils.IsFullMatch(REGISTER_REGEX, s) || Utils.IsFullMatch(NUMBER_REGEX, s) || Utils.IsValidIdentifier(s);
		}

		public static string GetRealVariableName(string name)
		{
			name = Utils.RemovePadding(name);
			if (!IsValidValue(name))
			{
				throw new CompilerException($"Invalid value: {name}");
			}
			if (Utils.IsFullMatch(REGISTER_REGEX, name))
			{
				//If "Dereference operator" is used
				if (name.StartsWith("*"))
				{
					return $"({name.Substring(1)})";
				}
				//Compare the actual value of the register
				return name;
			}
			else if (Utils.IsFullMatch(NUMBER_REGEX, name))
			{
				return $"#{(name.StartsWith("0x") ? $"${new string(name.Skip(2).ToArray())}" : name)}";
			}
			//It is a variable name, will be compiled in Compiler.ReplaceVars
			return $"%{name}%";
		}
	}
}