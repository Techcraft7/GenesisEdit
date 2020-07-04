using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler
{
	internal class Sprite : ICompileable<object>
	{
		public string Name;
		public Bitmap Texture;

		public Sprite(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

		public string Compile(object arg)
		{
			throw new NotImplementedException();
		}
	}
}
