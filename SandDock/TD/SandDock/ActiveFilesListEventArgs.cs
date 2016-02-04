using System;
using System.Drawing;
using System.Windows.Forms;

namespace TD.SandDock
{
    public delegate void ActiveFilesListEventHandler(object sender, ActiveFilesListEventArgs e);

    public class ActiveFilesListEventArgs : EventArgs
	{
		internal ActiveFilesListEventArgs(DockControl[] windows, Control control, Point position)
		{
            Windows = windows;
            Control = control;
            Position = position;
		}

		public Control Control { get; }

		public Point Position { get; }

        public DockControl[] Windows { get; }

	}
}
