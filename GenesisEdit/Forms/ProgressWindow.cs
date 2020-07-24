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
	public partial class ProgressWindow : Form
	{
		private const string PROMPT = "Please wait while GenesisEdit works on things...";
		private readonly string text;
		public bool IsShown = false;
		public string Caption { get; set; }

		/// <summary>
		/// Update progress bar
		/// </summary>
		/// <param name="amount">Value between 0 and 1 (0 = 0% 1 = 100%)</param>
		/// <param name="action">Text to be displayed under <see cref="ProgressWindow.PROMPT"/></param>
		public void UpdateProgress(double amount, string action = null) => Invoke(new Action(() =>
		{
			try
			{
				Text = $"{text} ({Progress.Value / Progress.Maximum}%)";
				Application.DoEvents();
				Progress.Value = (int)(amount * 100);
				if (action != null)
				{
					ActionText.Text = $"{PROMPT}{Environment.NewLine}{action}";
				}
			}
			catch (Exception e)
			{
				Utils.Log(e);
			}
		}));

		public ProgressWindow(string title)
		{
			InitializeComponent();
			text = title;
		}

		private void ProgressWindow_Shown(object sender, EventArgs e)
		{
			ActionText.Text = PROMPT;
			Progress.Value = 0;
		}
	}
}
