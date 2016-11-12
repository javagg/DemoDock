using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TD.SandDock.Rendering;

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
			LayoutSystemBase[] array = this.method_9(out num);
			if (num != 0)
			{
				if (num > 1)
				{
					floating = false;
				}
				int num2 = (this._splitMode == Orientation.Horizontal) ? (bounds.Height - (num - 1) * 4) : (bounds.Width - (num - 1) * 4);
				float num3 = 0f;
				for (int i = 0; i < num; i++)
				{
					num3 += ((this._splitMode == Orientation.Horizontal) ? array[i].WorkingSize.Height : array[i].WorkingSize.Width);
				}
				this._splitterRects.Clear();
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
							float num5 = (this._splitMode != Orientation.Horizontal) ? array2[k].Width : array2[k].Height;
							num5 += num4 * (num5 / num3);
							if (this._splitMode != Orientation.Horizontal)
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
							num3 += ((this._splitMode == Orientation.Horizontal) ? array2[l].Height : array2[l].Width);
						}
						num4 = (float)num2 - num3;
						if (this._splitMode != Orientation.Horizontal)
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
					int num6 = (this._splitMode != Orientation.Horizontal) ? bounds.X : bounds.Y;
					for (int m = 0; m < num; m++)
					{
						int num7 = (this._splitMode != Orientation.Horizontal) ? Convert.ToInt32(array2[m].Width) : Convert.ToInt32(array2[m].Height);
						num7 = Math.Max(num7, 4);
						if (this._splitMode == Orientation.Horizontal)
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
						if (this._splitMode != Orientation.Horizontal)
						{
							bounds.Offset(bounds.Width, 0);
							bounds.Width = 4;
						}
						else
						{
							bounds.Offset(0, bounds.Height);
							bounds.Height = 4;
						}
						this._splitterRects.Add(bounds);
					}
				}
				return;
			}
		}

		private void method_10()
		{
			_splittingManager.Event_0 -= method_11;
			_splittingManager.SplittingManagerFinished -= OnSplittingManagerFinished;
			_splittingManager = null;
		}

		private void method_11(object sender, EventArgs e)
		{
			this.method_10();
		}

		private void OnSplittingManagerFinished(LayoutSystemBase aboveLayout, LayoutSystemBase belowLayout, float aboveSize, float belowSize)
		{
			this.method_10();
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
		    this.method_8();
		}

		internal bool method_13()
		{
			IEnumerator enumerator = this.LayoutSystems.GetEnumerator();
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
						if (!controlLayoutSystem.Collapsed || (IsInContainer && !DockContainer.Boolean_6))
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
				    continue;
				}
				this.method_4((SplitLayoutSystem)layoutSystemBase, arrayList_1);
			}
		}

		internal void method_5(Point point_0, out LayoutSystemBase layoutSystemBase_0, out LayoutSystemBase layoutSystemBase_1)
		{
			var index = LayoutSystems.Cast<LayoutSystemBase>()
			    .Where(
			        layoutSystemBase =>
			            (!(layoutSystemBase is ControlLayoutSystem) || !((ControlLayoutSystem) layoutSystemBase).Collapsed) &&
			            ((SplitMode == Orientation.Horizontal && point_0.Y >= layoutSystemBase.Bounds.Bottom &&
			              point_0.Y <= layoutSystemBase.Bounds.Bottom + 4) ||
			             (SplitMode == Orientation.Vertical && point_0.X >= layoutSystemBase.Bounds.Right &&
			              point_0.X <= layoutSystemBase.Bounds.Right + 4)))
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

		internal bool Contains(int x, int y) => this._splitterRects.Cast<Rectangle>().Any(r => r.Contains(x, y));

	    internal void method_7()
		{
	        DockContainer?.vmethod_2();
            Event_0?.Invoke(this, EventArgs.Empty);
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
					if (!controlLayoutSystem.Collapsed || (IsInContainer && !DockContainer.Boolean_6))
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

            DockControl[] dockControl_ = this.DockControls;
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
		    if (this._splitterRects.Cast<Rectangle>().Any(rectangle => rectangle.Contains(e.X, e.Y)))
		    {
		        LayoutSystemBase aboveLayout;
		        LayoutSystemBase belowLayout;
		        this.method_5(new Point(e.X, e.Y), out aboveLayout, out belowLayout);
		        this._splittingManager?.Dispose();
		        DockingHints dockingHints = DockContainer.Manager?.DockingHints ?? DockingHints.TranslucentFill;
		        _splittingManager = new SplittingManager(DockContainer, this, aboveLayout, belowLayout, new Point(e.X, e.Y), dockingHints);
		        _splittingManager.Event_0 += this.method_11;
		        _splittingManager.SplittingManagerFinished += OnSplittingManagerFinished;
		    }
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
			var splitLayoutSystem2 = (SplitLayoutSystem)LayoutSystems[0];
		    if (splitLayoutSystem2.LayoutSystems.Count == 1 && splitLayoutSystem2.LayoutSystems[0] is SplitLayoutSystem && ((SplitLayoutSystem) splitLayoutSystem2.LayoutSystems[0]).SplitMode == SplitMode)
		    {
		        SplitLayoutSystem splitLayoutSystem3 = (SplitLayoutSystem) splitLayoutSystem2.LayoutSystems[0];
		        LayoutSystemBase[] array2 = new LayoutSystemBase[splitLayoutSystem3.LayoutSystems.Count];
		        splitLayoutSystem3.LayoutSystems.CopyTo(array2, 0);
		        splitLayoutSystem3.LayoutSystems.bool_0 = true;
		        splitLayoutSystem3.LayoutSystems.Clear();
		        LayoutSystems.bool_0 = true;
		        LayoutSystems.Clear();
		        LayoutSystems.AddRange(array2);
		        LayoutSystems.bool_0 = false;
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
			FloatingContainer @class = (FloatingContainer)base.DockContainer;
			SandDockManager manager = base.DockContainer.Manager;
			if (dockTarget_0.type == Class7.DockTargetType.Float)
			{
				@class.method_19(dockTarget_0.Bounds, true, true);
				return;
			}
			DockControl[] dockControl_ = this.DockControls;
			DockControl dockControl_2 = @class.SelectedControl;
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
						this.MoveToLayoutSystem(layoutSystem);
					}
					else
					{
						base.method_2(manager, dockTarget_0.DockLocation, dockTarget_0.middle ? ContainerDockEdge.Inside : ContainerDockEdge.Outside);
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
							dockTarget_0.layoutSystem.SplitForLayoutSystem(controlLayoutSystem, dockTarget_0.DockSide);
						}
						else if (this.LayoutSystems.Count == 1 && this.LayoutSystems[0] is ControlLayoutSystem)
						{
							ControlLayoutSystem layoutSystem2 = (ControlLayoutSystem)this.LayoutSystems[0];
							this.LayoutSystems.Remove(layoutSystem2);
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
					this.MoveToLayoutSystem(dockTarget_0.layoutSystem, dockTarget_0.index);
				}
			}
			finally
			{
				dockControl_2.Activate();
			}
		}

		internal override void vmethod_2(DockContainer container)
		{
			base.vmethod_2(container);
		    foreach (LayoutSystemBase layoutSystemBase in LayoutSystems)
		        layoutSystemBase.vmethod_2(container);
		}

		internal override bool vmethod_3(ContainerDockLocation containerDockLocation_0) => LayoutSystems.Cast<LayoutSystemBase>().All(layout => layout.vmethod_3(containerDockLocation_0));

	    internal override void vmethod_4(RendererBase renderer, Graphics g, Font font)
		{
		    if (DockContainer == null)
		        return;
		    var container = DockContainer.Manager?.DockSystemContainer;
		    foreach (Rectangle bounds in _splitterRects)
		        renderer.DrawSplitter(container, DockContainer, g, bounds, _splitMode);
		    foreach (LayoutSystemBase layoutSystem in LayoutSystems)
			{
				if (!(layoutSystem is ControlLayoutSystem) || !((ControlLayoutSystem)layoutSystem).Collapsed || !base.DockContainer.Boolean_6)
				{
					var clip = g.Clip;
					g.SetClip(layoutSystem.Bounds);
					layoutSystem.vmethod_4(renderer, g, font);
					g.Clip = clip;
				}
			}
		}

		internal override bool PersistState => LayoutSystems.Cast<LayoutSystemBase>().Any(layoutSystemBase => layoutSystemBase.PersistState);

	    internal override bool AllowFloat => LayoutSystems.Cast<LayoutSystemBase>().All(layoutSystemBase => layoutSystemBase.AllowFloat);

		internal override bool AllowTab => LayoutSystems.Cast<LayoutSystemBase>().All(layoutSystemBase => layoutSystemBase.AllowTab);

        internal override DockControl[] DockControls
        {
            get
            {
                var arrayList = new ArrayList();
                this.method_4(this, arrayList);
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
				this.method_8();
			}
		}

	    internal event EventHandler Event_0;

		private ArrayList _splitterRects;

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
				this.bool_0 = true;
			    foreach (var layoutSystem in layoutSystems)
			        Add(layoutSystem);
			    this.bool_0 = false;
				this.method_0();
			}

			public bool Contains(LayoutSystemBase layoutSystem) => List.Contains(layoutSystem);

		    public void CopyTo(LayoutSystemBase[] array, int index) => List.CopyTo(array, index);

		    public int IndexOf(LayoutSystemBase layoutSystem) => List.IndexOf(layoutSystem);

		    public void Insert(int index, LayoutSystemBase layoutSystem)
			{
		        if (layoutSystem.splitLayoutSystem_0 != null)
		            throw new ArgumentException("Layout system already has a parent. You must first remove it from its parent.");
		        List.Insert(index, layoutSystem);
			}

			private void method_0()
			{
				_parent.method_7();
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
				layoutSystemBase.splitLayoutSystem_0 = this._parent;
				layoutSystemBase.vmethod_2(this._parent.DockContainer);
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
					if (base.Count > 1 || this._parent.splitLayoutSystem_0 == null)
					{
						this.method_0();
					}
					else
					{
						SplitLayoutSystem splitLayoutSystem = this._parent.splitLayoutSystem_0;
						if (base.Count == 1)
						{
							LayoutSystemBase layoutSystem = this[0];
							this.bool_0 = true;
							this.Remove(layoutSystem);
							this.bool_0 = false;
							splitLayoutSystem.LayoutSystems.bool_0 = true;
							int index2 = splitLayoutSystem.LayoutSystems.IndexOf(this._parent);
							splitLayoutSystem.LayoutSystems.Remove(this._parent);
							splitLayoutSystem.LayoutSystems.Insert(index2, layoutSystem);
							splitLayoutSystem.LayoutSystems.bool_0 = false;
							splitLayoutSystem.method_7();
							return;
						}
						if (base.Count == 0)
						{
							splitLayoutSystem.LayoutSystems.Remove(this._parent);
							return;
						}
					}
				}
			}

			public void Remove(LayoutSystemBase layoutSystem) => List.Remove(layoutSystem);

		    public LayoutSystemBase this[int index] => (LayoutSystemBase)List[index];

		    internal bool bool_0;

			private readonly SplitLayoutSystem _parent;
		}
	}
}
