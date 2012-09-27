using Microsoft.Xna.Framework;

namespace DepthsBelow
{
    public static class Grid
    {
        public const int TileSize = 32;

		public static Vector2 GridToScreen(Vector2 gridPos)
		{
			return gridPos * TileSize;
		}
		public static Vector2 ScreenToGrid(Vector2 screenPos)
		{
			return screenPos / TileSize;
		}
    }
}
