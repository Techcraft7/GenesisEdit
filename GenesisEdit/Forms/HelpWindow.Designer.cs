namespace GenesisEdit.Forms
{
	partial class HelpWindow
	{
		///<summary>
		///Required designer variable.
		///</summary>
		private System.ComponentModel.IContainer components = null;

		///<summary>
		///Clean up any resources being used.
		///</summary>
		///<param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		///<summary>
		///Required method for Designer support - do not modify
		///the contents of this method with the code editor.
		///</summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Syntax Notation");
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Macro Values");
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("%IF%");
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("%IFMODE%");
			System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("%SPRITE%");
			System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Macros", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5});
			System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Genesis Edit Help", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode6});
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpWindow));
			this.TopicFinder = new System.Windows.Forms.TreeView();
			this.HelpText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			//
			//TopicFinder
			//
			this.TopicFinder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TopicFinder.Location = new System.Drawing.Point(12, 12);
			this.TopicFinder.Name = "TopicFinder";
			treeNode1.Name = "SyntaxNotation";
			treeNode1.Text = "Syntax Notation";
			treeNode2.Name = "Macro Values";
			treeNode2.Text = "Macro Values";
			treeNode3.Name = "IF";
			treeNode3.Text = "%IF%";
			treeNode4.Name = "IFMODE";
			treeNode4.Text = "%IFMODE%";
			treeNode5.Name = "SPRITE";
			treeNode5.Text = "%SPRITE%";
			treeNode6.Name = "Macros";
			treeNode6.Text = "Macros";
			treeNode7.Name = "Help";
			treeNode7.Text = "Genesis Edit Help";
			this.TopicFinder.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7});
			this.TopicFinder.PathSeparator = ".";
			this.TopicFinder.Size = new System.Drawing.Size(141, 334);
			this.TopicFinder.TabIndex = 0;
			this.TopicFinder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TopicFinder_AfterSelect);
			//
			//HelpText
			//
			this.HelpText.AcceptsReturn = true;
			this.HelpText.AcceptsTab = true;
			this.HelpText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.HelpText.BackColor = System.Drawing.SystemColors.Control;
			this.HelpText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.HelpText.Font = new System.Drawing.Font("Lucida Console", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HelpText.ImeMode = System.Windows.Forms.ImeMode.Close;
			this.HelpText.Location = new System.Drawing.Point(159, 12);
			this.HelpText.MaxLength = 99999999;
			this.HelpText.Multiline = true;
			this.HelpText.Name = "HelpText";
			this.HelpText.ReadOnly = true;
			this.HelpText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.HelpText.Size = new System.Drawing.Size(410, 334);
			this.HelpText.TabIndex = 4;
			this.HelpText.TabStop = false;
			//
			//HelpWindow
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 361);
			this.Controls.Add(this.HelpText);
			this.Controls.Add(this.TopicFinder);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "HelpWindow";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.Text = "Genesis Edit Help";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView TopicFinder;
		private System.Windows.Forms.TextBox HelpText;
	}
}