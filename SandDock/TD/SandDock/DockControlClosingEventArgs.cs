using System;

namespace TD.SandDock
{
    public delegate void DockControlClosingEventHandler(object sender, DockControlClosingEventArgs e);


    public class DockControlClosingEventArgs : DockControlEventArgs
	{
		internal DockControlClosingEventArgs(DockControl dockControl, bool cancel) : base(dockControl)
		{
            Cancel = cancel;
		}

		public bool Cancel { get; set; }
	}
}
