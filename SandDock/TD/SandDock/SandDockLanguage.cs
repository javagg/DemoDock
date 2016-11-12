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
//		    string text =
//@"Visual Studio is attempting to load designers from a different assembly than the one your components are being created with. This will result in failure to load your designed component. This message is being displayed because SandDock has detected this condition and will give you more information that will help you to correct the problem.

//Component Assembly:

//";
//            string text2 = text;
//			text = string.Concat(text2, "Component Assembly:", Environment.NewLine, componentAssembly.Location, Environment.NewLine, Environment.NewLine);
//			string text3 = text;
//			text = string.Concat(text3, "Designer Assembly:", Environment.NewLine, designerAssembly.Location, Environment.NewLine, Environment.NewLine);
//			string text4 = text;
//			text = string.Concat(text4, "The component in question should be installed in only one location, by default within the \"Program Files\\Divelements\" folder. Please close Visual Studio, remove the errant assembly and try loading your designer again.", Environment.NewLine, Environment.NewLine, "Ensure that you do not attempt to save any designer that opens with errors, as this can result in loss of work. Note that you may receive this message multiple times, once for each component instance in your designer.");
			MessageBox.Show("", "Visual Studio Error Detected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		[Localizable(true)]
		public static string ActiveFilesText
		{
			get
			{
				return _activeFilesText;
			}
			set
			{
				_activeFilesText = value?? string.Empty;
			}
		}

		[Localizable(true)]
		public static string AutoHideText { get; set; } = "Auto Hide";

	    [Localizable(true)]
		public static string CloseText { get; set; } = "Close";

	    [Localizable(true)]
		public static string ScrollLeftText { get; set; } = "Scroll Left";

	    [Localizable(true)]
		public static string ScrollRightText { get; set; } = "Scroll Right";

	    [Localizable(true)]
		public static string WindowPositionText { get; set; } = "Window  Position";

	    private static string _activeFilesText = "Active Files";
    }
}
