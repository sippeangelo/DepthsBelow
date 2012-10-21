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

		public EntityManager EntityManager;
		public TurnManager TurnManager;

		public Camera Camera;
		public Interface Interface;

		public static MouseInput MouseInput;
        public static KeyboardInput KeyboardInput;
		public List<Soldier> Squad;
		public DynamicGroupManager GroupManager;
        public List<SmallEnemy> Swarm;
        public List<Shot> Volley;

        public List<MonsterSpawn> Spawn;

		public static Map Map;

		public SmallEnemy TestMonster;

		public Core()
		{
			GraphicsDeviceManager = new GraphicsDeviceManager(this);
			GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
			GraphicsDeviceManager.PreferredBackBufferHeight = 720;
			GraphicsDeviceManager.IsFullScreen = false;
			//graphics.SynchronizeWithVerticalRetrace = false;
			GraphicsDeviceManager.ApplyChanges();

			this.IsFixedTimeStep = false;
			this.IsMouseVisible = true;

			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			GameServices.AddService<GraphicsDevice>(GraphicsDevice);
			GameServices.AddService<ContentManager>(Content);

			Lua = new Lua();

			EntityManager = new EntityManager();
			TurnManager = new TurnManager(new string[] { "Player", "Computer" });

			Camera = new Camera(this);
			Interface = new Interface(this);

			Squad = new List<Soldier>();
			Swarm = new List<SmallEnemy>();
			Volley = new List<Shot>();

            Spawn = new List<MonsterSpawn>();

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
			GroupManager = new DynamicGroupManager(Squad.Cast<Entity>().ToList(), (float)Grid.TileSize * 1.5f);
			Interface.CreateUnitFrames(Squad);

			// TODO: use this.Content to load your game content here
			Soldier.LoadContent();
            SmallEnemy.LoadContent();
            Shot.LoadContent();
            
			MouseInput = new MouseInput(this);
			MouseInput.LoadContent();
            KeyboardInput = new KeyboardInput(this);

			TestMonster = new SmallEnemy(EntityManager, ref Swarm);
			TestMonster.X = 12;
			TestMonster.Y = 4;
            Swarm.Add(TestMonster);

			// Load Lua scripts after everything is created
			Lua.LoadScripts();
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

			EntityManager.Update(gameTime);

			Interface.Update(gameTime);
			GUIManager.Update(gameTime);
            
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
			Map.Draw(spriteBatch);

			// Draw entities
			EntityManager.Draw(spriteBatch);

			// Draw mouse input visuals
			MouseInput.Draw(spriteBatch);

			spriteBatch.End();

			// Start drawing GUI components
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, Matrix.Identity);
			GUIManager.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
