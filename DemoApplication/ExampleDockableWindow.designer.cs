namespace DemoApplication
{
	partial class ExampleDockableWindow
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.chkAllowTopDock = new System.Windows.Forms.CheckBox();
			this.chkAllowBottomDock = new System.Windows.Forms.CheckBox();
			this.chkAllowLeftDock = new System.Windows.Forms.CheckBox();
			this.chkAllowRightDock = new System.Windows.Forms.CheckBox();
			this.chkAllowCenterDock = new System.Windows.Forms.CheckBox();
			this.chkAllowFloat = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(5, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(244, 51);
			this.label1.TabIndex = 0;
			this.label1.Text = "This is an example dockable window. You can change some of its behavior propertie" +
				"s using the checkboxes below.";
			// 
			// chkAllowTopDock
			// 
			this.chkAllowTopDock.Checked = true;
			this.chkAllowTopDock.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowTopDock.Location = new System.Drawing.Point(8, 58);
			this.chkAllowTopDock.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowTopDock.Name = "chkAllowTopDock";
			this.chkAllowTopDock.Size = new System.Drawing.Size(119, 17);
			this.chkAllowTopDock.TabIndex = 1;
			this.chkAllowTopDock.Text = "Allow Top Dock";
			this.chkAllowTopDock.UseVisualStyleBackColor = true;
			this.chkAllowTopDock.CheckedChanged += new System.EventHandler(this.chkAllowTopDock_CheckedChanged);
			// 
			// chkAllowBottomDock
			// 
			this.chkAllowBottomDock.Checked = true;
			this.chkAllowBottomDock.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowBottomDock.Location = new System.Drawing.Point(8, 79);
			this.chkAllowBottomDock.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowBottomDock.Name = "chkAllowBottomDock";
			this.chkAllowBottomDock.Size = new System.Drawing.Size(119, 17);
			this.chkAllowBottomDock.TabIndex = 1;
			this.chkAllowBottomDock.Text = "Allow Bottom Dock";
			this.chkAllowBottomDock.UseVisualStyleBackColor = true;
			this.chkAllowBottomDock.CheckedChanged += new System.EventHandler(this.chkAllowBottomDock_CheckedChanged);
			// 
			// chkAllowLeftDock
			// 
			this.chkAllowLeftDock.Checked = true;
			this.chkAllowLeftDock.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowLeftDock.Location = new System.Drawing.Point(8, 100);
			this.chkAllowLeftDock.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowLeftDock.Name = "chkAllowLeftDock";
			this.chkAllowLeftDock.Size = new System.Drawing.Size(119, 17);
			this.chkAllowLeftDock.TabIndex = 1;
			this.chkAllowLeftDock.Text = "Allow Left Dock";
			this.chkAllowLeftDock.UseVisualStyleBackColor = true;
			this.chkAllowLeftDock.CheckedChanged += new System.EventHandler(this.chkAllowLeftDock_CheckedChanged);
			// 
			// chkAllowRightDock
			// 
			this.chkAllowRightDock.Checked = true;
			this.chkAllowRightDock.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowRightDock.Location = new System.Drawing.Point(8, 121);
			this.chkAllowRightDock.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowRightDock.Name = "chkAllowRightDock";
			this.chkAllowRightDock.Size = new System.Drawing.Size(119, 17);
			this.chkAllowRightDock.TabIndex = 1;
			this.chkAllowRightDock.Text = "Allow Right Dock";
			this.chkAllowRightDock.UseVisualStyleBackColor = true;
			this.chkAllowRightDock.CheckedChanged += new System.EventHandler(this.chkAllowRightDock_CheckedChanged);
			// 
			// chkAllowCenterDock
			// 
			this.chkAllowCenterDock.Location = new System.Drawing.Point(129, 58);
			this.chkAllowCenterDock.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowCenterDock.Name = "chkAllowCenterDock";
			this.chkAllowCenterDock.Size = new System.Drawing.Size(119, 17);
			this.chkAllowCenterDock.TabIndex = 1;
			this.chkAllowCenterDock.Text = "Allow Tab";
			this.chkAllowCenterDock.UseVisualStyleBackColor = true;
			this.chkAllowCenterDock.CheckedChanged += new System.EventHandler(this.chkAllowCenterDock_CheckedChanged);
			// 
			// chkAllowFloat
			// 
			this.chkAllowFloat.Checked = true;
			this.chkAllowFloat.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowFloat.Location = new System.Drawing.Point(129, 100);
			this.chkAllowFloat.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowFloat.Name = "chkAllowFloat";
			this.chkAllowFloat.Size = new System.Drawing.Size(119, 17);
			this.chkAllowFloat.TabIndex = 1;
			this.chkAllowFloat.Text = "Allow Float";
			this.chkAllowFloat.UseVisualStyleBackColor = true;
			this.chkAllowFloat.CheckedChanged += new System.EventHandler(this.chkAllowFloat_CheckedChanged);
			// 
			// ExampleDockableWindow
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.CloseAction = TD.SandDock.DockControlCloseAction.Dispose;
			this.Controls.Add(this.chkAllowFloat);
			this.Controls.Add(this.chkAllowCenterDock);
			this.Controls.Add(this.chkAllowRightDock);
			this.Controls.Add(this.chkAllowLeftDock);
			this.Controls.Add(this.chkAllowBottomDock);
			this.Controls.Add(this.chkAllowTopDock);
			this.Controls.Add(this.label1);
			this.Name = "ExampleDockableWindow";
			this.Padding = new System.Windows.Forms.Padding(4);
			this.Size = new System.Drawing.Size(254, 360);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkAllowTopDock;
		private System.Windows.Forms.CheckBox chkAllowBottomDock;
		private System.Windows.Forms.CheckBox chkAllowLeftDock;
		private System.Windows.Forms.CheckBox chkAllowRightDock;
		private System.Windows.Forms.CheckBox chkAllowCenterDock;
		private System.Windows.Forms.CheckBox chkAllowFloat;
	}
}
