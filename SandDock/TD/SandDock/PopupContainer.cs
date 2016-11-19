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
			_autoHideBar = bar;
			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.Selectable, false);
		    _toolTips0 = new ToolTips(this) {Boolean_0 = false};
		    _toolTips0.Event_0 += method_4;
			BackColor = SystemColors.Control;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				if (ContainsFocus && _autoHideBar.Manager.OwnerForm != null && _autoHideBar.Manager.OwnerForm.IsMdiContainer && _autoHideBar.Manager.OwnerForm.ActiveMdiChild != null)
				{
					_autoHideBar.Manager.OwnerForm.ActiveControl = _autoHideBar.Manager.OwnerForm.ActiveMdiChild;
				}
				_autoHideBar.method_6(true);
				_autoHideBar = null;
				ControlLayoutSystem_0 = null;
				if (_toolTips0 != null)
				{
					_toolTips0.Dispose();
					_toolTips0 = null;
				}
				if (class9_0 != null)
				{
					method_1();
				}
			}
			base.Dispose(disposing);
		}

		private void method_0(Point point_0)
		{
			class9_0 = new ResizingManager(_autoHideBar, this, point_0);
			class9_0.Cancalled += method_2;
			class9_0.ResizingManagerFinished += OnResizingManagerFinished;
		}

		private void method_1()
		{
			class9_0.Cancalled -= method_2;
			class9_0.ResizingManagerFinished -= OnResizingManagerFinished;
			class9_0 = null;
		}

		private void method_2(object sender, EventArgs e)
		{
			method_1();
		}

		private void OnResizingManagerFinished(int size)
		{
			method_1();
			PopupSize = size;
		}

		private string method_4(Point point_0)
		{
		    return rectangle_0.Contains(point_0) && controlLayoutSystem_0 != null
		        ? controlLayoutSystem_0.vmethod_5(point_0) : "";
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
		    controlLayoutSystem_0?.OnLeave();
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
		    if (controlLayoutSystem_0 == null) return;

		    rectangle_0 = ClientRectangle;
		    if (!Boolean_1)
		    {
		        rectangle_1 = Rectangle.Empty;
		    }
		    else
		    {
		        switch (_autoHideBar.Dock)
		        {
		            case DockStyle.Top:
		                rectangle_1 = new Rectangle(rectangle_0.X, rectangle_0.Bottom - 4, rectangle_0.Width, 4);
		                rectangle_0.Height = rectangle_0.Height - 4;
		                break;
		            case DockStyle.Bottom:
		                rectangle_1 = new Rectangle(rectangle_0.X, rectangle_0.Y, rectangle_0.Width, 4);
		                rectangle_0.Y = rectangle_0.Y + 4;
		                rectangle_0.Height = rectangle_0.Height - 4;
		                break;
		            case DockStyle.Left:
		                rectangle_1 = new Rectangle(rectangle_0.Right - 4, rectangle_0.Y, 4, rectangle_0.Height);
		                rectangle_0.Width = rectangle_0.Width - 4;
		                break;
		            case DockStyle.Right:
		                rectangle_1 = new Rectangle(rectangle_0.X, rectangle_0.Y, 4, rectangle_0.Height);
		                rectangle_0.X = rectangle_0.X + 4;
		                rectangle_0.Width = rectangle_0.Width - 4;
		                break;
		            default:
		                rectangle_1 = Rectangle.Empty;
		                break;
		        }
		    }
		    controlLayoutSystem_0.LayoutCollapsed(_autoHideBar.Manager.Renderer, rectangle_0);
		    Invalidate();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
		    controlLayoutSystem_0?.OnLeave();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
		    base.OnMouseDown(e);
		    if (rectangle_1.Contains(e.X, e.Y) && e.Button == MouseButtons.Left)
		    {
		        method_0(new Point(e.X, e.Y));
		    }
		    else
		    {
		        if (rectangle_0.Contains(e.X, e.Y))
		        {
		            controlLayoutSystem_0?.OnMouseDown(e);
		        }
		    }
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
		    controlLayoutSystem_0?.OnMouseLeave();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!rectangle_1.Contains(e.X, e.Y) && class9_0 == null)
			{
				Cursor.Current = Cursors.Default;
			}
			else if (_autoHideBar.Dock != DockStyle.Left && _autoHideBar.Dock != DockStyle.Right)
			{
				Cursor.Current = Cursors.HSplit;
			}
			else
			{
				Cursor.Current = Cursors.VSplit;
			}
			if (Capture && class9_0 != null)
			{
				class9_0.OnMouseMove(new Point(e.X, e.Y));
				return;
			}
			if (rectangle_0.Contains(e.X, e.Y))
			{
				controlLayoutSystem_0?.OnMouseMove(e);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (class9_0 != null && e.Button == MouseButtons.Left)
			{
				class9_0.Commit();
				return;
			}
			if (rectangle_0.Contains(e.X, e.Y))
			{
				controlLayoutSystem_0?.OnMouseUp(e);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			_autoHideBar.Manager.Renderer.StartRenderSession(HotkeyPrefix.None);
		    controlLayoutSystem_0?.vmethod_4(_autoHideBar.Manager.Renderer, e.Graphics, Font);
		    if (Boolean_1)
			{
				_autoHideBar.Manager.Renderer.DrawSplitter(null, this, e.Graphics, rectangle_1, (_autoHideBar.Dock == DockStyle.Top || _autoHideBar.Dock == DockStyle.Bottom) ? Orientation.Horizontal : Orientation.Vertical);
			}
			_autoHideBar.Manager.Renderer.FinishRenderSession();
		}

		public bool Boolean_0 => class9_0 != null;

	    private bool Boolean_1 => _autoHideBar.Manager.AllowDockContainerResize;

	    public ControlLayoutSystem ControlLayoutSystem_0
		{
			get
			{
				return controlLayoutSystem_0;
			}
			set
			{
				controlLayoutSystem_0 = value;
				PerformLayout();
			}
		}

		public int PopupSize
		{
			get {
			    return _autoHideBar.Dock != DockStyle.Left && _autoHideBar.Dock != DockStyle.Right
			        ? rectangle_0.Height
			        : rectangle_0.Width;
			}
		    set
			{
				var bounds = Bounds;
				int num = value;
			    if (Boolean_1)
			        num += 4;
			    switch (_autoHideBar.Dock)
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
				Bounds = bounds;
				ControlLayoutSystem_0.PopupSize = value;
			}
		}

		private ToolTips _toolTips0;

		private ResizingManager class9_0;

		private AutoHideBar _autoHideBar;

		private ControlLayoutSystem controlLayoutSystem_0;

		private Rectangle rectangle_0;

		private Rectangle rectangle_1;
	}
}
