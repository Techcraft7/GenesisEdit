using GenesisEdit.Compiler.Macros;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
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
		// Regexes
		private static readonly Regex REGISTER_REGEX = new Regex("(\\*?A[0-7])|(D[0-7])");
		private static readonly Regex NUMBER_REGEX = new Regex("(0x[0-9A-F]+)|([0-9]+)", RegexOptions.IgnoreCase);
		// Assembler process things
		private static readonly Tuple<string, string> ASM_CMD = new Tuple<string, string>("asm68k.exe", "/p /i /w /ov+ /oos+ /oop+ /oow+ /ooz+ /ooaq+ /oosq+ /oomq+ /ow+ %NAME%.S,%NAME%.BIN,%NAME%");
		private static readonly Process ASSEMBLER;
		private static string ASM_STDOUT_BUF;
		// Macros to compile
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
					Arguments = ASM_CMD.Item2,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};
		}

		public static void Compile(List<GenesisEvent> events, List<Variable> variables)
		{
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
				Utils.Log($"Compiling Palettes");
				Utils.Log($"Filling in templates");
				Utils.Log($"Filling in Code Template");
				Utils.Log($"Filling in System Template");
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
			ASM_STDOUT_BUF = ASSEMBLER.StandardOutput.ReadToEnd();
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
				ASM_STDOUT_BUF = string.Empty;
				ASSEMBLER.Start();
				ASSEMBLER.WaitForExit();
			});
			//Wait for task to complete without freezing UI
			while (!t.IsCompleted)
			{
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

		// This method counts the maximum number of nested blocks in a string
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
						// If the part after the % starts with the prefix
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
			// It is a variable name, will be compiled in Compiler.ReplaceVars
			return $"%{name}%";
		}
	}
}