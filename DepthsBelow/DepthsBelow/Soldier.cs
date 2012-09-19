using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DepthsBelow
{
	class Soldier : Entity
	{
		Core game;
		static Texture2D Texture;

		// Components
		TransformComponent transform;

		static public void LoadContent(Core game)
		{
			Texture = game.Content.Load<Texture2D>("images/soldier");
		}

		public Soldier(Core game)
		{
			if (Texture == null)
				LoadContent(game);

			SpriteRenderComponent rc = new SpriteRenderComponent(this);
			rc.Texture = Texture;
			this.AddComponent(rc);

			transform = new TransformComponent(this);
			this.AddComponent(transform);
		}

		public void Update(GameTime gameTime)
		{
			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Left))
				transform.Position.X--;
		}
	}
}
