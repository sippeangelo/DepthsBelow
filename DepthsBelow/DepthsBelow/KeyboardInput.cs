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

        public KeyboardInput(Core core)
        {
            this.core = core;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Enter)) 
            {
                foreach (var unit in core.Squad)
                {
                    unit.step = 0;
                }
            }
        }
    }
}
