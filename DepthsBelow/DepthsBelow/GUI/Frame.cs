using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
		/// The x-coordinate of the left side of the frame.
		/// </summary>
		public int Left
		{
			get { return this.Rectangle.Left; }
		}
		/// <summary>
		/// The y-coordinate of the top side of the frame.
		/// </summary>
		public int Top
		{
			get { return this.Rectangle.Top; }
		}
		/// <summary>
		/// The x-coordinate of the right side of the frame.
		/// </summary>
		public int Right
		{
			get { return this.Rectangle.Right; }
		}
		/// <summary>
		/// The y-coordinate of the bottom side of the frame.
		/// </summary>
		public int Bottom
		{
			get { return this.Rectangle.Bottom; }
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
		/// The layer of the frame. 
		/// Bigger number means closer to the screen.
		/// </summary>
		public int Layer = 0;
		/// <summary>
		/// Gets the sum of this frame and all parent layers.
		/// </summary>
		public int LayerSum
		{
			get { return (Parent != null) ? this.Layer + Parent.LayerSum : this.Layer; }
		}

		/// <summary>
		/// The unique identifier of the frame.
		/// </summary>
		public int UID { get; private set; }
		/// <summary>
		/// Potential parent frame.
		/// </summary>
		public Frame Parent;
		/// <summary>
		/// Potential children frames.
		/// </summary>
		public List<Frame> Children;

		#region Events
		/// <summary>
		/// Occurs when the frame is clicked inside it's bounding rectangle.
		/// </summary>
		public event OnClickHandler OnClick;
		/// <summary>
		/// Gets the number of subscribers to the <see cref="OnClickHandler" /> event.
		/// </summary>
		public int OnClickCount
		{
			get { return (OnClick != null) ? OnClick.GetInvocationList().Length : 0; }
		}
		/// <summary>
		/// <see cref="OnClickHandler" /> event arguments.
		/// </summary>
		public class OnClickArgs : EventArgs
		{
			/// <summary>
			/// The position of the click relative to the frame position.
			/// </summary>
			public Point Position;

			/// <summary>
			/// The time of the click, since the start of the program.
			/// </summary>
			public TimeSpan Time;
		}
		/// <summary>
		/// Handler for click events.
		/// </summary>
		/// <param name="frame">The clicked frame.</param>
		/// <param name="e">The <see cref="OnClickArgs" /> instance containing the event data.</param>
		public delegate void OnClickHandler(Frame frame, OnClickArgs e);
		/// <summary>
		/// Raise an <see cref="OnClick" /> event on the frame.
		/// </summary>
		/// <param name="e">The <see cref="OnClickArgs" /> instance containing the event data.</param>
		public void Click(OnClickArgs e)
		{
			if (OnClick != null)
				OnClick(this, e);
		}
		#endregion

		private MouseState lastMouseState;

		/// <summary>
		/// Create a blank frame.
		/// </summary>
		public Frame()
		{
			this.Rectangle = Rectangle.Empty;
			this.Texture = null;
			this.Color = Color.White;

			this.Children = new List<Frame>();

			UID = GUIManager.Add(this);
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
			/*MouseState ms = Mouse.GetState();

			if (ms.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
			{
				// HACK: This should be handled globally somewhere else to prevent event bubbling
				if (
					ms.X >= AbsoluteRectangle.Left
					&& ms.X < AbsoluteRectangle.Right
					&& ms.Y >= AbsoluteRectangle.Top
					&& ms.Y < AbsoluteRectangle.Bottom)
				{
					if (OnClick != null)
					{
						var args = new OnClickArgs()
							           {
										   Position = new Point(ms.X - AbsoluteRectangle.X, ms.Y - AbsoluteRectangle.Y),
										   Time = gameTime.TotalGameTime
							           };
						OnClick(this, args);
					}
				}
			}

			lastMouseState = ms;*/

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
