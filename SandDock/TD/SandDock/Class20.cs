using System.Drawing;

namespace TD.SandDock
{
	internal class Class20
	{
        private const int SM_REMOTESESSION = 0x1000;
        public static bool Boolean_0 => Native.GetSystemMetrics(SM_REMOTESESSION) != 0;

	    public static Color Color_0 => ColorTranslator.FromWin32(Native.GetSysColor(COLOR_GRADIENTACTIVECAPTION));

	    private const int COLOR_GRADIENTACTIVECAPTION = 27;
        public const int int_0 = -1;

		public const int int_2 = 33;
	}
}
