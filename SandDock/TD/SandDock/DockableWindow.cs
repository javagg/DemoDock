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
			Init();
		}

		public DockableWindow(SandDockManager manager, Control control, string text) : base(manager, control, text)
		{
			Init();
		}

		protected override DockingRules CreateDockingRules() => new DockingRules(true, false, true);

	    private void Init()
		{
		    if (Text.Length == 0) Text = "Dockable Window";
		    SetPositionMetaData(DockSituation.Docked, ContainerDockLocation.Right);
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
