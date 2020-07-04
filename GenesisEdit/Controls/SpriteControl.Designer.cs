namespace GenesisEdit.Controls
{
	partial class SpriteControl
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
			this.DeleteButton = new System.Windows.Forms.Button();
			this.NameBox = new System.Windows.Forms.TextBox();
			this.PreviewBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
			this.SuspendLayout();
			// 
			// DeleteButton
			// 
			this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DeleteButton.Location = new System.Drawing.Point(160, 0);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(40, 40);
			this.DeleteButton.TabIndex = 0;
			this.DeleteButton.Text = "X";
			this.DeleteButton.UseVisualStyleBackColor = true;
			this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// NameBox
			// 
			this.NameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NameBox.Location = new System.Drawing.Point(46, 11);
			this.NameBox.Name = "NameBox";
			this.NameBox.Size = new System.Drawing.Size(108, 20);
			this.NameBox.TabIndex = 1;
			this.NameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
			// 
			// PreviewBox
			// 
			this.PreviewBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.PreviewBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.PreviewBox.Location = new System.Drawing.Point(0, 0);
			this.PreviewBox.Name = "PreviewBox";
			this.PreviewBox.Size = new System.Drawing.Size(40, 40);
			this.PreviewBox.TabIndex = 2;
			this.PreviewBox.TabStop = false;
			this.PreviewBox.Click += new System.EventHandler(this.PreviewBox_Click);
			// 
			// SpriteControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PreviewBox);
			this.Controls.Add(this.NameBox);
			this.Controls.Add(this.DeleteButton);
			this.Name = "SpriteControl";
			this.Size = new System.Drawing.Size(200, 40);
			((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button DeleteButton;
		private System.Windows.Forms.TextBox NameBox;
		private System.Windows.Forms.PictureBox PreviewBox;
	}
}
