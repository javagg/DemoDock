using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace TD.SandDock
{
	[Designer("TD.SandDock.Design.UserDockControlDesigner, SandDock.Design, Version=1.0.0.1, Culture=neutral, PublicKeyToken=75b7ec17dd7c14c3", typeof(IDesigner)), Designer("TD.SandDock.Design.UserDockControlDocumentDesigner, SandDock.Design, Version=1.0.0.1, Culture=neutral, PublicKeyToken=75b7ec17dd7c14c3", typeof(IRootDesigner))]
	public class UserDockableWindow : DockableWindow
	{
		public UserDockableWindow()
		{
		}
	}
}
