using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace TD.SandDock
{
	public class DockableWindow : DockControl
	{
		public DockableWindow()
		{
			this.method_18();
		}

		public DockableWindow(SandDockManager manager, Control control, string text) : base(manager, control, text)
		{
			this.method_18();
		}

		protected override DockingRules CreateDockingRules() => new DockingRules(true, false, true);

	    private void method_18()
		{
			if (this.Text.Length == 0)
			{
				this.Text = "Dockable Window";
			}
			base.SetPositionMetaData(DockSituation.Docked, ContainerDockLocation.Right);
		}

		public override void Open()
		{
            Open(WindowOpenMethod.OnScreenSelect);
		}

		protected override Size DefaultSize => new Size(250, 400);
	}

    [Designer("Design.UserDockControlDesigner", typeof(IDesigner)), Designer("Design.UserDockControlDocumentDesigner", typeof(IRootDesigner))]
    public class UserDockableWindow : DockableWindow
    {
    }
}
