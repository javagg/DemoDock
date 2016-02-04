using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TD.SandDock
{
	internal class Class14
	{
		private Class14()
		{
		}

		//[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		//private static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int dwMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

		public static bool Boolean_0
		{
			get
			{
				string text = Class14.String_0;
				text = Path.GetFileName(text).ToLower();
				return text == "luna.msstyles";
			}
		}

		public static string String_0
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(512);
				//Class14.GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, null, 0, null, 0);
				return stringBuilder.ToString();
			}
		}

		public static string String_1
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(512);
				//Class14.GetCurrentThemeName(null, 0, stringBuilder, stringBuilder.Capacity, null, 0);
				return stringBuilder.ToString();
			}
		}
	}
}
