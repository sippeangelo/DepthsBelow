using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DepthsBelow.GUI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace DepthsBelow
{
	// TODO: The frames created here must be disposed correctly when the Lua context is disposed
	static class LuaGUI
	{
		public static GUI.Frame CreateFrame()
		{
			return new Frame();
		}
		public static GUI.Frame CreateFrame(GUI.Frame parent)
		{
			return new GUI.Frame(parent);
		}

		public static GUI.Button CreateButton()
		{
			return new Button();
		}
		public static GUI.Button CreateButton(GUI.Frame parent)
		{
			return new GUI.Button(parent);
		}
	}

	public class Lua
	{
		private LuaInterface.Lua lua;

		public Lua()
		{
			ResetContext();
			ExposeLibraries();

			/*lua.DoString(@"
				local frame = CreateFrame('Frame');
				frame.X = 100; 
				local button = CreateFrame('Button', frame); 
				button:SetTexture('images/Enter'); 
				button.Width = 100; 
				button.Height = 50; 
				button.OnClick = function(clickPos) 
					Console.WriteLine(clickPos.X .. ' ' .. clickPos.Y); 
				end
			");*/
		}

		public void ExposeLibraries()
		{
			lua.NewTable("Console");
			lua.RegisterFunction("Console.Write", null, typeof(System.Console).GetMethod("Write", new Type[] { typeof(string) }));
			lua.RegisterFunction("Console.WriteLine", null, typeof(System.Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

			// Register GUI creation functions
			lua.RegisterFunction("_CreateFrame", null, typeof(LuaGUI).GetMethod("CreateFrame", new Type[] { }));
			lua.RegisterFunction("_CreateFrameAsChild", null, typeof(LuaGUI).GetMethod("CreateFrame", new Type[] { typeof(GUI.Frame) }));
			lua.RegisterFunction("_CreateButton", null, typeof(LuaGUI).GetMethod("CreateButton", new Type[] { }));
			lua.RegisterFunction("_CreateButtonAsChild", null, typeof(LuaGUI).GetMethod("CreateButton", new Type[] { typeof(GUI.Frame) }));
			lua.NewTable("GUI");
			/*
			 *	Because Lua can't dynamically call overloads of registered functions, a generic Lua function needs to do the decision making.
			 *	This way we can also dynamically create different frame classes with the same function.
			 */
			lua.DoString(@"
				CreateFrame = function(frameType, parent)
					frameType = string.lower(frameType):gsub('^%l', string.upper)
					if (parent == nil) then
						return _G['_Create' .. frameType]()
					else
						return _G['_Create' .. frameType .. 'AsChild'](parent)
					end
				end
			");
		}

		public void ResetContext()
		{
			if (lua != null)
				lua.Dispose();

			lua = new LuaInterface.Lua();
		}

		public void LoadScripts()
		{
			// Load scripts from script folder
			// HACK: This needs to load files on the fly instead of loading precompiled files by the content manager.
			// http://xbox.create.msdn.com/en-US/education/catalog/sample/winforms_series_2 maybe?
			var scripts = GameServices.GetService<ContentManager>().LoadContent<string>("scripts");
			foreach (var script in scripts)
			{
				try
				{
					lua.DoString(script.Value);
				}
				catch (LuaInterface.LuaException e)
				{
					Console.WriteLine("Lua error: " + e.Message);
				}
				
			}
		}
	}
}
