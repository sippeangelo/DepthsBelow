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

		private KeyboardState lastKeyboardState;

		static public void LoadContent(Core game)
		{
			Texture = game.Content.Load<Texture2D>("images/soldier");
		}

		public Soldier(Core game)
		{
			if (Texture == null)
				LoadContent(game);

			Component.SpriteRenderer rc = new Component.SpriteRenderer(this);
			rc.Texture = Texture;
			rc.Color = Color.HotPink;
			this.AddComponent(rc);
		}

		public void Update(GameTime gameTime)
		{
			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Right) && lastKeyboardState.IsKeyUp(Keys.Right))
				gridTransform.Position.X += 1;
			lastKeyboardState = ks;

			Console.WriteLine(pixelTransform.X + " " + gridTransform.X);
			if (pixelTransform.Position.X < gridTransform.X * 32)
			{
				pixelTransform.Position.X += 2;
			}
		}
	}
}
