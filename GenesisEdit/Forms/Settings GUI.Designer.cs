using System.Windows.Forms;

namespace GenesisEdit.Forms
{
	partial class SettingsWindow : Form
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
			this.label1 = new System.Windows.Forms.Label();
			this.ThemePicker = new System.Windows.Forms.ComboBox();
			this.VibrantBox = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// OKButton
			// 
			this.OKButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.OKButton.Location = new System.Drawing.Point(0, 231);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(284, 30);
			this.OKButton.TabIndex = 0;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(284, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Theme*";
			// 
			// ThemePicker
			// 
			this.ThemePicker.BackColor = System.Drawing.Color.White;
			this.ThemePicker.Dock = System.Windows.Forms.DockStyle.Top;
			this.ThemePicker.FormattingEnabled = true;
			this.ThemePicker.Items.AddRange(new object[] {
            "Light",
            "Dark"});
			this.ThemePicker.Location = new System.Drawing.Point(0, 13);
			this.ThemePicker.Name = "ThemePicker";
			this.ThemePicker.Size = new System.Drawing.Size(284, 21);
			this.ThemePicker.TabIndex = 2;
			// 
			// VibrantBox
			// 
			this.VibrantBox.AutoSize = true;
			this.VibrantBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.VibrantBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.VibrantBox.Location = new System.Drawing.Point(0, 34);
			this.VibrantBox.Name = "VibrantBox";
			this.VibrantBox.Size = new System.Drawing.Size(284, 17);
			this.VibrantBox.TabIndex = 3;
			this.VibrantBox.Text = "Use Vibrant Colors*";
			this.VibrantBox.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.label2.Location = new System.Drawing.Point(0, 216);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(284, 15);
			this.label2.TabIndex = 4;
			this.label2.Text = "* Does not effect game";
			// 
			// SettingsWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.VibrantBox);
			this.Controls.Add(this.ThemePicker);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.OKButton);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SettingsWindow";
			this.Text = "Settings";
			this.Load += new System.EventHandler(this.SettingsWindow_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox ThemePicker;
		private System.Windows.Forms.CheckBox VibrantBox;
		private System.Windows.Forms.Label label2;
	}
}