using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
    public class Grid
    {
        public const int TilePixelSize = 32;
        public int TileSize() { return TilePixelSize; }
        /*public Vector2 GridToScreen(Vector2 gridPos);
        public Vector2 ScreenToGrid(Vector2 screenPos);*/

        public Vector2 GridToScreen;
        public Vector2 ScreenToGrid;

        public Grid()
        {

        }

        public Vector2 FindScreenPos(Vector2 gridPos) 
        {
            return gridPos * TileSize();
        }
        public Vector2 FindGridPos(Vector2 screenPos)
        {
            return screenPos / TileSize();
        }
    }
}
