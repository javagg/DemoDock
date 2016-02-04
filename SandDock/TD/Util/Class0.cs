using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TD.Util
{
	internal class Class0 : IDisposable
	{
		public Class0(Control control)
		{
			this.control_0 = control;
			control.MouseMove += new MouseEventHandler(this.method_2);
			control.MouseLeave += new EventHandler(this.method_4);
			control.MouseDown += new MouseEventHandler(this.method_5);
			control.MouseWheel += new MouseEventHandler(this.method_6);
			control.Disposed += new EventHandler(this.method_7);
			control.FontChanged += new EventHandler(this.method_8);
			this.form0_0 = new Class0.Form0(this);
			this.form0_0.MouseMove += new MouseEventHandler(this.form0_0_MouseMove);
			this.timer_0 = new Timer();
			this.timer_0.Interval = SystemInformation.DoubleClickTime;
			this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
		}

		public void Dispose()
		{
			if (!this.bool_2)
			{
				this.method_1();
				this.form0_0.MouseMove -= new MouseEventHandler(this.form0_0_MouseMove);
				this.form0_0.Dispose();
				this.form0_0 = null;
				this.control_0.MouseMove -= new MouseEventHandler(this.method_2);
				this.control_0.MouseLeave -= new EventHandler(this.method_4);
				this.control_0.MouseDown -= new MouseEventHandler(this.method_5);
				this.control_0.MouseWheel -= new MouseEventHandler(this.method_6);
				this.control_0.Disposed -= new EventHandler(this.method_7);
				this.control_0.FontChanged -= new EventHandler(this.method_8);
				this.control_0 = null;
				this.timer_0.Tick -= new EventHandler(this.timer_0_Tick);
				this.timer_0.Dispose();
				this.bool_2 = true;
			}
		}

		private void form0_0_MouseMove(object sender, MouseEventArgs e)
		{
			this.method_1();
		}

		private void form_0_Deactivate(object sender, EventArgs e)
		{
			this.method_1();
		}

		public void method_0(Point point_1, string string_0)
		{
			this.form0_0.Text = string_0;
			Size size = Size.Ceiling(this.form0_0.method_0(string_0));
			size.Height += 4;
			size.Width += 4;
			point_1.Y += 19;
			Screen screen = Screen.FromPoint(point_1);
			if (point_1.X < screen.Bounds.Left)
			{
				point_1.X = screen.Bounds.Left;
			}
			if (point_1.X + size.Width > screen.Bounds.Right)
			{
				point_1.X = screen.Bounds.Right - size.Width;
				if (point_1.X < screen.Bounds.Left)
				{
					return;
				}
			}
			if (point_1.Y < screen.Bounds.Top)
			{
				point_1.Y = screen.Bounds.Top;
			}
			if (point_1.Y + size.Height > screen.Bounds.Bottom)
			{
				point_1.Y = screen.Bounds.Bottom - size.Height;
				if (point_1.Y < screen.Bounds.Top)
				{
					return;
				}
				point_1.X++;
			}
			Class0.SetWindowPos(this.form0_0.Handle, -1, point_1.X, point_1.Y, size.Width, size.Height, 80);
			VisualStyleElement normal = VisualStyleElement.ToolTip.Standard.Normal;
			if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
			{
				VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(normal);
				using (Graphics graphics = this.form0_0.CreateGraphics())
				{
					this.form0_0.Region = visualStyleRenderer.GetBackgroundRegion(graphics, this.form0_0.ClientRectangle);
				}
			}
			this.form0_0.Invalidate();
			this.bool_0 = true;
			if (this.form_0 != null)
			{
				this.form_0.Deactivate -= new EventHandler(this.form_0_Deactivate);
			}
			this.form_0 = this.method_3(this.control_0);
			if (this.form_0 != null)
			{
				this.form_0.Deactivate += new EventHandler(this.form_0_Deactivate);
				this.form0_0.Owner = this.form_0;
			}
		}

		public void method_1()
		{
			this.form0_0.Owner = null;
			this.form0_0.Visible = false;
			this.bool_0 = false;
			if (this.form_0 != null)
			{
				this.form_0.Deactivate -= new EventHandler(this.form_0_Deactivate);
				this.form_0 = null;
			}
		}

		private void method_2(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.None)
			{
				return;
			}
			if (this.bool_0)
			{
				string text = this.delegate0_0(new Point(e.X, e.Y));
				if (text == null || text.Length == 0)
				{
					this.method_1();
					return;
				}
				if (text.Length != 0 && text != this.form0_0.Text)
				{
					this.method_0(Cursor.Position, text);
					return;
				}
			}
			else
			{
				Point left = new Point(e.X, e.Y);
				if (left != this.point_0)
				{
					this.point_0 = left;
					this.timer_0.Enabled = false;
					this.timer_0.Enabled = true;
				}
			}
		}

		private Form method_3(Control control_1)
		{
			while (control_1.Parent != null)
			{
				control_1 = control_1.Parent;
			}
			return control_1 as Form;
		}

		private void method_4(object sender, EventArgs e)
		{
			if (this.bool_0)
			{
				this.method_1();
			}
			this.timer_0.Enabled = false;
		}

		private void method_5(object sender, MouseEventArgs e)
		{
			if (this.bool_0)
			{
				this.method_1();
			}
			this.timer_0.Enabled = false;
		}

		private void method_6(object sender, MouseEventArgs e)
		{
			if (this.bool_0)
			{
				this.method_1();
			}
			this.timer_0.Enabled = false;
		}

		private void method_7(object sender, EventArgs e)
		{
			this.Dispose();
		}

		private void method_8(object sender, EventArgs e)
		{
			this.form0_0.Font = this.control_0.Font;
		}

		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int flags);

		private static bool smethod_0()
		{
			bool result = false;
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				result = (Environment.OSVersion.Version >= new Version(5, 1, 0, 0));
			}
			return result;
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			this.timer_0.Enabled = false;
			Point point = this.control_0.PointToClient(Cursor.Position);
			if (!this.control_0.ClientRectangle.Contains(point))
			{
				return;
			}
			string text = this.delegate0_0(point);
			if (text != null && text.Length != 0)
			{
				Form form = this.method_3(this.control_0);
				Form activeForm = Form.ActiveForm;
				if (form != null && activeForm != null && (activeForm == form || activeForm == form.Owner) && this.control_0.Visible)
				{
					this.method_0(Cursor.Position, text);
				}
				return;
			}
		}

		public bool Boolean_0
		{
			get
			{
				return this.bool_1;
			}
			set
			{
				this.bool_1 = value;
			}
		}

		public bool Boolean_1
		{
			get
			{
				return this.form0_0.Boolean_0;
			}
			set
			{
				this.form0_0.Boolean_0 = value;
			}
		}

		public event Class0.Delegate0 Event_0
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.delegate0_0 = (Class0.Delegate0)Delegate.Combine(this.delegate0_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.delegate0_0 = (Class0.Delegate0)Delegate.Remove(this.delegate0_0, value);
			}
		}

		private bool bool_0;

		private bool bool_1 = true;

		private bool bool_2;

		private Control control_0;

		private Class0.Delegate0 delegate0_0;

		private Class0.Form0 form0_0;

		private Form form_0;

		private const int int_0 = 16;

		private const int int_1 = 64;

		private const int int_2 = 128;

		private const int int_3 = 2;

		private const int int_4 = 1;

		private Point point_0;

		private Timer timer_0;

		internal delegate string Delegate0(Point location);

		private class Form0 : Form
		{
			public Form0(Class0 tooltips)
			{
				this.class0_0 = tooltips;
				base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
				this.Font = tooltips.control_0.Font;
				this.textFormatFlags_0 = (TextFormatFlags.NoClipping | TextFormatFlags.VerticalCenter);
				base.ShowInTaskbar = false;
				base.FormBorderStyle = FormBorderStyle.None;
				base.ControlBox = false;
				base.StartPosition = FormStartPosition.Manual;
			}

			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
			}

			public SizeF method_0(string string_0)
			{
				SizeF result;
				using (Graphics graphics = base.CreateGraphics())
				{
					VisualStyleElement normal = VisualStyleElement.ToolTip.Standard.Normal;
					if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
					{
						VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(normal);
						Rectangle textExtent = visualStyleRenderer.GetTextExtent(graphics, string_0, TextFormatFlags.Default);
						result = visualStyleRenderer.GetBackgroundExtent(graphics, textExtent).Size;
					}
					else
					{
						SizeF sizeF = TextRenderer.MeasureText(graphics, string_0, this.Font, new Size(SystemInformation.PrimaryMonitorSize.Width, 2147483647), this.textFormatFlags_0);
						sizeF.Width -= 2f;
						sizeF.Height += 2f;
						result = sizeF;
					}
				}
				return result;
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				VisualStyleElement normal = VisualStyleElement.ToolTip.Standard.Normal;
				if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
				{
					VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(normal);
					visualStyleRenderer.DrawBackground(e.Graphics, base.ClientRectangle);
					Rectangle textExtent = visualStyleRenderer.GetTextExtent(e.Graphics, base.ClientRectangle, this.Text, this.textFormatFlags_0);
					textExtent.X = base.ClientRectangle.X + base.ClientRectangle.Width / 2 - textExtent.Width / 2;
					textExtent.Y = base.ClientRectangle.Y + base.ClientRectangle.Height / 2 - textExtent.Height / 2;
					visualStyleRenderer.DrawText(e.Graphics, textExtent, this.Text, false, this.textFormatFlags_0);
					return;
				}
				e.Graphics.FillRectangle(SystemBrushes.Info, base.ClientRectangle);
				Pen pen = (!SystemInformation.HighContrast) ? SystemPens.Control : SystemPens.InfoText;
				e.Graphics.DrawLine(pen, base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Right, base.ClientRectangle.Top);
				e.Graphics.DrawLine(pen, base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Left, base.ClientRectangle.Bottom);
				e.Graphics.DrawLine(SystemPens.InfoText, base.ClientRectangle.Left, base.ClientRectangle.Bottom - 1, base.ClientRectangle.Right, base.ClientRectangle.Bottom - 1);
				e.Graphics.DrawLine(SystemPens.InfoText, base.ClientRectangle.Right - 1, base.ClientRectangle.Top, base.ClientRectangle.Right - 1, base.ClientRectangle.Bottom);
				Rectangle clientRectangle = base.ClientRectangle;
				clientRectangle.Inflate(-2, -2);
				TextRenderer.DrawText(e.Graphics, this.Text, this.Font, clientRectangle, SystemColors.InfoText, this.textFormatFlags_0);
			}

			[DllImport("user32.dll")]
			private static extern bool SystemParametersInfo(int nAction, int nParam, ref int i, int nUpdate);

			public bool Boolean_0
			{
				get
				{
					return (this.textFormatFlags_0 & TextFormatFlags.HidePrefix) != TextFormatFlags.HidePrefix;
				}
				set
				{
					if (value)
					{
						this.textFormatFlags_0 |= TextFormatFlags.HidePrefix;
						this.textFormatFlags_0 &= ~TextFormatFlags.NoPrefix;
						return;
					}
					this.textFormatFlags_0 &= ~TextFormatFlags.HidePrefix;
					this.textFormatFlags_0 |= TextFormatFlags.NoPrefix;
				}
			}

			private static bool Boolean_1
			{
				get
				{
					int value = 0;
					if (Class0.smethod_0())
					{
						Class0.Form0.SystemParametersInfo(4132, 0, ref value, 0);
						return Convert.ToBoolean(value);
					}
					return false;
				}
			}

			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					if (this.class0_0 != null && this.class0_0.Boolean_0 && Class0.Form0.Boolean_1)
					{
						createParams.ClassStyle |= 131072;
					}
					return createParams;
				}
			}

			private Class0 class0_0;

			private const int int_0 = 32;

			private const int int_1 = -2147483648;

			private const int int_2 = 131072;

			private const int int_3 = 4132;

			private TextFormatFlags textFormatFlags_0;
		}
	}
}
