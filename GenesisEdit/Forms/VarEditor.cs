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

		public IEnumerable<Variable> GetVariables()
		{
			List<Control> cs = new List<Control>();
			foreach (Control c in VariablesList.Controls)
			{
				cs.Add(c);
			}
			return cs.Where(c => c != null && !c.IsDisposed && c.GetType().IsEquivalentTo(typeof(VariableControl)))
				.Select(c => ((VariableControl)c).Var);
		}

		private void AddButton_Click(object sender, EventArgs e) => VariablesList.Controls.Add(new VariableControl());
	}
}
