using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler.Macros
{
	internal class SpriteMacro : Macro
	{
		// Dictionary for Operator -> Instruction mappings
		private static readonly Dictionary<string, string> OPERATORS = new Dictionary<string, string>()
		{
			{ "=", "MOVE" },
			{ "+=", "ADD" },
			{ "-=", "SUB" },
			{ "*=", "MUL" },
			{ "/=", "DIV" }
		};

		public override string CompileMacro(string code)
		{
			string[] args = GetArgs(code);
			// Remove nulls
			for (int i = 0; i < args.Length; i++)
			{
				args[i] = args[i] ?? string.Empty;
			}
			switch (args.Length)
			{
				case 5:
					// Syntax: SPRITE [XY] (=|+=|-=|*=|/=) <VALUE> [BWL][SU]
					Utils.Log("Using XY manipulation submacro");
					string name = args[0];
					string coord = args[1].ToUpper();
					string operation = args[2];
					string value = args[3];
					string mode = args[4]; //(B, W, or L) + (S or U)
					List<Func<bool>> validators = new List<Func<bool>>()
					{
						() => new string[] { "X", "Y" }.Contains(coord),
						() => OPERATORS.Keys.Contains(operation),
						() => Compiler.IsValidValue(value),
						() => mode.Length == 2 && new char[] { 'B', 'W', 'L' }.Contains(mode[0]) && new char[] { 'S', 'U' }.Contains(mode[1])
					};
					if (!Utils.Validate(validators))
					{
						ThrowBecauseOfInvalidMacro();
					}
					string opCode = OPERATORS[operation];
					string src = Compiler.GetRealVariableName(value);
					string dst = $"GE_SPRITE_{name}_{coord}";
					return $"{opCode}{("*=/=".Contains(operation) ? mode[1].ToString() : string.Empty)}.{mode[0]} {src},{dst}";
				case 3:
					Utils.Log("Using DIR manipulation submacro");
					switch(args[1])
					{
						case "DIR":
							return "";
					}
					ThrowBecauseOfInvalidMacro();
					break;		
			}
			throw new CompilerException($"SPRITE Macro had the wrong number of arguments");
		}

		public override string GetPrefix() => "SPRITE";
	}
}
