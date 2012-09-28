using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	public class Tile
	{
		public int LocalId;
		public Texture2D Texture;
		public Rectangle SourceRectangle;
		public SpriteEffects SpriteEffects;
	}

	public class Layer
	{
		public int Width;
		public int Height;
		public string Name;
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
				if (layer.Name == "Collision")
					continue;

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

		public byte[,] GetCollisionMap()
		{
			byte[,] collisionMap = new byte[1024, 1024];
			foreach (var layer in Layers)
			{
				if (layer.Name == "Collision")
				{
					for (int y = 0; y < layer.Height; y++)
					{
						for (int x = 0; x < layer.Width; x++)
						{
							Tile tile = layer.Tiles[y*layer.Width + x];
							if (tile == null || tile.LocalId == 0)
								collisionMap[x, y] = 1;
							else
								collisionMap[x, y] = 0;
						}
					}

					break;
				}
			}

			//if (collisionMap == null)
			//	throw new Exception("Map lacks a Collision layer!");

			return collisionMap;
		}
	}
}
