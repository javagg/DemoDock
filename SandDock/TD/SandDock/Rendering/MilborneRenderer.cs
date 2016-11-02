using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
    [TypeConverter(typeof(Class26))]
    public class MilborneRenderer : ITabControlRenderer
    {
        public virtual void DrawFakeTabControlBackgroundExtension(Graphics graphics, Rectangle bounds, Color backColor)
        {
            using (var solidBrush = new SolidBrush(this.color_7))
                graphics.FillRectangle(solidBrush, bounds);
            using (var pen = new Pen(this.color_2))
                graphics.DrawLine(pen, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
        }

        public void DrawTabControlBackground(Graphics graphics, Rectangle bounds, Color backColor, bool client)
        {
            if (bounds.Width > 0 && bounds.Height > 0)
            {
                Color color = Color.FromArgb(252, 252, 254);
                Color color2 = Color.FromArgb(244, 243, 238);
                if (this.PageColorBlend > 0.0)
                {
                    color = RendererBase.InterpolateColors(color, backColor, (float)this.PageColorBlend);
                    color2 = RendererBase.InterpolateColors(color2, backColor, (float)this.PageColorBlend);
                }
                if (!client)
                {
                    Rectangle rect = bounds;
                    rect.Height = this.TabControlPadding.Height;
                    using (SolidBrush solidBrush = new SolidBrush(color))
                    {
                        graphics.FillRectangle(solidBrush, rect);
                    }
                    Rectangle rect2 = bounds;
                    rect2.Y += this.TabControlPadding.Height;
                    rect2.Height -= this.TabControlPadding.Height * 2;
                    if (rect2.Width > 0 && rect2.Height > 0)
                    {
                        using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect2, color, color2, LinearGradientMode.Vertical))
                        {
                            graphics.FillRectangle(linearGradientBrush, rect2);
                        }
                    }
                    Rectangle rect3 = bounds;
                    rect3.Y = rect3.Bottom - this.TabControlPadding.Height;
                    rect3.Height = this.TabControlPadding.Height;
                    using (SolidBrush solidBrush2 = new SolidBrush(color2))
                    {
                        graphics.FillRectangle(solidBrush2, rect3);
                    }
                    using (Pen pen = new Pen(this.color_2))
                    {
                        graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 2);
                        graphics.DrawLine(pen, bounds.X, bounds.Bottom - 2, bounds.Right - 2, bounds.Bottom - 2);
                        graphics.DrawLine(pen, bounds.Right - 2, bounds.Bottom - 2, bounds.Right - 2, bounds.Y);
                    }
                    using (Pen pen2 = new Pen(RendererBase.InterpolateColors(this.color_2, SystemColors.Control, 0.8f)))
                    {
                        graphics.DrawLine(pen2, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
                        graphics.DrawLine(pen2, bounds.Right - 1, bounds.Bottom - 1, bounds.Right - 1, bounds.Y);
                        return;
                    }
                }
                using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(bounds, color, color2, LinearGradientMode.Vertical))
                {
                    graphics.FillRectangle(linearGradientBrush2, bounds);
                }
                return;
            }
        }

        public void DrawTabControlButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
        {
            if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                bounds.Offset(1, 1);
            switch (buttonType)
            {
                case SandDockButtonType.ScrollLeft:
                    Class15.smethod_1(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
                    return;
                case SandDockButtonType.ScrollRight:
                    Class15.smethod_2(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
                    return;
                default:
                    return;
            }
        }

        public void DrawTabControlTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
        {
            int arg_07_0 = bounds.Height;
            bool flag;
            Color color = (!(flag = ((state & DrawItemState.Selected) == DrawItemState.Selected))) ? this.color_6 : this.color_4;
            Color color2 = (!flag) ? this.color_7 : this.color_5;
            if (this.TabColorBlend > 0.0)
            {
                color = RendererBase.InterpolateColors(color, backColor, (float)this.TabColorBlend);
                color2 = RendererBase.InterpolateColors(color2, backColor, (float)this.TabColorBlend);
            }
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, color, color2, LinearGradientMode.Vertical))
            {
                graphics.FillPolygon(linearGradientBrush, this.method_0(bounds, false));
            }
            SmoothingMode smoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen((!flag) ? this.color_2 : this.color_0))
            {
                graphics.DrawLines(pen, this.method_0(bounds, false));
            }
            using (Pen pen2 = new Pen((!flag) ? this.color_3 : this.color_1))
            {
                graphics.DrawLines(pen2, this.method_0(bounds, true));
                if (!flag)
                {
                    Color color3 = RendererBase.InterpolateColors(this.color_3, this.color_2, 0.5f);
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
                Rectangle rectangle = bounds;
                rectangle.X += this.TabControlTabExtra;
                rectangle.Width -= this.TabControlTabExtra;
                rectangle.Inflate(-4, -3);
                rectangle.X++;
                rectangle.Height++;
                ControlPaint.DrawFocusRectangle(graphics, rectangle);
            }
            bounds.X += this.TabControlTabExtra + 6;
            bounds.Width -= this.TabControlTabExtra + 6;
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
                        TextRenderer.DrawText(graphics, text, font2, bounds, foreColor, this.textFormatFlags_0);
                        return;
                    }
                }
                TextRenderer.DrawText(graphics, text, font, bounds, foreColor, this.textFormatFlags_0);
            }
        }

        public void DrawTabControlTabStripBackground(Graphics graphics, Rectangle bounds, Color backColor)
        {
            if (backColor != Color.Transparent)
            {
                Class16.smethod_0(graphics, backColor);
            }
            using (var pen = new Pen(this.color_2))
                graphics.DrawLine(pen, bounds.X, bounds.Bottom - 1, bounds.Right - 2, bounds.Bottom - 1);
        }

        public void FinishRenderSession() { }

        public Size MeasureTabControlTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
        {
            int num;
            using (new Font(font, FontStyle.Bold))
            {
                num = TextRenderer.MeasureText(graphics, text, font, new Size(2147483647, 2147483647), this.textFormatFlags_0).Width;
            }
            num += 24;
            if (image != null)
            {
                num += image.Width + 4;
            }
            num += this.TabControlTabExtra;
            return new Size(num, 0);
        }

        private Point[] method_0(Rectangle rectangle_0, bool bool_0)
        {
            int num = Math.Min(rectangle_0.Width, rectangle_0.Height);
            if (!bool_0)
            {
                return new[]
                {
                    new Point(rectangle_0.X, rectangle_0.Bottom - 1),
                    new Point(rectangle_0.X + num - 4, rectangle_0.Y + 3),
                    new Point(rectangle_0.X + num + 1, rectangle_0.Y),
                    new Point(rectangle_0.Right - 4, rectangle_0.Y),
                    new Point(rectangle_0.Right - 1, rectangle_0.Y + 3),
                    new Point(rectangle_0.Right - 1, rectangle_0.Bottom - 1)
                };
            }
            return new[]
            {
                new Point(rectangle_0.X + 2, rectangle_0.Bottom - 2),
                new Point(rectangle_0.X + num - 3, rectangle_0.Y + 3),
                new Point(rectangle_0.X + num + 1, rectangle_0.Y + 1),
                new Point(rectangle_0.Right - 4, rectangle_0.Y + 1),
                new Point(rectangle_0.Right - 2, rectangle_0.Y + 3),
                new Point(rectangle_0.Right - 2, rectangle_0.Bottom - 2)
            };
        }

        public void StartRenderSession(HotkeyPrefix tabHotKeys)
        {
            this.textFormatFlags_0 = (TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPadding);
            if (tabHotKeys != HotkeyPrefix.None)
            {
                if (tabHotKeys == HotkeyPrefix.Hide)
                {
                    this.textFormatFlags_0 |= TextFormatFlags.HidePrefix;
                }
                return;
            }
            this.textFormatFlags_0 |= TextFormatFlags.NoPrefix;
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
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentException("Value must lie between 0 and 1.");
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
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentException("Value must lie between 0 and 1.");
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

        private TextFormatFlags textFormatFlags_0;
    }
}
