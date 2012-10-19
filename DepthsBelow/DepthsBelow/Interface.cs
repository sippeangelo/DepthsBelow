using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DepthsBelow.GUI;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	public class Interface
	{
		Core core;

		public Interface(Core core)
		{
			this.core = core;

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
			unit1.OnClick += delegate(Frame frame, Frame.OnClickArgs args)
			{
				frame.Color = Color.Red;
			};
			var unit2 = CreateUnitFrame("Mr. Sparkle");
			unit2.X = unit1.X;
			unit2.Y = unit1.Y + unit1.Height;
			unit2.OnClick += delegate(Frame frame, Frame.OnClickArgs args)
			{
				frame.Color = Color.Red;
			};*/

			var frame1 = new GUI.Frame();
			frame1.Layer = 0;
			frame1.SetTexture("images/Enter");
			frame1.Color = Color.Red;
			frame1.Width = 100;
			frame1.Height = 100;
			frame1.OnClick += delegate(Frame frame, Frame.OnClickArgs args)
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
				frame1_child.OnClick += delegate(Frame frame, Frame.OnClickArgs args)
				{
					if (frame.Color == Color.Purple)
						frame.Color = Color.Pink;
					else
						frame.Color = Color.Purple;
				};

			var frame2 = new GUI.Frame();
			frame2.Layer = 0;
			frame2.SetTexture("images/Enter");
			frame2.Color = Color.Blue;
			frame2.Width = 100;
			frame2.Height = 100;
			frame2.X = 50;
			frame2.Y = 50;
			frame2.OnClick += delegate(Frame frame, Frame.OnClickArgs args)
			{
				if (frame.Color == Color.Blue)
					frame.Color = Color.Yellow;
				else
					frame.Color = Color.Blue;
			};
		}

		private GUI.Frame CreatePanicFrame()
		{
			var frame = new GUI.Frame();
			frame.SetTexture("images/GUI/unit_background");
			frame.Color = Color.Black;
			frame.Width = 164;
			frame.Height = 19;
	
			var healthBg = new GUI.Frame(frame);
			healthBg.SetTexture("images/GUI/health_background");
			healthBg.Width = 136;
			healthBg.Height = 13;
			healthBg.X = 3;
			healthBg.Y = 3;

			var healthBar = new GUI.Frame(healthBg);
			healthBar.SetTexture("images/GUI/bar_solid");
			healthBar.Color = new Color(87, 55, 253);
			healthBar.Width = (int)(healthBg.Width * 0.5);
			healthBar.Height = 11;
			healthBar.X = 1;
			healthBar.Y = 1;

			return frame;
		}

		private GUI.Frame CreateUnitFrame(string unitName)
		{
			var frame = new GUI.Frame();
			frame.SetTexture("images/GUI/unit_background");
			frame.Color = Color.Black;
			frame.Width = 164;
			frame.Height = 35;

				var nameText = new GUI.Text(frame);
				nameText.SetFont("fonts/UnitName");
				nameText.Value = unitName;
				nameText.X = 4;
				nameText.Y = 1;

				var healthBg = new GUI.Frame(frame);
				healthBg.SetTexture("images/GUI/health_background");
				healthBg.Width = 136;
				healthBg.Height = 13;
				healthBg.X = 3;
				healthBg.Y = 19;

					var healthBar = new GUI.Frame(healthBg);
					healthBar.SetTexture("images/GUI/bar_solid");
					healthBar.Color = Color.Red;
					healthBar.Width = (int)(healthBg.Width * 0.5f);
					healthBar.Height = 11;
					healthBar.X = 1;
					healthBar.Y = 1;

			return frame;
		}
	}
}
