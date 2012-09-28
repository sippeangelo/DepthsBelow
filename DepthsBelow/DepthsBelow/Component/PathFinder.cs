using System;
using System.Collections.Generic;
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
			var _start = new System.Drawing.Point(start.X, start.Y);
			var _goal = new System.Drawing.Point(goal.X, goal.Y);
			path = Core.PathFinder.FindPath(_start, _goal);
			if (path == null)
				Console.WriteLine("Path not found!");
		}
	}
}
