using System.ComponentModel;
using System.Drawing;

namespace TD.SandDock.Design
{
	internal class DocumentContainerDesigner : DockContainerDesigner
	{
	    protected override bool GetHitTest(Point point)
		{
			point = _container.PointToClient(point);
			var layoutSystem = _container.GetLayoutSystemAt(point);
		    if (!(layoutSystem is DocumentLayoutSystem)) return base.GetHitTest(point);
		    var documentLayoutSystem = (DocumentLayoutSystem) layoutSystem;
		    if (documentLayoutSystem.LeftScrollButtonBounds.Contains(point) || documentLayoutSystem.RightScrollButtonBounds.Contains(point))
		        return true;
		    return base.GetHitTest(point);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			_container = (DockContainer)component;
		}

		private DockContainer _container;
	}
}
