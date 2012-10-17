using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
    static class Utility
    {
        public static int CalculateHitChance(Entity attacker, Entity defender)
        {
            Component.Stat shooting = attacker.GetComponent<Component.Stat>();
            Component.Stat dodging = defender.GetComponent<Component.Stat>();
            if (shooting == null || dodging == null) 
            {
                return 0;
            }
            int baseHitChance = shooting.GetAim;
            int baseDodgeChance = dodging.GetDodge;
            int basePenalty = shooting.Penalty((int)Vector2.Distance(attacker.GetComponent<Component.Transform>().World.Position, defender.GetComponent<Component.Transform>().World.Position) / Grid.TileSize);
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
    }
}
