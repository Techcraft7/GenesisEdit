using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler
{
	interface ICompileable<T>
	{
		string Compile(T arg);
	}
}
