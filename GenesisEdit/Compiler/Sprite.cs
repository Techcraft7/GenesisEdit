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
		public byte Palette = 1;
		public string Name { get => name; set => name = value ?? $"Sprite_{new Random().Next():X}"; }
		public Bitmap Texture;

		public Sprite(string name) => Name = name;

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
			"GE_SPRITE_{0}_PAL",
		}.Select(v => v.Replace("{0}", Name) + ":\tRS.W\t1").ToArray();

		public string InitVars() => string.Join(Environment.NewLine, GetVariables().Select(v => $"\t\tMOVE.W #{(v.Split('\t')[0].EndsWith("PAL") ? $"S_PAL{Palette}" : "0")},{v.Split('\t')[0]}")) + Environment.NewLine;
	}
}
