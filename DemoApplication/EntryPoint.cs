using System;

namespace DemoApplication
{
	public class EntryPoint
	{

		[STAThread()]
		public static void Main()
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.Run(new frmMain());
		}

	}
}
