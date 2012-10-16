using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
	class Lua
	{
		public Lua()
		{
			LuaInterface.Lua lua = new LuaInterface.Lua();

			lua.RegisterFunction("GetKeyboardState", null, typeof(DepthsBelow.Lua).GetMethod("GetKeyboardState"));
			object[] ret = lua.DoString("return GetKeyboardState()");
			Console.WriteLine(((KeyboardState)ret[0]).ToString());
		}

		public KeyboardState GetKeyboardState()
		{
			return Keyboard.GetState();
		}


	}
}
