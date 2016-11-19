using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
    internal class MilborneRendererConverter : RendererBaseConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) => new StandardValuesCollection(new[] { "Everett", "Office 2003", "Whidbey", "Milborne", "Office 2007" });
    }

    [TypeConverter(typeof(MilborneRendererConverter))]
    public class MilborneRenderer : ITabControlRenderer
    {
        public virtual void DrawFakeTabControlBackgroundExtension(Graphics graphics, Rectangle bounds, Color backColor)
        {
            using (var brush = new SolidBrush(color_7))
                graphics.FillRectangle(brush, bounds);
            using (var pen = new Pen(color_2))
                graphics.DrawLine(pen, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
        }

        public void DrawTabControlBackground(Graphics graphics, Rectangle bounds, Color backColor, bool client)
        {
            if (bounds.Width <= 0 || bounds.Height <= 0) return;
            Color color = Color.FromArgb(252, 252, 254);
            Color color2 = Color.FromArgb(244, 243, 238);
            if (PageColorBlend > 0.0)
            {
                color = RendererBase.InterpolateColors(color, backColor, (float)PageColorBlend);
                color2 = RendererBase.InterpolateColors(color2, backColor, (float)PageColorBlend);
            }
            if (!client)
            {
                Rectangle rect = bounds;
                rect.Height = TabControlPadding.Height;
                using (var brush = new SolidBrush(color))
                    graphics.FillRectangle(brush, rect);
                Rectangle rect2 = bounds;
                rect2.Y += TabControlPadding.Height;
                rect2.Height -= TabControlPadding.Height * 2;
                if (rect2.Width > 0 && rect2.Height > 0)
                {
                    using (var brush = new LinearGradientBrush(rect2, color, color2, LinearGradientMode.Vertical))
                        graphics.FillRectangle(brush, rect2);
                }
                Rectangle rect3 = bounds;
                rect3.Y = rect3.Bottom - TabControlPadding.Height;
                rect3.Height = TabControlPadding.Height;
                using (var brush = new SolidBrush(color2))
                {
                    graphics.FillRectangle(brush, rect3);
                }
                using (var pen = new Pen(color_2))
                {
                    graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 2);
                    graphics.DrawLine(pen, bounds.X, bounds.Bottom - 2, bounds.Right - 2, bounds.Bottom - 2);
                    graphics.DrawLine(pen, bounds.Right - 2, bounds.Bottom - 2, bounds.Right - 2, bounds.Y);
                }
                using (var pen = new Pen(RendererBase.InterpolateColors(color_2, SystemColors.Control, 0.8f)))
                {
                    graphics.DrawLine(pen, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
                    graphics.DrawLine(pen, bounds.Right - 1, bounds.Bottom - 1, bounds.Right - 1, bounds.Y);
                    return;
                }
            }
            using (var brush = new LinearGradientBrush(bounds, color, color2, LinearGradientMode.Vertical))
                graphics.FillRectangle(brush, bounds);
        }

        public void DrawTabControlButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
        {
            if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                bounds.Offset(1, 1);
            switch (buttonType)
            {
                case SandDockButtonType.ScrollLeft:
                    ButtonRenderHelper.DrawScrollLeftDockButton(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
                    return;
                case SandDockButtonType.ScrollRight:
                    ButtonRenderHelper.DrawScrollRightDockButton(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
                    return;
                default:
                    return;
            }
        }

        public void DrawTabControlTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
        {
            int arg_07_0 = bounds.Height;
            var flag = (state & DrawItemState.Selected) == DrawItemState.Selected;

            Color color = flag ? color_4 : color_6;
            Color color2 = flag ? color_5 : color_7;
            if (TabColorBlend > 0.0)
            {
                color = RendererBase.InterpolateColors(color, backColor, (float)TabColorBlend);
                color2 = RendererBase.InterpolateColors(color2, backColor, (float)TabColorBlend);
            }
            using (var brush = new LinearGradientBrush(bounds, color, color2, LinearGradientMode.Vertical))
            {
                graphics.FillPolygon(brush, method_0(bounds, false));
            }
            var smoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(flag ? color_0 : color_2))
                graphics.DrawLines(pen, method_0(bounds, false));
            using (Pen pen2 = new Pen(flag ? color_1 : color_3))
            {
                graphics.DrawLines(pen2, method_0(bounds, true));
                if (!flag)
                {
                    Color color3 = RendererBase.InterpolateColors(color_3, color_2, 0.5f);
                    using (Pen pen3 = new Pen(color3))
                    {
                        graphics.DrawLines(pen3, new[]
                        {
                            new Point(bounds.Right - 4, bounds.Y + 1),
                            new Point(bounds.Right - 2, bounds.Y + 3),
                            new Point(bounds.Right - 2, bounds.Bottom - 2)
                        });
                    }
                }
            }
            if (flag)
            {
                using (Pen pen4 = new Pen(color2))
                {
                    graphics.DrawLine(pen4, bounds.X + 1, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
                }
            }
            graphics.SmoothingMode = smoothingMode;
            if ((state & DrawItemState.Checked) == DrawItemState.Checked)
            {
                var rectangle = bounds;
                rectangle.X += TabControlTabExtra;
                rectangle.Width -= TabControlTabExtra;
                rectangle.Inflate(-4, -3);
                rectangle.X++;
                rectangle.Height++;
                ControlPaint.DrawFocusRectangle(graphics, rectangle);
            }
            bounds.X += TabControlTabExtra + 6;
            bounds.Width -= TabControlTabExtra + 6;
            bounds.Inflate(-2, 0);
            if (image != null)
            {
                Rectangle destRect = new Rectangle(bounds.X, bounds.Y + bounds.Height / 2 - image.Height / 2, image.Width, image.Height);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                bounds.X += image.Width + 4;
                bounds.Width -= image.Width + 4;
            }
            if (bounds.Width > 4)
            {
                if (flag)
                {
                    using (Font font2 = new Font(font, FontStyle.Bold))
                    {
                        TextRenderer.DrawText(graphics, text, font2, bounds, foreColor, _textFormat);
                        return;
                    }
                }
                TextRenderer.DrawText(graphics, text, font, bounds, foreColor, _textFormat);
            }
        }

        public void DrawTabControlTabStripBackground(Graphics graphics, Rectangle bounds, Color backColor)
        {
            if (backColor != Color.Transparent)
                RenderHelper.ClearBackground(graphics, backColor);
            using (var pen = new Pen(color_2))
                graphics.DrawLine(pen, bounds.X, bounds.Bottom - 1, bounds.Right - 2, bounds.Bottom - 1);
        }

        public void FinishRenderSession() { }

        public Size MeasureTabControlTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
        {
            int w;
            using (new Font(font, FontStyle.Bold))
                w = TextRenderer.MeasureText(graphics, text, font, new Size(2147483647, 2147483647), _textFormat).Width;
            w += 24;
            if (image != null)
                w += image.Width + 4;
            w += TabControlTabExtra;
            return new Size(w, 0);
        }

        private Point[] method_0(Rectangle bounds, bool bool_0)
        {
            var min = Math.Min(bounds.Width, bounds.Height);
            return bool_0
                ? new[]
                {
                    new Point(bounds.X + 2, bounds.Bottom - 2),
                    new Point(bounds.X + min - 3, bounds.Y + 3),
                    new Point(bounds.X + min + 1, bounds.Y + 1),
                    new Point(bounds.Right - 4, bounds.Y + 1),
                    new Point(bounds.Right - 2, bounds.Y + 3),
                    new Point(bounds.Right - 2, bounds.Bottom - 2)
                }
                : new[]
                {
                    new Point(bounds.X, bounds.Bottom - 1),
                    new Point(bounds.X + min - 4, bounds.Y + 3),
                    new Point(bounds.X + min + 1, bounds.Y),
                    new Point(bounds.Right - 4, bounds.Y),
                    new Point(bounds.Right - 1, bounds.Y + 3),
                    new Point(bounds.Right - 1, bounds.Bottom - 1)
                };
        }

        public void StartRenderSession(HotkeyPrefix tabHotKeys)
        {
            _textFormat = TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPadding;
            if (tabHotKeys == HotkeyPrefix.None)
            {
                _textFormat |= TextFormatFlags.NoPrefix;
            }
            else if (tabHotKeys == HotkeyPrefix.Hide)
            {
                _textFormat |= TextFormatFlags.HidePrefix;
            }

        }

        public override string ToString() => "Milborne";

        public double PageColorBlend
        {
            get
            {
                return _pageColorBlend;
            }
            set
            {
                if (value < 0.0 || value > 1.0) throw new ArgumentException("Value must lie between 0 and 1.");
                _pageColorBlend = value;
            }
        }

        public virtual bool ShouldDrawControlBorder => false;

        public bool ShouldDrawTabControlBackground => true;

        public double TabColorBlend
        {
            get
            {
                return _tabColorBlend;
            }
            set
            {
                if (value < 0.0 || value > 1.0) throw new ArgumentException("Value must lie between 0 and 1.");
                _tabColorBlend = value;
            }
        }

        public virtual Size TabControlPadding => new Size(4, 4);

        public int TabControlTabExtra => TabControlTabStripHeight - 7;

        public int TabControlTabHeight => TabControlTabStripHeight;

        public int TabControlTabStripHeight => Control.DefaultFont.Height + 8;

        private Color color_0 = Color.FromArgb(124, 124, 148);

        private Color color_1 = SystemColors.ControlLight;

        private Color color_2 = Color.FromArgb(117, 116, 147);

        private Color color_3 = Color.FromArgb(255, 255, 255);

        private Color color_4 = Color.FromArgb(255, 255, 255);

        private Color color_5 = Color.FromArgb(252, 252, 254);

        private Color color_6 = Color.FromArgb(244, 243, 248);

        private Color color_7 = Color.FromArgb(216, 216, 228);
        
        private Color color_8 = Color.FromArgb(243, 242, 247);

        private Color color_9 = Color.FromArgb(255, 255, 255);

        private double _tabColorBlend = 0.05;

        private double _pageColorBlend;

        private TextFormatFlags _textFormat;
    }
}
