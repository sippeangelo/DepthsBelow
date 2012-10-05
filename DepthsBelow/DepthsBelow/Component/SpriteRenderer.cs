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
			Transform transform = this.Parent.GetComponent<Transform>();
			//spriteBatch.Draw(Texture, tc.Position, this.Color);
			spriteBatch.Draw(Texture, transform.World.Position + new Vector2(Grid.TileSize / 2, Grid.TileSize / 2), null, Color, transform.World.Rotation, transform.World.Origin, Scale, SpriteEffects, 0);
		}
	}
}
