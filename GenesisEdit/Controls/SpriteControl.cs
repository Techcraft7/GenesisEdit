﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenesisEdit.Compiler;
using GenesisEdit.Forms;

namespace GenesisEdit.Controls
{
	internal partial class SpriteControl : UserControl
	{
		public Sprite Sprite { get; private set; }

		private OpenFileDialog openDialog = new OpenFileDialog()
		{
			Title = "Open Image",
			Filter = Utils.AutoFilter("png", "jpg", "bmp")
		};

		public SpriteControl()
		{
			InitializeComponent();
			Sprite = new Sprite(null);
			NameBox.Text = Sprite.Name;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (Parent != null)
			{
				Parent.Controls.Remove(this);
			}
			Dispose();
		}

		private void NameBox_TextChanged(object sender, EventArgs e)
		{
			if (!Utils.IsValidIdentifier(NameBox.Text) && !NameBox.Text.Equals(string.Empty))
			{
				NameBox.Text = $"Sprite_{new Random().Next():X}";
			}
			Sprite.Name = NameBox.Text;
		}

		private void PreviewBox_Click(object sender, EventArgs e)
		{
			BackgroundEditor.SetImage(ref PreviewBox);
			Sprite.Texture = (Bitmap)PreviewBox.Image;
		}

		private void Resizer_Tick(object sender, EventArgs e) => Width = Parent.Width;
	}
}