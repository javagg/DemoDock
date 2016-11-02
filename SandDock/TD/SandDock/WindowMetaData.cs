using System;
using System.Drawing;

namespace TD.SandDock
{
    internal class Class18
    {
        internal Class18()
        {
            int[] array = new int[1];
            this.Int32_0 = array;
            this.SizeF_0 = new SizeF(250f, 400f);
        }

        public Guid Guid_0 { get; set; }

        public int[] Int32_0 { get; set; }

        public int Int32_1 { get; set; }

        public SizeF SizeF_0 { get; set; }
    }

    internal class Class19 : Class18
    {
        public int Int32_2 { get; set; }

        public int Int32_3 { get; set; }
    }

    public class WindowMetaData
	{
		internal WindowMetaData()
		{
		}

		internal void method_0(DateTime dateTime_1)
		{
			LastFocused = dateTime_1;
		}

		internal void method_1(ContainerDockLocation containerDockLocation_1)
		{
			LastFixedDockSide = containerDockLocation_1;
		}

		internal void method_2(int int_1)
		{
			this.int_0 = int_1;
		}

		internal void method_3(DockSituation dockSituation_2)
		{
			LastOpenDockSituation = dockSituation_2;
		}

		internal void method_4(DockSituation dockSituation_2)
		{
			LastFixedDockSituation = dockSituation_2;
		}

		internal void method_5(Guid guid)
		{
			LastFloatingWindowGuid = guid;
		}

		internal bool Boolean_0 => this.int_0 != -1;

	    internal Class18 Class18_0 { get; } = new Class18();

	    internal Class18 Class18_1 { get; } = new Class18();

	    internal Class19 Class19_0 { get; } = new Class19();

	    public int DockedContentSize => this.int_0 == -1 ? 200 : this.int_0;

	    public ContainerDockLocation LastFixedDockSide { get; private set; } = ContainerDockLocation.Right;

	    public DockSituation LastFixedDockSituation { get; private set; }

	    public Guid LastFloatingWindowGuid { get; private set; }

	    public DateTime LastFocused { get; private set; } = DateTime.FromFileTime(0);

	    public DockSituation LastOpenDockSituation { get; private set; }

	    private int int_0 = -1;
	}
}
