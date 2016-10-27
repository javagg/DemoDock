using System.Drawing;

namespace TD.SandDock
{
	internal class Class20
	{
		public static bool Boolean_0
		{
			get
			{
				return Native.GetSystemMetrics(4096) != 0;
                //return true;
			}
		}

		public static Color Color_0
		{
			get
			{
				int sysColor = Native.GetSysColor(27);
				return ColorTranslator.FromWin32(sysColor);
			    //return SystemColors.ActiveCaption;
			}
		}

		public const int int_0 = -1;

		public const int int_1 = 4096;

		public const int int_2 = 33;
	}
}
