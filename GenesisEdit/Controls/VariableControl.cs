using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenesisEdit.Compiler;

namespace GenesisEdit.Controls
{
	internal partial class VariableControl : UserControl
	{
		public Variable Var;
		private double NB_Ratio;
		private double TS_Ratio;
		private double TS_XRatio;
		private double X_Ratio;
		private double X_XRatio;
		private double LS_Ratio;
		private double LS_XRatio;

		public VariableControl()
		{
			InitializeComponent();

			//Set ratios
			NB_Ratio = (double)NameBox.Width / Width;
			TS_Ratio = (double)TypeSelector.Width / Width;
			TS_XRatio = (double)TypeSelector.Location.X / Width;
			X_Ratio = (double)DeleteButton.Width / Width;
			X_XRatio = (double)DeleteButton.Location.X / Width;
			LS_Ratio = (double)LengthSel.Width / Width;
			LS_XRatio = (double)LengthSel.Location.X / Width;

			//Other init
			Var = new VarByte();
			NameBox.Text = Var.Name;
			VariableControl_Resize(null, null);
			TypeSelector.SelectedIndex = 0;
			TypeSelector.SelectedItem = TypeSelector.Items[0];
			LengthSel.Value = 1;
			LengthSel.Maximum = int.MaxValue;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			//Remove from parent's controls and update
			Parent.Controls.Remove(this);

			//Dont need this instance anymore
			Dispose();
		}

		private void VariableControl_Resize(object sender, EventArgs e)
		{
			Width = Parent != null ? Parent.Width : 400;
			NameBox.Width = (int)(Width * NB_Ratio);
			TypeSelector.Width = (int)(Width * TS_Ratio);
			TypeSelector.Location = new Point((int)(Width * TS_XRatio), TypeSelector.Location.Y);
			DeleteButton.Width = (int)(Width * X_Ratio);
			DeleteButton.Location = new Point((int)(Width * X_XRatio), DeleteButton.Location.Y);
			LengthSel.Width = (int)(Width * LS_Ratio);
			LengthSel.Location = new Point((int)(Width * LS_XRatio), LengthSel.Location.Y);
		}

		private void NameBox_TextChanged(object sender, EventArgs e)
		{
			if (!Utils.IsValidIdentifier(NameBox.Text))
			{
				Var.Name = string.Empty;
				return;
			}
			Var = GetVar();
		}

		private Variable GetVar()
		{
			VariableType t = (VariableType)Enum.Parse(typeof(VariableType), (TypeSelector.SelectedItem ?? "WORD").ToString().ToUpper());
			string name = NameBox.Text;
			switch (t)
			{
				case VariableType.BYTE:
					return new VarByte(name, (int)LengthSel.Value);
				case VariableType.WORD:
					return new VarWord(name, (int)LengthSel.Value);
				case VariableType.LONG:
					return new VarLong(name, (int)LengthSel.Value);
				default:
					throw new InvalidOperationException();
			}
		}

		private void TypeSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			Var = GetVar();
		}

		private void LengthSel_ValueChanged(object sender, EventArgs e)
		{
			Var = GetVar();
		}
	}
}
