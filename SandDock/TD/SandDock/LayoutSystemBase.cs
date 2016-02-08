using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
	public abstract class LayoutSystemBase
	{
		internal LayoutSystemBase()
		{
		}

		protected internal virtual void Layout(RendererBase renderer, Graphics graphics, Rectangle bounds, bool floating)
		{
			Bounds = bounds;
		}

		internal void method_0(SandDockManager sandDockManager_0, DockContainer dockContainer_1, LayoutSystemBase layoutSystemBase_0, DockControl dockControl_0, int int_2, Point point_0, DockingHints dockingHints_0, DockingManager dockingManager_0)
		{
			if (dockingManager_0 == DockingManager.Whidbey && Class6.smethod_0())
			{
				this.class7_0 = new Class8(sandDockManager_0, this.DockContainer, this, dockControl_0, int_2, point_0, dockingHints_0);
			}
			else
			{
				this.class7_0 = new Class7(sandDockManager_0, this.DockContainer, this, dockControl_0, int_2, point_0, dockingHints_0);
			}
			this.class7_0.DockingManagerFinished += this.vmethod_0;
			this.class7_0.Event_0 += this.vmethod_1;
			this.class7_0.OnMouseMove(Cursor.Position);
		}

		private void method_1()
		{
			this.class7_0.DockingManagerFinished -= this.vmethod_0;
			this.class7_0.Event_0 -= this.vmethod_1;
			this.class7_0 = null;
		}

		internal void method_2(SandDockManager sandDockManager_0, ContainerDockLocation containerDockLocation_0, ContainerDockEdge containerDockEdge_0)
		{
			DockControl[] dockControl_ = this.DockControl_0;
			int num = 0;
			if (dockControl_.Length > 0)
			{
				num = dockControl_[0].MetaData.DockedContentSize;
			}
			Rectangle rectangle = Class7.smethod_1(sandDockManager_0.DockSystemContainer);
			if (containerDockLocation_0 != ContainerDockLocation.Left)
			{
				if (containerDockLocation_0 != ContainerDockLocation.Right)
				{
					if (containerDockLocation_0 != ContainerDockLocation.Top && containerDockLocation_0 != ContainerDockLocation.Bottom)
					{
						goto IL_7C;
					}
					num = Math.Min(num, Convert.ToInt32((double)rectangle.Height * 0.9));
					goto IL_7C;
				}
			}
			num = Math.Min(num, Convert.ToInt32((double)rectangle.Width * 0.9));
			IL_7C:
			if (!(this is ControlLayoutSystem))
			{
			    this.Parent?.LayoutSystems.Remove(this);
			}
			else
			{
				LayoutUtilities.smethod_10((ControlLayoutSystem)this);
			}
			DockContainer dockContainer = sandDockManager_0.CreateNewDockContainer(containerDockLocation_0, containerDockEdge_0, num);
			if (!(dockContainer is DocumentContainer))
			{
				dockContainer.LayoutSystem.LayoutSystems.Add(this);
				return;
			}
			ControlLayoutSystem controlLayoutSystem = dockContainer.CreateNewLayoutSystem(this.WorkingSize);
			dockContainer.LayoutSystem.LayoutSystems.Add(controlLayoutSystem);
			if (this is SplitLayoutSystem)
			{
				((SplitLayoutSystem)this).MoveToLayoutSystem(controlLayoutSystem);
				return;
			}
			controlLayoutSystem.Controls.AddRange(this.DockControl_0);
		}

		private void method_3()
		{
		    if (Manager == null)
		        throw new InvalidOperationException("No SandDockManager is associated with this ControlLayoutSystem.");
		}

		protected internal virtual void OnDragOver(DragEventArgs drgevent)
		{
		}

		protected internal virtual void OnMouseDoubleClick()
		{
		}

		protected internal virtual void OnMouseDown(MouseEventArgs e)
		{
		}

		protected internal virtual void OnMouseLeave()
		{
		}

		protected internal virtual void OnMouseMove(MouseEventArgs e)
		{
		}

		protected internal virtual void OnMouseUp(MouseEventArgs e)
		{
		}

		internal virtual void vmethod_0(Class7.DockTarget dockTarget_0)
		{
			this.method_1();
		}

		internal virtual void vmethod_1(object sender, EventArgs e)
		{
			this.method_1();
		}

		internal virtual void vmethod_2(DockContainer dockContainer_1)
		{
			this.DockContainer = dockContainer_1;
		}

		internal abstract bool vmethod_3(ContainerDockLocation containerDockLocation_0);

		internal abstract void vmethod_4(RendererBase rendererBase_0, Graphics graphics_0, Font font_0);

		internal abstract bool Boolean_2
		{
			get;
		}

		internal abstract bool Boolean_3
		{
			get;
		}

		internal abstract bool Boolean_4
		{
			get;
		}

		public Rectangle Bounds { get; private set; } = Rectangle.Empty;

	    public DockContainer DockContainer { get; private set; }

	    internal abstract DockControl[] DockControl_0
		{
			get;
		}

		public bool IsInContainer => DockContainer != null;

	    public SplitLayoutSystem Parent => this.splitLayoutSystem_0;

	    private SandDockManager Manager => DockContainer?.Manager;

	    [EditorBrowsable(EditorBrowsableState.Advanced)]
		public SizeF WorkingSize
		{
			get
			{
				return this.sizeF_0;
			}
			set
			{
				if (value.Width <= 0f || value.Height <= 0f)
				{
					throw new ArgumentException("value");
				}
				this.sizeF_0 = value;
			}
		}

		internal Class7 class7_0;

	    internal const int int_0 = 250;

		internal const int int_1 = 400;

	    private SizeF sizeF_0 = new SizeF(250f, 400f);

		internal SplitLayoutSystem splitLayoutSystem_0;
	}
}
