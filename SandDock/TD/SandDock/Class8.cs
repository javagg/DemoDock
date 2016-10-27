using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
	internal class Class8 : Class7
	{
		public Class8(SandDockManager manager, DockContainer container, LayoutSystemBase sourceControlSystem, DockControl sourceControl, int dockedSize, Point startPoint, DockingHints dockingHints) : base(manager, container, sourceControlSystem, sourceControl, dockedSize, startPoint, dockingHints)
		{
			this.arrayList_0 = new ArrayList();
			if (base.Manager != null && base.Manager.DockSystemContainer != null)
			{
				this.method_22();
			}
		}

		public override void Dispose()
		{
			if (this.form4_0 != null)
			{
				this.form4_0.method_11();
				this.form4_0 = null;
			}
			foreach (Class8.Form4 form in this.arrayList_0)
			{
				form.method_11();
			}
			this.arrayList_0.Clear();
			base.Dispose();
		}

		protected override Class7.DockTarget FindDockTarget(Point position)
		{
			Class7.DockTarget dockTarget = null;
			bool flag = this.rectangle_1.Contains(position);
			bool flag2 = this.rectangle_2.Contains(position);
			if (flag == this.bool_2)
			{
				if (flag2 == this.bool_3)
				{
					goto IL_C1;
				}
			}
			object[] array = this.arrayList_0.ToArray();
			int i = 0;
			while (i < array.Length)
			{
				Class8.Form4 form = (Class8.Form4)array[i];
				if (form.DockStyle_0 != DockStyle.Fill)
				{
					goto IL_7B;
				}
				if (flag2 == this.bool_3)
				{
					goto IL_7B;
				}
				if (!flag2)
				{
					form.method_12();
				}
				else
				{
					form.method_13();
				}
				IL_A5:
				i++;
				continue;
				IL_7B:
				if (form.DockStyle_0 == DockStyle.Fill)
				{
					goto IL_A5;
				}
				if (flag == this.bool_2)
				{
					goto IL_A5;
				}
				if (flag)
				{
					form.method_13();
					goto IL_A5;
				}
				form.method_12();
				goto IL_A5;
			}
			this.bool_2 = flag;
			this.bool_3 = flag2;
			IL_C1:
			ControlLayoutSystem controlLayoutSystem = this.method_23(position, out dockTarget);
			if (controlLayoutSystem == base.LayoutSystemBase_0 && base.DockControl_0 == null)
			{
				controlLayoutSystem = null;
			}
			if (controlLayoutSystem != this.controlLayoutSystem_1)
			{
				if (this.form4_0 != null)
				{
					this.form4_0.method_11();
					this.form4_0 = null;
				}
				this.controlLayoutSystem_1 = controlLayoutSystem;
				if (this.controlLayoutSystem_1 != null)
				{
					this.form4_0 = new Class8.Form4(this, this.controlLayoutSystem_1);
					this.form4_0.method_13();
				}
			}
			if (dockTarget != null && dockTarget.type == Class7.DockTargetType.Undefined)
			{
				dockTarget = null;
			}
			if (this.form4_0 != null && this.form4_0.Rectangle_5.Contains(position) && dockTarget == null)
			{
				dockTarget = this.form4_0.method_4(position);
			}
			object[] array2 = this.arrayList_0.ToArray();
			for (int j = 0; j < array2.Length; j++)
			{
				Class8.Form4 form2 = (Class8.Form4)array2[j];
				if (dockTarget == null && form2.Rectangle_5.Contains(position))
				{
					dockTarget = form2.method_4(position);
				}
			}
			return dockTarget;
		}

		private void method_22()
		{
			this.rectangle_1 = Class7.smethod_2(base.Manager.DockSystemContainer.ClientRectangle, base.Manager.DockSystemContainer);
			this.rectangle_2 = Class7.smethod_2(Class7.smethod_1(base.Manager.DockSystemContainer), base.Manager.DockSystemContainer);
			if (base.method_5(ContainerDockLocation.Top))
			{
				this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_1, DockStyle.Top));
			}
			if (base.method_5(ContainerDockLocation.Left))
			{
				this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_1, DockStyle.Left));
			}
			if (base.method_5(ContainerDockLocation.Bottom))
			{
				this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_1, DockStyle.Bottom));
			}
			if (base.method_5(ContainerDockLocation.Right))
			{
				this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_1, DockStyle.Right));
			}
			bool flag = base.DockContainer_0.Dock == DockStyle.Fill && !base.DockContainer_0.IsFloating;
			bool flag2 = base.method_5(ContainerDockLocation.Left) || base.method_5(ContainerDockLocation.Right) || base.method_5(ContainerDockLocation.Top) || base.method_5(ContainerDockLocation.Bottom);
			if (!flag && (base.method_5(ContainerDockLocation.Center) || flag2))
			{
				this.arrayList_0.Add(new Class8.Form4(this, this.rectangle_2, DockStyle.Fill));
			}
			if (base.Manager != null && base.Manager.OwnerForm != null)
			{
				foreach (Form ownedForm in this.arrayList_0)
				{
					base.Manager.OwnerForm.AddOwnedForm(ownedForm);
				}
			}
		}

		private ControlLayoutSystem method_23(Point point_1, out Class7.DockTarget dockTarget_1)
		{
			dockTarget_1 = null;
			for (int i = 1; i >= 0; i--)
			{
				bool flag = Convert.ToBoolean(i);
				ControlLayoutSystem[] controlLayoutSystem_ = base.ControlLayoutSystem_0;
				for (int j = 0; j < controlLayoutSystem_.Length; j++)
				{
					ControlLayoutSystem controlLayoutSystem = controlLayoutSystem_[j];
					if (controlLayoutSystem.DockContainer.IsFloating == flag)
					{
						Rectangle rectangle = new Rectangle(controlLayoutSystem.DockContainer.PointToScreen(controlLayoutSystem.Bounds.Location), controlLayoutSystem.Bounds.Size);
						if (rectangle.Contains(point_1))
						{
							dockTarget_1 = base.method_13(controlLayoutSystem.DockContainer, controlLayoutSystem, point_1, false);
							ControlLayoutSystem result;
							if (dockTarget_1.type != Class7.DockTargetType.Undefined)
							{
								result = null;
							}
							else
							{
								result = controlLayoutSystem;
							}
							return result;
						}
					}
				}
			}
			return null;
		}

		private ArrayList arrayList_0;

		private bool bool_2;

		private bool bool_3;

		private ControlLayoutSystem controlLayoutSystem_1;

		private Class8.Form4 form4_0;

		private Rectangle rectangle_1;

		private Rectangle rectangle_2;

		private class Form4 : Form3
		{
			private Form4()
			{
				base.FormBorderStyle = FormBorderStyle.None;
				base.ShowInTaskbar = false;
				base.StartPosition = FormStartPosition.Manual;
				this.timer_0 = new Timer();
				this.timer_0.Interval = 50;
				this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
				this.bitmap_0 = new Bitmap(88, 88, PixelFormat.Format32bppArgb);
			}

			public Form4(Class8 manager, ControlLayoutSystem layoutSystem) : this()
			{
				this.class8_0 = manager;
				this.controlLayoutSystem_0 = layoutSystem;
				this.rectangle_0 = new Rectangle(layoutSystem.DockContainer.PointToScreen(layoutSystem.Bounds.Location), layoutSystem.Bounds.Size);
				this.rectangle_0 = new Rectangle(this.rectangle_0.X + this.rectangle_0.Width / 2 - 44, this.rectangle_0.Y + this.rectangle_0.Height / 2 - 44, 88, 88);
				this.method_1();
			}

			public Form4(Class8 manager, Rectangle fc, DockStyle dockStyle) : this()
			{
				this.class8_0 = manager;
				this.DockStyle_0 = dockStyle;
				switch (dockStyle)
				{
				case DockStyle.Top:
					this.rectangle_0 = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Y + 15, 88, 88);
					break;
				case DockStyle.Bottom:
					this.rectangle_0 = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Bottom - 88 - 15, 88, 88);
					break;
				case DockStyle.Left:
					this.rectangle_0 = new Rectangle(fc.X + 15, fc.Y + fc.Height / 2 - 44, 88, 88);
					break;
				case DockStyle.Right:
					this.rectangle_0 = new Rectangle(fc.Right - 88 - 15, fc.Y + fc.Height / 2 - 44, 88, 88);
					break;
				case DockStyle.Fill:
					this.rectangle_0 = new Rectangle(fc.X + fc.Width / 2 - 44, fc.Y + fc.Height / 2 - 44, 88, 88);
					break;
				}
				this.method_1();
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					this.bitmap_0.Dispose();
					this.timer_0.Tick -= new EventHandler(this.timer_0_Tick);
					this.timer_0.Dispose();
				}
				base.Dispose(disposing);
			}

			private void method_1()
			{
				using (Graphics graphics = Graphics.FromImage(this.bitmap_0))
				{
					Class16.smethod_0(graphics, Color.Transparent);
					if (this.DockStyle_0 != DockStyle.None && this.DockStyle_0 != DockStyle.Fill)
					{
						if (this.DockStyle_0 == DockStyle.Top)
						{
							using (Image image = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghinttop.png")))
							{
								graphics.DrawImageUnscaled(image, 29, 0);
								goto IL_170;
							}
						}
						if (this.DockStyle_0 != DockStyle.Bottom)
						{
							if (this.DockStyle_0 != DockStyle.Left)
							{
								if (this.DockStyle_0 != DockStyle.Right)
								{
									goto IL_170;
								}
								using (Image image2 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintright.png")))
								{
									graphics.DrawImageUnscaled(image2, 57, 29);
									goto IL_170;
								}
							}
							using (Image image3 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintleft.png")))
							{
								graphics.DrawImageUnscaled(image3, 0, 29);
								goto IL_170;
							}
						}
						using (Image image4 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintbottom.png")))
						{
							graphics.DrawImageUnscaled(image4, 29, 57);
							goto IL_170;
						}
					}
					using (Image image5 = Image.FromStream(typeof(Class8.Form4).Assembly.GetManifestResourceStream("TD.SandDock.Resources.dockinghintcenter.png")))
					{
						graphics.DrawImageUnscaled(image5, 0, 0);
					}
					IL_170:
					Color highlight = SystemColors.Highlight;
					Color transparent = Color.Transparent;
					if (this.DockStyle_0 == DockStyle.None || this.DockStyle_0 == DockStyle.Fill || this.DockStyle_0 == DockStyle.Top)
					{
						this.method_10(graphics, (!this.bool_0 || this.dockSide_0 != DockSide.Top) ? transparent : highlight);
					}
					if (this.DockStyle_0 != DockStyle.None)
					{
						if (this.DockStyle_0 != DockStyle.Fill)
						{
							if (this.DockStyle_0 != DockStyle.Right)
							{
								goto IL_1F3;
							}
						}
					}
					Graphics arg_1EE_1 = graphics;
					Color arg_1EE_2;
					if (this.bool_0)
					{
						if (this.dockSide_0 == DockSide.Right)
						{
							arg_1EE_2 = highlight;
							goto IL_1EE;
						}
					}
					arg_1EE_2 = transparent;
					IL_1EE:
					this.method_9(arg_1EE_1, arg_1EE_2);
					IL_1F3:
					if (this.DockStyle_0 == DockStyle.None || this.DockStyle_0 == DockStyle.Fill || this.DockStyle_0 == DockStyle.Bottom)
					{
						Graphics arg_22A_1 = graphics;
						Color arg_22A_2;
						if (this.bool_0)
						{
							if (this.dockSide_0 == DockSide.Bottom)
							{
								arg_22A_2 = highlight;
								goto IL_22A;
							}
						}
						arg_22A_2 = transparent;
						IL_22A:
						this.method_8(arg_22A_1, arg_22A_2);
					}
					if (this.DockStyle_0 != DockStyle.None)
					{
						if (this.DockStyle_0 != DockStyle.Fill)
						{
							if (this.DockStyle_0 != DockStyle.Left)
							{
								goto IL_26B;
							}
						}
					}
					Graphics arg_266_1 = graphics;
					Color arg_266_2;
					if (this.bool_0)
					{
						if (this.dockSide_0 == DockSide.Left)
						{
							arg_266_2 = highlight;
							goto IL_266;
						}
					}
					arg_266_2 = transparent;
					IL_266:
					this.method_7(arg_266_1, arg_266_2);
					IL_26B:
					if (this.DockStyle_0 == DockStyle.None || this.DockStyle_0 == DockStyle.Fill)
					{
						Graphics arg_299_1 = graphics;
						Color arg_299_2;
						if (this.bool_0)
						{
							if (this.dockSide_0 == DockSide.None)
							{
								arg_299_2 = highlight;
								goto IL_299;
							}
						}
						arg_299_2 = transparent;
						IL_299:
						this.method_6(arg_299_1, arg_299_2);
					}
				}
				base.method_0(this.bitmap_0, 255);
			}

			private void method_10(Graphics graphics_0, Color color_0)
			{
				using (Pen pen = new Pen(color_0))
				{
					graphics_0.DrawLine(pen, 29, 0, 57, 0);
					graphics_0.DrawLine(pen, 57, 0, 57, 23);
					graphics_0.DrawLine(pen, 29, 0, 29, 23);
				}
			}

			public void method_11()
			{
				this.bool_2 = true;
				this.method_12();
			}

			public void method_12()
			{
				if (base.Visible || (!this.bool_1 && this.timer_0.Enabled))
				{
					this.int_5 = Environment.TickCount;
					this.bool_1 = true;
					this.timer_0.Start();
				}
			}

			public void method_13()
			{
				base.method_0(this.bitmap_0, 0);
				this.method_14();
				this.int_5 = Environment.TickCount;
				this.bool_1 = false;
				this.timer_0.Start();
			}

			public void method_14()
			{
			//	Native.SetWindowPos(new HandleRef(this, Handle), -1, this.rectangle_0.X, this.rectangle_0.Y, this.rectangle_0.Width, this.rectangle_0.Height, 80);
                Native.SetWindowPos(Handle, new IntPtr(-1), this.rectangle_0.X, this.rectangle_0.Y, this.rectangle_0.Width, this.rectangle_0.Height, 80);

            }

            private DockTarget method_2(Point point_0)
			{
				DockTarget dockTarget = new Class7.DockTarget(Class7.DockTargetType.SplitExistingSystem);
				dockTarget.layoutSystem = this.controlLayoutSystem_0;
				dockTarget.dockContainer = this.controlLayoutSystem_0.DockContainer;
				if (this.method_5(this.Rectangle_1, point_0))
				{
					dockTarget.dockSide = DockSide.Top;
				}
				else if (!this.method_5(this.Rectangle_2, point_0))
				{
					if (!this.method_5(this.Rectangle_3, point_0))
					{
						if (!this.method_5(this.Rectangle_4, point_0))
						{
							if (!this.method_5(this.Rectangle_0, point_0))
							{
								dockTarget.type = Class7.DockTargetType.Undefined;
							}
							else
							{
								dockTarget.type = Class7.DockTargetType.JoinExistingSystem;
								dockTarget.dockSide = DockSide.None;
							}
						}
						else
						{
							dockTarget.dockSide = DockSide.Left;
						}
					}
					else
					{
						dockTarget.dockSide = DockSide.Bottom;
					}
				}
				else
				{
					dockTarget.dockSide = DockSide.Right;
				}
				dockTarget.bounds = this.class8_0.method_20(this.controlLayoutSystem_0.DockContainer, this.controlLayoutSystem_0, dockTarget.dockSide);
				return dockTarget;
			}

			private Class7.DockTarget method_3(Point point_0)
			{
				Class7.DockTarget dockTarget = new Class7.DockTarget(Class7.DockTargetType.SplitExistingSystem);
				dockTarget.layoutSystem = this.controlLayoutSystem_0;
				dockTarget.dockContainer = ((this.controlLayoutSystem_0 != null) ? this.controlLayoutSystem_0.DockContainer : null);
				if (this.method_5(this.Rectangle_1, point_0) && this.class8_0.method_5(ContainerDockLocation.Top))
				{
					if (this.DockStyle_0 != DockStyle.Top)
					{
						if (this.DockStyle_0 != DockStyle.Fill)
						{
							goto IL_75;
						}
					}
					dockTarget.dockLocation = ContainerDockLocation.Top;
					dockTarget.dockSide = DockSide.Top;
					goto IL_178;
				}
				IL_75:
				if (this.method_5(this.Rectangle_2, point_0) && this.class8_0.method_5(ContainerDockLocation.Right))
				{
					if (this.DockStyle_0 != DockStyle.Right)
					{
						if (this.DockStyle_0 != DockStyle.Fill)
						{
							goto IL_B9;
						}
					}
					dockTarget.dockLocation = ContainerDockLocation.Right;
					dockTarget.dockSide = DockSide.Right;
					goto IL_178;
				}
				IL_B9:
				if (this.method_5(this.Rectangle_3, point_0) && this.class8_0.method_5(ContainerDockLocation.Bottom))
				{
					if (this.DockStyle_0 == DockStyle.Bottom || this.DockStyle_0 == DockStyle.Fill)
					{
						dockTarget.dockLocation = ContainerDockLocation.Bottom;
						dockTarget.dockSide = DockSide.Bottom;
						goto IL_178;
					}
				}
				if (!this.method_5(this.Rectangle_4, point_0) || !this.class8_0.method_5(ContainerDockLocation.Left) || (this.DockStyle_0 != DockStyle.Left && this.DockStyle_0 != DockStyle.Fill))
				{
					if (this.method_5(this.Rectangle_0, point_0) && this.class8_0.method_5(ContainerDockLocation.Center))
					{
						if (this.DockStyle_0 == DockStyle.Fill)
						{
							dockTarget.dockLocation = ContainerDockLocation.Center;
							dockTarget.dockSide = DockSide.None;
							goto IL_178;
						}
					}
					dockTarget.type = Class7.DockTargetType.Undefined;
				}
				else
				{
					dockTarget.dockLocation = ContainerDockLocation.Left;
					dockTarget.dockSide = DockSide.Left;
				}
				IL_178:
				if (dockTarget.type != Class7.DockTargetType.Undefined)
				{
					dockTarget.type = Class7.DockTargetType.CreateNewContainer;
					dockTarget.middle = (this.DockStyle_0 == DockStyle.Fill);
					dockTarget.bounds = Class7.smethod_2(this.class8_0.method_8(dockTarget.dockLocation, this.class8_0.Int32_0, dockTarget.middle), this.class8_0.Manager.DockSystemContainer);
				}
				return dockTarget;
			}

			public Class7.DockTarget method_4(Point point_0)
			{
				Point point_ = base.PointToClient(point_0);
				Class7.DockTarget dockTarget;
				if (this.controlLayoutSystem_0 != null)
				{
					dockTarget = this.method_2(point_);
				}
				else
				{
					dockTarget = this.method_3(point_);
				}
				bool flag = dockTarget.type != Class7.DockTargetType.Undefined;
				DockSide dockSide = (dockTarget.type == Class7.DockTargetType.Undefined) ? this.dockSide_0 : dockTarget.dockSide;
				if (flag != this.bool_0 || dockSide != this.dockSide_0)
				{
					this.bool_0 = flag;
					this.dockSide_0 = dockSide;
					this.method_1();
				}
				return dockTarget;
			}

			private bool method_5(Rectangle rectangle_1, Point point_0)
			{
				return rectangle_1.Contains(point_0);
			}

			private void method_6(Graphics graphics_0, Color color_0)
			{
				using (Pen pen = new Pen(color_0))
				{
					graphics_0.DrawLine(pen, 22, 29, 29, 22);
					graphics_0.DrawLine(pen, 57, 22, 64, 29);
					graphics_0.DrawLine(pen, 64, 57, 57, 64);
					graphics_0.DrawLine(pen, 29, 64, 22, 57);
				}
			}

			private void method_7(Graphics graphics_0, Color color_0)
			{
				using (Pen pen = new Pen(color_0))
				{
					graphics_0.DrawLine(pen, 0, 29, 0, 57);
					graphics_0.DrawLine(pen, 0, 57, 23, 57);
					graphics_0.DrawLine(pen, 0, 29, 23, 29);
				}
			}

			private void method_8(Graphics graphics_0, Color color_0)
			{
				using (Pen pen = new Pen(color_0))
				{
					graphics_0.DrawLine(pen, 29, 87, 57, 87);
					graphics_0.DrawLine(pen, 29, 87, 29, 64);
					graphics_0.DrawLine(pen, 57, 87, 57, 64);
				}
			}

			private void method_9(Graphics graphics_0, Color color_0)
			{
				using (Pen pen = new Pen(color_0))
				{
					graphics_0.DrawLine(pen, 87, 29, 87, 57);
					graphics_0.DrawLine(pen, 87, 29, 64, 29);
					graphics_0.DrawLine(pen, 87, 57, 64, 57);
				}
			}

			private void timer_0_Tick(object sender, EventArgs e)
			{
				int num = Environment.TickCount - this.int_5;
				if (num > 200)
				{
					num = 200;
				}
				double num2 = (double)num / 200.0;
				if (this.bool_1)
				{
					num2 = (1.0 - num2) * 255.0;
				}
				else
				{
					num2 *= 255.0;
				}
				base.method_0(this.bitmap_0, (byte)num2);
				if (num >= 200)
				{
					this.timer_0.Stop();
					base.Visible = !this.bool_1;
					if (this.bool_2)
					{
						base.Dispose();
					}
				}
			}

			public DockStyle DockStyle_0 { get; }

		    private Rectangle Rectangle_0 => new Rectangle(28, 28, 32, 32);

		    private Rectangle Rectangle_1 => new Rectangle(29, 0, 29, 28);

		    private Rectangle Rectangle_2 => new Rectangle(60, 29, 28, 29);

		    private Rectangle Rectangle_3 => new Rectangle(29, 60, 29, 28);

		    private Rectangle Rectangle_4 => new Rectangle(0, 29, 28, 29);

		    public Rectangle Rectangle_5 => this.rectangle_0;

		    private Bitmap bitmap_0;

			private bool bool_0;

			private bool bool_1;

			private bool bool_2;

			private Class8 class8_0;

			private ControlLayoutSystem controlLayoutSystem_0;

			private DockSide dockSide_0 = DockSide.None;

		    private const int int_0 = 88;

			private const int int_1 = 88;

			private const int int_2 = 200;

			private const int int_3 = 16;

			private const int int_4 = 64;

			private int int_5;

			private Rectangle rectangle_0 = Rectangle.Empty;

			private Timer timer_0;
		}
	}
}
