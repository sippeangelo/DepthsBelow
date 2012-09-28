using System;
using System.Collections.Generic;
using System.Linq;
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

		public Camera Camera;
		MouseInput mouseInput;
		//public Soldier soldier;
		public List<Soldier> Squad; 
		private Map map;

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

			map = Content.Load<Map>("maps/Cave.Level1");
			map.ParseObjects(this);

			// TODO: use this.Content to load your game content here
			Soldier.LoadContent(this);
			//soldier = new Soldier(this);

			mouseInput = new MouseInput(this);
			mouseInput.LoadContent();
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
			mouseInput.Update(gameTime);

			// TODO: Add your update logic here
			//soldier.Update(gameTime);
			foreach (var soldier in Squad)
				soldier.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// Start drawing using the Camera transform
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
			                  Camera.Transform);

			// Draw the level
			map.Draw(spriteBatch);
			// Draw units
			// TODO: Foreach etc...
			foreach (var soldier in Squad)
			{
				var sr = soldier.GetComponent<Component.SpriteRenderer>();
				Console.WriteLine("Drawing soldier at: " + soldier.pixelTransform.X + "," + soldier.pixelTransform.Y);
				if (sr != null)
					sr.Draw(spriteBatch);
			}

		// Draw mouse input visuals
			mouseInput.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
