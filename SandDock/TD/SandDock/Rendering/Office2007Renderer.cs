using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using TD.Util;

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
			ColorScheme = colorScheme;
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
		    using (var brush = new SolidBrush(Background))
		        graphics.FillRectangle(brush, bounds);
		}

		protected internal override void DrawCollapsedTab(Graphics graphics, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool vertical)
		{
			using (Brush brush = CreateLinearGradientBrush(bounds, vertical ? CollapsedTabVerticalBackground : CollapsedTabHorizontalBackground, vertical ? LinearGradientMode.Horizontal : LinearGradientMode.Vertical))
			{
			    if (dockSide != DockSide.Left && dockSide != DockSide.Right)
			    {
			        RenderHelper.smethod_6(graphics, bounds, dockSide, image, text, font, brush, Brushes.Black, CollapsedTabBorder, TabTextDisplay == TabTextDisplayMode.AllTabs);
			        return;
			    }
			    using (var bm = new Bitmap(image))
				{
					bm.RotateFlip(RotateFlipType.Rotate90FlipNone);
					RenderHelper.smethod_6(graphics, bounds, dockSide, bm, text, font, brush, Brushes.Black, CollapsedTabBorder, TabTextDisplay == TabTextDisplayMode.AllTabs);
				}
			}
		}

		protected internal override void DrawControlClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
			using (var pen = new Pen(DockedWindowOuterBorder))
			{
				graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
			}
		}

		protected internal override void DrawDockContainerBackground(Graphics graphics, DockContainer container, Rectangle bounds)
		{
		    if (bounds.Width <= 0 || bounds.Height <= 0) return;

		    if (!(container is DocumentContainer))
		    {
		        RenderHelper.ClearBackground(graphics, Background);
		    }
		    else
		    {
		        using (var brush = CreateLinearGradientBrush(bounds, DocumentContainerBackground, LinearGradientMode.Vertical))
		            graphics.FillRectangle(brush, bounds);
		    }
		}

		protected internal override void DrawDocumentClientBackground(Graphics graphics, Rectangle bounds, Color backColor)
		{
		    using (var brush = new SolidBrush(backColor))
		        graphics.FillRectangle(brush, bounds);
		    using (var pen = new Pen(DocumentStripBorder))
			{
				graphics.DrawLines(pen, new[]
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
		    if (bounds.Width <= 0 || bounds.Height <= 0) return;
		    using (var pen = new Pen(DocumentStripBorder))
		        graphics.DrawLine(pen, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
		}

		protected internal override void DrawDocumentStripButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
		{
			method_7(graphics, bounds, state, true);
			switch (buttonType)
			{
			case SandDockButtonType.Close:
				ButtonRenderHelper.DrawCloseDockButton(graphics, bounds, SystemPens.ControlText);
				return;
			case SandDockButtonType.Pin:
			case SandDockButtonType.WindowPosition:
				break;
			case SandDockButtonType.ScrollLeft:
				ButtonRenderHelper.DrawScrollLeftDockButton(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
				return;
			case SandDockButtonType.ScrollRight:
				ButtonRenderHelper.DrawScrollRightDockButton(graphics, bounds, SystemColors.ControlText, (state & DrawItemState.Disabled) != DrawItemState.Disabled);
				return;
			case SandDockButtonType.ActiveFiles:
				bounds.Inflate(1, 1);
				bounds.X--;
				ButtonRenderHelper.DrawPositionDockButton(graphics, bounds, SystemPens.ControlText);
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
					color = DocumentSelectedTabOuterBorder;
					color2 = DocumentSelectedTabInnerBorder;
					colorBlend_ = DocumentSelectedTabBackground;
				}
				else if ((state & DrawItemState.HotLight) != DrawItemState.HotLight)
				{
					color = DocumentNormalTabOuterBorder;
					color2 = DocumentNormalTabInnerBorder;
					colorBlend_ = DocumentNormalTabBackground;
				}
				else
				{
					color = DocumentHotTabOuterBorder;
					color2 = DocumentHotTabInnerBorder;
					colorBlend_ = DocumentHotTabBackground;
				}
				using (Brush brush = CreateLinearGradientBrush(bounds, colorBlend_, LinearGradientMode.Vertical))
				{
					using (Pen pen = new Pen(color))
					{
						using (Pen pen2 = new Pen(color2))
						{
							method_8(graphics, bounds, contentBounds, image, text, font, backColor, pen, pen2, brush, state, bool_, DocumentTabSize, DocumentTabExtra, TextFormat);
						}
					}
				}
			}
		}

		protected internal override void DrawSplitter(Control container, Control control, Graphics graphics, Rectangle bounds, Orientation orientation)
		{
			if (!(control is DocumentContainer))
			{
				using (SolidBrush solidBrush = new SolidBrush(Background))
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
				using (Pen pen = new Pen(DocumentStripBorder))
				{
					graphics.DrawLines(pen, new[]
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
			using (SolidBrush solidBrush = new SolidBrush(Background))
			{
				graphics.FillRectangle(solidBrush, bounds);
			}
			using (Pen pen = new Pen(TabStripInnerBorder))
			{
				graphics.DrawLine(pen, bounds.X, bounds.Top + 1, bounds.Right - 1, bounds.Top + 1);
			}
			using (Pen pen2 = new Pen(TabStripOuterBorder))
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
				using (LinearGradientBrush linearGradientBrush = CreateLinearGradientBrush(bounds, TabStripSelectedTabBackground, LinearGradientMode.Vertical))
				{
					RenderHelper.smethod_4(graphics, bounds, image, ImageSize, text, font, linearGradientBrush, SystemColors.ControlText, TabStripOuterBorder, state, TextFormat);
					return;
				}
			}
			RenderHelper.smethod_3(graphics, bounds, image, ImageSize, text, font, SystemInformation.HighContrast ? SystemColors.Control : backColor, SystemInformation.HighContrast ? SystemColors.Control : SystemColors.ControlLightLight, TabStripNormalTabForeground, TabStripOuterBorder, state, TextFormat);
		}

		protected internal override void DrawTitleBarBackground(Graphics graphics, Rectangle bounds, bool focused)
		{
			using (Pen pen = new Pen(DockedWindowOuterBorder))
			{
				graphics.DrawLines(pen, new[]
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
				using (LinearGradientBrush linearGradientBrush = CreateLinearGradientBrush(bounds, focused ? ActiveTitleBarBackground : InactiveTitleBarBackground, LinearGradientMode.Vertical))
				{
					graphics.FillRectangle(linearGradientBrush, bounds);
				}
			}
			using (Pen pen2 = new Pen(DockedWindowInnerBorder))
			{
				graphics.DrawLines(pen2, new[]
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
			method_7(graphics, bounds, state, focused);
			using (Pen pen = (!focused) ? new Pen(Color.Black) : new Pen(Color.Black))
			{
				switch (buttonType)
				{
				case SandDockButtonType.Close:
					ButtonRenderHelper.DrawCloseDockButton(graphics, bounds, pen);
					break;
				case SandDockButtonType.Pin:
					ButtonRenderHelper.DrawPinDockButton(graphics, bounds, pen, toggled);
					break;
				case SandDockButtonType.WindowPosition:
					ButtonRenderHelper.DrawPositionDockButton(graphics, bounds, pen);
					break;
				}
			}
		}

		protected internal override void DrawTitleBarText(Graphics graphics, Rectangle bounds, bool focused, string text, Font font)
		{
			bounds.Inflate(-3, 0);
			TextFormatFlags textFormatFlags = TextFormat;
			textFormatFlags |= TextFormatFlags.NoPrefix;
			bounds.X += 3;
			TextRenderer.DrawText(graphics, text, font, bounds, focused ? Color.Black : Color.Black, textFormatFlags);
		}

		public override void FinishRenderSession()
		{
			_sessionCount = Math.Max(_sessionCount - 1, 0);
		}

		protected internal override Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			TextFormatFlags textFormatFlags = TextFormat;
			textFormatFlags &= ~TextFormatFlags.NoPrefix;
			int num;
			using (Font font2 = new Font(font, FontStyle.Bold))
			{
				num = TextRenderer.MeasureText(graphics, text, font2, new Size(2147483647, 2147483647), textFormatFlags).Width;
			}
			num += 14;
			if (image != null)
			{
				num += ImageSize.Width + 4;
			}
			num += DocumentTabExtra;
			return new Size(num, 0);
		}

		protected internal override Size MeasureTabStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
		{
			return RenderHelper.MeasureTabStripTab(graphics, image, ImageSize, text, font, TextFormat);
		}

        [Naming]
		private void ClearMetrics()
		{
			_tabMetrics = null;
			_tabStripMetrics = null;
			_titleBarMetrics = null;
		}

		private ColorBlend CreateColorBlend(float[] floats, Color[] colors)
		{
			var colorBlend = new ColorBlend(floats.Length);
			for (var i = 0; i < floats.Length; i++)
			{
				colorBlend.Positions[i] = floats[i];
				colorBlend.Colors[i] = colors[i];
			}
			return colorBlend;
		}

		private LinearGradientBrush CreateLinearGradientBrush(Rectangle rect, ColorBlend colorBlend, LinearGradientMode linearGradientMode)
		{
			return new LinearGradientBrush(rect, Color.Black, Color.White, linearGradientMode)
			{
				InterpolationColors = colorBlend
			};
		}

        [Naming]
		private void ApplyBlueColorScheme()
		{
			Background = ColorTranslator.FromHtml("#BFDBFF");
			DockedWindowOuterBorder = ColorTranslator.FromHtml("#7596BF");
			DockedWindowInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			InactiveTitleBarBackground = CreateColorBlend(new[]
			{
				0f,
				0.35f,
				0.35f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#E4EBF6"),
				ColorTranslator.FromHtml("#D9E7F9"),
				ColorTranslator.FromHtml("#CADEF7"),
				ColorTranslator.FromHtml("#DBF4FE")
			});
			ActiveTitleBarBackground = CreateColorBlend(new[]
			{
				0f,
				0.7f,
				0.7f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFCDA"),
				ColorTranslator.FromHtml("#FFE790"),
				ColorTranslator.FromHtml("#FFD74C"),
				ColorTranslator.FromHtml("#FFD346")
			});
			TabStripOuterBorder = ColorTranslator.FromHtml("#7596BF");
			TabStripInnerBorder = ColorTranslator.FromHtml("#E7EFF8");
			TabStripSelectedTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F7FBFF"),
				ColorTranslator.FromHtml("#EEF5FB"),
				ColorTranslator.FromHtml("#E1EAF6"),
				ColorTranslator.FromHtml("#F7FBFF")
			});
			TabStripSelectedTabBorder = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.7f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#E1EAF6"),
				ColorTranslator.FromHtml("#CDFBFF"),
				ColorTranslator.FromHtml("#D0FBFF"),
				ColorTranslator.FromHtml("#F4F9FF")
			});
			TabStripNormalTabForeground = ColorTranslator.FromHtml("#15428B");
			ButtonHotOuterBorder = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#DBCE99"),
				ColorTranslator.FromHtml("#B9A074"),
				ColorTranslator.FromHtml("#CBC3AA")
			});
			ButtonHotInnerBorder = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFFFB"),
				ColorTranslator.FromHtml("#FFF9E3"),
				ColorTranslator.FromHtml("#FFF2C9"),
				ColorTranslator.FromHtml("#FFFCDF")
			});
			ButtonHotBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFCE6"),
				ColorTranslator.FromHtml("#FFECA3"),
				ColorTranslator.FromHtml("#FFD844"),
				ColorTranslator.FromHtml("#FFE47F")
			});
			ButtonPressedOuterBorder = CreateColorBlend(new[]
			{
				0f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#7B6645"),
				ColorTranslator.FromHtml("#7B6645")
			});
			ButtonPressedInnerBorder = CreateColorBlend(new[]
			{
				0f,
				0.1f,
				0.6f,
				0.6f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#B2855C"),
				ColorTranslator.FromHtml("#F1B072"),
				ColorTranslator.FromHtml("#F1963B"),
				ColorTranslator.FromHtml("#ED7804"),
				ColorTranslator.FromHtml("#FDAD03")
			});
			ButtonPressedBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F3A570"),
				ColorTranslator.FromHtml("#E57840"),
				ColorTranslator.FromHtml("#DE550A"),
				ColorTranslator.FromHtml("#FEA14E")
			});
			CollapsedTabBorder = ColorTranslator.FromHtml("#7596BF");
			CollapsedTabHorizontalBackground = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F7FBFF"),
				ColorTranslator.FromHtml("#EEF5FB"),
				ColorTranslator.FromHtml("#E1EAF6"),
				ColorTranslator.FromHtml("#F7FBFF")
			});
			CollapsedTabVerticalBackground = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F7FBFF"),
				ColorTranslator.FromHtml("#EEF5FB"),
				ColorTranslator.FromHtml("#E1EAF6"),
				ColorTranslator.FromHtml("#F7FBFF")
			});
			DocumentContainerBackground = CreateColorBlend(new[]
			{
				0f,
				0.7f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#A3C2EA"),
				ColorTranslator.FromHtml("#567DB0"),
				ColorTranslator.FromHtml("#6591CD")
			});
			DocumentStripBorder = ColorTranslator.FromHtml("#678CBD");
			DocumentNormalTabOuterBorder = ColorTranslator.FromHtml("#6593CF");
			DocumentNormalTabInnerBorder = ColorTranslator.FromHtml("#E3EFFF");
			DocumentNormalTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#BEDAFF"),
				ColorTranslator.FromHtml("#AED2FF"),
				ColorTranslator.FromHtml("#8FBCF6"),
				ColorTranslator.FromHtml("#98C4FD")
			});
			DocumentHotTabOuterBorder = ColorTranslator.FromHtml("#6593CF");
			DocumentHotTabInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			DocumentHotTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#E1EEFF"),
				ColorTranslator.FromHtml("#D7E8FF"),
				ColorTranslator.FromHtml("#AED2FF"),
				ColorTranslator.FromHtml("#BEDAFF")
			});
			DocumentSelectedTabOuterBorder = ColorTranslator.FromHtml("#95774A");
			DocumentSelectedTabInnerBorder = ColorTranslator.FromHtml("#CDB69C");
			DocumentSelectedTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.25f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFD19C"),
				ColorTranslator.FromHtml("#FFDBB3"),
				ColorTranslator.FromHtml("#FFFFFE")
			});
		}

        [Naming]
        private void ApplyBlackColorScheme()
		{
			Background = ColorTranslator.FromHtml("#535353");
			DockedWindowOuterBorder = ColorTranslator.FromHtml("#8C8E8F");
			DockedWindowInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			InactiveTitleBarBackground = CreateColorBlend(new[]
			{
				0f,
				0.35f,
				0.35f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#D7DADF"),
				ColorTranslator.FromHtml("#C1C6CF"),
				ColorTranslator.FromHtml("#B4BBC5"),
				ColorTranslator.FromHtml("#EBEBEB")
			});
			ActiveTitleBarBackground = CreateColorBlend(new[]
			{
				0f,
				0.7f,
				0.7f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFCDA"),
				ColorTranslator.FromHtml("#FFE790"),
				ColorTranslator.FromHtml("#FFD74C"),
				ColorTranslator.FromHtml("#FFD346")
			});
			TabStripOuterBorder = ColorTranslator.FromHtml("#BEBEBE");
			TabStripInnerBorder = ColorTranslator.FromHtml("#D7DADF");
			TabStripSelectedTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F0F0F0"),
				ColorTranslator.FromHtml("#E3E6E9"),
				ColorTranslator.FromHtml("#D6D9DE"),
				ColorTranslator.FromHtml("#F0F1F2")
			});
			TabStripSelectedTabBorder = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.7f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#D5DBDC"),
				ColorTranslator.FromHtml("#B8F6FC"),
				ColorTranslator.FromHtml("#B7F7FD"),
				ColorTranslator.FromHtml("#E8EDEF")
			});
			TabStripNormalTabForeground = ColorTranslator.FromHtml("#FFFFFF");
			ButtonHotOuterBorder = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#DBCE99"),
				ColorTranslator.FromHtml("#B9A074"),
				ColorTranslator.FromHtml("#CBC3AA")
			});
			ButtonHotInnerBorder = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFFFB"),
				ColorTranslator.FromHtml("#FFF9E3"),
				ColorTranslator.FromHtml("#FFF2C9"),
				ColorTranslator.FromHtml("#FFFCDF")
			});
			ButtonHotBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFCE6"),
				ColorTranslator.FromHtml("#FFECA3"),
				ColorTranslator.FromHtml("#FFD844"),
				ColorTranslator.FromHtml("#FFE47F")
			});
			ButtonPressedOuterBorder = CreateColorBlend(new[]
			{
				0f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#7B6645"),
				ColorTranslator.FromHtml("#7B6645")
			});
			ButtonPressedInnerBorder = CreateColorBlend(new[]
			{
				0f,
				0.1f,
				0.6f,
				0.6f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#B2855C"),
				ColorTranslator.FromHtml("#F1B072"),
				ColorTranslator.FromHtml("#F1963B"),
				ColorTranslator.FromHtml("#ED7804"),
				ColorTranslator.FromHtml("#FDAD03")
			});
			ButtonPressedBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F3A570"),
				ColorTranslator.FromHtml("#E57840"),
				ColorTranslator.FromHtml("#DE550A"),
				ColorTranslator.FromHtml("#FEA14E")
			});
			CollapsedTabBorder = ColorTranslator.FromHtml("#BEBEBE");
			CollapsedTabHorizontalBackground = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F0F0F0"),
				ColorTranslator.FromHtml("#E3E6E9"),
				ColorTranslator.FromHtml("#D6D9DE"),
				ColorTranslator.FromHtml("#F0F1F2")
			});
			CollapsedTabVerticalBackground = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F0F0F0"),
				ColorTranslator.FromHtml("#E3E6E9"),
				ColorTranslator.FromHtml("#D6D9DE"),
				ColorTranslator.FromHtml("#F0F1F2")
			});
			DocumentContainerBackground = CreateColorBlend(new[]
			{
				0f,
				0.7f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#4F4F4F"),
				ColorTranslator.FromHtml("#3B3B3B"),
				ColorTranslator.FromHtml("#0A0A0A")
			});
			DocumentStripBorder = ColorTranslator.FromHtml("#000000");
			DocumentNormalTabOuterBorder = ColorTranslator.FromHtml("#9199A4");
			DocumentNormalTabInnerBorder = ColorTranslator.FromHtml("#F0F1F2");
			DocumentNormalTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#DBDEE1"),
				ColorTranslator.FromHtml("#D3D6DB"),
				ColorTranslator.FromHtml("#BCC1C8"),
				ColorTranslator.FromHtml("#C5C9CF")
			});
			DocumentHotTabOuterBorder = ColorTranslator.FromHtml("#616A76");
			DocumentHotTabInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			DocumentHotTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F2F2F3"),
				ColorTranslator.FromHtml("#F8F8F9"),
				ColorTranslator.FromHtml("#D3D6DB"),
				ColorTranslator.FromHtml("#DBDEE1")
			});
			DocumentSelectedTabOuterBorder = ColorTranslator.FromHtml("#3D3D3D");
			DocumentSelectedTabInnerBorder = ColorTranslator.FromHtml("#CDB69C");
			DocumentSelectedTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.25f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFD19C"),
				ColorTranslator.FromHtml("#FFDBB3"),
				ColorTranslator.FromHtml("#FFFFFE")
			});
		}

        [Naming]
        private void ApplySilverColorScheme()
		{
			Background = ColorTranslator.FromHtml("#D0D4DD");
			DockedWindowOuterBorder = ColorTranslator.FromHtml("#BDBFC1");
			DockedWindowInnerBorder = ColorTranslator.FromHtml("#FFFFFF");
			InactiveTitleBarBackground = CreateColorBlend(new[]
			{
				0f,
				0.35f,
				0.35f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F2F4F8"),
				ColorTranslator.FromHtml("#E1E6EE"),
				ColorTranslator.FromHtml("#D5DBE7"),
				ColorTranslator.FromHtml("#F9F9F9")
			});
			ActiveTitleBarBackground = CreateColorBlend(new[]
			{
				0f,
				0.7f,
				0.7f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFCDA"),
				ColorTranslator.FromHtml("#FFE790"),
				ColorTranslator.FromHtml("#FFD74C"),
				ColorTranslator.FromHtml("#FFD346")
			});
			TabStripOuterBorder = ColorTranslator.FromHtml("#838383");
			TabStripInnerBorder = ColorTranslator.FromHtml("#F2F4F8");
			TabStripSelectedTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFFFF"),
				ColorTranslator.FromHtml("#F7F6F8"),
				ColorTranslator.FromHtml("#EEF1F5"),
				ColorTranslator.FromHtml("#F2F7F9")
			});
			TabStripSelectedTabBorder = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.7f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#EAEFF5"),
				ColorTranslator.FromHtml("#C1FAFF"),
				ColorTranslator.FromHtml("#C6FAFF"),
				ColorTranslator.FromHtml("#ECFAFB")
			});
			TabStripNormalTabForeground = ColorTranslator.FromHtml("#4C535C");
			ButtonHotOuterBorder = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#DBCE99"),
				ColorTranslator.FromHtml("#B9A074"),
				ColorTranslator.FromHtml("#CBC3AA")
			});
			ButtonHotInnerBorder = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFFFB"),
				ColorTranslator.FromHtml("#FFF9E3"),
				ColorTranslator.FromHtml("#FFF2C9"),
				ColorTranslator.FromHtml("#FFFCDF")
			});
			ButtonHotBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFCE6"),
				ColorTranslator.FromHtml("#FFECA3"),
				ColorTranslator.FromHtml("#FFD844"),
				ColorTranslator.FromHtml("#FFE47F")
			});
			ButtonPressedOuterBorder = CreateColorBlend(new[]
			{
				0f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#7B6645"),
				ColorTranslator.FromHtml("#7B6645")
			});
			ButtonPressedInnerBorder = CreateColorBlend(new[]
			{
				0f,
				0.1f,
				0.6f,
				0.6f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#B2855C"),
				ColorTranslator.FromHtml("#F1B072"),
				ColorTranslator.FromHtml("#F1963B"),
				ColorTranslator.FromHtml("#ED7804"),
				ColorTranslator.FromHtml("#FDAD03")
			});
			ButtonPressedBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#F3A570"),
				ColorTranslator.FromHtml("#E57840"),
				ColorTranslator.FromHtml("#DE550A"),
				ColorTranslator.FromHtml("#FEA14E")
			});
			CollapsedTabBorder = ColorTranslator.FromHtml("#838383");
			CollapsedTabHorizontalBackground = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFFFF"),
				ColorTranslator.FromHtml("#F7F6F8"),
				ColorTranslator.FromHtml("#EEF1F5"),
				ColorTranslator.FromHtml("#F2F7F9")
			});
			CollapsedTabVerticalBackground = CreateColorBlend(new[]
			{
				0f,
				0.3f,
				0.3f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFFFFF"),
				ColorTranslator.FromHtml("#F7F6F8"),
				ColorTranslator.FromHtml("#EEF1F5"),
				ColorTranslator.FromHtml("#F2F7F9")
			});
			DocumentContainerBackground = CreateColorBlend(new[]
			{
				0f,
				0.7f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#CCCFD8"),
				ColorTranslator.FromHtml("#BDC0C9"),
				ColorTranslator.FromHtml("#9B9FA6")
			});
			DocumentStripBorder = ColorTranslator.FromHtml("#858585");
			DocumentNormalTabOuterBorder = ColorTranslator.FromHtml("#6F7074");
			DocumentNormalTabInnerBorder = ColorTranslator.FromHtml("#EDF3F4");
			DocumentNormalTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#DCE0E5"),
				ColorTranslator.FromHtml("#D8DDE2"),
				ColorTranslator.FromHtml("#B5BAC3"),
				ColorTranslator.FromHtml("#C6CBD1")
			});
			DocumentHotTabOuterBorder = ColorTranslator.FromHtml("#6F7074");
			DocumentHotTabInnerBorder = ColorTranslator.FromHtml("#EDF3F4");
			DocumentHotTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.5f,
				0.5f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FBFBFB"),
				ColorTranslator.FromHtml("#F1F1F2"),
				ColorTranslator.FromHtml("#CFD3D6"),
				ColorTranslator.FromHtml("#DEE0E3")
			});
			DocumentSelectedTabOuterBorder = ColorTranslator.FromHtml("#95774A");
			DocumentSelectedTabInnerBorder = ColorTranslator.FromHtml("#CDB69C");
			DocumentSelectedTabBackground = CreateColorBlend(new[]
			{
				0f,
				0.25f,
				1f
			}, new[]
			{
				ColorTranslator.FromHtml("#FFD19C"),
				ColorTranslator.FromHtml("#FFDBB3"),
				ColorTranslator.FromHtml("#FFFFFE")
			});
		}

		private void method_7(Graphics g, Rectangle bounds, DrawItemState state, bool bool_1)
		{
		    if ((state & DrawItemState.HotLight) != DrawItemState.HotLight) return;
		    var selected = (state & DrawItemState.Selected) == DrawItemState.Selected;
		    using (var brush = CreateLinearGradientBrush(bounds, selected ? ButtonPressedOuterBorder : ButtonHotOuterBorder, LinearGradientMode.Vertical))
		    using (var pen = new Pen(brush))
		    {
		        g.DrawPolygon(pen, new[]
		        {
		            new Point(bounds.X + 1, bounds.Y),
		            new Point(bounds.Right - 2, bounds.Y),
		            new Point(bounds.Right - 1, bounds.Y + 1),
		            new Point(bounds.Right - 1, bounds.Bottom - 2),
		            new Point(bounds.Right - 2, bounds.Bottom - 1),
		            new Point(bounds.X + 1, bounds.Bottom - 1),
		            new Point(bounds.X, bounds.Bottom - 2),
		            new Point(bounds.X, bounds.Y + 1)
		        });
		    }
		    using (var brush = CreateLinearGradientBrush(bounds, selected ? ButtonPressedInnerBorder : ButtonHotInnerBorder, LinearGradientMode.Vertical))
		    using (var pen2 = new Pen(brush))
		        g.DrawRectangle(pen2, bounds.X + 1, bounds.Y + 1, bounds.Width - 3, bounds.Height - 3);
		    using (var brush = CreateLinearGradientBrush(bounds, selected ? ButtonPressedBackground : ButtonHotBackground, LinearGradientMode.Vertical))
		        g.FillRectangle(brush, bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
		}

		private void method_8(Graphics g, Rectangle bounds, Rectangle rectangle_1, Image image, string text, Font font, Color color_14, Pen pen_0, Pen pen_1, Brush brush_0, DrawItemState state, bool bool_1, int int_1, int int_2, TextFormatFlags textFormat)
		{
			if ((state & DrawItemState.Selected) == DrawItemState.Selected)
			{
				bounds.Height++;
				int_1++;
			}
			g.DrawLine(pen_0, bounds.Left + 1, bounds.Bottom - 2, bounds.Left + int_1 - 3, bounds.Top + 2);
			g.DrawLine(pen_0, bounds.Left + int_1 - 3, bounds.Top + 2, bounds.Left + int_1 - 2, bounds.Top + 2);
			g.DrawLine(pen_0, bounds.Left + int_1 - 1, bounds.Top + 1, bounds.Left + int_1, bounds.Top + 1);
			g.DrawLine(pen_0, bounds.Left + int_1 + 1, bounds.Top, bounds.Right - 3, bounds.Top);
			g.DrawLine(pen_0, bounds.Right - 3, bounds.Top, bounds.Right - 1, bounds.Top + 2);
			g.DrawLine(pen_0, bounds.Right - 1, bounds.Top + 2, bounds.Right - 1, bounds.Bottom - 2);
			g.DrawLine(pen_1, bounds.Left + 2, bounds.Bottom - 2, bounds.Left + int_1 - 3, bounds.Top + 3);
			g.DrawLine(pen_1, bounds.Left + int_1 - 3, bounds.Top + 3, bounds.Left + int_1 - 2, bounds.Top + 3);
			g.DrawLine(pen_1, bounds.Left + int_1 - 1, bounds.Top + 2, bounds.Left + int_1, bounds.Top + 2);
			g.DrawLine(pen_1, bounds.Left + int_1 + 1, bounds.Top + 1, bounds.Right - 4, bounds.Top + 1);
			g.DrawLine(pen_1, bounds.Right - 3, bounds.Top + 1, bounds.Right - 2, bounds.Top + 2);
			g.DrawLine(pen_1, bounds.Right - 2, bounds.Top + 2, bounds.Right - 2, bounds.Bottom - 2);
			g.FillPolygon(brush_0, new[]
			{
				new Point(bounds.Left + 2, bounds.Bottom - 1),
				new Point(bounds.Left + int_1 - 3, bounds.Top + 4),
				new Point(bounds.Left + int_1 + 1, bounds.Top + 2),
				new Point(bounds.Right - 2, bounds.Top + 2),
				new Point(bounds.Right - 2, bounds.Bottom - 1)
			});
			bounds = rectangle_1;
			bounds.X += int_2;
			bounds.Width -= int_2;
			if (image != null)
			{
				g.DrawImage(image, bounds.X + 4, bounds.Y + 2, ImageSize.Width, ImageSize.Height);
				bounds.X += ImageSize.Width + 4;
				bounds.Width -= ImageSize.Width + 4;
			}
			if (bounds.Width > 8)
			{
				textFormat |= TextFormatFlags.HorizontalCenter;
				textFormat &= (TextFormatFlags)(-1);
				TextRenderer.DrawText(g, text, font, bounds, SystemColors.ControlText, textFormat);
			}
			if (bool_1)
			{
				Rectangle rectangle = bounds;
				rectangle.Inflate(-2, -2);
				rectangle.Height += 2;
				rectangle.X++;
				rectangle.Width--;
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		public override void ModifyDefaultWindowColors(DockControl window, ref Color backColor, ref Color borderColor)
		{
			borderColor = DockedWindowOuterBorder;
		}

		public override void StartRenderSession(HotkeyPrefix hotKeys)
		{
			_textFormat = (TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPadding);
			if (hotKeys != HotkeyPrefix.None)
			{
				if (hotKeys == HotkeyPrefix.Hide)
				{
					_textFormat |= TextFormatFlags.HidePrefix;
				}
			}
			else
			{
				_textFormat |= TextFormatFlags.NoPrefix;
			}
			_sessionCount++;
		}

		public override string ToString() => "Office 2007";

	    public ColorBlend ActiveTitleBarBackground
		{
			get
			{
				return _activeTitleBarBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _activeTitleBarBackground = value;
			}
		}

		public Color Background { get; set; }

	    public ColorBlend ButtonHotBackground
		{
			get
			{
				return _buttonHotBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _buttonHotBackground = value;
			}
		}

		public ColorBlend ButtonHotInnerBorder
		{
			get
			{
				return _buttonHotInnerBorder;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _buttonHotInnerBorder = value;
			}
		}

		public ColorBlend ButtonHotOuterBorder
		{
			get
			{
				return _buttonHotOuterBorder;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _buttonHotOuterBorder = value;
			}
		}

		public ColorBlend ButtonPressedBackground
		{
			get
			{
				return _buttonPressedBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _buttonPressedBackground = value;
			}
		}

		public ColorBlend ButtonPressedInnerBorder
		{
			get
			{
				return _buttonPressedInnerBorder;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _buttonPressedInnerBorder = value;
			}
		}

		public ColorBlend ButtonPressedOuterBorder
		{
			get
			{
				return _buttonPressedOuterBorder;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _buttonPressedOuterBorder = value;
			}
		}

		public Color CollapsedTabBorder { get; set; }

	    public ColorBlend CollapsedTabHorizontalBackground
		{
			get
			{
				return _collapsedTabHorizontalBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _collapsedTabHorizontalBackground = value;
			}
		}

		public ColorBlend CollapsedTabVerticalBackground
		{
			get
			{
				return _collapsedTabVerticalBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _collapsedTabVerticalBackground = value;
			}
		}

		public Office2007ColorScheme ColorScheme
		{
			get
			{
				return _colorScheme;
			}
			set
			{
			    if (value == _colorScheme) return;
			    _colorScheme = value;
			    switch (_colorScheme)
			    {
			        case Office2007ColorScheme.Blue:
			            ApplyBlueColorScheme();
			            return;
			        case Office2007ColorScheme.Silver:
			            ApplySilverColorScheme();
			            break;
			        case Office2007ColorScheme.Black:
			            ApplyBlackColorScheme();
			            return;
			        default:
			            return;
			    }
			}
		}

		public Color DockedWindowInnerBorder { get; set; }

	    public Color DockedWindowOuterBorder { get; set; }

	    public ColorBlend DocumentContainerBackground
		{
			get
			{
				return _documentContainerBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _documentContainerBackground = value;
			}
		}

		public ColorBlend DocumentHotTabBackground
		{
			get
			{
				return _documentHotTabBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _documentHotTabBackground = value;
			}
		}

		public Color DocumentHotTabInnerBorder { get; set; }

	    public Color DocumentHotTabOuterBorder { get; set; }

	    public ColorBlend DocumentNormalTabBackground
		{
			get
			{
				return _documentNormalTabBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _documentNormalTabBackground = value;
			}
		}

		public Color DocumentNormalTabInnerBorder { get; set; }

	    public Color DocumentNormalTabOuterBorder { get; set; }

	    public ColorBlend DocumentSelectedTabBackground
		{
			get
			{
				return _documentSelectedTabBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _documentSelectedTabBackground = value;
			}
		}

		public Color DocumentSelectedTabInnerBorder { get; set; }

	    public Color DocumentSelectedTabOuterBorder { get; set; }

	    public Color DocumentStripBorder { get; set; }

	    protected internal override int DocumentTabExtra => ImageSize.Width;

	    protected internal override int DocumentTabSize => Math.Max(Control.DefaultFont.Height, ImageSize.Height) + 5;

	    protected internal override int DocumentTabStripSize => Math.Max(Control.DefaultFont.Height, ImageSize.Height) + 7;

	    public override Size ImageSize
		{
			get
			{
				return base.ImageSize;
			}
			set
			{
				ClearMetrics();
				base.ImageSize = value;
			}
		}

		public ColorBlend InactiveTitleBarBackground
		{
			get
			{
				return _inactiveTitleBarBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _inactiveTitleBarBackground = value;
			}
		}

		public override bool ShouldDrawControlBorder => false;

	    public override Size TabControlPadding => new Size(3, 3);

	    protected internal override BoxModel TabMetrics => _tabMetrics ?? (_tabMetrics = new BoxModel(0, 0, 0, 0, 0, 0, 0, 0, -1, 0));

	    public Color TabStripInnerBorder { get; set; }

	    protected internal override BoxModel TabStripMetrics
		{
			get
			{
			    if (_tabStripMetrics != null) return _tabStripMetrics;
	            _tabStripMetrics = new BoxModel(0, Math.Max(Control.DefaultFont.Height, ImageSize.Height) + 8, 0, 0, 0, 1, 0, 0, 0, 0);
			    return _tabStripMetrics;
			}
		}

		public Color TabStripNormalTabForeground { get; set; }

	    public Color TabStripOuterBorder { get; set; }

	    public ColorBlend TabStripSelectedTabBackground
		{
			get
			{
				return _tabStripSelectedTabBackground;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _tabStripSelectedTabBackground = value;
			}
		}

		public ColorBlend TabStripSelectedTabBorder
		{
			get
			{
				return _tabStripSelectedTabBorder;
			}
			set
			{
			    if (value == null) throw new ArgumentNullException(nameof(value));
			    _tabStripSelectedTabBorder = value;
			}
		}

		protected internal override TabTextDisplayMode TabTextDisplay => TabTextDisplayMode.AllTabs;

	    protected TextFormatFlags TextFormat => _textFormat;

	    protected internal override BoxModel TitleBarMetrics => _titleBarMetrics ?? (_titleBarMetrics = new BoxModel(0, Control.DefaultFont.Height + 8, 0, 0, 0, 0, 0, 0, 0, 0));

	    private BoxModel _tabMetrics;

		private BoxModel _tabStripMetrics;

		private BoxModel _titleBarMetrics;

		private ColorBlend _activeTitleBarBackground;

		private ColorBlend _inactiveTitleBarBackground;

		private ColorBlend _collapsedTabHorizontalBackground;

		private ColorBlend _collapsedTabVerticalBackground;

		private ColorBlend _documentContainerBackground;

		private ColorBlend _documentNormalTabBackground;

		private ColorBlend _documentHotTabBackground;

		private ColorBlend _documentSelectedTabBackground;

		private ColorBlend _tabStripSelectedTabBackground;

		private ColorBlend _tabStripSelectedTabBorder;

		private ColorBlend _buttonHotOuterBorder;

		private ColorBlend _buttonHotInnerBorder;

		private ColorBlend _buttonHotBackground;

		private ColorBlend _buttonPressedOuterBorder;

		private ColorBlend _buttonPressedInnerBorder;

		private ColorBlend _buttonPressedBackground;

	    private int _sessionCount;

		private Office2007ColorScheme _colorScheme = (Office2007ColorScheme)(-1);

		private TextFormatFlags _textFormat;
	}
}
