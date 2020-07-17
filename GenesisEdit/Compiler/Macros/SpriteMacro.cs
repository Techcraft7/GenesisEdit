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
			string name = args[0];
			switch (args.Length)
			{
				case 5:
					// Syntax: %SPRITE <NAME> [XY] (=|+=|-=|*=|/=) <VALUE> [BWL][SU]%
					Utils.Log("Using XY manipulation submacro");
					string xy_coord = args[1].ToUpper();
					string xy_operation = args[2];
					string xy_value = args[3];
					string xy_mode = args[4]; //(B, W, or L) + (S or U)
					List<Func<bool>> xy_validators = new List<Func<bool>>()
					{
						() => new string[] { "X", "Y" }.Contains(xy_coord),
						() => OPERATORS.Keys.Contains(xy_operation),
						() => Compiler.IsValidValue(xy_value),
						() => xy_mode.Length == 2 && new char[] { 'B', 'W', 'L' }.Contains(xy_mode[0]) && new char[] { 'S', 'U' }.Contains(xy_mode[1])
					};
					if (!Utils.Validate(xy_validators))
					{
						ThrowBecauseOfInvalidMacro();
					}
					string opCode = OPERATORS[xy_operation];
					string src = Compiler.GetRealVariableName(xy_value);
					string dst = $"GE_SPRITE_{name}_{xy_coord}";
					return $"{opCode}{("*=/=".Contains(xy_operation) ? xy_mode[1].ToString() : string.Empty)}.{xy_mode[0]} {src},{dst}";
				case 3:
					// Syntax: %SPRITE <NAME> DIR <VALUE>%
					Utils.Log("Using DIR manipulation submacro");
					List<Func<bool>> validators = new List<Func<bool>>()
					{
						() => args[1].ToUpper().Equals("DIR"),
						() => new string[] { "LEFT", "RIGHT" }.Contains(args[2].ToUpper())
					};
					if (!Utils.Validate(validators))
					{
						ThrowBecauseOfInvalidMacro();
					}
					return $"MOVE.W {args[2]}";
			}
			throw new CompilerException($"SPRITE Macro had the wrong number of arguments");
		}

		public override string GetPrefix() => "SPRITE";
	}
}
