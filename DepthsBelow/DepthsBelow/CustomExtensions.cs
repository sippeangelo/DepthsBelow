﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace DepthsBelow
{
	static class CustomExtensions
	{
		/// <summary>
		/// Load all content within a certain folder. The function
		/// returns a dictionary where the file name, without type
		/// extension, is the key and the texture object is the value.
		///
		/// The contentFolder parameter has to be relative to the
		/// game.Content.RootDirectory folder.
		/// 
		/// http://danielsaidi.wordpress.com/2010/01/26/xna-load-all-content-files-in-a-folder/
		/// </summary>
		/// <typeparam name="T">The content type.</typeparam>
		/// <param name="contentManager">The content manager for which content is to be loaded.</param>
		/// <param name="contentFolder">The game project root folder relative folder path.</param>
		/// <returns>A list of loaded content objects.</returns>
		public static Dictionary<String, T> LoadContent<T>(this ContentManager contentManager, string contentFolder)
		{
			//Load directory info, abort if none
			DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
			if (!dir.Exists)
				throw new DirectoryNotFoundException();
			//Init the resulting list
			Dictionary<String, T> result = new Dictionary<String, T>();

			//Load all files that matches the file filter
			FileInfo[] files = dir.GetFiles("*.*");
			foreach (FileInfo file in files)
			{
				string key = Path.GetFileNameWithoutExtension(file.Name);

				result[key] = contentManager.Load<T>(contentFolder + "/" + key);
			}
			//Return the result
			return result;
		}
	}
}
