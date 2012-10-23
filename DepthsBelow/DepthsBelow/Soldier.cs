﻿using System;
using System.Collections.Generic;
using DepthsBelow.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
	public class Soldier : Entity
	{
		public static Texture2D Texture;
		public Color Color;
		public string Name;
		private bool _selected;

		public List<Soldier> Squad;

		private PathFinder.Node nextNode;
		private PathFinder.Node lastNode;
		private PathFinder.Node lastLastNode;

        private bool hasFiredAlready = false;

        public bool Fired
        {
            get { return hasFiredAlready; }
            set
            {
                hasFiredAlready = value;
            }
        }

        public int numberOfSteps = 14;
        int currentStep = 0;

        public int step 
        {
            get { return currentStep; }
            set 
            {
                currentStep = value;
                GetComponent<Component.Stat>().GetStep = currentStep;
            }
        }

		public Soldier(EntityManager entityManager, ref List<Soldier> squad)
			: base(entityManager)
		{
			if (Texture == null)
				LoadContent();

			this.Squad = squad;

			Transform.World.Origin = new Vector2(16, 16);

			this.Color = Color.White;
			var rc = new SpriteRenderer(this) {Texture = Texture, Color = Color.White};
			AddComponent(rc);

			var cc = new Collision(this, 32, 32);
			AddComponent(cc);

			var pfc = new PathFinder(this);
			AddComponent(pfc);

			var flashlight = new Flashlight(this)
				{
					Intensity = 0.8f
				};
			AddComponent(flashlight);

			var stat = new Component.Stat(this)
				           {
							   MaxHP = 100,
							   MaxPanic = 100,
					           //Life = 10, 
							   Defence = 10, 
							   Strength = 10, 
							   GetAim = 40, 
							   GetDodge = 20
				           };
			AddComponent(stat);
		}

		public bool Selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				GetComponent<SpriteRenderer>().Color = (value) ? Color.Blue : this.Color;
			}
		}

		public static void LoadContent()
		{
			Texture = GameServices.GetService<ContentManager>().Load<Texture2D>("images/soldier5");
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			float elapsed = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            
			if (nextNode == null)
				nextNode = GetComponent<PathFinder>().Next();

			if (nextNode != null)
			{
				List<Point> soldierCollisions = new List<Point>();

				// Collision with units at a standstill
				foreach (var soldier in Squad)
				{
					if (soldier != this && !soldier.GetComponent<PathFinder>().IsMoving)
					{
						soldierCollisions.Add(soldier.Transform.Grid);
					}
				}

				// Collision with moving units
				foreach (var soldier in Squad)
				{
					if (soldier != this)
					{
						var pathFinder = soldier.GetComponent<PathFinder>();
						var position = nextNode.Position;
						if (soldier.Transform.Grid == position)
						{
							soldierCollisions.Add(soldier.Transform.Grid);
							GetComponent<PathFinder>().RecreatePath(soldierCollisions);
							nextNode = GetComponent<PathFinder>().Next();
							break;
						}
					}
				}

				if (nextNode != null)
				{
					var nodeWorldPos = Grid.GridToWorld(nextNode.Position);
					// HACK: Make this work properly with other speeds...
					float speed = 4f;
					if (Transform.World.X < nodeWorldPos.X)
						Transform.World.X += speed;
					if (Transform.World.X > nodeWorldPos.X)
						Transform.World.X -= speed;
					if (Transform.World.Y < nodeWorldPos.Y)
						Transform.World.Y += speed;
					if (Transform.World.Y > nodeWorldPos.Y)
						Transform.World.Y -= speed;

					if (Transform.World == nodeWorldPos)
					{
                        if (step < numberOfSteps)
                        {
                            step++;
                            lastLastNode = lastNode;
                            lastNode = nextNode;
                            nextNode = GetComponent<PathFinder>().Next();
                        }
                        else
                        {
                            GetComponent<PathFinder>().Stop();
                        }
					}
				}
			}
		}
	}
}