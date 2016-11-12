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

        public void method_0(Rectangle rect, bool bool_1)
        {
            Native.SetWindowPos(Handle, IntPtr.Zero, rect.X, rect.Y, rect.Width, rect.Height, 80);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Native.SetLayeredWindowAttributes(Handle, 0, Alpha, LWA_ALPHA);
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
                var createParams = base.CreateParams;
                createParams.Style = WS_OVERLAPPED; //- 2147483648;
                createParams.ExStyle |= WS_SYSMENU;
                return createParams;
            }
        }

        private readonly bool _hollow;

        private const int LWA_ALPHA = 2;

        private const int WS_SYSMENU = 0x00080000; //524288 
        private const int WS_OVERLAPPED = 0;
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
            bool flag = false; 
#else
            bool flag = OSFeature.Feature.IsPresent(OSFeature.LayeredWindows);
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
            Event_0?.Invoke(this, EventArgs.Empty);
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

        protected void method_1(Rectangle bounds, bool bool_2)
        {
            if (_hintRect == bounds) return;
            if (_dockingHints == DockingHints.RubberBand)
                DrawRubberBandHints();

            if (_dockingHints == DockingHints.RubberBand)
            {
                if (_hollow)
                    Native.DrawIndicators(null, bounds, bool_2, _tabStripSize);
                else
                    Native.DrawIndicator(null, bounds);
                _hintRect = bounds;
                this.bool_0 = bool_2;
                return;
            }
            _translucentForm.method_0(bounds, bool_2);
        }

        [GuessedName]
        protected void UpdateHintForm()
        {
            if (_dockingHints == DockingHints.RubberBand)
                DrawRubberBandHints();
            else
                _translucentForm.Hide();
        }

        [GuessedName]
        private void DrawRubberBandHints()
        {
            if (_hintRect != Rectangle.Empty)
            {
                if (_hollow)
                    Native.DrawIndicators(null, _hintRect, this.bool_0, _tabStripSize);
                else
                    Native.DrawIndicator(null, _hintRect);
            }
            _hintRect = Rectangle.Empty;
        }

        [GuessedName]
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

        internal static bool smethod_0() => Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(5, 0, 0, 0);

        public event EventHandler Event_0;

        private bool bool_0;

        private readonly bool _hollow;

        private Control _control;

        private readonly DockingHints _dockingHints;

        private TranslucentForm _translucentForm;

        private Form _form;

        private readonly int _tabStripSize = 21;

        private Rectangle _hintRect = Rectangle.Empty;
    }

    internal class Class7 : AbstractManager
    {
        public Class7(SandDockManager manager, DockContainer container, LayoutSystemBase sourceControlSystem, DockControl sourceControl, int dockedSize, Point startPoint, DockingHints dockingHints) : base(container, dockingHints, true, container.Renderer.TabStripMetrics.Height)
        {
            Manager = manager;
            Container = container;
            SourceControlSystem = sourceControlSystem;
            SourceControl = sourceControl;
            DockedSize = dockedSize;
            if (container is DocumentContainer)
            {
                this.cursor_0 = new Cursor(GetType().Assembly.GetManifestResourceStream("TD.SandDock.Resources.splitting.cur"));
                this.cursor_1 = new Cursor(GetType().Assembly.GetManifestResourceStream("TD.SandDock.Resources.splittingno.cur"));
            }
            if (sourceControlSystem is SplitLayoutSystem)
            {
                this.size_0 = ((FloatingContainer)container).FloatingSize;
            }
            else if (sourceControl == null)
            {
                this.size_0 = (sourceControlSystem as ControlLayoutSystem)?.SelectedControl?.FloatingSize ?? sourceControlSystem.Bounds.Size;
            }
            else
            {
                this.size_0 = sourceControl.FloatingSize;
            }
            var bounds = sourceControlSystem.Bounds;
            if (bounds.Width <= 0)
            {
                startPoint.X = this.size_0.Width / 2;
            }
            else
            {
                startPoint.X -= bounds.Left;
                startPoint.X = Convert.ToInt32((float)startPoint.X / (float)bounds.Width * (float)this.size_0.Width);
            }
            this.point_0 = sourceControl == null ? new Point(startPoint.X, startPoint.Y - bounds.Top) : new Point(startPoint.X, this.size_0.Height - (bounds.Bottom - startPoint.Y));
            this.point_0.Y = Math.Max(this.point_0.Y, 0);
            this.point_0.Y = Math.Min(this.point_0.Y, this.size_0.Height);
            this.ControlLayoutSystem_0 = this.method_10();
            Container.OnDockingStarted(EventArgs.Empty);
        }

        public override void Commit()
        {
            base.Commit();
            LayoutUtilities.Increase();
            try
            {
                DockingManagerFinished?.Invoke(this.Target);
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
                    if (dockContainer.IsFloating && ((FloatingContainer) dockContainer).UnderlyingForm.Visible &&
                        ((FloatingContainer) dockContainer).HasSingleControlLayoutSystem && dockContainer.LayoutSystem != SourceControlSystem && ((FloatingContainer) dockContainer).FloatingBounds.Contains(position) && !new Rectangle(
                            dockContainer.PointToScreen(dockContainer.LayoutSystem.LayoutSystems[0].Bounds.Location),
                            dockContainer.LayoutSystem.LayoutSystems[0].Bounds.Size).Contains(position))
                    {
                        return new DockTarget(DockTargetType.JoinExistingSystem)
                        {
                            dockContainer = dockContainer,
                            layoutSystem = (ControlLayoutSystem) dockContainer.LayoutSystem.LayoutSystems[0],
                            Bounds = ((FloatingContainer) dockContainer).FloatingBounds
                        };
                    }
                }
            }
            ControlLayoutSystem[] array = this.ControlLayoutSystem_0;
            foreach (var controlLayoutSystem in array)
            {
                if (new Rectangle(controlLayoutSystem.DockContainer.PointToScreen(controlLayoutSystem.Bounds.Location), controlLayoutSystem.Bounds.Size).Contains(position))
                {
                    DockTarget dockTarget = this.method_13(controlLayoutSystem.DockContainer, controlLayoutSystem, position, true);
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
                if (this.method_5(containerDockLocation))
                {
                    if (Class7.smethod_2(this.method_7(containerDockLocation, true), this.Manager.DockSystemContainer).Contains(position))
                    {
                        return new Class7.DockTarget(Class7.DockTargetType.CreateNewContainer)
                        {
                            DockLocation = containerDockLocation,
                            Bounds = Class7.smethod_2(this.method_8(containerDockLocation, this.DockedSize, true), this.Manager.DockSystemContainer),
                            middle = true
                        };
                    }
                    if (Class7.smethod_2(this.method_7(containerDockLocation, false), this.Manager.DockSystemContainer).Contains(position))
                    {
                        return new Class7.DockTarget(Class7.DockTargetType.CreateNewContainer)
                        {
                            DockLocation = containerDockLocation,
                            Bounds = Class7.smethod_2(this.method_8(containerDockLocation, this.DockedSize, false), this.Manager.DockSystemContainer)
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
                    (isFloating || this.method_5(LayoutUtilities.Convert(dockContainer.Dock))) && (!flag || flag2) &&
                    (!flag2 || Container == dockContainer))
                    this.method_11(dockContainer, arrayList);
            }
            var array3 = new ControlLayoutSystem[arrayList.Count];
            arrayList.CopyTo(array3, 0);
            return array3;
        }

        private void method_11(DockContainer container, ArrayList arrayList_0)
        {
            if ((container.Width > 0 || container.Height > 0) && container.Enabled && container.Visible)
            {
                this.method_12(container, container.LayoutSystem, arrayList_0);
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
                        if (this.SourceControl == null || layoutSystemBase != this.SourceControlSystem)
                        {
                            goto IL_59;
                        }
                        if (this.SourceControl.LayoutSystem.Controls.Count != 1)
                        {
                            goto IL_59;
                        }
                        bool arg_67_0 = false;
                        IL_67:
                        if (arg_67_0)
                        {
                            arrayList_0.Add(layoutSystemBase);
                            continue;
                        }
                        continue;
                        IL_59:
                        arg_67_0 = !((ControlLayoutSystem)layoutSystemBase).Collapsed;
                        goto IL_67;
                    }
                }
                else
                {
                    this.method_12(dockContainer_1, (SplitLayoutSystem)layoutSystemBase, arrayList_0);
                }
            }
        }

        protected DockTarget method_13(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Point point_1, bool bool_2)
        {
            DockTarget dockTarget = new DockTarget(DockTargetType.Undefined);
            Point point = dockContainer_1.PointToClient(point_1);
            if (this.SourceControl != null || controlLayoutSystem_1 != this.SourceControlSystem)
            {
                if (controlLayoutSystem_1.Rectangle_0.Contains(point) || controlLayoutSystem_1.rectangle_2.Contains(point))
                {
                    dockTarget = new Class7.DockTarget(Class7.DockTargetType.JoinExistingSystem)
                    {
                        dockContainer = dockContainer_1,
                        layoutSystem = controlLayoutSystem_1,
                        DockSide = DockSide.None,
                        Bounds = new Rectangle(dockContainer_1.PointToScreen(controlLayoutSystem_1.Bounds.Location),
                                controlLayoutSystem_1.Bounds.Size)
                    };
                    dockTarget.index = !controlLayoutSystem_1.rectangle_2.Contains(point) ? controlLayoutSystem_1.Controls.Count : controlLayoutSystem_1.method_15(point);
                }
                if (dockTarget.type == DockTargetType.Undefined && bool_2)
                {
                    dockTarget = this.method_14(dockContainer_1, controlLayoutSystem_1, point_1);
                }
                return dockTarget;
            }
            return controlLayoutSystem_1.Rectangle_0.Contains(point) ? new DockTarget(DockTargetType.None) : new DockTarget(DockTargetType.Undefined);
        }

        private DockTarget method_14(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Point point_1)
        {
            DockTarget dockTarget = null;
            Point point = dockContainer_1.PointToClient(point_1);
            var rectangle_ = controlLayoutSystem_1.rectangle_3;
            if (new Rectangle(rectangle_.Left, rectangle_.Top, rectangle_.Width, 30).Contains(point))
            {
                dockTarget = this.method_21(dockContainer_1, controlLayoutSystem_1);
                if (point.X >= rectangle_.Left + 30)
                {
                    if (point.X > rectangle_.Right - 30)
                    {
                        this.method_17(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                    }
                    else
                    {
                        this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Top);
                    }
                }
                else
                {
                    this.method_18(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                }
            }
            else if (new Rectangle(rectangle_.Left, rectangle_.Top, 30, rectangle_.Height).Contains(point))
            {
                dockTarget = this.method_21(dockContainer_1, controlLayoutSystem_1);
                if (point.Y >= rectangle_.Top + 30)
                {
                    if (point.Y <= rectangle_.Bottom - 30)
                    {
                        this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Left);
                    }
                    else
                    {
                        this.method_16(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                    }
                }
                else
                {
                    this.method_18(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                }
            }
            else if (!new Rectangle(rectangle_.Right - 30, rectangle_.Top, 30, rectangle_.Height).Contains(point))
            {
                if (new Rectangle(rectangle_.Left, rectangle_.Bottom - 30, rectangle_.Width, 30).Contains(point))
                {
                    dockTarget = this.method_21(dockContainer_1, controlLayoutSystem_1);
                    if (point.X >= rectangle_.Left + 30)
                    {
                        if (point.X <= rectangle_.Right - 30)
                        {
                            this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Bottom);
                        }
                        else
                        {
                            this.method_15(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                        }
                    }
                    else
                    {
                        this.method_16(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                    }
                }
            }
            else
            {
                dockTarget = this.method_21(dockContainer_1, controlLayoutSystem_1);
                if (point.Y >= rectangle_.Top + 30)
                {
                    if (point.Y <= rectangle_.Bottom - 30)
                    {
                        this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Right);
                    }
                    else
                    {
                        this.method_15(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
                    }
                }
                else
                {
                    this.method_17(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
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
            this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, point_1.Y > rectangle_1.Top + (int)(rectangle_1.Height * ((float)point_1.X / rectangle_1.Width))
                ? DockSide.Bottom
                : DockSide.Right);
        }

        private void method_16(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            rectangle_1.Y = rectangle_1.Bottom - 30;
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            if (point_1.Y >
                rectangle_1.Bottom - (int)(rectangle_1.Height * (point_1.X / (float)rectangle_1.Width)))
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Bottom);
            }
            else
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Left);
            }
        }

        private void method_17(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            rectangle_1.X = rectangle_1.Right - 30;
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            if (point_1.Y <= rectangle_1.Top + (int)((float)rectangle_1.Height * ((float)(rectangle_1.Right - point_1.X) / (float)rectangle_1.Width)))
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Top);
            }
            else
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Right);
            }
        }

        private void method_18(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            if (point_1.Y <=
                rectangle_1.Top + (int)((float)rectangle_1.Height * ((float)point_1.X / (float)rectangle_1.Width)))
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Top);
            }
            else
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Left);
            }
        }

        private void method_19(DockContainer container, ControlLayoutSystem layoutSystem, DockTarget dockTarget, DockSide dockSide)
        {
            dockTarget.Bounds = this.method_20(container, layoutSystem, dockSide);
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

        public bool method_5(ContainerDockLocation location)
        {
            return SourceControl?.method_13(location) ?? SourceControlSystem.vmethod_3(location);
        }

        private Rectangle method_6(Rectangle rect)
        {
            if (rect.X >= Screen.PrimaryScreen.Bounds.X && rect.Right <= Screen.PrimaryScreen.Bounds.Right &&
                rect.Y > Screen.PrimaryScreen.WorkingArea.Bottom)
                rect.Y = Screen.PrimaryScreen.WorkingArea.Bottom - rect.Height;
            var screen = Screen.FromRectangle(rect);
            if (rect.Y < screen.WorkingArea.Y)
                rect.Y = screen.WorkingArea.Y;
            return rect;
        }

        protected Rectangle method_7(ContainerDockLocation location, bool bool_2)
        {
            if (bool_2)
                return this.method_8(location, 30, true);
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
            var rectangle = smethod_1(Manager.DockSystemContainer);
            Rectangle result = rectangle;
            if (!bool_2)
            {
                result = this.Manager.DockSystemContainer.ClientRectangle;
            }
            int val = int_8 + 4;
            switch (location)
            {
                case ContainerDockLocation.Left:
                    return new Rectangle(result.Left, result.Top, Math.Min(val, Convert.ToInt32((double)rectangle.Width * 0.9)), result.Height);
                case ContainerDockLocation.Right:
                    return new Rectangle(result.Right - Math.Min(val, Convert.ToInt32((double)rectangle.Width * 0.9)), result.Top, Math.Min(val, Convert.ToInt32((double)rectangle.Width * 0.9)), result.Height);
                case ContainerDockLocation.Top:
                    return new Rectangle(result.Left, result.Top, result.Width, Math.Min(val, Convert.ToInt32((double)rectangle.Height * 0.9)));
                case ContainerDockLocation.Bottom:
                    return new Rectangle(result.Left, result.Bottom - Math.Min(val, Convert.ToInt32((double)rectangle.Height * 0.9)), result.Width, Math.Min(val, Convert.ToInt32((double)rectangle.Height * 0.9)));
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
                dockTarget = this.FindDockTarget(position);
            }
            if (dockTarget == null || (dockTarget.type == Class7.DockTargetType.Undefined && this.Manager != null && this.AllowFloat))
            {
                if (Manager != null && AllowFloat)
                {
                    dockTarget = new DockTarget(DockTargetType.Float);
                }
                else
                {
                    dockTarget = new Class7.DockTarget(Class7.DockTargetType.None);
                }
            }
            if (dockTarget.type == Class7.DockTargetType.Undefined)
            {
                dockTarget.type = Class7.DockTargetType.None;
            }
            if (dockTarget.type == Class7.DockTargetType.Float)
            {
                dockTarget.Bounds = new Rectangle(this.Point_0, this.size_0);
                dockTarget.Bounds = this.method_6(dockTarget.Bounds);
            }
            if (dockTarget.layoutSystem == this.SourceControlSystem && this.SourceControl != null)
            {
                if (dockTarget.DockSide == DockSide.None)
                {
                    base.UpdateHintForm();
                    ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)this.SourceControlSystem;
                    if (dockTarget.index != controlLayoutSystem.Controls.IndexOf(this.SourceControl))
                    {
                        if (dockTarget.index != controlLayoutSystem.Controls.IndexOf(this.SourceControl) + 1)
                        {
                            controlLayoutSystem.Controls.SetChildIndex(this.SourceControl, dockTarget.index);
                        }
                    }
                    dockTarget.type = Class7.DockTargetType.AlreadyActioned;
                    goto IL_147;
                }
            }
            if (dockTarget.type != Class7.DockTargetType.None)
            {
                base.method_1(dockTarget.Bounds, dockTarget.type == Class7.DockTargetType.JoinExistingSystem);
            }
            else
            {
                base.UpdateHintForm();
            }
            IL_147:
            if (this.Container is DocumentContainer)
            {
                if (dockTarget.type == Class7.DockTargetType.AlreadyActioned)
                {
                    Cursor.Current = Cursors.Default;
                }
                else if (dockTarget.type != Class7.DockTargetType.None)
                {
                    Cursor.Current = this.cursor_0;
                }
                else
                {
                    Cursor.Current = this.cursor_1;
                }
            }
            this.Target = dockTarget;
        }

        public static Rectangle smethod_1(Control parentControl)
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

        public static Rectangle smethod_2(Rectangle bounds, Control control)
        {
            return new Rectangle(control.PointToScreen(bounds.Location), bounds.Size);
        }

        public bool Boolean_0 => Container.Boolean_0;

        public bool AllowFloat
        {
            get
            {
                if (Boolean_0) return false;
                return SourceControl?.DockingRules.AllowFloat ?? SourceControlSystem.AllowFloat;
            }
        }

        protected ControlLayoutSystem[] ControlLayoutSystem_0 { get; }

        public DockContainer Container { get; }

        public DockControl SourceControl { get; }

        public DockTarget Target { get; private set; }

        public int DockedSize { get; }

        public LayoutSystemBase SourceControlSystem { get; }

        private Point Point_0 => new Point(Cursor.Position.X - this.point_0.X, Cursor.Position.Y - this.point_0.Y);

        public SandDockManager Manager { get; }

        public event DockingManagerFinishedEventHandler DockingManagerFinished;

        private Cursor cursor_0;

        private Cursor cursor_1;

        private const int int_6 = 30;

        private Point point_0 = Point.Empty;

        private Size size_0 = Size.Empty;

        public delegate void DockingManagerFinishedEventHandler(DockTarget target);

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

    internal class Class8 : Class7
    {
        public Class8(SandDockManager manager, DockContainer container, LayoutSystemBase sourceControlSystem, DockControl sourceControl, int dockedSize, Point startPoint, DockingHints dockingHints) : base(manager, container, sourceControlSystem, sourceControl, dockedSize, startPoint, dockingHints)
        {
            this.arrayList_0 = new ArrayList();
            if (Manager?.DockSystemContainer != null)
                this.method_22();
        }

        public override void Dispose()
        {
            if (this.form4_0 != null)
            {
                this.form4_0.method_11();
                this.form4_0 = null;
            }
            foreach (Form4 form in this.arrayList_0)
            {
                form.method_11();
            }
            this.arrayList_0.Clear();
            base.Dispose();
        }

        protected override DockTarget FindDockTarget(Point position)
        {
            DockTarget target;
            bool flag = this.rectangle_1.Contains(position);
            bool flag2 = this.rectangle_2.Contains(position);
            if (flag == this.bool_2)
            {
                if (flag2 == this.bool_3)
                {
                    goto IL_C1;
                }
            }
            object[] array = this.arrayList_0.ToArray();
            int i = 0;
            while (i < array.Length)
            {
                Class8.Form4 form = (Class8.Form4)array[i];
                if (form.DockStyle != DockStyle.Fill)
                {
                    goto IL_7B;
                }
                if (flag2 == this.bool_3)
                {
                    goto IL_7B;
                }
                if (!flag2)
                {
                    form.method_12();
                }
                else
                {
                    form.method_13();
                }
                IL_A5:
                i++;
                continue;
                IL_7B:
                if (form.DockStyle == DockStyle.Fill)
                {
                    goto IL_A5;
                }
                if (flag == this.bool_2)
                {
                    goto IL_A5;
                }
                if (flag)
                {
                    form.method_13();
                    goto IL_A5;
                }
                form.method_12();
                goto IL_A5;
            }
            this.bool_2 = flag;
            this.bool_3 = flag2;
            IL_C1:
            ControlLayoutSystem controlLayoutSystem = this.method_23(position, out target);
            if (controlLayoutSystem == base.SourceControlSystem && base.SourceControl == null)
            {
                controlLayoutSystem = null;
            }
            if (controlLayoutSystem != this.controlLayoutSystem_1)
            {
                if (this.form4_0 != null)
                {
                    this.form4_0.method_11();
                    this.form4_0 = null;
                }
                this.controlLayoutSystem_1 = controlLayoutSystem;
                if (this.controlLayoutSystem_1 != null)
                {
                    this.form4_0 = new Class8.Form4(this, this.controlLayoutSystem_1);
                    this.form4_0.method_13();
                }
            }
            if (target != null && target.type == Class7.DockTargetType.Undefined)
            {
                target = null;
            }
            if (this.form4_0 != null && this.form4_0.Rectangle_5.Contains(position) && target == null)
            {
                target = this.form4_0.method_4(position);
            }
            object[] array2 = this.arrayList_0.ToArray();
            for (int j = 0; j < array2.Length; j++)
            {
                Class8.Form4 form2 = (Class8.Form4)array2[j];
                if (target == null && form2.Rectangle_5.Contains(position))
                {
                    target = form2.method_4(position);
                }
            }
            return target;
        }

        private void method_22()
        {
            this.rectangle_1 = smethod_2(Manager.DockSystemContainer.ClientRectangle, Manager.DockSystemContainer);
            this.rectangle_2 = smethod_2(smethod_1(Manager.DockSystemContainer), Manager.DockSystemContainer);
            if (base.method_5(ContainerDockLocation.Top))
            {
                this.arrayList_0.Add(new Form4(this, this.rectangle_1, DockStyle.Top));
            }
            if (base.method_5(ContainerDockLocation.Left))
            {
                this.arrayList_0.Add(new Form4(this, this.rectangle_1, DockStyle.Left));
            }
            if (base.method_5(ContainerDockLocation.Bottom))
            {
                this.arrayList_0.Add(new Form4(this, this.rectangle_1, DockStyle.Bottom));
            }
            if (base.method_5(ContainerDockLocation.Right))
            {
                this.arrayList_0.Add(new Form4(this, this.rectangle_1, DockStyle.Right));
            }
            bool flag = Container.Dock == DockStyle.Fill && !Container.IsFloating;
            bool flag2 = base.method_5(ContainerDockLocation.Left) || base.method_5(ContainerDockLocation.Right) || base.method_5(ContainerDockLocation.Top) || base.method_5(ContainerDockLocation.Bottom);
            if (!flag && (base.method_5(ContainerDockLocation.Center) || flag2))
            {
                this.arrayList_0.Add(new Form4(this, this.rectangle_2, DockStyle.Fill));
            }
            if (Manager?.OwnerForm != null)
            {
                foreach (Form ownedForm in this.arrayList_0)
                    Manager.OwnerForm.AddOwnedForm(ownedForm);
            }
        }

        private ControlLayoutSystem method_23(Point point_1, out DockTarget dockTarget_1)
        {
            dockTarget_1 = null;
            for (int i = 1; i >= 0; i--)
            {
                bool flag = Convert.ToBoolean(i);
                ControlLayoutSystem[] controlLayoutSystem_ = base.ControlLayoutSystem_0;
                foreach (ControlLayoutSystem controlLayoutSystem in controlLayoutSystem_)
                {
                    if (controlLayoutSystem.DockContainer.IsFloating == flag)
                    {
                        Rectangle rectangle = new Rectangle(controlLayoutSystem.DockContainer.PointToScreen(controlLayoutSystem.Bounds.Location), controlLayoutSystem.Bounds.Size);
                        if (rectangle.Contains(point_1))
                        {
                            dockTarget_1 = base.method_13(controlLayoutSystem.DockContainer, controlLayoutSystem, point_1, false);
                            ControlLayoutSystem result;
                            if (dockTarget_1.type != Class7.DockTargetType.Undefined)
                            {
                                result = null;
                            }
                            else
                            {
                                result = controlLayoutSystem;
                            }
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        private ArrayList arrayList_0;

        private bool bool_2;

        private bool bool_3;

        private ControlLayoutSystem controlLayoutSystem_1;

        private Form4 form4_0;

        private Rectangle rectangle_1;

        private Rectangle rectangle_2;

        internal class Form3 : Form
        {
            public Form3()
            {
                FormBorderStyle = FormBorderStyle.None;
            }

            public void method_0(Bitmap bitmap, byte alpha)
            {
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
                    Native.UpdateLayeredWindow(Handle, dc, ref point2, ref size, cdc, ref point, 0, ref blend, 2);
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

            protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.ExStyle |= WS_SYSMENU;
                    return cp;
                }
            }

            private const int WS_SYSMENU = 0x00080000; //524288 
        }

        private class Form4 : Form3
        {
            private Form4()
            {
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.Manual;
                _timer = new Timer { Interval = 50 };
                _timer.Tick += OnTimerTick;
                _bitmap = new Bitmap(88, 88, PixelFormat.Format32bppArgb);
            }

            public Form4(Class8 manager, ControlLayoutSystem layoutSystem) : this()
            {
                _manager = manager;
                _layoutSystem = layoutSystem;
                this.Rectangle_5 = new Rectangle(layoutSystem.DockContainer.PointToScreen(layoutSystem.Bounds.Location), layoutSystem.Bounds.Size);
                this.Rectangle_5 = new Rectangle(this.Rectangle_5.X + this.Rectangle_5.Width / 2 - 44, this.Rectangle_5.Y + this.Rectangle_5.Height / 2 - 44, 88, 88);
                this.method_1();
            }

            public Form4(Class8 manager, Rectangle fc, DockStyle dockStyle) : this()
            {
                _manager = manager;
                DockStyle = dockStyle;
                switch (dockStyle)
                {
                    case DockStyle.Top:
                        Rectangle_5 = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Y + 15, 88, 88);
                        break;
                    case DockStyle.Bottom:
                        Rectangle_5 = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Bottom - 88 - 15, 88, 88);
                        break;
                    case DockStyle.Left:
                        Rectangle_5 = new Rectangle(fc.X + 15, fc.Y + fc.Height / 2 - 44, 88, 88);
                        break;
                    case DockStyle.Right:
                        Rectangle_5 = new Rectangle(fc.Right - 88 - 15, fc.Y + fc.Height / 2 - 44, 88, 88);
                        break;
                    case DockStyle.Fill:
                        Rectangle_5 = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Y + fc.Height / 2 - 44, 88, 88);
                        break;
                }
                this.method_1();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _bitmap.Dispose();
                    _timer.Tick -= OnTimerTick;
                    _timer.Dispose();
                }
                base.Dispose(disposing);
            }

            private void method_1()
            {
                using (var g = Graphics.FromImage(_bitmap))
                {
                    Class16.smethod_0(g, Color.Transparent);
                    if (DockStyle != DockStyle.None && DockStyle != DockStyle.Fill)
                    {
                        if (DockStyle == DockStyle.Top)
                        {
                            using (var image = Image.FromStream(typeof(Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghinttop.png")))
                            {
                                g.DrawImageUnscaled(image, 29, 0);
                                goto IL_170;
                            }
                        }
                        if (this.DockStyle != DockStyle.Bottom)
                        {
                            if (this.DockStyle != DockStyle.Left)
                            {
                                if (this.DockStyle != DockStyle.Right)
                                {
                                    goto IL_170;
                                }
                                using (Image image2 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintright.png")))
                                {
                                    g.DrawImageUnscaled(image2, 57, 29);
                                    goto IL_170;
                                }
                            }
                            using (Image image3 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintleft.png")))
                            {
                                g.DrawImageUnscaled(image3, 0, 29);
                                goto IL_170;
                            }
                        }
                        using (Image image4 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintbottom.png")))
                        {
                            g.DrawImageUnscaled(image4, 29, 57);
                            goto IL_170;
                        }
                    }
                    using (Image image5 = Image.FromStream(typeof(Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintcenter.png")))
                    {
                        g.DrawImageUnscaled(image5, 0, 0);
                    }
                    IL_170:
                    Color highlight = SystemColors.Highlight;
                    Color transparent = Color.Transparent;
                    if (this.DockStyle == DockStyle.None || this.DockStyle == DockStyle.Fill || this.DockStyle == DockStyle.Top)
                    {
                        this.method_10(g, (!this.bool_0 || this._dockSide != DockSide.Top) ? transparent : highlight);
                    }
                    if (this.DockStyle != DockStyle.None)
                    {
                        if (this.DockStyle != DockStyle.Fill)
                        {
                            if (this.DockStyle != DockStyle.Right)
                            {
                                goto IL_1F3;
                            }
                        }
                    }
                    Graphics arg_1EE_1 = g;
                    Color arg_1EE_2;
                    if (this.bool_0)
                    {
                        if (this._dockSide == DockSide.Right)
                        {
                            arg_1EE_2 = highlight;
                            goto IL_1EE;
                        }
                    }
                    arg_1EE_2 = transparent;
                    IL_1EE:
                    this.method_9(arg_1EE_1, arg_1EE_2);
                    IL_1F3:
                    if (this.DockStyle == DockStyle.None || this.DockStyle == DockStyle.Fill || this.DockStyle == DockStyle.Bottom)
                    {
                        Graphics arg_22A_1 = g;
                        Color arg_22A_2;
                        if (this.bool_0)
                        {
                            if (this._dockSide == DockSide.Bottom)
                            {
                                arg_22A_2 = highlight;
                                goto IL_22A;
                            }
                        }
                        arg_22A_2 = transparent;
                        IL_22A:
                        this.method_8(arg_22A_1, arg_22A_2);
                    }
                    if (this.DockStyle != DockStyle.None)
                    {
                        if (this.DockStyle != DockStyle.Fill)
                        {
                            if (this.DockStyle != DockStyle.Left)
                            {
                                goto IL_26B;
                            }
                        }
                    }
                    Graphics arg_266_1 = g;
                    Color arg_266_2;
                    if (this.bool_0)
                    {
                        if (this._dockSide == DockSide.Left)
                        {
                            arg_266_2 = highlight;
                            goto IL_266;
                        }
                    }
                    arg_266_2 = transparent;
                    IL_266:
                    this.method_7(arg_266_1, arg_266_2);
                    IL_26B:
                    if (this.DockStyle == DockStyle.None || this.DockStyle == DockStyle.Fill)
                    {
                        Graphics arg_299_1 = g;
                        Color arg_299_2;
                        if (this.bool_0)
                        {
                            if (this._dockSide == DockSide.None)
                            {
                                arg_299_2 = highlight;
                                goto IL_299;
                            }
                        }
                        arg_299_2 = transparent;
                        IL_299:
                        this.method_6(arg_299_1, arg_299_2);
                    }
                }
                base.method_0(this._bitmap, 255);
            }

            private void method_10(Graphics g, Color c)
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
                this.bool_2 = true;
                this.method_12();
            }

            public void method_12()
            {
                if (Visible || (!this.bool_1 && _timer.Enabled))
                {
                    this.int_5 = Environment.TickCount;
                    this.bool_1 = true;
                    _timer.Start();
                }
            }

            public void method_13()
            {
                base.method_0(this._bitmap, 0);
                this.method_14();
                this.int_5 = Environment.TickCount;
                this.bool_1 = false;
                this._timer.Start();
            }

            private void method_14()
            {
                Native.SetWindowPos(Handle, new IntPtr(-1), Rectangle_5.X, Rectangle_5.Y, Rectangle_5.Width, Rectangle_5.Height, WMConstants.SWP_SHOWWINDOW + WMConstants.SWP_NOACTIVATE);

            }

            private DockTarget method_2(Point point_0)
            {
                var dockTarget = new DockTarget(DockTargetType.SplitExistingSystem)
                {
                    layoutSystem = this._layoutSystem,
                    dockContainer = this._layoutSystem.DockContainer
                };
                if (this.method_5(this.Rectangle_1, point_0))
                {
                    dockTarget.DockSide = DockSide.Top;
                }
                else if (!this.method_5(this.Rectangle_2, point_0))
                {
                    if (!this.method_5(this.Rectangle_3, point_0))
                    {
                        if (!this.method_5(this.Rectangle_4, point_0))
                        {
                            if (!this.method_5(this.Rectangle_0, point_0))
                            {
                                dockTarget.type = Class7.DockTargetType.Undefined;
                            }
                            else
                            {
                                dockTarget.type = Class7.DockTargetType.JoinExistingSystem;
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
                dockTarget.Bounds = _manager.method_20(this._layoutSystem.DockContainer, this._layoutSystem, dockTarget.DockSide);
                return dockTarget;
            }

            private DockTarget method_3(Point point_0)
            {
                var dockTarget = new DockTarget(DockTargetType.SplitExistingSystem)
                {
                    layoutSystem = this._layoutSystem,
                    dockContainer = _layoutSystem?.DockContainer
                };
                if (this.method_5(this.Rectangle_1, point_0) && this._manager.method_5(ContainerDockLocation.Top))
                {
                    if (this.DockStyle != DockStyle.Top)
                    {
                        if (this.DockStyle != DockStyle.Fill)
                        {
                            goto IL_75;
                        }
                    }
                    dockTarget.DockLocation = ContainerDockLocation.Top;
                    dockTarget.DockSide = DockSide.Top;
                    goto IL_178;
                }
                IL_75:
                if (this.method_5(this.Rectangle_2, point_0) && this._manager.method_5(ContainerDockLocation.Right))
                {
                    if (this.DockStyle != DockStyle.Right)
                    {
                        if (this.DockStyle != DockStyle.Fill)
                        {
                            goto IL_B9;
                        }
                    }
                    dockTarget.DockLocation = ContainerDockLocation.Right;
                    dockTarget.DockSide = DockSide.Right;
                    goto IL_178;
                }
                IL_B9:
                if (this.method_5(this.Rectangle_3, point_0) && this._manager.method_5(ContainerDockLocation.Bottom))
                {
                    if (this.DockStyle == DockStyle.Bottom || this.DockStyle == DockStyle.Fill)
                    {
                        dockTarget.DockLocation = ContainerDockLocation.Bottom;
                        dockTarget.DockSide = DockSide.Bottom;
                        goto IL_178;
                    }
                }
                if (!this.method_5(this.Rectangle_4, point_0) || !this._manager.method_5(ContainerDockLocation.Left) || (this.DockStyle != DockStyle.Left && this.DockStyle != DockStyle.Fill))
                {
                    if (this.method_5(this.Rectangle_0, point_0) && this._manager.method_5(ContainerDockLocation.Center))
                    {
                        if (this.DockStyle == DockStyle.Fill)
                        {
                            dockTarget.DockLocation = ContainerDockLocation.Center;
                            dockTarget.DockSide = DockSide.None;
                            goto IL_178;
                        }
                    }
                    dockTarget.type = Class7.DockTargetType.Undefined;
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
                    dockTarget.Bounds = smethod_2(_manager.method_8(dockTarget.DockLocation, _manager.DockedSize, dockTarget.middle), _manager.Manager.DockSystemContainer);
                }
                return dockTarget;
            }

            public DockTarget method_4(Point point)
            {
                var p = PointToClient(point);
                var dockTarget = this._layoutSystem != null ? this.method_2(p) : this.method_3(p);
                bool flag = dockTarget.type != DockTargetType.Undefined;
                var dockSide = (dockTarget.type == DockTargetType.Undefined) ? _dockSide : dockTarget.DockSide;
                if (flag != this.bool_0 || dockSide != _dockSide)
                {
                    this.bool_0 = flag;
                    this._dockSide = dockSide;
                    this.method_1();
                }
                return dockTarget;
            }

            private bool method_5(Rectangle r, Point p)
            {
                return r.Contains(p);
            }

            private void method_6(Graphics g, Color c)
            {
                using (var pen = new Pen(c))
                {
                    g.DrawLine(pen, 22, 29, 29, 22);
                    g.DrawLine(pen, 57, 22, 64, 29);
                    g.DrawLine(pen, 64, 57, 57, 64);
                    g.DrawLine(pen, 29, 64, 22, 57);
                }
            }

            private void method_7(Graphics g, Color c)
            {
                using (var pen = new Pen(c))
                {
                    g.DrawLine(pen, 0, 29, 0, 57);
                    g.DrawLine(pen, 0, 57, 23, 57);
                    g.DrawLine(pen, 0, 29, 23, 29);
                }
            }

            private void method_8(Graphics g, Color c)
            {
                using (var pen = new Pen(c))
                {
                    g.DrawLine(pen, 29, 87, 57, 87);
                    g.DrawLine(pen, 29, 87, 29, 64);
                    g.DrawLine(pen, 57, 87, 57, 64);
                }
            }

            private void method_9(Graphics g, Color c)
            {
                using (var pen = new Pen(c))
                {
                    g.DrawLine(pen, 87, 29, 87, 57);
                    g.DrawLine(pen, 87, 29, 64, 29);
                    g.DrawLine(pen, 87, 57, 64, 57);
                }
            }

            private void OnTimerTick(object sender, EventArgs e)
            {
                int num = Environment.TickCount - this.int_5;
                if (num > 200)
                {
                    num = 200;
                }
                double num2 = (double)num / 200.0;
                if (this.bool_1)
                {
                    num2 = (1.0 - num2) * 255.0;
                }
                else
                {
                    num2 *= 255.0;
                }
                base.method_0(_bitmap, (byte)num2);
                if (num >= 200)
                {
                    this._timer.Stop();
                    base.Visible = !this.bool_1;
                    if (this.bool_2)
                    {
                        base.Dispose();
                    }
                }
            }

            public DockStyle DockStyle { get; }

            private Rectangle Rectangle_0 => new Rectangle(28, 28, 32, 32);

            private Rectangle Rectangle_1 => new Rectangle(29, 0, 29, 28);

            private Rectangle Rectangle_2 => new Rectangle(60, 29, 28, 29);

            private Rectangle Rectangle_3 => new Rectangle(29, 60, 29, 28);

            private Rectangle Rectangle_4 => new Rectangle(0, 29, 28, 29);

            public Rectangle Rectangle_5 { get; } = Rectangle.Empty;

            private readonly Bitmap _bitmap;

            private bool bool_0;

            private bool bool_1;

            private bool bool_2;

            private readonly Class8 _manager;

            private readonly ControlLayoutSystem _layoutSystem;

            private DockSide _dockSide = DockSide.None;

            private const int int_0 = 88;

            private const int int_1 = 88;

            private const int int_2 = 200;

            private const int int_3 = 16;

            private const int int_4 = 64;

            private int int_5;

            private readonly Timer _timer;
        }
    }

    internal class ResizingManager : AbstractManager
    {
        public ResizingManager(AutoHideBar bar, PopupContainer popupContainer, Point startPoint) : base(bar, bar.Manager?.DockingHints ?? DockingHints.TranslucentFill, false)
        {
            this.control0_0 = bar;
            this.control1_0 = popupContainer;
            this.point_0 = startPoint;
            int num = bar.Manager?.MinimumDockContainerSize ?? 30;
            int num2 = bar.Manager?.MaximumDockContainerSize ?? 500;
            this.int_6 = popupContainer.Int32_0;
            switch (bar.Dock)
            {
                case DockStyle.Top:
                    if (bar.Manager?.DockSystemContainer != null)
                    {
                        num2 = Math.Max(bar.Manager.DockSystemContainer.Height - popupContainer.Bounds.Top - num, num);
                    }
                    this.int_7 = startPoint.Y - (this.int_6 - num);
                    this.int_8 = startPoint.Y + (num2 - this.int_6);
                    break;
                case DockStyle.Bottom:
                    if (bar.Manager?.DockSystemContainer != null)
                    {
                        num2 = Math.Max(popupContainer.Bounds.Bottom - num, num);
                    }
                    this.int_7 = startPoint.Y - (num2 - this.int_6);
                    this.int_8 = startPoint.Y + (this.int_6 - num);
                    break;
                case DockStyle.Left:
                    if (bar.Manager?.DockSystemContainer != null)
                    {
                        num2 = Math.Max(bar.Manager.DockSystemContainer.Width - popupContainer.Bounds.Left - num, num);
                    }
                    this.int_7 = startPoint.X - (this.int_6 - num);
                    this.int_8 = startPoint.X + (num2 - this.int_6);
                    break;
                case DockStyle.Right:
                    if (bar.Manager?.DockSystemContainer != null)
                    {
                        num2 = Math.Max(popupContainer.Bounds.Right - num, num);
                    }
                    this.int_7 = startPoint.X - (num2 - this.int_6);
                    this.int_8 = startPoint.X + (this.int_6 - num);
                    break;
            }
            OnMouseMove(startPoint);
        }

        public override void Commit()
        {
            base.Commit();
            ResizingManagerFinished?.Invoke(this.int_9);
        }

        public override void OnMouseMove(Point position)
        {
            Rectangle empty = Rectangle.Empty;
            if (!this.control0_0.Boolean_0)
            {
                if (position.Y < this.int_7)
                {
                    position.Y = this.int_7;
                }
                if (position.Y > this.int_8)
                {
                    position.Y = this.int_8;
                }
                empty = new Rectangle(0, position.Y - 2, this.control1_0.Width, 4);
            }
            else
            {
                if (position.X < this.int_7)
                {
                    position.X = this.int_7;
                }
                if (position.X > this.int_8)
                {
                    position.X = this.int_8;
                }
                empty = new Rectangle(position.X - 2, 0, 4, this.control1_0.Height);
            }
            switch (this.control0_0.Dock)
            {
                case DockStyle.Top:
                    this.int_9 = this.int_6 + (position.Y - this.point_0.Y);
                    break;
                case DockStyle.Bottom:
                    this.int_9 = this.int_6 + (this.point_0.Y - position.Y);
                    break;
                case DockStyle.Left:
                    this.int_9 = this.int_6 + (position.X - this.point_0.X);
                    break;
                case DockStyle.Right:
                    this.int_9 = this.int_6 + (this.point_0.X - position.X);
                    break;
            }
            base.method_1(new Rectangle(this.control1_0.PointToScreen(empty.Location), empty.Size), false);
        }

        public event ResizingManagerFinishedEventHandler ResizingManagerFinished;

        private AutoHideBar control0_0;

        private PopupContainer control1_0;

        private int int_6;

        private int int_7;

        private int int_8;

        private int int_9;

        private Point point_0;

        public delegate void ResizingManagerFinishedEventHandler(int newSize);
    }

    internal class SplittingManager : AbstractManager
    {
        public SplittingManager(DockContainer container, SplitLayoutSystem splitLayout, LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, Point startPoint, DockingHints dockingHints) : base(container, dockingHints, false)
        {
            this.container = container;
            this.SplitLayout = splitLayout;
            this.aboveLayout = aboveLayout;
            this.belowLayout = belowLayout;
            this.startPoint = startPoint;
            if (splitLayout.SplitMode != Orientation.Horizontal)
            {
                this.int_7 = aboveLayout.Bounds.X + 25;
                this.int_8 = belowLayout.Bounds.Right - 25;
                this.float_2 = aboveLayout.WorkingSize.Width + belowLayout.WorkingSize.Width;
            }
            else
            {
                this.int_7 = aboveLayout.Bounds.Y + 25;
                this.int_8 = belowLayout.Bounds.Bottom - 25;
                this.float_2 = aboveLayout.WorkingSize.Height + belowLayout.WorkingSize.Height;
            }
            OnMouseMove(startPoint);
        }

        public override void Commit()
        {
            base.Commit();
            SplittingManagerFinished?.Invoke(this.aboveLayout, this.belowLayout, this.aboveSize, this.belowSize);
        }

        public override void OnMouseMove(Point position)
        {
            Rectangle empty = Rectangle.Empty;
            if (this.SplitLayout.SplitMode != Orientation.Horizontal)
            {
                empty = new Rectangle(position.X - 2, this.SplitLayout.Bounds.Y, 4, this.SplitLayout.Bounds.Height);
                empty.X = Math.Max(empty.X, this.int_7);
                empty.X = Math.Min(empty.X, this.int_8 - 4);
                float num = (float)(this.belowLayout.Bounds.Right - this.aboveLayout.Bounds.Left - 4);
                this.aboveSize = (float)(empty.X - this.aboveLayout.Bounds.Left) / num * this.float_2;
                this.belowSize = this.float_2 - this.aboveSize;
            }
            else
            {
                empty = new Rectangle(this.SplitLayout.Bounds.X, position.Y - 2, this.SplitLayout.Bounds.Width, 4);
                empty.Y = Math.Max(empty.Y, this.int_7);
                empty.Y = Math.Min(empty.Y, this.int_8 - 4);
                float num2 = (float)(this.belowLayout.Bounds.Bottom - this.aboveLayout.Bounds.Top - 4);
                this.aboveSize = (float)(empty.Y - this.aboveLayout.Bounds.Top) / num2 * this.float_2;
                this.belowSize = this.float_2 - this.aboveSize;
            }
            base.method_1(new Rectangle(this.container.PointToScreen(empty.Location), empty.Size), false);
            Cursor.Current = SplitLayout.SplitMode != Orientation.Horizontal ? Cursors.VSplit : Cursors.HSplit;
        }

        public SplitLayoutSystem SplitLayout { get; }

        public event SplittingManagerFinishedEventHandler SplittingManagerFinished;

        private DockContainer container;

        private float aboveSize;

        private float belowSize;

        private float float_2;

        internal const int int_6 = 25;

        private int int_7;

        private int int_8;

        private LayoutSystemBase aboveLayout;

        private LayoutSystemBase belowLayout;

        private Point startPoint = Point.Empty;

        public delegate void SplittingManagerFinishedEventHandler(LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, float aboveSize, float belowSize);
    }

    internal class Class11 : AbstractManager
    {
        public Class11(SandDockManager manager, DockContainer container, Point startPoint) : base(container, manager.DockingHints, false)
        {
            this._container = container;
            Rectangle rectangle = Rectangle.Empty;
            rectangle = Class7.smethod_2(Class7.smethod_1(container.Parent), container.Parent);
            rectangle = new Rectangle(container.PointToClient(rectangle.Location), rectangle.Size);
            int num = manager?.MinimumDockContainerSize ?? 30;
            num = Math.Max(num, LayoutUtilities.smethod_12(container));
            int num2 = manager?.MaximumDockContainerSize ?? 500;
            int int32_ = container.Int32_0;
            switch (container.Dock)
            {
                case DockStyle.Top:
                    this.int_6 = startPoint.Y - (int32_ - num);
                    this.int_7 = Math.Min(rectangle.Bottom - 20, startPoint.Y + (num2 - int32_));
                    this.int_9 = startPoint.Y - container.Rectangle_0.Y;
                    break;
                case DockStyle.Bottom:
                    this.int_6 = Math.Max(rectangle.Top + 20, startPoint.Y - (num2 - int32_));
                    this.int_7 = startPoint.Y + (int32_ - num);
                    this.int_9 = startPoint.Y - container.Rectangle_0.Y;
                    break;
                case DockStyle.Left:
                    this.int_6 = startPoint.X - (int32_ - num);
                    this.int_7 = Math.Min(rectangle.Right - 20, startPoint.X + (num2 - int32_));
                    this.int_9 = startPoint.X - container.Rectangle_0.X;
                    break;
                case DockStyle.Right:
                    this.int_6 = Math.Max(rectangle.Left + 20, startPoint.X - (num2 - int32_));
                    this.int_7 = startPoint.X + (int32_ - num);
                    this.int_9 = startPoint.X - container.Rectangle_0.X;
                    break;
            }
            OnMouseMove(startPoint);
        }

        public override void Commit()
        {
            base.Commit();
            ResizingManagerFinished?.Invoke(this.int_8);
        }

        public override void OnMouseMove(Point position)
        {
            var empty = Rectangle.Empty;
            if (_container.Boolean_1)
            {
                empty = new Rectangle(position.X - this.int_9, 0, 4, this._container.Height);
                if (empty.X < this.int_6)
                {
                    empty.X = this.int_6;
                }
                if (empty.X > this.int_7 - 4)
                {
                    empty.X = this.int_7 - 4;
                }
            }
            else
            {
                empty = new Rectangle(0, position.Y - this.int_9, _container.Width, 4);
                if (empty.Y < this.int_6)
                {
                    empty.Y = this.int_6;
                }
                if (empty.Y > this.int_7 - 4)
                {
                    empty.Y = this.int_7 - 4;
                }
            }
            switch (_container.Dock)
            {
                case DockStyle.Top:
                    this.int_8 = _container.ContentSize + (empty.Y - _container.Rectangle_0.Y);
                    break;
                case DockStyle.Bottom:
                    this.int_8 = _container.ContentSize + (_container.Rectangle_0.Y - empty.Y);
                    break;
                case DockStyle.Left:
                    this.int_8 = _container.ContentSize + (empty.X - _container.Rectangle_0.X);
                    break;
                case DockStyle.Right:
                    this.int_8 = _container.ContentSize + (_container.Rectangle_0.X - empty.X);
                    break;
            }
            base.method_1(new Rectangle(_container.PointToScreen(empty.Location), empty.Size), false);
            Cursor.Current = _container.Dock != DockStyle.Left && _container.Dock != DockStyle.Right ? Cursors.HSplit : Cursors.VSplit;
        }

        public event ResizingManagerFinishedEventHandler ResizingManagerFinished;

        private readonly DockContainer _container;

        private int int_6;

        private int int_7;

        private int int_8;

        private int int_9;

        public delegate void ResizingManagerFinishedEventHandler(int newSize);
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

        internal void method_0(SandDockManager manager, DockContainer container, LayoutSystemBase layoutSystem, DockControl control, int int_2, Point point_0, DockingHints dockingHints, DockingManager dockingManager_0)
        {
            if (dockingManager_0 == DockingManager.Whidbey && AbstractManager.smethod_0())
            {
                this.class7_0 = new Class8(manager, DockContainer, this, control, int_2, point_0, dockingHints);
            }
            else
            {
                this.class7_0 = new Class7(manager, DockContainer, this, control, int_2, point_0, dockingHints);
            }
            this.class7_0.DockingManagerFinished += OnDockingManagerFinished;
            this.class7_0.Event_0 += this.vmethod_1;
            this.class7_0.OnMouseMove(Cursor.Position);
        }

        private void method_1()
        {
            this.class7_0.DockingManagerFinished -= OnDockingManagerFinished;
            this.class7_0.Event_0 -= this.vmethod_1;
            this.class7_0 = null;
        }

        internal void method_2(SandDockManager sandDockManager_0, ContainerDockLocation containerDockLocation_0, ContainerDockEdge containerDockEdge_0)
        {
            DockControl[] dockControl_ = DockControls;
            int num = 0;
            if (dockControl_.Length > 0)
            {
                num = dockControl_[0].MetaData.DockedContentSize;
            }
            Rectangle rectangle = Class7.smethod_1(sandDockManager_0.DockSystemContainer);
            if (containerDockLocation_0 != ContainerDockLocation.Left)
            {
                if (containerDockLocation_0 != ContainerDockLocation.Right)
                {
                    if (containerDockLocation_0 != ContainerDockLocation.Top && containerDockLocation_0 != ContainerDockLocation.Bottom)
                    {
                        goto IL_7C;
                    }
                    num = Math.Min(num, Convert.ToInt32((double)rectangle.Height * 0.9));
                    goto IL_7C;
                }
            }
            num = Math.Min(num, Convert.ToInt32((double)rectangle.Width * 0.9));
            IL_7C:
            if (!(this is ControlLayoutSystem))
            {
                this.Parent?.LayoutSystems.Remove(this);
            }
            else
            {
                LayoutUtilities.smethod_10((ControlLayoutSystem)this);
            }
            DockContainer dockContainer = sandDockManager_0.CreateNewDockContainer(containerDockLocation_0, containerDockEdge_0, num);
            if (!(dockContainer is DocumentContainer))
            {
                dockContainer.LayoutSystem.LayoutSystems.Add(this);
                return;
            }
            var controlLayoutSystem = dockContainer.CreateNewLayoutSystem(WorkingSize);
            dockContainer.LayoutSystem.LayoutSystems.Add(controlLayoutSystem);
            if (this is SplitLayoutSystem)
            {
                ((SplitLayoutSystem)this).MoveToLayoutSystem(controlLayoutSystem);
                return;
            }
            controlLayoutSystem.Controls.AddRange(this.DockControls);
        }

        private void method_3()
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

        internal virtual void OnDockingManagerFinished(Class7.DockTarget target)
        {
            this.method_1();
        }

        internal virtual void vmethod_1(object sender, EventArgs e)
        {
            this.method_1();
        }

        internal virtual void vmethod_2(DockContainer container)
        {
            DockContainer = container;
        }

        internal abstract bool vmethod_3(ContainerDockLocation containerDockLocation_0);

        internal abstract void vmethod_4(RendererBase renderer, Graphics graphics_0, Font font);

        internal abstract bool PersistState { get; }

        internal abstract bool AllowFloat { get; }

        internal abstract bool AllowTab { get; }

        public Rectangle Bounds { get; private set; } = Rectangle.Empty;

        public DockContainer DockContainer { get; private set; }

        internal abstract DockControl[] DockControls { get; }

        public bool IsInContainer => DockContainer != null;

        public SplitLayoutSystem Parent => this.splitLayoutSystem_0;

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
                if (value.Width <= 0f || value.Height <= 0f)
                    throw new ArgumentException("value");
                _workingSize = value;
            }
        }

        internal Class7 class7_0;

        internal const int int_0 = 250;

        internal const int int_1 = 400;

        private SizeF _workingSize = new SizeF(250f, 400f);

        internal SplitLayoutSystem splitLayoutSystem_0;
    }
}
