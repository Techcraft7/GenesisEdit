using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler
{
	[Serializable]
	public class CompilerException : Exception
	{
		public static int CURRENT_LINE = -1;

		public CompilerException(string error) : base(CURRENT_LINE < 0 ? $"Compiler error: {error}" : $"Compiler error on line {CURRENT_LINE + 1}: {error}") { }
		public CompilerException(int line, string error) : base($"Compiler error on line {line + 1}: {error}") { }
	}
}
