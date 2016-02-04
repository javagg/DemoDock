using System;

namespace TD.SandDock
{
    public delegate void DockControlEventHandler(object sender, DockControlEventArgs e);

    public class DockControlEventArgs : EventArgs
	{
		internal DockControlEventArgs(DockControl dockControl)
		{
            DockControl = dockControl;
		}

		public DockControl DockControl { get; }

	}
}
