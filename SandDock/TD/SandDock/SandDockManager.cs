using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
    public delegate void ActiveFilesListEventHandler(object sender, ActiveFilesListEventArgs e);

    public class ActiveFilesListEventArgs : EventArgs
    {
        internal ActiveFilesListEventArgs(DockControl[] windows, Control control, Point position)
        {
            Windows = windows;
            Control = control;
            Position = position;
        }

        public Control Control { get; }

        public Point Position { get; }

        public DockControl[] Windows { get; }
    }

    public enum ContextMenuContext
    {
        Keyboard,
        RightClick,
        OptionsButton,
        Other
    }

    public delegate void ShowControlContextMenuEventHandler(object sender, ShowControlContextMenuEventArgs e);

    public class ShowControlContextMenuEventArgs : DockControlEventArgs
    {
        internal ShowControlContextMenuEventArgs(DockControl dockControl, Point position, ContextMenuContext context) : base(dockControl)
        {
            Position = position;
            Context = context;
        }

        public ContextMenuContext Context { get; }

        public Point Position { get; }
    }

    internal class Class22
    {
        internal static void smethod_0(SandDockManager manager, XmlNode xmlNode_0)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(long));
            TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(int));
            TypeConverter converter3 = TypeDescriptor.GetConverter(typeof(Size));
            TypeConverter converter4 = TypeDescriptor.GetConverter(typeof(Point));
            DockControl dockControl = manager.FindControl(new Guid(xmlNode_0.Attributes["Guid"].Value));
            if (dockControl != null)
            {
                if (xmlNode_0.Attributes["LastFocused"] != null)
                {
                    dockControl.MetaData.SaveFocused(DateTime.FromFileTime((long)converter.ConvertFromString(null, CultureInfo.InvariantCulture, xmlNode_0.Attributes["LastFocused"].Value)));
                }
                if (xmlNode_0.Attributes["DockedSize"] != null)
                {
                    dockControl.MetaData.SaveDockedContentSize((int)converter2.ConvertFromString(xmlNode_0.Attributes["DockedSize"].Value));
                }
                if (xmlNode_0.Attributes["PopupSize"] != null)
                {
                    dockControl.PopupSize = (int)converter2.ConvertFromString(xmlNode_0.Attributes["PopupSize"].Value);
                }
                dockControl.FloatingLocation = (Point)converter4.ConvertFromString(null, CultureInfo.InvariantCulture, xmlNode_0.Attributes["FloatingLocation"].Value);
                dockControl.FloatingSize = (Size)converter3.ConvertFromString(null, CultureInfo.InvariantCulture, xmlNode_0.Attributes["FloatingSize"].Value);
                if (xmlNode_0.Attributes["LastOpenDockSituation"] != null)
                {
                    dockControl.MetaData.SaveOpenDockSituation((DockSituation)Enum.Parse(typeof(DockSituation), xmlNode_0.Attributes["LastOpenDockSituation"].Value));
                }
                if (xmlNode_0.Attributes["LastFixedDockSituation"] != null)
                {
                    dockControl.MetaData.SaveFixedDockSituation((DockSituation)Enum.Parse(typeof(DockSituation), xmlNode_0.Attributes["LastFixedDockSituation"].Value));
                }
                if (xmlNode_0.Attributes["LastFixedDockLocation"] != null)
                {
                    ContainerDockLocation containerDockLocation = (ContainerDockLocation)Enum.Parse(typeof(ContainerDockLocation), xmlNode_0.Attributes["LastFixedDockLocation"].Value);
                    if (!Enum.IsDefined(typeof(ContainerDockLocation), containerDockLocation))
                    {
                        containerDockLocation = ContainerDockLocation.Right;
                    }
                    dockControl.MetaData.SaveFixedDockSide(containerDockLocation);
                }
                if (xmlNode_0.Attributes["LastFloatingWindowGuid"] != null)
                {
                    dockControl.MetaData.SaveFloatingWindowGuid(new Guid(xmlNode_0.Attributes["LastFloatingWindowGuid"].Value));
                }
                if (xmlNode_0.Attributes["LastDockContainerCount"] != null)
                {
                    dockControl.MetaData.DockedState.Count = (int)converter2.ConvertFromString(xmlNode_0.Attributes["LastDockContainerCount"].Value);
                }
                if (xmlNode_0.Attributes["LastDockContainerIndex"] != null)
                {
                    dockControl.MetaData.DockedState.Index = (int)converter2.ConvertFromString(xmlNode_0.Attributes["LastDockContainerIndex"].Value);
                }
                Class22.smethod_1(dockControl, xmlNode_0, dockControl.MetaData.DockedState, "Docked");
                Class22.smethod_1(dockControl, xmlNode_0, dockControl.MetaData.DocumentState, "Document");
                Class22.smethod_1(dockControl, xmlNode_0, dockControl.MetaData.FloatingState, "Floating");
                return;
            }
        }

        private static void smethod_1(DockControl dockControl_0, XmlNode xmlNode_0, DockingState dockingState0, string string_0)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
            if (xmlNode_0.Attributes[string_0 + "WorkingSize"] != null)
            {
                dockingState0.Size = SandDockManager.ConvertStringToSizeF(xmlNode_0.Attributes[string_0 + "WorkingSize"].Value);
            }
            if (xmlNode_0.Attributes[string_0 + "WindowGroupGuid"] != null)
            {
                dockingState0.LastLayoutSystemGuid = new Guid(xmlNode_0.Attributes[string_0 + "WindowGroupGuid"].Value);
            }
            if (xmlNode_0.Attributes[string_0 + "IndexInWindowGroup"] != null)
            {
                dockingState0.Int32_1 = (int)converter.ConvertFromString(null, CultureInfo.InvariantCulture, xmlNode_0.Attributes[string_0 + "IndexInWindowGroup"].Value);
            }
            if (xmlNode_0.Attributes[string_0 + "SplitPath"] != null)
            {
                dockingState0.Int32_0 = Class22.smethod_2(xmlNode_0.Attributes[string_0 + "SplitPath"].Value);
            }
        }

        private static int[] smethod_2(string string_0)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
            string[] array = string_0.Split(new char[]
            {
                '|'
            });
            int[] array2 = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array2[i] = (int)converter.ConvertFromString(null, CultureInfo.InvariantCulture, array[i]);
            }
            return array2;
        }

        internal static void smethod_3(DockControl dockControl_0, XmlTextWriter xmlTextWriter_0)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(long));
            TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(int));
            TypeConverter converter3 = TypeDescriptor.GetConverter(typeof(Size));
            TypeConverter converter4 = TypeDescriptor.GetConverter(typeof(Point));
            xmlTextWriter_0.WriteStartElement("Window");
            xmlTextWriter_0.WriteAttributeString("Guid", dockControl_0.Guid.ToString());
            xmlTextWriter_0.WriteAttributeString("LastFocused", converter.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.MetaData.LastFocused.ToFileTime()));
            xmlTextWriter_0.WriteAttributeString("DockedSize", converter2.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.MetaData.DockedContentSize));
            xmlTextWriter_0.WriteAttributeString("PopupSize", converter2.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.PopupSize));
            xmlTextWriter_0.WriteAttributeString("FloatingLocation", converter4.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.FloatingLocation));
            xmlTextWriter_0.WriteAttributeString("FloatingSize", converter3.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.FloatingSize));
            xmlTextWriter_0.WriteAttributeString("LastOpenDockSituation", dockControl_0.MetaData.LastOpenDockSituation.ToString());
            xmlTextWriter_0.WriteAttributeString("LastFixedDockSituation", dockControl_0.MetaData.LastFixedDockSituation.ToString());
            xmlTextWriter_0.WriteAttributeString("LastFixedDockLocation", dockControl_0.MetaData.LastFixedDockSide.ToString());
            xmlTextWriter_0.WriteAttributeString("LastFloatingWindowGuid", dockControl_0.MetaData.LastFloatingWindowGuid.ToString());
            xmlTextWriter_0.WriteAttributeString("LastDockContainerCount", converter2.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.MetaData.DockedState.Count));
            xmlTextWriter_0.WriteAttributeString("LastDockContainerIndex", converter2.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.MetaData.DockedState.Index));
            Class22.smethod_4(dockControl_0, xmlTextWriter_0, dockControl_0.MetaData.DockedState, "Docked");
            Class22.smethod_4(dockControl_0, xmlTextWriter_0, dockControl_0.MetaData.DocumentState, "Document");
            Class22.smethod_4(dockControl_0, xmlTextWriter_0, dockControl_0.MetaData.FloatingState, "Floating");
            xmlTextWriter_0.WriteEndElement();
        }

        private static void smethod_4(DockControl dockControl_0, XmlTextWriter xmlTextWriter_0, DockingState dockingState0, string string_0)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
            xmlTextWriter_0.WriteAttributeString(string_0 + "WorkingSize", SandDockManager.ConvertSizeFToString(dockingState0.Size));
            xmlTextWriter_0.WriteAttributeString(string_0 + "WindowGroupGuid", dockingState0.LastLayoutSystemGuid.ToString());
            xmlTextWriter_0.WriteAttributeString(string_0 + "IndexInWindowGroup", converter.ConvertToString(null, CultureInfo.InvariantCulture, dockingState0.Int32_1));
            xmlTextWriter_0.WriteAttributeString(string_0 + "SplitPath", smethod_5(dockingState0.Int32_0));
        }

        private static string smethod_5(int[] int_0)
        {
            var converter = TypeDescriptor.GetConverter(typeof(int));
            var array = new string[int_0.Length];
            for (var i = 0; i < int_0.Length; i++)
            {
                array[i] = converter.ConvertToString(null, CultureInfo.InvariantCulture, int_0[i]);
            }
            return string.Join("|", array);
        }
    }
    [DefaultEvent("ActiveTabbedDocumentChanged"), Designer("Design.SandDockManagerDesigner"), ToolboxBitmap(typeof(SandDockManager))]
    public class SandDockManager : Component
    {
        public SandDockManager()
        {
            _renderer = new WhidbeyRenderer();
            _containers = new ArrayList();
            _windows = new Hashtable();
            _autoHideBars = new ArrayList();
        }

        public static void ActivateProduct(string licenseKey)
        {
        }

        private string ConvertBoolToString(bool value) => !value ? "0" : "1";

        private string ConvertPointToString(Point point)
        {
            return (string)TypeDescriptor.GetConverter(typeof(Point)).ConvertTo(null, CultureInfo.InvariantCulture, point, typeof(string));
        }

        private string ConvertRectangleToString(Rectangle rectangle)
        {
            return (string)TypeDescriptor.GetConverter(typeof(Rectangle)).ConvertTo(null, CultureInfo.InvariantCulture, rectangle, typeof(string));
        }

        internal static string ConvertSizeFToString(SizeF size)
        {
            return (string)TypeDescriptor.GetConverter(typeof(SizeF)).ConvertTo(null, CultureInfo.InvariantCulture, size, typeof(string));
        }

        private string ConvertSizeToString(Size size)
        {
            return (string)TypeDescriptor.GetConverter(typeof(Size)).ConvertTo(null, CultureInfo.InvariantCulture, size, typeof(string));
        }

        private bool ConvertStringToBool(string str) => str != "0";

        private Point ConvertStringToPoint(string str)
        {
            return (Point)TypeDescriptor.GetConverter(typeof(Point)).ConvertFrom(null, CultureInfo.InvariantCulture, str);
        }

        private Rectangle ConvertStringToRectangle(string str)
        {
            return (Rectangle)TypeDescriptor.GetConverter(typeof(Rectangle)).ConvertFrom(null, CultureInfo.InvariantCulture, str);
        }

        private Size ConvertStringToSize(string str)
        {
            return (Size)TypeDescriptor.GetConverter(typeof(Size)).ConvertFrom(null, CultureInfo.InvariantCulture, str);
        }

        internal static SizeF ConvertStringToSizeF(string str)
        {
            return (SizeF)TypeDescriptor.GetConverter(typeof(SizeF)).ConvertFrom(null, CultureInfo.InvariantCulture, str);
        }

        public DockContainer CreateNewDockContainer(ContainerDockLocation dockLocation, ContainerDockEdge edge, int contentSize)
        {
            EnsureDockSystemContainer();
            DockSystemContainer.SuspendLayout();
            DockContainer result;
            try
            {
                var dockContainer = CreateNewDockContainerCore(dockLocation);
                dockContainer.Manager = this;
                var dockStyle = LayoutUtilities.Convert(dockLocation);
                dockContainer.Dock = dockStyle;
                dockContainer.ContentSize = contentSize;
               // IntPtr arg_3B_0 = dockContainer.Handle;
                var newIndex = dockLocation != ContainerDockLocation.Center ? (edge == ContainerDockEdge.Inside ? GetInsideControlIndex(DockSystemContainer)
                        : GetOutsideControlIndex(DockSystemContainer, dockStyle)) : 0;
                DockSystemContainer.Controls.Add(dockContainer);
                DockSystemContainer.Controls.SetChildIndex(dockContainer, newIndex);
                foreach (var control2 in DockSystemContainer.Controls.Cast<Control>().Select(control => control as PopupContainer))
                    control2?.BringToFront();
                result = dockContainer;
            }
            finally
            {
                DockSystemContainer.ResumeLayout();
            }
            return result;
        }

        protected virtual DockContainer CreateNewDockContainerCore(ContainerDockLocation dockLocation)
        {
            return dockLocation == ContainerDockLocation.Center && EnableTabbedDocuments ? new DocumentContainer() : new DockContainer();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var array = new DockContainer[_containers.Count];
                _containers.CopyTo(array);
                foreach (var dockContainer in array)
                    dockContainer.Dispose();
                _containers.Clear();
                var array3 = new AutoHideBar[_autoHideBars.Count];
                _autoHideBars.CopyTo(array3);
                var array4 = array3;
                foreach (var control in array4)
                    control.Dispose();
                _autoHideBars.Clear();
            }
            base.Dispose(disposing);
        }

        private void EnsureDockSystemContainer()
        {
            if (DockSystemContainer == null)
                throw new InvalidOperationException("This SandDockManager does not have its DockSystemContainer property set.");
        }

        private void EnsureHandles()
        {
        }

        public DockControl FindControl(Guid guid)
        {
            var control = (DockControl)_windows[guid];
            if (control != null) return control;
            var e = new ResolveDockControlEventArgs(guid);
            OnResolveDockControl(e);
            if (e.DockControl == null) return null;
            e.DockControl.Manager = this;
            return e.DockControl;
        }

        public DockContainer FindDockContainer(ContainerDockLocation location) => FindDockedContainer(LayoutUtilities.Convert(location));

        internal DockContainer FindDockedContainer(DockStyle dockStyle) => _containers.Cast<DockContainer>().FirstOrDefault(c => c.Dock == dockStyle && !c.IsFloating);

        private Control FindDockSystemContainer(IDesignerHost designerHost, Control parent)
        {
            var ctl = parent.Controls.Cast<Control>().FirstOrDefault(c => c.Dock == DockStyle.Fill && c.Site != null && c.Site.DesignMode && !c.Controls.IsReadOnly);
            return ctl != null ? FindDockSystemContainer(designerHost, ctl) : parent;
        }

        internal FloatingContainer FindFloatingDockContainer(Guid guid) => GetFloatingDockContainerList().FirstOrDefault(@class => @class.Guid == guid);

        public DockControl FindMostRecentlyUsedWindow() => FindMostRecentlyUsedWindow((DockSituation)(-1));

        public DockControl FindMostRecentlyUsedWindow(DockSituation dockSituation) => FindMostRecentlyUsedWindow(dockSituation, null);

        internal DockControl FindMostRecentlyUsedWindow(DockSituation dockSituation, DockControl notThisOne)
        {
            var t = DateTime.MinValue;
            DockControl result = null;
            foreach (var control in GetDockControls().Where(dockControl => dockControl != notThisOne && dockControl.MetaData.LastFocused >= t && (dockSituation == (DockSituation)(-1) || dockControl.DockSituation == dockSituation)))
            {
                t = control.MetaData.LastFocused;
                result = control;
            }
            return result;
        }

        internal AutoHideBar GetAutoHideBar(DockStyle dock)
        {
            if (dock == DockStyle.Fill || dock == DockStyle.None) return null;
            var result = _autoHideBars.Cast<AutoHideBar>().FirstOrDefault(c => c.Dock == dock);
            if (result != null) return result;
            DockSystemContainer.SuspendLayout();
            try
            {
                var bar = new AutoHideBar
                {
                    Manager = this,
                    Dock = dock,
                    Parent = DockSystemContainer
                };
                DockSystemContainer.Controls.SetChildIndex(bar, GetOutsideControlIndex(DockSystemContainer, dock));
                result = bar;
            }
            finally
            {
                DockSystemContainer.ResumeLayout();
            }
            return result;
        }

        public DockContainer[] GetDockContainers()
        {
            return (DockContainer[])_containers.ToArray(typeof(DockContainer));
        }

        internal DockContainer[] GetDockContainers(DockStyle dockStyle)
        {
            if (dockStyle == DockStyle.Fill)
                throw new ArgumentException(nameof(dockStyle));
            if (DockSystemContainer == null)
                return new DockContainer[0];

            var array = new DockContainer[DockSystemContainer.Controls.Count];
            int num = 0;
            for (int i = DockSystemContainer.Controls.Count - 1; i >= 0; i--)
            {
                var dockContainer = DockSystemContainer.Controls[i] as DockContainer;
                if (dockContainer?.Dock == dockStyle)
                {
                    array[num++] = dockContainer;
                }
            }
            DockContainer[] array2 = new DockContainer[num];
            Array.Copy(array, array2, num);
            return array2;
        }

        public DockControl[] GetDockControls()
        {
            var array = new DockControl[_windows.Count];
            _windows.Values.CopyTo(array, 0);
            return array;
        }

        public DockControl[] GetDockControls(DockSituation dockSituation) => _windows.Values.Cast<DockControl>().Where(control => control.DockSituation == dockSituation).ToArray();

        private FloatingContainer[] GetFloatingDockContainerList() => _containers.Cast<DockContainer>().Where(container => container.IsFloating).Cast<FloatingContainer>().ToArray();

        private int GetInsideControlIndex(Control container)
        {
            var num = int.MaxValue;
            for (var i = container.Controls.Count - 1; i >= 0; i--)
            {
                var control = container.Controls[i];
                if (control.Dock != DockStyle.Fill && control.Dock != DockStyle.None && i < num)
                {
                    num = i;
                }
            }
            return num;
        }

        public string GetLayout()
        {
            EnsureDockSystemContainer();
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("Layout");
            foreach (var dockControl in _windows.Values.Cast<DockControl>().Where(c => c.PersistState))
                SaveWindowLayout(dockControl, xmlTextWriter);

            DockContainer[] orderedDockedDockContainerList = this.GetOrderedDockedDockContainerList();
            foreach (var dockContainer in orderedDockedDockContainerList)
            {
                if (dockContainer.LayoutSystem.ContainsPersistableDockControls)
                {
                    SaveContainerLayout(dockContainer, xmlTextWriter);
                }
            }
            FloatingContainer[] floatingDockContainerList = GetFloatingDockContainerList();
            foreach (DockContainer dockContainer2 in floatingDockContainerList)
            {
                if (dockContainer2.LayoutSystem.ContainsPersistableDockControls)
                {
                    SaveContainerLayout(dockContainer2, xmlTextWriter);
                }
            }
            DocumentContainer documentContainer = this.FindDockedContainer(DockStyle.Fill) as DocumentContainer;
            if (documentContainer != null && this.SerializeTabbedDocuments && documentContainer.LayoutSystem.ContainsPersistableDockControls)
            {
                this.SaveContainerLayout(documentContainer, xmlTextWriter);
            }
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
            return stringWriter.ToString();
        }

        private DockContainer[] GetOrderedDockedDockContainerList()
        {
            if (DockSystemContainer == null) return new DockContainer[0];

            ArrayList arrayList = new ArrayList();
            for (var i = 0; i < DockSystemContainer.Controls.Count; i++)
            {
                var control = DockSystemContainer.Controls[i];
                if (_containers.Contains(control) && !(control is DocumentContainer))
                    arrayList.Add(control);
            }
            return (DockContainer[])arrayList.ToArray(typeof(DockContainer));
        }

        private int GetOutsideControlIndex(Control container, DockStyle dockStyle)
        {
            int result = container.Controls.Count;
            for (var i = container.Controls.Count - 1; i >= 0; i--)
            {
                Control control = container.Controls[i];
                if (control.Dock == DockStyle.Fill)
                {
                    if (!(control is MdiClient))
                    {
                        break;
                    }
                }
                if (!(control is DockContainer))
                {
                    result = i;
                }
                if (control is DockContainer && control.Dock == dockStyle)
                {
                    break;
                }
            }
            return result;
        }

        private string GetSettingsKey() => OwnerForm != null ? OwnerForm.GetType().FullName : "default";

        public void LoadLayout()
        {
            var settings = new LayoutSettings(GetSettingsKey());
            if (!settings.IsDefault && !string.IsNullOrEmpty(settings.LayoutXml))
                SetLayout(settings.LayoutXml);
        }

        protected internal virtual void OnActiveTabbedDocumentChanged(EventArgs e) => ActiveTabbedDocumentChanged?.Invoke(this, e);

        private void OnActiveTabbedDocumentDockSituationChanged(object sender, EventArgs e)
        {
            var dockControl = (DockControl)sender;
            if (dockControl.DockSituation != DockSituation.Document)
                SetActiveTabbedDocument(FindMostRecentlyUsedWindow(DockSituation.Document));
        }

        protected internal virtual void OnDockControlActivated(DockControlEventArgs e)
        {
            DockControlActivated?.Invoke(this, e);
            if (e.DockControl.DockSituation == DockSituation.Document)
                SetActiveTabbedDocument(e.DockControl);
        }

        protected virtual void OnDockControlAdded(DockControlEventArgs e)
        {
            DockControlAdded?.Invoke(this, e);
        }

        protected internal virtual void OnDockControlClosing(DockControlClosingEventArgs e)
        {
            DockControlClosing?.Invoke(this, e);
        }

        protected virtual void OnDockControlRemoved(DockControlEventArgs e)
        {
            DockControlRemoved?.Invoke(this, e);
        }

        protected internal virtual void OnDockingFinished(EventArgs e)
        {
            DockingFinished?.Invoke(this, e);
        }

        protected internal virtual void OnDockingStarted(EventArgs e)
        {
            DockingStarted?.Invoke(this, e);
        }

        private void OnDockSystemContainerResize(object sender, EventArgs e)
        {
            if (OwnerForm?.WindowState == FormWindowState.Minimized) return;
            var form = DockSystemContainer?.FindForm();
            if (form != null)
            {
                if (form.WindowState == FormWindowState.Minimized)
                {
                    return;
                }
                var form2 = form.Parent?.FindForm();
                if (form2 != null)
                {
                    if (form2.WindowState == FormWindowState.Minimized)
                        return;
                    if (form2.ActiveMdiChild != null && form2.ActiveMdiChild != form && form2.ActiveMdiChild.WindowState == FormWindowState.Maximized)
                        return;
                }
            }
            var rectangle = StandardDockingManager.GetDockingBounds(DockSystemContainer);
            int num = -rectangle.Width;
            int num2 = -rectangle.Height;
            if (this.DockSystemContainer is ToolStripContentPanel && (rectangle.Width <= 0 || rectangle.Height <= 0))
            {
                return;
            }
            if (num > 0)
            {
                ArrayList arrayList = new ArrayList();
                int num3 = 0;
                foreach (DockContainer dockContainer in this._containers)
                {
                    if (dockContainer.Dock != DockStyle.Left)
                    {
                        if (dockContainer.Dock != DockStyle.Right)
                        {
                            continue;
                        }
                    }
                    num3 += dockContainer.Width;
                    arrayList.Add(dockContainer);
                }
                if (num3 > 0)
                {
                    foreach (DockContainer dockContainer2 in arrayList)
                    {
                        int num4 = Convert.ToInt32((double)dockContainer2.Width / (double)num3 * (double)num);
                        num3 -= dockContainer2.Width;
                        num -= num4;
                        dockContainer2.ContentSize -= num4;
                        if (num3 == 0)
                        {
                            break;
                        }
                    }
                }
            }
            if (num2 > 0)
            {
                ArrayList arrayList2 = new ArrayList();
                int num5 = 0;
                foreach (DockContainer dockContainer3 in this._containers)
                {
                    if (dockContainer3.Dock != DockStyle.Top)
                    {
                        if (dockContainer3.Dock != DockStyle.Bottom)
                        {
                            continue;
                        }
                    }
                    num5 += dockContainer3.Height;
                    arrayList2.Add(dockContainer3);
                }
                if (num5 > 0)
                {
                    foreach (DockContainer dockContainer4 in arrayList2)
                    {
                        int num6 = Convert.ToInt32((double)dockContainer4.Height / (double)num5 * (double)num2);
                        num5 -= dockContainer4.Height;
                        num2 -= num6;
                        dockContainer4.ContentSize -= num6;
                        if (num5 == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void OnOwnerFormActivated(object sender, EventArgs e)
        {
            foreach (var container in _containers.Cast<DockContainer>().Where(c => !c.IsFloating))
                container.OnActivated(sender, e);
        }

        private void OnOwnerFormClosing(object sender, CancelEventArgs e)
        {
            if (AutoSaveLayout)
                SaveLayout();
        }

        private void OnOwnerFormDeactivated(object sender, EventArgs e)
        {
            foreach (var container in _containers.Cast<DockContainer>().Where(c => !c.IsFloating))
                container.OnDeactivate(sender, e);
        }

        private void OnOwnerFormLoad(object sender, EventArgs e)
        {
            if (AutoSaveLayout)
                LoadLayout();
        }

        private void OnRendererMetricsChanged(object sender, EventArgs e)
        {
            PropagateNewRenderer();
        }

        protected virtual void OnResolveDockControl(ResolveDockControlEventArgs e)
        {
            ResolveDockControl?.Invoke(this, e);
        }

        protected internal virtual void OnShowActiveFilesList(ActiveFilesListEventArgs e)
        {
            ShowActiveFilesList?.Invoke(this, e);
        }

        protected internal virtual void OnShowControlContextMenu(ShowControlContextMenuEventArgs e)
        {
            ShowControlContextMenu?.Invoke(this, e);
        }

        private void PropagateNewRenderer()
        {
            foreach (DockContainer dockContainer in _containers)
                dockContainer.PropagateNewRenderer();
            foreach (AutoHideBar control in _autoHideBars)
                control.PropagateNewRenderer();
        }

        private void ReadContainerProperties(XmlNode containerNode, DockContainer container)
        {
            var dockStyle = (DockStyle)int.Parse(containerNode.Attributes["Dock"].Value);
            int contentSize = 0;
            if (containerNode.Attributes["ContentSize"] != null)
            {
                contentSize = int.Parse(containerNode.Attributes["ContentSize"].Value);
            }
            if (container == null)
            {
                container = CreateNewDockContainer(LayoutUtilities.Convert(dockStyle), ContainerDockEdge.Outside, contentSize);
            }
            container.Dock = dockStyle;
            container.ContentSize = contentSize;
            foreach (XmlNode xmlNode in containerNode.ChildNodes)
            {
                if (xmlNode.NodeType == XmlNodeType.Element)
                {
                    if (xmlNode.Name == "SplitLayoutSystem")
                    {
                        SplitLayoutSystem splitLayoutSystem = ReadSplitLayoutSystem(xmlNode, container);
                        if (splitLayoutSystem != null)
                        {
                            container.LayoutSystem = splitLayoutSystem;
                            break;
                        }
                        container.Dispose();
                        break;
                    }
                }
            }
        }

        private ControlLayoutSystem ReadControlLayoutSystem(XmlNode controlNode, DockContainer container)
        {
            Guid empty = Guid.Empty;
            SizeF size = SandDockManager.ConvertStringToSizeF(controlNode.Attributes["WorkingSize"].Value);
            bool collapsed = this.ConvertStringToBool(controlNode.Attributes["Collapsed"].Value);
            DockControl dockControl = null;
            if (controlNode.Attributes["SelectedControl"] != null)
            {
                Guid guid = new Guid(controlNode.Attributes["SelectedControl"].Value);
                dockControl = this.FindControl(guid);
            }
            if (controlNode.Attributes["Guid"] != null)
            {
                empty = new Guid(controlNode.Attributes["Guid"].Value);
            }
            ArrayList arrayList = new ArrayList();
            foreach (XmlNode xmlNode in controlNode.ChildNodes)
            {
                if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "Controls")
                {
                    foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                    {
                        if (xmlNode2.NodeType == XmlNodeType.Element && xmlNode2.Name == "Control")
                        {
                            Guid guid2 = new Guid(xmlNode2.Attributes["Guid"].Value);
                            DockControl dockControl2 = this.FindControl(guid2);
                            if (dockControl2 != null)
                            {
                                arrayList.Add(dockControl2);
                            }
                        }
                    }
                }
            }
            if (arrayList.Count == 0)
            {
                return null;
            }
            ControlLayoutSystem controlLayoutSystem = container.CreateNewLayoutSystem(size);
            controlLayoutSystem.Controls.AddRange((DockControl[])arrayList.ToArray(typeof(DockControl)));
            if (dockControl != null)
            {
                controlLayoutSystem.SelectedControl = dockControl;
            }
            controlLayoutSystem.Collapsed = collapsed;
            if (empty != Guid.Empty)
            {
                controlLayoutSystem.Guid = empty;
            }
            return controlLayoutSystem;
        }

        private void ReadFloatingContainerProperties(XmlNode node, FloatingContainer container)
        {
            var rectangle_ = ConvertStringToRectangle(node.Attributes["Bounds"].Value);
            Guid guid = Guid.NewGuid();
            if (node.Attributes["Guid"] != null)
            {
                guid = new Guid(node.Attributes["Guid"].Value);
            }
            if (container == null)
            {
                container = new FloatingContainer(this, guid);
            }
            foreach (XmlNode xmlNode in node.ChildNodes)
            {
                if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "SplitLayoutSystem")
                {
                    SplitLayoutSystem splitLayoutSystem = this.ReadSplitLayoutSystem(xmlNode, container);
                    if (splitLayoutSystem == null)
                    {
                        container.Dispose();
                        return;
                    }
                    container.LayoutSystem = splitLayoutSystem;
                }
            }
            container.method_19(rectangle_, true, false);
        }

        private SplitLayoutSystem ReadSplitLayoutSystem(XmlNode splitNode, DockContainer container)
        {
            SizeF workingSize = ConvertStringToSizeF(splitNode.Attributes["WorkingSize"].Value);
            workingSize.Width = Math.Max(workingSize.Width, 1f);
            workingSize.Height = Math.Max(workingSize.Height, 1f);
            Orientation splitMode = (Orientation)int.Parse(splitNode.Attributes["SplitMode"].Value);
            ArrayList arrayList = new ArrayList();
            foreach (XmlNode xmlNode in splitNode.ChildNodes)
            {
                if (xmlNode.NodeType == XmlNodeType.Element)
                {
                    if (xmlNode.Name == "SplitLayoutSystem")
                    {
                        SplitLayoutSystem splitLayoutSystem = this.ReadSplitLayoutSystem(xmlNode, container);
                        if (splitLayoutSystem != null)
                        {
                            arrayList.Add(splitLayoutSystem);
                            continue;
                        }
                        continue;
                    }
                }
                if (xmlNode.NodeType == XmlNodeType.Element)
                {
                    if (xmlNode.Name == "ControlLayoutSystem")
                    {
                        ControlLayoutSystem controlLayoutSystem = this.ReadControlLayoutSystem(xmlNode, container);
                        if (controlLayoutSystem != null)
                        {
                            arrayList.Add(controlLayoutSystem);
                        }
                    }
                }
            }
            if (arrayList.Count == 0)
            {
                return null;
            }
            return new SplitLayoutSystem(workingSize, splitMode, (LayoutSystemBase[])arrayList.ToArray(typeof(LayoutSystemBase)));
        }

        private void ReadWindowProperties(XmlNode node)
        {
            Class22.smethod_0(this, node);
        }

        internal void RegisterAutoHideBar(AutoHideBar bar)
        {
            if (!_autoHideBars.Contains(bar))
                _autoHideBars.Add(bar);
            bar.AllowDrop = SelectTabsOnDrag;
        }

        internal void RegisterDockContainer(DockContainer container)
        {
            if (container is DocumentContainer && DocumentContainer != null)
                throw new InvalidOperationException("Only one DocumentContainer can exist in a SandDock layout.");
            if (!_containers.Contains(container))
                _containers.Add(container);
            if (DockSystemContainer == null && container.Parent is ContainerControl && !container.IsFloating)
                DockSystemContainer = (ContainerControl)container.Parent;
            container.AllowDrop = SelectTabsOnDrag;
            if (container is DocumentContainer)
            {
                DocumentContainer = (DocumentContainer)container;
                DocumentContainer.BorderStyle = BorderStyle;
                DocumentContainer.DocumentOverflow = DocumentOverflow;
                DocumentContainer.IntegralClose = IntegralClose;
            }
        }

        internal void RegisterWindow(DockControl control)
        {
            _windows[control.Guid] = control;
            OnDockControlAdded(new DockControlEventArgs(control));
        }

        internal void ReRegisterWindow(DockControl control, Guid oldGuid)
        {
            if (_windows.Contains(oldGuid))
                _windows.Remove(oldGuid);
            _windows[control.Guid] = control;
        }

        private void SaveContainerLayout(DockContainer container, XmlTextWriter writer)
        {
            if (!(container is FloatingContainer))
            {
                if (container is DocumentContainer)
                    writer.WriteStartElement("DocumentContainer");
                else
                    writer.WriteStartElement("Container");

                writer.WriteAttributeString("Dock", ((int) container.Dock).ToString());
                if (container.Dock != DockStyle.Fill && container.Dock != DockStyle.None)
                    writer.WriteAttributeString("ContentSize", container.ContentSize.ToString());
                SaveLayoutSystem(container.LayoutSystem, writer);
                writer.WriteEndElement();
            }
            else
            {
                var @class = (FloatingContainer) container;
                writer.WriteStartElement("FloatingContainer");
                writer.WriteAttributeString("Bounds", this.ConvertRectangleToString(@class.FloatingBounds));
                writer.WriteAttributeString("Guid", @class.Guid.ToString());
                SaveLayoutSystem(container.LayoutSystem, writer);
                writer.WriteEndElement();
            }
        }

        public void SaveLayout()
        {
            new LayoutSettings(GetSettingsKey()) { LayoutXml = GetLayout() }.Save();
        }

        private void SaveLayoutSystem(LayoutSystemBase layoutSystem, XmlTextWriter writer)
        {
            if (layoutSystem is SplitLayoutSystem)
            {
                writer.WriteStartElement("SplitLayoutSystem");
            }
            else
            {
                if (!(layoutSystem is ControlLayoutSystem))
                {
                    return;
                }
                writer.WriteStartElement("ControlLayoutSystem");
            }
            writer.WriteAttributeString("WorkingSize", SandDockManager.ConvertSizeFToString(layoutSystem.WorkingSize));
            if (!(layoutSystem is SplitLayoutSystem))
            {
                if (layoutSystem is ControlLayoutSystem)
                {
                    ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)layoutSystem;
                    writer.WriteAttributeString("Guid", controlLayoutSystem.Guid.ToString());
                    writer.WriteAttributeString("Collapsed", this.ConvertBoolToString(controlLayoutSystem.Collapsed));
                    if (controlLayoutSystem.SelectedControl != null && controlLayoutSystem.SelectedControl.PersistState)
                    {
                        writer.WriteAttributeString("SelectedControl", controlLayoutSystem.SelectedControl.Guid.ToString());
                    }
                    writer.WriteStartElement("Controls");
                    foreach (DockControl dockControl in controlLayoutSystem.Controls)
                    {
                        if (dockControl.PersistState)
                        {
                            writer.WriteStartElement("Control");
                            writer.WriteAttributeString("Guid", dockControl.Guid.ToString());
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();
                }
            }
            else
            {
                SplitLayoutSystem splitLayoutSystem = (SplitLayoutSystem)layoutSystem;
                writer.WriteAttributeString("SplitMode", ((int)splitLayoutSystem.SplitMode).ToString());
                foreach (LayoutSystemBase layoutSystemBase in splitLayoutSystem.LayoutSystems)
                {
                    if (layoutSystemBase.ContainsPersistableDockControls)
                    {
                        this.SaveLayoutSystem(layoutSystemBase, writer);
                    }
                }
            }
            writer.WriteEndElement();
        }

        private void SaveWindowLayout(DockControl control, XmlTextWriter writer)
        {
            Class22.smethod_3(control, writer);
        }

        private void SetActiveTabbedDocument(DockControl value)
        {
            if (value != null && value.DockSituation != DockSituation.Document)
                throw new ArgumentException("The specified window is not currently being displayed as a document, therefore it cannot be set as the active document.", "value");
            if (value == ActiveTabbedDocument) return;

            if (ActiveTabbedDocument != null)
            {
                ActiveTabbedDocument.DockSituationChanged -= OnActiveTabbedDocumentDockSituationChanged;
                ActiveTabbedDocument.CalculateAllMetricsAndLayout();
            }
            ActiveTabbedDocument = value;
            if (ActiveTabbedDocument != null)
            {
                ActiveTabbedDocument.DockSituationChanged += OnActiveTabbedDocumentDockSituationChanged;
                ActiveTabbedDocument.CalculateAllMetricsAndLayout();
            }
            OnActiveTabbedDocumentChanged(EventArgs.Empty);
        }

        public void SetLayout(string layout)
        {
            EnsureDockSystemContainer();
            EnsureHandles();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(layout);
            GetLayout();
            DockContainer[] orderedDockedDockContainerList = GetOrderedDockedDockContainerList();
            FloatingContainer[] floatingDockContainerList = GetFloatingDockContainerList();
            int num = 0;
            int num2 = 0;
            ArrayList arrayList = new ArrayList(orderedDockedDockContainerList);
            arrayList.AddRange(floatingDockContainerList);
            DocumentContainer documentContainer = null;
            if (this.SerializeTabbedDocuments)
            {
                documentContainer = this.FindDockedContainer(DockStyle.Fill) as DocumentContainer;
            }
            if (documentContainer != null)
            {
                arrayList.Add(documentContainer);
            }
            try
            {
                foreach (DockContainer dockContainer in arrayList)
                {
                    dockContainer.method_2();
                }
                this.DockSystemContainer.SuspendLayout();
                XmlNode xmlNode = xmlDocument.GetElementsByTagName("Layout")[0];
                foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                {
                    if (xmlNode2.NodeType == XmlNodeType.Element && xmlNode2.Name == "Window")
                    {
                        this.ReadWindowProperties(xmlNode2);
                    }
                    else if (xmlNode2.NodeType == XmlNodeType.Element && (xmlNode2.Name == "Container" || xmlNode2.Name == "DocumentContainer") && xmlNode2.HasChildNodes)
                    {
                        DockContainer container = null;
                        if (!(xmlNode2.Name == "DocumentContainer"))
                        {
                            if (xmlNode2.Name == "Container" && num < orderedDockedDockContainerList.Length)
                            {
                                container = orderedDockedDockContainerList[num++];
                            }
                        }
                        else if (documentContainer != null)
                        {
                            container = documentContainer;
                            documentContainer = null;
                        }
                        this.ReadContainerProperties(xmlNode2, container);
                    }
                    else if (xmlNode2.NodeType == XmlNodeType.Element)
                    {
                        if (xmlNode2.Name == "FloatingContainer" && xmlNode2.HasChildNodes)
                        {
                            FloatingContainer container2 = null;
                            if (num2 < floatingDockContainerList.Length)
                            {
                                container2 = floatingDockContainerList[num2++];
                            }
                            this.ReadFloatingContainerProperties(xmlNode2, container2);
                        }
                    }
                }
                for (int i = num; i < orderedDockedDockContainerList.Length; i++)
                {
                    orderedDockedDockContainerList[i].Dispose();
                }
                for (var j = num2; j < floatingDockContainerList.Length; j++)
                {
                    floatingDockContainerList[j].Dispose();
                }
                documentContainer?.Dispose();
                foreach (var control in GetDockControls().Where(c => !c.IsInContainer && c.CloseAction == DockControlCloseAction.Dispose))
                    control.Dispose();
            }
            catch (Exception innerException)
            {
                throw new ArgumentException("The layout information provided could not be interpreted.", innerException);
            }
            finally
            {
                foreach (DockContainer dockContainer2 in arrayList)
                {
                    if (dockContainer2 != null && !dockContainer2.IsDisposed)
                    {
                        dockContainer2.method_3();
                    }
                }
                DockSystemContainer.ResumeLayout();
            }
        }

        private bool ShouldSerializeRenderer()
        {
            return !(_renderer is WhidbeyRenderer);
        }

        internal void UnregisterAutoHideBar(AutoHideBar bar)
        {
            if (_autoHideBars.Contains(bar))
                _autoHideBars.Remove(bar);
        }

        internal void UnregisterDockContainer(DockContainer container)
        {
            if (_containers.Contains(container))
                _containers.Remove(container);
            if (DocumentContainer == container)
                DocumentContainer = null;
        }

        internal void UnregisterWindow(DockControl control)
        {
            _windows.Remove(control.Guid);
            OnDockControlRemoved(new DockControlEventArgs(control));
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DockControl ActiveTabbedDocument { get; private set; }

        [Category("Behavior"), DefaultValue(true), Description("Indicates whether DockContainers can be resized by the user.")]
        public bool AllowDockContainerResize
        {
            get
            {
                return _allowDockContainerResize;
            }
            set
            {
                _allowDockContainerResize = value;
                foreach (var dockContainer in GetOrderedDockedDockContainerList())
                    dockContainer.CalculateAllMetricsAndLayout();
            }
        }

        [Category("Behavior"), DefaultValue(true), Description("Indicates whether the user will be able to use the keyboard to navigate between tabbed documents and dockable windows.")]
        public bool AllowKeyboardNavigation { get; set; } = true;

        [Category("Behavior"), DefaultValue(true), Description("Indicates whether the middle mouse button can be used to close windows by their tabs.")]
        public bool AllowMiddleButtonClosure { get; set; } = true;

        [Category("Behavior"), DefaultValue(false), Description("Indicates whether the user-configured window layout is automatically persisted.")]
        public bool AutoSaveLayout { get; set; }

        [Category("Appearance"), DefaultValue(typeof(Rendering.BorderStyle), "Flat"), Description("The type of border to be drawn around the tabbed document area.")]
        public Rendering.BorderStyle BorderStyle
        {
            get
            {
                return _borderStyle;
            }
            set
            {
                _borderStyle = value;
                if (DocumentContainer != null)
                    DocumentContainer.BorderStyle = _borderStyle;
            }
        }

        [Category("Appearance"), DefaultValue(typeof(DockingHints), "TranslucentFill"), Description("Indicates the type of visual artifacts drawn to the screen to indicate size and position while docking.")]
        public DockingHints DockingHints { get; set; } = DockingHints.TranslucentFill;

        [Category("Behavior"), DefaultValue(typeof(DockingManager), "Whidbey"), Description("Indicates the method of user interaction during a docking operation.")]
        public DockingManager DockingManager { get; set; } = DockingManager.Whidbey;

        [Category("Advanced"), Description("The control that will act as a container for all docked windows. You should rarely, if ever, need to change this property.")]
        public Control DockSystemContainer
        {
            get
            {
                return _dockSystemContainer;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value is DockContainer) throw new ArgumentException("A DockContainer cannot act as a host for a SandDock layout.");
                if (_dockSystemContainer == value) return;
                var arrayList = _containers.Cast<DockContainer>().Where(c => c.Parent != null && c.Parent != value).Cast<Control>().ToArray();
                if (_dockSystemContainer != null)
                    _dockSystemContainer.Resize -= OnDockSystemContainerResize;
                _dockSystemContainer = value;
                if (_dockSystemContainer != null)
                    _dockSystemContainer.Resize += OnDockSystemContainerResize;
                value.Controls.AddRange(arrayList);
            }
        }

        [Browsable(false)]
        public DocumentContainer DocumentContainer { get; private set; }

        [Category("Behavior"), DefaultValue(typeof(DocumentContainerWindowOpenPosition), "Last"), Description("Specifies whether documents are opened at the first position or the last.")]
        public DocumentContainerWindowOpenPosition DocumentOpenPosition { get; set; } = DocumentContainerWindowOpenPosition.Last;

        [Category("Behavior"), DefaultValue(typeof(DocumentOverflowMode), "Scrollable"), Description("Determines how document tabs that overflow past the visible area are accessed.")]
        public DocumentOverflowMode DocumentOverflow
        {
            get
            {
                return _documentOverflow;
            }
            set
            {
                if (_documentOverflow == value) return;
                _documentOverflow = value;
                if (DocumentContainer != null)
                    DocumentContainer.DocumentOverflow = _documentOverflow;
            }
        }

        [Browsable(false), Obsolete("Use the GetDockControls method passing DockSituation.Document instead.")]
        public DockControl[] Documents => GetDockControls(DockSituation.Document);

        [Category("Behavior"), DefaultValue(false), Description("Indicates whether an empty container is left when all tabbed documents have been removed.")]
        public bool EnableEmptyEnvironment { get; set; }

        [Category("Behavior"), DefaultValue(true), Description("Indicates whether tabbed documents can be shown in the centre of the container.")]
        public bool EnableTabbedDocuments
        {
            get
            {
                return _enableTabbedDocuments;
            }
            set
            {
                if (FindDockedContainer(DockStyle.Fill) != null)
                    throw new InvalidOperationException("This property can only be changed when there are no DockControls being shown in the centre of the container.");
                _enableTabbedDocuments = value;
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Indicates whether the close button is displayed inside the active tab.")]
        public bool IntegralClose
        {
            get
            {
                return _integralClose;
            }
            set
            {
                if (_integralClose == value) return;
                _integralClose = value;
                if (DocumentContainer != null)
                    DocumentContainer.IntegralClose = _integralClose;
            }
        }

        [Category("Behavior"), DefaultValue(500), Description("Indicates the maximum size of a docked strip of toolwindows.")]
        public int MaximumDockContainerSize { get; set; } = 500;

        [Category("Behavior"), DefaultValue(30), Description("Indicates the minimum size of a docked strip of toolwindows.")]
        public int MinimumDockContainerSize { get; set; } = 30;

        [Browsable(false)]
        public Form OwnerForm
        {
            get
            {
                return _ownerForm;
            }
            set
            {
                if (_ownerForm != null && _ownerForm == value) return;
                if (_ownerForm != null)
                {
                    _ownerForm.Activated -= OnOwnerFormActivated;
                    _ownerForm.Deactivate -= OnOwnerFormDeactivated;
                    _ownerForm.Load -= OnOwnerFormLoad;
                    _ownerForm.Closing -= OnOwnerFormClosing;
                }
                _ownerForm = value;
                if (_ownerForm != null)
                {
                    _ownerForm.Activated += OnOwnerFormActivated;
                    _ownerForm.Deactivate += OnOwnerFormDeactivated;
                    _ownerForm.Load += OnOwnerFormLoad;
                    _ownerForm.Closing += OnOwnerFormClosing;
                }
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Indicates whether standard validation events are fired when the user changes tabs.")]
        public bool RaiseValidationEvents { get; set; }

        [Category("Appearance"), Description("The renderer used to calculate object metrics and draw contents.")]
        public RendererBase Renderer
        {
            get
            {
                return _renderer;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (_renderer != null)
                {
                    _renderer.MetricsChanged -= OnRendererMetricsChanged;
                    _renderer.Dispose();
                }
                _renderer = value;
                _renderer.MetricsChanged += OnRendererMetricsChanged;
                PropagateNewRenderer();
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Indicates whether window groups will respond when an OLE drag operation occurs over their tabs.")]
        public bool SelectTabsOnDrag
        {
            get
            {
                return _selectTabsOnDrag;
            }
            set
            {
                _selectTabsOnDrag = value;
                foreach (DockContainer dockContainer in _containers)
                    dockContainer.AllowDrop = value;
                foreach (AutoHideBar control in _autoHideBars)
                    control.AllowDrop = value;
            }
        }

        [Category("Serialization"), DefaultValue(false), Description("Indicates whether tabbed document layout will be serialized alongside dockable window layout.")]
        public bool SerializeTabbedDocuments { get; set; }

        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
                if (value == null) return;
                var designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
                if (designerHost?.RootComponent is Form)
                    _ownerForm = (Form)designerHost.RootComponent;
                if (designerHost?.RootComponent is Control && DockSystemContainer == null)
                    DockSystemContainer = FindDockSystemContainer(designerHost, (Control)designerHost.RootComponent);
            }
        }

        public event EventHandler ActiveTabbedDocumentChanged;

        public event DockControlEventHandler DockControlActivated;

        public event DockControlEventHandler DockControlAdded;

        public event DockControlClosingEventHandler DockControlClosing;

        public event DockControlEventHandler DockControlRemoved;

        public event EventHandler DockingFinished;

        public event EventHandler DockingStarted;

        public event ResolveDockControlEventHandler ResolveDockControl;

        public event ActiveFilesListEventHandler ShowActiveFilesList;

        public event ShowControlContextMenuEventHandler ShowControlContextMenu;

        internal ArrayList _containers;

        private readonly ArrayList _autoHideBars;

        private bool _allowDockContainerResize = true;

        private bool _enableTabbedDocuments = true;

        private bool _selectTabsOnDrag;

        private bool _integralClose;

        private Rendering.BorderStyle _borderStyle = Rendering.BorderStyle.Flat;

        private Control _dockSystemContainer;

        private DocumentOverflowMode _documentOverflow = DocumentOverflowMode.Scrollable;

        private Form _ownerForm;

        private readonly Hashtable _windows;

        private RendererBase _renderer;
    }
}
