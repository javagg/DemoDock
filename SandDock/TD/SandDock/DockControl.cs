using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock.Design;
using TD.SandDock.Rendering;

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
                image_0 = Image.FromStream(typeof(DockControl).Assembly.GetManifestResourceStream("TD.SandDock.sanddock.png"));

            MetaData = new WindowMetaData();
            _dockingRules = CreateDockingRules();
            if (_dockingRules == null)
                throw new InvalidOperationException();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, false);
            BackColor = SystemColors.Control;
            _floatingSize = DefaultSize;
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

            var form = control as Form;
            if (form != null)
            {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
            }
            SuspendLayout();
            Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.BringToFront();
            ResumeLayout();
            control.Visible = true;
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
            if (DockSituation == DockSituation.Floating)
            {
                Class5_0.Activate();
            }
            if (IsOpen)
            {
                this.bool_1 = true;
                try
                {
                    var containerControl = Parent.GetContainerControl();
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
                    Manager = null;
            }
            base.Dispose(disposing);
        }

        public void DockInNewContainer(ContainerDockLocation dockLocation, ContainerDockEdge edge)
        {
            this.method_10();
            Remove();
            var dockContainer = Manager.CreateNewDockContainer(dockLocation, edge, MetaData.DockedContentSize);
            var layoutSystem = dockContainer.CreateNewLayoutSystem(this, FloatingSize);
            dockContainer.LayoutSystem.LayoutSystems.Add(layoutSystem);
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use the OpenWith method instead.")]
        public void DockNextTo(DockControl existingWindow) => OpenWith(existingWindow);

        protected virtual Point GetDefaultFloatingLocation()
        {
            Point point;
            if (IsInContainer)
            {
                point = LayoutSystem.DockContainer.PointToScreen(LayoutSystem.Bounds.Location);
                point -= new Size(SystemInformation.CaptionHeight, SystemInformation.CaptionHeight);
            }
            else
            {
                var screen = Manager.DockSystemContainer == null ? Screen.PrimaryScreen : Screen.FromControl(Manager.DockSystemContainer);
                var workingArea = screen.WorkingArea;
                point = new Point(workingArea.X + workingArea.Width / 2 - _floatingSize.Width / 2, workingArea.Y + workingArea.Height / 2 - _floatingSize.Height / 2);
            }
            return point;
        }

        public Form GetFloatingForm() => DockSituation == DockSituation.Floating && Parent != null ? Parent.Parent as Form : null;

        internal void method_0(bool bool_8)
        {
            SetVisibleCore(bool_8);
        }

        internal void CalculateAllMetricsAndLayout() => LayoutSystem?.CalculateAllMetricsAndLayout();

        private void method_10()
        {
            EnsureDockSystemContainerNotNull();
            if (Manager.DockSystemContainer == null)
                throw new InvalidOperationException("The SandDockManager associated with this DockControl does not have its DockSystemContainer property set.");
        }

        internal Rectangle method_11()
        {
            EnsureDockSystemContainerNotNull();
            if (_floatingLocation.X == -1 && _floatingLocation.Y == -1)
                _floatingLocation = GetDefaultFloatingLocation();
            return new Rectangle(_floatingLocation, _floatingSize);
        }

        internal void method_12(bool bool_8)
        {
            if (LayoutSystem != null)
            {
                if (this.LayoutSystem.SelectedControl != this)
                {
                    this.LayoutSystem.SelectedControl = this;
                    if (this.LayoutSystem.SelectedControl != this)
                    {
                        return;
                    }
                }
                if (this.LayoutSystem.AutoHideBar != null)
                {
                    this.LayoutSystem.AutoHideBar.method_7(this, true, bool_8);
                    return;
                }
            }
            if (bool_8)
            {
                Activate();
            }
        }

        internal bool method_13(ContainerDockLocation location)
        {
            switch (location)
            {
                case ContainerDockLocation.Left:
                    return DockingRules.AllowDockLeft;
                case ContainerDockLocation.Right:
                    return DockingRules.AllowDockRight;
                case ContainerDockLocation.Top:
                    return DockingRules.AllowDockTop;
                case ContainerDockLocation.Bottom:
                    return DockingRules.AllowDockBottom;
                default:
                    return DockingRules.AllowTab;
            }
        }

        internal bool method_14(bool bool_8)
        {
            var e = new DockControlClosingEventArgs(this, false);
            Manager?.OnDockControlClosing(e);
            if (!e.Cancel)
                OnClosing(e);
            if (e.Cancel)
                return false;
            LayoutUtilities.smethod_11(this);
            OnClosed(EventArgs.Empty);
            if (CloseAction == DockControlCloseAction.Dispose)
                Dispose();
            return true;
        }

        internal void method_15(ControlLayoutSystem layoutSystem, int index)
        {
            if (LayoutSystem != layoutSystem)
            {
                LayoutUtilities.smethod_11(this);
                layoutSystem.Controls.Insert(index, this);
            }
            else
            {
                layoutSystem.Controls.SetChildIndex(this, index);
            }
            layoutSystem.SelectedControl = this;
        }

        internal void method_16(ControlLayoutSystem layoutSystem)
        {
            LayoutSystem = layoutSystem;
        }

        private void method_17(DockSituation situation)
        {
            if (_dockSituationChanging)
                throw new InvalidOperationException("The requested operation is not valid on a window that is currently engaged in an activity further up the call stack. Consider using BeginInvoke to postpone the operation until the stack has unwound.");
            if (situation == DockSituation) return;
            DockSituation = situation;
            _dockSituationChanging = true;
            try
            {
                OnDockSituationChanged(EventArgs.Empty);
            }
            finally
            {
                _dockSituationChanging = false;
            }
        }

        internal void method_2()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void method_3()
        {
            if (this.bool_1) return;
            if (Manager?.DocumentContainer == null || !Manager.DocumentContainer.Boolean_3)
                MetaData.SaveFocused(DateTime.Now);
            Manager?.OnDockControlActivated(new DockControlEventArgs(this));
        }

        internal void method_4(DockContainer container)
        {
            if (container?.Manager != null && container.Manager != Manager)
                Manager = container.Manager;
            this.method_5();
        }

        internal void method_5()
        {
            var dockSituation = LayoutSystem?.DockContainer != null ? LayoutUtilities.smethod_2(LayoutSystem.DockContainer) : DockSituation.None;
            if (dockSituation != DockSituation.None)
                MetaData.SaveOpenDockSituation(dockSituation);

            Class18 @class = null;
            switch (dockSituation)
            {
                case DockSituation.Docked:
                    @class = MetaData.Class19_0;
                    this.MetaData.SaveFixedDockSituation(DockSituation.Docked);
                    this.MetaData.SaveFixedDockSide(LayoutUtilities.Convert(LayoutSystem.DockContainer.Dock));
                    this.MetaData.SaveDockedContentSize(this.LayoutSystem.DockContainer.ContentSize);
                    if (this.Manager != null)
                    {
                        DockContainer[] dockContainers = Manager.GetDockContainers(LayoutSystem.DockContainer.Dock);
                        this.MetaData.Class19_0.Int32_3 = dockContainers.Length;
                        this.MetaData.Class19_0.Int32_2 = Array.IndexOf<DockContainer>(dockContainers, LayoutSystem.DockContainer);
                    }
                    break;
                case DockSituation.Document:
                    @class = this.MetaData.Class18_0;
                    MetaData.SaveFixedDockSituation(DockSituation.Document);
                    break;
                case DockSituation.Floating:
                    @class = this.MetaData.Class18_1;
                    MetaData.SaveFloatingWindowGuid(((FloatingContainer)LayoutSystem.DockContainer).Guid);
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
            if (LayoutSystem == null) return;
            class18_0.Guid_0 = LayoutSystem.Guid;
            class18_0.Int32_1 = LayoutSystem.Controls.IndexOf(this);
            class18_0.SizeF_0 = LayoutSystem.WorkingSize;
            class18_0.Int32_0 = LayoutUtilities.smethod_5(LayoutSystem);
        }

        private void EnsureNotDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        internal void method_8()
        {
            CreateControl();
        }

        private void EnsureDockSystemContainerNotNull()
        {
            EnsureNotDisposed();
            if (_manager == null)
                throw new InvalidOperationException("No SandDockManager is associated with this DockControl. To create an association, set the Manager property.");
        }

        protected internal virtual void OnAutoHidePopupClosed(EventArgs e)
        {
            AutoHidePopupClosed?.Invoke(this, e);
        }

        protected internal virtual void OnAutoHidePopupOpened(EventArgs e)
        {
            AutoHidePopupOpened?.Invoke(this, e);
        }

        protected internal virtual void OnClosed(EventArgs e) => Closed?.Invoke(this, e);

        protected internal virtual void OnClosing(DockControlClosingEventArgs e) => Closing?.Invoke(this, e);

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            OnLoad(EventArgs.Empty);
        }

        protected virtual void OnDockSituationChanged(EventArgs e) => DockSituationChanged?.Invoke(this, e);

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (LayoutSystem != null)
                LayoutSystem.Boolean_1 = true;
            this.method_3();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (!Boolean_0)
                CalculateAllMetricsAndLayout();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (LayoutSystem != null)
                LayoutSystem.Boolean_1 = false;
        }

        protected virtual void OnLoad(EventArgs e) => Load?.Invoke(this, e);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            smethod_0(this, e.Graphics, _borderStyle);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            switch (BorderStyle)
            {
                case Rendering.BorderStyle.Flat:
                case Rendering.BorderStyle.RaisedThin:
                case Rendering.BorderStyle.SunkenThin:
                    rect.Inflate(-1, -1);
                    break;
                case Rendering.BorderStyle.RaisedThick:
                case Rendering.BorderStyle.SunkenThick:
                    rect.Inflate(-2, -2);
                    break;
            }
            var backColor = BackColor;
            var borderColor = Color.Transparent;
            Manager?.Renderer.ModifyDefaultWindowColors(this, ref backColor, ref borderColor);
            if (rect != ClientRectangle)
                e.Graphics.SetClip(rect);
            if (BackgroundImage == null)
                Class16.smethod_0(e.Graphics, backColor);
            else
                base.OnPaintBackground(e);
        }

        protected internal virtual void OnTabDoubleClick()
        {
            switch (DockSituation)
            {
                case DockSituation.Docked:
                case DockSituation.Document:
                    if (DockingRules.AllowFloat)
                        OpenFloating(WindowOpenMethod.OnScreenActivate);
                    return;
                case DockSituation.Floating:
                    if (MetaData.LastFixedDockSituation == DockSituation.Docked && this.method_13(MetaData.LastFixedDockSide))
                    {
                        OpenDocked(WindowOpenMethod.OnScreenActivate);
                        return;
                    }
                    if (MetaData.LastFixedDockSituation == DockSituation.Document && this.method_13(ContainerDockLocation.Center))
                        OpenDocument(WindowOpenMethod.OnScreenActivate);
                    return;
                default:
                    return;
            }
        }

        public abstract void Open();

        public void Open(WindowOpenMethod openMethod)
        {
            EnsureDockSystemContainerNotNull();
            if (DockSituation == DockSituation.None)
            {
                switch (MetaData.LastOpenDockSituation)
                {
                    case DockSituation.Docked:
                        OpenDocked(openMethod);
                        return;
                    case DockSituation.Document:
                        OpenDocument(openMethod);
                        return;
                    case DockSituation.Floating:
                        OpenFloating(openMethod);
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
            existingWindow.LayoutSystem.SplitForLayoutSystem(new ControlLayoutSystem(MetaData.Class19_0.SizeF_0, new[] { this }, this), side);
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
            EnsureDockSystemContainerNotNull();
            this.method_8();
            if (DockSituation == DockSituation.Docked)
                return;
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
            controlLayoutSystem = @struct.SplitLayout.DockContainer.CreateNewLayoutSystem(this, this.MetaData.Class19_0.SizeF_0);
            if (this.MetaData.Class19_0.Guid_0 == Guid.Empty)
            {
                this.MetaData.Class19_0.Guid_0 = Guid.NewGuid();
            }
            controlLayoutSystem.Guid = this.MetaData.Class19_0.Guid_0;
            @struct.SplitLayout.LayoutSystems.Insert(@struct.Index, controlLayoutSystem);
            if (openMethod != WindowOpenMethod.OnScreen)
            {
                this.method_12(openMethod == WindowOpenMethod.OnScreenActivate);
            }
        }

        public void OpenDocked(ContainerDockLocation dockLocation, WindowOpenMethod openMethod)
        {
            if (dockLocation == ContainerDockLocation.Center)
            {
                OpenDocument(openMethod);
                return;
            }
            EnsureDockSystemContainerNotNull();
            if (DockSituation == DockSituation.Docked && MetaData.LastFixedDockSide == dockLocation)
                return;

            Remove();
            MetaData.SaveFixedDockSide(dockLocation);
            MetaData.Class19_0.Guid_0 = Guid.Empty;
            MetaData.Class19_0.Int32_0 = new int[0];
            OpenDocked(openMethod);
        }

        public void OpenDocument(WindowOpenMethod openMethod)
        {
            EnsureDockSystemContainerNotNull();
            this.method_8();
            if (DockSituation == DockSituation.Document)
                return;

            Remove();
            ControlLayoutSystem controlLayoutSystem = LayoutUtilities.smethod_4(Manager, DockSituation.Document, this.MetaData.Class18_0);
            if (controlLayoutSystem != null)
            {
                controlLayoutSystem.Controls.Insert(Math.Min(this.MetaData.Class19_0.Int32_1, controlLayoutSystem.Controls.Count), this);
                if (openMethod != WindowOpenMethod.OnScreen)
                {
                    this.method_12(openMethod == WindowOpenMethod.OnScreenActivate);
                }
                return;
            }
            DockContainer dockContainer = Manager.FindDockContainer(ContainerDockLocation.Center) ?? Manager.CreateNewDockContainer(ContainerDockLocation.Center, ContainerDockEdge.Inside, MetaData.DockedContentSize);
            controlLayoutSystem = LayoutUtilities.FindControlLayoutSystem(dockContainer);
            if (controlLayoutSystem == null)
            {
                controlLayoutSystem = dockContainer.CreateNewLayoutSystem(this, this.MetaData.Class18_0.SizeF_0);
                dockContainer.LayoutSystem.LayoutSystems.Add(controlLayoutSystem);
            }
            else if (Manager.DocumentOpenPosition != DocumentContainerWindowOpenPosition.First)
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
            EnsureDockSystemContainerNotNull();
            this.method_8();
            if (DockSituation == DockSituation.Floating)
                return;

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
            FloatingContainer @class = Manager.FindFloatingDockContainer(MetaData.LastFloatingWindowGuid);
            if (@class != null)
            {
                Struct0 @struct = LayoutUtilities.smethod_15(@class, MetaData.Class18_1.Int32_0);
                controlLayoutSystem = @struct.SplitLayout.DockContainer.CreateNewLayoutSystem(this, this.MetaData.Class18_1.SizeF_0);
                if (this.MetaData.Class18_1.Guid_0 == Guid.Empty)
                {
                    this.MetaData.Class18_1.Guid_0 = Guid.NewGuid();
                }
                controlLayoutSystem.Guid = this.MetaData.Class18_1.Guid_0;
                @struct.SplitLayout.LayoutSystems.Insert(@struct.Index, controlLayoutSystem);
                return;
            }
            if (MetaData.LastFloatingWindowGuid == Guid.Empty)
            {
                MetaData.SaveFloatingWindowGuid(Guid.NewGuid());
            }
            @class = new FloatingContainer(Manager, MetaData.LastFloatingWindowGuid);
            controlLayoutSystem = @class.CreateNewLayoutSystem(this, this.MetaData.Class18_1.SizeF_0);
            if (this.MetaData.Class18_1.Guid_0 == Guid.Empty)
            {
                this.MetaData.Class18_1.Guid_0 = Guid.NewGuid();
            }
            controlLayoutSystem.Guid = MetaData.Class18_1.Guid_0;
            @class.LayoutSystem.LayoutSystems.Add(controlLayoutSystem);
            @class.method_19(rectangle_, true, openMethod == WindowOpenMethod.OnScreenActivate);
        }

        public void OpenFloating(Rectangle bounds, WindowOpenMethod openMethod)
        {
            EnsureDockSystemContainerNotNull();
            Remove();
            MetaData.SaveFloatingWindowGuid(Guid.Empty);
            MetaData.Class18_1.Guid_0 = Guid.Empty;
            FloatingLocation = bounds.Location;
            FloatingSize = bounds.Size;
            OpenFloating(openMethod);
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
            if (LayoutSystem == null || !AllowKeyboardNavigation)
                return base.ProcessCmdKey(ref msg, keyData);

            if (keyData == (Keys.LButton | Keys.Space | Keys.Control))
            {
                var index = LayoutSystem.Controls.IndexOf(this);
                index--;
                if (index < 0)
                    index = LayoutSystem.Controls.Count - 1;
                LayoutSystem.SelectedControl = LayoutSystem.Controls[index];
                if (LayoutSystem.SelectedControl == LayoutSystem.Controls[index])
                    LayoutSystem.Controls[index].Activate();
                return true;
            }
            if (keyData == (Keys.RButton | Keys.Space | Keys.Control))
            {
                var index = LayoutSystem.Controls.IndexOf(this);
                index++;
                if (index >= LayoutSystem.Controls.Count)
                    index = 0;
                LayoutSystem.SelectedControl = LayoutSystem.Controls[index];
                if (LayoutSystem.SelectedControl == LayoutSystem.Controls[index])
                    LayoutSystem.Controls[index].Activate();
                return true;
            }
            if (keyData == (Keys.LButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17 | Keys.Alt) && LayoutSystem.IsInContainer)
            {
                LayoutSystem.DockContainer.ShowControlContextMenu(new ShowControlContextMenuEventArgs(this, new Point(0, 0), ContextMenuContext.Keyboard));
                return true;
            }
            if (keyData == Keys.Escape && Manager != null && DockSituation != DockSituation.Document)
            {
                Manager.OwnerForm?.Activate();
                var dockControl = Manager.FindMostRecentlyUsedWindow(DockSituation.Document);
                dockControl?.Activate();
                return true;
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
            MetaData.SaveOpenDockSituation(dockSituation);
            MetaData.SaveFixedDockSituation(dockSituation);
        }

        public void SetPositionMetaData(DockSituation dockSituation, ContainerDockLocation dockLocation)
        {
            if (DockSituation != DockSituation.None)
                throw new InvalidOperationException("This operation is only valid when the window is not currently open.");
            if (dockSituation == DockSituation.None)
                throw new ArgumentException("dockSituation");
            if (dockLocation == ContainerDockLocation.Center) throw new ArgumentException("dockLocation");
            MetaData.SaveOpenDockSituation(dockSituation);
            if (dockSituation == DockSituation.Document || dockSituation == DockSituation.Docked)
                MetaData.SaveFixedDockSituation(dockSituation);
            MetaData.SaveFixedDockSide(dockLocation);

        }

        private bool ShouldSerializeDockingRules()
        {
            var rules = CreateDockingRules();
            return rules.AllowDockTop != DockingRules.AllowDockTop || rules.AllowDockBottom != DockingRules.AllowDockBottom ||
                   rules.AllowDockLeft != DockingRules.AllowDockLeft || rules.AllowDockRight != DockingRules.AllowDockRight ||
                   rules.AllowTab != DockingRules.AllowTab || rules.AllowFloat != DockingRules.AllowFloat;
        }

        private bool ShouldSerializeTabText() => _tabText.Length != 0 && _tabText != Text;

        internal static void smethod_0(Control control, Graphics g, Rendering.BorderStyle borderStyle)
        {
            if (borderStyle == Rendering.BorderStyle.None)
                return;
            var rectangle = new Rectangle(0, 0, control.Width, control.Height);
            if (borderStyle != Rendering.BorderStyle.Flat)
            {
                Border3DStyle style;
                switch (borderStyle)
                {
                    case Rendering.BorderStyle.Flat:
                        style = Border3DStyle.Flat;
                        break;
                    case Rendering.BorderStyle.RaisedThick:
                        style = Border3DStyle.Raised;
                        break;
                    case Rendering.BorderStyle.RaisedThin:
                        style = Border3DStyle.RaisedInner;
                        break;
                    case Rendering.BorderStyle.SunkenThick:
                        style = Border3DStyle.Sunken;
                        break;
                    default:
                        style = Border3DStyle.SunkenOuter;
                        break;
                }
                ControlPaint.DrawBorder3D(g, rectangle, style);
                return;
            }
            var backColor = control.BackColor;
            var borderColor = SystemColors.ControlDark;
            var dockControl = control as DockControl;
            dockControl?.Manager?.Renderer.ModifyDefaultWindowColors(dockControl, ref backColor, ref borderColor);
            rectangle.Width--;
            rectangle.Height--;
            using (var pen = new Pen(borderColor))
                g.DrawRectangle(pen, rectangle);
        }

        public void Split(DockSide direction)
        {
            if (!IsInContainer)
                throw new InvalidOperationException("A window cannot be split while it is not hosted in a DockContainer.");
            if (LayoutSystem.Controls.Count < 2)
                throw new InvalidOperationException("A minimum of 2 windows need to be present in a tab group before one can be split off. Check LayoutSystem.Controls.Count before calling this method.");
            if (direction == DockSide.None)
                throw new ArgumentException("direction");

            var layoutSystem = LayoutSystem;
            LayoutUtilities.smethod_11(this);
            var newLayoutSystem = layoutSystem.DockContainer.CreateNewLayoutSystem(this, LayoutSystem.WorkingSize);
            layoutSystem.SplitForLayoutSystem(newLayoutSystem, direction);
            Activate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WMConstants.WM_MOUSEACTIVATE && !ContainsFocus)
                Activate();
        }

        [Category("Docking"), DefaultValue(true), Description("Indicates whether this control will be closable by the user.")]
        public virtual bool AllowClose
        {
            get
            {
                return _allowClose;
            }
            set
            {
                _allowClose = value;
                CalculateAllMetricsAndLayout();
            }
        }

        [Category("Docking"), DefaultValue(true), Description("Indicates whether the user will be able to put this control in to auto-hide mode.")]
        public virtual bool AllowCollapse
        {
            get
            {
                return _allowCollapse;
            }
            set
            {
                _allowCollapse = value;
                if (Collapsed && !value)
                    Collapsed = false;
                this.CalculateAllMetricsAndLayout();
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
                if (_bindingContext != null) return _bindingContext;
                if (Manager?.DockSystemContainer != null) return Manager.DockSystemContainer.BindingContext;
                return DesignMode ? base.BindingContext : null;
            }
            set
            {
                _bindingContext = value;
                base.BindingContext = value;
            }
        }

        internal bool Boolean_0 { get; set; }

        [Browsable(false)]
        internal bool IsInContainer => LayoutSystem?.DockContainer != null;

        [Category("Appearance"), DefaultValue(Rendering.BorderStyle.None), Description("The type of border to be drawn around the control.")]
        public Rendering.BorderStyle BorderStyle
        {
            get
            {
                return _borderStyle;
            }
            set
            {
                _borderStyle = value;
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
                switch (_borderStyle)
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
                return _dockingRules;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _dockingRules = value;
            }
        }

        [Browsable(false)]
        public DockSituation DockSituation { get; private set; }

        [Browsable(false), DefaultValue(typeof(Point), "-1, -1")]
        public Point FloatingLocation
        {
            get
            {
                return _floatingLocation;
            }
            set
            {
                _floatingLocation = value;
                if (DockSituation == DockSituation.Floating && Class5_0.FloatingLocation != _floatingLocation)
                    Class5_0.FloatingLocation = _floatingLocation;
            }
        }

        [Category("Layout"), DefaultValue(typeof(Size), "250, 400"), Description("Indicates the default size this control will assume when floating on its own.")]
        public Size FloatingSize
        {
            get
            {
                return _floatingSize;
            }
            set
            {
                if (value.Width <= 0 || value.Height <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                _floatingSize = value;
                if (DockSituation == DockSituation.Floating && Class5_0.FloatingSize != _floatingSize)
                    Class5_0.FloatingSize = _floatingSize;
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
                LayoutSystem?.DockContainer?.Invalidate(TabBounds);
            }
        }

        [Category("Advanced"), Description("The unique identifier for the window.")]
        public Guid Guid
        {
            get
            {
                return _guid;
            }
            set
            {
                var oldGuid = _guid;
                _guid = value;
                Manager?.ReRegisterWindow(this, oldGuid);
            }
        }

        internal Image Image_0 => _tabImage ?? image_0;

        [Browsable(false), Obsolete("Use the DockSituation property instead.")]
        public bool IsDocked => IsInContainer && !(LayoutSystem.DockContainer is DocumentContainer) && !(LayoutSystem.DockContainer is FloatingContainer);

        [Browsable(false), Obsolete("Use the DockSituation property instead.")]
        public bool IsFloating => IsInContainer && LayoutSystem.DockContainer.IsFloating;

        [Browsable(false)]
        public bool IsOpen => IsInContainer && LayoutSystem != null && LayoutSystem.SelectedControl == this && LayoutSystem.Collapsed && LayoutSystem.IsPoppedUp;

        [Browsable(false), Obsolete("Use the DockSituation property instead.")]
        public bool IsTabbedDocument => IsInContainer && LayoutSystem.DockContainer is DocumentContainer;

        [Browsable(false)]
        public ControlLayoutSystem LayoutSystem { get; private set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SandDockManager Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                if (_manager == value) return;
                _manager?.UnregisterWindow(this);
                _manager = value;
                _manager?.RegisterWindow(this);
            }
        }

        [Category("Layout"), DefaultValue(0), Description("Indicates the maximum width of the tab representing the window.")]
        public int MaximumTabWidth
        {
            get
            {
                return _maximumTabWidth;
            }
            set
            {
                if (value < 0) throw new ArgumentException("Value must be greater than or equal to zero.");
                _maximumTabWidth = value;
                CalculateAllMetricsAndLayout();
            }
        }

        [Browsable(false)]
        public WindowMetaData MetaData { get; }

        [Category("Layout"), DefaultValue(0), Description("Indicates the minimum width of the tab representing the window.")]
        public int MinimumTabWidth
        {
            get
            {
                return _minimumTabWidth;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                _minimumTabWidth = value;
                CalculateAllMetricsAndLayout();
            }
        }

        [Browsable(true), Category("Behavior"), DefaultValue(true), Description("Indicates whether the location of the DockControl will be included in layout serialization.")]
        public virtual bool PersistState { get; set; } = true;

        [Category("Docking"), DefaultValue(0), Description("The size of the control when popped up from a collapsed state. Leave this as zero for the default size.")]
        public int PopupSize
        {
            get
            {
                return _popupSize;
            }
            set
            {
                if (value < 0) throw new ArgumentException("Value must be at least equal to zero.");
                _popupSize = value;
                if (!MetaData.Boolean_0)
                    MetaData.SaveDockedContentSize(value);
                if (LayoutSystem?.AutoHideBar != null && LayoutSystem.AutoHideBar.LayoutSystem == LayoutSystem)
                    LayoutSystem.AutoHideBar.Int32_1 = value;
            }
        }

        [Category("Behavior"), DefaultValue(typeof(Control), null), Description("The control that will be focused when the window is activated.")]
        public Control PrimaryControl { get; set; }

        [Category("Appearance"), DefaultValue(true), Description("Indicates whether an options button will be displayed in the titlebar for this window.")]
        public bool ShowOptions
        {
            get
            {
                return _showOptions;
            }
            set
            {
                _showOptions = value;
                CalculateAllMetricsAndLayout();
            }
        }

        [Browsable(false)]
        public Rectangle TabBounds { get; internal set; }

        [AmbientValue(typeof(Image), null), Category("Appearance"), DefaultValue(typeof(Image), null), Description("The image displayed for this control on docking tabs.")]
        public Image TabImage
        {
            get
            {
                return _tabImage;
            }
            set
            {
                _tabImage = value;
                CalculateAllMetricsAndLayout();
            }
        }

        [Category("Appearance"), Description("The text to display on the tab for the DockControl. This can be different to the standard text."), Localizable(true)]
        public virtual string TabText
        {
            get
            {
                return _tabText.Length != 0 ? _tabText : Text;
            }
            set
            {
                _tabText = value ?? string.Empty;
                CalculateAllMetricsAndLayout();
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
                CalculateAllMetricsAndLayout();
                if (DockSituation == DockSituation.Floating && Class5_0.HasSingleControlLayoutSystem && LayoutSystem.SelectedControl == this)
                    Class5_0.method_21();
            }
        }

        [Category("Appearance"), DefaultValue(""), Description("Gets or sets the text that appears as a ToolTip for the control tab."), Localizable(true)]
        public virtual string ToolTipText
        {
            get
            {
                return _toolTipText;
            }
            set { _toolTipText = value ?? string.Empty; }
        }

        public event EventHandler AutoHidePopupClosed;

        public event EventHandler AutoHidePopupOpened;

        public event EventHandler Closed;

        public event DockControlClosingEventHandler Closing;

        public event EventHandler DockSituationChanged;

        public event EventHandler Load;

        private BindingContext _bindingContext;

        private bool bool_1;

        private bool _dockSituationChanging;

        internal bool bool_3;

        private bool _allowClose = true;

        private bool _allowCollapse = true;

        private bool _showOptions = true;

        private Rendering.BorderStyle _borderStyle;

        private DockingRules _dockingRules;

        private Guid _guid = Guid.NewGuid();

        private static Image image_0;

        private Image _tabImage;

        private int _popupSize;

        private int _maximumTabWidth;

        private int _minimumTabWidth;

        private Point _floatingLocation = new Point(-1, -1);

        //private Rectangle _tabBounds = Rectangle.Empty;

        internal Rectangle rectangle_1 = Rectangle.Empty;

        private SandDockManager _manager;

        private Size _floatingSize;

        private string _toolTipText = "";

        private string _tabText = string.Empty;
    }
}
