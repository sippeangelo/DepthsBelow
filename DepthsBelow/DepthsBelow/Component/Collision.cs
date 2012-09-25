using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        public void Update(GameTime gameTime)
        {
            PixelTransform pt = this.Parent.GetComponent<PixelTransform>();
	        this.Rectangle.X = (int)pt.X;
	        this.Rectangle.Y = (int)pt.Y;
        }
    }
}
