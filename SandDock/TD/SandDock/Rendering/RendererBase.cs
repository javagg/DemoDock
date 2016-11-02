using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
    public enum TabTextDisplayMode
    {
        AllTabs,
        SelectedTab
    }

    public enum BorderStyle
    {
        None,
        Flat,
        RaisedThick,
        RaisedThin,
        SunkenThick,
        SunkenThin
    }

    [TypeConverter(typeof(Class25))]
    public abstract class RendererBase : ITabControlRenderer, IDisposable
    {
        public RendererBase()
        {
            SystemEvents.UserPreferenceChanged += method_0;
            GetColorsFromSystem();
        }

        protected internal virtual Rectangle AdjustDockControlClientBounds(ControlLayoutSystem layoutSystem, DockControl control, Rectangle clientBounds)
        {
            return clientBounds;
        }

        public void Dispose()
        {
            SystemEvents.UserPreferenceChanged -= method_0;
        }

        protected internal abstract void DrawAutoHideBarBackground(Control container, Control control, Graphics graphics, Rectangle bounds);

        protected internal abstract void DrawCollapsedTab(Graphics graphics, Rectangle bounds, DockSide dockSide, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool vertical);

        protected internal abstract void DrawControlClientBackground(Graphics graphics, Rectangle bounds, Color backColor);

        protected internal abstract void DrawDockContainerBackground(Graphics graphics, DockContainer container, Rectangle bounds);

        protected internal abstract void DrawDocumentClientBackground(Graphics graphics, Rectangle bounds, Color backColor);

        protected internal abstract void DrawDocumentStripBackground(Graphics graphics, Rectangle bounds);

        protected internal abstract void DrawDocumentStripButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state);

        protected internal abstract void DrawDocumentStripTab(Graphics graphics, Rectangle bounds, Rectangle contentBounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator);

        public virtual void DrawFakeTabControlBackgroundExtension(Graphics graphics, Rectangle bounds, Color backColor)
        {
            using (var brush = new SolidBrush(backColor))
                graphics.FillRectangle(brush, bounds);
        }

        protected internal abstract void DrawSplitter(Control container, Control control, Graphics graphics, Rectangle bounds, Orientation orientation);

        public virtual void DrawTabControlBackground(Graphics graphics, Rectangle bounds, Color backColor, bool client)
        {
            using (var brush = new SolidBrush(backColor))
                graphics.FillRectangle(brush, bounds);
        }

        public virtual void DrawTabControlButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state)
        {
            DrawDocumentStripButton(graphics, bounds, buttonType, state);
        }

        public virtual void DrawTabControlTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator)
        {
            DrawDocumentStripTab(graphics, bounds, bounds, image, text, font, backColor, foreColor, state, drawSeparator);
        }

        public virtual void DrawTabControlTabStripBackground(Graphics graphics, Rectangle bounds, Color backColor)
        {
            DrawDocumentStripBackground(graphics, bounds);
        }

        protected internal abstract void DrawTabStripBackground(Control container, Control control, Graphics graphics, Rectangle bounds, int selectedTabOffset);

        protected internal abstract void DrawTabStripTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator);

        protected internal abstract void DrawTitleBarBackground(Graphics graphics, Rectangle bounds, bool focused);

        protected internal abstract void DrawTitleBarButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state, bool focused, bool toggled);

        protected internal abstract void DrawTitleBarText(Graphics graphics, Rectangle bounds, bool focused, string text, Font font);

        public abstract void FinishRenderSession();

        protected virtual void GetColorsFromSystem()
        {
            _customColors = false;
        }

        protected internal static Color InterpolateColors(Color color1, Color color2, float percentage)
        {
            int r = (int)color1.R;
            int g = (int)color1.G;
            int b = (int)color1.B;
            int a = (int)color1.A;
            int r2 = (int)color2.R;
            int g2 = (int)color2.G;
            int b2 = (int)color2.B;
            int a2 = (int)color2.A;
            byte red = Convert.ToByte(color1.R + (color2.R - color1.R) * percentage);
            byte green = Convert.ToByte((float)g + (float)(g2 - g) * percentage);
            byte blue = Convert.ToByte((float)b + (float)(b2 - b) * percentage);
            byte alpha = Convert.ToByte((float)a + (float)(a2 - a) * percentage);
            return Color.FromArgb((int)alpha, Convert.ToByte(color1.R + (color2.R - color1.R) * percentage), (int)green, (int)blue);
        }

        protected internal abstract Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state);

        public virtual Size MeasureTabControlTab(Graphics graphics, Image image, string text, Font font, DrawItemState state)
        {
            return MeasureDocumentStripTab(graphics, image, text, font, state);
        }

        protected internal abstract Size MeasureTabStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state);

        private void method_0(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Color && !_customColors)
            {
                GetColorsFromSystem();
                _customColors = false;
            }
        }

        public virtual void ModifyDefaultWindowColors(DockControl window, ref Color backColor, ref Color borderColor)
        {
        }

        protected virtual void OnMetricsChanged(EventArgs e)
        {
            MetricsChanged?.Invoke(this, e);
        }

        public abstract void StartRenderSession(HotkeyPrefix hotKeys);

        public bool CustomColors
        {
            get
            {
                return _customColors;
            }
            set
            {
                _customColors = value;
                if (!_customColors)
                    GetColorsFromSystem();
            }
        }

        protected internal abstract int DocumentTabExtra { get; }

        protected internal abstract int DocumentTabSize { get; }

        protected internal abstract int DocumentTabStripSize { get; }

        public virtual Size ImageSize
        {
            get
            {
                return _imageSize;
            }
            set
            {
                _imageSize = value;
                OnMetricsChanged(EventArgs.Empty);
            }
        }

        public virtual bool ShouldDrawControlBorder => true;

        public virtual bool ShouldDrawTabControlBackground => false;

        public abstract Size TabControlPadding { get; }

        public virtual int TabControlTabExtra => DocumentTabExtra;

        public virtual int TabControlTabHeight => DocumentTabSize;

        public virtual int TabControlTabStripHeight => DocumentTabStripSize;

        protected internal abstract BoxModel TabMetrics { get; }

        protected internal abstract BoxModel TabStripMetrics { get; }

        protected internal abstract TabTextDisplayMode TabTextDisplay { get; }

        protected internal abstract BoxModel TitleBarMetrics { get; }

        public event EventHandler MetricsChanged;

        private bool _customColors;

        private Size _imageSize = new Size(16, 16);
    }
}
