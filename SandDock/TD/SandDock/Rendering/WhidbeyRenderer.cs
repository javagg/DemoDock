using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
	public class WhidbeyRenderer : ThemeAwareRendererBase
	{
		public WhidbeyRenderer()
		{
		}

		public WhidbeyRenderer(WindowsColorScheme colorScheme)
		{
			ColorScheme = colorScheme;
		}

		protected override void ApplyLunaBlueColors()
		{
			LayoutBackgroundColor1 = Color.FromArgb(229, 229, 215);
			LayoutBackgroundColor2 = Color.FromArgb(243, 242, 231);
			_documentStripBackgroundColor1 = Color.FromArgb(228, 226, 213);
			_documentStripBackgroundColor2 = _documentStripBackgroundColor1;
			ActiveDocumentBorderColor = Color.FromArgb(127, 157, 185);
			InactiveDocumentBorderColor = SystemColors.ControlDark;
			_activeTitleBarBackgroundColor1 = Color.FromArgb(59, 128, 237);
			_activeTitleBarBackgroundColor2 = Color.FromArgb(49, 106, 197);
			_activeTitleBarForegroundColor = Color.White;
			_inactiveTitleBarBackgroundColor = Color.FromArgb(204, 199, 186);
			_inactiveTitleBarForegroundColor = Color.Black;
			_inactiveButtonBackgroundColor = SystemColors.Control;
			_inactiveButtonBorderColor = Color.FromArgb(140, 134, 123);
			_activeButtonBackgroundColor = Color.FromArgb(156, 182, 231);
			_activeButtonBorderColor = Color.FromArgb(60, 90, 170);
			_activeHotButtonBackgroundColor = Color.FromArgb(120, 150, 210);
			_activeHotButtonBorderColor = Color.FromArgb(60, 90, 170);
		}

		protected override void ApplyLunaOliveColors()
		{
			LayoutBackgroundColor1 = Color.FromArgb(229, 229, 215);
			LayoutBackgroundColor2 = Color.FromArgb(243, 242, 231);
			_documentStripBackgroundColor1 = Color.FromArgb(228, 226, 213);
			_documentStripBackgroundColor2 = _documentStripBackgroundColor1;
			ActiveDocumentBorderColor = Color.FromArgb(127, 157, 185);
			InactiveDocumentBorderColor = SystemColors.ControlDark;
			_activeTitleBarBackgroundColor1 = Color.FromArgb(182, 195, 146);
			_activeTitleBarBackgroundColor2 = Color.FromArgb(145, 160, 117);
			_activeTitleBarForegroundColor = Color.White;
			_inactiveTitleBarBackgroundColor = Color.FromArgb(204, 199, 186);
			_inactiveTitleBarForegroundColor = Color.Black;
			_inactiveButtonBackgroundColor = SystemColors.Control;
			_inactiveButtonBorderColor = Color.FromArgb(140, 134, 123);
			_activeButtonBackgroundColor = Color.FromArgb(181, 199, 140);
			_activeButtonBorderColor = Color.FromArgb(118, 128, 95);
			_activeHotButtonBackgroundColor = Color.FromArgb(148, 162, 115);
			_activeHotButtonBorderColor = Color.FromArgb(118, 128, 95);
		}

		protected override void ApplyLunaSilverColors()
		{
			LayoutBackgroundColor1 = Color.FromArgb(215, 215, 229);
			LayoutBackgroundColor2 = Color.FromArgb(243, 243, 247);
			_documentStripBackgroundColor1 = Color.FromArgb(238, 238, 238);
			_documentStripBackgroundColor2 = _documentStripBackgroundColor1;
			ActiveDocumentBorderColor = Color.FromArgb(127, 157, 185);
			InactiveDocumentBorderColor = SystemColors.ControlDark;
			_activeTitleBarBackgroundColor1 = Color.FromArgb(211, 212, 221);
			_activeTitleBarBackgroundColor2 = Color.FromArgb(166, 165, 191);
			_activeTitleBarForegroundColor = Color.Black;
			_inactiveTitleBarBackgroundColor = Color.FromArgb(240, 240, 245);
			_inactiveTitleBarForegroundColor = Color.Black;
			_inactiveButtonBackgroundColor = Color.FromArgb(214, 215, 222);
			_inactiveButtonBorderColor = Color.FromArgb(123, 125, 148);
			_activeButtonBackgroundColor = Color.FromArgb(255, 227, 173);
			_activeButtonBorderColor = Color.FromArgb(74, 73, 107);
			_activeHotButtonBackgroundColor = Color.FromArgb(255, 182, 115);
			_activeHotButtonBorderColor = Color.FromArgb(74, 73, 107);
		}

		protected override void ApplyStandardColors()
		{
			if (SystemInformation.HighContrast)
			{
				LayoutBackgroundColor1 = SystemColors.Control;
				LayoutBackgroundColor2 = SystemColors.Control;
				ActiveDocumentBorderColor = SystemColors.ActiveCaption;
				InactiveDocumentBorderColor = SystemColors.ControlDark;
			}
			else
			{
				LayoutBackgroundColor1 = SystemColors.Control;
				LayoutBackgroundColor2 = InterpolateColors(SystemColors.Control, SystemColors.Window, 0.8f);
				ActiveDocumentBorderColor = SystemColors.AppWorkspace;
				InactiveDocumentBorderColor = SystemColors.ControlDark;
			}
			_documentStripBackgroundColor1 = SystemColors.Control;
			_documentStripBackgroundColor2 = _documentStripBackgroundColor1;
			_activeTitleBarBackgroundColor1 = SystemColors.ActiveCaption;
			_activeTitleBarBackgroundColor2 = SystemColors.ActiveCaption;
			_activeTitleBarForegroundColor = SystemColors.ActiveCaptionText;
			_inactiveTitleBarBackgroundColor = SystemColors.InactiveCaption;
			_inactiveTitleBarForegroundColor = SystemColors.InactiveCaptionText;
			_inactiveButtonBackgroundColor = Color.Transparent;
			_inactiveButtonBorderColor = SystemColors.ControlLightLight;
			_activeButtonBackgroundColor = Color.Transparent;
			_activeButtonBorderColor = SystemColors.ControlLightLight;
			_activeHotButtonBackgroundColor = (SystemInformation.HighContrast ? Color.Transparent : Color.FromArgb(100, SystemColors.Control));
			_activeHotButtonBorderColor = SystemColors.ControlLightLight;
		}

		protected internal override void DrawCollapsedTab(Graphics graphics, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool vertical)
		{
			if (dockSide != DockSide.Left && dockSide != DockSide.Right)
			{
				RenderHelper.smethod_5(graphics, bounds, dockSide, image, text, font, SystemBrushes.ControlDarkDark, SystemColors.ControlDark, TabTextDisplay == TabTextDisplayMode.AllTabs);
			}
			else
			{
				using (Image image2 = new Bitmap(image))
				{
					image2.RotateFlip(RotateFlipType.Rotate90FlipNone);
					RenderHelper.smethod_5(graphics, bounds, dockSide, image2, text, font, SystemBrushes.ControlDarkDark, SystemColors.ControlDark, TabTextDisplay == TabTextDisplayMode.AllTabs);
				}
			}
		}

		protected internal override void DrawControlClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
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
		    if (bounds.Width <= 0 || bounds.Height <= 0) return;
		    using (var brush = new LinearGradientBrush(new Point(bounds.X, bounds.Y - 1), new Point(bounds.X, bounds.Bottom), _documentStripBackgroundColor1, _documentStripBackgroundColor2))
		        graphics.FillRectangle(brush, bounds);
		    using (var pen = new Pen(_activeDocumentBorderColor))
		        graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
		}

		protected internal override void DrawDocumentStripButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
		{
			vmethod_0(graphics, bounds, state, true);
			switch (buttonType)
			{
			case SandDockButtonType.Close:
				ButtonRenderHelper.DrawCloseDockButton(graphics, bounds, SystemPens.ControlText);
				return;
			case SandDockButtonType.Pin:
			case SandDockButtonType.WindowPosition:
				break;
			case SandDockButtonType.ScrollLeft:
				ButtonRenderHelper.DrawScrollLeftDockButton(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
				return;
			case SandDockButtonType.ScrollRight:
				ButtonRenderHelper.DrawScrollRightDockButton(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
				return;
			case SandDockButtonType.ActiveFiles:
				bounds.Inflate(1, 1);
				bounds.X--;
				ButtonRenderHelper.DrawPositionDockButton(graphics, bounds, SystemPens.ControlText);
				break;
			default:
				return;
			}
		}

		protected internal override void DrawDocumentStripTab(Graphics graphics, Rectangle bounds, Rectangle contentBounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			bool bool_ = (state & DrawItemState.Checked) == DrawItemState.Checked;
			if ((state & DrawItemState.Selected) != DrawItemState.Selected)
			{
				RenderHelper.smethod_1(graphics, bounds, contentBounds, image, ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemInformation.HighContrast ? SystemColors.Control : backColor, SystemBrushes.ControlText, InactiveDocumentBorderColor, _inactiveDocumentHighlightColor, _inactiveDocumentShadowColor, false, DocumentTabSize, DocumentTabExtra, TextFormat, bool_);
				return;
			}
			RenderHelper.smethod_1(graphics, bounds, contentBounds, image, ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemBrushes.ControlText, ActiveDocumentBorderColor, _activeDocumentHighlightColor, _activeDocumentShadowColor, true, DocumentTabSize, DocumentTabExtra, TextFormat, bool_);
		}

		protected internal override void DrawTabStripBackground(Control container, Control control, Graphics graphics, Rectangle bounds, int selectedTabOffset)
		{
			base.DrawTabStripBackground(container, control, graphics, bounds, selectedTabOffset);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Top + 2, bounds.Right - 1, bounds.Top + 2);
			if (!SystemInformation.HighContrast)
			{
				using (var pen = new Pen(SystemColors.ControlLightLight))
				{
					graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
					graphics.DrawLine(pen, bounds.Left, bounds.Top + 1, bounds.Right - 1, bounds.Top + 1);
				}
			}
		}

		protected internal override void DrawTabStripTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			bounds.Y += 2;
			bounds.Height -= 2;
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				RenderHelper.smethod_3(graphics, bounds, image, ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemColors.ControlText, SystemColors.ControlDark, state, TextFormat);
			}
			else
			{
				RenderHelper.smethod_3(graphics, bounds, image, ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : backColor, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemColors.ControlDarkDark, SystemColors.ControlDark, state, TextFormat);
			}
			if ((state & DrawItemState.Selected) != DrawItemState.Selected && drawSeparator)
			{
				graphics.DrawLine(SystemPens.ControlDark, bounds.Right - 2, bounds.Top + 3, bounds.Right - 2, bounds.Bottom - 4);
			}
		}

		protected internal override void DrawTitleBarBackground(Graphics graphics, Rectangle bounds, bool focused)
		{
		    if (focused)
		        using (var brush = new LinearGradientBrush(new Point(bounds.X, bounds.Y - 1), new Point(bounds.X, bounds.Bottom), _activeTitleBarBackgroundColor1, _activeTitleBarBackgroundColor2))
		            graphics.FillRectangle(brush, bounds);
		    else
		        using (var brush = new SolidBrush(_inactiveTitleBarBackgroundColor))
		            graphics.FillRectangle(brush, bounds);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
		}

		protected internal override void DrawTitleBarButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state, bool focused, bool toggled)
		{
			vmethod_0(graphics, bounds, state, focused);
			using (var pen = focused ? new Pen(_activeTitleBarForegroundColor) : new Pen(_inactiveTitleBarForegroundColor))
			{
				switch (buttonType)
				{
				case SandDockButtonType.Close:
					ButtonRenderHelper.DrawCloseDockButton(graphics, bounds, pen);
					break;
				case SandDockButtonType.Pin:
					ButtonRenderHelper.DrawPinDockButton(graphics, bounds, pen, toggled);
					break;
				case SandDockButtonType.WindowPosition:
					ButtonRenderHelper.DrawPositionDockButton(graphics, bounds, pen);
					break;
				}
			}
		}

		protected internal override void DrawTitleBarText(Graphics graphics, Rectangle bounds, bool focused, string text, Font font)
		{
			bounds.Inflate(-3, 0);
			var format = TextFormat;
			format |= TextFormatFlags.NoPrefix;
			bounds.X += 3;
			TextRenderer.DrawText(graphics, text, font, bounds, focused ? _activeTitleBarForegroundColor : _inactiveTitleBarForegroundColor, format);
		}

		protected override void GetColorsFromSystem()
		{
			base.GetColorsFromSystem();
		    if (SystemInformation.HighContrast)
		    {
		        _activeDocumentHighlightColor = SystemColors.Control;
		        _inactiveDocumentHighlightColor = SystemColors.Control;
		        _activeDocumentShadowColor = SystemColors.Control;
		        _inactiveDocumentShadowColor = SystemColors.Control;
		    }
		    else
		    {
		        _activeDocumentHighlightColor = SystemColors.ControlLightLight;
		        _inactiveDocumentHighlightColor = SystemColors.ControlLightLight;
		        _activeDocumentShadowColor = SystemColors.ControlLightLight;
		        _inactiveDocumentShadowColor = SystemColors.Control;
		    }
		}

		protected internal override Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			TextFormatFlags textFormatFlags = TextFormat;
			textFormatFlags &= ~TextFormatFlags.NoPrefix;
			int num;
			using (Font font2 = new Font(font, FontStyle.Bold))
			{
				num = TextRenderer.MeasureText(graphics, text, font2, new Size(2147483647, 2147483647), textFormatFlags).Width;
			}
			num += 14;
			if (image != null)
			{
				num += 20;
			}
			num += DocumentTabExtra;
			return new Size(num, 0);
		}

		protected internal override Size MeasureTabStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			return RenderHelper.MeasureTabStripTab(graphics, image, ImageSize, text, font, TextFormat);
		}

		private void ResetMetrics()
		{
			_tabStripMetrics = null;
			_tabMetrics = null;
			_titleBarMetrics = null;
		}

		internal static bool IsSupported() => Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(5, 1, 0, 0);

		public override string ToString() => "Whidbey";

	    internal virtual void vmethod_0(Graphics g, Rectangle bounds, DrawItemState state, bool bool_1)
		{
			if ((state & DrawItemState.HotLight) == DrawItemState.HotLight)
			{
				Color color;
				Color color2;
				Color color3;
				if (!bool_1)
				{
					color = _inactiveButtonBorderColor;
					color2 = _inactiveButtonBorderColor;
					color3 = _inactiveButtonBackgroundColor;
				}
				else if ((state & DrawItemState.Selected) != DrawItemState.Selected)
				{
					color = _activeButtonBorderColor;
					color2 = _activeButtonBorderColor;
					color3 = _activeButtonBackgroundColor;
				}
				else
				{
					color = _activeHotButtonBorderColor;
					color2 = _activeHotButtonBorderColor;
					color3 = _activeHotButtonBackgroundColor;
				}
				using (SolidBrush solidBrush = new SolidBrush(color3))
				{
					g.FillRectangle(solidBrush, bounds);
				}
				using (Pen pen = new Pen(color))
				{
					g.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
					g.DrawLine(pen, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
				}
				using (Pen pen2 = new Pen(color2))
				{
					g.DrawLine(pen2, bounds.Right - 1, bounds.Bottom - 1, bounds.Right - 1, bounds.Top);
					g.DrawLine(pen2, bounds.Right - 1, bounds.Bottom - 1, bounds.Left, bounds.Bottom - 1);
				}
			}
		}

		public Color ActiveButtonBackgroundColor
		{
			get
			{
				return _activeButtonBackgroundColor;
			}
			set
			{
				_activeButtonBackgroundColor = value;
				CustomColors = true;
			}
		}

		public Color ActiveButtonBorderColor
		{
			get
			{
				return _activeButtonBorderColor;
			}
			set
			{
				_activeButtonBorderColor = value;
				CustomColors = true;
			}
		}

		public Color ActiveDocumentBorderColor
		{
			get
			{
				return _activeDocumentBorderColor;
			}
			set
			{
				_activeDocumentBorderColor = value;
				CustomColors = true;
			}
		}

		public Color ActiveDocumentHighlightColor
		{
			get
			{
				return _activeDocumentHighlightColor;
			}
			set
			{
				_activeDocumentHighlightColor = value;
				CustomColors = true;
			}
		}

		public Color ActiveDocumentShadowColor
		{
			get
			{
				return _activeDocumentShadowColor;
			}
			set
			{
				_activeDocumentShadowColor = value;
				CustomColors = true;
			}
		}

		public Color ActiveHotButtonBackgroundColor
		{
			get
			{
				return _activeHotButtonBackgroundColor;
			}
			set
			{
				_activeHotButtonBackgroundColor = value;
				CustomColors = true;
			}
		}

		public Color ActiveHotButtonBorderColor
		{
			get
			{
				return _activeHotButtonBorderColor;
			}
			set
			{
				_activeHotButtonBorderColor = value;
				CustomColors = true;
			}
		}

		public Color ActiveTitleBarBackgroundColor1
		{
			get
			{
				return _activeTitleBarBackgroundColor1;
			}
			set
			{
				_activeTitleBarBackgroundColor1 = value;
				CustomColors = true;
			}
		}

		public Color ActiveTitleBarBackgroundColor2
		{
			get
			{
				return _activeTitleBarBackgroundColor2;
			}
			set
			{
				_activeTitleBarBackgroundColor2 = value;
				CustomColors = true;
			}
		}

		public Color ActiveTitleBarForegroundColor
		{
			get
			{
				return _activeTitleBarForegroundColor;
			}
			set
			{
				_activeTitleBarForegroundColor = value;
				CustomColors = true;
			}
		}

		public Color DocumentStripBackgroundColor1
		{
			get
			{
				return _documentStripBackgroundColor1;
			}
			set
			{
				_documentStripBackgroundColor1 = value;
				CustomColors = true;
			}
		}

		public Color DocumentStripBackgroundColor2
		{
			get
			{
				return _documentStripBackgroundColor2;
			}
			set
			{
				_documentStripBackgroundColor2 = value;
				CustomColors = true;
			}
		}

		protected internal override int DocumentTabExtra => ImageSize.Width - 4;

	    protected internal override int DocumentTabSize => Math.Max(Control.DefaultFont.Height, ImageSize.Height) + 4;

	    protected internal override int DocumentTabStripSize => Math.Max(Control.DefaultFont.Height, ImageSize.Height) + 5;

	    public override Size ImageSize
		{
			get
			{
				return base.ImageSize;
			}
			set
			{
				ResetMetrics();
				base.ImageSize = value;
			}
		}

		public Color InactiveButtonBackgroundColor
		{
			get
			{
				return _inactiveButtonBackgroundColor;
			}
			set
			{
				_inactiveButtonBackgroundColor = value;
				CustomColors = true;
			}
		}

		public Color InactiveButtonBorderColor
		{
			get
			{
				return _inactiveButtonBorderColor;
			}
			set
			{
				_inactiveButtonBorderColor = value;
				CustomColors = true;
			}
		}

		public Color InactiveDocumentBorderColor
		{
			get
			{
				return _inactiveDocumentBorderColor;
			}
			set
			{
				_inactiveDocumentBorderColor = value;
				CustomColors = true;
			}
		}

		public Color InactiveDocumentHighlightColor
		{
			get
			{
				return _inactiveDocumentHighlightColor;
			}
			set
			{
				_inactiveDocumentHighlightColor = value;
				CustomColors = true;
			}
		}

		public Color InactiveDocumentShadowColor
		{
			get
			{
				return _inactiveDocumentShadowColor;
			}
			set
			{
				_inactiveDocumentShadowColor = value;
				CustomColors = true;
			}
		}

		public Color InactiveTitleBarBackgroundColor
		{
			get
			{
				return _inactiveTitleBarBackgroundColor;
			}
			set
			{
				_inactiveTitleBarBackgroundColor = value;
				CustomColors = true;
			}
		}

		public Color InactiveTitleBarForegroundColor
		{
			get
			{
				return _inactiveTitleBarForegroundColor;
			}
			set
			{
				_inactiveTitleBarForegroundColor = value;
				CustomColors = true;
			}
		}

		public override Size TabControlPadding => new Size(3, 3);

	    protected internal override BoxModel TabMetrics => _tabMetrics ?? (_tabMetrics = new BoxModel(0, 0, 0, 0, 0, 0, 0, 0, -1, 0));

	    protected internal override BoxModel TabStripMetrics
		{
			get
			{
			    if (_tabStripMetrics != null) return _tabStripMetrics;
			    _tabStripMetrics = new BoxModel(0, Math.Max(Control.DefaultFont.Height, ImageSize.Height) + 8, 0, 0, 0, 1, 0, 0, 0, 0);
			    return _tabStripMetrics;
			}
		}

		protected internal override TabTextDisplayMode TabTextDisplay => TabTextDisplayMode.AllTabs;

	    protected internal override BoxModel TitleBarMetrics => _titleBarMetrics ?? (_titleBarMetrics = new BoxModel(0, SystemInformation.ToolWindowCaptionHeight, 0, 0, 0, 0, 0, 0, 0, 0));

	    private BoxModel _tabStripMetrics;

		private BoxModel _tabMetrics;

		private BoxModel _titleBarMetrics;

		private Color _inactiveTitleBarBackgroundColor;

		private Color _inactiveTitleBarForegroundColor;

		private Color _activeTitleBarBackgroundColor1;

		private Color _activeTitleBarBackgroundColor2;

		private Color _activeTitleBarForegroundColor;

		private Color _inactiveButtonBackgroundColor;

		private Color _inactiveButtonBorderColor;

		private Color _activeButtonBackgroundColor;

		private Color _activeButtonBorderColor;

		private Color _activeHotButtonBackgroundColor;

		private Color _activeDocumentBorderColor;

		private Color _activeHotButtonBorderColor;

		private Color _inactiveDocumentBorderColor;

		private Color _activeDocumentHighlightColor;

		private Color _inactiveDocumentHighlightColor;

		private Color _activeDocumentShadowColor;

		private Color _inactiveDocumentShadowColor;

		private Color _documentStripBackgroundColor1;

		private Color _documentStripBackgroundColor2;
	}
}
