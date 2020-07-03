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
			string[] lines = code.Replace("\r", string.Empty).Split('\n');
			Stack<Tuple<int, int>> blocks = new Stack<Tuple<int, int>>();
			List<Tuple<int, int>> closed = new List<Tuple<int, int>>();
			Console.WriteLine($"Building \"{GetPrefix()}\" Block Stack");
			for (int i = 0; i < lines.Length; i++)
			{
				string line = Utils.RemovePadding(lines[i]);
				if (line.StartsWith("%") && line.EndsWith("%") && line.Length > 1)
				{
					// If the part after the % starts with the prefix
					if (line.Substring(1).ToUpper().StartsWith(GetPrefix().ToUpper()))
					{
						Console.WriteLine($"Push line {i + 1}: {line}");
						blocks.Push(new Tuple<int, int>(i, -1));
					}
					else if (line.Substring(1).ToUpper().StartsWith(GetSuffix().ToUpper()))
					{
						if (blocks.Count == 0)
						{
							throw new CompilerException(i, $"\"{GetPrefix()}\" macro ended without a start");
						}
						Console.WriteLine($"Pop line {i + 1}: {line}");
						closed.Add(new Tuple<int, int>(blocks.Pop().Item1, i));
					}
				}
			}
			if (blocks.Count > 0)
			{
				throw new CompilerException($"\"{GetPrefix()}\" macro had no closer");
			}
			Console.WriteLine("Compiling blocks");
			// Convert to text
			// Sort by number of blocks inside, the inner most blocks should be first
			Dictionary<Tuple<int, int>, string> closed2 = closed.OrderByDescending(b => Compiler.CountBlocks(string.Join("\n", lines.Skip(b.Item1).Take(b.Item2 - b.Item1 + 1)), GetPrefix(), GetSuffix(), false)).ToDictionary(t => t, t => string.Join("\n", lines.Skip(t.Item1).Take(t.Item2 - t.Item1 + 1)));
			// replace blocks
			foreach (Block kv in closed2)
			{
				string compiled = CompileMacro(kv.Value);
				// set line in
				lines[kv.Key.Item1] = compiled;
				// replace extra lines with empty lines, will be removed later
				for (int i = kv.Key.Item1 + 1; i <= kv.Key.Item2; i++)
				{
					lines[i] = string.Empty;
				}
			}
			lines = lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
			return string.Join("\n", lines);
		}

		public abstract string GetSuffix();
	}
}
