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

		private Point goal;
		public Point Goal
		{
			get { return goal; }
			set 
			{
				this.goal = value;
				this.Start = this.Parent.GetComponent<Transform>().Grid.Position;
				this.FindPath(this.Start, value);
			}
		}
		
		private AStar.PathFinderFast pathFinder;
		private List<AStar.PathFinderNode> path;

		public PathFinder(Entity parent)
			: base(parent)
		{
			pathFinder = new AStar.PathFinderFast(Core.Map.GetCollisionMap());
			pathFinder.Formula = AStar.HeuristicFormula.Manhattan;
			pathFinder.Diagonals = false;
			pathFinder.HeavyDiagonals = false;
			pathFinder.HeuristicEstimate = 2;
			pathFinder.PunishChangeDirection = true;
			pathFinder.TieBreaker = false;
			pathFinder.SearchLimit = 50000;
			pathFinder.DebugProgress = false;
			pathFinder.DebugFoundPath = false;
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

		public List<AStar.PathFinderNode> FindPath(Point start, Point goal)
		{
			var _start = new System.Drawing.Point(start.X, start.Y);
			var _goal = new System.Drawing.Point(goal.X, goal.Y);
			//path = Core.PathFinder.FindPath(_start, _goal);
			path = pathFinder.FindPath(_start, _goal);
			if (path == null)
				Debug.WriteLine(this.Parent.ToString() + ": Path not found!");

			return path;
		}
	}
}
