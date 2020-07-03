using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler.Macros
{
	internal class IfStatementMacro : BlockMacro
	{
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

		public override string CompileMacro(string code)
		{
			string[] args = GetArgs(code);
			List<Func<bool>> funcs = new List<Func<bool>>()
			{
				() => args.Length == 3,
				() => Compiler.IsValidValue(args[0]),
				() => new string[] { "" }.Contains(args[1]),
				() => Compiler.IsValidValue(args[2])
			};
			if (!Utils.Validate(funcs))
			{
				ThrowBecauseOfInvalidMacro();
			}
			throw new NotImplementedException();
		}

		public override string GetPrefix() => "IF";
		public override string GetSuffix() => "ENDIF";
	}
}
