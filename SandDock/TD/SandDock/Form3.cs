using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Form3 : Form
	{
		public Form3()
		{
			base.FormBorderStyle = FormBorderStyle.None;
		}

		public void method_0(Bitmap bitmap_0, byte byte_0)
		{
			IntPtr dC = Form3.Class21.GetDC(IntPtr.Zero);
			IntPtr intPtr = Form3.Class21.CreateCompatibleDC(dC);
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr hObject = IntPtr.Zero;
			try
			{
				intPtr2 = bitmap_0.GetHbitmap(Color.FromArgb(0));
				hObject = Form3.Class21.SelectObject(intPtr, intPtr2);
				Form3.Class21.Size size = new Form3.Class21.Size(bitmap_0.Width, bitmap_0.Height);
				Form3.Class21.Point point = new Form3.Class21.Point(0, 0);
				Form3.Class21.Point point2 = new Form3.Class21.Point(base.Left, base.Top);
				Form3.Class21.BLENDFUNCTION bLENDFUNCTION = default(Form3.Class21.BLENDFUNCTION);
				bLENDFUNCTION.BlendOp = 0;
				bLENDFUNCTION.BlendFlags = 0;
				bLENDFUNCTION.SourceConstantAlpha = byte_0;
				bLENDFUNCTION.AlphaFormat = 1;
				Form3.Class21.UpdateLayeredWindow(base.Handle, dC, ref point2, ref size, intPtr, ref point, 0, ref bLENDFUNCTION, 2);
			}
			finally
			{
				if (intPtr2 != IntPtr.Zero)
				{
					Form3.Class21.SelectObject(intPtr, hObject);
					Form3.Class21.DeleteObject(intPtr2);
				}
				Form3.Class21.ReleaseDC(IntPtr.Zero, dC);
				Form3.Class21.DeleteDC(intPtr);
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 524288;
				return createParams;
			}
		}

		private class Class21
		{
			private Class21()
			{
			}

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
			public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Form3.Class21.Point pptDst, ref Form3.Class21.Size psize, IntPtr hdcSrc, ref Form3.Class21.Point pprSrc, int crKey, ref Form3.Class21.BLENDFUNCTION pblend, int dwFlags);

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
		}
	}
}
