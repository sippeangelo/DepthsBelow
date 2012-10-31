using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DepthsBelow.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	public class Door : Entity
	{
		public Texture2D Texture;

		public static Texture2D TextureOpen;
		public static Texture2D TextureClosed;
		public static Texture2D TextureDestroyed;

		public bool Alive = true;
		public bool IsOpen;

		public Door(bool horizontal)
			: base()
		{
			LoadContent();

			IsOpen = false;

			if (horizontal)
				Transform.World.Rotation = -MathHelper.PiOver2;

			var rc = new SpriteRenderer(this) { Texture = Texture, Color = Color.White };
			if (horizontal)
				rc.Offset.Y += Texture.Width;
			AddComponent(rc);

			Collision cc;
			if (horizontal)
				cc = new Collision(this, Texture.Height, Texture.Width);
			else
				cc = new Collision(this, Texture.Width, Texture.Height);
			cc.Solid = true;
			AddComponent(cc);
		}

		public void LoadContent()
		{
			TextureOpen = GameServices.GetService<ContentManager>().Load<Texture2D>("images/door_open");
			TextureClosed = GameServices.GetService<ContentManager>().Load<Texture2D>("images/door_closed");
			TextureDestroyed = GameServices.GetService<ContentManager>().Load<Texture2D>("images/door_destroyed");

			Texture = TextureClosed;
		}

		public void Open()
		{
			if (Alive && !IsOpen)
			{
				IsOpen = true;
				this.GetComponent<SpriteRenderer>().Texture = TextureOpen;
				this.GetComponent<Collision>().Solid = false;
			}
		}

		public void Close()
		{
			if (Alive && IsOpen)
			{
				IsOpen = false;
				this.GetComponent<SpriteRenderer>().Texture = TextureClosed;
				this.GetComponent<Collision>().Solid = true;
			}
		}

		public void Toggle()
		{
			if (IsOpen)
				Close();
			else
				Open();
		}

		public void Kill()
		{
			Alive = false;
			this.GetComponent<SpriteRenderer>().Texture = TextureDestroyed;
			this.GetComponent<Collision>().Solid = false;
		}
	}
}
