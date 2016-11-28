using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock.Design;
using TD.Util;
using BorderStyle = TD.SandDock.Rendering.BorderStyle;

namespace TD.SandDock
{
    public enum DocumentContainerWindowOpenPosition
    {
        First,
        Last
    }

    public static class WMConstants
    {
        public const int COLOR_GRADIENTACTIVECAPTION = 27;
        public const int SM_REMOTESESSION = 0x1000;

        public const int WS_EX_NOACTIVATE = 0x08000000;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_SYSMENU = 0x00080000; //524288 
        public const int WS_OVERLAPPED = 0;

        public const int HWND_TOPMOST = -1;
        public const int HWND_TOP = 0;
        public const int SWP_SHOWWINDOW = 64;
        
        public const int SWP_NOACTIVATE = 16;

        public const int SWP_HIDEWINDOW = 128;

        public const int SWP_NOZORDER = 4;

        public const int ULW_ALPHA = 2;
        public const int LWA_ALPHA = 2;

        public const int WM_KEYFIRST = 256;
        public const int WM_KEYLAST = 264;
        public const int WM_KEYUP = 257;
        public const int WM_SYSKEYDOWN = 260;
        public const int WM_SYSKEYUP = 261;
        public const int WM_PAINT = 15;
        public const int VK_TAB = 9;
        public const int VK_SHIFT = 16;
        public const int VK_CONTROL = 17;
        private const int MK_MBUTTON = 16;
        public const int WM_MOUSEACTIVATE = 33;
        public const int WM_NCLBUTTONDOWN= 161;

        public const int WM_NCLBUTTONDBLCLK = 163;
        public const int WM_NCRBUTTONDOWN = 164;
      

        public const int VK_MENU = 0x12;
        public const int HTCAPTION = 2;
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
			if (int_8 > _documents.Length)
			{
				int_8 = _documents.Length;
			}
			var i = _documents.Length - 1 - int_8;
			_documents[i].method_12(true);
			return _documents[i];
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DockControl.DrawBorder(this, e.Graphics, _borderStyle);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((keyData != (Keys.LButton | Keys.Back | Keys.Control) && keyData != (Keys.LButton | Keys.Back | Keys.Shift | Keys.Control)) || !AllowKeyboardNavigation)
			{
				return base.ProcessCmdKey(ref msg, keyData);
			}
			var documents = Manager.GetDockControls(DockSituation.Document);
		    if (documents.Length < 2) return true;
		    var array = new DateTime[documents.Length];
			for (var i = 0; i < documents.Length; i++)
			{
				array[i] = documents[i].MetaData.LastFocused;
			}
			Array.Sort(array, documents);
			_documents = documents;
			if ((keyData & Keys.Shift) == Keys.Shift)
			{
				int_8 = _documents.Length - 1;
			}
			else
			{
				int_8 = 1;
			}
			method_17();
			Application.AddMessageFilter(this);
			return true;
		}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			if (m.Msg == WMConstants.WM_KEYFIRST && m.WParam.ToInt32() == WMConstants.VK_TAB)
			{
				if ((ModifierKeys & Keys.Shift) != Keys.Shift)
				{
					int_8++;
				}
				else
				{
					int_8--;
				}
				if (int_8 > _documents.Length - 1)
				{
					int_8 = 0;
				}
				if (int_8 < 0)
				{
					int_8 = _documents.Length - 1;
				}
				method_17();
				return true;
			}
			if (m.Msg == WMConstants.WM_KEYFIRST && m.WParam.ToInt32() == WMConstants.VK_SHIFT)
			{
				return true;
			}
			if (m.Msg == WMConstants.WM_KEYUP)
			{
				if (m.WParam.ToInt32() == WMConstants.VK_CONTROL)
				{
					goto IL_DB;
				}
			}
			if (m.Msg != WMConstants.WM_KEYFIRST)
			{
				return false;
			}
			IL_DB:
			DockControl dockControl = method_17();
			int_8 = -1;
			_documents = null;
			dockControl.method_12(true);
			Application.RemoveMessageFilter(this);
			return true;
		}

		internal override ControlLayoutSystem CreateNewControlLayoutSystem() => new DocumentLayoutSystem();

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

        [Naming]
        internal bool HasDocuments => _documents != null;

        [Naming(NamingType.FromOldVersion)]
        private bool AllowKeyboardNavigation => Manager?.AllowKeyboardNavigation ?? true;

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

		internal override bool CanShowCollapsed => false;

        [Category("Appearance"), DefaultValue(BorderStyle.Flat), Description("The type of border to be drawn around the control.")]
		internal BorderStyle BorderStyle
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
				case BorderStyle.Flat:
				case BorderStyle.RaisedThin:
				case BorderStyle.SunkenThin:
					rect.Inflate(-1, -1);
					break;
				case BorderStyle.RaisedThick:
				case BorderStyle.SunkenThick:
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
			    if (value != DockStyle.Fill) throw new ArgumentException("Only the Fill dock style is valid for this type of container.");
			    base.Dock = value;
			}
		}

        [Naming(NamingType.FromOldVersion)]
		internal DocumentOverflowMode DocumentOverflow
		{
			get
			{
				return _documentOverflow;
			}
			set
			{
				_documentOverflow = value;
				CalculateAllMetricsAndLayout();
			}
		}

		private bool _integralClose;

		private BorderStyle _borderStyle = BorderStyle.Flat;

		private DockControl[] _documents;

		private DocumentOverflowMode _documentOverflow = DocumentOverflowMode.Scrollable;

		private int int_8 = -1;
	}
}
