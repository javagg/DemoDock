using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TD.SandDock;

namespace TD.Util
{
    internal enum NamingType
    {
        WildGuessed,
        FromOldVersion,
        Deduced
    }
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class NamingAttribute : Attribute
    {
        public NamingAttribute(NamingType type= NamingType.WildGuessed) { }
    }

	internal class Tooltip : IDisposable
	{
		public Tooltip(Control control)
		{
			_control = control;
            _control.MouseMove += OnControlMouseMove;
            _control.MouseLeave += OnControlMouseLeave;
            _control.MouseDown += OnControlMouseDown;
            _control.MouseWheel += OnControlMouseWheel;
            _control.Disposed += OnControlDisposed;
            _control.FontChanged += OnControlFontChanged;
			_tooltipForm = new TooltipForm(this);
			_tooltipForm.MouseMove += OnTooltipFormMouseMove;
		    _timer = new Timer {Interval = SystemInformation.DoubleClickTime};
		    _timer.Tick += OnTimerTick;
		}

		public void Dispose()
		{
		    if (_disposed) return;
		    HideTooltip();
		    _tooltipForm.MouseMove -= OnTooltipFormMouseMove;
		    _tooltipForm.Dispose();
		    _tooltipForm = null;
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

		private void OnTooltipFormMouseMove(object sender, MouseEventArgs e)
		{
			HideTooltip();
		}

		private void OnFormDeactivate(object sender, EventArgs e)
		{
			HideTooltip();
		}

		public void DrawTooltip(Point point, string text)
		{
			_tooltipForm.Text = text;
			var size = Size.Ceiling(_tooltipForm.CalculateTextSize(text));
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
			        return;
			}
		    if (point.Y < screen.Bounds.Top)
		        point.Y = screen.Bounds.Top;
		    if (point.Y + size.Height > screen.Bounds.Bottom)
			{
				point.Y = screen.Bounds.Bottom - size.Height;
			    if (point.Y < screen.Bounds.Top)
			        return;
			    point.X++;
			}
			//Native.SetWindowPos(_tooltipForm.Handle, WMConstants.HWND_TOPMOST, point.X, point.Y, size.Width, size.Height, WMConstants.SWP_SHOWWINDOW + WMConstants.SWP_NOACTIVATE);
            _tooltipForm.Location = point;
            _tooltipForm.Size = size;
            _tooltipForm.Visible = true;

            var normal = VisualStyleElement.ToolTip.Standard.Normal;
			if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
			{
				var renderer = new VisualStyleRenderer(normal);
			    using (var g = _tooltipForm.CreateGraphics())
			        _tooltipForm.Region = renderer.GetBackgroundRegion(g, _tooltipForm.ClientRectangle);
			}
			_tooltipForm.Invalidate();
			_visible = true;
		    if (_ownerForm != null)
		        _ownerForm.Deactivate -= OnFormDeactivate;
		    _ownerForm = FindTopContainingForm(_control);
		    if (_ownerForm == null) return;
		    _ownerForm.Deactivate += OnFormDeactivate;
		    _tooltipForm.Owner = _ownerForm;
		}

		public void HideTooltip()
		{
			_tooltipForm.Owner = null;
			_tooltipForm.Visible = false;
			_visible = false;
		    if (_ownerForm == null) return;
		    _ownerForm.Deactivate -= OnFormDeactivate;
		    _ownerForm = null;
		}

		private void OnControlMouseMove(object sender, MouseEventArgs e)
		{
		    if (e.Button != MouseButtons.None) return;
		    if (_visible)
			{
				var text = GetTooltipText(new Point(e.X, e.Y));
				if (string.IsNullOrEmpty(text))
				{
					HideTooltip();
					return;
				}
				if (text.Length != 0 && text != _tooltipForm.Text)
				{
					DrawTooltip(Cursor.Position, text);
				}
			}
			else
			{
				var newPoint = new Point(e.X, e.Y);
				if (newPoint != lastPoint)
				{
					lastPoint = newPoint;
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
		    if (_visible)
		        HideTooltip();
		    _timer.Enabled = false;
		}

		private void OnControlMouseDown(object sender, MouseEventArgs e)
		{
		    if (_visible)
		        HideTooltip();
		    _timer.Enabled = false;
		}

		private void OnControlMouseWheel(object sender, MouseEventArgs e)
		{
		    if (_visible)
		        HideTooltip();
		    _timer.Enabled = false;
		}

		private void OnControlDisposed(object sender, EventArgs e)
		{
			Dispose();
		}

		private void OnControlFontChanged(object sender, EventArgs e)
		{
			_tooltipForm.Font = _control.Font;
		}

        private static bool LaterThanWinXp()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(5, 1, 0, 0);
        }

		private void OnTimerTick(object sender, EventArgs e)
		{
			_timer.Enabled = false;
			var point = _control.PointToClient(Cursor.Position);
		    if (!_control.ClientRectangle.Contains(point)) return;
		    var text = GetTooltipText(point);
		    if (string.IsNullOrEmpty(text)) return;
		    var form = FindTopContainingForm(_control);
		    var activeForm = Form.ActiveForm;
		    if (form != null && activeForm != null && (activeForm == form || activeForm == form.Owner) && _control.Visible)
		        DrawTooltip(Cursor.Position, text);
		}

        [Naming(NamingType.FromOldVersion)]
        public bool DropShadow { get; set; } = true;
        [Naming(NamingType.FromOldVersion)]
        public bool ProcessHotKeys
		{
			get
			{
				return _tooltipForm.ProcessHotKeys;
			}
			set
			{
				_tooltipForm.ProcessHotKeys = value;
			}
		}
        [Naming(NamingType.FromOldVersion)]
	    public event GetTooltipTextEventHandler GetTooltipText;

		private bool _visible;

	    private bool _disposed;

		private Control _control;

		private TooltipForm _tooltipForm;

		private Form _ownerForm;

		private const int int_0 = 16;

		private const int int_1 = 64;

		private const int int_2 = 128;

		private const int int_3 = 2;

		private const int int_4 = 1;

		private Point lastPoint;

		private readonly Timer _timer;

        [Naming(NamingType.FromOldVersion)]
        internal delegate string GetTooltipTextEventHandler(Point location);

		private class TooltipForm : Form
		{
			public TooltipForm(Tooltip tooltip)
			{
				_parent = tooltip;
				SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
				Font = tooltip._control.Font;
				_textFormat = TextFormatFlags.NoClipping | TextFormatFlags.VerticalCenter;
				ShowInTaskbar = false;
				FormBorderStyle = FormBorderStyle.None;
				ControlBox = false;
				StartPosition = FormStartPosition.Manual;
            }

            [Naming]
			public SizeF CalculateTextSize(string text)
			{
				SizeF result;
				using (var g = CreateGraphics())
				{
					var normal = VisualStyleElement.ToolTip.Standard.Normal;
					if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
					{
						var renderer = new VisualStyleRenderer(normal);
						var textExtent = renderer.GetTextExtent(g, text, TextFormatFlags.Default);
						result = renderer.GetBackgroundExtent(g, textExtent).Size;
					}
					else
					{
						SizeF sizeF = TextRenderer.MeasureText(g, text, Font, new Size(SystemInformation.PrimaryMonitorSize.Width, int.MaxValue), _textFormat);
						sizeF.Width -= 2;
						sizeF.Height += 2;
						result = sizeF;
					}
				}
				return result;
			}

			protected override void OnPaint(PaintEventArgs e)
			{
			    var g = e.Graphics;
			    var normal = VisualStyleElement.ToolTip.Standard.Normal;
			    if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(normal))
			    {
			        var renderer = new VisualStyleRenderer(normal);
			        renderer.DrawBackground(g, ClientRectangle);
			        var textExtent = renderer.GetTextExtent(g, ClientRectangle, Text, _textFormat);
			        textExtent.X = ClientRectangle.X + ClientRectangle.Width/2 - textExtent.Width/2;
			        textExtent.Y = ClientRectangle.Y + ClientRectangle.Height/2 - textExtent.Height/2;
			        renderer.DrawText(g, textExtent, Text, false, _textFormat);
			    }
			    else
			    {
			        g.FillRectangle(SystemBrushes.Info, ClientRectangle);
			        var pen = SystemInformation.HighContrast ? SystemPens.InfoText : SystemPens.Control;
			        g.DrawLine(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Right, ClientRectangle.Top);
			        g.DrawLine(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Left,ClientRectangle.Bottom);
			        g.DrawLine(SystemPens.InfoText, ClientRectangle.Left, ClientRectangle.Bottom - 1,ClientRectangle.Right, ClientRectangle.Bottom - 1);
			        g.DrawLine(SystemPens.InfoText, ClientRectangle.Right - 1, ClientRectangle.Top,ClientRectangle.Right - 1, ClientRectangle.Bottom);
			        var crect = ClientRectangle;
			        crect.Inflate(-2, -2);
			        TextRenderer.DrawText(e.Graphics, Text, Font, crect, SystemColors.InfoText, _textFormat);
			    }
			}
            [Naming(NamingType.FromOldVersion)]
            public bool ProcessHotKeys
			{
				get
				{
					return (_textFormat & TextFormatFlags.HidePrefix) != TextFormatFlags.HidePrefix;
				}
				set
				{
				    if (value)
				    {
				        _textFormat |= TextFormatFlags.HidePrefix;
				        _textFormat &= ~TextFormatFlags.NoPrefix;
				    }
				    else
				    {
				        _textFormat &= ~TextFormatFlags.HidePrefix;
				        _textFormat |= TextFormatFlags.NoPrefix;
				    }
				}
			}

			private static bool DropShadow
			{
				get
				{
					int value = 0;
				    if (!LaterThanWinXp()) return false;
                    if (!Native.IsMono())
                        Native.SystemParametersInfo(SPI_GETDROPSHADOW, 0, ref value, 0);
				    return Convert.ToBoolean(value);
				}
			}

            // This truely prevents focus stealing
            protected override bool ShowWithoutActivation => true;

            protected override CreateParams CreateParams
			{
				get
				{
					var cp = base.CreateParams;
				    if (_parent != null && _parent.DropShadow && DropShadow)
				        cp.ClassStyle |= CS_DROPSHADOW;
				    cp.ExStyle |= WS_EX_NOACTIVATE;
                    return cp;
				}
			}

		    private readonly Tooltip _parent;

			private const int int_0 = 32;


		    private const int WS_EX_NOACTIVATE = 0x08000000;
			private const int CS_DROPSHADOW = 131072;// 0x20000

			private const int SPI_GETDROPSHADOW = 4132;

			private TextFormatFlags _textFormat;
		}
	}
}
