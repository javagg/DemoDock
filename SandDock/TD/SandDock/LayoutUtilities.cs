using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using TD.Util;

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
            if (layoutSystem == null) throw new ArgumentNullException();

            var dockContainer = layoutSystem.DockContainer;
            if (layoutSystem.AutoHideBar != null && layoutSystem.AutoHideBar.ShowingLayoutSystem == layoutSystem)
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
            var containsFocus = control.ContainsFocus;
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
                var dockControl = control.Manager.FindMostRecentlyUsedWindow(DockSituation.Document, control) ?? control.Manager.FindMostRecentlyUsedWindow((DockSituation)(-1), control);
                dockControl?.method_12(true);
            }
        }

        internal static int smethod_12(DockContainer container)
        {
            int num = container.AllowResize ? 4 : 0;
            return num + smethod_13(container.LayoutSystem, container.Vertical ? Orientation.Vertical : Orientation.Horizontal) * 5;
        }

        private static int smethod_13(SplitLayoutSystem splitLayout, Orientation splitMode)
        {
            var num2 = splitLayout.LayoutSystems.OfType<SplitLayoutSystem>().Select(splitLayoutSystem => smethod_13(splitLayoutSystem, splitMode)).Concat(new[] { 0 }).Max();
            if (splitLayout.LayoutSystems.Count > 1 && splitLayout.SplitMode == splitMode)
                num2 += splitLayout.LayoutSystems.Count - 1;
            return num2;
        }

        internal static Struct0 smethod_14(SandDockManager manager, WindowMetaData metaData)
        {
            var dockContainers = manager.GetDockContainers(Convert(metaData.LastFixedDockSide));
            if (dockContainers.Length == 0)
            {
                var dockContainer = manager.CreateNewDockContainer(metaData.LastFixedDockSide, ContainerDockEdge.Inside, metaData.DockedContentSize);
                return new Struct0(dockContainer.LayoutSystem, 0);
            }
            if (dockContainers.Length >= metaData.DockedState.Count && metaData.DockedState.Index < dockContainers.Length && metaData.DockedState.Index != -1)
            {
                return smethod_15(dockContainers[metaData.DockedState.Index], metaData.DockedState.Int32_0);
            }
            if (metaData.DockedState.Count >= 2)
            {
                if (metaData.DockedState.Index == 0)
                {
                    DockContainer dockContainer2 = manager.CreateNewDockContainer(metaData.LastFixedDockSide, ContainerDockEdge.Outside, metaData.DockedContentSize);
                    return new Struct0(dockContainer2.LayoutSystem, 0);
                }
                if (metaData.DockedState.Index == metaData.DockedState.Count - 1)
                {
                    DockContainer dockContainer3 = manager.CreateNewDockContainer(metaData.LastFixedDockSide, ContainerDockEdge.Inside, metaData.DockedContentSize);
                    return new Struct0(dockContainer3.LayoutSystem, 0);
                }
            }
            if (dockContainers.Length != 0)
            {
                return smethod_15(dockContainers[0], metaData.DockedState.Int32_0);
            }
            DockContainer dockContainer4 = manager.CreateNewDockContainer(metaData.LastFixedDockSide, ContainerDockEdge.Inside, metaData.DockedContentSize);
            return new Struct0(dockContainer4.LayoutSystem, 0);
        }

        internal static Struct0 smethod_15(DockContainer dockContainer_0, int[] int_1)
        {
            var splitLayoutSystem = dockContainer_0.LayoutSystem;
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

        [Naming]
        internal static DockSituation GetDockSituation(DockContainer container)
        {
            if (container == null) return DockSituation.None;
            if (container.IsFloating) return DockSituation.Floating;
            return container.Dock == DockStyle.Fill ? DockSituation.Document : DockSituation.Docked;
        }

        internal static ControlLayoutSystem[] GetControlLayoutSystemsFor(DockContainer container) => container.layoutSystems.OfType<ControlLayoutSystem>().ToArray();

        internal static ControlLayoutSystem smethod_4(SandDockManager manager, DockSituation dockSituation, DockingState dockingState0)
        {
            switch (dockSituation)
            {
                case DockSituation.None:
                    throw new InvalidOperationException();
                case DockSituation.Docked:
                    return
                        manager.GetDockContainers()
                            .Where(container => GetDockSituation(container) == dockSituation)
                            .SelectMany(GetControlLayoutSystemsFor)
                            .FirstOrDefault(layout => layout.Guid == dockingState0.LastLayoutSystemGuid);
                case DockSituation.Document:
                    return manager.DocumentContainer == null
                        ? null
                        : GetControlLayoutSystemsFor(manager.DocumentContainer)
                            .FirstOrDefault(layout => layout.Guid == dockingState0.LastLayoutSystemGuid);
                case DockSituation.Floating:
                {
                    return manager.GetDockContainers().Where(c => GetDockSituation(c) == dockSituation)
                        .SelectMany(GetControlLayoutSystemsFor,
                            (container, layoutSystem) => new {dockContainer_2 = container, controlLayoutSystem3 = layoutSystem})
                        .Where(@t => t.controlLayoutSystem3.Guid == dockingState0.LastLayoutSystemGuid)
                        .Select(@t => t.controlLayoutSystem3).FirstOrDefault();
                }
                default:
                    throw new InvalidOperationException();
            }
        }

        internal static int[] smethod_5(ControlLayoutSystem layoutSystem)
        {
            var arrayList = new ArrayList();
            for (LayoutSystemBase layout = layoutSystem; layout != null; layout = layout.Parent)
            {
                if (layout.Parent != null)
                    arrayList.Add(layout.Parent.LayoutSystems.IndexOf(layout));
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
                default:
                    return DockStyle.Fill;
            }
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
                default:
                    return ContainerDockLocation.Center;
            }
        }

        internal static void smethod_8(Control control)
        {
            if (control.Parent == null) return;

            if (control.ContainsFocus)
                control.Parent.Focus();

            var dockControl = control as DockControl;
            if (dockControl != null)
                dockControl.IgnoreFontEvents = true;
            try
            {
                var container = control.Parent.GetContainerControl();
                if (container != null)
                {
                    var dockContainer = container as DockContainer;
                    if (dockContainer != null && !dockContainer.Boolean_2 && dockContainer.Manager?.OwnerForm != null && dockContainer.Manager.OwnerForm.IsMdiContainer)
                    {
                        smethod_9(dockContainer, dockContainer.LayoutSystem);
                    }
                    else if (container.ActiveControl == control)
                    {
                        container.ActiveControl = null;
                    }
                }
                control.Parent.Controls.Remove(control);
            }
            finally
            {
                var dockControl1 = control as DockControl;
                if (dockControl1 != null)
                    dockControl1.IgnoreFontEvents = false;
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
                                return true;
                            }
                        }
                    }
                    else if (smethod_9(container, (SplitLayoutSystem)layoutSystemBase))
                    {
                        return true;
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
        [Naming(NamingType.FromOldVersion)]
        internal static bool LayoutInProgress => _cnt > 0;

        private static int _cnt;
    }
}
