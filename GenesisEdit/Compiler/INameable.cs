using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler
{
	interface INameable
	{
		string Name { get; set; }
		bool Validate();
	}
}
