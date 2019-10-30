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
using System.IO;

using Syncfusion.Windows.Forms.Diagram;
using Syncfusion.Windows.Forms.Diagram.Controls;

namespace Syncfusion.Windows.Forms.Diagram.Samples.DiagramTool
{
	/// <summary>
	/// Summary description for DiagramForm.
	/// </summary>
	public class DiagramForm : System.Windows.Forms.Form
	{
		private Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagramComponent;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ChildFrameBarManager childFrameBarManager;
		private System.Windows.Forms.ImageList smBarItemImages;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSelect;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemLine;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemRectangle;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEllipse;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemText;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemPolyline;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemPolygon;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemArc;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemCurve;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemClosedCurve;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemImage;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemGroup;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemUngroup;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemBringToFront;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSendToBack;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemBringForward;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSendBackward;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemNudgeUp;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemNudgeDown;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemNudgeLeft;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemNudgeRight;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemRotate;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemRotateLeft;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemRotateRight;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemFlipVertical;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemFlipHorizontal;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barDrawing;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barNode;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barNudge;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barRotate;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemPan;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar bar1;
		private System.ComponentModel.IContainer components;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemShowGrid;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemSnapToGrid;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemZoom;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ComboBoxBarItem comboBoxBarItemMagnification;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemOrthogonalLink;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditVertices;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemLink;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barLinks;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemDirectedLink;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemArcLink;
		private string fileName = null;

		public DiagramForm()
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
				return this.diagramComponent;
			}
		}

		public void OpenFile(string fileName)
		{
			FileStream iStream = new FileStream(fileName, FileMode.Open);
			if (iStream != null)
			{
				this.diagramComponent.LoadBinary(iStream);
				iStream.Close();
				this.FileName = fileName;
			}
		}

		public void SaveFile()
		{
			if (!this.HasFileName)
			{
				throw new EInvalidParameter();
			}

			this.SaveAsFile(this.fileName);
		}

		public void SaveAsFile(string fileName)
		{
			FileStream oStream = null;
			
			try
			{
				oStream = new FileStream(fileName, FileMode.Create);
			}
			catch (Exception ex)
			{
				oStream = null;  // just to be sure
				MessageBox.Show("Error opening " + fileName + " - " + ex.Message);
			}

			if (oStream != null)
			{
				try
				{
					this.diagramComponent.SaveBinary(oStream);
					this.FileName = fileName;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Serialization error - " + ex.Message);
				}
				finally
				{
					oStream.Close();
				}
			}
		}

		public string FileName
		{
			get
			{
				return this.fileName;
			}
			set
			{
				this.fileName = value;

				int startIndex = 0;
				int pathPos = this.fileName.LastIndexOf("\\");
				if (pathPos > startIndex)
				{
					startIndex = pathPos + 1;
				}
				pathPos = this.fileName.LastIndexOf("/");
				if (pathPos > startIndex)
				{
					startIndex = pathPos + 1;
				}
				int length;
				int extensionPos = this.fileName.LastIndexOf(".");
				if (extensionPos >= 0)
				{
					length = extensionPos - startIndex;
				}
				else
				{
					length = this.fileName.Length - startIndex;
				}

				this.Text = this.fileName.Substring(startIndex, length);
			}
		}

		public bool HasFileName
		{
			get
			{
				return (this.fileName != null && this.fileName.Length > 0);
			}
		}

		protected PropertyEditor PropertyEditor
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DiagramForm));
			this.diagramComponent = new Syncfusion.Windows.Forms.Diagram.Controls.Diagram();
			this.childFrameBarManager = new Syncfusion.Windows.Forms.Tools.XPMenus.ChildFrameBarManager(this.components, this);
			this.barDrawing = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Drawing");
			this.barItemSelect = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.smBarItemImages = new System.Windows.Forms.ImageList(this.components);
			this.barItemLine = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemRectangle = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEllipse = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemText = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemPolyline = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemPolygon = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemArc = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemCurve = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemClosedCurve = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemImage = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barNode = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Node");
			this.barItemGroup = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemUngroup = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemBringToFront = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSendToBack = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemBringForward = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSendBackward = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barNudge = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Nudge");
			this.barItemNudgeUp = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemNudgeDown = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemNudgeLeft = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemNudgeRight = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barLinks = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Links");
			this.barItemLink = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barRotate = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "Rotate");
			this.barItemRotate = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemRotateLeft = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemRotateRight = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemFlipVertical = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemFlipHorizontal = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.bar1 = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.childFrameBarManager, "View");
			this.barItemPan = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemShowGrid = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemSnapToGrid = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemZoom = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.comboBoxBarItemMagnification = new Syncfusion.Windows.Forms.Tools.XPMenus.ComboBoxBarItem();
			this.barItemOrthogonalLink = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEditVertices = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemDirectedLink = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemArcLink = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			((System.ComponentModel.ISupportInitialize)(this.childFrameBarManager)).BeginInit();
			this.SuspendLayout();
			// 
			// diagramComponent
			// 
			this.diagramComponent.AllowDrop = true;
			// 
			// diagramComponent.Controller
			// 
			this.diagramComponent.Controller.MaxHistory = 256;
			this.diagramComponent.Controller.SelectHandleMode = Syncfusion.Windows.Forms.Diagram.SelectHandleType.Resize;
			this.diagramComponent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagramComponent.HScroll = true;
			this.diagramComponent.LayoutManager = null;
			this.diagramComponent.Location = new System.Drawing.Point(0, 0);
			// 
			// diagramComponent.Model
			// 
			this.diagramComponent.Model.BoundaryConstraintsEnabled = true;
			this.diagramComponent.Model.Height = 1056F;
			this.diagramComponent.Model.MeasurementScale = 1F;
			this.diagramComponent.Model.MeasurementUnits = System.Drawing.GraphicsUnit.Pixel;
			this.diagramComponent.Model.Name = "Model";
			this.diagramComponent.Model.Width = 816F;
			this.diagramComponent.Name = "diagramComponent";
			this.diagramComponent.NudgeIncrement = 1F;
			this.diagramComponent.ScrollGranularity = 0.5F;
			this.diagramComponent.Size = new System.Drawing.Size(560, 414);
			this.diagramComponent.TabIndex = 0;
			this.diagramComponent.Tag = "";
			// 
			// diagramComponent.View
			// 
			this.diagramComponent.View.BackgroundColor = System.Drawing.Color.SteelBlue;
			this.diagramComponent.View.Grid.Color = System.Drawing.Color.Black;
			this.diagramComponent.View.Grid.HorizontalSpacing = 10F;
			this.diagramComponent.View.Grid.SnapToGrid = true;
			this.diagramComponent.View.Grid.VerticalSpacing = 10F;
			this.diagramComponent.View.Grid.Visible = true;
			this.diagramComponent.View.HandleAnchorColor = System.Drawing.Color.LightGray;
			this.diagramComponent.View.HandleColor = System.Drawing.Color.White;
			this.diagramComponent.View.HandleSize = 6;
			this.diagramComponent.View.ShowPageBounds = true;
			this.diagramComponent.View.MagnificationChanged += new Syncfusion.Windows.Forms.Diagram.ViewMagnificationEventHandler(this.diagramComponent_View_MagnificationChanged);
			this.diagramComponent.VScroll = true;
			// 
			// childFrameBarManager
			// 
			this.childFrameBarManager.BarPositionInfo = ((System.IO.MemoryStream)(resources.GetObject("childFrameBarManager.BarPositionInfo")));
			this.childFrameBarManager.Bars.Add(this.barDrawing);
			this.childFrameBarManager.Bars.Add(this.barNode);
			this.childFrameBarManager.Bars.Add(this.barNudge);
			this.childFrameBarManager.Bars.Add(this.barLinks);
			this.childFrameBarManager.Bars.Add(this.barRotate);
			this.childFrameBarManager.Bars.Add(this.bar1);
			this.childFrameBarManager.Categories.Add("Drawing Tools");
			this.childFrameBarManager.Categories.Add("Node Tools");
			this.childFrameBarManager.Categories.Add("Connection Tools");
			this.childFrameBarManager.Categories.Add("Nudge Tools");
			this.childFrameBarManager.Categories.Add("Rotate Tools");
			this.childFrameBarManager.Categories.Add("View Tools");
			this.childFrameBarManager.CurrentBaseFormType = "System.Windows.Forms.Form";
			this.childFrameBarManager.Form = this;
			this.childFrameBarManager.ImageList = null;
			this.childFrameBarManager.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																											  this.barItemSelect,
																											  this.barItemLine,
																											  this.barItemRectangle,
																											  this.barItemEllipse,
																											  this.barItemText,
																											  this.barItemPolyline,
																											  this.barItemPolygon,
																											  this.barItemArc,
																											  this.barItemCurve,
																											  this.barItemClosedCurve,
																											  this.barItemImage,
																											  this.barItemGroup,
																											  this.barItemUngroup,
																											  this.barItemBringToFront,
																											  this.barItemSendToBack,
																											  this.barItemBringForward,
																											  this.barItemSendBackward,
																											  this.barItemLink,
																											  this.barItemNudgeUp,
																											  this.barItemNudgeDown,
																											  this.barItemNudgeLeft,
																											  this.barItemNudgeRight,
																											  this.barItemRotate,
																											  this.barItemRotateLeft,
																											  this.barItemRotateRight,
																											  this.barItemFlipVertical,
																											  this.barItemFlipHorizontal,
																											  this.barItemPan,
																											  this.barItemShowGrid,
																											  this.barItemSnapToGrid,
																											  this.barItemZoom,
																											  this.comboBoxBarItemMagnification,
																											  this.barItemOrthogonalLink,
																											  this.barItemEditVertices,
																											  this.barItemDirectedLink,
																											  this.barItemArcLink});
			this.childFrameBarManager.LargeImageList = null;
			// 
			// barDrawing
			// 
			this.barDrawing.BarName = "Drawing";
			this.barDrawing.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																									this.barItemSelect,
																									this.barItemLine,
																									this.barItemRectangle,
																									this.barItemEllipse,
																									this.barItemText,
																									this.barItemPolyline,
																									this.barItemPolygon,
																									this.barItemArc,
																									this.barItemCurve,
																									this.barItemClosedCurve,
																									this.barItemImage,
																									this.barItemEditVertices});
			this.barDrawing.Manager = this.childFrameBarManager;
			// 
			// barItemSelect
			// 
			this.barItemSelect.CategoryIndex = 0;
			this.barItemSelect.ID = "Pointer";
			this.barItemSelect.ImageIndex = 0;
			this.barItemSelect.ImageList = this.smBarItemImages;
			this.barItemSelect.Tag = "SelectTool";
			this.barItemSelect.Text = "Select";
			this.barItemSelect.Tooltip = "Select";
			this.barItemSelect.Click += new System.EventHandler(this.toolActivate);
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
			this.barItemLine.ImageIndex = 2;
			this.barItemLine.ImageList = this.smBarItemImages;
			this.barItemLine.Tag = "LineTool";
			this.barItemLine.Text = "Line";
			this.barItemLine.Tooltip = "Line";
			this.barItemLine.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemRectangle
			// 
			this.barItemRectangle.CategoryIndex = 0;
			this.barItemRectangle.ID = "Rectangle";
			this.barItemRectangle.ImageIndex = 1;
			this.barItemRectangle.ImageList = this.smBarItemImages;
			this.barItemRectangle.Tag = "RectangleTool";
			this.barItemRectangle.Text = "Rectangle";
			this.barItemRectangle.Tooltip = "Rectangle";
			this.barItemRectangle.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemEllipse
			// 
			this.barItemEllipse.CategoryIndex = 0;
			this.barItemEllipse.ID = "Ellipse";
			this.barItemEllipse.ImageIndex = 4;
			this.barItemEllipse.ImageList = this.smBarItemImages;
			this.barItemEllipse.Tag = "EllipseTool";
			this.barItemEllipse.Text = "Ellipse";
			this.barItemEllipse.Tooltip = "Ellipse";
			this.barItemEllipse.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemText
			// 
			this.barItemText.CategoryIndex = 0;
			this.barItemText.ID = "Text";
			this.barItemText.ImageIndex = 5;
			this.barItemText.ImageList = this.smBarItemImages;
			this.barItemText.Tag = "TextTool";
			this.barItemText.Text = "Text";
			this.barItemText.Tooltip = "Text";
			this.barItemText.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemPolyline
			// 
			this.barItemPolyline.CategoryIndex = 0;
			this.barItemPolyline.ID = "Polyline";
			this.barItemPolyline.ImageIndex = 3;
			this.barItemPolyline.ImageList = this.smBarItemImages;
			this.barItemPolyline.Tag = "PolyLineTool";
			this.barItemPolyline.Text = "Polyline";
			this.barItemPolyline.Tooltip = "Polyline";
			this.barItemPolyline.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemPolygon
			// 
			this.barItemPolygon.CategoryIndex = 0;
			this.barItemPolygon.ID = "Polygon";
			this.barItemPolygon.ImageIndex = 23;
			this.barItemPolygon.ImageList = this.smBarItemImages;
			this.barItemPolygon.Tag = "PolygonTool";
			this.barItemPolygon.Text = "Polygon";
			this.barItemPolygon.Tooltip = "Polygon";
			this.barItemPolygon.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemArc
			// 
			this.barItemArc.CategoryIndex = 0;
			this.barItemArc.ID = "Arc";
			this.barItemArc.ImageIndex = 40;
			this.barItemArc.ImageList = this.smBarItemImages;
			this.barItemArc.Tag = "ArcTool";
			this.barItemArc.Text = "Arc";
			this.barItemArc.Tooltip = "Arc";
			this.barItemArc.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemCurve
			// 
			this.barItemCurve.CategoryIndex = 0;
			this.barItemCurve.ID = "Curve";
			this.barItemCurve.ImageIndex = 22;
			this.barItemCurve.ImageList = this.smBarItemImages;
			this.barItemCurve.Tag = "CurveTool";
			this.barItemCurve.Text = "Curve";
			this.barItemCurve.Tooltip = "Curve";
			this.barItemCurve.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemClosedCurve
			// 
			this.barItemClosedCurve.CategoryIndex = 0;
			this.barItemClosedCurve.ID = "Closed Curve";
			this.barItemClosedCurve.ImageIndex = 21;
			this.barItemClosedCurve.ImageList = this.smBarItemImages;
			this.barItemClosedCurve.Tag = "ClosedCurveTool";
			this.barItemClosedCurve.Text = "Closed Curve";
			this.barItemClosedCurve.Tooltip = "Closed Curve";
			this.barItemClosedCurve.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemImage
			// 
			this.barItemImage.CategoryIndex = 0;
			this.barItemImage.ID = "Image";
			this.barItemImage.ImageIndex = 42;
			this.barItemImage.ImageList = this.smBarItemImages;
			this.barItemImage.Tag = "ImageTool";
			this.barItemImage.Text = "Image";
			this.barItemImage.Tooltip = "Image";
			this.barItemImage.Click += new System.EventHandler(this.barItemImage_Click);
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
			this.barItemGroup.CategoryIndex = 1;
			this.barItemGroup.ID = "Group";
			this.barItemGroup.ImageIndex = 6;
			this.barItemGroup.ImageList = this.smBarItemImages;
			this.barItemGroup.Tag = "GroupTool";
			this.barItemGroup.Text = "Group";
			this.barItemGroup.Tooltip = "Group";
			this.barItemGroup.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemUngroup
			// 
			this.barItemUngroup.CategoryIndex = 1;
			this.barItemUngroup.ID = "Ungroup";
			this.barItemUngroup.ImageIndex = 7;
			this.barItemUngroup.ImageList = this.smBarItemImages;
			this.barItemUngroup.Tag = "UngroupTool";
			this.barItemUngroup.Text = "Ungroup";
			this.barItemUngroup.Tooltip = "Ungroup";
			this.barItemUngroup.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemBringToFront
			// 
			this.barItemBringToFront.CategoryIndex = 1;
			this.barItemBringToFront.ID = "Bring To Front";
			this.barItemBringToFront.ImageIndex = 20;
			this.barItemBringToFront.ImageList = this.smBarItemImages;
			this.barItemBringToFront.Text = "Bring To Front";
			this.barItemBringToFront.Tooltip = "Bring To Front";
			this.barItemBringToFront.Click += new System.EventHandler(this.barItemBringToFront_Click);
			// 
			// barItemSendToBack
			// 
			this.barItemSendToBack.CategoryIndex = 1;
			this.barItemSendToBack.ID = "Send To Back";
			this.barItemSendToBack.ImageIndex = 33;
			this.barItemSendToBack.ImageList = this.smBarItemImages;
			this.barItemSendToBack.Text = "Send To Back";
			this.barItemSendToBack.Click += new System.EventHandler(this.barItemSendToBack_Click);
			// 
			// barItemBringForward
			// 
			this.barItemBringForward.CategoryIndex = 1;
			this.barItemBringForward.ID = "Bring Forward";
			this.barItemBringForward.ImageIndex = 19;
			this.barItemBringForward.ImageList = this.smBarItemImages;
			this.barItemBringForward.Text = "Bring Forward";
			this.barItemBringForward.Click += new System.EventHandler(this.barItemBringForward_Click);
			// 
			// barItemSendBackward
			// 
			this.barItemSendBackward.CategoryIndex = 1;
			this.barItemSendBackward.ID = "Send Backward";
			this.barItemSendBackward.ImageIndex = 32;
			this.barItemSendBackward.ImageList = this.smBarItemImages;
			this.barItemSendBackward.Text = "Send Backward";
			this.barItemSendBackward.Click += new System.EventHandler(this.barItemSendBackward_Click);
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
			this.barItemNudgeUp.CategoryIndex = 3;
			this.barItemNudgeUp.ID = "Nudge Up";
			this.barItemNudgeUp.ImageIndex = 29;
			this.barItemNudgeUp.ImageList = this.smBarItemImages;
			this.barItemNudgeUp.Text = "Nudge Up";
			this.barItemNudgeUp.Tooltip = "Nudge Up";
			this.barItemNudgeUp.Click += new System.EventHandler(this.barItemNudgeUp_Click);
			// 
			// barItemNudgeDown
			// 
			this.barItemNudgeDown.CategoryIndex = 3;
			this.barItemNudgeDown.ID = "Nudge Down";
			this.barItemNudgeDown.ImageIndex = 26;
			this.barItemNudgeDown.ImageList = this.smBarItemImages;
			this.barItemNudgeDown.Text = "Nudge Down";
			this.barItemNudgeDown.Tooltip = "Nudge Down";
			this.barItemNudgeDown.Click += new System.EventHandler(this.barItemNudgeDown_Click);
			// 
			// barItemNudgeLeft
			// 
			this.barItemNudgeLeft.CategoryIndex = 3;
			this.barItemNudgeLeft.ID = "Nudge Left";
			this.barItemNudgeLeft.ImageIndex = 27;
			this.barItemNudgeLeft.ImageList = this.smBarItemImages;
			this.barItemNudgeLeft.Text = "Nudge Left";
			this.barItemNudgeLeft.Tooltip = "Nudge Left";
			this.barItemNudgeLeft.Click += new System.EventHandler(this.barItemNudgeLeft_Click);
			// 
			// barItemNudgeRight
			// 
			this.barItemNudgeRight.CategoryIndex = 3;
			this.barItemNudgeRight.ID = "Nudge Right";
			this.barItemNudgeRight.ImageIndex = 28;
			this.barItemNudgeRight.ImageList = this.smBarItemImages;
			this.barItemNudgeRight.Text = "Nudge Right";
			this.barItemNudgeRight.Tooltip = "Nudge Right";
			this.barItemNudgeRight.Click += new System.EventHandler(this.barItemNudgeRight_Click);
			// 
			// barLinks
			// 
			this.barLinks.BarName = "Links";
			this.barLinks.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																								  this.barItemLink,
																								  this.barItemOrthogonalLink,
																								  this.barItemDirectedLink,
																								  this.barItemArcLink});
			this.barLinks.Manager = this.childFrameBarManager;
			// 
			// barItemLink
			// 
			this.barItemLink.CategoryIndex = 2;
			this.barItemLink.ID = "Link";
			this.barItemLink.ImageIndex = 43;
			this.barItemLink.ImageList = this.smBarItemImages;
			this.barItemLink.Text = "Link";
			this.barItemLink.Tooltip = "Link";
			this.barItemLink.Click += new System.EventHandler(this.barItemLinkSymbols_Click);
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
			this.barItemRotate.CategoryIndex = 4;
			this.barItemRotate.ID = "Rotate";
			this.barItemRotate.ImageIndex = 9;
			this.barItemRotate.ImageList = this.smBarItemImages;
			this.barItemRotate.Tag = "RotateTool";
			this.barItemRotate.Text = "Rotate";
			this.barItemRotate.Tooltip = "Rotate";
			this.barItemRotate.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemRotateLeft
			// 
			this.barItemRotateLeft.CategoryIndex = 4;
			this.barItemRotateLeft.ID = "Rotate Left";
			this.barItemRotateLeft.ImageIndex = 30;
			this.barItemRotateLeft.ImageList = this.smBarItemImages;
			this.barItemRotateLeft.Text = "Rotate Left";
			this.barItemRotateLeft.Tooltip = "Rotate Left";
			this.barItemRotateLeft.Click += new System.EventHandler(this.barItemRotateLeft_Click);
			// 
			// barItemRotateRight
			// 
			this.barItemRotateRight.CategoryIndex = 4;
			this.barItemRotateRight.ID = "Rotate Right";
			this.barItemRotateRight.ImageIndex = 31;
			this.barItemRotateRight.ImageList = this.smBarItemImages;
			this.barItemRotateRight.Text = "Rotate Right";
			this.barItemRotateRight.Tooltip = "Rotate Right";
			this.barItemRotateRight.Click += new System.EventHandler(this.barItemRotateRight_Click);
			// 
			// barItemFlipVertical
			// 
			this.barItemFlipVertical.CategoryIndex = 4;
			this.barItemFlipVertical.ID = "Flip Vertical";
			this.barItemFlipVertical.ImageIndex = 25;
			this.barItemFlipVertical.ImageList = this.smBarItemImages;
			this.barItemFlipVertical.Text = "Flip Vertical";
			this.barItemFlipVertical.Tooltip = "Flip Vertical";
			this.barItemFlipVertical.Click += new System.EventHandler(this.barItemFlipVertical_Click);
			// 
			// barItemFlipHorizontal
			// 
			this.barItemFlipHorizontal.CategoryIndex = 4;
			this.barItemFlipHorizontal.ID = "Flip Horizontal";
			this.barItemFlipHorizontal.ImageIndex = 24;
			this.barItemFlipHorizontal.ImageList = this.smBarItemImages;
			this.barItemFlipHorizontal.Text = "Flip Horizontal";
			this.barItemFlipHorizontal.Tooltip = "Flip Horizontal";
			this.barItemFlipHorizontal.Click += new System.EventHandler(this.barItemFlipHorizontal_Click);
			// 
			// bar1
			// 
			this.bar1.BarName = "View";
			this.bar1.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																							  this.barItemPan,
																							  this.barItemShowGrid,
																							  this.barItemSnapToGrid,
																							  this.barItemZoom,
																							  this.comboBoxBarItemMagnification});
			this.bar1.Manager = this.childFrameBarManager;
			// 
			// barItemPan
			// 
			this.barItemPan.CategoryIndex = 5;
			this.barItemPan.ID = "Pan";
			this.barItemPan.ImageIndex = 10;
			this.barItemPan.ImageList = this.smBarItemImages;
			this.barItemPan.Tag = "PanTool";
			this.barItemPan.Text = "Pan";
			this.barItemPan.Tooltip = "Pan";
			this.barItemPan.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemShowGrid
			// 
			this.barItemShowGrid.CategoryIndex = 5;
			this.barItemShowGrid.ID = "Show Grid";
			this.barItemShowGrid.ImageIndex = 44;
			this.barItemShowGrid.ImageList = this.smBarItemImages;
			this.barItemShowGrid.Text = "Show Grid";
			this.barItemShowGrid.Tooltip = "Show Grid";
			this.barItemShowGrid.Click += new System.EventHandler(this.barItemShowGrid_Click);
			// 
			// barItemSnapToGrid
			// 
			this.barItemSnapToGrid.CategoryIndex = 5;
			this.barItemSnapToGrid.ID = "Snap To Grid";
			this.barItemSnapToGrid.ImageIndex = 45;
			this.barItemSnapToGrid.ImageList = this.smBarItemImages;
			this.barItemSnapToGrid.Text = "Snap To Grid";
			this.barItemSnapToGrid.Click += new System.EventHandler(this.barItemSnapToGrid_Click);
			// 
			// barItemZoom
			// 
			this.barItemZoom.CategoryIndex = 5;
			this.barItemZoom.ID = "Zoom";
			this.barItemZoom.ImageIndex = 8;
			this.barItemZoom.ImageList = this.smBarItemImages;
			this.barItemZoom.Tag = "ZoomTool";
			this.barItemZoom.Text = "Zoom";
			this.barItemZoom.Tooltip = "Zoom";
			this.barItemZoom.Click += new System.EventHandler(this.toolActivate);
			// 
			// comboBoxBarItemMagnification
			// 
			this.comboBoxBarItemMagnification.CategoryIndex = 5;
			this.comboBoxBarItemMagnification.ChoiceList.AddRange(new string[] {
																				   "25%",
																				   "50%",
																				   "75%",
																				   "100%",
																				   "125%",
																				   "150%",
																				   "175%",
																				   "200%"});
			this.comboBoxBarItemMagnification.Editable = false;
			this.comboBoxBarItemMagnification.ID = "Magnification";
			this.comboBoxBarItemMagnification.Text = "Magnification";
			this.comboBoxBarItemMagnification.Tooltip = "Magnification";
			this.comboBoxBarItemMagnification.Click += new System.EventHandler(this.comboBoxBarItemMagnification_Click);
			// 
			// barItemOrthogonalLink
			// 
			this.barItemOrthogonalLink.CategoryIndex = 2;
			this.barItemOrthogonalLink.ID = "Orthogonal Link";
			this.barItemOrthogonalLink.ImageIndex = 46;
			this.barItemOrthogonalLink.ImageList = this.smBarItemImages;
			this.barItemOrthogonalLink.Text = "Orthogonal Link";
			this.barItemOrthogonalLink.Click += new System.EventHandler(this.barItemOrthogonalLink_Click);
			// 
			// barItemEditVertices
			// 
			this.barItemEditVertices.CategoryIndex = 0;
			this.barItemEditVertices.ID = "Edit Vertices";
			this.barItemEditVertices.ImageIndex = 11;
			this.barItemEditVertices.ImageList = this.smBarItemImages;
			this.barItemEditVertices.Tag = "VertexEditTool";
			this.barItemEditVertices.Text = "Edit Vertices";
			this.barItemEditVertices.Click += new System.EventHandler(this.toolActivate);
			// 
			// barItemDirectedLink
			// 
			this.barItemDirectedLink.CategoryIndex = 2;
			this.barItemDirectedLink.ID = "Directed Link";
			this.barItemDirectedLink.ImageIndex = 47;
			this.barItemDirectedLink.ImageList = this.smBarItemImages;
			this.barItemDirectedLink.Text = "Directed Link";
			this.barItemDirectedLink.Tooltip = "Directed Link";
			this.barItemDirectedLink.Click += new System.EventHandler(this.barItemDirectedLink_Click);
			// 
			// barItemArcLink
			// 
			this.barItemArcLink.CategoryIndex = 2;
			this.barItemArcLink.ID = "Arc Link";
			this.barItemArcLink.ImageIndex = 48;
			this.barItemArcLink.ImageList = this.smBarItemImages;
			this.barItemArcLink.Text = "Arc Link";
			this.barItemArcLink.Tooltip = "Arc Link";
			this.barItemArcLink.Click += new System.EventHandler(this.barItemArcLink_Click);
			// 
			// DiagramForm
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(560, 414);
			this.Controls.Add(this.diagramComponent);
			this.Name = "DiagramForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Diagram";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.DiagramForm_Closing);
			this.Load += new System.EventHandler(this.DiagramForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.childFrameBarManager)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		protected Link CreateLink(PointF[] pts)
		{
			Link link = new Link(pts);
			//link.EndPoints.LastEndPoint = new FilledArrowDecorator();
			return link;
		}

		protected Link CreateDirectedLink(PointF[] pts)
		{
			Link link = new Link(pts);
			link.EndPoints.LastEndPoint = new FilledArrowDecorator();
			return link;
		}

		protected Link CreateOrthogonalLink(PointF[] pts)
		{
			Link link = new Link(Link.Shapes.OrthogonalLine, pts);
			//link.EndPoints.LastEndPoint = new FilledArrowDecorator();
			return link;
		}

		protected Link CreateDirectedOrthogonalLink(PointF[] pts)
		{
			Link link = new Link(Link.Shapes.OrthogonalLine, pts);
			link.EndPoints.LastEndPoint = new FilledArrowDecorator();
			return link;
		}

		protected Link CreateArcLink(PointF[] pts)
		{
			Link link = new Link(Link.Shapes.Arc, pts);
			return link;
		}

		private void UpdateMagnificationComboBox()
		{
			System.Drawing.Size szMag = this.Diagram.View.Magnification;
			this.comboBoxBarItemMagnification.TextBoxValue = szMag.Width + "%";
		}

		private void toolActivate(object sender, System.EventArgs e)
		{
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem = (Syncfusion.Windows.Forms.Tools.XPMenus.BarItem) sender;
			string toolName = (string) barItem.Tag;
			this.diagramComponent.ActivateTool(toolName);
		}

		private void barItemLinkSymbols_Click(object sender, System.EventArgs e)
		{
			Tool linkTool = this.diagramComponent.Controller.GetTool("LinkTool");
			if (linkTool != null)
			{
				this.diagramComponent.Controller.ActivateTool(linkTool);
			}
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

		private void DiagramForm_Load(object sender, System.EventArgs e)
		{
			this.barItemShowGrid.Checked = this.Diagram.View.Grid.Visible;
			this.barItemSnapToGrid.Checked = this.Diagram.View.Grid.SnapToGrid;
			UpdateMagnificationComboBox();

			// Attach link factory to the default link tool
			Tool linkTool = this.diagramComponent.Controller.GetTool("LinkTool");
			if (linkTool != null && linkTool.GetType() == typeof(LinkTool))
			{
				((LinkTool)linkTool).LinkFactory = new LinkFactory(this.CreateLink);
			}

			// Create and register a tool for orthogonal links
			LinkTool orthogonalLinkTool = new LinkTool("OrthogonalLinkTool");
			orthogonalLinkTool.LinkFactory = new LinkFactory(this.CreateOrthogonalLink);
			this.diagramComponent.Controller.RegisterTool(orthogonalLinkTool);

			// Create and register a tool for directed links
			LinkTool directedLinkTool = new LinkTool("DirectedLinkTool");
			directedLinkTool.LinkFactory = new LinkFactory(this.CreateDirectedLink);
			this.diagramComponent.Controller.RegisterTool(directedLinkTool);

			// Create and register a tool for arc links
			LinkTool arcLinkTool = new LinkTool("ArcLinkTool");
			arcLinkTool.LinkFactory = new LinkFactory(this.CreateArcLink);
			this.diagramComponent.Controller.RegisterTool(arcLinkTool);

			PropertyEditor propEditor = this.PropertyEditor;
			if (propEditor != null)
			{
				propEditor.Diagram = this.diagramComponent;
			}

			this.diagramComponent.Focus();
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

		private void diagramComponent_View_MagnificationChanged(object sender, ViewMagnificationEventArgs evtArgs)
		{
			UpdateMagnificationComboBox();
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

		private void barItemImage_Click(object sender, System.EventArgs e)
		{
			this.Diagram.InsertImage();
		}

		private void DiagramForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			PropertyEditor propEditor = this.PropertyEditor;
			if (propEditor != null)
			{
				propEditor.Diagram = null;
			}
		}

		private void barItemOrthogonalLink_Click(object sender, System.EventArgs e)
		{
			this.diagramComponent.ActivateTool("OrthogonalLinkTool");
		}

		private void barItemDirectedLink_Click(object sender, System.EventArgs e)
		{
			this.diagramComponent.ActivateTool("DirectedLinkTool");
		}

		private void barItemArcLink_Click(object sender, System.EventArgs e)
		{
			this.diagramComponent.ActivateTool("ArcLinkTool");
		}
	}
}
