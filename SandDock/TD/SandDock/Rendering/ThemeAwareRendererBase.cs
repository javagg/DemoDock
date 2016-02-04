using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
	public abstract class ThemeAwareRendererBase : RendererBase
	{
		protected ThemeAwareRendererBase()
		{
		}

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
			switch (this.windowsColorScheme_0)
			{
			case WindowsColorScheme.Automatic:
			{
				string text = (!WhidbeyRenderer.smethod_0() || !Class14.Boolean_0) ? "none" : Class14.String_1;
				string a;
				if ((a = text) != null)
				{
					if (a == "NormalColor")
					{
						this.ApplyLunaBlueColors();
						break;
					}
					if (a == "HomeStead")
					{
						this.ApplyLunaOliveColors();
						break;
					}
					if (a == "Metallic")
					{
						this.ApplyLunaSilverColors();
						break;
					}
				}
				this.ApplyStandardColors();
				break;
			}
			case WindowsColorScheme.Standard:
				this.ApplyStandardColors();
				break;
			case WindowsColorScheme.LunaBlue:
				this.ApplyLunaBlueColors();
				break;
			case WindowsColorScheme.LunaOlive:
				this.ApplyLunaOliveColors();
				break;
			case WindowsColorScheme.LunaSilver:
				this.ApplyLunaSilverColors();
				break;
			}
			base.GetColorsFromSystem();
		}

		private void method_1(Control control_0, Control control_1, Graphics graphics_0, Rectangle rectangle_0)
		{
			if (control_0.ClientRectangle.Width > 0 && control_0.ClientRectangle.Height > 0 && rectangle_0.Width > 0 && rectangle_0.Height > 0)
			{
				Color layoutBackgroundColor = this.LayoutBackgroundColor1;
				Color layoutBackgroundColor2 = this.LayoutBackgroundColor2;
				Point point = control_1.PointToClient(control_0.PointToScreen(new Point(0, 0)));
				Point point2 = control_1.PointToClient(control_0.PointToScreen(new Point(control_0.ClientRectangle.Right, 0)));
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(point, point2, layoutBackgroundColor, layoutBackgroundColor2))
				{
					graphics_0.FillRectangle(linearGradientBrush, rectangle_0);
				}
				return;
			}
		}

		public override void StartRenderSession(HotkeyPrefix hotKeys)
		{
			this.textFormatFlags_0 = (TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPadding);
			if (hotKeys != HotkeyPrefix.None)
			{
				if (hotKeys == HotkeyPrefix.Hide)
				{
					this.textFormatFlags_0 |= TextFormatFlags.HidePrefix;
				}
			}
			else
			{
				this.textFormatFlags_0 |= TextFormatFlags.NoPrefix;
			}
			this.int_0++;
		}

		public WindowsColorScheme ColorScheme
		{
			get
			{
				return this.windowsColorScheme_0;
			}
			set
			{
				this.windowsColorScheme_0 = value;
				this.GetColorsFromSystem();
			}
		}

		public Color LayoutBackgroundColor1
		{
			get
			{
				return this.color_0;
			}
			set
			{
				this.color_0 = value;
				base.CustomColors = true;
			}
		}

		public Color LayoutBackgroundColor2
		{
			get
			{
				return this.color_1;
			}
			set
			{
				this.color_1 = value;
				base.CustomColors = true;
			}
		}

		protected TextFormatFlags TextFormat
		{
			get
			{
				return this.textFormatFlags_0;
			}
		}

		private Color color_0;

		private Color color_1;

		private int int_0;

		private TextFormatFlags textFormatFlags_0;

		private WindowsColorScheme windowsColorScheme_0;
	}
}
