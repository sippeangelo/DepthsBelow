using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
    public class MonsterSpawn : Entity
    {
        //EntityManager entityManager;

        int wakingDistance = 13;

        List<SmallEnemy> swarm;

        public MonsterSpawn(EntityManager entityManager, ref List<SmallEnemy> Swarm)
            : base(entityManager)
        {
            //this.entityManager = entityManager;
            swarm = Swarm;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entityManager.Entities)
            {
                if (entity is Soldier) 
                {
                    Vector2 distance1 = new Vector2(this.GetComponent<Component.Transform>().Grid.Position.X, this.GetComponent<Component.Transform>().Grid.Position.Y);
                    Vector2 distance2 = new Vector2(entity.GetComponent<Component.Transform>().Grid.Position.X, entity.GetComponent<Component.Transform>().Grid.Position.Y);
                    if (Vector2.Distance(distance1, distance2) > wakingDistance)
                    {
                        var enemy = new SmallEnemy(entityManager, ref swarm);
                        enemy.X = this.X;
                        enemy.Y = this.Y;
                        entityManager.Add(enemy);
                        swarm.Add(enemy);
                    }
                }
            }
        }
    }
}
