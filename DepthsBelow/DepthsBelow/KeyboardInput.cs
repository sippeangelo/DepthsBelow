﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DepthsBelow
{
    public class KeyboardInput
    {
        Core core;
	    private KeyboardState lastKeyboardState;

        int checkerSpeed = 2;

        int bulletCost = 5;
        int rocketCost = 10;

        public KeyboardInput(Core core)
        {
            this.core = core;
        }

		/*public List<List<Soldier>> GetGroups(List<Soldier> soldiers)
		{
			var ungrouped = soldiers.ToList();
			var groups = new List<List<Soldier>>();

			for (int index = 0; index < soldiers.Count; index++)
			{
				var checkingSoldier = soldiers[index];
				var group = new List<Soldier>();

				for (int i = 0; i < soldiers.Count; i++)
				{
					var soldier = soldiers[i];
					if (ungrouped.Contains(soldier))
					{
						float distance = Vector2.Distance(checkingSoldier.Transform.World + checkingSoldier.Transform.World.Origin,
						                                  soldier.Transform.World + soldier.Transform.World.Origin);
						if (distance <= ((float) Grid.TileSize * 1f))
						{
							ungrouped.Remove(soldier);
							group.Add(soldier);
						}
					}
				}

				if (group.Count > 0)
					groups.Add(group);
			}

			return groups;
		}*/

		// Get a list of dynamic soldier groups
		public List<List<Soldier>> GetGroups(List<Soldier> soldiers, float maxRange)
		{
			var alreadyGrouped = new List<Soldier>();
			var groups = new List<List<Soldier>>();

			for (int index = 0; index < soldiers.Count; index++)
			{
				var soldier = soldiers[index];

				if (!alreadyGrouped.Contains(soldier))
				{
					var group = new List<Soldier>();
					GetNearChain(ref group, soldiers, soldier, maxRange);
					alreadyGrouped.AddRange(group);

					if (group.Count > 0)
						groups.Add(group);
				}
			}

			return groups;
		}

		// Get all soldiers who are close to each other in a chain
		public void GetNearChain(ref List<Soldier> group, List<Soldier> soldiers, Soldier soldier, float maxRange)
		{
			for (int index = 0; index < soldiers.Count; index++)
			{
				var nearSoldier = soldiers[index];

				if (!group.Contains(nearSoldier))
				{
					float distance = Vector2.Distance(nearSoldier.Transform.World + nearSoldier.Transform.World.Origin,
						                                  soldier.Transform.World + soldier.Transform.World.Origin);
					if (distance <= maxRange)
					{
						group.Add(nearSoldier);
						GetNearChain(ref group, soldiers, nearSoldier, maxRange);
					}
				}
			}
		}

	    private DynamicGroupManager groupManager;

	    public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

			// Brightness
			if (ks.IsKeyUp(Keys.Up) && lastKeyboardState.IsKeyDown(Keys.Up))
				core.Brightness = (int)MathHelper.Clamp(core.Brightness + 5, 0, 254);
			if (ks.IsKeyUp(Keys.Down) && lastKeyboardState.IsKeyDown(Keys.Down))
				core.Brightness = (int)MathHelper.Clamp(core.Brightness - 5, 0, 254);			

			// Debug test: grouping
			if (ks.IsKeyUp(Keys.K) && lastKeyboardState.IsKeyDown(Keys.K))
			{
				if (groupManager == null)
					groupManager = new DynamicGroupManager(core.Squad.Cast<Entity>().ToList(), Grid.TileSize * 1.5f);
				groupManager.UpdateGroups();
				//var groups = GetGroups(core.Squad, (float)Grid.TileSize * 1.5f);
				var groups = groupManager.Groups;
				foreach (var group in groups)
				{
					Debug.WriteLine("Group #" + groups.IndexOf(group) + " Panic: " + group.Panic);
					foreach (var entity in group.Entities)
					{
						var soldier = (Soldier)entity;
						Debug.WriteLine(soldier.Name + " " + soldier.GetHashCode());
					}
				}
			}

			// Next turn
            if (ks.IsKeyDown(Keys.Enter) && lastKeyboardState.IsKeyUp(Keys.Enter)) 
            {
				if (core.TurnManager.CurrentTurn == core.TurnManager["Player"])
                {
                    foreach (var unit in core.Squad)
                    {
                        unit.AP = unit.MaxActionPoints;
                        unit.Fired = false;
                    }

					core.TurnManager.EndTurn();
                }
                
				if (core.TurnManager.CurrentTurn == core.TurnManager["Computer"])
                {
                    foreach (var enemy in core.Swarm)
                    {
                        enemy.step = 0;
                        enemy.AttackedThisTurn = false;
                        Point target = Point.Zero;
                        float distance = 7000;
                        foreach (var unit in core.Squad)
                        {
                            Vector2 soldier = new Vector2(unit.Transform.Grid.X, unit.Transform.Grid.Y);
                            Vector2 monster = new Vector2(enemy.Transform.Grid.X, enemy.Transform.Grid.Y);

                            float newDistance = Vector2.Distance(soldier, monster);

                            if (newDistance < distance)
                            {
                                target = unit.Transform.Grid;
                                distance = newDistance;
                            }
                        }
                        enemy.GetComponent<Component.PathFinder>().Goal = target;
                        distance = 7000;

                    }

                    core.TestMonster.step = 0;
                    Point testTarget = Point.Zero;
                    float testDistance = 7000;
                    foreach (var unit in core.Squad)
                    {
                        Vector2 soldier = new Vector2(unit.Transform.Grid.X, unit.Transform.Grid.Y);
                        Vector2 monster = new Vector2(core.TestMonster.Transform.Grid.X, core.TestMonster.Transform.Grid.Y);

                        float newDistance = Vector2.Distance(soldier, monster);

                        if (newDistance < testDistance)
                        {
                            testTarget = unit.Transform.Grid;
                            testDistance = newDistance;
                        }
                    }
                    core.TestMonster.GetComponent<Component.PathFinder>().Goal = testTarget;
                    testDistance = 7000;

					core.TurnManager.EndTurn();
                }
            }
            if (ks.IsKeyDown(Keys.A)) 
            {
                /*foreach (var unit in core.Swarm)
                {
                    unit.X = 200;
                    unit.Y = 200;
                }*/
                core.TestMonster.X = 12;
                core.TestMonster.Y = 4;
            }
			if (ks.IsKeyUp(Keys.O) && lastKeyboardState.IsKeyDown(Keys.O))
            {
	            try
	            {
					EntityManager.GetEntities<Door>()[0].Toggle();
	            }
	            catch
	            {
		            
	            }
            }
			if (ks.IsKeyUp(Keys.P) && lastKeyboardState.IsKeyDown(Keys.P))
            {
	            try
	            {
		            var pos = core.GroupManager.Groups[1].Entities[0].Transform.World.Position;
					var enemy = new SmallEnemy(ref core.Swarm);
		            enemy.Transform.World.Position = pos + new Vector2(0, Grid.TileSize*3);
					core.Swarm.Add(enemy);
	            }
	            catch
	            {
		            
	            }
            }
            if (ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.R)) 
            {
                foreach (var soldier in core.Squad) 
                {
                    if (soldier.Selected == true && soldier.Fired == false)
                    {
                        bool enoughActionPoints = true;
                        int cost = 0;
                        string type = "";
                        if (ks.IsKeyDown(Keys.S)) 
                        {
                            cost = bulletCost;
                            type = "bullet";
                        } 
                        else if (ks.IsKeyDown(Keys.R)) 
                        {
                            cost = rocketCost;
                            type = "Rocket";
                        }
                        if (soldier.AP >= cost)
                        {
                            core.Volley.Add(new Shot(soldier.GetComponent<Component.Stat>(), type));
                            soldier.AP -= cost;
                            soldier.Fired = true;
                        }
                        else
                        {
                            enoughActionPoints = false;
                        }
                        if (enoughActionPoints == true)
                        {
                            foreach (var shot in core.Volley)
                            {
                                if (shot.select == false)
                                {
                                    shot.select = true;
                                    shot.X = soldier.X;
                                    shot.Y = soldier.Y;
	                                //shot.Transform.World.Position += soldier.Transform.World.Origin;
	                                //shot.Transform.World.X += Grid.TileSize / 2;
	                                //shot.Transform.World.Y += Grid.TileSize / 2;
                                }
                                if (shot.direction == Vector2.Zero)
                                {
                                    double directionX = Math.Cos(soldier.Transform.World.Rotation);
                                    double directionY = Math.Sin(soldier.Transform.World.Rotation);
                                    shot.direction = new Vector2((float)directionX, (float)directionY);
                                    shot.Transform.World.Rotation = soldier.Transform.World.Rotation;
                                    Vector2 yo = soldier.Transform.World.Position;
                                    shot.Stat.Remember = yo;
                                }
                            }
                        }
                    }
                }
            }
            if (ks.IsKeyDown(Keys.C)) 
            {
                foreach (var soldier in core.Squad)
                {
                    if (soldier.Selected == true && soldier.Fired == false)
                    {
                        Point checker = Point.Zero;
                        double directionX = Math.Cos(soldier.GetComponent<Component.Transform>().World.Rotation);
                        double directionY = Math.Sin(soldier.GetComponent<Component.Transform>().World.Rotation);
                        Vector2 direction = new Vector2((int)directionX, (int)directionY);
                        int range = 120;
                        int current = 0;
                        bool hit = false;
                        checker = new Point((int)soldier.Transform.World.Origin.X, (int)soldier.Transform.World.Origin.Y);
                        do
                        {
                            checker.X += (int)direction.X * checkerSpeed;
                            checker.Y += (int)direction.Y * checkerSpeed;
                            foreach (var creature in core.Swarm) 
                            {
                                if (creature.GetComponent<Component.Collision>().Rectangle.Contains(checker)) 
                                {
                                    hit = true;
                                }
                            }
                            if (core.TestMonster.GetComponent<Component.Collision>().Rectangle.Contains(checker)) 
                            {

                            }
                            current++;
                        } while (hit == false && current < range);
                        current = 0;
                        if (soldier.Selected == true && soldier.Fired == false && hit == true)
                        {
                            core.Volley.Add(new Shot(soldier.GetComponent<Component.Stat>(), "Bullet"));
                            soldier.Fired = true;
                            foreach (var shot in core.Volley)
                            {
                                if (shot.select == false)
                                {
                                    shot.select = true;
                                    shot.X = soldier.X;
                                    shot.Y = soldier.Y;
                                    //shot.Transform.World.X += Grid.TileSize / 2;
                                    //shot.Transform.World.Y += Grid.TileSize / 2;
                                }
                                if (shot.direction == Vector2.Zero)
                                {
                                    shot.direction = new Vector2((float)directionX, (float)directionY);
                                    shot.Transform.World.Rotation = soldier.Transform.World.Rotation;
                                }
                            }
                        }
                        hit = false;
                    }
                }
            }

	        lastKeyboardState = ks;
        }
    }
}