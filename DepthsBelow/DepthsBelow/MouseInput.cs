﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		private Point pressLocation;
		
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

		public void OnPress(MouseState ms, GameTime gameTime)
		{
			pressLocation = new Point(ms.X, ms.Y);
			var mouseEvent = new GUIManager.MouseEventArgs()
			{
				MouseState = ms,
				Time = gameTime.TotalGameTime
			};
			bool handledByUI = GUIManager.RaiseAt("OnPress", mouseEvent, pressLocation);

			if (handledByUI)
				return;

			// If the mouse event is above a GUI frame that handles mouse events, do nothing
			//if (GUIManager.FramesIntersectingPoint(mousePos).Any(frame => frame.MouseEnabled))
			//	return;
		}

		public void OnRelease(MouseState ms, GameTime gameTime)
		{
			var mouseEvent = new GUIManager.MouseEventArgs()
			{
				MouseState = ms,
				Time = gameTime.TotalGameTime
			};

			bool handledByUI = GUIManager.RaiseAt("OnRelease", mouseEvent, pressLocation);

			if (handledByUI)
				return;
		}

		public void OnClick(MouseState ms, GameTime gameTime)
		{
			var mouseEvent = new GUIManager.MouseEventArgs()
			{
				MouseState = ms,
				Time = gameTime.TotalGameTime
			};

			bool handledByUI = GUIManager.RaiseAt("OnClick", mouseEvent, new Point(ms.X, ms.Y));

			if (handledByUI)
				return;
		}

		public void Update(GameTime gameTime)
		{
			MouseState ms = Mouse.GetState();
			Vector2 mouseWorldPos = core.Camera.ScreenToWorld(new Vector2(ms.X, ms.Y));
			Rectangle mouseWorldRectangle = new Rectangle((int)mouseWorldPos.X, (int)mouseWorldPos.Y, 1, 1);
			KeyboardState ks = Keyboard.GetState();
			
			// OnPress event
			if (
				(ms.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
				|| (ms.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
			)
				OnPress(ms, gameTime);

			// OnRelease event
			if (
				(ms.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
				|| (ms.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed)
			)
			{
				OnClick(ms, gameTime);
				OnRelease(ms, gameTime);
			}


			// Tooltip stuff
			var tooltip = core.Lua.GetObject<GUI.Frame>("ToolTip");
			if (tooltip != null)
			{
				tooltip.Visible = false;
				foreach(var entity in core.Squad)
				{
					if (mouseWorldRectangle.Intersects(entity.GetComponent<Component.Collision>().Rectangle))
					{
						tooltip.Visible = true;
						break;
					}
				}

				if (tooltip.Visible)
				{
					tooltip.X = ms.X + 10;
					tooltip.Y = ms.Y + 15;
				}
			}

            if (core.TurnManager.CurrentTurn == core.TurnManager["Player"])
            {
                // Selection rectangle
                /*if (ms.LeftButton == ButtonState.Pressed)
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
                }*/

                // When the mouse is released
                /*if (ms.LeftButton == ButtonState.Released && selectionRectangle != Rectangle.Empty)
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
                }*/

                // Send orders with right click
                if (ms.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed)
                {
                    if (checkingDirection == false)
                    {
                        foreach (var unit in core.Squad)
                            if (unit.Selected)
                            {
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
                                */
                            unit.GetComponent<Component.Transform>().World.Rotation = (float)Math.Atan2(mouseWorldPos.Y - unit.Transform.World.Y - unit.Transform.World.Origin.Y, mouseWorldPos.X - unit.Transform.World.X - unit.Transform.World.Origin.X);/*
                        }*/
                            //Console.WriteLine(angle);
                            //unit.GetComponent<Component.Transform>().World.Rotation = angle;
                        }
                }
                else
                {
                    checkingDirection = false;
                }
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
                Utility.DrawLine(spriteBatch, Color.White, 2, directionStart, directionNow);
            }

		}

	}
}
