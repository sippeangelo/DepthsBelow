using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
		/// Absolute rectangle of the frame. 
		/// Takes parent frame positions in consideration and converts negative widths and heights.
		/// </summary>
		public Rectangle AbsoluteRectangle
		{
			get
			{
				if (Parent != null)
					return Utility.MakeRectanglePositive(new Rectangle(Parent.AbsoluteRectangle.X + Rectangle.X, Parent.AbsoluteRectangle.Y + Rectangle.Y, Rectangle.Width, Rectangle.Height));
				else
					return Utility.MakeRectanglePositive(Rectangle);
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

		public bool _Visible = true;
		/// <summary>
		/// If the frame is visible or not.
		/// </summary>
		public bool Visible
		{
			get { return (Parent != null) ? _Visible && Parent.Visible : _Visible; }
			set { _Visible = value; }
		}

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

		/// <summary>
		/// Storage for property data
		/// </summary>
		public Dictionary<string, object> Properties = new Dictionary<string, object>();
		/// <summary>
		/// Shorthand for frame properties.
		/// </summary>
		public object this[string key]
		{
			get { return Properties[key]; }
			set { Properties[key] = value; }
		}

		/// <summary>
		/// Controls whether mouse events can be handled by the frame.
		/// </summary>
		public bool MouseEnabled = false;

		#region Events

		/// <summary>
		/// Handler for click events.
		/// </summary>
		/// <param name="frame">The frame.</param>
		/// <param name="args">The <see cref="GUIManager.MouseEventArgs" /> instance containing the event data.</param>
		public delegate void OnClickHandler(Frame frame, GUIManager.MouseEventArgs args);
		/// <summary>
		/// Occurs when the frame is clicked inside it's bounding rectangle.
		/// </summary>
		public event OnClickHandler OnClick;

		/// <summary>
		/// Handler for press events.
		/// </summary>
		/// <param name="frame">The frame.</param>
		/// <param name="args">The <see cref="GUIManager.MouseEventArgs" /> instance containing the event data.</param>
		public delegate void OnPressHandler(Frame frame, GUIManager.MouseEventArgs args);
		/// <summary>
		/// Occurs when the frame is clicked inside it's bounding rectangle.
		/// </summary>
		public event OnPressHandler OnPress;

		/// <summary>
		/// Handler for release events.
		/// </summary>
		/// <param name="frame">The frame.</param>
		/// <param name="args">The <see cref="GUIManager.MouseEventArgs" /> instance containing the event data.</param>
		public delegate void OnReleaseHandler(Frame frame, GUIManager.MouseEventArgs args);
		/// <summary>
		/// Occurs when the frame is clicked inside it's bounding rectangle.
		/// </summary>
		public event OnReleaseHandler OnRelease;

		/// <summary>
		/// Handler for mouse over events.
		/// </summary>
		/// <param name="frame">The frame.</param>
		/// <param name="args">The <see cref="GUIManager.MouseEventArgs" /> instance containing the event data.</param>
		public delegate void OnMouseOverHandler(Frame frame, GUIManager.MouseEventArgs args);
		/// <summary>
		/// Occurs when the frame is clicked inside it's bounding rectangle.
		/// </summary>
		public event OnMouseOverHandler OnMouseOver;

		/// <summary>
		/// Handler for mouse out events.
		/// </summary>
		/// <param name="frame">The frame.</param>
		/// <param name="args">The <see cref="GUIManager.MouseEventArgs" /> instance containing the event data.</param>
		public delegate void OnMouseOutHandler(Frame frame, GUIManager.MouseEventArgs args);
		/// <summary>
		/// Occurs when the frame is clicked inside it's bounding rectangle.
		/// </summary>
		public event OnMouseOutHandler OnMouseOut;


		/// <summary>
		/// Raises the specified event by name.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
		/// <returns>Returns true if any event was raised.</returns>
		public bool Raise(string eventName, EventArgs eventArgs)
		{
			// Get the event field info
			var fieldInfo = this.GetType().GetField(eventName, BindingFlags.NonPublic | BindingFlags.Instance);
			// If the event exists
			if (fieldInfo != null)
			{
				var eventDelegate = (MulticastDelegate)fieldInfo.GetValue(this);
				// If there's any subscribed events...
				if (eventDelegate != null)
				{
					// Invoke their raise methods
					foreach (var handler in eventDelegate.GetInvocationList())
					{
						try
						{
							handler.Method.Invoke(handler.Target, new object[] { this, eventArgs });
						}
						catch (Exception)
						{
							return false;
						}
					}

					return true;
				}

				return false;
			}

			return false;
		}

		/// <summary>
		/// Gets the subscriber count for a specific event.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <returns>Returns the number of subscribers of the event.</returns>
		public int GetSubscriberCount(string eventName)
		{
			// Get the event field info
			var fieldInfo = this.GetType().GetField(eventName, BindingFlags.NonPublic | BindingFlags.Instance);
			// If the event exists
			if (fieldInfo != null)
			{
				var eventDelegate = (MulticastDelegate)fieldInfo.GetValue(this);
				// If there's any subscribed events...
				if (eventDelegate != null)
					return eventDelegate.GetInvocationList().Length;

				return 0;
			}

			return 0;
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

			this.UID = GUIManager.Add(this);
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
						var args = new MouseEventArgs()
							           {
										   Position = new Point(ms.X - AbsoluteRectangle.X, ms.Y - AbsoluteRectangle.Y),
										   Time = gameTime.TotalGameTime
							           };
						OnClick(this, args);
					}
				}
			}

			lastMouseState = ms;*/

			/*foreach (var child in Children)
				child.Update(gameTime);*/
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
			{
				if (Texture != null)
					spriteBatch.Draw(Texture, 
						AbsoluteRectangle, 
						null, 
						Color, 
						0, 
						Vector2.Zero, 
						((Width < 0) ? SpriteEffects.FlipHorizontally : 0)
						| ((Height < 0) ? SpriteEffects.FlipVertically : 0),
						0);

				/*foreach (var child in Children)
					child.Draw(spriteBatch);*/
			}
		}
	}
}
