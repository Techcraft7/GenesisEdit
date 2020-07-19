namespace GenesisEdit.Controls
{
	partial class VariableControl
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

		#region Component Designer generated code

		///<summary> 
		///Required method for Designer support - do not modify 
		///the contents of this method with the code editor.
		///</summary>
		private void InitializeComponent()
		{
			this.DeleteButton = new System.Windows.Forms.Button();
			this.NameBox = new System.Windows.Forms.TextBox();
			this.TypeSelector = new System.Windows.Forms.ComboBox();
			this.LengthSel = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.LengthSel)).BeginInit();
			this.SuspendLayout();
			// 
			// DeleteButton
			// 
			this.DeleteButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.DeleteButton.Location = new System.Drawing.Point(344, 0);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(40, 40);
			this.DeleteButton.TabIndex = 0;
			this.DeleteButton.Text = "X";
			this.DeleteButton.UseVisualStyleBackColor = true;
			this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// NameBox
			// 
			this.NameBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.NameBox.Location = new System.Drawing.Point(3, 11);
			this.NameBox.MaxLength = 20;
			this.NameBox.Name = "NameBox";
			this.NameBox.Size = new System.Drawing.Size(152, 20);
			this.NameBox.TabIndex = 1;
			this.NameBox.Text = "VarName";
			this.NameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
			// 
			// TypeSelector
			// 
			this.TypeSelector.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TypeSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TypeSelector.FormattingEnabled = true;
			this.TypeSelector.Items.AddRange(new object[] {
            "Byte",
            "Word",
            "Long"});
			this.TypeSelector.Location = new System.Drawing.Point(161, 10);
			this.TypeSelector.Name = "TypeSelector";
			this.TypeSelector.Size = new System.Drawing.Size(105, 21);
			this.TypeSelector.TabIndex = 2;
			this.TypeSelector.SelectedIndexChanged += new System.EventHandler(this.TypeSelector_SelectedIndexChanged);
			// 
			// LengthSel
			// 
			this.LengthSel.Location = new System.Drawing.Point(273, 10);
			this.LengthSel.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.LengthSel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.LengthSel.Name = "LengthSel";
			this.LengthSel.Size = new System.Drawing.Size(65, 20);
			this.LengthSel.TabIndex = 3;
			this.LengthSel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.LengthSel.ValueChanged += new System.EventHandler(this.LengthSel_ValueChanged);
			// 
			// VariableControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.LengthSel);
			this.Controls.Add(this.TypeSelector);
			this.Controls.Add(this.NameBox);
			this.Controls.Add(this.DeleteButton);
			this.Name = "VariableControl";
			this.Size = new System.Drawing.Size(384, 40);
			this.Resize += new System.EventHandler(this.VariableControl_Resize);
			((System.ComponentModel.ISupportInitialize)(this.LengthSel)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button DeleteButton;
		private System.Windows.Forms.TextBox NameBox;
		private System.Windows.Forms.ComboBox TypeSelector;
		private System.Windows.Forms.NumericUpDown LengthSel;
	}
}
