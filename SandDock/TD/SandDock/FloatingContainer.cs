using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TD.Util;

namespace TD.SandDock
{
    internal class FloatingForm : Form
    {
        public FloatingForm(FloatingContainer container)
        {
            _parent = container;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
        }

        [Naming]
        private bool ShowContextMenu()
        {
            if (!_parent.HasSingleControlLayoutSystem) return false;
            var layoutSystem = (ControlLayoutSystem)_parent.LayoutSystem.LayoutSystems[0];
            if (layoutSystem.SelectedControl == null) return false;
            _parent.ShowControlContextMenu(new ShowControlContextMenuEventArgs(layoutSystem.SelectedControl, layoutSystem.SelectedControl.PointToClient(Cursor.Position), ContextMenuContext.RightClick));
            return true;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (_parent.ActiveControl != null) return;
            var layoutSystem = LayoutUtilities.FindControlLayoutSystem(_parent);
            if (layoutSystem?.SelectedControl != null)
                _parent.ActiveControl = layoutSystem.SelectedControl;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.Left || _dragPoint == Point.Empty) return;
            var r = new Rectangle(_dragPoint, SystemInformation.DragSize);
            r.Offset(-SystemInformation.DragSize.Width / 2, -SystemInformation.DragSize.Height / 2);
            if (r.Contains(e.X, e.Y)) return;
            _dragPoint.Y = _dragPoint.Y + SystemInformation.ToolWindowCaptionHeight + SystemInformation.FrameBorderSize.Height;
            _parent.LayoutSystem.StartDockingSession(_parent.Manager, _parent, _parent.LayoutSystem, null, _parent.SelectedControl.MetaData.DockedContentSize, _dragPoint, _parent.Manager.DockingHints, _parent.Manager.DockingManager);
            _parent._layoutSystem = _parent.LayoutSystem;
            Capture = false;
            _parent.Capture = true;
            _dragPoint = Point.Empty;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _dragPoint = Point.Empty;
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            if (_parent == null) return;
            foreach (var control in _parent.LayoutSystem.AllControls)
                control.FloatingLocation = Location;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_parent == null) return;
            foreach (var control in _parent.LayoutSystem.AllControls)
                control.FloatingSize = Size;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WMConstants.WM_NCLBUTTONDOWN && m.WParam.ToInt32() == WMConstants.HTCAPTION)
            {
                Native.ReleaseCapture();
                Activate();
                _dragPoint = PointToClient(Cursor.Position);
                Capture = true;
                m.Result = IntPtr.Zero;
                return;
            }
            if (m.Msg == WMConstants.WM_NCLBUTTONDBLCLK && m.WParam.ToInt32() == WMConstants.HTCAPTION)
            {

                OnDoubleClick(EventArgs.Empty);
                m.Result = IntPtr.Zero;
                return;
            }
            if (m.Msg == WMConstants.WM_NCRBUTTONDOWN)
            {
                Capture = false;
                if (ShowContextMenu())
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private readonly FloatingContainer _parent;

        private Point _dragPoint;
    }

    internal class FloatingContainer : DockContainer
    {
        public FloatingContainer(SandDockManager manager, Guid guid)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            FloatingForm = new FloatingForm(this);
            FloatingForm.Activated += OnActivated;
            FloatingForm.Deactivate += OnDeactivate;
            FloatingForm.Closing += OnClosing;
            FloatingForm.DoubleClick += OnDoubleClick;
            LayoutSystem.LayoutSystemsChanged += OnLayoutSystemsChanged;
            OnLayoutSystemsChanged(LayoutSystem, EventArgs.Empty);
            Manager = manager;
            Guid = guid;
            FloatingForm.Controls.Add(this);
            Dock = DockStyle.Fill;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                LayoutSystem.LayoutSystemsChanged -= OnLayoutSystemsChanged;
                FloatingForm.Activated -= OnActivated;
                FloatingForm.Deactivate -= OnDeactivate;
                FloatingForm.Closing -= OnClosing;
                FloatingForm.DoubleClick -= OnDoubleClick;
                LayoutUtilities.smethod_8(this);
                FloatingForm.Dispose();
            }
            base.Dispose(disposing);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!bool_2) return;
            var controls = LayoutSystem.AllControls;
            if (controls.All(control => control.AllowClose))
            {
                if (controls.Any(c => !c.Close()))
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            var controls = LayoutSystem.AllControls;
            var selected = SelectedControl;
            if (controls[0].MetaData.LastFixedDockSituation == DockSituation.Docked && !LayoutSystem.AllowDock(selected.MetaData.LastFixedDockSide))
                return;
            if (controls[0].MetaData.LastFixedDockSituation == DockSituation.Document && !LayoutSystem.AllowDock(ContainerDockLocation.Center))
                return;
            LayoutSystem = new SplitLayoutSystem();
            Dispose();
            if (selected.MetaData.LastFixedDockSituation == DockSituation.Docked)
                controls[0].OpenDocked(WindowOpenMethod.OnScreenActivate);
            else
                controls[0].OpenDocument(WindowOpenMethod.OnScreenActivate);
            var array = new DockControl[controls.Length - 1];
            Array.Copy(controls, 1, array, 0, controls.Length - 1);
            controls[0].LayoutSystem.Controls.AddRange(array);
            controls[0].LayoutSystem.SelectedControl = selected;
        }

        public void ShowForm()
        {
            FloatingForm.Show();
        }

        public void HideForm()
        {
            FloatingForm.Hide();
        }

        public void method_19(Rectangle bounds, bool visible, bool activated)
        {
            var flags = 0;
            flags |= visible ? WMConstants.SWP_SHOWWINDOW : WMConstants.SWP_HIDEWINDOW;
            if (!activated)
                flags |= WMConstants.SWP_NOACTIVATE;

            Native.SetWindowPos(FloatingForm.Handle, WMConstants.HWND_TOP, bounds.X, bounds.Y, bounds.Width, bounds.Height, flags);
            FloatingForm.Location = bounds.Location;
            FloatingForm.Size = bounds.Size;
            FloatingForm.Visible = visible;
            if (visible)
                foreach (Control control in FloatingForm.Controls)
                    control.Visible = true;
        }

        private void OnSelectedControlChanged(DockControl oldSelection, DockControl newSelection)
        {
            FloatingForm.Text = newSelection == null ? "" : newSelection.Text;
        }

        public void method_21()
        {
            OnLayoutSystemsChanged(null, null);
        }

        private void OnLayoutSystemsChanged(object sender, EventArgs e)
        {
            if (controlLayoutSystem_0 != null)
            {
                controlLayoutSystem_0.SelectedControlChanged -= OnSelectedControlChanged;
            }
            if (HasSingleControlLayoutSystem)
            {
                controlLayoutSystem_0 = (ControlLayoutSystem) LayoutSystem.LayoutSystems[0];
                controlLayoutSystem_0.SelectedControlChanged += OnSelectedControlChanged;
                OnSelectedControlChanged(null, controlLayoutSystem_0.SelectedControl);
            }
            else
            {
                FloatingForm.Text = "";
                controlLayoutSystem_0 = null;
            }
        }

        internal void Activate()
        {
            FloatingForm.Activate();
        }

        [Naming(NamingType.FromOldVersion)]
        internal override bool CanShowCollapsed => false;

        public DockControl SelectedControl
        {
            get
            {
                var layoutSystem = LayoutUtilities.FindControlLayoutSystem(this);
                if (layoutSystem == null) throw new InvalidOperationException("A docking operation was started while the window hierarchy is in an invalid state.");
                return layoutSystem.SelectedControl;
            }
        }

        [Naming(NamingType.FromOldVersion)]
        public Form FloatingForm { get; }

        public Guid Guid { get; }

        public override bool IsFloating => true;

        public override SplitLayoutSystem LayoutSystem
        {
            get
            {
                return base.LayoutSystem;
            }
            set
            {
                LayoutSystem.LayoutSystemsChanged -= OnLayoutSystemsChanged;
                base.LayoutSystem = value;
                LayoutSystem.LayoutSystemsChanged += OnLayoutSystemsChanged;
                OnLayoutSystemsChanged(LayoutSystem, EventArgs.Empty);
            }
        }

        public override SandDockManager Manager
        {
            get
            {
                return base.Manager;
            }
            set
            {
                Manager?.OwnerForm?.RemoveOwnedForm(FloatingForm);
                base.Manager = value;
                if (Manager?.OwnerForm == null) return;
                Manager.OwnerForm.AddOwnedForm(FloatingForm);
                Font = new Font(Manager.OwnerForm.Font, Manager.OwnerForm.Font.Style);
            }
        }

        public Point FloatingLocation
        {
            get
            {
                return FloatingForm.Location;
            }
            set
            {
                FloatingForm.Location = value;
            }
        }

        public Rectangle FloatingBounds => FloatingForm.Bounds;

        public Size FloatingSize
        {
            get
            {
                return FloatingForm.Size;
            }
            set
            {
                FloatingForm.Size = value;
            }
        }

        private bool bool_2 = true;

        private ControlLayoutSystem controlLayoutSystem_0;
    }
}
