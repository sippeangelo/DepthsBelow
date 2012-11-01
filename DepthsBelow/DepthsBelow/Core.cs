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
		private SpriteBatch spriteBatch;
		private Effect lightShader;
		private RenderTarget2D lightRenderTarget;
		private RenderTarget2D sceneRenderTarget;

		public Lua Lua;

		public TurnManager TurnManager;

		public Camera Camera;
		public Interface Interface;

		public/* static*/ MouseInput MouseInput;
        public static KeyboardInput KeyboardInput;
		public List<Soldier> Squad;
		public DynamicGroupManager GroupManager;
        public List<SmallEnemy> Swarm;
        public List<Shot> Volley;

        public List<MonsterSpawn> Spawn;

		public static Map Map;

		public SmallEnemy TestMonster;

		public int Brightness = 30;

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
			lightShader = Content.Load<Effect>("shaders/LightEffect");
			lightRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			sceneRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

			Map = Content.Load<Map>("maps/prototyp2");
			//Map = Content.Load<Map>("maps/Aztek.Test");
            //Map = Content.Load<Map>("maps/Cave.Level1");

			// Load map objects
			Map.ParseObjects(this);

			Squad[0].Name = "Dean H. Jones";
			Squad[0].Transform.World.Rotation = -MathHelper.PiOver2;
			Squad[1].Name = "Walter L. Verville";
			Squad[1].Transform.World.Rotation = -MathHelper.PiOver2;
			Squad[2].Name = "Patrick Y. Southerland";
			Squad[2].Transform.World.Rotation = -MathHelper.PiOver2;
			Squad[3].Name = "Andrew C. Friend";
			Squad[3].Transform.World.Rotation = -MathHelper.PiOver2;
			Squad[4].Name = "Gary B. Whitley";
			Squad[4].Transform.World.Rotation = -MathHelper.PiOver2;

			// Center the camera on a unit
			Camera.Position = Squad[0].Transform.World.Position * -1 + new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

			GroupManager = new DynamicGroupManager(Squad.Cast<Entity>().ToList(), (float)Grid.TileSize * 3.5f);
           
			Interface.CreateUnitFrames(Squad);

			// TODO: use this.Content to load your game content here
			Soldier.LoadContent();
            //SmallEnemy.LoadContent();
            //Shot.LoadContent();
            
			MouseInput = new MouseInput(this);
			MouseInput.LoadContent();
            KeyboardInput = new KeyboardInput(this);

			TestMonster = new SmallEnemy(ref Swarm);
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
			MouseState ms = Mouse.GetState();

			// Draw to the lighting render target
			GraphicsDevice.SetRenderTarget(lightRenderTarget);
			GraphicsDevice.Clear(new Color(Brightness, Brightness, Brightness));
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null,
							  Camera.Transform);

			// Draw flashlights
			foreach (var light in EntityManager.GetComponents<Component.Flashlight>())
					light.Draw(spriteBatch);

			spriteBatch.End();
			GraphicsDevice.SetRenderTarget(null);

			// Draw the scene to a render target
			GraphicsDevice.SetRenderTarget(sceneRenderTarget);
			GraphicsDevice.Clear(Color.Black);
			// Start drawing using the Camera transform
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null,
			                  Camera.Transform);

			// Draw the level
			Map.Draw(spriteBatch);

			// Draw entities
			foreach (var spriteRenderer in EntityManager.GetComponents<Component.SpriteRenderer>())
			{
				// Only render illuminated entities
				var position = Camera.WorldToScreen(spriteRenderer.Parent.Transform.World + spriteRenderer.Parent.Transform.World.Origin);
				var sourceRect = new Rectangle((int)position.X, (int)position.Y, 1, 1);
				if (
					sourceRect.X >= 0 
					&& sourceRect.Y >= 0 
					&& sourceRect.X < lightRenderTarget.Width 
					&& sourceRect.Y < lightRenderTarget.Height
				)
				{
					var lightData = new Color[1];
					lightRenderTarget.GetData<Color>(0, sourceRect, lightData, 0, 1);
					if (lightData[0] != new Color(Brightness, Brightness, Brightness))
						spriteRenderer.Draw(spriteBatch);
				}
			}

#if DEBUG
			var debugTexture = new Texture2D(GraphicsDevice, 1, 1);
			debugTexture.SetData(new Color[] { Color.White });
			foreach (var collisionComponent in EntityManager.GetComponents<Component.Collision>())
			{
				spriteBatch.Draw(debugTexture, collisionComponent.Rectangle, Color.Red * 0.5f);
			}
#endif

			// Draw mouse input visuals
			MouseInput.Draw(spriteBatch);

			spriteBatch.End();
			GraphicsDevice.SetRenderTarget(null);

			// Draw the scene from the render target with the light shader
			lightShader.Parameters["LightsTexture"].SetValue(lightRenderTarget);
			lightShader.CurrentTechnique.Passes[0].Apply();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, null, null, lightShader,
							  Matrix.Identity);
			spriteBatch.Draw(sceneRenderTarget, Vector2.Zero, Color.White);
			spriteBatch.End();

			// Draw GUI components
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, Matrix.Identity);
			GUIManager.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
