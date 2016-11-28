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
		    _toolTips0 = new Tooltip(this) {DropShadow = false};
		    _toolTips0.GetTooltipText += OnGetTooltipText;
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
				LayoutSystem = null;
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
			class9_0.Committed += OnCommitted;
		}

		private void method_1()
		{
			class9_0.Cancalled -= method_2;
			class9_0.Committed -= OnCommitted;
			class9_0 = null;
		}

		private void method_2(object sender, EventArgs e)
		{
			method_1();
		}

		private void OnCommitted(int size)
		{
			method_1();
			PopupSize = size;
		}

		private string OnGetTooltipText(Point point_0)
		{
		    return _popupBounds.Contains(point_0) && _layoutSystem != null
		        ? _layoutSystem.GetDockButtonTextAt(point_0) : "";
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
		    _layoutSystem?.OnLeave();
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
		    if (_layoutSystem == null) return;

		    _popupBounds = ClientRectangle;
		    if (!Resizable)
		    {
		        rectangle_1 = Rectangle.Empty;
		    }
		    else
		    {
		        switch (_autoHideBar.Dock)
		        {
		            case DockStyle.Top:
		                rectangle_1 = new Rectangle(_popupBounds.X, _popupBounds.Bottom - 4, _popupBounds.Width, 4);
		                _popupBounds.Height = _popupBounds.Height - 4;
		                break;
		            case DockStyle.Bottom:
		                rectangle_1 = new Rectangle(_popupBounds.X, _popupBounds.Y, _popupBounds.Width, 4);
		                _popupBounds.Y = _popupBounds.Y + 4;
		                _popupBounds.Height = _popupBounds.Height - 4;
		                break;
		            case DockStyle.Left:
		                rectangle_1 = new Rectangle(_popupBounds.Right - 4, _popupBounds.Y, 4, _popupBounds.Height);
		                _popupBounds.Width = _popupBounds.Width - 4;
		                break;
		            case DockStyle.Right:
		                rectangle_1 = new Rectangle(_popupBounds.X, _popupBounds.Y, 4, _popupBounds.Height);
		                _popupBounds.X = _popupBounds.X + 4;
		                _popupBounds.Width = _popupBounds.Width - 4;
		                break;
		            default:
		                rectangle_1 = Rectangle.Empty;
		                break;
		        }
		    }
		    _layoutSystem.LayoutCollapsed(_autoHideBar.Manager.Renderer, _popupBounds);
		    Invalidate();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
		    _layoutSystem?.OnLeave();
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
		        if (_popupBounds.Contains(e.X, e.Y))
		        {
		            _layoutSystem?.OnMouseDown(e);
		        }
		    }
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
		    _layoutSystem?.OnMouseLeave();
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
			if (_popupBounds.Contains(e.X, e.Y))
			{
				_layoutSystem?.OnMouseMove(e);
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
			if (_popupBounds.Contains(e.X, e.Y))
			{
				_layoutSystem?.OnMouseUp(e);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			_autoHideBar.Manager.Renderer.StartRenderSession(HotkeyPrefix.None);
		    _layoutSystem?.vmethod_4(_autoHideBar.Manager.Renderer, e.Graphics, Font);
		    if (Resizable)
			{
				_autoHideBar.Manager.Renderer.DrawSplitter(null, this, e.Graphics, rectangle_1, (_autoHideBar.Dock == DockStyle.Top || _autoHideBar.Dock == DockStyle.Bottom) ? Orientation.Horizontal : Orientation.Vertical);
			}
			_autoHideBar.Manager.Renderer.FinishRenderSession();
		}

        [Naming(NamingType.FromOldVersion)]
        public bool IsSplitting => class9_0 != null;

        [Naming(NamingType.FromOldVersion)]
	    private bool Resizable => _autoHideBar.Manager.AllowDockContainerResize;
        [Naming(NamingType.FromOldVersion)]
        public ControlLayoutSystem LayoutSystem
		{
			get
			{
				return _layoutSystem;
			}
			set
			{
				_layoutSystem = value;
				PerformLayout();
			}
		}

		public int PopupSize
		{
			get {
			    return _autoHideBar.Dock != DockStyle.Left && _autoHideBar.Dock != DockStyle.Right
			        ? _popupBounds.Height
			        : _popupBounds.Width;
			}
		    set
			{
				var bounds = Bounds;
				int num = value;
			    if (Resizable)
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
				LayoutSystem.PopupSize = value;
			}
		}

		private Tooltip _toolTips0;

		private ResizingManager class9_0;

		private AutoHideBar _autoHideBar;

		private ControlLayoutSystem _layoutSystem;

		private Rectangle _popupBounds;

		private Rectangle rectangle_1;
	}
}
