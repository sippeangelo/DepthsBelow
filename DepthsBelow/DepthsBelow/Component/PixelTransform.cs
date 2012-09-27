using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	public class PixelTransform : Component
	{
		public Vector2 Position;
		public float X
		{
			get
			{
				return Position.X;
			}
			set
			{
				Position.X = value;
			}
		}
		public float Y
		{
			get
			{
				return Position.Y;
			}
			set
			{
				Position.Y = value;
			}
		}
		public float Rotation;
		public Vector2 Origin;

		public PixelTransform(Entity parent)
			: base(parent)
		{
			Position = Vector2.Zero;
			Rotation = 0.0f;
			Origin = Vector2.Zero;
		}
	}
}
