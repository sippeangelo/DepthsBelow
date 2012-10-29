using System;
using System.Collections.Generic;
using DepthsBelow.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
    public class Shot : Entity
    {
        public Texture2D Texture;
        public Color Color;
        public static Point Origin;
        string type = "";

        Stat soldierStat;

        public Stat Stat
        {
            get { return soldierStat; }
            set { soldierStat = value; }
        }

        private bool _selected = false; //I guess as in "currently doing something"

        public bool select
        {
            get { return _selected; }
            set
            {
                _selected = value;
            }
        }

        public List<Shot> Volley;

        public Vector2 direction = Vector2.Zero;

        public Shot(EntityManager entityManager, Stat _soldierStat, string _type)
			: base(entityManager)
        {
            type = _type;

            if (Texture == null)
                LoadContent();

            Transform.World.Origin = new Vector2(16, 16);

            Stat = _soldierStat;

            this.Color = Color.White;
            var rc = new SpriteRenderer(this) { Texture = Texture, Color = Color.White };
            AddComponent(rc);

            var cc = new Collision(this, 32, 32);
            AddComponent(cc);

        }
        public void LoadContent()
        {
            if (type == "Rocket")
            {
                Texture = GameServices.GetService<ContentManager>().Load<Texture2D>("images/Shot");
            }
            else
            {
                Texture = GameServices.GetService<ContentManager>().Load<Texture2D>("images/Shot3");
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float elapsed = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            List<Point> soldierCollisions = new List<Point>();

            // HACK: Make this work properly with other speeds...
            float speed = 4f;
            Transform.World.Position += speed * direction;

            foreach (var entity in entityManager.Entities)
            {
                if (entity is SmallEnemy) 
                {
                    if (this.GetComponent<Component.Collision>().Rectangle.Intersects(entity.GetComponent<Component.Collision>().Rectangle))
                    {
                        if (Utility.AttackTest(Stat, entity, Utility.CalculateHitChance(Stat, this.Stat.Remember, entity)))
                        {
                            this.Remove();
                            return;
                        }
                    }
                }
            }
        }
    }
}