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
			this.container = container;
			this.SplitLayout = splitLayout;
			this.aboveLayout = aboveLayout;
			this.belowLayout = belowLayout;
			this.startPoint = startPoint;
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
			OnMouseMove(startPoint);
		}

		public override void Commit()
		{
			base.Commit();
            SplittingManagerFinished?.Invoke(this.aboveLayout, this.belowLayout, this.aboveSize, this.belowSize);
		}

		public override void OnMouseMove(Point position)
		{
			Rectangle empty = Rectangle.Empty;
			if (this.SplitLayout.SplitMode != Orientation.Horizontal)
			{
				empty = new Rectangle(position.X - 2, this.SplitLayout.Bounds.Y, 4, this.SplitLayout.Bounds.Height);
				empty.X = Math.Max(empty.X, this.int_7);
				empty.X = Math.Min(empty.X, this.int_8 - 4);
				float num = (float)(this.belowLayout.Bounds.Right - this.aboveLayout.Bounds.Left - 4);
				this.aboveSize = (float)(empty.X - this.aboveLayout.Bounds.Left) / num * this.float_2;
				this.belowSize = this.float_2 - this.aboveSize;
			}
			else
			{
				empty = new Rectangle(this.SplitLayout.Bounds.X, position.Y - 2, this.SplitLayout.Bounds.Width, 4);
				empty.Y = Math.Max(empty.Y, this.int_7);
				empty.Y = Math.Min(empty.Y, this.int_8 - 4);
				float num2 = (float)(this.belowLayout.Bounds.Bottom - this.aboveLayout.Bounds.Top - 4);
				this.aboveSize = (float)(empty.Y - this.aboveLayout.Bounds.Top) / num2 * this.float_2;
				this.belowSize = this.float_2 - this.aboveSize;
			}
			base.method_1(new Rectangle(this.container.PointToScreen(empty.Location), empty.Size), false);
			Cursor.Current = SplitLayout.SplitMode != Orientation.Horizontal ? Cursors.VSplit : Cursors.HSplit;
		}

		public SplitLayoutSystem SplitLayout { get; }

	    public event SplittingManagerFinishedEventHandler SplittingManagerFinished;

		private DockContainer container;

		private float aboveSize;

		private float belowSize;

		private float float_2;

		internal const int int_6 = 25;

		private int int_7;

		private int int_8;

		private LayoutSystemBase aboveLayout;

		private LayoutSystemBase belowLayout;

		private Point startPoint = Point.Empty;

	    //private Class10.SplittingManagerFinishedEventHandler splittingManagerFinishedEventHandler_0;

		public delegate void SplittingManagerFinishedEventHandler(LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, float aboveSize, float belowSize);
	}
}
