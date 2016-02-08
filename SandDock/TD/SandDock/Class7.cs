using System;
using System.Collections;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Class7 : Class6
	{
		public Class7(SandDockManager manager, DockContainer container, LayoutSystemBase sourceControlSystem, DockControl sourceControl, int dockedSize, Point startPoint, DockingHints dockingHints) : base(container, dockingHints, true, container.RendererBase_0.TabStripMetrics.Height)
		{
			Manager = manager;
			this.DockContainer_0 = container;
			this.LayoutSystemBase_0 = sourceControlSystem;
			this.DockControl_0 = sourceControl;
			this.Int32_0 = dockedSize;
			if (container is DocumentContainer)
			{
				this.cursor_0 = new Cursor(GetType().Assembly.GetManifestResourceStream("TD.SandDock.Resources.splitting.cur"));
				this.cursor_1 = new Cursor(GetType().Assembly.GetManifestResourceStream("TD.SandDock.Resources.splittingno.cur"));
			}
			if (sourceControlSystem is SplitLayoutSystem)
			{
				this.size_0 = ((FloatingContainer)container).Size_0;
			}
			else if (sourceControl == null)
			{
				if (!(sourceControlSystem is ControlLayoutSystem) || ((ControlLayoutSystem)sourceControlSystem).SelectedControl == null)
				{
					this.size_0 = sourceControlSystem.Bounds.Size;
				}
				else
				{
					this.size_0 = ((ControlLayoutSystem)sourceControlSystem).SelectedControl.FloatingSize;
				}
			}
			else
			{
				this.size_0 = sourceControl.FloatingSize;
			}
			Rectangle bounds = sourceControlSystem.Bounds;
			if (bounds.Width <= 0)
			{
				startPoint.X = this.size_0.Width / 2;
			}
			else
			{
				startPoint.X -= bounds.Left;
				startPoint.X = Convert.ToInt32((float)startPoint.X / (float)bounds.Width * (float)this.size_0.Width);
			}
			this.point_0 = sourceControl == null ? new Point(startPoint.X, startPoint.Y - bounds.Top) : new Point(startPoint.X, this.size_0.Height - (bounds.Bottom - startPoint.Y));
			this.point_0.Y = Math.Max(this.point_0.Y, 0);
			this.point_0.Y = Math.Min(this.point_0.Y, this.size_0.Height);
			this.ControlLayoutSystem_0 = this.method_10();
			this.DockContainer_0.OnDockingStarted(EventArgs.Empty);
		}

		public override void Commit()
		{
			base.Commit();
			LayoutUtilities.smethod_0();
			try
			{
                DockingManagerFinished?.Invoke(this.DockTarget_0);
			}
			finally
			{
				LayoutUtilities.smethod_1();
			}
		}

		public override void Dispose()
		{
			this.DockContainer_0.OnDockingFinished(EventArgs.Empty);
		    this.cursor_0?.Dispose();
		    this.cursor_1?.Dispose();
		    base.Dispose();
		}

		protected virtual DockTarget FindDockTarget(Point position)
		{
			if (this.Manager != null && this.Boolean_1)
			{
				foreach (DockContainer dockContainer in this.Manager.arrayList_0)
				{
					if (dockContainer.IsFloating && ((FloatingContainer)dockContainer).Form_0.Visible && ((FloatingContainer)dockContainer).HasSingleControlLayoutSystem)
					{
						if (dockContainer.LayoutSystem != this.LayoutSystemBase_0)
						{
							if (((FloatingContainer)dockContainer).Rectangle_1.Contains(position) && !new Rectangle(dockContainer.PointToScreen(dockContainer.LayoutSystem.LayoutSystems[0].Bounds.Location), dockContainer.LayoutSystem.LayoutSystems[0].Bounds.Size).Contains(position))
							{
								Class7.DockTarget result = new Class7.DockTarget(Class7.DockTargetType.JoinExistingSystem)
								{
									dockContainer = dockContainer,
									layoutSystem = (ControlLayoutSystem)dockContainer.LayoutSystem.LayoutSystems[0],
									bounds = ((FloatingContainer)dockContainer).Rectangle_1
								};
								return result;
							}
						}
					}
				}
			}
			ControlLayoutSystem[] array = this.ControlLayoutSystem_0;
			for (int i = 0; i < array.Length; i++)
			{
				ControlLayoutSystem controlLayoutSystem = array[i];
				if (new Rectangle(controlLayoutSystem.DockContainer.PointToScreen(controlLayoutSystem.Bounds.Location), controlLayoutSystem.Bounds.Size).Contains(position))
				{
					Class7.DockTarget dockTarget = this.method_13(controlLayoutSystem.DockContainer, controlLayoutSystem, position, true);
					if (dockTarget != null)
					{
						Class7.DockTarget result = dockTarget;
						return result;
					}
				}
			}
			if (this.Manager != null)
			{
				for (int j = 1; j <= 4; j++)
				{
					ContainerDockLocation containerDockLocation = (ContainerDockLocation)j;
					if (this.method_5(containerDockLocation))
					{
						if (Class7.smethod_2(this.method_7(containerDockLocation, true), this.Manager.DockSystemContainer).Contains(position))
						{
							return new Class7.DockTarget(Class7.DockTargetType.CreateNewContainer)
							{
								dockLocation = containerDockLocation,
								bounds = Class7.smethod_2(this.method_8(containerDockLocation, this.Int32_0, true), this.Manager.DockSystemContainer),
								middle = true
							};
						}
						if (Class7.smethod_2(this.method_7(containerDockLocation, false), this.Manager.DockSystemContainer).Contains(position))
						{
							return new Class7.DockTarget(Class7.DockTargetType.CreateNewContainer)
							{
								dockLocation = containerDockLocation,
								bounds = Class7.smethod_2(this.method_8(containerDockLocation, this.Int32_0, false), this.Manager.DockSystemContainer)
							};
						}
					}
				}
			}
			return null;
		}

		private ControlLayoutSystem[] method_10()
		{
			ArrayList arrayList = new ArrayList();
			DockContainer[] array;
		    array = Manager != null ? this.Manager.GetDockContainers() : new[] {this.DockContainer_0};
		    DockContainer[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				DockContainer dockContainer = array2[i];
				bool isFloating = dockContainer.IsFloating;
				bool flag = dockContainer.Dock == DockStyle.Fill && !dockContainer.IsFloating;
				bool flag2 = this.DockContainer_0.Dock == DockStyle.Fill && !this.DockContainer_0.IsFloating;
				if ((!isFloating || this.LayoutSystemBase_0.DockContainer != dockContainer || !(this.LayoutSystemBase_0 is SplitLayoutSystem)) && (!isFloating || this.Boolean_1 || this.LayoutSystemBase_0.DockContainer == dockContainer) && (isFloating || this.method_5(LayoutUtilities.smethod_7(dockContainer.Dock))) && (!flag || flag2))
				{
					if (!flag2 || this.DockContainer_0 == dockContainer)
					{
						this.method_11(dockContainer, arrayList);
					}
				}
			}
			ControlLayoutSystem[] array3 = new ControlLayoutSystem[arrayList.Count];
			arrayList.CopyTo(array3, 0);
			return array3;
		}

		private void method_11(DockContainer container, ArrayList arrayList_0)
		{
			if ((container.Width > 0 || container.Height > 0) && container.Enabled && container.Visible)
			{
				this.method_12(container, container.LayoutSystem, arrayList_0);
			}
		}

		private void method_12(DockContainer dockContainer_1, SplitLayoutSystem splitLayoutSystem_0, ArrayList arrayList_0)
		{
			foreach (LayoutSystemBase layoutSystemBase in splitLayoutSystem_0.LayoutSystems)
			{
				if (!(layoutSystemBase is SplitLayoutSystem))
				{
					if (layoutSystemBase is ControlLayoutSystem)
					{
						if (this.DockControl_0 == null || layoutSystemBase != this.LayoutSystemBase_0)
						{
							goto IL_59;
						}
						if (this.DockControl_0.LayoutSystem.Controls.Count != 1)
						{
							goto IL_59;
						}
						bool arg_67_0 = false;
						IL_67:
						if (arg_67_0)
						{
							arrayList_0.Add(layoutSystemBase);
							continue;
						}
						continue;
						IL_59:
						arg_67_0 = !((ControlLayoutSystem)layoutSystemBase).Collapsed;
						goto IL_67;
					}
				}
				else
				{
					this.method_12(dockContainer_1, (SplitLayoutSystem)layoutSystemBase, arrayList_0);
				}
			}
		}

		protected Class7.DockTarget method_13(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Point point_1, bool bool_2)
		{
			Class7.DockTarget dockTarget = new Class7.DockTarget(Class7.DockTargetType.Undefined);
			Point point = dockContainer_1.PointToClient(point_1);
			if (this.DockControl_0 != null || controlLayoutSystem_1 != this.LayoutSystemBase_0)
			{
				if (controlLayoutSystem_1.Rectangle_0.Contains(point) || controlLayoutSystem_1.rectangle_2.Contains(point))
				{
					dockTarget = new Class7.DockTarget(Class7.DockTargetType.JoinExistingSystem);
					dockTarget.dockContainer = dockContainer_1;
					dockTarget.layoutSystem = controlLayoutSystem_1;
					dockTarget.dockSide = DockSide.None;
					dockTarget.bounds = new Rectangle(dockContainer_1.PointToScreen(controlLayoutSystem_1.Bounds.Location), controlLayoutSystem_1.Bounds.Size);
					if (!controlLayoutSystem_1.rectangle_2.Contains(point))
					{
						dockTarget.index = controlLayoutSystem_1.Controls.Count;
					}
					else
					{
						dockTarget.index = controlLayoutSystem_1.method_15(point);
					}
				}
				if (dockTarget.type == Class7.DockTargetType.Undefined && bool_2)
				{
					dockTarget = this.method_14(dockContainer_1, controlLayoutSystem_1, point_1);
				}
				return dockTarget;
			}
			if (controlLayoutSystem_1.Rectangle_0.Contains(point))
			{
				return new Class7.DockTarget(Class7.DockTargetType.None);
			}
			return new Class7.DockTarget(Class7.DockTargetType.Undefined);
		}

		private Class7.DockTarget method_14(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Point point_1)
		{
			Class7.DockTarget dockTarget = null;
			Point point = dockContainer_1.PointToClient(point_1);
			Rectangle rectangle_ = controlLayoutSystem_1.rectangle_3;
			if (new Rectangle(rectangle_.Left, rectangle_.Top, rectangle_.Width, 30).Contains(point))
			{
				dockTarget = this.method_21(dockContainer_1, controlLayoutSystem_1);
				if (point.X >= rectangle_.Left + 30)
				{
					if (point.X > rectangle_.Right - 30)
					{
						this.method_17(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
					}
					else
					{
						this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Top);
					}
				}
				else
				{
					this.method_18(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
				}
			}
			else if (new Rectangle(rectangle_.Left, rectangle_.Top, 30, rectangle_.Height).Contains(point))
			{
				dockTarget = this.method_21(dockContainer_1, controlLayoutSystem_1);
				if (point.Y >= rectangle_.Top + 30)
				{
					if (point.Y <= rectangle_.Bottom - 30)
					{
						this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Left);
					}
					else
					{
						this.method_16(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
					}
				}
				else
				{
					this.method_18(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
				}
			}
			else if (!new Rectangle(rectangle_.Right - 30, rectangle_.Top, 30, rectangle_.Height).Contains(point))
			{
				if (new Rectangle(rectangle_.Left, rectangle_.Bottom - 30, rectangle_.Width, 30).Contains(point))
				{
					dockTarget = this.method_21(dockContainer_1, controlLayoutSystem_1);
					if (point.X >= rectangle_.Left + 30)
					{
						if (point.X <= rectangle_.Right - 30)
						{
							this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Bottom);
						}
						else
						{
							this.method_15(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
						}
					}
					else
					{
						this.method_16(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
					}
				}
			}
			else
			{
				dockTarget = this.method_21(dockContainer_1, controlLayoutSystem_1);
				if (point.Y >= rectangle_.Top + 30)
				{
					if (point.Y <= rectangle_.Bottom - 30)
					{
						this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget, DockSide.Right);
					}
					else
					{
						this.method_15(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
					}
				}
				else
				{
					this.method_17(dockContainer_1, controlLayoutSystem_1, dockTarget, rectangle_, point);
				}
			}
			return dockTarget;
		}

		private void method_15(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
		{
			rectangle_1.X = rectangle_1.Right - 30;
			rectangle_1.Y = rectangle_1.Bottom - 30;
			point_1.X -= rectangle_1.Left;
			point_1.Y -= rectangle_1.Top;
			rectangle_1 = new Rectangle(0, 0, 30, 30);
			if (point_1.Y <= rectangle_1.Top + (int)((float)rectangle_1.Height * ((float)point_1.X / (float)rectangle_1.Width)))
			{
				this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Right);
				return;
			}
			this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Bottom);
		}

		private void method_16(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
		{
			rectangle_1.Y = rectangle_1.Bottom - 30;
			point_1.X -= rectangle_1.Left;
			point_1.Y -= rectangle_1.Top;
			rectangle_1 = new Rectangle(0, 0, 30, 30);
			if (point_1.Y <= rectangle_1.Bottom - (int)((float)rectangle_1.Height * ((float)point_1.X / (float)rectangle_1.Width)))
			{
				this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Left);
				return;
			}
			this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Bottom);
		}

		private void method_17(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
		{
			rectangle_1.X = rectangle_1.Right - 30;
			point_1.X -= rectangle_1.Left;
			point_1.Y -= rectangle_1.Top;
			rectangle_1 = new Rectangle(0, 0, 30, 30);
			if (point_1.Y > rectangle_1.Top + (int)((float)rectangle_1.Height * ((float)(rectangle_1.Right - point_1.X) / (float)rectangle_1.Width)))
			{
				this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Right);
				return;
			}
			this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Top);
		}

		private void method_18(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, Rectangle rectangle_1, Point point_1)
		{
			point_1.X -= rectangle_1.Left;
			point_1.Y -= rectangle_1.Top;
			rectangle_1 = new Rectangle(0, 0, 30, 30);
			if (point_1.Y > rectangle_1.Top + (int)((float)rectangle_1.Height * ((float)point_1.X / (float)rectangle_1.Width)))
			{
				this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Left);
				return;
			}
			this.method_19(dockContainer_1, controlLayoutSystem_1, dockTarget_1, DockSide.Top);
		}

		private void method_19(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, Class7.DockTarget dockTarget_1, DockSide dockSide_0)
		{
			dockTarget_1.bounds = this.method_20(dockContainer_1, controlLayoutSystem_1, dockSide_0);
			dockTarget_1.dockSide = dockSide_0;
		}

		internal Rectangle method_20(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1, DockSide dockSide_0)
		{
			Rectangle result = new Rectangle(dockContainer_1.PointToScreen(controlLayoutSystem_1.Bounds.Location), controlLayoutSystem_1.Bounds.Size);
			switch (dockSide_0)
			{
			case DockSide.Top:
				result.Height /= 2;
				break;
			case DockSide.Bottom:
				result.Offset(0, result.Height / 2);
				result.Height /= 2;
				break;
			case DockSide.Left:
				result.Width /= 2;
				break;
			case DockSide.Right:
				result.Offset(result.Width / 2, 0);
				result.Width /= 2;
				break;
			}
			return result;
		}

		private DockTarget method_21(DockContainer dockContainer_1, ControlLayoutSystem controlLayoutSystem_1)
		{
			return new DockTarget(DockTargetType.SplitExistingSystem)
			{
				dockContainer = dockContainer_1,
				layoutSystem = controlLayoutSystem_1
			};
		}

		public bool method_5(ContainerDockLocation containerDockLocation_0)
		{
		    return this.DockControl_0?.method_13(containerDockLocation_0) ?? this.LayoutSystemBase_0.vmethod_3(containerDockLocation_0);
		}

	    private Rectangle method_6(Rectangle rectangle_1)
		{
			if (rectangle_1.X >= Screen.PrimaryScreen.Bounds.X && rectangle_1.Right <= Screen.PrimaryScreen.Bounds.Right && rectangle_1.Y > Screen.PrimaryScreen.WorkingArea.Bottom)
			{
				rectangle_1.Y = Screen.PrimaryScreen.WorkingArea.Bottom - rectangle_1.Height;
			}
			Screen screen = Screen.FromRectangle(rectangle_1);
			if (screen != null && rectangle_1.Y < screen.WorkingArea.Y)
			{
				rectangle_1.Y = screen.WorkingArea.Y;
			}
			return rectangle_1;
		}

		protected Rectangle method_7(ContainerDockLocation containerDockLocation_0, bool bool_2)
		{
			if (bool_2)
			{
				return this.method_8(containerDockLocation_0, 30, true);
			}
			Control dockSystemContainer = this.Manager.DockSystemContainer;
			int num = 0;
			int width = dockSystemContainer.ClientRectangle.Width;
			int num2 = 0;
			int height = dockSystemContainer.ClientRectangle.Height;
			switch (containerDockLocation_0)
			{
			case ContainerDockLocation.Left:
				return new Rectangle(num - 30, num2, 30, height - num2);
			case ContainerDockLocation.Right:
				return new Rectangle(width, num2, 30, height - num2);
			case ContainerDockLocation.Top:
				return new Rectangle(num, num2 - 30, width - num, 30);
			case ContainerDockLocation.Bottom:
				return new Rectangle(num, height, width - num, 30);
			default:
				return Rectangle.Empty;
			}
		}

		protected Rectangle method_8(ContainerDockLocation containerDockLocation_0, int int_8, bool bool_2)
		{
			Rectangle rectangle = Class7.smethod_1(this.Manager.DockSystemContainer);
			Rectangle result = rectangle;
			if (!bool_2)
			{
				result = this.Manager.DockSystemContainer.ClientRectangle;
			}
			int val = int_8 + 4;
			switch (containerDockLocation_0)
			{
			case ContainerDockLocation.Left:
				return new Rectangle(result.Left, result.Top, Math.Min(val, Convert.ToInt32((double)rectangle.Width * 0.9)), result.Height);
			case ContainerDockLocation.Right:
				return new Rectangle(result.Right - Math.Min(val, Convert.ToInt32((double)rectangle.Width * 0.9)), result.Top, Math.Min(val, Convert.ToInt32((double)rectangle.Width * 0.9)), result.Height);
			case ContainerDockLocation.Top:
				return new Rectangle(result.Left, result.Top, result.Width, Math.Min(val, Convert.ToInt32((double)rectangle.Height * 0.9)));
			case ContainerDockLocation.Bottom:
				return new Rectangle(result.Left, result.Bottom - Math.Min(val, Convert.ToInt32((double)rectangle.Height * 0.9)), result.Width, Math.Min(val, Convert.ToInt32((double)rectangle.Height * 0.9)));
			}
			return result;
		}

		protected bool method_9()
		{
			return Manager.FindDockedContainer(DockStyle.Fill) is DocumentContainer;
		}

		public override void OnMouseMove(Point position)
		{
			DockTarget dockTarget = null;
			if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
			{
				dockTarget = this.FindDockTarget(position);
			}
			if (dockTarget == null || (dockTarget.type == Class7.DockTargetType.Undefined && this.Manager != null && this.Boolean_1))
			{
				if (this.Manager != null && this.Boolean_1)
				{
					dockTarget = new Class7.DockTarget(Class7.DockTargetType.Float);
				}
				else
				{
					dockTarget = new Class7.DockTarget(Class7.DockTargetType.None);
				}
			}
			if (dockTarget.type == Class7.DockTargetType.Undefined)
			{
				dockTarget.type = Class7.DockTargetType.None;
			}
			if (dockTarget.type == Class7.DockTargetType.Float)
			{
				dockTarget.bounds = new Rectangle(this.Point_0, this.size_0);
				dockTarget.bounds = this.method_6(dockTarget.bounds);
			}
			if (dockTarget.layoutSystem == this.LayoutSystemBase_0 && this.DockControl_0 != null)
			{
				if (dockTarget.dockSide == DockSide.None)
				{
					base.method_2();
					ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)this.LayoutSystemBase_0;
					if (dockTarget.index != controlLayoutSystem.Controls.IndexOf(this.DockControl_0))
					{
						if (dockTarget.index != controlLayoutSystem.Controls.IndexOf(this.DockControl_0) + 1)
						{
							controlLayoutSystem.Controls.SetChildIndex(this.DockControl_0, dockTarget.index);
						}
					}
					dockTarget.type = Class7.DockTargetType.AlreadyActioned;
					goto IL_147;
				}
			}
			if (dockTarget.type != Class7.DockTargetType.None)
			{
				base.method_1(dockTarget.bounds, dockTarget.type == Class7.DockTargetType.JoinExistingSystem);
			}
			else
			{
				base.method_2();
			}
			IL_147:
			if (this.DockContainer_0 is DocumentContainer)
			{
				if (dockTarget.type == Class7.DockTargetType.AlreadyActioned)
				{
					Cursor.Current = Cursors.Default;
				}
				else if (dockTarget.type != Class7.DockTargetType.None)
				{
					Cursor.Current = this.cursor_0;
				}
				else
				{
					Cursor.Current = this.cursor_1;
				}
			}
			this.DockTarget_0 = dockTarget;
		}

		public static Rectangle smethod_1(Control control_1)
		{
			int num = 0;
			int num2 = control_1.ClientRectangle.Width;
			int num3 = 0;
			int num4 = control_1.ClientRectangle.Height;
			foreach (Control control in control_1.Controls)
			{
				if (control.Visible)
				{
					switch (control.Dock)
					{
					case DockStyle.Top:
						if (control.Bounds.Bottom > num3)
						{
							num3 = control.Bounds.Bottom;
						}
						break;
					case DockStyle.Bottom:
						if (control.Bounds.Top < num4)
						{
							num4 = control.Bounds.Top;
						}
						break;
					case DockStyle.Left:
						if (control.Bounds.Right > num)
						{
							num = control.Bounds.Right;
						}
						break;
					case DockStyle.Right:
						if (control.Bounds.Left < num2)
						{
							num2 = control.Bounds.Left;
						}
						break;
					}
				}
			}
			return new Rectangle(num, num3, num2 - num, num4 - num3);
		}

		public static Rectangle smethod_2(Rectangle rectangle_1, Control control_1)
		{
			return new Rectangle(control_1.PointToScreen(rectangle_1.Location), rectangle_1.Size);
		}

		public bool Boolean_0
		{
			get
			{
				return this.DockContainer_0.Boolean_0;
			}
		}

		public bool Boolean_1
		{
			get
			{
				if (this.Boolean_0)
				{
					return false;
				}
				if (this.DockControl_0 == null)
				{
					return this.LayoutSystemBase_0.Boolean_3;
				}
				return this.DockControl_0.DockingRules.AllowFloat;
			}
		}

		protected ControlLayoutSystem[] ControlLayoutSystem_0 { get; }

	    public DockContainer DockContainer_0 { get; }

	    public DockControl DockControl_0 { get; }

	    public Class7.DockTarget DockTarget_0 { get; private set; }

	    public int Int32_0 { get; }

	    public LayoutSystemBase LayoutSystemBase_0 { get; }

	    private Point Point_0 => new Point(Cursor.Position.X - this.point_0.X, Cursor.Position.Y - this.point_0.Y);

	    public SandDockManager Manager { get; }

	    public event DockingManagerFinishedEventHandler DockingManagerFinished;

	    private Cursor cursor_0;

		private Cursor cursor_1;

	   // private Class7.DockingManagerFinishedEventHandler dockingManagerFinishedEventHandler_0;

	    private const int int_6 = 30;

	    private Point point_0 = Point.Empty;

	    private Size size_0 = Size.Empty;

		public delegate void DockingManagerFinishedEventHandler(DockTarget target);

		public class DockTarget
		{
			public DockTarget(DockTargetType type)
			{
				this.type = type;
			}

			public Rectangle bounds = Rectangle.Empty;

			public DockContainer dockContainer;

			public ContainerDockLocation dockLocation = ContainerDockLocation.Center;

			public DockSide dockSide = DockSide.None;

			public int index;

			public ControlLayoutSystem layoutSystem;

			public bool middle;

			public DockTargetType type;
		}

		public enum DockTargetType
		{
			Undefined,
			None,
			Float,
			SplitExistingSystem,
			JoinExistingSystem,
			CreateNewContainer,
			AlreadyActioned
		}
	}
}
