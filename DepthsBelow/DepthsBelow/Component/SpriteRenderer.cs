using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow.Component
{
	public class SpriteRenderer : Component
	{
		public Texture2D Texture;
		public Color Color;

		public SpriteRenderer(Entity parent) : base(parent)
		{
			this.Color = Color.White;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			PixelTransform tc = this.Parent.GetComponent<PixelTransform>();
			spriteBatch.Draw(Texture, tc.Position, this.Color);
		}
	}
}
