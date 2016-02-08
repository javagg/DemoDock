using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
	public class Office2003Renderer : ThemeAwareRendererBase
	{
		public Office2003Renderer()
		{
		}

		public Office2003Renderer(WindowsColorScheme colorScheme)
		{
			base.ColorScheme = colorScheme;
		}

		protected internal override Rectangle AdjustDockControlClientBounds(ControlLayoutSystem layoutSystem, DockControl control, Rectangle clientBounds)
		{
			if (layoutSystem is DocumentLayoutSystem)
			{
				clientBounds.Inflate(-4, -4);
				return clientBounds;
			}
			return base.AdjustDockControlClientBounds(layoutSystem, control, clientBounds);
		}

		protected override void ApplyLunaBlueColors()
		{
			base.LayoutBackgroundColor1 = Color.FromArgb(158, 190, 245);
			base.LayoutBackgroundColor2 = Color.FromArgb(195, 218, 249);
			this.color_2 = Color.FromArgb(0, 0, 128);
			this.color_3 = Color.FromArgb(255, 244, 204);
			this.color_4 = Color.FromArgb(255, 211, 142);
			this.color_16 = Color.FromArgb(221, 236, 254);
			this.color_17 = Color.FromArgb(129, 169, 226);
			this.color_14 = Color.FromArgb(255, 211, 142);
			this.color_15 = Color.FromArgb(254, 145, 78);
			this.color_18 = Color.FromArgb(39, 65, 118);
			this.color_5 = Color.FromArgb(196, 218, 250);
			this.color_6 = SystemColors.ControlLightLight;
			this.color_7 = Color.FromArgb(59, 97, 156);
			this.color_8 = Color.FromArgb(0, 53, 154);
			this.color_9 = SystemColors.ControlLightLight;
			this.color_10 = SystemColors.ControlLightLight;
			this.color_11 = SystemColors.ControlLightLight;
			this.color_12 = Color.FromArgb(117, 166, 241);
			this.color_13 = SystemColors.ControlText;
		}

		protected override void ApplyLunaOliveColors()
		{
			base.LayoutBackgroundColor1 = Color.FromArgb(217, 217, 167);
			base.LayoutBackgroundColor2 = Color.FromArgb(242, 240, 228);
			this.color_2 = Color.FromArgb(63, 93, 56);
			this.color_3 = Color.FromArgb(255, 244, 204);
			this.color_4 = Color.FromArgb(255, 211, 142);
			this.color_16 = Color.FromArgb(244, 247, 222);
			this.color_17 = Color.FromArgb(183, 198, 145);
			this.color_14 = Color.FromArgb(255, 211, 142);
			this.color_15 = Color.FromArgb(254, 145, 78);
			this.color_18 = Color.FromArgb(81, 94, 51);
			this.color_5 = Color.FromArgb(242, 241, 228);
			this.color_6 = SystemColors.ControlLightLight;
			this.color_7 = Color.FromArgb(96, 128, 88);
			this.color_8 = Color.FromArgb(96, 119, 107);
			this.color_9 = SystemColors.ControlLightLight;
			this.color_10 = SystemColors.ControlLightLight;
			this.color_11 = SystemColors.ControlLightLight;
			this.color_12 = Color.FromArgb(176, 194, 140);
			this.color_13 = SystemColors.ControlText;
		}

		protected override void ApplyLunaSilverColors()
		{
			base.LayoutBackgroundColor1 = Color.FromArgb(215, 215, 229);
			base.LayoutBackgroundColor2 = Color.FromArgb(243, 243, 247);
			this.color_2 = Color.FromArgb(75, 75, 111);
			this.color_3 = Color.FromArgb(255, 244, 204);
			this.color_4 = Color.FromArgb(255, 211, 142);
			this.color_16 = Color.FromArgb(243, 244, 250);
			this.color_17 = Color.FromArgb(140, 138, 172);
			this.color_14 = Color.FromArgb(255, 211, 142);
			this.color_15 = Color.FromArgb(254, 145, 78);
			this.color_18 = Color.FromArgb(84, 84, 117);
			this.color_5 = Color.FromArgb(243, 243, 247);
			this.color_6 = SystemColors.ControlLightLight;
			this.color_7 = Color.FromArgb(124, 124, 148);
			this.color_8 = Color.FromArgb(118, 116, 146);
			this.color_9 = SystemColors.ControlLightLight;
			this.color_10 = SystemColors.ControlLightLight;
			this.color_11 = SystemColors.ControlLightLight;
			this.color_12 = Color.FromArgb(186, 185, 206);
			this.color_13 = SystemColors.ControlText;
		}

		protected override void ApplyStandardColors()
		{
			if (SystemInformation.HighContrast)
			{
				base.LayoutBackgroundColor1 = SystemColors.Control;
				base.LayoutBackgroundColor2 = SystemColors.Control;
				this.color_7 = SystemColors.ActiveCaption;
				this.color_8 = SystemColors.ControlDark;
				this.color_9 = SystemColors.Control;
				this.color_10 = SystemColors.Control;
				this.color_11 = SystemColors.Control;
				this.color_12 = SystemColors.Control;
			}
			else
			{
				base.LayoutBackgroundColor1 = SystemColors.Control;
				base.LayoutBackgroundColor2 = RendererBase.InterpolateColors(SystemColors.Control, SystemColors.Window, 0.8f);
				this.color_7 = SystemColors.ControlDark;
				this.color_8 = SystemColors.ControlDark;
				this.color_9 = SystemColors.ControlLightLight;
				this.color_10 = SystemColors.Control;
				this.color_11 = SystemColors.ControlLightLight;
				this.color_12 = SystemColors.Control;
			}
			this.color_2 = SystemColors.Highlight;
			this.color_3 = RendererBase.InterpolateColors(this.color_2, SystemColors.Window, 0.7f);
			this.color_4 = this.color_3;
			this.color_16 = base.LayoutBackgroundColor2;
			this.color_17 = RendererBase.InterpolateColors(SystemColors.Control, Color.Black, 0.03f);
			this.color_14 = Color.FromArgb(212, 213, 216);
			this.color_15 = Color.FromArgb(212, 213, 216);
			this.color_18 = SystemColors.ControlDark;
			this.color_5 = SystemColors.Control;
			this.color_6 = SystemColors.ControlLightLight;
			this.color_13 = SystemColors.ControlText;
		}

		protected internal override void DrawCollapsedTab(Graphics graphics, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool vertical)
		{
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				Class16.smethod_5(graphics, bounds, dockSide, image, text, font, SystemBrushes.ControlText, SystemColors.ControlDarkDark, this.TabTextDisplay == TabTextDisplayMode.AllTabs);
				return;
			}
			Class16.smethod_5(graphics, bounds, dockSide, image, text, font, SystemBrushes.ControlText, SystemColors.ControlDarkDark, this.TabTextDisplay == TabTextDisplayMode.AllTabs);
		}

		protected internal override void DrawControlClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
		}

		protected internal override void DrawDockContainerBackground(Graphics graphics, DockContainer container, Rectangle bounds)
		{
			Class16.smethod_0(graphics, container.BackColor);
		}

		protected internal override void DrawDocumentClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
			using (SolidBrush solidBrush = new SolidBrush(backColor))
			{
				graphics.FillRectangle(solidBrush, bounds);
			}
		}

		protected internal override void DrawDocumentStripBackground(Graphics graphics, Rectangle bounds)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(bounds.X, bounds.Y - 1), new Point(bounds.X, bounds.Bottom), this.color_5, this.color_6))
			{
				graphics.FillRectangle(linearGradientBrush, bounds);
			}
			using (Pen pen = new Pen(this.color_7))
			{
				graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
			}
		}

		protected internal override void DrawDocumentStripButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
		{
			this.method_2(graphics, bounds, state);
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				bounds.Offset(1, 1);
			}
			switch (buttonType)
			{
			case SandDockButtonType.Close:
				using (Pen pen = new Pen(this.color_13))
				{
					Class15.smethod_5(graphics, bounds, pen);
					return;
				}
				break;
			case SandDockButtonType.Pin:
			case SandDockButtonType.WindowPosition:
				return;
			case SandDockButtonType.ScrollLeft:
				break;
			case SandDockButtonType.ScrollRight:
				Class15.smethod_2(graphics, bounds, this.color_13, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
				return;
			case SandDockButtonType.ActiveFiles:
				using (Pen pen2 = new Pen(this.color_13))
				{
					Class15.smethod_0(graphics, bounds, pen2);
				}
				return;
			default:
				return;
			}
			Class15.smethod_1(graphics, bounds, this.color_13, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
		}

		protected internal override void DrawDocumentStripTab(Graphics graphics, Rectangle bounds, Rectangle contentBounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			Color color_ = RendererBase.InterpolateColors(backColor, SystemColors.ControlLightLight, 0.78f);
			bool bool_ = (state & DrawItemState.Checked) == DrawItemState.Checked;
			if ((state & DrawItemState.Selected) != DrawItemState.Selected)
			{
				Class16.smethod_1(graphics, bounds, contentBounds, image, this.ImageSize, text, font, color_, backColor, SystemBrushes.ControlText, this.color_8, this.color_10, this.color_12, false, this.DocumentTabSize, this.DocumentTabExtra, base.TextFormat, bool_);
				return;
			}
			Class16.smethod_1(graphics, bounds, contentBounds, image, this.ImageSize, text, font, color_, backColor, SystemBrushes.ControlText, this.color_7, this.color_9, this.color_11, true, this.DocumentTabSize, this.DocumentTabExtra, base.TextFormat, bool_);
		}

		protected internal override void DrawTabStripTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				Class16.smethod_3(graphics, bounds, image, this.size_1, text, font, this.color_3, this.color_4, SystemColors.ControlText, SystemColors.ControlDark, state, base.TextFormat);
				return;
			}
			Class16.smethod_3(graphics, bounds, image, this.size_1, text, font, backColor, SystemColors.ControlLightLight, SystemColors.ControlText, SystemColors.ControlDark, state, base.TextFormat);
		}

		protected internal override void DrawTitleBarBackground(Graphics graphics, Rectangle bounds, bool focused)
		{
			if (focused)
			{
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, this.color_14, this.color_15, LinearGradientMode.Vertical))
				{
					graphics.FillRectangle(linearGradientBrush, bounds);
					goto IL_54;
				}
			}
			using (Brush brush = this.method_3(bounds, LinearGradientMode.Vertical, this.color_16, this.color_17))
			{
				graphics.FillRectangle(brush, bounds);
			}
			IL_54:
			bounds.Inflate(0, -2);
			using (SolidBrush solidBrush = new SolidBrush(this.color_18))
			{
				int num = (bounds.Height - 2) / 4;
				int num2 = num * 4 - 2;
				int num3 = bounds.X + 3;
				int num4 = bounds.Y + bounds.Height / 2 - num2 / 2;
				for (int i = num4; i <= num4 + num2; i += 4)
				{
					graphics.FillRectangle(SystemBrushes.ControlLightLight, new Rectangle(num3 + 1, i + 1, 2, 2));
					graphics.FillRectangle(solidBrush, new Rectangle(num3, i, 2, 2));
				}
			}
		}

		protected internal override void DrawTitleBarButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state, bool focused, bool toggled)
		{
			bounds.Width--;
			bounds.Height--;
			this.method_2(graphics, bounds, state);
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				bounds.Offset(1, 1);
			}
			switch (buttonType)
			{
			case SandDockButtonType.Close:
				Class15.smethod_6(graphics, bounds, focused ? SystemPens.ControlText : SystemPens.ControlText);
				return;
			case SandDockButtonType.Pin:
				Class15.smethod_4(graphics, bounds, focused ? SystemPens.ControlText : SystemPens.ControlText, toggled);
				return;
			case SandDockButtonType.ScrollLeft:
			case SandDockButtonType.ScrollRight:
				break;
			case SandDockButtonType.WindowPosition:
				Class15.smethod_0(graphics, bounds, focused ? SystemPens.ControlText : SystemPens.ControlText);
				break;
			default:
				return;
			}
		}

		protected internal override void DrawTitleBarText(Graphics graphics, Rectangle bounds, bool focused, string text, Font font)
		{
			bounds.Inflate(-3, 0);
			using (Font font2 = new Font(font, FontStyle.Bold))
			{
				TextFormatFlags textFormatFlags = base.TextFormat;
				textFormatFlags |= TextFormatFlags.NoPrefix;
				bounds.X += 3;
				TextRenderer.DrawText(graphics, text, font2, bounds, SystemColors.ControlText, textFormatFlags);
			}
		}

		protected internal override Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			TextFormatFlags textFormatFlags = base.TextFormat;
			textFormatFlags &= ~TextFormatFlags.NoPrefix;
			int num;
			if ((state & DrawItemState.Focus) != DrawItemState.Focus)
			{
				num = TextRenderer.MeasureText(graphics, text, font, new Size(2147483647, 2147483647), textFormatFlags).Width;
			}
			else
			{
				using (Font font2 = new Font(font, FontStyle.Bold))
				{
					num = TextRenderer.MeasureText(graphics, text, font2, new Size(2147483647, 2147483647), textFormatFlags).Width;
				}
			}
			num += 24;
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

		private void method_2(Graphics graphics_0, Rectangle rectangle_0, DrawItemState drawItemState_0)
		{
			if ((drawItemState_0 & DrawItemState.HotLight) == DrawItemState.HotLight)
			{
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle_0, this.color_3, this.color_4, LinearGradientMode.Vertical))
				{
					graphics_0.FillRectangle(linearGradientBrush, rectangle_0);
				}
				using (Pen pen = new Pen(this.color_2))
				{
					graphics_0.DrawRectangle(pen, rectangle_0);
				}
			}
		}

		private Brush method_3(Rectangle rectangle_0, LinearGradientMode linearGradientMode_0, Color color_19, Color color_20)
		{
			Color color = InterpolateColors(color_19, color_20, 0.25f);
			return new LinearGradientBrush(rectangle_0, color_19, color_20, linearGradientMode_0)
			{
				InterpolationColors = new ColorBlend(3)
				{
					Colors = new Color[]
					{
						color_19,
						color,
						color_20
					},
					Positions = new float[]
					{
						0f,
						0.5f,
						1f
					}
				}
			};
		}

		public override string ToString() => "Office 2003";

	    public Color ActiveDocumentBorderColor
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

		public Color ActiveDocumentHighlightColor
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

		public Color ActiveDocumentShadowColor
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

		public Color ActiveTitleBarColor1
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

		public Color ActiveTitleBarColor2
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

		public Color DocumentStripBackgroundColor1
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

		public Color DocumentStripBackgroundColor2
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

		protected internal override int DocumentTabExtra => 18;

	    protected internal override int DocumentTabSize => Control.DefaultFont.Height + 7;

	    protected internal override int DocumentTabStripSize => Control.DefaultFont.Height + 15;

	    public Color GripperColor
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

		public Color HighlightBackgroundColor1
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

		public Color HighlightBackgroundColor2
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

		public Color HighlightBorderColor
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

		public Color InactiveDocumentBorderColor
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

		public Color InactiveDocumentHighlightColor
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

		public Color InactiveDocumentShadowColor
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

		public Color InactiveTitleBarColor1
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

		public Color InactiveTitleBarColor2
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
					this.boxModel_0 = new BoxModel(0, Control.DefaultFont.Height + 10, 0, 0, 0, 1, 0, 0, 0, 0);
				}
				return this.boxModel_0;
			}
		}

		protected internal override TabTextDisplayMode TabTextDisplay
		{
			get
			{
				return TabTextDisplayMode.SelectedTab;
			}
		}

		protected internal override BoxModel TitleBarMetrics
		{
			get
			{
				return new BoxModel(0, 25, 4, 0, 0, 0, 0, 0, 0, 0);
			}
		}

		public Color WidgetColor
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

		private BoxModel boxModel_0;

		private BoxModel boxModel_1;

		private Color color_10;

		private Color color_11;

		private Color color_12;

		private Color color_13;

		private Color color_14;

		private Color color_15;

		private Color color_16;

		private Color color_17;

		private Color color_18;

		private Color color_2;

		private Color color_3;

		private Color color_4;

		private Color color_5;

		private Color color_6;

		private Color color_7;

		private Color color_8;

		private Color color_9;

		private Size size_1 = new Size(16, 16);
	}
}
