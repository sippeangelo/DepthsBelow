using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow.GUI
{
	/// <summary>
	/// A basic text control.
	/// </summary>
	class Text : Frame
	{
		/// <summary>
		/// Text to display.
		/// </summary>
		public String Value;
		/// <summary>
		/// Font to use while rendering.
		/// </summary>
		public SpriteFont Font;

		public Text()
			: base()
		{
			this.Color = Color.White;
		}

		public Text(Frame parent)
			: base(parent)
		{
			this.Color = Color.White;
		}

		/// <summary>
		/// Set the font to use while rendering.
		/// </summary>
		/// <param name="spriteFontName">Filename of the font.</param>
		public void SetFont(string spriteFontName)
		{
			Font = GameServices.GetService<ContentManager>().Load<SpriteFont>(spriteFontName);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);

			if (Visible && Font != null)
				spriteBatch.DrawString(Font, Value, new Vector2(Rectangle.X + Parent.AbsoluteRectangle.X, Rectangle.Y + Parent.AbsoluteRectangle.Y), Color);
		}
	}
}
