using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

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

    internal class RendererBaseConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string)) return base.ConvertFrom(context, culture, value);

            switch (value as string)
            {
                case "Everett":
                    return new EverettRenderer();
                case "Office 2003":
                    return new Office2003Renderer();
                case "Whidbey":
                    return new WhidbeyRenderer();
                case "Milborne":
                    return new MilborneRenderer();
                case "Office 2007":
                    return new Office2007Renderer();
            }
            return null;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return value is string ? value : value.ToString();
            if (destinationType != typeof(InstanceDescriptor))
                return base.ConvertTo(context, culture, value, destinationType);
            var constructor = value.GetType().GetConstructor(Type.EmptyTypes);
            return new InstanceDescriptor(constructor, null, true);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var list = new ArrayList();
            if (context.Instance is DockContainer)
            {
                list.Add("(default)");
            }
            list.Add("Everett");
            list.Add("Office 2003");
            list.Add("Whidbey");
            list.Add("Office 2007");
            return new StandardValuesCollection(list);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
    }

    [TypeConverter(typeof(RendererBaseConverter))]
    public abstract class RendererBase : ITabControlRenderer, IDisposable
    {
        public RendererBase()
        {
            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
            GetColorsFromSystem();
        }

        protected internal virtual Rectangle AdjustDockControlClientBounds(ControlLayoutSystem layoutSystem, DockControl control, Rectangle clientBounds) => clientBounds;

        public void Dispose()
        {
            SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
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
            var red = Convert.ToByte(color1.R + (color2.R - color1.R) * percentage);
            var green = Convert.ToByte(color1.G + (color2.G - color1.G) * percentage);
            var blue = Convert.ToByte(color1.B + (color2.B - color1.B) * percentage);
            var alpha = Convert.ToByte(color1.A + (color2.A - color1.A) * percentage);
            return Color.FromArgb(alpha, red, green, blue);
        }

        protected internal abstract Size MeasureDocumentStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state);

        public virtual Size MeasureTabControlTab(Graphics graphics, Image image, string text, Font font, DrawItemState state) => MeasureDocumentStripTab(graphics, image, text, font, state);

        protected internal abstract Size MeasureTabStripTab(Graphics graphics, Image image, string text, Font font, DrawItemState state);

        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
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
