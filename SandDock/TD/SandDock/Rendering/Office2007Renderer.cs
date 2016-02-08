using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
    public enum Office2007ColorScheme
    {
        Blue,
        Silver,
        Black
    }
    public class Office2007Renderer : RendererBase
	{
		public Office2007Renderer() : this(Office2007ColorScheme.Blue)
		{
		}

		public Office2007Renderer(Office2007ColorScheme colorScheme)
		{
			this.ColorScheme = colorScheme;
		}

		protected internal override Rectangle AdjustDockControlClientBounds(ControlLayoutSystem layoutSystem, DockControl control, Rectangle clientBounds)
		{
			if (layoutSystem is DocumentLayoutSystem)
			{
				clientBounds.X++;
				clientBounds.Width -= 2;
				clientBounds.Height--;
				return clientBounds;
			}
			return base.AdjustDockControlClientBounds(layoutSystem, control, clientBounds);
		}

		protected internal override void DrawAutoHideBarBackground(Control container, Control autoHideBar, Graphics graphics, Rectangle bounds)
		{
			using (SolidBrush solidBrush = new SolidBrush(this.Background))
			{
				graphics.FillRectangle(solidBrush, bounds);
			}
		}

		protected internal override void DrawCollapsedTab(Graphics graphics, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool vertical)
		{
			using (Brush brush = this.method_3(bounds, vertical ? this.CollapsedTabVerticalBackground : this.CollapsedTabHorizontalBackground, vertical ? LinearGradientMode.Horizontal : LinearGradientMode.Vertical))
			{
				if (dockSide != DockSide.Left)
				{
					if (dockSide != DockSide.Right)
					{
						Class16.smethod_6(graphics, bounds, dockSide, image, text, font, brush, Brushes.Black, this.CollapsedTabBorder, this.TabTextDisplay == TabTextDisplayMode.AllTabs);
						goto IL_8E;
					}
				}
				using (Image image2 = new Bitmap(image))
				{
					image2.RotateFlip(RotateFlipType.Rotate90FlipNone);
					Class16.smethod_6(graphics, bounds, dockSide, image2, text, font, brush, Brushes.Black, this.CollapsedTabBorder, this.TabTextDisplay == TabTextDisplayMode.AllTabs);
				}
				IL_8E:;
			}
		}

		protected internal override void DrawControlClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
			using (Pen pen = new Pen(this.DockedWindowOuterBorder))
			{
				graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
			}
		}

		protected internal override void DrawDockContainerBackground(Graphics graphics, DockContainer container, Rectangle bounds)
		{
			if (bounds.Width > 0 && bounds.Height > 0)
			{
				if (!(container is DocumentContainer))
				{
					Class16.smethod_0(graphics, this.Background);
				}
				else
				{
					using (Brush brush = this.method_3(bounds, this.DocumentContainerBackground, LinearGradientMode.Vertical))
					{
						graphics.FillRectangle(brush, bounds);
					}
				}
				return;
			}
		}

		protected internal override void DrawDocumentClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
			using (SolidBrush solidBrush = new SolidBrush(backColor))
			{
				graphics.FillRectangle(solidBrush, bounds);
			}
			using (Pen pen = new Pen(this.DocumentStripBorder))
			{
				graphics.DrawLines(pen, new Point[]
				{
					new Point(bounds.X, bounds.Y),
					new Point(bounds.X, bounds.Bottom - 1),
					new Point(bounds.Right - 1, bounds.Bottom - 1),
					new Point(bounds.Right - 1, bounds.Y)
				});
			}
		}

		protected internal override void DrawDocumentStripBackground(Graphics graphics, Rectangle bounds)
		{
			if (bounds.Width > 0 && bounds.Height > 0)
			{
				using (Pen pen = new Pen(this.DocumentStripBorder))
				{
					graphics.DrawLine(pen, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
				}
				return;
			}
		}

		protected internal override void DrawDocumentStripButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
		{
			this.method_7(graphics, bounds, state, true);
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
			if (bounds.Width > 0 && bounds.Height > 0)
			{
				bool bool_ = (state & DrawItemState.Checked) == DrawItemState.Checked;
				Color color;
				Color color2;
				ColorBlend colorBlend_;
				if ((state & DrawItemState.Selected) == DrawItemState.Selected)
				{
					color = this.DocumentSelectedTabOuterBorder;
					color2 = this.DocumentSelectedTabInnerBorder;
					colorBlend_ = this.DocumentSelectedTabBackground;
				}
				else if ((state & DrawItemState.HotLight) != DrawItemState.HotLight)
				{
					color = this.DocumentNormalTabOuterBorder;
					color2 = this.DocumentNormalTabInnerBorder;
					colorBlend_ = this.DocumentNormalTabBackground;
				}
				else
				{
					color = this.DocumentHotTabOuterBorder;
					color2 = this.DocumentHotTabInnerBorder;
					colorBlend_ = this.DocumentHotTabBackground;
				}
				using (Brush brush = this.method_3(bounds, colorBlend_, LinearGradientMode.Vertical))
				{
					using (Pen pen = new Pen(color))
					{
						using (Pen pen2 = new Pen(color2))
						{
							this.method_8(graphics, bounds, contentBounds, image, text, font, backColor, pen, pen2, brush, state, bool_, this.DocumentTabSize, this.DocumentTabExtra, this.TextFormat);
						}
					}
				}
				return;
			}
		}

		protected internal override void DrawSplitter(Control container, Control control, Graphics graphics, Rectangle bounds, Orientation orientation)
		{
			if (!(control is DocumentContainer))
			{
				using (SolidBrush solidBrush = new SolidBrush(this.Background))
				{
					graphics.FillRectangle(solidBrush, bounds);
				}
			}
		}

		public override void DrawTabControlBackground(Graphics graphics, Rectangle bounds, Color backColor, bool client)
		{
			if (!client)
			{
				using (SolidBrush solidBrush = new SolidBrush(backColor))
				{
					graphics.FillRectangle(solidBrush, bounds);
				}
				using (Pen pen = new Pen(this.DocumentStripBorder))
				{
					graphics.DrawLines(pen, new Point[]
					{
						new Point(bounds.X, bounds.Y),
						new Point(bounds.X, bounds.Bottom - 1),
						new Point(bounds.Right - 1, bounds.Bottom - 1),
						new Point(bounds.Right - 1, bounds.Y)
					});
				}
			}
		}

		protected internal override void DrawTabStripBackground(Control container, Control control, Graphics graphics, Rectangle bounds, int selectedTabOffset)
		{
			using (SolidBrush solidBrush = new SolidBrush(this.Background))
			{
				graphics.FillRectangle(solidBrush, bounds);
			}
			using (Pen pen = new Pen(this.TabStripInnerBorder))
			{
				graphics.DrawLine(pen, bounds.X, bounds.Top + 1, bounds.Right - 1, bounds.Top + 1);
			}
			using (Pen pen2 = new Pen(this.TabStripOuterBorder))
			{
				graphics.DrawLine(pen2, bounds.X, bounds.Top + 2, bounds.Right - 1, bounds.Top + 2);
			}
		}

		protected internal override void DrawTabStripTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
		{
			bounds.Y += 2;
			bounds.Height -= 2;
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				using (LinearGradientBrush linearGradientBrush = this.method_3(bounds, this.TabStripSelectedTabBackground, LinearGradientMode.Vertical))
				{
					Class16.smethod_4(graphics, bounds, image, this.ImageSize, text, font, linearGradientBrush, SystemColors.ControlText, this.TabStripOuterBorder, state, this.TextFormat);
					return;
				}
			}
			Class16.smethod_3(graphics, bounds, image, this.ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : backColor, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, this.TabStripNormalTabForeground, this.TabStripOuterBorder, state, this.TextFormat);
		}

		protected internal override void DrawTitleBarBackground(Graphics graphics, Rectangle bounds, bool focused)
		{
			using (Pen pen = new Pen(this.DockedWindowOuterBorder))
			{
				graphics.DrawLines(pen, new Point[]
				{
					new Point(bounds.X, bounds.Bottom - 1),
					new Point(bounds.X, bounds.Y + 1),
					new Point(bounds.X + 1, bounds.Y),
					new Point(bounds.Right - 2, bounds.Y),
					new Point(bounds.Right - 1, bounds.Y + 1),
					new Point(bounds.Right - 1, bounds.Bottom - 1)
				});
			}
			bounds.X++;
			bounds.Y++;
			bounds.Width -= 2;
			bounds.Height--;
			if (bounds.Width > 0 && bounds.Height > 0)
			{
				using (LinearGradientBrush linearGradientBrush = this.method_3(bounds, focused ? this.ActiveTitleBarBackground : this.InactiveTitleBarBackground, LinearGradientMode.Vertical))
				{
					graphics.FillRectangle(linearGradientBrush, bounds);
				}
			}
			using (Pen pen2 = new Pen(this.DockedWindowInnerBorder))
			{
				graphics.DrawLines(pen2, new Point[]
				{
					new Point(bounds.X, bounds.Bottom - 1),
					new Point(bounds.X, bounds.Y),
					new Point(bounds.Right - 1, bounds.Y),
					new Point(bounds.Right - 1, bounds.Bottom - 1)
				});
			}
		}

		protected internal override void DrawTitleBarButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state, bool focused, bool toggled)
		{
			this.method_7(graphics, bounds, state, focused);
			using (Pen pen = (!focused) ? new Pen(Color.Black) : new Pen(Color.Black))
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
			TextFormatFlags textFormatFlags = this.TextFormat;
			textFormatFlags |= TextFormatFlags.NoPrefix;
			bounds.X += 3;
			TextRenderer.DrawText(graphics, text, font, bounds, focused ? Color.Black : Color.Black, textFormatFlags);
		}

		public override void FinishRenderSession()
		{
			this.int_0 = Math.Max(this.int_0 - 1, 0);
		}

		protected internal override Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			TextFormatFlags textFormatFlags = this.TextFormat;
			textFormatFlags &= ~TextFormatFlags.NoPrefix;
			int num;
			using (Font font2 = new Font(font, FontStyle.Bold))
			{
				num = TextRenderer.MeasureText(graphics, text, font2, new Size(2147483647, 2147483647), textFormatFlags).Width;
			}
			num += 14;
			if (image != null)
			{
				num += this.ImageSize.Width + 4;
			}
			num += this.DocumentTabExtra;
			return new Size(num, 0);
		}

		protected internal override Size MeasureTabStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			return Class16.smethod_2(graphics, image, this.ImageSize, text, font, this.TextFormat);
		}

		private void method_1()
		{
			this.boxModel_0 = null;
			this.boxModel_1 = null;
			this.boxModel_2 = null;
		}

		private ColorBlend method_2(float[] float_0, Color[] color_14)
		{
			ColorBlend colorBlend = new ColorBlend(float_0.Length);
			for (int i = 0; i < float_0.Length; i++)
			{
				colorBlend.Positions[i] = float_0[i];
				colorBlend.Colors[i] = color_14[i];
			}
			return colorBlend;
		}

		private LinearGradientBrush method_3(Rectangle rectangle_0, ColorBlend colorBlend_16, LinearGradientMode linearGradientMode_0)
		{
			return new LinearGradientBrush(rectangle_0, Color.Black, Color.White, linearGradientMode_0)
			{
				InterpolationColors = colorBlend_16
			};
		}

		private void method_4()
		{
			this.Background = ColorTranslator.FromHtml("#BFDBFF");
			this.DockedWindowOuterBorder = ColorTranslator.FromHtml("#7596BF");
			this.DockedWindowInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			this.InactiveTitleBarBackground = this.method_2(new float[]
			{
				0f,
				0.35f,
				0.35f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#E4EBF6"),
				ColorTranslator.FromHtml("#D9E7F9"),
				ColorTranslator.FromHtml("#CADEF7"),
				ColorTranslator.FromHtml("#DBF4FE")
			});
			this.ActiveTitleBarBackground = this.method_2(new float[]
			{
				0f,
				0.7f,
				0.7f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFCDA"),
				ColorTranslator.FromHtml("#FFE790"),
				ColorTranslator.FromHtml("#FFD74C"),
				ColorTranslator.FromHtml("#FFD346")
			});
			this.TabStripOuterBorder = ColorTranslator.FromHtml("#7596BF");
			this.TabStripInnerBorder = ColorTranslator.FromHtml("#E7EFF8");
			this.TabStripSelectedTabBackground = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F7FBFF"),
				ColorTranslator.FromHtml("#EEF5FB"),
				ColorTranslator.FromHtml("#E1EAF6"),
				ColorTranslator.FromHtml("#F7FBFF")
			});
			this.TabStripSelectedTabBorder = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.7f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#E1EAF6"),
				ColorTranslator.FromHtml("#CDFBFF"),
				ColorTranslator.FromHtml("#D0FBFF"),
				ColorTranslator.FromHtml("#F4F9FF")
			});
			this.TabStripNormalTabForeground = ColorTranslator.FromHtml("#15428B");
			this.ButtonHotOuterBorder = this.method_2(new float[]
			{
				0f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#DBCE99"),
				ColorTranslator.FromHtml("#B9A074"),
				ColorTranslator.FromHtml("#CBC3AA")
			});
			this.ButtonHotInnerBorder = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFFFB"),
				ColorTranslator.FromHtml("#FFF9E3"),
				ColorTranslator.FromHtml("#FFF2C9"),
				ColorTranslator.FromHtml("#FFFCDF")
			});
			this.ButtonHotBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFCE6"),
				ColorTranslator.FromHtml("#FFECA3"),
				ColorTranslator.FromHtml("#FFD844"),
				ColorTranslator.FromHtml("#FFE47F")
			});
			this.ButtonPressedOuterBorder = this.method_2(new float[]
			{
				0f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#7B6645"),
				ColorTranslator.FromHtml("#7B6645")
			});
			this.ButtonPressedInnerBorder = this.method_2(new float[]
			{
				0f,
				0.1f,
				0.6f,
				0.6f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#B2855C"),
				ColorTranslator.FromHtml("#F1B072"),
				ColorTranslator.FromHtml("#F1963B"),
				ColorTranslator.FromHtml("#ED7804"),
				ColorTranslator.FromHtml("#FDAD03")
			});
			this.ButtonPressedBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F3A570"),
				ColorTranslator.FromHtml("#E57840"),
				ColorTranslator.FromHtml("#DE550A"),
				ColorTranslator.FromHtml("#FEA14E")
			});
			this.CollapsedTabBorder = ColorTranslator.FromHtml("#7596BF");
			this.CollapsedTabHorizontalBackground = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F7FBFF"),
				ColorTranslator.FromHtml("#EEF5FB"),
				ColorTranslator.FromHtml("#E1EAF6"),
				ColorTranslator.FromHtml("#F7FBFF")
			});
			this.CollapsedTabVerticalBackground = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F7FBFF"),
				ColorTranslator.FromHtml("#EEF5FB"),
				ColorTranslator.FromHtml("#E1EAF6"),
				ColorTranslator.FromHtml("#F7FBFF")
			});
			this.DocumentContainerBackground = this.method_2(new float[]
			{
				0f,
				0.7f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#A3C2EA"),
				ColorTranslator.FromHtml("#567DB0"),
				ColorTranslator.FromHtml("#6591CD")
			});
			this.DocumentStripBorder = ColorTranslator.FromHtml("#678CBD");
			this.DocumentNormalTabOuterBorder = ColorTranslator.FromHtml("#6593CF");
			this.DocumentNormalTabInnerBorder = ColorTranslator.FromHtml("#E3EFFF");
			this.DocumentNormalTabBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#BEDAFF"),
				ColorTranslator.FromHtml("#AED2FF"),
				ColorTranslator.FromHtml("#8FBCF6"),
				ColorTranslator.FromHtml("#98C4FD")
			});
			this.DocumentHotTabOuterBorder = ColorTranslator.FromHtml("#6593CF");
			this.DocumentHotTabInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			this.DocumentHotTabBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#E1EEFF"),
				ColorTranslator.FromHtml("#D7E8FF"),
				ColorTranslator.FromHtml("#AED2FF"),
				ColorTranslator.FromHtml("#BEDAFF")
			});
			this.DocumentSelectedTabOuterBorder = ColorTranslator.FromHtml("#95774A");
			this.DocumentSelectedTabInnerBorder = ColorTranslator.FromHtml("#CDB69C");
			this.DocumentSelectedTabBackground = this.method_2(new float[]
			{
				0f,
				0.25f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFD19C"),
				ColorTranslator.FromHtml("#FFDBB3"),
				ColorTranslator.FromHtml("#FFFFFE")
			});
		}

		private void method_5()
		{
			this.Background = ColorTranslator.FromHtml("#535353");
			this.DockedWindowOuterBorder = ColorTranslator.FromHtml("#8C8E8F");
			this.DockedWindowInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			this.InactiveTitleBarBackground = this.method_2(new float[]
			{
				0f,
				0.35f,
				0.35f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#D7DADF"),
				ColorTranslator.FromHtml("#C1C6CF"),
				ColorTranslator.FromHtml("#B4BBC5"),
				ColorTranslator.FromHtml("#EBEBEB")
			});
			this.ActiveTitleBarBackground = this.method_2(new float[]
			{
				0f,
				0.7f,
				0.7f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFCDA"),
				ColorTranslator.FromHtml("#FFE790"),
				ColorTranslator.FromHtml("#FFD74C"),
				ColorTranslator.FromHtml("#FFD346")
			});
			this.TabStripOuterBorder = ColorTranslator.FromHtml("#BEBEBE");
			this.TabStripInnerBorder = ColorTranslator.FromHtml("#D7DADF");
			this.TabStripSelectedTabBackground = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F0F0F0"),
				ColorTranslator.FromHtml("#E3E6E9"),
				ColorTranslator.FromHtml("#D6D9DE"),
				ColorTranslator.FromHtml("#F0F1F2")
			});
			this.TabStripSelectedTabBorder = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.7f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#D5DBDC"),
				ColorTranslator.FromHtml("#B8F6FC"),
				ColorTranslator.FromHtml("#B7F7FD"),
				ColorTranslator.FromHtml("#E8EDEF")
			});
			this.TabStripNormalTabForeground = ColorTranslator.FromHtml("#FFFFFF");
			this.ButtonHotOuterBorder = this.method_2(new float[]
			{
				0f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#DBCE99"),
				ColorTranslator.FromHtml("#B9A074"),
				ColorTranslator.FromHtml("#CBC3AA")
			});
			this.ButtonHotInnerBorder = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFFFB"),
				ColorTranslator.FromHtml("#FFF9E3"),
				ColorTranslator.FromHtml("#FFF2C9"),
				ColorTranslator.FromHtml("#FFFCDF")
			});
			this.ButtonHotBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFCE6"),
				ColorTranslator.FromHtml("#FFECA3"),
				ColorTranslator.FromHtml("#FFD844"),
				ColorTranslator.FromHtml("#FFE47F")
			});
			this.ButtonPressedOuterBorder = this.method_2(new float[]
			{
				0f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#7B6645"),
				ColorTranslator.FromHtml("#7B6645")
			});
			this.ButtonPressedInnerBorder = this.method_2(new float[]
			{
				0f,
				0.1f,
				0.6f,
				0.6f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#B2855C"),
				ColorTranslator.FromHtml("#F1B072"),
				ColorTranslator.FromHtml("#F1963B"),
				ColorTranslator.FromHtml("#ED7804"),
				ColorTranslator.FromHtml("#FDAD03")
			});
			this.ButtonPressedBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F3A570"),
				ColorTranslator.FromHtml("#E57840"),
				ColorTranslator.FromHtml("#DE550A"),
				ColorTranslator.FromHtml("#FEA14E")
			});
			this.CollapsedTabBorder = ColorTranslator.FromHtml("#BEBEBE");
			this.CollapsedTabHorizontalBackground = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F0F0F0"),
				ColorTranslator.FromHtml("#E3E6E9"),
				ColorTranslator.FromHtml("#D6D9DE"),
				ColorTranslator.FromHtml("#F0F1F2")
			});
			this.CollapsedTabVerticalBackground = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F0F0F0"),
				ColorTranslator.FromHtml("#E3E6E9"),
				ColorTranslator.FromHtml("#D6D9DE"),
				ColorTranslator.FromHtml("#F0F1F2")
			});
			this.DocumentContainerBackground = this.method_2(new float[]
			{
				0f,
				0.7f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#4F4F4F"),
				ColorTranslator.FromHtml("#3B3B3B"),
				ColorTranslator.FromHtml("#0A0A0A")
			});
			this.DocumentStripBorder = ColorTranslator.FromHtml("#000000");
			this.DocumentNormalTabOuterBorder = ColorTranslator.FromHtml("#9199A4");
			this.DocumentNormalTabInnerBorder = ColorTranslator.FromHtml("#F0F1F2");
			this.DocumentNormalTabBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#DBDEE1"),
				ColorTranslator.FromHtml("#D3D6DB"),
				ColorTranslator.FromHtml("#BCC1C8"),
				ColorTranslator.FromHtml("#C5C9CF")
			});
			this.DocumentHotTabOuterBorder = ColorTranslator.FromHtml("#616A76");
			this.DocumentHotTabInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			this.DocumentHotTabBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F2F2F3"),
				ColorTranslator.FromHtml("#F8F8F9"),
				ColorTranslator.FromHtml("#D3D6DB"),
				ColorTranslator.FromHtml("#DBDEE1")
			});
			this.DocumentSelectedTabOuterBorder = ColorTranslator.FromHtml("#3D3D3D");
			this.DocumentSelectedTabInnerBorder = ColorTranslator.FromHtml("#CDB69C");
			this.DocumentSelectedTabBackground = this.method_2(new float[]
			{
				0f,
				0.25f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFD19C"),
				ColorTranslator.FromHtml("#FFDBB3"),
				ColorTranslator.FromHtml("#FFFFFE")
			});
		}

		private void method_6()
		{
			this.Background = ColorTranslator.FromHtml("#D0D4DD");
			this.DockedWindowOuterBorder = ColorTranslator.FromHtml("#BDBFC1");
			this.DockedWindowInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			this.InactiveTitleBarBackground = this.method_2(new float[]
			{
				0f,
				0.35f,
				0.35f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F2F4F8"),
				ColorTranslator.FromHtml("#E1E6EE"),
				ColorTranslator.FromHtml("#D5DBE7"),
				ColorTranslator.FromHtml("#F9F9F9")
			});
			this.ActiveTitleBarBackground = this.method_2(new float[]
			{
				0f,
				0.7f,
				0.7f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFCDA"),
				ColorTranslator.FromHtml("#FFE790"),
				ColorTranslator.FromHtml("#FFD74C"),
				ColorTranslator.FromHtml("#FFD346")
			});
			this.TabStripOuterBorder = ColorTranslator.FromHtml("#838383");
			this.TabStripInnerBorder = ColorTranslator.FromHtml("#F2F4F8");
			this.TabStripSelectedTabBackground = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFFFF"),
				ColorTranslator.FromHtml("#F7F6F8"),
				ColorTranslator.FromHtml("#EEF1F5"),
				ColorTranslator.FromHtml("#F2F7F9")
			});
			this.TabStripSelectedTabBorder = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.7f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#EAEFF5"),
				ColorTranslator.FromHtml("#C1FAFF"),
				ColorTranslator.FromHtml("#C6FAFF"),
				ColorTranslator.FromHtml("#ECFAFB")
			});
			this.TabStripNormalTabForeground = ColorTranslator.FromHtml("#4C535C");
			this.ButtonHotOuterBorder = this.method_2(new float[]
			{
				0f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#DBCE99"),
				ColorTranslator.FromHtml("#B9A074"),
				ColorTranslator.FromHtml("#CBC3AA")
			});
			this.ButtonHotInnerBorder = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFFFB"),
				ColorTranslator.FromHtml("#FFF9E3"),
				ColorTranslator.FromHtml("#FFF2C9"),
				ColorTranslator.FromHtml("#FFFCDF")
			});
			this.ButtonHotBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFCE6"),
				ColorTranslator.FromHtml("#FFECA3"),
				ColorTranslator.FromHtml("#FFD844"),
				ColorTranslator.FromHtml("#FFE47F")
			});
			this.ButtonPressedOuterBorder = this.method_2(new float[]
			{
				0f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#7B6645"),
				ColorTranslator.FromHtml("#7B6645")
			});
			this.ButtonPressedInnerBorder = this.method_2(new float[]
			{
				0f,
				0.1f,
				0.6f,
				0.6f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#B2855C"),
				ColorTranslator.FromHtml("#F1B072"),
				ColorTranslator.FromHtml("#F1963B"),
				ColorTranslator.FromHtml("#ED7804"),
				ColorTranslator.FromHtml("#FDAD03")
			});
			this.ButtonPressedBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#F3A570"),
				ColorTranslator.FromHtml("#E57840"),
				ColorTranslator.FromHtml("#DE550A"),
				ColorTranslator.FromHtml("#FEA14E")
			});
			this.CollapsedTabBorder = ColorTranslator.FromHtml("#838383");
			this.CollapsedTabHorizontalBackground = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFFFF"),
				ColorTranslator.FromHtml("#F7F6F8"),
				ColorTranslator.FromHtml("#EEF1F5"),
				ColorTranslator.FromHtml("#F2F7F9")
			});
			this.CollapsedTabVerticalBackground = this.method_2(new float[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFFFFF"),
				ColorTranslator.FromHtml("#F7F6F8"),
				ColorTranslator.FromHtml("#EEF1F5"),
				ColorTranslator.FromHtml("#F2F7F9")
			});
			this.DocumentContainerBackground = this.method_2(new float[]
			{
				0f,
				0.7f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#CCCFD8"),
				ColorTranslator.FromHtml("#BDC0C9"),
				ColorTranslator.FromHtml("#9B9FA6")
			});
			this.DocumentStripBorder = ColorTranslator.FromHtml("#858585");
			this.DocumentNormalTabOuterBorder = ColorTranslator.FromHtml("#6F7074");
			this.DocumentNormalTabInnerBorder = ColorTranslator.FromHtml("#EDF3F4");
			this.DocumentNormalTabBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#DCE0E5"),
				ColorTranslator.FromHtml("#D8DDE2"),
				ColorTranslator.FromHtml("#B5BAC3"),
				ColorTranslator.FromHtml("#C6CBD1")
			});
			this.DocumentHotTabOuterBorder = ColorTranslator.FromHtml("#6F7074");
			this.DocumentHotTabInnerBorder = ColorTranslator.FromHtml("#EDF3F4");
			this.DocumentHotTabBackground = this.method_2(new float[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FBFBFB"),
				ColorTranslator.FromHtml("#F1F1F2"),
				ColorTranslator.FromHtml("#CFD3D6"),
				ColorTranslator.FromHtml("#DEE0E3")
			});
			this.DocumentSelectedTabOuterBorder = ColorTranslator.FromHtml("#95774A");
			this.DocumentSelectedTabInnerBorder = ColorTranslator.FromHtml("#CDB69C");
			this.DocumentSelectedTabBackground = this.method_2(new float[]
			{
				0f,
				0.25f,
				1f
			}, new Color[]
			{
				ColorTranslator.FromHtml("#FFD19C"),
				ColorTranslator.FromHtml("#FFDBB3"),
				ColorTranslator.FromHtml("#FFFFFE")
			});
		}

		private void method_7(Graphics graphics_0, Rectangle rectangle_0, DrawItemState drawItemState_0, bool bool_1)
		{
			if ((drawItemState_0 & DrawItemState.HotLight) == DrawItemState.HotLight)
			{
				bool flag = (drawItemState_0 & DrawItemState.Selected) == DrawItemState.Selected;
				using (Brush brush = this.method_3(rectangle_0, flag ? this.ButtonPressedOuterBorder : this.ButtonHotOuterBorder, LinearGradientMode.Vertical))
				{
					using (Pen pen = new Pen(brush))
					{
						graphics_0.DrawPolygon(pen, new Point[]
						{
							new Point(rectangle_0.X + 1, rectangle_0.Y),
							new Point(rectangle_0.Right - 2, rectangle_0.Y),
							new Point(rectangle_0.Right - 1, rectangle_0.Y + 1),
							new Point(rectangle_0.Right - 1, rectangle_0.Bottom - 2),
							new Point(rectangle_0.Right - 2, rectangle_0.Bottom - 1),
							new Point(rectangle_0.X + 1, rectangle_0.Bottom - 1),
							new Point(rectangle_0.X, rectangle_0.Bottom - 2),
							new Point(rectangle_0.X, rectangle_0.Y + 1)
						});
					}
				}
				using (Brush brush2 = this.method_3(rectangle_0, flag ? this.ButtonPressedInnerBorder : this.ButtonHotInnerBorder, LinearGradientMode.Vertical))
				{
					using (Pen pen2 = new Pen(brush2))
					{
						graphics_0.DrawRectangle(pen2, rectangle_0.X + 1, rectangle_0.Y + 1, rectangle_0.Width - 3, rectangle_0.Height - 3);
					}
				}
				using (Brush brush3 = this.method_3(rectangle_0, flag ? this.ButtonPressedBackground : this.ButtonHotBackground, LinearGradientMode.Vertical))
				{
					graphics_0.FillRectangle(brush3, rectangle_0.X + 2, rectangle_0.Y + 2, rectangle_0.Width - 4, rectangle_0.Height - 4);
				}
			}
		}

		private void method_8(Graphics graphics_0, Rectangle rectangle_0, Rectangle rectangle_1, Image image_0, string string_0, Font font_0, Color color_14, Pen pen_0, Pen pen_1, Brush brush_0, DrawItemState drawItemState_0, bool bool_1, int int_1, int int_2, TextFormatFlags textFormatFlags_1)
		{
			if ((drawItemState_0 & DrawItemState.Selected) == DrawItemState.Selected)
			{
				rectangle_0.Height++;
				int_1++;
			}
			graphics_0.DrawLine(pen_0, rectangle_0.Left + 1, rectangle_0.Bottom - 2, rectangle_0.Left + int_1 - 3, rectangle_0.Top + 2);
			graphics_0.DrawLine(pen_0, rectangle_0.Left + int_1 - 3, rectangle_0.Top + 2, rectangle_0.Left + int_1 - 2, rectangle_0.Top + 2);
			graphics_0.DrawLine(pen_0, rectangle_0.Left + int_1 - 1, rectangle_0.Top + 1, rectangle_0.Left + int_1, rectangle_0.Top + 1);
			graphics_0.DrawLine(pen_0, rectangle_0.Left + int_1 + 1, rectangle_0.Top, rectangle_0.Right - 3, rectangle_0.Top);
			graphics_0.DrawLine(pen_0, rectangle_0.Right - 3, rectangle_0.Top, rectangle_0.Right - 1, rectangle_0.Top + 2);
			graphics_0.DrawLine(pen_0, rectangle_0.Right - 1, rectangle_0.Top + 2, rectangle_0.Right - 1, rectangle_0.Bottom - 2);
			graphics_0.DrawLine(pen_1, rectangle_0.Left + 2, rectangle_0.Bottom - 2, rectangle_0.Left + int_1 - 3, rectangle_0.Top + 3);
			graphics_0.DrawLine(pen_1, rectangle_0.Left + int_1 - 3, rectangle_0.Top + 3, rectangle_0.Left + int_1 - 2, rectangle_0.Top + 3);
			graphics_0.DrawLine(pen_1, rectangle_0.Left + int_1 - 1, rectangle_0.Top + 2, rectangle_0.Left + int_1, rectangle_0.Top + 2);
			graphics_0.DrawLine(pen_1, rectangle_0.Left + int_1 + 1, rectangle_0.Top + 1, rectangle_0.Right - 4, rectangle_0.Top + 1);
			graphics_0.DrawLine(pen_1, rectangle_0.Right - 3, rectangle_0.Top + 1, rectangle_0.Right - 2, rectangle_0.Top + 2);
			graphics_0.DrawLine(pen_1, rectangle_0.Right - 2, rectangle_0.Top + 2, rectangle_0.Right - 2, rectangle_0.Bottom - 2);
			graphics_0.FillPolygon(brush_0, new Point[]
			{
				new Point(rectangle_0.Left + 2, rectangle_0.Bottom - 1),
				new Point(rectangle_0.Left + int_1 - 3, rectangle_0.Top + 4),
				new Point(rectangle_0.Left + int_1 + 1, rectangle_0.Top + 2),
				new Point(rectangle_0.Right - 2, rectangle_0.Top + 2),
				new Point(rectangle_0.Right - 2, rectangle_0.Bottom - 1)
			});
			rectangle_0 = rectangle_1;
			rectangle_0.X += int_2;
			rectangle_0.Width -= int_2;
			if (image_0 != null)
			{
				graphics_0.DrawImage(image_0, rectangle_0.X + 4, rectangle_0.Y + 2, this.ImageSize.Width, this.ImageSize.Height);
				rectangle_0.X += this.ImageSize.Width + 4;
				rectangle_0.Width -= this.ImageSize.Width + 4;
			}
			if (rectangle_0.Width > 8)
			{
				textFormatFlags_1 |= TextFormatFlags.HorizontalCenter;
				textFormatFlags_1 &= (TextFormatFlags)(-1);
				TextRenderer.DrawText(graphics_0, string_0, font_0, rectangle_0, SystemColors.ControlText, textFormatFlags_1);
			}
			if (bool_1)
			{
				Rectangle rectangle = rectangle_0;
				rectangle.Inflate(-2, -2);
				rectangle.Height += 2;
				rectangle.X++;
				rectangle.Width--;
				ControlPaint.DrawFocusRectangle(graphics_0, rectangle);
			}
		}

		public override void ModifyDefaultWindowColors(DockControl window, ref Color backColor, ref Color borderColor)
		{
			borderColor = this.DockedWindowOuterBorder;
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

		public override string ToString()
		{
			return "Office 2007";
		}

		public ColorBlend ActiveTitleBarBackground
		{
			get
			{
				return this.colorBlend_0;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_0 = value;
			}
		}

		public Color Background
		{
			get
			{
				return this.color_0;
			}
			set
			{
				this.color_0 = value;
			}
		}

		public ColorBlend ButtonHotBackground
		{
			get
			{
				return this.colorBlend_6;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_6 = value;
			}
		}

		public ColorBlend ButtonHotInnerBorder
		{
			get
			{
				return this.colorBlend_5;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_5 = value;
			}
		}

		public ColorBlend ButtonHotOuterBorder
		{
			get
			{
				return this.colorBlend_4;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_4 = value;
			}
		}

		public ColorBlend ButtonPressedBackground
		{
			get
			{
				return this.colorBlend_9;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_9 = value;
			}
		}

		public ColorBlend ButtonPressedInnerBorder
		{
			get
			{
				return this.colorBlend_8;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_8 = value;
			}
		}

		public ColorBlend ButtonPressedOuterBorder
		{
			get
			{
				return this.colorBlend_7;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_7 = value;
			}
		}

		public Color CollapsedTabBorder
		{
			get
			{
				return this.color_6;
			}
			set
			{
				this.color_6 = value;
			}
		}

		public ColorBlend CollapsedTabHorizontalBackground
		{
			get
			{
				return this.colorBlend_10;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_10 = value;
			}
		}

		public ColorBlend CollapsedTabVerticalBackground
		{
			get
			{
				return this.colorBlend_11;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_11 = value;
			}
		}

		public Office2007ColorScheme ColorScheme
		{
			get
			{
				return this.office2007ColorScheme_0;
			}
			set
			{
				if (value != this.office2007ColorScheme_0)
				{
					this.office2007ColorScheme_0 = value;
					switch (this.office2007ColorScheme_0)
					{
					case Office2007ColorScheme.Blue:
						this.method_4();
						return;
					case Office2007ColorScheme.Silver:
						this.method_6();
						break;
					case Office2007ColorScheme.Black:
						this.method_5();
						return;
					default:
						return;
					}
				}
			}
		}

		public Color DockedWindowInnerBorder
		{
			get
			{
				return this.color_2;
			}
			set
			{
				this.color_2 = value;
			}
		}

		public Color DockedWindowOuterBorder
		{
			get
			{
				return this.color_1;
			}
			set
			{
				this.color_1 = value;
			}
		}

		public ColorBlend DocumentContainerBackground
		{
			get
			{
				return this.colorBlend_12;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_12 = value;
			}
		}

		public ColorBlend DocumentHotTabBackground
		{
			get
			{
				return this.colorBlend_14;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_14 = value;
			}
		}

		public Color DocumentHotTabInnerBorder
		{
			get
			{
				return this.color_11;
			}
			set
			{
				this.color_11 = value;
			}
		}

		public Color DocumentHotTabOuterBorder
		{
			get
			{
				return this.color_10;
			}
			set
			{
				this.color_10 = value;
			}
		}

		public ColorBlend DocumentNormalTabBackground
		{
			get
			{
				return this.colorBlend_13;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_13 = value;
			}
		}

		public Color DocumentNormalTabInnerBorder
		{
			get
			{
				return this.color_9;
			}
			set
			{
				this.color_9 = value;
			}
		}

		public Color DocumentNormalTabOuterBorder
		{
			get
			{
				return this.color_8;
			}
			set
			{
				this.color_8 = value;
			}
		}

		public ColorBlend DocumentSelectedTabBackground
		{
			get
			{
				return this.colorBlend_15;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_15 = value;
			}
		}

		public Color DocumentSelectedTabInnerBorder
		{
			get
			{
				return this.color_13;
			}
			set
			{
				this.color_13 = value;
			}
		}

		public Color DocumentSelectedTabOuterBorder
		{
			get
			{
				return this.color_12;
			}
			set
			{
				this.color_12 = value;
			}
		}

		public Color DocumentStripBorder
		{
			get
			{
				return this.color_7;
			}
			set
			{
				this.color_7 = value;
			}
		}

		protected internal override int DocumentTabExtra
		{
			get
			{
				return this.ImageSize.Width;
			}
		}

		protected internal override int DocumentTabSize
		{
			get
			{
				int num = Math.Max(Control.DefaultFont.Height, this.ImageSize.Height);
				return num + 5;
			}
		}

		protected internal override int DocumentTabStripSize
		{
			get
			{
				int num = Math.Max(Control.DefaultFont.Height, this.ImageSize.Height);
				return num + 7;
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
				this.method_1();
				base.ImageSize = value;
			}
		}

		public ColorBlend InactiveTitleBarBackground
		{
			get
			{
				return this.colorBlend_1;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_1 = value;
			}
		}

		public override bool ShouldDrawControlBorder
		{
			get
			{
				return false;
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
				if (this.boxModel_0 == null)
				{
					this.boxModel_0 = new BoxModel(0, 0, 0, 0, 0, 0, 0, 0, -1, 0);
				}
				return this.boxModel_0;
			}
		}

		public Color TabStripInnerBorder
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

		protected internal override BoxModel TabStripMetrics
		{
			get
			{
				if (this.boxModel_1 == null)
				{
					int height = Control.DefaultFont.Height;
					int num = Math.Max(height, this.ImageSize.Height);
					this.boxModel_1 = new BoxModel(0, num + 8, 0, 0, 0, 1, 0, 0, 0, 0);
				}
				return this.boxModel_1;
			}
		}

		public Color TabStripNormalTabForeground
		{
			get
			{
				return this.color_5;
			}
			set
			{
				this.color_5 = value;
			}
		}

		public Color TabStripOuterBorder
		{
			get
			{
				return this.color_3;
			}
			set
			{
				this.color_3 = value;
			}
		}

		public ColorBlend TabStripSelectedTabBackground
		{
			get
			{
				return this.colorBlend_2;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_2 = value;
			}
		}

		public ColorBlend TabStripSelectedTabBorder
		{
			get
			{
				return this.colorBlend_3;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.colorBlend_3 = value;
			}
		}

		protected internal override TabTextDisplayMode TabTextDisplay
		{
			get
			{
				return TabTextDisplayMode.AllTabs;
			}
		}

		protected TextFormatFlags TextFormat
		{
			get
			{
				return this.textFormatFlags_0;
			}
		}

		protected internal override BoxModel TitleBarMetrics
		{
			get
			{
				if (this.boxModel_2 == null)
				{
					this.boxModel_2 = new BoxModel(0, Control.DefaultFont.Height + 8, 0, 0, 0, 0, 0, 0, 0, 0);
				}
				return this.boxModel_2;
			}
		}

		private BoxModel boxModel_0;

		private BoxModel boxModel_1;

		private BoxModel boxModel_2;

		private ColorBlend colorBlend_0;

		private ColorBlend colorBlend_1;

		private ColorBlend colorBlend_10;

		private ColorBlend colorBlend_11;

		private ColorBlend colorBlend_12;

		private ColorBlend colorBlend_13;

		private ColorBlend colorBlend_14;

		private ColorBlend colorBlend_15;

		private ColorBlend colorBlend_2;

		private ColorBlend colorBlend_3;

		private ColorBlend colorBlend_4;

		private ColorBlend colorBlend_5;

		private ColorBlend colorBlend_6;

		private ColorBlend colorBlend_7;

		private ColorBlend colorBlend_8;

		private ColorBlend colorBlend_9;

		private Color color_0;

		private Color color_1;

		private Color color_10;

		private Color color_11;

		private Color color_12;

		private Color color_13;

		private Color color_2;

		private Color color_3;

		private Color color_4;

		private Color color_5;

		private Color color_6;

		private Color color_7;

		private Color color_8;

		private Color color_9;

		private int int_0;

		private Office2007ColorScheme office2007ColorScheme_0 = (Office2007ColorScheme)(-1);

		private TextFormatFlags textFormatFlags_0;
	}
}
