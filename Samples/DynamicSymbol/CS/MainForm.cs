using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Syncfusion.Windows.Forms.Diagram;

namespace DynamicSymbol
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram;
		private System.Windows.Forms.ImageList imageList;
		private Syncfusion.Windows.Forms.Tools.XPMenus.MainFrameBarManager mainFrameBarManager;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemInsertSymbol;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar bar1;
		private System.ComponentModel.IContainer components;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// Register an InsertSymbolTool with the controller for inserting
			// MySymbol objects.
			//
			this.diagram.Controller.RegisterTool(new InsertSymbolTool("InsertMySymbol", typeof(MySymbol)));
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.diagram = new Syncfusion.Windows.Forms.Diagram.Controls.Diagram();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.mainFrameBarManager = new Syncfusion.Windows.Forms.Tools.XPMenus.MainFrameBarManager(this.components, this);
			this.barItemInsertSymbol = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.bar1 = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "Edit");
			((System.ComponentModel.ISupportInitialize)(this.mainFrameBarManager)).BeginInit();
			this.SuspendLayout();
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
			this.diagram.Location = new System.Drawing.Point(0, 26);
			// 
			// diagram.Model
			// 
			this.diagram.Model.BoundaryConstraintsEnabled = true;
			this.diagram.Model.Height = 5000F;
			this.diagram.Model.Name = "Model";
			this.diagram.Model.MeasurementScale = 1F;
			this.diagram.Model.MeasurementUnits = System.Drawing.GraphicsUnit.Pixel;
			this.diagram.Model.Width = 5000F;
			this.diagram.Name = "diagram";
			this.diagram.NudgeIncrement = 1F;
			this.diagram.ScrollGranularity = 0.5F;
			this.diagram.Size = new System.Drawing.Size(464, 340);
			this.diagram.TabIndex = 0;
			this.diagram.Text = "diagram1";
			// 
			// diagram.View
			// 
			this.diagram.View.BackgroundColor = System.Drawing.Color.DarkGray;
			this.diagram.View.Grid.HorizontalSpacing = 10F;
			this.diagram.View.Grid.MinPixelSpacing = 4;
			this.diagram.View.Grid.SnapToGrid = true;
			this.diagram.View.Grid.VerticalSpacing = 10F;
			this.diagram.View.Grid.Visible = true;
			this.diagram.View.HandleAnchorColor = System.Drawing.Color.LightGray;
			this.diagram.View.HandleColor = System.Drawing.Color.White;
			this.diagram.View.HandleSize = 6;
			this.diagram.View.ShowPageBounds = true;
			this.diagram.VScroll = true;
			// 
			// imageList
			// 
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// mainFrameBarManager
			// 
			this.mainFrameBarManager.BarPositionInfo = ((System.IO.MemoryStream)(resources.GetObject("mainFrameBarManager.BarPositionInfo")));
			this.mainFrameBarManager.Bars.Add(this.bar1);
			this.mainFrameBarManager.Categories.Add("Edit");
			this.mainFrameBarManager.CurrentBaseFormType = "";
			this.mainFrameBarManager.Form = this;
			this.mainFrameBarManager.ImageList = this.imageList;
			this.mainFrameBarManager.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																											 this.barItemInsertSymbol});
			this.mainFrameBarManager.LargeImageList = null;
			this.mainFrameBarManager.ResetCustomization = false;
			// 
			// barItemInsertSymbol
			// 
			this.barItemInsertSymbol.CategoryIndex = 0;
			this.barItemInsertSymbol.ID = "InsertSymbol";
			this.barItemInsertSymbol.ImageIndex = 0;
			this.barItemInsertSymbol.Text = "InsertSymbol";
			this.barItemInsertSymbol.Click += new System.EventHandler(this.barItemInsertSymbol_Click);
			// 
			// bar1
			// 
			this.bar1.BarName = "Edit";
			this.bar1.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																							  this.barItemInsertSymbol});
			this.bar1.Manager = this.mainFrameBarManager;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 366);
			this.Controls.Add(this.diagram);
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Dynamic Symbol";
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

		private void barItemInsertSymbol_Click(object sender, System.EventArgs e)
		{
			this.diagram.ActivateTool("InsertMySymbol");
		}
	}
}
