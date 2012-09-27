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
		public float Scale;
		public SpriteEffects SpriteEffects;

		public SpriteRenderer(Entity parent) : base(parent)
		{
			this.Color = Color.White;
			this.Scale = 1;
			this.SpriteEffects = SpriteEffects.None;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			PixelTransform transform = this.Parent.GetComponent<PixelTransform>();
			//spriteBatch.Draw(Texture, tc.Position, this.Color);
			spriteBatch.Draw(Texture, transform.Position + transform.Origin, null, Color, transform.Rotation, transform.Origin, Scale, SpriteEffects, 0);
		}
	}
}
