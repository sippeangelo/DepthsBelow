using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AStar;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	public class PathFinder : Component
	{
		public class Node
		{
			public Point Position;
			public int F;
			public int G;

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

		public Point Start { get; private set; }

		public byte[,] CollisionMap;

		private Point goal;
		public Point Goal
		{
			get { return goal; }
			set 
			{
				this.goal = value;
				//this.Start = this.Parent.GetComponent<Transform>().Grid.Position;
				this.FindPath(this.Parent.GetComponent<Transform>().Grid.Position, value, null);
			}
		}

		public bool IsMoving
		{
			get
			{
				return path != null && path.Count() != 0;
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

		public void Stop()
		{
			path = null;
		}

		private List<AStar.PathFinderNode> FindPath(Point start, Point goal, List<Point> appendCollisionMap)
		{
			byte[,] mapCollisionMap = Core.Map.GetCollisionMap();

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

			this.CollisionMap = new byte[100, 100];

			return path;
		}

		public void RecreatePath(List<Point> appendCollisionMap)
		{
			FindPath(this.Parent.GetComponent<Transform>().Grid.Position, this.Goal, appendCollisionMap);
		}
	}
}
