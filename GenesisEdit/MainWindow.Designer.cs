namespace GenesisEdit
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.MainMenu = new System.Windows.Forms.MenuStrip();
			this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.NewButton = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenButton = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
			this.RunButton = new System.Windows.Forms.ToolStripMenuItem();
			this.VariablesButton = new System.Windows.Forms.ToolStripMenuItem();
			this.PalettesButton = new System.Windows.Forms.ToolStripMenuItem();
			this.ROMInfoButton = new System.Windows.Forms.ToolStripMenuItem();
			this.TitleBox = new System.Windows.Forms.ToolStripTextBox();
			this.AuthorBox = new System.Windows.Forms.ToolStripTextBox();
			this.SubtitleBox = new System.Windows.Forms.ToolStripTextBox();
			this.Subtitle2Box = new System.Windows.Forms.ToolStripTextBox();
			this.ProdNBox = new System.Windows.Forms.ToolStripTextBox();
			this.SpritesButton = new System.Windows.Forms.ToolStripMenuItem();
			this.BackgroundsButton = new System.Windows.Forms.ToolStripMenuItem();
			this.SettingsButton = new System.Windows.Forms.ToolStripMenuItem();
			this.HelpButton = new System.Windows.Forms.ToolStripMenuItem();
			this.CodeBox = new System.Windows.Forms.RichTextBox();
			this.EventsList = new System.Windows.Forms.FlowLayoutPanel();
			this.AddEventButton = new System.Windows.Forms.Button();
			this.EventSel = new System.Windows.Forms.ComboBox();
			this.EventUpdater = new System.Windows.Forms.Timer(this.components);
			this.MainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainMenu
			// 
			this.MainMenu.BackColor = System.Drawing.Color.White;
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.RunButton,
            this.VariablesButton,
            this.PalettesButton,
            this.ROMInfoButton,
            this.SpritesButton,
            this.BackgroundsButton,
            this.SettingsButton,
            this.HelpButton});
			this.MainMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.MainMenu.Size = new System.Drawing.Size(800, 24);
			this.MainMenu.TabIndex = 0;
			this.MainMenu.Text = "Menu";
			// 
			// FileMenu
			// 
			this.FileMenu.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
			this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewButton,
            this.OpenButton,
            this.SaveButton});
			this.FileMenu.Name = "FileMenu";
			this.FileMenu.Size = new System.Drawing.Size(37, 20);
			this.FileMenu.Text = "File";
			// 
			// NewButton
			// 
			this.NewButton.Name = "NewButton";
			this.NewButton.Size = new System.Drawing.Size(103, 22);
			this.NewButton.Text = "New";
			this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
			// 
			// OpenButton
			// 
			this.OpenButton.Name = "OpenButton";
			this.OpenButton.Size = new System.Drawing.Size(103, 22);
			this.OpenButton.Text = "Open";
			// 
			// SaveButton
			// 
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(103, 22);
			this.SaveButton.Text = "Save";
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// RunButton
			// 
			this.RunButton.Image = ((System.Drawing.Image)(resources.GetObject("RunButton.Image")));
			this.RunButton.Name = "RunButton";
			this.RunButton.Size = new System.Drawing.Size(56, 20);
			this.RunButton.Text = "Run";
			this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
			// 
			// VariablesButton
			// 
			this.VariablesButton.Name = "VariablesButton";
			this.VariablesButton.Size = new System.Drawing.Size(65, 20);
			this.VariablesButton.Text = "Variables";
			this.VariablesButton.Click += new System.EventHandler(this.VariablesButton_Click);
			// 
			// PalettesButton
			// 
			this.PalettesButton.Name = "PalettesButton";
			this.PalettesButton.Size = new System.Drawing.Size(60, 20);
			this.PalettesButton.Text = "Palettes";
			this.PalettesButton.Click += new System.EventHandler(this.PalettesButton_Click);
			// 
			// ROMInfoButton
			// 
			this.ROMInfoButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TitleBox,
            this.AuthorBox,
            this.SubtitleBox,
            this.Subtitle2Box,
            this.ProdNBox});
			this.ROMInfoButton.Name = "ROMInfoButton";
			this.ROMInfoButton.Size = new System.Drawing.Size(70, 20);
			this.ROMInfoButton.Text = "ROM Info";
			this.ROMInfoButton.DropDownClosed += new System.EventHandler(this.ROMInfoButton_DropDownClosed);
			// 
			// TitleBox
			// 
			this.TitleBox.MaxLength = 16;
			this.TitleBox.Name = "TitleBox";
			this.TitleBox.Size = new System.Drawing.Size(100, 23);
			this.TitleBox.Text = "Title";
			// 
			// AuthorBox
			// 
			this.AuthorBox.MaxLength = 16;
			this.AuthorBox.Name = "AuthorBox";
			this.AuthorBox.Size = new System.Drawing.Size(100, 23);
			this.AuthorBox.Text = "Author";
			// 
			// SubtitleBox
			// 
			this.SubtitleBox.MaxLength = 48;
			this.SubtitleBox.Name = "SubtitleBox";
			this.SubtitleBox.Size = new System.Drawing.Size(100, 23);
			this.SubtitleBox.Text = "Subtitle";
			// 
			// Subtitle2Box
			// 
			this.Subtitle2Box.MaxLength = 48;
			this.Subtitle2Box.Name = "Subtitle2Box";
			this.Subtitle2Box.Size = new System.Drawing.Size(100, 23);
			this.Subtitle2Box.Text = "Subtitle2";
			// 
			// ProdNBox
			// 
			this.ProdNBox.MaxLength = 14;
			this.ProdNBox.Name = "ProdNBox";
			this.ProdNBox.Size = new System.Drawing.Size(100, 23);
			this.ProdNBox.Text = "Product #";
			// 
			// SpritesButton
			// 
			this.SpritesButton.Name = "SpritesButton";
			this.SpritesButton.Size = new System.Drawing.Size(54, 20);
			this.SpritesButton.Text = "Sprites";
			this.SpritesButton.Click += new System.EventHandler(this.SpritesButton_Click);
			// 
			// BackgroundsButton
			// 
			this.BackgroundsButton.Name = "BackgroundsButton";
			this.BackgroundsButton.Size = new System.Drawing.Size(88, 20);
			this.BackgroundsButton.Text = "Backgrounds";
			this.BackgroundsButton.Click += new System.EventHandler(this.BackgroundsButton_Click);
			// 
			// SettingsButton
			// 
			this.SettingsButton.Name = "SettingsButton";
			this.SettingsButton.Size = new System.Drawing.Size(61, 20);
			this.SettingsButton.Text = "Settings";
			this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
			// 
			// HelpButton
			// 
			this.HelpButton.Name = "HelpButton";
			this.HelpButton.Size = new System.Drawing.Size(44, 20);
			this.HelpButton.Text = "Help";
			this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
			// 
			// CodeBox
			// 
			this.CodeBox.AcceptsTab = true;
			this.CodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.CodeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.CodeBox.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CodeBox.Location = new System.Drawing.Point(306, 55);
			this.CodeBox.Name = "CodeBox";
			this.CodeBox.Size = new System.Drawing.Size(491, 391);
			this.CodeBox.TabIndex = 1;
			this.CodeBox.Text = "";
			this.CodeBox.WordWrap = false;
			this.CodeBox.TextChanged += new System.EventHandler(this.CodeBox_TextChanged);
			// 
			// EventsList
			// 
			this.EventsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.EventsList.AutoScroll = true;
			this.EventsList.Location = new System.Drawing.Point(0, 55);
			this.EventsList.Name = "EventsList";
			this.EventsList.Size = new System.Drawing.Size(300, 395);
			this.EventsList.TabIndex = 2;
			this.EventsList.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.EventsList_ControlAdded);
			this.EventsList.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.EventsList_ControlRemoved);
			this.EventsList.Resize += new System.EventHandler(this.Sidebar_Resize);
			// 
			// AddEventButton
			// 
			this.AddEventButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.AddEventButton.Location = new System.Drawing.Point(0, 27);
			this.AddEventButton.Name = "AddEventButton";
			this.AddEventButton.Size = new System.Drawing.Size(300, 22);
			this.AddEventButton.TabIndex = 4;
			this.AddEventButton.Text = "Add Event";
			this.AddEventButton.UseVisualStyleBackColor = true;
			this.AddEventButton.Click += new System.EventHandler(this.AddEventButton_Click);
			// 
			// EventSel
			// 
			this.EventSel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.EventSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EventSel.FormattingEnabled = true;
			this.EventSel.Location = new System.Drawing.Point(307, 28);
			this.EventSel.Name = "EventSel";
			this.EventSel.Size = new System.Drawing.Size(490, 21);
			this.EventSel.TabIndex = 5;
			this.EventSel.SelectedIndexChanged += new System.EventHandler(this.EventSel_SelectedIndexChanged);
			// 
			// EventUpdater
			// 
			this.EventUpdater.Enabled = true;
			this.EventUpdater.Tick += new System.EventHandler(this.EventUpdater_Tick);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.EventSel);
			this.Controls.Add(this.AddEventButton);
			this.Controls.Add(this.CodeBox);
			this.Controls.Add(this.EventsList);
			this.Controls.Add(this.MainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainWindow";
			this.Text = "Genesis Edit";
			this.Resize += new System.EventHandler(this.MainWindow_Resize);
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MainMenu;
		private System.Windows.Forms.ToolStripMenuItem FileMenu;
		private System.Windows.Forms.ToolStripMenuItem NewButton;
		private System.Windows.Forms.ToolStripMenuItem OpenButton;
		private System.Windows.Forms.ToolStripMenuItem SaveButton;
		private System.Windows.Forms.ToolStripMenuItem RunButton;
		private System.Windows.Forms.RichTextBox CodeBox;
		private System.Windows.Forms.ToolStripMenuItem VariablesButton;
		private System.Windows.Forms.ToolStripMenuItem SettingsButton;
		private System.Windows.Forms.ToolStripMenuItem PalettesButton;
		private System.Windows.Forms.ToolStripMenuItem HelpButton;
		private System.Windows.Forms.Button AddEventButton;
		private System.Windows.Forms.ToolStripMenuItem ROMInfoButton;
		private System.Windows.Forms.ToolStripTextBox TitleBox;
		private System.Windows.Forms.ToolStripTextBox AuthorBox;
		private System.Windows.Forms.ToolStripTextBox SubtitleBox;
		private System.Windows.Forms.ToolStripTextBox Subtitle2Box;
		private System.Windows.Forms.ToolStripTextBox ProdNBox;
		private System.Windows.Forms.ComboBox EventSel;
		private System.Windows.Forms.Timer EventUpdater;
		private System.Windows.Forms.FlowLayoutPanel EventsList;
		private System.Windows.Forms.ToolStripMenuItem SpritesButton;
		private System.Windows.Forms.ToolStripMenuItem BackgroundsButton;
	}
}

