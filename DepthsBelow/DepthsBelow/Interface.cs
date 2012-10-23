using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DepthsBelow.GUI;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DepthsBelow
{
	public class Interface
	{
		Core core;

		/// <summary>
		/// The master frame covering the whole game screen. Parent of all frames.
		/// This frame handles all mouse interaction with the game world
		/// </summary>
		public Frame UIParent;

		public List<Frame> PanicFrames = new List<Frame>();
		public List<Frame> UnitFrames = new List<Frame>();
		public Dictionary<Soldier, Frame> SoldierToFrame = new Dictionary<Soldier, Frame>();
		
		Random random = new Random();

		private bool checkingDirection;
		private Vector2 directionStart;

		public Interface(Core core)
		{
			this.core = core;

			UIParent = new Frame();
			UIParent.Width = GameServices.GetService<GraphicsDevice>().Viewport.Width;
			UIParent.Height = GameServices.GetService<GraphicsDevice>().Viewport.Height;

			// Selection rectangle
			var selectionFrame = new Frame(UIParent);
			selectionFrame.Texture = Utility.GetSolidTexture();
			selectionFrame.Color = Color.Red * 0.5f;
			selectionFrame.Visible = false;
			UIParent["selectionFrame"] = selectionFrame;

			// OnClick
			UIParent.OnClick += delegate(Frame frame, GUIManager.MouseEventArgs args)
			{
				KeyboardState ks = Keyboard.GetState();
				MouseState ms = args.MouseState;

				var mousePos = new Point(ms.X, ms.Y);
				var mouseRectangle = new Rectangle(mousePos.X, mousePos.Y, 1, 1);
				var mouseWorldPos = core.Camera.ScreenToWorld(new Vector2(mousePos.X, mousePos.Y));
				var mouseWorldRectangle = new Rectangle((int)mouseWorldPos.X, (int)mouseWorldPos.Y, 1, 1);

				if (args.LeftButton == ButtonState.Pressed)
				{
					Debug.WriteLine("Left button pressed on UIParent");
					// Deselect all units
					/*if (!ks.IsKeyDown(Keys.LeftControl))
					{
						foreach (var unit in core.Squad)
							unit.Selected = false;

						foreach (var unitFrame in UnitFrames)
							unitFrame.Color = Color.Black;
					}

					// Select all units in the rectangle
					foreach (var soldier in core.Squad)
					{
						if (mouseWorldRectangle.Intersects(soldier.GetComponent<Component.Collision>().Rectangle))
						{
							soldier.Selected = true;

							foreach (var unitFrame in UnitFrames)
							{
								if (unitFrame["soldier"] == soldier)
									unitFrame.Color = Color.Blue;
							}
						}
					}*/
				}



				// Hide the selection rectangle
				//selectionRectangle = Rectangle.Empty;
			};

			// OnPress
			UIParent.OnPress += delegate(Frame frame, GUIManager.MouseEventArgs args)
			{
				//KeyboardState ks = Keyboard.GetState();
				MouseState ms = args.MouseState;
				var mousePos = new Point(ms.X, ms.Y);
				var mouseRectangle = new Rectangle(mousePos.X, mousePos.Y, 1, 1);
				var mouseWorldPos = core.Camera.ScreenToWorld(new Vector2(mousePos.X, mousePos.Y));
				var mouseWorldRectangle = new Rectangle((int)mouseWorldPos.X, (int)mouseWorldPos.Y, 1, 1);

				// Show the selection frame
				if (args.LeftButton == ButtonState.Pressed)
				{
					var selectFrame = (GUI.Frame)frame["selectionFrame"];
					if (!selectFrame.Visible)
					{
						selectFrame.X = mousePos.X;
						selectFrame.Y = mousePos.Y;
						selectFrame.Visible = true;
					}
				}

				if (args.RightButton == ButtonState.Pressed)
				{
					// Unit rotation
					foreach (var unit in core.Squad)
					{
						if (unit.Selected && mouseWorldRectangle.Intersects(unit.GetComponent<Component.Collision>().Rectangle))
						{
							checkingDirection = true;
							directionStart = unit.Transform.World + unit.Transform.World.Origin;
						}
					}
				}
			};

			// OnRelease
			UIParent.OnRelease += delegate(Frame frame, GUIManager.MouseEventArgs args)
			{
				KeyboardState ks = Keyboard.GetState();
				MouseState ms = args.MouseState;
				var mousePos = new Point(ms.X, ms.Y);
				var mouseRectangle = new Rectangle(mousePos.X, mousePos.Y, 1, 1);
				var mouseWorldPos = core.Camera.ScreenToWorld(new Vector2(mousePos.X, mousePos.Y));
				var mouseWorldRectangle = new Rectangle((int)mouseWorldPos.X, (int)mouseWorldPos.Y, 1, 1);

				if (args.LeftButton == ButtonState.Pressed)
				{
					// Deselect all units
					if (!ks.IsKeyDown(Keys.LeftControl))
					{
						foreach (var unit in core.Squad)
							unit.Selected = false;

						foreach (var unitFrame in UnitFrames)
							unitFrame.Color = Color.Black;
					}
					
					// Select all soldier units inside the selection rectangle
					var selectFrame = (GUI.Frame)frame["selectionFrame"];
					if (selectFrame.Visible)
					{
						Console.WriteLine("Before: " + selectFrame.AbsoluteRectangle.X + ", " + selectFrame.AbsoluteRectangle.Y + ", " + selectFrame.AbsoluteRectangle.Width + ", " + selectFrame.AbsoluteRectangle.Height);

						var intersectionRectangle = core.Camera.ScreenToWorld(selectFrame.AbsoluteRectangle);

						Console.WriteLine("After: " + intersectionRectangle.X + ", " + intersectionRectangle.Y + ", " + intersectionRectangle.Width + ", " + intersectionRectangle.Height);


						// Select all units in the rectangle
						foreach (var soldier in core.Squad)
						{
							if (intersectionRectangle.Intersects(soldier.GetComponent<Component.Collision>().Rectangle))
							{
								soldier.Selected = true;
								foreach (var unitFrame in UnitFrames)
								{
									if (unitFrame["soldier"] == soldier)
										unitFrame.Color = Color.Blue;
								}
							}
						}

						selectFrame.Visible = false;
					}
				}

				// Send orders with right click
				if (args.RightButton == ButtonState.Pressed)
				{
					if (checkingDirection)
					{
						checkingDirection = false;
					}
					else
					{
						Debug.WriteLine("Right button pressed on UIParent");
						foreach (var unit in core.Squad)
							if (unit.Selected)
								unit.GetComponent<Component.PathFinder>().Goal = Grid.WorldToGrid(mouseWorldPos);
					}
				}
			};

			var button = new GUI.Button(UIParent);
			button.SetTexture("images/Enter");
			button.X = 200;
			button.Y = 100;
			button.Width = 55;
			button.Height = 40;

			// Test GUI frames
			/*var frame = new GUI.Frame()
			{
				Y = 100
			};

			var button = new GUI.Button(frame)
			{
				Width = 48,
				Height = 23,
				Texture = core.Content.Load<Texture2D>("images/Enter"),
				OnClick = delegate(Point clickPos)
				{
					Debug.WriteLine("That ugly button was clicked at (" + clickPos.X + "," + clickPos.Y + ")");
				}
			};

			var text = new GUI.Text(frame);
			text.SetFont("Arial");
			text.Value = "Hello world!";*/

			// Create some unit frames
			/*var panicFrame = CreatePanicFrame();
			var unit1 = CreateUnitFrame("Flight captain Rainbow");
			unit1.X = panicFrame.X;
			unit1.Y = panicFrame.Y + panicFrame.Height;
			unit1.OnClick += delegate(Frame frame, Frame.MouseEventArgs args)
			{
				frame.Color = Color.Red;
			};
			var unit2 = CreateUnitFrame("Mr. Sparkle");
			unit2.X = unit1.X;
			unit2.Y = unit1.Y + unit1.Height;
			unit2.OnClick += delegate(Frame frame, Frame.MouseEventArgs args)
			{
				frame.Color = Color.Red;
			};*/

			/*var frame1 = new GUI.Frame();
			frame1.Layer = 0;
			frame1.SetTexture("images/Enter");
			frame1.Color = Color.Red;
			frame1.Width = 100;
			frame1.Height = 100;
			frame1.OnClick += delegate(Frame frame, Frame.MouseEventArgs args)
			{
				if (frame.Color == Color.Red)
					frame.Color = Color.Green;
				else
					frame.Color = Color.Red;
			};
				var frame1_child = new GUI.Frame(frame1);
				frame1_child.SetTexture("images/Enter");
				frame1_child.Color = Color.Purple;
				frame1_child.Width = 60;
				frame1_child.Height = 60;
				frame1_child.X = 20;
				frame1_child.Y = 20;

			var frame2 = new GUI.Frame();
			frame2.Layer = 0;
			frame2.SetTexture("images/Enter");
			frame2.Color = Color.Blue;
			frame2.Width = 100;
			frame2.Height = 100;
			frame2.X = 50;
			frame2.Y = 50;
			frame2.OnClick += delegate(Frame frame, Frame.MouseEventArgs args)
			{
				if (frame.Color == Color.Blue)
					frame.Color = Color.Yellow;
				else
					frame.Color = Color.Blue;
			};*/
		}

		private GUI.Frame CreatePanicFrame()
		{
			var frame = new GUI.Frame(UIParent);
			frame.SetTexture("images/GUI/unit_background");
			frame.Color = Color.Black;
			frame.Width = 164;
			frame.Height = 19;

			frame.OnClick += delegate(Frame frame1, GUIManager.MouseEventArgs args)
			{
				DynamicGroupManager.Group group = (DynamicGroupManager.Group)frame1["group"];
				if (group != null)
					group.Panic += 5;
			};

			var panicBarBg = new GUI.Frame(frame);
			panicBarBg.SetTexture("images/GUI/health_background");
			panicBarBg.Width = 136;
			panicBarBg.Height = 13;
			panicBarBg.X = 3;
			panicBarBg.Y = 3;
			frame["panicBarBg"] = panicBarBg;

			var panicBar = new GUI.Frame(panicBarBg);
			panicBar.SetTexture("images/GUI/bar_solid");
			panicBar.Color = new Color(87, 55, 253);
			panicBar.Width = (int)(panicBarBg.Width * 0.5);
			panicBar.Height = 11;
			panicBar.X = 1;
			panicBar.Y = 1;
			frame["panicBar"] = panicBar;

			var text = new GUI.Text(frame);
			text.SetFont("fonts/UnitName");
			text.Value = "0%";
			text.X = 4;
			text.Y = 1;
			frame["text"] = text;

			return frame;
		}

		private GUI.Frame CreateUnitFrame(Soldier soldier)
		{
			var frame = new GUI.Frame(UIParent);
			frame.SetTexture("images/GUI/unit_background");
			frame.Color = Color.Black;
			frame.Width = 164;
			frame.Height = 35;

			var nameText = new GUI.Text(frame);
			nameText.SetFont("fonts/UnitName");
			nameText.Value = soldier.Name + " " + soldier.GetHashCode();
			nameText.X = 4;
			nameText.Y = 1;
			frame["nameText"] = nameText;

			var healthBarBg = new GUI.Frame(frame);
			healthBarBg.SetTexture("images/GUI/health_background");
			healthBarBg.Width = 136;
			healthBarBg.Height = 13;
			healthBarBg.X = 3;
			healthBarBg.Y = 19;
			frame["healthBarBg"] = healthBarBg;

			var healthBar = new GUI.Frame(healthBarBg);
			healthBar.SetTexture("images/GUI/bar_solid");
			healthBar.Color = Color.Red;
			healthBar.Width = (int)((healthBarBg.Width - 2) * (soldier.GetComponent<Component.Stat>().HP / soldier.GetComponent<Component.Stat>().MaxHP));
			healthBar.Height = 11;
			healthBar.X = 1;
			healthBar.Y = 1;
			frame["healthBar"] = healthBar;

			return frame;
		}

		public void CreateUnitFrames(List<Soldier> squad)
		{
			// TODO: ForEach squad in Squads...
			/*GUI.Frame lastFrame = null;
			foreach (Soldier soldier in squad)
			{
				// TODO: Remove this test ;) 
				soldier.GetComponent<Component.Stat>().HP = random.Next(0, (int) soldier.GetComponent<Component.Stat>().MaxHP - 1);

				var uFrame = CreateUnitFrame(soldier);
				uFrame["soldier"] = soldier;
				soldier["unitFrame"] = uFrame;
				if (lastFrame != null)
				{
					uFrame.X = lastFrame.X;
					uFrame.Y = lastFrame.Y + lastFrame.Height;
				}
				uFrame.OnClick += delegate(Frame f, GUIManager.MouseEventArgs e)
				{
					var s = (Soldier) f["soldier"];
					KeyboardState ks = Keyboard.GetState();

					if (!ks.IsKeyDown(Keys.LeftControl))
					{
						// Deselect all units
						foreach (var unitFrame in UnitFrames)
						{
							((Soldier) unitFrame["soldier"]).Selected = false;
							unitFrame.Color = Color.Black;
						}
					}

					if (!s.Selected)
					{
						s.Selected = true;
						uFrame.Color = Color.Blue;
					}
				};
				UnitFrames.Add(uFrame);
				lastFrame = uFrame;
			}*/

			foreach (var soldier in squad)
			{
				PanicFrames.Add(CreatePanicFrame());

				// TODO: Remove this test ;) 
				soldier.GetComponent<Component.Stat>().HP = random.Next(0, (int)soldier.GetComponent<Component.Stat>().MaxHP - 1);

				var uFrame = CreateUnitFrame(soldier);
				uFrame["soldier"] = soldier;
				soldier["unitFrame"] = uFrame;
				uFrame.OnClick += delegate(Frame f, GUIManager.MouseEventArgs e)
				{
					var s = (Soldier)f["soldier"];
					KeyboardState ks = Keyboard.GetState();

					if (!ks.IsKeyDown(Keys.LeftControl))
					{
						// Deselect all units
						foreach (var unitFrame in UnitFrames)
						{
							((Soldier)unitFrame["soldier"]).Selected = false;
							unitFrame.Color = Color.Black;
						}
					}

					if (!s.Selected)
					{
						s.Selected = true;
						uFrame.Color = Color.Blue;
					}
				};
				UnitFrames.Add(uFrame);
			}
		}

		public void UpdateUnitFrames()
		{
			core.GroupManager.UpdateGroups();
			var groups = core.GroupManager.Groups;

			foreach (var panicFrame in PanicFrames)
				panicFrame.Visible = false;

			GUI.Frame lastFrame = null;
			for (int index = 0; index < groups.Count; index++)
			{
				var group = groups[index];

				var pFrame = PanicFrames[index];
				pFrame["group"] = group;
				var pFrameBarBg = (GUI.Frame)pFrame["panicBarBg"];
				var pFrameBar = (GUI.Frame)pFrame["panicBar"];
				var pFrameText = (GUI.Text)pFrame["text"];
				pFrameBar.Width = (int)(pFrameBarBg.Width * (group.Panic / group.MaxPanic));
				pFrameText.Value = group.Panic.ToString() + " (" + (group.Panic / group.MaxPanic * 100f).ToString() + "%)";

				pFrame.Visible = true;
				if (lastFrame != null)
				{
					pFrame.X = lastFrame.X;
					pFrame.Y = lastFrame.Y + lastFrame.Height + 15;
				}
				lastFrame = pFrame;

				foreach (Soldier soldier in @group.Entities)
				{
					var frame = (GUI.Frame) soldier["unitFrame"];
					frame.X = lastFrame.X;
					frame.Y = lastFrame.Y + lastFrame.Height;
					lastFrame = frame;
				}
			}
		}

		public void Update(GameTime gameTime)
		{
			MouseState ms = Mouse.GetState();
			var mousePos = new Point(ms.X, ms.Y);
			var mouseRectangle = new Rectangle(mousePos.X, mousePos.Y, 1, 1);
			var mouseWorldPos = core.Camera.ScreenToWorld(new Vector2(mousePos.X, mousePos.Y));
			var mouseWorldRectangle = new Rectangle((int)mouseWorldPos.X, (int)mouseWorldPos.Y, 1, 1);

			// Update the selection rectangle
			var selectionFrame = (GUI.Frame)UIParent["selectionFrame"];
			if (selectionFrame.Visible)
			{
				selectionFrame.Width = (int)mousePos.X - selectionFrame.X;
				selectionFrame.Height = (int)mousePos.Y - selectionFrame.Y;
			}

			// Update rotations
			if (checkingDirection)
			{
				foreach (var unit in core.Squad)
					if (unit.Selected)
					{
						float directionX = mouseWorldPos.X - unit.Transform.World.X;
						float directionY = mouseWorldPos.Y - unit.Transform.World.Y;
						//var mouseDirection = mouseWorldPos;
						//mouseDirection.Normalize();
						unit.GetComponent<Component.Transform>().World.Rotation =
							(float)
							Math.Atan2(mouseWorldPos.Y - unit.Transform.World.Y - unit.Transform.World.Origin.Y,
							           mouseWorldPos.X - unit.Transform.World.X - unit.Transform.World.Origin.X);
					}
			}

			UpdateUnitFrames();
		}
	}
}
