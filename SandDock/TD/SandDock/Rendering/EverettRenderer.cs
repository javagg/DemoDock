using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
	public class EverettRenderer : RendererBase
	{
		public EverettRenderer()
		{
		}

		protected internal override Rectangle AdjustDockControlClientBounds(ControlLayoutSystem layoutSystem, DockControl control, Rectangle clientBounds)
		{
			if (layoutSystem is DocumentLayoutSystem)
			{
				clientBounds.Inflate(-2, -2);
				return clientBounds;
			}
			return base.AdjustDockControlClientBounds(layoutSystem, control, clientBounds);
		}

		protected internal override void DrawAutoHideBarBackground(Control container, Control autoHideBar, Graphics graphics, Rectangle bounds)
		{
			using (this.solidBrush_0 = new SolidBrush(this.color_7))
			{
				graphics.FillRectangle(this.solidBrush_0, bounds);
			}
		}

		protected internal override void DrawCollapsedTab(Graphics graphics, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool vertical)
		{
			using (SolidBrush solidBrush = new SolidBrush(backColor))
			{
				graphics.FillRectangle(solidBrush, bounds);
			}
			if (dockSide != DockSide.Top)
			{
				graphics.DrawLine(this.pen_2, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
			}
			if (dockSide != DockSide.Right)
			{
				graphics.DrawLine(this.pen_2, bounds.Right, bounds.Top, bounds.Right, bounds.Bottom);
			}
			if (dockSide != DockSide.Bottom)
			{
				graphics.DrawLine(this.pen_2, bounds.Left, bounds.Bottom, bounds.Right, bounds.Bottom);
			}
			if (dockSide != DockSide.Left)
			{
				graphics.DrawLine(this.pen_2, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom);
			}
			bounds.Inflate(-2, -2);
			if (vertical)
			{
				bounds.Offset(0, 1);
			}
			else
			{
				bounds.Offset(1, 0);
			}
			graphics.DrawImage(image, new Rectangle(bounds.Left, bounds.Top, image.Width, image.Height));
			if (text.Length != 0)
			{
				if (vertical)
				{
					bounds.Offset(0, 23);
					graphics.DrawString(text, font, this.solidBrush_1, bounds, EverettRenderer.StringFormat_1);
					return;
				}
				bounds.Offset(23, 0);
				graphics.DrawString(text, font, this.solidBrush_1, bounds, EverettRenderer.StringFormat_0);
			}
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
			graphics.FillRectangle(this.solidBrush_0, bounds);
			graphics.DrawLine(this.pen_1, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
		}

		protected internal override void DrawDocumentStripButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
		{
			this.vmethod_0(graphics, bounds, state);
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				bounds.Offset(1, 1);
			}
			switch (buttonType)
			{
			case SandDockButtonType.Close:
				using (Pen pen = new Pen(this.color_6))
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
				Class15.smethod_2(graphics, bounds, this.color_6, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
				return;
			case SandDockButtonType.ActiveFiles:
				Class15.smethod_0(graphics, bounds, SystemPens.ControlText);
				return;
			default:
				return;
			}
			Class15.smethod_1(graphics, bounds, this.color_6, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
		}

		protected internal override void DrawDocumentStripTab(Graphics graphics, Rectangle bounds, Rectangle contentBounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				using (SolidBrush solidBrush = new SolidBrush(backColor))
				{
					graphics.FillRectangle(solidBrush, bounds);
				}
				graphics.DrawLine(this.pen_1, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
				graphics.DrawLine(this.pen_1, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
				graphics.DrawLine(this.pen_0, bounds.Right - 1, bounds.Top + 1, bounds.Right - 1, bounds.Bottom - 1);
			}
			else if (drawSeparator)
			{
				graphics.DrawLine(SystemPens.ControlDark, bounds.Right, bounds.Top + 3, bounds.Right, bounds.Bottom - 3);
			}
			bounds = contentBounds;
			if (image != null)
			{
				graphics.DrawImage(image, bounds.X + 4, bounds.Y + 2, 16, 16);
				bounds.X += 20;
				bounds.Width -= 20;
			}
			if (bounds.Width > 8)
			{
				Font font2 = font;
				if ((state & DrawItemState.Focus) == DrawItemState.Focus)
				{
					font2 = new Font(font, FontStyle.Bold);
				}
				if ((state & DrawItemState.Selected) != DrawItemState.Selected)
				{
					graphics.DrawString(text, font2, this.solidBrush_1, bounds, this.stringFormat_2);
				}
				else
				{
					using (SolidBrush solidBrush2 = new SolidBrush(foreColor))
					{
						graphics.DrawString(text, font2, solidBrush2, bounds, this.stringFormat_2);
					}
				}
				if ((state & DrawItemState.Focus) == DrawItemState.Focus)
				{
					font2.Dispose();
				}
			}
		}

		protected internal override void DrawSplitter(Control container, Control control, Graphics graphics, Rectangle bounds, Orientation orientation)
		{
		}

		protected internal override void DrawTabStripBackground(Control container, Control control, Graphics graphics, Rectangle bounds, int selectedTabOffset)
		{
			graphics.FillRectangle(this.solidBrush_0, bounds);
			graphics.DrawLine(this.pen_0, bounds.X, bounds.Y, bounds.Right, bounds.Y);
		}

		protected internal override void DrawTabStripTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				using (SolidBrush solidBrush = new SolidBrush(backColor))
				{
					graphics.FillRectangle(solidBrush, bounds);
				}
				graphics.DrawLine(this.pen_1, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
				graphics.DrawLine(this.pen_0, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
				graphics.DrawLine(this.pen_0, bounds.Right, bounds.Top, bounds.Right, bounds.Bottom - 1);
			}
			else if (drawSeparator)
			{
				graphics.DrawLine(SystemPens.ControlDark, bounds.Right, bounds.Top + 3, bounds.Right, bounds.Bottom - 3);
			}
			if (bounds.Width >= 24)
			{
				graphics.DrawImage(image, new Rectangle(bounds.X + 4, bounds.Y + 2, image.Width, image.Height));
			}
			bounds.X += 23;
			bounds.Width -= 25;
			if (bounds.Width > 8)
			{
				if ((state & DrawItemState.Selected) == DrawItemState.Selected)
				{
					using (SolidBrush solidBrush2 = new SolidBrush(foreColor))
					{
						graphics.DrawString(text, font, solidBrush2, bounds, EverettRenderer.StringFormat_0);
						return;
					}
				}
				graphics.DrawString(text, font, this.solidBrush_1, bounds, EverettRenderer.StringFormat_0);
			}
		}

		protected internal override void DrawTitleBarBackground(Graphics graphics, Rectangle bounds, bool focused)
		{
			if (focused)
			{
				graphics.FillRectangle(SystemBrushes.ActiveCaption, bounds);
				return;
			}
			graphics.FillRectangle(SystemBrushes.Control, bounds);
			graphics.DrawLine(SystemPens.ControlDark, bounds.X + 1, bounds.Y, bounds.Right - 2, bounds.Y);
			graphics.DrawLine(SystemPens.ControlDark, bounds.X + 1, bounds.Bottom - 1, bounds.Right - 2, bounds.Bottom - 1);
			graphics.DrawLine(SystemPens.ControlDark, bounds.X, bounds.Y + 1, bounds.X, bounds.Bottom - 2);
			graphics.DrawLine(SystemPens.ControlDark, bounds.Right - 1, bounds.Y + 1, bounds.Right - 1, bounds.Bottom - 2);
		}

		protected internal override void DrawTitleBarButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state, bool focused, bool toggled)
		{
			bounds.Width--;
			bounds.Height--;
			this.vmethod_0(graphics, bounds, state);
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				bounds.Offset(1, 1);
			}
			switch (buttonType)
			{
			case SandDockButtonType.Close:
				Class15.smethod_6(graphics, bounds, focused ? SystemPens.ActiveCaptionText : SystemPens.ControlText);
				return;
			case SandDockButtonType.Pin:
				Class15.smethod_4(graphics, bounds, focused ? SystemPens.ActiveCaptionText : SystemPens.ControlText, toggled);
				return;
			case SandDockButtonType.ScrollLeft:
			case SandDockButtonType.ScrollRight:
				break;
			case SandDockButtonType.WindowPosition:
				Class15.smethod_0(graphics, bounds, focused ? SystemPens.ActiveCaptionText : SystemPens.ControlText);
				break;
			default:
				return;
			}
		}

		protected internal override void DrawTitleBarText(Graphics graphics, Rectangle bounds, bool focused, string text, Font font)
		{
			Brush brush = focused ? SystemBrushes.ActiveCaptionText : SystemBrushes.ControlText;
			bounds.Inflate(-3, 0);
			graphics.DrawString(text, font, brush, bounds, EverettRenderer.StringFormat_0);
		}

		public override void FinishRenderSession()
		{
			this.solidBrush_0.Dispose();
			this.pen_0.Dispose();
			this.pen_1.Dispose();
			this.solidBrush_1.Dispose();
			this.pen_2.Dispose();
			this.stringFormat_2.Dispose();
		}

		protected override void GetColorsFromSystem()
		{
			this.color_7 = this.method_1(SystemColors.Control);
		}

		protected internal override Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			int num;
			if ((state & DrawItemState.Focus) == DrawItemState.Focus)
			{
				using (Font font2 = new Font(font, FontStyle.Bold))
				{
					num = (int)Math.Ceiling((double)graphics.MeasureString(text, font2, 999, this.stringFormat_2).Width);
					goto IL_65;
				}
			}
			num = (int)Math.Ceiling((double)graphics.MeasureString(text, font, 999, this.stringFormat_2).Width);
			IL_65:
			num += 2 + this.Int32_0 * 2 + 2;
			if (image != null)
			{
				num += 20;
			}
			num += this.DocumentTabExtra;
			return new Size(num, 0);
		}

		protected internal override Size MeasureTabStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			int num = (int)Math.Ceiling((double)graphics.MeasureString(text, font, 2147483647, this.stringFormat_2).Width);
			num += 30;
			return new Size(num, 18);
		}

		private Color method_1(Color color_8)
		{
			byte b = color_8.R;
			byte b2 = color_8.G;
			byte b3 = color_8.B;
			byte b4 = Math.Max(Math.Max(b, b2), b3);
			if (b4 != 0)
			{
				byte b5 = (byte)((b4 <= 220) ? 35 : (255 - b4));
				b += (byte)((double)((float)b5 * ((float)b / (float)b4)) + 0.5);
				b2 += (byte)((double)((float)b5 * ((float)b2 / (float)b4)) + 0.5);
				b3 += (byte)((double)((float)b5 * ((float)b3 / (float)b4)) + 0.5);
				return Color.FromArgb((int)b, (int)b2, (int)b3);
			}
			return Color.FromArgb(35, 35, 35);
		}

		public override void StartRenderSession(HotkeyPrefix hotKeys)
		{
			this.solidBrush_0 = new SolidBrush(this.color_7);
			this.pen_0 = new Pen(this.color_2);
			this.pen_1 = new Pen(this.color_3);
			this.solidBrush_1 = new SolidBrush(this.color_4);
			this.pen_2 = new Pen(this.color_5);
			this.stringFormat_2 = new StringFormat(StringFormat.GenericDefault);
			this.stringFormat_2.FormatFlags = StringFormatFlags.NoWrap;
			this.stringFormat_2.Alignment = StringAlignment.Center;
			this.stringFormat_2.LineAlignment = StringAlignment.Center;
			this.stringFormat_2.HotkeyPrefix = hotKeys;
		}

		public override string ToString()
		{
			return "Everett";
		}

		internal virtual void vmethod_0(Graphics graphics_0, Rectangle rectangle_0, DrawItemState drawItemState_0)
		{
			if ((drawItemState_0 & DrawItemState.HotLight) == DrawItemState.HotLight)
			{
				Pen pen;
				Pen pen2;
				if ((drawItemState_0 & DrawItemState.Selected) != DrawItemState.Selected)
				{
					pen = this.pen_0;
					pen2 = this.pen_1;
				}
				else
				{
					pen2 = this.pen_0;
					pen = this.pen_1;
				}
				graphics_0.DrawLine(pen2, rectangle_0.Left, rectangle_0.Top, rectangle_0.Right - 1, rectangle_0.Top);
				graphics_0.DrawLine(pen2, rectangle_0.Left, rectangle_0.Top, rectangle_0.Left, rectangle_0.Bottom - 1);
				graphics_0.DrawLine(pen, rectangle_0.Right - 1, rectangle_0.Bottom - 1, rectangle_0.Right - 1, rectangle_0.Top);
				graphics_0.DrawLine(pen, rectangle_0.Right - 1, rectangle_0.Bottom - 1, rectangle_0.Left, rectangle_0.Bottom - 1);
			}
		}

		public Color ActiveTitleBarColor
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

		public Color BackgroundTabForeColor
		{
			get
			{
				return this.color_4;
			}
			set
			{
				this.color_4 = value;
			}
		}

		public Color CollapsedTabOutlineColor
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

		protected internal override int DocumentTabExtra
		{
			get
			{
				return 0;
			}
		}

		protected internal override int DocumentTabSize
		{
			get
			{
				return Control.DefaultFont.Height + 6;
			}
		}

		protected internal override int DocumentTabStripSize
		{
			get
			{
				return Control.DefaultFont.Height + 8;
			}
		}

		public Color HighlightColor
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

		public Color InactiveTitleBarColor
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

		internal virtual int Int32_0
		{
			get
			{
				return 5;
			}
		}

		public Color ShadowColor
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

		internal static StringFormat StringFormat_0
		{
			get
			{
				if (EverettRenderer.stringFormat_0 == null)
				{
					EverettRenderer.stringFormat_0 = new StringFormat(StringFormat.GenericDefault);
					EverettRenderer.stringFormat_0.Alignment = StringAlignment.Near;
					EverettRenderer.stringFormat_0.LineAlignment = StringAlignment.Center;
					EverettRenderer.stringFormat_0.Trimming = StringTrimming.EllipsisCharacter;
					EverettRenderer.stringFormat_0.FormatFlags |= StringFormatFlags.NoWrap;
				}
				return EverettRenderer.stringFormat_0;
			}
		}

		internal static StringFormat StringFormat_1
		{
			get
			{
				if (EverettRenderer.stringFormat_1 == null)
				{
					EverettRenderer.stringFormat_1 = new StringFormat(StringFormat.GenericDefault);
					EverettRenderer.stringFormat_1.Alignment = StringAlignment.Near;
					EverettRenderer.stringFormat_1.LineAlignment = StringAlignment.Center;
					EverettRenderer.stringFormat_1.Trimming = StringTrimming.EllipsisCharacter;
					EverettRenderer.stringFormat_1.FormatFlags |= StringFormatFlags.NoWrap;
					EverettRenderer.stringFormat_1.FormatFlags |= StringFormatFlags.DirectionVertical;
				}
				return EverettRenderer.stringFormat_1;
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
					this.boxModel_1 = new BoxModel(0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
				}
				return this.boxModel_1;
			}
		}

		public Color TabStripBackgroundColor
		{
			get
			{
				return this.color_7;
			}
		}

		protected internal override BoxModel TabStripMetrics
		{
			get
			{
				if (this.boxModel_0 == null)
				{
					this.boxModel_0 = new BoxModel(0, Control.DefaultFont.Height + 9, 4, 0, 5, 1, 0, 2, 0, 0);
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
				if (this.boxModel_2 == null)
				{
					this.boxModel_2 = new BoxModel(0, SystemInformation.ToolWindowCaptionHeight + 2, 0, 0, 0, 0, 0, 0, 0, 2);
				}
				return this.boxModel_2;
			}
		}

		private BoxModel boxModel_0;

		private BoxModel boxModel_1;

		private BoxModel boxModel_2;

		private Color color_0 = SystemColors.InactiveCaption;

		private Color color_1 = SystemColors.ActiveCaption;

		private Color color_2 = SystemColors.ControlText;

		private Color color_3 = SystemColors.ControlLightLight;

		private Color color_4 = SystemColors.ControlDarkDark;

		private Color color_5 = SystemColors.ControlDark;

		private Color color_6 = SystemColors.ControlDarkDark;

		private Color color_7;

		private Pen pen_0;

		private Pen pen_1;

		private Pen pen_2;

		private SolidBrush solidBrush_0;

		private SolidBrush solidBrush_1;

		private static StringFormat stringFormat_0;

		private static StringFormat stringFormat_1;

		private StringFormat stringFormat_2;
	}
}
