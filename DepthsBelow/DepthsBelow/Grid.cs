using System;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
	/// <summary>
	/// Global grid utility functions.
	/// </summary>
    public static class Grid
    {
		/// <summary>
		/// The size of a tile in pixels.
		/// </summary>
        public const int TileSize = 32;

		/// <summary>
		/// Converts a grid position into a world position.
		/// </summary>
		/// <param name="gridPos">The grid position to convert.</param>
		/// <returns>Returns a Vector2 world position.</returns>
		public static Vector2 GridToWorld(Point gridPos)
		{
			return new Vector2(gridPos.X * TileSize, gridPos.Y * TileSize);
		}

		/// <summary>
		/// Converts a world position into a grid position.
		/// </summary>
		/// <param name="screenPos">The world position to convert.</param>
		/// <returns>Returns a Point grid position.</returns>
	    public static Point WorldToGrid(Vector2 screenPos)
	    {
		    return new Point(
				(int)Math.Floor(screenPos.X / TileSize),	
				(int)Math.Floor(screenPos.Y / TileSize)	
			);
		}
    }
}
