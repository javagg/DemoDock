using System;
using System.Collections;
using System.ComponentModel;
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
    internal struct Struct0
    {
        public Struct0(SplitLayoutSystem splitLayoutSystem, int index)
        {
            SplitLayout = splitLayoutSystem;
            Index = index;
        }

        public SplitLayoutSystem SplitLayout;

        public int Index;
    }

    internal class Class24 : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException();
            if (destinationType != typeof(InstanceDescriptor) || value.GetType().Name != "SplitLayoutSystem")
                return base.ConvertTo(context, culture, value, destinationType);

            Type type = value.GetType();
            Type baseType = type.BaseType;
            MemberInfo constructor = type.GetConstructor(new[] { typeof(SizeF), typeof(Orientation), MakeArrayType(baseType) });
            PropertyInfo property = type.GetProperty("LayoutSystems", BindingFlags.Instance | BindingFlags.Public);
            ICollection collection = (ICollection)property.GetValue(value, null);
            object[] array = (object[])Activator.CreateInstance(MakeArrayType(baseType), collection.Count);
            collection.CopyTo(array, 0);
            PropertyInfo property2 = type.GetProperty("WorkingSize", BindingFlags.Instance | BindingFlags.Public);
            SizeF sizeF = (SizeF)property2.GetValue(value, null);
            PropertyInfo property3 = type.GetProperty("SplitMode", BindingFlags.Instance | BindingFlags.Public);
            Orientation orientation = (Orientation)property3.GetValue(value, null);
            return new InstanceDescriptor(constructor, new object[] { sizeF, orientation, array });
        }

        private Type MakeArrayType(Type firstType) => firstType.Assembly.GetType(firstType.FullName + "[]");
    }
    [TypeConverter(typeof(Class24))]
	public class SplitLayoutSystem : LayoutSystemBase
	{
		public SplitLayoutSystem()
		{
            LayoutSystems = new LayoutSystemBaseCollection(this);
			_splitterRects = new ArrayList();
		}

		public SplitLayoutSystem(int desiredWidth, int desiredHeight) : this()
		{
			WorkingSize = new SizeF(desiredWidth, desiredHeight);
		}

		public SplitLayoutSystem(SizeF workingSize, Orientation splitMode, LayoutSystemBase[] layoutSystems) : this()
		{
			WorkingSize = workingSize;
			_splitMode = splitMode;
			LayoutSystems.AddRange(layoutSystems);
		}

		[Obsolete("Use the constructor taking a SizeF instead.")]
		public SplitLayoutSystem(int desiredWidth, int desiredHeight, Orientation splitMode, LayoutSystemBase[] layoutSystems) : this(desiredWidth, desiredHeight)
		{
			_splitMode = splitMode;
			LayoutSystems.AddRange(layoutSystems);
		}

		protected internal override void Layout(RendererBase renderer, Graphics graphics, Rectangle bounds, bool floating)
		{
			base.Layout(renderer, graphics, bounds, floating);
			int num;
			LayoutSystemBase[] array = method_9(out num);
			if (num != 0)
			{
				if (num > 1)
				{
					floating = false;
				}
				int num2 = (_splitMode == Orientation.Horizontal) ? (bounds.Height - (num - 1) * 4) : (bounds.Width - (num - 1) * 4);
				float num3 = 0f;
				for (int i = 0; i < num; i++)
				{
					num3 += ((_splitMode == Orientation.Horizontal) ? array[i].WorkingSize.Height : array[i].WorkingSize.Width);
				}
				_splitterRects.Clear();
				if (num2 > 0)
				{
					SizeF[] array2 = new SizeF[num];
					for (int j = 0; j < num; j++)
					{
						array2[j] = array[j].WorkingSize;
					}
					if (num2 != num3)
					{
						float num4 = num2 - num3;
						for (int k = 0; k < num; k++)
						{
							float num5 = (_splitMode != Orientation.Horizontal) ? array2[k].Width : array2[k].Height;
							num5 += num4 * (num5 / num3);
							if (_splitMode != Orientation.Horizontal)
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
							num3 += ((_splitMode == Orientation.Horizontal) ? array2[l].Height : array2[l].Width);
						}
						num4 = num2 - num3;
						if (_splitMode != Orientation.Horizontal)
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
					int num6 = (_splitMode != Orientation.Horizontal) ? bounds.X : bounds.Y;
					for (int m = 0; m < num; m++)
					{
						int num7 = (_splitMode != Orientation.Horizontal) ? Convert.ToInt32(array2[m].Width) : Convert.ToInt32(array2[m].Height);
						num7 = Math.Max(num7, 4);
						if (_splitMode == Orientation.Horizontal)
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
						if (_splitMode != Orientation.Horizontal)
						{
							bounds.Offset(bounds.Width, 0);
							bounds.Width = 4;
						}
						else
						{
							bounds.Offset(0, bounds.Height);
							bounds.Height = 4;
						}
						_splitterRects.Add(bounds);
					}
				}
			}
		}

		private void method_10()
		{
			_splittingManager.Cancalled -= method_11;
			_splittingManager.SplittingManagerFinished -= OnSplittingManagerFinished;
			_splittingManager = null;
		}

		private void method_11(object sender, EventArgs e)
		{
			method_10();
		}

		private void OnSplittingManagerFinished(LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, float aboveSize, float belowSize)
		{
			method_10();
		    if (aboveSize <= 0 || belowSize <= 0) return;

            var aboveWorkingSize = aboveLayout.WorkingSize;
		    var belowWorkingSize = belowLayout.WorkingSize;
		    if (SplitMode != Orientation.Horizontal)
		    {
		        aboveWorkingSize.Width = aboveSize;
		        belowWorkingSize.Width = belowSize;
		    }
		    else
		    {
		        aboveWorkingSize.Height = aboveSize;
		        belowWorkingSize.Height = belowSize;
		    }
		    aboveLayout.WorkingSize = aboveWorkingSize;
		    belowLayout.WorkingSize = belowWorkingSize;
		    method_8();
		}

		internal bool method_13()
		{
			IEnumerator enumerator = LayoutSystems.GetEnumerator();
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
						if (!controlLayoutSystem.Collapsed || (IsInContainer && !DockContainer.CanShowCollapsed))
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
		}

		private void method_4(SplitLayoutSystem splitLayoutSystem_1, ArrayList arrayList_1)
		{
			foreach (LayoutSystemBase layoutSystemBase in splitLayoutSystem_1.LayoutSystems)
			{
				if (!(layoutSystemBase is SplitLayoutSystem))
				{
				    if (layoutSystemBase is ControlLayoutSystem)
				    {
				        IEnumerator enumerator2 = ((ControlLayoutSystem) layoutSystemBase).Controls.GetEnumerator();
				        try
				        {
				            while (enumerator2.MoveNext())
				            {
				                DockControl value = (DockControl) enumerator2.Current;
				                arrayList_1.Add(value);
				            }
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
				    continue;
				}
				method_4((SplitLayoutSystem)layoutSystemBase, arrayList_1);
			}
		}

		internal void method_5(Point point_0, out LayoutSystemBase layoutSystemBase_0, out LayoutSystemBase layoutSystemBase_1)
		{
			var index = LayoutSystems.Cast<LayoutSystemBase>()
			    .Where(			        layout =>
			            (!(layout is ControlLayoutSystem) || !((ControlLayoutSystem) layout).Collapsed) &&
			            ((SplitMode == Orientation.Horizontal && point_0.Y >= layout.Bounds.Bottom &&
			              point_0.Y <= layout.Bounds.Bottom + 4) ||
			             (SplitMode == Orientation.Vertical && point_0.X >= layout.Bounds.Right &&
			              point_0.X <= layout.Bounds.Right + 4)))
			    .Select(layoutSystemBase => LayoutSystems.IndexOf(layoutSystemBase)).FirstOrDefault();
		    layoutSystemBase_0 = LayoutSystems[index];
			layoutSystemBase_1 = layoutSystemBase_0;
			for (var i = index + 1; i < LayoutSystems.Count; i++)
			{
				if (!(LayoutSystems[i] is ControlLayoutSystem) || !((ControlLayoutSystem)LayoutSystems[i]).Collapsed)
				{
					layoutSystemBase_1 = LayoutSystems[i];
					return;
				}
			}
		}

		internal bool Contains(int x, int y) => _splitterRects.Cast<Rectangle>().Any(r => r.Contains(x, y));

	    internal void method_7()
		{
	        DockContainer?.vmethod_2();
            LayoutSystemsChanged?.Invoke(this, EventArgs.Empty);
		}

		internal void method_8()
		{
            DockContainer?.method_10(this, Bounds);
		    DockContainer?.Invalidate(Bounds);
		}

		private LayoutSystemBase[] method_9(out int int_3)
		{
			int_3 = 0;
			var array = new LayoutSystemBase[LayoutSystems.Count];
			foreach (LayoutSystemBase layoutSystemBase in LayoutSystems)
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
					if (!controlLayoutSystem.Collapsed || (IsInContainer && !DockContainer.CanShowCollapsed))
					{
						array[int_3++] = layoutSystemBase;
					}
				}
			}
			return array;
		}

		public void MoveToLayoutSystem(ControlLayoutSystem layoutSystem)
		{
			MoveToLayoutSystem(layoutSystem, 0);
		}

		public void MoveToLayoutSystem(ControlLayoutSystem layoutSystem, int index)
		{
			DockControl dockControl = null;
		    if (LayoutSystems.Count == 1 && LayoutSystems[0] is ControlLayoutSystem)
		        dockControl = ((ControlLayoutSystem) LayoutSystems[0]).SelectedControl;

            DockControl[] dockControl_ = AllControls;
			for (var i = dockControl_.Length - 1; i >= 0; i--)
			{
				var control = dockControl_[i];
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
		    if (!_splitterRects.Cast<Rectangle>().Any(rectangle => rectangle.Contains(e.X, e.Y))) return;
		    LayoutSystemBase aboveLayout;
		    LayoutSystemBase belowLayout;
		    method_5(new Point(e.X, e.Y), out aboveLayout, out belowLayout);
		    _splittingManager?.Dispose();
		    var dockingHints = DockContainer.Manager?.DockingHints ?? DockingHints.TranslucentFill;
		    _splittingManager = new SplittingManager(DockContainer, this, aboveLayout, belowLayout, new Point(e.X, e.Y), dockingHints);
		    _splittingManager.Cancalled += method_11;
		    _splittingManager.SplittingManagerFinished += OnSplittingManagerFinished;
		}

	    protected internal override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (class7_0 != null)
				{
					class7_0.OnMouseMove(Cursor.Position);
					return;
				}
				if (_splittingManager != null)
				{
					_splittingManager.OnMouseMove(new Point(e.X, e.Y));
					return;
				}
			}
			if (!Contains(e.X, e.Y))
			{
				Cursor.Current = Cursors.Default;
			}
			else if (_splitMode == Orientation.Horizontal)
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
		    if (class7_0 != null)
		        class7_0.Commit();
		    else
		        _splittingManager?.Commit();
		}

		public bool Optimize()
		{
			if (LayoutSystems.Count != 1 || !(LayoutSystems[0] is SplitLayoutSystem))
			{
				IEnumerator enumerator = LayoutSystems.GetEnumerator();
				bool result;
				try
				{
					while (enumerator.MoveNext())
					{
						LayoutSystemBase layoutSystemBase = (LayoutSystemBase)enumerator.Current;
						if (layoutSystemBase is SplitLayoutSystem)
						{
							SplitLayoutSystem splitLayoutSystem = (SplitLayoutSystem)layoutSystemBase;
							if (splitLayoutSystem.SplitMode == SplitMode)
							{
								LayoutSystemBase[] array = new LayoutSystemBase[splitLayoutSystem.LayoutSystems.Count];
								splitLayoutSystem.LayoutSystems.CopyTo(array, 0);
								splitLayoutSystem.LayoutSystems.updating = true;
								splitLayoutSystem.LayoutSystems.Clear();
								int index = LayoutSystems.IndexOf(splitLayoutSystem);
								LayoutSystems.updating = true;
								LayoutSystems.Remove(splitLayoutSystem);
								for (int i = array.Length - 1; i >= 0; i--)
								{
									LayoutSystems.Insert(index, array[i]);
								}
								LayoutSystems.updating = false;
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
			var splitLayoutSystem2 = (SplitLayoutSystem)LayoutSystems[0];
		    if (splitLayoutSystem2.LayoutSystems.Count == 1 && splitLayoutSystem2.LayoutSystems[0] is SplitLayoutSystem && ((SplitLayoutSystem) splitLayoutSystem2.LayoutSystems[0]).SplitMode == SplitMode)
		    {
		        SplitLayoutSystem splitLayoutSystem3 = (SplitLayoutSystem) splitLayoutSystem2.LayoutSystems[0];
		        LayoutSystemBase[] array2 = new LayoutSystemBase[splitLayoutSystem3.LayoutSystems.Count];
		        splitLayoutSystem3.LayoutSystems.CopyTo(array2, 0);
		        splitLayoutSystem3.LayoutSystems.updating = true;
		        splitLayoutSystem3.LayoutSystems.Clear();
		        LayoutSystems.updating = true;
		        LayoutSystems.Clear();
		        LayoutSystems.AddRange(array2);
		        LayoutSystems.updating = false;
		        return true;
		    }
		    return false;
		}

		internal override void OnDockingManagerFinished(Class7.DockTarget dockTarget_0)
		{
			base.OnDockingManagerFinished(dockTarget_0);
			if (dockTarget_0 == null || dockTarget_0.type == Class7.DockTargetType.None || dockTarget_0.type == Class7.DockTargetType.AlreadyActioned)
			{
				return;
			}
			var @class = (FloatingContainer)DockContainer;
			var manager = DockContainer.Manager;
			if (dockTarget_0.type == Class7.DockTargetType.Float)
			{
				@class.method_19(dockTarget_0.Bounds, true, true);
				return;
			}
			var dockControl_ = AllControls;
			var dockControl_2 = @class.SelectedControl;
			@class.LayoutSystem = new SplitLayoutSystem();
			@class.Dispose();
			try
			{
				if (dockTarget_0.type == Class7.DockTargetType.CreateNewContainer)
				{
					DockContainer dockContainer = manager.FindDockContainer(dockTarget_0.DockLocation);
					if (dockTarget_0.DockLocation == ContainerDockLocation.Center && dockContainer != null)
					{
						ControlLayoutSystem layoutSystem = LayoutUtilities.FindControlLayoutSystem(dockContainer);
						MoveToLayoutSystem(layoutSystem);
					}
					else
					{
						method_2(manager, dockTarget_0.DockLocation, dockTarget_0.middle ? ContainerDockEdge.Inside : ContainerDockEdge.Outside);
					}
				}
				else if (dockTarget_0.type != Class7.DockTargetType.JoinExistingSystem)
				{
					if (dockTarget_0.type == Class7.DockTargetType.SplitExistingSystem)
					{
						if (dockTarget_0.dockContainer is DocumentContainer)
						{
							ControlLayoutSystem controlLayoutSystem = dockTarget_0.dockContainer.CreateNewLayoutSystem(WorkingSize);
							controlLayoutSystem.Controls.AddRange(dockControl_);
							dockTarget_0.layoutSystem.SplitForLayoutSystem(controlLayoutSystem, dockTarget_0.DockSide);
						}
						else if (LayoutSystems.Count == 1 && LayoutSystems[0] is ControlLayoutSystem)
						{
							ControlLayoutSystem layoutSystem2 = (ControlLayoutSystem)LayoutSystems[0];
							LayoutSystems.Remove(layoutSystem2);
							dockTarget_0.layoutSystem.SplitForLayoutSystem(layoutSystem2, dockTarget_0.DockSide);
						}
						else
						{
							dockTarget_0.layoutSystem.SplitForLayoutSystem(this, dockTarget_0.DockSide);
						}
					}
				}
				else
				{
					MoveToLayoutSystem(dockTarget_0.layoutSystem, dockTarget_0.index);
				}
			}
			finally
			{
				dockControl_2.Activate();
			}
		}

		internal override void SetDockContainer(DockContainer container)
		{
			base.SetDockContainer(container);
		    foreach (LayoutSystemBase layoutSystemBase in LayoutSystems)
		        layoutSystemBase.SetDockContainer(container);
		}

		internal override bool vmethod_3(ContainerDockLocation location) => LayoutSystems.Cast<LayoutSystemBase>().All(layout => layout.vmethod_3(location));

	    internal override void vmethod_4(RendererBase renderer, Graphics g, Font font)
		{
		    if (DockContainer == null) return;
		    var container = DockContainer.Manager?.DockSystemContainer;
		    foreach (Rectangle bounds in _splitterRects)
		        renderer.DrawSplitter(container, DockContainer, g, bounds, _splitMode);
		    foreach (LayoutSystemBase layoutSystem in LayoutSystems)
			{
				if (!(layoutSystem is ControlLayoutSystem) || !((ControlLayoutSystem)layoutSystem).Collapsed || !DockContainer.CanShowCollapsed)
				{
					var clip = g.Clip;
					g.SetClip(layoutSystem.Bounds);
					layoutSystem.vmethod_4(renderer, g, font);
					g.Clip = clip;
				}
			}
		}

		internal override bool ContainsPersistableDockControls => LayoutSystems.Cast<LayoutSystemBase>().Any(layoutSystemBase => layoutSystemBase.ContainsPersistableDockControls);

        [Naming(NamingType.FromOldVersion)]
	    internal override bool AllowFloat => LayoutSystems.Cast<LayoutSystemBase>().All(layoutSystemBase => layoutSystemBase.AllowFloat);

		internal override bool AllowTab => LayoutSystems.Cast<LayoutSystemBase>().All(layoutSystemBase => layoutSystemBase.AllowTab);

        internal override DockControl[] AllControls
        {
            get
            {
                var arrayList = new ArrayList();
                method_4(this, arrayList);
                return (DockControl[])arrayList.ToArray(typeof(DockControl));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public LayoutSystemBaseCollection LayoutSystems { get; }

	    [Category("Layout"), DefaultValue(typeof(Orientation), "Horizontal"), Description("Indicates whether this layout is split horizontally or vertically.")]
		public Orientation SplitMode
		{
			get
			{
				return _splitMode;
			}
			set
			{
				_splitMode = value;
				method_8();
			}
		}

        [Naming(NamingType.FromOldVersion)]
        internal event EventHandler LayoutSystemsChanged;

		private readonly ArrayList _splitterRects;

		private SplittingManager _splittingManager;

		internal const int int_2 = 4;

	    private Orientation _splitMode;

		public class LayoutSystemBaseCollection : CollectionBase
		{
			internal LayoutSystemBaseCollection(SplitLayoutSystem parent)
			{
				_parent = parent;
			}

			public int Add(LayoutSystemBase layoutSystem)
			{
				var count = Count;
				Insert(count, layoutSystem);
				return count;
			}

			public void AddRange(LayoutSystemBase[] layoutSystems)
			{
				updating = true;
			    foreach (var layoutSystem in layoutSystems)
			        Add(layoutSystem);
			    updating = false;
				method_0();
			}

			public bool Contains(LayoutSystemBase layoutSystem) => List.Contains(layoutSystem);

		    public void CopyTo(LayoutSystemBase[] array, int index) => List.CopyTo(array, index);

		    public int IndexOf(LayoutSystemBase layoutSystem) => List.IndexOf(layoutSystem);

		    public void Insert(int index, LayoutSystemBase layoutSystem)
			{
		        if (layoutSystem._parent != null) throw new ArgumentException("Layout system already has a parent. You must first remove it from its parent.");
		        List.Insert(index, layoutSystem);
			}

			private void method_0()
			{
				_parent.method_7();
			}

			protected override void OnClear()
			{
				base.OnClear();
				foreach (LayoutSystemBase layout in this)
				{
					layout._parent = null;
					layout.SetDockContainer(null);
				}
			}

			protected override void OnClearComplete()
			{
				base.OnClearComplete();
			    if (!updating)
			        method_0();
			}

			protected override void OnInsertComplete(int index, object value)
			{
				base.OnInsertComplete(index, value);
				var layout = (LayoutSystemBase)value;
				layout._parent = _parent;
				layout.SetDockContainer(_parent.DockContainer);
			    if (!updating)
			        method_0();
			}

			protected override void OnRemoveComplete(int index, object value)
			{
				base.OnRemoveComplete(index, value);
				((LayoutSystemBase)value)._parent = null;
				((LayoutSystemBase)value).SetDockContainer(null);
			    if (updating) return;
			    if (Count > 1 || _parent._parent == null)
			    {
			        method_0();
			    }
			    else
			    {
			        var splitLayoutSystem = _parent._parent;
			        if (Count == 1)
			        {
			            LayoutSystemBase layoutSystem = this[0];
			            updating = true;
			            Remove(layoutSystem);
			            updating = false;
			            splitLayoutSystem.LayoutSystems.updating = true;
			            int index2 = splitLayoutSystem.LayoutSystems.IndexOf(_parent);
			            splitLayoutSystem.LayoutSystems.Remove(_parent);
			            splitLayoutSystem.LayoutSystems.Insert(index2, layoutSystem);
			            splitLayoutSystem.LayoutSystems.updating = false;
			            splitLayoutSystem.method_7();
			            return;
			        }
			        if (Count == 0)
			        {
			            splitLayoutSystem.LayoutSystems.Remove(_parent);
			        }
			    }
			}

			public void Remove(LayoutSystemBase layoutSystem) => List.Remove(layoutSystem);

		    public LayoutSystemBase this[int index] => (LayoutSystemBase)List[index];

		    internal bool updating;

			private readonly SplitLayoutSystem _parent;
		}
	}
}
