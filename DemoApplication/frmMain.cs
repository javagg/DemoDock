using System;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using TD.SandDock;
using TD.SandDock.Rendering;
using BorderStyle = TD.SandDock.Rendering.BorderStyle;
using TabControl = System.Windows.Forms.TabControl;

namespace DemoApplication
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public partial class FrmMain : Form
	{
		// Members
		private DockControl lastActivatedWindow;
		private string windowLayout;
		
		public FrmMain()
		{
			InitializeComponent();
		}

		#region Event Logging

		private void sandDockManager1_DockControlAdded(object sender, DockControlEventArgs e)
		{
			AppendLog("DockControl Added: " + e.DockControl.Text);

			e.DockControl.AutoHidePopupOpened += OnDockControlAutoHidePopupOpened;
			e.DockControl.AutoHidePopupClosed += OnDockControlAutoHidePopupClosed;
			e.DockControl.Closed += OnDockControlClosed;
			e.DockControl.Load += OnDockControlLoad;
			e.DockControl.DockSituationChanged += OnDockControlDockSituationChanged;
		}

		private void sandDockManager1_DockControlRemoved(object sender, DockControlEventArgs e)
		{
			AppendLog("DockControl Removed: " + e.DockControl.Text);
		}

		private void AppendLog(string text)
		{
			if (!txtEventLog.IsDisposed)
			{
				txtEventLog.SelectionStart = txtEventLog.Text.Length;
				txtEventLog.SelectedText = DateTime.Now.ToString("HH:mm:ss") + ": " + text + Environment.NewLine;
			}
		}

		private void OnDockControlAutoHidePopupOpened(object sender, EventArgs e)
		{
			DockControl control = (DockControl)sender;
			AppendLog("AutoHide Popup Opened: " + control.Text);
		}

		private void OnDockControlAutoHidePopupClosed(object sender, EventArgs e)
		{
			DockControl control = (DockControl)sender;
			AppendLog("AutoHide Popup Closed: " + control.Text);
		}

		private void sandDockManager1_ResolveDockControl(object sender, ResolveDockControlEventArgs e)
		{
			AppendLog("Resolve DockControl: " + e.Guid);
		}

		private void OnDockControlClosed(object sender, EventArgs e)
		{
			AppendLog("Closed: " + ((Control)sender).Text);
		}

		private void OnDockControlLoad(object sender, EventArgs e)
		{
			AppendLog("Loaded: " + ((Control)sender).Text);
		}

		private void OnDockControlDockSituationChanged(object sender, EventArgs e)
		{
			AppendLog("Dock Situation Changed: " + ((Control)sender).Text);
		}

		private void sandDockManager1_DockControlActivated(object sender, DockControlEventArgs e)
		{
			AppendLog("Activated: " + e.DockControl.Text);

			lastActivatedWindow = e.DockControl;
		}

		private void sandDockManager1_ActiveTabbedDocumentChanged(object sender, EventArgs e)
		{
			DockControl document = sandDockManager1.ActiveTabbedDocument;
			string text = document != null ? document.Text : "(none)";
			AppendLog("Active Tabbed Document: " + text);
		}

		private void sandDockManager1_DockControlClosing(object sender, DockControlClosingEventArgs e)
		{
			// If this is a tabbed document, prompt to save changes
			if (e.DockControl is TabbedDocument)
			{
				DialogResult result = MessageBox.Show(this, "Do you wish to save changes made to " + e.DockControl.Text + "?", "Close Document", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
				if (result == DialogResult.Cancel)
					e.Cancel = true;
			}
		}

		#endregion

		private void sandDockManager1_ShowControlContextMenu(object sender, ShowControlContextMenuEventArgs e)
		{
			ctxWindow.Show(e.DockControl, e.Position);
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			// Get RTF text for the new child
			Stream stream = GetType().Assembly.GetManifestResourceStream("DemoApplication.welcome.rtf");
			StreamReader reader = new StreamReader(stream);
			string rtf = reader.ReadToEnd();
			reader.Close();
			stream.Close();
			richTextBox1.Rtf = rtf;

			// Fill the renderer treeview with renderer instances ready to be used
			tvwRenderer.Nodes[0].Tag = new EverettRenderer();
			tvwRenderer.Nodes[1].Tag = new Office2003Renderer();
			tvwRenderer.Nodes[1].Nodes[0].Tag = new Office2003Renderer(WindowsColorScheme.LunaBlue);
			tvwRenderer.Nodes[1].Nodes[1].Tag = new Office2003Renderer(WindowsColorScheme.LunaSilver);
			tvwRenderer.Nodes[1].Nodes[2].Tag = new Office2003Renderer(WindowsColorScheme.LunaOlive);
			tvwRenderer.Nodes[1].Nodes[3].Tag = new Office2003Renderer(WindowsColorScheme.Standard);
			tvwRenderer.Nodes[2].Tag = new WhidbeyRenderer();
			tvwRenderer.Nodes[2].Nodes[0].Tag = new WhidbeyRenderer(WindowsColorScheme.LunaBlue);
			tvwRenderer.Nodes[2].Nodes[1].Tag = new WhidbeyRenderer(WindowsColorScheme.LunaSilver);
			tvwRenderer.Nodes[2].Nodes[2].Tag = new WhidbeyRenderer(WindowsColorScheme.LunaOlive);
			tvwRenderer.Nodes[2].Nodes[3].Tag = new WhidbeyRenderer(WindowsColorScheme.Standard);
			tvwRenderer.Nodes[3].Tag = new Office2007Renderer();
			tvwRenderer.Nodes[3].Nodes[0].Tag = new Office2007Renderer(Office2007ColorScheme.Blue);
			tvwRenderer.Nodes[3].Nodes[1].Tag = new Office2007Renderer(Office2007ColorScheme.Black);
			tvwRenderer.Nodes[3].Nodes[2].Tag = new Office2007Renderer(Office2007ColorScheme.Silver);
			tvwRenderer.Nodes[2].Expand();
			tvwRenderer.Nodes[3].Expand();
			tvwRenderer.SelectedNode = tvwRenderer.Nodes[3];

			PopulateToolbox();
			windowLayout = sandDockManager1.GetLayout();
		}

		private void sandDockManager1_DockingStarted(object sender, EventArgs e)
		{
			AppendLog("Docking Started");

			statusBar1.Text = "Hold down CTRL to prevent docking. Point to title bar of destination window to tab link.";
		}

		private void sandDockManager1_DockingFinished(object sender, EventArgs e)
		{
			AppendLog("Docking Finished");

			statusBar1.Text = "";
		}

		private void mnuFileExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void mnuHelpAbout_Click(object sender, EventArgs e)
		{
			MessageBox.Show(this, "SandDock Demonstration Application" + Environment.NewLine + Environment.NewLine + "SandDock Version " + typeof(SandDockManager).Assembly.GetName().Version, "About");
		}

		private void chkAllowPin_CheckedChanged(object sender, EventArgs e)
		{
			foreach (DockControl window in sandDockManager1.GetDockControls())
				window.AllowCollapse = chkAllowPin.Checked;
		}

		private void chkAllowClose_CheckedChanged(object sender, EventArgs e)
		{
			foreach (DockControl window in sandDockManager1.GetDockControls())
				window.AllowClose = chkAllowClose.Checked;
		}

		private void chkAllowOptions_CheckedChanged(object sender, EventArgs e)
		{
			foreach (DockControl window in sandDockManager1.GetDockControls())
				window.ShowOptions = chkAllowOptions.Checked;
		}

		private int dockableWindowSeed = 1, tabbedDocumentSeed = 1;

		private void btnFloatWindows_Click(object sender, EventArgs e)
		{
			foreach (DockControl control in sandDockManager1.GetDockControls())
			{
				if (control.DockSituation == DockSituation.Docked)
				{
					// Open the window at a random position
					Random r = new Random();
					Rectangle bounds = Screen.PrimaryScreen.WorkingArea;
					int x = r.Next(bounds.Left, bounds.Right - control.FloatingSize.Width);
					int y = r.Next(bounds.Top, bounds.Bottom - control.FloatingSize.Height);

					control.OpenFloating(new Rectangle(x, y, control.FloatingSize.Width, control.FloatingSize.Height), WindowOpenMethod.OnScreenSelect);
				}
			}
		}

		private void btnSaveLayout_Click(object sender, EventArgs e)
		{
			windowLayout = sandDockManager1.GetLayout();
			MessageBox.Show(this, "The current window layout has been saved. To restore this layout at any time, press the Load button.", "SandDock");
		}

		private void btnLoadLayout_Click(object sender, EventArgs e)
		{
			sandDockManager1.SetLayout(windowLayout);
		}

		private void chkAllowDockContainerResize_CheckedChanged(object sender, EventArgs e)
		{
			sandDockManager1.AllowDockContainerResize = chkAllowDockContainerResize.Checked;
		}

		#region Toolbox Population

		private void PopulateToolbox()
		{
			// Populate our fake toolbox with some images and text
			foreach (Type type in DataToolBoxTypes)
			{
				ToolboxItem toolboxItem = new ToolboxItem(type);
				ilTools.Images.Add(toolboxItem.Bitmap);
				ListViewItem item = new ListViewItem(toolboxItem.DisplayName);
				item.ImageIndex = ilTools.Images.Count - 1;
				lvwTools.Items.Add(item);
			}
		}

		private Type[] WinformsToolBoxTypes
		{
			get
			{
				return new[] { typeof(Button), typeof(CheckBox), typeof(CheckedListBox), typeof(ColorDialog), typeof(ComboBox), typeof(ContextMenu)
									  , typeof(DataGrid), typeof(DateTimePicker), typeof(DomainUpDown), typeof(ErrorProvider)
									  , typeof(FontDialog), typeof(GroupBox), typeof(HelpProvider), typeof(HScrollBar), typeof(ImageList), typeof(Label)
									  , typeof(LinkLabel), typeof(ListBox), typeof(ListView), typeof(MainMenu), typeof(MonthCalendar), typeof(NotifyIcon)
									  , typeof(NumericUpDown), typeof(OpenFileDialog), typeof(PageSetupDialog), typeof(Panel), typeof(PictureBox), typeof(PrintDialog)
									  , typeof(PrintPreviewControl), typeof(PrintPreviewDialog), typeof(ProgressBar), typeof(PropertyGrid)
									  , typeof(RadioButton), typeof(RichTextBox), typeof(SaveFileDialog), typeof(Splitter), typeof(StatusBar), typeof(TabControl)
									  , typeof(TextBox), typeof(Timer), typeof(ToolBar), typeof(ToolTip), typeof(TrackBar), typeof(TreeView), typeof(VScrollBar) };
			}
		}

		private Type[] DataToolBoxTypes
		{
			get
			{
				return new[] { typeof(DataSet), typeof(OleDbDataAdapter), typeof(OleDbConnection), typeof(OleDbCommand), typeof(SqlDataAdapter), typeof(SqlConnection), typeof(SqlCommand), typeof(OdbcDataAdapter), typeof(OdbcConnection), typeof(OdbcCommand), typeof(DataView) };
			}
		}

		#endregion

		private void OnViewWindowItemClick(object sender, EventArgs e)
		{
			// All view items have their Tag set to the unique identifier of the window they represent
			ToolStripItem item = (ToolStripItem)sender;
			Guid windowID = new Guid((string)item.Tag);

			// Find the window by its Guid, and activate it
			DockControl window = sandDockManager1.FindControl(windowID);
		    window?.Open(WindowOpenMethod.OnScreenActivate);
		}

		private void ctxWindow_Opening(object sender, CancelEventArgs e)
		{
			// Configure tabbed document commands
			bool isTabbedDocument = lastActivatedWindow is TabbedDocument;
			bool multipleWindowsInGroup = lastActivatedWindow.LayoutSystem.Controls.Count >= 2;
			mnuWindowNewHorizontalTabGroup.Visible = isTabbedDocument;
			mnuWindowNewHorizontalTabGroup.Enabled = multipleWindowsInGroup;
			mnuWindowNewVerticalTabGroup.Visible = isTabbedDocument;
			mnuWindowNewVerticalTabGroup.Enabled = multipleWindowsInGroup;

			// Configure dockable window commands
			bool isDockableWindow = lastActivatedWindow is DockableWindow;
			mnuWindowDock.Visible = isDockableWindow;
			mnuWindowDock.Enabled = lastActivatedWindow.DockSituation == DockSituation.Floating;
			mnuWindowFloat.Visible = isDockableWindow;
			mnuWindowFloat.Enabled = lastActivatedWindow.DockSituation == DockSituation.Docked && !lastActivatedWindow.Collapsed;
			mnuWindowAutoHide.Visible = isDockableWindow;
			mnuWindowAutoHide.Enabled = lastActivatedWindow.DockSituation == DockSituation.Docked;
			mnuWindowAutoHide.Checked = lastActivatedWindow.Collapsed;

			// Depending on the window's closeaction, change the close item text
			mnuWindowClose.Text = lastActivatedWindow.CloseAction == DockControlCloseAction.Dispose ? "&Close" : "&Hide";
		}

		private void mnuWindowClose_Click(object sender, EventArgs e)
		{
			lastActivatedWindow.Close();
		}

		private void mnuWindowNewHorizontalTabGroup_Click(object sender, EventArgs e)
		{
			lastActivatedWindow.Split(DockSide.Bottom);
		}

		private void mnuWindowNewVerticalTabGroup_Click(object sender, EventArgs e)
		{
			lastActivatedWindow.Split(DockSide.Right);
		}

		private void mnuWindowFloat_Click(object sender, EventArgs e)
		{
			lastActivatedWindow.OpenFloating();
		}

		private void mnuWindowDock_Click(object sender, EventArgs e)
		{
			lastActivatedWindow.OpenDocked(WindowOpenMethod.OnScreenActivate);
		}

		private void mnuWindowAutoHide_Click(object sender, EventArgs e)
		{
			lastActivatedWindow.Collapsed = !lastActivatedWindow.Collapsed;
		}

		private void chkShowActiveFilesList_CheckedChanged(object sender, EventArgs e)
		{
			if (chkShowActiveFilesList.Checked)
				sandDockManager1.DocumentOverflow = DocumentOverflowMode.Menu;
			else
				sandDockManager1.DocumentOverflow = DocumentOverflowMode.Scrollable;
		}

		private void sandDockManager1_ShowActiveFilesList(object sender, ActiveFilesListEventArgs e)
		{
			// Create an array of menu items representing the list of windows
			MenuItem[] items = new MenuItem[e.Windows.Length];
			for (int i = 0; i < e.Windows.Length; i++)
			{
				ActiveFileMenuItem item = new ActiveFileMenuItem(e.Windows[i]);
				items[i] = item;
			}
			ContextMenu menu = new ContextMenu(items);

			// Show a context menu for the user
			menu.Show(e.Control, e.Position);

			// TODO: Ensure menu items are disposed after use
		}

		private class ActiveFileMenuItem : MenuItem
		{
			private DockControl window;

			public ActiveFileMenuItem(DockControl window)
				: base(window.Text)
			{
				this.window = window;
			}

			protected override void OnClick(EventArgs e)
			{
				window.Activate();
			}
		}

		private void chkIntegralClose_CheckedChanged(object sender, EventArgs e)
		{
			sandDockManager1.IntegralClose = chkIntegralClose.Checked;
		}

		/// <summary>
		/// This method takes an array of tab colors from Microsoft OneNote and applies them to all loaded documents to illustrate the tab coloring effect
		/// </summary>
		private void ApplyColorsToDocuments()
		{
			Color[] oneNoteColours = { Color.FromArgb(138, 168, 228), Color.FromArgb(145, 186, 174), Color.FromArgb(246, 176, 120),
														 Color.FromArgb(213, 164, 187), Color.FromArgb(180, 158, 222), Color.FromArgb(238, 149, 151), Color.FromArgb(183, 201, 151),
														 Color.FromArgb(255, 216, 105) };
			int colorIndex = 0;
			foreach (DockControl control in sandDockManager1.GetDockControls(DockSituation.Document))
			{
				control.BackColor = oneNoteColours[colorIndex++];
				if (colorIndex == oneNoteColours.Length)
					colorIndex = 0;
			}
		}

		private void RemoveColorsFromDocuments()
		{
			foreach (DockControl control in sandDockManager1.GetDockControls(DockSituation.Document))
			{
				control.BackColor = control == tdWelcome ? SystemColors.Window : SystemColors.Control;
			}
		}

		private void tvwRenderer_AfterSelect(object sender, TreeViewEventArgs e)
		{
			// Our example stores a renderer instance in the tag of each node
			sandDockManager1.Renderer = (RendererBase)e.Node.Tag;
			if (sandDockManager1.Renderer is Office2003Renderer)
				ApplyColorsToDocuments();
			else
				RemoveColorsFromDocuments();

			// Attempt to keep the ToolStrip vaguely in keeping with the selected SandDock theme - SandBar offers more control over this.
			ToolStripManager.VisualStylesEnabled = sandDockManager1.Renderer is Office2003Renderer || sandDockManager1.Renderer is Office2007Renderer;

			// Only show a border around documents if we're not using the Office 2007 renderer, where it doesn't look so great
			sandDockManager1.BorderStyle = sandDockManager1.Renderer is Office2007Renderer ? BorderStyle.None : BorderStyle.Flat;

			// Redraw everything
			Invalidate(true);
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			// Create new window
		    var textBox = new TextBox
		    {
		        Multiline = true,
		        BorderStyle = System.Windows.Forms.BorderStyle.None,
		        Dock = DockStyle.Fill
		    };
		    DockControl window = new TabbedDocument(sandDockManager1, textBox, "Document " + tabbedDocumentSeed++);

			// Open it
			window.Open();
		}

	}
}
