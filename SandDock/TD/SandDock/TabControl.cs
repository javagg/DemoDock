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

    [DefaultEvent("SelectedPageChanged"), DefaultProperty("TabLayout"), Designer("TD.SandDock.Design.TabControlDesigner, SandDock.Design"), ToolboxItem(true), ToolboxBitmap(typeof(TabControl))]
	public class TabControl : Control
	{
		public TabControl()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.Selectable, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			_renderer = new MilborneRenderer();
			TabPages = new TabPageCollection(this);
			class17_0 = new DockButtonInfo();
			class17_1 = new DockButtonInfo();
		    _timer = new Timer {Interval = 20};
		    _timer.Tick += OnTimerTick;
		}

		protected override ControlCollection CreateControlsInstance() => new Control2(this);

        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			    (_renderer as IDisposable)?.Dispose();
			    _timer.Dispose();
			}
			base.Dispose(disposing);
		}

		public TabPage GetTabPageAt(Point position) => Controls.Cast<TabPage>().FirstOrDefault(page => page.TabBounds.Contains(position));

        protected override bool IsInputKey(Keys keyData)
        {
            return keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.Right || keyData == Keys.Down || base.IsInputKey(keyData);
        }

		private void method_0(Graphics g, ITabControlRenderer renderer, DockButtonInfo class17_3, SandDockButtonType buttonType, bool bool_2)
		{
			if (class17_3.Visible)
			{
				DrawItemState drawItemState = DrawItemState.Default;
				if (HighlightedButton == class17_3)
				{
					drawItemState |= DrawItemState.HotLight;
					if (bool_1)
					{
						drawItemState |= DrawItemState.Selected;
					}
				}
				if (!bool_2)
				{
					drawItemState |= DrawItemState.Disabled;
				}
				renderer.DrawTabControlButton(g, class17_3.Bounds, buttonType, drawItemState);
			}
		}

		private void method_1(Graphics g)
		{
            var array = Controls.Cast<TabPage>().Select(p => p.int_1).Distinct().ToArray();
            Array.Sort(array);
			for (var i = 0; i < array.Length; i++)
			{
				for (var j = Controls.Count - 1; j >= 0; j--)
				{
					TabPage tabPage2 = (TabPage)Controls[j];
					if (tabPage2.int_1 == array[i])
					{
						method_2(g, tabPage2);
						if (i < array.Length - 1)
						{
							var bounds = tabPage2.TabBounds;
							bounds.X = TabStripBounds.X;
							bounds.Width = TabStripBounds.Width;
							bounds.Y = bounds.Bottom - 1;
							bounds.Height = rectangle_1.Y - bounds.Y - 2;
							_renderer.DrawFakeTabControlBackgroundExtension(g, bounds, tabPage2.BackColor);
						}
					}
				}
			}
		}

		private void method_10(int int_4)
		{
			int_2 += int_4;
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
			method_3();
		}

		private DockButtonInfo method_11(int int_4, int int_5)
		{
			if (class17_0.Visible && class17_0.bool_1 && class17_0.Bounds.Contains(int_4, int_5))
			{
				return class17_0;
			}
			if (class17_1.Visible && class17_1.bool_1 && class17_1.Bounds.Contains(int_4, int_5))
			{
				return class17_1;
			}
			return null;
		}

		private void method_12(DockButtonInfo class17_3)
		{
			if (class17_3 == class17_0 || class17_3 == class17_1)
			{
				method_9();
			}
		}

		private void method_13(DockButtonInfo class17_3)
		{
			if (class17_3 == class17_0 || class17_3 == class17_1)
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
		    stripBounds.Width -= TabStripBounds.Right - class17_0.Bounds.Left;
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
		    int num2 = SelectedPage.int_1;
		    num2 += int_4;
		    foreach (TabPage tabPage in Controls)
		    {
		        bounds = tabPage.TabBounds;
		        if (tabPage.int_1 == num2)
		        {
		            if (bounds.X <= num && bounds.Right >= num)
		            {
		                method_14(tabPage, bool_2);
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
		        i = (bool_3 ? 0 : (Controls.Count - 1));
		    }
		    if (i < 0)
		    {
		        i = ((!bool_3) ? 0 : (Controls.Count - 1));
		    }
		    method_14((TabPage)Controls[i], bool_2);
		}

		private void method_17(object sender, EventArgs e)
		{
			method_3();
			PerformLayout();
		}

		private void method_2(Graphics g, TabPage page)
		{
			var state = DrawItemState.Default;
			if (page == SelectedPage)
			{
				state |= DrawItemState.Selected;
				if (Focused && ShowFocusCues)
				{
					state |= DrawItemState.Checked;
				}
			}
			Renderer.DrawTabControlTab(g, page.TabBounds, page.TabImage, page.Text, Font, page.BackColor, page.ForeColor, state, true);
		}

		internal void method_3()
		{
		    if (!IsHandleCreated) return;
		    var renderer = Renderer;
			using (var graphics = CreateGraphics())
			{
				renderer.StartRenderSession(HotkeyPrefix.Hide);
				foreach (TabPage page in Controls)
				{
					page.bool_0 = false;
					var state = page != SelectedPage ? DrawItemState.Default : DrawItemState.Selected;
					page.double_0tw = renderer.MeasureTabControlTab(graphics, page.TabImage, page.Text, Font, state).Width;
					if (page.MaximumTabWidth != 0 && page.MaximumTabWidth < page.double_0tw)
					{
						page.double_0tw = page.MaximumTabWidth;
						page.bool_0 = true;
					}
				}
				renderer.FinishRenderSession();
			}
			var tabLayout = TabLayout;
			if (tabLayout != TabLayout.MultipleLine)
			{
                _tabStripBounds = DisplayRectangle;
                _tabStripBounds.Height = renderer.TabControlTabStripHeight;
			}
			else
			{
				int width = DisplayRectangle.Width;
				int num = 1;
				int num2 = 0;
				foreach (TabPage page in Controls)
				{
					num2 += (int)page.double_0tw;
				    if (num2 > width && num2 != (int) page.double_0tw)
				    {
				        num++;
				        num2 = (int) page.double_0tw;
				    }
				    num2 -= renderer.TabControlTabExtra;
				}
				int num3 = (renderer.TabControlTabHeight - 2) * num + (renderer.TabControlTabStripHeight - renderer.TabControlTabHeight);
				num3 += 2;
                _tabStripBounds = DisplayRectangle;
			    _tabStripBounds.Height = num3;
			}
			rectangle_1 = DisplayRectangle;
			rectangle_1.Offset(0, TabStripBounds.Height);
			rectangle_1.Height = rectangle_1.Height - TabStripBounds.Height;
			rectangle_2 = rectangle_1;
			rectangle_2.Inflate(-renderer.TabControlPadding.Width, -renderer.TabControlPadding.Height);
			switch (TabLayout)
			{
			case TabLayout.SingleLineScrollable:
				method_5();
				break;
			case TabLayout.SingleLineFixed:
				method_6();
				break;
			case TabLayout.MultipleLine:
				method_4();
				break;
			}
			Invalidate(renderer.ShouldDrawTabControlBackground);
		}

		private void method_4()
		{
			ArrayList arrayList = new ArrayList();
			int arg_14_0 = DisplayRectangle.Width;
			ArrayList arrayList2 = null;
			ArrayList arrayList3 = new ArrayList();
			int num = TabStripBounds.Left;
			bool flag = false;
			foreach (TabPage tabPage in Controls)
			{
				if ((arrayList3.Count != 0 || flag) && num + tabPage.double_0tw > TabStripBounds.Right)
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
				num += (int)tabPage.double_0tw - _renderer.TabControlTabExtra;
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
			int num2 = TabStripBounds.Top + (_renderer.TabControlTabStripHeight - _renderer.TabControlTabHeight);
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
					tabPage2.int_1 = num3;
					int num4 = (int)Math.Round(tabPage2.double_0tw, 0);
					tabPage2.TabBounds = new Rectangle(num, num2, num4, _renderer.TabControlTabHeight);
					num += num4 - _renderer.TabControlTabExtra;
				}
				num2 += _renderer.TabControlTabHeight - 2;
			}
		}

		private void method_5()
		{
			int y = TabStripBounds.Top + TabStripBounds.Height / 2 - 7;
			int num = TabStripBounds.Right - 2;
			class17_1.Visible = true;
			class17_1.Bounds = new Rectangle(num - 14, y, 14, 15);
			num -= 15;
			class17_0.Visible = true;
			class17_0.Bounds = new Rectangle(num - 14, y, 14, 15);
			num -= 15;
			int num2 = TabStripBounds.Left;
			foreach (TabPage tabPage in Controls)
			{
				int num3 = (int)Math.Round(tabPage.double_0tw, 0);
				tabPage.TabBounds = new Rectangle(num2, TabStripBounds.Bottom - _renderer.TabControlTabHeight, num3, _renderer.TabControlTabHeight);
				num2 += num3 - _renderer.TabControlTabExtra;
			}
			if (Controls.Count != 0)
			{
				num2 += _renderer.TabControlTabExtra;
			}
			int num4 = class17_0.Bounds.Left - TabStripBounds.Left;
			int_3 = num2 - num4;
			if (int_3 < 0)
			{
				int_3 = 0;
			}
			if (int_2 > int_3)
			{
				int_2 = int_3;
			}
			class17_0.bool_1 = (int_2 > 0);
			class17_1.bool_1 = (int_2 < int_3);
			foreach (TabPage tabPage2 in Controls)
			{
				Rectangle rectangle = tabPage2.TabBounds;
				rectangle.Offset(-int_2, 0);
				tabPage2.TabBounds = rectangle;
			}
		}

		private void method_6()
		{
			method_7(Controls, false);
			int num = TabStripBounds.Left;
			foreach (TabPage tabPage in Controls)
			{
				int num2 = (int)Math.Round(tabPage.double_0tw, 0);
				tabPage.TabBounds = new Rectangle(num, TabStripBounds.Bottom - _renderer.TabControlTabHeight, num2, _renderer.TabControlTabHeight);
				num += num2 - _renderer.TabControlTabExtra;
			}
		}

		private void method_7(IList ilist_0, bool bool_2)
		{
			int width = TabStripBounds.Width;
			double num = 0.0;
			foreach (TabPage tabPage in ilist_0)
			{
				num += tabPage.double_0tw;
			}
			if (ilist_0.Count >= 1)
			{
				num -= (ilist_0.Count - 1) * _renderer.TabControlTabExtra;
			}
			if (num > width)
			{
				double num2 = num - width;
				for (int i = 0; i < ilist_0.Count; i++)
				{
					TabPage tabPage2 = (TabPage)ilist_0[i];
					double num3 = (i != 0) ? (tabPage2.double_0tw - _renderer.TabControlTabExtra) : tabPage2.double_0tw;
					double num4 = num3 / num;
					num3 -= num2 * num4;
					tabPage2.bool_0 = true;
					tabPage2.double_0tw = ((i == 0) ? num3 : (num3 + _renderer.TabControlTabExtra));
				}
				return;
			}
			if (bool_2 && num < width)
			{
				double num5 = width - num;
				for (int j = 0; j < ilist_0.Count; j++)
				{
					TabPage tabPage3 = (TabPage)ilist_0[j];
					double num6 = (j != 0) ? (tabPage3.double_0tw - _renderer.TabControlTabExtra) : tabPage3.double_0tw;
					double num7 = num6 / num;
					num6 += num5 * num7;
					tabPage3.double_0tw = ((j == 0) ? num6 : (num6 + _renderer.TabControlTabExtra));
				}
			}
		}

		private void method_8()
		{
			_timer.Enabled = false;
			HighlightedButton = null;
			bool_1 = false;
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
			method_3();
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
			method_3();
			PerformLayout();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			method_3();
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
			method_3();
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
					bool_1 = true;
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
			bool_1 = false;
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
				bool_1 = false;
				Invalidate(TabStripBounds);
			}
			base.OnMouseUp(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Renderer.StartRenderSession(ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide);
			DockControl.smethod_0(this, e.Graphics, _borderStyle);
			_renderer.DrawTabControlTabStripBackground(e.Graphics, TabStripBounds, BackColor);
			Region clip = null;
			if (TabLayout == TabLayout.SingleLineScrollable)
			{
				clip = e.Graphics.Clip;
				Rectangle clip2 = TabStripBounds;
				clip2.Width -= TabStripBounds.Right - class17_0.Bounds.Left;
				e.Graphics.SetClip(clip2);
			}
			if (TabLayout != TabLayout.MultipleLine)
			{
				for (int i = Controls.Count - 1; i >= 0; i--)
				{
					method_2(e.Graphics, (TabPage)Controls[i]);
				}
			}
			else
			{
				method_1(e.Graphics);
			}
			if (SelectedPage != null)
			{
				method_2(e.Graphics, SelectedPage);
			}
			if (TabLayout == TabLayout.SingleLineScrollable)
			{
				e.Graphics.Clip = clip;
			}
			if (SelectedPage != null)
			{
				_renderer.DrawTabControlBackground(e.Graphics, rectangle_1, SelectedPage.BackColor, false);
			}
			if (TabLayout == TabLayout.SingleLineScrollable)
			{
				method_0(e.Graphics, _renderer, class17_1, SandDockButtonType.ScrollRight, class17_1.bool_1);
				method_0(e.Graphics, _renderer, class17_0, SandDockButtonType.ScrollLeft, class17_0.bool_1);
			}
			Renderer.FinishRenderSession();
		    using (var brush = new SolidBrush(Color.FromArgb(30, Color.Black)))
		    using (var font = new Font(Font.FontFamily.Name, 14f, FontStyle.Bold))
		        e.Graphics.DrawString("evaluation", font, brush, TabStripBounds.Left + 4, TabStripBounds.Top - 4, StringFormat.GenericTypographic);
		}

		protected override void OnResize(EventArgs e)
		{
			method_3();
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
					goto IL_3C;
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
			IL_3C:
			method_16(1, true, true);
			return true;
		}

		protected override bool ProcessMnemonic(char charCode)
		{
		    var page = Controls.Cast<TabPage>().FirstOrDefault(p => IsMnemonic(charCode, p.Text));
		    if (page == null) return base.ProcessMnemonic(charCode);
		    method_14(page, true);
		    return true;

		    IEnumerator enumerator = Controls.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					TabPage tabPage = (TabPage)enumerator.Current;
					if (IsMnemonic(charCode, tabPage.Text))
					{
						method_14(tabPage, true);
						result = true;
						return result;
					}
				}
				goto IL_52;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return result;
			IL_52:
			return base.ProcessMnemonic(charCode);
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			if (HighlightedButton == class17_0)
			{
				method_10(-15);
				return;
			}
			if (HighlightedButton == class17_1)
			{
				method_10(15);
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
				method_3();
				PerformLayout();
			}
		}

        [Naming(NamingType.FromOldVersion)]
		internal DockButtonInfo HighlightedButton
		{
			get
			{
				return class17_2;
			}
			set
			{
			    if (class17_2 == value) return;
			    if (class17_2 != null)
			        Invalidate(TabStripBounds);
			    class17_2 = value;
			    if (class17_2 != null)
			        Invalidate(TabStripBounds);
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
			    if (value == null)
			        throw new ArgumentNullException();
			    (_renderer as IDisposable)?.Dispose();
			    if (_renderer is RendererBase)
				{
					((RendererBase)_renderer).MetricsChanged -= method_17;
				}
				_renderer = value;
				if (value.ShouldDrawControlBorder && BorderStyle == BorderStyle.None)
				{
					BorderStyle = BorderStyle.Flat;
				}
				else if (!value.ShouldDrawControlBorder && BorderStyle != BorderStyle.None)
				{
					BorderStyle = BorderStyle.None;
				}
			    var @base = _renderer as RendererBase;
			    if (@base != null)
				{
					@base.MetricsChanged += method_17;
				}
				method_3();
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
			    method_3();
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
				method_3();
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

		private bool bool_1;

		private BorderStyle _borderStyle = BorderStyle.Flat;

		private DockButtonInfo class17_0;

		private DockButtonInfo class17_1;

		private DockButtonInfo class17_2;

		private const int int_0 = 14;

		private const int int_1 = 15;

		private int int_2;

		private int int_3;

		private ITabControlRenderer _renderer;

		private Rectangle _tabStripBounds;

		private Rectangle rectangle_1;

		private Rectangle rectangle_2;

		private TabLayout _tabLayout;

        private TabPage _selectedPage;

		private readonly Timer _timer;

		internal class Control2 : ControlCollection
		{
			public Control2(TabControl owner) : base(owner)
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

			public void Add(TabPage tabPage) => _parent.Controls.Add(tabPage);

		    public void AddRange(TabPage[] tabPages) => _parent.Controls.AddRange(tabPages);

		    public void Clear() => _parent.Controls.Clear();

		    public bool Contains(TabPage tabPage) => _parent.Controls.Contains(tabPage);

		    public void CopyTo(TabPage[] array, int index) => _parent.Controls.CopyTo(array, index);

		    public IEnumerator GetEnumerator()
			{
				var array = new TabPage[Count];
				CopyTo(array, 0);
				return array.GetEnumerator();
			}

			public int IndexOf(TabPage tabPage) => _parent.Controls.IndexOf(tabPage);

		    public void Remove(TabPage tabPage) => _parent.Controls.Remove(tabPage);

		    public void RemoveAt(int index) => _parent.Controls.RemoveAt(index);

		    public void SetChildIndex(TabPage tabPage, int index) => _parent.Controls.SetChildIndex(tabPage, index);

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
