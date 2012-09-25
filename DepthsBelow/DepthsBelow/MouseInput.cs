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

		public MouseInput(Core core)
		{
			this.core = core;

			selectionRectangle = Rectangle.Empty;
		}

		public void LoadContent()
		{
			selectionTexture = new Texture2D(core.GraphicsDevice, 1, 1);
			selectionTexture.SetData(new Color[] { Color.White });
		}

		public void Update(GameTime gameTime)
		{
			MouseState ms = Mouse.GetState();

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
                /*if (selectionRectangle.Intersects()) 
                {

                }*/
				selectionRectangle = Rectangle.Empty;
			}

			lastMouseState = ms;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(selectionTexture, selectionRectangle, Color.Red * 0.5f);
		}
	}
}
