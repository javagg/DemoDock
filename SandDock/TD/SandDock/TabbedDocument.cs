using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace TD.SandDock
{
	public class TabbedDocument : DockControl
	{
		public TabbedDocument()
		{
			method_18();
		}

		public TabbedDocument(SandDockManager manager, Control control, string text) : base(manager, control, text)
		{
			method_18();
		}

		protected override DockingRules CreateDockingRules() => new DockingRules(false, true, false);
        
        // Init
	    private void method_18()
		{
	        if (Text.Length == 0)
	            Text = "Tabbed Document";
	        CloseAction = DockControlCloseAction.Dispose;
			PersistState = false;
			SetPositionMetaData(DockSituation.Document);
		}

		public override void Open() => Open(WindowOpenMethod.OnScreenActivate);

	    [DefaultValue(typeof(DockControlCloseAction), "Dispose")]
		public override DockControlCloseAction CloseAction
		{
			get
			{
				return base.CloseAction;
			}
			set
			{
				base.CloseAction = value;
			}
		}

		protected override Size DefaultSize => new Size(550, 400);

	    [DefaultValue(false)]
		public override bool PersistState
		{
			get
			{
				return base.PersistState;
			}
			set
			{
				base.PersistState = value;
			}
		}
	}

    [Designer("Design.UserDockControlDocumentDesigner", typeof(IRootDesigner)), Designer("Design.UserDockControlDesigner", typeof(IDesigner))]
    public class UserTabbedDocument : TabbedDocument
    {
    }
}
