﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
    public class MonsterSpawn : Entity
    {
        //EntityManager entityManager;

        int wakingDistance = 6;

        List<SmallEnemy> swarm;

        public MonsterSpawn(ref List<SmallEnemy> Swarm)
            : base()
        {
            //this.entityManager = entityManager;
            swarm = Swarm;
        }

        public override void Update(GameTime gameTime)
        {
	        for (int index = 0; index < EntityManager.Entities.Count; index++)
	        {
		        var entity = EntityManager.Entities[index];
		        if (entity is Soldier)
		        {
			        Vector2 distance1 = new Vector2(this.GetComponent<Component.Transform>().Grid.Position.X,
			                                        this.GetComponent<Component.Transform>().Grid.Position.Y);
			        Vector2 distance2 = new Vector2(entity.GetComponent<Component.Transform>().Grid.Position.X,
			                                        entity.GetComponent<Component.Transform>().Grid.Position.Y);
			        if (Vector2.Distance(distance1, distance2) < wakingDistance)
			        {
				        var enemy = new SmallEnemy(ref swarm);
				        enemy.X = this.X;
				        enemy.Y = this.Y;
				        swarm.Add(enemy);
						this.Remove();
				        break;
			        }
		        }
	        }
        }
    }
}
