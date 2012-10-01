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

		private MouseState lastMouseState;

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
			if (!core.IsActive)
				return;

			float elapsed = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

			MouseState ms = Mouse.GetState();

			if (ms.X < 20)
				Position.X += Speed * elapsed;
			if (ms.X > Core.GraphicsDevice.Viewport.Width - 20)
				Position.X -= Speed * elapsed;

			if (ms.Y < 20)
				Position.Y += Speed * elapsed;
			if (ms.Y > Core.GraphicsDevice.Viewport.Height - 20)
				Position.Y -= Speed * elapsed;

			int scrollChange = ms.ScrollWheelValue - lastMouseState.ScrollWheelValue;

			if (scrollChange > 0)
				Zoom *= 1.15f;
			else if (scrollChange < 0)
				Zoom /= 1.15f;

			// Store the current screen and world position of the mouse for zoom compensation
			var mousePosBefore = new Vector2(ms.X, ms.Y);
			var mousePosBeforeWorld = this.ScreenToWorld(mousePosBefore);

			// Apply the matrix transformation
			Transform = Matrix.Identity
			            * Matrix.CreateRotationZ(Rotation)
			            * Matrix.CreateScale(Zoom)
						* Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0));

			// If we've zoomed
			if (scrollChange != 0)
			{
				// Translate the stored world position back to new screen coordinates
				var mousePosAfter = this.WorldToScreen(mousePosBeforeWorld);
				var change = mousePosBefore - mousePosAfter;

				// Update position and matrix transformation
				Position += change;
				Transform *= Matrix.CreateTranslation(new Vector3(change.X, change.Y, 0));
			}

			lastMouseState = ms;
		}
	}
}
