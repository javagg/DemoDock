using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
    public class EverettRenderer : RendererBase
	{
	    protected internal override Rectangle AdjustDockControlClientBounds(ControlLayoutSystem layoutSystem, DockControl control, Rectangle clientBounds)
		{
		    if (!(layoutSystem is DocumentLayoutSystem)) return base.AdjustDockControlClientBounds(layoutSystem, control, clientBounds);
		    clientBounds.Inflate(-2, -2);
		    return clientBounds;
		}

		protected internal override void DrawAutoHideBarBackground(Control container, Control autoHideBar, Graphics graphics, Rectangle bounds)
		{
		    using (_backBrush = new SolidBrush(TabStripBackgroundColor))
		        graphics.FillRectangle(_backBrush, bounds);
		}

		protected internal override void DrawCollapsedTab(Graphics graphics, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool vertical)
		{
		    using (var brush = new SolidBrush(backColor))
		        graphics.FillRectangle(brush, bounds);
		    if (dockSide != DockSide.Top)
			{
				graphics.DrawLine(pen_2, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
			}
			if (dockSide != DockSide.Right)
			{
				graphics.DrawLine(pen_2, bounds.Right, bounds.Top, bounds.Right, bounds.Bottom);
			}
			if (dockSide != DockSide.Bottom)
			{
				graphics.DrawLine(pen_2, bounds.Left, bounds.Bottom, bounds.Right, bounds.Bottom);
			}
			if (dockSide != DockSide.Left)
			{
				graphics.DrawLine(pen_2, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom);
			}
			bounds.Inflate(-2, -2);
		    if (vertical)
		        bounds.Offset(0, 1);
		    else
		        bounds.Offset(1, 0);
		    graphics.DrawImage(image, new Rectangle(bounds.Left, bounds.Top, image.Width, image.Height));
		    if (text.Length == 0) return;
            if (vertical)
		    {
		        bounds.Offset(0, 23);
		        graphics.DrawString(text, font, solidBrush_1, bounds, VerticalTextFormat);
		        return;
		    }
		    bounds.Offset(23, 0);
		    graphics.DrawString(text, font, solidBrush_1, bounds, HorizontalTextFormat);
		}

		protected internal override void DrawControlClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
		}

		protected internal override void DrawDockContainerBackground(Graphics graphics, DockContainer container, Rectangle bounds)
		{
			RenderHelper.ClearBackground(graphics, container.BackColor);
		}

		protected internal override void DrawDocumentClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
		    using (var brush = new SolidBrush(backColor))
		        graphics.FillRectangle(brush, bounds);
		}

		protected internal override void DrawDocumentStripBackground(Graphics graphics, Rectangle bounds)
		{
			graphics.FillRectangle(_backBrush, bounds);
			graphics.DrawLine(pen_1, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
		}

		protected internal override void DrawDocumentStripButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
		{
			vmethod_0(graphics, bounds, state);
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				bounds.Offset(1, 1);
			}
			switch (buttonType)
			{
			case SandDockButtonType.Close:
				using (var pen = new Pen(color_6))
				{
					ButtonRenderHelper.DrawDocumentStripCloseButton(graphics, bounds, pen);
					return;
				}
			    case SandDockButtonType.Pin:
			case SandDockButtonType.WindowPosition:
				return;
			case SandDockButtonType.ScrollLeft:
				break;
			case SandDockButtonType.ScrollRight:
				ButtonRenderHelper.DrawScrollRightDockButton(graphics, bounds, color_6, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
				return;
			case SandDockButtonType.ActiveFiles:
				ButtonRenderHelper.DrawPositionDockButton(graphics, bounds, SystemPens.ControlText);
				return;
			default:
				return;
			}
			ButtonRenderHelper.DrawScrollLeftDockButton(graphics, bounds, color_6, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
		}

		protected internal override void DrawDocumentStripTab(Graphics graphics, Rectangle bounds, Rectangle contentBounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				using (SolidBrush solidBrush = new SolidBrush(backColor))
				{
					graphics.FillRectangle(solidBrush, bounds);
				}
				graphics.DrawLine(pen_1, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
				graphics.DrawLine(pen_1, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
				graphics.DrawLine(pen_0, bounds.Right - 1, bounds.Top + 1, bounds.Right - 1, bounds.Bottom - 1);
			}
			else if (drawSeparator)
			{
				graphics.DrawLine(SystemPens.ControlDark, bounds.Right, bounds.Top + 3, bounds.Right, bounds.Bottom - 3);
			}
			bounds = contentBounds;
			if (image != null)
			{
				graphics.DrawImage(image, bounds.X + 4, bounds.Y + 2, 16, 16);
				bounds.X += 20;
				bounds.Width -= 20;
			}
			if (bounds.Width > 8)
			{
				Font font2 = font;
				if ((state & DrawItemState.Focus) == DrawItemState.Focus)
				{
					font2 = new Font(font, FontStyle.Bold);
				}
				if ((state & DrawItemState.Selected) != DrawItemState.Selected)
				{
					graphics.DrawString(text, font2, solidBrush_1, bounds, stringFormat_2);
				}
				else
				{
					using (SolidBrush solidBrush2 = new SolidBrush(foreColor))
					{
						graphics.DrawString(text, font2, solidBrush2, bounds, stringFormat_2);
					}
				}
				if ((state & DrawItemState.Focus) == DrawItemState.Focus)
				{
					font2.Dispose();
				}
			}
		}

		protected internal override void DrawSplitter(Control container, Control control, Graphics graphics, Rectangle bounds, Orientation orientation)
		{
		}

		protected internal override void DrawTabStripBackground(Control container, Control control, Graphics graphics, Rectangle bounds, int selectedTabOffset)
		{
			graphics.FillRectangle(_backBrush, bounds);
			graphics.DrawLine(pen_0, bounds.X, bounds.Y, bounds.Right, bounds.Y);
		}

		protected internal override void DrawTabStripTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
			    using (var brush = new SolidBrush(backColor))
			        graphics.FillRectangle(brush, bounds);
			    graphics.DrawLine(pen_1, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
				graphics.DrawLine(pen_0, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
				graphics.DrawLine(pen_0, bounds.Right, bounds.Top, bounds.Right, bounds.Bottom - 1);
			}
			else if (drawSeparator)
			{
				graphics.DrawLine(SystemPens.ControlDark, bounds.Right, bounds.Top + 3, bounds.Right, bounds.Bottom - 3);
			}
			if (bounds.Width >= 24)
			{
				graphics.DrawImage(image, new Rectangle(bounds.X + 4, bounds.Y + 2, image.Width, image.Height));
			}
			bounds.X += 23;
			bounds.Width -= 25;
		    if (bounds.Width <= 8) return;

		    if ((state & DrawItemState.Selected) != DrawItemState.Selected)
		        graphics.DrawString(text, font, solidBrush_1, bounds, HorizontalTextFormat);
		    else
		        using (var brush = new SolidBrush(foreColor))
		            graphics.DrawString(text, font, brush, bounds, HorizontalTextFormat);
		    
		}

		protected internal override void DrawTitleBarBackground(Graphics graphics, Rectangle bounds, bool focused)
		{
		    if (focused)
		        graphics.FillRectangle(SystemBrushes.ActiveCaption, bounds);
		    else
		    {
		        graphics.FillRectangle(SystemBrushes.Control, bounds);
		        graphics.DrawLine(SystemPens.ControlDark, bounds.X + 1, bounds.Y, bounds.Right - 2, bounds.Y);
		        graphics.DrawLine(SystemPens.ControlDark, bounds.X + 1, bounds.Bottom - 1, bounds.Right - 2, bounds.Bottom - 1);
		        graphics.DrawLine(SystemPens.ControlDark, bounds.X, bounds.Y + 1, bounds.X, bounds.Bottom - 2);
		        graphics.DrawLine(SystemPens.ControlDark, bounds.Right - 1, bounds.Y + 1, bounds.Right - 1, bounds.Bottom - 2);
		    }
		}

		protected internal override void DrawTitleBarButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state, bool focused, bool toggled)
		{
			bounds.Width--;
			bounds.Height--;
			vmethod_0(graphics, bounds, state);
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				bounds.Offset(1, 1);
			}
			switch (buttonType)
			{
			case SandDockButtonType.Close:
				ButtonRenderHelper.DrawCloseDockButton(graphics, bounds, focused ? SystemPens.ActiveCaptionText : SystemPens.ControlText);
				return;
			case SandDockButtonType.Pin:
				ButtonRenderHelper.DrawPinDockButton(graphics, bounds, focused ? SystemPens.ActiveCaptionText : SystemPens.ControlText, toggled);
				return;
			case SandDockButtonType.ScrollLeft:
			case SandDockButtonType.ScrollRight:
				break;
			case SandDockButtonType.WindowPosition:
				ButtonRenderHelper.DrawPositionDockButton(graphics, bounds, focused ? SystemPens.ActiveCaptionText : SystemPens.ControlText);
				break;
			default:
				return;
			}
		}

		protected internal override void DrawTitleBarText(Graphics graphics, Rectangle bounds, bool focused, string text, Font font)
		{
			var brush = focused ? SystemBrushes.ActiveCaptionText : SystemBrushes.ControlText;
			bounds.Inflate(-3, 0);
			graphics.DrawString(text, font, brush, bounds, HorizontalTextFormat);
		}

		public override void FinishRenderSession()
		{
			_backBrush.Dispose();
			pen_0.Dispose();
			pen_1.Dispose();
			solidBrush_1.Dispose();
			pen_2.Dispose();
			stringFormat_2.Dispose();
		}

		protected override void GetColorsFromSystem()
		{
			TabStripBackgroundColor = method_1(SystemColors.Control);
		}

		protected internal override Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			int num;
			if ((state & DrawItemState.Focus) == DrawItemState.Focus)
			{
				using (Font font2 = new Font(font, FontStyle.Bold))
				{
					num = (int)Math.Ceiling(graphics.MeasureString(text, font2, 999, stringFormat_2).Width);
					goto IL_65;
				}
			}
			num = (int)Math.Ceiling(graphics.MeasureString(text, font, 999, stringFormat_2).Width);
			IL_65:
			num += 2 + Int32_0 * 2 + 2;
			if (image != null)
			{
				num += 20;
			}
			num += DocumentTabExtra;
			return new Size(num, 0);
		}

		protected internal override Size MeasureTabStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			int num = (int)Math.Ceiling(graphics.MeasureString(text, font, 2147483647, stringFormat_2).Width);
			num += 30;
			return new Size(num, 18);
		}

		private Color method_1(Color color)
		{
			byte b = color.R;
			byte b2 = color.G;
			byte b3 = color.B;
			byte b4 = Math.Max(Math.Max(b, b2), b3);
			if (b4 != 0)
			{
				byte b5 = (byte)((b4 <= 220) ? 35 : (255 - b4));
				b += (byte)(b5 * (b / (float)b4) + 0.5);
				b2 += (byte)(b5 * (b2 / (float)b4) + 0.5);
				b3 += (byte)(b5 * (b3 / (float)b4) + 0.5);
				return Color.FromArgb(b, b2, b3);
			}
			return Color.FromArgb(35, 35, 35);
		}

		public override void StartRenderSession(HotkeyPrefix hotKeys)
		{
			_backBrush = new SolidBrush(TabStripBackgroundColor);
			pen_0 = new Pen(_shadowColor);
			pen_1 = new Pen(_highlightColor);
			solidBrush_1 = new SolidBrush(BackgroundTabForeColor);
			pen_2 = new Pen(_collapsedTabOutlineColor);
		    stringFormat_2 = new StringFormat(StringFormat.GenericDefault)
		    {
		        FormatFlags = StringFormatFlags.NoWrap,
		        Alignment = StringAlignment.Center,
		        LineAlignment = StringAlignment.Center,
		        HotkeyPrefix = hotKeys
		    };
		}

		public override string ToString() => "Everett";

	    internal virtual void vmethod_0(Graphics g, Rectangle bounds, DrawItemState state)
		{
		    if ((state & DrawItemState.HotLight) != DrawItemState.HotLight) return;
		    Pen pen;
		    Pen pen2;
		    if ((state & DrawItemState.Selected) != DrawItemState.Selected)
		    {
		        pen = pen_0;
		        pen2 = pen_1;
		    }
		    else
		    {
		        pen2 = pen_0;
		        pen = pen_1;
		    }
		    g.DrawLine(pen2, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
		    g.DrawLine(pen2, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
		    g.DrawLine(pen, bounds.Right - 1, bounds.Bottom - 1, bounds.Right - 1, bounds.Top);
		    g.DrawLine(pen, bounds.Right - 1, bounds.Bottom - 1, bounds.Left, bounds.Bottom - 1);
		}

		public Color ActiveTitleBarColor
		{
			get
			{
				return _activeTitleBarColor;
			}
			set
			{
				_activeTitleBarColor = value;
				CustomColors = true;
			}
		}

		public Color BackgroundTabForeColor { get; set; } = SystemColors.ControlDarkDark;

	    public Color CollapsedTabOutlineColor
		{
			get
			{
				return _collapsedTabOutlineColor;
			}
			set
			{
				_collapsedTabOutlineColor = value;
				CustomColors = true;
			}
		}

		protected internal override int DocumentTabExtra => 0;

	    protected internal override int DocumentTabSize => Control.DefaultFont.Height + 6;

	    protected internal override int DocumentTabStripSize => Control.DefaultFont.Height + 8;

	    public Color HighlightColor
		{
			get
			{
				return _highlightColor;
			}
			set
			{
				_highlightColor = value;
				CustomColors = true;
			}
		}

		public Color InactiveTitleBarColor
		{
			get
			{
				return _inactiveTitleBarColor;
			}
			set
			{
				_inactiveTitleBarColor = value;
				CustomColors = true;
			}
		}

		internal virtual int Int32_0 { get; } = 5;

	    public Color ShadowColor
		{
			get
			{
				return _shadowColor;
			}
			set
			{
				_shadowColor = value;
				CustomColors = true;
			}
		}

		internal static StringFormat HorizontalTextFormat
		{
			get
			{
			    if (_horizontalTextFormat != null) return _horizontalTextFormat;
			    _horizontalTextFormat = new StringFormat(StringFormat.GenericDefault)
			    {
			        Alignment = StringAlignment.Near,
			        LineAlignment = StringAlignment.Center,
			        Trimming = StringTrimming.EllipsisCharacter
			    };
			    _horizontalTextFormat.FormatFlags |= StringFormatFlags.NoWrap;
			    return _horizontalTextFormat;
			}
		}

		internal static StringFormat VerticalTextFormat
		{
			get
			{
			    if (_verticalTextFormat != null) return _verticalTextFormat;
			    _verticalTextFormat = new StringFormat(StringFormat.GenericDefault)
			    {
			        Alignment = StringAlignment.Near,
			        LineAlignment = StringAlignment.Center,
			        Trimming = StringTrimming.EllipsisCharacter
			    };
			    _verticalTextFormat.FormatFlags |= StringFormatFlags.NoWrap;
			    _verticalTextFormat.FormatFlags |= StringFormatFlags.DirectionVertical;
			    return _verticalTextFormat;
			}
		}

		public override Size TabControlPadding => new Size(3, 3);

	    protected internal override BoxModel TabMetrics => boxModel_1 ?? (boxModel_1 = new BoxModel(0, 0, 0, 0, 0, 0, 0, 0, 1, 0));

	    public Color TabStripBackgroundColor { get; private set; }

	    protected internal override BoxModel TabStripMetrics => boxModel_0 ?? (boxModel_0 = new BoxModel(0, Control.DefaultFont.Height + 9, 4, 0, 5, 1, 0, 2, 0, 0));

	    protected internal override TabTextDisplayMode TabTextDisplay => TabTextDisplayMode.SelectedTab;

	    protected internal override BoxModel TitleBarMetrics => boxModel_2 ?? (boxModel_2 = new BoxModel(0, SystemInformation.ToolWindowCaptionHeight + 2, 0, 0, 0, 0, 0, 0, 0, 2));

	    private BoxModel boxModel_0;

		private BoxModel boxModel_1;

		private BoxModel boxModel_2;

		private Color _inactiveTitleBarColor = SystemColors.InactiveCaption;

		private Color _activeTitleBarColor = SystemColors.ActiveCaption;

		private Color _shadowColor = SystemColors.ControlText;

		private Color _highlightColor = SystemColors.ControlLightLight;

	    private Color _collapsedTabOutlineColor = SystemColors.ControlDark;

		private Color color_6 = SystemColors.ControlDarkDark;

	    private Pen pen_0;

		private Pen pen_1;

		private Pen pen_2;

		private SolidBrush _backBrush;

		private SolidBrush solidBrush_1;

		private static StringFormat _horizontalTextFormat;

		private static StringFormat _verticalTextFormat;

		private StringFormat stringFormat_2;
	}
}
