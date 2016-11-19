using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
	internal class RenderHelper
	{
	    internal static void ClearBackground(Graphics g, Color color)
		{
			try
			{
				g.Clear(color);
			}
			catch
			{
			    // ignored
			}
		}

		public static void smethod_1(Graphics g, Rectangle rectangle_0, Rectangle rectangle_1, Image image, Size size_0, string text, Font font, Color color_0, Color color_1, Brush brush_0, Color color_2, Color color_3, Color color_4, bool bool_0, int int_0, int int_1, TextFormatFlags flags, bool bool_1)
		{
		    if (rectangle_0.Width <= 0 || rectangle_0.Height <= 0) return;

		    using (var pen = new Pen(color_2))
		    {
		        g.DrawLine(pen, rectangle_0.Left, rectangle_0.Bottom - 2, rectangle_0.Left + 1, rectangle_0.Bottom - 2);
		        g.DrawLine(pen, rectangle_0.Left + 1, rectangle_0.Bottom - 2, rectangle_0.Left + int_0 - 3, rectangle_0.Top + 2);
		        g.DrawLine(pen, rectangle_0.Left + int_0 - 3, rectangle_0.Top + 2, rectangle_0.Left + int_0 - 2, rectangle_0.Top + 2);
		        g.DrawLine(pen, rectangle_0.Left + int_0 - 1, rectangle_0.Top + 1, rectangle_0.Left + int_0, rectangle_0.Top + 1);
		        g.DrawLine(pen, rectangle_0.Left + int_0 + 1, rectangle_0.Top, rectangle_0.Right - 3, rectangle_0.Top);
		        g.DrawLine(pen, rectangle_0.Right - 3, rectangle_0.Top, rectangle_0.Right - 1, rectangle_0.Top + 2);
		        g.DrawLine(pen, rectangle_0.Right - 1, rectangle_0.Top + 2, rectangle_0.Right - 1, rectangle_0.Bottom - 2);
		    }
		    using (var pen2 = new Pen(color_3))
		    {
		        g.DrawLine(pen2, rectangle_0.Left + 2, rectangle_0.Bottom - 2, rectangle_0.Left + int_0 - 3, rectangle_0.Top + 3);
		        g.DrawLine(pen2, rectangle_0.Left + int_0 - 3, rectangle_0.Top + 3, rectangle_0.Left + int_0 - 2, rectangle_0.Top + 3);
		        g.DrawLine(pen2, rectangle_0.Left + int_0 - 1, rectangle_0.Top + 2, rectangle_0.Left + int_0, rectangle_0.Top + 2);
		        g.DrawLine(pen2, rectangle_0.Left + int_0 + 1, rectangle_0.Top + 1, rectangle_0.Right - 4, rectangle_0.Top + 1);
		    }
		    using (var pen3 = new Pen(color_4))
		    {
		        g.DrawLine(pen3, rectangle_0.Right - 3, rectangle_0.Top + 1, rectangle_0.Right - 2, rectangle_0.Top + 2);
		        g.DrawLine(pen3, rectangle_0.Right - 2, rectangle_0.Top + 2, rectangle_0.Right - 2, rectangle_0.Bottom - 2);
		    }
		    Point[] points = {
		        new Point(rectangle_0.Left + 2, rectangle_0.Bottom - 1),
		        new Point(rectangle_0.Left + int_0 - 3, rectangle_0.Top + 4),
		        new Point(rectangle_0.Left + int_0 + 1, rectangle_0.Top + 2),
		        new Point(rectangle_0.Right - 2, rectangle_0.Top + 2),
		        new Point(rectangle_0.Right - 2, rectangle_0.Bottom - 1)
		    };
		    using (var brush = new LinearGradientBrush(rectangle_0, color_0, color_1, LinearGradientMode.Vertical))
		        g.FillPolygon(brush, points);
		    if (bool_0)
		    {
		        using (var pen4 = new Pen(color_1))
		        {
		            g.DrawLine(pen4, rectangle_0.Left, rectangle_0.Bottom - 1, rectangle_0.Right - 1, rectangle_0.Bottom - 1);
		        }
		    }
		    rectangle_0 = rectangle_1;
		    rectangle_0.X += int_1;
		    rectangle_0.Width -= int_1;
		    if (image != null)
		    {
		        g.DrawImage(image, rectangle_0.X + 4, rectangle_0.Y + 2, size_0.Width, size_0.Height);
		        rectangle_0.X += size_0.Width + 4;
		        rectangle_0.Width -= size_0.Width + 4;
		    }
		    if (rectangle_0.Width > 8)
		    {
		        flags |= TextFormatFlags.HorizontalCenter;
		        flags &= (TextFormatFlags)(-1);
		        TextRenderer.DrawText(g, text, font, rectangle_0, SystemColors.ControlText, flags);
		    }
		    if (bool_1)
		    {
		        var rectangle = rectangle_0;
		        rectangle.Inflate(-2, -2);
		        rectangle.Height += 2;
		        rectangle.X++;
		        rectangle.Width--;
		        ControlPaint.DrawFocusRectangle(g, rectangle);
		    }
		}

		public static Size MeasureTabStripTab(Graphics g, Image image, Size size, string text, Font font, TextFormatFlags flags)
		{
			var width = TextRenderer.MeasureText(g, text, font, new Size(2147483647, 2147483647), flags).Width + 3;
			width += 6;
			width += size.Width + 4;
			return new Size(width, size.Height);
		}

		public static void smethod_3(Graphics g, Rectangle bounds, Image image, Size size, string text, Font font, Color color_0, Color color_1, Color color_2, Color color_3, DrawItemState state, TextFormatFlags flags)
		{
		    using (var brush = new LinearGradientBrush(bounds, color_0, color_1, LinearGradientMode.Vertical))
		        smethod_4(g, bounds, image, size, text, font, brush, color_2, color_3, state, flags);
		}

		public static void smethod_4(Graphics g, Rectangle rectangle_0, Image image, Size size_0, string text, Font font, Brush brush, Color color_0, Color color_1, DrawItemState state, TextFormatFlags flags)
		{
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				Rectangle rect = rectangle_0;
				rect.Inflate(-1, 0);
				rect.Height--;
				g.FillRectangle(brush, rect);
				Point[] points = {
					new Point(rectangle_0.Left, rectangle_0.Top),
					new Point(rectangle_0.Left, rectangle_0.Bottom - 3),
					new Point(rectangle_0.Left + 2, rectangle_0.Bottom - 1),
					new Point(rectangle_0.Right - 3, rectangle_0.Bottom - 1),
					new Point(rectangle_0.Right - 1, rectangle_0.Bottom - 3),
					new Point(rectangle_0.Right - 1, rectangle_0.Top)
				};
				using (Pen pen = new Pen(color_1))
				{
					g.DrawLines(pen, points);
				}
			}
			rectangle_0.Inflate(-3, 0);
			if (rectangle_0.Width >= size_0.Width + 4)
			{
				g.DrawImage(image, new Rectangle(rectangle_0.X + 1, rectangle_0.Y + 2, size_0.Width, size_0.Height));
				rectangle_0.X += size_0.Width + 4;
				rectangle_0.Width -= size_0.Width + 4;
			}
			if (rectangle_0.Width >= 8)
			{
				rectangle_0.Y--;
				flags = flags;
				flags &= ~TextFormatFlags.HorizontalCenter;
				TextRenderer.DrawText(g, text, font, rectangle_0, color_0, flags);
			}
		}

		public static void smethod_5(Graphics g, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Brush brush, Color color, bool bool_0)
		{
			smethod_6(g, bounds, dockSide, image, text, font, null, brush, color, bool_0);
		}

		public static void smethod_6(Graphics g, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Brush brush, Brush brush_1, Color color_0, bool allTab)
		{
			bool vertical = false;
			Point[] array = new Point[6];
			switch (dockSide)
			{
			case DockSide.Top:
				array[0] = new Point(bounds.Left, bounds.Top);
				array[1] = new Point(bounds.Right, bounds.Top);
				array[2] = new Point(bounds.Right, bounds.Bottom - 2);
				array[3] = new Point(bounds.Right - 2, bounds.Bottom);
				array[4] = new Point(bounds.Left + 2, bounds.Bottom);
				array[5] = new Point(bounds.Left, bounds.Bottom - 2);
				break;
			case DockSide.Bottom:
				array[0] = new Point(bounds.Left + 2, bounds.Top);
				array[1] = new Point(bounds.Right - 2, bounds.Top);
				array[2] = new Point(bounds.Right, bounds.Top + 2);
				array[3] = new Point(bounds.Right, bounds.Bottom);
				array[4] = new Point(bounds.Left, bounds.Bottom);
				array[5] = new Point(bounds.Left, bounds.Top + 2);
				break;
			case DockSide.Left:
				array[0] = new Point(bounds.Left, bounds.Top);
				array[1] = new Point(bounds.Right - 2, bounds.Top);
				array[2] = new Point(bounds.Right, bounds.Top + 2);
				array[3] = new Point(bounds.Right, bounds.Bottom - 2);
				array[4] = new Point(bounds.Right - 2, bounds.Bottom);
				array[5] = new Point(bounds.Left, bounds.Bottom);
				vertical = true;
				break;
			case DockSide.Right:
				array[0] = new Point(bounds.Left + 2, bounds.Top);
				array[1] = new Point(bounds.Right, bounds.Top);
				array[2] = new Point(bounds.Right, bounds.Bottom);
				array[3] = new Point(bounds.Left + 2, bounds.Bottom);
				array[4] = new Point(bounds.Left, bounds.Bottom - 2);
				array[5] = new Point(bounds.Left, bounds.Top + 2);
				vertical = true;
				break;
			}
			if (brush != null)
			{
				g.FillPolygon(brush, array);
			}
			using (var pen = new Pen(color_0))
			{
				g.DrawPolygon(pen, array);
			}
			bounds.Inflate(-2, -2);
			if (vertical)
			{
				bounds.Offset(0, 1);
			}
			else
			{
				bounds.Offset(1, 0);
			}
			g.DrawImage(image, new Rectangle(bounds.Left, bounds.Top, image.Width, image.Height));
		    if (text.Length == 0) return;
		    var num = allTab ? 21 : 23;
		    if (vertical)
		    {
		        bounds.Offset(0, num);
		        g.DrawString(text, font, brush_1, bounds, EverettRenderer.VerticalTextFormat);
		    }
		    else
		    {
		        bounds.Offset(num, 0);
		        g.DrawString(text, font, brush_1, bounds, EverettRenderer.HorizontalTextFormat);
		    }
		}
	}
}
