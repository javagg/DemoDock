using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TD.SandDock.Design
{
    internal class DockControlDesigner : ParentControlDesigner
    {
        protected override void Dispose(bool disposing)
        {
            _control.ControlAdded -= OnControlAddedOrRemoved;
            _control.ControlRemoved -= OnControlAddedOrRemoved;
            _selectionService.SelectionChanged -= OnSelectionChanged;
            base.Dispose(disposing);
        }

        private void OnControlAddedOrRemoved(object sender, ControlEventArgs e)
        {
            if (_control.Controls.Count == 0 || _control.Controls.Count == 1)
                _control.Invalidate();
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (!(component is DockControl))
                SandDockLanguage.ShowCachedAssemblyError(component.GetType().Assembly, GetType().Assembly);

            _changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            _designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
            _selectionService = (ISelectionService)GetService(typeof(ISelectionService));
            _control = (DockControl)component;
            _control.InitStyle();
            _selectionService.SelectionChanged += OnSelectionChanged;
            _control.ControlAdded += OnControlAddedOrRemoved;
            _control.ControlRemoved += OnControlAddedOrRemoved;
            if (_control.Collapsed)
            {
                Collapsed = true;
                _control.Collapsed = false;
            }
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            var selected = _selectionService.GetComponentSelected(Component);
            if (selected == _selected) return;
            _selected = selected;
            ((DockControl)Component).LayoutSystem.OnLeave();
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            base.OnPaintAdornments(pe);
            if (_control.Controls.Count == 0)
            {
                var rect = _control.ClientRectangle;
                rect.Inflate(-10, -10);
                using (var font = new Font(_control.Font.Name, 6.75f))
                    TextRenderer.DrawText(pe.Graphics,
                        "To redock windows, click and drag their tabs or titlebars to other locations on your form.", font,
                        rect, SystemColors.ControlDarkDark, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
            }
            if (_control.BorderStyle != Rendering.BorderStyle.None) return;
            using (var pen = new Pen(SystemColors.ControlDark))
            {
                pen.DashStyle = DashStyle.Dot;
                var rect = _control.ClientRectangle;
                rect.Width--;
                rect.Height--;
                pe.Graphics.DrawRectangle(pen, rect);
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            foreach (var key in new[] { "Collapsed" })
            {
                var p = (PropertyDescriptor)properties[key];
                if (p != null)
                    properties[key] = TypeDescriptor.CreateProperty(typeof(DockControlDesigner), p);
            }
        }

        public bool Collapsed
        {
            get
            {
                return (bool)ShadowProperties["Collapsed"];
            }
            set
            {
                ShadowProperties["Collapsed"] = value;
                if (_control.LayoutSystem == null || _collapsed) return;
                _collapsed = true;
                try
                {
                    foreach (var control in _control.LayoutSystem.Controls.Cast<DockControl>().Where(c => c != _control))
                        TypeDescriptor.GetProperties(control)["Collapsed"].SetValue(control, value);
                }
                finally
                {
                    _collapsed = false;
                }
            }
        }

        public override SelectionRules SelectionRules => SelectionRules.None;

        private static bool _collapsed;

        private bool _selected;

        private DockControl _control;

        private IComponentChangeService _changeService;

        private IDesignerHost _designerHost;

        private ISelectionService _selectionService;
    }
}
