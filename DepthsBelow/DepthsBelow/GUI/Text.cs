using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow.GUI
{
	class Text : Frame
	{
		public String Value;
		public SpriteFont Font;
		public Color Color;

		public Text(SpriteFont font)
			: base()
		{
			this.Font = font;
			this.Color = Color.Black;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);

			spriteBatch.DrawString(Font, Value, new Vector2(Rectangle.X + Parent.Rectangle.X, Rectangle.Y + Parent.Rectangle.Y), Color);
		}
	}
}
