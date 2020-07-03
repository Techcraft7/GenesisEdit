using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler.Macros
{
	internal class IfStatementMacro : BlockMacro
	{
		//Mode
		public static bool SignedMode = true;
		//These two are used to create labels
		private static readonly Random RNG = new Random();
		private static readonly List<ulong> USED_IF_IDS = new List<ulong>();

		private static ulong GetNextID()
		{
			byte[] buffer = new byte[8];
			do
			{
				RNG.NextBytes(buffer);
			}
			while (USED_IF_IDS.Contains(BitConverter.ToUInt64(buffer, 0)));
			return BitConverter.ToUInt64(buffer, 0);
		}

		//Use the opposite operation because if the condition
		//is false we jump over the coding inside the if 
		private static readonly Dictionary<string, string> OPERATORS_SIGNED = new Dictionary<string, string>()
		{
			{ "==", "BNE" },
			{ "!=", "BEQ" },
			{ "<=", "BGT" },
			{ ">=", "BLT" },
			{ "<", "BGE" },
			{ ">", "BLE" }
		};

		private static readonly Dictionary<string, string> OPERATORS_UNSIGNED = new Dictionary<string, string>()
		{
			{ "==", "BNE" },
			{ "!=", "BEQ" },
			{ "<=", "BHS" },
			{ ">=", "BLO" },
			{ "<", "BGE" },
			{ ">", "BLE" }
		};

		public override string CompileMacro(string code)
		{
			string[] args = GetArgs(code);
			string[] lines = Utils.GetLines(code).Select(l => Utils.RemovePadding(l)).ToArray();
			//Remove first and last lines (%IF ...% and %ENDIF%)
			string inside = string.Join("\n", Utils.SkipFirstAndLast(lines, 1));
			List<Func<bool>> funcs = new List<Func<bool>>()
			{
				() => args.Length == 3,
				() => Compiler.IsValidValue(args[0]),
				() => OPERATORS_SIGNED.Keys.Contains(args[1]),
				() => Compiler.IsValidValue(args[2]),
				() => new int[] { 0, 1 }.Contains(lines.Where(l => l.Equals("%ELSE%")).Count()),
			};
			if (!Utils.Validate(funcs))
			{
				ThrowBecauseOfInvalidMacro();
			}
			//CMP A,B
			//B <OPERATOR> A 
			string comparer = (SignedMode ? OPERATORS_SIGNED : OPERATORS_UNSIGNED)[args[1]];
			string label = $"GE_IF_SKIP{GetNextID():X}";
			string op1 = Compiler.GetRealVariableName(args[2]);
			string op2 = Compiler.GetRealVariableName(args[0]);
			if (lines.Contains("%ELSE%"))
			{
				string label2 = $"GE_ELSE_SKIP{GetNextID():X}";
				string[] split = inside.Split(new string[] { "%ELSE%" }, StringSplitOptions.None);
				if (split.Length != 2)
				{
					ThrowBecauseOfInvalidMacro();
				}
				//CMP args[2],args[0]
				//Bcc label
				//split[0] ;if true
				//BRA label2
				//label:
				//split[1] ;if false
				//label2:
				return $"CMP {op1},{op2}\n{comparer} {label}\n{split[0]}\nBRA {label2}\n{label}:\n{split[1]}\n{label2}:\n";
			}
			//CMP args[2],args[0]
			//Bcc label
			//inside
			//label:
			return $"CMP {op1},{op2}\n{comparer} {label}\n{inside}\n{label}:\n";
		}

		public override string GetPrefix() => "IF";
		public override string GetSuffix() => "ENDIF";
	}
}
