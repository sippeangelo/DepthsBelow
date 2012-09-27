using DepthsBelow.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
	public class Soldier : Entity
	{
		public static Texture2D Texture;
		private bool _selected;
		private Core game;

		private KeyboardState lastKeyboardState;

		public Soldier(Core game)
		{
			this.game = game;

			if (Texture == null)
				LoadContent(game);

			var rc = new SpriteRenderer(this) {Texture = Texture, Color = Color.HotPink};
			AddComponent(rc);

			var cc = new Collision(this, 32, 32);
			AddComponent(cc);
		}

		public bool Selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				GetComponent<SpriteRenderer>().Color = (value) ? Color.Blue : Color.HotPink;
			}
		}

		public static void LoadContent(Core game)
		{
			Texture = game.Content.Load<Texture2D>("images/soldier");
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Right) && lastKeyboardState.IsKeyUp(Keys.Right))
				gridTransform.X += 1;
			if (ks.IsKeyDown(Keys.Left) && lastKeyboardState.IsKeyUp(Keys.Left))
				gridTransform.X -= 1;
			if (ks.IsKeyDown(Keys.Up) && lastKeyboardState.IsKeyUp(Keys.Up))
				gridTransform.Y -= 1;
			if (ks.IsKeyDown(Keys.Down) && lastKeyboardState.IsKeyUp(Keys.Down))
				gridTransform.Y += 1;
			lastKeyboardState = ks;

			int speed = 2;
			if (pixelTransform.X < gridTransform.X * 32)
				pixelTransform.X += speed;
			if (pixelTransform.X > gridTransform.X * 32)
				pixelTransform.X -= speed;
			if (pixelTransform.Y < gridTransform.Y * 32)
				pixelTransform.Y += speed;
			if (pixelTransform.Y > gridTransform.Y * 32)
				pixelTransform.Y -= speed;
		}
	}
}