using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow.GUI
{
	/// <summary>
	/// A button frame which can handle clicks.
	/// </summary>
	public class Button : Frame
	{
		//public delegate void OnClickHandler(Point pos);

		/// <summary>
		/// Handler of OnClick events.
		/// </summary>
		//public OnClickHandler OnClick;

		private MouseState lastMouseState;

		public Button()
			: base()
		{

		}

		public Button(Frame parent)
			: base(parent)
		{
			
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			/*MouseState ms = Mouse.GetState();

			if (ms.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
			{
				// HACK: This should be handled globally somewhere else to prevent event bubbling
				if (
					ms.X >= AbsoluteRectangle.Left
					&& ms.X < AbsoluteRectangle.Right
					&& ms.Y >= AbsoluteRectangle.Top
					&& ms.Y < AbsoluteRectangle.Bottom)
				{
					if (OnClick != null)
					{
						var clickPos = new Point(ms.X - AbsoluteRectangle.X, ms.Y - AbsoluteRectangle.Y);
						OnClick(clickPos);
					}
				}
			}

			lastMouseState = ms;*/
		}
	}
}
