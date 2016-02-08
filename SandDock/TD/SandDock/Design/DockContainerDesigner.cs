using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TD.SandDock.Design
{
	internal class DockContainerDesigner : ParentControlDesigner
	{
		public DockContainerDesigner()
		{
			EnableDragDrop(false);
		}

		protected override void Dispose(bool disposing)
		{
			ISelectionService arg_15_0 = (ISelectionService)this.GetService(typeof(ISelectionService));
			this.icomponentChangeService_0.ComponentRemoving += this.icomponentChangeService_0_ComponentRemoving;
			this.icomponentChangeService_0.ComponentRemoved += this.icomponentChangeService_0_ComponentRemoved;
			base.Dispose(disposing);
		}

		private void icomponentChangeService_0_ComponentRemoved(object sender, ComponentEventArgs e)
		{
			if (e.Component == this.dockControl_0)
			{
				this.dockControl_0 = null;
				base.RaiseComponentChanged(TypeDescriptor.GetProperties(this.dockContainer_0)["LayoutSystem"], null, null);
			}
		}

		private void icomponentChangeService_0_ComponentRemoving(object sender, ComponentEventArgs e)
		{
			DockControl dockControl = e.Component as DockControl;
			if (dockControl?.LayoutSystem != null)
			{
				if (dockControl.LayoutSystem.DockContainer == this.dockContainer_0)
				{
					this.dockControl_0 = dockControl;
					base.RaiseComponentChanging(TypeDescriptor.GetProperties(this.dockContainer_0)["LayoutSystem"]);
				}
			}
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			if (!(component is DockContainer))
			{
				SandDockLanguage.ShowCachedAssemblyError(component.GetType().Assembly, base.GetType().Assembly);
			}
			ISelectionService arg_3F_0 = (ISelectionService)this.GetService(typeof(ISelectionService));
			this.icomponentChangeService_0 = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			this.idesignerHost_0 = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			this.icomponentChangeService_0.ComponentRemoving += new ComponentEventHandler(this.icomponentChangeService_0_ComponentRemoving);
			this.icomponentChangeService_0.ComponentRemoved += new ComponentEventHandler(this.icomponentChangeService_0_ComponentRemoved);
			this.dockContainer_0 = (DockContainer)component;
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			this.method_9();
		}

		private DockControl method_0(Point point_1)
		{
		    LayoutSystemBase layoutSystemAt = this.dockContainer_0.GetLayoutSystemAt(point_1);
		    return !(layoutSystemAt is ControlLayoutSystem)
		        ? null
		        : ((ControlLayoutSystem) layoutSystemAt).GetControlAt(point_1);
		}

	    private void method_1()
		{
			this.class11_0.Event_0 -= this.method_3;
			this.class11_0.ResizingManagerFinished -= this.method_2;
			this.class11_0 = null;
		}

		private void method_2(int newSize)
		{
			this.method_1();
			DesignerTransaction designerTransaction = this.idesignerHost_0.CreateTransaction("Resize Docked Windows");
			base.RaiseComponentChanging(TypeDescriptor.GetProperties(base.Component)["ContentSize"]);
			this.dockContainer_0.ContentSize = newSize;
			base.RaiseComponentChanged(TypeDescriptor.GetProperties(base.Component)["ContentSize"], null, null);
			designerTransaction.Commit();
		}

		private void method_3(object sender, EventArgs e)
		{
			this.method_1();
		}

		private void method_4(Point point_1)
		{
			LayoutSystemBase layoutSystemAt = this.dockContainer_0.GetLayoutSystemAt(point_1);
			if (layoutSystemAt is ControlLayoutSystem && this.class7_0 == null)
			{
				ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)layoutSystemAt;
				DockControl controlAt = controlLayoutSystem.GetControlAt(point_1);
				this.class7_0 = new Class8(this.dockContainer_0.Manager, this.dockContainer_0, controlLayoutSystem, controlAt, controlLayoutSystem.SelectedControl.MetaData.DockedContentSize, point_1, DockingHints.TranslucentFill);
				this.class7_0.DockingManagerFinished += this.vmethod_0;
				this.class7_0.Event_0 += this.vmethod_1;
				this.dockContainer_0.Capture = true;
			}
		}

		private void method_5()
		{
			this.class7_0.DockingManagerFinished -= this.vmethod_0;
			this.class7_0.Event_0 -= this.vmethod_1;
			this.class7_0 = null;
		}

		private void method_6()
		{
			this.class10_0.Event_0 -= new EventHandler(this.method_7);
			this.class10_0.SplittingManagerFinished -= this.method_8;
			this.class10_0 = null;
		}

		private void method_7(object sender, EventArgs e)
		{
			this.method_6();
		}

		private void method_8(LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, float aboveSize, float belowSize)
		{
			SplitLayoutSystem splitLayoutSystem_ = this.class10_0.SplitLayout;
			this.method_6();
			DesignerTransaction designerTransaction = this.idesignerHost_0.CreateTransaction("Resize Docked Windows");
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			componentChangeService.OnComponentChanging(this.dockContainer_0, TypeDescriptor.GetProperties(this.dockContainer_0)["LayoutSystem"]);
			SizeF workingSize = aboveLayout.WorkingSize;
			SizeF workingSize2 = belowLayout.WorkingSize;
			if (splitLayoutSystem_.SplitMode == Orientation.Horizontal)
			{
				workingSize.Height = aboveSize;
				workingSize2.Height = belowSize;
			}
			else
			{
				workingSize.Width = aboveSize;
				workingSize2.Width = belowSize;
			}
			aboveLayout.WorkingSize = workingSize;
			belowLayout.WorkingSize = workingSize2;
			componentChangeService.OnComponentChanged(this.dockContainer_0, TypeDescriptor.GetProperties(this.dockContainer_0)["LayoutSystem"], null, null);
			designerTransaction.Commit();
			splitLayoutSystem_.method_8();
		}

		private void method_9()
		{
			IExtenderListService extenderListService = (IExtenderListService)GetService(typeof(IExtenderListService));
			IExtenderProvider[] extenderProviders = extenderListService.GetExtenderProviders();
			int i = 0;
			while (i < extenderProviders.Length)
			{
				IExtenderProvider extenderProvider = extenderProviders[i];
				if (!(extenderProvider.GetType().FullName == "System.ComponentModel.Design.Serialization.CodeDomDesignerLoader+ModifiersExtenderProvider"))
				{
					i++;
				}
				else
				{
					MethodInfo method = extenderProvider.GetType().GetMethod("SetGenerateMember", BindingFlags.Instance | BindingFlags.Public);
					if (method == null)
					{
						return;
					}
					method.Invoke(extenderProvider, new object[]
					{
						base.Component,
						false
					});
					return;
				}
			}
		}

		protected override void OnMouseDragBegin(int x, int y)
		{
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			Point point = this.dockContainer_0.PointToClient(new Point(x, y));
			LayoutSystemBase layoutSystemAt = this.dockContainer_0.GetLayoutSystemAt(point);
			if (!(layoutSystemAt is SplitLayoutSystem))
			{
				if (this.dockContainer_0.Rectangle_0 != Rectangle.Empty && this.dockContainer_0.Rectangle_0.Contains(point))
				{
					this.class11_0 = new Class11(this.dockContainer_0.Manager, this.dockContainer_0, point);
					this.class11_0.Event_0 += new EventHandler(this.method_3);
					this.class11_0.ResizingManagerFinished += this.method_2;
					this.dockContainer_0.Capture = true;
					return;
				}
				if (layoutSystemAt is ControlLayoutSystem)
				{
					ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)layoutSystemAt;
					DockControl controlAt = controlLayoutSystem.GetControlAt(point);
					if (controlAt != null)
					{
						if (controlAt.LayoutSystem.SelectedControl != controlAt)
						{
							IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
							componentChangeService.OnComponentChanging(this.dockContainer_0, TypeDescriptor.GetProperties(this.dockContainer_0)["LayoutSystem"]);
							controlAt.LayoutSystem.SelectedControl = controlAt;
							componentChangeService.OnComponentChanged(this.dockContainer_0, TypeDescriptor.GetProperties(this.dockContainer_0)["LayoutSystem"], null, null);
						}
					}
					if (!controlLayoutSystem.rectangle_1.Contains(point) && controlAt == null)
					{
						selectionService.SetSelectedComponents(new object[]
						{
							this.dockContainer_0
						}, SelectionTypes.MouseDown | SelectionTypes.Click);
						this.dockContainer_0.Capture = true;
						return;
					}
					if (controlLayoutSystem.SelectedControl != null)
					{
						selectionService.SetSelectedComponents(new object[]
						{
							controlLayoutSystem.SelectedControl
						}, SelectionTypes.Click);
					}
					this.point_0 = new Point(x, y);
					return;
				}
			}
			else
			{
				SplitLayoutSystem splitLayoutSystem = (SplitLayoutSystem)layoutSystemAt;
				if (splitLayoutSystem.method_6(point.X, point.Y))
				{
					LayoutSystemBase aboveLayout;
					LayoutSystemBase belowLayout;
					splitLayoutSystem.method_5(point, out aboveLayout, out belowLayout);
					this.class10_0 = new Class10(this.dockContainer_0, splitLayoutSystem, aboveLayout, belowLayout, point, DockingHints.TranslucentFill);
					this.class10_0.Event_0 += new EventHandler(this.method_7);
					this.class10_0.SplittingManagerFinished += new Class10.SplittingManagerFinishedEventHandler(this.method_8);
					this.dockContainer_0.Capture = true;
					return;
				}
			}
			selectionService.SetSelectedComponents(new object[]
			{
				this.dockContainer_0
			}, SelectionTypes.MouseDown | SelectionTypes.Click);
		}

		protected override void OnMouseDragEnd(bool cancel)
		{
			this.point_0 = Point.Empty;
			try
			{
				if (this.class10_0 != null)
				{
					this.class10_0.Commit();
					this.dockContainer_0.Capture = false;
				}
				else if (this.class11_0 != null)
				{
					this.class11_0.Commit();
					this.dockContainer_0.Capture = false;
				}
				else if (this.class7_0 != null)
				{
					this.class7_0.Commit();
					this.dockContainer_0.Capture = false;
				}
				else if (this.method_0(this.dockContainer_0.PointToClient(Cursor.Position)) == null)
				{
					LayoutSystemBase layoutSystemAt = this.dockContainer_0.GetLayoutSystemAt(this.dockContainer_0.PointToClient(Cursor.Position));
					if (layoutSystemAt is ControlLayoutSystem)
					{
					}
				}
			}
			finally
			{
				if (this.Control != null)
				{
					this.Control.Capture = false;
				}
			}
		}

		protected override void OnMouseDragMove(int x, int y)
		{
			Point position = this.dockContainer_0.PointToClient(new Point(x, y));
			if (this.class10_0 != null)
			{
				this.class10_0.OnMouseMove(position);
				return;
			}
			if (this.class11_0 != null)
			{
				this.class11_0.OnMouseMove(position);
				return;
			}
			if (this.class7_0 == null)
			{
				if (this.point_0 != Point.Empty)
				{
					Rectangle rectangle = new Rectangle(this.point_0, SystemInformation.DragSize);
					rectangle.Offset(-SystemInformation.DragSize.Width / 2, -SystemInformation.DragSize.Height / 2);
					if (!rectangle.Contains(x, y))
					{
						this.method_4(this.dockContainer_0.PointToClient(this.point_0));
						this.point_0 = Point.Empty;
					}
				}
				return;
			}
			this.class7_0.OnMouseMove(Cursor.Position);
			if (this.class7_0.DockTarget_0 != null && this.class7_0.DockTarget_0.type != Class7.DockTargetType.None)
			{
				Cursor.Current = Cursors.Default;
				return;
			}
			Cursor.Current = Cursors.No;
		}

		protected override void OnSetCursor()
		{
			Point point = this.dockContainer_0.PointToClient(Cursor.Position);
			LayoutSystemBase layoutSystemAt = this.dockContainer_0.GetLayoutSystemAt(point);
			SplitLayoutSystem splitLayoutSystem = layoutSystemAt as SplitLayoutSystem;
			if (splitLayoutSystem != null && splitLayoutSystem.method_6(point.X, point.Y))
			{
				if (splitLayoutSystem.SplitMode != Orientation.Horizontal)
				{
					Cursor.Current = Cursors.VSplit;
					return;
				}
				Cursor.Current = Cursors.HSplit;
				return;
			}
			else
			{
				if (!(this.dockContainer_0.Rectangle_0 != Rectangle.Empty) || !this.dockContainer_0.Rectangle_0.Contains(point))
				{
					Cursor.Current = Cursors.Default;
					return;
				}
				if (!this.dockContainer_0.Boolean_1)
				{
					Cursor.Current = Cursors.HSplit;
					return;
				}
				Cursor.Current = Cursors.VSplit;
				return;
			}
		}

		internal virtual void vmethod_0(Class7.DockTarget target)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)this.class7_0.LayoutSystemBase_0;
			bool flag = this.class7_0.DockControl_0 == null;
			DockControl selectedControl = controlLayoutSystem.SelectedControl;
			this.method_5();
			if (target != null && target.type != Class7.DockTargetType.None && target.type != Class7.DockTargetType.AlreadyActioned)
			{
				DesignerTransaction designerTransaction = designerHost.CreateTransaction("Move DockControl");
				try
				{
					Control control = this.dockContainer_0.Manager?.DockSystemContainer;
					if (control != null)
					{
						selectionService.SetSelectedComponents(new object[]
						{
							this.dockContainer_0.Manager.DockSystemContainer
						}, SelectionTypes.Replace);
					}
					else
					{
						selectionService.SetSelectedComponents(new object[]
						{
							designerHost.RootComponent
						}, SelectionTypes.Replace);
					}
					if (control != null)
					{
						componentChangeService.OnComponentChanging(control, TypeDescriptor.GetProperties(control)["Controls"]);
					}
					componentChangeService.OnComponentChanging(this.dockContainer_0, TypeDescriptor.GetProperties(this.dockContainer_0)["Manager"]);
					componentChangeService.OnComponentChanging(this.dockContainer_0, TypeDescriptor.GetProperties(this.dockContainer_0)["LayoutSystem"]);
					if (!flag)
					{
						LayoutUtilities.smethod_11(selectedControl);
					}
					else
					{
						LayoutUtilities.smethod_10(controlLayoutSystem);
					}
					componentChangeService.OnComponentChanged(this.dockContainer_0, TypeDescriptor.GetProperties(this.dockContainer_0)["LayoutSystem"], null, null);
					componentChangeService.OnComponentChanged(this.dockContainer_0, TypeDescriptor.GetProperties(this.dockContainer_0)["Manager"], null, null);
					if (control != null)
					{
						componentChangeService.OnComponentChanged(control, TypeDescriptor.GetProperties(control)["Controls"], null, null);
					}
					if (target.dockContainer == null)
					{
						if (target.type == Class7.DockTargetType.CreateNewContainer)
						{
							if (control != null)
							{
								componentChangeService.OnComponentChanging(control, TypeDescriptor.GetProperties(control)["Controls"]);
							}
							controlLayoutSystem.method_11(selectedControl.Manager, selectedControl, flag, target);
							designerHost.Container.Add(selectedControl.LayoutSystem.DockContainer);
							if (control != null)
							{
								componentChangeService.OnComponentChanged(control, TypeDescriptor.GetProperties(control)["Controls"], null, null);
							}
						}
					}
					else
					{
						componentChangeService.OnComponentChanging(target.dockContainer, TypeDescriptor.GetProperties(target.dockContainer)["LayoutSystem"]);
						controlLayoutSystem.method_11(target.dockContainer.Manager, selectedControl, flag, target);
						componentChangeService.OnComponentChanged(target.dockContainer, TypeDescriptor.GetProperties(target.dockContainer)["LayoutSystem"], null, null);
					}
					designerTransaction.Commit();
				}
				catch
				{
					designerTransaction.Cancel();
				}
				return;
			}
		}

		internal virtual void vmethod_1(object sender, EventArgs e)
		{
			this.method_5();
		}

		[Browsable(false), DefaultValue(false)]
		protected override bool DrawGrid
		{
			get
			{
				return false;
			}
			set
			{
				base.DrawGrid = value;
			}
		}

		public override SelectionRules SelectionRules => SelectionRules.Visible;

	    private Class10 class10_0;

		private Class11 class11_0;

		private Class7 class7_0;

		private DockContainer dockContainer_0;

		private DockControl dockControl_0;

		private IComponentChangeService icomponentChangeService_0;

		private IDesignerHost idesignerHost_0;

		private Point point_0 = Point.Empty;
	}
}
