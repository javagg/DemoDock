using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock.Design;

namespace TD.SandDock
{
    public enum DocumentContainerWindowOpenPosition
    {
        First,
        Last
    }

    [Designer(typeof(DocumentContainerDesigner)), ToolboxItem(false)]
	public class DocumentContainer : DockContainer, IMessageFilter
	{
		public DocumentContainer()
		{
			Dock = DockStyle.Fill;
			BackColor = SystemColors.AppWorkspace;
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
			DockControl.smethod_0(this, e.Graphics, this.borderStyle);
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

		internal override ControlLayoutSystem vmethod_1() => new DocumentLayoutSystem();

        [DefaultValue(typeof(Color), "AppWorkspace")]
		public sealed override Color BackColor
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

		internal bool Boolean_3 => this.dockControl_0 != null;

        private bool Boolean_4 => Manager?.AllowKeyboardNavigation ?? true;

        internal bool IntegralClose
		{
			get
			{
				return this.integralClose;
			}
			set
			{
				this.integralClose = value;
				CalculateAllMetricsAndLayout();
			}
		}

		internal override bool Boolean_6 => false;

        [Category("Appearance"), DefaultValue(Rendering.BorderStyle.Flat), Description("The type of border to be drawn around the control.")]
		internal Rendering.BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				this.borderStyle = value;
				OnResize(EventArgs.Empty);
			}
		}

		protected override Size DefaultSize => new Size(300, 300);

        public override Rectangle DisplayRectangle
		{
			get
			{
				var rect = base.DisplayRectangle;
				switch (BorderStyle)
				{
				case Rendering.BorderStyle.Flat:
				case Rendering.BorderStyle.RaisedThin:
				case Rendering.BorderStyle.SunkenThin:
					rect.Inflate(-1, -1);
					break;
				case Rendering.BorderStyle.RaisedThick:
				case Rendering.BorderStyle.SunkenThick:
					rect.Inflate(-2, -2);
					break;
				}
				return rect;
			}
		}

		[DefaultValue(DockStyle.Fill)]
		public sealed override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
			    if (value != DockStyle.Fill)
			        throw new ArgumentException("Only the Fill dock style is valid for this type of container.");
			    base.Dock = value;
			}
		}

		internal DocumentOverflowMode DocumentOverflow
		{
			get
			{
				return this.documentOverflowMode_0;
			}
			set
			{
				this.documentOverflowMode_0 = value;
				CalculateAllMetricsAndLayout();
			}
		}

		private bool integralClose;

		private Rendering.BorderStyle borderStyle = Rendering.BorderStyle.Flat;

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
