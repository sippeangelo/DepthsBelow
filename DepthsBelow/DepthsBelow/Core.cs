using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DepthsBelow.GUI;
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
		public static GraphicsDeviceManager GraphicsDeviceManager;
		SpriteBatch spriteBatch;

		public Lua Lua;

		public Camera Camera;

        public bool PlayerTurn = true;
		public static MouseInput MouseInput;
        public static KeyboardInput KeyboardInput;
		public List<Soldier> Squad;
        public List<SmallEnemy> Swarm;
        public List<Shot> Volley;
		public static Map Map;

		public SmallEnemy TestMonster;

		public Core()
		{
			GraphicsDeviceManager = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
			GraphicsDeviceManager.PreferredBackBufferHeight = 720;
			GraphicsDeviceManager.IsFullScreen = false;

			//graphics.SynchronizeWithVerticalRetrace = false;
			this.IsFixedTimeStep = false;
			GraphicsDeviceManager.ApplyChanges();

			this.IsMouseVisible = true;

			GameServices.AddService<GraphicsDevice>(GraphicsDevice);
			GameServices.AddService<ContentManager>(Content);
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
            Swarm = new List<SmallEnemy>();
            Volley = new List<Shot>();

			TestMonster = new SmallEnemy(this, ref Swarm);

			// Run scripts after everything is initialized
			Lua = new Lua();
			Lua.LoadScripts();

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
			Map.ParseObjects(this);

			// TODO: use this.Content to load your game content here
			Soldier.LoadContent(this);
            SmallEnemy.LoadContent(this);
            Shot.LoadContent(this);
            
			MouseInput = new MouseInput(this);
			MouseInput.LoadContent();
            KeyboardInput = new KeyboardInput(this);

            TestMonster.X = 12;
            TestMonster.Y = 4;
			
			//frame.Add(text);
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
            KeyboardInput.Update(gameTime);

			// Update soldiers in squad
			foreach (var soldier in Squad)
				soldier.Update(gameTime);

            foreach (var body in Swarm)
                body.Update(gameTime);

            foreach (var bullet in Volley)
                bullet.Update(gameTime);

            TestMonster.Update(gameTime);
			GUIManager.Update(gameTime);

			base.Update(gameTime);
		}

        public int FindDistance(Vector2 target, Vector2 start)
        {
            Vector2 combine = new Vector2(target.X - start.X, target.Y - start.Y);
            
            int result = 0;
            int firstRestult = (int)Math.Sqrt((int)combine.X^2 + (int)combine.Y^2);

            result = (int)Vector2.Distance(target, start);

            return result;
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
			Map.Draw(spriteBatch);

			// Draw units
			foreach (var soldier in Squad)
			{
				var sr = soldier.GetComponent<Component.SpriteRenderer>();
				if (sr != null)
					sr.Draw(spriteBatch);
			}
            foreach (var body in Swarm)
            {
                var sr = body.GetComponent<Component.SpriteRenderer>();
                if (sr != null)
                    sr.Draw(spriteBatch);
            }
            foreach (var bullet in Volley)
            {
                var sr = bullet.GetComponent<Component.SpriteRenderer>();
                if (sr != null)
                    sr.Draw(spriteBatch);
            }

			// Draw mouse input visuals
			MouseInput.Draw(spriteBatch);

			TestMonster.GetComponent<Component.SpriteRenderer>().Draw(spriteBatch);

			spriteBatch.End();

			// Start drawing GUI components
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, Matrix.Identity);
			GUIManager.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}


	}
}
