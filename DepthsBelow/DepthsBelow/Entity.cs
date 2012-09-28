using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
	public class Entity
	{
		List<Component.Component> Components;
		public Component.PixelTransform pixelTransform;
		public Component.GridTransform gridTransform;
		public int X
		{
			get { return this.gridTransform.X; }
			set 
			{ 
				this.gridTransform.X = value;
				this.pixelTransform.X = this.gridTransform.ToWorld().X;
			}
		}
		public int Y
		{
			get { return this.gridTransform.Y; }
			set
			{
				this.gridTransform.Y = value;
				this.pixelTransform.Y = this.gridTransform.ToWorld().Y;
			}
		}

		private Core core;

		public Entity(Core core)
		{
			this.core = core;

			Components = new List<Component.Component>();
			pixelTransform = new Component.PixelTransform(this);
			AddComponent(pixelTransform);
			gridTransform = new Component.GridTransform(this);
			AddComponent(gridTransform);
		}

		public T GetComponent<T>() where T : Component.Component
		{
			foreach (Component.Component c in Components)
			{
				if (c is T)
					return (T)c;
			}

			return null;
		}

		public void AddComponent(Component.Component c)
		{
			Components.Add(c);
		}

		public virtual void Update(GameTime gameTime)
		{
			// Update components
			foreach (Component.Component c in Components)
				c.Update(gameTime);
		}
	}
}
