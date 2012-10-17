using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DepthsBelow.Component
{
    public class Stat : Component
    {
        protected int stepsTaken = 0;
        protected int distanceToTarget = 0;
        protected int panic = 0;
        protected int soldierAim = 0;
        protected int weaponAccuracy = 0;
        protected int enemyDodge = 0;

        protected int penaltyRange = 5;

        public int GetAim()
        {
            return (soldierAim + weaponAccuracy);
        }

        public int GetDodge()
        {
            return enemyDodge;
        }

        public int Penalty(int distance)
        {
            return panic + GetPenalty(stepsTaken) + (int)(GetPenalty(distance) / 2);
        }

        public Stat(Entity parent) : base (parent)
        {

        }

        public bool targetInSight()
        {
            return false;
        }

        public int GetPenalty(int distance)
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
