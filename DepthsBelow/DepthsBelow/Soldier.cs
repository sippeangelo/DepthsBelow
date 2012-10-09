using System;
using System.Collections.Generic;
using DepthsBelow.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
	public class Soldier : Entity
	{
		public static Texture2D Texture;
		public Color Color;
		public static Point Origin;
		private bool _selected;

		public List<Soldier> Squad;

		private PathFinder.Node nextNode;
		private PathFinder.Node lastNode;
		private PathFinder.Node lastLastNode;

        public int numberOfSteps = 14;
        public int currentStep = 0;

        public int step 
        {
            get
            {
                return currentStep;
            }
            set 
            {
                currentStep = value;
            }
        }

		public Soldier(Core core, ref List<Soldier> squad) : base(core)
		{
			if (Texture == null)
				LoadContent(core);

			this.Squad = squad;

			Transform.World.Origin = new Vector2(16, 16);

			this.Color = Color.White;
			var rc = new SpriteRenderer(this) {Texture = Texture, Color = Color.White};
			AddComponent(rc);

			var cc = new Collision(this, 32, 32);
			AddComponent(cc);

			var pfc = new PathFinder(this);
			AddComponent(pfc);
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

		public static void LoadContent(Core core)
		{
			Texture = core.Content.Load<Texture2D>("images/soldier2");
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

				foreach (var soldier in Squad)
				{
					if (soldier != this && !soldier.GetComponent<PathFinder>().IsMoving)
					{
						soldierCollisions.Add(soldier.Transform.Grid);
					}
				}

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
                        if (currentStep < numberOfSteps)
                        {
                            currentStep++;
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

			/**/
		}
	}
}