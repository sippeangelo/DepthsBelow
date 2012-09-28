using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	class PathFinder : Component
	{
		public Point Start { get; private set; }
		public Point Goal
		{
			get { return goal; }
			set 
			{
				this.goal = value;
				this.Start = this.Parent.GetComponent<GridTransform>().Position;
				this.FindPath(this.Start, value);
			}
		}

		private Point goal;
		private List<AStar.PathFinderNode> path;

		public PathFinder(Entity parent)
			: base(parent)
		{
			
		}

		public override void Update(GameTime gameTime)
		{
			if (path != null && path.Count != 0)
			{
				var nextNode = path.Last();
				var nodePos = new Point(nextNode.X, nextNode.Y);
				Parent.gridTransform.Position = nodePos;
				if (Parent.pixelTransform.Position == Parent.gridTransform.ToWorld())
					path.Remove(nextNode);
			}

			base.Update(gameTime);
		}

		public void FindPath(Point start, Point goal)
		{
			AStar.PathFinderFast pathFinder = new AStar.PathFinderFast(Core.Map.GetCollisionMap());
			pathFinder.Formula = AStar.HeuristicFormula.Manhattan;
			pathFinder.Diagonals = false;
			pathFinder.HeavyDiagonals = false;
			pathFinder.HeuristicEstimate = 2;
			pathFinder.PunishChangeDirection = true;
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
		}
	}
}
