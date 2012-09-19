using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
	class TransformComponent : Component
	{
		public Vector2 Position;
		public float Rotation;
		public Point Origin;

		public TransformComponent(Entity parent) : base(parent)
		{
			Position = Vector2.Zero;
			Rotation = 0.0f;
			Origin = Point.Zero;
		}
	}
}
