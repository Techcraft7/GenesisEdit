using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler
{
	internal enum VariableType
	{
		BYTE,
		WORD,
		LONG,
	}
	internal abstract class Variable : ICompileable<object>
	{
		public int Length { get; private set; }
		public string Name
		{
			get => name;
			set => name = value ?? throw new ArgumentNullException();
		}
		private string name;

		public Variable(string name = null, int length = 1)
		{
			//Set length if > 0 or throw
			Length = length > 0 ? length : throw new ArgumentException("Invalid variable length"); ;
			//If name is null give a random name
			Name = name ?? $"Var_{new Random().Next():X}";
		}

		public abstract VariableType GetVariableType();
		public abstract char GetVariableTypeChar();

		public string Compile(object _) => $"USER_{Name}:\t\tRC.{GetVariableTypeChar()}\t{Length}";
	}

	internal class VarLong : Variable
	{
		public VarLong(string name = null, int length = 1) : base(name, length) { }

		public override VariableType GetVariableType() => VariableType.LONG;
		public override char GetVariableTypeChar() => 'L';
	}
	internal class VarWord : Variable
	{
		public VarWord(string name = null, int length = 1) : base(name, length) { }

		public override VariableType GetVariableType() => VariableType.WORD;
		public override char GetVariableTypeChar() => 'W';
	}
	internal class VarByte : Variable
	{
		public VarByte(string name = null, int length = 1) : base(name, length) { }

		public override VariableType GetVariableType() => VariableType.BYTE;
		public override char GetVariableTypeChar() => 'B';
	}
}