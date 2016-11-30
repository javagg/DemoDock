using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using TD.SandDock.Rendering;
using TD.Util;
using BorderStyle = TD.SandDock.Rendering.BorderStyle;

namespace TD.SandDock
{
    public enum TabLayout
    {
        SingleLineScrollable,
        SingleLineFixed,
        MultipleLine
    }

    internal class DockButtonInfo
    {
        public bool Visible;

        public bool bool_1 = true;

        public Rectangle Bounds = Rectangle.Empty;
    }

    [DefaultEvent("SelectedPageChanged"), DefaultProperty("TabLayout"), Designer("Design.TabControlDesigner"), ToolboxItem(true), ToolboxBitmap(typeof(TabControl))]
	public class TabControl : Control
	{
		public TabControl()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.Selectable, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			_renderer = new MilborneRenderer();
			TabPages = new TabPageCollection(this);
			_leftScrollButton = new DockButtonInfo();
			_rightScrollButton = new DockButtonInfo();
		    _timer = new Timer {Interval = 20};
		    _timer.Tick += OnTimerTick;
		}

		protected override ControlCollection CreateControlsInstance() => new TabPageControls(this);

        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			    (Renderer as IDisposable)?.Dispose();
			    _timer.Dispose();
			}
			base.Dispose(disposing);
		}

		public TabPage GetTabPageAt(Point position) => Controls.Cast<TabPage>().FirstOrDefault(page => page.TabBounds.Contains(position));

        protected override bool IsInputKey(Keys keyData)
        {
            return keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.Right || keyData == Keys.Down || base.IsInputKey(keyData);
        }

		private void DrawButton(Graphics g, ITabControlRenderer renderer, DockButtonInfo button, SandDockButtonType buttonType, bool enabled)
		{
		    if (!button.Visible) return;

		    var state = DrawItemState.Default;
		    if (HighlightedButton == button)
		    {
		        state |= DrawItemState.HotLight;
		        if (_canFocus)
		            state |= DrawItemState.Selected;
		    }
		    if (!enabled)
		        state |= DrawItemState.Disabled;
		    renderer.DrawTabControlButton(g, button.Bounds, buttonType, state);
		}

		private void DrawPages(Graphics g)
		{
            var array = Controls.Cast<TabPage>().Select(p => p.Index).Distinct().ToArray();
            Array.Sort(array);
			for (var i = 0; i < array.Length; i++)
			{
				for (var j = Controls.Count - 1; j >= 0; j--)
				{
					var page = (TabPage)Controls[j];
					if (page.Index == array[i])
					{
						DrawPage(g, page);
						if (i < array.Length - 1)
						{
							var bounds = page.TabBounds;
							bounds.X = TabStripBounds.X;
							bounds.Width = TabStripBounds.Width;
							bounds.Y = bounds.Bottom - 1;
							bounds.Height = rectangle_1.Y - bounds.Y - 2;
                            Renderer.DrawFakeTabControlBackgroundExtension(g, bounds, page.BackColor);
						}
					}
				}
			}
		}

		private void method_10(int offset)
		{
			int_2 += offset;
			if (int_2 > int_3)
			{
				int_2 = int_3;
				method_8();
			}
			if (int_2 < 0)
			{
				int_2 = 0;
				method_8();
			}
			MeasureTabStripBounds();
		}

		private DockButtonInfo method_11(int x, int y)
		{
			if (_leftScrollButton.Visible && _leftScrollButton.bool_1 && _leftScrollButton.Bounds.Contains(x, y))
			{
				return _leftScrollButton;
			}
			if (_rightScrollButton.Visible && _rightScrollButton.bool_1 && _rightScrollButton.Bounds.Contains(x, y))
			{
				return _rightScrollButton;
			}
			return null;
		}

		private void method_12(DockButtonInfo button)
		{
			if (button == _leftScrollButton || button == _rightScrollButton)
			{
				method_9();
			}
		}

		private void method_13(DockButtonInfo button)
		{
			if (button == _leftScrollButton || button == _rightScrollButton)
			{
				method_8();
			}
		}

		private void method_14(TabPage page, bool bool_2)
		{
			SelectedPage = page;
		    if (bool_2)
		        SelectedPage.SelectNextControl(null, true, true, true, true);
		    if (TabLayout != TabLayout.SingleLineScrollable) return;
		    var stripBounds = TabStripBounds;
		    stripBounds.Width -= TabStripBounds.Right - _leftScrollButton.Bounds.Left;
		    var bounds = page.TabBounds;
		    if (stripBounds.Contains(bounds)) return;
		    var num = 0;
		    if (bounds.Right > stripBounds.Right)
		    {
		        num = bounds.Right - stripBounds.Right + 20;
		    }
		    else if (bounds.Left < stripBounds.Left)
		    {
		        num = bounds.Left - stripBounds.Left - 20;
		    }
		    if (num != 0)
		        method_10(num);
		}

		private void method_15(int int_4, bool bool_2)
		{
		    if (SelectedPage == null) return;
		    var bounds = SelectedPage.TabBounds;
		    int num = bounds.X + bounds.Width / 2;
		    int num2 = SelectedPage.Index;
		    num2 += int_4;
		    foreach (TabPage page in Controls)
		    {
		        bounds = page.TabBounds;
		        if (page.Index == num2)
		        {
		            if (bounds.X <= num && bounds.Right >= num)
		            {
		                method_14(page, bool_2);
		                break;
		            }
		        }
		    }
		}

		private void method_16(int int_4, bool bool_2, bool bool_3)
		{
		    if (SelectedPage == null) return;

		    int i = Controls.IndexOf(SelectedPage);
		    i += int_4;
		    if (i > Controls.Count - 1)
		    {
		        i = bool_3 ? 0 : Controls.Count - 1;
		    }
		    if (i < 0)
		    {
		        i = bool_3 ? Controls.Count - 1 : 0;
		    }
		    method_14((TabPage)Controls[i], bool_2);
		}

		private void OnMetricsChanged(object sender, EventArgs e)
		{
			MeasureTabStripBounds();
			PerformLayout();
		}

        [Naming]
		private void DrawPage(Graphics g, TabPage page)
		{
			var state = DrawItemState.Default;
			if (page == SelectedPage)
			{
				state |= DrawItemState.Selected;
			    if (Focused && ShowFocusCues)
			        state |= DrawItemState.Checked;
			}
			Renderer.DrawTabControlTab(g, page.TabBounds, page.TabImage, page.Text, Font, page.BackColor, page.ForeColor, state, true);
		}
        [Naming]
        internal void MeasureTabStripBounds()
		{
		    if (!IsHandleCreated) return;

            using (var g = CreateGraphics())
			{
                Renderer.StartRenderSession(HotkeyPrefix.Hide);
				foreach (TabPage page in Controls)
				{
					page.bool_0 = false;
					var state = page == SelectedPage ? DrawItemState.Selected : DrawItemState.Default;
					page.TabWidth = Renderer.MeasureTabControlTab(g, page.TabImage, page.Text, Font, state).Width;
					if (page.MaximumTabWidth != 0 && page.MaximumTabWidth < page.TabWidth)
					{
						page.TabWidth = page.MaximumTabWidth;
						page.bool_0 = true;
					}
				}
                Renderer.FinishRenderSession();
			}
            if (TabLayout != TabLayout.MultipleLine)
			{
                _tabStripBounds = DisplayRectangle;
                _tabStripBounds.Height = Renderer.TabControlTabStripHeight;
			}
			else
			{
				int width = DisplayRectangle.Width;
				int num = 1;
				int num2 = 0;
				foreach (TabPage page in Controls)
				{
					num2 += (int)page.TabWidth;
				    if (num2 > width && num2 != (int) page.TabWidth)
				    {
				        num++;
				        num2 = (int) page.TabWidth;
				    }
				    num2 -= Renderer.TabControlTabExtra;
				}
				int num3 = (Renderer.TabControlTabHeight - 2) * num + (Renderer.TabControlTabStripHeight - Renderer.TabControlTabHeight);
				num3 += 2;
                _tabStripBounds = DisplayRectangle;
			    _tabStripBounds.Height = num3;
			}
			rectangle_1 = DisplayRectangle;
			rectangle_1.Offset(0, TabStripBounds.Height);
			rectangle_1.Height = rectangle_1.Height - TabStripBounds.Height;
			rectangle_2 = rectangle_1;
            rectangle_2.Inflate(-Renderer.TabControlPadding.Width, -Renderer.TabControlPadding.Height);
			switch (TabLayout)
			{
			case TabLayout.SingleLineScrollable:
				MeasureSingleLineScrollableTabPageBounds();
				break;
			case TabLayout.SingleLineFixed:
				MeasureSingleLineFixedTabPageBounds();
				break;
			case TabLayout.MultipleLine:
				MeasureMultipleLineTabPageBounds();
				break;
			}
            Invalidate(Renderer.ShouldDrawTabControlBackground);
		}
        [Naming]
        private void MeasureMultipleLineTabPageBounds()
		{
			ArrayList arrayList = new ArrayList();
			int arg_14_0 = DisplayRectangle.Width;
			ArrayList arrayList2 = null;
			ArrayList arrayList3 = new ArrayList();
			int num = TabStripBounds.Left;
			bool flag = false;
			foreach (TabPage tabPage in Controls)
			{
				if ((arrayList3.Count != 0 || flag) && num + tabPage.TabWidth > TabStripBounds.Right)
				{
					arrayList.Add(arrayList3);
					arrayList3 = new ArrayList();
					num = TabStripBounds.Left;
					arrayList3.Add(tabPage);
					if (SelectedPage == tabPage)
					{
						arrayList2 = arrayList3;
					}
				}
				else
				{
					arrayList3.Add(tabPage);
					if (SelectedPage == tabPage)
					{
						arrayList2 = arrayList3;
					}
				}
				num += (int)tabPage.TabWidth - Renderer.TabControlTabExtra;
			}
			if (arrayList3.Count != 0)
			{
				arrayList.Add(arrayList3);
			}
			if (arrayList2 != null)
			{
				arrayList.Remove(arrayList2);
				arrayList.Add(arrayList2);
			}
			int num2 = TabStripBounds.Top + (Renderer.TabControlTabStripHeight - Renderer.TabControlTabHeight);
			foreach (ArrayList arrayList4 in arrayList)
			{
				int num3 = arrayList.IndexOf(arrayList4);
				if (arrayList.Count > 1)
				{
					method_7(arrayList4, true);
				}
				num = TabStripBounds.Left;
				foreach (TabPage tabPage2 in arrayList4)
				{
					tabPage2.Index = num3;
					int num4 = (int)Math.Round(tabPage2.TabWidth, 0);
					tabPage2.TabBounds = new Rectangle(num, num2, num4, Renderer.TabControlTabHeight);
					num += num4 - Renderer.TabControlTabExtra;
				}
				num2 += Renderer.TabControlTabHeight - 2;
			}
		}
        [Naming]
        private void MeasureSingleLineScrollableTabPageBounds()
		{
			int y = TabStripBounds.Top + TabStripBounds.Height / 2 - 7;
			int num = TabStripBounds.Right - 2;
			_rightScrollButton.Visible = true;
			_rightScrollButton.Bounds = new Rectangle(num - 14, y, 14, 15);
			num -= _scrollStepLength;
			_leftScrollButton.Visible = true;
			_leftScrollButton.Bounds = new Rectangle(num - 14, y, 14, 15);
			num -= _scrollStepLength;
			int num2 = TabStripBounds.Left;
			foreach (TabPage tabPage in Controls)
			{
				int num3 = (int)Math.Round(tabPage.TabWidth, 0);
				tabPage.TabBounds = new Rectangle(num2, TabStripBounds.Bottom - Renderer.TabControlTabHeight, num3, Renderer.TabControlTabHeight);
				num2 += num3 - Renderer.TabControlTabExtra;
			}
			if (Controls.Count != 0)
			{
				num2 += Renderer.TabControlTabExtra;
			}
			int num4 = _leftScrollButton.Bounds.Left - TabStripBounds.Left;
			int_3 = num2 - num4;
			if (int_3 < 0)
			{
				int_3 = 0;
			}
			if (int_2 > int_3)
			{
				int_2 = int_3;
			}
			_leftScrollButton.bool_1 = int_2 > 0;
			_rightScrollButton.bool_1 = int_2 < int_3;
			foreach (TabPage page in Controls)
			{
				var rectangle = page.TabBounds;
				rectangle.Offset(-int_2, 0);
				page.TabBounds = rectangle;
			}
		}
        [Naming]
        private void MeasureSingleLineFixedTabPageBounds()
		{
			method_7(Controls, false);
			var left = TabStripBounds.Left;
			foreach (TabPage tabPage in Controls)
			{
				int width = (int)Math.Round(tabPage.TabWidth, 0);
				tabPage.TabBounds = new Rectangle(left, TabStripBounds.Bottom - Renderer.TabControlTabHeight, width, Renderer.TabControlTabHeight);
				left += width - Renderer.TabControlTabExtra;
			}
		}

		private void method_7(IList ilist_0, bool bool_2)
		{
			int width = TabStripBounds.Width;
			double num = 0.0;
			foreach (TabPage tabPage in ilist_0)
			{
				num += tabPage.TabWidth;
			}
			if (ilist_0.Count >= 1)
			{
				num -= (ilist_0.Count - 1) * Renderer.TabControlTabExtra;
			}
			if (num > width)
			{
				double num2 = num - width;
				for (int i = 0; i < ilist_0.Count; i++)
				{
					TabPage tabPage2 = (TabPage)ilist_0[i];
					double num3 = i != 0 ? tabPage2.TabWidth - Renderer.TabControlTabExtra : tabPage2.TabWidth;
					double num4 = num3 / num;
					num3 -= num2 * num4;
					tabPage2.bool_0 = true;
					tabPage2.TabWidth = i == 0 ? num3 : num3 + Renderer.TabControlTabExtra;
				}
				return;
			}
			if (bool_2 && num < width)
			{
				double num5 = width - num;
				for (int j = 0; j < ilist_0.Count; j++)
				{
					TabPage tabPage3 = (TabPage)ilist_0[j];
					double num6 = j != 0 ? tabPage3.TabWidth - Renderer.TabControlTabExtra : tabPage3.TabWidth;
					double num7 = num6 / num;
					num6 += num5 * num7;
					tabPage3.TabWidth = j == 0 ? num6 : num6 + Renderer.TabControlTabExtra;
				}
			}
		}

		private void method_8()
		{
			_timer.Enabled = false;
			HighlightedButton = null;
			_canFocus = false;
			Invalidate(TabStripBounds);
		}

		private void method_9()
		{
			_timer.Enabled = true;
			OnTimerTick(_timer, EventArgs.Empty);
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			MeasureTabStripBounds();
			PerformLayout();
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			if (SelectedPage == e.Control)
			{
				if (TabPages.Count == 0)
				{
					_selectedPage = null;
					OnSelectedPageChanged(EventArgs.Empty);
				}
				else
				{
					SelectedPage = TabPages[0];
				}
			}
			MeasureTabStripBounds();
			PerformLayout();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			MeasureTabStripBounds();
			PerformLayout();
			base.OnFontChanged(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Invalidate(TabStripBounds);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			MeasureTabStripBounds();
			PerformLayout();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
			case Keys.Left:
				method_16(-1, false, false);
				return;
			case Keys.Right:
				method_16(1, false, false);
				return;
			case Keys.Up:
			        if (TabLayout == TabLayout.MultipleLine)
			            method_15(-1, false);
			        break;

			case Keys.Down:
				break;
			default:
				base.OnKeyDown(e);
				break;
			}
		}

		protected override void OnLayout(LayoutEventArgs e)
		{
		    if (rectangle_2.Width <= 0 || rectangle_2.Height <= 0) return;
		    foreach (Control control in Controls)
		        control.Bounds = rectangle_2;
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate(TabStripBounds);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (HighlightedButton != null)
				{
					_canFocus = true;
					Invalidate(TabStripBounds);
					method_12(HighlightedButton);
					return;
				}
				var tabPageAt = GetTabPageAt(new Point(e.X, e.Y));
				if (tabPageAt != null)
				{
					if (SelectedPage != tabPageAt)
					{
						method_14(tabPageAt, true);
						return;
					}
					Focus();
					return;
				}
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			HighlightedButton = null;
			_canFocus = false;
			base.OnMouseLeave(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (TabLayout == TabLayout.SingleLineScrollable)
			{
				HighlightedButton = method_11(e.X, e.Y);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left && HighlightedButton != null)
			{
				method_13(HighlightedButton);
				_canFocus = false;
				Invalidate(TabStripBounds);
			}
			base.OnMouseUp(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Renderer.StartRenderSession(ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide);
			DockControl.DrawBorder(this, e.Graphics, _borderStyle);
			Renderer.DrawTabControlTabStripBackground(e.Graphics, TabStripBounds, BackColor);
			Region clip = null;
			if (TabLayout == TabLayout.SingleLineScrollable)
			{
				clip = e.Graphics.Clip;
				var clip2 = TabStripBounds;
				clip2.Width -= TabStripBounds.Right - _leftScrollButton.Bounds.Left;
				e.Graphics.SetClip(clip2);
			}
			if (TabLayout != TabLayout.MultipleLine)
			{
				for (var i = Controls.Count - 1; i >= 0; i--)
				{
					DrawPage(e.Graphics, (TabPage)Controls[i]);
				}
			}
			else
			{
				DrawPages(e.Graphics);
			}
			if (SelectedPage != null)
			{
				DrawPage(e.Graphics, SelectedPage);
			}
			if (TabLayout == TabLayout.SingleLineScrollable)
			{
				e.Graphics.Clip = clip;
			}
			if (SelectedPage != null)
			{
                Renderer.DrawTabControlBackground(e.Graphics, rectangle_1, SelectedPage.BackColor, false);
			}
			if (TabLayout == TabLayout.SingleLineScrollable)
			{
				DrawButton(e.Graphics, Renderer, _rightScrollButton, SandDockButtonType.ScrollRight, _rightScrollButton.bool_1);
				DrawButton(e.Graphics, Renderer, _leftScrollButton, SandDockButtonType.ScrollLeft, _leftScrollButton.bool_1);
			}
			Renderer.FinishRenderSession();
		    using (var brush = new SolidBrush(Color.FromArgb(30, Color.Black)))
		    using (var font = new Font(Font.FontFamily.Name, 14f, FontStyle.Bold))
		        e.Graphics.DrawString("evaluation", font, brush, TabStripBounds.Left + 4, TabStripBounds.Top - 4, StringFormat.GenericTypographic);
		}

		protected override void OnResize(EventArgs e)
		{
			MeasureTabStripBounds();
			base.OnResize(e);
		}

		protected virtual void OnSelectedPageChanged(EventArgs e)
		{
            SelectedPageChanged?.Invoke(this, e);
		}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData != (Keys.LButton | Keys.Back | Keys.Control))
			{
				switch (keyData)
				{
				case Keys.LButton | Keys.Space | Keys.Control:
					break;
				case Keys.RButton | Keys.Space | Keys.Control:
                        method_16(1, true, true);
                        return true;
                    default:
					if (keyData != (Keys.LButton | Keys.Back | Keys.Shift | Keys.Control))
					{
						return base.ProcessCmdKey(ref msg, keyData);
					}
					break;
				}
				method_16(-1, true, true);
				return true;
			}
			method_16(1, true, true);
			return true;
		}

		protected override bool ProcessMnemonic(char charCode)
		{
		    var page = Controls.Cast<TabPage>().FirstOrDefault(p => IsMnemonic(charCode, p.Text));
		    if (page == null) return base.ProcessMnemonic(charCode);
		    method_14(page, true);
		    return true;
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			if (HighlightedButton == _leftScrollButton)
			{
				method_10(-_scrollStepLength);
				return;
			}
			if (HighlightedButton == _rightScrollButton)
			{
				method_10(_scrollStepLength);
				return;
			}
			method_8();
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the TabLayout property instead.")]
		public bool AllowScrolling
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		[Category("Appearance"), DefaultValue(typeof(BorderStyle), "Flat"), Description("The type of border to be drawn around the control.")]
		public BorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				_borderStyle = value;
				MeasureTabStripBounds();
				PerformLayout();
			}
		}

        [Naming(NamingType.FromOldVersion)]
		internal DockButtonInfo HighlightedButton
		{
			get
			{
				return _highlightedButton;
			}
			set
			{
			    if (_highlightedButton == value) return;
			    if (_highlightedButton != null) Invalidate(TabStripBounds);
			    _highlightedButton = value;
			    if (_highlightedButton != null) Invalidate(TabStripBounds);
			}
		}

		protected override Size DefaultSize => new Size(300, 200);

        public override Rectangle DisplayRectangle
		{
			get
			{
				var r = base.DisplayRectangle;
				switch (_borderStyle)
				{
				case BorderStyle.Flat:
				case BorderStyle.RaisedThin:
				case BorderStyle.SunkenThin:
					r.Inflate(-1, -1);
					break;
				case BorderStyle.RaisedThick:
				case BorderStyle.SunkenThick:
					r.Inflate(-2, -2);
					break;
				}
				return r;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete]
		public SplitLayoutSystem LayoutSystem
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		[Category("Appearance"), Description("The renderer used to calculate object metrics and draw contents."), RefreshProperties(RefreshProperties.All), TypeConverter(typeof(MilborneRendererConverter))]
		public ITabControlRenderer Renderer
		{
			get
			{
				return _renderer;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException();
			    (_renderer as IDisposable)?.Dispose();
			    if (_renderer is RendererBase)
			        ((RendererBase) _renderer).MetricsChanged -= OnMetricsChanged;
			    _renderer = value;
			    if (value.ShouldDrawControlBorder && BorderStyle == BorderStyle.None)
			        BorderStyle = BorderStyle.Flat;
			    else if (!value.ShouldDrawControlBorder && BorderStyle != BorderStyle.None)
			        BorderStyle = BorderStyle.None;
			    var renderer = _renderer as RendererBase;
			    if (renderer != null)
			        renderer.MetricsChanged += OnMetricsChanged;
			    MeasureTabStripBounds();
				PerformLayout();
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get
			{
				return TabPages.IndexOf(SelectedPage);
			}
			set
			{
				SelectedPage = TabPages[value];
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TabPage SelectedPage
		{
			get
			{
				return _selectedPage;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException();
			    if (!Controls.Contains(value)) throw new ArgumentException("Specified TabPage does not belong to this TabControl.");
                _selectedPage = value;
			    MeasureTabStripBounds();
			    SuspendLayout();
			    foreach (var p in TabPages.Cast<TabPage>())
			        p.Visible = p == _selectedPage;
			    ResumeLayout();
			    OnSelectedPageChanged(EventArgs.Empty);
			}
		}

		[Category("Behavior"), DefaultValue(typeof(TabLayout), "SingleLineScrollable"), Description("How the tabs of child controls are laid out.")]
		public TabLayout TabLayout
		{
			get
			{
				return _tabLayout;
			}
			set
			{
				_tabLayout = value;
				MeasureTabStripBounds();
				PerformLayout();
			}
		}

		[Category("Behavior"), Description("A collection of TabPage controls belonging to this control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TabPageCollection TabPages { get; }

        [Browsable(false)]
		public Rectangle TabStripBounds => _tabStripBounds;

	    [Browsable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

        public event EventHandler SelectedPageChanged;

		private static bool bool_0;

		private bool _canFocus;

		private BorderStyle _borderStyle = BorderStyle.Flat;

		private DockButtonInfo _leftScrollButton;

		private DockButtonInfo _rightScrollButton;

		private DockButtonInfo _highlightedButton;

		private const int int_0 = 14;

		private const int _scrollStepLength = 15;

		private int int_2;

		private int int_3;

		private ITabControlRenderer _renderer;

		private Rectangle _tabStripBounds;

		private Rectangle rectangle_1;

		private Rectangle rectangle_2;

		private TabLayout _tabLayout;

        private TabPage _selectedPage;

		private readonly Timer _timer;

		internal class TabPageControls : ControlCollection
		{
			public TabPageControls(TabControl owner) : base(owner)
			{
				_owner = owner;
			}

			public override void Add(Control value)
			{
			    if (!(value is TabPage))
			        throw new ArgumentException("Only TabPage controls can be added to a TabControl control.");
			    value.Visible = false;
				base.Add(value);
			    if (Count == 1)
			        _owner.SelectedPage = (TabPage) value;
			}

			private readonly TabControl _owner;
		}

		public class TabPageCollection : IList
		{
			internal TabPageCollection(TabControl parent)
			{
				_parent = parent;
			}

			public void Add(TabPage page) => _parent.Controls.Add(page);

		    public void AddRange(TabPage[] pages) => _parent.Controls.AddRange(pages);

		    public void Clear() => _parent.Controls.Clear();

		    public bool Contains(TabPage page) => _parent.Controls.Contains(page);

		    public void CopyTo(TabPage[] pages, int index) => _parent.Controls.CopyTo(pages, index);

		    public IEnumerator GetEnumerator()
			{
				var pages = new TabPage[Count];
				CopyTo(pages, 0);
				return pages.GetEnumerator();
			}

			public int IndexOf(TabPage page) => _parent.Controls.IndexOf(page);

		    public void Remove(TabPage page) => _parent.Controls.Remove(page);

		    public void RemoveAt(int index) => _parent.Controls.RemoveAt(index);

		    public void SetChildIndex(TabPage page, int index) => _parent.Controls.SetChildIndex(page, index);

		    void ICollection.CopyTo(Array array, int index)
			{
			    if (array is TabPage[])
			        CopyTo((TabPage[]) array, index);
			}

			int IList.Add(object value)
			{
			    if (!(value is TabPage))
			        throw new NotSupportedException();
			    _parent.Controls.Add((TabPage)value);
				return IndexOf((TabPage)value);
			}

			bool IList.Contains(object value) => value is TabPage && Contains((TabPage)value);

		    int IList.IndexOf(object value) => value is TabPage ? IndexOf((TabPage) value) : -1;

		    void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			void IList.Remove(object value)
			{
			    if (value is TabPage)
			        Remove((TabPage) value);
			}

			public int Count => _parent.Controls.Count;

		    public TabPage this[int index] => (TabPage)_parent.Controls[index];

		    bool ICollection.IsSynchronized => false;

		    object ICollection.SyncRoot => this;

		    bool IList.IsFixedSize => false;

		    bool IList.IsReadOnly => false;

		    object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
				}
			}

			private readonly TabControl _parent;
		}
	}
}
