using System;
using System.Collections.Generic;
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
        bool Enter = false;
        bool turn;
        int checkerSpeed = 2;

        public KeyboardInput(Core core)
        {
            this.core = core;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyUp(Keys.Enter) && Enter == true) 
            {
                Enter = false;
            }

            if (ks.IsKeyDown(Keys.Enter) && Enter == false) 
            {
                Enter = true;
                if (core.PlayerTurn == false)
                {
                    core.PlayerTurn = true;
                    foreach (var unit in core.Squad)
                    {
                        unit.step = 0;
                        unit.Fired = false;
                    }
                }
                else
                {
                    core.TestMonster.step = 0;
                    core.PlayerTurn = false;
                    Point target = Point.Zero;
                    float distance = 7000;
                    foreach (var unit in core.Squad)
                    {
                        Vector2 soldier = new Vector2(unit.Transform.Grid.X, unit.Transform.Grid.Y);
                        Vector2 monster = new Vector2(core.TestMonster.Transform.Grid.X, core.TestMonster.Transform.Grid.Y);

                        float newDistance = Vector2.Distance(soldier, monster);

                        if (newDistance < distance)
                        {
                            target = unit.Transform.Grid;
                            distance = newDistance;
                        }
                    }
                    core.TestMonster.GetComponent<Component.PathFinder>().Goal = target;
                    distance = 7000;
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
            if (ks.IsKeyDown(Keys.S)) 
            {
                foreach (var soldier in core.Squad) 
                {
                    if (soldier.Selected == true && soldier.Fired == false)
                    {
                        core.Volley.Add(new Shot(core.EntityManager));
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
                                double directionX = Math.Cos(soldier.GetComponent<Component.Transform>().World.Rotation);
                                double directionY = Math.Sin(soldier.GetComponent<Component.Transform>().World.Rotation);
                                shot.direction = new Vector2((float)directionX, (float)directionY);
                                shot.Transform.World.Rotation = soldier.Transform.World.Rotation;
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
                            core.Volley.Add(new Shot(core.EntityManager));
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
        }
    }
}