using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TD.SandDock
{
	public class TabbedDocument : DockControl
	{
		public TabbedDocument()
		{
			this.method_18();
		}

		public TabbedDocument(SandDockManager manager, Control control, string text) : base(manager, control, text)
		{
			this.method_18();
		}

		protected override DockingRules CreateDockingRules()
		{
			return new DockingRules(false, true, false);
		}

		private void method_18()
		{
			if (this.Text.Length == 0)
			{
				this.Text = "Tabbed Document";
			}
			this.CloseAction = DockControlCloseAction.Dispose;
			this.PersistState = false;
			base.SetPositionMetaData(DockSituation.Document);
		}

		public override void Open()
		{
			base.Open(WindowOpenMethod.OnScreenActivate);
		}

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

		protected override Size DefaultSize
		{
			get
			{
				return new Size(550, 400);
			}
		}

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
}
