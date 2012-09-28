using System;
using DepthsBelow.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
	public class Soldier : Entity
	{
		public static Texture2D Texture;
		public static Point Origin;
		private bool _selected;

		private KeyboardState lastKeyboardState;

		public Soldier(Core core) : base(core)
		{
			if (Texture == null)
				LoadContent(core);

			pixelTransform.Origin = new Vector2(16, 16);
			gridTransform.Position = new Point(7, 3);
			pixelTransform.Position = gridTransform.ToWorld();

			var rc = new SpriteRenderer(this) {Texture = Texture, Color = Color.White};
			AddComponent(rc);

			var cc = new Collision(this, 32, 32);
			AddComponent(cc);

			var pfc = new PathFinder(this);
			pfc.Goal = new Point(8, 39);
			AddComponent(pfc);
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

		public static void LoadContent(Core core)
		{
			Texture = core.Content.Load<Texture2D>("images/soldier2");
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Right) && lastKeyboardState.IsKeyUp(Keys.Right))
			{
				gridTransform.X += 1;
				pixelTransform.Rotation = (float)0;
			}

			if (ks.IsKeyDown(Keys.Left) && lastKeyboardState.IsKeyUp(Keys.Left))
			{
				gridTransform.X -= 1;
				pixelTransform.Rotation = (float)Math.PI;
			}
			if (ks.IsKeyDown(Keys.Up) && lastKeyboardState.IsKeyUp(Keys.Up))
			{
				gridTransform.Y -= 1;
				pixelTransform.Rotation = (float)(3.0f*Math.PI/2.0f);
			}
			if (ks.IsKeyDown(Keys.Down) && lastKeyboardState.IsKeyUp(Keys.Down))
			{
				gridTransform.Y += 1;
				pixelTransform.Rotation = (float)(Math.PI / 2.0f);
			}
			lastKeyboardState = ks;

			int speed = 1;
			if (pixelTransform.X < gridTransform.ToWorld().X)
				pixelTransform.X += speed;
			if (pixelTransform.X > gridTransform.ToWorld().X)
				pixelTransform.X -= speed;
			if (pixelTransform.Y < gridTransform.ToWorld().Y)
				pixelTransform.Y += speed;
			if (pixelTransform.Y > gridTransform.ToWorld().Y)
				pixelTransform.Y -= speed;
		}
	}
}