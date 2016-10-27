using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Class9 : Class6
	{
		public Class9(AutoHideBar bar, PopupContainer popupContainer, Point startPoint) : base(bar, bar.Manager?.DockingHints ?? DockingHints.TranslucentFill, false)
		{
			this.control0_0 = bar;
			this.control1_0 = popupContainer;
			this.point_0 = startPoint;
			int num = bar.Manager?.MinimumDockContainerSize ?? 30;
			int num2 = bar.Manager?.MaximumDockContainerSize ?? 500;
			this.int_6 = popupContainer.Int32_0;
			switch (bar.Dock)
			{
			case DockStyle.Top:
				if (bar.Manager?.DockSystemContainer != null)
				{
					num2 = Math.Max(bar.Manager.DockSystemContainer.Height - popupContainer.Bounds.Top - num, num);
				}
				this.int_7 = startPoint.Y - (this.int_6 - num);
				this.int_8 = startPoint.Y + (num2 - this.int_6);
				break;
			case DockStyle.Bottom:
				if (bar.Manager?.DockSystemContainer != null)
				{
					num2 = Math.Max(popupContainer.Bounds.Bottom - num, num);
				}
				this.int_7 = startPoint.Y - (num2 - this.int_6);
				this.int_8 = startPoint.Y + (this.int_6 - num);
				break;
			case DockStyle.Left:
				if (bar.Manager?.DockSystemContainer != null)
				{
					num2 = Math.Max(bar.Manager.DockSystemContainer.Width - popupContainer.Bounds.Left - num, num);
				}
				this.int_7 = startPoint.X - (this.int_6 - num);
				this.int_8 = startPoint.X + (num2 - this.int_6);
				break;
			case DockStyle.Right:
				if (bar.Manager != null && bar.Manager.DockSystemContainer != null)
				{
					num2 = Math.Max(popupContainer.Bounds.Right - num, num);
				}
				this.int_7 = startPoint.X - (num2 - this.int_6);
				this.int_8 = startPoint.X + (this.int_6 - num);
				break;
			}
			this.OnMouseMove(startPoint);
		}

		public override void Commit()
		{
			base.Commit();
            ResizingManagerFinished?.Invoke(this.int_9);
		}

		public override void OnMouseMove(Point position)
		{
			Rectangle empty = Rectangle.Empty;
			if (!this.control0_0.Boolean_0)
			{
				if (position.Y < this.int_7)
				{
					position.Y = this.int_7;
				}
				if (position.Y > this.int_8)
				{
					position.Y = this.int_8;
				}
				empty = new Rectangle(0, position.Y - 2, this.control1_0.Width, 4);
			}
			else
			{
				if (position.X < this.int_7)
				{
					position.X = this.int_7;
				}
				if (position.X > this.int_8)
				{
					position.X = this.int_8;
				}
				empty = new Rectangle(position.X - 2, 0, 4, this.control1_0.Height);
			}
			switch (this.control0_0.Dock)
			{
			case DockStyle.Top:
				this.int_9 = this.int_6 + (position.Y - this.point_0.Y);
				break;
			case DockStyle.Bottom:
				this.int_9 = this.int_6 + (this.point_0.Y - position.Y);
				break;
			case DockStyle.Left:
				this.int_9 = this.int_6 + (position.X - this.point_0.X);
				break;
			case DockStyle.Right:
				this.int_9 = this.int_6 + (this.point_0.X - position.X);
				break;
			}
			base.method_1(new Rectangle(this.control1_0.PointToScreen(empty.Location), empty.Size), false);
		}

	    public event ResizingManagerFinishedEventHandler ResizingManagerFinished;

		private AutoHideBar control0_0;

		private PopupContainer control1_0;

		private int int_6;

		private int int_7;

		private int int_8;

		private int int_9;

		private Point point_0;

		//private Class9.ResizingManagerFinishedEventHandler resizingManagerFinishedEventHandler_0;

		public delegate void ResizingManagerFinishedEventHandler(int newSize);
	}
}