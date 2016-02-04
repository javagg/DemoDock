using System;
using System.Collections;
using System.Windows.Forms;

namespace TD.SandDock
{
	public sealed class LayoutUtilities
	{
		private LayoutUtilities()
		{
		}

		public static ControlLayoutSystem FindControlLayoutSystem(DockContainer container)
		{
			IEnumerator enumerator = container.arrayList_0.GetEnumerator();
			ControlLayoutSystem result;
			try
			{
				while (enumerator.MoveNext())
				{
					LayoutSystemBase layoutSystemBase = (LayoutSystemBase)enumerator.Current;
					if (layoutSystemBase is ControlLayoutSystem)
					{
						result = (ControlLayoutSystem)layoutSystemBase;
						return result;
					}
				}
				goto IL_49;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return result;
			IL_49:
			return null;
		}

		internal static void smethod_0()
		{
			LayoutUtilities.int_0++;
		}

		internal static void smethod_1()
		{
			if (LayoutUtilities.int_0 > 0)
			{
				LayoutUtilities.int_0--;
			}
		}

		internal static void smethod_10(ControlLayoutSystem controlLayoutSystem_0)
		{
			if (controlLayoutSystem_0 == null)
			{
				throw new ArgumentNullException();
			}
			DockContainer dockContainer = controlLayoutSystem_0.DockContainer;
			if (controlLayoutSystem_0.Control0_0 != null)
			{
				if (controlLayoutSystem_0.Control0_0.ControlLayoutSystem_0 == controlLayoutSystem_0)
				{
					controlLayoutSystem_0.Control0_0.method_6(true);
				}
			}
			if (controlLayoutSystem_0.Parent != null)
			{
				controlLayoutSystem_0.Parent.LayoutSystems.Remove(controlLayoutSystem_0);
				if (dockContainer != null && dockContainer.method_8() && (!(dockContainer is DocumentContainer) || dockContainer.Manager == null || !dockContainer.Manager.EnableEmptyEnvironment))
				{
					dockContainer.Dispose();
				}
			}
		}

		internal static void smethod_11(DockControl dockControl_0)
		{
			if (dockControl_0 == null)
			{
				throw new ArgumentNullException();
			}
			ControlLayoutSystem layoutSystem = dockControl_0.LayoutSystem;
			if (layoutSystem == null)
			{
				return;
			}
			DockContainer arg_1A_0 = layoutSystem.DockContainer;
			bool containsFocus;
			if (containsFocus = dockControl_0.ContainsFocus)
			{
				Form form = dockControl_0.FindForm();
				if (form != null)
				{
					form.ActiveControl = null;
				}
			}
			layoutSystem.Controls.Remove(dockControl_0);
			if (layoutSystem.Controls.Count == 0)
			{
				LayoutUtilities.smethod_10(layoutSystem);
			}
			if (containsFocus && dockControl_0.Manager != null)
			{
				DockControl dockControl = dockControl_0.Manager.FindMostRecentlyUsedWindow(DockSituation.Document, dockControl_0);
				if (dockControl == null)
				{
					dockControl = dockControl_0.Manager.FindMostRecentlyUsedWindow((DockSituation)(-1), dockControl_0);
				}
				if (dockControl != null)
				{
					dockControl.method_12(true);
				}
			}
		}

		internal static int smethod_12(DockContainer dockContainer_0)
		{
			int num = dockContainer_0.AllowResize ? 4 : 0;
			return num + LayoutUtilities.smethod_13(dockContainer_0.LayoutSystem, dockContainer_0.Boolean_1 ? Orientation.Vertical : Orientation.Horizontal) * 5;
		}

		private static int smethod_13(SplitLayoutSystem splitLayoutSystem_0, Orientation orientation_0)
		{
			int num = 0;
			foreach (LayoutSystemBase layoutSystemBase in splitLayoutSystem_0.LayoutSystems)
			{
				SplitLayoutSystem splitLayoutSystem = layoutSystemBase as SplitLayoutSystem;
				if (splitLayoutSystem != null)
				{
					num = Math.Max(num, LayoutUtilities.smethod_13(splitLayoutSystem, orientation_0));
				}
			}
			int num2 = num;
			if (splitLayoutSystem_0.LayoutSystems.Count > 1)
			{
				if (splitLayoutSystem_0.SplitMode == orientation_0)
				{
					num2 += splitLayoutSystem_0.LayoutSystems.Count - 1;
				}
			}
			return num2;
		}

		internal static Struct0 smethod_14(SandDockManager sandDockManager_0, WindowMetaData windowMetaData_0)
		{
			DockContainer[] dockContainers = sandDockManager_0.GetDockContainers(LayoutUtilities.smethod_6(windowMetaData_0.LastFixedDockSide));
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

		internal static DockSituation smethod_2(DockContainer dockContainer_0)
		{
			if (dockContainer_0 == null)
			{
				return DockSituation.None;
			}
			if (dockContainer_0.IsFloating)
			{
				return DockSituation.Floating;
			}
			if (dockContainer_0.Dock != DockStyle.Fill)
			{
				return DockSituation.Docked;
			}
			return DockSituation.Document;
		}

		internal static ControlLayoutSystem[] smethod_3(DockContainer dockContainer_0)
		{
			ArrayList arrayList = new ArrayList();
			foreach (LayoutSystemBase layoutSystemBase in dockContainer_0.arrayList_0)
			{
				if (layoutSystemBase is ControlLayoutSystem)
				{
					arrayList.Add(layoutSystemBase);
				}
			}
			return (ControlLayoutSystem[])arrayList.ToArray(typeof(ControlLayoutSystem));
		}

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
							if (controlLayoutSystem.Guid_0 == class18_0.Guid_0)
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
						if (controlLayoutSystem2.Guid_0 == class18_0.Guid_0)
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
				for (int l = 0; l < dockContainers2.Length; l++)
				{
					DockContainer dockContainer_2 = dockContainers2[l];
					if (LayoutUtilities.smethod_2(dockContainer_2) == dockSituation_0)
					{
						ControlLayoutSystem[] array3 = LayoutUtilities.smethod_3(dockContainer_2);
						for (int m = 0; m < array3.Length; m++)
						{
							ControlLayoutSystem controlLayoutSystem3 = array3[m];
							if (controlLayoutSystem3.Guid_0 == class18_0.Guid_0)
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
				{
					arrayList.Add(layoutSystemBase.Parent.LayoutSystems.IndexOf(layoutSystemBase));
				}
			}
			arrayList.Reverse();
			return (int[])arrayList.ToArray(typeof(int));
		}

		internal static DockStyle smethod_6(ContainerDockLocation containerDockLocation_0)
		{
			switch (containerDockLocation_0)
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

		internal static ContainerDockLocation smethod_7(DockStyle dockStyle_0)
		{
			switch (dockStyle_0)
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
			{
				return;
			}
			if (control_0.ContainsFocus)
			{
				control_0.Parent.Focus();
			}
			if (control_0 is DockControl)
			{
				((DockControl)control_0).Boolean_0 = true;
			}
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

		private static bool smethod_9(DockContainer dockContainer_0, SplitLayoutSystem splitLayoutSystem_0)
		{
			IEnumerator enumerator = splitLayoutSystem_0.LayoutSystems.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					LayoutSystemBase layoutSystemBase = (LayoutSystemBase)enumerator.Current;
					if (!(layoutSystemBase is SplitLayoutSystem))
					{
						ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)layoutSystemBase;
						if (!controlLayoutSystem.Collapsed && dockContainer_0.Controls.Contains(controlLayoutSystem.SelectedControl) && controlLayoutSystem.SelectedControl.Visible)
						{
							if (controlLayoutSystem.SelectedControl.Enabled)
							{
								dockContainer_0.ActiveControl = controlLayoutSystem.SelectedControl;
								result = true;
								return result;
							}
						}
					}
					else if (LayoutUtilities.smethod_9(dockContainer_0, (SplitLayoutSystem)layoutSystemBase))
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
			return result;
		}

		internal static bool Boolean_0
		{
			get
			{
				return LayoutUtilities.int_0 > 0;
			}
		}

		private static int int_0;
	}
}
