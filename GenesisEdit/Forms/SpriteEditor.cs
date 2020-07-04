using GenesisEdit.Compiler;
using GenesisEdit.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenesisEdit.Forms
{
	internal partial class SpriteEditor : Form
	{
		public List<Sprite> Sprites => SpritesList.Controls.OfType<SpriteControl>().ToList().Select(sc => sc.Sprite).ToList();

		public SpriteEditor() => InitializeComponent();

		private void OKButton_Click(object sender, EventArgs e) => Close();

		private void AddButton_Click(object sender, EventArgs e)
		{
			SpritesList.Controls.Add(new SpriteControl());
		}
	}
}
