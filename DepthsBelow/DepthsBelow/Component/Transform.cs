using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	public class Transform : Component
	{
		public class GridTransform
		{
			private Point position;
			public Point Position
			{
				get { return position; }
				set
				{
					position = value;
					masterTransform.World.SetWithoutRelation((Vector2)this);
				}
			}

			public int X
			{
				get { return position.X; }
				set
				{
					position.X = value;
					masterTransform.World.SetWithoutRelation((Vector2)this);
				}
			}
			public int Y
			{
				get { return position.Y; }
				set
				{
					position.Y = value;
					masterTransform.World.SetWithoutRelation((Vector2)this);
				}
			}

			public static implicit operator Point(GridTransform gridTransform)
			{
				return gridTransform.position;
			}

			public static explicit operator Vector2(GridTransform gridTransform)
			{
				var worldPosition = DepthsBelow.Grid.GridToWorld(gridTransform.Position);
				return new Vector2(worldPosition.X, worldPosition.Y);
			}

			private readonly Transform masterTransform;
			public GridTransform(Transform masterTransform)
			{
				this.masterTransform = masterTransform;
			}

			public void SetWithoutRelation(Point position)
			{
				this.position = position;
			}
		}

		public class WorldTransform
		{
			private Vector2 position;
			public Vector2 Position
			{
				get { return position; }
				set
				{
					position = value;
					masterTransform.Grid.SetWithoutRelation((Point)this);
				}
			}

			public float X
			{
				get { return position.X; }
				set
				{
					position.X = value;
					masterTransform.Grid.SetWithoutRelation((Point)this);
				}
			}
			public float Y
			{
				get { return position.Y; }
				set
				{
					position.Y = value;
					masterTransform.Grid.SetWithoutRelation((Point)this);
				}
			}

			public float Rotation;
			public Vector2 Origin;

			public static implicit operator Vector2(WorldTransform worldTransform)
			{
				return worldTransform.Position;
			}

			public static explicit operator Point(WorldTransform worldTransform)
			{
				var gridPosition = DepthsBelow.Grid.WorldToGrid(worldTransform.Position);
				return new Point(gridPosition.X, gridPosition.Y);
			}

			private readonly Transform masterTransform;
			public WorldTransform(Transform masterTransform)
			{
				this.masterTransform = masterTransform;
			}

			public void SetWithoutRelation(Vector2 position)
			{
				this.position = position;
			}
		}

		public GridTransform Grid;
		public WorldTransform World;
		
		public Transform(Entity parent)
			: base(parent)
		{
			Grid = new GridTransform(this);
			World = new WorldTransform(this);
		}

		public static implicit operator Point(Transform transform)
		{
			return transform.Grid;
		}

		public static implicit operator Vector2(Transform transform)
		{
			return transform.World;
		}
	}
}
