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
        bool readjust = false;

		
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
			Vector2 mouseWorldPos = core.Camera.ScreenToWorld(new Vector2(ms.X, ms.Y));
			KeyboardState ks = Keyboard.GetState();

			// Selection rectangle
			if (ms.LeftButton == ButtonState.Pressed)
			{
                
				if (selectionRectangle == Rectangle.Empty)
				{
                    directionStart = mouseWorldPos;
					selectionRectangle = new Rectangle((int)mouseWorldPos.X, (int)mouseWorldPos.Y, 0, 0);
                    readjust = false;
				}
				else
				{
                    if (mouseWorldPos.X > directionStart.X && mouseWorldPos.Y < directionStart.Y || mouseWorldPos.X < directionStart.X && mouseWorldPos.Y > directionStart.Y)
                    {
                        int rectangleX = (int)mouseWorldPos.X - ((int)mouseWorldPos.X - (int)directionStart.X);
                        int rectangleY = (int)mouseWorldPos.Y;
                        int rectangleWidth = (int)mouseWorldPos.X - (int)directionStart.X;
                        int rectangleHeight = (int)directionStart.Y - (int)mouseWorldPos.Y;
                        selectionRectangle = new Rectangle(rectangleX, rectangleY, rectangleWidth, rectangleHeight);
                        readjust = true;
                    }
                    else
                    {
                        if (readjust == true)
                        {
                            selectionRectangle = new Rectangle((int)directionStart.X, (int)directionStart.Y, 0, 0);
                        }
                        selectionRectangle.Width = (int)mouseWorldPos.X - selectionRectangle.X;
                        selectionRectangle.Height = (int)mouseWorldPos.Y - selectionRectangle.Y;
                        readjust = false;
                    }
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
                        if (checkingDirection == false) {
						    unit.GetComponent<Component.PathFinder>().Goal = Grid.WorldToGrid(mouseWorldPos);
                        }
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
                            */unit.GetComponent<Component.Transform>().World.Rotation = (float)Math.Atan2(mouseWorldPos.Y - unit.Transform.World.Y - unit.Transform.World.Origin.Y, mouseWorldPos.X - unit.Transform.World.X - unit.Transform.World.Origin.X);/*
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
                RenderHelpers.DrawLine(spriteBatch, Color.White, 2, directionStart, directionNow);
            }

		}

	}
}
