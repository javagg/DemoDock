using System;

namespace TD.SandDock
{
	public class WindowMetaData
	{
		internal WindowMetaData()
		{
			this.class19_0 = new Class19();
			this.class18_0 = new Class18();
			this.class18_1 = new Class18();
		}

		internal void method_0(DateTime dateTime_1)
		{
			this.dateTime_0 = dateTime_1;
		}

		internal void method_1(ContainerDockLocation containerDockLocation_1)
		{
			this.containerDockLocation_0 = containerDockLocation_1;
		}

		internal void method_2(int int_1)
		{
			this.int_0 = int_1;
		}

		internal void method_3(DockSituation dockSituation_2)
		{
			this.dockSituation_0 = dockSituation_2;
		}

		internal void method_4(DockSituation dockSituation_2)
		{
			this.dockSituation_1 = dockSituation_2;
		}

		internal void method_5(Guid guid_1)
		{
			this.guid_0 = guid_1;
		}

		internal bool Boolean_0
		{
			get
			{
				return this.int_0 != -1;
			}
		}

		internal Class18 Class18_0
		{
			get
			{
				return this.class18_0;
			}
		}

		internal Class18 Class18_1
		{
			get
			{
				return this.class18_1;
			}
		}

		internal Class19 Class19_0
		{
			get
			{
				return this.class19_0;
			}
		}

		public int DockedContentSize
		{
			get
			{
				if (this.int_0 == -1)
				{
					return 200;
				}
				return this.int_0;
			}
		}

		public ContainerDockLocation LastFixedDockSide
		{
			get
			{
				return this.containerDockLocation_0;
			}
		}

		public DockSituation LastFixedDockSituation
		{
			get
			{
				return this.dockSituation_1;
			}
		}

		public Guid LastFloatingWindowGuid
		{
			get
			{
				return this.guid_0;
			}
		}

		public DateTime LastFocused
		{
			get
			{
				return this.dateTime_0;
			}
		}

		public DockSituation LastOpenDockSituation
		{
			get
			{
				return this.dockSituation_0;
			}
		}

		private Class18 class18_0;

		private Class18 class18_1;

		private Class19 class19_0;

		private ContainerDockLocation containerDockLocation_0 = ContainerDockLocation.Right;

		private DateTime dateTime_0 = DateTime.FromFileTime(0L);

		private DockSituation dockSituation_0;

		private DockSituation dockSituation_1;

		private Guid guid_0;

		private int int_0 = -1;
	}
}
