using System;
using System.Collections.Generic;
using DepthsBelow.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
    public class SmallEnemy : Entity
    {
        public static Texture2D Texture;
        public Color Color;
        public static Point Origin;
        private bool _selected; //I guess as in "currently doing something"

        public List<SmallEnemy> Swarm;

        private PathFinder.Node nextNode;

        public int numberOfSteps = 5;
        public int currentStep = 0;

        public bool AttackedThisTurn = false;

        public int step
        {
            get { return currentStep; }
            set
            {
                currentStep = value;
            }
        }

        public SmallEnemy(EntityManager entityManager, ref List<SmallEnemy> swarm)
			: base(entityManager)
        {
            if (Texture == null)
                LoadContent();

            this.Swarm = swarm;

            Transform.World.Origin = new Vector2(20, 16);

            this.Color = Color.White;
            var rc = new SpriteRenderer(this) { Texture = Texture, Color = Color.White };
            AddComponent(rc);

            var cc = new Collision(this, 32, 32);
            AddComponent(cc);

            var pfc = new PathFinder(this);
            AddComponent(pfc);

	        var stat = new Component.Stat(this)
		                   {
			                   Life = 50, 
							   Defence = 25, 
							   Strength = 26, 
							   GetAim = 100, 
							   GetDodge = 50
		                   };
	        AddComponent(stat);
        }
		public override void Remove()
		{
			Swarm.Remove(this);

			base.Remove();
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
			Texture = GameServices.GetService<ContentManager>().Load<Texture2D>("images/Monster");
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

				// Collision with moving units
                foreach (var body in Swarm)
                {
                    if (body != this)
                    {
                        if (body.Transform.Grid == nextNode.Position)
                        {
                            soldierCollisions.Add(body.Transform.Grid);
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
                    {
                        Transform.World.X += speed;
                        GetComponent<Component.Transform>().World.Rotation = MathHelper.ToRadians(0);
                    }
                    if (Transform.World.X > nodeWorldPos.X)
                    {
                        Transform.World.X -= speed;
                        GetComponent<Component.Transform>().World.Rotation = MathHelper.ToRadians(180);
                    }
                    if (Transform.World.Y < nodeWorldPos.Y)
                    {
                        Transform.World.Y += speed;
                        GetComponent<Component.Transform>().World.Rotation = MathHelper.ToRadians(90);
                    }
                    if (Transform.World.Y > nodeWorldPos.Y)
                    {
                        Transform.World.Y -= speed;
                        GetComponent<Component.Transform>().World.Rotation = MathHelper.ToRadians(270);
                    }
                    if (Transform.World == nodeWorldPos)
                    {
                        if (currentStep < numberOfSteps)
                        {
                            currentStep++;
                            //lastLastNode = lastNode;
                            //lastNode = nextNode;
                            nextNode = GetComponent<PathFinder>().Next();
                        }
                        else
                        {
                            
                            GetComponent<PathFinder>().Stop();
                        }

                        foreach (var entity in entityManager.Entities)
                        {
                            if (entity is Soldier)
                            {
                                if (entity.GetComponent<Component.Collision>().Rectangle.Intersects(this.GetComponent<Component.Collision>().Rectangle))
                                {
                                    if (AttackedThisTurn == false)
                                    {
                                        if (Utility.AttackTest(this.GetComponent<Component.Stat>(), entity, Utility.CalculateHitChance(this.GetComponent<Component.Stat>(), this.Transform.World.Position, entity)))
                                        {
                                            AttackedThisTurn = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}