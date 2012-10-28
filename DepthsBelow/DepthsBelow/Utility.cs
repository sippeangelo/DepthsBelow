using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DepthsBelow.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DepthsBelow
{
	/// <summary>
	/// Various global utility functions.
	/// </summary>
    static class Utility
    {
		/// <summary>
		/// Calculate the chance of an entity with a Stat component to hit another unit.
		/// </summary>
		/// <param name="attacker">The attacking unit.</param>
		/// <param name="defender">The recieving unit.</param>
		/// <returns>Returns a hit chance percentage.</returns>
        public static int CalculateHitChance(Entity attacker, Entity defender)
        {
            Stat shooting = attacker.GetComponent<Stat>();
            Stat dodging = defender.GetComponent<Stat>();
            if (shooting == null || dodging == null) 
            {
                return 0;
            }
            int baseHitChance = shooting.GetAim;
            int baseDodgeChance = dodging.GetDodge;
            int distance = (int)Vector2.Distance(attacker.GetComponent<Transform>().World.Position, defender.GetComponent<Transform>().World.Position);
            int basePenalty = shooting.Penalty(distance / (Grid.TileSize));
            Console.WriteLine("Penalty = " + basePenalty);
            int chanceToHit = baseHitChance - baseDodgeChance - basePenalty;
            //Console.WriteLine(chanceToHit);
            return chanceToHit;
        }
        public static bool HitTest(Entity attacker, Entity defender, int chanceToHit)
        {
            Component.Stat shooting = attacker.GetComponent<Component.Stat>();
            Component.Stat dodging = defender.GetComponent<Component.Stat>();
            Random rand = new Random();
            if (rand.Next(0, 100) < chanceToHit) 
            {
                dodging.Life -= shooting.Strength - dodging.Defence / 2;
                return true;
            }
            return false;
        }
		private static Texture2D blank;

		public static void DrawLine(SpriteBatch batch, Color color, float width, Vector2 start, Vector2 end)
		{
			if (blank == null)
			{
				blank = new Texture2D(Core.GraphicsDeviceManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
				blank.SetData(new[] { color });
			}

			float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
			float length = Vector2.Distance(start, end);

			batch.Draw(blank, start, null, color,
			           angle, Vector2.Zero, new Vector2(length, width),
			           SpriteEffects.None, 0);
		}

		private static Texture2D solidTexture = null;
		/// <summary>
		/// Creates a 1x1 solid white XNA texture.
		/// </summary>
		/// <returns>Returns a 1x1 solid white Texture2D object.</returns>
		public static Texture2D GetSolidTexture()
		{
			if (solidTexture == null)
			{
				solidTexture = new Texture2D(GameServices.GetService<GraphicsDevice>(), 1, 1);
				solidTexture.SetData(new Color[] { Color.White });
			}

			return solidTexture;
		}

		/// <summary>
		/// Converts rectangles with negative sizes to positive sizes with its position offset.
		/// </summary>
		/// <param name="rect">The rectangle with potentially negative sizes.</param>
		/// <returns>Returns a rectangle with positive sizes.</returns>
		public static Rectangle MakeRectanglePositive(Rectangle rect)
		{
			if (rect.Width < 0)
			{
				rect.X += rect.Width;
				rect.Width = -rect.Width;
			}

			if (rect.Height < 0)
			{
				rect.Y += rect.Height;
				rect.Height = -rect.Height;
			}

			return rect;
		}
    }
}
