using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
	[TypeConverter(typeof(Class24))]
	public class SplitLayoutSystem : LayoutSystemBase
	{
		public SplitLayoutSystem()
		{
			this.layoutSystemBaseCollection_0 = new SplitLayoutSystem.LayoutSystemBaseCollection(this);
			this.arrayList_0 = new ArrayList();
		}

		public SplitLayoutSystem(int desiredWidth, int desiredHeight) : this()
		{
			base.WorkingSize = new SizeF((float)desiredWidth, (float)desiredHeight);
		}

		public SplitLayoutSystem(SizeF workingSize, Orientation splitMode, LayoutSystemBase[] layoutSystems) : this()
		{
			base.WorkingSize = workingSize;
			this.orientation_0 = splitMode;
			this.layoutSystemBaseCollection_0.AddRange(layoutSystems);
		}

		[Obsolete("Use the constructor taking a SizeF instead.")]
		public SplitLayoutSystem(int desiredWidth, int desiredHeight, Orientation splitMode, LayoutSystemBase[] layoutSystems) : this(desiredWidth, desiredHeight)
		{
			this.orientation_0 = splitMode;
			this.layoutSystemBaseCollection_0.AddRange(layoutSystems);
		}

		protected internal override void Layout(RendererBase renderer, Graphics graphics, Rectangle bounds, bool floating)
		{
			base.Layout(renderer, graphics, bounds, floating);
			int num;
			LayoutSystemBase[] array = this.method_9(out num);
			if (num != 0)
			{
				if (num > 1)
				{
					floating = false;
				}
				int num2 = (this.orientation_0 == Orientation.Horizontal) ? (bounds.Height - (num - 1) * 4) : (bounds.Width - (num - 1) * 4);
				float num3 = 0f;
				for (int i = 0; i < num; i++)
				{
					num3 += ((this.orientation_0 == Orientation.Horizontal) ? array[i].WorkingSize.Height : array[i].WorkingSize.Width);
				}
				this.arrayList_0.Clear();
				if (num2 > 0)
				{
					SizeF[] array2 = new SizeF[num];
					for (int j = 0; j < num; j++)
					{
						array2[j] = array[j].WorkingSize;
					}
					if ((float)num2 != num3)
					{
						float num4 = (float)num2 - num3;
						for (int k = 0; k < num; k++)
						{
							float num5 = (this.orientation_0 != Orientation.Horizontal) ? array2[k].Width : array2[k].Height;
							num5 += num4 * (num5 / num3);
							if (this.orientation_0 != Orientation.Horizontal)
							{
								array2[k].Width = num5;
							}
							else
							{
								array2[k].Height = num5;
							}
						}
						num3 = 0f;
						for (int l = 0; l < num; l++)
						{
							num3 += ((this.orientation_0 == Orientation.Horizontal) ? array2[l].Height : array2[l].Width);
						}
						num4 = (float)num2 - num3;
						if (this.orientation_0 != Orientation.Horizontal)
						{
							SizeF[] expr_1AE_cp_0 = array2;
							int expr_1AE_cp_1 = 0;
							expr_1AE_cp_0[expr_1AE_cp_1].Width = expr_1AE_cp_0[expr_1AE_cp_1].Width + num4;
						}
						else
						{
							SizeF[] expr_1C6_cp_0 = array2;
							int expr_1C6_cp_1 = 0;
							expr_1C6_cp_0[expr_1C6_cp_1].Height = expr_1C6_cp_0[expr_1C6_cp_1].Height + num4;
						}
					}
					int num6 = (this.orientation_0 != Orientation.Horizontal) ? bounds.X : bounds.Y;
					for (int m = 0; m < num; m++)
					{
						int num7 = (this.orientation_0 != Orientation.Horizontal) ? Convert.ToInt32(array2[m].Width) : Convert.ToInt32(array2[m].Height);
						num7 = Math.Max(num7, 4);
						if (this.orientation_0 == Orientation.Horizontal)
						{
							if (m == num - 1)
							{
								num7 = bounds.Bottom - num6;
							}
							array[m].Layout(renderer, graphics, new Rectangle(bounds.X, num6, bounds.Width, num7), floating);
						}
						else
						{
							if (m == num - 1)
							{
								num7 = bounds.Right - num6;
							}
							array[m].Layout(renderer, graphics, new Rectangle(num6, bounds.Y, num7, bounds.Height), floating);
						}
						num6 += num7 + 4;
					}
					for (int n = 0; n < num - 1; n++)
					{
						bounds = array[n].Bounds;
						if (this.orientation_0 != Orientation.Horizontal)
						{
							bounds.Offset(bounds.Width, 0);
							bounds.Width = 4;
						}
						else
						{
							bounds.Offset(0, bounds.Height);
							bounds.Height = 4;
						}
						this.arrayList_0.Add(bounds);
					}
				}
				return;
			}
		}

		private void method_10()
		{
			this.class10_0.Event_0 -= new EventHandler(this.method_11);
			this.class10_0.Event_1 -= new Class10.SplittingManagerFinishedEventHandler(this.method_12);
			this.class10_0 = null;
		}

		private void method_11(object sender, EventArgs e)
		{
			this.method_10();
		}

		private void method_12(LayoutSystemBase layoutSystemBase_0, LayoutSystemBase layoutSystemBase_1, float float_0, float float_1)
		{
			this.method_10();
			if (float_0 > 0f && float_1 > 0f)
			{
				SizeF workingSize = layoutSystemBase_0.WorkingSize;
				SizeF workingSize2 = layoutSystemBase_1.WorkingSize;
				if (this.SplitMode != Orientation.Horizontal)
				{
					workingSize.Width = float_0;
					workingSize2.Width = float_1;
				}
				else
				{
					workingSize.Height = float_0;
					workingSize2.Height = float_1;
				}
				layoutSystemBase_0.WorkingSize = workingSize;
				layoutSystemBase_1.WorkingSize = workingSize2;
				this.method_8();
			}
		}

		internal bool method_13()
		{
			IEnumerator enumerator = this.layoutSystemBaseCollection_0.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					LayoutSystemBase layoutSystemBase = (LayoutSystemBase)enumerator.Current;
					if (!(layoutSystemBase is ControlLayoutSystem))
					{
						if (((SplitLayoutSystem)layoutSystemBase).method_13())
						{
							result = true;
							return result;
						}
					}
					else
					{
						ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)layoutSystemBase;
						if (!controlLayoutSystem.Collapsed || (base.IsInContainer && !base.DockContainer.Boolean_6))
						{
							result = true;
							return result;
						}
					}
				}
				return false;
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

		private void method_4(SplitLayoutSystem splitLayoutSystem_1, ArrayList arrayList_1)
		{
			foreach (LayoutSystemBase layoutSystemBase in splitLayoutSystem_1.layoutSystemBaseCollection_0)
			{
				if (!(layoutSystemBase is SplitLayoutSystem))
				{
					if (!(layoutSystemBase is ControlLayoutSystem))
					{
						continue;
					}
					IEnumerator enumerator2 = ((ControlLayoutSystem)layoutSystemBase).Controls.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							DockControl value = (DockControl)enumerator2.Current;
							arrayList_1.Add(value);
						}
						continue;
					}
					finally
					{
						IDisposable disposable = enumerator2 as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				this.method_4((SplitLayoutSystem)layoutSystemBase, arrayList_1);
			}
		}

		internal void method_5(Point point_0, out LayoutSystemBase layoutSystemBase_0, out LayoutSystemBase layoutSystemBase_1)
		{
			int num = 0;
			foreach (LayoutSystemBase layoutSystemBase in this.LayoutSystems)
			{
				if ((!(layoutSystemBase is ControlLayoutSystem) || !((ControlLayoutSystem)layoutSystemBase).Collapsed) && ((this.SplitMode == Orientation.Horizontal && point_0.Y >= layoutSystemBase.Bounds.Bottom && point_0.Y <= layoutSystemBase.Bounds.Bottom + 4) || (this.SplitMode == Orientation.Vertical && point_0.X >= layoutSystemBase.Bounds.Right && point_0.X <= layoutSystemBase.Bounds.Right + 4)))
				{
					num = this.LayoutSystems.IndexOf(layoutSystemBase);
					break;
				}
			}
			layoutSystemBase_0 = this.LayoutSystems[num];
			layoutSystemBase_1 = layoutSystemBase_0;
			for (int i = num + 1; i < this.layoutSystemBaseCollection_0.Count; i++)
			{
				if (!(this.layoutSystemBaseCollection_0[i] is ControlLayoutSystem) || !((ControlLayoutSystem)this.layoutSystemBaseCollection_0[i]).Collapsed)
				{
					layoutSystemBase_1 = this.LayoutSystems[i];
					return;
				}
			}
		}

		internal bool method_6(int int_3, int int_4)
		{
			IEnumerator enumerator = this.arrayList_0.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					Rectangle rectangle = (Rectangle)enumerator.Current;
					if (rectangle.Contains(int_3, int_4))
					{
						result = true;
						return result;
					}
				}
				return false;
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

		internal void method_7()
		{
			if (base.DockContainer != null)
			{
				base.DockContainer.vmethod_2();
			}
			if (this.eventHandler_0 != null)
			{
				this.eventHandler_0(this, EventArgs.Empty);
			}
		}

		internal void method_8()
		{
			if (base.DockContainer != null)
			{
				base.DockContainer.method_10(this, base.Bounds);
			}
			if (base.DockContainer != null)
			{
				base.DockContainer.Invalidate(base.Bounds);
			}
		}

		private LayoutSystemBase[] method_9(out int int_3)
		{
			int_3 = 0;
			LayoutSystemBase[] array = new LayoutSystemBase[this.LayoutSystems.Count];
			foreach (LayoutSystemBase layoutSystemBase in this.LayoutSystems)
			{
				if (!(layoutSystemBase is ControlLayoutSystem))
				{
					if (layoutSystemBase is SplitLayoutSystem)
					{
						SplitLayoutSystem splitLayoutSystem = (SplitLayoutSystem)layoutSystemBase;
						if (splitLayoutSystem.method_13())
						{
							array[int_3++] = layoutSystemBase;
						}
					}
				}
				else
				{
					ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)layoutSystemBase;
					if (!controlLayoutSystem.Collapsed || (base.IsInContainer && !base.DockContainer.Boolean_6))
					{
						array[int_3++] = layoutSystemBase;
					}
				}
			}
			return array;
		}

		public void MoveToLayoutSystem(ControlLayoutSystem layoutSystem)
		{
			this.MoveToLayoutSystem(layoutSystem, 0);
		}

		public void MoveToLayoutSystem(ControlLayoutSystem layoutSystem, int index)
		{
			DockControl dockControl = null;
			if (this.LayoutSystems.Count == 1)
			{
				if (this.LayoutSystems[0] is ControlLayoutSystem)
				{
					dockControl = ((ControlLayoutSystem)this.LayoutSystems[0]).SelectedControl;
				}
			}
			DockControl[] dockControl_ = this.DockControl_0;
			for (int i = dockControl_.Length - 1; i >= 0; i--)
			{
				DockControl control = dockControl_[i];
				layoutSystem.Controls.Insert(index, control);
			}
			if (dockControl != null)
			{
				layoutSystem.SelectedControl = dockControl;
			}
		}

		protected internal override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			foreach (Rectangle rectangle in this.arrayList_0)
			{
				if (rectangle.Contains(e.X, e.Y))
				{
					LayoutSystemBase aboveLayout;
					LayoutSystemBase belowLayout;
					this.method_5(new Point(e.X, e.Y), out aboveLayout, out belowLayout);
					if (this.class10_0 != null)
					{
						this.class10_0.Dispose();
					}
					DockingHints dockingHints = (base.DockContainer.Manager != null) ? base.DockContainer.Manager.DockingHints : DockingHints.TranslucentFill;
					this.class10_0 = new Class10(base.DockContainer, this, aboveLayout, belowLayout, new Point(e.X, e.Y), dockingHints);
					this.class10_0.Event_0 += new EventHandler(this.method_11);
					this.class10_0.Event_1 += new Class10.SplittingManagerFinishedEventHandler(this.method_12);
					break;
				}
			}
		}

		protected internal override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (this.class7_0 != null)
				{
					this.class7_0.OnMouseMove(Cursor.Position);
					return;
				}
				if (this.class10_0 != null)
				{
					this.class10_0.OnMouseMove(new Point(e.X, e.Y));
					return;
				}
			}
			if (!this.method_6(e.X, e.Y))
			{
				Cursor.Current = Cursors.Default;
			}
			else if (this.orientation_0 == Orientation.Horizontal)
			{
				Cursor.Current = Cursors.HSplit;
			}
			else
			{
				Cursor.Current = Cursors.VSplit;
			}
			base.OnMouseMove(e);
		}

		protected internal override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this.class7_0 == null)
			{
				if (this.class10_0 != null)
				{
					this.class10_0.Commit();
				}
				return;
			}
			this.class7_0.Commit();
		}

		public bool Optimize()
		{
			if (this.LayoutSystems.Count != 1 || !(this.LayoutSystems[0] is SplitLayoutSystem))
			{
				IEnumerator enumerator = this.LayoutSystems.GetEnumerator();
				bool result;
				try
				{
					while (enumerator.MoveNext())
					{
						LayoutSystemBase layoutSystemBase = (LayoutSystemBase)enumerator.Current;
						if (layoutSystemBase is SplitLayoutSystem)
						{
							SplitLayoutSystem splitLayoutSystem = (SplitLayoutSystem)layoutSystemBase;
							if (splitLayoutSystem.SplitMode == this.SplitMode)
							{
								LayoutSystemBase[] array = new LayoutSystemBase[splitLayoutSystem.LayoutSystems.Count];
								splitLayoutSystem.LayoutSystems.CopyTo(array, 0);
								splitLayoutSystem.LayoutSystems.bool_0 = true;
								splitLayoutSystem.LayoutSystems.Clear();
								int index = this.LayoutSystems.IndexOf(splitLayoutSystem);
								this.LayoutSystems.bool_0 = true;
								this.LayoutSystems.Remove(splitLayoutSystem);
								for (int i = array.Length - 1; i >= 0; i--)
								{
									this.LayoutSystems.Insert(index, array[i]);
								}
								this.LayoutSystems.bool_0 = false;
								result = true;
								return result;
							}
							if (splitLayoutSystem.Optimize())
							{
								result = true;
								return result;
							}
						}
					}
					return false;
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
			SplitLayoutSystem splitLayoutSystem2 = (SplitLayoutSystem)this.LayoutSystems[0];
			if (splitLayoutSystem2.LayoutSystems.Count == 1 && splitLayoutSystem2.LayoutSystems[0] is SplitLayoutSystem)
			{
				if (((SplitLayoutSystem)splitLayoutSystem2.LayoutSystems[0]).SplitMode == this.SplitMode)
				{
					SplitLayoutSystem splitLayoutSystem3 = (SplitLayoutSystem)splitLayoutSystem2.LayoutSystems[0];
					LayoutSystemBase[] array2 = new LayoutSystemBase[splitLayoutSystem3.LayoutSystems.Count];
					splitLayoutSystem3.LayoutSystems.CopyTo(array2, 0);
					splitLayoutSystem3.LayoutSystems.bool_0 = true;
					splitLayoutSystem3.LayoutSystems.Clear();
					this.LayoutSystems.bool_0 = true;
					this.LayoutSystems.Clear();
					this.LayoutSystems.AddRange(array2);
					this.LayoutSystems.bool_0 = false;
					return true;
				}
			}
			return false;
		}

		internal override void vmethod_0(Class7.DockTarget dockTarget_0)
		{
			base.vmethod_0(dockTarget_0);
			if (dockTarget_0 == null || dockTarget_0.type == Class7.DockTargetType.None || dockTarget_0.type == Class7.DockTargetType.AlreadyActioned)
			{
				return;
			}
			Class5 @class = (Class5)base.DockContainer;
			SandDockManager manager = base.DockContainer.Manager;
			if (dockTarget_0.type == Class7.DockTargetType.Float)
			{
				@class.method_19(dockTarget_0.bounds, true, true);
				return;
			}
			DockControl[] dockControl_ = this.DockControl_0;
			DockControl dockControl_2 = @class.DockControl_0;
			@class.LayoutSystem = new SplitLayoutSystem();
			@class.Dispose();
			try
			{
				if (dockTarget_0.type == Class7.DockTargetType.CreateNewContainer)
				{
					DockContainer dockContainer = manager.FindDockContainer(dockTarget_0.dockLocation);
					if (dockTarget_0.dockLocation == ContainerDockLocation.Center && dockContainer != null)
					{
						ControlLayoutSystem layoutSystem = LayoutUtilities.FindControlLayoutSystem(dockContainer);
						this.MoveToLayoutSystem(layoutSystem);
					}
					else
					{
						base.method_2(manager, dockTarget_0.dockLocation, dockTarget_0.middle ? ContainerDockEdge.Inside : ContainerDockEdge.Outside);
					}
				}
				else if (dockTarget_0.type != Class7.DockTargetType.JoinExistingSystem)
				{
					if (dockTarget_0.type == Class7.DockTargetType.SplitExistingSystem)
					{
						if (dockTarget_0.dockContainer is DocumentContainer)
						{
							ControlLayoutSystem controlLayoutSystem = dockTarget_0.dockContainer.CreateNewLayoutSystem(base.WorkingSize);
							controlLayoutSystem.Controls.AddRange(dockControl_);
							dockTarget_0.layoutSystem.SplitForLayoutSystem(controlLayoutSystem, dockTarget_0.dockSide);
						}
						else if (this.LayoutSystems.Count == 1 && this.LayoutSystems[0] is ControlLayoutSystem)
						{
							ControlLayoutSystem layoutSystem2 = (ControlLayoutSystem)this.LayoutSystems[0];
							this.LayoutSystems.Remove(layoutSystem2);
							dockTarget_0.layoutSystem.SplitForLayoutSystem(layoutSystem2, dockTarget_0.dockSide);
						}
						else
						{
							dockTarget_0.layoutSystem.SplitForLayoutSystem(this, dockTarget_0.dockSide);
						}
					}
				}
				else
				{
					this.MoveToLayoutSystem(dockTarget_0.layoutSystem, dockTarget_0.index);
				}
			}
			finally
			{
				dockControl_2.Activate();
			}
		}

		internal override void vmethod_2(DockContainer dockContainer_1)
		{
			base.vmethod_2(dockContainer_1);
			foreach (LayoutSystemBase layoutSystemBase in this.LayoutSystems)
			{
				layoutSystemBase.vmethod_2(dockContainer_1);
			}
		}

		internal override bool vmethod_3(ContainerDockLocation containerDockLocation_0)
		{
			IEnumerator enumerator = this.LayoutSystems.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					LayoutSystemBase layoutSystemBase = (LayoutSystemBase)enumerator.Current;
					if (!layoutSystemBase.vmethod_3(containerDockLocation_0))
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
			Control container = (base.DockContainer.Manager != null) ? base.DockContainer.Manager.DockSystemContainer : null;
			foreach (Rectangle bounds in this.arrayList_0)
			{
				rendererBase_0.DrawSplitter(container, base.DockContainer, graphics_0, bounds, this.orientation_0);
			}
			foreach (LayoutSystemBase layoutSystemBase in this.LayoutSystems)
			{
				if (!(layoutSystemBase is ControlLayoutSystem) || !((ControlLayoutSystem)layoutSystemBase).Collapsed || !base.DockContainer.Boolean_6)
				{
					Region clip = graphics_0.Clip;
					graphics_0.SetClip(layoutSystemBase.Bounds);
					layoutSystemBase.vmethod_4(rendererBase_0, graphics_0, font_0);
					graphics_0.Clip = clip;
				}
			}
		}

		internal override bool Boolean_2
		{
			get
			{
				foreach (LayoutSystemBase layoutSystemBase in this.LayoutSystems)
				{
					if (layoutSystemBase.Boolean_2)
					{
						return true;
					}
				}
				return false;
			}
		}

		internal override bool Boolean_3
		{
			get
			{
				IEnumerator enumerator = this.LayoutSystems.GetEnumerator();
				bool result;
				try
				{
					while (enumerator.MoveNext())
					{
						LayoutSystemBase layoutSystemBase = (LayoutSystemBase)enumerator.Current;
						if (!layoutSystemBase.Boolean_3)
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

		internal override bool Boolean_4
		{
			get
			{
				IEnumerator enumerator = this.LayoutSystems.GetEnumerator();
				bool result;
				try
				{
					while (enumerator.MoveNext())
					{
						LayoutSystemBase layoutSystemBase = (LayoutSystemBase)enumerator.Current;
						if (!layoutSystemBase.Boolean_4)
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

		internal override DockControl[] DockControl_0
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				this.method_4(this, arrayList);
				return (DockControl[])arrayList.ToArray(typeof(DockControl));
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SplitLayoutSystem.LayoutSystemBaseCollection LayoutSystems
		{
			get
			{
				return this.layoutSystemBaseCollection_0;
			}
		}

		[Category("Layout"), DefaultValue(typeof(Orientation), "Horizontal"), Description("Indicates whether this layout is split horizontally or vertically.")]
		public Orientation SplitMode
		{
			get
			{
				return this.orientation_0;
			}
			set
			{
				this.orientation_0 = value;
				this.method_8();
			}
		}

		internal event EventHandler Event_0
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.eventHandler_0 = (EventHandler)Delegate.Combine(this.eventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.eventHandler_0 = (EventHandler)Delegate.Remove(this.eventHandler_0, value);
			}
		}

		private ArrayList arrayList_0;

		private Class10 class10_0;

		private EventHandler eventHandler_0;

		internal const int int_2 = 4;

		private SplitLayoutSystem.LayoutSystemBaseCollection layoutSystemBaseCollection_0;

		private Orientation orientation_0;

		public class LayoutSystemBaseCollection : CollectionBase
		{
			internal LayoutSystemBaseCollection(SplitLayoutSystem parent)
			{
				this.splitLayoutSystem_0 = parent;
			}

			public int Add(LayoutSystemBase layoutSystem)
			{
				int count = base.Count;
				this.Insert(count, layoutSystem);
				return count;
			}

			public void AddRange(LayoutSystemBase[] layoutSystems)
			{
				this.bool_0 = true;
				for (int i = 0; i < layoutSystems.Length; i++)
				{
					LayoutSystemBase layoutSystem = layoutSystems[i];
					this.Add(layoutSystem);
				}
				this.bool_0 = false;
				this.method_0();
			}

			public bool Contains(LayoutSystemBase layoutSystem)
			{
				return base.List.Contains(layoutSystem);
			}

			public void CopyTo(LayoutSystemBase[] array, int index)
			{
				base.List.CopyTo(array, index);
			}

			public int IndexOf(LayoutSystemBase layoutSystem)
			{
				return base.List.IndexOf(layoutSystem);
			}

			public void Insert(int index, LayoutSystemBase layoutSystem)
			{
				if (layoutSystem.splitLayoutSystem_0 != null)
				{
					throw new ArgumentException("Layout system already has a parent. You must first remove it from its parent.");
				}
				base.List.Insert(index, layoutSystem);
			}

			private void method_0()
			{
				this.splitLayoutSystem_0.method_7();
			}

			protected override void OnClear()
			{
				base.OnClear();
				foreach (LayoutSystemBase layoutSystemBase in this)
				{
					layoutSystemBase.splitLayoutSystem_0 = null;
					layoutSystemBase.vmethod_2(null);
				}
			}

			protected override void OnClearComplete()
			{
				base.OnClearComplete();
				if (!this.bool_0)
				{
					this.method_0();
				}
			}

			protected override void OnInsertComplete(int index, object value)
			{
				base.OnInsertComplete(index, value);
				LayoutSystemBase layoutSystemBase = (LayoutSystemBase)value;
				layoutSystemBase.splitLayoutSystem_0 = this.splitLayoutSystem_0;
				layoutSystemBase.vmethod_2(this.splitLayoutSystem_0.DockContainer);
				if (!this.bool_0)
				{
					this.method_0();
				}
			}

			protected override void OnRemoveComplete(int index, object value)
			{
				base.OnRemoveComplete(index, value);
				((LayoutSystemBase)value).splitLayoutSystem_0 = null;
				((LayoutSystemBase)value).vmethod_2(null);
				if (!this.bool_0)
				{
					if (base.Count > 1 || this.splitLayoutSystem_0.splitLayoutSystem_0 == null)
					{
						this.method_0();
					}
					else
					{
						SplitLayoutSystem splitLayoutSystem = this.splitLayoutSystem_0.splitLayoutSystem_0;
						if (base.Count == 1)
						{
							LayoutSystemBase layoutSystem = this[0];
							this.bool_0 = true;
							this.Remove(layoutSystem);
							this.bool_0 = false;
							splitLayoutSystem.LayoutSystems.bool_0 = true;
							int index2 = splitLayoutSystem.LayoutSystems.IndexOf(this.splitLayoutSystem_0);
							splitLayoutSystem.LayoutSystems.Remove(this.splitLayoutSystem_0);
							splitLayoutSystem.LayoutSystems.Insert(index2, layoutSystem);
							splitLayoutSystem.LayoutSystems.bool_0 = false;
							splitLayoutSystem.method_7();
							return;
						}
						if (base.Count == 0)
						{
							splitLayoutSystem.LayoutSystems.Remove(this.splitLayoutSystem_0);
							return;
						}
					}
				}
			}

			public void Remove(LayoutSystemBase layoutSystem)
			{
				base.List.Remove(layoutSystem);
			}

			public LayoutSystemBase this[int index]
			{
				get
				{
					return (LayoutSystemBase)base.List[index];
				}
			}

			internal bool bool_0;

			private SplitLayoutSystem splitLayoutSystem_0;
		}
	}
}
