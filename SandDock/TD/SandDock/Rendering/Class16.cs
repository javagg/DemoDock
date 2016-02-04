using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
	internal class Class16
	{
		public Class16()
		{
		}

		internal static void smethod_0(Graphics graphics_0, Color color_0)
		{
			try
			{
				graphics_0.Clear(color_0);
			}
			catch
			{
			}
		}

		public static void smethod_1(Graphics graphics_0, Rectangle rectangle_0, Rectangle rectangle_1, Image image_0, Size size_0, string string_0, Font font_0, Color color_0, Color color_1, Brush brush_0, Color color_2, Color color_3, Color color_4, bool bool_0, int int_0, int int_1, TextFormatFlags textFormatFlags_0, bool bool_1)
		{
			if (rectangle_0.Width > 0 && rectangle_0.Height > 0)
			{
				using (Pen pen = new Pen(color_2))
				{
					graphics_0.DrawLine(pen, rectangle_0.Left, rectangle_0.Bottom - 2, rectangle_0.Left + 1, rectangle_0.Bottom - 2);
					graphics_0.DrawLine(pen, rectangle_0.Left + 1, rectangle_0.Bottom - 2, rectangle_0.Left + int_0 - 3, rectangle_0.Top + 2);
					graphics_0.DrawLine(pen, rectangle_0.Left + int_0 - 3, rectangle_0.Top + 2, rectangle_0.Left + int_0 - 2, rectangle_0.Top + 2);
					graphics_0.DrawLine(pen, rectangle_0.Left + int_0 - 1, rectangle_0.Top + 1, rectangle_0.Left + int_0, rectangle_0.Top + 1);
					graphics_0.DrawLine(pen, rectangle_0.Left + int_0 + 1, rectangle_0.Top, rectangle_0.Right - 3, rectangle_0.Top);
					graphics_0.DrawLine(pen, rectangle_0.Right - 3, rectangle_0.Top, rectangle_0.Right - 1, rectangle_0.Top + 2);
					graphics_0.DrawLine(pen, rectangle_0.Right - 1, rectangle_0.Top + 2, rectangle_0.Right - 1, rectangle_0.Bottom - 2);
				}
				using (Pen pen2 = new Pen(color_3))
				{
					graphics_0.DrawLine(pen2, rectangle_0.Left + 2, rectangle_0.Bottom - 2, rectangle_0.Left + int_0 - 3, rectangle_0.Top + 3);
					graphics_0.DrawLine(pen2, rectangle_0.Left + int_0 - 3, rectangle_0.Top + 3, rectangle_0.Left + int_0 - 2, rectangle_0.Top + 3);
					graphics_0.DrawLine(pen2, rectangle_0.Left + int_0 - 1, rectangle_0.Top + 2, rectangle_0.Left + int_0, rectangle_0.Top + 2);
					graphics_0.DrawLine(pen2, rectangle_0.Left + int_0 + 1, rectangle_0.Top + 1, rectangle_0.Right - 4, rectangle_0.Top + 1);
				}
				using (Pen pen3 = new Pen(color_4))
				{
					graphics_0.DrawLine(pen3, rectangle_0.Right - 3, rectangle_0.Top + 1, rectangle_0.Right - 2, rectangle_0.Top + 2);
					graphics_0.DrawLine(pen3, rectangle_0.Right - 2, rectangle_0.Top + 2, rectangle_0.Right - 2, rectangle_0.Bottom - 2);
				}
				Point[] points = new Point[]
				{
					new Point(rectangle_0.Left + 2, rectangle_0.Bottom - 1),
					new Point(rectangle_0.Left + int_0 - 3, rectangle_0.Top + 4),
					new Point(rectangle_0.Left + int_0 + 1, rectangle_0.Top + 2),
					new Point(rectangle_0.Right - 2, rectangle_0.Top + 2),
					new Point(rectangle_0.Right - 2, rectangle_0.Bottom - 1)
				};
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle_0, color_0, color_1, LinearGradientMode.Vertical))
				{
					graphics_0.FillPolygon(linearGradientBrush, points);
				}
				if (bool_0)
				{
					using (Pen pen4 = new Pen(color_1))
					{
						graphics_0.DrawLine(pen4, rectangle_0.Left, rectangle_0.Bottom - 1, rectangle_0.Right - 1, rectangle_0.Bottom - 1);
					}
				}
				rectangle_0 = rectangle_1;
				rectangle_0.X += int_1;
				rectangle_0.Width -= int_1;
				if (image_0 != null)
				{
					graphics_0.DrawImage(image_0, rectangle_0.X + 4, rectangle_0.Y + 2, size_0.Width, size_0.Height);
					rectangle_0.X += size_0.Width + 4;
					rectangle_0.Width -= size_0.Width + 4;
				}
				if (rectangle_0.Width > 8)
				{
					textFormatFlags_0 |= TextFormatFlags.HorizontalCenter;
					textFormatFlags_0 &= (TextFormatFlags)(-1);
					TextRenderer.DrawText(graphics_0, string_0, font_0, rectangle_0, SystemColors.ControlText, textFormatFlags_0);
				}
				if (bool_1)
				{
					Rectangle rectangle = rectangle_0;
					rectangle.Inflate(-2, -2);
					rectangle.Height += 2;
					rectangle.X++;
					rectangle.Width--;
					ControlPaint.DrawFocusRectangle(graphics_0, rectangle);
				}
				return;
			}
		}

		public static Size smethod_2(Graphics graphics_0, Image image_0, Size size_0, string string_0, Font font_0, TextFormatFlags textFormatFlags_0)
		{
			int num = TextRenderer.MeasureText(graphics_0, string_0, font_0, new Size(2147483647, 2147483647), textFormatFlags_0).Width + 3;
			num += 6;
			num += size_0.Width + 4;
			return new Size(num, size_0.Height);
		}

		public static void smethod_3(Graphics graphics_0, Rectangle rectangle_0, Image image_0, Size size_0, string string_0, Font font_0, Color color_0, Color color_1, Color color_2, Color color_3, DrawItemState drawItemState_0, TextFormatFlags textFormatFlags_0)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle_0, color_0, color_1, LinearGradientMode.Vertical))
			{
				Class16.smethod_4(graphics_0, rectangle_0, image_0, size_0, string_0, font_0, linearGradientBrush, color_2, color_3, drawItemState_0, textFormatFlags_0);
			}
		}

		public static void smethod_4(Graphics graphics_0, Rectangle rectangle_0, Image image_0, Size size_0, string string_0, Font font_0, Brush brush_0, Color color_0, Color color_1, DrawItemState drawItemState_0, TextFormatFlags textFormatFlags_0)
		{
			if ((drawItemState_0 & DrawItemState.Selected) == DrawItemState.Selected)
			{
				Rectangle rect = rectangle_0;
				rect.Inflate(-1, 0);
				rect.Height--;
				graphics_0.FillRectangle(brush_0, rect);
				Point[] points = new Point[]
				{
					new Point(rectangle_0.Left, rectangle_0.Top),
					new Point(rectangle_0.Left, rectangle_0.Bottom - 3),
					new Point(rectangle_0.Left + 2, rectangle_0.Bottom - 1),
					new Point(rectangle_0.Right - 3, rectangle_0.Bottom - 1),
					new Point(rectangle_0.Right - 1, rectangle_0.Bottom - 3),
					new Point(rectangle_0.Right - 1, rectangle_0.Top)
				};
				using (Pen pen = new Pen(color_1))
				{
					graphics_0.DrawLines(pen, points);
				}
			}
			rectangle_0.Inflate(-3, 0);
			if (rectangle_0.Width >= size_0.Width + 4)
			{
				graphics_0.DrawImage(image_0, new Rectangle(rectangle_0.X + 1, rectangle_0.Y + 2, size_0.Width, size_0.Height));
				rectangle_0.X += size_0.Width + 4;
				rectangle_0.Width -= size_0.Width + 4;
			}
			if (rectangle_0.Width >= 8)
			{
				rectangle_0.Y--;
				textFormatFlags_0 = textFormatFlags_0;
				textFormatFlags_0 &= ~TextFormatFlags.HorizontalCenter;
				TextRenderer.DrawText(graphics_0, string_0, font_0, rectangle_0, color_0, textFormatFlags_0);
			}
		}

		public static void smethod_5(Graphics graphics_0, Rectangle rectangle_0, DockSide dockSide_0, Image image_0, string string_0, Font font_0, Brush brush_0, Color color_0, bool bool_0)
		{
			Class16.smethod_6(graphics_0, rectangle_0, dockSide_0, image_0, string_0, font_0, null, brush_0, color_0, bool_0);
		}

		public static void smethod_6(Graphics graphics_0, Rectangle rectangle_0, DockSide dockSide_0, Image image_0, string string_0, Font font_0, Brush brush_0, Brush brush_1, Color color_0, bool bool_0)
		{
			bool flag = false;
			Point[] array = new Point[6];
			switch (dockSide_0)
			{
			case DockSide.Top:
				array[0] = new Point(rectangle_0.Left, rectangle_0.Top);
				array[1] = new Point(rectangle_0.Right, rectangle_0.Top);
				array[2] = new Point(rectangle_0.Right, rectangle_0.Bottom - 2);
				array[3] = new Point(rectangle_0.Right - 2, rectangle_0.Bottom);
				array[4] = new Point(rectangle_0.Left + 2, rectangle_0.Bottom);
				array[5] = new Point(rectangle_0.Left, rectangle_0.Bottom - 2);
				break;
			case DockSide.Bottom:
				array[0] = new Point(rectangle_0.Left + 2, rectangle_0.Top);
				array[1] = new Point(rectangle_0.Right - 2, rectangle_0.Top);
				array[2] = new Point(rectangle_0.Right, rectangle_0.Top + 2);
				array[3] = new Point(rectangle_0.Right, rectangle_0.Bottom);
				array[4] = new Point(rectangle_0.Left, rectangle_0.Bottom);
				array[5] = new Point(rectangle_0.Left, rectangle_0.Top + 2);
				break;
			case DockSide.Left:
				array[0] = new Point(rectangle_0.Left, rectangle_0.Top);
				array[1] = new Point(rectangle_0.Right - 2, rectangle_0.Top);
				array[2] = new Point(rectangle_0.Right, rectangle_0.Top + 2);
				array[3] = new Point(rectangle_0.Right, rectangle_0.Bottom - 2);
				array[4] = new Point(rectangle_0.Right - 2, rectangle_0.Bottom);
				array[5] = new Point(rectangle_0.Left, rectangle_0.Bottom);
				flag = true;
				break;
			case DockSide.Right:
				array[0] = new Point(rectangle_0.Left + 2, rectangle_0.Top);
				array[1] = new Point(rectangle_0.Right, rectangle_0.Top);
				array[2] = new Point(rectangle_0.Right, rectangle_0.Bottom);
				array[3] = new Point(rectangle_0.Left + 2, rectangle_0.Bottom);
				array[4] = new Point(rectangle_0.Left, rectangle_0.Bottom - 2);
				array[5] = new Point(rectangle_0.Left, rectangle_0.Top + 2);
				flag = true;
				break;
			}
			if (brush_0 != null)
			{
				graphics_0.FillPolygon(brush_0, array);
			}
			using (Pen pen = new Pen(color_0))
			{
				graphics_0.DrawPolygon(pen, array);
			}
			rectangle_0.Inflate(-2, -2);
			if (flag)
			{
				rectangle_0.Offset(0, 1);
			}
			else
			{
				rectangle_0.Offset(1, 0);
			}
			graphics_0.DrawImage(image_0, new Rectangle(rectangle_0.Left, rectangle_0.Top, image_0.Width, image_0.Height));
			if (string_0.Length != 0)
			{
				int num = (!bool_0) ? 23 : 21;
				if (flag)
				{
					rectangle_0.Offset(0, num);
					graphics_0.DrawString(string_0, font_0, brush_1, rectangle_0, EverettRenderer.StringFormat_1);
					return;
				}
				rectangle_0.Offset(num, 0);
				graphics_0.DrawString(string_0, font_0, brush_1, rectangle_0, EverettRenderer.StringFormat_0);
			}
		}
	}
}
