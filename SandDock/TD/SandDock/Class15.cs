using System;
using System.Drawing;

namespace TD.SandDock
{
	internal class Class15
	{
		private Class15()
		{
		}

		public static void smethod_0(Graphics graphics_0, Rectangle rectangle_0, Pen pen_0)
		{
			int num = rectangle_0.Width / 4;
			for (int i = 1; i <= num; i++)
			{
				int num2 = (num - i) * 2;
				int num3 = rectangle_0.Left + rectangle_0.Width / 2 - (num - i);
				int num4 = rectangle_0.Top + rectangle_0.Height / 2 + (i - 1);
				graphics_0.DrawLine(pen_0, num3, num4, num3 + num2 + 1, num4);
			}
		}

		public static void smethod_1(Graphics graphics_0, Rectangle rectangle_0, Color color_0, bool bool_0)
		{
			int num = rectangle_0.Left + rectangle_0.Width / 2;
			int num2 = rectangle_0.Top + rectangle_0.Height / 2;
			Class15.smethod_3(graphics_0, new Point[]
			{
				new Point(num + 2, num2 - 5),
				new Point(num - 2, num2 - 1),
				new Point(num + 2, num2 + 3)
			}, color_0, bool_0);
		}

		public static void smethod_2(Graphics graphics_0, Rectangle rectangle_0, Color color_0, bool bool_0)
		{
			int num = rectangle_0.Left + rectangle_0.Width / 2;
			int num2 = rectangle_0.Top + rectangle_0.Height / 2;
			Class15.smethod_3(graphics_0, new Point[]
			{
				new Point(num - 2, num2 - 5),
				new Point(num + 2, num2 - 1),
				new Point(num - 2, num2 + 3)
			}, color_0, bool_0);
		}

		private static void smethod_3(Graphics graphics_0, Point[] point_0, Color color_0, bool bool_0)
		{
			if (bool_0)
			{
				using (SolidBrush solidBrush = new SolidBrush(color_0))
				{
					graphics_0.FillPolygon(solidBrush, point_0);
					return;
				}
			}
			using (Pen pen = new Pen(color_0))
			{
				graphics_0.DrawPolygon(pen, point_0);
			}
		}

		public static void smethod_4(Graphics graphics_0, Rectangle rectangle_0, Pen pen_0, bool bool_0)
		{
			int num = rectangle_0.Left + rectangle_0.Width / 2;
			int num2 = rectangle_0.Top + rectangle_0.Height / 2;
			if (bool_0)
			{
				graphics_0.DrawLine(pen_0, num - 5, num2, num - 2, num2);
				graphics_0.DrawLine(pen_0, num - 2, num2 - 3, num - 2, num2 + 3);
				graphics_0.DrawLine(pen_0, num - 2, num2 - 2, num + 4, num2 - 2);
				graphics_0.DrawLine(pen_0, num - 2, num2 + 1, num + 4, num2 + 1);
				graphics_0.DrawLine(pen_0, num - 2, num2 + 2, num + 4, num2 + 2);
				graphics_0.DrawLine(pen_0, num + 4, num2 - 2, num + 4, num2 + 2);
				return;
			}
			graphics_0.DrawLine(pen_0, num - 3, num2 + 2, num + 3, num2 + 2);
			graphics_0.DrawLine(pen_0, num - 2, num2 - 3, num - 2, num2 + 2);
			graphics_0.DrawLine(pen_0, num - 2, num2 - 3, num + 2, num2 - 3);
			graphics_0.DrawLine(pen_0, num + 1, num2 - 3, num + 1, num2 + 2);
			graphics_0.DrawLine(pen_0, num + 2, num2 - 3, num + 2, num2 + 2);
			graphics_0.DrawLine(pen_0, num, num2 + 2, num, num2 + 5);
		}

		public static void smethod_5(Graphics graphics_0, Rectangle rectangle_0, Pen pen_0)
		{
			int num = rectangle_0.Left + rectangle_0.Width / 2 - 1;
			int num2 = rectangle_0.Top + rectangle_0.Height / 2;
			graphics_0.DrawLine(pen_0, num - 3, num2 - 4, num + 3, num2 + 2);
			graphics_0.DrawLine(pen_0, num - 2, num2 - 4, num + 4, num2 + 2);
			graphics_0.DrawLine(pen_0, num - 3, num2 + 2, num + 3, num2 - 4);
			graphics_0.DrawLine(pen_0, num - 2, num2 + 2, num + 4, num2 - 4);
		}

		public static void smethod_6(Graphics graphics_0, Rectangle rectangle_0, Pen pen_0)
		{
			int num = rectangle_0.Left + rectangle_0.Width / 2 - 1;
			int num2 = rectangle_0.Top + rectangle_0.Height / 2;
			graphics_0.DrawLine(pen_0, num - 3, num2 - 3, num + 4, num2 + 4);
			graphics_0.DrawLine(pen_0, num - 2, num2 - 3, num + 4, num2 + 3);
			graphics_0.DrawLine(pen_0, num - 3, num2 - 2, num + 3, num2 + 4);
			graphics_0.DrawLine(pen_0, num + 4, num2 - 3, num - 3, num2 + 4);
			graphics_0.DrawLine(pen_0, num + 3, num2 - 3, num - 3, num2 + 3);
			graphics_0.DrawLine(pen_0, num + 4, num2 - 2, num - 2, num2 + 4);
		}
	}
}
