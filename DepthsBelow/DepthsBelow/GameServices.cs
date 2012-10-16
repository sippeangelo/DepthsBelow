using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
	/*
	 * http://roy-t.nl/index.php/2010/08/25/xna-accessing-contentmanager-and-graphicsdevice-anywhere-anytime-the-gameservicecontainer/
	 */
	public static class GameServices
	{
		private static GameServiceContainer container;
		public static GameServiceContainer Instance
		{
			get { return container ?? (container = new GameServiceContainer()); }
		}

		public static T GetService<T>()
		{
			return (T)Instance.GetService(typeof(T));
		}

		public static void AddService<T>(T service)
		{
			Instance.AddService(typeof(T), service);
		}

		public static void RemoveService<T>()
		{
			Instance.RemoveService(typeof(T));
		}
	}
}
