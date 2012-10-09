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
                    }
                }
                else
                {
                    core.TestMonster.step = 0;
                    core.PlayerTurn = false;
                    Point target = Point.Zero;
                    int distance = 7000;
                    foreach (var unit in core.Squad)
                    {
                        Vector2 soldier = new Vector2(unit.Transform.Grid.X, unit.Transform.Grid.Y);
                        Vector2 monster = new Vector2(core.TestMonster.Transform.Grid.X, core.TestMonster.Transform.Grid.Y);

                        int newDistance = core.FindDistance(soldier, monster);
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
        }
    }
}