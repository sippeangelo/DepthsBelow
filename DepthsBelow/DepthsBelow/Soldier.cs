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
	public class Soldier : Entity
	{
		Core game;
		private bool _selected;
		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				_selected = value;
				this.GetComponent<Component.SpriteRenderer>().Color = (value) ? Color.Blue : Color.HotPink; 
			}
		}
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

			Component.Collision cc = new Component.Collision(this, 32, 32);
			this.AddComponent(cc);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			/*KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Right) && lastKeyboardState.IsKeyUp(Keys.Right))
				gridTransform.Position.X += 1;
			lastKeyboardState = ks;*/
			
			if (pixelTransform.Position.X < gridTransform.X * 32)
			{
				pixelTransform.Position.X += 2;
			}
		}
	}
}
