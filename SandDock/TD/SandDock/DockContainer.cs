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
            this._splitLayout = new SplitLayoutSystem();
            this._splitLayout.vmethod_2(this);
            base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.Selectable, false);
            this.layoutSystems = new ArrayList();
            this._toolTips0 = new ToolTips(this) { Boolean_0 = false };
            this._toolTips0.Event_0 += this.method_16;
            this.BackColor = SystemColors.Control;
        }

        public void CalculateAllMetricsAndLayout()
        {
            if (!IsHandleCreated)
                return;

            if (Capture && !IsFloating)
                Capture = false;

            this.rectangle_1 = this.DisplayRectangle;
            if (!AllowResize)
            {
                this.rectangle_0 = Rectangle.Empty;
            }
            else
            {
                switch (Dock)
                {
                    case DockStyle.Top:
                        this.rectangle_0 = new Rectangle(this.rectangle_1.Left, this.rectangle_1.Bottom - 4, this.rectangle_1.Width, 4);
                        this.rectangle_1.Height = this.rectangle_1.Height - 4;
                        break;
                    case DockStyle.Bottom:
                        this.rectangle_0 = new Rectangle(this.rectangle_1.Left, this.rectangle_1.Top, this.rectangle_1.Width, 4);
                        this.rectangle_1.Offset(0, 4);
                        this.rectangle_1.Height = this.rectangle_1.Height - 4;
                        break;
                    case DockStyle.Left:
                        this.rectangle_0 = new Rectangle(this.rectangle_1.Right - 4, this.rectangle_1.Top, 4, this.rectangle_1.Height);
                        this.rectangle_1.Width = this.rectangle_1.Width - 4;
                        break;
                    case DockStyle.Right:
                        this.rectangle_0 = new Rectangle(this.rectangle_1.Left, this.rectangle_1.Top, 4, this.rectangle_1.Height);
                        this.rectangle_1.Offset(4, 0);
                        this.rectangle_1.Width = this.rectangle_1.Width - 4;
                        break;
                    default:
                        this.rectangle_0 = Rectangle.Empty;
                        break;
                }
            }
            this.method_10(this._splitLayout, this.rectangle_1);
            Invalidate();
        }

        public ControlLayoutSystem CreateNewLayoutSystem(SizeF size) => CreateNewLayoutSystem(new DockControl[0], size);

        public ControlLayoutSystem CreateNewLayoutSystem(DockControl control, SizeF size) => CreateNewLayoutSystem(new[] { control }, size);

        public ControlLayoutSystem CreateNewLayoutSystem(DockControl[] controls, SizeF size)
        {
            if (controls == null)
                throw new ArgumentNullException(nameof(controls));

            var layout = this.vmethod_1();
            layout.WorkingSize = size;
            if (controls.Length != 0)
                layout.Controls.AddRange(controls);
            return layout;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._renderer != null)
                {
                    this._renderer.Dispose();
                    this._renderer = null;
                }
                Manager = null;
                this._toolTips0.Event_0 -= this.method_16;
                this._toolTips0.Dispose();
            }
            base.Dispose(disposing);
        }

        public LayoutSystemBase GetLayoutSystemAt(Point position)
        {
            LayoutSystemBase layoutSystemBase = null;
            foreach (LayoutSystemBase layoutSystemBase2 in this.layoutSystems)
            {
                if (layoutSystemBase2.Bounds.Contains(position) && (!(layoutSystemBase2 is ControlLayoutSystem) || !((ControlLayoutSystem)layoutSystemBase2).Collapsed))
                {
                    layoutSystemBase = layoutSystemBase2;
                    if (layoutSystemBase is ControlLayoutSystem)
                    {
                        break;
                    }
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
            if (!IsHandleCreated)
                return;

            using (var graphics = CreateGraphics())
            {
                Renderer.StartRenderSession(HotkeyPrefix.None);
                layoutSystem.Layout(Renderer, graphics, bounds, layoutSystem == this._splitLayout && IsFloating);
                Renderer.FinishRenderSession();
            }
        }

        internal void OnActivated(object sender, EventArgs e)
        {
            foreach (var controlLayoutSystem in this.layoutSystems.OfType<ControlLayoutSystem>().Where(controlLayoutSystem => controlLayoutSystem.method_8()))
            {
                break;
            }
        }

        internal void OnDeactivate(object sender, EventArgs e)
        {
            foreach (ControlLayoutSystem layoutSystemBase in this.layoutSystems.OfType<ControlLayoutSystem>())
            {
                layoutSystemBase.method_9();
            }
        }

        private void method_13()
        {
            this.class11_0.Event_0 -= this.method_14;
            this.class11_0.ResizingManagerFinished -= this.method_15;
            this.class11_0 = null;
        }

        private void method_14(object sender, EventArgs e)
        {
            this.method_13();
        }

        private void method_15(int size)
        {
            this.method_13();
            this.ContentSize = size;
        }

        private string method_16(Point point)
        {
            var layoutSystem = GetLayoutSystemAt(point) as ControlLayoutSystem;
            return layoutSystem == null ? "" : layoutSystem.vmethod_5(point);
        }

        internal void method_2()
        {
            this.int_1++;
            this._splitLayout.vmethod_2(null);
            foreach (var layoutSystemBase in this.layoutSystems.OfType<ControlLayoutSystem>())
                layoutSystemBase.Controls.Clear();
            this._splitLayout = new SplitLayoutSystem();
        }

        internal void method_3()
        {
            if (this.int_1 > 0)
            {
                this.int_1--;
            }
            if (this.int_1 == 0)
            {
                this.vmethod_2();
            }
        }

        internal void PropagateNewRenderer()
        {
            CalculateAllMetricsAndLayout();
        }

        private void method_5(bool bool_2)
        {
            this.layoutSystems.Clear();
            _layoutSystem = null;
            this.method_7(this._splitLayout);
            if (!bool_2 && this.int_1 == 0)
            {
                this.method_9();
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
                this.method_5(true);
        }

        private void method_7(LayoutSystemBase layoutSystemBase_1)
        {
            this.layoutSystems.Add(layoutSystemBase_1);
            if (layoutSystemBase_1 is SplitLayoutSystem)
            {
                foreach (LayoutSystemBase layoutSystemBase_2 in ((SplitLayoutSystem)layoutSystemBase_1).LayoutSystems)
                {
                    this.method_7(layoutSystemBase_2);
                }
            }
        }

        internal bool method_8() => !this.layoutSystems.OfType<ControlLayoutSystem>().Any();

        internal void method_9()
        {
            if (Boolean_6)
            {
                var allCollapsed = layoutSystems.OfType<ControlLayoutSystem>().All(controlLayoutSystem => controlLayoutSystem.Collapsed);
                int num = 0;
                if (!allCollapsed)
                    num += ContentSize + (AllowResize ? 4 : 0);
                if (Boolean_1 && Width != num)
                {
                    Width = num;
                    return;
                }
                if (!Boolean_1 && Height != num)
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
            if (this._layoutSystem != null)
            {
                this._layoutSystem.OnMouseDown(e);
                return;
            }
            if (this.rectangle_0.Contains(e.X, e.Y) && this.Manager != null && e.Button == MouseButtons.Left)
            {
                this.class11_0?.Dispose();
                this.class11_0 = new Class11(this.Manager, this, new Point(e.X, e.Y));
                this.class11_0.Event_0 += this.method_14;
                this.class11_0.ResizingManagerFinished += this.method_15;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (this._layoutSystem != null)
            {
                this._layoutSystem.OnMouseLeave();
                this._layoutSystem = null;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!base.Capture)
            {
                LayoutSystemBase layoutSystemAt = this.GetLayoutSystemAt(new Point(e.X, e.Y));
                if (layoutSystemAt != null)
                {
                    if (this._layoutSystem != null)
                    {
                        if (this._layoutSystem != layoutSystemAt)
                        {
                            this._layoutSystem.OnMouseLeave();
                        }
                    }
                    layoutSystemAt.OnMouseMove(e);
                    this._layoutSystem = layoutSystemAt;
                    return;
                }
                if (this._layoutSystem != null)
                {
                    this._layoutSystem.OnMouseLeave();
                    this._layoutSystem = null;
                }
                if (!this.rectangle_0.Contains(e.X, e.Y))
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (this.Boolean_1)
                {
                    Cursor.Current = Cursors.VSplit;
                    return;
                }
                Cursor.Current = Cursors.HSplit;
                return;
            }
            else
            {
                if (this._layoutSystem == null)
                {
                    this.class11_0?.OnMouseMove(new Point(e.X, e.Y));
                    return;
                }
                this._layoutSystem.OnMouseMove(e);
                return;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            //if (this.class2_0.Locked)
            //{
            //	return;
            //}
            if (this._layoutSystem != null)
            {
                this._layoutSystem.OnMouseUp(e);
                return;
            }
            this.class11_0?.Commit();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (bool_0)
                return;
            var container = Manager?.DockSystemContainer;
            Renderer.StartRenderSession(HotkeyPrefix.None);
            LayoutSystem.vmethod_4(Renderer, e.Graphics, Font);
            if (AllowResize)
                Renderer.DrawSplitter(container, this, e.Graphics, this.rectangle_0,Dock == DockStyle.Top || Dock == DockStyle.Bottom? Orientation.Horizontal: Orientation.Vertical);
            Renderer.FinishRenderSession();
            using (var brush = new SolidBrush(Color.FromArgb(50, Color.White)))
            using (var font = new Font(Font.FontFamily.Name, 14f, FontStyle.Bold))
                e.Graphics.DrawString("evaluation", font, brush, this.rectangle_1.Left + 4, this.rectangle_1.Top - 4,StringFormat.GenericTypographic);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            Renderer.DrawDockContainerBackground(pevent.Graphics, this, DisplayRectangle);
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

        internal virtual ControlLayoutSystem vmethod_1() => new ControlLayoutSystem();

        internal virtual void vmethod_2()
        {
            this.method_5(false);
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

        internal bool Boolean_0 => DesignMode;

        internal bool Boolean_1 => Dock == DockStyle.Left || Dock == DockStyle.Right;

        internal bool Boolean_2 => this.int_1 > 0;

        [Browsable(false)]
        internal virtual bool Boolean_6 => Dock != DockStyle.Fill && Dock != DockStyle.None;

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
                this.bool_1 = true;
                _contentSize = value;
                this.method_9();
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

        internal int Int32_0 => this.Boolean_1 ? this.rectangle_1.Width : this.rectangle_1.Height;

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
                    this._splitLayout?.vmethod_2(null);
                    if (!this.bool_1)
                    {
                        this._contentSize = Convert.ToInt32(Boolean_1 ? value.WorkingSize.Width : value.WorkingSize.Height);
                        this.bool_1 = true;
                    }
                    _splitLayout = value;
                    _splitLayout.vmethod_2(this);
                    this.vmethod_2();
                    return;
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
                LayoutSystem.vmethod_2(this);
            }
        }

        internal Rectangle Rectangle_0 => this.rectangle_0;

        internal virtual RendererBase Renderer => Manager?.Renderer ?? (_renderer ?? (_renderer = new WhidbeyRenderer()));

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

        private ToolTips _toolTips0;

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
