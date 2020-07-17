using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler
{
	internal class Sprite : INameable
	{
		private string name = null;
		public string Name { get => name; set => name = value ?? $"Sprite_{new Random().Next():X}"; }
		public Bitmap Texture;

		public Sprite(string name) => Name = name ?? $"Sprite_{new Random().Next():X}";

		public bool Validate()
		{
			//temp bitmap to prevent NRE's
			using (Bitmap temp = new Bitmap(1, 1))
			{
				return Utils.Validate(new List<Func<bool>>()
				{
					() => Texture != null,
					() => new int[] { 1 * 8, 2 * 8, 3 * 8, 4 * 8}.Contains(Texture == null ? temp.Width : Texture.Width),
					() => new int[] { 1 * 8, 2 * 8, 3 * 8, 4 * 8}.Contains(Texture == null ? temp.Height : Texture.Height),
					() => Utils.ValidateImage(Texture ?? temp)
				});
			}
		}

		public string[] GetVariables() => new string[]
		{
			"GE_SPRITE_{0}_X",
			"GE_SPRITE_{0}_Y",
			"GE_SPRITE_{0}_DIR",
			"GE_SPRITE_{0}_SIZE",
		};
	}
}
