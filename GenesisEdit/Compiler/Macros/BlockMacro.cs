using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Block = System.Collections.Generic.KeyValuePair<System.Tuple<int, int>, string>;

namespace GenesisEdit.Compiler.Macros
{
	internal abstract class BlockMacro : Macro
	{
		public override string Compile(string code)
		{
			Utils.Log($"Compiling \"{GetPrefix()}\" Macro");
			string[] lines = code.Replace("\r", string.Empty).Split('\n');
			Stack<Tuple<int, int>> blocks = new Stack<Tuple<int, int>>();
			List<Tuple<int, int>> closed = new List<Tuple<int, int>>();
			Utils.Log("Building block stack");
			for (int i = 0; i < lines.Length; i++)
			{
				string line = Utils.RemovePadding(lines[i]);
				if (line.StartsWith("%") && line.EndsWith("%") && line.Length > 1)
				{
					//If the part after the % starts with the prefix
					if (line.Substring(1).ToUpper().StartsWith(GetPrefix().ToUpper()))
					{
						blocks.Push(new Tuple<int, int>(i, -1));
					}
					else if (line.Substring(1).ToUpper().StartsWith(GetSuffix().ToUpper()))
					{
						if (blocks.Count == 0)
						{
							throw new CompilerException(i, $"\"{GetPrefix()}\" macro ended without a start");
						}
						closed.Add(new Tuple<int, int>(blocks.Pop().Item1, i));
					}
				}
			}
			if (blocks.Count > 0)
			{
				throw new CompilerException($"\"{GetPrefix()}\" macro had no closer");
			}
			Utils.Log("Checking if any blocks were found");
			//Exit recursion
			if (closed.Count == 0)
			{
				Utils.Log($"Done compiling block macro \"{GetPrefix()}\"");
				return code;
			}
			//Convert to text
			//Sort by number of blocks inside, the inner most blocks should be first
			Block first = closed.OrderBy(b => Compiler.CountBlocks(string.Join("\n", lines.Skip(b.Item1).Take(b.Item2 - b.Item1 + 1)), false)).ToDictionary(t => t, t => string.Join("\n", lines.Skip(t.Item1).Take(t.Item2 - t.Item1 + 1))).First();
			//replace blocks
			CompilerException.CURRENT_LINE = first.Key.Item1;
			string compiled = CompileMacro(first.Value);
			CompilerException.CURRENT_LINE = -1;
			lines[first.Key.Item1] = compiled;
			for (int i = first.Key.Item1 + 1; i <= first.Key.Item2; i++)
			{
				lines[i] = string.Empty;
			}
			//Compile next blocks recursively
			Utils.Log("Found sub-blocks! Compiling recursively");
			string ret = string.Join("\n", lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray());
			return Compile(ret);
		}

		public abstract string GetSuffix();
	}
}
