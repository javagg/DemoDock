using System;
using System.Drawing;

namespace TD.SandDock.Rendering
{
    public class BoxEdges
    {
        public BoxEdges()
        {
        }

        public BoxEdges(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Bottom { get; }

        public int Left { get; }

        public int Right { get; }

        public int Top { get; }
    }

    public class BoxModel
	{
		public BoxModel()
		{
            Margin = new BoxEdges();
            Padding = new BoxEdges();
		}

		public BoxModel(int width, int height, int paddingLeft, int paddingTop, int paddingRight, int paddingBottom, int marginLeft, int marginTop, int marginRight, int marginBottom)
		{
            Width = width;
            Height = height;
            Padding = new BoxEdges(paddingLeft, paddingTop, paddingRight, paddingBottom);
            Margin = new BoxEdges(marginLeft, marginTop, marginRight, marginBottom);
		}

		public Rectangle RemoveMargin(Rectangle source)
		{
			source.X += Margin.Left;
			source.Y += Margin.Top;
			source.Width -= Margin.Left + Margin.Right;
			source.Height -= Margin.Top + Margin.Bottom;
			return source;
		}

		public Rectangle RemovePadding(Rectangle source)
		{
			source.X += Padding.Left;
			source.Y += Padding.Top;
			source.Width -= Padding.Left + Padding.Right;
			source.Height -= Padding.Top + Padding.Bottom;
			return source;
		}

		public int ExtraHeight => Margin.Top + Margin.Bottom + Padding.Top + Padding.Bottom;

	    public int ExtraWidth => Margin.Left + Margin.Right + Padding.Left + Padding.Right;

	    public int Height { get; set; }

	    public Size InnerSize => new Size(Width - Margin.Left - Margin.Right, Height - Margin.Top - Margin.Bottom);

	    public BoxEdges Margin { get; set; }

	    public BoxEdges Padding { get; set; }

	    public int Width { get; set; }
	}
}
