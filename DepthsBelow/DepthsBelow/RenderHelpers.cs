using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	static class RenderHelpers
	{
		static Texture2D blank;

		public static void DrawLine(SpriteBatch batch, Color color, float width, Vector2 start, Vector2 end)
		{
			if (blank == null)
			{
				blank = new Texture2D(Core.GraphicsDeviceManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
				blank.SetData(new[] { color });
			}

			float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
			float length = Vector2.Distance(start, end);

			batch.Draw(blank, start, null, color,
					   angle, Vector2.Zero, new Vector2(length, width),
					   SpriteEffects.None, 0);
		}
	}
}
