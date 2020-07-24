namespace GenesisEdit.Forms
{
	partial class ProgressWindow
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
			this.ActionText = new System.Windows.Forms.Label();
			this.Progress = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// ActionText
			// 
			this.ActionText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ActionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ActionText.Location = new System.Drawing.Point(0, 0);
			this.ActionText.Name = "ActionText";
			this.ActionText.Size = new System.Drawing.Size(404, 106);
			this.ActionText.TabIndex = 0;
			this.ActionText.Text = "Please wait while GenesisEdit works on things...";
			this.ActionText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Progress
			// 
			this.Progress.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.Progress.Location = new System.Drawing.Point(0, 106);
			this.Progress.Name = "Progress";
			this.Progress.Size = new System.Drawing.Size(404, 35);
			this.Progress.TabIndex = 1;
			// 
			// ProgressWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(404, 141);
			this.Controls.Add(this.ActionText);
			this.Controls.Add(this.Progress);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ProgressWindow";
			this.Text = "Working... (0%)";
			this.Shown += new System.EventHandler(this.ProgressWindow_Shown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label ActionText;
		private System.Windows.Forms.ProgressBar Progress;
	}
}