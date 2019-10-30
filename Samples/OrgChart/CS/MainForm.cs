using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Syncfusion.Windows.Forms.Diagram;

namespace Syncfusion.OrgChart
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel panel1;
		private Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram;
		private Syncfusion.Windows.Forms.Tools.XPMenus.MainFrameBarManager mainFrameBarManager;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemLink;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar bar2;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemUpdateLayout;
		private System.Windows.Forms.ImageList imageList1;
		private Syncfusion.OrgChart.OrgChartManager orgChartManager;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemFileExit;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItemFile;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemAdd;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar bar1;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSelectTool;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemAutoLayout;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditCut;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditCopy;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditPaste;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemHelpAbout;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem1;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem2;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemDimensions;
		private InsertSymbolTool insSymbolTool = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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

		protected Link CreateOrthogonalLink(PointF[] pts)
		{
			Link link = new Link(Link.Shapes.OrthogonalLine, pts);
			link.EndPoints.LastEndPoint = new FilledArrowDecorator();
			return link;
		}

		protected Link CreateLink(PointF[] pts)
		{
			Link link = new Link(Link.Shapes.Line, pts);
			link.EndPoints.LastEndPoint = new FilledArrowDecorator();
			return link;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.panel1 = new System.Windows.Forms.Panel();
			this.diagram = new Syncfusion.Windows.Forms.Diagram.Controls.Diagram();
			this.orgChartManager = new Syncfusion.OrgChart.OrgChartManager();
			this.mainFrameBarManager = new Syncfusion.Windows.Forms.Tools.XPMenus.MainFrameBarManager(this.components, this);
			this.bar2 = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "Actions");
			this.barItemSelectTool = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemAdd = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemLink = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemUpdateLayout = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemAutoLayout = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemDimensions = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.bar1 = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "MainMenu");
			this.parentBarItemFile = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.barItemFileExit = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.barItemEditCut = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEditCopy = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEditPaste = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItem2 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.barItemHelpAbout = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainFrameBarManager)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.diagram);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 52);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(616, 410);
			this.panel1.TabIndex = 2;
			// 
			// diagram
			// 
			this.diagram.AllowDrop = true;
			// 
			// diagram.Controller
			// 
			this.diagram.Controller.MaxHistory = 256;
			this.diagram.Controller.SelectHandleMode = Syncfusion.Windows.Forms.Diagram.SelectHandleType.Resize;
			this.diagram.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagram.HScroll = true;
			this.diagram.LayoutManager = this.orgChartManager;
			this.diagram.Location = new System.Drawing.Point(0, 0);
			// 
			// diagram.Model
			// 
			this.diagram.Model.BoundaryConstraintsEnabled = true;
			this.diagram.Model.Height = 1056F;
			this.diagram.Model.MeasurementScale = 1F;
			this.diagram.Model.MeasurementUnits = System.Drawing.GraphicsUnit.Pixel;
			this.diagram.Model.Name = "Model";
			this.diagram.Model.Width = 816F;
			this.diagram.Name = "diagram";
			this.diagram.NudgeIncrement = 1F;
			this.diagram.ScrollGranularity = 0.5F;
			this.diagram.Size = new System.Drawing.Size(616, 410);
			this.diagram.TabIndex = 1;
			this.diagram.Text = "OrgChart Diagram";
			// 
			// diagram.View
			// 
			this.diagram.View.BackgroundColor = System.Drawing.Color.DarkGray;
			this.diagram.View.Grid.Color = System.Drawing.Color.Black;
			this.diagram.View.Grid.HorizontalSpacing = 10F;
			this.diagram.View.Grid.SnapToGrid = true;
			this.diagram.View.Grid.VerticalSpacing = 10F;
			this.diagram.View.Grid.Visible = true;
			this.diagram.View.HandleAnchorColor = System.Drawing.Color.LightGray;
			this.diagram.View.HandleColor = System.Drawing.Color.White;
			this.diagram.View.HandleSize = 6;
			this.diagram.View.ShowPageBounds = true;
			this.diagram.VScroll = true;
			// 
			// orgChartManager
			// 
			this.orgChartManager.AutoLayout = false;
			this.orgChartManager.HorizontalSpacing = 200F;
			this.orgChartManager.VerticalSpacing = 140F;
			// 
			// mainFrameBarManager
			// 
			this.mainFrameBarManager.BarPositionInfo = ((System.IO.MemoryStream)(resources.GetObject("mainFrameBarManager.BarPositionInfo")));
			this.mainFrameBarManager.Bars.Add(this.bar2);
			this.mainFrameBarManager.Bars.Add(this.bar1);
			this.mainFrameBarManager.Categories.Add("Actions");
			this.mainFrameBarManager.Categories.Add("File");
			this.mainFrameBarManager.Categories.Add("Edit");
			this.mainFrameBarManager.Categories.Add("Help");
			this.mainFrameBarManager.CurrentBaseFormType = "System.Windows.Forms.Form";
			this.mainFrameBarManager.Form = this;
			this.mainFrameBarManager.ImageList = this.imageList1;
			this.mainFrameBarManager.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																											 this.barItemLink,
																											 this.barItemUpdateLayout,
																											 this.barItemFileExit,
																											 this.parentBarItemFile,
																											 this.barItemAdd,
																											 this.barItemSelectTool,
																											 this.barItemAutoLayout,
																											 this.barItemEditCut,
																											 this.barItemEditCopy,
																											 this.barItemEditPaste,
																											 this.barItemHelpAbout,
																											 this.parentBarItem1,
																											 this.parentBarItem2,
																											 this.barItemDimensions});
			this.mainFrameBarManager.LargeImageList = null;
			this.mainFrameBarManager.ResetCustomization = false;
			// 
			// bar2
			// 
			this.bar2.BarName = "Actions";
			this.bar2.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																							  this.barItemSelectTool,
																							  this.barItemAdd,
																							  this.barItemLink,
																							  this.barItemUpdateLayout,
																							  this.barItemAutoLayout,
																							  this.barItemDimensions});
			this.bar2.Manager = this.mainFrameBarManager;
			// 
			// barItemSelectTool
			// 
			this.barItemSelectTool.CategoryIndex = 0;
			this.barItemSelectTool.ID = "Select";
			this.barItemSelectTool.ImageIndex = 3;
			this.barItemSelectTool.Text = "Select";
			this.barItemSelectTool.Tooltip = "Select Tool";
			this.barItemSelectTool.Click += new System.EventHandler(this.barItemSelectTool_Click);
			// 
			// barItemAdd
			// 
			this.barItemAdd.CategoryIndex = 0;
			this.barItemAdd.ID = "Add";
			this.barItemAdd.ImageIndex = 2;
			this.barItemAdd.Text = "Add";
			this.barItemAdd.Tooltip = "Add Member";
			this.barItemAdd.Click += new System.EventHandler(this.barItemAdd_Click);
			// 
			// barItemLink
			// 
			this.barItemLink.CategoryIndex = 0;
			this.barItemLink.ID = "Link";
			this.barItemLink.ImageIndex = 0;
			this.barItemLink.Text = "Link";
			this.barItemLink.Click += new System.EventHandler(this.barItemLink_Click);
			// 
			// barItemUpdateLayout
			// 
			this.barItemUpdateLayout.CategoryIndex = 0;
			this.barItemUpdateLayout.ID = "Update Layout";
			this.barItemUpdateLayout.ImageIndex = 1;
			this.barItemUpdateLayout.Text = "Update Layout";
			this.barItemUpdateLayout.Click += new System.EventHandler(this.barItemUpdateLayout_Click);
			// 
			// barItemAutoLayout
			// 
			this.barItemAutoLayout.CategoryIndex = 0;
			this.barItemAutoLayout.ID = "AutoLayout";
			this.barItemAutoLayout.ImageIndex = 4;
			this.barItemAutoLayout.Text = "AutoLayout";
			this.barItemAutoLayout.Tooltip = "Auto Layout";
			this.barItemAutoLayout.Click += new System.EventHandler(this.barItemAutoLayout_Click);
			// 
			// barItemDimensions
			// 
			this.barItemDimensions.CategoryIndex = 0;
			this.barItemDimensions.ID = "Dimensions";
			this.barItemDimensions.ImageIndex = 8;
			this.barItemDimensions.Text = "Dimensions";
			this.barItemDimensions.Click += new System.EventHandler(this.barItemDimensions_Click);
			// 
			// bar1
			// 
			this.bar1.BarName = "MainMenu";
			this.bar1.BarStyle = ((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle)((((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.AllowQuickCustomizing | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.IsMainMenu) 
				| Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.Visible) 
				| Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.DrawDragBorder)));
			this.bar1.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																							  this.parentBarItemFile,
																							  this.parentBarItem1,
																							  this.parentBarItem2});
			this.bar1.Manager = this.mainFrameBarManager;
			// 
			// parentBarItemFile
			// 
			this.parentBarItemFile.CategoryIndex = 1;
			this.parentBarItemFile.ID = "File";
			this.parentBarItemFile.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										   this.barItemFileExit});
			this.parentBarItemFile.Text = "File";
			// 
			// barItemFileExit
			// 
			this.barItemFileExit.CategoryIndex = 1;
			this.barItemFileExit.ID = "Exit";
			this.barItemFileExit.Text = "Exit";
			this.barItemFileExit.Click += new System.EventHandler(this.barItemFileExit_Click);
			// 
			// parentBarItem1
			// 
			this.parentBarItem1.CategoryIndex = 2;
			this.parentBarItem1.ID = "Edit";
			this.parentBarItem1.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										this.barItemEditCut,
																										this.barItemEditCopy,
																										this.barItemEditPaste});
			this.parentBarItem1.Text = "Edit";
			// 
			// barItemEditCut
			// 
			this.barItemEditCut.CategoryIndex = 2;
			this.barItemEditCut.ID = "Cut";
			this.barItemEditCut.ImageIndex = 5;
			this.barItemEditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.barItemEditCut.Text = "Cu&t";
			this.barItemEditCut.Click += new System.EventHandler(this.barItemEditCut_Click);
			// 
			// barItemEditCopy
			// 
			this.barItemEditCopy.CategoryIndex = 2;
			this.barItemEditCopy.ID = "Copy";
			this.barItemEditCopy.ImageIndex = 6;
			this.barItemEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.barItemEditCopy.Text = "&Copy";
			this.barItemEditCopy.Click += new System.EventHandler(this.barItemEditCopy_Click);
			// 
			// barItemEditPaste
			// 
			this.barItemEditPaste.CategoryIndex = 2;
			this.barItemEditPaste.ID = "Paste";
			this.barItemEditPaste.ImageIndex = 7;
			this.barItemEditPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.barItemEditPaste.Text = "&Paste";
			this.barItemEditPaste.Click += new System.EventHandler(this.barItemEditPaste_Click);
			// 
			// parentBarItem2
			// 
			this.parentBarItem2.CategoryIndex = 3;
			this.parentBarItem2.ID = "Help";
			this.parentBarItem2.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										this.barItemHelpAbout});
			this.parentBarItem2.Text = "Help";
			// 
			// barItemHelpAbout
			// 
			this.barItemHelpAbout.CategoryIndex = 3;
			this.barItemHelpAbout.ID = "About";
			this.barItemHelpAbout.Text = "About";
			this.barItemHelpAbout.Click += new System.EventHandler(this.barItemHelpAbout_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(616, 462);
			this.Controls.Add(this.panel1);
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Organizational Chart";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainFrameBarManager)).EndInit();
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

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			this.insSymbolTool = new InsertSymbolTool("InsertMemberSymbol", typeof(MemberSymbol));
			this.diagram.Controller.RegisterTool(this.insSymbolTool);

			// Attach link object factory to the link tool
			Tool linkTool = this.diagram.Controller.GetTool("LinkTool");
			if (linkTool != null && linkTool.GetType() == typeof(LinkTool))
			{
#if false
				((LinkTool)linkTool).LinkFactory = new LinkFactory(this.CreateOrthogonalLink);
#else
				((LinkTool)linkTool).LinkFactory = new LinkFactory(this.CreateLink);
#endif
			}

			this.barItemAutoLayout.Checked = this.orgChartManager.AutoLayout;
		}

		private void barItemLink_Click(object sender, System.EventArgs e)
		{
			this.diagram.ActivateTool("LinkTool");
		}

		private void barItemUpdateLayout_Click(object sender, System.EventArgs e)
		{
			this.diagram.LayoutManager.UpdateLayout(null);
		}

		private void barItemAdd_Click(object sender, System.EventArgs e)
		{
			OrgMemberInfoDlg infoDlg = new OrgMemberInfoDlg();
			if (infoDlg.ShowDialog(this) == DialogResult.OK)
			{
				MemberSymbol memSym = new MemberSymbol();
				memSym.MemberName = infoDlg.MemberName;
				memSym.Title = infoDlg.Title;
				this.insSymbolTool.Symbol = memSym;
				this.diagram.Controller.ActivateTool(this.insSymbolTool);
			}
		}

		private void barItemFileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void barItemSelectTool_Click(object sender, System.EventArgs e)
		{
			this.diagram.ActivateTool("SelectTool");
		}

		private void barItemAutoLayout_Click(object sender, System.EventArgs e)
		{
			if (this.barItemAutoLayout.Checked)
			{
				this.barItemAutoLayout.Checked = false;
			}
			else
			{
				this.barItemAutoLayout.Checked = true;
			}
			this.orgChartManager.AutoLayout = this.barItemAutoLayout.Checked;
		}

		private void barItemEditCut_Click(object sender, System.EventArgs e)
		{
			this.diagram.Cut();
		}

		private void barItemEditCopy_Click(object sender, System.EventArgs e)
		{
			this.diagram.Copy();
		}

		private void barItemEditPaste_Click(object sender, System.EventArgs e)
		{
			this.diagram.Paste();
		}

		private void barItemDimensions_Click(object sender, System.EventArgs e)
		{
			DimensionsDlg dlg = new DimensionsDlg();

			dlg.DiagramWidth = this.diagram.Model.Width;
			dlg.DiagramHeight = this.diagram.Model.Height;

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this.diagram.Model.Width = dlg.DiagramWidth;
				this.diagram.Model.Height = dlg.DiagramHeight;
			}
		}

		private void barItemHelpAbout_Click(object sender, System.EventArgs e)
		{
			AboutForm about = new AboutForm();
			about.ShowDialog(this);
		}
	}
}
