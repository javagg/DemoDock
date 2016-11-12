using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;

namespace TD.SandDock
{
	public sealed class LayoutUtilities
	{
		private LayoutUtilities()
		{
		}

		public static ControlLayoutSystem FindControlLayoutSystem(DockContainer container) => container.layoutSystems.OfType<ControlLayoutSystem>().FirstOrDefault();

	    internal static void Increase()
		{
			_cnt++;
		}

		internal static void Decrease()
		{
		    if (_cnt > 0) _cnt--;
		}

		internal static void smethod_10(ControlLayoutSystem layoutSystem)
		{
		    if (layoutSystem == null)
		        throw new ArgumentNullException();

		    var dockContainer = layoutSystem.DockContainer;
		    if (layoutSystem.AutoHideBar != null && layoutSystem.AutoHideBar.LayoutSystem == layoutSystem)
		        layoutSystem.AutoHideBar.method_6(true);

            if (layoutSystem.Parent == null) return;

		    layoutSystem.Parent.LayoutSystems.Remove(layoutSystem);
		    if (dockContainer != null && dockContainer.method_8() && (!(dockContainer is DocumentContainer) || dockContainer.Manager == null || !dockContainer.Manager.EnableEmptyEnvironment))
		        dockContainer.Dispose();
		}

		internal static void smethod_11(DockControl control)
		{
		    if (control == null)
		        throw new ArgumentNullException();
		    var layoutSystem = control.LayoutSystem;
		    if (layoutSystem == null) return;
		    var arg_1A_0 = layoutSystem.DockContainer;
			var containsFocus= control.ContainsFocus;
			if (containsFocus)
			{
                var form = control.FindForm();
			    if (form != null)
			        form.ActiveControl = null;
			}
			layoutSystem.Controls.Remove(control);
		    if (layoutSystem.Controls.Count == 0)
		        smethod_10(layoutSystem);
		    if (containsFocus && control.Manager != null)
			{
				var dockControl = control.Manager.FindMostRecentlyUsedWindow(DockSituation.Document, control) ??control.Manager.FindMostRecentlyUsedWindow((DockSituation)(-1), control);
			    dockControl?.method_12(true);
			}
		}

		internal static int smethod_12(DockContainer dockContainer_0)
		{
			int num = dockContainer_0.AllowResize ? 4 : 0;
			return num + smethod_13(dockContainer_0.LayoutSystem, dockContainer_0.Boolean_1 ? Orientation.Vertical : Orientation.Horizontal) * 5;
		}

		private static int smethod_13(SplitLayoutSystem splitLayoutSystem_0, Orientation splitMode)
		{
			int num = splitLayoutSystem_0.LayoutSystems.OfType<SplitLayoutSystem>().Select(splitLayoutSystem => smethod_13(splitLayoutSystem, splitMode)).Concat(new[] {0}).Max();
		    int num2 = num;
			if (splitLayoutSystem_0.LayoutSystems.Count > 1)
			{
				if (splitLayoutSystem_0.SplitMode == splitMode)
				{
					num2 += splitLayoutSystem_0.LayoutSystems.Count - 1;
				}
			}
			return num2;
		}

		internal static Struct0 smethod_14(SandDockManager sandDockManager_0, WindowMetaData windowMetaData_0)
		{
			DockContainer[] dockContainers = sandDockManager_0.GetDockContainers(LayoutUtilities.Convert(windowMetaData_0.LastFixedDockSide));
			if (dockContainers.Length == 0)
			{
				DockContainer dockContainer = sandDockManager_0.CreateNewDockContainer(windowMetaData_0.LastFixedDockSide, ContainerDockEdge.Inside, windowMetaData_0.DockedContentSize);
				return new Struct0(dockContainer.LayoutSystem, 0);
			}
			if (dockContainers.Length >= windowMetaData_0.Class19_0.Int32_3 && windowMetaData_0.Class19_0.Int32_2 < dockContainers.Length && windowMetaData_0.Class19_0.Int32_2 != -1)
			{
				return LayoutUtilities.smethod_15(dockContainers[windowMetaData_0.Class19_0.Int32_2], windowMetaData_0.Class19_0.Int32_0);
			}
			if (windowMetaData_0.Class19_0.Int32_3 >= 2)
			{
				if (windowMetaData_0.Class19_0.Int32_2 == 0)
				{
					DockContainer dockContainer2 = sandDockManager_0.CreateNewDockContainer(windowMetaData_0.LastFixedDockSide, ContainerDockEdge.Outside, windowMetaData_0.DockedContentSize);
					return new Struct0(dockContainer2.LayoutSystem, 0);
				}
				if (windowMetaData_0.Class19_0.Int32_2 == windowMetaData_0.Class19_0.Int32_3 - 1)
				{
					DockContainer dockContainer3 = sandDockManager_0.CreateNewDockContainer(windowMetaData_0.LastFixedDockSide, ContainerDockEdge.Inside, windowMetaData_0.DockedContentSize);
					return new Struct0(dockContainer3.LayoutSystem, 0);
				}
			}
			if (dockContainers.Length != 0)
			{
				return LayoutUtilities.smethod_15(dockContainers[0], windowMetaData_0.Class19_0.Int32_0);
			}
			DockContainer dockContainer4 = sandDockManager_0.CreateNewDockContainer(windowMetaData_0.LastFixedDockSide, ContainerDockEdge.Inside, windowMetaData_0.DockedContentSize);
			return new Struct0(dockContainer4.LayoutSystem, 0);
		}

		internal static Struct0 smethod_15(DockContainer dockContainer_0, int[] int_1)
		{
			SplitLayoutSystem splitLayoutSystem = dockContainer_0.LayoutSystem;
			int i = 0;
			while (i < int_1.Length)
			{
				int num = int_1[i];
				Struct0 result;
				if (num < splitLayoutSystem.LayoutSystems.Count)
				{
					SplitLayoutSystem splitLayoutSystem2 = splitLayoutSystem.LayoutSystems[num] as SplitLayoutSystem;
					if (splitLayoutSystem2 != null)
					{
						splitLayoutSystem = splitLayoutSystem2;
						i++;
						continue;
					}
					result = new Struct0(splitLayoutSystem, num);
				}
				else
				{
					result = new Struct0(splitLayoutSystem, splitLayoutSystem.LayoutSystems.Count);
				}
				return result;
			}
			return new Struct0(dockContainer_0.LayoutSystem, 0);
		}

		internal static DockSituation smethod_2(DockContainer container)
		{
		    if (container == null)
		        return DockSituation.None;
		    if (container.IsFloating)
		        return DockSituation.Floating;
		    return container.Dock == DockStyle.Fill ? DockSituation.Document : DockSituation.Docked;
		}

		internal static ControlLayoutSystem[] smethod_3(DockContainer container) => container.layoutSystems.OfType<ControlLayoutSystem>().ToArray();

	    internal static ControlLayoutSystem smethod_4(SandDockManager sandDockManager_0, DockSituation dockSituation_0, Class18 class18_0)
		{
			switch (dockSituation_0)
			{
			case DockSituation.None:
				IL_18:
				throw new InvalidOperationException();
			case DockSituation.Docked:
			{
				DockContainer[] dockContainers = sandDockManager_0.GetDockContainers();
				for (int i = 0; i < dockContainers.Length; i++)
				{
					DockContainer dockContainer_ = dockContainers[i];
					if (LayoutUtilities.smethod_2(dockContainer_) == dockSituation_0)
					{
						ControlLayoutSystem[] array = LayoutUtilities.smethod_3(dockContainer_);
						for (int j = 0; j < array.Length; j++)
						{
							ControlLayoutSystem controlLayoutSystem = array[j];
							if (controlLayoutSystem.Guid == class18_0.Guid_0)
							{
								ControlLayoutSystem result = controlLayoutSystem;
								return result;
							}
						}
					}
				}
				goto IL_133;
			}
			case DockSituation.Document:
				if (sandDockManager_0.DocumentContainer != null)
				{
					ControlLayoutSystem[] array2 = LayoutUtilities.smethod_3(sandDockManager_0.DocumentContainer);
					for (int k = 0; k < array2.Length; k++)
					{
						ControlLayoutSystem controlLayoutSystem2 = array2[k];
						if (controlLayoutSystem2.Guid == class18_0.Guid_0)
						{
							ControlLayoutSystem result = controlLayoutSystem2;
							return result;
						}
					}
					goto IL_133;
				}
				goto IL_133;
			case DockSituation.Floating:
			{
				DockContainer[] dockContainers2 = sandDockManager_0.GetDockContainers();
				foreach (DockContainer dockContainer_2 in dockContainers2)
				{
				    if (smethod_2(dockContainer_2) == dockSituation_0)
				    {
				        foreach (var controlLayoutSystem3 in smethod_3(dockContainer_2))
				        {
				            if (controlLayoutSystem3.Guid == class18_0.Guid_0)
				            {
				                ControlLayoutSystem result = controlLayoutSystem3;
				                return result;
				            }
				        }
				    }
				}
				goto IL_133;
			}
			}
			throw new InvalidOperationException();
			IL_133:
			return null;
		}

		internal static int[] smethod_5(ControlLayoutSystem controlLayoutSystem_0)
		{
			ArrayList arrayList = new ArrayList();
			for (LayoutSystemBase layoutSystemBase = controlLayoutSystem_0; layoutSystemBase != null; layoutSystemBase = layoutSystemBase.Parent)
			{
			    if (layoutSystemBase.Parent != null)
			        arrayList.Add(layoutSystemBase.Parent.LayoutSystems.IndexOf(layoutSystemBase));
			}
			arrayList.Reverse();
			return (int[])arrayList.ToArray(typeof(int));
		}

		internal static DockStyle Convert(ContainerDockLocation location)
		{
			switch (location)
			{
			case ContainerDockLocation.Left:
				return DockStyle.Left;
			case ContainerDockLocation.Right:
				return DockStyle.Right;
			case ContainerDockLocation.Top:
				return DockStyle.Top;
			case ContainerDockLocation.Bottom:
				return DockStyle.Bottom;
			}
			return DockStyle.Fill;
		}

		internal static ContainerDockLocation Convert(DockStyle style)
		{
			switch (style)
			{
			case DockStyle.Top:
				return ContainerDockLocation.Top;
			case DockStyle.Bottom:
				return ContainerDockLocation.Bottom;
			case DockStyle.Left:
				return ContainerDockLocation.Left;
			case DockStyle.Right:
				return ContainerDockLocation.Right;
			}
			return ContainerDockLocation.Center;
		}

		internal static void smethod_8(Control control_0)
		{
		    if (control_0.Parent == null)
		        return;
		    if (control_0.ContainsFocus)
			{
				control_0.Parent.Focus();
			}
		    var control = control_0 as DockControl;
		    if (control != null)
		        control.Boolean_0 = true;
		    try
			{
				IContainerControl containerControl = control_0.Parent.GetContainerControl();
				if (containerControl != null)
				{
					DockContainer dockContainer = containerControl as DockContainer;
					if (dockContainer != null && !dockContainer.Boolean_2 && dockContainer.Manager != null && dockContainer.Manager.OwnerForm != null && dockContainer.Manager.OwnerForm.IsMdiContainer)
					{
						LayoutUtilities.smethod_9(dockContainer, dockContainer.LayoutSystem);
					}
					else if (containerControl.ActiveControl == control_0)
					{
						containerControl.ActiveControl = null;
					}
				}
				control_0.Parent.Controls.Remove(control_0);
			}
			finally
			{
				if (control_0 is DockControl)
				{
					((DockControl)control_0).Boolean_0 = false;
				}
			}
		}

		private static bool smethod_9(DockContainer container, SplitLayoutSystem splitLayout)
		{
			IEnumerator enumerator = splitLayout.LayoutSystems.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					LayoutSystemBase layoutSystemBase = (LayoutSystemBase)enumerator.Current;
					if (!(layoutSystemBase is SplitLayoutSystem))
					{
						ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)layoutSystemBase;
						if (!controlLayoutSystem.Collapsed && container.Controls.Contains(controlLayoutSystem.SelectedControl) && controlLayoutSystem.SelectedControl.Visible)
						{
							if (controlLayoutSystem.SelectedControl.Enabled)
							{
								container.ActiveControl = controlLayoutSystem.SelectedControl;
								result = true;
								return result;
							}
						}
					}
					else if (LayoutUtilities.smethod_9(container, (SplitLayoutSystem)layoutSystemBase))
					{
						result = true;
						return result;
					}
				}
				return false;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		internal static bool Boolean_0 => _cnt > 0;

	    private static int _cnt;
	}
}
