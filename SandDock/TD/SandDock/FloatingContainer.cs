using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class FloatingContainer : DockContainer
	{
		public FloatingContainer(SandDockManager manager, Guid guid)
		{
		    if (manager == null)
		        throw new ArgumentNullException(nameof(manager));

		    this.form2_0 = new Form2(this);
			this.form2_0.Activated += base.method_11;
			this.form2_0.Deactivate += base.method_12;
			this.form2_0.Closing += this.form2_0_Closing;
			this.form2_0.DoubleClick += this.form2_0_DoubleClick;
			this.LayoutSystem.Event_0 += this.method_22;
			this.method_22(this.LayoutSystem, EventArgs.Empty);
			this.Manager = manager;
			this.Guid_0 = guid;
			this.form2_0.Controls.Add(this);
			this.Dock = DockStyle.Fill;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				this.LayoutSystem.Event_0 -= this.method_22;
				this.form2_0.Activated -= base.method_11;
				this.form2_0.Deactivate -= base.method_12;
				this.form2_0.Closing -= this.form2_0_Closing;
				this.form2_0.DoubleClick -= this.form2_0_DoubleClick;
				LayoutUtilities.smethod_8(this);
				this.form2_0.Dispose();
			}
			base.Dispose(disposing);
		}

		private void form2_0_Closing(object sender, CancelEventArgs e)
		{
			if (this.bool_2)
			{
				DockControl[] dockControl_ = this.LayoutSystem.DockControl_0;
				DockControl[] array = dockControl_;
				for (int i = 0; i < array.Length; i++)
				{
					DockControl dockControl = array[i];
					if (!dockControl.AllowClose)
					{
						e.Cancel = true;
						return;
					}
				}
				DockControl[] array2 = dockControl_;
				for (int j = 0; j < array2.Length; j++)
				{
					DockControl dockControl2 = array2[j];
					if (!dockControl2.Close())
					{
						e.Cancel = true;
						break;
					}
				}
			}
		}

		private void form2_0_DoubleClick(object sender, EventArgs e)
		{
			Form arg_05_0 = Form.ActiveForm;
			Form arg_0C_0 = this.Form_0;
			DockControl[] dockControl_ = this.LayoutSystem.DockControl_0;
			DockControl dockControl_2 = this.DockControl_0;
			if (dockControl_[0].MetaData.LastFixedDockSituation == DockSituation.Docked)
			{
				if (!this.LayoutSystem.vmethod_3(dockControl_2.MetaData.LastFixedDockSide))
				{
					return;
				}
			}
			if (dockControl_[0].MetaData.LastFixedDockSituation == DockSituation.Document && !this.LayoutSystem.vmethod_3(ContainerDockLocation.Center))
			{
				return;
			}
			SandDockManager arg_72_0 = this.Manager;
			this.LayoutSystem = new SplitLayoutSystem();
			base.Dispose();
			if (dockControl_2.MetaData.LastFixedDockSituation == DockSituation.Docked)
			{
				dockControl_[0].OpenDocked(WindowOpenMethod.OnScreenActivate);
			}
			else
			{
				dockControl_[0].OpenDocument(WindowOpenMethod.OnScreenActivate);
			}
			DockControl[] array = new DockControl[dockControl_.Length - 1];
			Array.Copy(dockControl_, 1, array, 0, dockControl_.Length - 1);
			dockControl_[0].LayoutSystem.Controls.AddRange(array);
			dockControl_[0].LayoutSystem.SelectedControl = dockControl_2;
		}

		public void method_17()
		{
			this.form2_0.Show();
		}

		public void method_18()
		{
			this.form2_0.Hide();
		}

		public void method_19(Rectangle rectangle_2, bool bool_3, bool bool_4)
		{
			int num = 0;
			if (!bool_3)
			{
				num |= 128;
			}
			else
			{
				num |= 64;
			}
			if (!bool_4)
			{
				num |= 16;
			}
			IntPtr zero = IntPtr.Zero;
			Native.SetWindowPos(form2_0.Handle, IntPtr.Zero, rectangle_2.X, rectangle_2.Y, rectangle_2.Width, rectangle_2.Height, num);
			this.form2_0.Visible = bool_3;
			if (bool_3)
			{
				foreach (Control control in this.form2_0.Controls)
				{
					control.Visible = true;
				}
			}
		}

		private void method_20(DockControl dockControl_0, DockControl dockControl_1)
		{
			if (dockControl_1 != null)
			{
				this.form2_0.Text = dockControl_1.Text;
				return;
			}
			this.form2_0.Text = "";
		}

		public void method_21()
		{
			this.method_22(null, null);
		}

		private void method_22(object sender, EventArgs e)
		{
			if (this.controlLayoutSystem_0 != null)
			{
				this.controlLayoutSystem_0.Event_0 -= this.method_20;
			}
			if (!base.HasSingleControlLayoutSystem)
			{
				this.form2_0.Text = "";
				this.controlLayoutSystem_0 = null;
				return;
			}
			this.controlLayoutSystem_0 = (ControlLayoutSystem)this.LayoutSystem.LayoutSystems[0];
			this.controlLayoutSystem_0.Event_0 += this.method_20;
			this.method_20(null, this.controlLayoutSystem_0.SelectedControl);
		}

		internal void method_23()
		{
			this.form2_0.Activate();
		}

		internal override bool Boolean_6 => false;

	    public DockControl DockControl_0
		{
			get
			{
				ControlLayoutSystem controlLayoutSystem = LayoutUtilities.FindControlLayoutSystem(this);
				if (controlLayoutSystem == null)
				{
					throw new InvalidOperationException("A docking operation was started while the window hierarchy is in an invalid state.");
				}
				return controlLayoutSystem.SelectedControl;
			}
		}

		public Form Form_0 => this.form2_0;

	    public Guid Guid_0 { get; }

	    public override bool IsFloating => true;

	    public override SplitLayoutSystem LayoutSystem
		{
			get
			{
				return base.LayoutSystem;
			}
			set
			{
				this.LayoutSystem.Event_0 -= this.method_22;
				base.LayoutSystem = value;
				this.LayoutSystem.Event_0 += this.method_22;
				this.method_22(this.LayoutSystem, EventArgs.Empty);
			}
		}

		public override SandDockManager Manager
		{
			get
			{
				return base.Manager;
			}
			set
			{
			    Manager?.OwnerForm?.RemoveOwnedForm(this.form2_0);
			    base.Manager = value;
				if (Manager?.OwnerForm != null)
				{
					Manager.OwnerForm.AddOwnedForm(this.form2_0);
					Font = new Font(Manager.OwnerForm.Font, Manager.OwnerForm.Font.Style);
				}
			}
		}

		public Point Point_0
		{
			get
			{
				return this.form2_0.Location;
			}
			set
			{
				this.form2_0.Location = value;
			}
		}

		public Rectangle Rectangle_1 => this.form2_0.Bounds;

	    public Size Size_0
		{
			get
			{
				return this.form2_0.Size;
			}
			set
			{
				this.form2_0.Size = value;
			}
		}

		private bool bool_2 = true;

		private ControlLayoutSystem controlLayoutSystem_0;

		private Form2 form2_0;

	    private const int int_3 = 64;

		private const int int_4 = 16;

		private const int int_5 = 128;

		private const int int_6 = 4;
	}
}
