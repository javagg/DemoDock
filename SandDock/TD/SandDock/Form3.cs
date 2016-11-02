using System;
using System.Drawing;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Form3 : Form
	{
		public Form3()
		{
			FormBorderStyle = FormBorderStyle.None;
		}

		public void method_0(Bitmap bitmap_0, byte byte_0)
		{
			IntPtr dC = Native.GetDC(IntPtr.Zero);
			IntPtr intPtr = Native.CreateCompatibleDC(dC);
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr hObject = IntPtr.Zero;
			try
			{
				intPtr2 = bitmap_0.GetHbitmap(Color.FromArgb(0));
				hObject = Native.SelectObject(intPtr, intPtr2);
				Native.Size size = new Native.Size(bitmap_0.Width, bitmap_0.Height);
				Native.Point point = new Native.Point(0, 0);
				Native.Point point2 = new Native.Point(base.Left, base.Top);
				Native.BLENDFUNCTION bLENDFUNCTION = default(Native.BLENDFUNCTION);
				bLENDFUNCTION.BlendOp = 0;
				bLENDFUNCTION.BlendFlags = 0;
				bLENDFUNCTION.SourceConstantAlpha = byte_0;
				bLENDFUNCTION.AlphaFormat = 1;
				Native.UpdateLayeredWindow(base.Handle, dC, ref point2, ref size, intPtr, ref point, 0, ref bLENDFUNCTION, 2);
			}
			finally
			{
				if (intPtr2 != IntPtr.Zero)
				{
					Native.SelectObject(intPtr, hObject);
					Native.DeleteObject(intPtr2);
				}
				Native.ReleaseDC(IntPtr.Zero, dC);
				Native.DeleteDC(intPtr);
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				var cp = base.CreateParams;
				cp.ExStyle |= WS_SYSMENU;
				return cp;
			}
        }

        private const int WS_SYSMENU = 0x00080000; //524288 

    }
}
