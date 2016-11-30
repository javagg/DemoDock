using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using TD.SandDock.Design;
using TD.SandDock.Rendering;
using TD.Util;

namespace TD.SandDock
{
    public enum WindowOpenMethod
    {
        OnScreen = 1,
        OnScreenSelect,
        OnScreenActivate
    }
    public enum ContainerDockLocation
    {
        Left = 1,
        Right,
        Top,
        Bottom,
        Center
    }
    public enum ContainerDockEdge
    {
        Inside,
        Outside
    }
    public enum DockingManager
    {
        Standard,
        Whidbey
    }
    public enum DockingHints
    {
        RubberBand,
        TranslucentFill
    }
    public enum DockSide
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    }
    public enum DockSituation
    {
        None,
        Docked,
        Document,
        Floating
    }

    [Designer(typeof(DockContainerDesigner)), ToolboxItem(false)]
    public class DockContainer : ContainerControl
    {
        public DockContainer()
        {
            _splitLayout = new SplitLayoutSystem();
            _splitLayout.SetDockContainer(this);
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);
            layoutSystems = new ArrayList();
            _toolTips0 = new Tooltip(this) { DropShadow = false };
            _toolTips0.GetTooltipText += OnGetTooltipText;
            BackColor = SystemColors.Control;
        }

        public void CalculateAllMetricsAndLayout()
        {
            if (!IsHandleCreated) return;

            if (Capture && !IsFloating)
                Capture = false;

            rectangle_1 = DisplayRectangle;
            if (!AllowResize)
            {
                rectangle_0 = Rectangle.Empty;
            }
            else
            {
                switch (Dock)
                {
                    case DockStyle.Top:
                        rectangle_0 = new Rectangle(rectangle_1.Left, rectangle_1.Bottom - 4, rectangle_1.Width, 4);
                        rectangle_1.Height = rectangle_1.Height - 4;
                        break;
                    case DockStyle.Bottom:
                        rectangle_0 = new Rectangle(rectangle_1.Left, rectangle_1.Top, rectangle_1.Width, 4);
                        rectangle_1.Offset(0, 4);
                        rectangle_1.Height = rectangle_1.Height - 4;
                        break;
                    case DockStyle.Left:
                        rectangle_0 = new Rectangle(rectangle_1.Right - 4, rectangle_1.Top, 4, rectangle_1.Height);
                        rectangle_1.Width = rectangle_1.Width - 4;
                        break;
                    case DockStyle.Right:
                        rectangle_0 = new Rectangle(rectangle_1.Left, rectangle_1.Top, 4, rectangle_1.Height);
                        rectangle_1.Offset(4, 0);
                        rectangle_1.Width = rectangle_1.Width - 4;
                        break;
                    default:
                        rectangle_0 = Rectangle.Empty;
                        break;
                }
            }
            method_10(_splitLayout, rectangle_1);
            Invalidate();
        }

        public ControlLayoutSystem CreateNewLayoutSystem(SizeF size) => CreateNewLayoutSystem(new DockControl[0], size);

        public ControlLayoutSystem CreateNewLayoutSystem(DockControl control, SizeF size) => CreateNewLayoutSystem(new[] { control }, size);

        public ControlLayoutSystem CreateNewLayoutSystem(DockControl[] controls, SizeF size)
        {
            if (controls == null) throw new ArgumentNullException(nameof(controls));

            var layout = CreateNewControlLayoutSystem();
            layout.WorkingSize = size;
            if (controls.Length != 0)
                layout.Controls.AddRange(controls);
            return layout;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_renderer != null)
                {
                    _renderer.Dispose();
                    _renderer = null;
                }
                Manager = null;
                _toolTips0.GetTooltipText -= OnGetTooltipText;
                _toolTips0.Dispose();
            }
            base.Dispose(disposing);
        }

        public LayoutSystemBase GetLayoutSystemAt(Point position)
        {
           // layoutSystems.Cast<LayoutSystemBase>()
            LayoutSystemBase layoutSystemBase = null;
            foreach (LayoutSystemBase layoutSystemBase2 in layoutSystems)
            {
                if (!layoutSystemBase2.Bounds.Contains(position) ||
                    (layoutSystemBase2 is ControlLayoutSystem && ((ControlLayoutSystem) layoutSystemBase2).Collapsed))
                    continue;
                layoutSystemBase = layoutSystemBase2;
                if (layoutSystemBase is ControlLayoutSystem)
                {
                    break;
                }
            }
            return layoutSystemBase;
        }

        public DockControl GetWindowAt(Point position) => (GetLayoutSystemAt(position) as ControlLayoutSystem)?.GetControlAt(position);

        internal void ShowControlContextMenu(ShowControlContextMenuEventArgs e) => Manager?.OnShowControlContextMenu(e);

        internal object method_1(Type type)
        {
            return GetService(type);
        }

        internal void method_10(LayoutSystemBase layoutSystem, Rectangle bounds)
        {
            if (!IsHandleCreated) return;

            using (var g = CreateGraphics())
            {
                WorkingRenderer.StartRenderSession(HotkeyPrefix.None);
                layoutSystem.Layout(WorkingRenderer, g, bounds, layoutSystem == _splitLayout && IsFloating);
                WorkingRenderer.FinishRenderSession();
            }
        }

        internal void OnActivated(object sender, EventArgs e)
        {
            foreach (var controlLayoutSystem in layoutSystems.OfType<ControlLayoutSystem>().Where(controlLayoutSystem => controlLayoutSystem.OnActivated()))
            {
            }
        }

        internal void OnDeactivate(object sender, EventArgs e)
        {
            foreach (var layoutSystemBase in layoutSystems.OfType<ControlLayoutSystem>())
            {
                layoutSystemBase.OnDeactivate();
            }
        }

        private void method_13()
        {
            class11_0.Cancalled -= method_14;
            class11_0.Committed -= method_15;
            class11_0 = null;
        }

        private void method_14(object sender, EventArgs e)
        {
            method_13();
        }

        private void method_15(int size)
        {
            method_13();
            ContentSize = size;
        }

        private string OnGetTooltipText(Point point)
        {
            var layoutSystem = GetLayoutSystemAt(point) as ControlLayoutSystem;
            return layoutSystem == null ? "" : layoutSystem.GetDockButtonTextAt(point);
        }

        internal void method_2()
        {
            int_1++;
            _splitLayout.SetDockContainer(null);
            foreach (var layoutSystemBase in layoutSystems.OfType<ControlLayoutSystem>())
                layoutSystemBase.Controls.Clear();
            _splitLayout = new SplitLayoutSystem();
        }

        internal void method_3()
        {
            if (int_1 > 0)
            {
                int_1--;
            }
            if (int_1 == 0)
            {
                vmethod_2();
            }
        }

        internal void PropagateNewRenderer()
        {
            CalculateAllMetricsAndLayout();
        }

        private void method_5(bool bool_2)
        {
            layoutSystems.Clear();
            _layoutSystem = null;
            method_7(_splitLayout);
            if (!bool_2 && int_1 == 0)
            {
                method_9();
                Application.Idle += OnIdle;
            }
        }

        private void OnIdle(object sender, EventArgs e)
        {
            Application.Idle -= OnIdle;
            var flag = false;
            while (LayoutSystem.Optimize())
                flag = true;
            if (flag)
                method_5(true);
        }

        private void method_7(LayoutSystemBase layoutSystemBase_1)
        {
            layoutSystems.Add(layoutSystemBase_1);
            if (layoutSystemBase_1 is SplitLayoutSystem)
            {
                foreach (LayoutSystemBase layoutSystemBase_2 in ((SplitLayoutSystem)layoutSystemBase_1).LayoutSystems)
                {
                    method_7(layoutSystemBase_2);
                }
            }
        }

        internal bool method_8() => !layoutSystems.OfType<ControlLayoutSystem>().Any();

        internal void method_9()
        {
            if (CanShowCollapsed)
            {
                var allCollapsed = layoutSystems.OfType<ControlLayoutSystem>().All(controlLayoutSystem => controlLayoutSystem.Collapsed);
                int num = 0;
                if (!allCollapsed)
                    num += ContentSize + (AllowResize ? 4 : 0);
                if (Vertical && Width != num)
                {
                    Width = num;
                    return;
                }
                if (!Vertical && Height != num)
                {
                    Height = num;
                    return;
                }
            }
            CalculateAllMetricsAndLayout();
        }

        protected internal virtual void OnDockingFinished(EventArgs e)
        {
            DockingFinished?.Invoke(this, e);
            Manager?.OnDockingFinished(e);
        }

        protected internal virtual void OnDockingStarted(EventArgs e)
        {
            DockingStarted?.Invoke(this, e);
            Manager?.OnDockingStarted(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            _layoutSystem?.OnMouseDoubleClick();
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);
            var position = PointToClient(new Point(drgevent.X, drgevent.Y));
            var layout = GetLayoutSystemAt(position);
            layout?.OnDragOver(drgevent);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            CalculateAllMetricsAndLayout();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            CalculateAllMetricsAndLayout();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            //if (this.class2_0.Locked)
            //{
            //	return;
            //}
            if (_layoutSystem != null)
            {
                _layoutSystem.OnMouseDown(e);
                return;
            }
            if (rectangle_0.Contains(e.X, e.Y) && Manager != null && e.Button == MouseButtons.Left)
            {
                class11_0?.Dispose();
                class11_0 = new Class11(Manager, this, new Point(e.X, e.Y));
                class11_0.Cancalled += method_14;
                class11_0.Committed += method_15;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_layoutSystem != null)
            {
                _layoutSystem.OnMouseLeave();
                _layoutSystem = null;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!Capture)
            {
                LayoutSystemBase layoutSystemAt = GetLayoutSystemAt(new Point(e.X, e.Y));
                if (layoutSystemAt != null)
                {
                    if (_layoutSystem != null)
                    {
                        if (_layoutSystem != layoutSystemAt)
                        {
                            _layoutSystem.OnMouseLeave();
                        }
                    }
                    layoutSystemAt.OnMouseMove(e);
                    _layoutSystem = layoutSystemAt;
                    return;
                }
                if (_layoutSystem != null)
                {
                    _layoutSystem.OnMouseLeave();
                    _layoutSystem = null;
                }
                if (!rectangle_0.Contains(e.X, e.Y))
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (Vertical)
                {
                    Cursor.Current = Cursors.VSplit;
                    return;
                }
                Cursor.Current = Cursors.HSplit;
            }
            else
            {
                if (_layoutSystem == null)
                {
                    class11_0?.OnMouseMove(new Point(e.X, e.Y));
                    return;
                }
                _layoutSystem.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            //if (this.class2_0.Locked)
            //{
            //	return;
            //}
            if (_layoutSystem != null)
            {
                _layoutSystem.OnMouseUp(e);
                return;
            }
            class11_0?.Commit();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (bool_0)
                return;
            var container = Manager?.DockSystemContainer;
            WorkingRenderer.StartRenderSession(HotkeyPrefix.None);
            LayoutSystem.DrawDocumentStrip(WorkingRenderer, e.Graphics, Font);
            if (AllowResize)
                WorkingRenderer.DrawSplitter(container, this, e.Graphics, rectangle_0,Dock == DockStyle.Top || Dock == DockStyle.Bottom? Orientation.Horizontal: Orientation.Vertical);
            WorkingRenderer.FinishRenderSession();
            using (var brush = new SolidBrush(Color.FromArgb(50, Color.White)))
            using (var font = new Font(Font.FontFamily.Name, 14f, FontStyle.Bold))
                e.Graphics.DrawString("evaluation", font, brush, rectangle_1.Left + 4, rectangle_1.Top - 4,StringFormat.GenericTypographic);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            WorkingRenderer.DrawDockContainerBackground(pevent.Graphics, this, DisplayRectangle);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Manager != null && Parent != null && !(Parent is FloatingForm))
                Manager.DockSystemContainer = Parent;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            var form = FindForm();
            if (form == null || form.WindowState != FormWindowState.Minimized)
                CalculateAllMetricsAndLayout();
        }

        internal virtual void vmethod_0()
        {
        }

        internal virtual ControlLayoutSystem CreateNewControlLayoutSystem() => new ControlLayoutSystem();

        internal virtual void vmethod_2()
        {
            method_5(false);
        }

        [Browsable(false)]
        public override bool AllowDrop
        {
            get
            {
                return base.AllowDrop;
            }
            set
            {
                base.AllowDrop = value;
            }
        }

        [Browsable(false)]
        protected internal virtual bool AllowResize => Manager?.AllowDockContainerResize ?? true;

        [Browsable(false), DefaultValue(typeof(Color), "Control")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Naming(NamingType.FromOldVersion)]
        internal bool FriendDesignMode => DesignMode;

        [Naming(NamingType.FromOldVersion)]
        internal bool Vertical => Dock == DockStyle.Left || Dock == DockStyle.Right;

        internal bool Boolean_2 => int_1 > 0;

        [Naming(NamingType.FromOldVersion)]
        [Browsable(false)]
        internal virtual bool CanShowCollapsed => Dock != DockStyle.Fill && Dock != DockStyle.None;

        public int ContentSize
        {
            get
            {
                return _contentSize;
            }
            set
            {
                value = Math.Max(value, 32);
                if (_contentSize == value) return;
                bool_1 = true;
                _contentSize = value;
                method_9();
            }
        }

        protected override Size DefaultSize => new Size(0, 0);

        [Browsable(false)]
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                if (DockStyle.None == value)
                    throw new ArgumentException("The value None is not supported for DockContainers.");
                base.Dock = value;
                var orientation = Orientation.Horizontal;
                if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
                    orientation = Orientation.Vertical;
                if (_splitLayout.SplitMode != orientation)
                    _splitLayout.SplitMode = orientation;
            }
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        [Browsable(false)]
        public bool HasSingleControlLayoutSystem => LayoutSystem.LayoutSystems.Count == 1 && LayoutSystem.LayoutSystems[0] is ControlLayoutSystem;

        [Naming(NamingType.FromOldVersion)]
        internal int CurrentSize => Vertical ? rectangle_1.Width : rectangle_1.Height;

        [Browsable(false)]
        public virtual bool IsFloating => false;

        [Browsable(false)]
        public virtual SplitLayoutSystem LayoutSystem
        {
            get
            {
                return _splitLayout;
            }
            set
            {
                if (_splitLayout == value) return;
                if (value == null) throw new ArgumentNullException(nameof(value));

                bool_0 = true;
                try
                {
                    _splitLayout?.SetDockContainer(null);
                    if (!bool_1)
                    {
                        _contentSize = Convert.ToInt32(Vertical ? value.WorkingSize.Width : value.WorkingSize.Height);
                        bool_1 = true;
                    }
                    _splitLayout = value;
                    _splitLayout.SetDockContainer(this);
                    vmethod_2();
                }
                finally
                {
                    bool_0 = false;
                }
            }
        }

        [Browsable(false)]
        public virtual SandDockManager Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                _manager?.UnregisterDockContainer(this);
                if (value?.DockSystemContainer != null && !IsFloating && Parent != null && Parent != value.DockSystemContainer)
                    throw new ArgumentException("This DockContainer cannot use the specified manager as the manager's DockSystemContainer differs from the DockContainer's Parent.");
                _manager = value;
                if (_manager == null) return;
                _manager.RegisterDockContainer(this);
                LayoutSystem.SetDockContainer(this);
            }
        }

        internal Rectangle Rectangle_0 => rectangle_0;

        [Naming(NamingType.FromOldVersion)]
        internal virtual RendererBase WorkingRenderer => Manager?.Renderer ?? (_renderer ?? (_renderer = new WhidbeyRenderer()));

        [Browsable(false)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public event EventHandler DockingFinished;

        public event EventHandler DockingStarted;

        internal ArrayList layoutSystems;

        private static bool bool_0;

        private bool bool_1;

        private Tooltip _toolTips0;

        private Class11 class11_0;

        private const int int_0 = 32;

        private int int_1;

        private int _contentSize = 100;

        internal LayoutSystemBase _layoutSystem;

        private Rectangle rectangle_0 = Rectangle.Empty;

        private Rectangle rectangle_1 = Rectangle.Empty;

        private RendererBase _renderer;

        private SandDockManager _manager;

        private SplitLayoutSystem _splitLayout;
    }
}
