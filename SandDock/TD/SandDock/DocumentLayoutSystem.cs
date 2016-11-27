using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock.Rendering;
using TD.Util;

namespace TD.SandDock
{
    public enum DocumentOverflowMode
    {
        None,
        Scrollable,
        Menu
    }

    public class DocumentLayoutSystem : ControlLayoutSystem
	{
		public DocumentLayoutSystem()
		{
			_leftScrollButton = new DockButtonInfo();
			_rightScrollButton = new DockButtonInfo();
			_closeButton = new DockButtonInfo();
			_menuButton = new DockButtonInfo();
		    timer_0 = new Timer {Interval = 20};
		    timer_0.Tick += OnTimerTick;
		}

		public DocumentLayoutSystem(int desiredWidth, int desiredHeight) : this()
		{
			WorkingSize = new SizeF(desiredWidth, desiredHeight);
		}

		public DocumentLayoutSystem(SizeF workingSize, DockControl[] windows, DockControl selectedWindow) : this()
		{
			WorkingSize = workingSize;
			Controls.AddRange(windows);
		    if (selectedWindow != null)
		        SelectedControl = selectedWindow;
		}

		[Obsolete("Use the constructor that takes a SizeF instead.")]
		public DocumentLayoutSystem(int desiredWidth, int desiredHeight, DockControl[] controls, DockControl selectedControl) : this(desiredWidth, desiredHeight)
		{
			Controls.AddRange(controls);
		    if (selectedControl != null)
		        SelectedControl = selectedControl;
		}

		protected override void CalculateLayout(RendererBase renderer, Rectangle bounds, bool floating, out Rectangle titlebarBounds, out Rectangle tabstripBounds, out Rectangle clientBounds, out Rectangle joinCatchmentBounds)
		{
			titlebarBounds = Rectangle.Empty;
			tabstripBounds = bounds;
			tabstripBounds.Height = renderer.DocumentTabStripSize;
			bounds.Offset(0, renderer.DocumentTabStripSize);
			bounds.Height -= renderer.DocumentTabStripSize;
			clientBounds = bounds;
			joinCatchmentBounds = tabstripBounds;
		}

		public override DockControl GetControlAt(Point position)
		{
			if (rectangle_2.Contains(position) && (position.X < rectangle_2.X + LeftPadding || position.X > rectangle_2.Right - RightPadding))
			{
				return null;
			}
			return base.GetControlAt(position);
		}

		protected internal override void Layout(RendererBase renderer, Graphics graphics, Rectangle bounds, bool floating)
		{
			base.Layout(renderer, graphics, bounds, floating);
			method_21(renderer, graphics, rectangle_2);
			method_22(renderer, graphics, rectangle_2);
		}

		private void method_20(RendererBase renderer, Graphics graphics, Font font_0, DockControl dockControl_2)
		{
			DrawItemState drawItemState = DrawItemState.Default;
			if (SelectedControl == dockControl_2)
			{
				drawItemState |= DrawItemState.Selected;
				if (DockContainer.Manager != null)
				{
					if (DockContainer.Manager.ActiveTabbedDocument == dockControl_2)
					{
						drawItemState |= DrawItemState.Focus;
					}
				}
			}
			if (dockControl_1 == dockControl_2)
			{
				drawItemState |= DrawItemState.HotLight;
			}
			if (!dockControl_2.Enabled)
			{
				drawItemState |= DrawItemState.Disabled;
			}
			bool drawSeparator = true;
			if (SelectedControl != null)
			{
				if (Controls.IndexOf(dockControl_2) == Controls.IndexOf(SelectedControl) - 1)
				{
					drawSeparator = false;
				}
			}
			Rectangle tabBounds = dockControl_2.TabBounds;
			if (IntegralClose && dockControl_2.AllowClose)
			{
				tabBounds.Width -= 17;
			}
			if ((drawItemState & DrawItemState.Focus) != DrawItemState.Focus)
			{
				renderer.DrawDocumentStripTab(graphics, dockControl_2.TabBounds, tabBounds, dockControl_2.TabImage, dockControl_2.TabText, font_0, dockControl_2.BackColor, dockControl_2.ForeColor, drawItemState, drawSeparator);
			}
			else
			{
				using (Font font = new Font(font_0, FontStyle.Bold))
				{
					renderer.DrawDocumentStripTab(graphics, dockControl_2.TabBounds, tabBounds, dockControl_2.TabImage, dockControl_2.TabText, font, dockControl_2.BackColor, dockControl_2.ForeColor, drawItemState, drawSeparator);
				}
			}
		}

		private void method_21(RendererBase rendere, Graphics graphics, Rectangle rectangle_5)
		{
			int y = rectangle_5.Top + rectangle_5.Height / 2 - 7;
			int num = rectangle_5.Right - 2;
			if (SelectedControl != null && SelectedControl.AllowClose && !IntegralClose)
			{
				_closeButton.Visible = true;
				_closeButton.Bounds = new Rectangle(num - 14, y, 14, 15);
				num -= 15;
			}
			else
			{
				_closeButton.Visible = false;
			}
			_rightScrollButton.Visible = false;
			_leftScrollButton.Visible = false;
			_menuButton.Visible = false;
			switch (DocumentOverflow)
			{
			case DocumentOverflowMode.Scrollable:
				_rightScrollButton.Visible = true;
				_rightScrollButton.Bounds = new Rectangle(num - 14, y, 14, 15);
				num -= 15;
				_leftScrollButton.Visible = true;
				_leftScrollButton.Bounds = new Rectangle(num - 14, y, 14, 15);
				num -= 15;
				return;
			case DocumentOverflowMode.Menu:
				_menuButton.Visible = true;
				_menuButton.Bounds = new Rectangle(num - 14, y, 14, 15);
				num -= 15;
				return;
			default:
				return;
			}
		}

		private void method_22(RendererBase renderer, Graphics graphics, Rectangle rectangle_5)
		{
			int num = 3;
			foreach (DockControl dockControl in Controls)
			{
				dockControl.bool_3 = false;
				DrawItemState drawItemState = DrawItemState.Default;
				if (SelectedControl == dockControl)
				{
					drawItemState |= DrawItemState.Selected;
					if (DockContainer.Manager != null)
					{
						if (DockContainer.Manager.ActiveTabbedDocument == dockControl)
						{
							drawItemState |= DrawItemState.Focus;
						}
					}
				}
				int num2 = renderer.MeasureDocumentStripTab(graphics, dockControl.TabImage, dockControl.TabText, dockControl.Font, drawItemState).Width;
				if (IntegralClose && dockControl.AllowClose)
				{
					num2 += 17;
				}
				if (dockControl.MinimumTabWidth != 0)
				{
					num2 = Math.Max(num2, dockControl.MinimumTabWidth);
				}
				if (dockControl.MaximumTabWidth != 0 && dockControl.MaximumTabWidth < num2)
				{
					num2 = dockControl.MaximumTabWidth;
					dockControl.bool_3 = true;
				}
				dockControl.TabBounds = new Rectangle(num, rectangle_5.Bottom - renderer.DocumentTabSize, num2, renderer.DocumentTabSize);
				num += num2 - renderer.DocumentTabExtra + 1;
			}
			if (Controls.Count != 0)
			{
				num += renderer.DocumentTabExtra;
			}
			num += 3;
			int num3 = rectangle_5.Width - LeftPadding - RightPadding;
			int_8 = num - num3;
			if (int_8 < 0)
			{
				int_8 = 0;
			}
			if (int_7 > int_8)
			{
				int_7 = int_8;
			}
			_leftScrollButton.bool_1 = (int_7 > 0);
			_rightScrollButton.bool_1 = (int_7 < int_8);
			foreach (DockControl dockControl2 in Controls)
			{
				Rectangle rectangle_6 = dockControl2.TabBounds;
				rectangle_6.Offset(rectangle_5.Left + LeftPadding - int_7, 0);
				dockControl2.TabBounds = rectangle_6;
			}
			if (IntegralClose && SelectedControl != null && SelectedControl.AllowClose)
			{
				_closeButton.Visible = true;
				Rectangle rectangle_7 = SelectedControl.TabBounds;
				_closeButton.Bounds = new Rectangle(rectangle_7.Right - 17, rectangle_7.Top + 2, 14, rectangle_7.Height - 3);
			}
		}

		private void method_23(object sender, EventArgs e)
		{
			if (DockContainer != null && SelectedControl != null)
			{
				method_24(SelectedControl);
			}
		}

		private void method_24(DockControl dockControl_2)
		{
			if (int_8 > 0)
			{
				Rectangle rectangle_ = dockControl_2.TabBounds;
				int num = rectangle_2.Right - RightPadding;
				int num2 = rectangle_2.Left + LeftPadding;
				int num3 = num - num2;
				int num4 = 0;
				if (rectangle_.Right > num)
				{
					num4 = rectangle_.Right - num3 + 30;
				}
				if (rectangle_.Left < num2)
				{
					num4 = rectangle_.Left - num2 - 30;
				}
				if (num4 != 0)
				{
					method_27(num4);
				}
			}
		}

		private void method_25()
		{
			timer_0.Enabled = false;
			HighlightedButton = null;
			bool_2 = false;
			OnLeave();
		}

		private void method_26()
		{
			timer_0.Enabled = true;
			OnTimerTick(timer_0, EventArgs.Empty);
		}

		private void method_27(int int_9)
		{
			int_7 += int_9;
			if (int_7 > int_8)
			{
				int_7 = int_8;
				method_25();
			}
			if (int_7 < 0)
			{
				int_7 = 0;
				method_25();
			}
			CalculateAllMetricsAndLayout();
		}

		protected internal override void OnMouseLeave()
		{
			base.OnMouseLeave();
			DockControl_0 = null;
		}

		protected internal override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == MouseButtons.None)
			{
				DockControl_0 = GetControlAt(new Point(e.X, e.Y));
			}
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			if (HighlightedButton == _leftScrollButton)
			{
				method_27(-15);
				return;
			}
			if (HighlightedButton != _rightScrollButton)
			{
				method_25();
				return;
			}
			method_27(15);
		}

		internal override void OnDockingManagerFinished(Class7.DockTarget dockTarget_0)
		{
			base.OnDockingManagerFinished(dockTarget_0);
			if (dockTarget_0 == null || dockTarget_0.type == Class7.DockTargetType.None)
			{
				if (SelectedControl != null && IsInContainer)
				{
					Point position = SelectedControl.PointToClient(Cursor.Position);
					DockContainer.ShowControlContextMenu(new ShowControlContextMenuEventArgs(SelectedControl, position, ContextMenuContext.Other));
				}
			}
		}

		internal override void vmethod_4(RendererBase renderer, Graphics g, Font font)
		{
			renderer.DrawDocumentStripBackground(g, rectangle_2);
		    renderer.DrawDocumentClientBackground(g, rectangle_3, SelectedControl == null ? SystemColors.Control : SelectedControl.BackColor);
		    Region clip = g.Clip;
			Rectangle rectangle_ = rectangle_2;
			rectangle_.X += LeftPadding;
			rectangle_.Width -= LeftPadding;
			rectangle_.Width -= RightPadding;
			g.SetClip(rectangle_);
			for (int i = Controls.Count - 1; i >= 0; i--)
			{
				DockControl dockControl = Controls[i];
				method_20(renderer, g, dockControl.Font, dockControl);
			}
			if (SelectedControl != null)
			{
				method_20(renderer, g, SelectedControl.Font, SelectedControl);
			}
			if (IntegralClose)
			{
				method_10(g, renderer, _closeButton, SandDockButtonType.Close, true);
			}
			g.Clip = clip;
			if (!IntegralClose)
			{
				method_10(g, renderer, _closeButton, SandDockButtonType.Close, true);
			}
			method_10(g, renderer, _rightScrollButton, SandDockButtonType.ScrollRight, _rightScrollButton.bool_1);
			method_10(g, renderer, _leftScrollButton, SandDockButtonType.ScrollLeft, _leftScrollButton.bool_1);
			method_10(g, renderer, _menuButton, SandDockButtonType.ActiveFiles, true);
		}

		internal override string vmethod_5(Point point)
		{
			DockButtonInfo @class = GetDockButtonAt(point.X, point.Y);
			if (@class == _leftScrollButton)
			{
				return SandDockLanguage.ScrollLeftText;
			}
			if (@class == _rightScrollButton)
			{
				return SandDockLanguage.ScrollRightText;
			}
			if (@class == _closeButton)
			{
				return SandDockLanguage.CloseText;
			}
			if (@class == _menuButton)
			{
				return SandDockLanguage.ActiveFilesText;
			}
			return base.vmethod_5(point);
		}

		internal override DockButtonInfo GetDockButtonAt(int x, int y)
		{
			if (_leftScrollButton.Visible && _leftScrollButton.bool_1 && _leftScrollButton.Bounds.Contains(x, y))
			{
				return _leftScrollButton;
			}
			if (_rightScrollButton.Visible && _rightScrollButton.bool_1 && _rightScrollButton.Bounds.Contains(x, y))
			{
				return _rightScrollButton;
			}
			if (_menuButton.Visible && _menuButton.bool_1 && _menuButton.Bounds.Contains(x, y))
			{
				return _menuButton;
			}
			if (_closeButton.Visible && _closeButton.bool_1 && _closeButton.Bounds.Contains(x, y))
			{
				return _closeButton;
			}
			return null;
		}

		internal override void vmethod_7(DockButtonInfo class17_8)
		{
			if (class17_8 == _leftScrollButton || class17_8 == _rightScrollButton)
			{
				method_26();
			}
		}

		internal override void vmethod_8(DockButtonInfo class17_8)
		{
			if (class17_8 == _closeButton)
			{
				OnCloseButtonClick(new CancelEventArgs());
				return;
			}
			if (class17_8 != _leftScrollButton && class17_8 != _rightScrollButton)
			{
				if (class17_8 == _menuButton && DockContainer?.Manager != null)
				{
					DockControl[] array = new DockControl[Controls.Count];
					Controls.CopyTo(array, 0);
					DockContainer.Manager.OnShowActiveFilesList(new ActiveFilesListEventArgs(array, DockContainer, new Point(_menuButton.Bounds.X, _menuButton.Bounds.Bottom)));
				}
				return;
			}
			method_25();
		}

		internal override void OnLeave()
		{
		    DockContainer?.Invalidate(rectangle_2);
		}

        [Naming(NamingType.FromOldVersion)]
        private bool IntegralClose
		{
			get
			{
				var documentContainer = DockContainer as DocumentContainer;
				return documentContainer != null && documentContainer.IntegralClose;
			}
		}

		public override bool Collapsed
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		private DockControl DockControl_0
		{
			get
			{
				return dockControl_1;
			}
			set
			{
			    if (dockControl_1 == value) return;
			    if (DockContainer != null && dockControl_1 != null)
			        DockContainer.Invalidate(dockControl_1.TabBounds);
			    dockControl_1 = value;
			    if (DockContainer != null && dockControl_1 != null)
			        DockContainer.Invalidate(dockControl_1.TabBounds);
			}
		}

        [Naming(NamingType.FromOldVersion)]
		private DocumentOverflowMode DocumentOverflow => (DockContainer as DocumentContainer)?.DocumentOverflow ?? DocumentOverflowMode.Scrollable;

        protected virtual int LeftPadding => 0;

        public Rectangle LeftScrollButtonBounds => _leftScrollButton.Bounds;

        protected virtual int RightPadding
		{
			get
			{
				if (_leftScrollButton.Visible)
				{
					return Bounds.Right - _leftScrollButton.Bounds.Left;
				}
				if (_menuButton.Visible)
				{
					return Bounds.Right - _menuButton.Bounds.Left;
				}
				if (!_closeButton.Visible)
				{
					return 0;
				}
				return Bounds.Right - _closeButton.Bounds.Left;
			}
		}

		public Rectangle RightScrollButtonBounds => _rightScrollButton.Bounds;

        public override DockControl SelectedControl
		{
			get
			{
				return base.SelectedControl;
			}
			set
			{
				base.SelectedControl = value;
			    if (value == null || DockContainer == null || !DockContainer.IsHandleCreated) return;
			    DockContainer.BeginInvoke(new EventHandler(method_23), new object[2]);
			}
		}

		private DockButtonInfo _leftScrollButton;

		private DockButtonInfo _rightScrollButton;

		private DockButtonInfo _closeButton;

		private DockButtonInfo _menuButton;

		private DockControl dockControl_1;

		private const int int_4 = 14;

		private const int int_5 = 15;

		private const int int_6 = 17;

		private int int_7;

		private int int_8;

		private Timer timer_0;
	}
}
