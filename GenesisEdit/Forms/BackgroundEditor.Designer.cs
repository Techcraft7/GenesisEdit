namespace GenesisEdit.Forms
{
	partial class BackgroundEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackgroundEditor));
			this.OKButton = new System.Windows.Forms.Button();
			this.BG1Box = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.BG2Box = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.BG1Box)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.BG2Box)).BeginInit();
			this.SuspendLayout();
			// 
			// OKButton
			// 
			this.OKButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.OKButton.Location = new System.Drawing.Point(0, 309);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(384, 52);
			this.OKButton.TabIndex = 0;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// BG1Box
			// 
			this.BG1Box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.BG1Box.InitialImage = null;
			this.BG1Box.Location = new System.Drawing.Point(12, 12);
			this.BG1Box.Name = "BG1Box";
			this.BG1Box.Size = new System.Drawing.Size(150, 150);
			this.BG1Box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.BG1Box.TabIndex = 1;
			this.BG1Box.TabStop = false;
			this.BG1Box.Click += new System.EventHandler(this.BG1Box_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 165);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(150, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "BG 1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(222, 165);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(150, 23);
			this.label2.TabIndex = 4;
			this.label2.Text = "BG 2";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BG2Box
			// 
			this.BG2Box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.BG2Box.InitialImage = null;
			this.BG2Box.Location = new System.Drawing.Point(222, 12);
			this.BG2Box.Name = "BG2Box";
			this.BG2Box.Size = new System.Drawing.Size(150, 150);
			this.BG2Box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.BG2Box.TabIndex = 5;
			this.BG2Box.TabStop = false;
			this.BG2Box.Click += new System.EventHandler(this.BG2Box_Click);
			// 
			// BackgroundEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 361);
			this.Controls.Add(this.BG2Box);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.BG1Box);
			this.Controls.Add(this.OKButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "BackgroundEditor";
			this.Text = "Background Editor";
			((System.ComponentModel.ISupportInitialize)(this.BG1Box)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.BG2Box)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.PictureBox BG1Box;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox BG2Box;
	}
}