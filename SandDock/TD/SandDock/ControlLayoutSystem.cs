using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
	[TypeConverter(typeof(Class23))]
	public class ControlLayoutSystem : LayoutSystemBase
    {
        public class DockControlCollection : CollectionBase
        {
            internal DockControlCollection(ControlLayoutSystem parent)
            {
                _parent = parent;
            }

            public int Add(DockControl control)
            {
                if (List.Contains(control))
                    throw new InvalidOperationException("The DockControl already belongs to this ControlLayoutSystem.");
                var count = Count;
                Insert(count, control);
                return count;
            }

            public void AddRange(DockControl[] controls)
            {
                this.bool_0 = true;
                foreach (var control in controls)
                    Add(control);
                this.bool_0 = false;
                this._parent.CalculateAllMetricsAndLayout();
            }

            public bool Contains(DockControl control) => List.Contains(control);

            public void CopyTo(DockControl[] array, int index) => List.CopyTo(array, index);

            public int IndexOf(DockControl control) => List.IndexOf(control);

            public void Insert(int index, DockControl control)
            {
                if (control == null) return;
                if (control.LayoutSystem == _parent && (IndexOf(control) == index || Count == 1))
                    return;

                if (control.LayoutSystem != null)
                {
                    if (Contains(control) && IndexOf(control) < index)
                    {
                        index--;
                    }
                    control.LayoutSystem.Controls.Remove(control);
                }
                List.Insert(index, control);
            }

            internal int method_0(int index, bool bool_2)
            {
                return index < 0 || index > Count ? (bool_2 ? Count : 0) : index;
            }

            protected override void OnClear()
            {
                base.OnClear();
                foreach (DockControl control in this)
                {
                    control.method_16(null);
                    control.method_5();
                }
            }

            protected override void OnClearComplete()
            {
                base.OnClearComplete();
                _parent.SelectedControl = null;
                _parent.CalculateAllMetricsAndLayout();
                _parent.DockContainer?.vmethod_0();
            }

            protected override void OnInsertComplete(int index, object value)
            {
                base.OnInsertComplete(index, value);
                if (bool_1)
                    return;
                DockControl dockControl = (DockControl)value;
                dockControl.method_16(this._parent);
                if (this._parent.IsInContainer && this._parent.DockContainer.Manager != null && this._parent.DockContainer.Manager != dockControl.Manager)
                {
                    dockControl.Manager = this._parent.DockContainer.Manager;
                }
                if (this._parent.IsInContainer)
                {
                    dockControl.method_4(this._parent.DockContainer);
                }
                if (this._parent.IsInContainer)
                {
                    if (dockControl.Parent != null)
                    {
                        LayoutUtilities.smethod_8(dockControl);
                    }
                    dockControl.Parent = this._parent.Control_0;
                }
                if (this._parent._selectedControl == null)
                {
                    this._parent.SelectedControl = dockControl;
                }
                this._parent.DockContainer?.vmethod_0();
                if (!this.bool_0)
                {
                    this._parent.CalculateAllMetricsAndLayout();
                }
            }

            protected override void OnRemoveComplete(int index, object value)
            {
                base.OnRemoveComplete(index, value);
                if (this.bool_1) return;

                DockControl dockControl = (DockControl)value;
                dockControl.method_16(null);
                dockControl.method_5();
                if (dockControl.Parent != null)
                {
                    if (dockControl.Parent == this._parent.Control_0)
                    {
                        LayoutUtilities.smethod_8(dockControl);
                    }
                }
                if (this._parent._selectedControl == value)
                {
                    if (this._parent.Controls.Count != 0)
                    {
                        this._parent.SelectedControl = this[0];
                    }
                    else
                    {
                        this._parent.SelectedControl = null;
                    }
                }
                this._parent.DockContainer?.vmethod_0();
                this._parent.CalculateAllMetricsAndLayout();
            }

            public void Remove(DockControl control)
            {
                if (control == null)
                    throw new ArgumentNullException(nameof(control));
                List.Remove(control);
            }

            public void SetChildIndex(DockControl control, int index)
            {
                if (control == null)
                    throw new ArgumentNullException(nameof(control));
                if (!Contains(control))
                    throw new ArgumentOutOfRangeException(nameof(control));
                if (index == IndexOf(control))
                    return;
                if (IndexOf(control) < index)
                    index--;
                this.bool_1 = true;
                List.Remove(control);
                List.Insert(index, control);
                bool_1 = false;
                _parent.CalculateAllMetricsAndLayout();
            }

            public DockControl this[int index] => (DockControl)List[index];

            private bool bool_0;

            private bool bool_1;

            private readonly ControlLayoutSystem _parent;
        }

        public ControlLayoutSystem()
		{
			Controls = new DockControlCollection(this);
			this.class17_0 = new Class17();
			this.class17_1 = new Class17();
			this.class17_2 = new Class17();
		}

		public ControlLayoutSystem(int desiredWidth, int desiredHeight) : this()
		{
			WorkingSize = new SizeF(desiredWidth, desiredHeight);
		}

		public ControlLayoutSystem(SizeF workingSize, DockControl[] windows, DockControl selectedWindow) : this()
		{
			WorkingSize = workingSize;
			Controls.AddRange(windows);
		    if (selectedWindow != null)
		        SelectedControl = selectedWindow;
		}

		[Obsolete("Use the constructor that takes a SizeF instead.")]
		public ControlLayoutSystem(int desiredWidth, int desiredHeight, DockControl[] controls, DockControl selectedControl) : this(desiredWidth, desiredHeight)
		{
			Controls.AddRange(controls);
		    if (selectedControl != null)
		        SelectedControl = selectedControl;
		}

		public ControlLayoutSystem(int desiredWidth, int desiredHeight, DockControl[] controls, DockControl selectedControl, bool collapsed) : this(new SizeF((float)desiredWidth, (float)desiredHeight), controls, selectedControl)
		{
			Collapsed = collapsed;
		}

		protected virtual void CalculateLayout(RendererBase renderer, Rectangle bounds, bool floating, out Rectangle titlebarBounds, out Rectangle tabstripBounds, out Rectangle clientBounds, out Rectangle joinCatchmentBounds)
		{
			if (floating)
			{
				titlebarBounds = Rectangle.Empty;
			}
			else
			{
				titlebarBounds = bounds;
				titlebarBounds.Offset(0, renderer.TitleBarMetrics.Margin.Top);
				titlebarBounds.Height = renderer.TitleBarMetrics.Height - (renderer.TitleBarMetrics.Margin.Top + renderer.TitleBarMetrics.Margin.Bottom);
				this.method_17();
				bounds.Offset(0, renderer.TitleBarMetrics.Height);
				bounds.Height -= renderer.TitleBarMetrics.Height;
			}
			if (Controls.Count <= 1 && !DockContainer.Boolean_0)
			{
				tabstripBounds = Rectangle.Empty;
			}
			else
			{
				tabstripBounds = bounds;
				tabstripBounds.Y = tabstripBounds.Bottom - renderer.TabStripMetrics.Height;
				tabstripBounds.Height = renderer.TabStripMetrics.Height;
				tabstripBounds = renderer.TabStripMetrics.RemoveMargin(tabstripBounds);
				bounds.Height -= renderer.TabStripMetrics.Height;
			}
			clientBounds = bounds;
			joinCatchmentBounds = titlebarBounds;
		}

		public void ClosePopup()
		{
		    if (IsPoppedUp)
		        AutoHideBar.method_6(true);
		}

		public void Dock(ControlLayoutSystem layoutSystem)
		{
		    if (layoutSystem == null)
		        throw new ArgumentNullException();
		    Dock(layoutSystem, 0);
		}

		public void Dock(ControlLayoutSystem layoutSystem, int index)
		{
		    if (layoutSystem == null)
		        throw new ArgumentNullException();
		    if (Parent != null)
		        throw new InvalidOperationException("This layout system already has a parent. To remove it, use the parent layout system's LayoutSystems.Remove method.");

            DockControl selectedControl = this.SelectedControl;
			while (this.Controls.Count != 0)
			{
				DockControl control = this.Controls[0];
				this.Controls.RemoveAt(0);
				layoutSystem.Controls.Insert(index, control);
			}
		    if (selectedControl != null)
		        layoutSystem.SelectedControl = selectedControl;
		}

		public void Float(SandDockManager manager)
		{
		    if (SelectedControl == null)
		        throw new InvalidOperationException("The layout system must have a selected control to be floated.");
		    Float(manager, SelectedControl.method_11(), WindowOpenMethod.OnScreenActivate);
		}

		public void Float(SandDockManager manager, Rectangle bounds, WindowOpenMethod openMethod)
		{
		    if (Parent != null)
		        LayoutUtilities.smethod_10(this);
		    if (SelectedControl.MetaData.LastFloatingWindowGuid == Guid.Empty)
		        SelectedControl.MetaData.SaveFloatingWindowGuid(Guid.NewGuid());
		    new FloatingContainer(manager, SelectedControl.MetaData.LastFloatingWindowGuid)
			{
				LayoutSystem = 
				{
					LayoutSystems = 
					{
						this
					}
				}
			}.method_19(bounds, true, openMethod == WindowOpenMethod.OnScreenActivate);

		    if (openMethod == WindowOpenMethod.OnScreenActivate)
		        SelectedControl.Activate();
		}

		public virtual DockControl GetControlAt(Point position)
		{
			if (this.rectangle_2.Contains(position) && !this.class17_0.Bounds.Contains(position) && !this.class17_1.Bounds.Contains(position))
			{
			    return Controls.Cast<DockControl>().FirstOrDefault(c => c.TabBounds.Contains(position));
			}
		    return null;
		}

		protected internal override void Layout(RendererBase renderer, Graphics graphics, Rectangle bounds, bool floating)
		{
			base.Layout(renderer, graphics, bounds, floating);
			this.method_18();
		    if (Collapsed && DockContainer.Boolean_6)
		        return;

		    CalculateLayout(renderer, bounds, floating, out this.rectangle_1, out this.rectangle_2, out this.rectangle_3, out this.rectangle_4);
			this.bool_4 = true;
			try
			{
				if (this.rectangle_1 != Rectangle.Empty)
				{
					this.method_17();
				}
				this.method_19(renderer, graphics, this.rectangle_2);
				foreach (DockControl dockControl in Controls)
				{
					if (dockControl != this.SelectedControl)
					{
						dockControl.method_0(false);
					}
				}
				foreach (DockControl dockControl2 in this.Controls)
				{
					if (dockControl2 == SelectedControl)
					{
						Rectangle bounds2 = renderer.AdjustDockControlClientBounds(this, dockControl2, this.rectangle_3);
						dockControl2.Bounds = bounds2;
						dockControl2.method_0(true);
					}
				}
			}
			finally
			{
				this.bool_4 = false;
			}
		}

		protected internal virtual void LayoutCollapsed(RendererBase renderer, Rectangle bounds)
		{
			this.rectangle_1 = bounds;
			this.rectangle_1.Offset(0, renderer.TitleBarMetrics.Margin.Top);
			this.rectangle_1.Height = renderer.TitleBarMetrics.Height - (renderer.TitleBarMetrics.Margin.Top + renderer.TitleBarMetrics.Margin.Bottom);
			this.method_17();
			bounds.Offset(0, renderer.TitleBarMetrics.Height);
			bounds.Height -= renderer.TitleBarMetrics.Height;
			this.rectangle_3 = bounds;
			this.rectangle_2 = Rectangle.Empty;
			foreach (DockControl dockControl in Controls)
			{
				Rectangle bounds2 = renderer.AdjustDockControlClientBounds(this, dockControl, this.rectangle_3);
				dockControl.method_0(dockControl == _selectedControl);
				dockControl.Bounds = bounds2;
			}
		}

		internal void method_10(Graphics graphics_0, RendererBase rendererBase_0, Class17 class17_4, SandDockButtonType sandDockButtonType_0, bool bool_7)
		{
			if (class17_4.bool_0)
			{
				DrawItemState drawItemState = DrawItemState.Default;
				if (this.Class17_0 == class17_4)
				{
					drawItemState |= DrawItemState.HotLight;
					if (this.bool_2)
					{
						drawItemState |= DrawItemState.Selected;
					}
				}
				if (!bool_7)
				{
					drawItemState |= DrawItemState.Disabled;
				}
				rendererBase_0.DrawDocumentStripButton(graphics_0, class17_4.Bounds, sandDockButtonType_0, drawItemState);
			}
		}

		internal void method_11(SandDockManager sandDockManager_0, DockControl dockControl_1, bool bool_7, Class7.DockTarget dockTarget_0)
		{
			if (dockTarget_0.type == Class7.DockTargetType.JoinExistingSystem)
			{
				if (!bool_7)
				{
					dockControl_1.method_15(dockTarget_0.layoutSystem, dockTarget_0.index);
					return;
				}
				this.Dock(dockTarget_0.layoutSystem, dockTarget_0.index);
				return;
			}
			else
			{
				if (dockTarget_0.type != Class7.DockTargetType.CreateNewContainer)
				{
					if (dockTarget_0.type == Class7.DockTargetType.SplitExistingSystem)
					{
						ControlLayoutSystem controlLayoutSystem = dockTarget_0.dockContainer.CreateNewLayoutSystem(bool_7 ? this.DockControls : new DockControl[]
						{
							dockControl_1
						}, base.WorkingSize);
						dockTarget_0.layoutSystem.SplitForLayoutSystem(controlLayoutSystem, dockTarget_0.DockSide);
					}
					return;
				}
				DockContainer dockContainer = sandDockManager_0.FindDockedContainer(DockStyle.Fill);
				if (dockTarget_0.DockLocation == ContainerDockLocation.Center && dockContainer != null)
				{
					ControlLayoutSystem controlLayoutSystem = LayoutUtilities.FindControlLayoutSystem(dockContainer);
					if (controlLayoutSystem != null)
					{
						if (bool_7)
						{
							this.Dock(controlLayoutSystem);
							return;
						}
						dockControl_1.method_15(controlLayoutSystem, 0);
						return;
					}
				}
				else
				{
					if (bool_7)
					{
						base.method_2(sandDockManager_0, dockTarget_0.DockLocation, dockTarget_0.middle ? ContainerDockEdge.Inside : ContainerDockEdge.Outside);
						return;
					}
					dockControl_1.DockInNewContainer(dockTarget_0.DockLocation, dockTarget_0.middle ? ContainerDockEdge.Inside : ContainerDockEdge.Outside);
				}
				return;
			}
		}

		private void method_12(LayoutSystemBase layoutSystem, int int_4, bool bool_7)
		{
			var parent = Parent;
			parent.LayoutSystems.bool_0 = true;
			parent.LayoutSystems.Insert(int_4, layoutSystem);
			parent.LayoutSystems.bool_0 = false;
			parent.method_7();
		}

		private void method_13(LayoutSystemBase layoutSystemBase_0, Orientation orientation_0, bool bool_7)
		{
			SplitLayoutSystem parent = base.Parent;
		    SplitLayoutSystem splitLayoutSystem = new SplitLayoutSystem
		    {
		        SplitMode = orientation_0,
		        WorkingSize = WorkingSize
		    };
		    int index = parent.LayoutSystems.IndexOf(this);
			parent.LayoutSystems.bool_0 = true;
			parent.LayoutSystems.Remove(this);
			parent.LayoutSystems.Insert(index, splitLayoutSystem);
			parent.LayoutSystems.bool_0 = false;
			splitLayoutSystem.LayoutSystems.Add(this);
			if (!bool_7)
			{
				splitLayoutSystem.LayoutSystems.Add(layoutSystemBase_0);
			}
			else
			{
				splitLayoutSystem.LayoutSystems.Insert(0, layoutSystemBase_0);
			}
			parent.method_7();
		}

		internal void method_14(DockSituation dockSituation)
		{
		    if (Controls.Count == 0)
		        throw new InvalidOperationException();

		    if (SelectedControl.DockSituation == dockSituation) return;

		    var selectedControl = SelectedControl;
		    var array = new DockControl[Controls.Count];
		    Controls.CopyTo(array, 0);
		    LayoutUtilities.smethod_10(this);
		    Controls.Clear();
		    if (dockSituation != DockSituation.Docked)
		    {
		        if (dockSituation != DockSituation.Document)
		        {
		            if (dockSituation != DockSituation.Floating)
		            {
		                throw new InvalidOperationException();
		            }
		            array[0].OpenFloating(WindowOpenMethod.OnScreenActivate);
		        }
		        else
		        {
		            array[0].OpenDocument(WindowOpenMethod.OnScreenActivate);
		        }
		    }
		    else
		    {
		        array[0].OpenDocked(WindowOpenMethod.OnScreenActivate);
		    }
		    DockControl[] array2 = new DockControl[array.Length - 1];
		    Array.Copy(array, 1, array2, 0, array.Length - 1);
		    array[0].LayoutSystem.Controls.AddRange(array2);
		    array[0].LayoutSystem.SelectedControl = selectedControl;
		}

		internal int method_15(Point point)
		{
		    return Controls.Cast<DockControl>().Select(control => control.TabBounds).Count(rect => point.X > rect.Left + rect.Width/2);
		}

	    internal void CalculateAllMetricsAndLayout()
		{
		    AutoHideBar?.method_0(this);
		    if (IsInContainer)
			{
				if (DockContainer.IsFloating)
				{
					DockContainer.CalculateAllMetricsAndLayout();
				}
				else
				{
					DockContainer.method_10(this, Bounds);
				}
				DockContainer.Invalidate(Bounds);
			}
		}

		private void method_17()
		{
			if (this._selectedControl == null)
			{
				this.class17_0.bool_0 = false;
				this.class17_1.bool_0 = false;
				this.class17_2.bool_0 = false;
				return;
			}
			int y = this.rectangle_1.Top + this.rectangle_1.Height / 2 - 7;
			int num = this.rectangle_1.Right - 2;
			if (this._selectedControl.AllowClose)
			{
				this.class17_0.bool_0 = true;
				this.class17_0.Bounds = new Rectangle(num - 19, y, 19, 15);
				num -= 21;
			}
			else
			{
				this.class17_0.bool_0 = false;
			}
			if (!this.AllowCollapse || (base.IsInContainer && !base.DockContainer.Boolean_6))
			{
				this.class17_1.bool_0 = false;
			}
			else
			{
				this.class17_1.bool_0 = true;
				this.class17_1.Bounds = new Rectangle(num - 19, y, 19, 15);
				num -= 21;
			}
			if (!this._selectedControl.ShowOptions)
			{
				this.class17_2.bool_0 = false;
				return;
			}
			this.class17_2.bool_0 = true;
			this.class17_2.Bounds = new Rectangle(num - 19, y, 19, 15);
			num -= 21;
		}

		private void method_18()
		{
		    foreach (DockControl dockControl in Controls)
		        dockControl.method_5();
		}

		private void method_19(RendererBase renderer, Graphics g, Rectangle bounds)
		{
			int num = 0;
			int num2 = bounds.Width - (renderer.TabStripMetrics.Padding.Left + renderer.TabStripMetrics.Padding.Right);
			int[] array = new int[Controls.Count];
			int num3 = 0;
			foreach (DockControl dockControl in Controls)
			{
				dockControl.bool_3 = false;
				int num4 = renderer.MeasureTabStripTab(g, dockControl.TabImage, dockControl.TabText, dockControl.Font, DrawItemState.Default).Width;
				if (dockControl.MinimumTabWidth != 0)
				{
					num4 = Math.Max(num4, dockControl.MinimumTabWidth);
				}
				if (dockControl.MaximumTabWidth != 0 && dockControl.MaximumTabWidth < num4)
				{
					num4 = dockControl.MaximumTabWidth;
					dockControl.bool_3 = true;
				}
				num += num4;
				array[num3++] = num4;
			}
			if (num > num2)
			{
				int num5 = num - num2;
				for (int i = 0; i < num3; i++)
				{
					array[i] -= (int)((float)num5 * ((float)array[i] / (float)num));
					this.Controls[i].bool_3 = true;
				}
			}
			bounds = renderer.TabStripMetrics.RemovePadding(bounds);
			int num6 = bounds.Left;
			num3 = 0;
			for (int j = 0; j < this.Controls.Count; j++)
			{
				DockControl dockControl2 = this.Controls[j];
				BoxModel tabMetrics = renderer.TabMetrics;
				Rectangle rectangle_6 = new Rectangle(num6 + tabMetrics.Margin.Left, bounds.Top + tabMetrics.Margin.Top, tabMetrics.Padding.Left + array[num3] + tabMetrics.Padding.Right, bounds.Height - (tabMetrics.Margin.Top + tabMetrics.Margin.Bottom));
				dockControl2.TabBounds = rectangle_6;
				num6 += rectangle_6.Width + tabMetrics.ExtraWidth;
				num3++;
			}
		}

		internal void SetAutoHideBar(AutoHideBar autoHideBar)
		{
			AutoHideBar = autoHideBar;
		}

		private void method_5(DockControl dockControl_1, DockControl dockControl_2)
		{
            SelectionChanged?.Invoke(dockControl_1, dockControl_2);
		}

	    private void method_6()
		{
			switch (this.SelectedControl.DockSituation)
			{
			case DockSituation.Docked:
			case DockSituation.Document:
				if (this.AllowFloat)
				{
					this.method_14(DockSituation.Floating);
					return;
				}
				break;
			case DockSituation.Floating:
				if (this.SelectedControl.MetaData.LastFixedDockSituation == DockSituation.Docked)
				{
					if (this.vmethod_3(this.SelectedControl.MetaData.LastFixedDockSide))
					{
						this.method_14(DockSituation.Docked);
						return;
					}
				}
				if (this.SelectedControl.MetaData.LastFixedDockSituation == DockSituation.Document)
				{
					if (this.vmethod_3(ContainerDockLocation.Center))
					{
						this.method_14(DockSituation.Document);
					}
				}
				break;
			default:
				return;
			}
		}

		private void method_7()
		{
			Point point = new Point(this.class17_2.Bounds.Left, this.class17_2.Bounds.Bottom);
			point = SelectedControl.Parent.PointToScreen(point);
			point = SelectedControl.PointToClient(point);
			DockContainer.ShowControlContextMenu(new ShowControlContextMenuEventArgs(SelectedControl, point, ContextMenuContext.OptionsButton));
		}

		internal bool method_8()
		{
		    if (!IsInContainer || SelectedControl == null || !SelectedControl.ContainsFocus) return false;

            Boolean_1 = true;
		    if (SelectedControl != null)
		        DockContainer.Manager.OnDockControlActivated(new DockControlEventArgs(this.SelectedControl));
		    return true;
		}

		internal void method_9()
		{
			this.Boolean_1 = false;
		}

		protected virtual void OnCloseButtonClick(EventArgs e)
		{
		    SelectedControl?.method_14(true);
		}

	    protected internal override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);
			var control = this.GetControlAt(base.DockContainer.PointToClient(new Point(drgevent.X, drgevent.Y)));
	        if (control != null && SelectedControl != control)
	            control.Open(WindowOpenMethod.OnScreenActivate);
		}

		protected internal override void OnMouseDoubleClick()
		{
			var point = DockContainer.PointToClient(Cursor.Position);
		    if (DockContainer.Manager == null)
		        return;
		    if (!LockControls)
			{
				if (this.rectangle_1.Contains(point) && !this.class17_0.Bounds.Contains(point) && !this.class17_1.Bounds.Contains(point) && this.Controls.Count != 0)
				{
					this.method_6();
					return;
				}
				DockControl controlAt = this.GetControlAt(point);
			    controlAt?.OnTabDoubleClick();
			}
		}

		protected internal override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.bool_1 = false;
			if (this.rectangle_1.Contains(e.X, e.Y))
			{
				this.SelectedControl?.Activate();
			}
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				if (this.rectangle_1.Contains(e.X, e.Y))
				{
					this.point_0 = new Point(e.X, e.Y);
				}
				if (this.Class17_0 != null)
				{
					this.bool_2 = true;
					this.vmethod_9();
					this.vmethod_7(this.Class17_0);
					this.point_0 = Point.Empty;
					return;
				}
			}
			DockControl controlAt = this.GetControlAt(new Point(e.X, e.Y));
			if (controlAt != null)
			{
				controlAt.Activate();
				this.bool_1 = true;
				if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				{
					this.point_0 = new Point(e.X, e.Y);
				}
			}
		}

		protected internal override void OnMouseLeave()
		{
			base.OnMouseLeave();
			this.Class17_0 = null;
			this.bool_2 = false;
		}

		protected internal override void OnMouseMove(MouseEventArgs e)
		{
			if (this.bool_4)
			{
				return;
			}
			if (e.Button == MouseButtons.None)
			{
				this.bool_1 = false;
			}
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				if (this.class7_0 != null)
				{
					this.class7_0.OnMouseMove(Cursor.Position);
					return;
				}
				Rectangle rectangle = new Rectangle(this.point_0, new Size(0, 0));
				rectangle.Inflate(SystemInformation.DragSize);
				if (!rectangle.Contains(e.X, e.Y) && base.IsInContainer && this.point_0 != Point.Empty && !this.Collapsed && !this.LockControls)
				{
					DockControl controlAt = this.GetControlAt(this.point_0);
					this.bool_5 = (controlAt == null);
					DockingHints dockingHints_;
					if (base.DockContainer.Manager == null)
					{
						dockingHints_ = DockingHints.TranslucentFill;
					}
					else
					{
						dockingHints_ = base.DockContainer.Manager.DockingHints;
					}
					DockingManager dockingManager_;
					if (base.DockContainer.Manager == null)
					{
						dockingManager_ = DockingManager.Standard;
					}
					else
					{
						dockingManager_ = base.DockContainer.Manager.DockingManager;
					}
					base.method_0(base.DockContainer.Manager, base.DockContainer, this, controlAt, this.SelectedControl.MetaData.DockedContentSize, this.point_0, dockingHints_, dockingManager_);
					return;
				}
			}
			if (!this.bool_1)
			{
				this.Class17_0 = this.vmethod_6(e.X, e.Y);
			}
		}

		protected internal override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.point_0 = Point.Empty;
			this.bool_1 = false;
			if (this.class7_0 != null)
			{
				this.class7_0.Commit();
				return;
			}
			if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
			{
				DockControl dockControl = this.GetControlAt(new Point(e.X, e.Y));
				if (dockControl == null && this.rectangle_1.Contains(e.X, e.Y))
				{
					dockControl = this.SelectedControl;
				}
				if (dockControl != null && base.IsInContainer)
				{
					Point point = new Point(e.X, e.Y);
					point = dockControl.Parent.PointToScreen(point);
					point = dockControl.PointToClient(point);
					base.DockContainer.ShowControlContextMenu(new ShowControlContextMenuEventArgs(dockControl, point, ContextMenuContext.RightClick));
					return;
				}
			}
			if ((e.Button & MouseButtons.Middle) == MouseButtons.Middle && base.IsInContainer && base.DockContainer.Manager != null && base.DockContainer.Manager.AllowMiddleButtonClosure)
			{
				DockControl controlAt = this.GetControlAt(new Point(e.X, e.Y));
				if (controlAt != null && controlAt.AllowClose)
				{
					controlAt.method_14(true);
				}
				return;
			}
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left && this.Class17_0 != null)
			{
				this.vmethod_8(this.Class17_0);
				this.bool_2 = false;
				this.vmethod_9();
			}
		}

		protected virtual void OnPinButtonClick()
		{
			Collapsed = !Collapsed;
			if (base.IsInContainer && this.SelectedControl != null)
			{
				if (this.Collapsed && this.AutoHideBar != null)
				{
					this.AutoHideBar.method_7(this.SelectedControl, true, false);
					this.AutoHideBar.method_6(false);
					return;
				}
				this.SelectedControl.Activate();
			}
		}

		public void SplitForLayoutSystem(LayoutSystemBase layoutSystem, DockSide side)
		{
		    if (layoutSystem == null)
		        throw new ArgumentNullException(nameof(layoutSystem));
		    if (side == DockSide.None)
		        throw new ArgumentException(nameof(side));
		    if (layoutSystem.Parent != null)
		        throw new InvalidOperationException(
		            "This layout system must be removed from its parent before it can be moved to a new layout system.");
		    if (Parent == null)
		        throw new InvalidOperationException("This layout system is not parented yet.");

            SplitLayoutSystem parent = base.Parent;
			if (parent.SplitMode != Orientation.Horizontal)
			{
				if (parent.SplitMode == Orientation.Vertical)
				{
					if (side == DockSide.Left || side == DockSide.Right)
					{
						this.method_12(layoutSystem, (side == DockSide.Left) ? parent.LayoutSystems.IndexOf(this) : (parent.LayoutSystems.IndexOf(this) + 1), false);
						return;
					}
					this.method_13(layoutSystem, Orientation.Horizontal, side == DockSide.Top);
				}
				return;
			}
			if (side != DockSide.Top && side != DockSide.Bottom)
			{
				this.method_13(layoutSystem, Orientation.Vertical, side == DockSide.Left);
				return;
			}
			this.method_12(layoutSystem, (side == DockSide.Top) ? parent.LayoutSystems.IndexOf(this) : (parent.LayoutSystems.IndexOf(this) + 1), true);
		}

		internal override void OnDockingManagerFinished(Class7.DockTarget dockTarget_0)
		{
			base.OnDockingManagerFinished(dockTarget_0);
			if (dockTarget_0 == null || dockTarget_0.type == Class7.DockTargetType.None || dockTarget_0.type == Class7.DockTargetType.AlreadyActioned)
			{
				return;
			}
			DockControl selectedControl = this.SelectedControl;
			SandDockManager manager = base.DockContainer.Manager;
			if (!this.bool_5)
			{
				LayoutUtilities.smethod_11(selectedControl);
			}
			else
			{
				LayoutUtilities.smethod_10(this);
			}
			if (dockTarget_0.type != Class7.DockTargetType.Float)
			{
				if (dockTarget_0.dockContainer != null || dockTarget_0.type == Class7.DockTargetType.CreateNewContainer)
				{
					this.method_11(manager, selectedControl, this.bool_5, dockTarget_0);
				    selectedControl?.Activate();
				}
				return;
			}
			selectedControl.MetaData.SaveFloatingWindowGuid(Guid.NewGuid());
			if (!this.bool_5)
			{
				selectedControl.OpenFloating(dockTarget_0.Bounds, WindowOpenMethod.OnScreenActivate);
				return;
			}
			this.Float(manager, dockTarget_0.Bounds, WindowOpenMethod.OnScreenActivate);
		}

		internal override void vmethod_1(object sender, EventArgs e)
		{
			base.vmethod_1(sender, e);
			this.point_0 = Point.Empty;
		}

		internal override void vmethod_2(DockContainer container)
		{
			if (container == null && base.IsInContainer)
			{
				foreach (DockControl dockControl in this.Controls)
				{
					if (dockControl.Parent == base.DockContainer)
					{
						LayoutUtilities.smethod_8(dockControl);
					}
				}
			}
			if (container != null && !base.IsInContainer)
			{
				foreach (DockControl dockControl2 in this.Controls)
				{
					if (dockControl2.Parent != null)
					{
						LayoutUtilities.smethod_8(dockControl2);
					}
					dockControl2.Location = new Point(container.Width, container.Height);
					if (!this.Collapsed || !container.Boolean_6)
					{
						dockControl2.Parent = container;
					}
				}
			}
			base.vmethod_2(container);
			foreach (DockControl dockControl3 in this.Controls)
			{
				dockControl3.method_4(container);
			}
			if (this.Collapsed)
			{
				if (container?.Manager != null && this.AutoHideBar == null)
				{
					AutoHideBar autoHideBar = container.Manager.GetAutoHideBar(container.Dock);
					if (autoHideBar != null)
					{
						autoHideBar.LayoutSystems.Add(this);
						return;
					}
				}
				else
				{
				    this.AutoHideBar?.LayoutSystems.Remove(this);
				}
			}
		}

		internal override bool vmethod_3(ContainerDockLocation containerDockLocation_0)
		{
		    return Controls.Cast<DockControl>().All(control => control.method_13(containerDockLocation_0));

		    //IEnumerator enumerator = Controls.GetEnumerator();
            //bool result;
            //try
            //{
            //	while (enumerator.MoveNext())
            //	{
            //		DockControl dockControl = (DockControl)enumerator.Current;
            //		if (!dockControl.method_13(containerDockLocation_0))
            //		{
            //			result = false;
            //			return result;
            //		}
            //	}
            //	return true;
            //}
            //finally
            //{
            //	IDisposable disposable = enumerator as IDisposable;
            //	if (disposable != null)
            //	{
            //		disposable.Dispose();
            //	}
            //}
            //return result;
		}

        internal override void vmethod_4(RendererBase renderer, Graphics graphics_0, Font font)
		{
		    if (DockContainer == null) return;
		    var container = DockContainer.IsFloating || DockContainer.Manager?.DockSystemContainer == null ? DockContainer : DockContainer.Manager.DockSystemContainer;
			bool focused;
			if (IsInContainer && DockContainer.Boolean_0)
			{
				var selectionService = (ISelectionService)DockContainer.method_1(typeof(ISelectionService));
				focused = selectionService.GetComponentSelected(SelectedControl);
			}
			else
			{
				focused = Boolean_1;
			}
		    renderer.DrawControlClientBackground(graphics_0, this.rectangle_3,SelectedControl?.BackColor ?? SystemColors.Control);
		    if ((this.Controls.Count > 1 || base.DockContainer.Boolean_0) && this.rectangle_2 != Rectangle.Empty)
			{
				int selectedTabOffset = 0;
				if (this._selectedControl != null)
				{
					Rectangle rectangle_ = this._selectedControl.TabBounds;
					selectedTabOffset = rectangle_.X - base.Bounds.Left;
				}
				renderer.DrawTabStripBackground(container, base.DockContainer, graphics_0, this.rectangle_2, selectedTabOffset);
				foreach (DockControl dockControl in this.Controls)
				{
					DrawItemState drawItemState = DrawItemState.Default;
					if (this._selectedControl == dockControl)
					{
						drawItemState |= DrawItemState.Selected;
					}
					bool drawSeparator = true;
					if (this._selectedControl != null)
					{
						if (this.Controls.IndexOf(dockControl) == this.Controls.IndexOf(this._selectedControl) - 1)
						{
							drawSeparator = false;
						}
					}
					if (this.Controls.IndexOf(dockControl) == this.Controls.Count - 1 && renderer is WhidbeyRenderer)
					{
						drawSeparator = false;
					}
					renderer.DrawTabStripTab(graphics_0, dockControl.TabBounds, dockControl.Image_0, dockControl.TabText, dockControl.Font, dockControl.BackColor, dockControl.ForeColor, drawItemState, drawSeparator);
				}
			}
			Rectangle rectangle = this.rectangle_1;
			if (rectangle != Rectangle.Empty && rectangle.Width > 0 && rectangle.Height > 0)
			{
				renderer.DrawTitleBarBackground(graphics_0, rectangle, focused);
				if (this.class17_0.bool_0)
				{
					rectangle.Width -= 21;
				}
				if (this.class17_1.bool_0)
				{
					rectangle.Width -= 21;
				}
				if (this.class17_2.bool_0)
				{
					rectangle.Width -= 21;
				}
				rectangle = renderer.TitleBarMetrics.RemovePadding(rectangle);
				if (rectangle.Width > 8)
				{
					renderer.DrawTitleBarText(graphics_0, rectangle, focused, (this._selectedControl == null) ? "Empty Layout System" : this._selectedControl.Text, (this._selectedControl != null) ? this._selectedControl.Font : base.DockContainer.Font);
				}
				if (this.class17_0.bool_0 && this.class17_0.Bounds.Left > this.rectangle_1.Left)
				{
					DrawItemState drawItemState2 = DrawItemState.Default;
					if (this.Class17_0 == this.class17_0)
					{
						drawItemState2 |= DrawItemState.HotLight;
						if (this.bool_2)
						{
							drawItemState2 |= DrawItemState.Selected;
						}
					}
					renderer.DrawTitleBarButton(graphics_0, this.class17_0.Bounds, SandDockButtonType.Close, drawItemState2, focused, false);
				}
				if (this.class17_1.bool_0 && this.class17_1.Bounds.Left > this.rectangle_1.Left)
				{
					DrawItemState drawItemState2 = DrawItemState.Default;
					if (this.Class17_0 == this.class17_1)
					{
						drawItemState2 |= DrawItemState.HotLight;
						if (this.bool_2)
						{
							drawItemState2 |= DrawItemState.Selected;
						}
					}
					renderer.DrawTitleBarButton(graphics_0, this.class17_1.Bounds, SandDockButtonType.Pin, drawItemState2, focused, this.Collapsed);
				}
				if (this.class17_2.bool_0 && this.class17_2.Bounds.Left > this.rectangle_1.Left)
				{
					DrawItemState drawItemState2 = DrawItemState.Default;
					if (this.Class17_0 == this.class17_2)
					{
						drawItemState2 |= DrawItemState.HotLight;
						if (this.bool_2)
						{
							drawItemState2 |= DrawItemState.Selected;
						}
					}
					renderer.DrawTitleBarButton(graphics_0, this.class17_2.Bounds, SandDockButtonType.WindowPosition, drawItemState2, focused, false);
				}
			}
		}

		internal virtual string vmethod_5(Point point)
		{
			var control = GetControlAt(point);
			if (control == null)
			{
				Class17 @class = this.vmethod_6(point.X, point.Y);
				if (@class == this.class17_0)
				{
					return SandDockLanguage.CloseText;
				}
				if (@class == this.class17_1)
				{
					return SandDockLanguage.AutoHideText;
				}
				if (@class == this.class17_2)
				{
					return SandDockLanguage.WindowPositionText;
				}
				return "";
			}
			else
			{
				if (control.ToolTipText.Length != 0)
				{
					return control.ToolTipText;
				}
				if (!control.bool_3)
				{
					return "";
				}
				return control.Text;
			}
		}

		internal virtual Class17 vmethod_6(int x, int y)
		{
		    if (this.class17_0.bool_0 && this.class17_0.Bounds.Contains(x, y))
		        return this.class17_0;
		    if (this.class17_1.bool_0 && this.class17_1.Bounds.Contains(x, y))
		        return this.class17_1;
		    if (this.class17_2.bool_0 && this.class17_2.Bounds.Contains(x, y))
		        return this.class17_2;
		    return null;
		}

		internal virtual void vmethod_7(Class17 class17_4)
		{
		}

		internal virtual void vmethod_8(Class17 class17_4)
		{
		    if (this.Class17_0 == this.class17_0)
			{
				OnCloseButtonClick(EventArgs.Empty);
				return;
			}
		    if (this.Class17_0 != this.class17_1)
		    {
		        if (this.Class17_0 == this.class17_2)
		        {
		            this.method_7();
		        }
		    }
		    else
		    {
		        OnPinButtonClick();
		    }
		}

		internal virtual void vmethod_9()
		{
			if (AutoHideBar != null)
			{
				if (AutoHideBar.LayoutSystem == this)
				{
					AutoHideBar.method_9(this.rectangle_1);
				}
			}
			else if (IsInContainer)
			{
				DockContainer.Invalidate(this.rectangle_1);
			}
		}

		private bool AllowCollapse => Controls.Cast<DockControl>().All(control => control.AllowCollapse);

	    internal bool Boolean_1
		{
			get
			{
				return this.bool_6;
			}
			set
			{
				if (value != this.bool_6)
				{
					this.bool_6 = value;
					this.vmethod_9();
				}
			}
		}

		internal override bool PersistState => Controls.Cast<DockControl>().Any(control => control.PersistState);

	    internal override bool AllowFloat => Controls.Cast<DockControl>().All(control => control.DockingRules.AllowFloat);

	    internal override bool AllowTab=> Controls.Cast<DockControl>().All(control => control.DockingRules.AllowTab);

		internal Class17 Class17_0
		{
			get
			{
				return this.class17_3;
			}
			set
			{
			    if (value == this.class17_3) return;

			    if (this.class17_3 != null)
			        this.vmethod_9();
			    this.class17_3 = value;
			    if (this.class17_3 != null)
			        this.vmethod_9();
			}
		}

		[Browsable(false), DefaultValue(false)]
		public virtual bool Collapsed
		{
			get
			{
				return _collapsed;
			}
			set
			{
			    if (_collapsed == value) return;
			    _collapsed = value;

				this.Class17_0 = null;
				if (_collapsed)
				{
					if (IsInContainer)
					{
						foreach (DockControl dockControl in Controls)
						{
							if (dockControl.Parent == DockContainer)
							{
								LayoutUtilities.smethod_8(dockControl);
							}
						}
						var autoHideBar = DockContainer.Manager.GetAutoHideBar(DockContainer.Dock);
					    autoHideBar?.LayoutSystems.Add(this);
					}
				}
				else
				{
				    AutoHideBar?.LayoutSystems.Remove(this);
				    foreach (DockControl dockControl2 in Controls)
					{
						if (dockControl2.Parent != DockContainer)
						{
							dockControl2.Parent = DockContainer;
						}
					}
				}
			    if (IsInContainer)
			        DockContainer.vmethod_2();
			}
		}

		internal AutoHideBar AutoHideBar { get; private set; }

	    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DockControlCollection Controls { get; }

	    internal Control Control_0
		{
			get
			{
			    if (!IsInContainer)
			        return null;
			    if (!IsPoppedUp)
			        return DockContainer;
			    return AutoHideBar.Control_0;
			}
		}

		internal override DockControl[] DockControls
		{
			get
			{
				var array = new DockControl[Controls.Count];
				Controls.CopyTo(array, 0);
				return array;
			}
		}

		internal Guid Guid { get; set; } = Guid.NewGuid();

	    internal int PopupSize
		{
			get
			{
			    if (SelectedControl != null && SelectedControl.PopupSize != 0)
			        return SelectedControl.PopupSize;
			    return IsInContainer ? DockContainer.ContentSize : 200;
			}
			set
			{
			    foreach (DockControl dockControl in Controls)
			        dockControl.PopupSize = value;
			}
		}

		public bool IsPoppedUp => AutoHideBar != null && AutoHideBar.LayoutSystem == this;

	    public bool LockControls { get; set; }

	    internal Rectangle Rectangle_0 => this.rectangle_4;

	    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual DockControl SelectedControl
		{
			get
			{
				return _selectedControl;
			}
			set
			{
			    if (value != null && !Controls.Contains(value))
			        throw new ArgumentOutOfRangeException(nameof(value));
			    if ((SelectedControl?.Manager?.RaiseValidationEvents ?? false) && !SelectedControl.ValidateChildren())
			        return;

			    DockControl dockControl = this._selectedControl;
				this._selectedControl = value;
				this.CalculateAllMetricsAndLayout();
				if (IsPoppedUp)
				{
				    dockControl?.OnAutoHidePopupClosed(EventArgs.Empty);
				    this._selectedControl?.OnAutoHidePopupOpened(EventArgs.Empty);
				}
				this.method_5(dockControl, _selectedControl);
			}
		}

	    internal event SelectionChangedEventHandler SelectionChanged;

		private bool _collapsed;

		private bool bool_1;

		internal bool bool_2;

	    internal bool bool_4;

		private bool bool_5;

		private bool bool_6;

		private Class17 class17_0;

		private Class17 class17_1;

		private Class17 class17_2;

		private Class17 class17_3;

	    //private Delegate2 delegate2_0;

	    private DockControl _selectedControl;

	    private const int int_2 = 19;

		private const int int_3 = 15;

		private Point point_0 = Point.Empty;

		internal Rectangle rectangle_1;

		internal Rectangle rectangle_2;

		internal Rectangle rectangle_3;

		internal Rectangle rectangle_4;

		internal delegate void SelectionChangedEventHandler(DockControl oldSelection, DockControl newSelection);

	}
}
