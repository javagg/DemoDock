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
			_hollow = hollow;
			BackColor = SystemColors.Highlight;
			ShowInTaskbar = false;
			SetStyle(ControlStyles.ResizeRedraw, true);
		}

		public void method_0(Rectangle rect, bool bool_1)
		{
            Native.SetWindowPos(Handle, IntPtr.Zero, rect.X, rect.Y, rect.Width, rect.Height, 80);
        }

        protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			Native.SetLayeredWindowAttributes(Handle, 0, 128, 2);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (_hollow)
			{
				var rect = ClientRectangle;
				rect.Width--;
				rect.Height--;
				e.Graphics.DrawRectangle(SystemPens.ControlDark, rect);
				rect.Inflate(-1, -1);
				e.Graphics.DrawRectangle(SystemPens.ControlDark, rect);
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				var createParams = base.CreateParams;
				createParams.Style = -2147483648;
				createParams.ExStyle |= WS_SYSMENU;
				return createParams;
			}
		}

		private bool _hollow;

		private const int int_0 = 2;

		private const int WS_SYSMENU = 0x00080000; //524288 

        private const int int_2 = 16;

		private const int int_3 = 64;

		private const int int_4 = 2;

		private const int int_5 = 1;
	}
}
