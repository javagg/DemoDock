using System;
using System.Drawing;

namespace TD.SandDock.Rendering
{
	public class BoxModel
	{
		public BoxModel()
		{
			this.boxEdges_0 = new BoxEdges();
			this.boxEdges_1 = new BoxEdges();
		}

		public BoxModel(int width, int height, int paddingLeft, int paddingTop, int paddingRight, int paddingBottom, int marginLeft, int marginTop, int marginRight, int marginBottom)
		{
			this.int_0 = width;
			this.int_1 = height;
			this.boxEdges_1 = new BoxEdges(paddingLeft, paddingTop, paddingRight, paddingBottom);
			this.boxEdges_0 = new BoxEdges(marginLeft, marginTop, marginRight, marginBottom);
		}

		public Rectangle RemoveMargin(Rectangle source)
		{
			source.X += this.boxEdges_0.Left;
			source.Y += this.boxEdges_0.Top;
			source.Width -= this.boxEdges_0.Left + this.boxEdges_0.Right;
			source.Height -= this.boxEdges_0.Top + this.boxEdges_0.Bottom;
			return source;
		}

		public Rectangle RemovePadding(Rectangle source)
		{
			source.X += this.boxEdges_1.Left;
			source.Y += this.boxEdges_1.Top;
			source.Width -= this.boxEdges_1.Left + this.boxEdges_1.Right;
			source.Height -= this.boxEdges_1.Top + this.boxEdges_1.Bottom;
			return source;
		}

		public int ExtraHeight
		{
			get
			{
				return this.boxEdges_0.Top + this.boxEdges_0.Bottom + this.boxEdges_1.Top + this.boxEdges_1.Bottom;
			}
		}

		public int ExtraWidth
		{
			get
			{
				return this.boxEdges_0.Left + this.boxEdges_0.Right + this.boxEdges_1.Left + this.boxEdges_1.Right;
			}
		}

		public int Height
		{
			get
			{
				return this.int_1;
			}
			set
			{
				this.int_1 = value;
			}
		}

		public Size InnerSize
		{
			get
			{
				return new Size(this.int_0 - this.boxEdges_0.Left - this.boxEdges_0.Right, this.int_1 - this.boxEdges_0.Top - this.boxEdges_0.Bottom);
			}
		}

		public BoxEdges Margin
		{
			get
			{
				return this.boxEdges_0;
			}
			set
			{
				this.boxEdges_0 = value;
			}
		}

		public BoxEdges Padding
		{
			get
			{
				return this.boxEdges_1;
			}
			set
			{
				this.boxEdges_1 = value;
			}
		}

		public int Width
		{
			get
			{
				return this.int_0;
			}
			set
			{
				this.int_0 = value;
			}
		}

		private BoxEdges boxEdges_0;

		private BoxEdges boxEdges_1;

		private int int_0;

		private int int_1;
	}
}
