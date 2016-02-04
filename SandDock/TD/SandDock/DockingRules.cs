using System;
using System.ComponentModel;

namespace TD.SandDock
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class DockingRules
	{
		public DockingRules()
		{
		}

		public DockingRules(bool allowDock, bool allowTab, bool allowFloat)
		{
			this.AllowDockLeft = allowDock;
			this.AllowDockRight = allowDock;
			this.AllowDockTop = allowDock;
			this.AllowDockBottom = allowDock;
			this.AllowTab = allowTab;
			this.AllowFloat = allowFloat;
		}

		internal void method_0(DockingRules[] dockingRules_0)
		{
			for (int i = 0; i < dockingRules_0.Length; i++)
			{
				DockingRules dockingRules = dockingRules_0[i];
				this.AllowDockLeft = (this.AllowDockLeft && dockingRules.AllowDockLeft);
				this.AllowDockRight = (this.AllowDockRight && dockingRules.AllowDockRight);
				this.AllowDockTop = (this.AllowDockTop && dockingRules.AllowDockTop);
				this.AllowDockBottom = (this.AllowDockBottom && dockingRules.AllowDockBottom);
				this.AllowTab = (this.AllowTab && dockingRules.AllowTab);
				this.AllowFloat = (this.AllowFloat && dockingRules.AllowFloat);
			}
		}

		public bool AllowDockBottom
		{
			get
			{
				return this.bool_3;
			}
			set
			{
				this.bool_3 = value;
			}
		}

		public bool AllowDockLeft
		{
			get
			{
				return this.bool_0;
			}
			set
			{
				this.bool_0 = value;
			}
		}

		public bool AllowDockRight
		{
			get
			{
				return this.bool_1;
			}
			set
			{
				this.bool_1 = value;
			}
		}

		public bool AllowDockTop
		{
			get
			{
				return this.bool_2;
			}
			set
			{
				this.bool_2 = value;
			}
		}

		public bool AllowFloat
		{
			get
			{
				return this.bool_5;
			}
			set
			{
				this.bool_5 = value;
			}
		}

		public bool AllowTab
		{
			get
			{
				return this.bool_4;
			}
			set
			{
				this.bool_4 = value;
			}
		}

		private bool bool_0 = true;

		private bool bool_1 = true;

		private bool bool_2 = true;

		private bool bool_3 = true;

		private bool bool_4 = true;

		private bool bool_5 = true;
	}
}
