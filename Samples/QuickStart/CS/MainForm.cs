using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace QuickStart
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panelSymbolPalettes;
		private Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar paletteGroupBar;
		private System.Windows.Forms.Panel panelProperties;
		private System.Windows.Forms.Panel panelDiagram;
		private Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor propertyEditor;
		private Syncfusion.Windows.Forms.Tools.GroupBarItem groupBarItemTestSymbols;
		private Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView paletteGroupViewTestSymbols;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItemFileOpen;
		private System.Windows.Forms.MenuItem menuItemFileSave;
		private System.Windows.Forms.MenuItem menuItemFileExit;
		private System.Windows.Forms.MenuItem menuItemToolsSelect;
		private System.Windows.Forms.MenuItem menuItemToolsPan;
		private System.Windows.Forms.MenuItem menuItemToolsZoom;
		private System.Windows.Forms.MenuItem menuItemToolsGroup;
		private System.Windows.Forms.MenuItem menuItemToolsUngroup;
		private System.Windows.Forms.MenuItem menuItemToolsLink;
		private Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelSymbolPalettes = new System.Windows.Forms.Panel();
			this.paletteGroupBar = new Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar();
			this.paletteGroupViewTestSymbols = new Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView();
			this.groupBarItemTestSymbols = new Syncfusion.Windows.Forms.Tools.GroupBarItem();
			this.panelProperties = new System.Windows.Forms.Panel();
			this.propertyEditor = new Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor();
			this.diagram1 = new Syncfusion.Windows.Forms.Diagram.Controls.Diagram();
			this.panelDiagram = new System.Windows.Forms.Panel();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemFileOpen = new System.Windows.Forms.MenuItem();
			this.menuItemFileSave = new System.Windows.Forms.MenuItem();
			this.menuItemFileExit = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItemToolsSelect = new System.Windows.Forms.MenuItem();
			this.menuItemToolsPan = new System.Windows.Forms.MenuItem();
			this.menuItemToolsZoom = new System.Windows.Forms.MenuItem();
			this.menuItemToolsGroup = new System.Windows.Forms.MenuItem();
			this.menuItemToolsUngroup = new System.Windows.Forms.MenuItem();
			this.menuItemToolsLink = new System.Windows.Forms.MenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.panelSymbolPalettes.SuspendLayout();
			this.paletteGroupBar.SuspendLayout();
			this.panelProperties.SuspendLayout();
			this.panelDiagram.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelSymbolPalettes
			// 
			this.panelSymbolPalettes.Controls.Add(this.paletteGroupBar);
			this.panelSymbolPalettes.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelSymbolPalettes.Location = new System.Drawing.Point(0, 0);
			this.panelSymbolPalettes.Name = "panelSymbolPalettes";
			this.panelSymbolPalettes.Size = new System.Drawing.Size(128, 518);
			this.panelSymbolPalettes.TabIndex = 0;
			// 
			// paletteGroupBar
			// 
			this.paletteGroupBar.AllowDrop = true;
			this.paletteGroupBar.Controls.Add(this.paletteGroupViewTestSymbols);
			this.paletteGroupBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.paletteGroupBar.EditMode = false;
			this.paletteGroupBar.GroupBarItems.AddRange(new Syncfusion.Windows.Forms.Tools.GroupBarItem[] {
																											  this.groupBarItemTestSymbols});
			this.paletteGroupBar.Location = new System.Drawing.Point(0, 0);
			this.paletteGroupBar.Name = "paletteGroupBar";
			this.paletteGroupBar.SelectedItem = 0;
			this.paletteGroupBar.Size = new System.Drawing.Size(128, 518);
			this.paletteGroupBar.TabIndex = 0;
			this.paletteGroupBar.Text = "paletteGroupBar1";
			// 
			// paletteGroupViewTestSymbols
			// 
			this.paletteGroupViewTestSymbols.EditMode = false;
			this.paletteGroupViewTestSymbols.Location = new System.Drawing.Point(2, 23);
			this.paletteGroupViewTestSymbols.Name = "paletteGroupViewTestSymbols";
			this.paletteGroupViewTestSymbols.SelectedItem = 0;
			this.paletteGroupViewTestSymbols.Size = new System.Drawing.Size(124, 493);
			this.paletteGroupViewTestSymbols.TabIndex = 0;
			this.paletteGroupViewTestSymbols.Text = "paletteGroupView1";
			this.paletteGroupViewTestSymbols.LoadPalette(typeof(QuickStart.MainForm).Assembly, "QuickStart.MainForm", "paletteGroupViewTestSymbols.Palette");
			// 
			// groupBarItemTestSymbols
			// 
			this.groupBarItemTestSymbols.Client = this.paletteGroupViewTestSymbols;
			this.groupBarItemTestSymbols.Text = "Test Symbols";
			// 
			// panelProperties
			// 
			this.panelProperties.Controls.Add(this.propertyEditor);
			this.panelProperties.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelProperties.Location = new System.Drawing.Point(544, 0);
			this.panelProperties.Name = "panelProperties";
			this.panelProperties.Size = new System.Drawing.Size(176, 518);
			this.panelProperties.TabIndex = 1;
			// 
			// propertyEditor
			// 
			this.propertyEditor.Diagram = this.diagram1;
			this.propertyEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyEditor.Location = new System.Drawing.Point(0, 0);
			this.propertyEditor.Name = "propertyEditor";
			this.propertyEditor.Size = new System.Drawing.Size(176, 518);
			this.propertyEditor.TabIndex = 0;
			// 
			// diagram1
			// 
			this.diagram1.AllowDrop = true;
			// 
			// diagram1.Controller
			// 
			this.diagram1.Controller.MaxHistory = 256;
			this.diagram1.Controller.SelectHandleMode = Syncfusion.Windows.Forms.Diagram.SelectHandleType.Resize;
			this.diagram1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagram1.HScroll = true;
			this.diagram1.LayoutManager = null;
			this.diagram1.Location = new System.Drawing.Point(0, 0);
			// 
			// diagram1.Model
			// 
			this.diagram1.Model.BoundaryConstraintsEnabled = true;
			this.diagram1.Model.Height = 1056F;
			this.diagram1.Model.MeasurementScale = 1F;
			this.diagram1.Model.MeasurementUnits = System.Drawing.GraphicsUnit.Pixel;
			this.diagram1.Model.Name = "Model";
			this.diagram1.Model.Width = 816F;
			this.diagram1.Name = "diagram1";
			this.diagram1.NudgeIncrement = 1F;
			this.diagram1.ScrollGranularity = 0.5F;
			this.diagram1.Size = new System.Drawing.Size(416, 518);
			this.diagram1.TabIndex = 0;
			this.diagram1.Text = "diagram1";
			// 
			// diagram1.View
			// 
			this.diagram1.View.BackgroundColor = System.Drawing.Color.DarkGray;
			this.diagram1.View.Grid.Color = System.Drawing.Color.Black;
			this.diagram1.View.Grid.HorizontalSpacing = 10F;
			this.diagram1.View.Grid.SnapToGrid = true;
			this.diagram1.View.Grid.VerticalSpacing = 10F;
			this.diagram1.View.Grid.Visible = true;
			this.diagram1.View.HandleAnchorColor = System.Drawing.Color.LightGray;
			this.diagram1.View.HandleColor = System.Drawing.Color.White;
			this.diagram1.View.HandleSize = 6;
			this.diagram1.View.ShowPageBounds = true;
			this.diagram1.VScroll = true;
			// 
			// panelDiagram
			// 
			this.panelDiagram.Controls.Add(this.diagram1);
			this.panelDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelDiagram.Location = new System.Drawing.Point(128, 0);
			this.panelDiagram.Name = "panelDiagram";
			this.panelDiagram.Size = new System.Drawing.Size(416, 518);
			this.panelDiagram.TabIndex = 2;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem5});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemFileOpen,
																					  this.menuItemFileSave,
																					  this.menuItemFileExit});
			this.menuItem1.Text = "&File";
			// 
			// menuItemFileOpen
			// 
			this.menuItemFileOpen.Index = 0;
			this.menuItemFileOpen.Text = "&Open";
			this.menuItemFileOpen.Click += new System.EventHandler(this.menuItemFileOpen_Click);
			// 
			// menuItemFileSave
			// 
			this.menuItemFileSave.Index = 1;
			this.menuItemFileSave.Text = "&Save";
			this.menuItemFileSave.Click += new System.EventHandler(this.menuItemFileSave_Click);
			// 
			// menuItemFileExit
			// 
			this.menuItemFileExit.Index = 2;
			this.menuItemFileExit.Text = "&Exit";
			this.menuItemFileExit.Click += new System.EventHandler(this.menuItemFileExit_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemToolsSelect,
																					  this.menuItemToolsPan,
																					  this.menuItemToolsZoom,
																					  this.menuItemToolsGroup,
																					  this.menuItemToolsUngroup,
																					  this.menuItemToolsLink});
			this.menuItem5.Text = "&Tools";
			// 
			// menuItemToolsSelect
			// 
			this.menuItemToolsSelect.Index = 0;
			this.menuItemToolsSelect.Text = "&Select";
			this.menuItemToolsSelect.Click += new System.EventHandler(this.menuItemToolsSelect_Click);
			// 
			// menuItemToolsPan
			// 
			this.menuItemToolsPan.Index = 1;
			this.menuItemToolsPan.Text = "&Pan";
			this.menuItemToolsPan.Click += new System.EventHandler(this.menuItemToolsPan_Click);
			// 
			// menuItemToolsZoom
			// 
			this.menuItemToolsZoom.Index = 2;
			this.menuItemToolsZoom.Text = "&Zoom";
			this.menuItemToolsZoom.Click += new System.EventHandler(this.menuItemToolsZoom_Click);
			// 
			// menuItemToolsGroup
			// 
			this.menuItemToolsGroup.Index = 3;
			this.menuItemToolsGroup.Text = "&Group";
			this.menuItemToolsGroup.Click += new System.EventHandler(this.menuItemToolsGroup_Click);
			// 
			// menuItemToolsUngroup
			// 
			this.menuItemToolsUngroup.Index = 4;
			this.menuItemToolsUngroup.Text = "&Ungroup";
			this.menuItemToolsUngroup.Click += new System.EventHandler(this.menuItemToolsUngroup_Click);
			// 
			// menuItemToolsLink
			// 
			this.menuItemToolsLink.Index = 5;
			this.menuItemToolsLink.Text = "&Link Symbols";
			this.menuItemToolsLink.Click += new System.EventHandler(this.menuItemToolsLink_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(720, 518);
			this.Controls.Add(this.panelDiagram);
			this.Controls.Add(this.panelProperties);
			this.Controls.Add(this.panelSymbolPalettes);
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.Text = "Quick Start Sample";
			this.panelSymbolPalettes.ResumeLayout(false);
			this.paletteGroupBar.ResumeLayout(false);
			this.panelProperties.ResumeLayout(false);
			this.panelDiagram.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void menuItemFileOpen_Click(object sender, System.EventArgs e)
		{
			if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				this.diagram1.LoadBinary(this.openFileDialog1.FileName);
			}
		}

		private void menuItemFileSave_Click(object sender, System.EventArgs e)
		{
			if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				this.diagram1.SaveBinary(this.openFileDialog1.FileName);
			}
		}

		private void menuItemFileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItemToolsSelect_Click(object sender, System.EventArgs e)
		{
			this.diagram1.ActivateTool("SelectTool");
		}

		private void menuItemToolsPan_Click(object sender, System.EventArgs e)
		{
			this.diagram1.ActivateTool("PanTool");
		}

		private void menuItemToolsZoom_Click(object sender, System.EventArgs e)
		{
			this.diagram1.ActivateTool("ZoomTool");
		}

		private void menuItemToolsGroup_Click(object sender, System.EventArgs e)
		{
			this.diagram1.ActivateTool("GroupTool");
		}

		private void menuItemToolsUngroup_Click(object sender, System.EventArgs e)
		{
			this.diagram1.ActivateTool("UngroupTool");
		}

		private void menuItemToolsLink_Click(object sender, System.EventArgs e)
		{
			this.diagram1.ActivateTool("LinkTool");
		}
	}
}
