using System.Windows.Forms;

namespace GenesisEdit.Forms
{
	partial class PalEditor : Form
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
			this.OKButton = new System.Windows.Forms.Button();
			this.CD = new System.Windows.Forms.ColorDialog();
			this.ColorSelector = new System.Windows.Forms.ComboBox();
			this.ColorPreview = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// OKButton
			// 
			this.OKButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.OKButton.Location = new System.Drawing.Point(0, 302);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(384, 59);
			this.OKButton.TabIndex = 0;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// ColorSelector
			// 
			this.ColorSelector.AutoCompleteCustomSource.AddRange(new string[] {
            "P1C1",
            "P1C2",
            "P1C3",
            "P1C4",
            "P1C5",
            "P1C6",
            "P1C7",
            "P1C8",
            "P1C9",
            "P1C10",
            "P1C11",
            "P1C12",
            "P1C13",
            "P1C14",
            "P1C15",
            "P2C1",
            "P2C2",
            "P2C3",
            "P2C4",
            "P2C5",
            "P2C6",
            "P2C7",
            "P2C8",
            "P2C9",
            "P2C10",
            "P2C11",
            "P2C12",
            "P2C13",
            "P2C14",
            "P2C15",
            "P3C1",
            "P3C2",
            "P3C3",
            "P3C4",
            "P3C5",
            "P3C6",
            "P3C7",
            "P3C8",
            "P3C9",
            "P3C10",
            "P3C11",
            "P3C12",
            "P3C13",
            "P3C14",
            "P3C15",
            "P4C1",
            "P4C2",
            "P4C3",
            "P4C4",
            "P4C5",
            "P4C6",
            "P4C7",
            "P4C8",
            "P4C9",
            "P4C10",
            "P4C11",
            "P4C12",
            "P4C13",
            "P4C14",
            "P4C15"});
			this.ColorSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ColorSelector.FormattingEnabled = true;
			this.ColorSelector.Items.AddRange(new object[] {
            "P1C1",
            "P1C2",
            "P1C3",
            "P1C4",
            "P1C5",
            "P1C6",
            "P1C7",
            "P1C8",
            "P1C9",
            "P1C10",
            "P1C11",
            "P1C12",
            "P1C13",
            "P1C14",
            "P1C15",
            "P2C1",
            "P2C2",
            "P2C3",
            "P2C4",
            "P2C5",
            "P2C6",
            "P2C7",
            "P2C8",
            "P2C9",
            "P2C10",
            "P2C11",
            "P2C12",
            "P2C13",
            "P2C14",
            "P2C15",
            "P3C1",
            "P3C2",
            "P3C3",
            "P3C4",
            "P3C5",
            "P3C6",
            "P3C7",
            "P3C8",
            "P3C9",
            "P3C10",
            "P3C11",
            "P3C12",
            "P3C13",
            "P3C14",
            "P3C15",
            "P4C1",
            "P4C2",
            "P4C3",
            "P4C4",
            "P4C5",
            "P4C6",
            "P4C7",
            "P4C8",
            "P4C9",
            "P4C10",
            "P4C11",
            "P4C12",
            "P4C13",
            "P4C14",
            "P4C15"});
			this.ColorSelector.Location = new System.Drawing.Point(12, 12);
			this.ColorSelector.MaxDropDownItems = 100;
			this.ColorSelector.Name = "ColorSelector";
			this.ColorSelector.Size = new System.Drawing.Size(360, 21);
			this.ColorSelector.TabIndex = 1;
			this.ColorSelector.SelectedIndexChanged += new System.EventHandler(this.ColorSelector_SelectedIndexChanged);
			// 
			// ColorPreview
			// 
			this.ColorPreview.Location = new System.Drawing.Point(59, 86);
			this.ColorPreview.Name = "ColorPreview";
			this.ColorPreview.Size = new System.Drawing.Size(200, 100);
			this.ColorPreview.TabIndex = 2;
			// 
			// PalEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 361);
			this.Controls.Add(this.ColorPreview);
			this.Controls.Add(this.ColorSelector);
			this.Controls.Add(this.OKButton);
			this.Name = "PalEditor";
			this.Text = "PalEditor";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.ColorDialog CD;
		private System.Windows.Forms.ComboBox ColorSelector;
		private System.Windows.Forms.Panel ColorPreview;
	}
}