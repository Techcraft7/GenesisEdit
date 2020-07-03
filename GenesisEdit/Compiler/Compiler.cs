using GenesisEdit.Compiler.Macros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler
{
	internal class Compiler
	{
		// Regexes
		private static readonly Regex REGISTER_REGEX = new Regex("(\\*?A[0-7])|(D[0-7])");
		private static readonly Regex NUMBER_REGEX = new Regex("(0x[0-9A-F]+)|([0-9]+)", RegexOptions.IgnoreCase);
		// Command line args to be passed in to the assembler
		private static readonly Tuple<string, string> ASM_CMD = new Tuple<string, string>("asm68k", "/p /i /w /ov+ /oos+ /oop+ /oow+ /ooz+ /ooaq+ /oosq+ /oomq+ /ow+ %NAME%.S,%NAME%.BIN,%NAME%");
		// Macros to compile
		private static readonly Macro[] MACROS = new Macro[] { new IfStatementMacro(), new SpriteMacro() };

		//Put block macros first, then non-blocks after
		static Compiler() => MACROS = MACROS.OrderByDescending(m => Convert.ToBoolean(m.GetType().IsSubclassOf(typeof(BlockMacro)))).ToArray();

		public static void Compile(List<GenesisEvent> events, List<Variable> variables)
		{
			Dictionary<EventType, GenesisEvent[]> toCompile = new Dictionary<EventType, GenesisEvent[]>();
			foreach (EventType type in Enum.GetValues(typeof(EventType)))
			{
				toCompile.Add(type, events.Where(e => e.Type.Equals(type)).ToArray());
			}
			Dictionary<EventType, string[]> compiled = toCompile.ToDictionary(kv => kv.Key, kv => kv.Value.Select(e => e.Compile(variables)).ToArray());
		}

		public static string CompileMacros(string code)
		{
			string output = code;
			foreach (Macro m in MACROS)
			{
				output = m.Compile(output);
			}
			return output;
		}

		public static string ReplaceVars(string code, List<Variable> vars)
		{
			string output = code;
			Dictionary<string, string> replacers = vars.ToDictionary(v => $"%{v.Name}%", v => $"USER_{v.Name}");
			foreach (KeyValuePair<string, string> kv in replacers)
			{
				output = output.Replace(kv.Key, kv.Value);
			}
			return output;
		}

		// This method counts the maximum number of nested blocks in a string
		public static int CountBlocks(string code, string prefix, string suffix, bool countFirst = true)
		{
			int maxBlocks = 0;
			int blocks = 0; 
			string[] lines = code.Replace("\r", string.Empty).Split('\n');
			for (int i = countFirst ? 0 : 1; i < lines.Length - (countFirst ? 0 : 1); i++)
			{
				string line = Utils.RemovePadding(lines[i]);
				if (line.StartsWith("%") && line.EndsWith("%") && line.Length > 1)
				{
					// If the part after the % starts with the prefix
					if (line.Substring(1).ToUpper().StartsWith(prefix.ToUpper()))
					{
						blocks++;
					}
					else if (line.Substring(1).ToUpper().StartsWith(suffix.ToUpper()))
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
