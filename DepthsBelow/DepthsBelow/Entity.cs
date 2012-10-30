using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
	/// <summary>
	/// Game object.
	/// </summary>
	public class Entity : IDisposable
	{
		List<Component.Component> Components;

		/// <summary>
		/// Shorthand for transform component.
		/// </summary>
		public Component.Transform Transform;

		/// <summary>
		/// Shorthand to the grid transform position.
		/// </summary>
		public Point Position
		{
			get { return this.Transform.Grid.Position; }
			set { this.Transform.Grid.Position = value; }
		}
		/// <summary>
		/// Shorthand to the grid transform x-position.
		/// </summary>
		public int X
		{
			get { return this.Transform.Grid.X; }
			set { this.Transform.Grid.X = value; }
		}
		/// <summary>
		/// Shorthand to the grid transform y-position.
		/// </summary>
		public int Y
		{
			get { return this.Transform.Grid.Y; }
			set { this.Transform.Grid.Y = value; }
		}

		/// <summary>
		/// Storage for property data
		/// </summary>
		public Dictionary<string, object> Properties = new Dictionary<string, object>();
		/// <summary>
		/// Shorthand for frame properties.
		/// </summary>
		public object this[string key]
		{
			get { return Properties[key]; }
			set { Properties[key] = value; }
		}

		/// <summary>
		/// Creates a game object and adds it to the referenced entity manager.
		/// </summary>
        /// 
        public Entity()
		{
			EntityManager.Add(this);
			
			Components = new List<Component.Component>();

			Transform = new Component.Transform(this);
			AddComponent(Transform);
		}

		/// <summary>
		/// Implementation of IDisposable.
		/// Dispose of any unmanaged resources.
		/// </summary>
		public virtual void Dispose()
		{
			
		}

		/// <summary>
		/// Removes the entity from the game world and disposes 
		/// potential unmanaged resources.
		/// </summary>
		public virtual void Remove()
		{
			EntityManager.Remove(this);
			Dispose();
		}

		/// <summary>
		/// Get the reference to a component contained in the entity.
		/// </summary>
		/// <typeparam name="T">The component type.</typeparam>
		/// <returns>Returns the component of requested type.</returns>
		public T GetComponent<T>() where T : Component.Component
		{
			foreach (Component.Component c in Components)
			{
				if (c is T)
					return (T)c;
			}

			return null;
		}

		/// <summary>
		/// Add a component to the entity.
		/// </summary>
		/// <param name="c">the component to add</param>
		public void AddComponent(Component.Component c)
		{
			Components.Add(c);
		}

		/// <summary>
		/// Update the entity and all child components.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
			// Update components
			foreach (Component.Component c in Components)
				c.Update(gameTime);
		}
	}
}
