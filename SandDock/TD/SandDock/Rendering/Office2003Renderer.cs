using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
    public class Office2003Renderer : ThemeAwareRendererBase
    {
        public Office2003Renderer()
        {
        }

        public Office2003Renderer(WindowsColorScheme colorScheme)
        {
            ColorScheme = colorScheme;
        }

        protected internal override Rectangle AdjustDockControlClientBounds(ControlLayoutSystem layoutSystem, DockControl control, Rectangle clientBounds)
        {
            if (layoutSystem is DocumentLayoutSystem)
            {
                clientBounds.Inflate(-4, -4);
                return clientBounds;
            }
            return base.AdjustDockControlClientBounds(layoutSystem, control, clientBounds);
        }

        protected override void ApplyLunaBlueColors()
        {
            LayoutBackgroundColor1 = Color.FromArgb(158, 190, 245);
            LayoutBackgroundColor2 = Color.FromArgb(195, 218, 249);
            _highlightBorderColor = Color.FromArgb(0, 0, 128);
            _highlightBackgroundColor1 = Color.FromArgb(255, 244, 204);
            _highlightBackgroundColor2 = Color.FromArgb(255, 211, 142);
            _inactiveTitleBarColor1 = Color.FromArgb(221, 236, 254);
            _inactiveTitleBarColor2 = Color.FromArgb(129, 169, 226);
            _activeTitleBarColor1 = Color.FromArgb(255, 211, 142);
            _activeTitleBarColor2 = Color.FromArgb(254, 145, 78);
            _gripperColor = Color.FromArgb(39, 65, 118);
            _documentStripBackgroundColor1 = Color.FromArgb(196, 218, 250);
            _documentStripBackgroundColor2 = SystemColors.ControlLightLight;
            _activeDocumentBorderColor = Color.FromArgb(59, 97, 156);
            _inactiveDocumentBorderColor = Color.FromArgb(0, 53, 154);
            _activeDocumentHighlightColor = SystemColors.ControlLightLight;
            _inactiveDocumentHighlightColor = SystemColors.ControlLightLight;
            _activeDocumentShadowColor = SystemColors.ControlLightLight;
            _inactiveDocumentShadowColor = Color.FromArgb(117, 166, 241);
            _widgetColor = SystemColors.ControlText;
        }

        protected override void ApplyLunaOliveColors()
        {
            LayoutBackgroundColor1 = Color.FromArgb(217, 217, 167);
            LayoutBackgroundColor2 = Color.FromArgb(242, 240, 228);
            _highlightBorderColor = Color.FromArgb(63, 93, 56);
            _highlightBackgroundColor1 = Color.FromArgb(255, 244, 204);
            _highlightBackgroundColor2 = Color.FromArgb(255, 211, 142);
            _inactiveTitleBarColor1 = Color.FromArgb(244, 247, 222);
            _inactiveTitleBarColor2 = Color.FromArgb(183, 198, 145);
            _activeTitleBarColor1 = Color.FromArgb(255, 211, 142);
            _activeTitleBarColor2 = Color.FromArgb(254, 145, 78);
            _gripperColor = Color.FromArgb(81, 94, 51);
            _documentStripBackgroundColor1 = Color.FromArgb(242, 241, 228);
            _documentStripBackgroundColor2 = SystemColors.ControlLightLight;
            _activeDocumentBorderColor = Color.FromArgb(96, 128, 88);
            _inactiveDocumentBorderColor = Color.FromArgb(96, 119, 107);
            _activeDocumentHighlightColor = SystemColors.ControlLightLight;
            _inactiveDocumentHighlightColor = SystemColors.ControlLightLight;
            _activeDocumentShadowColor = SystemColors.ControlLightLight;
            _inactiveDocumentShadowColor = Color.FromArgb(176, 194, 140);
            _widgetColor = SystemColors.ControlText;
        }

        protected override void ApplyLunaSilverColors()
        {
            LayoutBackgroundColor1 = Color.FromArgb(215, 215, 229);
            LayoutBackgroundColor2 = Color.FromArgb(243, 243, 247);
            _highlightBorderColor = Color.FromArgb(75, 75, 111);
            _highlightBackgroundColor1 = Color.FromArgb(255, 244, 204);
            _highlightBackgroundColor2 = Color.FromArgb(255, 211, 142);
            _inactiveTitleBarColor1 = Color.FromArgb(243, 244, 250);
            _inactiveTitleBarColor2 = Color.FromArgb(140, 138, 172);
            _activeTitleBarColor1 = Color.FromArgb(255, 211, 142);
            _activeTitleBarColor2 = Color.FromArgb(254, 145, 78);
            _gripperColor = Color.FromArgb(84, 84, 117);
            _documentStripBackgroundColor1 = Color.FromArgb(243, 243, 247);
            _documentStripBackgroundColor2 = SystemColors.ControlLightLight;
            _activeDocumentBorderColor = Color.FromArgb(124, 124, 148);
            _inactiveDocumentBorderColor = Color.FromArgb(118, 116, 146);
            _activeDocumentHighlightColor = SystemColors.ControlLightLight;
            _inactiveDocumentHighlightColor = SystemColors.ControlLightLight;
            _activeDocumentShadowColor = SystemColors.ControlLightLight;
            _inactiveDocumentShadowColor = Color.FromArgb(186, 185, 206);
            _widgetColor = SystemColors.ControlText;
        }

        protected override void ApplyStandardColors()
        {
            if (SystemInformation.HighContrast)
            {
                LayoutBackgroundColor1 = SystemColors.Control;
                LayoutBackgroundColor2 = SystemColors.Control;
                _activeDocumentBorderColor = SystemColors.ActiveCaption;
                _inactiveDocumentBorderColor = SystemColors.ControlDark;
                _activeDocumentHighlightColor = SystemColors.Control;
                _inactiveDocumentHighlightColor = SystemColors.Control;
                _activeDocumentShadowColor = SystemColors.Control;
                _inactiveDocumentShadowColor = SystemColors.Control;
            }
            else
            {
                LayoutBackgroundColor1 = SystemColors.Control;
                LayoutBackgroundColor2 = InterpolateColors(SystemColors.Control, SystemColors.Window, 0.8f);
                _activeDocumentBorderColor = SystemColors.ControlDark;
                _inactiveDocumentBorderColor = SystemColors.ControlDark;
                _activeDocumentHighlightColor = SystemColors.ControlLightLight;
                _inactiveDocumentHighlightColor = SystemColors.Control;
                _activeDocumentShadowColor = SystemColors.ControlLightLight;
                _inactiveDocumentShadowColor = SystemColors.Control;
            }
            _highlightBorderColor = SystemColors.Highlight;
            _highlightBackgroundColor1 = InterpolateColors(_highlightBorderColor, SystemColors.Window, 0.7f);
            _highlightBackgroundColor2 = _highlightBackgroundColor1;
            _inactiveTitleBarColor1 = LayoutBackgroundColor2;
            _inactiveTitleBarColor2 = InterpolateColors(SystemColors.Control, Color.Black, 0.03f);
            _activeTitleBarColor1 = Color.FromArgb(212, 213, 216);
            _activeTitleBarColor2 = Color.FromArgb(212, 213, 216);
            _gripperColor = SystemColors.ControlDark;
            _documentStripBackgroundColor1 = SystemColors.Control;
            _documentStripBackgroundColor2 = SystemColors.ControlLightLight;
            _widgetColor = SystemColors.ControlText;
        }

        protected internal override void DrawCollapsedTab(Graphics graphics, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool vertical)
        {
            if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                RenderHelper.smethod_5(graphics, bounds, dockSide, image, text, font, SystemBrushes.ControlText,
                    SystemColors.ControlDarkDark, TabTextDisplay == TabTextDisplayMode.AllTabs);
            else
                RenderHelper.smethod_5(graphics, bounds, dockSide, image, text, font, SystemBrushes.ControlText,
                    SystemColors.ControlDarkDark, TabTextDisplay == TabTextDisplayMode.AllTabs);
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
            using (var brush = new LinearGradientBrush(new Point(bounds.X, bounds.Y - 1), new Point(bounds.X, bounds.Bottom), _documentStripBackgroundColor1, _documentStripBackgroundColor2))
                graphics.FillRectangle(brush, bounds);
            using (var pen = new Pen(_activeDocumentBorderColor))
                graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
        }

        protected internal override void DrawDocumentStripButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
        {
            method_2(graphics, bounds, state);
            if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                bounds.Offset(1, 1);
            switch (buttonType)
            {
                case SandDockButtonType.Close:
                    using (var pen = new Pen(_widgetColor))
                        ButtonRenderHelper.DrawDocumentStripCloseButton(graphics, bounds, pen);
                    return;
                case SandDockButtonType.Pin:
                case SandDockButtonType.WindowPosition:
                    return;
                case SandDockButtonType.ScrollLeft:
                    ButtonRenderHelper.DrawScrollLeftDockButton(graphics, bounds, _widgetColor, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
                    return;
                case SandDockButtonType.ScrollRight:
                    ButtonRenderHelper.DrawScrollRightDockButton(graphics, bounds, _widgetColor, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
                    return;
                case SandDockButtonType.ActiveFiles:
                    using (var pen2 = new Pen(_widgetColor))
                        ButtonRenderHelper.DrawPositionDockButton(graphics, bounds, pen2);
                    return;
                default:
                    return;
            }
        }

        protected internal override void DrawDocumentStripTab(Graphics graphics, Rectangle bounds, Rectangle contentBounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
        {
            var color_ = InterpolateColors(backColor, SystemColors.ControlLightLight, 0.78f);
            var @checked = (state & DrawItemState.Checked) == DrawItemState.Checked;
            if ((state & DrawItemState.Selected) != DrawItemState.Selected)
                RenderHelper.smethod_1(graphics, bounds, contentBounds, image, ImageSize, text, font, color_, backColor,
                    SystemBrushes.ControlText, _inactiveDocumentBorderColor, _inactiveDocumentHighlightColor, _inactiveDocumentShadowColor, false, DocumentTabSize, DocumentTabExtra,
                    TextFormat, @checked);
            else
                RenderHelper.smethod_1(graphics, bounds, contentBounds, image, ImageSize, text, font, color_, backColor,
                    SystemBrushes.ControlText, _activeDocumentBorderColor, _activeDocumentHighlightColor, _activeDocumentShadowColor, true, DocumentTabSize, DocumentTabExtra, TextFormat,
                    @checked);
        }

        protected internal override void DrawTabStripTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
        {
            if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                RenderHelper.smethod_3(graphics, bounds, image, size_1, text, font, _highlightBackgroundColor1, _highlightBackgroundColor2, SystemColors.ControlText,
                    SystemColors.ControlDark, state, TextFormat);
            else
                RenderHelper.smethod_3(graphics, bounds, image, size_1, text, font, backColor, SystemColors.ControlLightLight,
                    SystemColors.ControlText, SystemColors.ControlDark, state, TextFormat);
        }

        protected internal override void DrawTitleBarBackground(Graphics graphics, Rectangle bounds, bool focused)
        {
            if (focused)
            {
                using (var brush = new LinearGradientBrush(bounds, _activeTitleBarColor1, _activeTitleBarColor2, LinearGradientMode.Vertical))
                {
                    graphics.FillRectangle(brush, bounds);
                    goto IL_54;
                }
            }
            using (var brush = method_3(bounds, LinearGradientMode.Vertical, _inactiveTitleBarColor1, _inactiveTitleBarColor2))
                graphics.FillRectangle(brush, bounds);
            IL_54:
            bounds.Inflate(0, -2);
            using (var brush = new SolidBrush(_gripperColor))
            {
                int num2 = (bounds.Height - 2) / 4 * 4 - 2;
                int num3 = bounds.X + 3;
                int num4 = bounds.Y + bounds.Height / 2 - num2 / 2;
                for (int i = num4; i <= num4 + num2; i += 4)
                {
                    graphics.FillRectangle(SystemBrushes.ControlLightLight, new Rectangle(num3 + 1, i + 1, 2, 2));
                    graphics.FillRectangle(brush, new Rectangle(num3, i, 2, 2));
                }
            }
        }

        protected internal override void DrawTitleBarButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state, bool focused, bool toggled)
        {
            bounds.Width--;
            bounds.Height--;
            method_2(graphics, bounds, state);
            if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                bounds.Offset(1, 1);
            switch (buttonType)
            {
                case SandDockButtonType.Close:
                    ButtonRenderHelper.DrawCloseDockButton(graphics, bounds, SystemPens.ControlText);
                    return;
                case SandDockButtonType.Pin:
                    ButtonRenderHelper.DrawPinDockButton(graphics, bounds, SystemPens.ControlText, toggled);
                    return;
                case SandDockButtonType.ScrollLeft:
                case SandDockButtonType.ScrollRight:
                    break;
                case SandDockButtonType.WindowPosition:
                    ButtonRenderHelper.DrawPositionDockButton(graphics, bounds, SystemPens.ControlText);
                    break;
                default:
                    return;
            }
        }

        protected internal override void DrawTitleBarText(Graphics graphics, Rectangle bounds, bool focused, string text, Font font)
        {
            bounds.Inflate(-3, 0);
            using (var font2 = new Font(font, FontStyle.Bold))
            {
                var format = TextFormat;
                format |= TextFormatFlags.NoPrefix;
                bounds.X += 3;
                TextRenderer.DrawText(graphics, text, font2, bounds, SystemColors.ControlText, format);
            }
        }

        protected internal override Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
        {
            var format = TextFormat;
            format &= ~TextFormatFlags.NoPrefix;
            int w;
            if ((state & DrawItemState.Focus) != DrawItemState.Focus)
            {
                w = TextRenderer.MeasureText(graphics, text, font, new Size(2147483647, 2147483647), format).Width;
            }
            else
            {
                using (var font2 = new Font(font, FontStyle.Bold))
                {
                    w = TextRenderer.MeasureText(graphics, text, font2, new Size(2147483647, 2147483647), format).Width;
                }
            }
            w += 24;
            if (image != null)
                w += 20;
            w += DocumentTabExtra;
            return new Size(w, 0);
        }

        protected internal override Size MeasureTabStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
        {
            return RenderHelper.MeasureTabStripTab(graphics, image, ImageSize, text, font, TextFormat);
        }

        private void method_2(Graphics g, Rectangle bounds, DrawItemState state)
        {
            if ((state & DrawItemState.HotLight) != DrawItemState.HotLight) return;
            using (var brush = new LinearGradientBrush(bounds, _highlightBackgroundColor1, _highlightBackgroundColor2, LinearGradientMode.Vertical))
                g.FillRectangle(brush, bounds);
            using (var pen = new Pen(_highlightBorderColor))
                g.DrawRectangle(pen, bounds);
        }

        private Brush method_3(Rectangle rect, LinearGradientMode linearGradientMode, Color color1, Color color2)
        {
            var color = InterpolateColors(color1, color2, 0.25f);
            return new LinearGradientBrush(rect, color1, color2, linearGradientMode)
            {
                InterpolationColors = new ColorBlend(3) { Colors = new[] { color1, color, color2 }, Positions = new[] { 0f, 0.5f, 1f } }
            };
        }

        public override string ToString() => "Office 2003";

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

        public Color ActiveTitleBarColor1
        {
            get
            {
                return _activeTitleBarColor1;
            }
            set
            {
                _activeTitleBarColor1 = value;
                CustomColors = true;
            }
        }

        public Color ActiveTitleBarColor2
        {
            get
            {
                return _activeTitleBarColor2;
            }
            set
            {
                _activeTitleBarColor2 = value;
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

        protected internal override int DocumentTabExtra => 18;

        protected internal override int DocumentTabSize => Control.DefaultFont.Height + 7;

        protected internal override int DocumentTabStripSize => Control.DefaultFont.Height + 15;

        public Color GripperColor
        {
            get
            {
                return _gripperColor;
            }
            set
            {
                _gripperColor = value;
                CustomColors = true;
            }
        }

        public Color HighlightBackgroundColor1
        {
            get
            {
                return _highlightBackgroundColor1;
            }
            set
            {
                _highlightBackgroundColor1 = value;
                CustomColors = true;
            }
        }

        public Color HighlightBackgroundColor2
        {
            get
            {
                return _highlightBackgroundColor2;
            }
            set
            {
                _highlightBackgroundColor2 = value;
                CustomColors = true;
            }
        }

        public Color HighlightBorderColor
        {
            get
            {
                return _highlightBorderColor;
            }
            set
            {
                _highlightBorderColor = value;
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

        public Color InactiveTitleBarColor1
        {
            get
            {
                return _inactiveTitleBarColor1;
            }
            set
            {
                _inactiveTitleBarColor1 = value;
                CustomColors = true;
            }
        }

        public Color InactiveTitleBarColor2
        {
            get
            {
                return _inactiveTitleBarColor2;
            }
            set
            {
                _inactiveTitleBarColor2 = value;
                CustomColors = true;
            }
        }

        public override Size TabControlPadding => new Size(3, 3);

        protected internal override BoxModel TabMetrics => _tabMetrics ?? (_tabMetrics = new BoxModel(0, 0, 0, 0, 0, 0, 0, 0, -1, 0));

        protected internal override BoxModel TabStripMetrics => _tabStripMetrics ?? (_tabStripMetrics = new BoxModel(0, Control.DefaultFont.Height + 10, 0, 0, 0, 1, 0, 0, 0, 0));

        protected internal override TabTextDisplayMode TabTextDisplay => TabTextDisplayMode.SelectedTab;

        protected internal override BoxModel TitleBarMetrics => new BoxModel(0, 25, 4, 0, 0, 0, 0, 0, 0, 0);

        public Color WidgetColor
        {
            get
            {
                return _widgetColor;
            }
            set
            {
                _widgetColor = value;
                CustomColors = true;
            }
        }

        private BoxModel _tabStripMetrics;

        private BoxModel _tabMetrics;

        private Color _inactiveDocumentHighlightColor;

        private Color _activeDocumentShadowColor;

        private Color _inactiveDocumentShadowColor;

        private Color _widgetColor;

        private Color _activeTitleBarColor1;

        private Color _activeTitleBarColor2;

        private Color _inactiveTitleBarColor1;

        private Color _inactiveTitleBarColor2;

        private Color _gripperColor;

        private Color _highlightBorderColor;

        private Color _highlightBackgroundColor1;

        private Color _highlightBackgroundColor2;

        private Color _documentStripBackgroundColor1;

        private Color _documentStripBackgroundColor2;

        private Color _activeDocumentBorderColor;

        private Color _inactiveDocumentBorderColor;

        private Color _activeDocumentHighlightColor;

        private readonly Size size_1 = new Size(16, 16);
    }
}
