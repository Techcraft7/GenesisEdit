using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace GenesisEdit.Compiler
{
	internal class GenesisEvent : ICompileable<List<Variable>>
	{
		public EventType Type;
		public Button Button;
		public string Name;
		public string Code
		{
			get => code;
			set => code = value ?? throw new ArgumentNullException();
		}

		private string code = string.Empty;

		public GenesisEvent(EventType type, string name, Button button = Button.NONE)
		{
			Type = type;
			Button = button;
			Name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public string Compile(List<Variable> vars)
		{
			Utils.Log($"Compiling event: {Name}");
			string output = Compiler.CompileMacros(Code);
			output = Compiler.ReplaceVars(output, vars);
			return output;
		}
	}

	internal enum Button
	{
		NONE = 0,
		UP = 1,
		DOWN,
		LEFT,
		RIGHT,
		A,
		B,
		C,
		START
	}

	internal enum EventType
	{
		ON_USER_INIT = 0,
		ON_TICK = 1,
		ON_VBI,
		ON_PRESS
	}
}