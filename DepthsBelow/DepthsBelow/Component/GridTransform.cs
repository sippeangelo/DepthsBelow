using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	public class GridTransform : Component
	{
		public Point Position;
		public int X
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
		public int Y
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

		public GridTransform(Entity parent) : base(parent)
		{
			Position = Point.Zero;
		}

		public Vector2 ToWorld()
		{
			return Grid.GridToWorld(Position);
		}
	}
}
