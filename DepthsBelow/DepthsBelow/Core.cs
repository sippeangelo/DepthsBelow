using System;
using System.Collections.Generic;
using System.Linq;
using Krypton.Lights;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DepthsBelow
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Core : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public Krypton.KryptonEngine KryptonEngine;

		public Camera Camera;

		public static MouseInput MouseInput;
		public List<Soldier> Squad; 
		public static Map Map;

		public Core()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.IsFullScreen = false;

			//graphics.SynchronizeWithVerticalRetrace = false;
			this.IsFixedTimeStep = false;
			graphics.ApplyChanges();

			this.IsMouseVisible = true;

			KryptonEngine = new Krypton.KryptonEngine(this, "KryptonEffect");
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			// Initialize lighting engine
			this.KryptonEngine.Initialize();
			this.KryptonEngine.SpriteBatchCompatablityEnabled = true;
			this.KryptonEngine.CullMode = CullMode.CullClockwiseFace;
			this.KryptonEngine.AmbientColor = new Color(35, 35, 35);

			Camera = new Camera(this);
			Squad = new List<Soldier>();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			Map = Content.Load<Map>("maps/Cave.Level1");
			// Load map objects
			Map.Initialize(this, KryptonEngine);

			// DEBUG: Test lights
			var light = new Krypton.Lights.Light2D()
				{
					Texture = Krypton.LightTextureBuilder.CreatePointLight(this.GraphicsDevice, 512),
					Range = (float)(500),
					Color = Color.White,
					Intensity = 1f,
					Angle = MathHelper.TwoPi,
					X = 17 * Grid.TileSize,
					Y = 7 * Grid.TileSize,
					Fov = MathHelper.TwoPi / 15,
					IsOn = true,
					ShadowType = ShadowType.Illuminated
				};
			this.KryptonEngine.Lights.Add(light);
			light = new Krypton.Lights.Light2D()
			{
				Texture = Krypton.LightTextureBuilder.CreatePointLight(this.GraphicsDevice, 512),
				Range = (float)(100),
				Color = Color.White,
				Intensity = 1f,
				Angle = MathHelper.TwoPi,
				X = 17 * Grid.TileSize,
				Y = 7 * Grid.TileSize,
				Fov = MathHelper.TwoPi,
				IsOn = true,
				ShadowType = ShadowType.Illuminated
			};
			this.KryptonEngine.Lights.Add(light);

			// TODO: use this.Content to load your game content here
			Soldier.LoadContent(this);

			MouseInput = new MouseInput(this);
			MouseInput.LoadContent();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			Camera.Update(gameTime);
			MouseInput.Update(gameTime);

			// Update soldiers in squad
			foreach (var soldier in Squad)
				soldier.Update(gameTime);

			foreach (Krypton.Lights.Light2D light in KryptonEngine.Lights)
				light.Position = Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			// Assign the matrix and pre-render the lightmap.
			this.KryptonEngine.Matrix = Camera.Transform;
			this.KryptonEngine.Bluriness = 1;
			this.KryptonEngine.LightMapPrepare();

			GraphicsDevice.Clear(Color.Black);
			
			/*
			 *	Draw game world
			 */
			//GraphicsDevice.BlendState = BlendState.AlphaBlend;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Camera.Transform);

				// Draw the level
				Map.Draw(spriteBatch);

				// Draw units
				foreach (var soldier in Squad)
				{
					var sr = soldier.GetComponent<Component.SpriteRenderer>();
					if (sr != null)
						sr.Draw(spriteBatch);
				}

			spriteBatch.End();

			/*
			 *	Draw Krypton lighting
			 */
			this.KryptonEngine.Draw(gameTime);

			/*
			 *	Draw HUD elements
			 */
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.Transform);

				// Draw mouse input visuals
				MouseInput.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
