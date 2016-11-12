using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
	internal class AutoHideBar : Control
	{
		public AutoHideBar()
		{
			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.Selectable, false);
			this.LayoutSystems = new LayoutSystemCollection(this);
		    this.timer_0 = new Timer {Interval = SystemInformation.DoubleClickTime};
		    this.timer_0.Tick += this.timer_0_Tick;
		    this.timer_1 = new Timer {Interval = 800};
		    this.timer_1.Tick += this.timer_1_Tick;
			Visible = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.method_6(true);
				this.timer_0.Tick -= this.timer_0_Tick;
				this.timer_0.Dispose();
				this.timer_0 = null;
				this.timer_1.Tick -= this.timer_1_Tick;
				this.timer_1.Dispose();
				this.timer_1 = null;
				if (this.control1_0 != null)
				{
					this.control1_0.Dispose();
					this.control1_0 = null;
				}
				this.LayoutSystems.Clear();
			}
			base.Dispose(disposing);
		}

		internal void method_0(ControlLayoutSystem controlLayoutSystem_1)
		{
			this.method_2();
		    if (LayoutSystem == controlLayoutSystem_1)
		        control1_0.PerformLayout();
		}

		internal void PropagateNewRenderer()
		{
			this.method_2();
		}

		private void method_2()
		{
			int num = 0;
			if (LayoutSystem != null && !this.LayoutSystems.Contains(LayoutSystem))
			{
				this.method_6(true);
			}
		    if (Manager == null) return;

		    var renderer = Manager.Renderer;
		    using (var graphics = CreateGraphics())
		    {
		        foreach (ControlLayoutSystem controlLayoutSystem in this.LayoutSystems)
		        {
		            num += 3;
		            int num2 = 0;
		            if (renderer.TabTextDisplay == TabTextDisplayMode.SelectedTab)
		            {
		                foreach (DockControl dockControl in controlLayoutSystem.Controls)
		                {
		                    int num3;
		                    if (!this.Boolean_0)
		                    {
		                        num3 = (int)Math.Ceiling((double)graphics.MeasureString(dockControl.TabText, this.Font, 2147483647, EverettRenderer.StringFormat_0).Width);
		                    }
		                    else
		                    {
		                        num3 = (int)Math.Ceiling((double)graphics.MeasureString(dockControl.TabText, this.Font, 2147483647, EverettRenderer.StringFormat_1).Height);
		                    }
		                    if (num3 > num2)
		                    {
		                        num2 = num3;
		                    }
		                }
		            }
		            foreach (DockControl dockControl2 in controlLayoutSystem.Controls)
		            {
		                Rectangle rectangle_ = new Rectangle(-1, -1, this.Int32_0 - 2, this.Int32_0 - 2);
		                switch (this.Dock)
		                {
		                    case DockStyle.Bottom:
		                        rectangle_.Offset(0, 3);
		                        break;
		                    case DockStyle.Right:
		                        rectangle_.Offset(3, 0);
		                        break;
		                }
		                int num4 = 23;
		                if (renderer.TabTextDisplay != TabTextDisplayMode.AllTabs)
		                {
		                    if (controlLayoutSystem.SelectedControl == dockControl2)
		                    {
		                        num4 += num2 + 16;
		                    }
		                }
		                else
		                {
		                    if (!this.Boolean_0)
		                    {
		                        num4 += (int)Math.Ceiling((double)graphics.MeasureString(dockControl2.TabText, this.Font, 2147483647, EverettRenderer.StringFormat_0).Width);
		                    }
		                    else
		                    {
		                        num4 += (int)Math.Ceiling((double)graphics.MeasureString(dockControl2.TabText, this.Font, 2147483647, EverettRenderer.StringFormat_1).Height);
		                    }
		                    num4 += 3;
		                }
		                if (this.Boolean_0)
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
		                dockControl2.rectangle_1 = rectangle_;
		            }
		            num += 10;
		        }
		    }
		    Visible = (this.LayoutSystems.Count != 0);
		    Invalidate();
		}

		private DockControl method_3(Point p)
		{
		    return LayoutSystems.Cast<ControlLayoutSystem>()
		        .SelectMany(layoutSystem => layoutSystem.Controls.Cast<DockControl>(),(controlLayoutSystem, dockControl) => new {controlLayoutSystem, dockControl})
		        .Select(@t => new {@t, rectangle_ = @t.dockControl.rectangle_1})
		        .Where(@t => @t.rectangle_.Contains(p))
		        .Select(@t => @t.@t.dockControl).FirstOrDefault();
		}

		private void method_4(PopupContainer control1_1, Rectangle rectangle_1, Rectangle rectangle_2)
		{
			this.bool_0 = true;
			try
			{
				float num = (float)(rectangle_2.X - rectangle_1.X);
				float num2 = (float)(rectangle_2.Y - rectangle_1.Y);
				float num3 = (float)(rectangle_2.Width - rectangle_1.Width);
				float num4 = (float)(rectangle_2.Height - rectangle_1.Height);
				int tickCount = Environment.TickCount;
				while (Environment.TickCount < tickCount + 100)
				{
					float num5 = (float)(Environment.TickCount - tickCount) / 100f;
					float num6 = (float)rectangle_1.X + num * num5;
					float num7 = (float)rectangle_1.Y + num2 * num5;
					float num8 = (float)rectangle_1.Width + num3 * num5;
					float num9 = (float)rectangle_1.Height + num4 * num5;
					Rectangle rectangle = new Rectangle((int)num6, (int)num7, (int)num8, (int)num9);
					control1_1.SetBounds(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, BoundsSpecified.All);
					Application.DoEvents();
				}
			}
			finally
			{
				this.bool_0 = false;
			}
		}

		private bool method_5()
		{
			return !Class20.Boolean_0;
		}

		internal void method_6(bool bool_1)
		{
		    if (LayoutSystem == null) return;
		    var control = this.control1_0;
			bool_1 = bool_1 || !this.method_5();
			this.timer_1.Enabled = false;
			if (!bool_1)
			{
				Rectangle rectangle_;
				this.method_8(LayoutSystem.PopupSize, out rectangle_);
				control.SuspendLayout();
				this.method_4(control, control.Bounds, rectangle_);
				control.ResumeLayout();
			}
			var controlLayoutSystem = LayoutSystem;
			LayoutSystem = null;
			var array = new Control[control.Controls.Count];
			control.Controls.CopyTo(array, 0);
		    foreach (var c in array)
		        LayoutUtilities.smethod_8(c);
		    control.Dispose();
		    controlLayoutSystem?.SelectedControl?.OnAutoHidePopupClosed(EventArgs.Empty);
		}

		internal void method_7(DockControl dockControl_0, bool bool_1, bool bool_2)
		{
		    if (dockControl_0.LayoutSystem == LayoutSystem && dockControl_0.LayoutSystem.SelectedControl == dockControl_0)
		    {
		        if (bool_2)
		        {
		            dockControl_0.Activate();
		        }
		        return;
		    }
		    bool_1 = (bool_1 || !this.method_5());
			dockControl_0.LayoutSystem.SelectedControl = dockControl_0;
			if (dockControl_0.LayoutSystem.SelectedControl == dockControl_0)
			{
				try
				{
					if (this.LayoutSystem != dockControl_0.LayoutSystem)
					{
						this.method_6(true);
						Rectangle rectangle;
						this.rectangle_0 = this.method_8(dockControl_0.LayoutSystem.PopupSize, out rectangle);
						PopupContainer control = new PopupContainer(this);
						foreach (DockControl dockControl in dockControl_0.LayoutSystem.Controls)
						{
							if (dockControl.Parent != null)
							{
								LayoutUtilities.smethod_8(dockControl);
							}
							dockControl.Parent = control;
						}
						control.ControlLayoutSystem_0 = dockControl_0.LayoutSystem;
						control.Visible = false;
						base.Parent.Controls.Add(control);
						control.Bounds = this.rectangle_0;
						control.SuspendLayout();
						control.Bounds = rectangle;
						control.Visible = true;
						control.BringToFront();
						if (!bool_1)
						{
							this.method_4(control, rectangle, this.rectangle_0);
						}
						control.Bounds = this.rectangle_0;
						control.ResumeLayout();
						if (!control.IsDisposed && control.Parent != null)
						{
							this.control1_0 = control;
							this.LayoutSystem = dockControl_0.LayoutSystem;
							this.timer_1.Enabled = true;
							dockControl_0.OnAutoHidePopupOpened(EventArgs.Empty);
						}
					}
				}
				finally
				{
					if (bool_2 && this.LayoutSystem == dockControl_0.LayoutSystem)
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
			switch (this.Dock)
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
			this.control1_0.Invalidate(bounds);
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);
			var dockControl = this.method_3(PointToClient(new Point(drgevent.X, drgevent.Y)));
		    if (dockControl != null)
		        this.method_7(dockControl, true, true);
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
		    if (LayoutSystem != null)
		        BeginInvoke(new Delegate1(this.method_6), true);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.bool_0)
			{
				return;
			}
			var dockControl = this.method_3(new Point(e.X, e.Y));
		    if (dockControl != null)
		        this.method_7(dockControl, false, true);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
		    if (this.bool_0) return;

		    var left = new Point(e.X, e.Y);
		    if (left != this.point_0)
		    {
		        this.point_0 = left;
		        this.timer_0.Enabled = false;
		        this.timer_0.Enabled = true;
		    }
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (Manager == null)
			{
				base.OnPaint(e);
				return;
			}
			DockSide dockSide = DockSide.Right;
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
			foreach (ControlLayoutSystem controlLayoutSystem in this.LayoutSystems)
			{
				foreach (DockControl dockControl in controlLayoutSystem.Controls)
				{
					DrawItemState drawItemState = DrawItemState.Default;
					if (dockControl == controlLayoutSystem.SelectedControl)
					{
						drawItemState |= DrawItemState.Selected;
					}
					string text = dockControl.TabText;
					if (Manager.Renderer.TabTextDisplay == TabTextDisplayMode.SelectedTab)
					{
						if (dockControl != controlLayoutSystem.SelectedControl)
						{
							text = "";
						}
					}
					Manager.Renderer.DrawCollapsedTab(e.Graphics, dockControl.rectangle_1, dockSide, dockControl.Image_0, text, this.Font, dockControl.BackColor, dockControl.ForeColor, drawItemState, this.Boolean_0);
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
		    if (LayoutSystem != null)
		        BeginInvoke(new Delegate1(this.method_6), true);
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			this.timer_0.Enabled = false;
		    if (this.bool_0) return;
		    var dockControl = this.method_3(PointToClient(Cursor.Position));
		    if (dockControl != null)
		        this.method_7(dockControl, false, false);
		}

		private void timer_1_Tick(object sender, EventArgs e)
		{
			bool flag = this.control1_0.ClientRectangle.Contains(this.control1_0.PointToClient(Cursor.Position));
			bool flag2 = base.ClientRectangle.Contains(PointToClient(Cursor.Position));
			if (!flag && !flag2 && !this.control1_0.Boolean_0 && !this.control1_0.ContainsFocus)
			{
				this.method_6(false);
			}
		}

		internal bool Boolean_0 => Dock == DockStyle.Left || Dock == DockStyle.Right;

	    public LayoutSystemCollection LayoutSystems { get; }

	    public ControlLayoutSystem LayoutSystem { get; private set; }

	    public Control Control_0 => this.control1_0;

	    protected override Size DefaultSize => new Size(this.Int32_0, this.Int32_0);

	    private int Int32_0 => Math.Max(DefaultFont.Height, 16) + 6;

	    public int Int32_1
		{
			get
			{
				return this.control1_0.Int32_0;
			}
			set
			{
			    if (this.control1_0.Int32_0 != value)
			        this.control1_0.Int32_0 = value;
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
			    this.method_2();
			}
		}

		private bool bool_0;

	    private PopupContainer control1_0;

	    private Point point_0;

		private Rectangle rectangle_0;

		private SandDockManager _manager;

		private Timer timer_0;

		private Timer timer_1;

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
