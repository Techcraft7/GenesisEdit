using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler.Macros
{
	internal class IfModeMacro : Macro
	{
		//This macro changes the if mode from signed to unsigned
		public override string CompileMacro(string code)
		{
			string[] args = GetArgs(code);
			List<Func<bool>> funcs = new List<Func<bool>>()
			{
				() => args.Length == 1,
				() => args[0] != null,
				() => Utils.IsFullMatch(new Regex("(UN)?SIGNED", RegexOptions.IgnoreCase), args[0] ?? string.Empty)
			};
			if (!Utils.Validate(funcs))
			{
				ThrowBecauseOfInvalidMacro();
			}
			//Probably an unnecessary null check but you cant be too careful
			IfStatementMacro.SignedMode = !(args[0] ?? string.Empty).ToUpper().Contains("UN");
			return string.Empty;
		}

		public override string GetPrefix() => "IFMODE";
	}
}
