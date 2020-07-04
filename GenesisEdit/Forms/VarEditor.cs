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
	internal partial class VarEditor : Form
	{
		public VarEditor()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		public IEnumerable<Variable> GetVariables() => VariablesList.Controls.OfType<VariableControl>().Where(c => c != null && !c.IsDisposed).Select(c => c.Var);

		private void AddButton_Click(object sender, EventArgs e) => VariablesList.Controls.Add(new VariableControl());
	}
}
