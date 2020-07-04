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
	public partial class BackgroundEditor : Form
	{
		private OpenFileDialog openDialog = new OpenFileDialog()
		{
			Filter = Utils.AutoFilter("png", "jpg", "bmp"),
			Title = "Open Image"
		};

		public BackgroundEditor() => InitializeComponent();

		private void OKButton_Click(object sender, EventArgs e) => Close();
		private void BG1Box_Click(object sender, EventArgs e) => SetImage(ref BG1Box);
		private void BG2Box_Click(object sender, EventArgs e) => SetImage(ref BG2Box);

		private void SetImage(ref PictureBox pb)
		{
			bool retry = false;
			do
			{
				switch (openDialog.ShowDialog())
				{
					case DialogResult.OK:
						break;
					case DialogResult.Cancel:
						return;
					default:
						retry = true;
						break;
				}
			}
			while (retry);
			try
			{
				Bitmap b = new Bitmap(openDialog.FileName);
				List<Func<bool>> funcs = new List<Func<bool>>()
				{
					() => b.Width % 8 == 0,
					() => b.Height % 8 == 0,
					() => Utils.ValidateColors(b)
				};
				if (!Utils.Validate(funcs))
				{
					throw new InvalidOperationException("Invalid image! Must only use 15 colors + transparency and also width and height must be divisible by 8!");
				}
				pb.Image = b;
			}
			catch (Exception e)
			{
				_ = Utils.AutoException(e, false);
			}
		}
	}
}
