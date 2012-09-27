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
	class MouseInput
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
			MouseState ms = Mouse.GetState();
			KeyboardState ks = Keyboard.GetState();

			// Selection rectangle
			if (ms.LeftButton == ButtonState.Pressed)
			{
				if (selectionRectangle == Rectangle.Empty)
				{
					selectionRectangle = new Rectangle(ms.X + (int)core.camera.Position.X, ms.Y + (int)core.camera.Position.Y, 0, 0);
				}
				else
				{
					selectionRectangle.Width = ms.X - selectionRectangle.X + (int)core.camera.Position.X;
					selectionRectangle.Height = ms.Y - selectionRectangle.Y + (int)core.camera.Position.Y;
				}
			}
			if (ms.LeftButton == ButtonState.Released && selectionRectangle != Rectangle.Empty)
			{
				if (!ks.IsKeyDown(Keys.LeftControl))
					core.soldier.Selected = false;

				if (selectionRectangle.Intersects(core.soldier.GetComponent<Component.Collision>().Rectangle))
				{
					core.soldier.Selected = true;
				}
				
				selectionRectangle = Rectangle.Empty;
			}

			// Grid highlighting
			Point currentGrid = Grid.ScreenToGrid(new Vector2(ms.X + core.camera.Position.X, ms.Y + core.camera.Position.Y));
			Vector2 currentScreen = Grid.GridToScreen(currentGrid);
			gridRectangle = new Rectangle((int)currentScreen.X, (int)currentScreen.Y, Grid.TileSize, Grid.TileSize);

			lastMouseState = ms;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(selectionTexture, selectionRectangle, Color.Red * 0.5f);
			spriteBatch.Draw(gridTexture, gridRectangle, Color.Yellow * 0.3f);
		}
	}
}
