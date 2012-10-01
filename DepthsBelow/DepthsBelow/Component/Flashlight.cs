using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow.Component
{
	public class Flashlight : Component
	{
		public float Range
		{
			get { return KryptonLight.Range; }
			set { KryptonLight.Range = value; }
		}
		public float Fov
		{
			get { return KryptonLight.Fov; }
			set { KryptonLight.Fov = value; }	
		}
		public float Intensity
		{
			get { return KryptonLight.Intensity; }
			set { KryptonLight.Intensity = value; }
		}
		public float Angle
		{
			get { return KryptonLight.Angle; }
			set { KryptonLight.Angle = value; }
		}
		public Vector2 Position
		{
			get { return KryptonLight.Position; }
			set { KryptonLight.Position = value; }
		}

		public Color Color
		{
			get { return KryptonLight.Color; }
			set { KryptonLight.Color = value; }
		}

		public bool IsOn
		{
			get { return KryptonLight.IsOn; }
			set { KryptonLight.IsOn = value; }
		}

		public Krypton.Lights.ShadowType ShadowType
		{
			get { return KryptonLight.ShadowType; }
			set { KryptonLight.ShadowType = value; }
		}

		public Krypton.Lights.Light2D KryptonLight;

		public Flashlight(Entity parent)
			: base(parent)
		{
			KryptonLight = new Krypton.Lights.Light2D()
			{
				Texture = Krypton.LightTextureBuilder.CreatePointLight(Core.GraphicsDevice, 512),
				Range = (float)(100),
				Position = parent.Transform.World + Parent.Transform.World.Origin,
				Angle = parent.Transform.World.Rotation,
				Color = Color.White,
				Intensity = 1f,
				Fov = MathHelper.TwoPi,
				IsOn = true,
				ShadowType = Krypton.Lights.ShadowType.Illuminated
			};
		}

		public static explicit operator Krypton.Lights.Light2D(Flashlight flashlight)
		{
			return flashlight.KryptonLight;
		}

		public override void Update(GameTime gameTime)
		{
			Position = Parent.Transform.World + Parent.Transform.World.Origin;
			Angle = Parent.Transform.World.Rotation;
		}

	}
}
