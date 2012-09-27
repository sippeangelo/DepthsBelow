using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	public class Component
	{
		public Entity Parent;

		public Component(Entity parent)
		{
			Parent = parent;
		}

		public virtual void Update(GameTime gameTime)
		{
			
		}
	}
}
