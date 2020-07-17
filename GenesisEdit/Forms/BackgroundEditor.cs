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
		public Bitmap BG1 => (Bitmap)BG1Box.Image;
		public Bitmap BG2 => (Bitmap)BG1Box.Image;

		private static readonly OpenFileDialog openDialog = new OpenFileDialog()
		{
			Filter = Utils.AutoFilter("png", "jpg", "bmp"),
			Title = "Open Image"
		};

		public BackgroundEditor() => InitializeComponent();

		private void OKButton_Click(object sender, EventArgs e) => Close();
		private void BG1Box_Click(object sender, EventArgs e) => SetImage(ref BG1Box);
		private void BG2Box_Click(object sender, EventArgs e) => SetImage(ref BG2Box);

		internal static void SetImage(ref PictureBox pb)
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
				if (!Utils.ValidateImage(b))
				{
					throw new InvalidOperationException("Invalid image! Must only use 16 colors (includes transparency) and width and height must be divisible by 8!");
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
