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
	public partial class PalEditor : Form
	{
		private bool first = true;
		public Color[,] Palettes { get; } = new Color[4, 15];

		public PalEditor()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
			ColorSelector.SelectedIndex = 0;
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void ColorSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (first)
			{
				first = false;
				return;
			}
			if (CD.ShowDialog() == DialogResult.OK)
			{
				//PXCYY
				int p = int.Parse(ColorSelector.SelectedItem.ToString().Substring(1, 1)) - 1;
				int c = int.Parse(ColorSelector.SelectedItem.ToString().Substring(3)) - 1;
				Color color = Utils.FromUShort(Utils.FromColor(CD.Color));
				ColorPreview.BackColor = color;
				Palettes[p, c] = color;
			}
		}
	}
}
