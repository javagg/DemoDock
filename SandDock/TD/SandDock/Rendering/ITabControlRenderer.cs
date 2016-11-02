using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TD.SandDock.Rendering
{
    public enum SandDockButtonType
    {
        Close,
        Pin,
        ScrollLeft,
        ScrollRight,
        WindowPosition,
        ActiveFiles
    }

    public interface ITabControlRenderer
    {
        void DrawFakeTabControlBackgroundExtension(Graphics graphics, Rectangle bounds, Color backColor);

        void DrawTabControlButton(Graphics graphics, Rectangle bounds, SandDockButtonType buttonType, DrawItemState state);

        void DrawTabControlBackground(Graphics graphics, Rectangle bounds, Color backColor, bool client);

        void DrawTabControlTab(Graphics graphics, Rectangle bounds, Image image, string text, Font font, Color backColor, Color foreColor, DrawItemState state, bool drawSeparator);

        Size MeasureTabControlTab(Graphics graphics, Image image, string text, Font font, DrawItemState state);

        void DrawTabControlTabStripBackground(Graphics graphics, Rectangle bounds, Color backColor);

        void StartRenderSession(HotkeyPrefix tabHotKeys);

        void FinishRenderSession();

        bool ShouldDrawControlBorder { get; }

        bool ShouldDrawTabControlBackground { get; }

        Size TabControlPadding { get; }

        int TabControlTabExtra { get; }

        int TabControlTabHeight { get; }

        int TabControlTabStripHeight { get; }
    }
}
