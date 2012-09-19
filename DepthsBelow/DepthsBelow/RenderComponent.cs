using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	public class SpriteRenderComponent : Component
	{
		public Texture2D Texture;

		public SpriteRenderComponent(Entity parent) : base(parent)
		{
			
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			TransformComponent tc = this.Parent.GetComponent<TransformComponent>();
			spriteBatch.Draw(Texture, tc.Position, Color.White);
		}
	}
}
