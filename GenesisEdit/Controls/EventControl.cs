using GenesisEdit.Compiler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Button = GenesisEdit.Compiler.Button;

namespace GenesisEdit.Controls
{
	internal partial class EventControl : UserControl
	{
		public GenesisEvent Event;

		private readonly Dictionary<Control, Tuple<double, double>> ratios = new Dictionary<Control, Tuple<double, double>>();

		public EventControl()
		{
			Event = new GenesisEvent(EventType.ON_TICK, $"Event_{new Random().Next():X}");
			InitializeComponent();
			NameBox.Text = Event.Name;

			foreach (Control c in Controls)
			{
				ratios.Add(c, new Tuple<double, double>((double)c.Width / Width, (double)c.Location.X / Width));
			}

			foreach (Button b in Enum.GetValues(typeof(Button)))
			{
				ButtonSel.Items.Add(Utils.FormatEnum(b.ToString()));
			}

			foreach (EventType et in Enum.GetValues(typeof(EventType)))
			{
				TypeSel.Items.Add(Utils.FormatEnum(et.ToString()));
			}

			TypeSel.SelectedIndex = (int)Event.Type;
			ButtonSel.SelectedIndex = (int)Event.Button;

			EventControl_Resize(null, null);
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			//Remove from parent's controls and update
			Parent.Controls.Remove(this);

			//Dont need this instance anymore
			Dispose();
		}

		private void EventControl_Resize(object sender, EventArgs e)
		{
			if (Parent != null)
			{
				MinimumSize = new Size(0, 40);
				MaximumSize = new Size(Parent.Width - 10, 40);
				Width = Parent.Width;
			}
			foreach (KeyValuePair<Control, Tuple<double, double>> c in ratios)
			{
				c.Key.Width = (int)(c.Value.Item1 * Width);
				c.Key.Location = new Point((int)(c.Value.Item2 * Width), c.Key.Location.Y);
			}
		}

		private void NameBox_TextChanged(object sender, EventArgs e)
		{
			if (!Utils.IsValidIdentifier(NameBox.Text))
			{
				Event.Name = $"Event_{new Random().Next():X}";
				return;
			}
			Event.Name = NameBox.Text;
		}

		private void Resizer_Tick(object sender, EventArgs e) => EventControl_Resize(null, null);
	}
}
