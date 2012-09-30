using System;
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

		private PathFinder.Node nextNode;

		public Soldier(Core core) : base(core)
		{
			if (Texture == null)
				LoadContent(core);

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

			if (nextNode == null)
				nextNode = GetComponent<PathFinder>().Next();

			if (nextNode != null)
			{
				var nodeWorldPos = Grid.GridToWorld(nextNode.Position);
				// HACK: Make this work properly with other speeds...
				int speed = 2*4;
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

			/**/
		}
	}
}