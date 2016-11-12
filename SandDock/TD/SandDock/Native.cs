using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TD.Util;
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
		private static extern IntPtr CreateBrushIndirect(Logbrush lb);

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

        public static void DrawIndicators(Control control, Rectangle bounds, bool bool_0, int size)
		{
			DrawIndicator(control, new Rectangle(bounds.X, bounds.Y, bounds.Width, 4));
		    if (bool_0)
		    {
		        DrawIndicator(control, new Rectangle(bounds.X, bounds.Y + 4, 4, bounds.Height - 4 - size));
		        DrawIndicator(control, new Rectangle(bounds.Right - 4, bounds.Y + 4, 4, bounds.Height - 4 - size));
		        DrawIndicator(control, new Rectangle(bounds.X, bounds.Bottom - size, 10, 4));
		        DrawIndicator(control, new Rectangle(bounds.X + 80, bounds.Bottom - size, bounds.Width - 80, 4));
		        DrawIndicator(control, new Rectangle(bounds.X + 10, bounds.Bottom - 4, 70, 4));
		        DrawIndicator(control, new Rectangle(bounds.X + 10, bounds.Bottom - size, 4, size - 4));
		        DrawIndicator(control, new Rectangle(bounds.X + 76, bounds.Bottom - size, 4, size - 4));
		    }
		    else
		    {
		        DrawIndicator(control, new Rectangle(bounds.X, bounds.Y + 4, 4, bounds.Height - 8));
		        DrawIndicator(control, new Rectangle(bounds.Right - 4, bounds.Y + 4, 4, bounds.Height - 8));
		        DrawIndicator(control, new Rectangle(bounds.X, bounds.Bottom - 4, bounds.Width, 4));
		    }
		}

		public static void DrawIndicator(Control control, Rectangle bounds)
		{
		    if (bounds == Rectangle.Empty) return;
            var handle = control?.Handle ?? IntPtr.Zero;
            var dc = GetDC(new HandleRef(control, handle));
		    var brush = CreateBrush();
		    var handle3 = SelectObject(new HandleRef(control, dc), new HandleRef(null, brush));
		    PatBlt(new HandleRef(control, dc), bounds.X, bounds.Y, bounds.Width, bounds.Height, 5898313);
		    SelectObject(new HandleRef(control, dc), new HandleRef(null, handle3));
		    DeleteObject(new HandleRef(null, brush));
		    ReleaseDC(new HandleRef(control, handle), new HandleRef(null, dc));
		}

        [GuessedName]
		private static IntPtr CreateBrush()
		{
			var array = new short[8];
		    for (var i = 0; i < 8; i++)
		        array[i] = (short) (21845 << (i & 1));
		    var bitmap = CreateBitmap(8, 8, 1, 1, array);
		    var brush = CreateBrushIndirect(new Logbrush {Color = ColorTranslator.ToWin32(Color.Black), Style = 3, Hatch = bitmap});
			DeleteObject(new HandleRef(null, bitmap));
			return brush;
		}

		[StructLayout(LayoutKind.Sequential)]
		private class Logbrush
		{
		    public int Style;
			public int Color;
			public IntPtr Hatch;
		}
	}
}
