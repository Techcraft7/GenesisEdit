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
		public CompilerException(string error) : base($"Compiler error: {error}") { }
		public CompilerException(int line, string error) : base($"Compiler error on line {line + 1}: {error}") { }
	}
}
