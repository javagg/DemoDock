using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using TD.SandDock.Design;
using TD.SandDock.Rendering;
using TD.Util;

namespace TD.SandDock
{
	[Designer(typeof(DockContainerDesigner)), ToolboxItem(false)]
	public class DockContainer : ContainerControl
	{
		public DockContainer()
		{
			//this.class2_0 = (LicenseManager.Validate(typeof(DockContainer), this) as Class2);
			this.splitLayoutSystem_0 = new SplitLayoutSystem();
			this.splitLayoutSystem_0.vmethod_2(this);
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.Selectable, false);
			this.arrayList_0 = new ArrayList();
			this.class0_0 = new Class0(this);
			this.class0_0.Boolean_0 = false;
			this.class0_0.Event_0 += new Class0.Delegate0(this.method_16);
			this.BackColor = SystemColors.Control;
		}

		public void CalculateAllMetricsAndLayout()
		{
			if (!base.IsHandleCreated)
			{
				return;
			}
			if (base.Capture && !this.IsFloating)
			{
				base.Capture = false;
			}
			this.rectangle_1 = this.DisplayRectangle;
			if (!this.AllowResize)
			{
				this.rectangle_0 = Rectangle.Empty;
			}
			else
			{
				switch (this.Dock)
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
			this.method_10(this.splitLayoutSystem_0, this.rectangle_1);
			base.Invalidate();
		}

		public ControlLayoutSystem CreateNewLayoutSystem(SizeF size)
		{
			return this.CreateNewLayoutSystem(new DockControl[0], size);
		}

		public ControlLayoutSystem CreateNewLayoutSystem(DockControl control, SizeF size) => CreateNewLayoutSystem(new[]{control}, size);

	    public ControlLayoutSystem CreateNewLayoutSystem(DockControl[] controls, SizeF size)
		{
			if (controls == null)
			{
				throw new ArgumentNullException("controls");
			}
			ControlLayoutSystem controlLayoutSystem = this.vmethod_1();
			controlLayoutSystem.WorkingSize = size;
			if (controls != null && controls.Length != 0)
			{
				controlLayoutSystem.Controls.AddRange(controls);
			}
			return controlLayoutSystem;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.rendererBase_0 != null)
				{
					this.rendererBase_0.Dispose();
					this.rendererBase_0 = null;
				}
				this.Manager = null;
				this.class0_0.Event_0 -= this.method_16;
				this.class0_0.Dispose();
			}
			base.Dispose(disposing);
		}

		public LayoutSystemBase GetLayoutSystemAt(Point position)
		{
			LayoutSystemBase layoutSystemBase = null;
			foreach (LayoutSystemBase layoutSystemBase2 in this.arrayList_0)
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

	    internal void method_0(ShowControlContextMenuEventArgs showControlContextMenuEventArgs_0) => Manager?.OnShowControlContextMenu(showControlContextMenuEventArgs_0);

	    internal object method_1(Type type_0)
		{
			return this.GetService(type_0);
		}

		internal void method_10(LayoutSystemBase layoutSystemBase_1, Rectangle rectangle_2)
		{
			if (!base.IsHandleCreated)
			{
				return;
			}
			using (Graphics graphics = base.CreateGraphics())
			{
				RendererBase rendererBase = this.RendererBase_0;
				rendererBase.StartRenderSession(HotkeyPrefix.None);
				if (layoutSystemBase_1 != this.splitLayoutSystem_0)
				{
					layoutSystemBase_1.Layout(rendererBase, graphics, rectangle_2, false);
				}
				else
				{
					layoutSystemBase_1.Layout(rendererBase, graphics, rectangle_2, this.IsFloating);
				}
				rendererBase.FinishRenderSession();
			}
		}

		internal void method_11(object sender, EventArgs e)
		{
		    foreach (ControlLayoutSystem controlLayoutSystem in this.arrayList_0.OfType<ControlLayoutSystem>().Where(controlLayoutSystem => controlLayoutSystem.method_8()))
		    {
		        break;
		    }
		}

	    internal void method_12(object sender, EventArgs e)
		{
		    foreach (ControlLayoutSystem layoutSystemBase in this.arrayList_0.OfType<ControlLayoutSystem>())
		    {
		        (layoutSystemBase).method_9();
		    }
		}

	    private void method_13()
		{
			this.class11_0.Event_0 -= this.method_14;
			this.class11_0.Event_1 -= this.method_15;
			this.class11_0 = null;
		}

		private void method_14(object sender, EventArgs e)
		{
			this.method_13();
		}

		private void method_15(int int_3)
		{
			this.method_13();
			this.ContentSize = int_3;
		}

		private string method_16(Point point_0)
		{
			var layoutSystemAt = GetLayoutSystemAt(point_0);
			if (!(layoutSystemAt is ControlLayoutSystem))
			{
				return "";
			}
			return ((ControlLayoutSystem)layoutSystemAt).vmethod_5(point_0);
		}

		internal void method_2()
		{
			this.int_1++;
			this.splitLayoutSystem_0.vmethod_2(null);
			foreach (ControlLayoutSystem layoutSystemBase in this.arrayList_0.OfType<ControlLayoutSystem>())
			{
			    (layoutSystemBase).Controls.Clear();
			}
			this.splitLayoutSystem_0 = new SplitLayoutSystem();
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

		internal void method_4()
		{
			this.CalculateAllMetricsAndLayout();
		}

		private void method_5(bool bool_2)
		{
			this.arrayList_0.Clear();
			this.layoutSystemBase_0 = null;
			this.method_7(this.splitLayoutSystem_0);
			if (!bool_2 && this.int_1 == 0)
			{
				this.method_9();
				Application.Idle += this.method_6;
			}
		}

		private void method_6(object sender, EventArgs e)
		{
			Application.Idle -= this.method_6;
			bool flag = false;
			while (this.LayoutSystem.Optimize())
			{
				flag = true;
			}
			if (flag)
			{
				this.method_5(true);
			}
		}

		private void method_7(LayoutSystemBase layoutSystemBase_1)
		{
			this.arrayList_0.Add(layoutSystemBase_1);
			if (layoutSystemBase_1 is SplitLayoutSystem)
			{
				foreach (LayoutSystemBase layoutSystemBase_2 in ((SplitLayoutSystem)layoutSystemBase_1).LayoutSystems)
				{
					this.method_7(layoutSystemBase_2);
				}
			}
		}

		internal bool method_8() => !this.arrayList_0.OfType<ControlLayoutSystem>().Any();

	    internal void method_9()
		{
			if (this.Boolean_6)
			{
				bool flag = true;
				foreach (LayoutSystemBase layoutSystemBase in this.arrayList_0)
				{
					if (layoutSystemBase is ControlLayoutSystem)
					{
						ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)layoutSystemBase;
						if (!controlLayoutSystem.Collapsed)
						{
							flag = false;
							break;
						}
					}
				}
				int num = 0;
				if (!flag)
				{
					num += this.ContentSize + (this.AllowResize ? 4 : 0);
				}
				if (this.Boolean_1)
				{
					if (base.Width != num)
					{
						base.Width = num;
						return;
					}
				}
				if (!this.Boolean_1)
				{
					if (base.Height != num)
					{
						base.Height = num;
						return;
					}
				}
				this.CalculateAllMetricsAndLayout();
				return;
			}
			this.CalculateAllMetricsAndLayout();
		}

		protected internal virtual void OnDockingFinished(EventArgs e)
		{
            DockingFinished?.Invoke(this, e);
		    this.Manager?.OnDockingFinished(e);
		}

		protected internal virtual void OnDockingStarted(EventArgs e)
		{
            DockingStarted?.Invoke(this, e);
		    this.Manager?.OnDockingStarted(e);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);
			//if (!this.class2_0.Locked)
			//{
		    this.layoutSystemBase_0?.OnMouseDoubleClick();
		    return;
			//}
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);
			Point position = base.PointToClient(new Point(drgevent.X, drgevent.Y));
			LayoutSystemBase layoutSystemAt = this.GetLayoutSystemAt(position);
		    layoutSystemAt?.OnDragOver(drgevent);
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.CalculateAllMetricsAndLayout();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.CalculateAllMetricsAndLayout();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			//if (this.class2_0.Locked)
			//{
			//	return;
			//}
			if (this.layoutSystemBase_0 != null)
			{
				this.layoutSystemBase_0.OnMouseDown(e);
				return;
			}
		    if (this.rectangle_0.Contains(e.X, e.Y) && this.Manager != null && e.Button == MouseButtons.Left)
		    {
		        this.class11_0?.Dispose();
		        this.class11_0 = new Class11(this.Manager, this, new Point(e.X, e.Y));
		        this.class11_0.Event_0 += this.method_14;
		        this.class11_0.Event_1 += this.method_15;
		    }
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.layoutSystemBase_0 != null)
			{
				this.layoutSystemBase_0.OnMouseLeave();
				this.layoutSystemBase_0 = null;
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
					if (this.layoutSystemBase_0 != null)
					{
						if (this.layoutSystemBase_0 != layoutSystemAt)
						{
							this.layoutSystemBase_0.OnMouseLeave();
						}
					}
					layoutSystemAt.OnMouseMove(e);
					this.layoutSystemBase_0 = layoutSystemAt;
					return;
				}
				if (this.layoutSystemBase_0 != null)
				{
					this.layoutSystemBase_0.OnMouseLeave();
					this.layoutSystemBase_0 = null;
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
				if (this.layoutSystemBase_0 == null)
				{
				    this.class11_0?.OnMouseMove(new Point(e.X, e.Y));
				    return;
				}
				this.layoutSystemBase_0.OnMouseMove(e);
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
			if (this.layoutSystemBase_0 != null)
			{
				this.layoutSystemBase_0.OnMouseUp(e);
				return;
			}
		    this.class11_0?.Commit();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (DockContainer.bool_0)
			{
				return;
			}
			Control container = this.Manager?.DockSystemContainer;
			this.RendererBase_0.StartRenderSession(HotkeyPrefix.None);
			this.LayoutSystem.vmethod_4(this.RendererBase_0, e.Graphics, this.Font);
			if (this.AllowResize)
			{
				this.RendererBase_0.DrawSplitter(container, this, e.Graphics, this.rectangle_0, (this.Dock == DockStyle.Top || this.Dock == DockStyle.Bottom) ? Orientation.Horizontal : Orientation.Vertical);
			}
			this.RendererBase_0.FinishRenderSession();
			//if (this.class2_0.Evaluation)
			//{
				using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(50, Color.White)))
				{
					using (Font font = new Font(this.Font.FontFamily.Name, 14f, FontStyle.Bold))
					{
						e.Graphics.DrawString("evaluation", font, solidBrush, (float)(this.rectangle_1.Left + 4), (float)(this.rectangle_1.Top - 4), StringFormat.GenericTypographic);
					}
				}
			//}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			this.RendererBase_0.DrawDockContainerBackground(pevent.Graphics, this, this.DisplayRectangle);
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (this.Manager != null && base.Parent != null && !(base.Parent is Form2))
			{
				this.Manager.DockSystemContainer = base.Parent;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Form form = base.FindForm();
			if (form == null || form.WindowState != FormWindowState.Minimized)
			{
				this.CalculateAllMetricsAndLayout();
			}
		}

		internal virtual void vmethod_0()
		{
		}

		internal virtual ControlLayoutSystem vmethod_1()
		{
			return new ControlLayoutSystem();
		}

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
		protected internal virtual bool AllowResize => this.Manager?.AllowDockContainerResize ?? true;

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

		internal bool Boolean_0
		{
			get
			{
				return base.DesignMode;
			}
		}

		internal bool Boolean_1 => this.Dock == DockStyle.Left || this.Dock == DockStyle.Right;

	    internal bool Boolean_2
		{
			get
			{
				return this.int_1 > 0;
			}
		}

		[Browsable(false)]
		internal virtual bool Boolean_6 => this.Dock != DockStyle.Fill && this.Dock != DockStyle.None;

	    public int ContentSize
		{
			get
			{
				return this.int_2;
			}
			set
			{
				value = Math.Max(value, 32);
				if (value != this.int_2)
				{
					this.bool_1 = true;
					this.int_2 = value;
					this.method_9();
				}
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
				if (value == DockStyle.None)
				{
					throw new ArgumentException("The value None is not supported for DockContainers.");
				}
				base.Dock = value;
				Orientation orientation = Orientation.Horizontal;
				if (this.Dock != DockStyle.Top)
				{
					if (this.Dock != DockStyle.Bottom)
					{
						goto IL_2F;
					}
				}
				orientation = Orientation.Vertical;
				IL_2F:
				if (this.splitLayoutSystem_0.SplitMode != orientation)
				{
					this.splitLayoutSystem_0.SplitMode = orientation;
				}
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
		public bool HasSingleControlLayoutSystem => this.LayoutSystem.LayoutSystems.Count == 1 && this.LayoutSystem.LayoutSystems[0] is ControlLayoutSystem;

	    internal int Int32_0 => this.Boolean_1 ? this.rectangle_1.Width : this.rectangle_1.Height;

	    [Browsable(false)]
		public virtual bool IsFloating => false;

	    [Browsable(false)]
		public virtual SplitLayoutSystem LayoutSystem
		{
			get
			{
				return this.splitLayoutSystem_0;
			}
			set
			{
				if (value != this.splitLayoutSystem_0)
				{
					if (value != null)
					{
						DockContainer.bool_0 = true;
						try
						{
						    this.splitLayoutSystem_0?.vmethod_2(null);
						    if (!this.bool_1)
							{
							    this.int_2 = Convert.ToInt32(this.Boolean_1 ? value.WorkingSize.Width : value.WorkingSize.Height);
							    this.bool_1 = true;
							}
							this.splitLayoutSystem_0 = value;
							this.splitLayoutSystem_0.vmethod_2(this);
							this.vmethod_2();
							return;
						}
						finally
						{
							DockContainer.bool_0 = false;
						}
					}
					throw new ArgumentNullException("value");
				}
			}
		}

		[Browsable(false)]
		public virtual SandDockManager Manager
		{
			get
			{
				return this.sandDockManager_0;
			}
			set
			{
			    this.sandDockManager_0?.UnregisterDockContainer(this);
			    if (value != null && value.DockSystemContainer != null && !this.IsFloating && base.Parent != null)
				{
					if (base.Parent != value.DockSystemContainer)
					{
						throw new ArgumentException("This DockContainer cannot use the specified manager as the manager's DockSystemContainer differs from the DockContainer's Parent.");
					}
				}
				this.sandDockManager_0 = value;
				if (this.sandDockManager_0 != null)
				{
					this.sandDockManager_0.RegisterDockContainer(this);
					this.LayoutSystem.vmethod_2(this);
				}
			}
		}

		internal Rectangle Rectangle_0 => this.rectangle_0;

	    internal virtual RendererBase RendererBase_0
		{
			get
			{
			    if (this.sandDockManager_0?.Renderer != null)
				{
					return this.sandDockManager_0.Renderer;
				}
			    return this.rendererBase_0 ?? (this.rendererBase_0 = new WhidbeyRenderer());
			}
		}

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

		internal ArrayList arrayList_0;

		private static bool bool_0;

		private bool bool_1;

		private Class0 class0_0;

		private Class11 class11_0;

		//private Class2 class2_0;

		//private EventHandler eventHandler_0;

		//private EventHandler eventHandler_1;

		private const int int_0 = 32;

		private int int_1;

		private int int_2 = 100;

		internal LayoutSystemBase layoutSystemBase_0;

		private Rectangle rectangle_0 = Rectangle.Empty;

		private Rectangle rectangle_1 = Rectangle.Empty;

		private RendererBase rendererBase_0;

		private SandDockManager sandDockManager_0;

		private SplitLayoutSystem splitLayoutSystem_0;
	}
}
