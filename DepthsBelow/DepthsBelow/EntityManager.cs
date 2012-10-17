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
		private List<Entity> entities;

		public EntityManager()
		{
			entities = new List<Entity>();
		}

		/// <summary>
		/// Implementation of IDisposable.
		/// Dispose of the entity manager, and all entities it contains.
		/// </summary>
		public void Dispose()
		{
			foreach (var entity in entities)
				entity.Dispose();

			entities = null;
		}

		/// <summary>
		/// Add an entity to the entity manager.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void Add(Entity entity)
		{
			entities.Add(entity);
		}

		/// <summary>
		/// Remove an entity from the entity manager.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		public void Remove(Entity entity)
		{
			entities.Remove(entity);
		}

		/// <summary>
		/// Reset the entity manager, disposing any entities it contains.
		/// </summary>
		public void Reset()
		{
			foreach (var entity in entities)
				entity.Dispose();

			entities.Clear();
		}

		/// <summary>
		/// Update all entities handled by the entity manager.
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			foreach (var entity in entities)
				entity.Update(gameTime);
		}

		/// <summary>
		/// Draw all entities handled by the entity manager.
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var entity in entities)
				entity.GetComponent<Component.SpriteRenderer>().Draw(spriteBatch);
		}
	}
}
