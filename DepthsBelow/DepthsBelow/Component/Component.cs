using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DepthsBelow.Component
{
	public class Component
	{
		public Entity Parent;

		public Component(Entity parent)
		{
			Parent = parent;
		}
	}
}
