using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
	public class Camera
	{
		public float Zoom;
		public Matrix Transform;
		public Vector2 Position;
		public float Rotation;

		public float Speed;

		private Core core;

		public Camera(Core core)
		{
			this.core = core;

			Zoom = 1.0f;
			Rotation = 0.0f;
			Position = Vector2.Zero;
			Speed = 300;
		}

		public Vector2 ScreenToWorld(Vector2 screenPos)
		{
			return Vector2.Transform(screenPos, Matrix.Invert(Transform));
		}

		public Vector2 WorldToScreen(Vector2 worldPos)
		{
			return Vector2.Transform(worldPos, Transform);
		}

		public void Update(GameTime gameTime)
		{
			float elapsed = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

			MouseState ms = Mouse.GetState();

			if (ms.X < 20)
				Position.X -= Speed * elapsed;
			if (ms.X > core.GraphicsDevice.Viewport.Width - 20)
				Position.X += Speed * elapsed;

			if (ms.Y < 20)
				Position.Y -= Speed * elapsed;
			if (ms.Y > core.GraphicsDevice.Viewport.Height - 20)
				Position.Y += Speed * elapsed;

			Transform = 
				Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0))
				* Matrix.CreateRotationZ(Rotation)
				* Matrix.CreateScale(Zoom)
			;
		}
	}
}
