using GenesisEdit.Properties;
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
	public partial class SettingsWindow : Form
	{
		public SettingsWindow()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			//Theme
			Settings.Default.Theme = (byte)ThemePicker.SelectedIndex;

			//Use Vibrant Colors
			Settings.Default.UseVibrantColors = VibrantBox.Checked;


			//Close
			Settings.Default.Save();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void SettingsWindow_Load(object sender, EventArgs e)
		{
			//Prevent invalid values
			Settings.Default.Theme = (byte)(Settings.Default.Theme >= ThemePicker.Items.Count ? ThemePicker.Items.Count - 1 : Settings.Default.Theme);
			ThemePicker.SelectedIndex = Settings.Default.Theme;
			VibrantBox.Checked = Settings.Default.UseVibrantColors;
		}
	}
}
