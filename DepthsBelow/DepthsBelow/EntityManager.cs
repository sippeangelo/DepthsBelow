using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	public class EntityManager : IDisposable
	{
		private List<Entity> entities;

		public EntityManager()
		{
			entities = new List<Entity>();
		}

		public void Dispose()
		{
			foreach (var entity in entities)
				entity.Dispose();
		}

		public void Add(Entity entity)
		{
			entities.Add(entity);
		}

		public void Remove(Entity entity)
		{
			entities.Remove(entity);
		}

		public void Reset()
		{
			foreach (var entity in entities)
				entity.Dispose();

			entities.Clear();
		}

		public void Update(GameTime gameTime)
		{
			foreach (var entity in entities)
				entity.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var entity in entities)
				entity.GetComponent<Component.SpriteRenderer>().Draw(spriteBatch);
		}
	}
}
