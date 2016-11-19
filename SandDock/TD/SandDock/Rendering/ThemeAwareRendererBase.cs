using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
    internal class WindowsThemeHelper
    {
        private WindowsThemeHelper()
        {
        }

        public static bool HasLunaMsStyles
        {
            get
            {
                var text = ThemeFileName;
                text = Path.GetFileName(text)?.ToLower();
                return text == "luna.msstyles";
            }
        }

        public static string ThemeFileName
        {
            get
            {
                var sb = new StringBuilder(512);
                Native.GetCurrentThemeName(sb, sb.Capacity, null, 0, null, 0);
                return sb.ToString();
            }
        }

        public static string ThemeColorName
        {
            get
            {
                var sb = new StringBuilder(512);
                Native.GetCurrentThemeName(null, 0, sb, sb.Capacity, null, 0);
                return sb.ToString();
            }
        }
    }

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
            method_1(container, autoHideBar, graphics, bounds);
        }

        protected internal override void DrawSplitter(Control container, Control control, Graphics graphics, Rectangle bounds, Orientation orientation)
        {
            if (container != null)
                method_1(container, control, graphics, bounds);
        }

        protected internal override void DrawTabStripBackground(Control container, Control control, Graphics graphics, Rectangle bounds, int selectedTabOffset)
        {
            method_1(container, control, graphics, bounds);
        }

        public override void FinishRenderSession()
        {
            _sessionCount = Math.Max(_sessionCount - 1, 0);
        }

        protected override void GetColorsFromSystem()
        {
            switch (_colorScheme)
            {
                case WindowsColorScheme.Automatic:
                    var text = Native.IsMono() ? "none" : WhidbeyRenderer.IsSupported() && WindowsThemeHelper.HasLunaMsStyles ? WindowsThemeHelper.ThemeColorName : "none";
                    switch (text ?? "")
                    {
                        case "NormalColor":
                            ApplyLunaBlueColors();
                            break;
                        case "HomeStead":
                            ApplyLunaOliveColors();
                            break;
                        case "Metallic":
                            ApplyLunaSilverColors();
                            break;
                        default:
                            ApplyStandardColors();
                            break;
                    }
                    break;
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
                default:
                    base.GetColorsFromSystem();
                    break;
            }
        }

        private void method_1(Control container, Control control, Graphics g, Rectangle bounds)
        {
            if (container.ClientRectangle.Width > 0 && container.ClientRectangle.Height > 0 && bounds.Width > 0 && bounds.Height > 0)
            {
                var p1 = control.PointToClient(container.PointToScreen(new Point(0, 0)));
                var p2 = control.PointToClient(container.PointToScreen(new Point(container.ClientRectangle.Right, 0)));
                using (var brush = new LinearGradientBrush(p1, p2, LayoutBackgroundColor1, LayoutBackgroundColor2))
                    g.FillRectangle(brush, bounds);
            }
        }

        public override void StartRenderSession(HotkeyPrefix hotKeys)
        {
            TextFormat = TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPadding;
            TextFormat |= hotKeys != HotkeyPrefix.None && hotKeys == HotkeyPrefix.Hide ? TextFormatFlags.HidePrefix : TextFormatFlags.NoPrefix;
            _sessionCount++;
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

        private int _sessionCount;

        private WindowsColorScheme _colorScheme;
    }
}
