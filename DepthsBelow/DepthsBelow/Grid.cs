using System;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
    public static class Grid
    {
        public const int TileSize = 32;

		public static Vector2 GridToWorld(Point gridPos)
		{
			return new Vector2(gridPos.X * TileSize, gridPos.Y * TileSize);
		}

	    public static Point WorldToGrid(Vector2 screenPos)
	    {
		    return new Point(
				(int)Math.Floor(screenPos.X / TileSize),	
				(int)Math.Floor(screenPos.Y / TileSize)	
			);
		}
    }
}
