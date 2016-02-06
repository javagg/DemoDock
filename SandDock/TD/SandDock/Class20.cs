using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TD.SandDock
{
	internal class Class20
	{
	    //[DllImport("user32.dll", CharSet = CharSet.Auto)]
		//private static extern int GetSysColor(int nIndex);

		//[DllImport("user32.dll")]
		//public static extern int GetSystemMetrics(int smIndex);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		public static bool Boolean_0
		{
			get
			{
				//return Class20.GetSystemMetrics(4096) != 0;
                return true;
			}
		}

		public static Color Color_0
		{
			get
			{
				//int sysColor = Class20.GetSysColor(27);
				//return ColorTranslator.FromWin32(sysColor);
			    return SystemColors.ActiveCaption;
			}
		}

		public const int int_0 = -1;

		public const int int_1 = 4096;

		public const int int_2 = 33;
	}
}
