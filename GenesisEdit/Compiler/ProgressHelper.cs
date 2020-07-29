using GenesisEdit.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.Compiler
{
	internal static class ProgressHelper
	{
		public static bool ENABLED = false;
		public static ProgressWindow PW = new ProgressWindow("Compiling...");
		public static double PROGRESS = 0;
		public static double ACTIONS = 0;

		public static void ResetProgress(string text)
		{
			try
			{
				PW = new ProgressWindow(text);
				PROGRESS = 0;
				ACTIONS = 0;
			}
			catch
			{
				Utils.Log("Coudld not reset progress!");
			}
		}
		public static void UpdateProgress(string text = null)
		{
			if (!ENABLED)
			{
				return;
			}
			try
			{
				Utils.Log($"{PROGRESS} / {ACTIONS} Completed");
				PW.UpdateProgress(PROGRESS / ACTIONS, text);
			}
			catch
			{
				Utils.Log("Coudld not update progres");
			}
		}
	}
}
