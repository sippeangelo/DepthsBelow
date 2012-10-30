﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow.Component
{
	/// <summary>
	/// Sprite render component.
	/// Handles rendering of a 2D texture.
	/// </summary>
	public class SpriteRenderer : Component
	{
		/// <summary>
		/// The texture to be rendered.
		/// </summary>
		public Texture2D Texture;
        public Texture2D AlternativeTexture;
		/// <summary>
		/// Color modifier.
		/// </summary>
		public Color Color;
		/// <summary>
		/// Scale of the texture.
		/// </summary>
		public float Scale;
		public SpriteEffects SpriteEffects;

		public SpriteRenderer(Entity parent) 
			: base(parent)
		{
			this.Color = Color.White;
			this.Scale = 1;
			this.SpriteEffects = SpriteEffects.None;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var transform = this.Parent.GetComponent<Transform>();
            if (AlternativeTexture == null)
            {
                spriteBatch.Draw(Texture, transform.World.Position + transform.World.Origin, null, Color, transform.World.Rotation, transform.World.Origin, Scale, SpriteEffects, 0);
            }
            else
            {
                spriteBatch.Draw(AlternativeTexture, transform.World.Position + transform.World.Origin - new Vector2(Grid.TileSize, Grid.TileSize), null, Color, transform.World.Rotation, transform.World.Origin, Scale, SpriteEffects, 0);
            }
		}
	}
}
