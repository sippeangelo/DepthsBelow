using System;
using System.Collections.Generic;
using DepthsBelow.Component;
using Microsoft.Xna.Framework;
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

        public SmallEnemy(Core core, ref List<SmallEnemy> swarm)
            : base(core)
        {
            if (Texture == null)
                LoadContent(core);

            this.Swarm = swarm;

            Transform.World.Origin = new Vector2(20, 16);

            this.Color = Color.White;
            var rc = new SpriteRenderer(this) { Texture = Texture, Color = Color.White };
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
            Texture = core.Content.Load<Texture2D>("images/Monster");
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
                        Transform.World.X += speed;
                    if (Transform.World.X > nodeWorldPos.X)
                        Transform.World.X -= speed;
                    if (Transform.World.Y < nodeWorldPos.Y)
                        Transform.World.Y += speed;
                    if (Transform.World.Y > nodeWorldPos.Y)
                        Transform.World.Y -= speed;

                    if (Transform.World == nodeWorldPos)
                        nextNode = GetComponent<PathFinder>().Next();
                }

            }
        }
    }
}