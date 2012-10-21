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
        public static Texture2D Texture;
        public Color Color;
        public static Point Origin;

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

        public Shot(EntityManager entityManager, Stat _soldierStat)
			: base(entityManager)
        {
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
        public static void LoadContent()
        {
			Texture = GameServices.GetService<ContentManager>().Load<Texture2D>("images/Shot");
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float elapsed = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            List<Point> soldierCollisions = new List<Point>();

            // HACK: Make this work properly with other speeds...
            float speed = 4f;
            Transform.World.Position += speed * direction;
        }
    }
}