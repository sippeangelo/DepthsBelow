using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DepthsBelow.Component
{
    public class Stat : Component
    {
        protected int hp = 0;
        protected int defence = 0;
        protected int stepsTaken = 0;
        protected int distanceToTarget = 0;
        protected int panic = 0;
        protected int soldierAim = 100;
        protected int weaponAccuracy = 0;
        protected int weaponStrength = 0;
        protected int ammo = 0;
        protected int enemyDodge = 0;

        protected int penaltyRange = 5;

        public int GetAim
        {
            get { return soldierAim + weaponAccuracy; }
            set { soldierAim = value; }
        }

        public int GetDodge
        {
            get { return enemyDodge; }
            set { enemyDodge = value; }
        }
        public int Life
        {
            get { return hp; }
            set 
            { 
                hp = value;
                if (hp <= 0) {
                    //If you kill me, I will become stronger than you can ever imagine.
                    Kill();
                }
            }
        }
        public int Defence
        {
            get { return defence; }
            set { defence = value; }
        }
        public int Strength
        {
            get { return weaponStrength; }
            set { weaponStrength = value; }
        }

        public void Kill()
        {
            hp = 0;
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
