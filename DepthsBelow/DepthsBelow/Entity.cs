using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
	public class Entity : IDisposable
	{
		List<Component.Component> Components;
		public Component.Transform Transform;
        
		public int X
		{
			get { return this.Transform.Grid.X; }
			set 
			{
				this.Transform.Grid.X = value;
			}
		}
		public int Y
		{
			get { return this.Transform.Grid.Y; }
			set
			{
				this.Transform.Grid.Y = value;
			}
		}

		private EntityManager entityManager;

		public Entity(EntityManager entityManager)
		{
			this.entityManager = entityManager;
			entityManager.Add(this);
			
			Components = new List<Component.Component>();

			Transform = new Component.Transform(this);
			AddComponent(Transform);
		}

		public virtual void Dispose()
		{
			
		}

		public virtual void Remove()
		{
			entityManager.Remove(this);
			Dispose();
		}

		public virtual void Kill()
		{
			Remove();
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
