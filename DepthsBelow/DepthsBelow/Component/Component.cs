using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	/// <summary>
	/// Base component.
	/// </summary>
	public class Component
	{
		/// <summary>
		/// Parent entity of which the component belongs.
		/// </summary>
		public Entity Parent;

		public Component(Entity parent)
		{
			Parent = parent;
		}

		/// <summary>
		/// Virtual component update function.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
			
		}
	}
}
