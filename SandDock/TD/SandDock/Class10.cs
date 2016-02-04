using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Class10 : Class6
	{
		public Class10(DockContainer container, SplitLayoutSystem splitLayout, LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, Point startPoint, DockingHints dockingHints) : base(container, dockingHints, false)
		{
			this.dockContainer_0 = container;
			this.splitLayoutSystem_0 = splitLayout;
			this.layoutSystemBase_0 = aboveLayout;
			this.layoutSystemBase_1 = belowLayout;
			this.point_0 = startPoint;
			if (splitLayout.SplitMode != Orientation.Horizontal)
			{
				this.int_7 = aboveLayout.Bounds.X + 25;
				this.int_8 = belowLayout.Bounds.Right - 25;
				this.float_2 = aboveLayout.WorkingSize.Width + belowLayout.WorkingSize.Width;
			}
			else
			{
				this.int_7 = aboveLayout.Bounds.Y + 25;
				this.int_8 = belowLayout.Bounds.Bottom - 25;
				this.float_2 = aboveLayout.WorkingSize.Height + belowLayout.WorkingSize.Height;
			}
			this.OnMouseMove(startPoint);
		}

		public override void Commit()
		{
			base.Commit();
			if (this.splittingManagerFinishedEventHandler_0 != null)
			{
				this.splittingManagerFinishedEventHandler_0(this.layoutSystemBase_0, this.layoutSystemBase_1, this.float_0, this.float_1);
			}
		}

		public override void OnMouseMove(Point position)
		{
			Rectangle empty = Rectangle.Empty;
			if (this.splitLayoutSystem_0.SplitMode != Orientation.Horizontal)
			{
				empty = new Rectangle(position.X - 2, this.splitLayoutSystem_0.Bounds.Y, 4, this.splitLayoutSystem_0.Bounds.Height);
				empty.X = Math.Max(empty.X, this.int_7);
				empty.X = Math.Min(empty.X, this.int_8 - 4);
				float num = (float)(this.layoutSystemBase_1.Bounds.Right - this.layoutSystemBase_0.Bounds.Left - 4);
				this.float_0 = (float)(empty.X - this.layoutSystemBase_0.Bounds.Left) / num * this.float_2;
				this.float_1 = this.float_2 - this.float_0;
			}
			else
			{
				empty = new Rectangle(this.splitLayoutSystem_0.Bounds.X, position.Y - 2, this.splitLayoutSystem_0.Bounds.Width, 4);
				empty.Y = Math.Max(empty.Y, this.int_7);
				empty.Y = Math.Min(empty.Y, this.int_8 - 4);
				float num2 = (float)(this.layoutSystemBase_1.Bounds.Bottom - this.layoutSystemBase_0.Bounds.Top - 4);
				this.float_0 = (float)(empty.Y - this.layoutSystemBase_0.Bounds.Top) / num2 * this.float_2;
				this.float_1 = this.float_2 - this.float_0;
			}
			base.method_1(new Rectangle(this.dockContainer_0.PointToScreen(empty.Location), empty.Size), false);
			Cursor.Current = ((this.splitLayoutSystem_0.SplitMode != Orientation.Horizontal) ? Cursors.VSplit : Cursors.HSplit);
		}

		public SplitLayoutSystem SplitLayoutSystem_0
		{
			get
			{
				return this.splitLayoutSystem_0;
			}
		}

		public event Class10.SplittingManagerFinishedEventHandler Event_1
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.splittingManagerFinishedEventHandler_0 = (Class10.SplittingManagerFinishedEventHandler)Delegate.Combine(this.splittingManagerFinishedEventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.splittingManagerFinishedEventHandler_0 = (Class10.SplittingManagerFinishedEventHandler)Delegate.Remove(this.splittingManagerFinishedEventHandler_0, value);
			}
		}

		private DockContainer dockContainer_0;

		private float float_0;

		private float float_1;

		private float float_2;

		internal const int int_6 = 25;

		private int int_7;

		private int int_8;

		private LayoutSystemBase layoutSystemBase_0;

		private LayoutSystemBase layoutSystemBase_1;

		private Point point_0 = Point.Empty;

		private SplitLayoutSystem splitLayoutSystem_0;

		private Class10.SplittingManagerFinishedEventHandler splittingManagerFinishedEventHandler_0;

		public delegate void SplittingManagerFinishedEventHandler(LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, float aboveSize, float belowSize);
	}
}
