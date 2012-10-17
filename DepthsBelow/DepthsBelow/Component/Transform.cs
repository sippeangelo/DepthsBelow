using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	/// <summary>
	/// Transform component. 
	/// Handles both grid and world positions and rotations, while keeping them in sync.
	/// </summary>
	public class Transform : Component
	{
		/// <summary>
		/// Represents a grid position.
		/// </summary>
		public class GridTransform
		{
			private Point position;
			/// <summary>
			/// Sets the grid position and updates the world position of WorldTransform.
			/// </summary>
			public Point Position
			{
				get { return position; }
				set
				{
					position = value;
					masterTransform.World.SetWithoutRelation((Vector2)this);
				}
			}

			/// <summary>
			/// Shorthand for the grid x-position.
			/// </summary>
			public int X
			{
				get { return position.X; }
				set
				{
					position.X = value;
					masterTransform.World.SetWithoutRelation((Vector2)this);
				}
			}

			/// <summary>
			/// Shorthand for the grid y-position.
			/// </summary>
			public int Y
			{
				get { return position.Y; }
				set
				{
					position.Y = value;
					masterTransform.World.SetWithoutRelation((Vector2)this);
				}
			}

			/// <summary>
			/// Implicit conversion between GridTransform and a Point grid position.
			/// </summary>
			public static implicit operator Point(GridTransform gridTransform)
			{
				return gridTransform.position;
			}

			/// <summary>
			/// Explicit conversion between GridTransform and a Vector2 world position.
			/// </summary>
			public static explicit operator Vector2(GridTransform gridTransform)
			{
				var worldPosition = DepthsBelow.Grid.GridToWorld(gridTransform.Position);
				return new Vector2(worldPosition.X, worldPosition.Y);
			}

			private readonly Transform masterTransform;
			/// <summary>
			/// Create a GridTransform within a master Transform component.
			/// </summary>
			/// <param name="masterTransform">The master Transform component.</param>
			public GridTransform(Transform masterTransform)
			{
				this.masterTransform = masterTransform;
			}

			/// <summary>
			/// Sets the position data without updating the associated world position.
			/// </summary>
			/// <param name="position">The grid position.</param>
			public void SetWithoutRelation(Point position)
			{
				this.position = position;
			}
		}

		public class WorldTransform
		{
			private Vector2 position;
			/// <summary>
			/// Sets the world position and updates the grid position of GridTransform.
			/// </summary>
			public Vector2 Position
			{
				get { return position; }
				set
				{
					position = value;
					masterTransform.Grid.SetWithoutRelation((Point)this);
				}
			}

			/// <summary>
			/// Shorthand for the world x-position.
			/// </summary>
			public float X
			{
				get { return position.X; }
				set
				{
					position.X = value;
					masterTransform.Grid.SetWithoutRelation((Point)this);
				}
			}

			/// <summary>
			/// Shorthand for the world y-position.
			/// </summary>
			public float Y
			{
				get { return position.Y; }
				set
				{
					position.Y = value;
					masterTransform.Grid.SetWithoutRelation((Point)this);
				}
			}

			/// <summary>
			/// Rotation in radians.
			/// </summary>
			public float Rotation;

			/// <summary>
			/// Origin to rotate around.
			/// </summary>
			public Vector2 Origin;

			/// <summary>
			/// Implicit conversion between WorldTransform and a Vector2 world position.
			/// </summary>
			public static implicit operator Vector2(WorldTransform worldTransform)
			{
				return worldTransform.Position;
			}

			/// <summary>
			/// Explicit conversion between WorldTransform and a Point grid position.
			/// </summary>
			public static explicit operator Point(WorldTransform worldTransform)
			{
				var gridPosition = DepthsBelow.Grid.WorldToGrid(worldTransform.Position);
				return new Point(gridPosition.X, gridPosition.Y);
			}

			private readonly Transform masterTransform;
			/// <summary>
			/// Create a WorldTransform within a master Transform component.
			/// </summary>
			/// <param name="masterTransform">The master Transform component.</param>
			public WorldTransform(Transform masterTransform)
			{
				this.masterTransform = masterTransform;
			}

			/// <summary>
			/// Sets the position data without updating the associated grid position.
			/// </summary>
			/// <param name="position">The world position.</param>
			public void SetWithoutRelation(Vector2 position)
			{
				this.position = position;
			}
		}

		/// <summary>
		/// The grid position.
		/// </summary>
		public GridTransform Grid;
		/// <summary>
		/// The world position.
		/// </summary>
		public WorldTransform World;
		
		public Transform(Entity parent)
			: base(parent)
		{
			Grid = new GridTransform(this);
			World = new WorldTransform(this);
		}

		/// <summary>
		/// Implicit conversion between Transform and a Point grid position.
		/// </summary>
		public static implicit operator Point(Transform transform)
		{
			return transform.Grid;
		}

		/// <summary>
		/// Implicit conversion between Transform and a Vector2 world position.
		/// </summary>
		public static implicit operator Vector2(Transform transform)
		{
			return transform.World;
		}
	}
}
