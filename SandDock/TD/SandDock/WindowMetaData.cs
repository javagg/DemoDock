using System;

namespace TD.SandDock
{
    public class WindowMetaData
	{
		internal WindowMetaData()
		{
		}

		internal void SaveFocused(DateTime dateTime)
		{
			LastFocused = dateTime;
		}

		internal void SaveFixedDockSide(ContainerDockLocation location)
		{
			LastFixedDockSide = location;
		}

		internal void SaveDockedContentSize(int size)
		{
			_dockedContentSize = size;
		}

		internal void SaveOpenDockSituation(DockSituation situation)
		{
			LastOpenDockSituation = situation;
		}

		internal void SaveFixedDockSituation(DockSituation situation)
		{
			LastFixedDockSituation = situation;
		}

		internal void SaveFloatingWindowGuid(Guid guid)
		{
			LastFloatingWindowGuid = guid;
		}

		internal bool Boolean_0 => _dockedContentSize != -1;

	    internal Class18 Class18_0 { get; } = new Class18();

	    internal Class18 Class18_1 { get; } = new Class18();

	    internal Class19 Class19_0 { get; } = new Class19();

	    public int DockedContentSize => _dockedContentSize != -1 ? _dockedContentSize : 200;

	    public ContainerDockLocation LastFixedDockSide { get; private set; } = ContainerDockLocation.Right;

	    public DockSituation LastFixedDockSituation { get; private set; }

	    public Guid LastFloatingWindowGuid { get; private set; }

	    public DateTime LastFocused { get; private set; } = DateTime.FromFileTime(0);

	    public DockSituation LastOpenDockSituation { get; private set; }

	    private int _dockedContentSize = -1;
	}
}
