using System.IO;
using System.Text;

namespace TD.SandDock
{
	internal class Class14
	{
		private Class14()
		{
		}

		public static bool Boolean_0
		{
			get
			{
				string text = String_0;
				text = Path.GetFileName(text).ToLower();
				return text == "luna.msstyles";
			}
		}

		public static string String_0
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(512);
				Native.GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, null, 0, null, 0);
				return stringBuilder.ToString();
			}
		}

		public static string String_1
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(512);
                Native.GetCurrentThemeName(null, 0, stringBuilder, stringBuilder.Capacity, null, 0);
				return stringBuilder.ToString();
			}
		}
	}
}
