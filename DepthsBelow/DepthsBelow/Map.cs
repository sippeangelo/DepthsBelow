using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	public class Tile
	{
		public Texture2D Texture;
		public Rectangle SourceRectangle;
		public SpriteEffects SpriteEffects;
	}

	public class Layer
	{
		public int Width;
		public int Height;
		public Tile[] Tiles;
	}

	public class Map
	{
		public int TileWidth;
		public int TileHeight;
		public List<Layer> Layers = new List<Layer>();

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var layer in Layers)
			{
				for (int y = 0; y < layer.Height; y++)
				{
					for (int x = 0; x < layer.Width; x++)
					{
						Tile tile = layer.Tiles[y*layer.Width + x];
						if (tile != null)
						{
							spriteBatch.Draw(
								tile.Texture,
								new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight),
								tile.SourceRectangle,
								Color.White,
								0,
								Vector2.Zero,
								tile.SpriteEffects,
								0);
						}
					}
				}
			}
		}
	}
}
