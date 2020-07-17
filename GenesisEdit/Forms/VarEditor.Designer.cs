using System.Windows.Forms;

namespace GenesisEdit.Forms
{
	partial class VarEditor : Form
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
			this.OKButton = new System.Windows.Forms.Button();
			this.AddButton = new System.Windows.Forms.Button();
			this.VariablesList = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			//
			//OKButton
			//
			this.OKButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.OKButton.Location = new System.Drawing.Point(0, 295);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(390, 66);
			this.OKButton.TabIndex = 0;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			//
			//AddButton
			//
			this.AddButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.AddButton.Location = new System.Drawing.Point(0, 0);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(390, 33);
			this.AddButton.TabIndex = 3;
			this.AddButton.Text = "Add";
			this.AddButton.UseVisualStyleBackColor = true;
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
			//
			//VariablesList
			//
			this.VariablesList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VariablesList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.VariablesList.Location = new System.Drawing.Point(0, 33);
			this.VariablesList.Margin = new System.Windows.Forms.Padding(0);
			this.VariablesList.Name = "VariablesList";
			this.VariablesList.Size = new System.Drawing.Size(390, 262);
			this.VariablesList.TabIndex = 1;
			//
			//VarEditor
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(390, 361);
			this.Controls.Add(this.VariablesList);
			this.Controls.Add(this.AddButton);
			this.Controls.Add(this.OKButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "VarEditor";
			this.Text = "Variables Editor";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.FlowLayoutPanel VariablesList;
	}
}