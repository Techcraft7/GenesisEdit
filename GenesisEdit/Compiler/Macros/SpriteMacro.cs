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
		//Dictionary for Operator -> Instruction mappings
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
			//Remove nulls
			for (int i = 0; i < args.Length; i++)
			{
				args[i] = args[i] ?? string.Empty;
			}
			string name = args[0];
			switch (args.Length)
			{
				case 4:
				case 5:
					//Syntax: %SPRITE <NAME> (X|Y) (=|+=|-=|*=|/=) <VALUE> [(B|W|L)(S|U)]%
					Utils.Log("Using XY manipulation submacro");
					string xy_coord = args[1].ToUpper();
					string xy_operation = args[2];
					string xy_value = args[3];
					string xy_mode = args.Length == 5 ? args[4] : "LS"; //(B, W, or L) + (S or U)
					xy_mode = xy_mode.ToUpper();
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
					string xy_opCode = OPERATORS[xy_operation];
					string xy_src = Compiler.GetRealVariableName(xy_value);
					string xy_dst = $"GE_SPRITE_{name}_{xy_coord}";
					bool xy_isMulOrDiv = "*=/=".Contains(xy_operation);
					if (xy_isMulOrDiv && xy_mode[0] == 'B')
					{
						throw new CompilerException("Multiplication and Division do not support using the BYTE size!");
					}
					return $"{xy_opCode}{(xy_isMulOrDiv ? xy_mode[1].ToString() : string.Empty)}.{xy_mode[0]} {xy_src},{xy_dst}";
				case 3:
					switch (args[1].ToUpper())
					{
						case "DIR":
							//Syntax: %SPRITE <NAME> DIR <VALUE>%
							Utils.Log("Using DIR manipulation submacro");
							List<Func<bool>> dir_validators = new List<Func<bool>>()
							{
								() => new string[] { "LEFT", "RIGHT" }.Contains(args[2].ToUpper())
							};
							if (!Utils.Validate(dir_validators))
							{
								ThrowBecauseOfInvalidMacro();
							}
							return $"MOVE.W #{(args[2].StartsWith("L") ? 1 : 0)},GE_SPRITE_{args[0]}_DIR";
					}
					throw new CompilerException("SPRITE Macro had invalid arguments. Check the syntax in the help menu");
			}
			throw new CompilerException($"SPRITE Macro had the wrong number of arguments");
		}

		public override string GetPrefix() => "SPRITE";
	}
}
