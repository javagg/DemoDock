using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace TD.SandDock
{
	public sealed class SandDockLanguage
	{
		public static void ShowCachedAssemblyError(Assembly componentAssembly, Assembly designerAssembly)
		{
			string text = SandDockLanguage.string_6 + Environment.NewLine + Environment.NewLine;
			string text2 = text;
			text = string.Concat(text2, "Component Assembly:", Environment.NewLine, componentAssembly.Location, Environment.NewLine, Environment.NewLine);
			string text3 = text;
			text = string.Concat(text3, "Designer Assembly:", Environment.NewLine, designerAssembly.Location, Environment.NewLine, Environment.NewLine);
			string text4 = text;
			text = string.Concat(text4, SandDockLanguage.string_7, Environment.NewLine, Environment.NewLine, SandDockLanguage.string_8);
			MessageBox.Show(text, "Visual Studio Error Detected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		[Localizable(true)]
		public static string ActiveFilesText
		{
			get
			{
				return SandDockLanguage.string_5;
			}
			set
			{
				SandDockLanguage.string_5 = value?? string.Empty;
			}
		}

		[Localizable(true)]
		public static string AutoHideText
		{
			get
			{
				return SandDockLanguage.string_1;
			}
			set
			{
				SandDockLanguage.string_1 = value;
			}
		}

		[Localizable(true)]
		public static string CloseText
		{
			get
			{
				return SandDockLanguage.string_0;
			}
			set
			{
				SandDockLanguage.string_0 = value;
			}
		}

		[Localizable(true)]
		public static string ScrollLeftText
		{
			get
			{
				return SandDockLanguage.string_2;
			}
			set
			{
				SandDockLanguage.string_2 = value;
			}
		}

		[Localizable(true)]
		public static string ScrollRightText
		{
			get
			{
				return SandDockLanguage.string_3;
			}
			set
			{
				SandDockLanguage.string_3 = value;
			}
		}

		[Localizable(true)]
		public static string WindowPositionText
		{
			get
			{
				return SandDockLanguage.string_4;
			}
			set
			{
				SandDockLanguage.string_4 = value;
			}
		}

		private static string string_0 = "Close";

		private static string string_1 = "Auto Hide";

		private static string string_2 = "Scroll Left";

		private static string string_3 = "Scroll Right";

		private static string string_4 = "Window  Position";

		private static string string_5 = "Active Files";

		private static string string_6 = "Visual Studio is attempting to load designers from a different assembly than the one your components are being created with. This will result in failure to load your designed component. This message is being displayed because SandDock has detected this condition and will give you more information that will help you to correct the problem.";

		private static string string_7 = "The component in question should be installed in only one location, by default within the \"Program Files\\Divelements\" folder. Please close Visual Studio, remove the errant assembly and try loading your designer again.";

		private static string string_8 = "Ensure that you do not attempt to save any designer that opens with errors, as this can result in loss of work. Note that you may receive this message multiple times, once for each component instance in your designer.";
	}
}
