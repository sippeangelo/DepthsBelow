using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow.Component
{
    public class Collision : Component
    {

	    public Rectangle Rectangle;

	    public int Width;
	    public int Height;
        
        public Collision(Entity parent, int width, int height)
            : base(parent)
        {
        	this.Width = width;
        	this.Height = height;
		    this.Rectangle = new Rectangle(0, 0, width, height);
        }

        public override void Update(GameTime gameTime)
        {
			Transform transform = this.Parent.GetComponent<Transform>();
			this.Rectangle.X = (int)transform.World.X;
	        this.Rectangle.Y = (int)transform.World.Y;
        }
    }
}
