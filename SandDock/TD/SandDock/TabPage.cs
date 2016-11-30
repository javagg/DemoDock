using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TD.SandDock
{
	[Designer("Design.TabPageDesigner"), ToolboxItem(false)]
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
		    var i = Parent?.Controls.IndexOf(this) ?? -1;
		    base.CreateHandle();
		    Parent?.Controls.SetChildIndex(this, i);
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
		    if (ClientRectangle == Rectangle.Empty) return;
		    var parent = Parent as TabControl;
		    if (parent != null && parent.Renderer.ShouldDrawTabControlBackground)
		        parent.Renderer.DrawTabControlBackground(pevent.Graphics, ClientRectangle, BackColor, true);
		    else
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
                (Parent as TabControl)?.Invalidate(TabBounds);
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
				return _maximumTabWidth;
			}
			set
			{
			    if (value < 0)
			        throw new ArgumentException("Value must be greater than or equal to zero.");
			    _maximumTabWidth = value;
			    (Parent as TabControl)?.MeasureTabStripBounds();
			}
		}

		[Browsable(false)]
		public Rectangle TabBounds { get; internal set; }

	    [AmbientValue(null), Category("Appearance"), DefaultValue(null), Description("The image displayed next to the text on the tab.")]
		public Image TabImage
		{
			get
			{
				return _tabImage;
			}
			set
			{
				_tabImage = value;
			    (Parent as TabControl)?.MeasureTabStripBounds();
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
			    (Parent as TabControl)?.MeasureTabStripBounds();
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

		internal double TabWidth;

		private Image _tabImage;

		private int _maximumTabWidth;

		internal int Index;
	}
}
