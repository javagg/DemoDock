using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using TD.Util;

namespace TD.SandDock
{
	internal class PopupContainer : Control
	{
		public PopupContainer(AutoHideBar bar)
		{
			this.control0_0 = bar;
			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.Selectable, false);
		    this.class0_0 = new Class0(this) {Boolean_0 = false};
		    this.class0_0.Event_0 += this.method_4;
			this.BackColor = SystemColors.Control;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				if (base.ContainsFocus && this.control0_0.Manager.OwnerForm != null && this.control0_0.Manager.OwnerForm.IsMdiContainer && this.control0_0.Manager.OwnerForm.ActiveMdiChild != null)
				{
					this.control0_0.Manager.OwnerForm.ActiveControl = this.control0_0.Manager.OwnerForm.ActiveMdiChild;
				}
				this.control0_0.method_6(true);
				this.control0_0 = null;
				this.ControlLayoutSystem_0 = null;
				if (this.class0_0 != null)
				{
					this.class0_0.Dispose();
					this.class0_0 = null;
				}
				if (this.class9_0 != null)
				{
					this.method_1();
				}
			}
			base.Dispose(disposing);
		}

		private void method_0(Point point_0)
		{
			this.class9_0 = new ResizingManager(this.control0_0, this, point_0);
			this.class9_0.Event_0 += this.method_2;
			this.class9_0.ResizingManagerFinished += this.method_3;
		}

		private void method_1()
		{
			this.class9_0.Event_0 -= this.method_2;
			this.class9_0.ResizingManagerFinished -= this.method_3;
			this.class9_0 = null;
		}

		private void method_2(object sender, EventArgs e)
		{
			this.method_1();
		}

		private void method_3(int int_0)
		{
			this.method_1();
			this.Int32_0 = int_0;
		}

		private string method_4(Point point_0)
		{
			if (this.rectangle_0.Contains(point_0) && this.controlLayoutSystem_0 != null)
			{
				return this.controlLayoutSystem_0.vmethod_5(point_0);
			}
			return "";
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
		    this.controlLayoutSystem_0?.vmethod_9();
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this.controlLayoutSystem_0 != null)
			{
				this.rectangle_0 = base.ClientRectangle;
				if (!this.Boolean_1)
				{
					this.rectangle_1 = Rectangle.Empty;
				}
				else
				{
					switch (this.control0_0.Dock)
					{
					case DockStyle.Top:
						this.rectangle_1 = new Rectangle(this.rectangle_0.X, this.rectangle_0.Bottom - 4, this.rectangle_0.Width, 4);
						this.rectangle_0.Height = this.rectangle_0.Height - 4;
						break;
					case DockStyle.Bottom:
						this.rectangle_1 = new Rectangle(this.rectangle_0.X, this.rectangle_0.Y, this.rectangle_0.Width, 4);
						this.rectangle_0.Y = this.rectangle_0.Y + 4;
						this.rectangle_0.Height = this.rectangle_0.Height - 4;
						break;
					case DockStyle.Left:
						this.rectangle_1 = new Rectangle(this.rectangle_0.Right - 4, this.rectangle_0.Y, 4, this.rectangle_0.Height);
						this.rectangle_0.Width = this.rectangle_0.Width - 4;
						break;
					case DockStyle.Right:
						this.rectangle_1 = new Rectangle(this.rectangle_0.X, this.rectangle_0.Y, 4, this.rectangle_0.Height);
						this.rectangle_0.X = this.rectangle_0.X + 4;
						this.rectangle_0.Width = this.rectangle_0.Width - 4;
						break;
					default:
						this.rectangle_1 = Rectangle.Empty;
						break;
					}
				}
				this.controlLayoutSystem_0.LayoutCollapsed(this.control0_0.Manager.Renderer, this.rectangle_0);
				base.Invalidate();
			}
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
		    this.controlLayoutSystem_0?.vmethod_9();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.rectangle_1.Contains(e.X, e.Y))
			{
				if (e.Button == MouseButtons.Left)
				{
					this.method_0(new Point(e.X, e.Y));
					return;
				}
			}
			if (this.rectangle_0.Contains(e.X, e.Y))
			{
				this.controlLayoutSystem_0?.OnMouseDown(e);
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
		    this.controlLayoutSystem_0?.OnMouseLeave();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!this.rectangle_1.Contains(e.X, e.Y) && this.class9_0 == null)
			{
				Cursor.Current = Cursors.Default;
			}
			else if (this.control0_0.Dock != DockStyle.Left && this.control0_0.Dock != DockStyle.Right)
			{
				Cursor.Current = Cursors.HSplit;
			}
			else
			{
				Cursor.Current = Cursors.VSplit;
			}
			if (base.Capture && this.class9_0 != null)
			{
				this.class9_0.OnMouseMove(new Point(e.X, e.Y));
				return;
			}
			if (this.rectangle_0.Contains(e.X, e.Y))
			{
				this.controlLayoutSystem_0?.OnMouseMove(e);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this.class9_0 != null && e.Button == MouseButtons.Left)
			{
				this.class9_0.Commit();
				return;
			}
			if (this.rectangle_0.Contains(e.X, e.Y))
			{
				this.controlLayoutSystem_0?.OnMouseUp(e);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			this.control0_0.Manager.Renderer.StartRenderSession(HotkeyPrefix.None);
		    this.controlLayoutSystem_0?.vmethod_4(this.control0_0.Manager.Renderer, e.Graphics, this.Font);
		    if (this.Boolean_1)
			{
				this.control0_0.Manager.Renderer.DrawSplitter(null, this, e.Graphics, this.rectangle_1, (this.control0_0.Dock == DockStyle.Top || this.control0_0.Dock == DockStyle.Bottom) ? Orientation.Horizontal : Orientation.Vertical);
			}
			this.control0_0.Manager.Renderer.FinishRenderSession();
		}

		public bool Boolean_0 => this.class9_0 != null;

	    private bool Boolean_1 => this.control0_0.Manager.AllowDockContainerResize;

	    public ControlLayoutSystem ControlLayoutSystem_0
		{
			get
			{
				return this.controlLayoutSystem_0;
			}
			set
			{
				this.controlLayoutSystem_0 = value;
				PerformLayout();
			}
		}

		public int Int32_0
		{
			get {
			    return this.control0_0.Dock != DockStyle.Left && this.control0_0.Dock != DockStyle.Right
			        ? this.rectangle_0.Height
			        : this.rectangle_0.Width;
			}
		    set
			{
				Rectangle bounds = base.Bounds;
				int num = value;
				if (this.Boolean_1)
				{
					num += 4;
				}
				switch (this.control0_0.Dock)
				{
				case DockStyle.Top:
					bounds.Height = num;
					break;
				case DockStyle.Bottom:
					bounds.Y = bounds.Bottom - num;
					bounds.Height = num;
					break;
				case DockStyle.Left:
					bounds.Width = num;
					break;
				case DockStyle.Right:
					bounds.X = bounds.Right - num;
					bounds.Width = num;
					break;
				}
				base.Bounds = bounds;
				this.ControlLayoutSystem_0.Int32_0 = value;
			}
		}

		private Class0 class0_0;

		private ResizingManager class9_0;

		private AutoHideBar control0_0;

		private ControlLayoutSystem controlLayoutSystem_0;

		private Rectangle rectangle_0;

		private Rectangle rectangle_1;
	}
}
