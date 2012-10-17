using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow.GUI
{
	/// <summary>
	/// A generic GUI frame.
	/// </summary>
	public class Frame
	{
		public string Name;
		/// <summary>
		/// Width of the frame.
		/// </summary>
		public int Width
		{
			get { return this.Rectangle.Width; }
			set { this.Rectangle.Width = value; }
		}
		/// <summary>
		/// Height of the frame.
		/// </summary>
		public int Height
		{
			get { return this.Rectangle.Height; }
			set { this.Rectangle.Height = value; }
		}
		/// <summary>
		/// X-position of the frame.
		/// </summary>
		public int X
		{
			get { return this.Rectangle.X; }
			set { this.Rectangle.X = value; }
		}
		/// <summary>
		/// Y-position of the frame.
		/// </summary>
		public int Y
		{
			get { return this.Rectangle.Y; }
			set { this.Rectangle.Y = value; }
		}

		/// <summary>
		/// Rectangle of the frame.
		/// </summary>
		public Rectangle Rectangle;
		/// <summary>
		/// Absolute rectangle of the frame. Takes parent frame positions in consideration.
		/// </summary>
		public Rectangle AbsoluteRectangle
		{
			get
			{
				if (Parent != null)
					return new Rectangle(Parent.AbsoluteRectangle.X + Rectangle.X, Parent.AbsoluteRectangle.Y + Rectangle.Y, Rectangle.Width, Rectangle.Height);
				else
					return Rectangle;
			}
		}
		/// <summary>
		/// Optional background texture of the frame.
		/// </summary>
		public Texture2D Texture;
		/// <summary>
		/// Texture color tint.
		/// </summary>
		public Color Color;
		/// <summary>
		/// If the frame is visible or not.
		/// </summary>
		public bool Visible = true;

		/// <summary>
		/// Potential parent frame.
		/// </summary>
		public Frame Parent;
		/// <summary>
		/// Potential children frames.
		/// </summary>
		public List<Frame> Children; 

		/// <summary>
		/// Create a blank frame.
		/// </summary>
		public Frame()
		{
			this.Rectangle = Rectangle.Empty;
			this.Texture = null;
			this.Color = Color.White;

			this.Children = new List<Frame>();

			GUIManager.Add(this);
		}

		/// <summary>
		/// Create a blank frame as the child of a frame.
		/// </summary>
		/// <param name="parentFrame">The parent frame.</param>
		public Frame(Frame parentFrame)
			: this()
		{
			this.Parent = parentFrame;
		}

		/// <summary>
		/// Add a child frame to the frame.
		/// </summary>
		/// <param name="childFrame">The child frame to add.</param>
		public void AddChild(Frame childFrame)
		{
			childFrame.Parent = this;
			if (!Children.Contains(childFrame))
				Children.Add(childFrame);
		}

		/// <summary>
		/// Remove a child frame from the frame.
		/// </summary>
		/// <param name="childFrame">The child frame to remove.</param>
		public void RemoveChild(Frame childFrame)
		{
			if (Children.Contains(childFrame))
				Children.Remove(childFrame);
		}

		/// <summary>
		/// Destroys the frame and all its children.
		/// </summary>
		public void Destroy()
		{
			if (Parent != null)
				Parent.RemoveChild(this);

			GUIManager.Remove(this);
		}

		/// <summary>
		/// Set the background texture of the frame.
		/// </summary>
		/// <param name="fileName">Filename of the texture.</param>
		public void SetTexture(string fileName)
		{
			Texture = GameServices.GetService<ContentManager>().Load<Texture2D>(fileName);
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
