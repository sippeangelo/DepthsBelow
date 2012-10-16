using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow.GUI
{
	public class Frame
	{
		public int Width
		{
			get { return this.Rectangle.Width; }
			set { this.Rectangle.Width = value; }
		}
		public int Height
		{
			get { return this.Rectangle.Height; }
			set { this.Rectangle.Height = value; }
		}
		public int X
		{
			get { return this.Rectangle.X; }
			set { this.Rectangle.X = value; }
		}
		public int Y
		{
			get { return this.Rectangle.Y; }
			set { this.Rectangle.Y = value; }
		}

		public Rectangle Rectangle;
		public Rectangle AbsoluteRectangle
		{
			get
			{
				if (Parent != null)
					return new Rectangle(Parent.Rectangle.X + Rectangle.X, Parent.Rectangle.Y + Rectangle.Y, Rectangle.Width, Rectangle.Height);
				else
					return Rectangle;
			}
		}
		public Texture2D Texture;
		public Color Color;
		public bool Visible = true;

		public Frame Parent;
		public List<Frame> Children; 

		public Frame()
		{
			this.Rectangle = Rectangle.Empty;
			this.Texture = null;
			this.Color = Color.White;

			this.Children = new List<Frame>();

			GUIManager.Add(this);
		}

		public Frame(Frame parentFrame)
			: this()
		{
			this.Parent = parentFrame;
		}

		public void Add(Frame childFrame)
		{
			childFrame.Parent = this;
			if (!Children.Contains(childFrame))
				Children.Add(childFrame);
		}

		public void Remove(Frame childFrame)
		{
			if (Children.Contains(childFrame))
				Children.Remove(childFrame);
		}

		public void Destroy()
		{
			if (Parent != null)
				Parent.Remove(this);

			GUIManager.Remove(this);
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach (var child in Children)
				child.Update(gameTime);
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
			{	
				if (Texture != null)
					spriteBatch.Draw(Texture, AbsoluteRectangle, Color);

				foreach (var child in Children)
					child.Draw(spriteBatch);
			}
		}
	}
}
