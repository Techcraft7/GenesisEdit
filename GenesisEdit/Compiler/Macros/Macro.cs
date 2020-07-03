using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler.Macros
{
	internal abstract class Macro : ICompileable<string>
	{
		public virtual string Compile(string code)
		{
			string[] lines = code.Replace("\r", string.Empty).Split('\n');
			for (int i = 0; i < lines.Length; i++)
			{
				string line = Utils.RemovePadding(lines[i]);
				if (line.StartsWith("%") && line.EndsWith("%") && line.Length > 1)
				{
					if (line.Substring(1).ToUpper().StartsWith(GetPrefix().ToUpper()))
					{
						lines[i] = CompileMacro(line);
					}
				}
			}
			return string.Join("\n", lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray());
		}

		internal string[] GetArgs(string macro)
		{
			//Get first line
			macro = Utils.FirstLine(macro);
			//Remove padding
			macro = Utils.RemovePadding(macro);
			if (macro.StartsWith("%") && macro.EndsWith("%"))
			{
				//Get split by space (minus "%<PREFIX> " and the ending "%")
				List<string> l = macro.Split(' ').ToList();
				l = l.Skip(1).Select(s => s.EndsWith("%") ? s.Substring(0, s.Length - 1) : s).ToList();
				l.RemoveAll(v => string.IsNullOrWhiteSpace(v));
				return l.ToArray().Select(a => Utils.RemovePadding(a)).Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a ?? string.Empty).ToArray();
			}
			throw new InvalidOperationException("Invalid macro!");
		}

		protected void ThrowBecauseOfInvalidMacro()
		{
			throw new CompilerException($"Invalid {GetPrefix()} macro!");
		}

		public abstract string CompileMacro(string code);
		public abstract string GetPrefix();
	}
}
