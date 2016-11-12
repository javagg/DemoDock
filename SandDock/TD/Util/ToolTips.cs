using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TD.SandDock;

namespace TD.Util
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class GuessedNameAttribute : Attribute
    {
    }

	internal class ToolTips : IDisposable
	{
		public ToolTips(Control control)
		{
			this.control_0 = control;
			control.MouseMove += this.method_2;
			control.MouseLeave += this.method_4;
			control.MouseDown += this.method_5;
			control.MouseWheel += this.method_6;
			control.Disposed += this.method_7;
			control.FontChanged += this.method_8;
			this._toolTipsForm0 = new ToolTipsForm(this);
			this._toolTipsForm0.MouseMove += OnMouseMove;
		    _timer = new Timer {Interval = SystemInformation.DoubleClickTime};
		    _timer.Tick += OnTimerTick;
		}

		public void Dispose()
		{
		    if (_disposed) return;
		    this.method_1();
		    this._toolTipsForm0.MouseMove -= OnMouseMove;
		    this._toolTipsForm0.Dispose();
		    this._toolTipsForm0 = null;
		    this.control_0.MouseMove -= this.method_2;
		    this.control_0.MouseLeave -= this.method_4;
		    this.control_0.MouseDown -= this.method_5;
		    this.control_0.MouseWheel -= this.method_6;
		    this.control_0.Disposed -= this.method_7;
		    this.control_0.FontChanged -= this.method_8;
		    this.control_0 = null;
		    _timer.Tick -= OnTimerTick;
		    _timer.Dispose();
		    _disposed = true;
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			this.method_1();
		}

		private void OnDeactivate(object sender, EventArgs e)
		{
			this.method_1();
		}

		public void method_0(Point point_1, string text)
		{
			_toolTipsForm0.Text = text;
			var size = Size.Ceiling(_toolTipsForm0.method_0(text));
			size.Height += 4;
			size.Width += 4;
			point_1.Y += 19;
			var screen = Screen.FromPoint(point_1);
		    if (point_1.X < screen.Bounds.Left)
		        point_1.X = screen.Bounds.Left;
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
			Native.SetWindowPos(_toolTipsForm0.Handle, new IntPtr(-1), point_1.X, point_1.Y, size.Width, size.Height, 80);
			var normal = VisualStyleElement.ToolTip.Standard.Normal;
			if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
			{
				var renderer = new VisualStyleRenderer(normal);
			    using (var g = _toolTipsForm0.CreateGraphics())
			        _toolTipsForm0.Region = renderer.GetBackgroundRegion(g, _toolTipsForm0.ClientRectangle);
			}
			this._toolTipsForm0.Invalidate();
			this.bool_0 = true;
			if (this.form_0 != null)
			{
				this.form_0.Deactivate -= OnDeactivate;
			}
			this.form_0 = this.method_3(this.control_0);
		    if (this.form_0 == null) return;
		    this.form_0.Deactivate += this.OnDeactivate;
		    this._toolTipsForm0.Owner = this.form_0;
		}

		public void method_1()
		{
			this._toolTipsForm0.Owner = null;
			this._toolTipsForm0.Visible = false;
			this.bool_0 = false;
		    if (this.form_0 == null) return;
		    this.form_0.Deactivate -= this.OnDeactivate;
		    this.form_0 = null;
		}

		private void method_2(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.None)
			{
				return;
			}
			if (this.bool_0)
			{
				var text = Event_0(new Point(e.X, e.Y));
				if (string.IsNullOrEmpty(text))
				{
					this.method_1();
					return;
				}
				if (text.Length != 0 && text != this._toolTipsForm0.Text)
				{
					this.method_0(Cursor.Position, text);
				}
			}
			else
			{
				var left = new Point(e.X, e.Y);
				if (left != this.point_0)
				{
					this.point_0 = left;
					this._timer.Enabled = false;
					this._timer.Enabled = true;
				}
			}
		}

		private Form method_3(Control control)
		{
			while (control.Parent != null)
			{
				control = control.Parent;
			}
			return control as Form;
		}

		private void method_4(object sender, EventArgs e)
		{
			if (this.bool_0)
			{
				this.method_1();
			}
			this._timer.Enabled = false;
		}

		private void method_5(object sender, MouseEventArgs e)
		{
			if (this.bool_0)
			{
				this.method_1();
			}
			this._timer.Enabled = false;
		}

		private void method_6(object sender, MouseEventArgs e)
		{
			if (this.bool_0)
			{
				this.method_1();
			}
			this._timer.Enabled = false;
		}

		private void method_7(object sender, EventArgs e)
		{
			Dispose();
		}

		private void method_8(object sender, EventArgs e)
		{
			this._toolTipsForm0.Font = control_0.Font;
		}

        private static bool smethod_0()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(5, 1, 0, 0);
        }

		private void OnTimerTick(object sender, EventArgs e)
		{
			_timer.Enabled = false;
			var point = this.control_0.PointToClient(Cursor.Position);
			if (!control_0.ClientRectangle.Contains(point))
			{
				return;
			}
			string text = Event_0(point);
		    if (string.IsNullOrEmpty(text)) return;
		    Form form = this.method_3(this.control_0);
		    var activeForm = Form.ActiveForm;
		    if (form != null && activeForm != null && (activeForm == form || activeForm == form.Owner) && this.control_0.Visible)
		    {
		        this.method_0(Cursor.Position, text);
		    }
		}

		public bool Boolean_0 { get; set; } = true;

	    public bool Boolean_1
		{
			get
			{
				return this._toolTipsForm0.ShowPrefix;
			}
			set
			{
				this._toolTipsForm0.ShowPrefix = value;
			}
		}

	    public event Delegate0 Event_0;

		private bool bool_0;

	    private bool _disposed;

		private Control control_0;

		private ToolTipsForm _toolTipsForm0;

		private Form form_0;

		private const int int_0 = 16;

		private const int int_1 = 64;

		private const int int_2 = 128;

		private const int int_3 = 2;

		private const int int_4 = 1;

		private Point point_0;

		private readonly Timer _timer;

		internal delegate string Delegate0(Point location);

		private class ToolTipsForm : Form
		{
			public ToolTipsForm(ToolTips tooltips)
			{
				_parent = tooltips;
				SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
				Font = tooltips.control_0.Font;
				_tfFlags = (TextFormatFlags.NoClipping | TextFormatFlags.VerticalCenter);
				ShowInTaskbar = false;
				FormBorderStyle = FormBorderStyle.None;
				ControlBox = false;
				StartPosition = FormStartPosition.Manual;
			}

			public SizeF method_0(string text)
			{
				SizeF result;
				using (var graphics = CreateGraphics())
				{
					var normal = VisualStyleElement.ToolTip.Standard.Normal;
					if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
					{
						var renderer = new VisualStyleRenderer(normal);
						var textExtent = renderer.GetTextExtent(graphics, text, TextFormatFlags.Default);
						result = renderer.GetBackgroundExtent(graphics, textExtent).Size;
					}
					else
					{
						SizeF sizeF = TextRenderer.MeasureText(graphics, text, Font, new Size(SystemInformation.PrimaryMonitorSize.Width, 2147483647), this._tfFlags);
						sizeF.Width -= 2f;
						sizeF.Height += 2f;
						result = sizeF;
					}
				}
				return result;
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				var normal = VisualStyleElement.ToolTip.Standard.Normal;
				if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
				{
					var renderer = new VisualStyleRenderer(normal);
					renderer.DrawBackground(e.Graphics, ClientRectangle);
					var textExtent = renderer.GetTextExtent(e.Graphics, ClientRectangle, Text, _tfFlags);
					textExtent.X = ClientRectangle.X + ClientRectangle.Width / 2 - textExtent.Width / 2;
					textExtent.Y = ClientRectangle.Y + ClientRectangle.Height / 2 - textExtent.Height / 2;
					renderer.DrawText(e.Graphics, textExtent, Text, false, _tfFlags);
					return;
				}
				e.Graphics.FillRectangle(SystemBrushes.Info, base.ClientRectangle);
				var pen = SystemInformation.HighContrast ? SystemPens.InfoText : SystemPens.Control;
				e.Graphics.DrawLine(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Right, ClientRectangle.Top);
				e.Graphics.DrawLine(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Left, ClientRectangle.Bottom);
				e.Graphics.DrawLine(SystemPens.InfoText, base.ClientRectangle.Left, base.ClientRectangle.Bottom - 1, base.ClientRectangle.Right, base.ClientRectangle.Bottom - 1);
				e.Graphics.DrawLine(SystemPens.InfoText, base.ClientRectangle.Right - 1, base.ClientRectangle.Top, base.ClientRectangle.Right - 1, base.ClientRectangle.Bottom);
				var crect = ClientRectangle;
				crect.Inflate(-2, -2);
				TextRenderer.DrawText(e.Graphics, Text, Font, crect, SystemColors.InfoText, _tfFlags);
			}

			public bool ShowPrefix
			{
				get
				{
					return (_tfFlags & TextFormatFlags.HidePrefix) != TextFormatFlags.HidePrefix;
				}
				set
				{
				    if (value)
				    {
				        _tfFlags |= TextFormatFlags.HidePrefix;
				        _tfFlags &= ~TextFormatFlags.NoPrefix;
				    }
				    else
				    {
				        _tfFlags &= ~TextFormatFlags.HidePrefix;
				        _tfFlags |= TextFormatFlags.NoPrefix;
				    }
				}
			}

			private static bool Boolean_1
			{
				get
				{
					int value = 0;
					if (smethod_0())
					{
						Native.SystemParametersInfo(SPI_GETDROPSHADOW, 0, ref value, 0);
						return Convert.ToBoolean(value);
					}
					return false;
				}
			}

			protected override CreateParams CreateParams
			{
				get
				{
					var cp = base.CreateParams;
					if (_parent != null && _parent.Boolean_0 && Boolean_1)
					{
						cp.ClassStyle |= CS_DROPSHADOW;
					}
					return cp;
				}
			}

			private readonly ToolTips _parent;

			private const int int_0 = 32;

			private const int int_1 = -2147483648;

			private const int CS_DROPSHADOW = 131072;// 0x20000

			private const int SPI_GETDROPSHADOW = 4132;

			private TextFormatFlags _tfFlags;
		}
	}
}
