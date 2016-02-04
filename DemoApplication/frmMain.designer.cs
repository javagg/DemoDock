namespace DemoApplication
{
	partial class frmMain
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			TD.SandDock.DocumentContainer documentContainer1;
			TD.SandDock.DockContainer dockContainer3;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Visual Studio 2003");
			System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Luna Blue");
			System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Luna Silver");
			System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Luna Olive");
			System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Classic");
			System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Office 2003", new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20});
			System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Luna Blue");
			System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Luna Silver");
			System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Luna Olive");
			System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Classic");
			System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Visual Studio 2005", new System.Windows.Forms.TreeNode[] {
            treeNode22,
            treeNode23,
            treeNode24,
            treeNode25});
			System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Blue");
			System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Black");
			System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Silver");
			System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Office 2007", new System.Windows.Forms.TreeNode[] {
            treeNode27,
            treeNode28,
            treeNode29});
			this.tdWelcome = new TD.SandDock.TabbedDocument();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.sandDockManager1 = new TD.SandDock.SandDockManager();
			this.panel1 = new System.Windows.Forms.Panel();
			this.dwToolbox = new TD.SandDock.DockableWindow();
			this.lvwTools = new System.Windows.Forms.ListView();
			this.ilTools = new System.Windows.Forms.ImageList(this.components);
			this.dockContainer2 = new TD.SandDock.DockContainer();
			this.dwOutput = new TD.SandDock.DockableWindow();
			this.txtEventLog = new System.Windows.Forms.TextBox();
			this.dwTaskList = new TD.SandDock.DockableWindow();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.dockContainer1 = new TD.SandDock.DockContainer();
			this.dwAppearance = new TD.SandDock.DockableWindow();
			this.tvwRenderer = new System.Windows.Forms.TreeView();
			this.dwNewWindows = new TD.SandDock.DockableWindow();
			this.dwBehavior = new TD.SandDock.DockableWindow();
			this.chkIntegralClose = new System.Windows.Forms.CheckBox();
			this.chkAllowOptions = new System.Windows.Forms.CheckBox();
			this.chkShowActiveFilesList = new System.Windows.Forms.CheckBox();
			this.chkAllowDockContainerResize = new System.Windows.Forms.CheckBox();
			this.chkAllowClose = new System.Windows.Forms.CheckBox();
			this.chkAllowPin = new System.Windows.Forms.CheckBox();
			this.dwClassView = new TD.SandDock.DockableWindow();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.appearanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.taskListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.classViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.behaviorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ctxWindow = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuWindowNewHorizontalTabGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWindowNewVerticalTabGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWindowFloat = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWindowDock = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWindowAutoHide = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuWindowClose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.btnNew = new System.Windows.Forms.ToolStripButton();
			this.btnSaveLayout = new System.Windows.Forms.ToolStripButton();
			this.btnLoadLayout = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnFloatAll = new System.Windows.Forms.ToolStripButton();
			documentContainer1 = new TD.SandDock.DocumentContainer();
			dockContainer3 = new TD.SandDock.DockContainer();
			documentContainer1.SuspendLayout();
			this.tdWelcome.SuspendLayout();
			this.panel1.SuspendLayout();
			dockContainer3.SuspendLayout();
			this.dwToolbox.SuspendLayout();
			this.dockContainer2.SuspendLayout();
			this.dwOutput.SuspendLayout();
			this.dwTaskList.SuspendLayout();
			this.dockContainer1.SuspendLayout();
			this.dwAppearance.SuspendLayout();
			this.dwBehavior.SuspendLayout();
			this.dwClassView.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.ctxWindow.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// documentContainer1
			// 
			documentContainer1.ContentSize = 329;
			documentContainer1.Controls.Add(this.tdWelcome);
			documentContainer1.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(534F, 329F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.DocumentLayoutSystem(new System.Drawing.SizeF(534F, 329F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.tdWelcome))}, this.tdWelcome)))});
			documentContainer1.Location = new System.Drawing.Point(112, 0);
			documentContainer1.Manager = this.sandDockManager1;
			documentContainer1.Name = "documentContainer1";
			documentContainer1.Size = new System.Drawing.Size(540, 354);
			documentContainer1.TabIndex = 0;
			// 
			// tdWelcome
			// 
			this.tdWelcome.BackColor = System.Drawing.SystemColors.Window;
			this.tdWelcome.Controls.Add(this.richTextBox1);
			this.tdWelcome.FloatingSize = new System.Drawing.Size(550, 400);
			this.tdWelcome.Guid = new System.Guid("a391a42f-b7f0-4773-9d75-04f2c86636d9");
			this.tdWelcome.Location = new System.Drawing.Point(1, 21);
			this.tdWelcome.Name = "tdWelcome";
			this.tdWelcome.Padding = new System.Windows.Forms.Padding(8);
			this.tdWelcome.Size = new System.Drawing.Size(538, 332);
			this.tdWelcome.TabIndex = 0;
			this.tdWelcome.Text = "Welcome";
			// 
			// richTextBox1
			// 
			this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Location = new System.Drawing.Point(8, 8);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(522, 316);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "Welcome to SandDock";
			// 
			// sandDockManager1
			// 
			this.sandDockManager1.DockSystemContainer = this.panel1;
			this.sandDockManager1.OwnerForm = this;
			this.sandDockManager1.SelectTabsOnDrag = true;
			this.sandDockManager1.DockingStarted += new System.EventHandler(this.sandDockManager1_DockingStarted);
			this.sandDockManager1.ActiveTabbedDocumentChanged += new System.EventHandler(this.sandDockManager1_ActiveTabbedDocumentChanged);
			this.sandDockManager1.ResolveDockControl += new TD.SandDock.ResolveDockControlEventHandler(this.sandDockManager1_ResolveDockControl);
			this.sandDockManager1.DockingFinished += new System.EventHandler(this.sandDockManager1_DockingFinished);
			this.sandDockManager1.DockControlRemoved += new TD.SandDock.DockControlEventHandler(this.sandDockManager1_DockControlRemoved);
			this.sandDockManager1.DockControlActivated += new TD.SandDock.DockControlEventHandler(this.sandDockManager1_DockControlActivated);
			this.sandDockManager1.ShowControlContextMenu += new TD.SandDock.ShowControlContextMenuEventHandler(this.sandDockManager1_ShowControlContextMenu);
			this.sandDockManager1.ShowActiveFilesList += new TD.SandDock.ActiveFilesListEventHandler(this.sandDockManager1_ShowActiveFilesList);
			this.sandDockManager1.DockControlClosing += new TD.SandDock.DockControlClosingEventHandler(this.sandDockManager1_DockControlClosing);
			this.sandDockManager1.DockControlAdded += new TD.SandDock.DockControlEventHandler(this.sandDockManager1_DockControlAdded);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(documentContainer1);
			this.panel1.Controls.Add(dockContainer3);
			this.panel1.Controls.Add(this.dockContainer2);
			this.panel1.Controls.Add(this.dockContainer1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 49);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(906, 515);
			this.panel1.TabIndex = 13;
			// 
			// dockContainer3
			// 
			dockContainer3.ContentSize = 108;
			dockContainer3.Controls.Add(this.dwToolbox);
			dockContainer3.Dock = System.Windows.Forms.DockStyle.Left;
			dockContainer3.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(108F, 331F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(108F, 331F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dwToolbox))}, this.dwToolbox)))});
			dockContainer3.Location = new System.Drawing.Point(0, 0);
			dockContainer3.Manager = this.sandDockManager1;
			dockContainer3.Name = "dockContainer3";
			dockContainer3.Size = new System.Drawing.Size(112, 354);
			dockContainer3.TabIndex = 12;
			// 
			// dwToolbox
			// 
			this.dwToolbox.BackColor = System.Drawing.SystemColors.Window;
			this.dwToolbox.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dwToolbox.Collapsed = true;
			this.dwToolbox.Controls.Add(this.lvwTools);
			this.dwToolbox.Guid = new System.Guid("e99cb778-d5f6-4b28-89dd-7e81982cfb27");
			this.dwToolbox.Location = new System.Drawing.Point(0, 18);
			this.dwToolbox.Name = "dwToolbox";
			this.dwToolbox.Padding = new System.Windows.Forms.Padding(4);
			this.dwToolbox.Size = new System.Drawing.Size(108, 312);
			this.dwToolbox.TabImage = ((System.Drawing.Image)(resources.GetObject("dwToolbox.TabImage")));
			this.dwToolbox.TabIndex = 1;
			this.dwToolbox.Text = "Toolbox";
			// 
			// lvwTools
			// 
			this.lvwTools.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvwTools.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwTools.Location = new System.Drawing.Point(5, 5);
			this.lvwTools.MultiSelect = false;
			this.lvwTools.Name = "lvwTools";
			this.lvwTools.Scrollable = false;
			this.lvwTools.Size = new System.Drawing.Size(98, 302);
			this.lvwTools.SmallImageList = this.ilTools;
			this.lvwTools.TabIndex = 0;
			this.lvwTools.UseCompatibleStateImageBehavior = false;
			this.lvwTools.View = System.Windows.Forms.View.List;
			// 
			// ilTools
			// 
			this.ilTools.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ilTools.ImageSize = new System.Drawing.Size(16, 16);
			this.ilTools.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// dockContainer2
			// 
			this.dockContainer2.ContentSize = 157;
			this.dockContainer2.Controls.Add(this.dwOutput);
			this.dockContainer2.Controls.Add(this.dwTaskList);
			this.dockContainer2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dockContainer2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dockContainer2.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(648F, 170F), System.Windows.Forms.Orientation.Vertical, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(385F, 170F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dwOutput)),
                        ((TD.SandDock.DockControl)(this.dwTaskList))}, this.dwOutput)))});
			this.dockContainer2.Location = new System.Drawing.Point(0, 354);
			this.dockContainer2.Manager = this.sandDockManager1;
			this.dockContainer2.Name = "dockContainer2";
			this.dockContainer2.Size = new System.Drawing.Size(652, 161);
			this.dockContainer2.TabIndex = 4;
			// 
			// dwOutput
			// 
			this.dwOutput.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dwOutput.Controls.Add(this.txtEventLog);
			this.dwOutput.Guid = new System.Guid("ca8fa1d3-5e9c-463b-b815-0fda4ca1ebea");
			this.dwOutput.Location = new System.Drawing.Point(0, 22);
			this.dwOutput.Name = "dwOutput";
			this.dwOutput.Size = new System.Drawing.Size(652, 115);
			this.dwOutput.TabImage = ((System.Drawing.Image)(resources.GetObject("dwOutput.TabImage")));
			this.dwOutput.TabIndex = 1;
			this.dwOutput.Text = "Events";
			// 
			// txtEventLog
			// 
			this.txtEventLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtEventLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtEventLog.Location = new System.Drawing.Point(1, 1);
			this.txtEventLog.Multiline = true;
			this.txtEventLog.Name = "txtEventLog";
			this.txtEventLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtEventLog.Size = new System.Drawing.Size(650, 113);
			this.txtEventLog.TabIndex = 0;
			// 
			// dwTaskList
			// 
			this.dwTaskList.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dwTaskList.Controls.Add(this.textBox1);
			this.dwTaskList.Guid = new System.Guid("88d3b582-f745-4dd4-b522-3910a141e136");
			this.dwTaskList.Location = new System.Drawing.Point(0, 0);
			this.dwTaskList.Name = "dwTaskList";
			this.dwTaskList.Size = new System.Drawing.Size(648, 128);
			this.dwTaskList.TabImage = ((System.Drawing.Image)(resources.GetObject("dwTaskList.TabImage")));
			this.dwTaskList.TabIndex = 2;
			this.dwTaskList.Text = "Task List";
			// 
			// textBox1
			// 
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(1, 1);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(646, 126);
			this.textBox1.TabIndex = 1;
			// 
			// dockContainer1
			// 
			this.dockContainer1.ContentSize = 250;
			this.dockContainer1.Controls.Add(this.dwAppearance);
			this.dockContainer1.Controls.Add(this.dwNewWindows);
			this.dockContainer1.Controls.Add(this.dwBehavior);
			this.dockContainer1.Controls.Add(this.dwClassView);
			this.dockContainer1.Dock = System.Windows.Forms.DockStyle.Right;
			this.dockContainer1.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 505F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(250F, 246.491F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dwAppearance)),
                        ((TD.SandDock.DockControl)(this.dwNewWindows))}, this.dwAppearance))),
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(250F, 255.509F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dwBehavior)),
                        ((TD.SandDock.DockControl)(this.dwClassView))}, this.dwBehavior)))});
			this.dockContainer1.Location = new System.Drawing.Point(652, 0);
			this.dockContainer1.Manager = this.sandDockManager1;
			this.dockContainer1.Name = "dockContainer1";
			this.dockContainer1.Size = new System.Drawing.Size(254, 515);
			this.dockContainer1.TabIndex = 9;
			// 
			// dwAppearance
			// 
			this.dwAppearance.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dwAppearance.Controls.Add(this.tvwRenderer);
			this.dwAppearance.Guid = new System.Guid("40a3d4f5-35eb-4b2a-acec-c3ed52b8f061");
			this.dwAppearance.Location = new System.Drawing.Point(4, 18);
			this.dwAppearance.Name = "dwAppearance";
			this.dwAppearance.Size = new System.Drawing.Size(250, 209);
			this.dwAppearance.TabImage = ((System.Drawing.Image)(resources.GetObject("dwAppearance.TabImage")));
			this.dwAppearance.TabIndex = 0;
			this.dwAppearance.Text = "Theme";
			// 
			// tvwRenderer
			// 
			this.tvwRenderer.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tvwRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwRenderer.HideSelection = false;
			this.tvwRenderer.Location = new System.Drawing.Point(1, 1);
			this.tvwRenderer.Name = "tvwRenderer";
			treeNode16.Name = "Node0";
			treeNode16.Text = "Visual Studio 2003";
			treeNode17.Name = "Node11";
			treeNode17.Text = "Luna Blue";
			treeNode18.Name = "Node12";
			treeNode18.Text = "Luna Silver";
			treeNode19.Name = "Node13";
			treeNode19.Text = "Luna Olive";
			treeNode20.Name = "Node14";
			treeNode20.Text = "Classic";
			treeNode21.Name = "Node1";
			treeNode21.Text = "Office 2003";
			treeNode22.Name = "Node15";
			treeNode22.Text = "Luna Blue";
			treeNode23.Name = "Node16";
			treeNode23.Text = "Luna Silver";
			treeNode24.Name = "Node17";
			treeNode24.Text = "Luna Olive";
			treeNode25.Name = "Node18";
			treeNode25.Text = "Classic";
			treeNode26.Name = "Node2";
			treeNode26.Text = "Visual Studio 2005";
			treeNode27.Name = "Node4";
			treeNode27.Text = "Blue";
			treeNode28.Name = "Node5";
			treeNode28.Text = "Black";
			treeNode29.Name = "Node6";
			treeNode29.Text = "Silver";
			treeNode30.Name = "Node3";
			treeNode30.Text = "Office 2007";
			this.tvwRenderer.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode16,
            treeNode21,
            treeNode26,
            treeNode30});
			this.tvwRenderer.Size = new System.Drawing.Size(248, 207);
			this.tvwRenderer.TabIndex = 4;
			this.tvwRenderer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwRenderer_AfterSelect);
			// 
			// dwNewWindows
			// 
			this.dwNewWindows.BackColor = System.Drawing.SystemColors.Window;
			this.dwNewWindows.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dwNewWindows.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dwNewWindows.Guid = new System.Guid("1c062ea5-0deb-4b2e-a36a-1564e1520d5f");
			this.dwNewWindows.Location = new System.Drawing.Point(0, 0);
			this.dwNewWindows.Name = "dwNewWindows";
			this.dwNewWindows.Size = new System.Drawing.Size(250, 192);
			this.dwNewWindows.TabImage = ((System.Drawing.Image)(resources.GetObject("dwNewWindows.TabImage")));
			this.dwNewWindows.TabIndex = 1;
			this.dwNewWindows.Text = "Properties";
			// 
			// dwBehavior
			// 
			this.dwBehavior.BackColor = System.Drawing.SystemColors.Window;
			this.dwBehavior.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dwBehavior.Controls.Add(this.chkIntegralClose);
			this.dwBehavior.Controls.Add(this.chkAllowOptions);
			this.dwBehavior.Controls.Add(this.chkShowActiveFilesList);
			this.dwBehavior.Controls.Add(this.chkAllowDockContainerResize);
			this.dwBehavior.Controls.Add(this.chkAllowClose);
			this.dwBehavior.Controls.Add(this.chkAllowPin);
			this.dwBehavior.Guid = new System.Guid("6144385b-ee41-4bff-814d-2ba7d497d492");
			this.dwBehavior.Location = new System.Drawing.Point(4, 273);
			this.dwBehavior.Name = "dwBehavior";
			this.dwBehavior.Size = new System.Drawing.Size(250, 218);
			this.dwBehavior.TabImage = ((System.Drawing.Image)(resources.GetObject("dwBehavior.TabImage")));
			this.dwBehavior.TabIndex = 2;
			this.dwBehavior.Text = "Behavior";
			// 
			// chkIntegralClose
			// 
			this.chkIntegralClose.AutoSize = true;
			this.chkIntegralClose.Location = new System.Drawing.Point(12, 132);
			this.chkIntegralClose.Name = "chkIntegralClose";
			this.chkIntegralClose.Size = new System.Drawing.Size(154, 17);
			this.chkIntegralClose.TabIndex = 3;
			this.chkIntegralClose.Text = "Integral Tab Close Buttons";
			this.chkIntegralClose.CheckedChanged += new System.EventHandler(this.chkIntegralClose_CheckedChanged);
			// 
			// chkAllowOptions
			// 
			this.chkAllowOptions.Checked = true;
			this.chkAllowOptions.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowOptions.Location = new System.Drawing.Point(12, 11);
			this.chkAllowOptions.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowOptions.Name = "chkAllowOptions";
			this.chkAllowOptions.Size = new System.Drawing.Size(160, 18);
			this.chkAllowOptions.TabIndex = 3;
			this.chkAllowOptions.Text = "Allow Options Menu";
			this.chkAllowOptions.CheckedChanged += new System.EventHandler(this.chkAllowOptions_CheckedChanged);
			// 
			// chkShowActiveFilesList
			// 
			this.chkShowActiveFilesList.AutoSize = true;
			this.chkShowActiveFilesList.Location = new System.Drawing.Point(12, 109);
			this.chkShowActiveFilesList.Name = "chkShowActiveFilesList";
			this.chkShowActiveFilesList.Size = new System.Drawing.Size(128, 17);
			this.chkShowActiveFilesList.TabIndex = 3;
			this.chkShowActiveFilesList.Text = "Show Active Files List";
			this.chkShowActiveFilesList.CheckedChanged += new System.EventHandler(this.chkShowActiveFilesList_CheckedChanged);
			// 
			// chkAllowDockContainerResize
			// 
			this.chkAllowDockContainerResize.Checked = true;
			this.chkAllowDockContainerResize.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowDockContainerResize.Location = new System.Drawing.Point(12, 77);
			this.chkAllowDockContainerResize.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowDockContainerResize.Name = "chkAllowDockContainerResize";
			this.chkAllowDockContainerResize.Size = new System.Drawing.Size(207, 18);
			this.chkAllowDockContainerResize.TabIndex = 3;
			this.chkAllowDockContainerResize.Text = "Allow Resizing of Docked Windows";
			this.chkAllowDockContainerResize.CheckedChanged += new System.EventHandler(this.chkAllowDockContainerResize_CheckedChanged);
			// 
			// chkAllowClose
			// 
			this.chkAllowClose.Checked = true;
			this.chkAllowClose.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowClose.Location = new System.Drawing.Point(12, 55);
			this.chkAllowClose.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowClose.Name = "chkAllowClose";
			this.chkAllowClose.Size = new System.Drawing.Size(160, 18);
			this.chkAllowClose.TabIndex = 3;
			this.chkAllowClose.Text = "Allow Closing of Windows";
			this.chkAllowClose.CheckedChanged += new System.EventHandler(this.chkAllowClose_CheckedChanged);
			// 
			// chkAllowPin
			// 
			this.chkAllowPin.Checked = true;
			this.chkAllowPin.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowPin.Location = new System.Drawing.Point(12, 33);
			this.chkAllowPin.Margin = new System.Windows.Forms.Padding(2);
			this.chkAllowPin.Name = "chkAllowPin";
			this.chkAllowPin.Size = new System.Drawing.Size(160, 18);
			this.chkAllowPin.TabIndex = 3;
			this.chkAllowPin.Text = "Allow Pinning of Windows";
			this.chkAllowPin.CheckedChanged += new System.EventHandler(this.chkAllowPin_CheckedChanged);
			// 
			// dwClassView
			// 
			this.dwClassView.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dwClassView.Controls.Add(this.textBox2);
			this.dwClassView.Guid = new System.Guid("2f4de550-af67-4685-8728-5087e47eff30");
			this.dwClassView.Location = new System.Drawing.Point(0, 0);
			this.dwClassView.Name = "dwClassView";
			this.dwClassView.Size = new System.Drawing.Size(250, 200);
			this.dwClassView.TabImage = ((System.Drawing.Image)(resources.GetObject("dwClassView.TabImage")));
			this.dwClassView.TabIndex = 3;
			this.dwClassView.Text = "Class View";
			// 
			// textBox2
			// 
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox2.Location = new System.Drawing.Point(1, 1);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox2.Size = new System.Drawing.Size(248, 198);
			this.textBox2.TabIndex = 1;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 564);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(906, 22);
			this.statusBar1.TabIndex = 8;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.menuStrip1.Size = new System.Drawing.Size(906, 24);
			this.menuStrip1.TabIndex = 11;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.mnuFileExit_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolboxToolStripMenuItem,
            this.appearanceToolStripMenuItem,
            this.outputToolStripMenuItem,
            this.taskListToolStripMenuItem,
            this.classViewToolStripMenuItem,
            this.behaviorToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// toolboxToolStripMenuItem
			// 
			this.toolboxToolStripMenuItem.Name = "toolboxToolStripMenuItem";
			this.toolboxToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.X)));
			this.toolboxToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.toolboxToolStripMenuItem.Tag = "e99cb778-d5f6-4b28-89dd-7e81982cfb27";
			this.toolboxToolStripMenuItem.Text = "Toolbo&x";
			this.toolboxToolStripMenuItem.Click += new System.EventHandler(this.OnViewWindowItemClick);
			// 
			// appearanceToolStripMenuItem
			// 
			this.appearanceToolStripMenuItem.Name = "appearanceToolStripMenuItem";
			this.appearanceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.T)));
			this.appearanceToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.appearanceToolStripMenuItem.Tag = "40a3d4f5-35eb-4b2a-acec-c3ed52b8f061";
			this.appearanceToolStripMenuItem.Text = "&Theme";
			this.appearanceToolStripMenuItem.Click += new System.EventHandler(this.OnViewWindowItemClick);
			// 
			// outputToolStripMenuItem
			// 
			this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
			this.outputToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.E)));
			this.outputToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.outputToolStripMenuItem.Tag = "ca8fa1d3-5e9c-463b-b815-0fda4ca1ebea";
			this.outputToolStripMenuItem.Text = "&Events";
			this.outputToolStripMenuItem.Click += new System.EventHandler(this.OnViewWindowItemClick);
			// 
			// taskListToolStripMenuItem
			// 
			this.taskListToolStripMenuItem.Name = "taskListToolStripMenuItem";
			this.taskListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.K)));
			this.taskListToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.taskListToolStripMenuItem.Tag = "88d3b582-f745-4dd4-b522-3910a141e136";
			this.taskListToolStripMenuItem.Text = "Tas&k List";
			this.taskListToolStripMenuItem.Click += new System.EventHandler(this.OnViewWindowItemClick);
			// 
			// classViewToolStripMenuItem
			// 
			this.classViewToolStripMenuItem.Name = "classViewToolStripMenuItem";
			this.classViewToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.C)));
			this.classViewToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.classViewToolStripMenuItem.Tag = "2f4de550-af67-4685-8728-5087e47eff30";
			this.classViewToolStripMenuItem.Text = "&Class View";
			this.classViewToolStripMenuItem.Click += new System.EventHandler(this.OnViewWindowItemClick);
			// 
			// behaviorToolStripMenuItem
			// 
			this.behaviorToolStripMenuItem.Name = "behaviorToolStripMenuItem";
			this.behaviorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.B)));
			this.behaviorToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.behaviorToolStripMenuItem.Tag = "6144385b-ee41-4bff-814d-2ba7d497d492";
			this.behaviorToolStripMenuItem.Text = "&Behavior";
			this.behaviorToolStripMenuItem.Click += new System.EventHandler(this.OnViewWindowItemClick);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
			this.aboutToolStripMenuItem.Text = "&About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.mnuHelpAbout_Click);
			// 
			// ctxWindow
			// 
			this.ctxWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWindowNewHorizontalTabGroup,
            this.mnuWindowNewVerticalTabGroup,
            this.mnuWindowFloat,
            this.mnuWindowDock,
            this.mnuWindowAutoHide,
            this.toolStripMenuItem1,
            this.mnuWindowClose});
			this.ctxWindow.Name = "ctxWindow";
			this.ctxWindow.Size = new System.Drawing.Size(211, 142);
			this.ctxWindow.Opening += new System.ComponentModel.CancelEventHandler(this.ctxWindow_Opening);
			// 
			// mnuWindowNewHorizontalTabGroup
			// 
			this.mnuWindowNewHorizontalTabGroup.Image = ((System.Drawing.Image)(resources.GetObject("mnuWindowNewHorizontalTabGroup.Image")));
			this.mnuWindowNewHorizontalTabGroup.Name = "mnuWindowNewHorizontalTabGroup";
			this.mnuWindowNewHorizontalTabGroup.Size = new System.Drawing.Size(210, 22);
			this.mnuWindowNewHorizontalTabGroup.Text = "New Hori&zontal Tab Group";
			this.mnuWindowNewHorizontalTabGroup.Click += new System.EventHandler(this.mnuWindowNewHorizontalTabGroup_Click);
			// 
			// mnuWindowNewVerticalTabGroup
			// 
			this.mnuWindowNewVerticalTabGroup.Image = ((System.Drawing.Image)(resources.GetObject("mnuWindowNewVerticalTabGroup.Image")));
			this.mnuWindowNewVerticalTabGroup.Name = "mnuWindowNewVerticalTabGroup";
			this.mnuWindowNewVerticalTabGroup.Size = new System.Drawing.Size(210, 22);
			this.mnuWindowNewVerticalTabGroup.Text = "New &Vertical Tab Group";
			this.mnuWindowNewVerticalTabGroup.Click += new System.EventHandler(this.mnuWindowNewVerticalTabGroup_Click);
			// 
			// mnuWindowFloat
			// 
			this.mnuWindowFloat.Name = "mnuWindowFloat";
			this.mnuWindowFloat.Size = new System.Drawing.Size(210, 22);
			this.mnuWindowFloat.Text = "&Float";
			this.mnuWindowFloat.Click += new System.EventHandler(this.mnuWindowFloat_Click);
			// 
			// mnuWindowDock
			// 
			this.mnuWindowDock.Name = "mnuWindowDock";
			this.mnuWindowDock.Size = new System.Drawing.Size(210, 22);
			this.mnuWindowDock.Text = "&Dock";
			this.mnuWindowDock.Click += new System.EventHandler(this.mnuWindowDock_Click);
			// 
			// mnuWindowAutoHide
			// 
			this.mnuWindowAutoHide.Name = "mnuWindowAutoHide";
			this.mnuWindowAutoHide.Size = new System.Drawing.Size(210, 22);
			this.mnuWindowAutoHide.Text = "&Auto Hide";
			this.mnuWindowAutoHide.Click += new System.EventHandler(this.mnuWindowAutoHide_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(207, 6);
			// 
			// mnuWindowClose
			// 
			this.mnuWindowClose.Name = "mnuWindowClose";
			this.mnuWindowClose.Size = new System.Drawing.Size(210, 22);
			this.mnuWindowClose.Text = "&Close";
			this.mnuWindowClose.Click += new System.EventHandler(this.mnuWindowClose_Click);
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnSaveLayout,
            this.btnLoadLayout,
            this.toolStripSeparator1,
            this.btnFloatAll});
			this.toolStrip2.Location = new System.Drawing.Point(0, 24);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(906, 25);
			this.toolStrip2.TabIndex = 14;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// btnNew
			// 
			this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
			this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(23, 22);
			this.btnNew.Text = "toolStripButton1";
			this.btnNew.ToolTipText = "New Document";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnSaveLayout
			// 
			this.btnSaveLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSaveLayout.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveLayout.Image")));
			this.btnSaveLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveLayout.Name = "btnSaveLayout";
			this.btnSaveLayout.Size = new System.Drawing.Size(23, 22);
			this.btnSaveLayout.Text = "toolStripButton1";
			this.btnSaveLayout.ToolTipText = "Save Layout";
			this.btnSaveLayout.Click += new System.EventHandler(this.btnSaveLayout_Click);
			// 
			// btnLoadLayout
			// 
			this.btnLoadLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnLoadLayout.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadLayout.Image")));
			this.btnLoadLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnLoadLayout.Name = "btnLoadLayout";
			this.btnLoadLayout.Size = new System.Drawing.Size(23, 22);
			this.btnLoadLayout.Text = "toolStripButton2";
			this.btnLoadLayout.ToolTipText = "Load Layout";
			this.btnLoadLayout.Click += new System.EventHandler(this.btnLoadLayout_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnFloatAll
			// 
			this.btnFloatAll.Image = ((System.Drawing.Image)(resources.GetObject("btnFloatAll.Image")));
			this.btnFloatAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnFloatAll.Name = "btnFloatAll";
			this.btnFloatAll.Size = new System.Drawing.Size(65, 22);
			this.btnFloatAll.Text = "Float All";
			this.btnFloatAll.Click += new System.EventHandler(this.btnFloatWindows_Click);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.ClientSize = new System.Drawing.Size(906, 586);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SandDock Demonstration Application";
			this.Load += new System.EventHandler(this.Form2_Load);
			documentContainer1.ResumeLayout(false);
			this.tdWelcome.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			dockContainer3.ResumeLayout(false);
			this.dwToolbox.ResumeLayout(false);
			this.dockContainer2.ResumeLayout(false);
			this.dwOutput.ResumeLayout(false);
			this.dwOutput.PerformLayout();
			this.dwTaskList.ResumeLayout(false);
			this.dwTaskList.PerformLayout();
			this.dockContainer1.ResumeLayout(false);
			this.dwAppearance.ResumeLayout(false);
			this.dwBehavior.ResumeLayout(false);
			this.dwBehavior.PerformLayout();
			this.dwClassView.ResumeLayout(false);
			this.dwClassView.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ctxWindow.ResumeLayout(false);
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.StatusBar statusBar1;
		private TD.SandDock.SandDockManager sandDockManager1;
		private TD.SandDock.DockContainer dockContainer2;
		private System.Windows.Forms.TextBox txtEventLog;
		private TD.SandDock.DockContainer dockContainer1;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private TD.SandDock.DockableWindow dwAppearance;
		private TD.SandDock.DockableWindow dwTaskList;
		private TD.SandDock.DockableWindow dwOutput;
		private TD.SandDock.DockableWindow dwToolbox;
		private TD.SandDock.DockableWindow dwClassView;
		private TD.SandDock.DockableWindow dwBehavior;
		private TD.SandDock.TabbedDocument tdWelcome;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolboxToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem appearanceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem taskListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem classViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.CheckBox chkAllowPin;
		private System.Windows.Forms.CheckBox chkAllowClose;
		private System.Windows.Forms.CheckBox chkAllowOptions;
		private System.Windows.Forms.CheckBox chkAllowDockContainerResize;
		private System.Windows.Forms.ListView lvwTools;
		private System.Windows.Forms.ImageList ilTools;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.ToolStripMenuItem behaviorToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip ctxWindow;
		private System.Windows.Forms.ToolStripMenuItem mnuWindowNewHorizontalTabGroup;
		private System.Windows.Forms.ToolStripMenuItem mnuWindowNewVerticalTabGroup;
		private System.Windows.Forms.ToolStripMenuItem mnuWindowFloat;
		private System.Windows.Forms.ToolStripMenuItem mnuWindowDock;
		private System.Windows.Forms.ToolStripMenuItem mnuWindowAutoHide;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuWindowClose;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox chkShowActiveFilesList;
		private System.Windows.Forms.CheckBox chkIntegralClose;
		private System.Windows.Forms.TreeView tvwRenderer;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton btnNew;
		private TD.SandDock.DockableWindow dwNewWindows;
		private System.Windows.Forms.ToolStripButton btnSaveLayout;
		private System.Windows.Forms.ToolStripButton btnLoadLayout;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnFloatAll;
	}
}