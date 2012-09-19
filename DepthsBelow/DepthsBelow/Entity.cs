using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DepthsBelow
{
	public class Entity
	{
		List<Component> Components;

		public Entity()
		{
			Components = new List<Component>();
		}

		public T GetComponent<T>() where T : Component
		{
			foreach (Component c in Components)
			{
				if (c is T)
					return (T)c;
			}

			return null;
		}

		public void AddComponent(Component c)
		{
			Components.Add(c);
		}
	}
}
