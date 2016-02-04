using System;
using System.Drawing;

namespace TD.SandDock
{
	internal class Class18
	{
		internal Class18()
		{
			int[] array = new int[1];
			this.int_0 = array;
			this.sizeF_0 = new SizeF(250f, 400f);
		}

		public Guid Guid_0
		{
			get
			{
				return this.guid_0;
			}
			set
			{
				this.guid_0 = value;
			}
		}

		public int[] Int32_0
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

		public int Int32_1
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

		public SizeF SizeF_0
		{
			get
			{
				return this.sizeF_0;
			}
			set
			{
				this.sizeF_0 = value;
			}
		}

		private Guid guid_0;

		private int[] int_0;

		private int int_1;

		private SizeF sizeF_0;
	}
}
