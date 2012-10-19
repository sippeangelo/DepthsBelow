using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DepthsBelow.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	static class GUIManager
	{
		public static List<Frame> Frames = new List<Frame>();
		private static int uidIndex = 0;

		public static int Add(Frame frame)
		{
			Frames.Add(frame);
			return uidIndex++;
		}

		public static void Remove(Frame frame)
		{
			Frames.Remove(frame);
		}

		public static void Click(Point position, TimeSpan time)
		{
			var clickRectangle = new Rectangle(position.X, position.Y, 1, 1);

			var intersections = new List<Frame>();

			foreach (var frame in Frames)
			{
				if (clickRectangle.Intersects(frame.AbsoluteRectangle))
					intersections.Add(frame);
			}

			var topFrame = GetTopFrame(intersections);

			if (topFrame != null)
			{
				var e = new Frame.OnClickArgs()
					        {
						        Position = position,
						        Time = time
					        };
				topFrame.Click(e);
			}
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
				frame.Draw(spriteBatch);
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
