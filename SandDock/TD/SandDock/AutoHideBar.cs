using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using TD.SandDock.Rendering;
using TD.Util;

namespace TD.SandDock
{
    /*
    internal class Class20
    {
        //private const int SM_REMOTESESSION = 0x1000;
        public static bool IsRemoteSession => Native.GetSystemMetrics(WMConstants.SM_REMOTESESSION) != 0;
        public static Color GradientActiveCaptionColor => ColorTranslator.FromWin32(Native.GetSysColor(WMConstants.COLOR_GRADIENTACTIVECAPTION));
       // public const int int_0 = -1;

      //  public const int int_2 = 33;
    }
    */

        [Naming(NamingType.FromOldVersion)]
    internal class AutoHideBar : Control
	{
		public AutoHideBar()
		{
			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.Selectable, false);
			LayoutSystems = new LayoutSystemCollection(this);
		    timer_0 = new Timer {Interval = SystemInformation.DoubleClickTime};
		    timer_0.Tick += OnTimerTick;
		    collapsingTimer = new Timer {Interval = 800};
		    collapsingTimer.Tick += OnCollapsingTimerTick;
			Visible = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				method_6(true);
				timer_0.Tick -= OnTimerTick;
				timer_0.Dispose();
				timer_0 = null;
				collapsingTimer.Tick -= OnCollapsingTimerTick;
				collapsingTimer.Dispose();
				collapsingTimer = null;
				if (_popupContainer != null)
				{
					_popupContainer.Dispose();
					_popupContainer = null;
				}
				LayoutSystems.Clear();
			}
			base.Dispose(disposing);
		}

		internal void method_0(ControlLayoutSystem layoutSystem)
		{
			method_2();
		    if (ShowingLayoutSystem == layoutSystem)
		        _popupContainer.PerformLayout();
		}

		internal void PropagateNewRenderer()
		{
			method_2();
		}

		private void method_2()
		{
			int num = 0;
			if (ShowingLayoutSystem != null && !LayoutSystems.Contains(ShowingLayoutSystem))
			{
				method_6(true);
			}
		    if (Manager == null) return;

		    var renderer = Manager.Renderer;
		    using (var graphics = CreateGraphics())
		    {
		        foreach (ControlLayoutSystem controlLayoutSystem in LayoutSystems)
		        {
		            num += 3;
		            int num2 = 0;
		            if (renderer.TabTextDisplay == TabTextDisplayMode.SelectedTab)
		            {
		                num2 = controlLayoutSystem.Controls.Cast<DockControl>()
		                    .Select(dockControl =>
		                            !Vertical
		                                ? (int)
		                                Math.Ceiling(
		                                    graphics.MeasureString(dockControl.TabText, Font, int.MaxValue,
		                                        EverettRenderer.StandardStringFormat).Width)
		                                : (int)
		                                Math.Ceiling(
		                                    graphics.MeasureString(dockControl.TabText, Font, int.MaxValue,
		                                        EverettRenderer.StandardVerticalStringFormat).Height)).Concat(new[] {num2}).Max();
		            }
		            foreach (DockControl dockControl2 in controlLayoutSystem.Controls)
		            {
		                Rectangle rectangle_ = new Rectangle(-1, -1, AutoHideSize - 2, AutoHideSize - 2);
		                switch (Dock)
		                {
		                    case DockStyle.Bottom:
		                        rectangle_.Offset(0, 3);
		                        break;
		                    case DockStyle.Right:
		                        rectangle_.Offset(3, 0);
		                        break;
		                }
		                var num4 = 23;
		                if (renderer.TabTextDisplay != TabTextDisplayMode.AllTabs)
		                {
		                    if (controlLayoutSystem.SelectedControl == dockControl2)
		                    {
		                        num4 += num2 + 16;
		                    }
		                }
		                else
		                {
		                    num4 += !Vertical
		                        ? (int)
		                        Math.Ceiling(
		                            graphics.MeasureString(dockControl2.TabText, Font, int.MaxValue,EverettRenderer.StandardStringFormat).Width)
		                        : (int)
		                        Math.Ceiling(
		                            graphics.MeasureString(dockControl2.TabText, Font, int.MaxValue,EverettRenderer.StandardVerticalStringFormat).Height);
		                    num4 += 3;
		                }
		                if (Vertical)
		                {
		                    rectangle_.Offset(0, num);
		                    rectangle_.Height = num4;
		                    num += num4;
		                }
		                else
		                {
		                    rectangle_.Offset(num, 0);
		                    rectangle_.Width = num4;
		                    num += num4;
		                }
		                dockControl2.CollapsedBounds = rectangle_;
		            }
		            num += 10;
		        }
		    }
		    Visible = (LayoutSystems.Count != 0);
		    Invalidate();
		}

		private DockControl method_3(Point p)
		{
		    return LayoutSystems.Cast<ControlLayoutSystem>()
		        .SelectMany(layoutSystem => layoutSystem.Controls.Cast<DockControl>(),(controlLayoutSystem, dockControl) => new {controlLayoutSystem, dockControl})
		        .Select(t => new {t, rect = t.dockControl.CollapsedBounds})
		        .Where(t => t.rect.Contains(p))
		        .Select(t => t.t.dockControl).FirstOrDefault();
		}

		private void method_4(PopupContainer container, Rectangle bounds1, Rectangle bounds2)
		{
			bool_0 = true;
			try
			{
                while (Environment.TickCount < Environment.TickCount + 100)
				{
					var ratio = (Environment.TickCount - Environment.TickCount) / 100f;
					var x = bounds1.X + (bounds2.X - bounds1.X) * ratio;
					var y = bounds1.Y + (bounds2.Y - bounds1.Y) * ratio;
					var w = bounds1.Width + (bounds2.Width - bounds1.Width) * ratio;
					var h = bounds1.Height + (bounds2.Height - bounds1.Height) * ratio;
					var r = new Rectangle((int)x, (int)y, (int)w, (int)h);
					container.SetBounds(r.X, r.Y, r.Width, r.Height, BoundsSpecified.All);
					Application.DoEvents();
				}
			}
			finally
			{
				bool_0 = false;
			}
		}

		private static bool NotRemoteSession() => Native.IsMono() || Native.GetSystemMetrics(WMConstants.SM_REMOTESESSION) != 0;

	    internal void method_6(bool bool_1)
		{
		    if (ShowingLayoutSystem == null) return;
		    var control = _popupContainer;
			bool_1 = bool_1 || !NotRemoteSession();
			collapsingTimer.Enabled = false;
			if (!bool_1)
			{
				Rectangle rectangle_;
				method_8(ShowingLayoutSystem.PopupSize, out rectangle_);
				control.SuspendLayout();
				method_4(control, control.Bounds, rectangle_);
				control.ResumeLayout();
			}
			var controlLayoutSystem = ShowingLayoutSystem;
			ShowingLayoutSystem = null;
			var array = new Control[control.Controls.Count];
			control.Controls.CopyTo(array, 0);
		    foreach (var c in array)
		        LayoutUtilities.smethod_8(c);
		    control.Dispose();
		    controlLayoutSystem?.SelectedControl?.OnAutoHidePopupClosed(EventArgs.Empty);
		}

		internal void method_7(DockControl dockControl_0, bool bool_1, bool bool_2)
		{
		    if (dockControl_0.LayoutSystem == ShowingLayoutSystem && dockControl_0.LayoutSystem.SelectedControl == dockControl_0)
		    {
		        if (bool_2)
		        {
		            dockControl_0.Activate();
		        }
		        return;
		    }
		    bool_1 = bool_1 || !NotRemoteSession();
			dockControl_0.LayoutSystem.SelectedControl = dockControl_0;
			if (dockControl_0.LayoutSystem.SelectedControl == dockControl_0)
			{
				try
				{
					if (ShowingLayoutSystem != dockControl_0.LayoutSystem)
					{
						method_6(true);
						Rectangle rectangle;
						rectangle_0 = method_8(dockControl_0.LayoutSystem.PopupSize, out rectangle);
						PopupContainer control = new PopupContainer(this);
						foreach (DockControl dockControl in dockControl_0.LayoutSystem.Controls)
						{
							if (dockControl.Parent != null)
							{
								LayoutUtilities.smethod_8(dockControl);
							}
							dockControl.Parent = control;
						}
						control.LayoutSystem = dockControl_0.LayoutSystem;
						control.Visible = false;
						Parent.Controls.Add(control);
						control.Bounds = rectangle_0;
						control.SuspendLayout();
						control.Bounds = rectangle;
						control.Visible = true;
						control.BringToFront();
						if (!bool_1)
						{
							method_4(control, rectangle, rectangle_0);
						}
						control.Bounds = rectangle_0;
						control.ResumeLayout();
						if (!control.IsDisposed && control.Parent != null)
						{
							_popupContainer = control;
							ShowingLayoutSystem = dockControl_0.LayoutSystem;
							collapsingTimer.Enabled = true;
							dockControl_0.OnAutoHidePopupOpened(EventArgs.Empty);
						}
					}
				}
				finally
				{
					if (bool_2 && ShowingLayoutSystem == dockControl_0.LayoutSystem)
					{
						dockControl_0.Activate();
					}
				}
			}
		}

		private Rectangle method_8(int int_0, out Rectangle rectangle_1)
		{
			var bounds = Bounds;
			switch (Dock)
			{
			case DockStyle.Top:
				bounds = new Rectangle(bounds.Left, bounds.Bottom, bounds.Width, 0);
				break;
			case DockStyle.Bottom:
				bounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width, 0);
				break;
			case DockStyle.Left:
				bounds = new Rectangle(bounds.Right, bounds.Top, 0, bounds.Height);
				break;
			case DockStyle.Right:
				bounds = new Rectangle(bounds.Left, bounds.Top, 0, bounds.Height);
				break;
			}
			rectangle_1 = bounds;
			int num = int_0 + 4;
			switch (Dock)
			{
			case DockStyle.Top:
				bounds.Height = num;
				break;
			case DockStyle.Bottom:
				bounds.Offset(0, -num);
				bounds.Height = num;
				break;
			case DockStyle.Left:
				bounds.Width = num;
				break;
			case DockStyle.Right:
				bounds.Offset(-num, 0);
				bounds.Width = num;
				break;
			}
			return bounds;
		}

		public void method_9(Rectangle bounds)
		{
			_popupContainer.Invalidate(bounds);
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);
			var dockControl = method_3(PointToClient(new Point(drgevent.X, drgevent.Y)));
		    if (dockControl != null)
		        method_7(dockControl, true, true);
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
		    if (ShowingLayoutSystem != null)
		        BeginInvoke(new Delegate1(method_6), true);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
		    if (bool_0)
		        return;
		    var dockControl = method_3(new Point(e.X, e.Y));
		    if (dockControl != null)
		        method_7(dockControl, false, true);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
		    if (bool_0) return;

		    var left = new Point(e.X, e.Y);
		    if (left != point_0)
		    {
		        point_0 = left;
		        timer_0.Enabled = false;
		        timer_0.Enabled = true;
		    }
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (Manager == null)
			{
				base.OnPaint(e);
				return;
			}
			var dockSide = DockSide.Right;
			switch (Dock)
			{
			case DockStyle.Top:
				dockSide = DockSide.Top;
				break;
			case DockStyle.Bottom:
				dockSide = DockSide.Bottom;
				break;
			case DockStyle.Left:
				dockSide = DockSide.Left;
				break;
			}
			Manager.Renderer.StartRenderSession(HotkeyPrefix.None);
			foreach (ControlLayoutSystem controlLayoutSystem in LayoutSystems)
			{
				foreach (DockControl dockControl in controlLayoutSystem.Controls)
				{
					var state = DrawItemState.Default;
				    if (dockControl == controlLayoutSystem.SelectedControl)
				        state |= DrawItemState.Selected;
				    var text = dockControl.TabText;
				    if (Manager.Renderer.TabTextDisplay == TabTextDisplayMode.SelectedTab && dockControl != controlLayoutSystem.SelectedControl)
				        text = "";

				    Manager.Renderer.DrawCollapsedTab(e.Graphics, dockControl.CollapsedBounds, dockSide, dockControl.CollapsedImage, text, Font, dockControl.BackColor, dockControl.ForeColor, state, Vertical);
				}
			}
			Manager.Renderer.FinishRenderSession();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		    if (Manager?.DockSystemContainer != null)
		        Manager.Renderer.DrawAutoHideBarBackground(Manager.DockSystemContainer, this, pevent.Graphics, ClientRectangle);
		    else
		        base.OnPaintBackground(pevent);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
		    if (ShowingLayoutSystem != null)
		        BeginInvoke(new Delegate1(method_6), true);
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			timer_0.Enabled = false;
		    if (bool_0) return;
		    var dockControl = method_3(PointToClient(Cursor.Position));
		    if (dockControl != null)
		        method_7(dockControl, false, false);
		}

		private void OnCollapsingTimerTick(object sender, EventArgs e)
		{
			bool inPopupContainer = _popupContainer.ClientRectangle.Contains(_popupContainer.PointToClient(Cursor.Position));
			bool inMyself = ClientRectangle.Contains(PointToClient(Cursor.Position));
		    if (!inPopupContainer && !inMyself && !_popupContainer.IsSplitting && !_popupContainer.ContainsFocus)
		        method_6(false);
		}

        [Naming(NamingType.FromOldVersion)]
		internal bool Vertical => Dock == DockStyle.Left || Dock == DockStyle.Right;

	    public LayoutSystemCollection LayoutSystems { get; }

	    public ControlLayoutSystem ShowingLayoutSystem { get; private set; }

        [Naming(NamingType.FromOldVersion)]
        public Control PopupContainer => _popupContainer;

	    protected override Size DefaultSize => new Size(AutoHideSize, AutoHideSize);

        [Naming(NamingType.FromOldVersion)]
        private int AutoHideSize => Math.Max(DefaultFont.Height, 16) + 6;

	    public int PopupSize
		{
			get
			{
				return _popupContainer.PopupSize;
			}
			set
			{
			    if (_popupContainer.PopupSize != value)
			        _popupContainer.PopupSize = value;
			}
		}

		public SandDockManager Manager
		{
			get
			{
				return _manager;
			}
			set
			{
			    _manager?.UnregisterAutoHideBar(this);
			    _manager = value;
			    if (_manager == null) return;
			    _manager.RegisterAutoHideBar(this);
			    method_2();
			}
		}

		private bool bool_0;

	    private PopupContainer _popupContainer;

	    private Point point_0;

		private Rectangle rectangle_0;

		private SandDockManager _manager;

		private Timer timer_0;

		private Timer collapsingTimer;

		internal class LayoutSystemCollection : CollectionBase
		{
			public LayoutSystemCollection(AutoHideBar parent)
			{
				_autoHideBar = parent;
			}

			public bool Contains(ControlLayoutSystem layout) => List.Contains(layout);

		    public int Add(ControlLayoutSystem layout) => List.Add(layout);

		    public void Remove(ControlLayoutSystem layout) => List.Remove(layout);

		    protected override void OnClear()
			{
			    foreach (ControlLayoutSystem layout in this)
			        layout.SetAutoHideBar(null);
			}

			protected override void OnClearComplete()
			{
				_autoHideBar.method_2();
			}

			protected override void OnInsertComplete(int index, object value)
			{
				var layout = (ControlLayoutSystem)value;
                layout.SetAutoHideBar(_autoHideBar);
				_autoHideBar.method_2();
			}

			protected override void OnRemoveComplete(int index, object value)
			{
				var layout = (ControlLayoutSystem)value;
				layout.SetAutoHideBar(null);
				_autoHideBar.method_2();
			}

			public ControlLayoutSystem this[int index] => (ControlLayoutSystem)List[index];

		    private readonly AutoHideBar _autoHideBar;
		}

		private delegate void Delegate1(bool quick);
	}
}
