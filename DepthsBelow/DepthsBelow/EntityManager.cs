using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	/// <summary>
	/// Manages a group of entities.
	/// </summary>
	public class EntityManager : IDisposable
	{
		public List<Entity> Entities;

        public List<Entity> Entities
        {
            get { return entities; }
        }

		public EntityManager()
		{
			Entities = new List<Entity>();
		}

		/// <summary>
		/// Implementation of IDisposable.
		/// Dispose of the entity manager, and all entities it contains.
		/// </summary>
		public void Dispose()
		{
			foreach (var entity in Entities)
				entity.Dispose();

			Entities = null;
		}

		/// <summary>
		/// Add an entity to the entity manager.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void Add(Entity entity)
		{
			Entities.Add(entity);
		}

		/// <summary>
		/// Remove an entity from the entity manager.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		public void Remove(Entity entity)
		{
			Entities.Remove(entity);
		}

		/// <summary>
		/// Reset the entity manager, disposing any entities it contains.
		/// </summary>
		public void Reset()
		{
			foreach (var entity in Entities)
				entity.Dispose();

			Entities.Clear();
		}

		/// <summary>
		/// Get the reference to a component contained in the entity.
		/// </summary>
		/// <typeparam name="T">The component type.</typeparam>
		/// <returns>Returns the component of requested type.</returns>
		public List<T> GetComponents<T>() where T : Component.Component
		{
			List<T> components = new List<T>();

			foreach (var entity in Entities)
			{
				var component = entity.GetComponent<T>();
				if (component != null)
					components.Add(component);
			}

			return components;
		}

		/// <summary>
		/// Update all entities handled by the entity manager.
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			foreach (var entity in Entities)
				entity.Update(gameTime);
		}

		/// <summary>
		/// Draw all entities handled by the entity manager.
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var entity in Entities)
			{
				entity.GetComponent<Component.SpriteRenderer>().Draw(spriteBatch);
			}
		}
	}
}
