using System;
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

		public static void Add(Frame frame)
		{
			Frames.Add(frame);
		}

		public static void Remove(Frame frame)
		{
			Frames.Remove(frame);
		}

		public static void Update(GameTime gameTime)
		{
			foreach (var frame in Frames)
				frame.Update(gameTime);
		}

		public static void Draw(SpriteBatch spriteBatch)
		{
			foreach (var frame in Frames)
				frame.Draw(spriteBatch);
		}
	}
}
