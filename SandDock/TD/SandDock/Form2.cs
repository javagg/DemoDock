using System;
using System.Drawing;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Form2 : Form
	{
		public Form2(Class5 container)
		{
			this.class5_0 = container;
			base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
			base.StartPosition = FormStartPosition.Manual;
			base.ShowInTaskbar = false;
		}

		private bool method_0()
		{
			if (this.class5_0.HasSingleControlLayoutSystem)
			{
				ControlLayoutSystem controlLayoutSystem = (ControlLayoutSystem)this.class5_0.LayoutSystem.LayoutSystems[0];
				if (controlLayoutSystem.SelectedControl != null)
				{
					this.class5_0.method_0(new ShowControlContextMenuEventArgs(controlLayoutSystem.SelectedControl, controlLayoutSystem.SelectedControl.PointToClient(Cursor.Position), ContextMenuContext.RightClick));
					return true;
				}
			}
			return false;
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this.class5_0.ActiveControl == null)
			{
				ControlLayoutSystem controlLayoutSystem = LayoutUtilities.FindControlLayoutSystem(this.class5_0);
				if (controlLayoutSystem != null && controlLayoutSystem.SelectedControl != null)
				{
					this.class5_0.ActiveControl = controlLayoutSystem.SelectedControl;
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == MouseButtons.Left)
			{
				if (this.point_0 != Point.Empty)
				{
					Rectangle rectangle = new Rectangle(this.point_0, SystemInformation.DragSize);
					rectangle.Offset(-SystemInformation.DragSize.Width / 2, -SystemInformation.DragSize.Height / 2);
					if (!rectangle.Contains(e.X, e.Y))
					{
						this.point_0.Y = this.point_0.Y + (SystemInformation.ToolWindowCaptionHeight + SystemInformation.FrameBorderSize.Height);
						this.class5_0.LayoutSystem.method_0(this.class5_0.Manager, this.class5_0, this.class5_0.LayoutSystem, null, this.class5_0.DockControl_0.MetaData.DockedContentSize, this.point_0, this.class5_0.Manager.DockingHints, this.class5_0.Manager.DockingManager);
						this.class5_0.layoutSystemBase_0 = this.class5_0.LayoutSystem;
						base.Capture = false;
						this.class5_0.Capture = true;
						this.point_0 = Point.Empty;
					}
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.point_0 = Point.Empty;
		}

		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			if (this.class5_0 != null)
			{
				DockControl[] dockControl_ = this.class5_0.LayoutSystem.DockControl_0;
				for (int i = 0; i < dockControl_.Length; i++)
				{
					DockControl dockControl = dockControl_[i];
					dockControl.FloatingLocation = base.Location;
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.class5_0 != null)
			{
				DockControl[] dockControl_ = this.class5_0.LayoutSystem.DockControl_0;
				for (int i = 0; i < dockControl_.Length; i++)
				{
					DockControl dockControl = dockControl_[i];
					dockControl.FloatingSize = base.Size;
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 161)
			{
				if (m.WParam.ToInt32() == 2)
				{
					Class20.ReleaseCapture();
					base.Activate();
					this.point_0 = base.PointToClient(Cursor.Position);
					base.Capture = true;
					m.Result = IntPtr.Zero;
					return;
				}
			}
			else if (m.Msg == 163)
			{
				if (m.WParam.ToInt32() == 2)
				{
					this.OnDoubleClick(EventArgs.Empty);
					m.Result = IntPtr.Zero;
					return;
				}
			}
			else if (m.Msg == 164)
			{
				base.Capture = false;
				if (this.method_0())
				{
					m.Result = IntPtr.Zero;
					return;
				}
			}
			base.WndProc(ref m);
		}

		private Class5 class5_0;

		private const int int_0 = 2;

		private const int int_1 = 165;

		private const int int_2 = 164;

		private const int int_3 = 161;

		private const int int_4 = 163;

		private Point point_0;
	}
}
