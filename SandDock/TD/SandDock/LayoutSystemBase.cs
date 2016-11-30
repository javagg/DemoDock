using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using TD.SandDock.Rendering;
using TD.Util;

namespace TD.SandDock
{
    internal class TranslucentForm : Form
    {
        public TranslucentForm(bool hollow)
        {
            _hollow = hollow;
            BackColor = SystemColors.Highlight;
            ShowInTaskbar = false;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public void method_0(Rectangle bounds, bool bool_1)
        {
            // Native.SetWindowPos(Handle, WMConstants.HWND_TOP, bounds.X, bounds.Y, bounds.Width, bounds.Height, WMConstants.SWP_SHOWWINDOW + WMConstants.SWP_NOACTIVATE);
            Location = bounds.Location;
            Size = bounds.Size;
            Visible = true;
        }

        protected override bool ShowWithoutActivation => true;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            TransparencyKey = BackColor;
            if (!Native.IsMono())
                Native.SetLayeredWindowAttributes(Handle, 0, Alpha, WMConstants.LWA_ALPHA);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_hollow)
            {
                var rect = ClientRectangle;
                rect.Width--;
                rect.Height--;
                e.Graphics.DrawRectangle(SystemPens.ControlDark, rect);
                rect.Inflate(-1, -1);
                e.Graphics.DrawRectangle(SystemPens.ControlDark, rect);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.Style = WMConstants.WS_OVERLAPPED; //- 2147483648;
                cp.ExStyle |= WMConstants.WS_EX_NOACTIVATE | WMConstants.WS_EX_TOOLWINDOW | WMConstants.WS_EX_TOPMOST;
                return cp;
            }
        }

        private readonly bool _hollow;

        //private const int SM_REMOTESESSION = 0x1000;

        //private const int WS_SYSMENU = 0x00080000; //524288 
        //private const int WS_OVERLAPPED = 0;
        private const int int_2 = 16;

        private const int int_3 = 64;

        private const int int_4 = 2;

        private const int int_5 = 1;

        private const byte Alpha = 128;
    }

    internal abstract class AbstractManager : IDisposable, IMessageFilter
    {
        protected AbstractManager(Control control, DockingHints dockingHints, bool hollow)
        {
            _control = control;
            _hollow = hollow;
#if NO_LAYERED_WINDOW
            var flag = Native.IsMono(); 
#else
            var flag = OSFeature.Feature.IsPresent(OSFeature.LayeredWindows);
#endif
            if (dockingHints == DockingHints.TranslucentFill && !flag)
                dockingHints = DockingHints.RubberBand;

            _dockingHints = dockingHints;
            _form = _control.FindForm();
            if (_form != null)
                _form.Deactivate += OnDeactivate;
            _control.MouseCaptureChanged += OnMouseCaptureChanged;
            Application.AddMessageFilter(this);
            if (_dockingHints == DockingHints.TranslucentFill)
                _translucentForm = new TranslucentForm(hollow);
        }

        protected AbstractManager(Control control, DockingHints dockingHints, bool hollow, int tabStripSize) : this(control, dockingHints, hollow)
        {
            _tabStripSize = tabStripSize;
        }

        public virtual void Cancel()
        {
            Dispose();
            Cancalled?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Commit()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            if (_control != null)
                _control.MouseCaptureChanged -= OnMouseCaptureChanged;

            UpdateHintForm();
            if (_dockingHints == DockingHints.TranslucentFill)
            {
                _translucentForm.Dispose();
                _translucentForm = null;
            }
            if (_form != null)
            {
                _form.Deactivate -= OnDeactivate;
            }
            Application.RemoveMessageFilter(this);
            _form = null;
            _control = null;
        }

        private void OnDeactivate(object sender, EventArgs e)
        {
            Cancel();
        }

        private void OnMouseCaptureChanged(object sender, EventArgs e)
        {
            Cancel();
        }

        protected void method_1(Rectangle bounds, bool flags)
        {
            if (_hintRect == bounds) return;
            if (_dockingHints == DockingHints.RubberBand)
                DrawRubberBandHints();

            if (_dockingHints == DockingHints.RubberBand)
            {
                if (_hollow)
                    Native.DrawRubberBands(null, bounds, flags, _tabStripSize);
                else
                    Native.DrawRubberBand(null, bounds);
                _hintRect = bounds;
                bool_0 = flags;
            }
            else
            {
                _translucentForm.method_0(bounds, flags);
            }
        }

        [Naming]
        protected void UpdateHintForm()
        {
            if (_dockingHints == DockingHints.RubberBand)
                DrawRubberBandHints();
            else
                _translucentForm.Hide();
        }

        [Naming]
        private void DrawRubberBandHints()
        {
            if (_hintRect != Rectangle.Empty)
            {
                if (_hollow)
                    Native.DrawRubberBands(null, _hintRect, bool_0, _tabStripSize);
                else
                    Native.DrawRubberBand(null, _hintRect);
            }
            _hintRect = Rectangle.Empty;
        }

        [Naming]
        private void OnWmPanit()
        {
            if (_dockingHints == DockingHints.RubberBand)
                DrawRubberBandHints();
        }

        public abstract void OnMouseMove(Point position);

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WMConstants.WM_PAINT)
                OnWmPanit();

            if ((m.Msg == WMConstants.WM_KEYFIRST || m.Msg == WMConstants.WM_KEYUP) && m.WParam.ToInt32() == WMConstants.VK_CONTROL)
            {
                OnMouseMove(Cursor.Position);
                return false;
            }
            if ((m.Msg == WMConstants.WM_KEYFIRST || m.Msg == WMConstants.WM_KEYUP) && m.WParam.ToInt32() == WMConstants.VK_SHIFT)
                return true;

            if ((m.Msg == WMConstants.WM_SYSKEYDOWN || m.Msg == WMConstants.WM_SYSKEYUP) && m.WParam.ToInt32() == WMConstants.VK_MENU)
                return true;

            if (m.Msg < WMConstants.WM_KEYFIRST || m.Msg > WMConstants.WM_KEYLAST)
                return false;
            Cancel();
            return true;
        }

        internal static bool WhidbeySupported() => Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(5, 0, 0, 0);

        public event EventHandler Cancalled;

        private bool bool_0;

        private readonly bool _hollow;

        private Control _control;

        private readonly DockingHints _dockingHints;

        private TranslucentForm _translucentForm;

        private Form _form;

        private readonly int _tabStripSize = 21;

        private Rectangle _hintRect = Rectangle.Empty;
    }

    internal class StandardDockingManager : AbstractManager
    {
        public StandardDockingManager(SandDockManager manager, DockContainer container, LayoutSystemBase sourceControlSystem, DockControl sourceControl, int dockedSize, Point startPoint, DockingHints dockingHints) : base(container, dockingHints, true, container.WorkingRenderer.TabStripMetrics.Height)
        {
            Manager = manager;
            Container = container;
            SourceControlSystem = sourceControlSystem;
            SourceControl = sourceControl;
            DockedSize = dockedSize;
            if (container is DocumentContainer)
            {
                cursor_0 = new Cursor(GetType().Assembly.GetManifestResourceStream("TD.SandDock.Resources.splitting.cur"));
                cursor_1 = new Cursor(GetType().Assembly.GetManifestResourceStream("TD.SandDock.Resources.splittingno.cur"));
            }
            if (sourceControlSystem is SplitLayoutSystem)
            {
                size_0 = ((FloatingContainer)container).FloatingSize;
            }
            else if (sourceControl == null)
            {
                size_0 = (sourceControlSystem as ControlLayoutSystem)?.SelectedControl?.FloatingSize ?? sourceControlSystem.Bounds.Size;
            }
            else
            {
                size_0 = sourceControl.FloatingSize;
            }
            var bounds = sourceControlSystem.Bounds;
            if (bounds.Width <= 0)
            {
                startPoint.X = size_0.Width / 2;
            }
            else
            {
                startPoint.X -= bounds.Left;
                startPoint.X = Convert.ToInt32(startPoint.X / (float)bounds.Width * size_0.Width);
            }
            point_0 = sourceControl == null ? new Point(startPoint.X, startPoint.Y - bounds.Top) : new Point(startPoint.X, size_0.Height - (bounds.Bottom - startPoint.Y));
            point_0.Y = Math.Max(point_0.Y, 0);
            point_0.Y = Math.Min(point_0.Y, size_0.Height);
            CheckableControlLayoutSystems = method_10();
            Container.OnDockingStarted(EventArgs.Empty);
        }

        public override void Commit()
        {
            base.Commit();
            LayoutUtilities.Increase();
            try
            {
                Committed?.Invoke(Target);
            }
            finally
            {
                LayoutUtilities.Decrease();
            }
        }

        public override void Dispose()
        {
            Container.OnDockingFinished(EventArgs.Empty);
            cursor_0?.Dispose();
            cursor_1?.Dispose();
            base.Dispose();
        }

        protected virtual DockTarget FindDockTarget(Point position)
        {
            if (Manager != null && AllowFloat)
            {
                foreach (var dockContainer in Manager._containers.Cast<DockContainer>())
                {
                    if (dockContainer.IsFloating && ((FloatingContainer)dockContainer).FloatingForm.Visible &&
                        ((FloatingContainer)dockContainer).HasSingleControlLayoutSystem && dockContainer.LayoutSystem != SourceControlSystem && ((FloatingContainer)dockContainer).FloatingBounds.Contains(position) && !new Rectangle(
                            dockContainer.PointToScreen(dockContainer.LayoutSystem.LayoutSystems[0].Bounds.Location),
                            dockContainer.LayoutSystem.LayoutSystems[0].Bounds.Size).Contains(position))
                    {
                        return new DockTarget(DockTargetType.JoinExistingSystem)
                        {
                            dockContainer = dockContainer,
                            layoutSystem = (ControlLayoutSystem)dockContainer.LayoutSystem.LayoutSystems[0],
                            Bounds = ((FloatingContainer)dockContainer).FloatingBounds
                        };
                    }
                }
            }
            ControlLayoutSystem[] array = CheckableControlLayoutSystems;
            foreach (var controlLayoutSystem in array)
            {
                if (new Rectangle(controlLayoutSystem.DockContainer.PointToScreen(controlLayoutSystem.Bounds.Location), controlLayoutSystem.Bounds.Size).Contains(position))
                {
                    DockTarget dockTarget = method_13(controlLayoutSystem.DockContainer, controlLayoutSystem, position, true);
                    if (dockTarget != null)
                    {
                        DockTarget result = dockTarget;
                        return result;
                    }
                }
            }
            if (Manager == null) return null;

            for (int j = 1; j <= 4; j++)
            {
                ContainerDockLocation containerDockLocation = (ContainerDockLocation)j;
                if (AllowDock(containerDockLocation))
                {
                    if (ToControlBounds(method_7(containerDockLocation, true), Manager.DockSystemContainer).Contains(position))
                    {
                        return new DockTarget(DockTargetType.CreateNewContainer)
                        {
                            DockLocation = containerDockLocation,
                            Bounds = ToControlBounds(method_8(containerDockLocation, DockedSize, true), Manager.DockSystemContainer),
                            middle = true
                        };
                    }
                    if (ToControlBounds(method_7(containerDockLocation, false), Manager.DockSystemContainer).Contains(position))
                    {
                        return new DockTarget(DockTargetType.CreateNewContainer)
                        {
                            DockLocation = containerDockLocation,
                            Bounds = ToControlBounds(method_8(containerDockLocation, DockedSize, false), Manager.DockSystemContainer)
                        };
                    }
                }
            }
            return null;
        }

        private ControlLayoutSystem[] method_10()
        {
            ArrayList arrayList = new ArrayList();
            var array = Manager != null ? Manager.GetDockContainers() : new[] { Container };
            foreach (var dockContainer in array)
            {
                bool isFloating = dockContainer.IsFloating;
                bool flag = dockContainer.Dock == DockStyle.Fill && !dockContainer.IsFloating;
                bool flag2 = Container.Dock == DockStyle.Fill && !Container.IsFloating;
                if ((!isFloating || SourceControlSystem.DockContainer != dockContainer ||
                     !(SourceControlSystem is SplitLayoutSystem)) &&
                    (!isFloating || AllowFloat || SourceControlSystem.DockContainer == dockContainer) &&
                    (isFloating || AllowDock(LayoutUtilities.Convert(dockContainer.Dock))) && (!flag || flag2) &&
                    (!flag2 || Container == dockContainer))
                    method_11(dockContainer, arrayList);
            }
            var array3 = new ControlLayoutSystem[arrayList.Count];
            arrayList.CopyTo(array3, 0);
            return array3;
        }

        private void method_11(DockContainer container, ArrayList arrayList_0)
        {
            if ((container.Width > 0 || container.Height > 0) && container.Enabled && container.Visible)
            {
                method_12(container, container.LayoutSystem, arrayList_0);
            }
        }

        private void method_12(DockContainer dockContainer_1, SplitLayoutSystem splitLayoutSystem_0, ArrayList arrayList_0)
        {
            foreach (LayoutSystemBase layoutSystemBase in splitLayoutSystem_0.LayoutSystems)
            {
                if (!(layoutSystemBase is SplitLayoutSystem))
                {
                    if (layoutSystemBase is ControlLayoutSystem)
                    {
                        if (SourceControl == null || layoutSystemBase != SourceControlSystem)
                        {
                            goto IL_59;
                        }
                        if (SourceControl.LayoutSystem.Controls.Count != 1)
                        {
                            goto IL_59;
                        }
                        bool arg_67_0 = false;
                        IL_67:
                        if (arg_67_0)
                        {
                            arrayList_0.Add(layoutSystemBase);
                        }
                        continue;
                        IL_59:
                        arg_67_0 = !((ControlLayoutSystem)layoutSystemBase).Collapsed;
                        goto IL_67;
                    }
                }
                else
                {
                    method_12(dockContainer_1, (SplitLayoutSystem)layoutSystemBase, arrayList_0);
                }
            }
        }

        protected DockTarget method_13(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Point point_1, bool bool_2)
        {
            DockTarget dockTarget = new DockTarget(DockTargetType.Undefined);
            Point point = dockContainer_1.PointToClient(point_1);
            if (SourceControl != null || controlLayoutSystem_1 != SourceControlSystem)
            {
                if (controlLayoutSystem_1.JoinCatchmentBounds.Contains(point) || controlLayoutSystem_1.TabstripBounds.Contains(point))
                {
                    dockTarget = new DockTarget(DockTargetType.JoinExistingSystem)
                    {
                        dockContainer = dockContainer_1,
                        layoutSystem = controlLayoutSystem_1,
                        DockSide = DockSide.None,
                        Bounds = new Rectangle(dockContainer_1.PointToScreen(controlLayoutSystem_1.Bounds.Location),
                                controlLayoutSystem_1.Bounds.Size)
                    };
                    dockTarget.index = !controlLayoutSystem_1.TabstripBounds.Contains(point) ? controlLayoutSystem_1.Controls.Count : controlLayoutSystem_1.method_15(point);
                }
                if (dockTarget.type == DockTargetType.Undefined && bool_2)
                {
                    dockTarget = method_14(dockContainer_1, controlLayoutSystem_1, point_1);
                }
                return dockTarget;
            }
            return controlLayoutSystem_1.JoinCatchmentBounds.Contains(point) ? new DockTarget(DockTargetType.None) : new DockTarget(DockTargetType.Undefined);
        }

        private DockTarget method_14(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Point point_1)
        {
            DockTarget dockTarget = null;
            Point point = dockContainer_1.PointToClient(point_1);
            var rectangle_ = controlLayoutSystem_1.ClientBounds;
            if (new Rectangle(rectangle_.Left, rectangle_.Top, rectangle_.Width, 30).Contains(point))
            {
                dockTarget = method_21(dockContainer_1, controlLayoutSystem_1);
                if (point.X >= rectangle_.Left + 30)
                {
                    if (point.X > rectangle_.Right - 30)
                    {
                        method_17(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                    }
                    else
                    {
                        method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Top);
                    }
                }
                else
                {
                    method_18(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                }
            }
            else if (new Rectangle(rectangle_.Left, rectangle_.Top, 30, rectangle_.Height).Contains(point))
            {
                dockTarget = method_21(dockContainer_1, controlLayoutSystem_1);
                if (point.Y >= rectangle_.Top + 30)
                {
                    if (point.Y <= rectangle_.Bottom - 30)
                    {
                        method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Left);
                    }
                    else
                    {
                        method_16(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                    }
                }
                else
                {
                    method_18(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                }
            }
            else if (!new Rectangle(rectangle_.Right - 30, rectangle_.Top, 30, rectangle_.Height).Contains(point))
            {
                if (new Rectangle(rectangle_.Left, rectangle_.Bottom - 30, rectangle_.Width, 30).Contains(point))
                {
                    dockTarget = method_21(dockContainer_1, controlLayoutSystem_1);
                    if (point.X >= rectangle_.Left + 30)
                    {
                        if (point.X <= rectangle_.Right - 30)
                        {
                            method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Bottom);
                        }
                        else
                        {
                            method_15(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                        }
                    }
                    else
                    {
                        method_16(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                    }
                }
            }
            else
            {
                dockTarget = method_21(dockContainer_1, controlLayoutSystem_1);
                if (point.Y >= rectangle_.Top + 30)
                {
                    if (point.Y <= rectangle_.Bottom - 30)
                    {
                        method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Right);
                    }
                    else
                    {
                        method_15(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                    }
                }
                else
                {
                    method_17(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                }
            }
            return dockTarget;
        }

        private void method_15(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            rectangle_1.X = rectangle_1.Right - 30;
            rectangle_1.Y = rectangle_1.Bottom - 30;
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, point_1.Y > rectangle_1.Top + (int)(rectangle_1.Height * ((float)point_1.X / rectangle_1.Width))
                ? DockSide.Bottom
                : DockSide.Right);
        }

        private void method_16(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            rectangle_1.Y = rectangle_1.Bottom - 30;
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, point_1.Y >
                                                                            rectangle_1.Bottom -
                                                                            (int)
                                                                            (rectangle_1.Height *
                                                                             (point_1.X / (float)rectangle_1.Width))
                ? DockSide.Bottom : DockSide.Left);
        }

        private void method_17(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            rectangle_1.X = rectangle_1.Right - 30;
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1,
                point_1.Y <=
                rectangle_1.Top + (int)(rectangle_1.Height * ((rectangle_1.Right - point_1.X) / (float)rectangle_1.Width))
                    ? DockSide.Top
                    : DockSide.Right);
        }

        private void method_18(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, point_1.Y <=
                                                                            rectangle_1.Top +
                                                                            (int)
                                                                            (rectangle_1.Height *
                                                                             (point_1.X / (float)rectangle_1.Width))
                ? DockSide.Top
                : DockSide.Left);
        }

        private void method_19(DockContainer container, ControlLayoutSystem layoutSystem, DockTarget dockTarget, DockSide dockSide)
        {
            dockTarget.Bounds = method_20(container, layoutSystem, dockSide);
            dockTarget.DockSide = dockSide;
        }

        internal Rectangle method_20(DockContainer container, ControlLayoutSystem layoutSystem, DockSide dockSide)
        {
            var result = new Rectangle(container.PointToScreen(layoutSystem.Bounds.Location), layoutSystem.Bounds.Size);
            switch (dockSide)
            {
                case DockSide.Top:
                    result.Height /= 2;
                    break;
                case DockSide.Bottom:
                    result.Offset(0, result.Height / 2);
                    result.Height /= 2;
                    break;
                case DockSide.Left:
                    result.Width /= 2;
                    break;
                case DockSide.Right:
                    result.Offset(result.Width / 2, 0);
                    result.Width /= 2;
                    break;
            }
            return result;
        }

        private DockTarget method_21(DockContainer container, ControlLayoutSystem layoutSystem)
        {
            return new DockTarget(DockTargetType.SplitExistingSystem)
            {
                dockContainer = container,
                layoutSystem = layoutSystem
            };
        }

        public bool AllowDock(ContainerDockLocation location)
        {
            return SourceControl?.AllowDock(location) ?? SourceControlSystem.AllowDock(location);
        }

        private Rectangle method_6(Rectangle rect)
        {
            if (rect.X >= Screen.PrimaryScreen.Bounds.X && rect.Right <= Screen.PrimaryScreen.Bounds.Right && rect.Y > Screen.PrimaryScreen.WorkingArea.Bottom)
                rect.Y = Screen.PrimaryScreen.WorkingArea.Bottom - rect.Height;
            var screen = Screen.FromRectangle(rect);
            if (rect.Y < screen.WorkingArea.Y)
                rect.Y = screen.WorkingArea.Y;
            return rect;
        }

        protected Rectangle method_7(ContainerDockLocation location, bool bool_2)
        {
            if (bool_2)
                return method_8(location, 30, true);
            var container = Manager.DockSystemContainer;
            var x = 0;
            var y = 0;
            var w = container.ClientRectangle.Width;
            var h = container.ClientRectangle.Height;
            switch (location)
            {
                case ContainerDockLocation.Left:
                    return new Rectangle(x - 30, y, 30, h - y);
                case ContainerDockLocation.Right:
                    return new Rectangle(w, y, 30, h - y);
                case ContainerDockLocation.Top:
                    return new Rectangle(x, y - 30, w - x, 30);
                case ContainerDockLocation.Bottom:
                    return new Rectangle(x, h, w - x, 30);
                default:
                    return Rectangle.Empty;
            }
        }

        protected Rectangle method_8(ContainerDockLocation location, int int_8, bool bool_2)
        {
            var rectangle = GetDockingBounds(Manager.DockSystemContainer);
            var result = rectangle;
            if (!bool_2)
            {
                result = Manager.DockSystemContainer.ClientRectangle;
            }
            int val = int_8 + 4;
            switch (location)
            {
                case ContainerDockLocation.Left:
                    return new Rectangle(result.Left, result.Top, Math.Min(val, Convert.ToInt32(rectangle.Width * 0.9)), result.Height);
                case ContainerDockLocation.Right:
                    return new Rectangle(result.Right - Math.Min(val, Convert.ToInt32(rectangle.Width * 0.9)), result.Top, Math.Min(val, Convert.ToInt32(rectangle.Width * 0.9)), result.Height);
                case ContainerDockLocation.Top:
                    return new Rectangle(result.Left, result.Top, result.Width, Math.Min(val, Convert.ToInt32(rectangle.Height * 0.9)));
                case ContainerDockLocation.Bottom:
                    return new Rectangle(result.Left, result.Bottom - Math.Min(val, Convert.ToInt32(rectangle.Height * 0.9)), result.Width, Math.Min(val, Convert.ToInt32(rectangle.Height * 0.9)));
            }
            return result;
        }

        protected bool method_9()
        {
            return Manager.FindDockedContainer(DockStyle.Fill) is DocumentContainer;
        }

        public override void OnMouseMove(Point position)
        {
            DockTarget dockTarget = null;
            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                dockTarget = FindDockTarget(position);
            }
            if (dockTarget == null || (dockTarget.type == DockTargetType.Undefined && Manager != null && AllowFloat))
            {
                dockTarget = Manager != null && AllowFloat ? new DockTarget(DockTargetType.Float) : new DockTarget(DockTargetType.None);
            }
            if (dockTarget.type == DockTargetType.Undefined)
            {
                dockTarget.type = DockTargetType.None;
            }
            if (dockTarget.type == DockTargetType.Float)
            {
                dockTarget.Bounds = new Rectangle(DragPoint, size_0);
                dockTarget.Bounds = method_6(dockTarget.Bounds);
            }
            if (dockTarget.layoutSystem == SourceControlSystem && SourceControl != null &&
                dockTarget.DockSide == DockSide.None)
            {
                UpdateHintForm();
                var controlLayoutSystem = (ControlLayoutSystem)SourceControlSystem;
                if (dockTarget.index != controlLayoutSystem.Controls.IndexOf(SourceControl) && dockTarget.index != controlLayoutSystem.Controls.IndexOf(SourceControl) + 1)
                    controlLayoutSystem.Controls.SetChildIndex(SourceControl, dockTarget.index);
                dockTarget.type = DockTargetType.AlreadyActioned;
            }
            else
            {
                if (dockTarget.type == DockTargetType.None)
                    UpdateHintForm();
                else
                    method_1(dockTarget.Bounds, dockTarget.type == DockTargetType.JoinExistingSystem);
            }
            if (Container is DocumentContainer)
            {
                if (dockTarget.type == DockTargetType.AlreadyActioned)
                {
                    Cursor.Current = Cursors.Default;
                }
                else if (dockTarget.type != DockTargetType.None)
                {
                    Cursor.Current = cursor_0;
                }
                else
                {
                    Cursor.Current = cursor_1;
                }
            }
            Target = dockTarget;
        }

        public static Rectangle GetDockingBounds(Control parentControl)
        {
            var x = 0;
            var y = 0;
            var w = parentControl.ClientRectangle.Width;
            var h = parentControl.ClientRectangle.Height;
            foreach (var control in parentControl.Controls.Cast<Control>().Where(c => c.Visible))
            {
                switch (control.Dock)
                {
                    case DockStyle.Top:
                        if (control.Bounds.Bottom > y)
                            y = control.Bounds.Bottom;
                        break;
                    case DockStyle.Bottom:
                        if (control.Bounds.Top < h)
                            h = control.Bounds.Top;
                        break;
                    case DockStyle.Left:
                        if (control.Bounds.Right > x)
                            x = control.Bounds.Right;
                        break;
                    case DockStyle.Right:
                        if (control.Bounds.Left < w)
                            w = control.Bounds.Left;
                        break;
                }
            }
            return new Rectangle(x, y, w - x, h - y);
        }

        public static Rectangle ToControlBounds(Rectangle bounds, Control control)
        {
            return new Rectangle(control.PointToScreen(bounds.Location), bounds.Size);
        }

        [Naming(NamingType.FromOldVersion)]
        public bool DesignMode => Container.FriendDesignMode;

        [Naming(NamingType.FromOldVersion)]
        public bool AllowFloat
        {
            get
            {
                if (DesignMode) return false;
                return SourceControl?.DockingRules.AllowFloat ?? SourceControlSystem.AllowFloat;
            }
        }
        [Naming(NamingType.FromOldVersion)]
        protected ControlLayoutSystem[] CheckableControlLayoutSystems { get; }

        public DockContainer Container { get; }

        public DockControl SourceControl { get; }

        public DockTarget Target { get; private set; }

        public int DockedSize { get; }

        public LayoutSystemBase SourceControlSystem { get; }
        [Naming(NamingType.FromOldVersion)]
        private Point DragPoint => new Point(Cursor.Position.X - point_0.X, Cursor.Position.Y - point_0.Y);

        public SandDockManager Manager { get; }

        public event CommittedEventHandler Committed;

        private Cursor cursor_0;

        private Cursor cursor_1;

        private const int int_6 = 30;

        private Point point_0 = Point.Empty;

        private Size size_0 = Size.Empty;

        public delegate void CommittedEventHandler(DockTarget target);

        public class DockTarget
        {
            public DockTarget(DockTargetType type)
            {
                this.type = type;
            }

            public Rectangle Bounds = Rectangle.Empty;

            public DockContainer dockContainer;

            public ContainerDockLocation DockLocation = ContainerDockLocation.Center;

            public DockSide DockSide = DockSide.None;

            public int index;

            public ControlLayoutSystem layoutSystem;

            public bool middle;

            public DockTargetType type;
        }

        public enum DockTargetType
        {
            Undefined,
            None,
            Float,
            SplitExistingSystem,
            JoinExistingSystem,
            CreateNewContainer,
            AlreadyActioned
        }
    }

    internal class WhidbeyDockingManager : StandardDockingManager
    {
        public WhidbeyDockingManager(SandDockManager manager, DockContainer container, LayoutSystemBase sourceControlSystem, DockControl sourceControl, int dockedSize, Point startPoint, DockingHints dockingHints) : base(manager, container, sourceControlSystem, sourceControl, dockedSize, startPoint, dockingHints)
        {
            _indicators = new ArrayList();
            if (Manager?.DockSystemContainer != null)
                method_22();
        }

        public override void Dispose()
        {
            if (_dockingIndicatorForm != null)
            {
                _dockingIndicatorForm.method_11();
                _dockingIndicatorForm = null;
            }
            foreach (DockingIndicatorForm form in _indicators)
            {
                form.method_11();
            }
            _indicators.Clear();
            base.Dispose();
        }

        protected override DockTarget FindDockTarget(Point position)
        {
            DockTarget target;
            bool flag = rectangle_1.Contains(position);
            bool flag2 = rectangle_2.Contains(position);
            if (flag == bool_2)
            {
                if (flag2 == bool_3)
                {
                    goto IL_C1;
                }
            }
            object[] array = _indicators.ToArray();
            int i = 0;
            while (i < array.Length)
            {
                DockingIndicatorForm form = (DockingIndicatorForm)array[i];
                if (form.DockStyle != DockStyle.Fill)
                {
                    goto IL_7B;
                }
                if (flag2 == bool_3)
                {
                    goto IL_7B;
                }
                if (!flag2)
                {
                    form.method_12();
                }
                else
                {
                    form.UpdateWindow();
                }
                IL_A5:
                i++;
                continue;
                IL_7B:
                if (form.DockStyle == DockStyle.Fill)
                {
                    goto IL_A5;
                }
                if (flag == bool_2)
                {
                    goto IL_A5;
                }
                if (flag)
                {
                    form.UpdateWindow();
                    goto IL_A5;
                }
                form.method_12();
                goto IL_A5;
            }
            bool_2 = flag;
            bool_3 = flag2;
            IL_C1:
            var controlLayoutSystem = method_23(position, out target);
            if (controlLayoutSystem == SourceControlSystem && SourceControl == null)
            {
                controlLayoutSystem = null;
            }
            if (controlLayoutSystem != controlLayoutSystem_1)
            {
                if (_dockingIndicatorForm != null)
                {
                    _dockingIndicatorForm.method_11();
                    _dockingIndicatorForm = null;
                }
                controlLayoutSystem_1 = controlLayoutSystem;
                if (controlLayoutSystem_1 != null)
                {
                    _dockingIndicatorForm = new DockingIndicatorForm(this, controlLayoutSystem_1);
                    _dockingIndicatorForm.UpdateWindow();
                }
            }
            if (target != null && target.type == DockTargetType.Undefined)
            {
                target = null;
            }
            if (_dockingIndicatorForm != null && _dockingIndicatorForm.IndicatorBounds.Contains(position) && target == null)
            {
                target = _dockingIndicatorForm.method_4(position);
            }
            foreach (var form2 in _indicators.Cast<DockingIndicatorForm>())
            {
                if (target == null && form2.IndicatorBounds.Contains(position))
                {
                    target = form2.method_4(position);
                }
            }
            return target;
        }

        private void method_22()
        {
            rectangle_1 = ToControlBounds(Manager.DockSystemContainer.ClientRectangle, Manager.DockSystemContainer);
            rectangle_2 = ToControlBounds(GetDockingBounds(Manager.DockSystemContainer), Manager.DockSystemContainer);
            if (AllowDock(ContainerDockLocation.Top))
            {
                _indicators.Add(new DockingIndicatorForm(this, rectangle_1, DockStyle.Top));
            }
            if (AllowDock(ContainerDockLocation.Left))
            {
                _indicators.Add(new DockingIndicatorForm(this, rectangle_1, DockStyle.Left));
            }
            if (AllowDock(ContainerDockLocation.Bottom))
            {
                _indicators.Add(new DockingIndicatorForm(this, rectangle_1, DockStyle.Bottom));
            }
            if (AllowDock(ContainerDockLocation.Right))
            {
                _indicators.Add(new DockingIndicatorForm(this, rectangle_1, DockStyle.Right));
            }
            bool flag = Container.Dock == DockStyle.Fill && !Container.IsFloating;
            bool flag2 = AllowDock(ContainerDockLocation.Left) || AllowDock(ContainerDockLocation.Right) || AllowDock(ContainerDockLocation.Top) || AllowDock(ContainerDockLocation.Bottom);
            if (!flag && (AllowDock(ContainerDockLocation.Center) || flag2))
                _indicators.Add(new DockingIndicatorForm(this, rectangle_2, DockStyle.Fill));
            if (Manager?.OwnerForm != null)
            {
                foreach (Form ownedForm in _indicators)
                    Manager.OwnerForm.AddOwnedForm(ownedForm);
            }
        }

        private ControlLayoutSystem method_23(Point point, out DockTarget target)
        {
            target = null;
            for (int i = 1; i >= 0; i--)
            {
                foreach (var controlLayoutSystem in CheckableControlLayoutSystems)
                {
                    if (controlLayoutSystem.DockContainer.IsFloating == Convert.ToBoolean(i))
                    {
                        var rectangle = new Rectangle(controlLayoutSystem.DockContainer.PointToScreen(controlLayoutSystem.Bounds.Location), controlLayoutSystem.Bounds.Size);
                        if (rectangle.Contains(point))
                        {
                            target = method_13(controlLayoutSystem.DockContainer, controlLayoutSystem, point, false);
                            return target.type != DockTargetType.Undefined ? null : controlLayoutSystem;
                        }
                    }
                }
            }
            return null;
        }

        private readonly ArrayList _indicators;

        private bool bool_2;

        private bool bool_3;

        private ControlLayoutSystem controlLayoutSystem_1;

        private DockingIndicatorForm _dockingIndicatorForm;

        private Rectangle rectangle_1;

        private Rectangle rectangle_2;

        internal class DockingIndicatorFormBase : Form
        {
            public DockingIndicatorFormBase()
            {
                FormBorderStyle = FormBorderStyle.None;
            }

            private void DrawTransparentBitmapOnLayeredWindowMono(Bitmap bitmap, byte alpha)
            {
                Size = bitmap.Size;
                Visible = true;
              
            }


                
            [Naming]
            public void DrawTransparentBitmapOnLayeredWindow(Bitmap bitmap, byte alpha)
            {
                if (!Native.IsMono())
                {
                    DrawTransparentBitmapOnLayeredWindowMono(bitmap, alpha);
                    return;
                }

                var dc = Native.GetDC(IntPtr.Zero);
                var cdc = Native.CreateCompatibleDC(dc);
                var bm = IntPtr.Zero;
                var hObject = IntPtr.Zero;
                try
                {
                    bm = bitmap.GetHbitmap(Color.FromArgb(0));
                    hObject = Native.SelectObject(cdc, bm);
                    var size = new Native.Size(bitmap.Width, bitmap.Height);
                    var point = new Native.Point(0, 0);
                    var point2 = new Native.Point(Left, Top);
                    var blend = default(Native.BLENDFUNCTION);
                    blend.BlendOp = 0;
                    blend.BlendFlags = 0;
                    blend.SourceConstantAlpha = alpha;
                    blend.AlphaFormat = 1;
                    Native.UpdateLayeredWindow(Handle, dc, ref point2, ref size, cdc, ref point, 0, ref blend, WMConstants.ULW_ALPHA);

                }
                finally
                {
                    if (bm != IntPtr.Zero)
                    {
                        Native.SelectObject(cdc, hObject);
                        Native.DeleteObject(bm);
                    }
                    Native.ReleaseDC(IntPtr.Zero, dc);
                    Native.DeleteDC(cdc);
                }
            }

            protected override bool ShowWithoutActivation => true;

            protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.ExStyle |= WMConstants.WS_SYSMENU | WMConstants.WS_EX_NOACTIVATE | WMConstants.WS_EX_TOPMOST;
                    return cp;
                }
            }
        }

        private class DockingIndicatorForm : DockingIndicatorFormBase
        {
            private DockingIndicatorForm()
            {
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.Manual;
                _indicatorAnimationTimer = new Timer { Interval = 50 };
                _indicatorAnimationTimer.Tick += OnIndicatorAnimationTimerTick;
                _bitmap = new Bitmap(IndicatorWidth, IndicatorHeight, PixelFormat.Format32bppArgb);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                if (!Native.IsMono() && _bitmap!=null)
                {
                    //_bitmap.MakeTransparent();
                    e.Graphics.DrawImage(_bitmap, 0,0,_bitmap.Width,_bitmap.Height);

                }
              
                base.OnPaint(e);
            }
            public DockingIndicatorForm(WhidbeyDockingManager manager, ControlLayoutSystem layoutSystem) : this()
            {
                _manager = manager;
                _layoutSystem = layoutSystem;
                IndicatorBounds = new Rectangle(layoutSystem.DockContainer.PointToScreen(layoutSystem.Bounds.Location), layoutSystem.Bounds.Size);
                IndicatorBounds = new Rectangle(IndicatorBounds.X + IndicatorBounds.Width / 2 - 44, IndicatorBounds.Y + IndicatorBounds.Height / 2 - 44, IndicatorWidth, IndicatorHeight);
                DrawDockingIndicators();
            }

            public DockingIndicatorForm(WhidbeyDockingManager manager, Rectangle fc, DockStyle dockStyle) : this()
            {
                _manager = manager;
                DockStyle = dockStyle;
                switch (dockStyle)
                {
                    case DockStyle.Top:
                        IndicatorBounds = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Y + 15, 88, 88);
                        break;
                    case DockStyle.Bottom:
                        IndicatorBounds = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Bottom - 88 - 15, 88, 88);
                        break;
                    case DockStyle.Left:
                        IndicatorBounds = new Rectangle(fc.X + 15, fc.Y + fc.Height / 2 - 44, 88, 88);
                        break;
                    case DockStyle.Right:
                        IndicatorBounds = new Rectangle(fc.Right - 88 - 15, fc.Y + fc.Height / 2 - 44, 88, 88);
                        break;
                    case DockStyle.Fill:
                        IndicatorBounds = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Y + fc.Height / 2 - 44, 88, 88);
                        break;
                }
                DrawDockingIndicators();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _bitmap.Dispose();
                    _indicatorAnimationTimer.Tick -= OnIndicatorAnimationTimerTick;
                    _indicatorAnimationTimer.Dispose();
                }
                base.Dispose(disposing);
            }

            private void DrawDockingIndicators()
            {
                using (var g = Graphics.FromImage(_bitmap))
                {
                    g.Clear(Color.Transparent);
                    switch (DockStyle)
                    {
                        case DockStyle.None:
                        case DockStyle.Fill:
                            using (
                                var image = Image.FromStream(typeof(DockingIndicatorForm).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintcenter.png")))
                                g.DrawImageUnscaled(image, 0, 0);
                            break;
                        case DockStyle.Top:
                            using (
                                var image =
                                    Image.FromStream(
                                        typeof(DockingIndicatorForm).Assembly.GetManifestResourceStream(
                                            "TD.SandDock.Resources.dockinghinttop.png")))
                                g.DrawImageUnscaled(image, 29, 0);
                            break;
                        case DockStyle.Bottom:
                            using (
                                var image =
                                    Image.FromStream(
                                        typeof(DockingIndicatorForm).Assembly.GetManifestResourceStream(
                                            "TD.SandDock.Resources.dockinghintbottom.png")))
                                g.DrawImageUnscaled(image, 29, 57);
                            break;
                        case DockStyle.Left:
                            using (
                                var image =
                                    Image.FromStream(
                                        typeof(DockingIndicatorForm).Assembly.GetManifestResourceStream(
                                            "TD.SandDock.Resources.dockinghintleft.png")))
                                g.DrawImageUnscaled(image, 0, 29);
                            break;
                        case DockStyle.Right:
                            using (
                                var image =
                                    Image.FromStream(
                                        typeof(DockingIndicatorForm).Assembly.GetManifestResourceStream(
                                            "TD.SandDock.Resources.dockinghintright.png")))
                                g.DrawImageUnscaled(image, 57, 29);
                            break;
                    }

                    var highlight = SystemColors.Highlight;
                    var transparent = Color.Transparent;

                    if (DockStyle == DockStyle.None || DockStyle == DockStyle.Fill || DockStyle == DockStyle.Top)
                    {
                        DrawTopHighlightedOutline(g, bool_0 && _dockSide == DockSide.Top ? highlight : transparent);
                    }
                    if (DockStyle == DockStyle.None || DockStyle == DockStyle.Fill || DockStyle == DockStyle.Right)
                    {
                        DrawRightHighlightedOutline(g, bool_0 && _dockSide == DockSide.Right ? highlight : transparent);
                    }
                    if (DockStyle == DockStyle.None || DockStyle == DockStyle.Fill || DockStyle == DockStyle.Bottom)
                    {
                        DrawBottomTopHighlightedOutline(g, bool_0 && _dockSide == DockSide.Bottom ? highlight : transparent);
                    }
                    if (DockStyle == DockStyle.None || DockStyle == DockStyle.Fill || DockStyle == DockStyle.Left)
                    {
                        DrawLeftHighlightedOutline(g, bool_0 && _dockSide == DockSide.Left ? highlight : transparent);
                    }
                    if (DockStyle == DockStyle.None || DockStyle == DockStyle.Fill)
                    {
                        DrawFillHighlightedOutline(g, bool_0 && _dockSide == DockSide.None ? highlight : transparent);
                    }
                }
                DrawTransparentBitmapOnLayeredWindow(_bitmap, 255);
            }

            private void DrawTopHighlightedOutline(Graphics g, Color c)
            {
                using (var pen = new Pen(c))
                {
                    g.DrawLine(pen, 29, 0, 57, 0);
                    g.DrawLine(pen, 57, 0, 57, 23);
                    g.DrawLine(pen, 29, 0, 29, 23);
                }
            }

            public void method_11()
            {
                bool_2 = true;
                method_12();
            }

            public void method_12()
            {
                if (Visible || (!bool_1 && _indicatorAnimationTimer.Enabled))
                {
                    int_5 = Environment.TickCount;
                    bool_1 = true;
                    _indicatorAnimationTimer.Start();
                }
            }

            public void UpdateWindow()
            {
                DrawTransparentBitmapOnLayeredWindow(_bitmap, 0);
                SetIndicatorPosition();
                int_5 = Environment.TickCount;
                bool_1 = false;
                _indicatorAnimationTimer.Start();
            }

            protected override bool ShowWithoutActivation => true;

            private void SetIndicatorPosition()
            {
                //Native.SetWindowPos(Handle, WMConstants.HWND_TOPMOST, IndicatorBounds.X, IndicatorBounds.Y, IndicatorBounds.Width, IndicatorBounds.Height, WMConstants.SWP_SHOWWINDOW + WMConstants.SWP_NOACTIVATE);
                Location = IndicatorBounds.Location;
                Size = IndicatorBounds.Size;
                Visible = true;
            }

            private DockTarget method_2(Point point)
            {
                var dockTarget = new DockTarget(DockTargetType.SplitExistingSystem)
                {
                    layoutSystem = _layoutSystem,
                    dockContainer = _layoutSystem.DockContainer
                };
                if (IsInRectangle(Rectangle_1, point))
                {
                    dockTarget.DockSide = DockSide.Top;
                }
                else if (!IsInRectangle(Rectangle_2, point))
                {
                    if (!IsInRectangle(Rectangle_3, point))
                    {
                        if (!IsInRectangle(Rectangle_4, point))
                        {
                            if (!IsInRectangle(Rectangle_0, point))
                            {
                                dockTarget.type = DockTargetType.Undefined;
                            }
                            else
                            {
                                dockTarget.type = DockTargetType.JoinExistingSystem;
                                dockTarget.DockSide = DockSide.None;
                            }
                        }
                        else
                        {
                            dockTarget.DockSide = DockSide.Left;
                        }
                    }
                    else
                    {
                        dockTarget.DockSide = DockSide.Bottom;
                    }
                }
                else
                {
                    dockTarget.DockSide = DockSide.Right;
                }
                dockTarget.Bounds = _manager.method_20(_layoutSystem.DockContainer, _layoutSystem, dockTarget.DockSide);
                return dockTarget;
            }

            private DockTarget method_3(Point point)
            {
                var dockTarget = new DockTarget(DockTargetType.SplitExistingSystem)
                {
                    layoutSystem = _layoutSystem,
                    dockContainer = _layoutSystem?.DockContainer
                };
                if (IsInRectangle(Rectangle_1, point) && _manager.AllowDock(ContainerDockLocation.Top))
                {
                    if (DockStyle != DockStyle.Top)
                    {
                        if (DockStyle != DockStyle.Fill)
                        {
                            goto IL_75;
                        }
                    }
                    dockTarget.DockLocation = ContainerDockLocation.Top;
                    dockTarget.DockSide = DockSide.Top;
                    goto IL_178;
                }
                IL_75:
                if (IsInRectangle(Rectangle_2, point) && _manager.AllowDock(ContainerDockLocation.Right))
                {
                    if (DockStyle != DockStyle.Right)
                    {
                        if (DockStyle != DockStyle.Fill)
                        {
                            goto IL_B9;
                        }
                    }
                    dockTarget.DockLocation = ContainerDockLocation.Right;
                    dockTarget.DockSide = DockSide.Right;
                    goto IL_178;
                }
                IL_B9:
                if (IsInRectangle(Rectangle_3, point) && _manager.AllowDock(ContainerDockLocation.Bottom))
                {
                    if (DockStyle == DockStyle.Bottom || DockStyle == DockStyle.Fill)
                    {
                        dockTarget.DockLocation = ContainerDockLocation.Bottom;
                        dockTarget.DockSide = DockSide.Bottom;
                        goto IL_178;
                    }
                }
                if (!IsInRectangle(Rectangle_4, point) || !_manager.AllowDock(ContainerDockLocation.Left) || (DockStyle != DockStyle.Left && DockStyle != DockStyle.Fill))
                {
                    if (IsInRectangle(Rectangle_0, point) && _manager.AllowDock(ContainerDockLocation.Center))
                    {
                        if (DockStyle == DockStyle.Fill)
                        {
                            dockTarget.DockLocation = ContainerDockLocation.Center;
                            dockTarget.DockSide = DockSide.None;
                            goto IL_178;
                        }
                    }
                    dockTarget.type = DockTargetType.Undefined;
                }
                else
                {
                    dockTarget.DockLocation = ContainerDockLocation.Left;
                    dockTarget.DockSide = DockSide.Left;
                }
                IL_178:
                if (dockTarget.type != DockTargetType.Undefined)
                {
                    dockTarget.type = DockTargetType.CreateNewContainer;
                    dockTarget.middle = DockStyle == DockStyle.Fill;
                    dockTarget.Bounds = ToControlBounds(_manager.method_8(dockTarget.DockLocation, _manager.DockedSize, dockTarget.middle), _manager.Manager.DockSystemContainer);
                }
                return dockTarget;
            }

            public DockTarget method_4(Point point)
            {
                var p = PointToClient(point);
                var dockTarget = _layoutSystem != null ? method_2(p) : method_3(p);
                bool flag = dockTarget.type != DockTargetType.Undefined;
                var dockSide = dockTarget.type != DockTargetType.Undefined ? dockTarget.DockSide : _dockSide;
                if (flag != bool_0 || dockSide != _dockSide)
                {
                    bool_0 = flag;
                    _dockSide = dockSide;
                    DrawDockingIndicators();
                }
                return dockTarget;
            }

            private bool IsInRectangle(Rectangle r, Point p)
            {
                return r.Contains(p);
            }

            private void DrawFillHighlightedOutline(Graphics g, Color c)
            {
                using (var pen = new Pen(c))
                {
                    g.DrawLine(pen, 22, 29, 29, 22);
                    g.DrawLine(pen, 57, 22, 64, 29);
                    g.DrawLine(pen, 64, 57, 57, 64);
                    g.DrawLine(pen, 29, 64, 22, 57);
                }
            }

            private void DrawLeftHighlightedOutline(Graphics g, Color c)
            {
                using (var pen = new Pen(c))
                {
                    g.DrawLine(pen, 0, 29, 0, 57);
                    g.DrawLine(pen, 0, 57, 23, 57);
                    g.DrawLine(pen, 0, 29, 23, 29);
                }
            }

            private void DrawBottomTopHighlightedOutline(Graphics g, Color c)
            {
                using (var pen = new Pen(c))
                {
                    g.DrawLine(pen, 29, 87, 57, 87);
                    g.DrawLine(pen, 29, 87, 29, 64);
                    g.DrawLine(pen, 57, 87, 57, 64);
                }
            }

            private void DrawRightHighlightedOutline(Graphics g, Color c)
            {
                using (var pen = new Pen(c))
                {
                    g.DrawLine(pen, 87, 29, 87, 57);
                    g.DrawLine(pen, 87, 29, 64, 29);
                    g.DrawLine(pen, 87, 57, 64, 57);
                }
            }

            private void OnIndicatorAnimationTimerTick(object sender, EventArgs e)
            {
                int num = Environment.TickCount - int_5;
                if (num > 200)
                {
                    num = 200;
                }
                var alpha = num / 200.0;
                if (bool_1)
                {
                    alpha = (1.0 - alpha) * 255.0;
                }
                else
                {
                    alpha *= 255.0;
                }
                DrawTransparentBitmapOnLayeredWindow(_bitmap, (byte)alpha);
                if (num >= 200)
                {
                    _indicatorAnimationTimer.Stop();
                    Visible = !bool_1;
                    if (bool_2)
                    {
                        Dispose();
                    }
                }
            }

            public DockStyle DockStyle { get; }

            private Rectangle Rectangle_0 => new Rectangle(28, 28, 32, 32);

            private Rectangle Rectangle_1 => new Rectangle(29, 0, 29, 28);

            private Rectangle Rectangle_2 => new Rectangle(60, 29, 28, 29);

            private Rectangle Rectangle_3 => new Rectangle(29, 60, 29, 28);

            private Rectangle Rectangle_4 => new Rectangle(0, 29, 28, 29);

            public Rectangle IndicatorBounds { get; } = Rectangle.Empty;

            private readonly Bitmap _bitmap;

            private bool bool_0;

            private bool bool_1;

            private bool bool_2;

            private readonly WhidbeyDockingManager _manager;

            private readonly ControlLayoutSystem _layoutSystem;

            private DockSide _dockSide = DockSide.None;

            private const int IndicatorWidth = 88;

            private const int IndicatorHeight = 88;

            private const int int_2 = 200;

            private const int int_3 = 16;

            private const int int_4 = 64;

            private int int_5;

            private readonly Timer _indicatorAnimationTimer;
        }
    }

    internal class ResizingManager : AbstractManager
    {
        public ResizingManager(AutoHideBar bar, PopupContainer popupContainer, Point startPoint) : base(bar, bar.Manager?.DockingHints ?? DockingHints.TranslucentFill, false)
        {
            _autoHideBar = bar;
            _popupContainer = popupContainer;
            _startPoint = startPoint;
            int num = bar.Manager?.MinimumDockContainerSize ?? 30;
            int num2 = bar.Manager?.MaximumDockContainerSize ?? 500;
            int_6 = popupContainer.PopupSize;
            switch (bar.Dock)
            {
                case DockStyle.Top:
                    if (bar.Manager?.DockSystemContainer != null)
                    {
                        num2 = Math.Max(bar.Manager.DockSystemContainer.Height - popupContainer.Bounds.Top - num, num);
                    }
                    int_7 = startPoint.Y - (int_6 - num);
                    int_8 = startPoint.Y + (num2 - int_6);
                    break;
                case DockStyle.Bottom:
                    if (bar.Manager?.DockSystemContainer != null)
                    {
                        num2 = Math.Max(popupContainer.Bounds.Bottom - num, num);
                    }
                    int_7 = startPoint.Y - (num2 - int_6);
                    int_8 = startPoint.Y + (int_6 - num);
                    break;
                case DockStyle.Left:
                    if (bar.Manager?.DockSystemContainer != null)
                    {
                        num2 = Math.Max(bar.Manager.DockSystemContainer.Width - popupContainer.Bounds.Left - num, num);
                    }
                    int_7 = startPoint.X - (int_6 - num);
                    int_8 = startPoint.X + (num2 - int_6);
                    break;
                case DockStyle.Right:
                    if (bar.Manager?.DockSystemContainer != null)
                    {
                        num2 = Math.Max(popupContainer.Bounds.Right - num, num);
                    }
                    int_7 = startPoint.X - (num2 - int_6);
                    int_8 = startPoint.X + (int_6 - num);
                    break;
            }
            OnMouseMove(startPoint);
        }

        public override void Commit()
        {
            base.Commit();
            Committed?.Invoke(int_9);
        }

        public override void OnMouseMove(Point position)
        {
            var empty = Rectangle.Empty;
            if (!_autoHideBar.Vertical)
            {
                if (position.Y < int_7)
                    position.Y = int_7;
                if (position.Y > int_8)
                    position.Y = int_8;
                empty = new Rectangle(0, position.Y - 2, _popupContainer.Width, 4);
            }
            else
            {
                if (position.X < int_7)
                    position.X = int_7;
                if (position.X > int_8)
                    position.X = int_8;
                empty = new Rectangle(position.X - 2, 0, 4, _popupContainer.Height);
            }
            switch (_autoHideBar.Dock)
            {
                case DockStyle.Top:
                    int_9 = int_6 + (position.Y - _startPoint.Y);
                    break;
                case DockStyle.Bottom:
                    int_9 = int_6 + (_startPoint.Y - position.Y);
                    break;
                case DockStyle.Left:
                    int_9 = int_6 + (position.X - _startPoint.X);
                    break;
                case DockStyle.Right:
                    int_9 = int_6 + (_startPoint.X - position.X);
                    break;
            }
            method_1(new Rectangle(_popupContainer.PointToScreen(empty.Location), empty.Size), false);
        }

        [Naming(NamingType.FromOldVersion)]
        public event CommittedEventHandler Committed;

        private readonly AutoHideBar _autoHideBar;

        private readonly PopupContainer _popupContainer;

        private int int_6;

        private int int_7;

        private int int_8;

        private int int_9;

        private Point _startPoint;

        [Naming(NamingType.FromOldVersion)]
        public delegate void CommittedEventHandler(int newSize);
    }

    internal class SplittingManager : AbstractManager
    {
        public SplittingManager(DockContainer container, SplitLayoutSystem splitLayoutSystem, LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, Point startPoint, DockingHints dockingHints) : base(container, dockingHints, false)
        {
            _container = container;
            SplitLayoutSystem = splitLayoutSystem;
            _aboveLayout = aboveLayout;
            _belowLayout = belowLayout;
            _startPoint = startPoint;
            if (splitLayoutSystem.SplitMode != Orientation.Horizontal)
            {
                int_7 = aboveLayout.Bounds.X + 25;
                int_8 = belowLayout.Bounds.Right - 25;
                float_2 = aboveLayout.WorkingSize.Width + belowLayout.WorkingSize.Width;
            }
            else
            {
                int_7 = aboveLayout.Bounds.Y + 25;
                int_8 = belowLayout.Bounds.Bottom - 25;
                float_2 = aboveLayout.WorkingSize.Height + belowLayout.WorkingSize.Height;
            }
            OnMouseMove(startPoint);
        }

        public override void Commit()
        {
            base.Commit();
            SplittingManagerFinished?.Invoke(_aboveLayout, _belowLayout, _aboveSize, _belowSize);
        }

        public override void OnMouseMove(Point position)
        {
            Rectangle empty = Rectangle.Empty;
            if (SplitLayoutSystem.SplitMode != Orientation.Horizontal)
            {
                empty = new Rectangle(position.X - 2, SplitLayoutSystem.Bounds.Y, 4, SplitLayoutSystem.Bounds.Height);
                empty.X = Math.Max(empty.X, int_7);
                empty.X = Math.Min(empty.X, int_8 - 4);
                float num = _belowLayout.Bounds.Right - _aboveLayout.Bounds.Left - 4;
                _aboveSize = (empty.X - _aboveLayout.Bounds.Left) / num * float_2;
                _belowSize = float_2 - _aboveSize;
            }
            else
            {
                empty = new Rectangle(SplitLayoutSystem.Bounds.X, position.Y - 2, SplitLayoutSystem.Bounds.Width, 4);
                empty.Y = Math.Max(empty.Y, int_7);
                empty.Y = Math.Min(empty.Y, int_8 - 4);
                float num2 = _belowLayout.Bounds.Bottom - _aboveLayout.Bounds.Top - 4;
                _aboveSize = (empty.Y - _aboveLayout.Bounds.Top) / num2 * float_2;
                _belowSize = float_2 - _aboveSize;
            }
            method_1(new Rectangle(_container.PointToScreen(empty.Location), empty.Size), false);
            Cursor.Current = SplitLayoutSystem.SplitMode != Orientation.Horizontal ? Cursors.VSplit : Cursors.HSplit;
        }

        public SplitLayoutSystem SplitLayoutSystem { get; }

        public event SplittingManagerFinishedEventHandler SplittingManagerFinished;

        private readonly DockContainer _container;

        private float _aboveSize;

        private float _belowSize;

        private float float_2;

        private const int int_6 = 25;

        private int int_7;

        private int int_8;

        private readonly LayoutSystemBase _aboveLayout;

        private readonly LayoutSystemBase _belowLayout;

        private Point _startPoint = Point.Empty;

        public delegate void SplittingManagerFinishedEventHandler(LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, float aboveSize, float belowSize);
    }

    internal class Class11 : AbstractManager
    {
        public Class11(SandDockManager manager, DockContainer container, Point startPoint) : base(container, manager.DockingHints, false)
        {
            _container = container;
            Rectangle rectangle = Rectangle.Empty;
            rectangle = StandardDockingManager.ToControlBounds(StandardDockingManager.GetDockingBounds(container.Parent), container.Parent);
            rectangle = new Rectangle(container.PointToClient(rectangle.Location), rectangle.Size);
            int minContainerSize = manager?.MinimumDockContainerSize ?? 30;
            minContainerSize = Math.Max(minContainerSize, LayoutUtilities.GetDockedSize(container));
            int maxContainerSize = manager?.MaximumDockContainerSize ?? 500;
            int currentContainerSize = container.CurrentSize;
            switch (container.Dock)
            {
                case DockStyle.Top:
                    int_6 = startPoint.Y - (currentContainerSize - minContainerSize);
                    int_7 = Math.Min(rectangle.Bottom - 20, startPoint.Y + (maxContainerSize - currentContainerSize));
                    int_9 = startPoint.Y - container.Rectangle_0.Y;
                    break;
                case DockStyle.Bottom:
                    int_6 = Math.Max(rectangle.Top + 20, startPoint.Y - (maxContainerSize - currentContainerSize));
                    int_7 = startPoint.Y + (currentContainerSize - minContainerSize);
                    int_9 = startPoint.Y - container.Rectangle_0.Y;
                    break;
                case DockStyle.Left:
                    int_6 = startPoint.X - (currentContainerSize - minContainerSize);
                    int_7 = Math.Min(rectangle.Right - 20, startPoint.X + (maxContainerSize - currentContainerSize));
                    int_9 = startPoint.X - container.Rectangle_0.X;
                    break;
                case DockStyle.Right:
                    int_6 = Math.Max(rectangle.Left + 20, startPoint.X - (maxContainerSize - currentContainerSize));
                    int_7 = startPoint.X + (currentContainerSize - minContainerSize);
                    int_9 = startPoint.X - container.Rectangle_0.X;
                    break;
            }
            OnMouseMove(startPoint);
        }

        public override void Commit()
        {
            base.Commit();
            Committed?.Invoke(int_8);
        }

        public override void OnMouseMove(Point position)
        {
            var empty = Rectangle.Empty;
            if (_container.Vertical)
            {
                empty = new Rectangle(position.X - int_9, 0, 4, _container.Height);
                if (empty.X < int_6)
                {
                    empty.X = int_6;
                }
                if (empty.X > int_7 - 4)
                {
                    empty.X = int_7 - 4;
                }
            }
            else
            {
                empty = new Rectangle(0, position.Y - int_9, _container.Width, 4);
                if (empty.Y < int_6)
                {
                    empty.Y = int_6;
                }
                if (empty.Y > int_7 - 4)
                {
                    empty.Y = int_7 - 4;
                }
            }
            switch (_container.Dock)
            {
                case DockStyle.Top:
                    int_8 = _container.ContentSize + (empty.Y - _container.Rectangle_0.Y);
                    break;
                case DockStyle.Bottom:
                    int_8 = _container.ContentSize + (_container.Rectangle_0.Y - empty.Y);
                    break;
                case DockStyle.Left:
                    int_8 = _container.ContentSize + (empty.X - _container.Rectangle_0.X);
                    break;
                case DockStyle.Right:
                    int_8 = _container.ContentSize + (_container.Rectangle_0.X - empty.X);
                    break;
            }
            method_1(new Rectangle(_container.PointToScreen(empty.Location), empty.Size), false);
            Cursor.Current = _container.Dock != DockStyle.Left && _container.Dock != DockStyle.Right ? Cursors.HSplit : Cursors.VSplit;
        }

        [Naming(NamingType.FromOldVersion)]
        public event CommittedEventHandler Committed;

        private readonly DockContainer _container;

        private int int_6;

        private int int_7;

        private int int_8;

        private int int_9;

        [Naming(NamingType.FromOldVersion)]
        public delegate void CommittedEventHandler(int newSize);
    }

    public abstract class LayoutSystemBase
    {
        internal LayoutSystemBase()
        {
        }

        protected internal virtual void Layout(RendererBase renderer, Graphics graphics, Rectangle bounds, bool floating)
        {
            Bounds = bounds;
        }

        internal void StartDockingSession(SandDockManager manager, DockContainer container, LayoutSystemBase layoutSystem, DockControl control, int dockedSize, Point startPoint, DockingHints dockingHints, DockingManager dockingManager)
        {
            if (dockingManager == DockingManager.Whidbey && AbstractManager.WhidbeySupported())
                _dockingManager = new WhidbeyDockingManager(manager, DockContainer, this, control, dockedSize, startPoint, dockingHints);
            else
                _dockingManager = new StandardDockingManager(manager, DockContainer, this, control, dockedSize, startPoint, dockingHints);
            _dockingManager.Committed += OnCommitted;
            _dockingManager.Cancalled += OnCancalled;
            _dockingManager.OnMouseMove(Cursor.Position);
        }

        private void FinishDockingSession()
        {
            _dockingManager.Committed -= OnCommitted;
            _dockingManager.Cancalled -= OnCancalled;
            _dockingManager = null;
        }

        internal void method_2(SandDockManager manager, ContainerDockLocation dockLocation, ContainerDockEdge edge)
        {
            var contentSize = AllControls.Length > 0 ? AllControls[0].MetaData.DockedContentSize : 0;
            var rectangle = StandardDockingManager.GetDockingBounds(manager.DockSystemContainer);
            if (dockLocation != ContainerDockLocation.Left && dockLocation != ContainerDockLocation.Right)
            {
                if (dockLocation == ContainerDockLocation.Top || dockLocation == ContainerDockLocation.Bottom)
                {
                    contentSize = Math.Min(contentSize, Convert.ToInt32(rectangle.Height * 0.9));
                }
            }
            else
                contentSize = Math.Min(contentSize, Convert.ToInt32(rectangle.Width * 0.9));
            if (this is ControlLayoutSystem)
                LayoutUtilities.smethod_10((ControlLayoutSystem) this);
            else
                Parent?.LayoutSystems.Remove(this);
            var dockContainer = manager.CreateNewDockContainer(dockLocation, edge, contentSize);
            if (dockContainer is DocumentContainer)
            {
                var layoutSystem = dockContainer.CreateNewLayoutSystem(WorkingSize);
                dockContainer.LayoutSystem.LayoutSystems.Add(layoutSystem);
                if (this is SplitLayoutSystem)
                    ((SplitLayoutSystem) this).MoveToLayoutSystem(layoutSystem);
                else
                    layoutSystem.Controls.AddRange(AllControls);
            }
            else
            {
                dockContainer.LayoutSystem.LayoutSystems.Add(this);
            }
        }

        private void EnsureSandDockManagerExists()
        {
            if (Manager == null)
                throw new InvalidOperationException("No SandDockManager is associated with this ControlLayoutSystem.");
        }

        protected internal virtual void OnDragOver(DragEventArgs drgevent)
        {
        }

        protected internal virtual void OnMouseDoubleClick()
        {
        }

        protected internal virtual void OnMouseDown(MouseEventArgs e)
        {
        }

        protected internal virtual void OnMouseLeave()
        {
        }

        protected internal virtual void OnMouseMove(MouseEventArgs e)
        {
        }

        protected internal virtual void OnMouseUp(MouseEventArgs e)
        {
        }

        internal virtual void OnCommitted(StandardDockingManager.DockTarget target)
        {
            FinishDockingSession();
        }

        internal virtual void OnCancalled(object sender, EventArgs e)
        {
            FinishDockingSession();
        }

        [Naming]
        internal virtual void SetDockContainer(DockContainer container)
        {
            DockContainer = container;
        }

        internal abstract bool AllowDock(ContainerDockLocation location);

        internal abstract void DrawDocumentStrip(RendererBase renderer, Graphics g, Font font);

        [Naming(NamingType.FromOldVersion)]
        internal abstract bool ContainsPersistableDockControls { get; }

        internal abstract bool AllowFloat { get; }

        internal abstract bool AllowTab { get; }

        public Rectangle Bounds { get; private set; } = Rectangle.Empty;

        public DockContainer DockContainer { get; private set; }

        internal abstract DockControl[] AllControls { get; }

        public bool IsInContainer => DockContainer != null;

        public SplitLayoutSystem Parent => _parent;

        private SandDockManager Manager => DockContainer?.Manager;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public SizeF WorkingSize
        {
            get
            {
                return _workingSize;
            }
            set
            {
                if (value.Width <= 0f || value.Height <= 0f) throw new ArgumentException("value");
                _workingSize = value;
            }
        }

        internal StandardDockingManager _dockingManager;

        private const int DefaultWidth = 250;

        private const int DefaultHeight = 400;

        private SizeF _workingSize = new SizeF(DefaultWidth, DefaultHeight);

        internal SplitLayoutSystem _parent;
    }
}
