namespace GenesisEdit.Controls
{
	partial class EventControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.DeleteButton = new System.Windows.Forms.Button();
			this.NameBox = new System.Windows.Forms.TextBox();
			this.TypeSel = new System.Windows.Forms.ComboBox();
			this.ButtonSel = new System.Windows.Forms.ComboBox();
			this.Resizer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// DeleteButton
			// 
			this.DeleteButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.DeleteButton.Location = new System.Drawing.Point(245, 0);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(55, 40);
			this.DeleteButton.TabIndex = 0;
			this.DeleteButton.Text = "X";
			this.DeleteButton.UseVisualStyleBackColor = true;
			this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// NameBox
			// 
			this.NameBox.Location = new System.Drawing.Point(4, 9);
			this.NameBox.Name = "NameBox";
			this.NameBox.Size = new System.Drawing.Size(91, 20);
			this.NameBox.TabIndex = 1;
			this.NameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
			// 
			// TypeSel
			// 
			this.TypeSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TypeSel.FormattingEnabled = true;
			this.TypeSel.Location = new System.Drawing.Point(101, 9);
			this.TypeSel.MaxDropDownItems = 100;
			this.TypeSel.Name = "TypeSel";
			this.TypeSel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TypeSel.Size = new System.Drawing.Size(76, 21);
			this.TypeSel.TabIndex = 2;
			// 
			// ButtonSel
			// 
			this.ButtonSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ButtonSel.FormattingEnabled = true;
			this.ButtonSel.Location = new System.Drawing.Point(183, 9);
			this.ButtonSel.MaxDropDownItems = 100;
			this.ButtonSel.Name = "ButtonSel";
			this.ButtonSel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ButtonSel.Size = new System.Drawing.Size(56, 21);
			this.ButtonSel.TabIndex = 3;
			// 
			// Resizer
			// 
			this.Resizer.Enabled = true;
			this.Resizer.Tick += new System.EventHandler(this.Resizer_Tick);
			// 
			// EventControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ButtonSel);
			this.Controls.Add(this.TypeSel);
			this.Controls.Add(this.NameBox);
			this.Controls.Add(this.DeleteButton);
			this.Name = "EventControl";
			this.Size = new System.Drawing.Size(300, 40);
			this.Resize += new System.EventHandler(this.EventControl_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button DeleteButton;
		private System.Windows.Forms.TextBox NameBox;
		private System.Windows.Forms.ComboBox TypeSel;
		private System.Windows.Forms.ComboBox ButtonSel;
		private System.Windows.Forms.Timer Resizer;
	}
}
