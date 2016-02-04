using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TD.SandDock
{
	[Designer("TD.SandDock.Design.TabPageDesigner, SandDock.Design, Version=1.0.0.1, Culture=neutral, PublicKeyToken=75b7ec17dd7c14c3"), ToolboxItem(false)]
	public class TabPage : Panel
	{
		public TabPage()
		{
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		public TabPage(string text) : this()
		{
			this.Text = text;
		}

		protected override void CreateHandle()
		{
			int newIndex = -1;
			if (base.Parent != null)
			{
				newIndex = base.Parent.Controls.IndexOf(this);
			}
			base.CreateHandle();
			if (base.Parent != null)
			{
				base.Parent.Controls.SetChildIndex(this, newIndex);
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this.OnLoad(EventArgs.Empty);
		}

		protected virtual void OnLoad(EventArgs e)
		{
			if (this.eventHandler_0 != null)
			{
				this.eventHandler_0(this, e);
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if (base.ClientRectangle == Rectangle.Empty)
			{
				return;
			}
			if (base.Parent is TabControl && ((TabControl)base.Parent).Renderer.ShouldDrawTabControlBackground)
			{
				((TabControl)base.Parent).Renderer.DrawTabControlBackground(pevent.Graphics, base.ClientRectangle, this.BackColor, true);
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
				if (base.Parent is TabControl)
				{
					base.Parent.Invalidate(this.rectangle_0);
				}
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
				if (base.Parent is TabControl)
				{
					((TabControl)base.Parent).method_3();
				}
			}
		}

		[Browsable(false)]
		public Rectangle TabBounds
		{
			get
			{
				return this.rectangle_0;
			}
		}

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
				if (base.Parent is TabControl)
				{
					((TabControl)base.Parent).method_3();
				}
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
				if (base.Parent is TabControl)
				{
					((TabControl)base.Parent).method_3();
				}
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

		public event EventHandler Load
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.eventHandler_0 = (EventHandler)Delegate.Combine(this.eventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.eventHandler_0 = (EventHandler)Delegate.Remove(this.eventHandler_0, value);
			}
		}

		internal bool bool_0;

		internal double double_0;

		private EventHandler eventHandler_0;

		private Image image_0;

		private int int_0;

		internal int int_1;

		internal Rectangle rectangle_0;
	}
}
