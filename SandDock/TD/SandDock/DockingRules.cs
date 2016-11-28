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

		internal void SetDockingRules(DockingRules[] rules)
		{
		    foreach (var rule in rules)
		    {
		        AllowDockLeft = AllowDockLeft && rule.AllowDockLeft;
                AllowDockRight = AllowDockRight && rule.AllowDockRight;
                AllowDockTop = AllowDockTop && rule.AllowDockTop;
                AllowDockBottom = AllowDockBottom && rule.AllowDockBottom;
                AllowTab = AllowTab && rule.AllowTab;
                AllowFloat = AllowFloat && rule.AllowFloat;
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
