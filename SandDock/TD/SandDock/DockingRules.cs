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
            AllowDockLeft = allowDock;
            AllowDockRight = allowDock;
            AllowDockTop = allowDock;
            AllowDockBottom = allowDock;
            AllowTab = allowTab;
            AllowFloat = allowFloat;
		}

		internal void method_0(DockingRules[] dockingRules_0)
		{
		    foreach (var dockingRules in dockingRules_0)
		    {
		        AllowDockLeft = AllowDockLeft && dockingRules.AllowDockLeft;
                AllowDockRight = AllowDockRight && dockingRules.AllowDockRight;
                AllowDockTop = AllowDockTop && dockingRules.AllowDockTop;
                AllowDockBottom = AllowDockBottom && dockingRules.AllowDockBottom;
                AllowTab = AllowTab && dockingRules.AllowTab;
                AllowFloat = AllowFloat && dockingRules.AllowFloat;
		    }
		}

	    public bool AllowDockBottom { get; set; } = true;

	    public bool AllowDockLeft { get; set; } = true;

	    public bool AllowDockRight { get; set; } = true;

	    public bool AllowDockTop { get; set; } = true;

	    public bool AllowFloat { get; set; } = true;

	    public bool AllowTab { get; set; } = true;
	}
}
