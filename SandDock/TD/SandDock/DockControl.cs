using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using TD.SandDock.Design;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
	[DefaultEvent("Closing"), Designer(typeof(DockControlDesigner)), ToolboxItem(false)]
	public abstract class DockControl : ContainerControl
	{
		protected DockControl()
		{
			if (DockControl.image_0 == null)
			{
				DockControl.image_0 = Image.FromStream(typeof(DockControl).Assembly.GetManifestResourceStream("TD.SandDock.sanddock.png"));
			}
			this.windowMetaData_0 = new WindowMetaData();
			this.dockingRules_0 = this.CreateDockingRules();
			if (this.dockingRules_0 == null)
			{
				throw new InvalidOperationException();
			}
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			base.SetStyle(ControlStyles.Selectable, false);
			this.BackColor = SystemColors.Control;
			this.size_0 = this.DefaultSize;
		}

		protected DockControl(SandDockManager manager, Control control, string text) : this()
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (text == null)
			{
				text = string.Empty;
			}
			this.Manager = manager;
			if (control is Form)
			{
				Form form = (Form)control;
				form.TopLevel = false;
				form.FormBorderStyle = FormBorderStyle.None;
			}
			if (control != null)
			{
				base.SuspendLayout();
				base.Controls.Add(control);
				control.Dock = DockStyle.Fill;
				control.BringToFront();
				base.ResumeLayout();
				control.Visible = true;
			}
			if (text != null)
			{
				this.Text = text;
			}
		}

		public void Activate()
		{
			if (this.LayoutSystem == null || base.Parent == null)
			{
				return;
			}
			if (this.LayoutSystem.SelectedControl != this)
			{
				this.LayoutSystem.SelectedControl = this;
				if (this.LayoutSystem.SelectedControl != this)
				{
					return;
				}
			}
			if (this.DockSituation == DockSituation.Floating)
			{
				this.Class5_0.method_23();
			}
			if (this.IsOpen)
			{
				this.bool_1 = true;
				try
				{
					IContainerControl containerControl = base.Parent.GetContainerControl();
					containerControl.ActiveControl = base.ActiveControl;
					if (!base.ContainsFocus)
					{
						if (this.PrimaryControl == null)
						{
							base.SelectNextControl(this, true, true, true, true);
						}
						else
						{
							this.PrimaryControl.Focus();
						}
						if (!base.ContainsFocus)
						{
							if (base.Controls.Count == 1)
							{
								base.Controls[0].Focus();
							}
							else
							{
								base.Focus();
							}
						}
					}
				}
				finally
				{
					this.bool_1 = false;
				}
				this.method_3();
				return;
			}
		}

		public bool Close()
		{
			return this.method_14(false);
		}

		protected abstract DockingRules CreateDockingRules();

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.controlLayoutSystem_0 != null)
				{
					LayoutUtilities.smethod_11(this);
				}
				if (this.Manager != null)
				{
					this.Manager = null;
				}
			}
			base.Dispose(disposing);
		}

		public void DockInNewContainer(ContainerDockLocation dockLocation, ContainerDockEdge edge)
		{
			this.method_10();
			this.Remove();
			DockContainer dockContainer = this.Manager.CreateNewDockContainer(dockLocation, edge, this.windowMetaData_0.DockedContentSize);
			ControlLayoutSystem layoutSystem = dockContainer.CreateNewLayoutSystem(this, this.FloatingSize);
			dockContainer.LayoutSystem.LayoutSystems.Add(layoutSystem);
		}

		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use the OpenWith method instead.")]
		public void DockNextTo(DockControl existingWindow)
		{
			this.OpenWith(existingWindow);
		}

		protected virtual Point GetDefaultFloatingLocation()
		{
			Point point;
			if (this.Boolean_1)
			{
				point = this.LayoutSystem.DockContainer.PointToScreen(this.LayoutSystem.Bounds.Location);
				point -= new Size(SystemInformation.CaptionHeight, SystemInformation.CaptionHeight);
			}
			else
			{
				Screen screen = (this.Manager.DockSystemContainer == null) ? Screen.PrimaryScreen : Screen.FromControl(this.Manager.DockSystemContainer);
				Rectangle workingArea = screen.WorkingArea;
				point = new Point(workingArea.X + workingArea.Width / 2 - this.size_0.Width / 2, workingArea.Y + workingArea.Height / 2 - this.size_0.Height / 2);
			}
			return point;
		}

		public Form GetFloatingForm()
		{
			if (this.DockSituation == DockSituation.Floating && base.Parent != null)
			{
				return base.Parent.Parent as Form;
			}
			return null;
		}

		internal void method_0(bool bool_8)
		{
			base.SetVisibleCore(bool_8);
		}

		internal void method_1()
		{
			if (this.LayoutSystem != null)
			{
				this.LayoutSystem.method_16();
			}
		}

		private void method_10()
		{
			this.method_9();
			if (this.Manager.DockSystemContainer == null)
			{
				throw new InvalidOperationException("The SandDockManager associated with this DockControl does not have its DockSystemContainer property set.");
			}
		}

		internal Rectangle method_11()
		{
			this.method_9();
			if (this.point_0.X == -1)
			{
				if (this.point_0.Y == -1)
				{
					this.point_0 = this.GetDefaultFloatingLocation();
				}
			}
			return new Rectangle(this.point_0, this.size_0);
		}

		internal void method_12(bool bool_8)
		{
			if (this.LayoutSystem != null)
			{
				if (this.LayoutSystem.SelectedControl != this)
				{
					this.LayoutSystem.SelectedControl = this;
					if (this.LayoutSystem.SelectedControl != this)
					{
						return;
					}
				}
				if (this.LayoutSystem.Control0_0 != null)
				{
					this.LayoutSystem.Control0_0.method_7(this, true, bool_8);
					return;
				}
			}
			if (bool_8)
			{
				this.Activate();
			}
		}

		internal bool method_13(ContainerDockLocation containerDockLocation_0)
		{
			switch (containerDockLocation_0)
			{
			case ContainerDockLocation.Left:
				return this.DockingRules.AllowDockLeft;
			case ContainerDockLocation.Right:
				return this.DockingRules.AllowDockRight;
			case ContainerDockLocation.Top:
				return this.DockingRules.AllowDockTop;
			case ContainerDockLocation.Bottom:
				return this.DockingRules.AllowDockBottom;
			}
			return this.DockingRules.AllowTab;
		}

		internal bool method_14(bool bool_8)
		{
			DockControlClosingEventArgs dockControlClosingEventArgs = new DockControlClosingEventArgs(this, false);
			if (this.Manager != null)
			{
				this.Manager.OnDockControlClosing(dockControlClosingEventArgs);
			}
			if (!dockControlClosingEventArgs.Cancel)
			{
				this.OnClosing(dockControlClosingEventArgs);
			}
			if (!dockControlClosingEventArgs.Cancel)
			{
				LayoutUtilities.smethod_11(this);
				this.OnClosed(EventArgs.Empty);
				if (this.CloseAction == DockControlCloseAction.Dispose)
				{
					base.Dispose();
				}
				return true;
			}
			return false;
		}

		internal void method_15(ControlLayoutSystem controlLayoutSystem_1, int int_3)
		{
			if (this.controlLayoutSystem_0 != controlLayoutSystem_1)
			{
				LayoutUtilities.smethod_11(this);
				controlLayoutSystem_1.Controls.Insert(int_3, this);
			}
			else
			{
				controlLayoutSystem_1.Controls.SetChildIndex(this, int_3);
			}
			controlLayoutSystem_1.SelectedControl = this;
		}

		internal void method_16(ControlLayoutSystem controlLayoutSystem_1)
		{
			this.controlLayoutSystem_0 = controlLayoutSystem_1;
		}

		private void method_17(DockSituation dockSituation_1)
		{
			if (this.bool_2)
			{
				throw new InvalidOperationException("The requested operation is not valid on a window that is currently engaged in an activity further up the call stack. Consider using BeginInvoke to postpone the operation until the stack has unwound.");
			}
			if (dockSituation_1 != this.dockSituation_0)
			{
				this.dockSituation_0 = dockSituation_1;
				this.bool_2 = true;
				try
				{
					this.OnDockSituationChanged(EventArgs.Empty);
				}
				finally
				{
					this.bool_2 = false;
				}
			}
		}

		internal void method_2()
		{
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}

		private void method_3()
		{
			if (!this.bool_1)
			{
				if (this.Manager == null || this.Manager.DocumentContainer == null || !this.Manager.DocumentContainer.Boolean_3)
				{
					this.MetaData.method_0(DateTime.Now);
				}
				if (this.Manager != null)
				{
					this.Manager.OnDockControlActivated(new DockControlEventArgs(this));
				}
			}
		}

		internal void method_4(DockContainer dockContainer_0)
		{
			if (dockContainer_0 != null && dockContainer_0.Manager != null)
			{
				if (dockContainer_0.Manager != this.Manager)
				{
					this.Manager = dockContainer_0.Manager;
				}
			}
			this.method_5();
		}

		internal void method_5()
		{
			DockSituation dockSituation;
			if (this.LayoutSystem != null && this.LayoutSystem.DockContainer != null)
			{
				dockSituation = LayoutUtilities.smethod_2(this.LayoutSystem.DockContainer);
			}
			else
			{
				dockSituation = DockSituation.None;
			}
			if (dockSituation != DockSituation.None)
			{
				this.windowMetaData_0.method_3(dockSituation);
			}
			Class18 @class = null;
			switch (dockSituation)
			{
			case DockSituation.Docked:
				@class = this.windowMetaData_0.Class19_0;
				this.windowMetaData_0.method_4(DockSituation.Docked);
				this.windowMetaData_0.method_1(LayoutUtilities.smethod_7(this.LayoutSystem.DockContainer.Dock));
				this.windowMetaData_0.method_2(this.LayoutSystem.DockContainer.ContentSize);
				if (this.Manager != null)
				{
					DockContainer[] dockContainers = this.Manager.GetDockContainers(this.LayoutSystem.DockContainer.Dock);
					this.windowMetaData_0.Class19_0.Int32_3 = dockContainers.Length;
					this.windowMetaData_0.Class19_0.Int32_2 = Array.IndexOf<DockContainer>(dockContainers, this.LayoutSystem.DockContainer);
				}
				break;
			case DockSituation.Document:
				@class = this.windowMetaData_0.Class18_0;
				this.windowMetaData_0.method_4(DockSituation.Document);
				break;
			case DockSituation.Floating:
				@class = this.windowMetaData_0.Class18_1;
				this.windowMetaData_0.method_5(((Class5)this.LayoutSystem.DockContainer).Guid_0);
				break;
			}
			if (@class != null)
			{
				this.method_6(@class);
			}
			this.method_17(dockSituation);
		}

		private void method_6(Class18 class18_0)
		{
			if (this.LayoutSystem != null)
			{
				class18_0.Guid_0 = this.LayoutSystem.Guid_0;
				class18_0.Int32_1 = this.LayoutSystem.Controls.IndexOf(this);
				class18_0.SizeF_0 = this.LayoutSystem.WorkingSize;
				class18_0.Int32_0 = LayoutUtilities.smethod_5(this.LayoutSystem);
			}
		}

		private void method_7()
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		internal void method_8()
		{
			base.CreateControl();
		}

		private void method_9()
		{
			this.method_7();
			if (this.sandDockManager_0 == null)
			{
				throw new InvalidOperationException("No SandDockManager is associated with this DockControl. To create an association, set the Manager property.");
			}
		}

		protected internal virtual void OnAutoHidePopupClosed(EventArgs e)
		{
			if (this.eventHandler_3 != null)
			{
				this.eventHandler_3(this, e);
			}
		}

		protected internal virtual void OnAutoHidePopupOpened(EventArgs e)
		{
			if (this.eventHandler_2 != null)
			{
				this.eventHandler_2(this, e);
			}
		}

		protected internal virtual void OnClosed(EventArgs e)
		{
			if (this.eventHandler_0 != null)
			{
				this.eventHandler_0(this, e);
			}
		}

		protected internal virtual void OnClosing(DockControlClosingEventArgs e)
		{
			if (this.dockControlClosingEventHandler_0 != null)
			{
				this.dockControlClosingEventHandler_0(this, e);
			}
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this.OnLoad(EventArgs.Empty);
		}

		protected virtual void OnDockSituationChanged(EventArgs e)
		{
			if (this.eventHandler_4 != null)
			{
				this.eventHandler_4(this, e);
			}
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if (this.LayoutSystem != null)
			{
				this.LayoutSystem.Boolean_1 = true;
			}
			this.method_3();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (!this.Boolean_0)
			{
				this.method_1();
			}
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			if (this.LayoutSystem != null)
			{
				this.LayoutSystem.Boolean_1 = false;
			}
		}

		protected virtual void OnLoad(EventArgs e)
		{
			if (this.eventHandler_1 != null)
			{
				this.eventHandler_1(this, e);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DockControl.smethod_0(this, e.Graphics, this.borderStyle_0);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			switch (this.BorderStyle)
			{
			case TD.SandDock.Rendering.BorderStyle.Flat:
			case TD.SandDock.Rendering.BorderStyle.RaisedThin:
			case TD.SandDock.Rendering.BorderStyle.SunkenThin:
				clientRectangle.Inflate(-1, -1);
				break;
			case TD.SandDock.Rendering.BorderStyle.RaisedThick:
			case TD.SandDock.Rendering.BorderStyle.SunkenThick:
				clientRectangle.Inflate(-2, -2);
				break;
			}
			Color backColor = this.BackColor;
			Color transparent = Color.Transparent;
			if (this.Manager != null)
			{
				this.Manager.Renderer.ModifyDefaultWindowColors(this, ref backColor, ref transparent);
			}
			if (clientRectangle != base.ClientRectangle)
			{
				e.Graphics.SetClip(clientRectangle);
			}
			if (this.BackgroundImage == null)
			{
				Class16.smethod_0(e.Graphics, backColor);
				return;
			}
			base.OnPaintBackground(e);
		}

		protected internal virtual void OnTabDoubleClick()
		{
			switch (this.DockSituation)
			{
			case DockSituation.Docked:
			case DockSituation.Document:
				if (this.DockingRules.AllowFloat)
				{
					this.OpenFloating(WindowOpenMethod.OnScreenActivate);
					return;
				}
				break;
			case DockSituation.Floating:
				if (this.windowMetaData_0.LastFixedDockSituation == DockSituation.Docked && this.method_13(this.windowMetaData_0.LastFixedDockSide))
				{
					this.OpenDocked(WindowOpenMethod.OnScreenActivate);
					return;
				}
				if (this.windowMetaData_0.LastFixedDockSituation == DockSituation.Document && this.method_13(ContainerDockLocation.Center))
				{
					this.OpenDocument(WindowOpenMethod.OnScreenActivate);
				}
				break;
			default:
				return;
			}
		}

		public abstract void Open();

		public void Open(WindowOpenMethod openMethod)
		{
			this.method_9();
			if (this.DockSituation == DockSituation.None)
			{
				switch (this.windowMetaData_0.LastOpenDockSituation)
				{
				case DockSituation.Docked:
					this.OpenDocked(openMethod);
					return;
				case DockSituation.Document:
					this.OpenDocument(openMethod);
					return;
				case DockSituation.Floating:
					this.OpenFloating(openMethod);
					return;
				}
			}
			if (openMethod != WindowOpenMethod.OnScreen)
			{
				this.method_12(openMethod == WindowOpenMethod.OnScreenActivate);
			}
		}

		public void OpenBeside(DockControl existingWindow, DockSide side)
		{
			if (existingWindow == null)
			{
				throw new ArgumentNullException();
			}
			if (existingWindow == this)
			{
				return;
			}
			if (existingWindow.DockSituation != DockSituation.None)
			{
				this.Remove();
				existingWindow.LayoutSystem.SplitForLayoutSystem(new ControlLayoutSystem(this.MetaData.Class19_0.SizeF_0, new DockControl[]
				{
					this
				}, this), side);
				return;
			}
			throw new InvalidOperationException("The specified window is not open.");
		}

		public void OpenDocked()
		{
			this.OpenDocked(this.windowMetaData_0.LastFixedDockSide);
		}

		public void OpenDocked(ContainerDockLocation dockLocation)
		{
			if (dockLocation == this.windowMetaData_0.LastFixedDockSide)
			{
				this.OpenDocked(WindowOpenMethod.OnScreenSelect);
				return;
			}
			this.OpenDocked(dockLocation, WindowOpenMethod.OnScreenSelect);
		}

		public void OpenDocked(WindowOpenMethod openMethod)
		{
			this.method_9();
			this.method_8();
			if (this.DockSituation == DockSituation.Docked)
			{
				return;
			}
			this.Remove();
			ControlLayoutSystem controlLayoutSystem = LayoutUtilities.smethod_4(this.Manager, DockSituation.Docked, this.windowMetaData_0.Class19_0);
			if (controlLayoutSystem != null)
			{
				controlLayoutSystem.Controls.Insert(Math.Min(this.windowMetaData_0.Class19_0.Int32_1, controlLayoutSystem.Controls.Count), this);
				if (openMethod != WindowOpenMethod.OnScreen)
				{
					this.method_12(openMethod == WindowOpenMethod.OnScreenActivate);
				}
				return;
			}
			Struct0 @struct = LayoutUtilities.smethod_14(this.Manager, this.windowMetaData_0);
			controlLayoutSystem = @struct.splitLayoutSystem_0.DockContainer.CreateNewLayoutSystem(this, this.windowMetaData_0.Class19_0.SizeF_0);
			if (this.MetaData.Class19_0.Guid_0 == Guid.Empty)
			{
				this.MetaData.Class19_0.Guid_0 = Guid.NewGuid();
			}
			controlLayoutSystem.Guid_0 = this.MetaData.Class19_0.Guid_0;
			@struct.splitLayoutSystem_0.LayoutSystems.Insert(@struct.int_0, controlLayoutSystem);
			if (openMethod != WindowOpenMethod.OnScreen)
			{
				this.method_12(openMethod == WindowOpenMethod.OnScreenActivate);
			}
		}

		public void OpenDocked(ContainerDockLocation dockLocation, WindowOpenMethod openMethod)
		{
			if (dockLocation == ContainerDockLocation.Center)
			{
				this.OpenDocument(openMethod);
				return;
			}
			this.method_9();
			if (this.DockSituation == DockSituation.Docked)
			{
				if (this.windowMetaData_0.LastFixedDockSide == dockLocation)
				{
					return;
				}
			}
			this.Remove();
			this.windowMetaData_0.method_1(dockLocation);
			this.windowMetaData_0.Class19_0.Guid_0 = Guid.Empty;
			this.windowMetaData_0.Class19_0.Int32_0 = new int[0];
			this.OpenDocked(openMethod);
		}

		public void OpenDocument(WindowOpenMethod openMethod)
		{
			this.method_9();
			this.method_8();
			if (this.DockSituation == DockSituation.Document)
			{
				return;
			}
			this.Remove();
			ControlLayoutSystem controlLayoutSystem = LayoutUtilities.smethod_4(this.Manager, DockSituation.Document, this.windowMetaData_0.Class18_0);
			if (controlLayoutSystem != null)
			{
				controlLayoutSystem.Controls.Insert(Math.Min(this.windowMetaData_0.Class19_0.Int32_1, controlLayoutSystem.Controls.Count), this);
				if (openMethod != WindowOpenMethod.OnScreen)
				{
					this.method_12(openMethod == WindowOpenMethod.OnScreenActivate);
				}
				return;
			}
			DockContainer dockContainer = this.Manager.FindDockContainer(ContainerDockLocation.Center);
			if (dockContainer == null)
			{
				dockContainer = this.Manager.CreateNewDockContainer(ContainerDockLocation.Center, ContainerDockEdge.Inside, this.MetaData.DockedContentSize);
			}
			controlLayoutSystem = LayoutUtilities.FindControlLayoutSystem(dockContainer);
			if (controlLayoutSystem == null)
			{
				controlLayoutSystem = dockContainer.CreateNewLayoutSystem(this, this.MetaData.Class18_0.SizeF_0);
				dockContainer.LayoutSystem.LayoutSystems.Add(controlLayoutSystem);
			}
			else if (this.Manager.DocumentOpenPosition != DocumentContainerWindowOpenPosition.First)
			{
				controlLayoutSystem.Controls.Add(this);
			}
			else
			{
				controlLayoutSystem.Controls.Insert(0, this);
			}
			if (openMethod != WindowOpenMethod.OnScreen)
			{
				this.method_12(openMethod == WindowOpenMethod.OnScreenActivate);
			}
		}

		public void OpenFloating()
		{
			this.OpenFloating(WindowOpenMethod.OnScreenActivate);
		}

		public void OpenFloating(WindowOpenMethod openMethod)
		{
			this.method_9();
			this.method_8();
			if (this.DockSituation == DockSituation.Floating)
			{
				return;
			}
			Rectangle rectangle_ = this.method_11();
			this.Remove();
			ControlLayoutSystem controlLayoutSystem = LayoutUtilities.smethod_4(this.Manager, DockSituation.Floating, this.MetaData.Class18_1);
			if (controlLayoutSystem != null)
			{
				controlLayoutSystem.Controls.Insert(Math.Min(this.MetaData.Class18_1.Int32_1, controlLayoutSystem.Controls.Count), this);
				if (openMethod != WindowOpenMethod.OnScreen)
				{
					this.method_12(openMethod == WindowOpenMethod.OnScreenActivate);
				}
				return;
			}
			Class5 @class = this.Manager.FindFloatingDockContainer(this.MetaData.LastFloatingWindowGuid);
			if (@class != null)
			{
				Struct0 @struct = LayoutUtilities.smethod_15(@class, this.MetaData.Class18_1.Int32_0);
				controlLayoutSystem = @struct.splitLayoutSystem_0.DockContainer.CreateNewLayoutSystem(this, this.MetaData.Class18_1.SizeF_0);
				if (this.MetaData.Class18_1.Guid_0 == Guid.Empty)
				{
					this.MetaData.Class18_1.Guid_0 = Guid.NewGuid();
				}
				controlLayoutSystem.Guid_0 = this.MetaData.Class18_1.Guid_0;
				@struct.splitLayoutSystem_0.LayoutSystems.Insert(@struct.int_0, controlLayoutSystem);
				return;
			}
			if (this.windowMetaData_0.LastFloatingWindowGuid == Guid.Empty)
			{
				this.windowMetaData_0.method_5(Guid.NewGuid());
			}
			@class = new Class5(this.Manager, this.windowMetaData_0.LastFloatingWindowGuid);
			controlLayoutSystem = @class.CreateNewLayoutSystem(this, this.windowMetaData_0.Class18_1.SizeF_0);
			if (this.MetaData.Class18_1.Guid_0 == Guid.Empty)
			{
				this.MetaData.Class18_1.Guid_0 = Guid.NewGuid();
			}
			controlLayoutSystem.Guid_0 = this.MetaData.Class18_1.Guid_0;
			@class.LayoutSystem.LayoutSystems.Add(controlLayoutSystem);
			@class.method_19(rectangle_, true, openMethod == WindowOpenMethod.OnScreenActivate);
		}

		public void OpenFloating(Rectangle bounds, WindowOpenMethod openMethod)
		{
			this.method_9();
			this.Remove();
			this.MetaData.method_5(Guid.Empty);
			this.MetaData.Class18_1.Guid_0 = Guid.Empty;
			this.FloatingLocation = bounds.Location;
			this.FloatingSize = bounds.Size;
			this.OpenFloating(openMethod);
		}

		public void OpenWith(DockControl existingWindow)
		{
			if (existingWindow == null)
			{
				throw new ArgumentNullException();
			}
			if (existingWindow == this)
			{
				return;
			}
			if (existingWindow.DockSituation != DockSituation.None)
			{
				this.Remove();
				existingWindow.LayoutSystem.Controls.Add(this);
				return;
			}
			throw new InvalidOperationException("The specified window is not open.");
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (this.controlLayoutSystem_0 != null && this.AllowKeyboardNavigation)
			{
				if (keyData == (Keys.LButton | Keys.Space | Keys.Control))
				{
					int num = this.controlLayoutSystem_0.Controls.IndexOf(this);
					num--;
					if (num < 0)
					{
						num = this.controlLayoutSystem_0.Controls.Count - 1;
					}
					this.controlLayoutSystem_0.SelectedControl = this.controlLayoutSystem_0.Controls[num];
					if (this.controlLayoutSystem_0.SelectedControl == this.controlLayoutSystem_0.Controls[num])
					{
						this.controlLayoutSystem_0.Controls[num].Activate();
					}
					return true;
				}
				if (keyData == (Keys.RButton | Keys.Space | Keys.Control))
				{
					int num2 = this.controlLayoutSystem_0.Controls.IndexOf(this);
					num2++;
					if (num2 >= this.controlLayoutSystem_0.Controls.Count)
					{
						num2 = 0;
					}
					this.controlLayoutSystem_0.SelectedControl = this.controlLayoutSystem_0.Controls[num2];
					if (this.controlLayoutSystem_0.SelectedControl == this.controlLayoutSystem_0.Controls[num2])
					{
						this.controlLayoutSystem_0.Controls[num2].Activate();
					}
					return true;
				}
				if (keyData == (Keys.LButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17 | Keys.Alt) && this.controlLayoutSystem_0.IsInContainer)
				{
					this.controlLayoutSystem_0.DockContainer.method_0(new ShowControlContextMenuEventArgs(this, new Point(0, 0), ContextMenuContext.Keyboard));
					return true;
				}
				if (keyData == Keys.Escape)
				{
					if (this.Manager != null)
					{
						if (this.DockSituation != DockSituation.Document)
						{
							if (this.Manager.OwnerForm != null)
							{
								this.Manager.OwnerForm.Activate();
							}
							DockControl dockControl = this.Manager.FindMostRecentlyUsedWindow(DockSituation.Document);
							if (dockControl != null)
							{
								dockControl.Activate();
							}
							return true;
						}
					}
				}
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		public void Remove()
		{
			LayoutUtilities.smethod_11(this);
		}

		public void SetPositionMetaData(DockSituation dockSituation)
		{
			if (this.DockSituation != DockSituation.None)
			{
				throw new InvalidOperationException("This operation is only valid when the window is not currently open.");
			}
			if (dockSituation == DockSituation.None)
			{
				throw new ArgumentException("dockSituation");
			}
			this.windowMetaData_0.method_3(dockSituation);
			this.windowMetaData_0.method_4(dockSituation);
		}

		public void SetPositionMetaData(DockSituation dockSituation, ContainerDockLocation dockLocation)
		{
			if (this.DockSituation != DockSituation.None)
			{
				throw new InvalidOperationException("This operation is only valid when the window is not currently open.");
			}
			if (dockSituation == DockSituation.None)
			{
				throw new ArgumentException("dockSituation");
			}
			if (dockLocation != ContainerDockLocation.Center)
			{
				this.windowMetaData_0.method_3(dockSituation);
				if (dockSituation != DockSituation.Document)
				{
					if (dockSituation != DockSituation.Docked)
					{
						goto IL_54;
					}
				}
				this.windowMetaData_0.method_4(dockSituation);
				IL_54:
				this.windowMetaData_0.method_1(dockLocation);
				return;
			}
			throw new ArgumentException("dockLocation");
		}

		private bool ShouldSerializeDockingRules()
		{
			DockingRules dockingRules = this.CreateDockingRules();
			if (dockingRules.AllowDockTop == this.DockingRules.AllowDockTop && dockingRules.AllowDockBottom == this.DockingRules.AllowDockBottom && dockingRules.AllowDockLeft == this.DockingRules.AllowDockLeft)
			{
				if (dockingRules.AllowDockRight == this.DockingRules.AllowDockRight)
				{
					if (dockingRules.AllowTab == this.DockingRules.AllowTab)
					{
						return dockingRules.AllowFloat != this.DockingRules.AllowFloat;
					}
				}
			}
			return true;
		}

		private bool ShouldSerializeTabText()
		{
			return this.string_1.Length != 0 && this.string_1 != this.Text;
		}

		internal static void smethod_0(Control control_1, Graphics graphics_0, TD.SandDock.Rendering.BorderStyle borderStyle_1)
		{
			if (borderStyle_1 == TD.SandDock.Rendering.BorderStyle.None)
			{
				return;
			}
			Rectangle rectangle = new Rectangle(0, 0, control_1.Width, control_1.Height);
			if (borderStyle_1 != TD.SandDock.Rendering.BorderStyle.Flat)
			{
				Border3DStyle style;
				switch (borderStyle_1)
				{
				case TD.SandDock.Rendering.BorderStyle.Flat:
					style = Border3DStyle.Flat;
					goto IL_5B;
				case TD.SandDock.Rendering.BorderStyle.RaisedThick:
					style = Border3DStyle.Raised;
					goto IL_5B;
				case TD.SandDock.Rendering.BorderStyle.RaisedThin:
					style = Border3DStyle.RaisedInner;
					goto IL_5B;
				case TD.SandDock.Rendering.BorderStyle.SunkenThick:
					style = Border3DStyle.Sunken;
					goto IL_5B;
				}
				style = Border3DStyle.SunkenOuter;
				IL_5B:
				ControlPaint.DrawBorder3D(graphics_0, rectangle, style);
				return;
			}
			Color backColor = control_1.BackColor;
			Color controlDark = SystemColors.ControlDark;
			DockControl dockControl = control_1 as DockControl;
			if (dockControl != null && dockControl.Manager != null)
			{
				dockControl.Manager.Renderer.ModifyDefaultWindowColors(dockControl, ref backColor, ref controlDark);
			}
			rectangle.Width--;
			rectangle.Height--;
			using (Pen pen = new Pen(controlDark))
			{
				graphics_0.DrawRectangle(pen, rectangle);
			}
		}

		public void Split(DockSide direction)
		{
			if (!this.Boolean_1)
			{
				throw new InvalidOperationException("A window cannot be split while it is not hosted in a DockContainer.");
			}
			if (this.LayoutSystem.Controls.Count < 2)
			{
				throw new InvalidOperationException("A minimum of 2 windows need to be present in a tab group before one can be split off. Check LayoutSystem.Controls.Count before calling this method.");
			}
			if (direction != DockSide.None)
			{
				SizeF workingSize = this.LayoutSystem.WorkingSize;
				ControlLayoutSystem layoutSystem = this.LayoutSystem;
				LayoutUtilities.smethod_11(this);
				ControlLayoutSystem layoutSystem2 = layoutSystem.DockContainer.CreateNewLayoutSystem(this, workingSize);
				layoutSystem.SplitForLayoutSystem(layoutSystem2, direction);
				this.Activate();
				return;
			}
			throw new ArgumentException("direction");
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 33)
			{
				base.WndProc(ref m);
				if (!base.ContainsFocus)
				{
					this.Activate();
				}
				return;
			}
			base.WndProc(ref m);
		}

		[Category("Docking"), DefaultValue(true), Description("Indicates whether this control will be closable by the user.")]
		public virtual bool AllowClose
		{
			get
			{
				return this.bool_4;
			}
			set
			{
				this.bool_4 = value;
				this.method_1();
			}
		}

		[Category("Docking"), DefaultValue(true), Description("Indicates whether the user will be able to put this control in to auto-hide mode.")]
		public virtual bool AllowCollapse
		{
			get
			{
				return this.bool_5;
			}
			set
			{
				this.bool_5 = value;
				if (!value && this.Collapsed)
				{
					this.Collapsed = false;
				}
				this.method_1();
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockBottom
		{
			get
			{
				return this.DockingRules.AllowDockBottom;
			}
			set
			{
				this.DockingRules.AllowDockBottom = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockCenter
		{
			get
			{
				return this.DockingRules.AllowTab;
			}
			set
			{
				this.DockingRules.AllowTab = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockLeft
		{
			get
			{
				return this.DockingRules.AllowDockLeft;
			}
			set
			{
				this.DockingRules.AllowDockLeft = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockRight
		{
			get
			{
				return this.DockingRules.AllowDockRight;
			}
			set
			{
				this.DockingRules.AllowDockRight = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockTop
		{
			get
			{
				return this.DockingRules.AllowDockTop;
			}
			set
			{
				this.DockingRules.AllowDockTop = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowFloat
		{
			get
			{
				return this.DockingRules.AllowFloat;
			}
			set
			{
				this.DockingRules.AllowFloat = value;
			}
		}

		[Category("Behavior"), DefaultValue(false), Description("Determines whether the user will be able to press tab to bring the focus within the window when docked.")]
		public bool AllowKeyboardFocus
		{
			get
			{
				return base.GetStyle(ControlStyles.Selectable);
			}
			set
			{
				base.SetStyle(ControlStyles.Selectable, value);
			}
		}

		[Browsable(false)]
		protected virtual bool AllowKeyboardNavigation
		{
			get
			{
				return this.Manager == null || this.Manager.AllowKeyboardNavigation;
			}
		}

		[DefaultValue(typeof(Color), "Control")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				if (this.LayoutSystem != null && this.LayoutSystem.DockContainer != null)
				{
					this.LayoutSystem.DockContainer.Invalidate(this.LayoutSystem.Bounds);
				}
			}
		}

		public override BindingContext BindingContext
		{
			get
			{
				if (this.bindingContext_0 != null)
				{
					return this.bindingContext_0;
				}
				if (this.Manager != null && this.Manager.DockSystemContainer != null)
				{
					return this.Manager.DockSystemContainer.BindingContext;
				}
				if (base.DesignMode)
				{
					return base.BindingContext;
				}
				return null;
			}
			set
			{
				this.bindingContext_0 = value;
				base.BindingContext = value;
			}
		}

		internal bool Boolean_0
		{
			get
			{
				return this.bool_0;
			}
			set
			{
				this.bool_0 = value;
			}
		}

		[Browsable(false)]
		internal bool Boolean_1
		{
			get
			{
				return this.controlLayoutSystem_0 != null && this.controlLayoutSystem_0.DockContainer != null;
			}
		}

		[Category("Appearance"), DefaultValue(typeof(TD.SandDock.Rendering.BorderStyle), "None"), Description("The type of border to be drawn around the control.")]
		public TD.SandDock.Rendering.BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle_0;
			}
			set
			{
				this.borderStyle_0 = value;
				base.PerformLayout();
				base.Invalidate();
			}
		}

		private Class5 Class5_0
		{
			get
			{
				return this.controlLayoutSystem_0.DockContainer as Class5;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the AllowClose property instead.", true)]
		public bool Closable
		{
			get
			{
				return this.AllowClose;
			}
			set
			{
				this.AllowClose = value;
			}
		}

		[Category("Behavior"), DefaultValue(DockControlCloseAction.HideOnly), Description("Indicates what action will be performed when the DockControl is closed.")]
		public virtual DockControlCloseAction CloseAction
		{
			get
			{
				return this.dockControlCloseAction_0;
			}
			set
			{
				this.dockControlCloseAction_0 = value;
			}
		}

		[Category("Layout"), DefaultValue(false), Description("Indicates whether the window is collapsed when docked.")]
		public bool Collapsed
		{
			get
			{
				return this.LayoutSystem != null && this.LayoutSystem.Collapsed;
			}
			set
			{
				if (this.LayoutSystem != null)
				{
					this.LayoutSystem.Collapsed = value;
				}
			}
		}

		protected override Size DefaultSize
		{
			get
			{
				return new Size(250, 400);
			}
		}

		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle displayRectangle = base.DisplayRectangle;
				switch (this.borderStyle_0)
				{
				case TD.SandDock.Rendering.BorderStyle.Flat:
				case TD.SandDock.Rendering.BorderStyle.RaisedThin:
				case TD.SandDock.Rendering.BorderStyle.SunkenThin:
					displayRectangle.Inflate(-1, -1);
					break;
				case TD.SandDock.Rendering.BorderStyle.RaisedThick:
				case TD.SandDock.Rendering.BorderStyle.SunkenThick:
					displayRectangle.Inflate(-2, -2);
					break;
				}
				return displayRectangle;
			}
		}

		[Browsable(false)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}

		[Category("Behavior"), Description("The rules with which to govern where the user can move the window.")]
		public DockingRules DockingRules
		{
			get
			{
				return this.dockingRules_0;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.dockingRules_0 = value;
			}
		}

		[Browsable(false)]
		public DockSituation DockSituation
		{
			get
			{
				return this.dockSituation_0;
			}
		}

		[Browsable(false), DefaultValue(typeof(Point), "-1, -1")]
		public Point FloatingLocation
		{
			get
			{
				return this.point_0;
			}
			set
			{
				this.point_0 = value;
				if (this.DockSituation == DockSituation.Floating)
				{
					if (this.Class5_0.Point_0 != this.point_0)
					{
						this.Class5_0.Point_0 = this.point_0;
					}
				}
			}
		}

		[Category("Layout"), DefaultValue(typeof(Size), "250, 400"), Description("Indicates the default size this control will assume when floating on its own.")]
		public Size FloatingSize
		{
			get
			{
				return this.size_0;
			}
			set
			{
				if (value.Width > 0 && value.Height > 0)
				{
					this.size_0 = value;
					if (this.DockSituation == DockSituation.Floating && this.Class5_0.Size_0 != this.size_0)
					{
						this.Class5_0.Size_0 = this.size_0;
					}
					return;
				}
				throw new ArgumentOutOfRangeException("value");
			}
		}

		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				if (this.LayoutSystem != null && this.LayoutSystem.DockContainer != null)
				{
					this.LayoutSystem.DockContainer.Invalidate(this.rectangle_0);
				}
			}
		}

		[Category("Advanced"), Description("The unique identifier for the window.")]
		public Guid Guid
		{
			get
			{
				return this.guid_0;
			}
			set
			{
				Guid oldGuid = this.guid_0;
				this.guid_0 = value;
				if (this.Manager != null)
				{
					this.Manager.ReRegisterWindow(this, oldGuid);
				}
			}
		}

		internal Image Image_0
		{
			get
			{
				if (this.image_1 == null)
				{
					return DockControl.image_0;
				}
				return this.image_1;
			}
		}

		[Browsable(false), Obsolete("Use the DockSituation property instead.")]
		public bool IsDocked
		{
			get
			{
				return this.Boolean_1 && !(this.controlLayoutSystem_0.DockContainer is DocumentContainer) && !(this.controlLayoutSystem_0.DockContainer is Class5);
			}
		}

		[Browsable(false), Obsolete("Use the DockSituation property instead.")]
		public bool IsFloating
		{
			get
			{
				return this.Boolean_1 && this.controlLayoutSystem_0.DockContainer.IsFloating;
			}
		}

		[Browsable(false)]
		public bool IsOpen
		{
			get
			{
				bool result;
				if ((result = (this.Boolean_1 && this.controlLayoutSystem_0 != null && this.controlLayoutSystem_0.SelectedControl == this)) && this.controlLayoutSystem_0.Collapsed)
				{
					result = this.controlLayoutSystem_0.IsPoppedUp;
				}
				return result;
			}
		}

		[Browsable(false), Obsolete("Use the DockSituation property instead.")]
		public bool IsTabbedDocument
		{
			get
			{
				return this.Boolean_1 && this.controlLayoutSystem_0.DockContainer is DocumentContainer;
			}
		}

		[Browsable(false)]
		public ControlLayoutSystem LayoutSystem
		{
			get
			{
				return this.controlLayoutSystem_0;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SandDockManager Manager
		{
			get
			{
				return this.sandDockManager_0;
			}
			set
			{
				if (value != this.sandDockManager_0)
				{
					if (this.sandDockManager_0 != null)
					{
						this.sandDockManager_0.UnregisterWindow(this);
					}
					this.sandDockManager_0 = value;
					if (this.sandDockManager_0 != null)
					{
						this.sandDockManager_0.RegisterWindow(this);
					}
				}
			}
		}

		[Category("Layout"), DefaultValue(0), Description("Indicates the maximum width of the tab representing the window.")]
		public int MaximumTabWidth
		{
			get
			{
				return this.int_1;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Value must be greater than or equal to zero.");
				}
				this.int_1 = value;
				this.method_1();
			}
		}

		[Browsable(false)]
		public WindowMetaData MetaData
		{
			get
			{
				return this.windowMetaData_0;
			}
		}

		[Category("Layout"), DefaultValue(0), Description("Indicates the minimum width of the tab representing the window.")]
		public int MinimumTabWidth
		{
			get
			{
				return this.int_2;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.int_2 = value;
				this.method_1();
			}
		}

		[Browsable(true), Category("Behavior"), DefaultValue(true), Description("Indicates whether the location of the DockControl will be included in layout serialization.")]
		public virtual bool PersistState
		{
			get
			{
				return this.bool_6;
			}
			set
			{
				this.bool_6 = value;
			}
		}

		[Category("Docking"), DefaultValue(0), Description("The size of the control when popped up from a collapsed state. Leave this as zero for the default size.")]
		public int PopupSize
		{
			get
			{
				return this.int_0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Value must be at least equal to zero.");
				}
				this.int_0 = value;
				if (!this.MetaData.Boolean_0)
				{
					this.MetaData.method_2(value);
				}
				if (this.LayoutSystem != null && this.LayoutSystem.Control0_0 != null)
				{
					if (this.LayoutSystem.Control0_0.ControlLayoutSystem_0 == this.LayoutSystem)
					{
						this.LayoutSystem.Control0_0.Int32_1 = value;
					}
				}
			}
		}

		[Category("Behavior"), DefaultValue(typeof(Control), null), Description("The control that will be focused when the window is activated.")]
		public Control PrimaryControl
		{
			get
			{
				return this.control_0;
			}
			set
			{
				this.control_0 = value;
			}
		}

		[Category("Appearance"), DefaultValue(true), Description("Indicates whether an options button will be displayed in the titlebar for this window.")]
		public bool ShowOptions
		{
			get
			{
				return this.bool_7;
			}
			set
			{
				this.bool_7 = value;
				this.method_1();
			}
		}

		[Browsable(false)]
		public Rectangle TabBounds
		{
			get
			{
				return this.rectangle_0;
			}
		}

		[AmbientValue(typeof(Image), null), Category("Appearance"), DefaultValue(typeof(Image), null), Description("The image displayed for this control on docking tabs.")]
		public Image TabImage
		{
			get
			{
				return this.image_1;
			}
			set
			{
				this.image_1 = value;
				this.method_1();
			}
		}

		[Category("Appearance"), Description("The text to display on the tab for the DockControl. This can be different to the standard text."), Localizable(true)]
		public virtual string TabText
		{
			get
			{
				if (this.string_1.Length == 0)
				{
					return this.Text;
				}
				return this.string_1;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.string_1 = value;
				this.method_1();
			}
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.method_1();
				if (this.DockSituation == DockSituation.Floating)
				{
					if (this.Class5_0.HasSingleControlLayoutSystem && this.LayoutSystem.SelectedControl == this)
					{
						this.Class5_0.method_21();
					}
				}
			}
		}

		[Category("Appearance"), DefaultValue(""), Description("Gets or sets the text that appears as a ToolTip for the control tab."), Localizable(true)]
		public virtual string ToolTipText
		{
			get
			{
				return this.string_0;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.string_0 = value;
			}
		}

		public event EventHandler AutoHidePopupClosed
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.eventHandler_3 = (EventHandler)Delegate.Combine(this.eventHandler_3, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.eventHandler_3 = (EventHandler)Delegate.Remove(this.eventHandler_3, value);
			}
		}

		public event EventHandler AutoHidePopupOpened
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.eventHandler_2 = (EventHandler)Delegate.Combine(this.eventHandler_2, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.eventHandler_2 = (EventHandler)Delegate.Remove(this.eventHandler_2, value);
			}
		}

		public event EventHandler Closed
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.eventHandler_0 = (EventHandler)Delegate.Combine(this.eventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.eventHandler_0 = (EventHandler)Delegate.Remove(this.eventHandler_0, value);
			}
		}

		public event DockControlClosingEventHandler Closing
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.dockControlClosingEventHandler_0 = (DockControlClosingEventHandler)Delegate.Combine(this.dockControlClosingEventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.dockControlClosingEventHandler_0 = (DockControlClosingEventHandler)Delegate.Remove(this.dockControlClosingEventHandler_0, value);
			}
		}

		public event EventHandler DockSituationChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.eventHandler_4 = (EventHandler)Delegate.Combine(this.eventHandler_4, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.eventHandler_4 = (EventHandler)Delegate.Remove(this.eventHandler_4, value);
			}
		}

		public event EventHandler Load
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.eventHandler_1 = (EventHandler)Delegate.Combine(this.eventHandler_1, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.eventHandler_1 = (EventHandler)Delegate.Remove(this.eventHandler_1, value);
			}
		}

		private BindingContext bindingContext_0;

		private bool bool_0;

		private bool bool_1;

		private bool bool_2;

		internal bool bool_3;

		private bool bool_4 = true;

		private bool bool_5 = true;

		private bool bool_6 = true;

		private bool bool_7 = true;

		private TD.SandDock.Rendering.BorderStyle borderStyle_0;

		private ControlLayoutSystem controlLayoutSystem_0;

		private Control control_0;

		private DockControlCloseAction dockControlCloseAction_0;

		private DockControlClosingEventHandler dockControlClosingEventHandler_0;

		private DockingRules dockingRules_0;

		private DockSituation dockSituation_0;

		private EventHandler eventHandler_0;

		private EventHandler eventHandler_1;

		private EventHandler eventHandler_2;

		private EventHandler eventHandler_3;

		private EventHandler eventHandler_4;

		private Guid guid_0 = Guid.NewGuid();

		private static Image image_0;

		private Image image_1;

		private int int_0;

		private int int_1;

		private int int_2;

		private Point point_0 = new Point(-1, -1);

		internal Rectangle rectangle_0 = Rectangle.Empty;

		internal Rectangle rectangle_1 = Rectangle.Empty;

		private SandDockManager sandDockManager_0;

		private Size size_0;

		private string string_0 = "";

		private string string_1 = string.Empty;

		private WindowMetaData windowMetaData_0;
	}
}
