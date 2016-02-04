using System;
using System.Drawing;

namespace TD.SandDock
{
    public delegate void ShowControlContextMenuEventHandler(object sender, ShowControlContextMenuEventArgs e);

    public class ShowControlContextMenuEventArgs : DockControlEventArgs
	{
		internal ShowControlContextMenuEventArgs(DockControl dockControl, Point position, ContextMenuContext context) : base(dockControl)
		{
			Position = position;
            Context = context;
		}

		public ContextMenuContext Context { get; }

		public Point Position { get; }
	}
}
