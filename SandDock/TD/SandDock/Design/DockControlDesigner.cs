using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TD.SandDock.Rendering;

namespace TD.SandDock.Design
{
	internal class DockControlDesigner : ParentControlDesigner
	{
		public DockControlDesigner()
		{
		}

		protected override void Dispose(bool disposing)
		{
			this.dockControl_0.ControlAdded -= new ControlEventHandler(this.dockControl_0_ControlRemoved);
			this.dockControl_0.ControlRemoved -= new ControlEventHandler(this.dockControl_0_ControlRemoved);
			this.iselectionService_0.SelectionChanged -= new EventHandler(this.iselectionService_0_SelectionChanged);
			base.Dispose(disposing);
		}

		private void dockControl_0_ControlRemoved(object sender, ControlEventArgs e)
		{
			if (this.dockControl_0.Controls.Count == 0 || this.dockControl_0.Controls.Count == 1)
			{
				this.dockControl_0.Invalidate();
			}
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			if (!(component is DockControl))
			{
				SandDockLanguage.ShowCachedAssemblyError(component.GetType().Assembly, base.GetType().Assembly);
			}
			this.icomponentChangeService_0 = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			this.idesignerHost_0 = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			this.iselectionService_0 = (ISelectionService)this.GetService(typeof(ISelectionService));
			this.dockControl_0 = (DockControl)component;
			this.dockControl_0.method_2();
			this.iselectionService_0.SelectionChanged += new EventHandler(this.iselectionService_0_SelectionChanged);
			this.dockControl_0.ControlAdded += new ControlEventHandler(this.dockControl_0_ControlRemoved);
			this.dockControl_0.ControlRemoved += new ControlEventHandler(this.dockControl_0_ControlRemoved);
			if (this.dockControl_0.Collapsed)
			{
				this.Collapsed = true;
				this.dockControl_0.Collapsed = false;
			}
		}

		private void iselectionService_0_SelectionChanged(object sender, EventArgs e)
		{
			bool componentSelected;
			if ((componentSelected = this.iselectionService_0.GetComponentSelected(base.Component)) != this.bool_1)
			{
				this.bool_1 = componentSelected;
				((DockControl)base.Component).LayoutSystem.vmethod_9();
			}
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);
			if (this.dockControl_0.Controls.Count == 0)
			{
				Rectangle clientRectangle = this.dockControl_0.ClientRectangle;
				clientRectangle.Inflate(-10, -10);
				using (Font font = new Font(this.dockControl_0.Font.Name, 6.75f))
				{
					TextRenderer.DrawText(pe.Graphics, "To redock windows, click and drag their tabs or titlebars to other locations on your form.", font, clientRectangle, SystemColors.ControlDarkDark, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
				}
			}
			if (this.dockControl_0.BorderStyle == TD.SandDock.Rendering.BorderStyle.None)
			{
				using (Pen pen = new Pen(SystemColors.ControlDark))
				{
					pen.DashStyle = DashStyle.Dot;
					Rectangle clientRectangle2 = this.dockControl_0.ClientRectangle;
					clientRectangle2.Width--;
					clientRectangle2.Height--;
					pe.Graphics.DrawRectangle(pen, clientRectangle2);
				}
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[]
			{
				"Collapsed"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string key = array2[i];
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[key];
				if (propertyDescriptor != null)
				{
					properties[key] = TypeDescriptor.CreateProperty(typeof(DockControlDesigner), propertyDescriptor, new Attribute[0]);
				}
			}
		}

		public bool Collapsed
		{
			get
			{
				return (bool)base.ShadowProperties["Collapsed"];
			}
			set
			{
				base.ShadowProperties["Collapsed"] = value;
				if (this.dockControl_0.LayoutSystem != null && !DockControlDesigner.bool_0)
				{
					DockControlDesigner.bool_0 = true;
					try
					{
						foreach (DockControl dockControl in this.dockControl_0.LayoutSystem.Controls)
						{
							if (dockControl != this.dockControl_0)
							{
								TypeDescriptor.GetProperties(dockControl)["Collapsed"].SetValue(dockControl, value);
							}
						}
					}
					finally
					{
						DockControlDesigner.bool_0 = false;
					}
				}
			}
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				return SelectionRules.None;
			}
		}

		private static bool bool_0;

		private bool bool_1;

		private DockControl dockControl_0;

		private IComponentChangeService icomponentChangeService_0;

		private IDesignerHost idesignerHost_0;

		private ISelectionService iselectionService_0;
	}
}
