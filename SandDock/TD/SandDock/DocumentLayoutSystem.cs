using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock.Rendering;

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
			this.class17_4 = new DockButtonInfo();
			this.class17_5 = new DockButtonInfo();
			this.class17_6 = new DockButtonInfo();
			this.class17_7 = new DockButtonInfo();
		    this.timer_0 = new Timer {Interval = 20};
		    this.timer_0.Tick += OnTimerTick;
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
			if (this.rectangle_2.Contains(position) && (position.X < this.rectangle_2.X + this.LeftPadding || position.X > this.rectangle_2.Right - this.RightPadding))
			{
				return null;
			}
			return base.GetControlAt(position);
		}

		protected internal override void Layout(RendererBase renderer, Graphics graphics, Rectangle bounds, bool floating)
		{
			base.Layout(renderer, graphics, bounds, floating);
			this.method_21(renderer, graphics, this.rectangle_2);
			this.method_22(renderer, graphics, this.rectangle_2);
		}

		private void method_20(RendererBase renderer, Graphics graphics, Font font_0, DockControl dockControl_2)
		{
			DrawItemState drawItemState = DrawItemState.Default;
			if (this.SelectedControl == dockControl_2)
			{
				drawItemState |= DrawItemState.Selected;
				if (base.DockContainer.Manager != null)
				{
					if (base.DockContainer.Manager.ActiveTabbedDocument == dockControl_2)
					{
						drawItemState |= DrawItemState.Focus;
					}
				}
			}
			if (this.dockControl_1 == dockControl_2)
			{
				drawItemState |= DrawItemState.HotLight;
			}
			if (!dockControl_2.Enabled)
			{
				drawItemState |= DrawItemState.Disabled;
			}
			bool drawSeparator = true;
			if (this.SelectedControl != null)
			{
				if (base.Controls.IndexOf(dockControl_2) == base.Controls.IndexOf(this.SelectedControl) - 1)
				{
					drawSeparator = false;
				}
			}
			Rectangle tabBounds = dockControl_2.TabBounds;
			if (this.Boolean_2 && dockControl_2.AllowClose)
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
			if (this.SelectedControl != null && this.SelectedControl.AllowClose && !this.Boolean_2)
			{
				this.class17_6.Visible = true;
				this.class17_6.Bounds = new Rectangle(num - 14, y, 14, 15);
				num -= 15;
			}
			else
			{
				this.class17_6.Visible = false;
			}
			this.class17_5.Visible = false;
			this.class17_4.Visible = false;
			this.class17_7.Visible = false;
			switch (this.DocumentOverflowMode_0)
			{
			case DocumentOverflowMode.Scrollable:
				this.class17_5.Visible = true;
				this.class17_5.Bounds = new Rectangle(num - 14, y, 14, 15);
				num -= 15;
				this.class17_4.Visible = true;
				this.class17_4.Bounds = new Rectangle(num - 14, y, 14, 15);
				num -= 15;
				return;
			case DocumentOverflowMode.Menu:
				this.class17_7.Visible = true;
				this.class17_7.Bounds = new Rectangle(num - 14, y, 14, 15);
				num -= 15;
				return;
			default:
				return;
			}
		}

		private void method_22(RendererBase renderer, Graphics graphics, Rectangle rectangle_5)
		{
			int num = 3;
			foreach (DockControl dockControl in base.Controls)
			{
				dockControl.bool_3 = false;
				DrawItemState drawItemState = DrawItemState.Default;
				if (this.SelectedControl == dockControl)
				{
					drawItemState |= DrawItemState.Selected;
					if (base.DockContainer.Manager != null)
					{
						if (base.DockContainer.Manager.ActiveTabbedDocument == dockControl)
						{
							drawItemState |= DrawItemState.Focus;
						}
					}
				}
				int num2 = renderer.MeasureDocumentStripTab(graphics, dockControl.TabImage, dockControl.TabText, dockControl.Font, drawItemState).Width;
				if (this.Boolean_2 && dockControl.AllowClose)
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
			if (base.Controls.Count != 0)
			{
				num += renderer.DocumentTabExtra;
			}
			num += 3;
			int num3 = rectangle_5.Width - this.LeftPadding - this.RightPadding;
			this.int_8 = num - num3;
			if (this.int_8 < 0)
			{
				this.int_8 = 0;
			}
			if (this.int_7 > this.int_8)
			{
				this.int_7 = this.int_8;
			}
			this.class17_4.bool_1 = (this.int_7 > 0);
			this.class17_5.bool_1 = (this.int_7 < this.int_8);
			foreach (DockControl dockControl2 in base.Controls)
			{
				Rectangle rectangle_6 = dockControl2.TabBounds;
				rectangle_6.Offset(rectangle_5.Left + this.LeftPadding - this.int_7, 0);
				dockControl2.TabBounds = rectangle_6;
			}
			if (this.Boolean_2 && this.SelectedControl != null && this.SelectedControl.AllowClose)
			{
				this.class17_6.Visible = true;
				Rectangle rectangle_7 = this.SelectedControl.TabBounds;
				this.class17_6.Bounds = new Rectangle(rectangle_7.Right - 17, rectangle_7.Top + 2, 14, rectangle_7.Height - 3);
			}
		}

		private void method_23(object sender, EventArgs e)
		{
			if (base.DockContainer != null && this.SelectedControl != null)
			{
				this.method_24(this.SelectedControl);
			}
		}

		private void method_24(DockControl dockControl_2)
		{
			if (this.int_8 > 0)
			{
				Rectangle rectangle_ = dockControl_2.TabBounds;
				int num = this.rectangle_2.Right - this.RightPadding;
				int num2 = this.rectangle_2.Left + this.LeftPadding;
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
					this.method_27(num4);
				}
			}
		}

		private void method_25()
		{
			this.timer_0.Enabled = false;
			base.ClickedDockButton = null;
			this.bool_2 = false;
			this.OnLeave();
		}

		private void method_26()
		{
			this.timer_0.Enabled = true;
			this.OnTimerTick(this.timer_0, EventArgs.Empty);
		}

		private void method_27(int int_9)
		{
			this.int_7 += int_9;
			if (this.int_7 > this.int_8)
			{
				this.int_7 = this.int_8;
				this.method_25();
			}
			if (this.int_7 < 0)
			{
				this.int_7 = 0;
				this.method_25();
			}
			base.CalculateAllMetricsAndLayout();
		}

		protected internal override void OnMouseLeave()
		{
			base.OnMouseLeave();
			this.DockControl_0 = null;
		}

		protected internal override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == MouseButtons.None)
			{
				this.DockControl_0 = this.GetControlAt(new Point(e.X, e.Y));
			}
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			if (base.ClickedDockButton == this.class17_4)
			{
				this.method_27(-15);
				return;
			}
			if (base.ClickedDockButton != this.class17_5)
			{
				this.method_25();
				return;
			}
			this.method_27(15);
		}

		internal override void OnDockingManagerFinished(Class7.DockTarget dockTarget_0)
		{
			base.OnDockingManagerFinished(dockTarget_0);
			if (dockTarget_0 == null || dockTarget_0.type == Class7.DockTargetType.None)
			{
				if (this.SelectedControl != null && base.IsInContainer)
				{
					Point position = this.SelectedControl.PointToClient(Cursor.Position);
					base.DockContainer.ShowControlContextMenu(new ShowControlContextMenuEventArgs(this.SelectedControl, position, ContextMenuContext.Other));
				}
			}
		}

		internal override void vmethod_4(RendererBase renderer, Graphics g, Font font)
		{
			renderer.DrawDocumentStripBackground(g, this.rectangle_2);
		    renderer.DrawDocumentClientBackground(g, this.rectangle_3, SelectedControl == null ? SystemColors.Control : this.SelectedControl.BackColor);
		    Region clip = g.Clip;
			Rectangle rectangle_ = this.rectangle_2;
			rectangle_.X += this.LeftPadding;
			rectangle_.Width -= this.LeftPadding;
			rectangle_.Width -= this.RightPadding;
			g.SetClip(rectangle_);
			for (int i = base.Controls.Count - 1; i >= 0; i--)
			{
				DockControl dockControl = base.Controls[i];
				this.method_20(renderer, g, dockControl.Font, dockControl);
			}
			if (this.SelectedControl != null)
			{
				this.method_20(renderer, g, this.SelectedControl.Font, this.SelectedControl);
			}
			if (this.Boolean_2)
			{
				base.method_10(g, renderer, this.class17_6, SandDockButtonType.Close, true);
			}
			g.Clip = clip;
			if (!this.Boolean_2)
			{
				base.method_10(g, renderer, this.class17_6, SandDockButtonType.Close, true);
			}
			base.method_10(g, renderer, this.class17_5, SandDockButtonType.ScrollRight, this.class17_5.bool_1);
			base.method_10(g, renderer, this.class17_4, SandDockButtonType.ScrollLeft, this.class17_4.bool_1);
			base.method_10(g, renderer, this.class17_7, SandDockButtonType.ActiveFiles, true);
		}

		internal override string vmethod_5(Point point)
		{
			DockButtonInfo @class = this.GetDockButtonAt(point.X, point.Y);
			if (@class == this.class17_4)
			{
				return SandDockLanguage.ScrollLeftText;
			}
			if (@class == this.class17_5)
			{
				return SandDockLanguage.ScrollRightText;
			}
			if (@class == this.class17_6)
			{
				return SandDockLanguage.CloseText;
			}
			if (@class == this.class17_7)
			{
				return SandDockLanguage.ActiveFilesText;
			}
			return base.vmethod_5(point);
		}

		internal override DockButtonInfo GetDockButtonAt(int x, int y)
		{
			if (this.class17_4.Visible && this.class17_4.bool_1 && this.class17_4.Bounds.Contains(x, y))
			{
				return this.class17_4;
			}
			if (this.class17_5.Visible && this.class17_5.bool_1 && this.class17_5.Bounds.Contains(x, y))
			{
				return this.class17_5;
			}
			if (this.class17_7.Visible && this.class17_7.bool_1 && this.class17_7.Bounds.Contains(x, y))
			{
				return this.class17_7;
			}
			if (this.class17_6.Visible && this.class17_6.bool_1 && this.class17_6.Bounds.Contains(x, y))
			{
				return this.class17_6;
			}
			return null;
		}

		internal override void vmethod_7(DockButtonInfo class17_8)
		{
			if (class17_8 == this.class17_4 || class17_8 == this.class17_5)
			{
				this.method_26();
			}
		}

		internal override void vmethod_8(DockButtonInfo class17_8)
		{
			if (class17_8 == this.class17_6)
			{
				this.OnCloseButtonClick(new CancelEventArgs());
				return;
			}
			if (class17_8 != this.class17_4 && class17_8 != this.class17_5)
			{
				if (class17_8 == this.class17_7 && base.DockContainer != null && base.DockContainer.Manager != null)
				{
					DockControl[] array = new DockControl[base.Controls.Count];
					base.Controls.CopyTo(array, 0);
					base.DockContainer.Manager.OnShowActiveFilesList(new ActiveFilesListEventArgs(array, base.DockContainer, new Point(this.class17_7.Bounds.X, this.class17_7.Bounds.Bottom)));
				}
				return;
			}
			this.method_25();
		}

		internal override void OnLeave()
		{
		    DockContainer?.Invalidate(this.rectangle_2);
		}

        private bool Boolean_2
		{
			get
			{
				DocumentContainer documentContainer = DockContainer as DocumentContainer;
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
				return this.dockControl_1;
			}
			set
			{
				if (value != this.dockControl_1)
				{
					if (base.DockContainer != null && this.dockControl_1 != null)
					{
						base.DockContainer.Invalidate(this.dockControl_1.TabBounds);
					}
					this.dockControl_1 = value;
					if (base.DockContainer != null && this.dockControl_1 != null)
					{
						base.DockContainer.Invalidate(this.dockControl_1.TabBounds);
					}
				}
			}
		}

		private DocumentOverflowMode DocumentOverflowMode_0 => (DockContainer as DocumentContainer)?.DocumentOverflow ?? DocumentOverflowMode.Scrollable;

        protected virtual int LeftPadding => 0;

        public Rectangle LeftScrollButtonBounds => this.class17_4.Bounds;

        protected virtual int RightPadding
		{
			get
			{
				if (this.class17_4.Visible)
				{
					return base.Bounds.Right - this.class17_4.Bounds.Left;
				}
				if (this.class17_7.Visible)
				{
					return base.Bounds.Right - this.class17_7.Bounds.Left;
				}
				if (!this.class17_6.Visible)
				{
					return 0;
				}
				return base.Bounds.Right - this.class17_6.Bounds.Left;
			}
		}

		public Rectangle RightScrollButtonBounds => this.class17_5.Bounds;

        public override DockControl SelectedControl
		{
			get
			{
				return base.SelectedControl;
			}
			set
			{
				base.SelectedControl = value;
				if (value != null && base.DockContainer != null && base.DockContainer.IsHandleCreated)
				{
					Control arg_39_0 = base.DockContainer;
					Delegate arg_39_1 = new EventHandler(this.method_23);
					object[] args = new object[2];
					arg_39_0.BeginInvoke(arg_39_1, args);
				}
			}
		}

		private DockButtonInfo class17_4;

		private DockButtonInfo class17_5;

		private DockButtonInfo class17_6;

		private DockButtonInfo class17_7;

		private DockControl dockControl_1;

		private const int int_4 = 14;

		private const int int_5 = 15;

		private const int int_6 = 17;

		private int int_7;

		private int int_8;

		private Timer timer_0;
	}
}
