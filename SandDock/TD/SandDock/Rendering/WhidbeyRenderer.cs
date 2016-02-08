using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
	public class WhidbeyRenderer : ThemeAwareRendererBase
	{
		public WhidbeyRenderer()
		{
		}

		public WhidbeyRenderer(WindowsColorScheme colorScheme)
		{
			base.ColorScheme = colorScheme;
		}

		protected override void ApplyLunaBlueColors()
		{
			base.LayoutBackgroundColor1 = Color.FromArgb(229, 229, 215);
			base.LayoutBackgroundColor2 = Color.FromArgb(243, 242, 231);
			this.color_8 = Color.FromArgb(228, 226, 213);
			this.color_9 = this.color_8;
			this.ActiveDocumentBorderColor = Color.FromArgb(127, 157, 185);
			this.InactiveDocumentBorderColor = SystemColors.ControlDark;
			this.color_12 = Color.FromArgb(59, 128, 237);
			this.color_13 = Color.FromArgb(49, 106, 197);
			this.color_14 = Color.White;
			this.color_10 = Color.FromArgb(204, 199, 186);
			this.color_11 = Color.Black;
			this.color_15 = SystemColors.Control;
			this.color_16 = Color.FromArgb(140, 134, 123);
			this.color_17 = Color.FromArgb(156, 182, 231);
			this.color_18 = Color.FromArgb(60, 90, 170);
			this.color_19 = Color.FromArgb(120, 150, 210);
			this.color_20 = Color.FromArgb(60, 90, 170);
		}

		protected override void ApplyLunaOliveColors()
		{
			base.LayoutBackgroundColor1 = Color.FromArgb(229, 229, 215);
			base.LayoutBackgroundColor2 = Color.FromArgb(243, 242, 231);
			this.color_8 = Color.FromArgb(228, 226, 213);
			this.color_9 = this.color_8;
			this.ActiveDocumentBorderColor = Color.FromArgb(127, 157, 185);
			this.InactiveDocumentBorderColor = SystemColors.ControlDark;
			this.color_12 = Color.FromArgb(182, 195, 146);
			this.color_13 = Color.FromArgb(145, 160, 117);
			this.color_14 = Color.White;
			this.color_10 = Color.FromArgb(204, 199, 186);
			this.color_11 = Color.Black;
			this.color_15 = SystemColors.Control;
			this.color_16 = Color.FromArgb(140, 134, 123);
			this.color_17 = Color.FromArgb(181, 199, 140);
			this.color_18 = Color.FromArgb(118, 128, 95);
			this.color_19 = Color.FromArgb(148, 162, 115);
			this.color_20 = Color.FromArgb(118, 128, 95);
		}

		protected override void ApplyLunaSilverColors()
		{
			base.LayoutBackgroundColor1 = Color.FromArgb(215, 215, 229);
			base.LayoutBackgroundColor2 = Color.FromArgb(243, 243, 247);
			this.color_8 = Color.FromArgb(238, 238, 238);
			this.color_9 = this.color_8;
			this.ActiveDocumentBorderColor = Color.FromArgb(127, 157, 185);
			this.InactiveDocumentBorderColor = SystemColors.ControlDark;
			this.color_12 = Color.FromArgb(211, 212, 221);
			this.color_13 = Color.FromArgb(166, 165, 191);
			this.color_14 = Color.Black;
			this.color_10 = Color.FromArgb(240, 240, 245);
			this.color_11 = Color.Black;
			this.color_15 = Color.FromArgb(214, 215, 222);
			this.color_16 = Color.FromArgb(123, 125, 148);
			this.color_17 = Color.FromArgb(255, 227, 173);
			this.color_18 = Color.FromArgb(74, 73, 107);
			this.color_19 = Color.FromArgb(255, 182, 115);
			this.color_20 = Color.FromArgb(74, 73, 107);
		}

		protected override void ApplyStandardColors()
		{
			if (SystemInformation.HighContrast)
			{
				base.LayoutBackgroundColor1 = SystemColors.Control;
				base.LayoutBackgroundColor2 = SystemColors.Control;
				this.ActiveDocumentBorderColor = SystemColors.ActiveCaption;
				this.InactiveDocumentBorderColor = SystemColors.ControlDark;
			}
			else
			{
				base.LayoutBackgroundColor1 = SystemColors.Control;
				base.LayoutBackgroundColor2 = RendererBase.InterpolateColors(SystemColors.Control, SystemColors.Window, 0.8f);
				this.ActiveDocumentBorderColor = SystemColors.AppWorkspace;
				this.InactiveDocumentBorderColor = SystemColors.ControlDark;
			}
			this.color_8 = SystemColors.Control;
			this.color_9 = this.color_8;
			this.color_12 = SystemColors.ActiveCaption;
			this.color_13 = SystemColors.ActiveCaption;
			this.color_14 = SystemColors.ActiveCaptionText;
			this.color_10 = SystemColors.InactiveCaption;
			this.color_11 = SystemColors.InactiveCaptionText;
			this.color_15 = Color.Transparent;
			this.color_16 = SystemColors.ControlLightLight;
			this.color_17 = Color.Transparent;
			this.color_18 = SystemColors.ControlLightLight;
			this.color_19 = (SystemInformation.HighContrast ? Color.Transparent : Color.FromArgb(100, SystemColors.Control));
			this.color_20 = SystemColors.ControlLightLight;
		}

		protected internal override void DrawCollapsedTab(Graphics graphics, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool vertical)
		{
			if (dockSide != DockSide.Left && dockSide != DockSide.Right)
			{
				Class16.smethod_5(graphics, bounds, dockSide, image, text, font, SystemBrushes.ControlDarkDark, SystemColors.ControlDark, this.TabTextDisplay == TabTextDisplayMode.AllTabs);
			}
			else
			{
				using (Image image2 = new Bitmap(image))
				{
					image2.RotateFlip(RotateFlipType.Rotate90FlipNone);
					Class16.smethod_5(graphics, bounds, dockSide, image2, text, font, SystemBrushes.ControlDarkDark, SystemColors.ControlDark, this.TabTextDisplay == TabTextDisplayMode.AllTabs);
				}
			}
		}

		protected internal override void DrawControlClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
		}

		protected internal override void DrawDockContainerBackground(Graphics graphics, DockContainer container, Rectangle bounds)
		{
			Class16.smethod_0(graphics, container.BackColor);
		}

		protected internal override void DrawDocumentClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
		    using (var brush = new SolidBrush(backColor))
		        graphics.FillRectangle(brush, bounds);
		}

		protected internal override void DrawDocumentStripBackground(Graphics graphics, Rectangle bounds)
		{
			if (bounds.Width > 0 && bounds.Height > 0)
			{
			    using (var brush = new LinearGradientBrush(new Point(bounds.X, bounds.Y - 1), new Point(bounds.X, bounds.Bottom), this.color_8, this.color_9))
			        graphics.FillRectangle(brush, bounds);
			    using (var pen = new Pen(this.color_2))
			        graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
			}
		}

		protected internal override void DrawDocumentStripButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
		{
			this.vmethod_0(graphics, bounds, state, true);
			switch (buttonType)
			{
			case SandDockButtonType.Close:
				Class15.smethod_6(graphics, bounds, SystemPens.ControlText);
				return;
			case SandDockButtonType.Pin:
			case SandDockButtonType.WindowPosition:
				break;
			case SandDockButtonType.ScrollLeft:
				Class15.smethod_1(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
				return;
			case SandDockButtonType.ScrollRight:
				Class15.smethod_2(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
				return;
			case SandDockButtonType.ActiveFiles:
				bounds.Inflate(1, 1);
				bounds.X--;
				Class15.smethod_0(graphics, bounds, SystemPens.ControlText);
				break;
			default:
				return;
			}
		}

		protected internal override void DrawDocumentStripTab(Graphics graphics, Rectangle bounds, Rectangle contentBounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			bool bool_ = (state & DrawItemState.Checked) == DrawItemState.Checked;
			if ((state & DrawItemState.Selected) != DrawItemState.Selected)
			{
				Class16.smethod_1(graphics, bounds, contentBounds, image, this.ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemInformation.HighContrast ? SystemColors.Control : backColor, SystemBrushes.ControlText, this.InactiveDocumentBorderColor, this.color_5, this.color_7, false, this.DocumentTabSize, this.DocumentTabExtra, base.TextFormat, bool_);
				return;
			}
			Class16.smethod_1(graphics, bounds, contentBounds, image, this.ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemBrushes.ControlText, this.ActiveDocumentBorderColor, this.color_4, this.color_6, true, this.DocumentTabSize, this.DocumentTabExtra, base.TextFormat, bool_);
		}

		protected internal override void DrawTabStripBackground(Control container, Control control, Graphics graphics, Rectangle bounds, int selectedTabOffset)
		{
			base.DrawTabStripBackground(container, control, graphics, bounds, selectedTabOffset);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Top + 2, bounds.Right - 1, bounds.Top + 2);
			if (!SystemInformation.HighContrast)
			{
				using (Pen pen = new Pen(SystemColors.ControlLightLight))
				{
					graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
					graphics.DrawLine(pen, bounds.Left, bounds.Top + 1, bounds.Right - 1, bounds.Top + 1);
				}
			}
		}

		protected internal override void DrawTabStripTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			bounds.Y += 2;
			bounds.Height -= 2;
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				Class16.smethod_3(graphics, bounds, image, this.ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemColors.ControlText, SystemColors.ControlDark, state, base.TextFormat);
			}
			else
			{
				Class16.smethod_3(graphics, bounds, image, this.ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : backColor, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, SystemColors.ControlDarkDark, SystemColors.ControlDark, state, base.TextFormat);
			}
			if ((state & DrawItemState.Selected) != DrawItemState.Selected && drawSeparator)
			{
				graphics.DrawLine(SystemPens.ControlDark, bounds.Right - 2, bounds.Top + 3, bounds.Right - 2, bounds.Bottom - 4);
			}
		}

		protected internal override void DrawTitleBarBackground(Graphics graphics, Rectangle bounds, bool focused)
		{
			if (focused)
			{
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(bounds.X, bounds.Y - 1), new Point(bounds.X, bounds.Bottom), this.color_12, this.color_13))
				{
					graphics.FillRectangle(linearGradientBrush, bounds);
					goto IL_71;
				}
			}
			using (SolidBrush solidBrush = new SolidBrush(this.color_10))
			{
				graphics.FillRectangle(solidBrush, bounds);
			}
			IL_71:
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
		}

		protected internal override void DrawTitleBarButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state, bool focused, bool toggled)
		{
			this.vmethod_0(graphics, bounds, state, focused);
			using (Pen pen = (!focused) ? new Pen(this.color_11) : new Pen(this.color_14))
			{
				switch (buttonType)
				{
				case SandDockButtonType.Close:
					Class15.smethod_6(graphics, bounds, pen);
					break;
				case SandDockButtonType.Pin:
					Class15.smethod_4(graphics, bounds, pen, toggled);
					break;
				case SandDockButtonType.WindowPosition:
					Class15.smethod_0(graphics, bounds, pen);
					break;
				}
			}
		}

		protected internal override void DrawTitleBarText(Graphics graphics, Rectangle bounds, bool focused, string text, Font font)
		{
			bounds.Inflate(-3, 0);
			TextFormatFlags textFormatFlags = base.TextFormat;
			textFormatFlags |= TextFormatFlags.NoPrefix;
			bounds.X += 3;
			TextRenderer.DrawText(graphics, text, font, bounds, focused ? this.color_14 : this.color_11, textFormatFlags);
		}

		protected override void GetColorsFromSystem()
		{
			base.GetColorsFromSystem();
			if (!SystemInformation.HighContrast)
			{
				this.color_4 = SystemColors.ControlLightLight;
				this.color_5 = SystemColors.ControlLightLight;
				this.color_6 = SystemColors.ControlLightLight;
				this.color_7 = SystemColors.Control;
				return;
			}
			this.color_4 = SystemColors.Control;
			this.color_5 = SystemColors.Control;
			this.color_6 = SystemColors.Control;
			this.color_7 = SystemColors.Control;
		}

		protected internal override Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			TextFormatFlags textFormatFlags = base.TextFormat;
			textFormatFlags &= ~TextFormatFlags.NoPrefix;
			int num;
			using (Font font2 = new Font(font, FontStyle.Bold))
			{
				num = TextRenderer.MeasureText(graphics, text, font2, new Size(2147483647, 2147483647), textFormatFlags).Width;
			}
			num += 14;
			if (image != null)
			{
				num += 20;
			}
			num += this.DocumentTabExtra;
			return new Size(num, 0);
		}

		protected internal override Size MeasureTabStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			return Class16.smethod_2(graphics, image, this.ImageSize, text, font, base.TextFormat);
		}

		private void method_2()
		{
			this.boxModel_0 = null;
			this.boxModel_1 = null;
			this.boxModel_2 = null;
		}

		internal static bool smethod_0()
		{
			bool result = false;
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				result = (Environment.OSVersion.Version >= new Version(5, 1, 0, 0));
			}
			return result;
		}

		public override void StartRenderSession(HotkeyPrefix hotKeys)
		{
			base.StartRenderSession(hotKeys);
		}

		public override string ToString()
		{
			return "Whidbey";
		}

		internal virtual void vmethod_0(Graphics graphics_0, Rectangle rectangle_0, DrawItemState drawItemState_0, bool bool_1)
		{
			if ((drawItemState_0 & DrawItemState.HotLight) == DrawItemState.HotLight)
			{
				Color color;
				Color color2;
				Color color3;
				if (!bool_1)
				{
					color = this.color_16;
					color2 = this.color_16;
					color3 = this.color_15;
				}
				else if ((drawItemState_0 & DrawItemState.Selected) != DrawItemState.Selected)
				{
					color = this.color_18;
					color2 = this.color_18;
					color3 = this.color_17;
				}
				else
				{
					color = this.color_20;
					color2 = this.color_20;
					color3 = this.color_19;
				}
				using (SolidBrush solidBrush = new SolidBrush(color3))
				{
					graphics_0.FillRectangle(solidBrush, rectangle_0);
				}
				using (Pen pen = new Pen(color))
				{
					graphics_0.DrawLine(pen, rectangle_0.Left, rectangle_0.Top, rectangle_0.Right - 1, rectangle_0.Top);
					graphics_0.DrawLine(pen, rectangle_0.Left, rectangle_0.Top, rectangle_0.Left, rectangle_0.Bottom - 1);
				}
				using (Pen pen2 = new Pen(color2))
				{
					graphics_0.DrawLine(pen2, rectangle_0.Right - 1, rectangle_0.Bottom - 1, rectangle_0.Right - 1, rectangle_0.Top);
					graphics_0.DrawLine(pen2, rectangle_0.Right - 1, rectangle_0.Bottom - 1, rectangle_0.Left, rectangle_0.Bottom - 1);
				}
			}
		}

		public Color ActiveButtonBackgroundColor
		{
			get
			{
				return this.color_17;
			}
			set
			{
				this.color_17 = value;
				base.CustomColors = true;
			}
		}

		public Color ActiveButtonBorderColor
		{
			get
			{
				return this.color_18;
			}
			set
			{
				this.color_18 = value;
				base.CustomColors = true;
			}
		}

		public Color ActiveDocumentBorderColor
		{
			get
			{
				return this.color_2;
			}
			set
			{
				this.color_2 = value;
				base.CustomColors = true;
			}
		}

		public Color ActiveDocumentHighlightColor
		{
			get
			{
				return this.color_4;
			}
			set
			{
				this.color_4 = value;
				base.CustomColors = true;
			}
		}

		public Color ActiveDocumentShadowColor
		{
			get
			{
				return this.color_6;
			}
			set
			{
				this.color_6 = value;
				base.CustomColors = true;
			}
		}

		public Color ActiveHotButtonBackgroundColor
		{
			get
			{
				return this.color_19;
			}
			set
			{
				this.color_19 = value;
				base.CustomColors = true;
			}
		}

		public Color ActiveHotButtonBorderColor
		{
			get
			{
				return this.color_20;
			}
			set
			{
				this.color_20 = value;
				base.CustomColors = true;
			}
		}

		public Color ActiveTitleBarBackgroundColor1
		{
			get
			{
				return this.color_12;
			}
			set
			{
				this.color_12 = value;
				base.CustomColors = true;
			}
		}

		public Color ActiveTitleBarBackgroundColor2
		{
			get
			{
				return this.color_13;
			}
			set
			{
				this.color_13 = value;
				base.CustomColors = true;
			}
		}

		public Color ActiveTitleBarForegroundColor
		{
			get
			{
				return this.color_14;
			}
			set
			{
				this.color_14 = value;
				base.CustomColors = true;
			}
		}

		public Color DocumentStripBackgroundColor1
		{
			get
			{
				return this.color_8;
			}
			set
			{
				this.color_8 = value;
				base.CustomColors = true;
			}
		}

		public Color DocumentStripBackgroundColor2
		{
			get
			{
				return this.color_9;
			}
			set
			{
				this.color_9 = value;
				base.CustomColors = true;
			}
		}

		protected internal override int DocumentTabExtra
		{
			get
			{
				return this.ImageSize.Width - 4;
			}
		}

		protected internal override int DocumentTabSize
		{
			get
			{
				int num = Math.Max(Control.DefaultFont.Height, this.ImageSize.Height);
				return num + 4;
			}
		}

		protected internal override int DocumentTabStripSize
		{
			get
			{
				int num = Math.Max(Control.DefaultFont.Height, this.ImageSize.Height);
				return num + 5;
			}
		}

		public override Size ImageSize
		{
			get
			{
				return base.ImageSize;
			}
			set
			{
				this.method_2();
				base.ImageSize = value;
			}
		}

		public Color InactiveButtonBackgroundColor
		{
			get
			{
				return this.color_15;
			}
			set
			{
				this.color_15 = value;
				base.CustomColors = true;
			}
		}

		public Color InactiveButtonBorderColor
		{
			get
			{
				return this.color_16;
			}
			set
			{
				this.color_16 = value;
				base.CustomColors = true;
			}
		}

		public Color InactiveDocumentBorderColor
		{
			get
			{
				return this.color_3;
			}
			set
			{
				this.color_3 = value;
				base.CustomColors = true;
			}
		}

		public Color InactiveDocumentHighlightColor
		{
			get
			{
				return this.color_5;
			}
			set
			{
				this.color_5 = value;
				base.CustomColors = true;
			}
		}

		public Color InactiveDocumentShadowColor
		{
			get
			{
				return this.color_7;
			}
			set
			{
				this.color_7 = value;
				base.CustomColors = true;
			}
		}

		public Color InactiveTitleBarBackgroundColor
		{
			get
			{
				return this.color_10;
			}
			set
			{
				this.color_10 = value;
				base.CustomColors = true;
			}
		}

		public Color InactiveTitleBarForegroundColor
		{
			get
			{
				return this.color_11;
			}
			set
			{
				this.color_11 = value;
				base.CustomColors = true;
			}
		}

		public override Size TabControlPadding
		{
			get
			{
				return new Size(3, 3);
			}
		}

		protected internal override BoxModel TabMetrics
		{
			get
			{
				if (this.boxModel_1 == null)
				{
					this.boxModel_1 = new BoxModel(0, 0, 0, 0, 0, 0, 0, 0, -1, 0);
				}
				return this.boxModel_1;
			}
		}

		protected internal override BoxModel TabStripMetrics
		{
			get
			{
				if (this.boxModel_0 == null)
				{
					int height = Control.DefaultFont.Height;
					int num = Math.Max(height, this.ImageSize.Height);
					this.boxModel_0 = new BoxModel(0, num + 8, 0, 0, 0, 1, 0, 0, 0, 0);
				}
				return this.boxModel_0;
			}
		}

		protected internal override TabTextDisplayMode TabTextDisplay
		{
			get
			{
				return TabTextDisplayMode.AllTabs;
			}
		}

		protected internal override BoxModel TitleBarMetrics
		{
			get
			{
				if (this.boxModel_2 == null)
				{
					this.boxModel_2 = new BoxModel(0, SystemInformation.ToolWindowCaptionHeight, 0, 0, 0, 0, 0, 0, 0, 0);
				}
				return this.boxModel_2;
			}
		}

		private BoxModel boxModel_0;

		private BoxModel boxModel_1;

		private BoxModel boxModel_2;

		private Color color_10;

		private Color color_11;

		private Color color_12;

		private Color color_13;

		private Color color_14;

		private Color color_15;

		private Color color_16;

		private Color color_17;

		private Color color_18;

		private Color color_19;

		private Color color_2;

		private Color color_20;

		private Color color_3;

		private Color color_4;

		private Color color_5;

		private Color color_6;

		private Color color_7;

		private Color color_8;

		private Color color_9;
	}
}
