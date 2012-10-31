using System;
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

        int wakingDistance = 12;
        int sleepingDistance = 6;
        int wakingTurn = 0;

        string type;

        List<SmallEnemy> swarm;

        public MonsterSpawn(string _type, int _wakingDistance, int _wakingTurn, ref List<SmallEnemy> Swarm)
            : base()
        {
            //this.entityManager = entityManager;
            type = _type;
            swarm = Swarm;
            wakingDistance = _wakingDistance;
            wakingTurn = _wakingTurn;
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
                    if (Vector2.Distance(distance1, distance2) < wakingDistance && TurnManager.currentTurn >= wakingTurn/* && Vector2.Distance(distance1, distance2) > sleepingDistance*/)
			        {
				        var enemy = new SmallEnemy(ref swarm);
                        if (type == "BUB") 
                        {
                            enemy.LoadBoss();
                        }
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
