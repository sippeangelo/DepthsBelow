using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DepthsBelow.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
	public static class GUIManager
	{
		/// <summary>
		/// GUI mouse event arguments.
		/// </summary>
		public class MouseEventArgs : EventArgs
		{
			/// <summary>
			/// The XNA mouse state.
			/// </summary>
			public MouseState MouseState;

			public ButtonState LeftButton;
			public ButtonState MiddleButton;
			public ButtonState RightButton;
			public int ScrollWheelValue;

			/// <summary>
			/// The time of the click, since the start of the program.
			/// </summary>
			public TimeSpan Time;
		}

		public static List<Frame> Frames = new List<Frame>();
		private static int uidIndex = 0;

		public static int Add(Frame frame)
		{
			Frames.Add(frame);
			return CreateUID();
		}

		public static void Remove(Frame frame)
		{
			Frames.Remove(frame);
		}

		public static int CreateUID()
		{
			return uidIndex++;
		}

		/// <summary>
		/// Calculates which frames are intersecting a point.
		/// </summary>
		/// <param name="point">A point on the screen.</param>
		/// <returns>Returns a list of frames intersecting the point.</returns>
		public static List<Frame> FramesIntersectingPoint(Point point)
		{
			var rect = new Rectangle(point.X, point.Y, 1, 1);
			return Frames.Where(frame => rect.Intersects(frame.AbsoluteRectangle)).ToList();
		}

		public static bool RaiseAt(string eventName, EventArgs eventArgs, Point position)
		{
			// Create a list of all frames which intersects with the click position
			var intersections = FramesIntersectingPoint(position);
			// Filter it by event
			intersections = intersections.Where(frame => frame.GetSubscriberCount(eventName) > 0).ToList();
			// Get the topmost frame of that list
			var topFrame = GetTopFrame(intersections);

			if (topFrame != null)
				return topFrame.Raise(eventName, eventArgs);
			else
				return false;
		}

		/// <summary>
		/// Gets the top frame in the frame stack.
		/// </summary>
		/// <param name="frames">A list of frames to check.</param>
		/// <returns>Returns the topmost frame.</returns>
		private static Frame GetTopFrame(List<Frame> frames)
		{
			if (frames.Count == 0)
				return null;

			if (frames.Count == 1)
				return frames.First();

			return frames.OrderByDescending(f => f, new FrameSort()).First();
		}

		public static void Update(GameTime gameTime)
		{
			foreach (var frame in Frames)
				frame.Update(gameTime);
		}

		public static void Draw(SpriteBatch spriteBatch)
		{
			foreach (var frame in Frames.OrderBy(f => f, new FrameSort()))
			{
				frame.Draw(spriteBatch);

				
			}
		}

		/// <summary>
		/// Sort frames based on layer, using UID as tie breaker.
		/// </summary>
		private class FrameSort : IComparer<Frame>
		{
			public int Compare(Frame a, Frame b)
			{
				var aLayerSum = a.LayerSum;
				var bLayerSum = b.LayerSum;

				// Compare the layer sum
				if (aLayerSum > bLayerSum)
					return 1;
				if (aLayerSum < bLayerSum)
					return -1;

				// If the result is indecisive, pick the frame with the biggest UID
				if (aLayerSum == bLayerSum)
				{
					if (a.UID > b.UID)
						return 1;

					if (a.UID < b.UID)
						return -1;
				}

				return 0;
			}
		}
	}
}
