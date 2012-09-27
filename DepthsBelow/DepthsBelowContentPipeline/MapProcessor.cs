using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TiledLib;

namespace DepthsBelowContentPipeline
{
	// Each tile has a texture, source rect, and sprite effects.
	[ContentSerializerRuntimeType("DepthsBelow.Tile, DepthsBelow")]
	public class MapTileContent
	{
		public ExternalReference<Texture2DContent> Texture;
		public Rectangle SourceRectangle;
		public SpriteEffects SpriteEffects;
	}

	// For each layer, we store the size of the layer and the tiles.
	[ContentSerializerRuntimeType("DepthsBelow.Layer, DepthsBelow")]
	public class MapLayerContent
	{
		public int Width;
		public int Height;
		public MapTileContent[] Tiles;
	}

	// For the map itself, we just store the size, tile size, and a list of layers.
	[ContentSerializerRuntimeType("DepthsBelow.Map, DepthsBelow")]
	public class MapContent
	{
		public int TileWidth;
		public int TileHeight;
		public List<MapLayerContent> Layers = new List<MapLayerContent>();
	}

	[ContentProcessor(DisplayName = "TMX Processor - DepthsBelow")]
	public class MapProcessor : ContentProcessor<TiledLib.MapContent, MapContent>
	{
		public override MapContent Process(TiledLib.MapContent input, ContentProcessorContext context)
		{
			//System.Diagnostics.Debugger.Launch();

			// build the textures
			TiledHelpers.BuildTileSetTextures(input, context, "maps");

			// generate source rectangles
			TiledHelpers.GenerateTileSourceRectangles(input, "maps");

			// now build our output, first by just copying over some data
			MapContent output = new MapContent
			{
				TileWidth = input.TileWidth,
				TileHeight = input.TileHeight
			};

			// iterate all the layers of the input
			foreach (LayerContent layer in input.Layers)
			{
				// we only care about tile layers in our demo
				TileLayerContent tlc = layer as TileLayerContent;
				if (tlc != null)
				{
					// create the new layer
					MapLayerContent outLayer = new MapLayerContent
					{
						Width = tlc.Width,
						Height = tlc.Height,
					};

					// we need to build up our tile list now
					outLayer.Tiles = new MapTileContent[tlc.Data.Length];
					for (int i = 0; i < tlc.Data.Length; i++)
					{
						// get the ID of the tile
						uint tileID = tlc.Data[i];

						// if the tile is empty, let the tile be null
						if (tileID == 0)
							continue;

						// use that to get the actual index as well as the SpriteEffects
						int tileIndex;
						SpriteEffects spriteEffects;
						TiledHelpers.DecodeTileID(tileID, out tileIndex, out spriteEffects);

						// figure out which tile set has this tile index in it and grab
						// the texture reference and source rectangle.
						ExternalReference<Texture2DContent> textureContent = null;
						Rectangle sourceRect = new Rectangle();

						// iterate all the tile sets
						foreach (var tileSet in input.TileSets)
						{
							// if our tile index is in this set
							if (tileIndex - tileSet.FirstId < tileSet.Tiles.Count)
							{
								// store the texture content and source rectangle
								textureContent = tileSet.Texture;
								sourceRect = tileSet.Tiles[(int)(tileIndex - tileSet.FirstId)].Source;

								// and break out of the foreach loop
								break;
							}
						}

						// now insert the tile into our output
						outLayer.Tiles[i] = new MapTileContent
						{
							Texture = textureContent,
							SourceRectangle = sourceRect,
							SpriteEffects = spriteEffects
						};
					}

					// add the layer to our output
					output.Layers.Add(outLayer);
				}
			}

			// return the output object. because we have ContentSerializerRuntimeType attributes on our
			// objects, we don't need a ContentTypeWriter and can just use the automatic serialization.
			return output;
		}
	}
}