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

    public static class WMConstants
    {
        public const int SWP_SHOWWINDOW = 64;
        
        public const int SWP_NOACTIVATE = 16;

        public const int SWP_HIDEWINDOW = 128;

        public const int SWP_NOZORDER = 4;

        public const int WM_KEYFIRST = 256;
        public const int WM_KEYLAST = 264;
        public const int WM_KEYUP = 257;
        public const int WM_SYSKEYDOWN = 260;
        public const int WM_SYSKEYUP = 261;
        public const int WM_PAINT = 15;
        private const int int_5 = 9;
        private const int int_6 = 17;
        private const int MK_MBUTTON = 16;
        public const int WM_MOUSEACTIVATE = 33;
        public const int WM_NCLBUTTONDOWN= 161;

        public const int WM_NCLBUTTONDBLCLK = 163;
        public const int WM_NCRBUTTONDOWN = 164;

        public const int VK_SHIFT = 0x10;
        
        public const int VK_CONTROL = 0x11;

        public const int VK_MENU = 0x12;

        public const int MK_RBUTTON = 2;
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
			DockControl.smethod_0(this, e.Graphics, _borderStyle);
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
			if (m.Msg == WMConstants.WM_KEYFIRST && m.WParam.ToInt32() == 9)
			{
				if ((ModifierKeys & Keys.Shift) != Keys.Shift)
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
			if (m.Msg == WMConstants.WM_KEYFIRST && m.WParam.ToInt32() == 16)
			{
				return true;
			}
			if (m.Msg == WMConstants.WM_KEYUP)
			{
				if (m.WParam.ToInt32() == 17)
				{
					goto IL_DB;
				}
			}
			if (m.Msg != WMConstants.WM_KEYFIRST)
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
				return _integralClose;
			}
			set
			{
				_integralClose = value;
				CalculateAllMetricsAndLayout();
			}
		}

		internal override bool Boolean_6 => false;

        [Category("Appearance"), DefaultValue(Rendering.BorderStyle.Flat), Description("The type of border to be drawn around the control.")]
		internal Rendering.BorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				_borderStyle = value;
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
				return _documentOverflowMode;
			}
			set
			{
				_documentOverflowMode = value;
				CalculateAllMetricsAndLayout();
			}
		}

		private bool _integralClose;

		private Rendering.BorderStyle _borderStyle = Rendering.BorderStyle.Flat;

		private DockControl[] dockControl_0;

		private DocumentOverflowMode _documentOverflowMode = DocumentOverflowMode.Scrollable;

		private int int_8 = -1;
	}
}
