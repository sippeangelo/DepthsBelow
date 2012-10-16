using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	class Interface
	{
		Core core;

		public Interface(Core core)
		{
			this.core = core;

			// Test GUI frames
			var frame = new GUI.Frame()
			{
				Y = 100
			};

			var button = new GUI.Button(frame)
			{
				Width = 48,
				Height = 23,
				Texture = core.Content.Load<Texture2D>("images/Enter"),
				OnClick = delegate(Point clickPos)
				{
					Debug.WriteLine("That ugly button was clicked at (" + clickPos.X + "," + clickPos.Y + ")");
				}
			};

			var text = new GUI.Text(frame, core.Content.Load<SpriteFont>("Arial"));
			text.Value = "Hello world!";
		}
	}
}
