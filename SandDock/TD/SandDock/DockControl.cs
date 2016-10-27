using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock.Design;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
    public delegate void DockControlEventHandler(object sender, DockControlEventArgs e);

    public class DockControlEventArgs : EventArgs
    {
        internal DockControlEventArgs(DockControl dockControl)
        {
            DockControl = dockControl;
        }

        public DockControl DockControl { get; }

    }

    public delegate void DockControlClosingEventHandler(object sender, DockControlClosingEventArgs e);

    public class DockControlClosingEventArgs : DockControlEventArgs
    {
        internal DockControlClosingEventArgs(DockControl dockControl, bool cancel) : base(dockControl)
        {
            Cancel = cancel;
        }

        public bool Cancel { get; set; }
    }

    public enum DockControlCloseAction
    {
        HideOnly,
        Dispose
    }

    [DefaultEvent("Closing"), Designer(typeof(DockControlDesigner)), ToolboxItem(false)]
	public abstract class DockControl : ContainerControl
	{
		protected DockControl()
		{
			if (image_0 == null)
			{
				image_0 = Image.FromStream(typeof(DockControl).Assembly.GetManifestResourceStream("TD.SandDock.sanddock.png"));
			}
			MetaData = new WindowMetaData();
			this.dockingRules = CreateDockingRules();
		    if (this.dockingRules == null)
		        throw new InvalidOperationException();
		    SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.Selectable, false);
			BackColor = SystemColors.Control;
			this.size_0 = DefaultSize;
		}

		protected DockControl(SandDockManager manager, Control control, string text) : this()
		{
		    if (manager == null)
		        throw new ArgumentNullException(nameof(manager));
		    if (control == null)
		        throw new ArgumentNullException(nameof(control));
		    if (text == null)
		        text = string.Empty;
		    Manager = manager;

			if (control is Form)
			{
				Form form = (Form)control;
				form.TopLevel = false;
				form.FormBorderStyle = FormBorderStyle.None;
			}
			if (control != null)
			{
				SuspendLayout();
				Controls.Add(control);
				control.Dock = DockStyle.Fill;
				control.BringToFront();
				ResumeLayout();
				control.Visible = true;
			}
		    if (text != null)
		        Text = text;
		}

		public void Activate()
		{
		    if (LayoutSystem == null || Parent == null)
		        return;
		    if (LayoutSystem.SelectedControl != this)
			{
				LayoutSystem.SelectedControl = this;
			    if (LayoutSystem.SelectedControl != this)
			        return;
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
			    if (LayoutSystem != null)
			        LayoutUtilities.smethod_11(this);
			    if (Manager != null)
			        this.Manager = null;
			}
			base.Dispose(disposing);
		}

		public void DockInNewContainer(ContainerDockLocation dockLocation, ContainerDockEdge edge)
		{
			this.method_10();
			this.Remove();
			DockContainer dockContainer = Manager.CreateNewDockContainer(dockLocation, edge, MetaData.DockedContentSize);
			ControlLayoutSystem layoutSystem = dockContainer.CreateNewLayoutSystem(this, FloatingSize);
			dockContainer.LayoutSystem.LayoutSystems.Add(layoutSystem);
		}

		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use the OpenWith method instead.")]
		public void DockNextTo(DockControl existingWindow) => OpenWith(existingWindow);

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
				Screen screen = (Manager.DockSystemContainer == null) ? Screen.PrimaryScreen : Screen.FromControl(Manager.DockSystemContainer);
				Rectangle workingArea = screen.WorkingArea;
				point = new Point(workingArea.X + workingArea.Width / 2 - this.size_0.Width / 2, workingArea.Y + workingArea.Height / 2 - this.size_0.Height / 2);
			}
			return point;
		}

		public Form GetFloatingForm() => DockSituation == DockSituation.Floating && Parent != null ? Parent.Parent as Form : null;

        internal void method_0(bool bool_8)
		{
			SetVisibleCore(bool_8);
		}

		internal void method_1() => LayoutSystem?.method_16();

	    private void method_10()
		{
			this.method_9();
	        if (Manager.DockSystemContainer == null)
	            throw new InvalidOperationException(
	                "The SandDockManager associated with this DockControl does not have its DockSystemContainer property set.");
		}

		internal Rectangle method_11()
		{
			this.method_9();
		    if (this.point_0.X == -1 && this.point_0.Y == -1)
		    {
		        this.point_0 = GetDefaultFloatingLocation();
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
		    Manager?.OnDockControlClosing(dockControlClosingEventArgs);

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
			if (this.LayoutSystem != controlLayoutSystem_1)
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
			this.LayoutSystem = controlLayoutSystem_1;
		}

		private void method_17(DockSituation dockSituation_1)
		{
			if (this.bool_2)
			{
				throw new InvalidOperationException("The requested operation is not valid on a window that is currently engaged in an activity further up the call stack. Consider using BeginInvoke to postpone the operation until the stack has unwound.");
			}
			if (dockSituation_1 != this.DockSituation)
			{
				this.DockSituation = dockSituation_1;
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
			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}

		private void method_3()
		{
			if (!this.bool_1)
			{
				if (this.Manager?.DocumentContainer == null || !this.Manager.DocumentContainer.Boolean_3)
				{
					this.MetaData.method_0(DateTime.Now);
				}
			    this.Manager?.OnDockControlActivated(new DockControlEventArgs(this));
			}
		}

		internal void method_4(DockContainer dockContainer_0)
		{
		    if (dockContainer_0?.Manager != null && dockContainer_0.Manager != this.Manager)
		    {
		        this.Manager = dockContainer_0.Manager;
		    }
		    this.method_5();
		}

	    internal void method_5()
		{
			DockSituation dockSituation;
			dockSituation = this.LayoutSystem?.DockContainer != null ? LayoutUtilities.smethod_2(this.LayoutSystem.DockContainer) : DockSituation.None;
			if (dockSituation != DockSituation.None)
			{
				this.MetaData.method_3(dockSituation);
			}
			Class18 @class = null;
			switch (dockSituation)
			{
			case DockSituation.Docked:
				@class = this.MetaData.Class19_0;
				this.MetaData.method_4(DockSituation.Docked);
				this.MetaData.method_1(LayoutUtilities.smethod_7(this.LayoutSystem.DockContainer.Dock));
				this.MetaData.method_2(this.LayoutSystem.DockContainer.ContentSize);
				if (this.Manager != null)
				{
					DockContainer[] dockContainers = this.Manager.GetDockContainers(this.LayoutSystem.DockContainer.Dock);
					this.MetaData.Class19_0.Int32_3 = dockContainers.Length;
					this.MetaData.Class19_0.Int32_2 = Array.IndexOf<DockContainer>(dockContainers, this.LayoutSystem.DockContainer);
				}
				break;
			case DockSituation.Document:
				@class = this.MetaData.Class18_0;
				this.MetaData.method_4(DockSituation.Document);
				break;
			case DockSituation.Floating:
				@class = this.MetaData.Class18_1;
				this.MetaData.method_5(((FloatingContainer)this.LayoutSystem.DockContainer).Guid_0);
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
		    if (IsDisposed)
		        throw new ObjectDisposedException(GetType().Name);
		}

		internal void method_8()
		{
			CreateControl();
		}

		private void method_9()
		{
			this.method_7();
		    if (this.manager == null)
		        throw new InvalidOperationException(
		            "No SandDockManager is associated with this DockControl. To create an association, set the Manager property.");
		}

		protected internal virtual void OnAutoHidePopupClosed(EventArgs e)
		{
            AutoHidePopupClosed?.Invoke(this, e);
		}

	    protected internal virtual void OnAutoHidePopupOpened(EventArgs e)
	    {
            AutoHidePopupOpened?.Invoke(this, e);
	    }

	    protected internal virtual void OnClosed(EventArgs e)
	    {
            Closed?.Invoke(this, e);
	    }

	    protected internal virtual void OnClosing(DockControlClosingEventArgs e)
	    {
            Closing?.Invoke(this, e);
	    }

	    protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this.OnLoad(EventArgs.Empty);
		}

		protected virtual void OnDockSituationChanged(EventArgs e)
		{
            DockSituationChanged?.Invoke(this, e);
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
            Load?.Invoke(this, e);
		}

	    protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			smethod_0(this, e.Graphics, this.borderStyle);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			switch (BorderStyle)
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
		    this.Manager?.Renderer.ModifyDefaultWindowColors(this, ref backColor, ref transparent);
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
			switch (DockSituation)
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
				if (this.MetaData.LastFixedDockSituation == DockSituation.Docked && this.method_13(this.MetaData.LastFixedDockSide))
				{
					this.OpenDocked(WindowOpenMethod.OnScreenActivate);
					return;
				}
				if (this.MetaData.LastFixedDockSituation == DockSituation.Document && this.method_13(ContainerDockLocation.Center))
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
				switch (this.MetaData.LastOpenDockSituation)
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
		        throw new ArgumentNullException();
		    if (existingWindow == this)
		        return;
		    if (existingWindow.DockSituation == DockSituation.None)
		        throw new InvalidOperationException("The specified window is not open.");
		    Remove();
		    existingWindow.LayoutSystem.SplitForLayoutSystem(new ControlLayoutSystem(MetaData.Class19_0.SizeF_0, new[] {this}, this), side);
		}

		public void OpenDocked() => OpenDocked(MetaData.LastFixedDockSide);

        public void OpenDocked(ContainerDockLocation dockLocation)
        {
            if (dockLocation == MetaData.LastFixedDockSide)
                OpenDocked(WindowOpenMethod.OnScreenSelect);
            else
                OpenDocked(dockLocation, WindowOpenMethod.OnScreenSelect);
        }

        public void OpenDocked(WindowOpenMethod openMethod)
		{
			this.method_9();
			this.method_8();
			if (DockSituation == DockSituation.Docked)
			{
				return;
			}
			Remove();
			ControlLayoutSystem controlLayoutSystem = LayoutUtilities.smethod_4(this.Manager, DockSituation.Docked, this.MetaData.Class19_0);
			if (controlLayoutSystem != null)
			{
				controlLayoutSystem.Controls.Insert(Math.Min(this.MetaData.Class19_0.Int32_1, controlLayoutSystem.Controls.Count), this);
				if (openMethod != WindowOpenMethod.OnScreen)
				{
					this.method_12(openMethod == WindowOpenMethod.OnScreenActivate);
				}
				return;
			}
			Struct0 @struct = LayoutUtilities.smethod_14(this.Manager, this.MetaData);
			controlLayoutSystem = @struct.splitLayoutSystem_0.DockContainer.CreateNewLayoutSystem(this, this.MetaData.Class19_0.SizeF_0);
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
				if (this.MetaData.LastFixedDockSide == dockLocation)
				{
					return;
				}
			}
			this.Remove();
			this.MetaData.method_1(dockLocation);
			this.MetaData.Class19_0.Guid_0 = Guid.Empty;
			this.MetaData.Class19_0.Int32_0 = new int[0];
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
			ControlLayoutSystem controlLayoutSystem = LayoutUtilities.smethod_4(this.Manager, DockSituation.Document, this.MetaData.Class18_0);
			if (controlLayoutSystem != null)
			{
				controlLayoutSystem.Controls.Insert(Math.Min(this.MetaData.Class19_0.Int32_1, controlLayoutSystem.Controls.Count), this);
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

		public void OpenFloating() => OpenFloating(WindowOpenMethod.OnScreenActivate);

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
			FloatingContainer @class = this.Manager.FindFloatingDockContainer(this.MetaData.LastFloatingWindowGuid);
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
			if (this.MetaData.LastFloatingWindowGuid == Guid.Empty)
			{
				this.MetaData.method_5(Guid.NewGuid());
			}
			@class = new FloatingContainer(this.Manager, this.MetaData.LastFloatingWindowGuid);
			controlLayoutSystem = @class.CreateNewLayoutSystem(this, this.MetaData.Class18_1.SizeF_0);
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
		        throw new ArgumentNullException();
		    if (existingWindow == this)
		        return;
		    if (existingWindow.DockSituation == DockSituation.None)
		        throw new InvalidOperationException("The specified window is not open.");
		    Remove();
		    existingWindow.LayoutSystem.Controls.Add(this);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (this.LayoutSystem != null && this.AllowKeyboardNavigation)
			{
				if (keyData == (Keys.LButton | Keys.Space | Keys.Control))
				{
					int num = this.LayoutSystem.Controls.IndexOf(this);
					num--;
					if (num < 0)
					{
						num = this.LayoutSystem.Controls.Count - 1;
					}
					this.LayoutSystem.SelectedControl = this.LayoutSystem.Controls[num];
					if (this.LayoutSystem.SelectedControl == this.LayoutSystem.Controls[num])
					{
						this.LayoutSystem.Controls[num].Activate();
					}
					return true;
				}
				if (keyData == (Keys.RButton | Keys.Space | Keys.Control))
				{
					int num2 = this.LayoutSystem.Controls.IndexOf(this);
					num2++;
					if (num2 >= this.LayoutSystem.Controls.Count)
					{
						num2 = 0;
					}
					this.LayoutSystem.SelectedControl = this.LayoutSystem.Controls[num2];
					if (this.LayoutSystem.SelectedControl == this.LayoutSystem.Controls[num2])
					{
						this.LayoutSystem.Controls[num2].Activate();
					}
					return true;
				}
				if (keyData == (Keys.LButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17 | Keys.Alt) && this.LayoutSystem.IsInContainer)
				{
					this.LayoutSystem.DockContainer.method_0(new ShowControlContextMenuEventArgs(this, new Point(0, 0), ContextMenuContext.Keyboard));
					return true;
				}
			    if (keyData == Keys.Escape && this.Manager != null && this.DockSituation != DockSituation.Document)
			    {
			        this.Manager.OwnerForm?.Activate();
			        DockControl dockControl = this.Manager.FindMostRecentlyUsedWindow(DockSituation.Document);
			        dockControl?.Activate();
			        return true;
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
		    if (DockSituation != DockSituation.None)
		        throw new InvalidOperationException("This operation is only valid when the window is not currently open.");
		    if (dockSituation == DockSituation.None)
		        throw new ArgumentException("dockSituation");
		    MetaData.method_3(dockSituation);
			MetaData.method_4(dockSituation);
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
				this.MetaData.method_3(dockSituation);
				if (dockSituation != DockSituation.Document)
				{
					if (dockSituation != DockSituation.Docked)
					{
						goto IL_54;
					}
				}
				this.MetaData.method_4(dockSituation);
				IL_54:
				this.MetaData.method_1(dockLocation);
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
			return this.tabText.Length != 0 && this.tabText != this.Text;
		}

		internal static void smethod_0(Control control_1, Graphics graphics_0, Rendering.BorderStyle borderStyle_1)
		{
			if (borderStyle_1 == Rendering.BorderStyle.None)
			{
				return;
			}
			Rectangle rectangle = new Rectangle(0, 0, control_1.Width, control_1.Height);
			if (borderStyle_1 != Rendering.BorderStyle.Flat)
			{
				Border3DStyle style;
				switch (borderStyle_1)
				{
				case Rendering.BorderStyle.Flat:
					style = Border3DStyle.Flat;
					goto IL_5B;
				case Rendering.BorderStyle.RaisedThick:
					style = Border3DStyle.Raised;
					goto IL_5B;
				case Rendering.BorderStyle.RaisedThin:
					style = Border3DStyle.RaisedInner;
					goto IL_5B;
				case Rendering.BorderStyle.SunkenThick:
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
		        throw new InvalidOperationException("A window cannot be split while it is not hosted in a DockContainer.");
		    if (LayoutSystem.Controls.Count < 2)
		        throw new InvalidOperationException("A minimum of 2 windows need to be present in a tab group before one can be split off. Check LayoutSystem.Controls.Count before calling this method.");
		    if (direction == DockSide.None)
                throw new ArgumentException("direction");

		    SizeF workingSize = LayoutSystem.WorkingSize;
		    ControlLayoutSystem layoutSystem = this.LayoutSystem;
		    LayoutUtilities.smethod_11(this);
		    ControlLayoutSystem layoutSystem2 = layoutSystem.DockContainer.CreateNewLayoutSystem(this, workingSize);
		    layoutSystem.SplitForLayoutSystem(layoutSystem2, direction);
		    Activate();
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
				return this.allowClose;
			}
			set
			{
				this.allowClose = value;
				this.method_1();
			}
		}

		[Category("Docking"), DefaultValue(true), Description("Indicates whether the user will be able to put this control in to auto-hide mode.")]
		public virtual bool AllowCollapse
		{
			get
			{
				return this.allowCollapse;
			}
			set
			{
				this.allowCollapse = value;
			    if (!value && Collapsed)
			        Collapsed = false;
			    this.method_1();
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockBottom
		{
			get
			{
				return DockingRules.AllowDockBottom;
			}
			set
			{
				DockingRules.AllowDockBottom = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockCenter
		{
			get
			{
				return DockingRules.AllowTab;
			}
			set
			{
				DockingRules.AllowTab = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockLeft
		{
			get
			{
				return DockingRules.AllowDockLeft;
			}
			set
			{
				DockingRules.AllowDockLeft = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockRight
		{
			get
			{
				return DockingRules.AllowDockRight;
			}
			set
			{
				DockingRules.AllowDockRight = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowDockTop
		{
			get
			{
				return DockingRules.AllowDockTop;
			}
			set
			{
				DockingRules.AllowDockTop = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the DockingRules property instead.")]
		public bool AllowFloat
		{
			get
			{
				return DockingRules.AllowFloat;
			}
			set
			{
				DockingRules.AllowFloat = value;
			}
		}

		[Category("Behavior"), DefaultValue(false), Description("Determines whether the user will be able to press tab to bring the focus within the window when docked.")]
		public bool AllowKeyboardFocus
		{
			get
			{
				return GetStyle(ControlStyles.Selectable);
			}
			set
			{
				SetStyle(ControlStyles.Selectable, value);
			}
		}

		[Browsable(false)]
		protected virtual bool AllowKeyboardNavigation => Manager?.AllowKeyboardNavigation ?? true;

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
			    LayoutSystem?.DockContainer?.Invalidate(LayoutSystem.Bounds);
			}
		}

		public override BindingContext BindingContext
		{
			get
			{
			    if (this.bindingContext != null)
			        return this.bindingContext;
			    if (Manager?.DockSystemContainer != null)
			        return Manager.DockSystemContainer.BindingContext;
			    if (DesignMode)
			        return base.BindingContext;
			    return null;
			}
			set
			{
				this.bindingContext = value;
				base.BindingContext = value;
			}
		}

		internal bool Boolean_0 { get; set; }

        [Browsable(false)]
		internal bool Boolean_1 => LayoutSystem?.DockContainer != null;

	    [Category("Appearance"), DefaultValue(Rendering.BorderStyle.None), Description("The type of border to be drawn around the control.")]
		public Rendering.BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				this.borderStyle = value;
				PerformLayout();
				Invalidate();
			}
		}

		private FloatingContainer Class5_0 => LayoutSystem.DockContainer as FloatingContainer;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the AllowClose property instead.", true)]
		public bool Closable
		{
			get
			{
				return AllowClose;
			}
			set
			{
				AllowClose = value;
			}
		}

		[Category("Behavior"), DefaultValue(DockControlCloseAction.HideOnly), Description("Indicates what action will be performed when the DockControl is closed.")]
		public virtual DockControlCloseAction CloseAction { get; set; }

        [Category("Layout"), DefaultValue(false), Description("Indicates whether the window is collapsed when docked.")]
		public bool Collapsed
		{
			get
			{
				return LayoutSystem?.Collapsed ?? false;
			}
			set
			{
			    if (LayoutSystem != null)
			        LayoutSystem.Collapsed = value;
			}
		}

		protected override Size DefaultSize => new Size(250, 400);

        public override Rectangle DisplayRectangle
		{
			get
			{
				var rectangle = base.DisplayRectangle;
				switch (this.borderStyle)
				{
				case Rendering.BorderStyle.Flat:
				case Rendering.BorderStyle.RaisedThin:
				case Rendering.BorderStyle.SunkenThin:
					rectangle.Inflate(-1, -1);
					break;
				case Rendering.BorderStyle.RaisedThick:
				case Rendering.BorderStyle.SunkenThick:
					rectangle.Inflate(-2, -2);
					break;
				}
				return rectangle;
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
				return this.dockingRules;
			}
			set
			{
			    if (value == null)
			        throw new ArgumentNullException(nameof(value));
			    this.dockingRules = value;
			}
		}

		[Browsable(false)]
		public DockSituation DockSituation { get; private set; }

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
				throw new ArgumentOutOfRangeException(nameof(value));
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
			    LayoutSystem?.DockContainer?.Invalidate(this.rectangle_0);
			}
		}

		[Category("Advanced"), Description("The unique identifier for the window.")]
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
			set
			{
				var oldGuid = this.guid;
				this.guid = value;
			    Manager?.ReRegisterWindow(this, oldGuid);
			}
		}

		internal Image Image_0 => this.tabImage ?? DockControl.image_0;

        [Browsable(false), Obsolete("Use the DockSituation property instead.")]
		public bool IsDocked
		{
			get
			{
				return this.Boolean_1 && !(LayoutSystem.DockContainer is DocumentContainer) && !(LayoutSystem.DockContainer is FloatingContainer);
			}
		}

		[Browsable(false), Obsolete("Use the DockSituation property instead.")]
		public bool IsFloating => Boolean_1 && LayoutSystem.DockContainer.IsFloating;

        [Browsable(false)]
		public bool IsOpen
		{
			get
			{
				bool result;
				if ((result = (this.Boolean_1 && this.LayoutSystem != null && this.LayoutSystem.SelectedControl == this)) && this.LayoutSystem.Collapsed)
				{
					result = this.LayoutSystem.IsPoppedUp;
				}
				return result;
			}
		}

		[Browsable(false), Obsolete("Use the DockSituation property instead.")]
		public bool IsTabbedDocument => Boolean_1 && LayoutSystem.DockContainer is DocumentContainer;

        [Browsable(false)]
		public ControlLayoutSystem LayoutSystem { get; private set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SandDockManager Manager
		{
			get
			{
				return this.manager;
			}
			set
			{
				if (value != this.manager)
				{
				    this.manager?.UnregisterWindow(this);
				    this.manager = value;
				    this.manager?.RegisterWindow(this);
				}
			}
		}

		[Category("Layout"), DefaultValue(0), Description("Indicates the maximum width of the tab representing the window.")]
		public int MaximumTabWidth
		{
			get
			{
				return this.maximumTabWidth;
			}
			set
			{
			    if (value < 0)
			        throw new ArgumentException("Value must be greater than or equal to zero.");
			    this.maximumTabWidth = value;
				this.method_1();
			}
		}

		[Browsable(false)]
		public WindowMetaData MetaData { get; }

        [Category("Layout"), DefaultValue(0), Description("Indicates the minimum width of the tab representing the window.")]
		public int MinimumTabWidth
		{
			get
			{
				return this.minimumTabWidth;
			}
			set
			{
			    if (value < 0)
			        throw new ArgumentOutOfRangeException(nameof(value));
			    this.minimumTabWidth = value;
				this.method_1();
			}
		}

		[Browsable(true), Category("Behavior"), DefaultValue(true), Description("Indicates whether the location of the DockControl will be included in layout serialization.")]
		public virtual bool PersistState { get; set; } = true;

        [Category("Docking"), DefaultValue(0), Description("The size of the control when popped up from a collapsed state. Leave this as zero for the default size.")]
		public int PopupSize
		{
			get
			{
				return this.popupSize;
			}
			set
			{
			    if (value < 0)
			        throw new ArgumentException("Value must be at least equal to zero.");
			    this.popupSize = value;
				if (!MetaData.Boolean_0)
				{
					this.MetaData.method_2(value);
				}
			    if (LayoutSystem?.Control0_0 != null &&
			        this.LayoutSystem.Control0_0.ControlLayoutSystem_0 == this.LayoutSystem)
			    {
			        this.LayoutSystem.Control0_0.Int32_1 = value;
			    }
			}
		}

		[Category("Behavior"), DefaultValue(typeof(Control), null), Description("The control that will be focused when the window is activated.")]
		public Control PrimaryControl { get; set; }

        [Category("Appearance"), DefaultValue(true), Description("Indicates whether an options button will be displayed in the titlebar for this window.")]
		public bool ShowOptions
		{
			get
			{
				return this.showOptions;
			}
			set
			{
				this.showOptions = value;
				this.method_1();
			}
		}

		[Browsable(false)]
		public Rectangle TabBounds => this.rectangle_0;

        [AmbientValue(typeof(Image), null), Category("Appearance"), DefaultValue(typeof(Image), null), Description("The image displayed for this control on docking tabs.")]
		public Image TabImage
		{
			get
			{
				return this.tabImage;
			}
			set
			{
				this.tabImage = value;
				this.method_1();
			}
		}

		[Category("Appearance"), Description("The text to display on the tab for the DockControl. This can be different to the standard text."), Localizable(true)]
		public virtual string TabText
		{
			get
			{
			    return this.tabText.Length != 0 ? this.tabText : Text;
			}
		    set
		    {
		        this.tabText = value ?? string.Empty;
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
				if (DockSituation == DockSituation.Floating)
				{
					if (this.Class5_0.HasSingleControlLayoutSystem && LayoutSystem.SelectedControl == this)
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
				return this.toolTipText;
			}
			set { this.toolTipText = value ?? string.Empty; }
		}

        public event EventHandler AutoHidePopupClosed;

        public event EventHandler AutoHidePopupOpened;

        public event EventHandler Closed;

        public event DockControlClosingEventHandler Closing;

        public event EventHandler DockSituationChanged;

        public event EventHandler Load;

		private BindingContext bindingContext;

        private bool bool_1;

		private bool bool_2;

		internal bool bool_3;

		private bool allowClose = true;

		private bool allowCollapse = true;

        private bool showOptions = true;

		private Rendering.BorderStyle borderStyle;

        //private DockControlClosingEventHandler dockControlClosingEventHandler_0;

		private DockingRules dockingRules;

        //private EventHandler eventHandler_0;

		//private EventHandler eventHandler_1;

		//private EventHandler eventHandler_2;

		//private EventHandler eventHandler_3;

		//private EventHandler eventHandler_4;

		private Guid guid = Guid.NewGuid();

		private static Image image_0;

		private Image tabImage;

		private int popupSize;

		private int maximumTabWidth;

		private int minimumTabWidth;

		private Point point_0 = new Point(-1, -1);

		internal Rectangle rectangle_0 = Rectangle.Empty;

		internal Rectangle rectangle_1 = Rectangle.Empty;

		private SandDockManager manager;

		private Size size_0;

		private string toolTipText = "";

		private string tabText = string.Empty;
	}
}
