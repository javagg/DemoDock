using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
	[TypeConverter(typeof(Class23))]
	public class ControlLayoutSystem : LayoutSystemBase
	{
		public ControlLayoutSystem()
		{
			this.Controls = new ControlLayoutSystem.DockControlCollection(this);
			this.class17_0 = new Class17();
			this.class17_1 = new Class17();
			this.class17_2 = new Class17();
		}

		public ControlLayoutSystem(int desiredWidth, int desiredHeight) : this()
		{
			base.WorkingSize = new SizeF((float)desiredWidth, (float)desiredHeight);
		}

		public ControlLayoutSystem(SizeF workingSize, DockControl[] windows, DockControl selectedWindow) : this()
		{
			base.WorkingSize = workingSize;
			this.Controls.AddRange(windows);
			if (selectedWindow != null)
			{
				this.SelectedControl = selectedWindow;
			}
		}

		[Obsolete("Use the constructor that takes a SizeF instead.")]
		public ControlLayoutSystem(int desiredWidth, int desiredHeight, DockControl[] controls, DockControl selectedControl) : this(desiredWidth, desiredHeight)
		{
			this.Controls.AddRange(controls);
			if (selectedControl != null)
			{
				this.SelectedControl = selectedControl;
			}
		}

		public ControlLayoutSystem(int desiredWidth, int desiredHeight, DockControl[] controls, DockControl selectedControl, bool collapsed) : this(new SizeF((float)desiredWidth, (float)desiredHeight), controls, selectedControl)
		{
			this.Collapsed = collapsed;
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
			if (this.Controls.Count <= 1 && !base.DockContainer.Boolean_0)
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
			if (this.IsPoppedUp)
			{
				this.Control0_0.method_6(true);
			}
		}

		public void Dock(ControlLayoutSystem layoutSystem)
		{
			if (layoutSystem == null)
			{
				throw new ArgumentNullException();
			}
			this.Dock(layoutSystem, 0);
		}

		public void Dock(ControlLayoutSystem layoutSystem, int index)
		{
			if (layoutSystem == null)
			{
				throw new ArgumentNullException();
			}
			if (base.Parent != null)
			{
				throw new InvalidOperationException("This layout system already has a parent. To remove it, use the parent layout system's LayoutSystems.Remove method.");
			}
			DockControl selectedControl = this.SelectedControl;
			while (this.Controls.Count != 0)
			{
				DockControl control = this.Controls[0];
				this.Controls.RemoveAt(0);
				layoutSystem.Controls.Insert(index, control);
			}
			if (selectedControl != null)
			{
				layoutSystem.SelectedControl = selectedControl;
			}
		}

		public void Float(SandDockManager manager)
		{
			if (this.SelectedControl == null)
			{
				throw new InvalidOperationException("The layout system must have a selected control to be floated.");
			}
			this.Float(manager, this.SelectedControl.method_11(), WindowOpenMethod.OnScreenActivate);
		}

		public void Float(SandDockManager manager, Rectangle bounds, WindowOpenMethod openMethod)
		{
			if (base.Parent != null)
			{
				LayoutUtilities.smethod_10(this);
			}
			if (this.SelectedControl.MetaData.LastFloatingWindowGuid == Guid.Empty)
			{
				this.SelectedControl.MetaData.method_5(Guid.NewGuid());
			}
			new Class5(manager, this.SelectedControl.MetaData.LastFloatingWindowGuid)
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
			{
				this.SelectedControl.Activate();
			}
		}

		public virtual DockControl GetControlAt(Point position)
		{
			if (this.rectangle_2.Contains(position) && !this.class17_0.rectangle_0.Contains(position) && !this.class17_1.rectangle_0.Contains(position))
			{
				IEnumerator enumerator = this.Controls.GetEnumerator();
				DockControl result;
				try
				{
					while (enumerator.MoveNext())
					{
						DockControl dockControl = (DockControl)enumerator.Current;
						Rectangle rectangle_ = dockControl.rectangle_0;
						if (rectangle_.Contains(position))
						{
							result = dockControl;
							return result;
						}
					}
					goto IL_84;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				return result;
			}
			IL_84:
			return null;
		}

		protected internal override void Layout(RendererBase renderer, Graphics graphics, Rectangle bounds, bool floating)
		{
			base.Layout(renderer, graphics, bounds, floating);
			this.method_18();
			if (this.Collapsed && base.DockContainer.Boolean_6)
			{
				return;
			}
			this.CalculateLayout(renderer, bounds, floating, out this.rectangle_1, out this.rectangle_2, out this.rectangle_3, out this.rectangle_4);
			this.bool_4 = true;
			try
			{
				if (this.rectangle_1 != Rectangle.Empty)
				{
					this.method_17();
				}
				this.method_19(renderer, graphics, this.rectangle_2);
				foreach (DockControl dockControl in this.Controls)
				{
					if (dockControl != this.SelectedControl)
					{
						dockControl.method_0(false);
					}
				}
				foreach (DockControl dockControl2 in this.Controls)
				{
					if (dockControl2 == this.SelectedControl)
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
			foreach (DockControl dockControl in this.Controls)
			{
				Rectangle bounds2 = renderer.AdjustDockControlClientBounds(this, dockControl, this.rectangle_3);
				dockControl.method_0(dockControl == this.dockControl_0);
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
				rendererBase_0.DrawDocumentStripButton(graphics_0, class17_4.rectangle_0, sandDockButtonType_0, drawItemState);
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
						ControlLayoutSystem controlLayoutSystem = dockTarget_0.dockContainer.CreateNewLayoutSystem(bool_7 ? this.DockControl_0 : new DockControl[]
						{
							dockControl_1
						}, base.WorkingSize);
						dockTarget_0.layoutSystem.SplitForLayoutSystem(controlLayoutSystem, dockTarget_0.dockSide);
					}
					return;
				}
				DockContainer dockContainer = sandDockManager_0.FindDockedContainer(DockStyle.Fill);
				if (dockTarget_0.dockLocation == ContainerDockLocation.Center && dockContainer != null)
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
						base.method_2(sandDockManager_0, dockTarget_0.dockLocation, dockTarget_0.middle ? ContainerDockEdge.Inside : ContainerDockEdge.Outside);
						return;
					}
					dockControl_1.DockInNewContainer(dockTarget_0.dockLocation, dockTarget_0.middle ? ContainerDockEdge.Inside : ContainerDockEdge.Outside);
				}
				return;
			}
		}

		private void method_12(LayoutSystemBase layoutSystemBase_0, int int_4, bool bool_7)
		{
			SplitLayoutSystem parent = base.Parent;
			parent.LayoutSystems.bool_0 = true;
			parent.LayoutSystems.Insert(int_4, layoutSystemBase_0);
			parent.LayoutSystems.bool_0 = false;
			parent.method_7();
		}

		private void method_13(LayoutSystemBase layoutSystemBase_0, Orientation orientation_0, bool bool_7)
		{
			SplitLayoutSystem parent = base.Parent;
			SplitLayoutSystem splitLayoutSystem = new SplitLayoutSystem();
			splitLayoutSystem.SplitMode = orientation_0;
			splitLayoutSystem.WorkingSize = base.WorkingSize;
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

		internal void method_14(DockSituation dockSituation_0)
		{
			if (this.Controls.Count == 0)
			{
				throw new InvalidOperationException();
			}
			if (this.SelectedControl.DockSituation != dockSituation_0)
			{
				DockControl selectedControl = this.SelectedControl;
				DockControl[] array = new DockControl[this.Controls.Count];
				this.Controls.CopyTo(array, 0);
				LayoutUtilities.smethod_10(this);
				this.Controls.Clear();
				if (dockSituation_0 != DockSituation.Docked)
				{
					if (dockSituation_0 != DockSituation.Document)
					{
						if (dockSituation_0 != DockSituation.Floating)
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
				return;
			}
		}

		internal int method_15(Point point_1)
		{
			int num = 0;
			foreach (DockControl dockControl in this.Controls)
			{
				Rectangle rectangle_ = dockControl.rectangle_0;
				if (point_1.X > rectangle_.Left + rectangle_.Width / 2)
				{
					num++;
				}
			}
			return num;
		}

		internal void method_16()
		{
		    this.Control0_0?.method_0(this);
		    if (base.IsInContainer)
			{
				if (base.DockContainer.IsFloating)
				{
					base.DockContainer.CalculateAllMetricsAndLayout();
				}
				else
				{
					base.DockContainer.method_10(this, base.Bounds);
				}
				base.DockContainer.Invalidate(base.Bounds);
			}
		}

		private void method_17()
		{
			if (this.dockControl_0 == null)
			{
				this.class17_0.bool_0 = false;
				this.class17_1.bool_0 = false;
				this.class17_2.bool_0 = false;
				return;
			}
			int y = this.rectangle_1.Top + this.rectangle_1.Height / 2 - 7;
			int num = this.rectangle_1.Right - 2;
			if (this.dockControl_0.AllowClose)
			{
				this.class17_0.bool_0 = true;
				this.class17_0.rectangle_0 = new Rectangle(num - 19, y, 19, 15);
				num -= 21;
			}
			else
			{
				this.class17_0.bool_0 = false;
			}
			if (!this.Boolean_0 || (base.IsInContainer && !base.DockContainer.Boolean_6))
			{
				this.class17_1.bool_0 = false;
			}
			else
			{
				this.class17_1.bool_0 = true;
				this.class17_1.rectangle_0 = new Rectangle(num - 19, y, 19, 15);
				num -= 21;
			}
			if (!this.dockControl_0.ShowOptions)
			{
				this.class17_2.bool_0 = false;
				return;
			}
			this.class17_2.bool_0 = true;
			this.class17_2.rectangle_0 = new Rectangle(num - 19, y, 19, 15);
			num -= 21;
		}

		private void method_18()
		{
			foreach (DockControl dockControl in this.Controls)
			{
				dockControl.method_5();
			}
		}

		private void method_19(RendererBase rendererBase_0, Graphics graphics_0, Rectangle rectangle_5)
		{
			int num = 0;
			int num2 = rectangle_5.Width - (rendererBase_0.TabStripMetrics.Padding.Left + rendererBase_0.TabStripMetrics.Padding.Right);
			int[] array = new int[this.Controls.Count];
			int num3 = 0;
			foreach (DockControl dockControl in this.Controls)
			{
				dockControl.bool_3 = false;
				int num4 = rendererBase_0.MeasureTabStripTab(graphics_0, dockControl.TabImage, dockControl.TabText, dockControl.Font, DrawItemState.Default).Width;
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
			rectangle_5 = rendererBase_0.TabStripMetrics.RemovePadding(rectangle_5);
			int num6 = rectangle_5.Left;
			num3 = 0;
			for (int j = 0; j < this.Controls.Count; j++)
			{
				DockControl dockControl2 = this.Controls[j];
				BoxModel tabMetrics = rendererBase_0.TabMetrics;
				Rectangle rectangle_6 = new Rectangle(num6 + tabMetrics.Margin.Left, rectangle_5.Top + tabMetrics.Margin.Top, tabMetrics.Padding.Left + array[num3] + tabMetrics.Padding.Right, rectangle_5.Height - (tabMetrics.Margin.Top + tabMetrics.Margin.Bottom));
				dockControl2.rectangle_0 = rectangle_6;
				num6 += rectangle_6.Width + tabMetrics.ExtraWidth;
				num3++;
			}
		}

		internal void method_4(Control0 control0_1)
		{
			this.Control0_0 = control0_1;
		}

		private void method_5(DockControl dockControl_1, DockControl dockControl_2)
		{
		    this.delegate2_0?.Invoke(dockControl_1, dockControl_2);
		}

	    private void method_6()
		{
			switch (this.SelectedControl.DockSituation)
			{
			case DockSituation.Docked:
			case DockSituation.Document:
				if (this.Boolean_3)
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
			Point point = new Point(this.class17_2.rectangle_0.Left, this.class17_2.rectangle_0.Bottom);
			point = this.SelectedControl.Parent.PointToScreen(point);
			point = this.SelectedControl.PointToClient(point);
			base.DockContainer.method_0(new ShowControlContextMenuEventArgs(this.SelectedControl, point, ContextMenuContext.OptionsButton));
		}

		internal bool method_8()
		{
			if (base.IsInContainer && this.SelectedControl != null && this.SelectedControl.ContainsFocus)
			{
				this.Boolean_1 = true;
				if (this.SelectedControl != null)
				{
					base.DockContainer.Manager.OnDockControlActivated(new DockControlEventArgs(this.SelectedControl));
				}
				return true;
			}
			return false;
		}

		internal void method_9()
		{
			this.Boolean_1 = false;
		}

		protected virtual void OnCloseButtonClick(EventArgs e)
		{
			if (this.SelectedControl != null)
			{
				this.SelectedControl.method_14(true);
			}
		}

		protected internal override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);
			DockControl controlAt = this.GetControlAt(base.DockContainer.PointToClient(new Point(drgevent.X, drgevent.Y)));
			if (controlAt != null)
			{
				if (this.SelectedControl != controlAt)
				{
					controlAt.Open(WindowOpenMethod.OnScreenActivate);
				}
			}
		}

		protected internal override void OnMouseDoubleClick()
		{
			Point point = base.DockContainer.PointToClient(Cursor.Position);
			if (base.DockContainer.Manager == null)
			{
				return;
			}
			if (!this.LockControls)
			{
				if (this.rectangle_1.Contains(point) && !this.class17_0.rectangle_0.Contains(point) && !this.class17_1.rectangle_0.Contains(point) && this.Controls.Count != 0)
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
					base.DockContainer.method_0(new ShowControlContextMenuEventArgs(dockControl, point, ContextMenuContext.RightClick));
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
			this.Collapsed = !this.Collapsed;
			if (base.IsInContainer && this.SelectedControl != null)
			{
				if (this.Collapsed && this.Control0_0 != null)
				{
					this.Control0_0.method_7(this.SelectedControl, true, false);
					this.Control0_0.method_6(false);
					return;
				}
				this.SelectedControl.Activate();
			}
		}

		public void SplitForLayoutSystem(LayoutSystemBase layoutSystem, DockSide side)
		{
			if (layoutSystem == null)
			{
				throw new ArgumentNullException("layoutSystem");
			}
			if (side == DockSide.None)
			{
				throw new ArgumentException("side");
			}
			if (layoutSystem.Parent != null)
			{
				throw new InvalidOperationException("This layout system must be removed from its parent before it can be moved to a new layout system.");
			}
			if (base.Parent == null)
			{
				throw new InvalidOperationException("This layout system is not parented yet.");
			}
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

		internal override void vmethod_0(Class7.DockTarget dockTarget_0)
		{
			base.vmethod_0(dockTarget_0);
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
			selectedControl.MetaData.method_5(Guid.NewGuid());
			if (!this.bool_5)
			{
				selectedControl.OpenFloating(dockTarget_0.bounds, WindowOpenMethod.OnScreenActivate);
				return;
			}
			this.Float(manager, dockTarget_0.bounds, WindowOpenMethod.OnScreenActivate);
		}

		internal override void vmethod_1(object sender, EventArgs e)
		{
			base.vmethod_1(sender, e);
			this.point_0 = Point.Empty;
		}

		internal override void vmethod_2(DockContainer dockContainer_1)
		{
			if (dockContainer_1 == null && base.IsInContainer)
			{
				foreach (DockControl dockControl in this.Controls)
				{
					if (dockControl.Parent == base.DockContainer)
					{
						LayoutUtilities.smethod_8(dockControl);
					}
				}
			}
			if (dockContainer_1 != null && !base.IsInContainer)
			{
				foreach (DockControl dockControl2 in this.Controls)
				{
					if (dockControl2.Parent != null)
					{
						LayoutUtilities.smethod_8(dockControl2);
					}
					dockControl2.Location = new Point(dockContainer_1.Width, dockContainer_1.Height);
					if (!this.Collapsed || !dockContainer_1.Boolean_6)
					{
						dockControl2.Parent = dockContainer_1;
					}
				}
			}
			base.vmethod_2(dockContainer_1);
			foreach (DockControl dockControl3 in this.Controls)
			{
				dockControl3.method_4(dockContainer_1);
			}
			if (this.Collapsed)
			{
				if (dockContainer_1?.Manager != null && this.Control0_0 == null)
				{
					Control0 autoHideBar = dockContainer_1.Manager.GetAutoHideBar(dockContainer_1.Dock);
					if (autoHideBar != null)
					{
						autoHideBar.Class4_0.method_1(this);
						return;
					}
				}
				else
				{
				    this.Control0_0?.Class4_0.method_2(this);
				}
			}
		}

		internal override bool vmethod_3(ContainerDockLocation containerDockLocation_0)
		{
			IEnumerator enumerator = this.Controls.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					DockControl dockControl = (DockControl)enumerator.Current;
					if (!dockControl.method_13(containerDockLocation_0))
					{
						result = false;
						return result;
					}
				}
				return true;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return result;
		}

		internal override void vmethod_4(RendererBase rendererBase_0, Graphics graphics_0, Font font_0)
		{
			if (base.DockContainer == null)
			{
				return;
			}
			Control container = (base.DockContainer.IsFloating || base.DockContainer.Manager == null || base.DockContainer.Manager.DockSystemContainer == null) ? base.DockContainer : base.DockContainer.Manager.DockSystemContainer;
			bool focused;
			if (base.IsInContainer && base.DockContainer.Boolean_0)
			{
				ISelectionService selectionService = (ISelectionService)base.DockContainer.method_1(typeof(ISelectionService));
				focused = selectionService.GetComponentSelected(this.SelectedControl);
			}
			else
			{
				focused = this.Boolean_1;
			}
			if (this.SelectedControl != null)
			{
				rendererBase_0.DrawControlClientBackground(graphics_0, this.rectangle_3, this.SelectedControl.BackColor);
			}
			else
			{
				rendererBase_0.DrawControlClientBackground(graphics_0, this.rectangle_3, SystemColors.Control);
			}
			if ((this.Controls.Count > 1 || base.DockContainer.Boolean_0) && this.rectangle_2 != Rectangle.Empty)
			{
				int selectedTabOffset = 0;
				if (this.dockControl_0 != null)
				{
					Rectangle rectangle_ = this.dockControl_0.rectangle_0;
					selectedTabOffset = rectangle_.X - base.Bounds.Left;
				}
				rendererBase_0.DrawTabStripBackground(container, base.DockContainer, graphics_0, this.rectangle_2, selectedTabOffset);
				foreach (DockControl dockControl in this.Controls)
				{
					DrawItemState drawItemState = DrawItemState.Default;
					if (this.dockControl_0 == dockControl)
					{
						drawItemState |= DrawItemState.Selected;
					}
					bool drawSeparator = true;
					if (this.dockControl_0 != null)
					{
						if (this.Controls.IndexOf(dockControl) == this.Controls.IndexOf(this.dockControl_0) - 1)
						{
							drawSeparator = false;
						}
					}
					if (this.Controls.IndexOf(dockControl) == this.Controls.Count - 1 && rendererBase_0 is WhidbeyRenderer)
					{
						drawSeparator = false;
					}
					rendererBase_0.DrawTabStripTab(graphics_0, dockControl.rectangle_0, dockControl.Image_0, dockControl.TabText, dockControl.Font, dockControl.BackColor, dockControl.ForeColor, drawItemState, drawSeparator);
				}
			}
			Rectangle rectangle = this.rectangle_1;
			if (rectangle != Rectangle.Empty && rectangle.Width > 0 && rectangle.Height > 0)
			{
				rendererBase_0.DrawTitleBarBackground(graphics_0, rectangle, focused);
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
				rectangle = rendererBase_0.TitleBarMetrics.RemovePadding(rectangle);
				if (rectangle.Width > 8)
				{
					rendererBase_0.DrawTitleBarText(graphics_0, rectangle, focused, (this.dockControl_0 == null) ? "Empty Layout System" : this.dockControl_0.Text, (this.dockControl_0 != null) ? this.dockControl_0.Font : base.DockContainer.Font);
				}
				if (this.class17_0.bool_0 && this.class17_0.rectangle_0.Left > this.rectangle_1.Left)
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
					rendererBase_0.DrawTitleBarButton(graphics_0, this.class17_0.rectangle_0, SandDockButtonType.Close, drawItemState2, focused, false);
				}
				if (this.class17_1.bool_0 && this.class17_1.rectangle_0.Left > this.rectangle_1.Left)
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
					rendererBase_0.DrawTitleBarButton(graphics_0, this.class17_1.rectangle_0, SandDockButtonType.Pin, drawItemState2, focused, this.Collapsed);
				}
				if (this.class17_2.bool_0 && this.class17_2.rectangle_0.Left > this.rectangle_1.Left)
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
					rendererBase_0.DrawTitleBarButton(graphics_0, this.class17_2.rectangle_0, SandDockButtonType.WindowPosition, drawItemState2, focused, false);
				}
			}
		}

		internal virtual string vmethod_5(Point point_1)
		{
			DockControl controlAt = this.GetControlAt(point_1);
			if (controlAt == null)
			{
				Class17 @class = this.vmethod_6(point_1.X, point_1.Y);
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
				if (controlAt.ToolTipText.Length != 0)
				{
					return controlAt.ToolTipText;
				}
				if (!controlAt.bool_3)
				{
					return "";
				}
				return controlAt.Text;
			}
		}

		internal virtual Class17 vmethod_6(int int_4, int int_5)
		{
			if (this.class17_0.bool_0 && this.class17_0.rectangle_0.Contains(int_4, int_5))
			{
				return this.class17_0;
			}
			if (this.class17_1.bool_0 && this.class17_1.rectangle_0.Contains(int_4, int_5))
			{
				return this.class17_1;
			}
			if (this.class17_2.bool_0 && this.class17_2.rectangle_0.Contains(int_4, int_5))
			{
				return this.class17_2;
			}
			return null;
		}

		internal virtual void vmethod_7(Class17 class17_4)
		{
		}

		internal virtual void vmethod_8(Class17 class17_4)
		{
			if (this.Class17_0 == this.class17_0)
			{
				this.OnCloseButtonClick(EventArgs.Empty);
				return;
			}
			if (this.Class17_0 != this.class17_1)
			{
				if (this.Class17_0 == this.class17_2)
				{
					this.method_7();
				}
				return;
			}
			this.OnPinButtonClick();
		}

		internal virtual void vmethod_9()
		{
			if (this.Control0_0 != null)
			{
				if (this.Control0_0.ControlLayoutSystem_0 == this)
				{
					this.Control0_0.method_9(this.rectangle_1);
					return;
				}
			}
			else if (base.IsInContainer)
			{
				base.DockContainer.Invalidate(this.rectangle_1);
			}
		}

		private bool Boolean_0
		{
			get
			{
				IEnumerator enumerator = this.Controls.GetEnumerator();
				bool result;
				try
				{
					while (enumerator.MoveNext())
					{
						DockControl dockControl = (DockControl)enumerator.Current;
						if (!dockControl.AllowCollapse)
						{
							result = false;
							return result;
						}
					}
					return true;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				return result;
			}
		}

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

		internal override bool Boolean_2 => Controls.Cast<DockControl>().Any(control => control.PersistState);

	    internal override bool Boolean_3 => Controls.Cast<DockControl>().All(control => control.DockingRules.AllowFloat);

	    internal override bool Boolean_4=> Controls.Cast<DockControl>().All(control => control.DockingRules.AllowTab);

		internal Class17 Class17_0
		{
			get
			{
				return this.class17_3;
			}
			set
			{
				if (value != this.class17_3)
				{
					if (this.class17_3 != null)
					{
						this.vmethod_9();
					}
					this.class17_3 = value;
					if (this.class17_3 != null)
					{
						this.vmethod_9();
					}
				}
			}
		}

		[Browsable(false), DefaultValue(false)]
		public virtual bool Collapsed
		{
			get
			{
				return this.bool_0;
			}
			set
			{
				if (this.bool_0 == value)
				{
					return;
				}
				this.bool_0 = value;
				this.Class17_0 = null;
				if (this.bool_0)
				{
					if (base.IsInContainer)
					{
						foreach (DockControl dockControl in this.Controls)
						{
							if (dockControl.Parent == base.DockContainer)
							{
								LayoutUtilities.smethod_8(dockControl);
							}
						}
						Control0 autoHideBar = base.DockContainer.Manager.GetAutoHideBar(base.DockContainer.Dock);
					    autoHideBar?.Class4_0.method_1(this);
					}
				}
				else
				{
				    this.Control0_0?.Class4_0.method_2(this);
				    foreach (DockControl dockControl2 in this.Controls)
					{
						if (dockControl2.Parent != base.DockContainer)
						{
							dockControl2.Parent = base.DockContainer;
						}
					}
				}
				if (base.IsInContainer)
				{
					base.DockContainer.vmethod_2();
				}
			}
		}

		internal Control0 Control0_0 { get; private set; }

	    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DockControlCollection Controls { get; }

	    internal Control Control_0
		{
			get
			{
				if (!base.IsInContainer)
				{
					return null;
				}
				if (!this.IsPoppedUp)
				{
					return base.DockContainer;
				}
				return this.Control0_0.Control_0;
			}
		}

		internal override DockControl[] DockControl_0
		{
			get
			{
				DockControl[] array = new DockControl[this.Controls.Count];
				this.Controls.CopyTo(array, 0);
				return array;
			}
		}

		internal Guid Guid_0 { get; set; } = Guid.NewGuid();

	    internal int Int32_0
		{
			get
			{
				if (this.SelectedControl != null && this.SelectedControl.PopupSize != 0)
				{
					return this.SelectedControl.PopupSize;
				}
				if (!base.IsInContainer)
				{
					return 200;
				}
				return base.DockContainer.ContentSize;
			}
			set
			{
				foreach (DockControl dockControl in this.Controls)
				{
					dockControl.PopupSize = value;
				}
			}
		}

		public bool IsPoppedUp
		{
			get
			{
				return this.Control0_0 != null && this.Control0_0.ControlLayoutSystem_0 == this;
			}
		}

		public bool LockControls { get; set; }

	    internal Rectangle Rectangle_0 => this.rectangle_4;

	    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual DockControl SelectedControl
		{
			get
			{
				return this.dockControl_0;
			}
			set
			{
				if (value != null && !this.Controls.Contains(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.SelectedControl != null && this.SelectedControl.Manager != null && this.SelectedControl.Manager.RaiseValidationEvents && !this.SelectedControl.ValidateChildren())
				{
					return;
				}
				DockControl dockControl = this.dockControl_0;
				this.dockControl_0 = value;
				this.method_16();
				if (this.IsPoppedUp)
				{
					if (dockControl != null)
					{
						dockControl.OnAutoHidePopupClosed(EventArgs.Empty);
					}
					if (this.dockControl_0 != null)
					{
						this.dockControl_0.OnAutoHidePopupOpened(EventArgs.Empty);
					}
				}
				this.method_5(dockControl, this.dockControl_0);
			}
		}

		internal event ControlLayoutSystem.Delegate2 Event_0
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.delegate2_0 = (ControlLayoutSystem.Delegate2)Delegate.Combine(this.delegate2_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.delegate2_0 = (ControlLayoutSystem.Delegate2)Delegate.Remove(this.delegate2_0, value);
			}
		}

		private bool bool_0;

		private bool bool_1;

		internal bool bool_2;

	    internal bool bool_4;

		private bool bool_5;

		private bool bool_6;

		private Class17 class17_0;

		private Class17 class17_1;

		private Class17 class17_2;

		private Class17 class17_3;

	    private Delegate2 delegate2_0;

	    private DockControl dockControl_0;

	    private const int int_2 = 19;

		private const int int_3 = 15;

		private Point point_0 = Point.Empty;

		internal Rectangle rectangle_1;

		internal Rectangle rectangle_2;

		internal Rectangle rectangle_3;

		internal Rectangle rectangle_4;

		internal delegate void Delegate2(DockControl oldSelection, DockControl newSelection);

		public class DockControlCollection : CollectionBase
		{
			internal DockControlCollection(ControlLayoutSystem parent)
			{
				this.controlLayoutSystem_0 = parent;
			}

			public int Add(DockControl control)
			{
				if (base.List.Contains(control))
				{
					throw new InvalidOperationException("The DockControl already belongs to this ControlLayoutSystem.");
				}
				int count = base.Count;
				this.Insert(count, control);
				return count;
			}

			public void AddRange(DockControl[] controls)
			{
				this.bool_0 = true;
				foreach (var control in controls)
				{
				    this.Add(control);
				}
				this.bool_0 = false;
				this.controlLayoutSystem_0.method_16();
			}

			public bool Contains(DockControl control)
			{
				return base.List.Contains(control);
			}

			public void CopyTo(DockControl[] array, int index)
			{
				base.List.CopyTo(array, index);
			}

			public int IndexOf(DockControl control)
			{
				return base.List.IndexOf(control);
			}

			public void Insert(int index, DockControl control)
			{
				if (control == null)
				{
					return;
				}
				if (control.LayoutSystem == this.controlLayoutSystem_0)
				{
					if (this.IndexOf(control) == index)
					{
						return;
					}
					if (base.Count == 1)
					{
						return;
					}
				}
				if (control.LayoutSystem != null)
				{
					if (this.Contains(control) && this.IndexOf(control) < index)
					{
						index--;
					}
					control.LayoutSystem.Controls.Remove(control);
				}
				base.List.Insert(index, control);
			}

			internal int method_0(int int_0, bool bool_2)
			{
				if (int_0 < 0 || int_0 > base.Count)
				{
					int_0 = ((!bool_2) ? 0 : base.Count);
				}
				return int_0;
			}

			protected override void OnClear()
			{
				base.OnClear();
				foreach (DockControl dockControl in this)
				{
					dockControl.method_16(null);
					dockControl.method_5();
				}
			}

			protected override void OnClearComplete()
			{
				base.OnClearComplete();
				this.controlLayoutSystem_0.SelectedControl = null;
				this.controlLayoutSystem_0.method_16();
			    this.controlLayoutSystem_0.DockContainer?.vmethod_0();
			}

			protected override void OnInsertComplete(int index, object value)
			{
				base.OnInsertComplete(index, value);
				if (this.bool_1)
				{
					return;
				}
				DockControl dockControl = (DockControl)value;
				dockControl.method_16(this.controlLayoutSystem_0);
				if (this.controlLayoutSystem_0.IsInContainer && this.controlLayoutSystem_0.DockContainer.Manager != null && this.controlLayoutSystem_0.DockContainer.Manager != dockControl.Manager)
				{
					dockControl.Manager = this.controlLayoutSystem_0.DockContainer.Manager;
				}
				if (this.controlLayoutSystem_0.IsInContainer)
				{
					dockControl.method_4(this.controlLayoutSystem_0.DockContainer);
				}
				if (this.controlLayoutSystem_0.IsInContainer)
				{
					if (dockControl.Parent != null)
					{
						LayoutUtilities.smethod_8(dockControl);
					}
					dockControl.Parent = this.controlLayoutSystem_0.Control_0;
				}
				if (this.controlLayoutSystem_0.dockControl_0 == null)
				{
					this.controlLayoutSystem_0.SelectedControl = dockControl;
				}
			    this.controlLayoutSystem_0.DockContainer?.vmethod_0();
			    if (!this.bool_0)
				{
					this.controlLayoutSystem_0.method_16();
				}
			}

			protected override void OnRemoveComplete(int index, object value)
			{
				base.OnRemoveComplete(index, value);
				if (!this.bool_1)
				{
					DockControl dockControl = (DockControl)value;
					dockControl.method_16(null);
					dockControl.method_5();
					if (dockControl.Parent != null)
					{
						if (dockControl.Parent == this.controlLayoutSystem_0.Control_0)
						{
							LayoutUtilities.smethod_8(dockControl);
						}
					}
					if (this.controlLayoutSystem_0.dockControl_0 == value)
					{
						if (this.controlLayoutSystem_0.Controls.Count != 0)
						{
							this.controlLayoutSystem_0.SelectedControl = this[0];
						}
						else
						{
							this.controlLayoutSystem_0.SelectedControl = null;
						}
					}
				    this.controlLayoutSystem_0.DockContainer?.vmethod_0();
				    this.controlLayoutSystem_0.method_16();
					return;
				}
			}

			public void Remove(DockControl control)
			{
				if (control == null)
				{
					throw new ArgumentNullException("control");
				}
				base.List.Remove(control);
			}

			public void SetChildIndex(DockControl control, int index)
			{
				if (control == null)
				{
					throw new ArgumentNullException("control");
				}
				if (!this.Contains(control))
				{
					throw new ArgumentOutOfRangeException("control");
				}
				if (index == this.IndexOf(control))
				{
					return;
				}
				if (this.IndexOf(control) < index)
				{
					index--;
				}
				this.bool_1 = true;
				base.List.Remove(control);
				base.List.Insert(index, control);
				this.bool_1 = false;
				this.controlLayoutSystem_0.method_16();
			}

			public DockControl this[int index] => (DockControl)base.List[index];

		    private bool bool_0;

			private bool bool_1;

			private ControlLayoutSystem controlLayoutSystem_0;
		}
	}
}
