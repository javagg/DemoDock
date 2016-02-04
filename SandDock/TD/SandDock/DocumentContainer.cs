using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock.Design;
using TD.SandDock.Rendering;

namespace TD.SandDock
{
	[Designer(typeof(DocumentContainerDesigner)), ToolboxItem(false)]
	public class DocumentContainer : DockContainer, IMessageFilter
	{
		public DocumentContainer()
		{
			this.Dock = DockStyle.Fill;
			this.BackColor = SystemColors.AppWorkspace;
		}

		private DockControl method_17()
		{
			if (this.int_8 > this.dockControl_0.Length)
			{
				this.int_8 = this.dockControl_0.Length;
			}
			int num = this.dockControl_0.Length - 1 - this.int_8;
			this.dockControl_0[num].method_12(true);
			return this.dockControl_0[num];
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DockControl.smethod_0(this, e.Graphics, this.borderStyle_0);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((keyData != (Keys.LButton | Keys.Back | Keys.Control) && keyData != (Keys.LButton | Keys.Back | Keys.Shift | Keys.Control)) || !this.Boolean_4)
			{
				return base.ProcessCmdKey(ref msg, keyData);
			}
			DockControl[] dockControls = this.Manager.GetDockControls(DockSituation.Document);
			if (dockControls.Length < 2)
			{
				return true;
			}
			DateTime[] array = new DateTime[dockControls.Length];
			for (int i = 0; i < dockControls.Length; i++)
			{
				array[i] = dockControls[i].MetaData.LastFocused;
			}
			Array.Sort<DateTime, DockControl>(array, dockControls);
			this.dockControl_0 = dockControls;
			if ((keyData & Keys.Shift) == Keys.Shift)
			{
				this.int_8 = this.dockControl_0.Length - 1;
			}
			else
			{
				this.int_8 = 1;
			}
			this.method_17();
			Application.AddMessageFilter(this);
			return true;
		}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			if (m.Msg == 256 && m.WParam.ToInt32() == 9)
			{
				if ((Control.ModifierKeys & Keys.Shift) != Keys.Shift)
				{
					this.int_8++;
				}
				else
				{
					this.int_8--;
				}
				if (this.int_8 > this.dockControl_0.Length - 1)
				{
					this.int_8 = 0;
				}
				if (this.int_8 < 0)
				{
					this.int_8 = this.dockControl_0.Length - 1;
				}
				this.method_17();
				return true;
			}
			if (m.Msg == 256 && m.WParam.ToInt32() == 16)
			{
				return true;
			}
			if (m.Msg == 257)
			{
				if (m.WParam.ToInt32() == 17)
				{
					goto IL_DB;
				}
			}
			if (m.Msg != 256)
			{
				return false;
			}
			IL_DB:
			DockControl dockControl = this.method_17();
			this.int_8 = -1;
			this.dockControl_0 = null;
			dockControl.method_12(true);
			Application.RemoveMessageFilter(this);
			return true;
		}

		internal override ControlLayoutSystem vmethod_1()
		{
			return new DocumentLayoutSystem();
		}

		[DefaultValue(typeof(Color), "AppWorkspace")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		internal bool Boolean_3
		{
			get
			{
				return this.dockControl_0 != null;
			}
		}

		private bool Boolean_4
		{
			get
			{
				return this.Manager == null || this.Manager.AllowKeyboardNavigation;
			}
		}

		internal bool Boolean_5
		{
			get
			{
				return this.bool_2;
			}
			set
			{
				this.bool_2 = value;
				base.CalculateAllMetricsAndLayout();
			}
		}

		internal override bool Boolean_6
		{
			get
			{
				return false;
			}
		}

		[Category("Appearance"), DefaultValue(typeof(TD.SandDock.Rendering.BorderStyle), "Flat"), Description("The type of border to be drawn around the control.")]
		internal TD.SandDock.Rendering.BorderStyle BorderStyle_0
		{
			get
			{
				return this.borderStyle_0;
			}
			set
			{
				this.borderStyle_0 = value;
				this.OnResize(EventArgs.Empty);
			}
		}

		protected override Size DefaultSize
		{
			get
			{
				return new Size(300, 300);
			}
		}

		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle displayRectangle = base.DisplayRectangle;
				switch (this.borderStyle_0)
				{
				case TD.SandDock.Rendering.BorderStyle.Flat:
				case TD.SandDock.Rendering.BorderStyle.RaisedThin:
				case TD.SandDock.Rendering.BorderStyle.SunkenThin:
					displayRectangle.Inflate(-1, -1);
					break;
				case TD.SandDock.Rendering.BorderStyle.RaisedThick:
				case TD.SandDock.Rendering.BorderStyle.SunkenThick:
					displayRectangle.Inflate(-2, -2);
					break;
				}
				return displayRectangle;
			}
		}

		[DefaultValue(typeof(DockStyle), "Fill")]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				if (value != DockStyle.Fill)
				{
					throw new ArgumentException("Only the Fill dock style is valid for this type of container.");
				}
				base.Dock = value;
			}
		}

		internal DocumentOverflowMode DocumentOverflowMode_0
		{
			get
			{
				return this.documentOverflowMode_0;
			}
			set
			{
				this.documentOverflowMode_0 = value;
				base.CalculateAllMetricsAndLayout();
			}
		}

		private bool bool_2;

		private TD.SandDock.Rendering.BorderStyle borderStyle_0 = TD.SandDock.Rendering.BorderStyle.Flat;

		private DockControl[] dockControl_0;

		private DocumentOverflowMode documentOverflowMode_0 = DocumentOverflowMode.Scrollable;

		private const int int_3 = 256;

		private const int int_4 = 257;

		private const int int_5 = 9;

		private const int int_6 = 17;

		private const int int_7 = 16;

		private int int_8 = -1;
	}
}
