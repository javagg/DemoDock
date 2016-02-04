using System;

namespace TD.SandDock
{
    public delegate void ResolveDockControlEventHandler(object sender, ResolveDockControlEventArgs e);

    public class ResolveDockControlEventArgs : EventArgs
	{
		internal ResolveDockControlEventArgs(Guid guid)
		{
            Guid = guid;
		}

		public DockControl DockControl { get; set; }

		public Guid Guid { get; }
	}
}
