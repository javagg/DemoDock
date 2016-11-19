using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TD.SandDock.Rendering;
using TD.Util;

namespace TD.SandDock
{
    internal class ControlLayoutSystemConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException();
            if (destinationType != typeof(InstanceDescriptor))
                return base.ConvertTo(context, culture, value, destinationType);
            if (value.GetType().Name != "ControlLayoutSystem" && value.GetType().Name != "DocumentLayoutSystem")
                return base.ConvertTo(context, culture, value, destinationType);

            Type type = value.GetType();
            type.Assembly.GetType("TD.SandDock.LayoutSystemBase");
            Type type2 = type.Assembly.GetType("TD.SandDock.DockControl");
            ConstructorInfo constructor = type.GetConstructor(new[] { typeof(SizeF), MakeArrayType(type2), type2 });
            PropertyInfo property = type.GetProperty("Controls", BindingFlags.Instance | BindingFlags.Public);
            ICollection collection = (ICollection)property.GetValue(value, null);
            object[] array = (object[])Activator.CreateInstance(MakeArrayType(type2), collection.Count);
            collection.CopyTo(array, 0);
            PropertyInfo property2 = type.GetProperty("WorkingSize", BindingFlags.Instance | BindingFlags.Public);
            SizeF sizeF = (SizeF)property2.GetValue(value, null);
            PropertyInfo property3 = type.GetProperty("SelectedControl", BindingFlags.Instance | BindingFlags.Public);
            object value2 = property3.GetValue(value, null);
            return new InstanceDescriptor(constructor, new[] { sizeF, array, value2 });
        }

        private Type MakeArrayType(Type firstType)
        {
            return firstType.Assembly.GetType(firstType.FullName + "[]");
        }
    }

    [TypeConverter(typeof(ControlLayoutSystemConverter))]
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
                bool_0 = true;
                foreach (var control in controls)
                    Add(control);
                bool_0 = false;
                _parent.CalculateAllMetricsAndLayout();
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
                if (_updating) return;
                DockControl dockControl = (DockControl)value;
                dockControl.method_16(_parent);
                if (_parent.IsInContainer && _parent.DockContainer.Manager != null &&
                    _parent.DockContainer.Manager != dockControl.Manager)
                    dockControl.Manager = _parent.DockContainer.Manager;
                if (_parent.IsInContainer)
                    dockControl.method_4(_parent.DockContainer);
                if (_parent.IsInContainer)
                {
                    if (dockControl.Parent != null)
                        LayoutUtilities.smethod_8(dockControl);
                    dockControl.Parent = _parent.Control_0;
                }
                if (_parent._selectedControl == null)
                    _parent.SelectedControl = dockControl;
                _parent.DockContainer?.vmethod_0();
                if (!bool_0)
                    _parent.CalculateAllMetricsAndLayout();
            }

            protected override void OnRemoveComplete(int index, object value)
            {
                base.OnRemoveComplete(index, value);
                if (_updating) return;

                DockControl dockControl = (DockControl)value;
                dockControl.method_16(null);
                dockControl.method_5();
                if (dockControl.Parent != null)
                {
                    if (dockControl.Parent == _parent.Control_0)
                    {
                        LayoutUtilities.smethod_8(dockControl);
                    }
                }
                if (_parent._selectedControl == value)
                    _parent.SelectedControl = _parent.Controls.Count != 0 ? this[0] : null;
                _parent.DockContainer?.vmethod_0();
                _parent.CalculateAllMetricsAndLayout();
            }

            public void Remove(DockControl control)
            {
                if (control == null) throw new ArgumentNullException(nameof(control));
                List.Remove(control);
            }

            public void SetChildIndex(DockControl control, int index)
            {
                if (control == null) throw new ArgumentNullException(nameof(control));
                if (!Contains(control)) throw new ArgumentOutOfRangeException(nameof(control));
                if (index == IndexOf(control)) return;

                if (IndexOf(control) < index)
                    index--;
                _updating = true;
                List.Remove(control);
                List.Insert(index, control);
                _updating = false;
                _parent.CalculateAllMetricsAndLayout();
            }

            public DockControl this[int index] => (DockControl)List[index];

            private bool bool_0;

            private bool _updating;

            private readonly ControlLayoutSystem _parent;
        }

        public ControlLayoutSystem()
		{
			Controls = new DockControlCollection(this);
			_closeDockButton = new DockButtonInfo();
			_pinDockButton = new DockButtonInfo();
			_positionDockButton = new DockButtonInfo();
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

		public ControlLayoutSystem(int desiredWidth, int desiredHeight, DockControl[] controls, DockControl selectedControl, bool collapsed) : this(new SizeF(desiredWidth, desiredHeight), controls, selectedControl)
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
				method_17();
				bounds.Offset(0, renderer.TitleBarMetrics.Height);
				bounds.Height -= renderer.TitleBarMetrics.Height;
			}
			if (Controls.Count <= 1 && !DockContainer.IsInDesign)
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

            DockControl selectedControl = SelectedControl;
			while (Controls.Count != 0)
			{
				DockControl control = Controls[0];
				Controls.RemoveAt(0);
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
			if (rectangle_2.Contains(position) && !_closeDockButton.Bounds.Contains(position) && !_pinDockButton.Bounds.Contains(position))
			{
			    return Controls.Cast<DockControl>().FirstOrDefault(c => c.TabBounds.Contains(position));
			}
		    return null;
		}

		protected internal override void Layout(RendererBase renderer, Graphics graphics, Rectangle bounds, bool floating)
		{
			base.Layout(renderer, graphics, bounds, floating);
			method_18();
		    if (Collapsed && DockContainer.Boolean_6)
		        return;

		    CalculateLayout(renderer, bounds, floating, out rectangle_1, out rectangle_2, out rectangle_3, out rectangle_4);
			bool_4 = true;
			try
			{
				if (rectangle_1 != Rectangle.Empty)
				{
					method_17();
				}
				method_19(renderer, graphics, rectangle_2);
				foreach (DockControl dockControl in Controls)
				{
					if (dockControl != SelectedControl)
					{
						dockControl.method_0(false);
					}
				}
				foreach (DockControl dockControl2 in Controls)
				{
					if (dockControl2 == SelectedControl)
					{
						Rectangle bounds2 = renderer.AdjustDockControlClientBounds(this, dockControl2, rectangle_3);
						dockControl2.Bounds = bounds2;
						dockControl2.method_0(true);
					}
				}
			}
			finally
			{
				bool_4 = false;
			}
		}

		protected internal virtual void LayoutCollapsed(RendererBase renderer, Rectangle bounds)
		{
			rectangle_1 = bounds;
			rectangle_1.Offset(0, renderer.TitleBarMetrics.Margin.Top);
			rectangle_1.Height = renderer.TitleBarMetrics.Height - (renderer.TitleBarMetrics.Margin.Top + renderer.TitleBarMetrics.Margin.Bottom);
			method_17();
			bounds.Offset(0, renderer.TitleBarMetrics.Height);
			bounds.Height -= renderer.TitleBarMetrics.Height;
			rectangle_3 = bounds;
			rectangle_2 = Rectangle.Empty;
			foreach (DockControl dockControl in Controls)
			{
				Rectangle bounds2 = renderer.AdjustDockControlClientBounds(this, dockControl, rectangle_3);
				dockControl.method_0(dockControl == _selectedControl);
				dockControl.Bounds = bounds2;
			}
		}

		internal void method_10(Graphics g, RendererBase renderer, DockButtonInfo class17_4, SandDockButtonType buttonType, bool bool_7)
		{
		    if (!class17_4.Visible) return;

		    var state = DrawItemState.Default;
		    if (ClickedDockButton == class17_4)
		    {
		        state |= DrawItemState.HotLight;
		        if (bool_2)
		        {
		            state |= DrawItemState.Selected;
		        }
		    }
		    if (!bool_7)
		    {
		        state |= DrawItemState.Disabled;
		    }
		    renderer.DrawDocumentStripButton(g, class17_4.Bounds, buttonType, state);
		}

		internal void method_11(SandDockManager manager, DockControl dockControl_1, bool bool_7, Class7.DockTarget dockTarget_0)
		{
			if (dockTarget_0.type == Class7.DockTargetType.JoinExistingSystem)
			{
				if (!bool_7)
				{
					dockControl_1.method_15(dockTarget_0.layoutSystem, dockTarget_0.index);
					return;
				}
				Dock(dockTarget_0.layoutSystem, dockTarget_0.index);
			}
			else
			{
				if (dockTarget_0.type != Class7.DockTargetType.CreateNewContainer)
				{
					if (dockTarget_0.type == Class7.DockTargetType.SplitExistingSystem)
					{
						ControlLayoutSystem controlLayoutSystem = dockTarget_0.dockContainer.CreateNewLayoutSystem(bool_7 ? DockControls : new[]
						{
							dockControl_1
						}, WorkingSize);
						dockTarget_0.layoutSystem.SplitForLayoutSystem(controlLayoutSystem, dockTarget_0.DockSide);
					}
					return;
				}
				var dockContainer = manager.FindDockedContainer(DockStyle.Fill);
				if (dockTarget_0.DockLocation == ContainerDockLocation.Center && dockContainer != null)
				{
					ControlLayoutSystem controlLayoutSystem = LayoutUtilities.FindControlLayoutSystem(dockContainer);
					if (controlLayoutSystem != null)
					{
						if (bool_7)
						{
							Dock(controlLayoutSystem);
							return;
						}
						dockControl_1.method_15(controlLayoutSystem, 0);
					}
				}
				else
				{
					if (bool_7)
					{
						method_2(manager, dockTarget_0.DockLocation, dockTarget_0.middle ? ContainerDockEdge.Inside : ContainerDockEdge.Outside);
						return;
					}
					dockControl_1.DockInNewContainer(dockTarget_0.DockLocation, dockTarget_0.middle ? ContainerDockEdge.Inside : ContainerDockEdge.Outside);
				}
			}
		}

		private void method_12(LayoutSystemBase layoutSystem, int index, bool bool_7)
		{
			var parent = Parent;
			parent.LayoutSystems.updating = true;
			parent.LayoutSystems.Insert(index, layoutSystem);
			parent.LayoutSystems.updating = false;
			parent.method_7();
		}

		private void method_13(LayoutSystemBase layoutSystem, Orientation splitMode, bool prepend)
		{
			var parent = Parent;
		    var splitLayout = new SplitLayoutSystem
		    {
		        SplitMode = splitMode,
		        WorkingSize = WorkingSize
		    };
		    var index = parent.LayoutSystems.IndexOf(this);
			parent.LayoutSystems.updating = true;
			parent.LayoutSystems.Remove(this);
			parent.LayoutSystems.Insert(index, splitLayout);
			parent.LayoutSystems.updating = false;
			splitLayout.LayoutSystems.Add(this);
		    if (prepend)
		        splitLayout.LayoutSystems.Insert(0, layoutSystem);
		    else
		        splitLayout.LayoutSystems.Add(layoutSystem);
		    parent.method_7();
		}

		internal void method_14(DockSituation dockSituation)
		{
		    if (Controls.Count == 0) throw new InvalidOperationException();
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
		    var array2 = new DockControl[array.Length - 1];
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
		    if (!IsInContainer) return;
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

		private void method_17()
		{
			if (_selectedControl == null)
			{
				_closeDockButton.Visible = false;
				_pinDockButton.Visible = false;
				_positionDockButton.Visible = false;
				return;
			}
			int y = rectangle_1.Top + rectangle_1.Height / 2 - 7;
			int num = rectangle_1.Right - 2;
			if (_selectedControl.AllowClose)
			{
				_closeDockButton.Visible = true;
				_closeDockButton.Bounds = new Rectangle(num - 19, y, 19, 15);
				num -= 21;
			}
			else
			{
				_closeDockButton.Visible = false;
			}
			if (!AllowCollapse || (IsInContainer && !DockContainer.Boolean_6))
			{
				_pinDockButton.Visible = false;
			}
			else
			{
				_pinDockButton.Visible = true;
				_pinDockButton.Bounds = new Rectangle(num - 19, y, 19, 15);
				num -= 21;
			}
			if (!_selectedControl.ShowOptions)
			{
				_positionDockButton.Visible = false;
				return;
			}
			_positionDockButton.Visible = true;
			_positionDockButton.Bounds = new Rectangle(num - 19, y, 19, 15);
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
					array[i] -= (int)(num5 * (array[i] / (float)num));
					Controls[i].bool_3 = true;
				}
			}
			bounds = renderer.TabStripMetrics.RemovePadding(bounds);
			int num6 = bounds.Left;
			num3 = 0;
			for (int j = 0; j < Controls.Count; j++)
			{
				DockControl dockControl2 = Controls[j];
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

		private void method_5(DockControl oldSelection, DockControl newSelection)
		{
            SelectionChanged?.Invoke(oldSelection, newSelection);
		}

	    private void method_6()
		{
			switch (SelectedControl.DockSituation)
			{
			case DockSituation.Docked:
			case DockSituation.Document:
				if (AllowFloat)
				{
					method_14(DockSituation.Floating);
				}
				break;
			case DockSituation.Floating:
				if (SelectedControl.MetaData.LastFixedDockSituation == DockSituation.Docked)
				{
					if (vmethod_3(SelectedControl.MetaData.LastFixedDockSide))
					{
						method_14(DockSituation.Docked);
						return;
					}
				}
				if (SelectedControl.MetaData.LastFixedDockSituation == DockSituation.Document)
				{
					if (vmethod_3(ContainerDockLocation.Center))
					{
						method_14(DockSituation.Document);
					}
				}
				break;
			default:
				return;
			}
		}

		private void OnPositionButtonClick()
		{
			var point = new Point(_positionDockButton.Bounds.Left, _positionDockButton.Bounds.Bottom);
			point = SelectedControl.Parent.PointToScreen(point);
			point = SelectedControl.PointToClient(point);
			DockContainer.ShowControlContextMenu(new ShowControlContextMenuEventArgs(SelectedControl, point, ContextMenuContext.OptionsButton));
		}

		internal bool method_8()
		{
		    if (!IsInContainer || SelectedControl == null || !SelectedControl.ContainsFocus) return false;

            Boolean_1 = true;
		    if (SelectedControl != null)
		        DockContainer.Manager.OnDockControlActivated(new DockControlEventArgs(SelectedControl));
		    return true;
		}

		internal void method_9()
		{
			Boolean_1 = false;
		}

		protected virtual void OnCloseButtonClick(EventArgs e)
		{
		    SelectedControl?.method_14(true);
		}

	    protected internal override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);
			var control = GetControlAt(DockContainer.PointToClient(new Point(drgevent.X, drgevent.Y)));
	        if (control != null && SelectedControl != control)
	            control.Open(WindowOpenMethod.OnScreenActivate);
		}

		protected internal override void OnMouseDoubleClick()
		{
			var point = DockContainer.PointToClient(Cursor.Position);
		    if (DockContainer.Manager == null) return;
		    if (LockControls) return;

		    if (rectangle_1.Contains(point) && !_closeDockButton.Bounds.Contains(point) && !_pinDockButton.Bounds.Contains(point) && Controls.Count != 0)
		    {
		        method_6();
		        return;
		    }
            GetControlAt(point)?.OnTabDoubleClick();
		}

		protected internal override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool_1 = false;
			if (rectangle_1.Contains(e.X, e.Y))
			{
				SelectedControl?.Activate();
			}
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				if (rectangle_1.Contains(e.X, e.Y))
				{
					point_0 = new Point(e.X, e.Y);
				}
				if (ClickedDockButton != null)
				{
					bool_2 = true;
					OnLeave();
					vmethod_7(ClickedDockButton);
					point_0 = Point.Empty;
					return;
				}
			}
			var controlAt = GetControlAt(new Point(e.X, e.Y));
		    if (controlAt == null) return;
		    controlAt.Activate();
		    bool_1 = true;
		    if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
		    {
		        point_0 = new Point(e.X, e.Y);
		    }
		}

		protected internal override void OnMouseLeave()
		{
			base.OnMouseLeave();
			ClickedDockButton = null;
			bool_2 = false;
		}

		protected internal override void OnMouseMove(MouseEventArgs e)
		{
		    if (bool_4)
		        return;
		    if (e.Button == MouseButtons.None)
			{
				bool_1 = false;
			}
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				if (class7_0 != null)
				{
					class7_0.OnMouseMove(Cursor.Position);
					return;
				}
				Rectangle rectangle = new Rectangle(point_0, new Size(0, 0));
				rectangle.Inflate(SystemInformation.DragSize);
				if (!rectangle.Contains(e.X, e.Y) && IsInContainer && point_0 != Point.Empty && !Collapsed && !LockControls)
				{
					DockControl controlAt = GetControlAt(point_0);
					bool_5 = (controlAt == null);
					DockingHints dockingHints_;
					if (DockContainer.Manager == null)
					{
						dockingHints_ = DockingHints.TranslucentFill;
					}
					else
					{
						dockingHints_ = DockContainer.Manager.DockingHints;
					}
					DockingManager dockingManager_;
					if (DockContainer.Manager == null)
					{
						dockingManager_ = DockingManager.Standard;
					}
					else
					{
						dockingManager_ = DockContainer.Manager.DockingManager;
					}
					method_0(DockContainer.Manager, DockContainer, this, controlAt, SelectedControl.MetaData.DockedContentSize, point_0, dockingHints_, dockingManager_);
					return;
				}
			}
			if (!bool_1)
			{
				ClickedDockButton = GetDockButtonAt(e.X, e.Y);
			}
		}

		protected internal override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			point_0 = Point.Empty;
			bool_1 = false;
			if (class7_0 != null)
			{
				class7_0.Commit();
				return;
			}
			if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
			{
				DockControl dockControl = GetControlAt(new Point(e.X, e.Y));
				if (dockControl == null && rectangle_1.Contains(e.X, e.Y))
				{
					dockControl = SelectedControl;
				}
				if (dockControl != null && IsInContainer)
				{
					Point point = new Point(e.X, e.Y);
					point = dockControl.Parent.PointToScreen(point);
					point = dockControl.PointToClient(point);
					DockContainer.ShowControlContextMenu(new ShowControlContextMenuEventArgs(dockControl, point, ContextMenuContext.RightClick));
					return;
				}
			}
			if ((e.Button & MouseButtons.Middle) == MouseButtons.Middle && IsInContainer && DockContainer.Manager != null && DockContainer.Manager.AllowMiddleButtonClosure)
			{
				DockControl controlAt = GetControlAt(new Point(e.X, e.Y));
				if (controlAt != null && controlAt.AllowClose)
				{
					controlAt.method_14(true);
				}
				return;
			}
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left && ClickedDockButton != null)
			{
				vmethod_8(ClickedDockButton);
				bool_2 = false;
				OnLeave();
			}
		}

		protected virtual void OnPinButtonClick()
		{
			Collapsed = !Collapsed;
		    if (!IsInContainer || SelectedControl == null) return;
		    if (Collapsed && AutoHideBar != null)
		    {
		        AutoHideBar.method_7(SelectedControl, true, false);
		        AutoHideBar.method_6(false);
		    }
		    else
		        SelectedControl.Activate();
		}

		public void SplitForLayoutSystem(LayoutSystemBase layoutSystem, DockSide side)
		{
		    if (layoutSystem == null)
		        throw new ArgumentNullException(nameof(layoutSystem));
		    if (side == DockSide.None)
		        throw new ArgumentException(nameof(side));
		    if (layoutSystem.Parent != null)
		        throw new InvalidOperationException("This layout system must be removed from its parent before it can be moved to a new layout system.");
		    if (Parent == null)
		        throw new InvalidOperationException("This layout system is not parented yet.");

            var parent = Parent;
			if (parent.SplitMode != Orientation.Horizontal)
			{
				if (parent.SplitMode == Orientation.Vertical)
				{
					if (side == DockSide.Left || side == DockSide.Right)
					{
						method_12(layoutSystem, (side == DockSide.Left) ? parent.LayoutSystems.IndexOf(this) : (parent.LayoutSystems.IndexOf(this) + 1), false);
						return;
					}
					method_13(layoutSystem, Orientation.Horizontal, side == DockSide.Top);
				}
				return;
			}
			if (side != DockSide.Top && side != DockSide.Bottom)
			{
				method_13(layoutSystem, Orientation.Vertical, side == DockSide.Left);
				return;
			}
			method_12(layoutSystem, (side == DockSide.Top) ? parent.LayoutSystems.IndexOf(this) : (parent.LayoutSystems.IndexOf(this) + 1), true);
		}

		internal override void OnDockingManagerFinished(Class7.DockTarget dockTarget_0)
		{
			base.OnDockingManagerFinished(dockTarget_0);
			if (dockTarget_0 == null || dockTarget_0.type == Class7.DockTargetType.None || dockTarget_0.type == Class7.DockTargetType.AlreadyActioned)
			{
				return;
			}
			DockControl selectedControl = SelectedControl;
			SandDockManager manager = DockContainer.Manager;
			if (!bool_5)
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
					method_11(manager, selectedControl, bool_5, dockTarget_0);
				    selectedControl?.Activate();
				}
				return;
			}
			selectedControl.MetaData.SaveFloatingWindowGuid(Guid.NewGuid());
			if (!bool_5)
			{
				selectedControl.OpenFloating(dockTarget_0.Bounds, WindowOpenMethod.OnScreenActivate);
				return;
			}
			Float(manager, dockTarget_0.Bounds, WindowOpenMethod.OnScreenActivate);
		}

		internal override void OnCancalled(object sender, EventArgs e)
		{
			base.OnCancalled(sender, e);
			point_0 = Point.Empty;
		}

		internal override void SetDockContainer(DockContainer container)
		{
			if (container == null && IsInContainer)
			{
				foreach (DockControl dockControl in Controls)
				{
					if (dockControl.Parent == DockContainer)
					{
						LayoutUtilities.smethod_8(dockControl);
					}
				}
			}
			if (container != null && !IsInContainer)
			{
				foreach (DockControl dockControl2 in Controls)
				{
					if (dockControl2.Parent != null)
					{
						LayoutUtilities.smethod_8(dockControl2);
					}
					dockControl2.Location = new Point(container.Width, container.Height);
					if (!Collapsed || !container.Boolean_6)
					{
						dockControl2.Parent = container;
					}
				}
			}
			base.SetDockContainer(container);
			foreach (DockControl dockControl3 in Controls)
			{
				dockControl3.method_4(container);
			}
			if (Collapsed)
			{
				if (container?.Manager != null && AutoHideBar == null)
				{
					AutoHideBar autoHideBar = container.Manager.GetAutoHideBar(container.Dock);
					if (autoHideBar != null)
					{
						autoHideBar.LayoutSystems.Add(this);
					}
				}
				else
				{
				    AutoHideBar?.LayoutSystems.Remove(this);
				}
			}
		}

		internal override bool vmethod_3(ContainerDockLocation location) => Controls.Cast<DockControl>().All(control => control.GetDockingRulesFrom(location));

        internal override void vmethod_4(RendererBase renderer, Graphics g, Font font)
		{
		    if (DockContainer == null) return;
		    var container = DockContainer.IsFloating || DockContainer.Manager?.DockSystemContainer == null ? DockContainer : DockContainer.Manager.DockSystemContainer;
			bool focused;
			if (IsInContainer && DockContainer.IsInDesign)
			{
				var selectionService = (ISelectionService)DockContainer.method_1(typeof(ISelectionService));
				focused = selectionService.GetComponentSelected(SelectedControl);
			}
			else
			{
				focused = Boolean_1;
			}
		    renderer.DrawControlClientBackground(g, rectangle_3,SelectedControl?.BackColor ?? SystemColors.Control);
		    if ((Controls.Count > 1 || DockContainer.IsInDesign) && rectangle_2 != Rectangle.Empty)
			{
				int selectedTabOffset = 0;
				if (_selectedControl != null)
				{
					Rectangle rectangle_ = _selectedControl.TabBounds;
					selectedTabOffset = rectangle_.X - Bounds.Left;
				}
				renderer.DrawTabStripBackground(container, DockContainer, g, rectangle_2, selectedTabOffset);
				foreach (DockControl dockControl in Controls)
				{
					DrawItemState drawItemState = DrawItemState.Default;
					if (_selectedControl == dockControl)
					{
						drawItemState |= DrawItemState.Selected;
					}
					bool drawSeparator = true;
					if (_selectedControl != null)
					{
						if (Controls.IndexOf(dockControl) == Controls.IndexOf(_selectedControl) - 1)
						{
							drawSeparator = false;
						}
					}
					if (Controls.IndexOf(dockControl) == Controls.Count - 1 && renderer is WhidbeyRenderer)
					{
						drawSeparator = false;
					}
					renderer.DrawTabStripTab(g, dockControl.TabBounds, dockControl.CollapsedImage, dockControl.TabText, dockControl.Font, dockControl.BackColor, dockControl.ForeColor, drawItemState, drawSeparator);
				}
			}
			Rectangle rectangle = rectangle_1;
			if (rectangle != Rectangle.Empty && rectangle.Width > 0 && rectangle.Height > 0)
			{
				renderer.DrawTitleBarBackground(g, rectangle, focused);
				if (_closeDockButton.Visible)
				{
					rectangle.Width -= 21;
				}
				if (_pinDockButton.Visible)
				{
					rectangle.Width -= 21;
				}
				if (_positionDockButton.Visible)
				{
					rectangle.Width -= 21;
				}
				rectangle = renderer.TitleBarMetrics.RemovePadding(rectangle);
				if (rectangle.Width > 8)
				{
					renderer.DrawTitleBarText(g, rectangle, focused, (_selectedControl == null) ? "Empty Layout System" : _selectedControl.Text, (_selectedControl != null) ? _selectedControl.Font : DockContainer.Font);
				}
				if (_closeDockButton.Visible && _closeDockButton.Bounds.Left > rectangle_1.Left)
				{
					DrawItemState drawItemState2 = DrawItemState.Default;
					if (ClickedDockButton == _closeDockButton)
					{
						drawItemState2 |= DrawItemState.HotLight;
						if (bool_2)
						{
							drawItemState2 |= DrawItemState.Selected;
						}
					}
					renderer.DrawTitleBarButton(g, _closeDockButton.Bounds, SandDockButtonType.Close, drawItemState2, focused, false);
				}
				if (_pinDockButton.Visible && _pinDockButton.Bounds.Left > rectangle_1.Left)
				{
					DrawItemState drawItemState2 = DrawItemState.Default;
					if (ClickedDockButton == _pinDockButton)
					{
						drawItemState2 |= DrawItemState.HotLight;
						if (bool_2)
						{
							drawItemState2 |= DrawItemState.Selected;
						}
					}
					renderer.DrawTitleBarButton(g, _pinDockButton.Bounds, SandDockButtonType.Pin, drawItemState2, focused, Collapsed);
				}
				if (_positionDockButton.Visible && _positionDockButton.Bounds.Left > rectangle_1.Left)
				{
					DrawItemState drawItemState2 = DrawItemState.Default;
					if (ClickedDockButton == _positionDockButton)
					{
						drawItemState2 |= DrawItemState.HotLight;
						if (bool_2)
						{
							drawItemState2 |= DrawItemState.Selected;
						}
					}
					renderer.DrawTitleBarButton(g, _positionDockButton.Bounds, SandDockButtonType.WindowPosition, drawItemState2, focused, false);
				}
			}
		}

		internal virtual string vmethod_5(Point point)
		{
			var control = GetControlAt(point);
		    if (control != null) return control.ToolTipText.Length != 0 ? control.ToolTipText : (!control.bool_3 ? "" : control.Text);
		    var button = GetDockButtonAt(point.X, point.Y);
		    if (button == _closeDockButton)
		        return SandDockLanguage.CloseText;
		    if (button == _pinDockButton)
		        return SandDockLanguage.AutoHideText;
		    if (button == _positionDockButton)
		        return SandDockLanguage.WindowPositionText;
		    return "";
		}

        [GuessedName]
		internal virtual DockButtonInfo GetDockButtonAt(int x, int y)
		{
		    if (_closeDockButton.Visible && _closeDockButton.Bounds.Contains(x, y))
		        return _closeDockButton;
		    if (_pinDockButton.Visible && _pinDockButton.Bounds.Contains(x, y))
		        return _pinDockButton;
		    if (_positionDockButton.Visible && _positionDockButton.Bounds.Contains(x, y))
		        return _positionDockButton;
		    return null;
		}

		internal virtual void vmethod_7(DockButtonInfo class17_4)
		{
		}

		internal virtual void vmethod_8(DockButtonInfo button)
		{
		    if (ClickedDockButton == _closeDockButton)
		        OnCloseButtonClick(EventArgs.Empty);
		    else if (ClickedDockButton == _pinDockButton)
		        OnPinButtonClick();
		    else if (ClickedDockButton == _positionDockButton)
		        OnPositionButtonClick();
		}

		internal virtual void OnLeave()
		{
			if (AutoHideBar != null)
			{
				if (AutoHideBar.LayoutSystem == this)
				{
					AutoHideBar.method_9(rectangle_1);
				}
			}
			else if (IsInContainer)
			{
				DockContainer.Invalidate(rectangle_1);
			}
		}

		private bool AllowCollapse => Controls.Cast<DockControl>().All(control => control.AllowCollapse);

	    internal bool Boolean_1
		{
			get
			{
				return bool_6;
			}
			set
			{
			    if (bool_6 == value) return;
			    bool_6 = value;
			    OnLeave();
			}
		}

		internal override bool PersistState => Controls.Cast<DockControl>().Any(control => control.PersistState);

	    internal override bool AllowFloat => Controls.Cast<DockControl>().All(control => control.DockingRules.AllowFloat);

	    internal override bool AllowTab=> Controls.Cast<DockControl>().All(control => control.DockingRules.AllowTab);

		internal DockButtonInfo ClickedDockButton
		{
			get
			{
				return _clickedDockButton;
			}
			set
			{
			    if (value == _clickedDockButton) return;
			    if (_clickedDockButton != null)
			        OnLeave();
			    _clickedDockButton = value;
			    if (_clickedDockButton != null)
			        OnLeave();
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

				ClickedDockButton = null;
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

	    internal Rectangle Rectangle_0 => rectangle_4;

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

			    DockControl dockControl = _selectedControl;
				_selectedControl = value;
				CalculateAllMetricsAndLayout();
				if (IsPoppedUp)
				{
				    dockControl?.OnAutoHidePopupClosed(EventArgs.Empty);
				    _selectedControl?.OnAutoHidePopupOpened(EventArgs.Empty);
				}
				method_5(dockControl, _selectedControl);
			}
		}

	    internal event SelectionChangedEventHandler SelectionChanged;

		private bool _collapsed;

		private bool bool_1;

		internal bool bool_2;

	    internal bool bool_4;

		private bool bool_5;

		private bool bool_6;

		private readonly DockButtonInfo _closeDockButton;

		private DockButtonInfo _pinDockButton;

		private readonly DockButtonInfo _positionDockButton;

		private DockButtonInfo _clickedDockButton;

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
