////////////////////////////////////////////////////////////////////////////////
//  Copyright Syncfusion Inc. 2003 - 2005. All rights reserved.
//
//  Use of this code is subject to the terms of our license.
//  A copy of the current license can be obtained at any time by e-mailing
//  licensing@syncfusion.com. Re-distribution in any form is strictly
//  prohibited. Any infringement will be prosecuted under applicable laws. 
//
//  Essential Diagram
//     Author:  Jeff Boenig
//     Created: March 2003
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Diagram;
using Syncfusion.Windows.Forms.Diagram.Controls;

namespace Syncfusion.SymbolDesigner
{
	/// <summary>
	/// Summary description for SymbolDocument.
	/// </summary>
	public class SymbolDocument : System.Windows.Forms.Form
	{
		private Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ChildFrameBarManager childFrameBarManager;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSelectTool;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemLine;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemRectangle;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEllipse;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemText;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemPolyline;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemPolygon;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemArc;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemPolycurve;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemClosedCurve;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemPort;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemImage;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemRotate;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemRotateLeft;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemRotateRight;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemFlipVertical;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemFlipHorizontal;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemGroup;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemUngroup;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemBringToFront;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSendToBack;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemBringForward;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSendBackward;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemPan;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemNudgeUp;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemNudgeDown;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemNudgeLeft;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemNudgeRight;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barDrawing;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barRotate;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barNode;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barView;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barNudge;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemShowGrid;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSnapToGrid;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemAlignBottom;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemAlignCenter;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemAlignLeft;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemAlignMiddle;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemAlignRight;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemAlignTop;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSpaceAcross;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSpaceDown;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSameWidth;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSameHeight;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSameSize;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar bar1;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar bar2;
		private System.Windows.Forms.ImageList smBarItemImages;
		private System.Windows.Forms.ContextMenu diagramContextMenu;
		private System.Windows.Forms.MenuItem menuItemProperties;
		private System.Windows.Forms.MenuItem menuItemShowGrid;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemZoom;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ComboBoxBarItem comboBoxBarItemMagnification;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemRoundRect;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditVertices;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemOrthogonalLine;
		private System.ComponentModel.IContainer components;

		public SymbolDocument()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// Wire up event handlers to canvas
			//
			this.Diagram.Controller.ToolActivate += new Controller.ToolEventHandler(OnDiagramToolActivate);
			this.Diagram.Controller.ToolDeactivate += new Controller.ToolEventHandler(OnDiagramToolDeactivate);
			this.Diagram.Controller.SelectionChanged += new NodeCollection.EventHandler(OnSelectionChanged);
		}

		public SymbolDocument(SymbolModel model)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// Wire up event handlers to canvas
			//
			this.Diagram.AttachModel(model);
			this.Diagram.Controller.ToolActivate += new Controller.ToolEventHandler(this.OnDiagramToolActivate);
			this.Diagram.Controller.ToolDeactivate += new Controller.ToolEventHandler(this.OnDiagramToolDeactivate);
			this.Diagram.Controller.SelectionChanged += new NodeCollection.EventHandler(this.OnSelectionChanged);
			this.Diagram.Model.PropertyChanged += new PropertyEventHandler(this.OnDiagramPropertyChanged);
			this.Diagram.View.PropertyChanged += new PropertyEventHandler(this.OnDiagramPropertyChanged);
			this.Text = model.Name;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public Syncfusion.Windows.Forms.Diagram.Controls.Diagram Diagram
		{
			get
			{
				return diagram;
			}
		}

		public SymbolModel Model
		{
			get
			{
				return this.Diagram.Model as SymbolModel;
			}
		}

		private PropertyEditor PropertyEditor
		{
			get
			{
				if (this.MdiParent != null)
				{
					MainForm mainForm = this.MdiParent as MainForm;
					if (mainForm != null)
					{
						return mainForm.PropertyEditor;
					}
				}
				return null;
			}
		}

		protected void OnDiagramToolActivate(object sender, Controller.ToolEventArgs evtArgs)
		{
#if false
			this.diagram.DetachContextMenu();
#endif
			this.CheckTool(evtArgs.Tool.Name);
		}

		protected void OnDiagramToolDeactivate(object sender, Controller.ToolEventArgs evtArgs)
		{
			this.UncheckTool(evtArgs.Tool.Name);
#if false
			this.diagram.AttachContextMenu();
#endif
		}

		private void CheckTool(string toolName)
		{
			int barIdx;
			int barItemIdx;
			Syncfusion.Windows.Forms.Tools.XPMenus.Bar curBar;
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem curBarItem;

			for (barIdx = 0; barIdx < childFrameBarManager.Bars.Count; barIdx++)
			{
				curBar = childFrameBarManager.Bars[barIdx];
				for (barItemIdx = 0; barItemIdx < curBar.Items.Count; barItemIdx++)
				{
					curBarItem = curBar.Items[barItemIdx];
					if ((string) curBarItem.Tag == toolName)
					{
						curBarItem.Checked = true;
					}
				}
			}
		}

		private void UncheckTool(string toolName)
		{
			int barIdx;
			int barItemIdx;
			Syncfusion.Windows.Forms.Tools.XPMenus.Bar curBar;
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem curBarItem;

			for (barIdx = 0; barIdx < childFrameBarManager.Bars.Count; barIdx++)
			{
				curBar = childFrameBarManager.Bars[barIdx];
				for (barItemIdx = 0; barItemIdx < curBar.Items.Count; barItemIdx++)
				{
					curBarItem = curBar.Items[barItemIdx];
					if ((string) curBarItem.Tag == toolName)
					{
						curBarItem.Checked = false;
					}
				}
			}
		}

		private void UncheckAll()
		{
			int barIdx;
			int barItemIdx;
			Syncfusion.Windows.Forms.Tools.XPMenus.Bar curBar;
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem curBarItem;

			for (barIdx = 0; barIdx < childFrameBarManager.Bars.Count; barIdx++)
			{
				curBar = childFrameBarManager.Bars[barIdx];
				for (barItemIdx = 0; barItemIdx < curBar.Items.Count; barItemIdx++)
				{
					curBarItem = curBar.Items[barItemIdx];
					curBarItem.Checked = false;
				}
			}
		}

		private void EnableTool(string toolName)
		{
			int barIdx;
			int barItemIdx;
			Syncfusion.Windows.Forms.Tools.XPMenus.Bar curBar;
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem curBarItem;

			for (barIdx = 0; barIdx < childFrameBarManager.Bars.Count; barIdx++)
			{
				curBar = childFrameBarManager.Bars[barIdx];
				for (barItemIdx = 0; barItemIdx < curBar.Items.Count; barItemIdx++)
				{
					curBarItem = curBar.Items[barItemIdx];
					if ((string) curBarItem.Tag == toolName)
					{
						curBarItem.Enabled = true;
					}
				}
			}
		}

		private void DisableTool(string toolName)
		{
			int barIdx;
			int barItemIdx;
			Syncfusion.Windows.Forms.Tools.XPMenus.Bar curBar;
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem curBarItem;

			for (barIdx = 0; barIdx < childFrameBarManager.Bars.Count; barIdx++)
			{
				curBar = childFrameBarManager.Bars[barIdx];
				for (barItemIdx = 0; barItemIdx < curBar.Items.Count; barItemIdx++)
				{
					curBarItem = curBar.Items[barItemIdx];
					if ((string) curBarItem.Tag == toolName)
					{
						curBarItem.Enabled = false;
					}
				}
			}
		}

		protected void OnDiagramPropertyChanged(object sender, PropertyEventArgs evtArgs)
		{
			if (evtArgs.Node == this.Model && evtArgs.PropertyName == "Name")
			{
				this.Text = evtArgs.Node.Name;
			}
			else if (evtArgs.PropertyName == "GridVisible")
			{
				this.barItemShowGrid.Checked = this.Diagram.View.Grid.Visible;
			}
			else if (evtArgs.PropertyName == "SnapToGrid")
			{
				this.barItemSnapToGrid.Checked = this.Diagram.View.Grid.SnapToGrid;
			}
		}

		protected void OnSelectionChanged(object sender, NodeCollection.EventArgs evtArgs)
		{
			int numSelected = this.Diagram.Controller.SelectionList.Count;

#if false
			IToolUIManager toolMgr = this.MdiParent as IToolUIManager;
			if (toolMgr != null)
			{
				if (numSelected == 0)
				{
					toolMgr.DisableTool("Cut");
					toolMgr.DisableTool("Copy");
				}
				else
				{
					toolMgr.EnableTool("Cut");
					toolMgr.EnableTool("Copy");
				}
			}
#endif
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SymbolDocument));
			this.diagram = new Syncfusion.Windows.Forms.Diagram.Controls.Diagram();
			this.diagramContextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItemProperties = new System.Windows.Forms.MenuItem();
			this.menuItemShowGrid = new System.Windows.Forms.MenuItem();
			this.childFrameBarManager = new Syncfusion.Windows.Forms.Tools.XPMenus.ChildFrameBarManager(this.components, this);
			this.barDrawing = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Drawing");
			this.barItemSelectTool = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.smBarItemImages = new System.Windows.Forms.ImageList(this.components);
			this.barItemLine = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemOrthogonalLine = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemRectangle = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemRoundRect = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEllipse = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemText = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemPolyline = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemPolygon = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemArc = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemPolycurve = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemClosedCurve = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemPort = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemImage = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEditVertices = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barRotate = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Rotate");
			this.barItemRotate = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemRotateLeft = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemRotateRight = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemFlipVertical = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemFlipHorizontal = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barNode = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Node");
			this.barItemGroup = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemUngroup = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemBringToFront = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSendToBack = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemBringForward = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSendBackward = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barView = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "View");
			this.barItemPan = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemShowGrid = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSnapToGrid = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemZoom = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.comboBoxBarItemMagnification = new Syncfusion.Windows.Forms.Tools.XPMenus.ComboBoxBarItem();
			this.barNudge = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Nudge");
			this.barItemNudgeUp = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemNudgeDown = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemNudgeLeft = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemNudgeRight = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.bar1 = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Align");
			this.barItemAlignLeft = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemAlignCenter = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemAlignRight = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemAlignTop = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemAlignMiddle = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemAlignBottom = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.bar2 = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Layout");
			this.barItemSpaceAcross = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSpaceDown = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSameWidth = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSameHeight = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSameSize = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			((System.ComponentModel.ISupportInitialize)(this.childFrameBarManager)).BeginInit();
			this.SuspendLayout();
			// 
			// diagram
			// 
			this.diagram.AllowDrop = true;
			this.diagram.ContextMenu = this.diagramContextMenu;
			// 
			// diagram.Controller
			// 
			this.diagram.Controller.MaxHistory = 256;
			this.diagram.Controller.SelectHandleMode = Syncfusion.Windows.Forms.Diagram.SelectHandleType.Resize;
			this.diagram.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagram.HScroll = true;
			this.diagram.LayoutManager = null;
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
			this.diagram.NudgeIncrement = 10F;
			this.diagram.ScrollGranularity = 0.5F;
			this.diagram.Size = new System.Drawing.Size(544, 382);
			this.diagram.TabIndex = 0;
			// 
			// diagram.View
			// 
			this.diagram.View.BackgroundColor = System.Drawing.Color.SteelBlue;
			this.diagram.View.Grid.Color = System.Drawing.Color.Black;
			this.diagram.View.Grid.HorizontalSpacing = 12F;
			this.diagram.View.Grid.SnapToGrid = true;
			this.diagram.View.Grid.VerticalSpacing = 12F;
			this.diagram.View.Grid.Visible = true;
			this.diagram.View.HandleAnchorColor = System.Drawing.Color.LightGray;
			this.diagram.View.HandleColor = System.Drawing.Color.White;
			this.diagram.View.HandleSize = 6;
			this.diagram.View.ShowPageBounds = true;
			this.diagram.View.MagnificationChanged += new Syncfusion.Windows.Forms.Diagram.ViewMagnificationEventHandler(this.diagram_View_MagnificationChanged);
			this.diagram.VScroll = true;
			this.diagram.MouseMove += new System.Windows.Forms.MouseEventHandler(this.diagram_MouseMove);
			// 
			// diagramContextMenu
			// 
			this.diagramContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							   this.menuItemProperties,
																							   this.menuItemShowGrid});
			// 
			// menuItemProperties
			// 
			this.menuItemProperties.Index = 0;
			this.menuItemProperties.Text = "Properties";
			this.menuItemProperties.Click += new System.EventHandler(this.menuItemProperties_Click);
			// 
			// menuItemShowGrid
			// 
			this.menuItemShowGrid.Index = 1;
			this.menuItemShowGrid.Text = "Show Grid";
			this.menuItemShowGrid.Click += new System.EventHandler(this.menuItemShowGrid_Click);
			// 
			// childFrameBarManager
			// 
			this.childFrameBarManager.BarPositionInfo = ((System.IO.MemoryStream)(resources.GetObject("childFrameBarManager.BarPositionInfo")));
			this.childFrameBarManager.Bars.Add(this.barDrawing);
			this.childFrameBarManager.Bars.Add(this.barRotate);
			this.childFrameBarManager.Bars.Add(this.barNode);
			this.childFrameBarManager.Bars.Add(this.barView);
			this.childFrameBarManager.Bars.Add(this.barNudge);
			this.childFrameBarManager.Bars.Add(this.bar1);
			this.childFrameBarManager.Bars.Add(this.bar2);
			this.childFrameBarManager.Categories.Add("Drawing Tools");
			this.childFrameBarManager.Categories.Add("Rotate Tools");
			this.childFrameBarManager.Categories.Add("Node Tools");
			this.childFrameBarManager.Categories.Add("View Tools");
			this.childFrameBarManager.Categories.Add("Nudge Tools");
			this.childFrameBarManager.Categories.Add("Align Tools");
			this.childFrameBarManager.Categories.Add("Layout Tools");
			this.childFrameBarManager.CurrentBaseFormType = "System.Windows.Forms.Form";
			this.childFrameBarManager.Form = this;
			this.childFrameBarManager.ImageList = null;
			this.childFrameBarManager.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																											  this.barItemSelectTool,
																											  this.barItemLine,
																											  this.barItemRectangle,
																											  this.barItemRoundRect,
																											  this.barItemAlignLeft,
																											  this.barItemEllipse,
																											  this.barItemText,
																											  this.barItemPolyline,
																											  this.barItemPolygon,
																											  this.barItemAlignCenter,
																											  this.barItemArc,
																											  this.barItemPolycurve,
																											  this.barItemClosedCurve,
																											  this.barItemPort,
																											  this.barItemAlignRight,
																											  this.barItemImage,
																											  this.barItemAlignTop,
																											  this.barItemAlignMiddle,
																											  this.barItemAlignBottom,
																											  this.barItemRotate,
																											  this.barItemRotateLeft,
																											  this.barItemRotateRight,
																											  this.barItemFlipVertical,
																											  this.barItemFlipHorizontal,
																											  this.barItemGroup,
																											  this.barItemUngroup,
																											  this.barItemBringToFront,
																											  this.barItemSendToBack,
																											  this.barItemBringForward,
																											  this.barItemSendBackward,
																											  this.barItemPan,
																											  this.barItemNudgeUp,
																											  this.barItemNudgeDown,
																											  this.barItemNudgeLeft,
																											  this.barItemNudgeRight,
																											  this.barItemShowGrid,
																											  this.barItemSnapToGrid,
																											  this.barItemSpaceAcross,
																											  this.barItemSpaceDown,
																											  this.barItemSameWidth,
																											  this.barItemSameHeight,
																											  this.barItemSameSize,
																											  this.barItemZoom,
																											  this.comboBoxBarItemMagnification,
																											  this.barItemEditVertices,
																											  this.barItemOrthogonalLine});
			this.childFrameBarManager.LargeImageList = null;
			// 
			// barDrawing
			// 
			this.barDrawing.BarName = "Drawing";
			this.barDrawing.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																									this.barItemSelectTool,
																									this.barItemLine,
																									this.barItemOrthogonalLine,
																									this.barItemRectangle,
																									this.barItemRoundRect,
																									this.barItemEllipse,
																									this.barItemText,
																									this.barItemPolyline,
																									this.barItemPolygon,
																									this.barItemArc,
																									this.barItemPolycurve,
																									this.barItemClosedCurve,
																									this.barItemPort,
																									this.barItemImage,
																									this.barItemEditVertices});
			this.barDrawing.Manager = this.childFrameBarManager;
			// 
			// barItemSelectTool
			// 
			this.barItemSelectTool.CategoryIndex = 0;
			this.barItemSelectTool.ID = "Select";
			this.barItemSelectTool.ImageIndex = 0;
			this.barItemSelectTool.ImageList = this.smBarItemImages;
			this.barItemSelectTool.Tag = "SelectTool";
			this.barItemSelectTool.Text = "Select";
			this.barItemSelectTool.Tooltip = "Select";
			this.barItemSelectTool.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// smBarItemImages
			// 
			this.smBarItemImages.ImageSize = new System.Drawing.Size(16, 16);
			this.smBarItemImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smBarItemImages.ImageStream")));
			this.smBarItemImages.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// barItemLine
			// 
			this.barItemLine.CategoryIndex = 0;
			this.barItemLine.ID = "Line";
			this.barItemLine.ImageIndex = 1;
			this.barItemLine.ImageList = this.smBarItemImages;
			this.barItemLine.Tag = "LineTool";
			this.barItemLine.Text = "Line";
			this.barItemLine.Tooltip = "Draw Line";
			this.barItemLine.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemOrthogonalLine
			// 
			this.barItemOrthogonalLine.CategoryIndex = 0;
			this.barItemOrthogonalLine.ID = "Orthogonal Line";
			this.barItemOrthogonalLine.ImageIndex = 44;
			this.barItemOrthogonalLine.ImageList = this.smBarItemImages;
			this.barItemOrthogonalLine.Tag = "OrthogonalLineTool";
			this.barItemOrthogonalLine.Text = "Orthogonal Line";
			this.barItemOrthogonalLine.Tooltip = "Orthogonal Line";
			this.barItemOrthogonalLine.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemRectangle
			// 
			this.barItemRectangle.CategoryIndex = 0;
			this.barItemRectangle.ID = "Rectangle";
			this.barItemRectangle.ImageIndex = 3;
			this.barItemRectangle.ImageList = this.smBarItemImages;
			this.barItemRectangle.Tag = "RectangleTool";
			this.barItemRectangle.Text = "Rectangle";
			this.barItemRectangle.Tooltip = "Draw Rectangle";
			this.barItemRectangle.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemRoundRect
			// 
			this.barItemRoundRect.CategoryIndex = 0;
			this.barItemRoundRect.ID = "RoundRect";
			this.barItemRoundRect.ImageIndex = 42;
			this.barItemRoundRect.ImageList = this.smBarItemImages;
			this.barItemRoundRect.Tag = "RoundRectTool";
			this.barItemRoundRect.Text = "RoundRect";
			this.barItemRoundRect.Tooltip = "Round Rectangle";
			this.barItemRoundRect.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemEllipse
			// 
			this.barItemEllipse.CategoryIndex = 0;
			this.barItemEllipse.ID = "Ellipse";
			this.barItemEllipse.ImageIndex = 4;
			this.barItemEllipse.ImageList = this.smBarItemImages;
			this.barItemEllipse.Tag = "EllipseTool";
			this.barItemEllipse.Text = "Ellipse";
			this.barItemEllipse.Tooltip = "Draw Ellipse";
			this.barItemEllipse.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemText
			// 
			this.barItemText.CategoryIndex = 0;
			this.barItemText.ID = "Text";
			this.barItemText.ImageIndex = 9;
			this.barItemText.ImageList = this.smBarItemImages;
			this.barItemText.Tag = "TextTool";
			this.barItemText.Text = "Text";
			this.barItemText.Tooltip = "Insert Text";
			this.barItemText.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemPolyline
			// 
			this.barItemPolyline.CategoryIndex = 0;
			this.barItemPolyline.ID = "Polyline";
			this.barItemPolyline.ImageIndex = 2;
			this.barItemPolyline.ImageList = this.smBarItemImages;
			this.barItemPolyline.Tag = "PolyLineTool";
			this.barItemPolyline.Text = "Polyline";
			this.barItemPolyline.Tooltip = "Draw Polyline";
			this.barItemPolyline.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemPolygon
			// 
			this.barItemPolygon.CategoryIndex = 0;
			this.barItemPolygon.ID = "Polygon";
			this.barItemPolygon.ImageIndex = 5;
			this.barItemPolygon.ImageList = this.smBarItemImages;
			this.barItemPolygon.Tag = "PolygonTool";
			this.barItemPolygon.Text = "Polygon";
			this.barItemPolygon.Tooltip = "Draw Polygon";
			this.barItemPolygon.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemArc
			// 
			this.barItemArc.CategoryIndex = 0;
			this.barItemArc.ID = "Arc";
			this.barItemArc.ImageIndex = 6;
			this.barItemArc.ImageList = this.smBarItemImages;
			this.barItemArc.Tag = "ArcTool";
			this.barItemArc.Text = "Arc";
			this.barItemArc.Tooltip = "Draw Arc";
			this.barItemArc.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemPolycurve
			// 
			this.barItemPolycurve.CategoryIndex = 0;
			this.barItemPolycurve.ID = "Polycurve";
			this.barItemPolycurve.ImageIndex = 8;
			this.barItemPolycurve.ImageList = this.smBarItemImages;
			this.barItemPolycurve.Tag = "CurveTool";
			this.barItemPolycurve.Text = "Polycurve";
			this.barItemPolycurve.Tooltip = "Draw Polycurve";
			this.barItemPolycurve.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemClosedCurve
			// 
			this.barItemClosedCurve.CategoryIndex = 0;
			this.barItemClosedCurve.ID = "ClosedCurve";
			this.barItemClosedCurve.ImageIndex = 7;
			this.barItemClosedCurve.ImageList = this.smBarItemImages;
			this.barItemClosedCurve.Tag = "ClosedCurveTool";
			this.barItemClosedCurve.Text = "ClosedCurve";
			this.barItemClosedCurve.Tooltip = "Draw Closed Curve";
			this.barItemClosedCurve.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemPort
			// 
			this.barItemPort.CategoryIndex = 0;
			this.barItemPort.ID = "Port";
			this.barItemPort.ImageIndex = 41;
			this.barItemPort.ImageList = this.smBarItemImages;
			this.barItemPort.Tag = "PortTool";
			this.barItemPort.Text = "Port";
			this.barItemPort.Tooltip = "Insert Port";
			this.barItemPort.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemImage
			// 
			this.barItemImage.CategoryIndex = 0;
			this.barItemImage.ID = "Image";
			this.barItemImage.ImageIndex = 10;
			this.barItemImage.ImageList = this.smBarItemImages;
			this.barItemImage.Tag = "ImageTool";
			this.barItemImage.Text = "Image";
			this.barItemImage.Tooltip = "Insert Image";
			this.barItemImage.Click += new System.EventHandler(this.barItemImage_Click);
			// 
			// barItemEditVertices
			// 
			this.barItemEditVertices.CategoryIndex = 0;
			this.barItemEditVertices.ID = "EditVertices";
			this.barItemEditVertices.ImageIndex = 43;
			this.barItemEditVertices.ImageList = this.smBarItemImages;
			this.barItemEditVertices.Tag = "VertexEditTool";
			this.barItemEditVertices.Text = "Edit Vertices";
			this.barItemEditVertices.Tooltip = "Edit Vertices";
			this.barItemEditVertices.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barRotate
			// 
			this.barRotate.BarName = "Rotate";
			this.barRotate.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																								   this.barItemRotate,
																								   this.barItemRotateLeft,
																								   this.barItemRotateRight,
																								   this.barItemFlipVertical,
																								   this.barItemFlipHorizontal});
			this.barRotate.Manager = this.childFrameBarManager;
			// 
			// barItemRotate
			// 
			this.barItemRotate.CategoryIndex = 1;
			this.barItemRotate.ID = "Rotate";
			this.barItemRotate.ImageIndex = 24;
			this.barItemRotate.ImageList = this.smBarItemImages;
			this.barItemRotate.Tag = "RotateTool";
			this.barItemRotate.Text = "Rotate";
			this.barItemRotate.Tooltip = "Rotate";
			this.barItemRotate.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemRotateLeft
			// 
			this.barItemRotateLeft.CategoryIndex = 1;
			this.barItemRotateLeft.ID = "Rotate Left";
			this.barItemRotateLeft.ImageIndex = 26;
			this.barItemRotateLeft.ImageList = this.smBarItemImages;
			this.barItemRotateLeft.Text = "Rotate Left";
			this.barItemRotateLeft.Tooltip = "Rotate Left";
			this.barItemRotateLeft.Click += new System.EventHandler(this.barItemRotateLeft_Click);
			// 
			// barItemRotateRight
			// 
			this.barItemRotateRight.CategoryIndex = 1;
			this.barItemRotateRight.ID = "Rotate Right";
			this.barItemRotateRight.ImageIndex = 25;
			this.barItemRotateRight.ImageList = this.smBarItemImages;
			this.barItemRotateRight.Text = "Rotate Right";
			this.barItemRotateRight.Tooltip = "Rotate Right";
			this.barItemRotateRight.Click += new System.EventHandler(this.barItemRotateRight_Click);
			// 
			// barItemFlipVertical
			// 
			this.barItemFlipVertical.CategoryIndex = 1;
			this.barItemFlipVertical.ID = "Flip Vertical";
			this.barItemFlipVertical.ImageIndex = 28;
			this.barItemFlipVertical.ImageList = this.smBarItemImages;
			this.barItemFlipVertical.Text = "Flip Vertical";
			this.barItemFlipVertical.Tooltip = "Flip Vertical";
			this.barItemFlipVertical.Click += new System.EventHandler(this.barItemFlipVertical_Click);
			// 
			// barItemFlipHorizontal
			// 
			this.barItemFlipHorizontal.CategoryIndex = 1;
			this.barItemFlipHorizontal.ID = "Flip Horizontal";
			this.barItemFlipHorizontal.ImageIndex = 27;
			this.barItemFlipHorizontal.ImageList = this.smBarItemImages;
			this.barItemFlipHorizontal.Text = "Flip Horizontal";
			this.barItemFlipHorizontal.Tooltip = "Flip Horizontal";
			this.barItemFlipHorizontal.Click += new System.EventHandler(this.barItemFlipHorizontal_Click);
			// 
			// barNode
			// 
			this.barNode.BarName = "Node";
			this.barNode.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																								 this.barItemGroup,
																								 this.barItemUngroup,
																								 this.barItemBringToFront,
																								 this.barItemSendToBack,
																								 this.barItemBringForward,
																								 this.barItemSendBackward});
			this.barNode.Manager = this.childFrameBarManager;
			// 
			// barItemGroup
			// 
			this.barItemGroup.CategoryIndex = 2;
			this.barItemGroup.ID = "Group";
			this.barItemGroup.ImageIndex = 15;
			this.barItemGroup.ImageList = this.smBarItemImages;
			this.barItemGroup.Tag = "GroupTool";
			this.barItemGroup.Text = "Group";
			this.barItemGroup.Tooltip = "Group";
			this.barItemGroup.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemUngroup
			// 
			this.barItemUngroup.CategoryIndex = 2;
			this.barItemUngroup.ID = "Ungroup";
			this.barItemUngroup.ImageIndex = 16;
			this.barItemUngroup.ImageList = this.smBarItemImages;
			this.barItemUngroup.Tag = "UngroupTool";
			this.barItemUngroup.Text = "Ungroup";
			this.barItemUngroup.Tooltip = "Ungroup";
			this.barItemUngroup.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemBringToFront
			// 
			this.barItemBringToFront.CategoryIndex = 2;
			this.barItemBringToFront.ID = "Bring to Front";
			this.barItemBringToFront.ImageIndex = 12;
			this.barItemBringToFront.ImageList = this.smBarItemImages;
			this.barItemBringToFront.Text = "Bring to Front";
			this.barItemBringToFront.Tooltip = "Bring to Front";
			this.barItemBringToFront.Click += new System.EventHandler(this.barItemBringToFront_Click);
			// 
			// barItemSendToBack
			// 
			this.barItemSendToBack.CategoryIndex = 2;
			this.barItemSendToBack.ID = "Send to Back";
			this.barItemSendToBack.ImageIndex = 14;
			this.barItemSendToBack.ImageList = this.smBarItemImages;
			this.barItemSendToBack.Text = "Send to Back";
			this.barItemSendToBack.Tooltip = "Send to Back";
			this.barItemSendToBack.Click += new System.EventHandler(this.barItemSendToBack_Click);
			// 
			// barItemBringForward
			// 
			this.barItemBringForward.CategoryIndex = 2;
			this.barItemBringForward.ID = "Bring Forward";
			this.barItemBringForward.ImageIndex = 11;
			this.barItemBringForward.ImageList = this.smBarItemImages;
			this.barItemBringForward.Text = "Bring Forward";
			this.barItemBringForward.Tooltip = "Bring Forward";
			this.barItemBringForward.Click += new System.EventHandler(this.barItemBringForward_Click);
			// 
			// barItemSendBackward
			// 
			this.barItemSendBackward.CategoryIndex = 2;
			this.barItemSendBackward.ID = "Send Backward";
			this.barItemSendBackward.ImageIndex = 13;
			this.barItemSendBackward.ImageList = this.smBarItemImages;
			this.barItemSendBackward.Text = "Send Backward";
			this.barItemSendBackward.Tooltip = "Send Backward";
			this.barItemSendBackward.Click += new System.EventHandler(this.barItemSendBackward_Click);
			// 
			// barView
			// 
			this.barView.BarName = "View";
			this.barView.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																								 this.barItemPan,
																								 this.barItemShowGrid,
																								 this.barItemSnapToGrid,
																								 this.barItemZoom,
																								 this.comboBoxBarItemMagnification});
			this.barView.Manager = this.childFrameBarManager;
			// 
			// barItemPan
			// 
			this.barItemPan.CategoryIndex = 3;
			this.barItemPan.ID = "Pan";
			this.barItemPan.ImageIndex = 33;
			this.barItemPan.ImageList = this.smBarItemImages;
			this.barItemPan.Tag = "PanTool";
			this.barItemPan.Text = "Pan";
			this.barItemPan.Tooltip = "Pan";
			this.barItemPan.Click += new System.EventHandler(this.barItemTool_Click);
			// 
			// barItemShowGrid
			// 
			this.barItemShowGrid.CategoryIndex = 3;
			this.barItemShowGrid.ID = "Show Grid";
			this.barItemShowGrid.ImageIndex = 17;
			this.barItemShowGrid.ImageList = this.smBarItemImages;
			this.barItemShowGrid.Text = "Show Grid";
			this.barItemShowGrid.Tooltip = "Show Grid";
			this.barItemShowGrid.Click += new System.EventHandler(this.barItemShowGrid_Click);
			// 
			// barItemSnapToGrid
			// 
			this.barItemSnapToGrid.CategoryIndex = 3;
			this.barItemSnapToGrid.ID = "Snap To Grid";
			this.barItemSnapToGrid.ImageIndex = 18;
			this.barItemSnapToGrid.ImageList = this.smBarItemImages;
			this.barItemSnapToGrid.Text = "Snap To Grid";
			this.barItemSnapToGrid.Tooltip = "Snap To Grid";
			this.barItemSnapToGrid.Click += new System.EventHandler(this.barItemSnapToGrid_Click);
			// 
			// barItemZoom
			// 
			this.barItemZoom.CategoryIndex = 3;
			this.barItemZoom.ID = "Zoom";
			this.barItemZoom.ImageIndex = 34;
			this.barItemZoom.ImageList = this.smBarItemImages;
			this.barItemZoom.Tag = "ZoomTool";
			this.barItemZoom.Text = "Zoom";
			this.barItemZoom.Tooltip = "Zoom";
			this.barItemZoom.Click += new System.EventHandler(this.barItemZoom_Click);
			// 
			// comboBoxBarItemMagnification
			// 
			this.comboBoxBarItemMagnification.CategoryIndex = 3;
			this.comboBoxBarItemMagnification.ChoiceList.AddRange(new string[] {
																				   "25%",
																				   "50%",
																				   "75%",
																				   "100%",
																				   "125%",
																				   "150%",
																				   "200%"});
			this.comboBoxBarItemMagnification.Editable = false;
			this.comboBoxBarItemMagnification.ID = "Magnification";
			this.comboBoxBarItemMagnification.Text = "Magnification";
			this.comboBoxBarItemMagnification.Click += new System.EventHandler(this.comboBoxBarItemMagnification_Click);
			// 
			// barNudge
			// 
			this.barNudge.BarName = "Nudge";
			this.barNudge.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																								  this.barItemNudgeUp,
																								  this.barItemNudgeDown,
																								  this.barItemNudgeLeft,
																								  this.barItemNudgeRight});
			this.barNudge.Manager = this.childFrameBarManager;
			// 
			// barItemNudgeUp
			// 
			this.barItemNudgeUp.CategoryIndex = 4;
			this.barItemNudgeUp.ID = "Nudge Up";
			this.barItemNudgeUp.ImageIndex = 31;
			this.barItemNudgeUp.ImageList = this.smBarItemImages;
			this.barItemNudgeUp.Text = "Nudge Up";
			this.barItemNudgeUp.Tooltip = "Nudge Up";
			this.barItemNudgeUp.Click += new System.EventHandler(this.barItemNudgeUp_Click);
			// 
			// barItemNudgeDown
			// 
			this.barItemNudgeDown.CategoryIndex = 4;
			this.barItemNudgeDown.ID = "Nudge Down";
			this.barItemNudgeDown.ImageIndex = 32;
			this.barItemNudgeDown.ImageList = this.smBarItemImages;
			this.barItemNudgeDown.Text = "Nudge Down";
			this.barItemNudgeDown.Tooltip = "Nudge Down";
			this.barItemNudgeDown.Click += new System.EventHandler(this.barItemNudgeDown_Click);
			// 
			// barItemNudgeLeft
			// 
			this.barItemNudgeLeft.CategoryIndex = 4;
			this.barItemNudgeLeft.ID = "Nudge Left";
			this.barItemNudgeLeft.ImageIndex = 30;
			this.barItemNudgeLeft.ImageList = this.smBarItemImages;
			this.barItemNudgeLeft.Text = "Nudge Left";
			this.barItemNudgeLeft.Tooltip = "Nudge Left";
			this.barItemNudgeLeft.Click += new System.EventHandler(this.barItemNudgeLeft_Click);
			// 
			// barItemNudgeRight
			// 
			this.barItemNudgeRight.CategoryIndex = 4;
			this.barItemNudgeRight.ID = "Nudge Right";
			this.barItemNudgeRight.ImageIndex = 29;
			this.barItemNudgeRight.ImageList = this.smBarItemImages;
			this.barItemNudgeRight.Text = "Nudge Right";
			this.barItemNudgeRight.Tooltip = "Nudge Right";
			this.barItemNudgeRight.Click += new System.EventHandler(this.barItemNudgeRight_Click);
			// 
			// bar1
			// 
			this.bar1.BarName = "Align";
			this.bar1.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																							  this.barItemAlignLeft,
																							  this.barItemAlignCenter,
																							  this.barItemAlignRight,
																							  this.barItemAlignTop,
																							  this.barItemAlignMiddle,
																							  this.barItemAlignBottom});
			this.bar1.Manager = this.childFrameBarManager;
			// 
			// barItemAlignLeft
			// 
			this.barItemAlignLeft.CategoryIndex = 5;
			this.barItemAlignLeft.ID = "Align Left";
			this.barItemAlignLeft.ImageIndex = 35;
			this.barItemAlignLeft.ImageList = this.smBarItemImages;
			this.barItemAlignLeft.Text = "Align Left";
			this.barItemAlignLeft.Tooltip = "Align Left";
			this.barItemAlignLeft.Click += new System.EventHandler(this.barItemAlignLeft_Click);
			// 
			// barItemAlignCenter
			// 
			this.barItemAlignCenter.CategoryIndex = 5;
			this.barItemAlignCenter.ID = "Align Center";
			this.barItemAlignCenter.ImageIndex = 36;
			this.barItemAlignCenter.ImageList = this.smBarItemImages;
			this.barItemAlignCenter.Text = "Align Center";
			this.barItemAlignCenter.Tooltip = "Align Center";
			this.barItemAlignCenter.Click += new System.EventHandler(this.barItemAlignCenter_Click);
			// 
			// barItemAlignRight
			// 
			this.barItemAlignRight.CategoryIndex = 5;
			this.barItemAlignRight.ID = "Align Right";
			this.barItemAlignRight.ImageIndex = 37;
			this.barItemAlignRight.ImageList = this.smBarItemImages;
			this.barItemAlignRight.Text = "Align Right";
			this.barItemAlignRight.Tooltip = "Align Right";
			this.barItemAlignRight.Click += new System.EventHandler(this.barItemAlignRight_Click);
			// 
			// barItemAlignTop
			// 
			this.barItemAlignTop.CategoryIndex = 5;
			this.barItemAlignTop.ID = "Align Top";
			this.barItemAlignTop.ImageIndex = 38;
			this.barItemAlignTop.ImageList = this.smBarItemImages;
			this.barItemAlignTop.Text = "Align Top";
			this.barItemAlignTop.Tooltip = "Align Top";
			this.barItemAlignTop.Click += new System.EventHandler(this.barItemAlignTop_Click);
			// 
			// barItemAlignMiddle
			// 
			this.barItemAlignMiddle.CategoryIndex = 5;
			this.barItemAlignMiddle.ID = "Align Middle";
			this.barItemAlignMiddle.ImageIndex = 39;
			this.barItemAlignMiddle.ImageList = this.smBarItemImages;
			this.barItemAlignMiddle.Text = "Align Middle";
			this.barItemAlignMiddle.Tooltip = "Align Middle";
			this.barItemAlignMiddle.Click += new System.EventHandler(this.barItemAlignMiddle_Click);
			// 
			// barItemAlignBottom
			// 
			this.barItemAlignBottom.CategoryIndex = 5;
			this.barItemAlignBottom.ID = "Align Bottom";
			this.barItemAlignBottom.ImageIndex = 40;
			this.barItemAlignBottom.ImageList = this.smBarItemImages;
			this.barItemAlignBottom.Text = "Align Bottom";
			this.barItemAlignBottom.Tooltip = "Align Bottom";
			this.barItemAlignBottom.Click += new System.EventHandler(this.barItemAlignBottom_Click);
			// 
			// bar2
			// 
			this.bar2.BarName = "Layout";
			this.bar2.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																							  this.barItemSpaceAcross,
																							  this.barItemSpaceDown,
																							  this.barItemSameWidth,
																							  this.barItemSameHeight,
																							  this.barItemSameSize});
			this.bar2.Manager = this.childFrameBarManager;
			// 
			// barItemSpaceAcross
			// 
			this.barItemSpaceAcross.CategoryIndex = 6;
			this.barItemSpaceAcross.ID = "Space Across";
			this.barItemSpaceAcross.ImageIndex = 22;
			this.barItemSpaceAcross.ImageList = this.smBarItemImages;
			this.barItemSpaceAcross.Text = "Space Across";
			this.barItemSpaceAcross.Tooltip = "Space Across";
			this.barItemSpaceAcross.Click += new System.EventHandler(this.barItemSpaceAcross_Click);
			// 
			// barItemSpaceDown
			// 
			this.barItemSpaceDown.CategoryIndex = 6;
			this.barItemSpaceDown.ID = "Space Down";
			this.barItemSpaceDown.ImageIndex = 23;
			this.barItemSpaceDown.ImageList = this.smBarItemImages;
			this.barItemSpaceDown.Text = "Space Down";
			this.barItemSpaceDown.Tooltip = "Space Down";
			this.barItemSpaceDown.Click += new System.EventHandler(this.barItemSpaceDown_Click);
			// 
			// barItemSameWidth
			// 
			this.barItemSameWidth.CategoryIndex = 6;
			this.barItemSameWidth.ID = "Space Width";
			this.barItemSameWidth.ImageIndex = 19;
			this.barItemSameWidth.ImageList = this.smBarItemImages;
			this.barItemSameWidth.Text = "Same Width";
			this.barItemSameWidth.Tooltip = "Same Width";
			// 
			// barItemSameHeight
			// 
			this.barItemSameHeight.CategoryIndex = 6;
			this.barItemSameHeight.ID = "Same Height";
			this.barItemSameHeight.ImageIndex = 20;
			this.barItemSameHeight.ImageList = this.smBarItemImages;
			this.barItemSameHeight.Text = "Same Height";
			this.barItemSameHeight.Tooltip = "Same Height";
			// 
			// barItemSameSize
			// 
			this.barItemSameSize.CategoryIndex = 6;
			this.barItemSameSize.ID = "Same Size";
			this.barItemSameSize.ImageIndex = 21;
			this.barItemSameSize.ImageList = this.smBarItemImages;
			this.barItemSameSize.Text = "Same Size";
			this.barItemSameSize.Tooltip = "Same Size";
			// 
			// SymbolDocument
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(544, 382);
			this.Controls.Add(this.diagram);
			this.Name = "SymbolDocument";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "SymbolDocument";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SymbolDocument_Closing);
			this.Load += new System.EventHandler(this.SymbolDocument_Load);
			((System.ComponentModel.ISupportInitialize)(this.childFrameBarManager)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void barItemTool_Click(object sender, System.EventArgs e)
		{
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem = sender as Syncfusion.Windows.Forms.Tools.XPMenus.BarItem;

			if (barItem != null)
			{
				string toolName = (string) barItem.Tag;
				this.Diagram.ActivateTool(toolName);
			}
		}

		private void barItemBringToFront_Click(object sender, System.EventArgs e)
		{
			this.Diagram.Controller.BringToFront();
		}

		private void barItemSendToBack_Click(object sender, System.EventArgs e)
		{
			this.Diagram.Controller.SendToBack();
		}

		private void barItemBringForward_Click(object sender, System.EventArgs e)
		{
			this.Diagram.Controller.BringForward();
		}

		private void barItemSendBackward_Click(object sender, System.EventArgs e)
		{
			this.Diagram.Controller.SendBackward();
		}

		private void barItemImage_Click(object sender, System.EventArgs e)
		{
			this.Diagram.InsertImage();
		}

		private void barItemNudgeUp_Click(object sender, System.EventArgs e)
		{
			this.Diagram.NudgeUp();
		}

		private void barItemNudgeDown_Click(object sender, System.EventArgs e)
		{
			this.Diagram.NudgeDown();
		}

		private void barItemNudgeLeft_Click(object sender, System.EventArgs e)
		{
			this.Diagram.NudgeLeft();
		}

		private void barItemNudgeRight_Click(object sender, System.EventArgs e)
		{
			this.Diagram.NudgeRight();
		}

		private void barItemShowGrid_Click(object sender, System.EventArgs e)
		{
			this.barItemShowGrid.Checked = !this.barItemShowGrid.Checked;
			this.Diagram.View.Grid.Visible = this.barItemShowGrid.Checked;
		}

		private void barItemSnapToGrid_Click(object sender, System.EventArgs e)
		{
			this.barItemSnapToGrid.Checked = !this.barItemSnapToGrid.Checked;
			this.Diagram.View.Grid.SnapToGrid = this.barItemSnapToGrid.Checked;
		}

		private void SymbolDocument_Load(object sender, System.EventArgs e)
		{
			this.barItemShowGrid.Checked = this.diagram.View.Grid.Visible;
			this.menuItemShowGrid.Checked = this.diagram.View.Grid.Visible;
			this.barItemSnapToGrid.Checked = this.diagram.View.Grid.SnapToGrid;
			UpdateMagnificationComboBox();

			PropertyEditor propEditor = this.PropertyEditor;
			if (propEditor != null)
			{
				propEditor.Diagram = this.diagram;
			}
		}

		private void barItemRotateLeft_Click(object sender, System.EventArgs e)
		{
			this.Diagram.Rotate(-90);
		}

		private void barItemRotateRight_Click(object sender, System.EventArgs e)
		{
			this.Diagram.Rotate(90);
		}

		private void barItemFlipVertical_Click(object sender, System.EventArgs e)
		{
			this.Diagram.FlipVertical();
		}

		private void barItemFlipHorizontal_Click(object sender, System.EventArgs e)
		{
			this.Diagram.FlipHorizontal();
		}

		private void menuItemProperties_Click(object sender, System.EventArgs e)
		{
		}

		private void menuItemShowGrid_Click(object sender, System.EventArgs e)
		{
			this.menuItemShowGrid.Checked = this.barItemShowGrid.Checked = !menuItemShowGrid.Checked;
			this.Diagram.View.Grid.Visible = menuItemShowGrid.Checked;
		}

		private void barItemAlignLeft_Click(object sender, System.EventArgs e)
		{
			this.Diagram.AlignLeft();
		}

		private void barItemAlignCenter_Click(object sender, System.EventArgs e)
		{
			this.Diagram.AlignCenter();
		}

		private void barItemAlignRight_Click(object sender, System.EventArgs e)
		{
			this.Diagram.AlignRight();
		}

		private void barItemAlignTop_Click(object sender, System.EventArgs e)
		{
			this.Diagram.AlignTop();
		}

		private void barItemAlignMiddle_Click(object sender, System.EventArgs e)
		{
			this.Diagram.AlignMiddle();
		}

		private void barItemAlignBottom_Click(object sender, System.EventArgs e)
		{
			this.Diagram.AlignBottom();
		}

		private void barItemSpaceAcross_Click(object sender, System.EventArgs e)
		{
			this.Diagram.SpaceAcross();
		}

		private void barItemSpaceDown_Click(object sender, System.EventArgs e)
		{
			this.Diagram.SpaceDown();
		}

		private void diagram_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (this.MdiParent != null)
			{
				IStatusUpdate statusUpdate = this.MdiParent as IStatusUpdate;
				if (statusUpdate != null)
				{
					if (this.diagram != null && this.diagram.View != null)
					{
						Syncfusion.Windows.Forms.Diagram.View view = this.diagram.View;
						System.Drawing.PointF pos = view.ViewToWorld(view.DeviceToView(new System.Drawing.Point(e.X, e.Y)));
						statusUpdate.SetXY(pos.X, pos.Y);
					}
				}
			}
		}

		private void barItemZoom_Click(object sender, System.EventArgs e)
		{
			this.Diagram.ActivateTool("ZoomTool");
		}

		private void diagram_View_MagnificationChanged(object sender, ViewMagnificationEventArgs evtArgs)
		{
			UpdateMagnificationComboBox();
		}

		private void UpdateMagnificationComboBox()
		{
			System.Drawing.Size szMag = this.Diagram.View.Magnification;
			this.comboBoxBarItemMagnification.TextBoxValue = szMag.Width + "%";
		}

		private void comboBoxBarItemMagnification_Click(object sender, System.EventArgs e)
		{
			string strMagValue = this.comboBoxBarItemMagnification.TextBoxValue;
			int idxPctSign = strMagValue.IndexOf('%');
			if (idxPctSign >= 0)
			{
				strMagValue = strMagValue.Remove(idxPctSign, 1);
			}
			int magVal = System.Convert.ToInt32(strMagValue);
			this.Diagram.View.Magnification = new System.Drawing.Size(magVal, magVal);
			this.Diagram.Refresh();
		}

		private void SymbolDocument_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			PropertyEditor propEditor = this.PropertyEditor;
			if (propEditor != null)
			{
				propEditor.Diagram = null;
			}
		}
	}
}
