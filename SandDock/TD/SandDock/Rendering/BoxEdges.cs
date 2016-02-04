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
			this.int_0 = left;
			this.int_1 = top;
			this.int_3 = right;
			this.int_2 = bottom;
		}

		public int Bottom
		{
			get
			{
				return this.int_2;
			}
		}

		public int Left
		{
			get
			{
				return this.int_0;
			}
		}

		public int Right
		{
			get
			{
				return this.int_3;
			}
		}

		public int Top
		{
			get
			{
				return this.int_1;
			}
		}

		private int int_0;

		private int int_1;

		private int int_2;

		private int int_3;
	}
}
