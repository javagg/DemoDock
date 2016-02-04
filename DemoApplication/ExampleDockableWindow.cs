using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using TD.SandDock;

namespace DemoApplication
{
	public partial class ExampleDockableWindow : UserDockableWindow
	{
		public ExampleDockableWindow()
		{
			InitializeComponent();
		}

		private void chkAllowTopDock_CheckedChanged(object sender, EventArgs e)
		{
			DockingRules.AllowDockTop = chkAllowTopDock.Checked;
		}

		private void chkAllowBottomDock_CheckedChanged(object sender, EventArgs e)
		{
			DockingRules.AllowDockBottom = chkAllowBottomDock.Checked;
		}

		private void chkAllowLeftDock_CheckedChanged(object sender, EventArgs e)
		{
			DockingRules.AllowDockLeft = chkAllowLeftDock.Checked;
		}

		private void chkAllowRightDock_CheckedChanged(object sender, EventArgs e)
		{
			DockingRules.AllowDockRight = chkAllowRightDock.Checked;
		}

		private void chkAllowCenterDock_CheckedChanged(object sender, EventArgs e)
		{
			DockingRules.AllowTab = chkAllowCenterDock.Checked;
		}

		private void chkAllowFloat_CheckedChanged(object sender, EventArgs e)
		{
			DockingRules.AllowFloat = chkAllowFloat.Checked;
		}
	}
}
