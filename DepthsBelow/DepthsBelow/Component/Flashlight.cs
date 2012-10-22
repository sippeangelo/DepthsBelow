using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow.Component
{
	public class Flashlight : Component
	{
		public float Scale = 1f;
		public float Intensity = 1f;
		public float Angle = 0;
		public Vector2 Position { get; private set; }
		public Vector2 Origin;
		public Texture2D Texture;
		public Color Color;

		public bool IsOn = true;

		public Flashlight(Entity parent)
			: base(parent)
		{
			Texture = GameServices.GetService<ContentManager>().Load<Texture2D>("images/lights/spot");
			Color = Color.White;
			Origin = new Vector2(50, 60);
		}

		public override void Update(GameTime gameTime)
		{
			Position = Parent.Transform.World + Parent.Transform.World.Origin;
			Angle = Parent.Transform.World.Rotation;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (IsOn)
				spriteBatch.Draw(Texture, Position, null, Color * Intensity, Angle, Origin, Scale, SpriteEffects.None, 0);
		}

	}
}
