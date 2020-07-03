using GenesisEdit.Compiler;
using GenesisEdit.Controls;
using GenesisEdit.FIleHandler;
using GenesisEdit.Forms;
using GenesisEdit.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ButtonControl = System.Windows.Forms.Button;
using DoublePoint = System.Tuple<double, double>;

namespace GenesisEdit
{
	public partial class MainWindow : Form
	{
		//Constants
		private const string INVALID_NAME = "GE_INVALID";
		private static readonly DialogResult[] VALID_DIALOG_RESULTS = new DialogResult[] { DialogResult.OK, DialogResult.Cancel };
		private static readonly Dictionary<Regex, Color> SYNTAX_COLORS = new Dictionary<Regex, Color>()
		{
			{ new Regex("[0-9]+"), Color.FromArgb(184, 215, 163) },
			{ new Regex("[A-Z_]{1}[A-Z0-9_]*", RegexOptions.Multiline | RegexOptions.IgnoreCase), Color.FromArgb(78, 201, 176) },
			{ new Regex("[A-Z]{2,4}(\\.[lwb])?\\s+", RegexOptions.Multiline | RegexOptions.IgnoreCase), Color.FromArgb(75, 156, 206) },
			{ new Regex("[AD][0-9]", RegexOptions.Multiline | RegexOptions.IgnoreCase), Color.FromArgb(181, 93, 47) }
		};
		private static Dictionary<byte, Color> THEME = new Dictionary<byte, Color>()
		{
			{ 0, Color.White }, //Light
			{ 1, Color.FromArgb(37, 37, 37) } //Dark
		};

		private FileStream file;
		private SaveFileDialog saveDialog = new SaveFileDialog();
		private Dictionary<Control, DoublePoint> resizeRatio;
		private Color defaultFG = Color.Black;
		private Task syntaxHighlightTask;

		//Genesis data
		private List<GenesisEvent> Events => EventsList.Controls.OfType<EventControl>().Select(e => e.Event).ToList();
		private List<Variable> Variables => varEditor.GetVariables().ToList();
		private Color[,] Palettes => palEditor.Palettes;
		private ROMInfo rom = new ROMInfo();


		//GUIS
		private readonly SettingsWindow settings = new SettingsWindow();
		private readonly VarEditor varEditor = new VarEditor();
		private readonly PalEditor palEditor = new PalEditor();

		public MainWindow()
		{
			//Init components
			InitializeComponent();

			//Save file dialog
			saveDialog.Filter = "GenesisEdit Files (*.ge)|*.ge";
			saveDialog.Title = "Save file";
			saveDialog.FileName = "untitled.ge";

			//Theme
			UpdateTheme();

			//Add resize ratios
			resizeRatio = new Dictionary<Control, DoublePoint>()
			{
				{ EventsList, new DoublePoint(0.33333, -1) }
			};

			foreach (Control c in Controls)
			{
				if (c.GetType().IsSubclassOf(typeof(ComboBox)))
				{
					((ComboBox)c).SelectedIndex = 0;
				}
			}

			//Update
			MainWindow_Resize(null, null);
			UpdateEvents();
		}

		private void NewButton_Click(object sender, EventArgs e)
		{
			if (file != null)
			{
				file.Dispose();
				Events.Clear();
				EventsList.Controls.Clear();
			}
			file = null;
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			if (file == null)
			{
				DialogResult d = DialogResult.None;
				while (!VALID_DIALOG_RESULTS.Contains(d))
				{
					d = saveDialog.ShowDialog();
				}
				switch (d)
				{
					case DialogResult.OK:
						try
						{
							file = File.Open(saveDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
						}
						catch
						{
							_ = MessageBox.Show("The file could not be saved! Do you have access to that file?", Text + " | Oh no!");
							file = null;
						}
						break;
					default:
						_ = MessageBox.Show("The file was NOT saved!", Text);
						file = null;
						return;
				}
			}
			if (file != null)
			{
				UpdateROMInfo();
				if (File.Exists(saveDialog.FileName))
				{
					file.Close();
					File.Delete(saveDialog.FileName);
				}
				file = File.Open(saveDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				FileHandler.SaveFile(file, rom, Variables, Events, Palettes);
			}
		}

		private void RunButton_Click(object sender, EventArgs e) => Compile();

		private void VariablesButton_Click(object sender, EventArgs e) => varEditor.ShowDialog();

		private void PalettesButton_Click(object sender, EventArgs e) => palEditor.ShowDialog();

		private void SettingsButton_Click(object sender, EventArgs e)
		{
			settings.ShowDialog();
			UpdateTheme();
		}

		private void UpdateTheme()
		{
			byte theme = Settings.Default.Theme;
			foreach (Form f in new Form[] { this, settings, varEditor })
			{
				foreach (Control c in f.Controls)
				{
					//Fix some controls because the render weird
					bool skip = false;
					foreach (Type t in new Type[] { typeof(ButtonControl), typeof(ComboBox) })
					{
						if (c.GetType().IsEquivalentTo(t))
						{
							skip = true;
							break;
						}
					}
					if (skip)
					{
						continue;
					}
					//Try to get color
					Color color;
					try
					{
						color = THEME[theme];
					}
					catch
					{
						Utils.Log("Error loading theme!");
						return;
					}
					//Convert to black and white and if it is below a threshold use white text to make it readable
					int bw = new List<int>() { color.R, color.G, color.B }.Sum() / 3;
					if (bw < 75)
					{
						defaultFG = Color.White;
					}
					else
					{
						defaultFG = Color.Black;
					}
					c.ForeColor = defaultFG;
					c.BackColor = color;
				}
				f.BackColor = THEME[theme];
			}
		}

		private void Compile() => Compiler.Compiler.Compile(Events, Variables);

		private void CodeBox_TextChanged(object sender, EventArgs e)
		{
			if (syntaxHighlightTask == null)
			{
				RunSyntaxHighlighter();
			}
			else if (syntaxHighlightTask.IsCompleted)
			{
				RunSyntaxHighlighter();
			}
			Events.Find(ge =>
			{
				if (ge == null)
				{
					return false;
				}
				return ge.Name.Equals(EventSel.SelectedItem ?? INVALID_NAME);
			}).Code = CodeBox.Text;
		}

		private void RunSyntaxHighlighter() => syntaxHighlightTask = Task.Run(delegate
		{
			Invoke(new Action(delegate
			{
				int loc = CodeBox.SelectionStart;
				foreach (KeyValuePair<Regex, Color> kv in SYNTAX_COLORS)
				{
					MatchCollection mc = kv.Key.Matches(CodeBox.Text);
					foreach (Match m in mc)
					{
						CodeBox.SelectionStart = m.Index;
						CodeBox.SelectionLength = m.Length;
						CodeBox.SelectionColor = kv.Value;
					}
				}
				CodeBox.DeselectAll();
				CodeBox.SelectionStart = loc;
				CodeBox.SelectionColor = defaultFG;
			}));
		});

		private void MainWindow_Resize(object sender, EventArgs e)
		{
			foreach (KeyValuePair<Control, DoublePoint> kv in resizeRatio)
			{
				foreach (Control c in Controls.Find(kv.Key.Name, true))
				{
					if (kv.Value.Item1 != -1)
					{
						c.Width = (int)(Width * kv.Value.Item1);
					}
					if (kv.Value.Item2 != -1)
					{
						c.Height = (int)(Height * kv.Value.Item2);
					}
				}
			}
			AddEventButton.Width = EventsList.Width;
			AddEventButton.Height = 22;
			int start = EventsList.Location.X + EventsList.Width + 6;
			CodeBox.Location = new Point(start, CodeBox.Location.Y);
			CodeBox.Size = new Size(Width - start - 24, CodeBox.Height);
			EventSel.Left = CodeBox.Location.X;
			EventSel.Width = CodeBox.Width;
		}

		private void Sidebar_Resize(object sender, EventArgs e)
		{
			foreach (Control c in EventsList.Controls)
			{
				c.Width = EventsList.Width;
				c.Height = EventsList.Height / EventsList.Controls.Count;
			}
		}

		private void HelpButton_Click(object sender, EventArgs e) => _ = MessageBox.Show("TODO: ADD THIS MESSAGE", Text);

		private void AddEventButton_Click(object sender, EventArgs e) => EventsList.Controls.Add(new EventControl());

		private void ROMInfoButton_DropDownClosed(object sender, EventArgs e) => UpdateROMInfo();

		private void UpdateROMInfo()
		{
			rom.Title = TitleBox.Text.PadRight(16, ' ');
			rom.AuthorAndDate = AuthorBox.Text.PadRight(16, ' ');
			rom.Subtitle = SubtitleBox.Text.PadRight(48, ' ');
			rom.Subtitle2 = Subtitle2Box.Text.PadRight(48, ' ');
			rom.ProductNo = ProdNBox.Text.PadRight(14, ' ');
		}

		private void EventSel_SelectedIndexChanged(object sender, EventArgs e) => CodeBox.Text = Events.Find(ge => ge.Name.Equals(EventSel.SelectedItem ?? INVALID_NAME)).Code;
		private void EventsList_ControlAdded(object sender, ControlEventArgs e) => UpdateEvents();
		private void EventsList_ControlRemoved(object sender, ControlEventArgs e) => UpdateEvents();

		private void UpdateEvents()
		{
			EventSel.Items.Clear();
			foreach (var i in Events.Select(e => e.Name))
			{
				EventSel.Items.Add(i);
			}
			if (EventSel.Items.Count > 0)
			{
				EventSel.SelectedIndex = 0;
			}
			CodeBox.Enabled = Events.Count > 0;
		}
	}
}
