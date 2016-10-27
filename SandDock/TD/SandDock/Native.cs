using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal static class Native
	{
	    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	    public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

	    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	    public static extern bool DeleteDC(IntPtr hdc);

	    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	    public static extern bool DeleteObject(IntPtr hObject);

	    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
	    public static extern IntPtr GetDC(IntPtr hWnd);

	    [DllImport("user32.dll", ExactSpelling = true)]
	    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

	    [DllImport("gdi32.dll", ExactSpelling = true)]
	    public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

	    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
	    public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

	    public const byte byte_0 = 0;

	    public const byte byte_1 = 1;

	    public const int int_0 = 1;

	    public const int int_1 = 2;

	    public const int int_2 = 4;

	    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	    public struct BLENDFUNCTION
	    {
	        public byte BlendOp;

	        public byte BlendFlags;

	        public byte SourceConstantAlpha;

	        public byte AlphaFormat;
	    }

	    public struct Point
	    {
	        public Point(int x, int y)
	        {
	            this.x = x;
	            this.y = y;
	        }

	        public int x;

	        public int y;
	    }

	    public struct Size
	    {
	        public Size(int cx, int cy)
	        {
	            this.cx = cx;
	            this.cy = cy;
	        }

	        public int cx;

	        public int cy;
	    }

	    [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetSysColor(int nIndex);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int smIndex);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
        public static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int dwMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

        [DllImport("gdi32.dll")]
		private static extern IntPtr CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, short[] lpvBits);

		[DllImport("gdi32.dll")]
		private static extern IntPtr CreateBrushIndirect(Class13 lb);

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

        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfo(int nAction, int nParam, ref int i, int nUpdate);

        public static void smethod_0(Control control_0, Rectangle rectangle_0, bool bool_0, int int_0)
		{
			smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Y, rectangle_0.Width, 4));
			if (!bool_0)
			{
				smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Y + 4, 4, rectangle_0.Height - 8));
				smethod_1(control_0, new Rectangle(rectangle_0.Right - 4, rectangle_0.Y + 4, 4, rectangle_0.Height - 8));
				smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Bottom - 4, rectangle_0.Width, 4));
				return;
			}
			smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Y + 4, 4, rectangle_0.Height - 4 - int_0));
			smethod_1(control_0, new Rectangle(rectangle_0.Right - 4, rectangle_0.Y + 4, 4, rectangle_0.Height - 4 - int_0));
			smethod_1(control_0, new Rectangle(rectangle_0.X, rectangle_0.Bottom - int_0, 10, 4));
			smethod_1(control_0, new Rectangle(rectangle_0.X + 80, rectangle_0.Bottom - int_0, rectangle_0.Width - 80, 4));
			smethod_1(control_0, new Rectangle(rectangle_0.X + 10, rectangle_0.Bottom - 4, 70, 4));
			smethod_1(control_0, new Rectangle(rectangle_0.X + 10, rectangle_0.Bottom - int_0, 4, int_0 - 4));
			smethod_1(control_0, new Rectangle(rectangle_0.X + 76, rectangle_0.Bottom - int_0, 4, int_0 - 4));
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
				IntPtr dC = GetDC(new HandleRef(control_0, handle));
				IntPtr handle2 = smethod_2();
				IntPtr handle3 = SelectObject(new HandleRef(control_0, dC), new HandleRef(null, handle2));
				PatBlt(new HandleRef(control_0, dC), rectangle_0.X, rectangle_0.Y, rectangle_0.Width, rectangle_0.Height, 5898313);
				SelectObject(new HandleRef(control_0, dC), new HandleRef(null, handle3));
				DeleteObject(new HandleRef(null, handle2));
				ReleaseDC(new HandleRef(control_0, handle), new HandleRef(null, dC));
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
			DeleteObject(new HandleRef(null, intPtr));
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
