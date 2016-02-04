using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Form1 : Form
	{
		public Form1(bool hollow)
		{
			this.bool_0 = hollow;
			this.BackColor = SystemColors.Highlight;
			base.ShowInTaskbar = false;
			base.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		public void method_0(Rectangle rectangle_0, bool bool_1)
		{
			Form1.SetWindowPos(new HandleRef(this, base.Handle), new HandleRef(this, IntPtr.Zero), rectangle_0.X, rectangle_0.Y, rectangle_0.Width, rectangle_0.Height, 80);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			Form1.SetLayeredWindowAttributes(base.Handle, 0, 128, 2);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this.bool_0)
			{
				Rectangle clientRectangle = base.ClientRectangle;
				clientRectangle.Width--;
				clientRectangle.Height--;
				e.Graphics.DrawRectangle(SystemPens.ControlDark, clientRectangle);
				clientRectangle.Inflate(-1, -1);
				e.Graphics.DrawRectangle(SystemPens.ControlDark, clientRectangle);
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style = -2147483648;
				createParams.ExStyle |= 524288;
				return createParams;
			}
		}

		private bool bool_0;

		private const int int_0 = 2;

		private const int int_1 = 524288;

		private const int int_2 = 16;

		private const int int_3 = 64;

		private const int int_4 = 2;

		private const int int_5 = 1;
	}
}
