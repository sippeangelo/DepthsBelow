using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DepthsBelow
{
	public class Entity
	{
		List<Component.Component> Components;
		public Component.PixelTransform pixelTransform;
		public Component.GridTransform gridTransform;
        public Component.Collision collision;

		public Entity()
		{
			Components = new List<Component.Component>();
			pixelTransform = new Component.PixelTransform(this);
			AddComponent(pixelTransform);
			gridTransform = new Component.GridTransform(this);
			AddComponent(gridTransform);
            collision = new Component.Collision(this, 32, 32);
            AddComponent(collision);
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
	}
}
