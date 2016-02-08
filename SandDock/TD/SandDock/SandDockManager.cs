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

    [DefaultEvent("ActiveTabbedDocumentChanged"), Designer("Design.SandDockManagerDesigner"), ToolboxBitmap(typeof(SandDockManager))]
	public class SandDockManager : Component
	{
		public SandDockManager()
		{
			this.rendererBase_0 = new WhidbeyRenderer();
			this.arrayList_0 = new ArrayList();
			this.hashtable_0 = new Hashtable();
			this.arrayList_1 = new ArrayList();
		}

		public static void ActivateProduct(string licenseKey)
		{
			//Class1.ActivateProduct(licenseKey);
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
				DockContainer dockContainer = this.CreateNewDockContainerCore(dockLocation);
				dockContainer.Manager = this;
				DockStyle dockStyle = LayoutUtilities.smethod_6(dockLocation);
				dockContainer.Dock = dockStyle;
				dockContainer.ContentSize = contentSize;
				IntPtr arg_3B_0 = dockContainer.Handle;
				int newIndex;
				if (dockLocation != ContainerDockLocation.Center)
				{
					newIndex = edge == ContainerDockEdge.Inside ? GetInsideControlIndex(DockSystemContainer) : GetOutsideControlIndex(DockSystemContainer, dockStyle);
				}
				else
				{
					newIndex = 0;
				}
				this.DockSystemContainer.Controls.Add(dockContainer);
				this.DockSystemContainer.Controls.SetChildIndex(dockContainer, newIndex);
				foreach (var control2 in
				    this.DockSystemContainer.Controls.Cast<Control>().Select(control => control as PopupContainer))
				{
				    control2?.BringToFront();
				}
				result = dockContainer;
			}
			finally
			{
				this.DockSystemContainer.ResumeLayout();
			}
			return result;
		}

		protected virtual DockContainer CreateNewDockContainerCore(ContainerDockLocation dockLocation)
		{
		    return dockLocation == ContainerDockLocation.Center && EnableTabbedDocuments
		        ? new DocumentContainer()
		        : new DockContainer();
		}

        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				DockContainer[] array = new DockContainer[this.arrayList_0.Count];
				this.arrayList_0.CopyTo(array);
				DockContainer[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DockContainer dockContainer = array2[i];
					dockContainer.Dispose();
				}
				this.arrayList_0.Clear();
				AutoHideBar[] array3 = new AutoHideBar[this.arrayList_1.Count];
				this.arrayList_1.CopyTo(array3);
				AutoHideBar[] array4 = array3;
				for (int j = 0; j < array4.Length; j++)
				{
					AutoHideBar control = array4[j];
					control.Dispose();
				}
				this.arrayList_1.Clear();
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
			DockControl dockControl = (DockControl)this.hashtable_0[guid];
			if (dockControl != null)
			{
				return dockControl;
			}
			ResolveDockControlEventArgs resolveDockControlEventArgs = new ResolveDockControlEventArgs(guid);
			this.OnResolveDockControl(resolveDockControlEventArgs);
			if (resolveDockControlEventArgs.DockControl == null)
			{
				return null;
			}
			resolveDockControlEventArgs.DockControl.Manager = this;
			return resolveDockControlEventArgs.DockControl;
		}

		public DockContainer FindDockContainer(ContainerDockLocation location)
		{
			return FindDockedContainer(LayoutUtilities.smethod_6(location));
		}

		internal DockContainer FindDockedContainer(DockStyle dockStyle) => this.arrayList_0.Cast<DockContainer>().FirstOrDefault(c => c.Dock == dockStyle && !c.IsFloating);

        private Control FindDockSystemContainer(IDesignerHost designerHost, Control parent)
		{
		    var ctl =
		        parent.Controls.Cast<Control>()
		            .FirstOrDefault(
		                c => c.Dock == DockStyle.Fill && c.Site != null && c.Site.DesignMode && !c.Controls.IsReadOnly);
		    return ctl != null ? FindDockSystemContainer(designerHost, ctl) : parent;

   //         IEnumerator enumerator = parent.Controls.GetEnumerator();
			//Control result;
			//try
			//{
			//	while (enumerator.MoveNext())
			//	{
			//	    Control control = (Control)enumerator.Current;
			//	    if (control.Dock == DockStyle.Fill && control.Site != null && control.Site.DesignMode && !control.Controls.IsReadOnly)
			//	    {
			//	        result = this.FindDockSystemContainer(designerHost, control);
			//	        return result;
			//	    }
			//	}
			//    return parent;
			//}
			//finally
			//{
			//	IDisposable disposable = enumerator as IDisposable;
			//	if (disposable != null)
			//	{
			//		disposable.Dispose();
			//	}
			//}
			//return result;
		}

		internal FloatingContainer FindFloatingDockContainer(Guid guid) => GetFloatingDockContainerList().FirstOrDefault(@class => @class.Guid_0 == guid);

	    public DockControl FindMostRecentlyUsedWindow() => FindMostRecentlyUsedWindow((DockSituation)(-1));

	    public DockControl FindMostRecentlyUsedWindow(DockSituation dockSituation) => FindMostRecentlyUsedWindow(dockSituation, null);

	    internal DockControl FindMostRecentlyUsedWindow(DockSituation dockSituation, DockControl notThisOne)
		{
			DateTime t = DateTime.MinValue;
			DockControl result = null;
			DockControl[] dockControls = this.GetDockControls();
			foreach (var dockControl in dockControls.Where(dockControl => dockControl != notThisOne && dockControl.MetaData.LastFocused >= t && (dockSituation == (DockSituation) (-1) || dockControl.DockSituation == dockSituation)))
			{
			    t = dockControl.MetaData.LastFocused;
			    result = dockControl;
			}
	        return result;
		}

		internal AutoHideBar GetAutoHideBar(DockStyle dock)
		{
			if (dock != DockStyle.Fill && dock != DockStyle.None)
			{
				AutoHideBar result;
				foreach (AutoHideBar control in this.arrayList_1)
				{
					if (control.Dock == dock)
					{
						result = control;
						return result;
					}
				}
				this.DockSystemContainer.SuspendLayout();
				try
				{
				    var control2 = new AutoHideBar
				    {
				        Manager = this,
				        Dock = dock,
				        Parent = DockSystemContainer
				    };
				    this.DockSystemContainer.Controls.SetChildIndex(control2, this.GetOutsideControlIndex(this.DockSystemContainer, dock));
					result = control2;
				}
				finally
				{
					this.DockSystemContainer.ResumeLayout();
				}
				return result;
			}
			return null;
		}

		public DockContainer[] GetDockContainers()
		{
			return (DockContainer[])this.arrayList_0.ToArray(typeof(DockContainer));
		}

		internal DockContainer[] GetDockContainers(DockStyle dockStyle)
		{
			if (dockStyle == DockStyle.Fill)
			{
				throw new ArgumentException(nameof(dockStyle));
			}
			if (this.DockSystemContainer != null)
			{
				DockContainer[] array = new DockContainer[this.DockSystemContainer.Controls.Count];
				int num = 0;
				for (int i = this.DockSystemContainer.Controls.Count - 1; i >= 0; i--)
				{
					DockContainer dockContainer = this.DockSystemContainer.Controls[i] as DockContainer;
				    if (dockContainer?.Dock == dockStyle)
				    {
				        array[num++] = dockContainer;
				    }
				}
				DockContainer[] array2 = new DockContainer[num];
				Array.Copy(array, array2, num);
				return array2;
			}
			return new DockContainer[0];
		}

		public DockControl[] GetDockControls()
		{
			var array = new DockControl[this.hashtable_0.Count];
			this.hashtable_0.Values.CopyTo(array, 0);
			return array;
		}

		public DockControl[] GetDockControls(DockSituation dockSituation) => this.hashtable_0.Values.Cast<DockControl>().Where(control => control.DockSituation == dockSituation).ToArray();

	    private FloatingContainer[] GetFloatingDockContainerList() => this.arrayList_0.Cast<DockContainer>().Where(container => container.IsFloating).Cast<FloatingContainer>().ToArray();

	    private int GetInsideControlIndex(Control container)
		{
			int num = int.MaxValue;
			for (int i = container.Controls.Count - 1; i >= 0; i--)
			{
				Control control = container.Controls[i];
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
		    XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
		    xmlTextWriter.WriteStartDocument();
			xmlTextWriter.WriteStartElement("Layout");
		    foreach (var dockControl in this.hashtable_0.Values.Cast<DockControl>().Where(c => c.PersistState))
		        SaveWindowLayout(dockControl, xmlTextWriter);

		    DockContainer[] orderedDockedDockContainerList = this.GetOrderedDockedDockContainerList();
			for (int i = 0; i < orderedDockedDockContainerList.Length; i++)
			{
				DockContainer dockContainer = orderedDockedDockContainerList[i];
				if (dockContainer.LayoutSystem.Boolean_2)
				{
					SaveContainerLayout(dockContainer, xmlTextWriter);
				}
			}
			FloatingContainer[] floatingDockContainerList = this.GetFloatingDockContainerList();
			for (int j = 0; j < floatingDockContainerList.Length; j++)
			{
				DockContainer dockContainer2 = floatingDockContainerList[j];
				if (dockContainer2.LayoutSystem.Boolean_2)
				{
					SaveContainerLayout(dockContainer2, xmlTextWriter);
				}
			}
			DocumentContainer documentContainer = this.FindDockedContainer(DockStyle.Fill) as DocumentContainer;
			if (documentContainer != null && this.SerializeTabbedDocuments && documentContainer.LayoutSystem.Boolean_2)
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
			if (this.DockSystemContainer == null)
			{
				return new DockContainer[0];
			}
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < this.DockSystemContainer.Controls.Count; i++)
			{
				Control control = this.DockSystemContainer.Controls[i];
				if (this.arrayList_0.Contains(control) && !(control is DocumentContainer))
				{
					arrayList.Add(control);
				}
			}
			return (DockContainer[])arrayList.ToArray(typeof(DockContainer));
		}

		private int GetOutsideControlIndex(Control container, DockStyle dockStyle)
		{
			int result = container.Controls.Count;
			for (int i = container.Controls.Count - 1; i >= 0; i--)
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
			var layoutSettings = new LayoutSettings(GetSettingsKey());
			if (!layoutSettings.IsDefault && !string.IsNullOrEmpty(layoutSettings.LayoutXml))
			{
				this.SetLayout(layoutSettings.LayoutXml);
			}
		}

		protected internal virtual void OnActiveTabbedDocumentChanged(EventArgs e) => ActiveTabbedDocumentChanged?.Invoke(this, e);

	    private void OnActiveTabbedDocumentDockSituationChanged(object sender, EventArgs e)
		{
			DockControl dockControl = (DockControl)sender;
			if (dockControl.DockSituation != DockSituation.Document)
			{
				this.SetActiveTabbedDocument(this.FindMostRecentlyUsedWindow(DockSituation.Document));
			}
		}

		protected internal virtual void OnDockControlActivated(DockControlEventArgs e)
		{
		    DockControlActivated?.Invoke(this, e);
		    if (e.DockControl.DockSituation == DockSituation.Document)
			{
				this.SetActiveTabbedDocument(e.DockControl);
			}
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
	        if (this.OwnerForm?.WindowState == FormWindowState.Minimized)
	        {
	            return;
	        }
	        Form form = this.DockSystemContainer?.FindForm();
	        if (form != null)
	        {
	            if (form.WindowState == FormWindowState.Minimized)
	            {
	                return;
	            }
	            if (form.Parent != null)
	            {
	                Form form2 = form.Parent.FindForm();
	                if (form2 != null)
	                {
	                    if (form2.WindowState == FormWindowState.Minimized)
	                    {
	                        return;
	                    }
	                    if (form2.ActiveMdiChild != null && form2.ActiveMdiChild != form)
	                    {
	                        if (form2.ActiveMdiChild.WindowState == FormWindowState.Maximized)
	                        {
	                            return;
	                        }
	                    }
	                }
	            }
	        }
	        Rectangle rectangle = Class7.smethod_1(this.DockSystemContainer);
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
				foreach (DockContainer dockContainer in this.arrayList_0)
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
				foreach (DockContainer dockContainer3 in this.arrayList_0)
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
		    foreach (DockContainer dockContainer in this.arrayList_0.Cast<DockContainer>().Where(dockContainer => !dockContainer.IsFloating))
		    {
		        dockContainer.method_11(sender, e);
		    }
		}

	    private void OnOwnerFormClosing(object sender, CancelEventArgs e)
		{
	        if (AutoSaveLayout)
	            SaveLayout();
		}

		private void OnOwnerFormDeactivated(object sender, EventArgs e)
		{
		    foreach (DockContainer dockContainer in this.arrayList_0.Cast<DockContainer>().Where(dockContainer => !dockContainer.IsFloating))
		    {
		        dockContainer.method_12(sender, e);
		    }
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
			foreach (DockContainer dockContainer in this.arrayList_0)
			{
				dockContainer.method_4();
			}
			foreach (AutoHideBar control in this.arrayList_1)
			{
				control.method_1();
			}
		}

		private void ReadContainerProperties(XmlNode containerNode, DockContainer container)
		{
			DockStyle dockStyle = (DockStyle)int.Parse(containerNode.Attributes["Dock"].Value);
			int contentSize = 0;
			if (containerNode.Attributes["ContentSize"] != null)
			{
				contentSize = int.Parse(containerNode.Attributes["ContentSize"].Value);
			}
			if (container == null)
			{
				container = this.CreateNewDockContainer(LayoutUtilities.smethod_7(dockStyle), ContainerDockEdge.Outside, contentSize);
			}
			container.Dock = dockStyle;
			container.ContentSize = contentSize;
			foreach (XmlNode xmlNode in containerNode.ChildNodes)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (xmlNode.Name == "SplitLayoutSystem")
					{
						SplitLayoutSystem splitLayoutSystem = this.ReadSplitLayoutSystem(xmlNode, container);
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
				controlLayoutSystem.Guid_0 = empty;
			}
			return controlLayoutSystem;
		}

		private void ReadFloatingContainerProperties(XmlNode node, FloatingContainer container)
		{
			Rectangle rectangle_ = this.ConvertStringToRectangle(node.Attributes["Bounds"].Value);
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
			SizeF workingSize = SandDockManager.ConvertStringToSizeF(splitNode.Attributes["WorkingSize"].Value);
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
			if (!this.arrayList_1.Contains(bar))
			{
				this.arrayList_1.Add(bar);
			}
			bar.AllowDrop = this.SelectTabsOnDrag;
		}

		internal void RegisterDockContainer(DockContainer container)
		{
		    if (container is DocumentContainer && this.DocumentContainer != null)
		        throw new InvalidOperationException("Only one DocumentContainer can exist in a SandDock layout.");
		    if (!this.arrayList_0.Contains(container))
			{
				this.arrayList_0.Add(container);
			}
			if (this.DockSystemContainer == null && container.Parent is ContainerControl && !container.IsFloating)
			{
				this.DockSystemContainer = (ContainerControl)container.Parent;
			}
			container.AllowDrop = this.SelectTabsOnDrag;
			if (container is DocumentContainer)
			{
				this.DocumentContainer = (DocumentContainer)container;
				this.DocumentContainer.BorderStyle = this.BorderStyle;
				this.DocumentContainer.DocumentOverflow = DocumentOverflow;
				this.DocumentContainer.IntegralClose = this.IntegralClose;
			}
		}

		internal void RegisterWindow(DockControl control)
		{
			this.hashtable_0[control.Guid] = control;
			this.OnDockControlAdded(new DockControlEventArgs(control));
		}

		internal void ReRegisterWindow(DockControl control, Guid oldGuid)
		{
			if (this.hashtable_0.Contains(oldGuid))
			{
				this.hashtable_0.Remove(oldGuid);
			}
			this.hashtable_0[control.Guid] = control;
		}

		private void SaveContainerLayout(DockContainer container, XmlTextWriter writer)
		{
			if (container is FloatingContainer)
			{
				FloatingContainer @class = (FloatingContainer)container;
				writer.WriteStartElement("FloatingContainer");
				writer.WriteAttributeString("Bounds", this.ConvertRectangleToString(@class.Rectangle_1));
				writer.WriteAttributeString("Guid", @class.Guid_0.ToString());
				this.SaveLayoutSystem(container.LayoutSystem, writer);
				writer.WriteEndElement();
				return;
			}
			if (!(container is DocumentContainer))
			{
				writer.WriteStartElement("Container");
			}
			else
			{
				writer.WriteStartElement("DocumentContainer");
			}
			writer.WriteAttributeString("Dock", ((int)container.Dock).ToString());
			if (container.Dock != DockStyle.Fill)
			{
				if (container.Dock != DockStyle.None)
				{
					writer.WriteAttributeString("ContentSize", container.ContentSize.ToString());
				}
			}
			this.SaveLayoutSystem(container.LayoutSystem, writer);
			writer.WriteEndElement();
		}

		public void SaveLayout()
		{
			new LayoutSettings(this.GetSettingsKey())
			{
				LayoutXml = this.GetLayout()
			}.Save();
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
					writer.WriteAttributeString("Guid", controlLayoutSystem.Guid_0.ToString());
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
					if (layoutSystemBase.Boolean_2)
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
			{
				throw new ArgumentException("The specified window is not currently being displayed as a document, therefore it cannot be set as the active document.", "value");
			}
			if (value != this.dockControl_0)
			{
				if (this.dockControl_0 != null)
				{
					this.dockControl_0.DockSituationChanged -= new EventHandler(this.OnActiveTabbedDocumentDockSituationChanged);
					this.dockControl_0.method_1();
				}
				this.dockControl_0 = value;
				if (this.dockControl_0 != null)
				{
					this.dockControl_0.DockSituationChanged += new EventHandler(this.OnActiveTabbedDocumentDockSituationChanged);
					this.dockControl_0.method_1();
				}
				this.OnActiveTabbedDocumentChanged(EventArgs.Empty);
			}
		}

		public void SetLayout(string layout)
		{
			this.EnsureDockSystemContainer();
			this.EnsureHandles();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(layout);
			this.GetLayout();
			DockContainer[] orderedDockedDockContainerList = this.GetOrderedDockedDockContainerList();
			FloatingContainer[] floatingDockContainerList = this.GetFloatingDockContainerList();
			int num = 0;
			int num2 = 0;
			ArrayList arrayList = new ArrayList(orderedDockedDockContainerList);
			arrayList.AddRange(floatingDockContainerList);
			DocumentContainer documentContainer = null;
			if (this.SerializeTabbedDocuments)
			{
				documentContainer = (this.FindDockedContainer(DockStyle.Fill) as DocumentContainer);
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
				for (int j = num2; j < floatingDockContainerList.Length; j++)
				{
					floatingDockContainerList[j].Dispose();
				}
				if (documentContainer != null)
				{
					documentContainer.Dispose();
				}
				DockControl[] dockControls = this.GetDockControls();
				for (int k = 0; k < dockControls.Length; k++)
				{
					DockControl dockControl = dockControls[k];
					if (!dockControl.Boolean_1)
					{
						if (dockControl.CloseAction == DockControlCloseAction.Dispose)
						{
							dockControl.Dispose();
						}
					}
				}
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
				this.DockSystemContainer.ResumeLayout();
			}
		}

		private bool ShouldSerializeRenderer()
		{
			return !(this.rendererBase_0 is WhidbeyRenderer);
		}

		internal void UnregisterAutoHideBar(AutoHideBar bar)
		{
			if (this.arrayList_1.Contains(bar))
			{
				this.arrayList_1.Remove(bar);
			}
		}

		internal void UnregisterDockContainer(DockContainer container)
		{
			if (this.arrayList_0.Contains(container))
			{
				this.arrayList_0.Remove(container);
			}
			if (this.DocumentContainer == container)
			{
				this.DocumentContainer = null;
			}
		}

		internal void UnregisterWindow(DockControl control)
		{
			this.hashtable_0.Remove(control.Guid);
			this.OnDockControlRemoved(new DockControlEventArgs(control));
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DockControl ActiveTabbedDocument
		{
			get
			{
				return this.dockControl_0;
			}
		}

		[Category("Behavior"), DefaultValue(true), Description("Indicates whether DockContainers can be resized by the user.")]
		public bool AllowDockContainerResize
		{
			get
			{
				return this.bool_0;
			}
			set
			{
				this.bool_0 = value;
				DockContainer[] orderedDockedDockContainerList = this.GetOrderedDockedDockContainerList();
				for (int i = 0; i < orderedDockedDockContainerList.Length; i++)
				{
					DockContainer dockContainer = orderedDockedDockContainerList[i];
					dockContainer.CalculateAllMetricsAndLayout();
				}
			}
		}

		[Category("Behavior"), DefaultValue(true), Description("Indicates whether the user will be able to use the keyboard to navigate between tabbed documents and dockable windows.")]
		public bool AllowKeyboardNavigation
		{
			get
			{
				return this.bool_1;
			}
			set
			{
				this.bool_1 = value;
			}
		}

		[Category("Behavior"), DefaultValue(true), Description("Indicates whether the middle mouse button can be used to close windows by their tabs.")]
		public bool AllowMiddleButtonClosure
		{
			get
			{
				return this.bool_3;
			}
			set
			{
				this.bool_3 = value;
			}
		}

		[Category("Behavior"), DefaultValue(false), Description("Indicates whether the user-configured window layout is automatically persisted.")]
		public bool AutoSaveLayout
		{
			get
			{
				return this.bool_7;
			}
			set
			{
				this.bool_7 = value;
			}
		}

		[Category("Appearance"), DefaultValue(typeof(TD.SandDock.Rendering.BorderStyle), "Flat"), Description("The type of border to be drawn around the tabbed document area.")]
		public TD.SandDock.Rendering.BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle_0;
			}
			set
			{
				this.borderStyle_0 = value;
				if (this.DocumentContainer != null)
				{
					this.DocumentContainer.BorderStyle = this.borderStyle_0;
				}
			}
		}

		[Category("Appearance"), DefaultValue(typeof(DockingHints), "TranslucentFill"), Description("Indicates the type of visual artifacts drawn to the screen to indicate size and position while docking.")]
		public DockingHints DockingHints
		{
			get
			{
				return this.dockingHints_0;
			}
			set
			{
				this.dockingHints_0 = value;
			}
		}

		[Category("Behavior"), DefaultValue(typeof(DockingManager), "Whidbey"), Description("Indicates the method of user interaction during a docking operation.")]
		public DockingManager DockingManager
		{
			get
			{
				return this.dockingManager_0;
			}
			set
			{
				this.dockingManager_0 = value;
			}
		}

		[Category("Advanced"), Description("The control that will act as a container for all docked windows. You should rarely, if ever, need to change this property.")]
		public Control DockSystemContainer
		{
			get
			{
				return this.control_0;
			}
			set
			{
			    if (value == null)
			        throw new ArgumentNullException(nameof(value));
			    if (value is DockContainer)
			        throw new ArgumentException("A DockContainer cannot act as a host for a SandDock layout.");
			    if (value != this.control_0)
				{
					ArrayList arrayList = new ArrayList();
					foreach (DockContainer dockContainer in this.arrayList_0)
					{
					    if (dockContainer.Parent != null && dockContainer.Parent != value)
					    {
					        arrayList.Add(dockContainer);
					    }
					}
					if (this.control_0 != null)
					{
						this.control_0.Resize -= this.OnDockSystemContainerResize;
					}
					this.control_0 = value;
					if (this.control_0 != null)
					{
						this.control_0.Resize += this.OnDockSystemContainerResize;
					}
					value.Controls.AddRange((Control[])arrayList.ToArray(typeof(Control)));
					return;
				}
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
				return this.documentOverflowMode_0;
			}
			set
			{
				if (value != this.documentOverflowMode_0)
				{
					this.documentOverflowMode_0 = value;
					if (this.DocumentContainer != null)
					{
						this.DocumentContainer.DocumentOverflow = this.DocumentOverflow;
					}
				}
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
				return this.bool_2;
			}
			set
			{
				if (this.FindDockedContainer(DockStyle.Fill) != null)
				{
					throw new InvalidOperationException("This property can only be changed when there are no DockControls being shown in the centre of the container.");
				}
				this.bool_2 = value;
			}
		}

		[Category("Behavior"), DefaultValue(false), Description("Indicates whether the close button is displayed inside the active tab.")]
		public bool IntegralClose
		{
			get
			{
				return this.bool_8;
			}
			set
			{
				if (value != this.bool_8)
				{
					this.bool_8 = value;
					if (this.DocumentContainer != null)
					{
						this.DocumentContainer.IntegralClose = this.IntegralClose;
					}
				}
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
				return this.form_0;
			}
			set
			{
				if (this.form_0 != null && this.form_0 == value)
				{
					return;
				}
				if (this.form_0 != null)
				{
					this.form_0.Activated -= new EventHandler(this.OnOwnerFormActivated);
					this.form_0.Deactivate -= new EventHandler(this.OnOwnerFormDeactivated);
					this.form_0.Load -= new EventHandler(this.OnOwnerFormLoad);
					this.form_0.Closing -= new CancelEventHandler(this.OnOwnerFormClosing);
				}
				this.form_0 = value;
				if (this.form_0 != null)
				{
					this.form_0.Activated += new EventHandler(this.OnOwnerFormActivated);
					this.form_0.Deactivate += new EventHandler(this.OnOwnerFormDeactivated);
					this.form_0.Load += new EventHandler(this.OnOwnerFormLoad);
					this.form_0.Closing += new CancelEventHandler(this.OnOwnerFormClosing);
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
				return this.rendererBase_0;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (this.rendererBase_0 != null)
				{
					this.rendererBase_0.MetricsChanged -= this.OnRendererMetricsChanged;
					this.rendererBase_0.Dispose();
				}
				this.rendererBase_0 = value;
				this.rendererBase_0.MetricsChanged += this.OnRendererMetricsChanged;
				this.PropagateNewRenderer();
			}
		}

		[Category("Behavior"), DefaultValue(false), Description("Indicates whether window groups will respond when an OLE drag operation occurs over their tabs.")]
		public bool SelectTabsOnDrag
		{
			get
			{
				return this.bool_6;
			}
			set
			{
				this.bool_6 = value;
			    foreach (DockContainer dockContainer in this.arrayList_0)
			        dockContainer.AllowDrop = value;
			    foreach (AutoHideBar control in this.arrayList_1)
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
				if (value != null)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null && designerHost.RootComponent is Form)
					{
						this.form_0 = (Form)designerHost.RootComponent;
					}
					if (designerHost != null && designerHost.RootComponent is Control && this.DockSystemContainer == null)
					{
						this.DockSystemContainer = this.FindDockSystemContainer(designerHost, (Control)designerHost.RootComponent);
					}
				}
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

		//private ActiveFilesListEventHandler activeFilesListEventHandler_0;

		internal ArrayList arrayList_0;

		internal ArrayList arrayList_1;

		private bool bool_0 = true;

		private bool bool_1 = true;

		private bool bool_2 = true;

		private bool bool_3 = true;

        private bool bool_6;

		private bool bool_7;

		private bool bool_8;

        private TD.SandDock.Rendering.BorderStyle borderStyle_0 = TD.SandDock.Rendering.BorderStyle.Flat;

		private Control control_0;

		//private DockControlClosingEventHandler dockControlClosingEventHandler_0;

		//private DockControlEventHandler dockControlEventHandler_0;

		//private DockControlEventHandler dockControlEventHandler_1;

		//private DockControlEventHandler dockControlEventHandler_2;

		private DockControl dockControl_0;

		private DockingHints dockingHints_0 = DockingHints.TranslucentFill;

		private DockingManager dockingManager_0 = DockingManager.Whidbey;

        private DocumentOverflowMode documentOverflowMode_0 = DocumentOverflowMode.Scrollable;

		//private EventHandler eventHandler_0;

		//private EventHandler eventHandler_1;

		//private EventHandler eventHandler_2;

		private Form form_0;

		private Hashtable hashtable_0;

        private RendererBase rendererBase_0;

		//private ResolveDockControlEventHandler resolveDockControlEventHandler_0;

		//private ShowControlContextMenuEventHandler showControlContextMenuEventHandler_0;
	}
}
