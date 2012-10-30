using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	public class MapTile
	{
		public int LocalId;
		public Texture2D Texture;
		public Rectangle SourceRectangle;
		public SpriteEffects SpriteEffects;
	}

	public enum MapObjectType : byte
	{
		Plain,
		Tile,
		Polygon,
		Polyline
	}

	public class MapObject
	{
		//public MapObjectType ObjectType;
		public string Name;
		public string Type;
		public Rectangle Bounds;
		public List<Vector2> Points;
	}

	public class MapLayer
	{
		public int Width;
		public int Height;
		public string Name;
	}

	public class MapTileLayer : MapLayer
	{
		public MapTile[] Tiles;
	}

	public class MapObjectLayer : MapLayer
	{
		public MapObject[] Objects;
	}

	public class Map
	{
		public int TileWidth;
		public int TileHeight;
		public List<MapLayer> Layers = new List<MapLayer>();

		public Map()
		{

		}

		public void ParseObjects(Core core)
		{
			foreach (var layer in Layers)
			{
				var objectLayer = layer as MapObjectLayer;

				if (objectLayer == null)
					continue;

				foreach (var mapObject in objectLayer.Objects)
				{
					if (mapObject.Type == "SquadStart")
					{
						var soldier = new Soldier(ref core.Squad);
						soldier.Name = "Derp";
						// HACK: For some reason, the tile object coordinates are offset by one tile on the Y-axis in the Tiled map file (https://github.com/bjorn/tiled/issues/91)
						var mapObjectPos = new Vector2(mapObject.Bounds.X, mapObject.Bounds.Y - Grid.TileSize);
						var mapObjectGridPos = Grid.WorldToGrid(mapObjectPos);
						soldier.X = mapObjectGridPos.X;
						soldier.Y = mapObjectGridPos.Y;
	
						core.Squad.Add(soldier);
					}
                    if (mapObject.Type == "MonsterSpawn")
                    {
                        var enemy = new MonsterSpawn("Monster", ref core.Swarm);
                        var mapObjectPos = new Vector2(mapObject.Bounds.X, mapObject.Bounds.Y - Grid.TileSize);
                        var mapObjectGridPos = Grid.WorldToGrid(mapObjectPos);
                        enemy.X = mapObjectGridPos.X;
                        enemy.Y = mapObjectGridPos.Y;
                    }
                    if (mapObject.Type == "BossSpawn")
                    {
                        var enemy = new MonsterSpawn("BUB", ref core.Swarm);
                        var mapObjectPos = new Vector2(mapObject.Bounds.X, mapObject.Bounds.Y - Grid.TileSize);
                        var mapObjectGridPos = Grid.WorldToGrid(mapObjectPos);
                        enemy.X = mapObjectGridPos.X;
                        enemy.Y = mapObjectGridPos.Y;
                    }
					if (mapObject.Type == "Door")
					{
						var door = new Door();
						var mapObjectPos = new Vector2(mapObject.Bounds.X, mapObject.Bounds.Y);
						var mapObjectGridPos = Grid.WorldToGrid(mapObjectPos);
						door.X = mapObjectGridPos.X;
						door.Y = mapObjectGridPos.Y;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var layer in Layers)
			{
				var tileLayer = layer as MapTileLayer;

				if (tileLayer == null)
					continue;

				// Don't draw the collision layer
				if (tileLayer.Name == "Collision")
					continue;

				for (int y = 0; y < tileLayer.Height; y++)
				{
					for (int x = 0; x < tileLayer.Width; x++)
					{
						MapTile mapTile = tileLayer.Tiles[y * layer.Width + x];
						if (mapTile != null)
						{
							spriteBatch.Draw(
								mapTile.Texture,
								new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight),
								mapTile.SourceRectangle,
								Color.White,
								0,
								Vector2.Zero,
								mapTile.SpriteEffects,
								0);
						}
					}
				}
			}
		}

		public byte[,] GetCollisionMap()
		{
			byte[,] collisionMap = null;

			foreach (var layer in Layers)
			{
				var tileLayer = layer as MapTileLayer;

				if (tileLayer != null && tileLayer.Name == "Collision")
				{
					collisionMap = new byte[1024,1024];
					for (int y = 0; y < tileLayer.Height; y++)
					{
						for (int x = 0; x < tileLayer.Width; x++)
						{
							MapTile tile = tileLayer.Tiles[y * layer.Width + x];
							if (tile == null || tile.LocalId == 0)
								collisionMap[x, y] = 1;
							else
								collisionMap[x, y] = 0;
						}
					}

					break;
				}
			}

			if (collisionMap == null)
				Debug.WriteLine("Map lacks a Collision layer! Pathfinding won't work.");

			return collisionMap;
		}
	}
}
