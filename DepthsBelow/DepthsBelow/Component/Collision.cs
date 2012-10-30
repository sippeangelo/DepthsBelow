using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow.Component
{
	/// <summary>
	/// Component for handling collisions.
	/// Contains a rectangle which defines its hitbox.
	/// </summary>
    public class Collision : Component
    {
		/// <summary>
		/// The collision rectangle.
		/// </summary>
	    public Rectangle Rectangle;

        /// <summary>
        /// Whether the entity is solid to other entities.
        /// </summary>
        public bool Solid = false;

		/// <summary>
		/// Width of the collision rectangle.
		/// </summary>
		public int Width
		{
			get { return this.Rectangle.Width; }
			set { this.Rectangle.Width = value; }
		}
		/// <summary>
		/// Height of the collision rectangle.
		/// </summary>
	    public int Height
		{
			get { return this.Rectangle.Height; }
			set { this.Rectangle.Height = value; }
		}
        
		/// <summary>
		/// Create a collision component.
		/// </summary>
		/// <param name="parent">Parent entity.</param>
		/// <param name="width">Width of the collision rectangle.</param>
		/// <param name="height">Height of the collision rectangle.</param>
        public Collision(Entity parent, int width, int height)
            : base(parent)
        {
		    this.Rectangle = new Rectangle(0, 0, width, height);
        }

		/// <summary>
		/// Updates the position of the collision rectangle to the position of its parent entity.
		/// </summary>
		/// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
			var transform = this.Parent.GetComponent<Transform>();
			this.Rectangle.X = (int)transform.World.X;
	        this.Rectangle.Y = (int)transform.World.Y;
        }
    }
}
