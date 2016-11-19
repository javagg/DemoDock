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
			_control = control;
            _control.MouseMove += OnControlMouseMove;
            _control.MouseLeave += OnControlMouseLeave;
            _control.MouseDown += OnControlMouseDown;
            _control.MouseWheel += OnControlMouseWheel;
            _control.Disposed += OnControlDisposed;
            _control.FontChanged += OnControlFontChanged;
			_toolTip = new ToolTipsForm(this);
			_toolTip.MouseMove += OnToolTipMouseMove;
		    _timer = new Timer {Interval = SystemInformation.DoubleClickTime};
		    _timer.Tick += OnTimerTick;
		}

		public void Dispose()
		{
		    if (_disposed) return;
		    HideToolTip();
		    _toolTip.MouseMove -= OnToolTipMouseMove;
		    _toolTip.Dispose();
		    _toolTip = null;
		    _control.MouseMove -= OnControlMouseMove;
		    _control.MouseLeave -= OnControlMouseLeave;
		    _control.MouseDown -= OnControlMouseDown;
		    _control.MouseWheel -= OnControlMouseWheel;
		    _control.Disposed -= OnControlDisposed;
		    _control.FontChanged -= OnControlFontChanged;
		    _control = null;
		    _timer.Tick -= OnTimerTick;
		    _timer.Dispose();
		    _disposed = true;
		}

		private void OnToolTipMouseMove(object sender, MouseEventArgs e)
		{
			HideToolTip();
		}

		private void OnFormDeactivate(object sender, EventArgs e)
		{
			HideToolTip();
		}

		public void method_0(Point point, string text)
		{
			_toolTip.Text = text;
			var size = Size.Ceiling(_toolTip.method_0(text));
			size.Height += 4;
			size.Width += 4;
			point.Y += 19;
			var screen = Screen.FromPoint(point);
		    if (point.X < screen.Bounds.Left)
		        point.X = screen.Bounds.Left;
		    if (point.X + size.Width > screen.Bounds.Right)
			{
				point.X = screen.Bounds.Right - size.Width;
				if (point.X < screen.Bounds.Left)
				{
					return;
				}
			}
			if (point.Y < screen.Bounds.Top)
			{
				point.Y = screen.Bounds.Top;
			}
			if (point.Y + size.Height > screen.Bounds.Bottom)
			{
				point.Y = screen.Bounds.Bottom - size.Height;
				if (point.Y < screen.Bounds.Top)
				{
					return;
				}
				point.X++;
			}
			Native.SetWindowPos(_toolTip.Handle, new IntPtr(-1), point.X, point.Y, size.Width, size.Height, 80);
			var normal = VisualStyleElement.ToolTip.Standard.Normal;
			if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
			{
				var renderer = new VisualStyleRenderer(normal);
			    using (var g = _toolTip.CreateGraphics())
			        _toolTip.Region = renderer.GetBackgroundRegion(g, _toolTip.ClientRectangle);
			}
			_toolTip.Invalidate();
			_tooltipShown = true;
		    if (_ownerForm != null)
		        _ownerForm.Deactivate -= OnFormDeactivate;
		    _ownerForm = FindTopContainingForm(_control);
		    if (_ownerForm == null) return;
		    _ownerForm.Deactivate += OnFormDeactivate;
		    _toolTip.Owner = _ownerForm;
		}

		public void HideToolTip()
		{
			_toolTip.Owner = null;
			_toolTip.Visible = false;
			_tooltipShown = false;
		    if (_ownerForm == null) return;
		    _ownerForm.Deactivate -= OnFormDeactivate;
		    _ownerForm = null;
		}

		private void OnControlMouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.None)
			{
				return;
			}
			if (_tooltipShown)
			{
				var text = Event_0(new Point(e.X, e.Y));
				if (string.IsNullOrEmpty(text))
				{
					HideToolTip();
					return;
				}
				if (text.Length != 0 && text != _toolTip.Text)
				{
					method_0(Cursor.Position, text);
				}
			}
			else
			{
				var left = new Point(e.X, e.Y);
				if (left != point_0)
				{
					point_0 = left;
					_timer.Enabled = false;
					_timer.Enabled = true;
				}
			}
		}

		private Form FindTopContainingForm(Control control)
		{
		    while (control.Parent != null)
		        control = control.Parent;
		    return control as Form;
		}

		private void OnControlMouseLeave(object sender, EventArgs e)
		{
		    if (_tooltipShown)
		        HideToolTip();
		    _timer.Enabled = false;
		}

		private void OnControlMouseDown(object sender, MouseEventArgs e)
		{
		    if (_tooltipShown)
		        HideToolTip();
		    _timer.Enabled = false;
		}

		private void OnControlMouseWheel(object sender, MouseEventArgs e)
		{
		    if (_tooltipShown)
		        HideToolTip();
		    _timer.Enabled = false;
		}

		private void OnControlDisposed(object sender, EventArgs e)
		{
			Dispose();
		}

		private void OnControlFontChanged(object sender, EventArgs e)
		{
			_toolTip.Font = _control.Font;
		}

        private static bool smethod_0()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(5, 1, 0, 0);
        }

		private void OnTimerTick(object sender, EventArgs e)
		{
			_timer.Enabled = false;
			var point = _control.PointToClient(Cursor.Position);
			if (!_control.ClientRectangle.Contains(point))
			{
				return;
			}
			var text = Event_0(point);
		    if (string.IsNullOrEmpty(text)) return;
		    var form = FindTopContainingForm(_control);
		    var activeForm = Form.ActiveForm;
		    if (form != null && activeForm != null && (activeForm == form || activeForm == form.Owner) && _control.Visible)
		    {
		        method_0(Cursor.Position, text);
		    }
		}

		public bool Boolean_0 { get; set; } = true;

	    public bool ShowPrefix
		{
			get
			{
				return _toolTip.ShowPrefix;
			}
			set
			{
				_toolTip.ShowPrefix = value;
			}
		}

	    public event Delegate0 Event_0;

		private bool _tooltipShown;

	    private bool _disposed;

		private Control _control;

		private ToolTipsForm _toolTip;

		private Form _ownerForm;

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
				Font = tooltips._control.Font;
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
						SizeF sizeF = TextRenderer.MeasureText(graphics, text, Font, new Size(SystemInformation.PrimaryMonitorSize.Width, 2147483647), _tfFlags);
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
				e.Graphics.FillRectangle(SystemBrushes.Info, ClientRectangle);
				var pen = SystemInformation.HighContrast ? SystemPens.InfoText : SystemPens.Control;
				e.Graphics.DrawLine(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Right, ClientRectangle.Top);
				e.Graphics.DrawLine(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Left, ClientRectangle.Bottom);
				e.Graphics.DrawLine(SystemPens.InfoText, ClientRectangle.Left, ClientRectangle.Bottom - 1, ClientRectangle.Right, ClientRectangle.Bottom - 1);
				e.Graphics.DrawLine(SystemPens.InfoText, ClientRectangle.Right - 1, ClientRectangle.Top, ClientRectangle.Right - 1, ClientRectangle.Bottom);
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
