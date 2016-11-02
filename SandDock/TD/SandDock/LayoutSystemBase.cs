using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
    internal abstract class Class6 : IDisposable, IMessageFilter
    {
        protected Class6(Control control, DockingHints dockingHints, bool hollow)
        {
            this.control_0 = control;
            this.bool_1 = hollow;
            bool flag = OSFeature.Feature.IsPresent(OSFeature.LayeredWindows);
            //   flag = false;
            if (dockingHints == DockingHints.TranslucentFill)
            {
                if (!flag)
                {
                    dockingHints = DockingHints.RubberBand;
                }
            }
            this.dockingHints_0 = dockingHints;
            this.form_0 = control.FindForm();
            if (this.form_0 != null)
            {
                this.form_0.Deactivate += this.form_0_Deactivate;
            }
            control.MouseCaptureChanged += this.method_0;
            Application.AddMessageFilter(this);
            if (dockingHints == DockingHints.TranslucentFill)
                this.form1_0 = new Form1(hollow);
        }

        protected Class6(Control control, DockingHints dockingHints, bool hollow, int tabStripSize) : this(control, dockingHints, hollow)
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
            if (this.control_0 != null)
            {
                this.control_0.MouseCaptureChanged -= this.method_0;
            }
            this.method_2();
            if (this.dockingHints_0 == DockingHints.TranslucentFill)
            {
                this.form1_0.Dispose();
                this.form1_0 = null;
            }
            if (this.form_0 != null)
            {
                this.form_0.Deactivate -= this.form_0_Deactivate;
            }
            Application.RemoveMessageFilter(this);
            this.form_0 = null;
            this.control_0 = null;
        }

        private void form_0_Deactivate(object sender, EventArgs e)
        {
            Cancel();
        }

        private void method_0(object sender, EventArgs e)
        {
            Cancel();
        }

        protected void method_1(Rectangle rectangle_1, bool bool_2)
        {
            if (this.rectangle_0 == rectangle_1)
            {
                return;
            }
            if (this.dockingHints_0 == DockingHints.RubberBand)
            {
                this.method_3();
            }
            if (this.dockingHints_0 == DockingHints.RubberBand)
            {
                if (this.bool_1)
                {
                    Native.smethod_0(null, rectangle_1, bool_2, this._tabStripSize);
                }
                else
                {
                    Native.smethod_1(null, rectangle_1);
                }
                this.rectangle_0 = rectangle_1;
                this.bool_0 = bool_2;
                return;
            }
            this.form1_0.method_0(rectangle_1, bool_2);
        }

        protected void method_2()
        {
            if (this.dockingHints_0 == DockingHints.RubberBand)
            {
                this.method_3();
                return;
            }
            this.form1_0.Hide();
        }

        private void method_3()
        {
            if (this.rectangle_0 != Rectangle.Empty)
            {
                if (!this.bool_1)
                {
                    Native.smethod_1(null, this.rectangle_0);
                }
                else
                {
                    Native.smethod_0(null, this.rectangle_0, this.bool_0, this._tabStripSize);
                }
            }
            this.rectangle_0 = Rectangle.Empty;
        }

        private void method_4()
        {
            if (this.dockingHints_0 == DockingHints.RubberBand)
            {
                this.method_3();
            }
        }

        public abstract void OnMouseMove(Point position);

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 15)
            {
                this.method_4();
            }
            if (m.Msg == 256 || m.Msg == 257)
            {
                if (m.WParam.ToInt32() == 17)
                {
                    this.OnMouseMove(Cursor.Position);
                    return false;
                }
            }
            if (m.Msg == 256 || m.Msg == 257)
            {
                if (m.WParam.ToInt32() == 16)
                {
                    return true;
                }
            }
            if (m.Msg != 260)
            {
                if (m.Msg != 261)
                {
                    goto IL_AB;
                }
            }
            if (m.WParam.ToInt32() == 18)
            {
                return true;
            }
            IL_AB:
            if (m.Msg < 256 || m.Msg > 264)
            {
                return false;
            }
            this.Cancel();
            return true;
        }

        internal static bool smethod_0()
        {
            bool result = false;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                result = (Environment.OSVersion.Version >= new Version(5, 0, 0, 0));
            }
            return result;
        }

        public event EventHandler Event_0;

        private bool bool_0;

        private bool bool_1;

        private Control control_0;

        private DockingHints dockingHints_0 = DockingHints.TranslucentFill;

        //private EventHandler eventHandler_0;

        private Form1 form1_0;

        private Form form_0;

        private const int int_0 = 256;

        private const int int_1 = 257;

        private const int int_2 = 260;

        private const int int_3 = 261;

        private const int int_4 = 15;

        private readonly int _tabStripSize = 21;

        private Rectangle rectangle_0 = Rectangle.Empty;
    }

    internal class Class7 : Class6
    {
        public Class7(SandDockManager manager, DockContainer container, LayoutSystemBase sourceControlSystem, DockControl sourceControl, int dockedSize, Point startPoint, DockingHints dockingHints) : base(container, dockingHints, true, container.Renderer.TabStripMetrics.Height)
        {
            Manager = manager;
            this.DockContainer_0 = container;
            this.LayoutSystemBase_0 = sourceControlSystem;
            this.DockControl_0 = sourceControl;
            this.Int32_0 = dockedSize;
            if (container is DocumentContainer)
            {
                this.cursor_0 = new Cursor(GetType().Assembly.GetManifestResourceStream("TD.SandDock.Resources.splitting.cur"));
                this.cursor_1 = new Cursor(GetType().Assembly.GetManifestResourceStream("TD.SandDock.Resources.splittingno.cur"));
            }
            if (sourceControlSystem is SplitLayoutSystem)
            {
                this.size_0 = ((FloatingContainer)container).Size_0;
            }
            else if (sourceControl == null)
            {
                if (!(sourceControlSystem is ControlLayoutSystem) || ((ControlLayoutSystem)sourceControlSystem).SelectedControl == null)
                {
                    this.size_0 = sourceControlSystem.Bounds.Size;
                }
                else
                {
                    this.size_0 = ((ControlLayoutSystem)sourceControlSystem).SelectedControl.FloatingSize;
                }
            }
            else
            {
                this.size_0 = sourceControl.FloatingSize;
            }
            Rectangle bounds = sourceControlSystem.Bounds;
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
            this.DockContainer_0.OnDockingStarted(EventArgs.Empty);
        }

        public override void Commit()
        {
            base.Commit();
            LayoutUtilities.smethod_0();
            try
            {
                DockingManagerFinished?.Invoke(this.DockTarget_0);
            }
            finally
            {
                LayoutUtilities.smethod_1();
            }
        }

        public override void Dispose()
        {
            this.DockContainer_0.OnDockingFinished(EventArgs.Empty);
            this.cursor_0?.Dispose();
            this.cursor_1?.Dispose();
            base.Dispose();
        }

        protected virtual DockTarget FindDockTarget(Point position)
        {
            if (Manager != null && this.AllowFloat)
            {
                foreach (DockContainer dockContainer in Manager.arrayList_0)
                {
                    if (dockContainer.IsFloating && ((FloatingContainer)dockContainer).Form_0.Visible && ((FloatingContainer)dockContainer).HasSingleControlLayoutSystem)
                    {
                        if (dockContainer.LayoutSystem != this.LayoutSystemBase_0)
                        {
                            if (((FloatingContainer)dockContainer).Rectangle_1.Contains(position) && !new Rectangle(dockContainer.PointToScreen(dockContainer.LayoutSystem.LayoutSystems[0].Bounds.Location), dockContainer.LayoutSystem.LayoutSystems[0].Bounds.Size).Contains(position))
                            {
                                Class7.DockTarget result = new Class7.DockTarget(Class7.DockTargetType.JoinExistingSystem)
                                {
                                    dockContainer = dockContainer,
                                    layoutSystem = (ControlLayoutSystem)dockContainer.LayoutSystem.LayoutSystems[0],
                                    bounds = ((FloatingContainer)dockContainer).Rectangle_1
                                };
                                return result;
                            }
                        }
                    }
                }
            }
            ControlLayoutSystem[] array = this.ControlLayoutSystem_0;
            for (int i = 0; i < array.Length; i++)
            {
                ControlLayoutSystem controlLayoutSystem = array[i];
                if (new Rectangle(controlLayoutSystem.DockContainer.PointToScreen(controlLayoutSystem.Bounds.Location), controlLayoutSystem.Bounds.Size).Contains(position))
                {
                    Class7.DockTarget dockTarget = this.method_13(controlLayoutSystem.DockContainer, controlLayoutSystem, position, true);
                    if (dockTarget != null)
                    {
                        Class7.DockTarget result = dockTarget;
                        return result;
                    }
                }
            }
            if (this.Manager != null)
            {
                for (int j = 1; j <= 4; j++)
                {
                    ContainerDockLocation containerDockLocation = (ContainerDockLocation)j;
                    if (this.method_5(containerDockLocation))
                    {
                        if (Class7.smethod_2(this.method_7(containerDockLocation, true), this.Manager.DockSystemContainer).Contains(position))
                        {
                            return new Class7.DockTarget(Class7.DockTargetType.CreateNewContainer)
                            {
                                dockLocation = containerDockLocation,
                                bounds = Class7.smethod_2(this.method_8(containerDockLocation, this.Int32_0, true), this.Manager.DockSystemContainer),
                                middle = true
                            };
                        }
                        if (Class7.smethod_2(this.method_7(containerDockLocation, false), this.Manager.DockSystemContainer).Contains(position))
                        {
                            return new Class7.DockTarget(Class7.DockTargetType.CreateNewContainer)
                            {
                                dockLocation = containerDockLocation,
                                bounds = Class7.smethod_2(this.method_8(containerDockLocation, this.Int32_0, false), this.Manager.DockSystemContainer)
                            };
                        }
                    }
                }
            }
            return null;
        }

        private ControlLayoutSystem[] method_10()
        {
            ArrayList arrayList = new ArrayList();
            DockContainer[] array;
            array = Manager != null ? this.Manager.GetDockContainers() : new[] { this.DockContainer_0 };
            DockContainer[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                DockContainer dockContainer = array2[i];
                bool isFloating = dockContainer.IsFloating;
                bool flag = dockContainer.Dock == DockStyle.Fill && !dockContainer.IsFloating;
                bool flag2 = this.DockContainer_0.Dock == DockStyle.Fill && !this.DockContainer_0.IsFloating;
                if ((!isFloating || this.LayoutSystemBase_0.DockContainer != dockContainer || !(this.LayoutSystemBase_0 is SplitLayoutSystem)) && (!isFloating || this.AllowFloat || this.LayoutSystemBase_0.DockContainer == dockContainer) && (isFloating || this.method_5(LayoutUtilities.smethod_7(dockContainer.Dock))) && (!flag || flag2))
                {
                    if (!flag2 || this.DockContainer_0 == dockContainer)
                    {
                        this.method_11(dockContainer, arrayList);
                    }
                }
            }
            ControlLayoutSystem[] array3 = new ControlLayoutSystem[arrayList.Count];
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
                        if (this.DockControl_0 == null || layoutSystemBase != this.LayoutSystemBase_0)
                        {
                            goto IL_59;
                        }
                        if (this.DockControl_0.LayoutSystem.Controls.Count != 1)
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

        protected Class7.DockTarget method_13(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Point point_1, bool bool_2)
        {
            Class7.DockTarget dockTarget = new Class7.DockTarget(Class7.DockTargetType.Undefined);
            Point point = dockContainer_1.PointToClient(point_1);
            if (this.DockControl_0 != null || controlLayoutSystem_1 != this.LayoutSystemBase_0)
            {
                if (controlLayoutSystem_1.Rectangle_0.Contains(point) || controlLayoutSystem_1.rectangle_2.Contains(point))
                {
                    dockTarget = new Class7.DockTarget(Class7.DockTargetType.JoinExistingSystem);
                    dockTarget.dockContainer = dockContainer_1;
                    dockTarget.layoutSystem = controlLayoutSystem_1;
                    dockTarget.dockSide = DockSide.None;
                    dockTarget.bounds = new Rectangle(dockContainer_1.PointToScreen(controlLayoutSystem_1.Bounds.Location), controlLayoutSystem_1.Bounds.Size);
                    if (!controlLayoutSystem_1.rectangle_2.Contains(point))
                    {
                        dockTarget.index = controlLayoutSystem_1.Controls.Count;
                    }
                    else
                    {
                        dockTarget.index = controlLayoutSystem_1.method_15(point);
                    }
                }
                if (dockTarget.type == Class7.DockTargetType.Undefined && bool_2)
                {
                    dockTarget = this.method_14(dockContainer_1, controlLayoutSystem_1, point_1);
                }
                return dockTarget;
            }
            if (controlLayoutSystem_1.Rectangle_0.Contains(point))
            {
                return new Class7.DockTarget(Class7.DockTargetType.None);
            }
            return new Class7.DockTarget(Class7.DockTargetType.Undefined);
        }

        private Class7.DockTarget method_14(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Point point_1)
        {
            Class7.DockTarget dockTarget = null;
            Point point = dockContainer_1.PointToClient(point_1);
            Rectangle rectangle_ = controlLayoutSystem_1.rectangle_3;
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
            if (point_1.Y <= rectangle_1.Top + (int)((float)rectangle_1.Height * ((float)point_1.X / (float)rectangle_1.Width)))
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Right);
                return;
            }
            this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Bottom);
        }

        private void method_16(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            rectangle_1.Y = rectangle_1.Bottom - 30;
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            if (point_1.Y <= rectangle_1.Bottom - (int)((float)rectangle_1.Height * ((float)point_1.X / (float)rectangle_1.Width)))
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Left);
                return;
            }
            this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Bottom);
        }

        private void method_17(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            rectangle_1.X = rectangle_1.Right - 30;
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            if (point_1.Y > rectangle_1.Top + (int)((float)rectangle_1.Height * ((float)(rectangle_1.Right - point_1.X) / (float)rectangle_1.Width)))
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Right);
                return;
            }
            this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Top);
        }

        private void method_18(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
        {
            point_1.X -= rectangle_1.Left;
            point_1.Y -= rectangle_1.Top;
            rectangle_1 = new Rectangle(0, 0, 30, 30);
            if (point_1.Y > rectangle_1.Top + (int)((float)rectangle_1.Height * ((float)point_1.X / (float)rectangle_1.Width)))
            {
                this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Left);
                return;
            }
            this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Top);
        }

        private void method_19(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, DockSide dockSide_0)
        {
            dockTarget_1.bounds = this.method_20(dockContainer_1, controlLayoutSystem_1, dockSide_0);
            dockTarget_1.dockSide = dockSide_0;
        }

        internal Rectangle method_20(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, DockSide dockSide_0)
        {
            Rectangle result = new Rectangle(dockContainer_1.PointToScreen(controlLayoutSystem_1.Bounds.Location), controlLayoutSystem_1.Bounds.Size);
            switch (dockSide_0)
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

        private DockTarget method_21(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1)
        {
            return new DockTarget(DockTargetType.SplitExistingSystem)
            {
                dockContainer = dockContainer_1,
                layoutSystem = controlLayoutSystem_1
            };
        }

        public bool method_5(ContainerDockLocation containerDockLocation_0)
        {
            return this.DockControl_0?.method_13(containerDockLocation_0) ?? this.LayoutSystemBase_0.vmethod_3(containerDockLocation_0);
        }

        private Rectangle method_6(Rectangle rectangle_1)
        {
            if (rectangle_1.X >= Screen.PrimaryScreen.Bounds.X && rectangle_1.Right <= Screen.PrimaryScreen.Bounds.Right && rectangle_1.Y > Screen.PrimaryScreen.WorkingArea.Bottom)
            {
                rectangle_1.Y = Screen.PrimaryScreen.WorkingArea.Bottom - rectangle_1.Height;
            }
            Screen screen = Screen.FromRectangle(rectangle_1);
            if (screen != null && rectangle_1.Y < screen.WorkingArea.Y)
            {
                rectangle_1.Y = screen.WorkingArea.Y;
            }
            return rectangle_1;
        }

        protected Rectangle method_7(ContainerDockLocation containerDockLocation_0, bool bool_2)
        {
            if (bool_2)
            {
                return this.method_8(containerDockLocation_0, 30, true);
            }
            Control dockSystemContainer = this.Manager.DockSystemContainer;
            int num = 0;
            int width = dockSystemContainer.ClientRectangle.Width;
            int num2 = 0;
            int height = dockSystemContainer.ClientRectangle.Height;
            switch (containerDockLocation_0)
            {
                case ContainerDockLocation.Left:
                    return new Rectangle(num - 30, num2, 30, height - num2);
                case ContainerDockLocation.Right:
                    return new Rectangle(width, num2, 30, height - num2);
                case ContainerDockLocation.Top:
                    return new Rectangle(num, num2 - 30, width - num, 30);
                case ContainerDockLocation.Bottom:
                    return new Rectangle(num, height, width - num, 30);
                default:
                    return Rectangle.Empty;
            }
        }

        protected Rectangle method_8(ContainerDockLocation containerDockLocation_0, int int_8, bool bool_2)
        {
            Rectangle rectangle = Class7.smethod_1(this.Manager.DockSystemContainer);
            Rectangle result = rectangle;
            if (!bool_2)
            {
                result = this.Manager.DockSystemContainer.ClientRectangle;
            }
            int val = int_8 + 4;
            switch (containerDockLocation_0)
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
                if (this.Manager != null && this.AllowFloat)
                {
                    dockTarget = new Class7.DockTarget(Class7.DockTargetType.Float);
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
                dockTarget.bounds = new Rectangle(this.Point_0, this.size_0);
                dockTarget.bounds = this.method_6(dockTarget.bounds);
            }
            if (dockTarget.layoutSystem == this.LayoutSystemBase_0 && this.DockControl_0 != null)
            {
                if (dockTarget.dockSide == DockSide.None)
                {
                    base.method_2();
                    ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)this.LayoutSystemBase_0;
                    if (dockTarget.index != controlLayoutSystem.Controls.IndexOf(this.DockControl_0))
                    {
                        if (dockTarget.index != controlLayoutSystem.Controls.IndexOf(this.DockControl_0) + 1)
                        {
                            controlLayoutSystem.Controls.SetChildIndex(this.DockControl_0, dockTarget.index);
                        }
                    }
                    dockTarget.type = Class7.DockTargetType.AlreadyActioned;
                    goto IL_147;
                }
            }
            if (dockTarget.type != Class7.DockTargetType.None)
            {
                base.method_1(dockTarget.bounds, dockTarget.type == Class7.DockTargetType.JoinExistingSystem);
            }
            else
            {
                base.method_2();
            }
            IL_147:
            if (this.DockContainer_0 is DocumentContainer)
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
            this.DockTarget_0 = dockTarget;
        }

        public static Rectangle smethod_1(Control control_1)
        {
            int num = 0;
            int num2 = control_1.ClientRectangle.Width;
            int num3 = 0;
            int num4 = control_1.ClientRectangle.Height;
            foreach (Control control in control_1.Controls)
            {
                if (control.Visible)
                {
                    switch (control.Dock)
                    {
                        case DockStyle.Top:
                            if (control.Bounds.Bottom > num3)
                            {
                                num3 = control.Bounds.Bottom;
                            }
                            break;
                        case DockStyle.Bottom:
                            if (control.Bounds.Top < num4)
                            {
                                num4 = control.Bounds.Top;
                            }
                            break;
                        case DockStyle.Left:
                            if (control.Bounds.Right > num)
                            {
                                num = control.Bounds.Right;
                            }
                            break;
                        case DockStyle.Right:
                            if (control.Bounds.Left < num2)
                            {
                                num2 = control.Bounds.Left;
                            }
                            break;
                    }
                }
            }
            return new Rectangle(num, num3, num2 - num, num4 - num3);
        }

        public static Rectangle smethod_2(Rectangle rectangle_1, Control control_1)
        {
            return new Rectangle(control_1.PointToScreen(rectangle_1.Location), rectangle_1.Size);
        }

        public bool Boolean_0 => this.DockContainer_0.Boolean_0;

        public bool AllowFloat
        {
            get
            {
                if (this.Boolean_0)
                    return false;
                return DockControl_0?.DockingRules.AllowFloat ?? LayoutSystemBase_0.AllowFloat;
            }
        }

        protected ControlLayoutSystem[] ControlLayoutSystem_0 { get; }

        public DockContainer DockContainer_0 { get; }

        public DockControl DockControl_0 { get; }

        public DockTarget DockTarget_0 { get; private set; }

        public int Int32_0 { get; }

        public LayoutSystemBase LayoutSystemBase_0 { get; }

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

            public Rectangle bounds = Rectangle.Empty;

            public DockContainer dockContainer;

            public ContainerDockLocation dockLocation = ContainerDockLocation.Center;

            public DockSide dockSide = DockSide.None;

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
            if (base.Manager != null && base.Manager.DockSystemContainer != null)
            {
                this.method_22();
            }
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
            DockTarget dockTarget = null;
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
                if (form.DockStyle_0 != DockStyle.Fill)
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
                if (form.DockStyle_0 == DockStyle.Fill)
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
            ControlLayoutSystem controlLayoutSystem = this.method_23(position, out dockTarget);
            if (controlLayoutSystem == base.LayoutSystemBase_0 && base.DockControl_0 == null)
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
            if (dockTarget != null && dockTarget.type == Class7.DockTargetType.Undefined)
            {
                dockTarget = null;
            }
            if (this.form4_0 != null && this.form4_0.Rectangle_5.Contains(position) && dockTarget == null)
            {
                dockTarget = this.form4_0.method_4(position);
            }
            object[] array2 = this.arrayList_0.ToArray();
            for (int j = 0; j < array2.Length; j++)
            {
                Class8.Form4 form2 = (Class8.Form4)array2[j];
                if (dockTarget == null && form2.Rectangle_5.Contains(position))
                {
                    dockTarget = form2.method_4(position);
                }
            }
            return dockTarget;
        }

        private void method_22()
        {
            this.rectangle_1 = Class7.smethod_2(base.Manager.DockSystemContainer.ClientRectangle, base.Manager.DockSystemContainer);
            this.rectangle_2 = Class7.smethod_2(Class7.smethod_1(base.Manager.DockSystemContainer), base.Manager.DockSystemContainer);
            if (base.method_5(ContainerDockLocation.Top))
            {
                this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_1, DockStyle.Top));
            }
            if (base.method_5(ContainerDockLocation.Left))
            {
                this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_1, DockStyle.Left));
            }
            if (base.method_5(ContainerDockLocation.Bottom))
            {
                this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_1, DockStyle.Bottom));
            }
            if (base.method_5(ContainerDockLocation.Right))
            {
                this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_1, DockStyle.Right));
            }
            bool flag = base.DockContainer_0.Dock == DockStyle.Fill && !base.DockContainer_0.IsFloating;
            bool flag2 = base.method_5(ContainerDockLocation.Left) || base.method_5(ContainerDockLocation.Right) || base.method_5(ContainerDockLocation.Top) || base.method_5(ContainerDockLocation.Bottom);
            if (!flag && (base.method_5(ContainerDockLocation.Center) || flag2))
            {
                this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_2, DockStyle.Fill));
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
                for (int j = 0; j < controlLayoutSystem_.Length; j++)
                {
                    ControlLayoutSystem controlLayoutSystem = controlLayoutSystem_[j];
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

        private Class8.Form4 form4_0;

        private Rectangle rectangle_1;

        private Rectangle rectangle_2;

        private class Form4 : Form3
        {
            private Form4()
            {
                base.FormBorderStyle = FormBorderStyle.None;
                base.ShowInTaskbar = false;
                base.StartPosition = FormStartPosition.Manual;
                this.timer_0 = new Timer {Interval = 50};
                this.timer_0.Tick += this.timer_0_Tick;
                this.bitmap_0 = new Bitmap(88, 88, PixelFormat.Format32bppArgb);
            }

            public Form4(Class8 manager, ControlLayoutSystem layoutSystem) : this()
            {
                this.class8_0 = manager;
                this.controlLayoutSystem_0 = layoutSystem;
                this.rectangle_0 = new Rectangle(layoutSystem.DockContainer.PointToScreen(layoutSystem.Bounds.Location), layoutSystem.Bounds.Size);
                this.rectangle_0 = new Rectangle(this.rectangle_0.X + this.rectangle_0.Width / 2 - 44, this.rectangle_0.Y + this.rectangle_0.Height / 2 - 44, 88, 88);
                this.method_1();
            }

            public Form4(Class8 manager, Rectangle fc, DockStyle dockStyle) : this()
            {
                this.class8_0 = manager;
                this.DockStyle_0 = dockStyle;
                switch (dockStyle)
                {
                    case DockStyle.Top:
                        this.rectangle_0 = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Y + 15, 88, 88);
                        break;
                    case DockStyle.Bottom:
                        this.rectangle_0 = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Bottom - 88 - 15, 88, 88);
                        break;
                    case DockStyle.Left:
                        this.rectangle_0 = new Rectangle(fc.X + 15, fc.Y + fc.Height / 2 - 44, 88, 88);
                        break;
                    case DockStyle.Right:
                        this.rectangle_0 = new Rectangle(fc.Right - 88 - 15, fc.Y + fc.Height / 2 - 44, 88, 88);
                        break;
                    case DockStyle.Fill:
                        this.rectangle_0 = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Y + fc.Height / 2 - 44, 88, 88);
                        break;
                }
                this.method_1();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    this.bitmap_0.Dispose();
                    this.timer_0.Tick -= this.timer_0_Tick;
                    this.timer_0.Dispose();
                }
                base.Dispose(disposing);
            }

            private void method_1()
            {
                using (Graphics graphics = Graphics.FromImage(this.bitmap_0))
                {
                    Class16.smethod_0(graphics, Color.Transparent);
                    if (this.DockStyle_0 != DockStyle.None && this.DockStyle_0 != DockStyle.Fill)
                    {
                        if (this.DockStyle_0 == DockStyle.Top)
                        {
                            using (Image image = Image.FromStream(typeof(Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghinttop.png")))
                            {
                                graphics.DrawImageUnscaled(image, 29, 0);
                                goto IL_170;
                            }
                        }
                        if (this.DockStyle_0 != DockStyle.Bottom)
                        {
                            if (this.DockStyle_0 != DockStyle.Left)
                            {
                                if (this.DockStyle_0 != DockStyle.Right)
                                {
                                    goto IL_170;
                                }
                                using (Image image2 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintright.png")))
                                {
                                    graphics.DrawImageUnscaled(image2, 57, 29);
                                    goto IL_170;
                                }
                            }
                            using (Image image3 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintleft.png")))
                            {
                                graphics.DrawImageUnscaled(image3, 0, 29);
                                goto IL_170;
                            }
                        }
                        using (Image image4 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintbottom.png")))
                        {
                            graphics.DrawImageUnscaled(image4, 29, 57);
                            goto IL_170;
                        }
                    }
                    using (Image image5 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintcenter.png")))
                    {
                        graphics.DrawImageUnscaled(image5, 0, 0);
                    }
                    IL_170:
                    Color highlight = SystemColors.Highlight;
                    Color transparent = Color.Transparent;
                    if (this.DockStyle_0 == DockStyle.None || this.DockStyle_0 == DockStyle.Fill || this.DockStyle_0 == DockStyle.Top)
                    {
                        this.method_10(graphics, (!this.bool_0 || this.dockSide_0 != DockSide.Top) ? transparent : highlight);
                    }
                    if (this.DockStyle_0 != DockStyle.None)
                    {
                        if (this.DockStyle_0 != DockStyle.Fill)
                        {
                            if (this.DockStyle_0 != DockStyle.Right)
                            {
                                goto IL_1F3;
                            }
                        }
                    }
                    Graphics arg_1EE_1 = graphics;
                    Color arg_1EE_2;
                    if (this.bool_0)
                    {
                        if (this.dockSide_0 == DockSide.Right)
                        {
                            arg_1EE_2 = highlight;
                            goto IL_1EE;
                        }
                    }
                    arg_1EE_2 = transparent;
                    IL_1EE:
                    this.method_9(arg_1EE_1, arg_1EE_2);
                    IL_1F3:
                    if (this.DockStyle_0 == DockStyle.None || this.DockStyle_0 == DockStyle.Fill || this.DockStyle_0 == DockStyle.Bottom)
                    {
                        Graphics arg_22A_1 = graphics;
                        Color arg_22A_2;
                        if (this.bool_0)
                        {
                            if (this.dockSide_0 == DockSide.Bottom)
                            {
                                arg_22A_2 = highlight;
                                goto IL_22A;
                            }
                        }
                        arg_22A_2 = transparent;
                        IL_22A:
                        this.method_8(arg_22A_1, arg_22A_2);
                    }
                    if (this.DockStyle_0 != DockStyle.None)
                    {
                        if (this.DockStyle_0 != DockStyle.Fill)
                        {
                            if (this.DockStyle_0 != DockStyle.Left)
                            {
                                goto IL_26B;
                            }
                        }
                    }
                    Graphics arg_266_1 = graphics;
                    Color arg_266_2;
                    if (this.bool_0)
                    {
                        if (this.dockSide_0 == DockSide.Left)
                        {
                            arg_266_2 = highlight;
                            goto IL_266;
                        }
                    }
                    arg_266_2 = transparent;
                    IL_266:
                    this.method_7(arg_266_1, arg_266_2);
                    IL_26B:
                    if (this.DockStyle_0 == DockStyle.None || this.DockStyle_0 == DockStyle.Fill)
                    {
                        Graphics arg_299_1 = graphics;
                        Color arg_299_2;
                        if (this.bool_0)
                        {
                            if (this.dockSide_0 == DockSide.None)
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
                base.method_0(this.bitmap_0, 255);
            }

            private void method_10(Graphics graphics_0, Color color_0)
            {
                using (Pen pen = new Pen(color_0))
                {
                    graphics_0.DrawLine(pen, 29, 0, 57, 0);
                    graphics_0.DrawLine(pen, 57, 0, 57, 23);
                    graphics_0.DrawLine(pen, 29, 0, 29, 23);
                }
            }

            public void method_11()
            {
                this.bool_2 = true;
                this.method_12();
            }

            public void method_12()
            {
                if (base.Visible || (!this.bool_1 && this.timer_0.Enabled))
                {
                    this.int_5 = Environment.TickCount;
                    this.bool_1 = true;
                    this.timer_0.Start();
                }
            }

            public void method_13()
            {
                base.method_0(this.bitmap_0, 0);
                this.method_14();
                this.int_5 = Environment.TickCount;
                this.bool_1 = false;
                this.timer_0.Start();
            }

            private void method_14()
            {
                Native.SetWindowPos(Handle, new IntPtr(-1), this.rectangle_0.X, this.rectangle_0.Y, this.rectangle_0.Width, this.rectangle_0.Height, 80);

            }

            private DockTarget method_2(Point point_0)
            {
                DockTarget dockTarget = new DockTarget(DockTargetType.SplitExistingSystem)
                {
                    layoutSystem = this.controlLayoutSystem_0,
                    dockContainer = this.controlLayoutSystem_0.DockContainer
                };
                if (this.method_5(this.Rectangle_1, point_0))
                {
                    dockTarget.dockSide = DockSide.Top;
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
                                dockTarget.dockSide = DockSide.None;
                            }
                        }
                        else
                        {
                            dockTarget.dockSide = DockSide.Left;
                        }
                    }
                    else
                    {
                        dockTarget.dockSide = DockSide.Bottom;
                    }
                }
                else
                {
                    dockTarget.dockSide = DockSide.Right;
                }
                dockTarget.bounds = this.class8_0.method_20(this.controlLayoutSystem_0.DockContainer, this.controlLayoutSystem_0, dockTarget.dockSide);
                return dockTarget;
            }

            private Class7.DockTarget method_3(Point point_0)
            {
                Class7.DockTarget dockTarget = new Class7.DockTarget(Class7.DockTargetType.SplitExistingSystem);
                dockTarget.layoutSystem = this.controlLayoutSystem_0;
                dockTarget.dockContainer = ((this.controlLayoutSystem_0 != null) ? this.controlLayoutSystem_0.DockContainer : null);
                if (this.method_5(this.Rectangle_1, point_0) && this.class8_0.method_5(ContainerDockLocation.Top))
                {
                    if (this.DockStyle_0 != DockStyle.Top)
                    {
                        if (this.DockStyle_0 != DockStyle.Fill)
                        {
                            goto IL_75;
                        }
                    }
                    dockTarget.dockLocation = ContainerDockLocation.Top;
                    dockTarget.dockSide = DockSide.Top;
                    goto IL_178;
                }
                IL_75:
                if (this.method_5(this.Rectangle_2, point_0) && this.class8_0.method_5(ContainerDockLocation.Right))
                {
                    if (this.DockStyle_0 != DockStyle.Right)
                    {
                        if (this.DockStyle_0 != DockStyle.Fill)
                        {
                            goto IL_B9;
                        }
                    }
                    dockTarget.dockLocation = ContainerDockLocation.Right;
                    dockTarget.dockSide = DockSide.Right;
                    goto IL_178;
                }
                IL_B9:
                if (this.method_5(this.Rectangle_3, point_0) && this.class8_0.method_5(ContainerDockLocation.Bottom))
                {
                    if (this.DockStyle_0 == DockStyle.Bottom || this.DockStyle_0 == DockStyle.Fill)
                    {
                        dockTarget.dockLocation = ContainerDockLocation.Bottom;
                        dockTarget.dockSide = DockSide.Bottom;
                        goto IL_178;
                    }
                }
                if (!this.method_5(this.Rectangle_4, point_0) || !this.class8_0.method_5(ContainerDockLocation.Left) || (this.DockStyle_0 != DockStyle.Left && this.DockStyle_0 != DockStyle.Fill))
                {
                    if (this.method_5(this.Rectangle_0, point_0) && this.class8_0.method_5(ContainerDockLocation.Center))
                    {
                        if (this.DockStyle_0 == DockStyle.Fill)
                        {
                            dockTarget.dockLocation = ContainerDockLocation.Center;
                            dockTarget.dockSide = DockSide.None;
                            goto IL_178;
                        }
                    }
                    dockTarget.type = Class7.DockTargetType.Undefined;
                }
                else
                {
                    dockTarget.dockLocation = ContainerDockLocation.Left;
                    dockTarget.dockSide = DockSide.Left;
                }
                IL_178:
                if (dockTarget.type != Class7.DockTargetType.Undefined)
                {
                    dockTarget.type = Class7.DockTargetType.CreateNewContainer;
                    dockTarget.middle = (this.DockStyle_0 == DockStyle.Fill);
                    dockTarget.bounds = Class7.smethod_2(this.class8_0.method_8(dockTarget.dockLocation, this.class8_0.Int32_0, dockTarget.middle), this.class8_0.Manager.DockSystemContainer);
                }
                return dockTarget;
            }

            public Class7.DockTarget method_4(Point point_0)
            {
                Point point_ = base.PointToClient(point_0);
                Class7.DockTarget dockTarget;
                if (this.controlLayoutSystem_0 != null)
                {
                    dockTarget = this.method_2(point_);
                }
                else
                {
                    dockTarget = this.method_3(point_);
                }
                bool flag = dockTarget.type != Class7.DockTargetType.Undefined;
                DockSide dockSide = (dockTarget.type == Class7.DockTargetType.Undefined) ? this.dockSide_0 : dockTarget.dockSide;
                if (flag != this.bool_0 || dockSide != this.dockSide_0)
                {
                    this.bool_0 = flag;
                    this.dockSide_0 = dockSide;
                    this.method_1();
                }
                return dockTarget;
            }

            private bool method_5(Rectangle rectangle_1, Point point_0)
            {
                return rectangle_1.Contains(point_0);
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

            private void timer_0_Tick(object sender, EventArgs e)
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
                base.method_0(this.bitmap_0, (byte)num2);
                if (num >= 200)
                {
                    this.timer_0.Stop();
                    base.Visible = !this.bool_1;
                    if (this.bool_2)
                    {
                        base.Dispose();
                    }
                }
            }

            public DockStyle DockStyle_0 { get; }

            private Rectangle Rectangle_0 => new Rectangle(28, 28, 32, 32);

            private Rectangle Rectangle_1 => new Rectangle(29, 0, 29, 28);

            private Rectangle Rectangle_2 => new Rectangle(60, 29, 28, 29);

            private Rectangle Rectangle_3 => new Rectangle(29, 60, 29, 28);

            private Rectangle Rectangle_4 => new Rectangle(0, 29, 28, 29);

            public Rectangle Rectangle_5 => this.rectangle_0;

            private Bitmap bitmap_0;

            private bool bool_0;

            private bool bool_1;

            private bool bool_2;

            private Class8 class8_0;

            private ControlLayoutSystem controlLayoutSystem_0;

            private DockSide dockSide_0 = DockSide.None;

            private const int int_0 = 88;

            private const int int_1 = 88;

            private const int int_2 = 200;

            private const int int_3 = 16;

            private const int int_4 = 64;

            private int int_5;

            private Rectangle rectangle_0 = Rectangle.Empty;

            private Timer timer_0;
        }
    }

    internal class ResizingManager : Class6
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

    internal class SplittingManager : Class6
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

        //private Class10.SplittingManagerFinishedEventHandler splittingManagerFinishedEventHandler_0;

        public delegate void SplittingManagerFinishedEventHandler(LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, float aboveSize, float belowSize);
    }

    internal class Class11 : Class6
    {
        public Class11(SandDockManager manager, DockContainer container, Point startPoint) : base(container, manager.DockingHints, false)
        {
            this.container = container;
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
            this.OnMouseMove(startPoint);
        }

        public override void Commit()
        {
            base.Commit();
            ResizingManagerFinished?.Invoke(this.int_8);
        }

        public override void OnMouseMove(Point position)
        {
            Rectangle empty = Rectangle.Empty;
            if (this.container.Boolean_1)
            {
                empty = new Rectangle(position.X - this.int_9, 0, 4, this.container.Height);
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
                empty = new Rectangle(0, position.Y - this.int_9, this.container.Width, 4);
                if (empty.Y < this.int_6)
                {
                    empty.Y = this.int_6;
                }
                if (empty.Y > this.int_7 - 4)
                {
                    empty.Y = this.int_7 - 4;
                }
            }
            switch (this.container.Dock)
            {
                case DockStyle.Top:
                    this.int_8 = this.container.ContentSize + (empty.Y - this.container.Rectangle_0.Y);
                    break;
                case DockStyle.Bottom:
                    this.int_8 = this.container.ContentSize + (this.container.Rectangle_0.Y - empty.Y);
                    break;
                case DockStyle.Left:
                    this.int_8 = this.container.ContentSize + (empty.X - this.container.Rectangle_0.X);
                    break;
                case DockStyle.Right:
                    this.int_8 = this.container.ContentSize + (this.container.Rectangle_0.X - empty.X);
                    break;
            }
            base.method_1(new Rectangle(this.container.PointToScreen(empty.Location), empty.Size), false);
            Cursor.Current = this.container.Dock != DockStyle.Left && this.container.Dock != DockStyle.Right
                ? Cursors.HSplit : Cursors.VSplit;
        }

        public event ResizingManagerFinishedEventHandler ResizingManagerFinished;

        private DockContainer container;

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

		internal void method_0(SandDockManager sandDockManager_0, DockContainer dockContainer_1, LayoutSystemBase layoutSystemBase_0, DockControl dockControl_0, int int_2, Point point_0, DockingHints dockingHints_0, DockingManager dockingManager_0)
		{
			if (dockingManager_0 == DockingManager.Whidbey && Class6.smethod_0())
			{
				this.class7_0 = new Class8(sandDockManager_0, this.DockContainer, this, dockControl_0, int_2, point_0, dockingHints_0);
			}
			else
			{
				this.class7_0 = new Class7(sandDockManager_0, this.DockContainer, this, dockControl_0, int_2, point_0, dockingHints_0);
			}
			this.class7_0.DockingManagerFinished += this.vmethod_0;
			this.class7_0.Event_0 += this.vmethod_1;
			this.class7_0.OnMouseMove(Cursor.Position);
		}

		private void method_1()
		{
			this.class7_0.DockingManagerFinished -= this.vmethod_0;
			this.class7_0.Event_0 -= this.vmethod_1;
			this.class7_0 = null;
		}

		internal void method_2(SandDockManager sandDockManager_0, ContainerDockLocation containerDockLocation_0, ContainerDockEdge containerDockEdge_0)
		{
			DockControl[] dockControl_ = this.DockControl_0;
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
			ControlLayoutSystem controlLayoutSystem = dockContainer.CreateNewLayoutSystem(this.WorkingSize);
			dockContainer.LayoutSystem.LayoutSystems.Add(controlLayoutSystem);
			if (this is SplitLayoutSystem)
			{
				((SplitLayoutSystem)this).MoveToLayoutSystem(controlLayoutSystem);
				return;
			}
			controlLayoutSystem.Controls.AddRange(this.DockControl_0);
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

		internal virtual void vmethod_0(Class7.DockTarget dockTarget_0)
		{
			this.method_1();
		}

		internal virtual void vmethod_1(object sender, EventArgs e)
		{
			this.method_1();
		}

		internal virtual void vmethod_2(DockContainer dockContainer_1)
		{
			this.DockContainer = dockContainer_1;
		}

		internal abstract bool vmethod_3(ContainerDockLocation containerDockLocation_0);

		internal abstract void vmethod_4(RendererBase rendererBase_0, Graphics graphics_0, Font font_0);

		internal abstract bool PersistState
		{
			get;
		}

		internal abstract bool AllowFloat
		{
			get;
		}

		internal abstract bool AllowTab
		{
			get;
		}

		public Rectangle Bounds { get; private set; } = Rectangle.Empty;

	    public DockContainer DockContainer { get; private set; }

	    internal abstract DockControl[] DockControl_0{get;}

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
