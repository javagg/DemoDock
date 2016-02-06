using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TD.SandDock
{
	[Designer("TD.SandDock.Design.TabPageDesigner, SandDock.Design"), ToolboxItem(false)]
	public class TabPage : Panel
	{
		public TabPage()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		public TabPage(string text) : this()
		{
            Text = text;
		}

		protected override void CreateHandle()
		{
			int newIndex = -1;
			if (Parent != null)
			{
				newIndex = Parent.Controls.IndexOf(this);
			}
			base.CreateHandle();
		    Parent?.Controls.SetChildIndex(this, newIndex);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			OnLoad(EventArgs.Empty);
		}

		protected virtual void OnLoad(EventArgs e)
		{
            Load?.Invoke(this,e);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if (base.ClientRectangle == Rectangle.Empty)
			{
				return;
			}
		    var parent = Parent as TabControl;
		    if (parent != null && parent.Renderer.ShouldDrawTabControlBackground)
			{
				parent.Renderer.DrawTabControlBackground(pevent.Graphics, base.ClientRectangle, this.BackColor, true);
				return;
			}
			base.OnPaintBackground(pevent);
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override AnchorStyles Anchor
		{
			get
			{
				return base.Anchor;
			}
			set
			{
				base.Anchor = value;
			}
		}

		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
                (Parent as TabControl)?.Invalidate(this.TabBounds);
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete]
		public Size FloatingSize
		{
			get
			{
				return Size.Empty;
			}
			set
			{
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete]
		public Guid Guid
		{
			get
			{
				return Guid.Empty;
			}
			set
			{
			}
		}

		[Category("Layout"), DefaultValue(0), Description("Indicates the maximum width of the tab.")]
		public int MaximumTabWidth
		{
			get
			{
				return this.int_0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Value must be greater than or equal to zero.");
				}
				this.int_0 = value;
			    (Parent as TabControl)?.method_3();
			}
		}

		[Browsable(false)]
		public Rectangle TabBounds { get; internal set; }

	    [AmbientValue(typeof(Image), null), Category("Appearance"), DefaultValue(typeof(Image), null), Description("The image displayed next to the text on the tab.")]
		public Image TabImage
		{
			get
			{
				return this.image_0;
			}
			set
			{
				this.image_0 = value;
			    (Parent as TabControl)?.method_3();
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete]
		public string TabText
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		[Browsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			    (Parent as TabControl)?.method_3();
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

	    public event EventHandler Load;

		internal bool bool_0;

		internal double double_0;

		//private EventHandler eventHandler_0;

		private Image image_0;

		private int int_0;

		internal int int_1;
	}
}
