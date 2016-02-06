using System;
using System.ComponentModel;
using System.Drawing;

namespace TD.SandDock.Design
{
	internal class DocumentContainerDesigner : DockContainerDesigner
	{
	    protected override bool GetHitTest(Point point)
		{
			point = this.dockContainer_1.PointToClient(point);
			LayoutSystemBase layoutSystemAt = this.dockContainer_1.GetLayoutSystemAt(point);
			if (layoutSystemAt is DocumentLayoutSystem)
			{
				DocumentLayoutSystem documentLayoutSystem = (DocumentLayoutSystem)layoutSystemAt;
				if (documentLayoutSystem.LeftScrollButtonBounds.Contains(point) || documentLayoutSystem.RightScrollButtonBounds.Contains(point))
				{
					return true;
				}
			}
			return base.GetHitTest(point);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.dockContainer_1 = (DockContainer)component;
		}

		private DockContainer dockContainer_1;
	}
}
