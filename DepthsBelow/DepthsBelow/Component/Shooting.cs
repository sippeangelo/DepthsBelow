using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DepthsBelow.Component
{
    class Shooting : Component
    {
        protected int stepsTaken = 0;
        protected int distanceToTarget = 0;
        protected int panic = 0;
        protected int soldierAim = 0;
        protected int weaponAccuracy = 0;
        protected int enemyDodge = 0;

        protected int penaltyRange = 5;

        public Shooting(Entity parent) : base (parent)
        {

        }

        public bool targetInSight()
        {
            return false;
        }

        public int CalculateChance()
        {
            int baseHitChance = soldierAim + weaponAccuracy;
            int baseDodgeChance = panic + enemyDodge;
            int basePenalty = ReturnPenalty(stepsTaken) + (int)(ReturnPenalty(distanceToTarget) / 2);
            int chanceToHit = baseHitChance - baseDodgeChance - basePenalty;
            return chanceToHit;
        }

        public int ReturnPenalty(int distance)
        {
            int penalty = 0;
            if (distance >= penaltyRange * 3)
            {
                penalty = 100;
            }
            else if (distance >= penaltyRange * 2)
            {
                distance -= penaltyRange * 2;
                penalty = penaltyRange + penaltyRange * 3 + distance * 5;
            }
            else if (distance >= penaltyRange)
            {
                distance -= penaltyRange;
                penalty = penaltyRange + distance * 3;
            }
            else
            {
                penalty = distance;
            }
            return penalty;
        }
    }
}
