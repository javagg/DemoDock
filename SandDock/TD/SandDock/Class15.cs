using System;
using System.Drawing;

namespace TD.SandDock
{
    internal class Class15
    {
        public static void smethod_0(Graphics g, Rectangle bounds, Pen pen)
        {
            var num = bounds.Width / 4;
            for (var i = 1; i <= num; i++)
            {
                int num2 = (num - i) * 2;
                int num3 = bounds.Left + bounds.Width / 2 - (num - i);
                int num4 = bounds.Top + bounds.Height / 2 + (i - 1);
                g.DrawLine(pen, num3, num4, num3 + num2 + 1, num4);
            }
        }

        public static void smethod_1(Graphics g, Rectangle bounds, Color color, bool fill)
        {
            var x = bounds.Left + bounds.Width / 2;
            var y = bounds.Top + bounds.Height / 2;
            smethod_3(g, new[] { new Point(x + 2, y - 5), new Point(x - 2, y - 1), new Point(x + 2, y + 3) }, color, fill);
        }

        public static void smethod_2(Graphics g, Rectangle bounds, Color color, bool fill)
        {
            var x = bounds.Left + bounds.Width / 2;
            var y = bounds.Top + bounds.Height / 2;
            smethod_3(g, new[] { new Point(x - 2, y - 5), new Point(x + 2, y - 1), new Point(x - 2, y + 3) }, color, fill);
        }

        private static void smethod_3(Graphics g, Point[] points, Color color, bool fill)
        {
            if (fill)
                using (var brush = new SolidBrush(color))
                    g.FillPolygon(brush, points);
            else
                using (var pen = new Pen(color))
                    g.DrawPolygon(pen, points);
        }

        public static void smethod_4(Graphics g, Rectangle bounds, Pen pen, bool bool_0)
        {
            var x = bounds.Left + bounds.Width / 2;
            var y = bounds.Top + bounds.Height / 2;
            if (bool_0)
            {
                g.DrawLine(pen, x - 5, y, x - 2, y);
                g.DrawLine(pen, x - 2, y - 3, x - 2, y + 3);
                g.DrawLine(pen, x - 2, y - 2, x + 4, y - 2);
                g.DrawLine(pen, x - 2, y + 1, x + 4, y + 1);
                g.DrawLine(pen, x - 2, y + 2, x + 4, y + 2);
                g.DrawLine(pen, x + 4, y - 2, x + 4, y + 2);
            }
            else
            {
                g.DrawLine(pen, x - 3, y + 2, x + 3, y + 2);
                g.DrawLine(pen, x - 2, y - 3, x - 2, y + 2);
                g.DrawLine(pen, x - 2, y - 3, x + 2, y - 3);
                g.DrawLine(pen, x + 1, y - 3, x + 1, y + 2);
                g.DrawLine(pen, x + 2, y - 3, x + 2, y + 2);
                g.DrawLine(pen, x, y + 2, x, y + 5);
            }
        }

        public static void smethod_5(Graphics g, Rectangle bounds, Pen pen)
        {
            var x = bounds.Left + bounds.Width / 2 - 1;
            var y = bounds.Top + bounds.Height / 2;
            g.DrawLine(pen, x - 3, y - 4, x + 3, y + 2);
            g.DrawLine(pen, x - 2, y - 4, x + 4, y + 2);
            g.DrawLine(pen, x - 3, y + 2, x + 3, y - 4);
            g.DrawLine(pen, x - 2, y + 2, x + 4, y - 4);
        }

        public static void smethod_6(Graphics g, Rectangle bounds, Pen pen)
        {
            var x = bounds.Left + bounds.Width / 2 - 1;
            var y = bounds.Top + bounds.Height / 2;
            g.DrawLine(pen, x - 3, y - 3, x + 4, y + 4);
            g.DrawLine(pen, x - 2, y - 3, x + 4, y + 3);
            g.DrawLine(pen, x - 3, y - 2, x + 3, y + 4);
            g.DrawLine(pen, x + 4, y - 3, x - 3, y + 4);
            g.DrawLine(pen, x + 3, y - 3, x - 3, y + 3);
            g.DrawLine(pen, x + 4, y - 2, x - 2, y + 4);
        }
    }
}
