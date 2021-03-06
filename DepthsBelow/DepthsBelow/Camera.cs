﻿using System;
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

		public Rectangle ScreenToWorld(Rectangle screenRectangle)
		{
			var pos = ScreenToWorld(new Vector2(screenRectangle.X, screenRectangle.Y));
			var size = ScreenToWorld(new Vector2(screenRectangle.X + screenRectangle.Width, screenRectangle.Y + screenRectangle.Height)) - pos;
			return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
		}

		public Vector2 WorldToScreen(Vector2 worldPos)
		{
			return Vector2.Transform(worldPos, Transform);
		}

		public Rectangle WorldToScreen(Rectangle worldRectangle)
		{
			var pos = WorldToScreen(new Vector2(worldRectangle.X, worldRectangle.Y));
			var size = WorldToScreen(new Vector2(worldRectangle.X + worldRectangle.Width, worldRectangle.Y + worldRectangle.Height)) - pos;
			return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
		}

		public void Update(GameTime gameTime)
		{
			float elapsed = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

			MouseState ms = Mouse.GetState();

			if (ms.X < 20)
				Position.X += Speed * elapsed;
			if (ms.X > core.GraphicsDevice.Viewport.Width - 20)
				Position.X -= Speed * elapsed;

			if (ms.Y < 20)
				Position.Y += Speed * elapsed;
			if (ms.Y > core.GraphicsDevice.Viewport.Height - 20)
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

			Transform = Matrix.Identity
						* Matrix.CreateRotationZ(Rotation)
						* Matrix.CreateScale(Zoom)
						* Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0));

			lastMouseState = ms;
		}
	}
}
