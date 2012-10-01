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

        Vector2 directionStart;
        Vector2 directionNow;

        bool checkingDirection = false;

		
		public MouseInput(Core core)
		{
			this.core = core;

			selectionRectangle = Rectangle.Empty;
			gridRectangle = Rectangle.Empty;
		}


		public void LoadContent()
		{
			selectionTexture = new Texture2D(Core.GraphicsDevice, 1, 1);
			selectionTexture.SetData(new Color[] { Color.White });

			gridTexture = new Texture2D(Core.GraphicsDevice, 1, 1);
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
                    {
						unit.GetComponent<Component.PathFinder>().Goal = Grid.WorldToGrid(mouseWorldPos);
                    }

			}
            if (ms.RightButton == ButtonState.Pressed)
            {
                foreach (var unit in core.Squad)
                {
                    if (unit.Selected && gridRectangle.Intersects(unit.GetComponent<Component.Collision>().Rectangle))
                    {
                        checkingDirection = true;
                        directionStart = unit.Transform.World + unit.Transform.World.Origin;
                    }
                }
            }
            if (checkingDirection == true && ms.RightButton == ButtonState.Pressed)
            {
                foreach (var unit in core.Squad)
                    if (unit.Selected)
                    {
                        directionNow.X = mouseWorldPos.X;
                        directionNow.Y = mouseWorldPos.Y;
                        float directionX = mouseWorldPos.X - unit.Transform.World.X;
                        float directionY = mouseWorldPos.Y - unit.Transform.World.Y;
                        var mouseDirection = mouseWorldPos;
                        mouseDirection.Normalize();
                        //float angle = (float)Math.Acos(Vector2.Dot(new Vector2(0, 1), mouseDirection)) * MathHelper.TwoPi;
                        /*if (directionX < 0)
                        {
                            directionX *= -1;
                            unit.GetComponent<Component.Transform>().World.Rotation = (float)Math.Atan(directionY / directionX);
                        }
                        else
                        {
                            */unit.GetComponent<Component.Transform>().World.Rotation = (float)Math.Atan2(mouseWorldPos.Y - unit.Transform.World.Y, mouseWorldPos.X - unit.Transform.World.X);/*
                        }*/
                        //Console.WriteLine(angle);
                        //unit.GetComponent<Component.Transform>().World.Rotation = angle;
                    }
            }
            else
            {
                checkingDirection = false;
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

            if (checkingDirection == true) 
            {
				Texture2D blank = new Texture2D(Core.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                blank.SetData(new[] { Color.White });
                DrawLine(spriteBatch, blank, 2, Color.White, directionStart, directionNow);
            }

		}

        void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
	}
}
