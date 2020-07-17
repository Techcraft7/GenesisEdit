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
			{ new Regex("[#$]{0,2}[0-9]+"), Color.FromArgb(184, 215, 163) },
			{ new Regex("[A-Z_]{1}[A-Z0-9_]*:?", RegexOptions.IgnoreCase), Color.FromArgb(78, 201, 176) },
			{ new Regex("[A-Z]{2,4}(\\.[lwb])?\\s+", RegexOptions.IgnoreCase), Color.FromArgb(75, 156, 206) },
			{ new Regex("[AD][0-9]", RegexOptions.IgnoreCase), Color.FromArgb(241, 120, 0) },
			{ new Regex("(;.+\r?\n)|(%[^\r\n%]*%)", RegexOptions.IgnoreCase), Color.FromArgb(87, 166, 74) },
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
		private ROMInfo rom = new ROMInfo();
		private List<Sprite> Sprites => spriteEditor.Sprites;
		private Bitmap BG1 => backgroundEditor.BG1;
		private Bitmap BG2 => backgroundEditor.BG2;

		//GUIS
		private readonly SettingsWindow settings = new SettingsWindow();
		private readonly VarEditor varEditor = new VarEditor();
		private readonly HelpWindow helpWindow = new HelpWindow();
		private readonly SpriteEditor spriteEditor = new SpriteEditor();
		private readonly BackgroundEditor backgroundEditor = new BackgroundEditor();

		public static MainWindow INSTACE;
		public static bool EVENT_LIST_STALE = false;

		public MainWindow()
		{
			INSTACE = this;
			//Init components
			InitializeComponent();

			//Save file dialog, allow using of custom INI files because why not
			saveDialog.Filter = "GenesisEdit Files (*.ge)|*.ge|INI Files (*.ini)|*.ini";
			saveDialog.Title = "Save file";
			saveDialog.FileName = "untitled.ge";

			//Theme
			UpdateTheme();

			//Add resize ratios
			resizeRatio = new Dictionary<Control, DoublePoint>()
			{
				{ EventsList, new DoublePoint(0.33333, -1) }
			};

			Controls.OfType<ComboBox>().ToList().ForEach(c =>
			{
				if (c.Items.Count > 0)
				{
					c.SelectedIndex = 0;
				}
			});

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
				FileHandler.SaveFile(file, rom, Variables, Events);
			}
		}

		private void RunButton_Click(object sender, EventArgs e) => Compile();

		private void VariablesButton_Click(object sender, EventArgs e) => varEditor.ShowDialog();

		private void SettingsButton_Click(object sender, EventArgs e)
		{
			settings.ShowDialog();
			UpdateTheme();
		}

		private void UpdateTheme()
		{
			byte theme = Settings.Default.Theme;
			foreach (Form f in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(f => f.FieldType.IsSubclassOf(typeof(Form))).Select(f => f.GetValue(this)).Concat(new Form[] { this }))
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

		private void Compile()
		{
			try
			{
				Task t = new Task(() =>
				{
					Compiler.Compiler.Compile(Events, Variables, Sprites, rom, BG1, BG2);
				});
				t.Start();
				while (!t.IsCompleted)
				{
					Application.DoEvents();
				}
			}
			catch (ArgumentNullException e)
			{
				_ = MessageBox.Show($"A sprite or background might be missing a texure!\nError while compiling:\n{e.GetType()} - {e.Message}\n{e.StackTrace}");
			}
			catch (Exception e)
			{
				_ = MessageBox.Show($"Error while compiling:\n{e.GetType()} - {e.Message}\n{e.StackTrace}");
			}
		}

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
			((Events ?? new List<GenesisEvent>()).Find(ge =>
			{
				if (ge == null)
				{
					return false;
				}
				if (EventSel.SelectedItem == null)
				{
					return false;
				}
				return ge.Name.Equals(EventSel.SelectedItem ?? INVALID_NAME);
			}) ?? new GenesisEvent(EventType.ON_TICK, string.Empty)).Code = CodeBox.Text;
		}

		private void RunSyntaxHighlighter() => syntaxHighlightTask = Task.Run(delegate
		{
			Invoke(new Action(delegate
			{
				int loc = CodeBox.SelectionStart;
				int len = CodeBox.SelectionLength;
				foreach (KeyValuePair<Regex, Color> kv in SYNTAX_COLORS)
				{
					MatchCollection mc = kv.Key.Matches(CodeBox.Text);
					foreach (Match m in mc)
					{
						CodeBox.Select(m.Index, m.Length);
						CodeBox.SelectionColor = kv.Value;
					}
				}
				CodeBox.DeselectAll();
				CodeBox.Select(loc, len);
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

		private void HelpButton_Click(object sender, EventArgs e) => helpWindow.ShowDialog();

		private void AddEventButton_Click(object sender, EventArgs e) => EventsList.Controls.Add(new EventControl());

		private void ROMInfoButton_DropDownClosed(object sender, EventArgs e) => UpdateROMInfo();

		private void SpritesButton_Click(object sender, EventArgs e) => _ = spriteEditor.ShowDialog();

		private void BackgroundsButton_Click(object sender, EventArgs e) => _ = backgroundEditor.ShowDialog();

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
			List<EventControl> ecs = EventsList.Controls.OfType<EventControl>().ToList();
			ecs = ecs.OrderBy(c => c.Location.Y).ToList();
			int j = 0;
			foreach (EventControl ec in ecs)
			{
				ec.Location = new Point(0, j * ec.Height);
				j++;
			}
		}

		private void EventUpdater_Tick(object sender, EventArgs e)
		{
			if (EVENT_LIST_STALE)
			{
				UpdateEvents();
				EVENT_LIST_STALE = false;
			}
		}
	}
}
