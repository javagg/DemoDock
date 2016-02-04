using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Class11 : Class6
	{
		public Class11(SandDockManager manager, DockContainer container, Point startPoint) : base(container, manager.DockingHints, false)
		{
			this.dockContainer_0 = container;
			Rectangle rectangle = Rectangle.Empty;
			rectangle = Class7.smethod_2(Class7.smethod_1(container.Parent), container.Parent);
			rectangle = new Rectangle(container.PointToClient(rectangle.Location), rectangle.Size);
			int num = (manager == null) ? 30 : manager.MinimumDockContainerSize;
			num = Math.Max(num, LayoutUtilities.smethod_12(container));
			int num2 = (manager == null) ? 500 : manager.MaximumDockContainerSize;
			int int32_ = container.Int32_0;
			switch (container.Dock)
			{
			case DockStyle.Top:
				this.int_6 = startPoint.Y - (int32_ - num);
				this.int_7 = Math.Min(rectangle.Bottom - 20, startPoint.Y + (num2 - int32_));
				this.int_9 = startPoint.Y - container.Rectangle_0.Y;
				break;
			case DockStyle.Bottom:
				this.int_6 = Math.Max(rectangle.Top + 20, startPoint.Y - (num2 - int32_));
				this.int_7 = startPoint.Y + (int32_ - num);
				this.int_9 = startPoint.Y - container.Rectangle_0.Y;
				break;
			case DockStyle.Left:
				this.int_6 = startPoint.X - (int32_ - num);
				this.int_7 = Math.Min(rectangle.Right - 20, startPoint.X + (num2 - int32_));
				this.int_9 = startPoint.X - container.Rectangle_0.X;
				break;
			case DockStyle.Right:
				this.int_6 = Math.Max(rectangle.Left + 20, startPoint.X - (num2 - int32_));
				this.int_7 = startPoint.X + (int32_ - num);
				this.int_9 = startPoint.X - container.Rectangle_0.X;
				break;
			}
			this.OnMouseMove(startPoint);
		}

		public override void Commit()
		{
			base.Commit();
			if (this.resizingManagerFinishedEventHandler_0 != null)
			{
				this.resizingManagerFinishedEventHandler_0(this.int_8);
			}
		}

		public override void OnMouseMove(Point position)
		{
			Rectangle empty = Rectangle.Empty;
			if (this.dockContainer_0.Boolean_1)
			{
				empty = new Rectangle(position.X - this.int_9, 0, 4, this.dockContainer_0.Height);
				if (empty.X < this.int_6)
				{
					empty.X = this.int_6;
				}
				if (empty.X > this.int_7 - 4)
				{
					empty.X = this.int_7 - 4;
				}
			}
			else
			{
				empty = new Rectangle(0, position.Y - this.int_9, this.dockContainer_0.Width, 4);
				if (empty.Y < this.int_6)
				{
					empty.Y = this.int_6;
				}
				if (empty.Y > this.int_7 - 4)
				{
					empty.Y = this.int_7 - 4;
				}
			}
			switch (this.dockContainer_0.Dock)
			{
			case DockStyle.Top:
				this.int_8 = this.dockContainer_0.ContentSize + (empty.Y - this.dockContainer_0.Rectangle_0.Y);
				break;
			case DockStyle.Bottom:
				this.int_8 = this.dockContainer_0.ContentSize + (this.dockContainer_0.Rectangle_0.Y - empty.Y);
				break;
			case DockStyle.Left:
				this.int_8 = this.dockContainer_0.ContentSize + (empty.X - this.dockContainer_0.Rectangle_0.X);
				break;
			case DockStyle.Right:
				this.int_8 = this.dockContainer_0.ContentSize + (this.dockContainer_0.Rectangle_0.X - empty.X);
				break;
			}
			base.method_1(new Rectangle(this.dockContainer_0.PointToScreen(empty.Location), empty.Size), false);
			if (this.dockContainer_0.Dock != DockStyle.Left)
			{
				if (this.dockContainer_0.Dock != DockStyle.Right)
				{
					Cursor.Current = Cursors.HSplit;
					return;
				}
			}
			Cursor.Current = Cursors.VSplit;
		}

		public event Class11.ResizingManagerFinishedEventHandler Event_1
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.resizingManagerFinishedEventHandler_0 = (Class11.ResizingManagerFinishedEventHandler)Delegate.Combine(this.resizingManagerFinishedEventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.resizingManagerFinishedEventHandler_0 = (Class11.ResizingManagerFinishedEventHandler)Delegate.Remove(this.resizingManagerFinishedEventHandler_0, value);
			}
		}

		private DockContainer dockContainer_0;

		private int int_6;

		private int int_7;

		private int int_8;

		private int int_9;

		private Class11.ResizingManagerFinishedEventHandler resizingManagerFinishedEventHandler_0;

		public delegate void ResizingManagerFinishedEventHandler(int newSize);
	}
}
