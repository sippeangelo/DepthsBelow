using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AStar;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	/// <summary>
	/// Component that handles pathfinding.
	/// </summary>
	public class PathFinder : Component
	{
		/// <summary>
		/// A pathfinder node.
		/// </summary>
		public class Node
		{
			/// <summary>
			/// Grid position of the node.
			/// </summary>
			public Point Position;
			/// <summary>
			/// A* F-score
			/// </summary>
			public int F;
			/// <summary>
			/// A* G-score
			/// </summary>
			public int G;

			/// <summary>
			/// Explicit conversion between A* library node and Node object.
			/// </summary>
			/// <param name="aStarNode"></param>
			/// <returns></returns>
			public static explicit operator Node(AStar.PathFinderNode aStarNode)
			{
				return new Node
							{
								Position = new Point(aStarNode.X, aStarNode.Y),
								F = aStarNode.F,
								G = aStarNode.G
							};
			}
		}

		/// <summary>
		/// Start position of the path.
		/// </summary>
		public Point Start { get; private set; }

		/// <summary>
		/// A collision bytemap.
		/// 0 = unwalkable node
		/// 1 = walkable node
		/// </summary>
		public byte[,] CollisionMap;

		private Point goal;
		/// <summary>
		/// Sets a goal point and finds a path to it.
		/// After a path is created, call Next() go get the next node in the path.
		/// </summary>
		public Point Goal
		{
			get { return goal; }
			set 
			{
				this.goal = value;
				this.FindPath(this.Parent.GetComponent<Transform>().Grid.Position, value, null);
			}
		}

		/// <summary>
		/// Is a path in progress?
		/// </summary>
		public bool IsMoving
		{
			get
			{
				return path != null && path.Count() != 0;
			}
		}

        public int Length
        {
            get
            {
                return (path != null) ? path.Count : 0;
            }
        }
		
		private AStar.PathFinderFast pathFinder;
		private List<AStar.PathFinderNode> path;

		public PathFinder(Entity parent)
			: base(parent)
		{
			
		}

		public override void Update(GameTime gameTime)
		{
			/*if (path != null && path.Count != 0)
			{
				var nextNode = path.Last();
				var nodePos = new Point(nextNode.X, nextNode.Y);
				Parent.Transform.Grid.Position = nodePos;
				if (Parent.Transform.World == (Vector2)Parent.Transform.Grid)
					path.Remove(nextNode);
			}*/

			base.Update(gameTime);
		}

		/// <summary>
		/// Gets the next node of the current path.
		/// </summary>
		/// <returns>The next node in the path.</returns>
		public Node Next()
		{
			if (path == null)
				return null;

			if (path.Count == 0)
				return null;

			var nextNode = path.Last();
			path.Remove(nextNode);
			return (Node)nextNode;
		}

		/// <summary>
		/// Cancels the current path in memory.
		/// </summary>
		public void Stop()
		{
			path = null;
		}

		/// <summary>
		/// Finds a path between two points.
		/// </summary>
		/// <param name="start">The start point.</param>
		/// <param name="goal">The goal point.</param>
		/// <param name="appendCollisionMap">A list of unwalkable points that will be joined with the original collision map.</param>
		/// <returns></returns>
		private List<AStar.PathFinderNode> FindPath(Point start, Point goal, List<Point> appendCollisionMap)
		{
			byte[,] mapCollisionMap = Core.Map.GetCollisionMap();

            // Add solid collision component entities
            foreach (var collision in EntityManager.GetComponents<Collision>())
            {
                if (collision.Solid && collision.Parent != this.Parent)
                {
					var pos = Grid.WorldToGrid(new Vector2(collision.Rectangle.X, collision.Rectangle.Y));

					for (int x = 0; x < Math.Ceiling(collision.Rectangle.Width / (double)Grid.TileSize); x++)
						for (int y = 0; y < Math.Ceiling(collision.Rectangle.Height / (double)Grid.TileSize); y++)
						{
							try
							{
								mapCollisionMap[pos.X + x, pos.Y + y] = 0;
							}
							catch
							{
							}
						}
                }
            }

			if (appendCollisionMap != null)
				foreach (var point in appendCollisionMap)
					mapCollisionMap[point.X, point.Y] = 0;

			pathFinder = new AStar.PathFinderFast(mapCollisionMap);
			pathFinder.Formula = AStar.HeuristicFormula.Manhattan;
			pathFinder.Diagonals = false;
			pathFinder.HeavyDiagonals = false;
			pathFinder.HeuristicEstimate = 2;
			pathFinder.PunishChangeDirection = false;
			pathFinder.TieBreaker = false;
			pathFinder.SearchLimit = 50000;
			pathFinder.DebugProgress = false;
			pathFinder.DebugFoundPath = false;

			var _start = new System.Drawing.Point(start.X, start.Y);
			var _goal = new System.Drawing.Point(goal.X, goal.Y);
			//path = Core.PathFinder.FindPath(_start, _goal);
			path = pathFinder.FindPath(_start, _goal);
			if (path == null)
				Debug.WriteLine(this.Parent.ToString() + ": Path not found!");

			//this.CollisionMap = new byte[100, 100];

			return path;
		}

		/// <summary>
		/// Recreate the current path in memory with a list of unwalkable points that will be joined with the original collision map.
		/// </summary>
		/// <param name="appendCollisionMap">A list of unwalkable points that will be joined with the collision map.</param>
		public void RecreatePath(List<Point> appendCollisionMap)
		{
			FindPath(this.Parent.GetComponent<Transform>().Grid.Position, this.Goal, appendCollisionMap);
		}
	}
}
