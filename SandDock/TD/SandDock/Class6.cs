using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal abstract class Class6 : IDisposable, IMessageFilter
	{
		public Class6(Control control, DockingHints dockingHints, bool hollow)
		{
			this.control_0 = control;
			this.bool_1 = hollow;
			bool flag = OSFeature.Feature.IsPresent(OSFeature.LayeredWindows);
			if (dockingHints == DockingHints.TranslucentFill)
			{
				if (!flag)
				{
					dockingHints = DockingHints.RubberBand;
				}
			}
			this.dockingHints_0 = dockingHints;
			this.form_0 = control.FindForm();
			if (this.form_0 != null)
			{
				this.form_0.Deactivate += new EventHandler(this.form_0_Deactivate);
			}
			control.MouseCaptureChanged += new EventHandler(this.method_0);
			Application.AddMessageFilter(this);
			if (dockingHints == DockingHints.TranslucentFill)
			{
				this.form1_0 = new Form1(hollow);
			}
		}

		public Class6(Control control, DockingHints dockingHints, bool hollow, int tabStripSize) : this(control, dockingHints, hollow)
		{
			this.int_5 = tabStripSize;
		}

		public virtual void Cancel()
		{
			this.Dispose();
            Event_0?.Invoke(this, EventArgs.Empty);
		}

		public virtual void Commit()
		{
			this.Dispose();
		}

		public virtual void Dispose()
		{
			if (this.control_0 != null)
			{
				this.control_0.MouseCaptureChanged -= this.method_0;
			}
			this.method_2();
			if (this.dockingHints_0 == DockingHints.TranslucentFill)
			{
				this.form1_0.Dispose();
				this.form1_0 = null;
			}
			if (this.form_0 != null)
			{
				this.form_0.Deactivate -= this.form_0_Deactivate;
			}
			Application.RemoveMessageFilter(this);
			this.form_0 = null;
			this.control_0 = null;
		}

		private void form_0_Deactivate(object sender, EventArgs e)
		{
			this.Cancel();
		}

		private void method_0(object sender, EventArgs e)
		{
			this.Cancel();
		}

		protected void method_1(Rectangle rectangle_1, bool bool_2)
		{
			if (this.rectangle_0 == rectangle_1)
			{
				return;
			}
			if (this.dockingHints_0 == DockingHints.RubberBand)
			{
				this.method_3();
			}
			if (this.dockingHints_0 == DockingHints.RubberBand)
			{
				if (this.bool_1)
				{
					Class12.smethod_0(null, rectangle_1, bool_2, this.int_5);
				}
				else
				{
					Class12.smethod_1(null, rectangle_1);
				}
				this.rectangle_0 = rectangle_1;
				this.bool_0 = bool_2;
				return;
			}
			this.form1_0.method_0(rectangle_1, bool_2);
		}

		protected void method_2()
		{
			if (this.dockingHints_0 == DockingHints.RubberBand)
			{
				this.method_3();
				return;
			}
			this.form1_0.Hide();
		}

		private void method_3()
		{
			if (this.rectangle_0 != Rectangle.Empty)
			{
				if (!this.bool_1)
				{
					Class12.smethod_1(null, this.rectangle_0);
				}
				else
				{
					Class12.smethod_0(null, this.rectangle_0, this.bool_0, this.int_5);
				}
			}
			this.rectangle_0 = Rectangle.Empty;
		}

		private void method_4()
		{
			if (this.dockingHints_0 == DockingHints.RubberBand)
			{
				this.method_3();
			}
		}

		public abstract void OnMouseMove(Point position);

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == 15)
			{
				this.method_4();
			}
			if (m.Msg == 256 || m.Msg == 257)
			{
				if (m.WParam.ToInt32() == 17)
				{
					this.OnMouseMove(Cursor.Position);
					return false;
				}
			}
			if (m.Msg == 256 || m.Msg == 257)
			{
				if (m.WParam.ToInt32() == 16)
				{
					return true;
				}
			}
			if (m.Msg != 260)
			{
				if (m.Msg != 261)
				{
					goto IL_AB;
				}
			}
			if (m.WParam.ToInt32() == 18)
			{
				return true;
			}
			IL_AB:
			if (m.Msg < 256 || m.Msg > 264)
			{
				return false;
			}
			this.Cancel();
			return true;
		}

		internal static bool smethod_0()
		{
			bool result = false;
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				result = (Environment.OSVersion.Version >= new Version(5, 0, 0, 0));
			}
			return result;
		}

	    public event EventHandler Event_0;

		private bool bool_0;

		private bool bool_1;

		private Control control_0;

		private DockingHints dockingHints_0 = DockingHints.TranslucentFill;

		//private EventHandler eventHandler_0;

		private Form1 form1_0;

		private Form form_0;

		private const int int_0 = 256;

		private const int int_1 = 257;

		private const int int_2 = 260;

		private const int int_3 = 261;

		private const int int_4 = 15;

		private int int_5 = 21;

		private Rectangle rectangle_0 = Rectangle.Empty;
	}
}
