using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Class12
	{
	    [DllImport("gdi32.dll")]
		private static extern IntPtr CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, short[] lpvBits);

		[DllImport("gdi32.dll")]
		private static extern IntPtr CreateBrushIndirect(Class12.Class13 lb);

		[DllImport("gdi32.dll")]
		private static extern bool DeleteObject(HandleRef hObject);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetDC(HandleRef hWnd);

		[DllImport("gdi32.dll")]
		private static extern bool PatBlt(HandleRef hdc, int left, int top, int width, int height, int rop);

		[DllImport("user32.dll")]
		private static extern int ReleaseDC(HandleRef hWnd, HandleRef hDC);

		[DllImport("gdi32.dll")]
		private static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);

		public static void smethod_0(Control control_0, Rectangle rectangle_0, bool bool_0, int int_0)
		{
			Class12.smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Y, rectangle_0.Width, 4));
			if (!bool_0)
			{
				Class12.smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Y + 4, 4, rectangle_0.Height - 8));
				Class12.smethod_1(control_0, new Rectangle(rectangle_0.Right - 4, rectangle_0.Y + 4, 4, rectangle_0.Height - 8));
				Class12.smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Bottom - 4, rectangle_0.Width, 4));
				return;
			}
			Class12.smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Y + 4, 4, rectangle_0.Height - 4 - int_0));
			Class12.smethod_1(control_0, new Rectangle(rectangle_0.Right - 4, rectangle_0.Y + 4, 4, rectangle_0.Height - 4 - int_0));
			Class12.smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Bottom - int_0, 10, 4));
			Class12.smethod_1(control_0, new Rectangle(rectangle_0.X + 80, rectangle_0.Bottom - int_0, rectangle_0.Width - 80, 4));
			Class12.smethod_1(control_0, new Rectangle(rectangle_0.X + 10, rectangle_0.Bottom - 4, 70, 4));
			Class12.smethod_1(control_0, new Rectangle(rectangle_0.X + 10, rectangle_0.Bottom - int_0, 4, int_0 - 4));
			Class12.smethod_1(control_0, new Rectangle(rectangle_0.X + 76, rectangle_0.Bottom - int_0, 4, int_0 - 4));
		}

		public static void smethod_1(Control control_0, Rectangle rectangle_0)
		{
			IntPtr handle = IntPtr.Zero;
			if (!(rectangle_0 == Rectangle.Empty))
			{
				if (control_0 == null)
				{
					handle = IntPtr.Zero;
				}
				else
				{
					handle = control_0.Handle;
				}
				IntPtr dC = Class12.GetDC(new HandleRef(control_0, handle));
				IntPtr handle2 = Class12.smethod_2();
				IntPtr handle3 = Class12.SelectObject(new HandleRef(control_0, dC), new HandleRef(null, handle2));
				Class12.PatBlt(new HandleRef(control_0, dC), rectangle_0.X, rectangle_0.Y, rectangle_0.Width, rectangle_0.Height, 5898313);
				Class12.SelectObject(new HandleRef(control_0, dC), new HandleRef(null, handle3));
				Class12.DeleteObject(new HandleRef(null, handle2));
				Class12.ReleaseDC(new HandleRef(control_0, handle), new HandleRef(null, dC));
				return;
			}
		}

		private static IntPtr smethod_2()
		{
			short[] array = new short[8];
			for (int i = 0; i < 8; i++)
			{
				array[i] = (short)(21845 << (i & 1));
			}
			IntPtr intPtr = CreateBitmap(8, 8, 1, 1, array);
			IntPtr result = CreateBrushIndirect(new Class13
			{
				int_1 = ColorTranslator.ToWin32(Color.Black),
				int_0 = 3,
				intptr_0 = intPtr
			});
			Class12.DeleteObject(new HandleRef(null, intPtr));
			return result;
		}

		[StructLayout(LayoutKind.Sequential)]
		private class Class13
		{
		    public int int_0;

			public int int_1;

			public IntPtr intptr_0;
		}
	}
}
