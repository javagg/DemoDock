using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
    public enum WindowsColorScheme
    {
        Automatic,
        Standard,
        LunaBlue,
        LunaOlive,
        LunaSilver
    }

    public abstract class ThemeAwareRendererBase : RendererBase
	{
        protected abstract void ApplyLunaBlueColors();

		protected abstract void ApplyLunaOliveColors();

		protected abstract void ApplyLunaSilverColors();

		protected abstract void ApplyStandardColors();

		protected internal override void DrawAutoHideBarBackground(Control container, Control autoHideBar, Graphics graphics, Rectangle bounds)
		{
			this.method_1(container, autoHideBar, graphics, bounds);
		}

		protected internal override void DrawSplitter(Control container, Control control, Graphics graphics, Rectangle bounds, Orientation orientation)
		{
			if (container != null)
			{
				this.method_1(container, control, graphics, bounds);
			}
		}

		protected internal override void DrawTabStripBackground(Control container, Control control, Graphics graphics, Rectangle bounds, int selectedTabOffset)
		{
			this.method_1(container, control, graphics, bounds);
		}

		public override void FinishRenderSession()
		{
			this.int_0 = Math.Max(this.int_0 - 1, 0);
		}

		protected override void GetColorsFromSystem()
		{
			switch (_colorScheme)
			{
			case WindowsColorScheme.Automatic:
			{
				string text = !WhidbeyRenderer.smethod_0() || !Class14.Boolean_0 ? "none" : Class14.String_1;
				string a;
				if ((a = text) != null)
				{
					if (a == "NormalColor")
					{
						ApplyLunaBlueColors();
						break;
					}
					if (a == "HomeStead")
					{
						ApplyLunaOliveColors();
						break;
					}
					if (a == "Metallic")
					{
						ApplyLunaSilverColors();
						break;
					}
				}
				ApplyStandardColors();
				break;
			}
			case WindowsColorScheme.Standard:
				ApplyStandardColors();
				break;
			case WindowsColorScheme.LunaBlue:
				ApplyLunaBlueColors();
				break;
			case WindowsColorScheme.LunaOlive:
				ApplyLunaOliveColors();
				break;
			case WindowsColorScheme.LunaSilver:
				ApplyLunaSilverColors();
				break;
			}
			base.GetColorsFromSystem();
		}

		private void method_1(Control control_0, Control control_1, Graphics g, Rectangle rect)
		{
			if (control_0.ClientRectangle.Width > 0 && control_0.ClientRectangle.Height > 0 && rect.Width > 0 && rect.Height > 0)
			{
				var point = control_1.PointToClient(control_0.PointToScreen(new Point(0, 0)));
				var point2 = control_1.PointToClient(control_0.PointToScreen(new Point(control_0.ClientRectangle.Right, 0)));
			    using (var brush = new LinearGradientBrush(point, point2, LayoutBackgroundColor1, LayoutBackgroundColor2))
			        g.FillRectangle(brush, rect);
			}
		}

		public override void StartRenderSession(HotkeyPrefix hotKeys)
		{
			TextFormat = TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPadding;
		    TextFormat |= hotKeys != HotkeyPrefix.None && hotKeys == HotkeyPrefix.Hide ? TextFormatFlags.HidePrefix : TextFormatFlags.NoPrefix;
		    this.int_0++;
		}

		public WindowsColorScheme ColorScheme
		{
			get
			{
				return _colorScheme;
			}
			set
			{
				_colorScheme = value;
				GetColorsFromSystem();
			}
		}

		public Color LayoutBackgroundColor1
		{
			get
			{
				return _layoutBackgroundColor1;
			}
			set
			{
				_layoutBackgroundColor1 = value;
				CustomColors = true;
			}
		}

		public Color LayoutBackgroundColor2
		{
			get
			{
				return _layoutBackgroundColor2;
			}
			set
			{
				_layoutBackgroundColor2 = value;
				CustomColors = true;
			}
		}

		protected TextFormatFlags TextFormat { get; private set; }

	    private Color _layoutBackgroundColor1;

		private Color _layoutBackgroundColor2;

		private int int_0;

	    private WindowsColorScheme _colorScheme;
	}
}
