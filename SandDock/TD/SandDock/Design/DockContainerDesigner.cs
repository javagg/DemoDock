using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TD.Util;

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
			//var arg_15_0 = (ISelectionService)GetService(typeof(ISelectionService));
            _component.ComponentRemoving += OnComponentRemoving;
			_component.ComponentRemoved += OnComponentRemoved;
			base.Dispose(disposing);
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
		    if (e.Component != _dockControl) return;
		    _dockControl = null;
		    RaiseComponentChanged(TypeDescriptor.GetProperties(_dockContainer)["LayoutSystem"], null, null);
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
		    var control = e.Component as DockControl;
		    if (control?.LayoutSystem != null && control.LayoutSystem.DockContainer == _dockContainer)
		    {
		        _dockControl = control;
		        RaiseComponentChanging(TypeDescriptor.GetProperties(_dockContainer)["LayoutSystem"]);
		    }
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
		    if (!(component is DockContainer))
		        SandDockLanguage.ShowCachedAssemblyError(component.GetType().Assembly, GetType().Assembly);

		    //ISelectionService arg_3F_0 = (ISelectionService)GetService(typeof(ISelectionService));
			_component = (IComponentChangeService)GetService(typeof(IComponentChangeService));
			_idesignerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
			_component.ComponentRemoving += OnComponentRemoving;
			_component.ComponentRemoved += OnComponentRemoved;
			_dockContainer = (DockContainer)component;
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			method_9();
		}

        [GuessedName]
		private DockControl GetDockControlAt(Point point)
		{
		    var layout = _dockContainer.GetLayoutSystemAt(point);
		    return (layout as ControlLayoutSystem)?.GetControlAt(point);
		}

	    private void method_1()
		{
			this.class11_0.Event_0 -= this.method_3;
			this.class11_0.ResizingManagerFinished -= this.OnResizingManagerFinished;
			this.class11_0 = null;
		}

		private void OnResizingManagerFinished(int newSize)
		{
			this.method_1();
			DesignerTransaction designerTransaction = this._idesignerHost.CreateTransaction("Resize Docked Windows");
			base.RaiseComponentChanging(TypeDescriptor.GetProperties(base.Component)["ContentSize"]);
			this._dockContainer.ContentSize = newSize;
			base.RaiseComponentChanged(TypeDescriptor.GetProperties(base.Component)["ContentSize"], null, null);
			designerTransaction.Commit();
		}

		private void method_3(object sender, EventArgs e)
		{
			this.method_1();
		}

		private void method_4(Point point_1)
		{
			LayoutSystemBase layoutSystemAt = this._dockContainer.GetLayoutSystemAt(point_1);
			if (layoutSystemAt is ControlLayoutSystem && this.class7_0 == null)
			{
				ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)layoutSystemAt;
				DockControl controlAt = controlLayoutSystem.GetControlAt(point_1);
				this.class7_0 = new Class8(this._dockContainer.Manager, this._dockContainer, controlLayoutSystem, controlAt, controlLayoutSystem.SelectedControl.MetaData.DockedContentSize, point_1, DockingHints.TranslucentFill);
				this.class7_0.DockingManagerFinished += this.vmethod_0;
				this.class7_0.Event_0 += this.vmethod_1;
				this._dockContainer.Capture = true;
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
			DesignerTransaction designerTransaction = this._idesignerHost.CreateTransaction("Resize Docked Windows");
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			componentChangeService.OnComponentChanging(this._dockContainer, TypeDescriptor.GetProperties(this._dockContainer)["LayoutSystem"]);
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
			componentChangeService.OnComponentChanged(this._dockContainer, TypeDescriptor.GetProperties(this._dockContainer)["LayoutSystem"], null, null);
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
			Point point = this._dockContainer.PointToClient(new Point(x, y));
			LayoutSystemBase layoutSystemAt = this._dockContainer.GetLayoutSystemAt(point);
			if (!(layoutSystemAt is SplitLayoutSystem))
			{
				if (this._dockContainer.Rectangle_0 != Rectangle.Empty && this._dockContainer.Rectangle_0.Contains(point))
				{
					this.class11_0 = new Class11(this._dockContainer.Manager, this._dockContainer, point);
					this.class11_0.Event_0 += new EventHandler(this.method_3);
					this.class11_0.ResizingManagerFinished += this.OnResizingManagerFinished;
					this._dockContainer.Capture = true;
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
							componentChangeService.OnComponentChanging(this._dockContainer, TypeDescriptor.GetProperties(this._dockContainer)["LayoutSystem"]);
							controlAt.LayoutSystem.SelectedControl = controlAt;
							componentChangeService.OnComponentChanged(this._dockContainer, TypeDescriptor.GetProperties(this._dockContainer)["LayoutSystem"], null, null);
						}
					}
					if (!controlLayoutSystem.rectangle_1.Contains(point) && controlAt == null)
					{
						selectionService.SetSelectedComponents(new object[]
						{
							this._dockContainer
						}, SelectionTypes.MouseDown | SelectionTypes.Click);
						this._dockContainer.Capture = true;
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
				if (splitLayoutSystem.Contains(point.X, point.Y))
				{
					LayoutSystemBase aboveLayout;
					LayoutSystemBase belowLayout;
					splitLayoutSystem.method_5(point, out aboveLayout, out belowLayout);
					this.class10_0 = new SplittingManager(this._dockContainer, splitLayoutSystem, aboveLayout, belowLayout, point, DockingHints.TranslucentFill);
					this.class10_0.Event_0 += new EventHandler(this.method_7);
					this.class10_0.SplittingManagerFinished += new SplittingManager.SplittingManagerFinishedEventHandler(this.method_8);
					this._dockContainer.Capture = true;
					return;
				}
			}
			selectionService.SetSelectedComponents(new object[]
			{
				this._dockContainer
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
					this._dockContainer.Capture = false;
				}
				else if (this.class11_0 != null)
				{
					this.class11_0.Commit();
					this._dockContainer.Capture = false;
				}
				else if (this.class7_0 != null)
				{
					this.class7_0.Commit();
					this._dockContainer.Capture = false;
				}
				else if (this.GetDockControlAt(this._dockContainer.PointToClient(Cursor.Position)) == null)
				{
					LayoutSystemBase layoutSystemAt = this._dockContainer.GetLayoutSystemAt(this._dockContainer.PointToClient(Cursor.Position));
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
			Point position = this._dockContainer.PointToClient(new Point(x, y));
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
						this.method_4(this._dockContainer.PointToClient(this.point_0));
						this.point_0 = Point.Empty;
					}
				}
				return;
			}
			this.class7_0.OnMouseMove(Cursor.Position);
			if (this.class7_0.Target != null && this.class7_0.Target.type != Class7.DockTargetType.None)
			{
				Cursor.Current = Cursors.Default;
				return;
			}
			Cursor.Current = Cursors.No;
		}

		protected override void OnSetCursor()
		{
			Point point = this._dockContainer.PointToClient(Cursor.Position);
			LayoutSystemBase layoutSystemAt = this._dockContainer.GetLayoutSystemAt(point);
			SplitLayoutSystem splitLayoutSystem = layoutSystemAt as SplitLayoutSystem;
			if (splitLayoutSystem != null && splitLayoutSystem.Contains(point.X, point.Y))
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
				if (!(this._dockContainer.Rectangle_0 != Rectangle.Empty) || !this._dockContainer.Rectangle_0.Contains(point))
				{
					Cursor.Current = Cursors.Default;
					return;
				}
				if (!this._dockContainer.Boolean_1)
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
			ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)this.class7_0.SourceControlSystem;
			bool flag = this.class7_0.SourceControl == null;
			DockControl selectedControl = controlLayoutSystem.SelectedControl;
			this.method_5();
			if (target != null && target.type != Class7.DockTargetType.None && target.type != Class7.DockTargetType.AlreadyActioned)
			{
				DesignerTransaction designerTransaction = designerHost.CreateTransaction("Move DockControl");
				try
				{
					Control control = this._dockContainer.Manager?.DockSystemContainer;
					if (control != null)
					{
						selectionService.SetSelectedComponents(new object[]
						{
							this._dockContainer.Manager.DockSystemContainer
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
					componentChangeService.OnComponentChanging(this._dockContainer, TypeDescriptor.GetProperties(this._dockContainer)["Manager"]);
					componentChangeService.OnComponentChanging(this._dockContainer, TypeDescriptor.GetProperties(this._dockContainer)["LayoutSystem"]);
					if (!flag)
					{
						LayoutUtilities.smethod_11(selectedControl);
					}
					else
					{
						LayoutUtilities.smethod_10(controlLayoutSystem);
					}
					componentChangeService.OnComponentChanged(this._dockContainer, TypeDescriptor.GetProperties(this._dockContainer)["LayoutSystem"], null, null);
					componentChangeService.OnComponentChanged(this._dockContainer, TypeDescriptor.GetProperties(this._dockContainer)["Manager"], null, null);
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

	    private SplittingManager class10_0;

		private Class11 class11_0;

		private Class7 class7_0;

		private DockContainer _dockContainer;

		private DockControl _dockControl;

		private IComponentChangeService _component;

		private IDesignerHost _idesignerHost;

		private Point point_0 = Point.Empty;
	}
}
