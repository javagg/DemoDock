using System;

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
}
