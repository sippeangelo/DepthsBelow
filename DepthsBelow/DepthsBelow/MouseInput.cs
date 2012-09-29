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
	public class MouseInput
	{
		Core core;
		MouseState lastMouseState;
		Rectangle selectionRectangle;
		Texture2D selectionTexture;
        Rectangle gridRectangle;
        Texture2D gridTexture;

		public MouseInput(Core core)
		{
			this.core = core;

			selectionRectangle = Rectangle.Empty;
			gridRectangle = Rectangle.Empty;
		}

		public void LoadContent()
		{
			selectionTexture = new Texture2D(core.GraphicsDevice, 1, 1);
			selectionTexture.SetData(new Color[] { Color.White });

            gridTexture = new Texture2D(core.GraphicsDevice, 1, 1);
            gridTexture.SetData(new Color[] { Color.White });
		}

		public void Update(GameTime gameTime)
		{
			// Only capture input if the game window is in focus
			if (!core.IsActive)
				return;

			MouseState ms = Mouse.GetState();
			Vector2 mouseWorldPos = core.Camera.ScreenToWorld(new Vector2(ms.X, ms.Y));
			KeyboardState ks = Keyboard.GetState();

			// Selection rectangle
			if (ms.LeftButton == ButtonState.Pressed)
			{
				if (selectionRectangle == Rectangle.Empty)
				{
					selectionRectangle = new Rectangle((int)mouseWorldPos.X, (int)mouseWorldPos.Y, 0, 0);
				}
				else
				{
					selectionRectangle.Width = (int)mouseWorldPos.X - selectionRectangle.X;
					selectionRectangle.Height = (int)mouseWorldPos.Y - selectionRectangle.Y;
				}
			}
			// When the mouse is released
			if (ms.LeftButton == ButtonState.Released && selectionRectangle != Rectangle.Empty)
			{
				// Deselect all units
				if (!ks.IsKeyDown(Keys.LeftControl))
				{
					foreach (var unit in core.Squad)
						unit.Selected = false;
				}

				// Select all units in the rectangle
				foreach (var unit in core.Squad)
				{
					if (selectionRectangle.Intersects(unit.GetComponent<Component.Collision>().Rectangle))
						unit.Selected = true;
				}

				// Hide the selection rectangle
				selectionRectangle = Rectangle.Empty;
			}

			// Send orders with right click
			if (ms.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed)
			{
				foreach (var unit in core.Squad)
					if (unit.Selected)
						unit.GetComponent<Component.PathFinder>().Goal = Grid.WorldToGrid(mouseWorldPos);
			}

			// Grid highlighting
			Point mouseGridPos = Grid.WorldToGrid(mouseWorldPos);
			Vector2 rectWorldPos = Grid.GridToWorld(mouseGridPos);
			gridRectangle = new Rectangle((int)rectWorldPos.X, (int)rectWorldPos.Y, Grid.TileSize, Grid.TileSize);

			lastMouseState = ms;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(selectionTexture, selectionRectangle, Color.Red * 0.5f);
			spriteBatch.Draw(gridTexture, gridRectangle, Color.Yellow * 0.3f);
		}
	}
}
