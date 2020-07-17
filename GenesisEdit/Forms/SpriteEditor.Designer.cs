namespace GenesisEdit.Forms
{
	partial class SpriteEditor
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
			this.AddButton = new System.Windows.Forms.Button();
			this.OKButton = new System.Windows.Forms.Button();
			this.SpritesList = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			//
			//AddButton
			//
			this.AddButton.Location = new System.Drawing.Point(12, 12);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(360, 55);
			this.AddButton.TabIndex = 0;
			this.AddButton.Text = "Add";
			this.AddButton.UseVisualStyleBackColor = true;
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
			//
			//OKButton
			//
			this.OKButton.Location = new System.Drawing.Point(12, 294);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(360, 55);
			this.OKButton.TabIndex = 1;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			//
			//SpritesList
			//
			this.SpritesList.Location = new System.Drawing.Point(13, 74);
			this.SpritesList.Name = "SpritesList";
			this.SpritesList.Size = new System.Drawing.Size(359, 214);
			this.SpritesList.TabIndex = 2;
			//
			//SpriteEditor
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 361);
			this.Controls.Add(this.SpritesList);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.AddButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SpriteEditor";
			this.Text = "SpriteEditor";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Panel SpritesList;
	}
}