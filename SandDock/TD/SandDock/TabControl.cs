using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
    public enum TabLayout
    {
        SingleLineScrollable,
        SingleLineFixed,
        MultipleLine
    }

    internal class Class17
    {
        public bool bool_0;

        public bool bool_1 = true;

        public Rectangle rectangle_0 = Rectangle.Empty;
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
			this.class17_0 = new Class17();
			this.class17_1 = new Class17();
		    _timer = new Timer {Interval = 20};
		    _timer.Tick += this.timer_0_Tick;
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

		private void method_0(Graphics g, ITabControlRenderer renderer, Class17 class17_3, SandDockButtonType buttonType, bool bool_2)
		{
			if (class17_3.bool_0)
			{
				DrawItemState drawItemState = DrawItemState.Default;
				if (this.Class17_0 == class17_3)
				{
					drawItemState |= DrawItemState.HotLight;
					if (this.bool_1)
					{
						drawItemState |= DrawItemState.Selected;
					}
				}
				if (!bool_2)
				{
					drawItemState |= DrawItemState.Disabled;
				}
				renderer.DrawTabControlButton(g, class17_3.rectangle_0, buttonType, drawItemState);
			}
		}

		private void method_1(Graphics graphics_0)
		{
   //         ArrayList arrayList = new ArrayList();
			//foreach (TabPage tabPage in base.Controls.Cast<TabPage>().Where(tabPage => !arrayList.Contains(tabPage.int_1)))
			//{
			//    arrayList.Add(tabPage.int_1);
			//}
			//int[] array = (int[])arrayList.ToArray(typeof(int));

            var array = Controls.Cast<TabPage>().Select(p => p.int_1).Distinct().ToArray();
            Array.Sort<int>(array);
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = base.Controls.Count - 1; j >= 0; j--)
				{
					TabPage tabPage2 = (TabPage)base.Controls[j];
					if (tabPage2.int_1 == array[i])
					{
						this.method_2(graphics_0, tabPage2);
						if (i < array.Length - 1)
						{
							Rectangle bounds = tabPage2.TabBounds;
							bounds.X = this._tabStripBounds.X;
							bounds.Width = this._tabStripBounds.Width;
							bounds.Y = bounds.Bottom - 1;
							bounds.Height = this.rectangle_1.Y - bounds.Y - 2;
							this._renderer.DrawFakeTabControlBackgroundExtension(graphics_0, bounds, tabPage2.BackColor);
						}
					}
				}
			}
		}

		private void method_10(int int_4)
		{
			this.int_2 += int_4;
			if (this.int_2 > this.int_3)
			{
				this.int_2 = this.int_3;
				this.method_8();
			}
			if (this.int_2 < 0)
			{
				this.int_2 = 0;
				this.method_8();
			}
			this.method_3();
		}

		private Class17 method_11(int int_4, int int_5)
		{
			if (this.class17_0.bool_0 && this.class17_0.bool_1 && this.class17_0.rectangle_0.Contains(int_4, int_5))
			{
				return this.class17_0;
			}
			if (this.class17_1.bool_0 && this.class17_1.bool_1 && this.class17_1.rectangle_0.Contains(int_4, int_5))
			{
				return this.class17_1;
			}
			return null;
		}

		private void method_12(Class17 class17_3)
		{
			if (class17_3 == this.class17_0 || class17_3 == this.class17_1)
			{
				this.method_9();
			}
		}

		private void method_13(Class17 class17_3)
		{
			if (class17_3 == this.class17_0 || class17_3 == this.class17_1)
			{
				this.method_8();
			}
		}

		private void method_14(TabPage tabPage_1, bool bool_2)
		{
			this.SelectedPage = tabPage_1;
			if (bool_2)
			{
				this.SelectedPage.SelectNextControl(null, true, true, true, true);
			}
			if (this.TabLayout == TabLayout.SingleLineScrollable)
			{
				Rectangle rectangle = this._tabStripBounds;
				rectangle.Width -= this._tabStripBounds.Right - this.class17_0.rectangle_0.Left;
				Rectangle rect = tabPage_1.TabBounds;
				if (!rectangle.Contains(rect))
				{
					int num = 0;
					if (rect.Right > rectangle.Right)
					{
						num = rect.Right - rectangle.Right + 20;
					}
					else if (rect.Left < rectangle.Left)
					{
						num = rect.Left - rectangle.Left - 20;
					}
					if (num != 0)
					{
						this.method_10(num);
					}
				}
			}
		}

		private void method_15(int int_4, bool bool_2)
		{
			if (this.SelectedPage != null)
			{
				Rectangle rectangle = this.SelectedPage.TabBounds;
				int num = rectangle.X + rectangle.Width / 2;
				int num2 = this.SelectedPage.int_1;
				num2 += int_4;
				foreach (TabPage tabPage in base.Controls)
				{
					rectangle = tabPage.TabBounds;
					if (tabPage.int_1 == num2)
					{
						if (rectangle.X <= num && rectangle.Right >= num)
						{
							this.method_14(tabPage, bool_2);
							break;
						}
					}
				}
			}
		}

		private void method_16(int int_4, bool bool_2, bool bool_3)
		{
			if (this.SelectedPage != null)
			{
				int num = base.Controls.IndexOf(this.SelectedPage);
				num += int_4;
				if (num > base.Controls.Count - 1)
				{
					num = (bool_3 ? 0 : (base.Controls.Count - 1));
				}
				if (num < 0)
				{
					num = ((!bool_3) ? 0 : (base.Controls.Count - 1));
				}
				this.method_14((TabPage)base.Controls[num], bool_2);
			}
		}

		private void method_17(object sender, EventArgs e)
		{
			this.method_3();
			base.PerformLayout();
		}

		private void method_2(Graphics graphics_0, TabPage tabPage_1)
		{
			DrawItemState drawItemState = DrawItemState.Default;
			if (tabPage_1 == this.SelectedPage)
			{
				drawItemState |= DrawItemState.Selected;
				if (this.Focused && this.ShowFocusCues)
				{
					drawItemState |= DrawItemState.Checked;
				}
			}
			this.Renderer.DrawTabControlTab(graphics_0, tabPage_1.TabBounds, tabPage_1.TabImage, tabPage_1.Text, this.Font, tabPage_1.BackColor, tabPage_1.ForeColor, drawItemState, true);
		}

		internal void method_3()
		{
		    if (!IsHandleCreated)
		        return;
		    ITabControlRenderer renderer = this.Renderer;
			using (var graphics = CreateGraphics())
			{
				renderer.StartRenderSession(HotkeyPrefix.Hide);
				foreach (TabPage tabPage in base.Controls)
				{
					tabPage.bool_0 = false;
					DrawItemState state = (tabPage != this.SelectedPage) ? DrawItemState.Default : DrawItemState.Selected;
					tabPage.double_0 = (double)renderer.MeasureTabControlTab(graphics, tabPage.TabImage, tabPage.Text, this.Font, state).Width;
					if (tabPage.MaximumTabWidth != 0 && (double)tabPage.MaximumTabWidth < tabPage.double_0)
					{
						tabPage.double_0 = (double)tabPage.MaximumTabWidth;
						tabPage.bool_0 = true;
					}
				}
				renderer.FinishRenderSession();
			}
			TabLayout tabLayout = this.TabLayout;
			if (tabLayout != TabLayout.MultipleLine)
			{
				this._tabStripBounds = this.DisplayRectangle;
                this._tabStripBounds.Height = renderer.TabControlTabStripHeight;
			}
			else
			{
				int width = this.DisplayRectangle.Width;
				int num = 1;
				int num2 = 0;
				foreach (TabPage tabPage2 in base.Controls)
				{
					num2 += (int)tabPage2.double_0;
					if (num2 > width)
					{
						if (num2 != (int)tabPage2.double_0)
						{
							num++;
							num2 = (int)tabPage2.double_0;
						}
					}
					num2 -= renderer.TabControlTabExtra;
				}
				int num3 = (renderer.TabControlTabHeight - 2) * num + (renderer.TabControlTabStripHeight - renderer.TabControlTabHeight);
				num3 += 2;
				this._tabStripBounds = this.DisplayRectangle;
				this._tabStripBounds.Height = num3;
			}
			this.rectangle_1 = this.DisplayRectangle;
			this.rectangle_1.Offset(0, this._tabStripBounds.Height);
			this.rectangle_1.Height = this.rectangle_1.Height - this._tabStripBounds.Height;
			this.rectangle_2 = this.rectangle_1;
			this.rectangle_2.Inflate(-renderer.TabControlPadding.Width, -renderer.TabControlPadding.Height);
			switch (this.TabLayout)
			{
			case TabLayout.SingleLineScrollable:
				this.method_5();
				break;
			case TabLayout.SingleLineFixed:
				this.method_6();
				break;
			case TabLayout.MultipleLine:
				this.method_4();
				break;
			}
			Invalidate(renderer.ShouldDrawTabControlBackground);
		}

		private void method_4()
		{
			ArrayList arrayList = new ArrayList();
			int arg_14_0 = this.DisplayRectangle.Width;
			ArrayList arrayList2 = null;
			ArrayList arrayList3 = new ArrayList();
			int num = this._tabStripBounds.Left;
			bool flag = false;
			foreach (TabPage tabPage in base.Controls)
			{
				if ((arrayList3.Count != 0 || flag) && (double)num + tabPage.double_0 > (double)this._tabStripBounds.Right)
				{
					arrayList.Add(arrayList3);
					arrayList3 = new ArrayList();
					num = this._tabStripBounds.Left;
					arrayList3.Add(tabPage);
					if (this.SelectedPage == tabPage)
					{
						arrayList2 = arrayList3;
					}
				}
				else
				{
					arrayList3.Add(tabPage);
					if (this.SelectedPage == tabPage)
					{
						arrayList2 = arrayList3;
					}
				}
				num += (int)tabPage.double_0 - this._renderer.TabControlTabExtra;
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
			int num2 = this._tabStripBounds.Top + (this._renderer.TabControlTabStripHeight - this._renderer.TabControlTabHeight);
			foreach (ArrayList arrayList4 in arrayList)
			{
				int num3 = arrayList.IndexOf(arrayList4);
				if (arrayList.Count > 1)
				{
					this.method_7(arrayList4, true);
				}
				num = this._tabStripBounds.Left;
				foreach (TabPage tabPage2 in arrayList4)
				{
					tabPage2.int_1 = num3;
					int num4 = (int)Math.Round(tabPage2.double_0, 0);
					tabPage2.TabBounds = new Rectangle(num, num2, num4, this._renderer.TabControlTabHeight);
					num += num4 - this._renderer.TabControlTabExtra;
				}
				num2 += this._renderer.TabControlTabHeight - 2;
			}
		}

		private void method_5()
		{
			int y = this._tabStripBounds.Top + this._tabStripBounds.Height / 2 - 7;
			int num = this._tabStripBounds.Right - 2;
			this.class17_1.bool_0 = true;
			this.class17_1.rectangle_0 = new Rectangle(num - 14, y, 14, 15);
			num -= 15;
			this.class17_0.bool_0 = true;
			this.class17_0.rectangle_0 = new Rectangle(num - 14, y, 14, 15);
			num -= 15;
			int num2 = this._tabStripBounds.Left;
			foreach (TabPage tabPage in base.Controls)
			{
				int num3 = (int)Math.Round(tabPage.double_0, 0);
				tabPage.TabBounds = new Rectangle(num2, this._tabStripBounds.Bottom - this._renderer.TabControlTabHeight, num3, this._renderer.TabControlTabHeight);
				num2 += num3 - this._renderer.TabControlTabExtra;
			}
			if (base.Controls.Count != 0)
			{
				num2 += this._renderer.TabControlTabExtra;
			}
			int num4 = this.class17_0.rectangle_0.Left - this._tabStripBounds.Left;
			this.int_3 = num2 - num4;
			if (this.int_3 < 0)
			{
				this.int_3 = 0;
			}
			if (this.int_2 > this.int_3)
			{
				this.int_2 = this.int_3;
			}
			this.class17_0.bool_1 = (this.int_2 > 0);
			this.class17_1.bool_1 = (this.int_2 < this.int_3);
			foreach (TabPage tabPage2 in base.Controls)
			{
				Rectangle rectangle = tabPage2.TabBounds;
				rectangle.Offset(-this.int_2, 0);
				tabPage2.TabBounds = rectangle;
			}
		}

		private void method_6()
		{
			this.method_7(base.Controls, false);
			int num = this._tabStripBounds.Left;
			foreach (TabPage tabPage in base.Controls)
			{
				int num2 = (int)Math.Round(tabPage.double_0, 0);
				tabPage.TabBounds = new Rectangle(num, this._tabStripBounds.Bottom - this._renderer.TabControlTabHeight, num2, this._renderer.TabControlTabHeight);
				num += num2 - this._renderer.TabControlTabExtra;
			}
		}

		private void method_7(IList ilist_0, bool bool_2)
		{
			int width = this._tabStripBounds.Width;
			double num = 0.0;
			foreach (TabPage tabPage in ilist_0)
			{
				num += tabPage.double_0;
			}
			if (ilist_0.Count >= 1)
			{
				num -= (double)((ilist_0.Count - 1) * this._renderer.TabControlTabExtra);
			}
			if (num > (double)width)
			{
				double num2 = num - (double)width;
				for (int i = 0; i < ilist_0.Count; i++)
				{
					TabPage tabPage2 = (TabPage)ilist_0[i];
					double num3 = (i != 0) ? (tabPage2.double_0 - (double)this._renderer.TabControlTabExtra) : tabPage2.double_0;
					double num4 = num3 / num;
					num3 -= num2 * num4;
					tabPage2.bool_0 = true;
					tabPage2.double_0 = ((i == 0) ? num3 : (num3 + (double)this._renderer.TabControlTabExtra));
				}
				return;
			}
			if (bool_2 && num < (double)width)
			{
				double num5 = (double)width - num;
				for (int j = 0; j < ilist_0.Count; j++)
				{
					TabPage tabPage3 = (TabPage)ilist_0[j];
					double num6 = (j != 0) ? (tabPage3.double_0 - (double)this._renderer.TabControlTabExtra) : tabPage3.double_0;
					double num7 = num6 / num;
					num6 += num5 * num7;
					tabPage3.double_0 = ((j == 0) ? num6 : (num6 + (double)this._renderer.TabControlTabExtra));
				}
			}
		}

		private void method_8()
		{
			this._timer.Enabled = false;
			this.Class17_0 = null;
			this.bool_1 = false;
			Invalidate(_tabStripBounds);
		}

		private void method_9()
		{
			this._timer.Enabled = true;
			this.timer_0_Tick(this._timer, EventArgs.Empty);
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			this.method_3();
			PerformLayout();
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			if (this.SelectedPage == e.Control)
			{
				if (this.TabPages.Count == 0)
				{
					this._selectedPage = null;
					this.OnSelectedPageChanged(EventArgs.Empty);
				}
				else
				{
					this.SelectedPage = this.TabPages[0];
				}
			}
			this.method_3();
			PerformLayout();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			this.method_3();
			base.PerformLayout();
			base.OnFontChanged(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Invalidate(this.TabStripBounds);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.method_3();
			PerformLayout();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
			case Keys.Left:
				this.method_16(-1, false, false);
				return;
			case Keys.Up:
				if (this.TabLayout == TabLayout.MultipleLine)
				{
					this.method_15(-1, false);
					return;
				}
				break;
			case Keys.Right:
				this.method_16(1, false, false);
				return;
			case Keys.Down:
				break;
			default:
				base.OnKeyDown(e);
				break;
			}
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this.rectangle_2.Width > 0 && this.rectangle_2.Height > 0)
			{
				foreach (Control control in base.Controls)
				{
					control.Bounds = this.rectangle_2;
				}
				return;
			}
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate(this.TabStripBounds);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (this.Class17_0 != null)
				{
					this.bool_1 = true;
					base.Invalidate(this._tabStripBounds);
					this.method_12(this.Class17_0);
					return;
				}
				TabPage tabPageAt = this.GetTabPageAt(new Point(e.X, e.Y));
				if (tabPageAt != null)
				{
					if (this.SelectedPage != tabPageAt)
					{
						this.method_14(tabPageAt, true);
						return;
					}
					base.Focus();
					return;
				}
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			this.Class17_0 = null;
			this.bool_1 = false;
			base.OnMouseLeave(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.TabLayout == TabLayout.SingleLineScrollable)
			{
				this.Class17_0 = this.method_11(e.X, e.Y);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left && this.Class17_0 != null)
			{
				this.method_13(this.Class17_0);
				this.bool_1 = false;
				Invalidate(_tabStripBounds);
			}
			base.OnMouseUp(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			this.Renderer.StartRenderSession(this.ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide);
			DockControl.smethod_0(this, e.Graphics, this._borderStyle);
			this._renderer.DrawTabControlTabStripBackground(e.Graphics, this._tabStripBounds, this.BackColor);
			Region clip = null;
			if (this.TabLayout == TabLayout.SingleLineScrollable)
			{
				clip = e.Graphics.Clip;
				Rectangle clip2 = this._tabStripBounds;
				clip2.Width -= this._tabStripBounds.Right - this.class17_0.rectangle_0.Left;
				e.Graphics.SetClip(clip2);
			}
			if (this.TabLayout != TabLayout.MultipleLine)
			{
				for (int i = base.Controls.Count - 1; i >= 0; i--)
				{
					this.method_2(e.Graphics, (TabPage)base.Controls[i]);
				}
			}
			else
			{
				this.method_1(e.Graphics);
			}
			if (this.SelectedPage != null)
			{
				this.method_2(e.Graphics, this.SelectedPage);
			}
			if (this.TabLayout == TabLayout.SingleLineScrollable)
			{
				e.Graphics.Clip = clip;
			}
			if (this.SelectedPage != null)
			{
				this._renderer.DrawTabControlBackground(e.Graphics, this.rectangle_1, this.SelectedPage.BackColor, false);
			}
			if (this.TabLayout == TabLayout.SingleLineScrollable)
			{
				this.method_0(e.Graphics, this._renderer, this.class17_1, SandDockButtonType.ScrollRight, this.class17_1.bool_1);
				this.method_0(e.Graphics, this._renderer, this.class17_0, SandDockButtonType.ScrollLeft, this.class17_0.bool_1);
			}
			Renderer.FinishRenderSession();
		    using (var brush = new SolidBrush(Color.FromArgb(30, Color.Black)))
		    using (var font = new Font(this.Font.FontFamily.Name, 14f, FontStyle.Bold))
		        e.Graphics.DrawString("evaluation", font, brush, _tabStripBounds.Left + 4, _tabStripBounds.Top - 4, StringFormat.GenericTypographic);
		}

		protected override void OnResize(EventArgs e)
		{
			this.method_3();
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
				this.method_16(-1, true, true);
				return true;
			}
			IL_3C:
			this.method_16(1, true, true);
			return true;
		}

		protected override bool ProcessMnemonic(char charCode)
		{
			IEnumerator enumerator = base.Controls.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					TabPage tabPage = (TabPage)enumerator.Current;
					if (IsMnemonic(charCode, tabPage.Text))
					{
						this.method_14(tabPage, true);
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

		private void timer_0_Tick(object sender, EventArgs e)
		{
			if (this.Class17_0 == this.class17_0)
			{
				this.method_10(-15);
				return;
			}
			if (this.Class17_0 == this.class17_1)
			{
				this.method_10(15);
				return;
			}
			this.method_8();
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

		[Category("Appearance"), DefaultValue(typeof(TD.SandDock.Rendering.BorderStyle), "Flat"), Description("The type of border to be drawn around the control.")]
		public Rendering.BorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				_borderStyle = value;
				this.method_3();
				PerformLayout();
			}
		}

		internal Class17 Class17_0
		{
			get
			{
				return this.class17_2;
			}
			set
			{
				if (value != this.class17_2)
				{
					if (this.class17_2 != null)
					{
						base.Invalidate(this._tabStripBounds);
					}
					this.class17_2 = value;
					if (this.class17_2 != null)
					{
						base.Invalidate(this._tabStripBounds);
					}
				}
			}
		}

		protected override Size DefaultSize => new Size(300, 200);

        public override Rectangle DisplayRectangle
		{
			get
			{
				var rect = base.DisplayRectangle;
				switch (this._borderStyle)
				{
				case Rendering.BorderStyle.Flat:
				case Rendering.BorderStyle.RaisedThin:
				case Rendering.BorderStyle.SunkenThin:
					rect.Inflate(-1, -1);
					break;
				case Rendering.BorderStyle.RaisedThick:
				case Rendering.BorderStyle.SunkenThick:
					rect.Inflate(-2, -2);
					break;
				}
				return rect;
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

		[Category("Appearance"), Description("The renderer used to calculate object metrics and draw contents."), RefreshProperties(RefreshProperties.All), TypeConverter(typeof(Class26))]
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
			    (this._renderer as IDisposable)?.Dispose();
			    if (this._renderer is RendererBase)
				{
					((RendererBase)this._renderer).MetricsChanged -= this.method_17;
				}
				this._renderer = value;
				if (value.ShouldDrawControlBorder && this.BorderStyle == TD.SandDock.Rendering.BorderStyle.None)
				{
					BorderStyle = Rendering.BorderStyle.Flat;
				}
				else if (!value.ShouldDrawControlBorder && this.BorderStyle != TD.SandDock.Rendering.BorderStyle.None)
				{
					BorderStyle = Rendering.BorderStyle.None;
				}
			    var @base = this._renderer as RendererBase;
			    if (@base != null)
				{
					@base.MetricsChanged += this.method_17;
				}
				this.method_3();
				base.PerformLayout();
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
				return this._selectedPage;
			}
			set
			{
			    if (value == null)
			        throw new ArgumentNullException();
			    if (Controls.Contains(value))
				{
					this._selectedPage = value;
					this.method_3();
					base.SuspendLayout();
					IEnumerator enumerator = this.TabPages.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							TabPage tabPage = (TabPage)enumerator.Current;
							tabPage.Visible = (tabPage == this._selectedPage);
						}
						goto IL_7A;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
					    disposable?.Dispose();
					}
					goto IL_6F;
					IL_7A:
					base.ResumeLayout();
					this.OnSelectedPageChanged(EventArgs.Empty);
					return;
				}
				IL_6F:
				throw new ArgumentException("Specified TabPage does not belong to this TabControl.");
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
				this.method_3();
				PerformLayout();
			}
		}

		[Category("Behavior"), Description("A collection of TabPage controls belonging to this control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TabPageCollection TabPages { get; }

        [Browsable(false)]
		public Rectangle TabStripBounds
        {
            get
			{
				return _tabStripBounds;
			}
        }

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

		private Rendering.BorderStyle _borderStyle = Rendering.BorderStyle.Flat;

		private Class17 class17_0;

		private Class17 class17_1;

		private Class17 class17_2;

		//private Class2 class2_0;

		//private EventHandler eventHandler_0;

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
