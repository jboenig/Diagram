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
using System.Data;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Diagram;
using Syncfusion.Windows.Forms.Diagram.Controls;

namespace Syncfusion.Windows.Forms.Diagram.Samples.DiagramTool
{
	/// <summary>
	/// MainForm for the Essential Diagram Builder.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		#region Fields

		private Syncfusion.Windows.Forms.Tools.DockingManager dockingManager;
		private Syncfusion.Windows.Forms.Tools.TabbedMDIManager tabbedMDIManager;
		private System.Windows.Forms.OpenFileDialog openPaletteDialog;
		private System.Windows.Forms.OpenFileDialog openDiagramDialog;
		private System.Windows.Forms.SaveFileDialog saveDiagramDialog;
		private System.Windows.Forms.ImageList smallImageList1;
		private System.Windows.Forms.ImageList smBarItemImages;
		private System.Windows.Forms.OpenFileDialog openImageDialog;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem1;
		private Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar symbolPaletteGroupBar;
		private Syncfusion.Windows.Forms.Tools.XPMenus.MainFrameBarManager mainFrameBarManager;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar mainMenuBar;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem7;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem biFileNew;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem biFileOpen;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem biFileSave;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem biFileSaveAs;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem biAddPalette;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem biFileExit;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem biTabbedMDI;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditCut;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditCopy;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditPaste;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditUndo;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditRedo;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemAbout;
		private Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor propertyEditor;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar standardToolbar;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem biFilePrint;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem biPageSetup;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemViewSymbolPalette;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemViewProperties;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemEditSelectAll;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItemEdit;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItemFile;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItemView;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItemWindow;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItemHelp;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructors

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// Initialize tabbed MDI manager
			//
			tabbedMDIManager = new Syncfusion.Windows.Forms.Tools.TabbedMDIManager();
			tabbedMDIManager.AttachToMdiContainer(this);
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

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.dockingManager = new Syncfusion.Windows.Forms.Tools.DockingManager(this.components);
			this.propertyEditor = new Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor();
			this.symbolPaletteGroupBar = new Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar();
			this.smBarItemImages = new System.Windows.Forms.ImageList(this.components);
			this.openPaletteDialog = new System.Windows.Forms.OpenFileDialog();
			this.openDiagramDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveDiagramDialog = new System.Windows.Forms.SaveFileDialog();
			this.smallImageList1 = new System.Windows.Forms.ImageList(this.components);
			this.openImageDialog = new System.Windows.Forms.OpenFileDialog();
			this.barItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.mainFrameBarManager = new Syncfusion.Windows.Forms.Tools.XPMenus.MainFrameBarManager(this.components, this);
			this.mainMenuBar = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "MainMenu");
			this.parentBarItemFile = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.biFileNew = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.biFileOpen = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.biFileSave = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.biFileSaveAs = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.biAddPalette = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.biPageSetup = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.biFilePrint = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.biFileExit = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItemEdit = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.barItemEditUndo = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEditRedo = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEditSelectAll = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEditCut = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEditCopy = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemEditPaste = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItemView = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.barItemViewSymbolPalette = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItemViewProperties = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItemWindow = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.biTabbedMDI = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItemHelp = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.barItemAbout = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.standardToolbar = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "Standard");
			this.parentBarItem7 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			((System.ComponentModel.ISupportInitialize)(this.dockingManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainFrameBarManager)).BeginInit();
			// 
			// dockingManager
			// 
			this.dockingManager.DockLayoutStream = ((System.IO.MemoryStream)(resources.GetObject("dockingManager.DockLayoutStream")));
			this.dockingManager.HostForm = this;
			this.dockingManager.DockVisibilityChanged += new Syncfusion.Windows.Forms.Tools.DockVisibilityChangedEventHandler(this.dockingManager_DockVisibilityChanged);
			this.dockingManager.SetDockLabel(this.propertyEditor, "Properties");
			this.dockingManager.SetDockLabel(this.symbolPaletteGroupBar, "Symbol Palettes");
			// 
			// propertyEditor
			// 
			this.propertyEditor.Diagram = null;
			this.dockingManager.SetEnableDocking(this.propertyEditor, true);
			this.propertyEditor.Location = new System.Drawing.Point(1, 20);
			this.propertyEditor.Name = "propertyEditor";
			this.propertyEditor.Size = new System.Drawing.Size(201, 493);
			this.propertyEditor.TabIndex = 11;
			// 
			// symbolPaletteGroupBar
			// 
			this.symbolPaletteGroupBar.AllowDrop = true;
			this.symbolPaletteGroupBar.EditMode = false;
			this.dockingManager.SetEnableDocking(this.symbolPaletteGroupBar, true);
			this.symbolPaletteGroupBar.Location = new System.Drawing.Point(1, 20);
			this.symbolPaletteGroupBar.Name = "symbolPaletteGroupBar";
			this.symbolPaletteGroupBar.Size = new System.Drawing.Size(138, 493);
			this.symbolPaletteGroupBar.TabIndex = 9;
			this.symbolPaletteGroupBar.Text = "paletteGroupBar1";
			// 
			// smBarItemImages
			// 
			this.smBarItemImages.ImageSize = new System.Drawing.Size(16, 16);
			this.smBarItemImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smBarItemImages.ImageStream")));
			this.smBarItemImages.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// openPaletteDialog
			// 
			this.openPaletteDialog.DefaultExt = "edp";
			this.openPaletteDialog.Filter = "Essential Diagram Palettes|*.edp|All files|*.*";
			this.openPaletteDialog.Title = "Add Palette";
			// 
			// openDiagramDialog
			// 
			this.openDiagramDialog.Filter = "Diagram Files|*.edd|All files|*.*";
			this.openDiagramDialog.Title = "Open Diagram";
			// 
			// saveDiagramDialog
			// 
			this.saveDiagramDialog.FileName = "doc1";
			this.saveDiagramDialog.Filter = "Diagram files|*.edd|All files|*.*";
			// 
			// smallImageList1
			// 
			this.smallImageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.smallImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImageList1.ImageStream")));
			this.smallImageList1.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// openImageDialog
			// 
			this.openImageDialog.Filter = "Windows Bitmaps|*.bmp|Enhanced Metafiles|*.emf|All files|*.*";
			this.openImageDialog.Title = "Insert Image";
			// 
			// barItem1
			// 
			this.barItem1.CategoryIndex = 3;
			this.barItem1.ID = "Pan";
			this.barItem1.ImageIndex = 10;
			this.barItem1.Tag = "PanTool";
			this.barItem1.Text = "Pan";
			this.barItem1.Tooltip = "Pan";
			// 
			// mainFrameBarManager
			// 
			this.mainFrameBarManager.BarPositionInfo = ((System.IO.MemoryStream)(resources.GetObject("mainFrameBarManager.BarPositionInfo")));
			this.mainFrameBarManager.Bars.Add(this.mainMenuBar);
			this.mainFrameBarManager.Bars.Add(this.standardToolbar);
			this.mainFrameBarManager.Categories.Add("Popups");
			this.mainFrameBarManager.Categories.Add("File");
			this.mainFrameBarManager.Categories.Add("Window");
			this.mainFrameBarManager.Categories.Add("Edit");
			this.mainFrameBarManager.Categories.Add("Help");
			this.mainFrameBarManager.Categories.Add("View");
			this.mainFrameBarManager.CurrentBaseFormType = "System.Windows.Forms.Form";
			this.mainFrameBarManager.Form = this;
			this.mainFrameBarManager.ImageList = null;
			this.mainFrameBarManager.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																											 this.biFileNew,
																											 this.biFileOpen,
																											 this.biFileSave,
																											 this.parentBarItemFile,
																											 this.biFileSaveAs,
																											 this.biPageSetup,
																											 this.biFilePrint,
																											 this.parentBarItemEdit,
																											 this.biAddPalette,
																											 this.biFileExit,
																											 this.parentBarItemView,
																											 this.parentBarItemWindow,
																											 this.parentBarItemHelp,
																											 this.biTabbedMDI,
																											 this.barItemEditCut,
																											 this.barItemEditCopy,
																											 this.barItemEditPaste,
																											 this.barItemEditUndo,
																											 this.barItemEditRedo,
																											 this.barItemAbout,
																											 this.barItemViewSymbolPalette,
																											 this.barItemViewProperties,
																											 this.barItemEditSelectAll});
			this.mainFrameBarManager.LargeImageList = null;
			this.mainFrameBarManager.ResetCustomization = false;
			// 
			// mainMenuBar
			// 
			this.mainMenuBar.BarName = "MainMenu";
			this.mainMenuBar.BarStyle = ((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle)((((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.AllowQuickCustomizing | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.IsMainMenu) 
				| Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.Visible) 
				| Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.DrawDragBorder)));
			this.mainMenuBar.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																									 this.parentBarItemFile,
																									 this.parentBarItemEdit,
																									 this.parentBarItemView,
																									 this.parentBarItemWindow,
																									 this.parentBarItemHelp});
			this.mainMenuBar.Manager = this.mainFrameBarManager;
			// 
			// parentBarItemFile
			// 
			this.parentBarItemFile.CategoryIndex = 0;
			this.parentBarItemFile.ID = "File";
			this.parentBarItemFile.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										   this.biFileNew,
																										   this.biFileOpen,
																										   this.biFileSave,
																										   this.biFileSaveAs,
																										   this.biAddPalette,
																										   this.biPageSetup,
																										   this.biFilePrint,
																										   this.biFileExit});
			this.parentBarItemFile.SeparatorIndices.AddRange(new int[] {
																		   4,
																		   5,
																		   7});
			this.parentBarItemFile.Text = "&File";
			// 
			// biFileNew
			// 
			this.biFileNew.CategoryIndex = 1;
			this.biFileNew.ID = "New";
			this.biFileNew.ImageIndex = 6;
			this.biFileNew.ImageList = this.smallImageList1;
			this.biFileNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.biFileNew.Text = "&New";
			this.biFileNew.Click += new System.EventHandler(this.biFileNew_Click);
			// 
			// biFileOpen
			// 
			this.biFileOpen.CategoryIndex = 1;
			this.biFileOpen.ID = "Open";
			this.biFileOpen.ImageIndex = 4;
			this.biFileOpen.ImageList = this.smallImageList1;
			this.biFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.biFileOpen.Text = "&Open";
			this.biFileOpen.Click += new System.EventHandler(this.biFileOpen_Click);
			// 
			// biFileSave
			// 
			this.biFileSave.CategoryIndex = 1;
			this.biFileSave.ID = "Save";
			this.biFileSave.ImageIndex = 0;
			this.biFileSave.ImageList = this.smallImageList1;
			this.biFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.biFileSave.Text = "&Save";
			this.biFileSave.Click += new System.EventHandler(this.biFileSave_Click);
			// 
			// biFileSaveAs
			// 
			this.biFileSaveAs.CategoryIndex = 1;
			this.biFileSaveAs.ID = "Save As...";
			this.biFileSaveAs.Text = "Save As...";
			this.biFileSaveAs.Click += new System.EventHandler(this.biFileSaveAs_Click);
			// 
			// biAddPalette
			// 
			this.biAddPalette.CategoryIndex = 1;
			this.biAddPalette.ID = "Add Palette";
			this.biAddPalette.Text = "Add Palette";
			this.biAddPalette.Click += new System.EventHandler(this.biFileAddPalette_Click);
			// 
			// biPageSetup
			// 
			this.biPageSetup.CategoryIndex = 1;
			this.biPageSetup.ID = "Page Setup";
			this.biPageSetup.ImageIndex = 9;
			this.biPageSetup.ImageList = this.smallImageList1;
			this.biPageSetup.Text = "Page Setup";
			// 
			// biFilePrint
			// 
			this.biFilePrint.CategoryIndex = 1;
			this.biFilePrint.ID = "Print";
			this.biFilePrint.ImageIndex = 8;
			this.biFilePrint.ImageList = this.smallImageList1;
			this.biFilePrint.Text = "Print";
			// 
			// biFileExit
			// 
			this.biFileExit.CategoryIndex = 1;
			this.biFileExit.ID = "Exit";
			this.biFileExit.Text = "Exit";
			this.biFileExit.Click += new System.EventHandler(this.biFileExit_Click);
			// 
			// parentBarItemEdit
			// 
			this.parentBarItemEdit.CategoryIndex = 0;
			this.parentBarItemEdit.ID = "Edit";
			this.parentBarItemEdit.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										   this.barItemEditUndo,
																										   this.barItemEditRedo,
																										   this.barItemEditSelectAll,
																										   this.barItemEditCut,
																										   this.barItemEditCopy,
																										   this.barItemEditPaste});
			this.parentBarItemEdit.SeparatorIndices.AddRange(new int[] {
																		   3,
																		   2});
			this.parentBarItemEdit.Text = "&Edit";
			// 
			// barItemEditUndo
			// 
			this.barItemEditUndo.CategoryIndex = 3;
			this.barItemEditUndo.ID = "Undo";
			this.barItemEditUndo.ImageIndex = 34;
			this.barItemEditUndo.ImageList = this.smBarItemImages;
			this.barItemEditUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.barItemEditUndo.Text = "&Undo";
			this.barItemEditUndo.Click += new System.EventHandler(this.barItemEditUndo_Click);
			// 
			// barItemEditRedo
			// 
			this.barItemEditRedo.CategoryIndex = 3;
			this.barItemEditRedo.ID = "EditRedo";
			this.barItemEditRedo.ImageIndex = 35;
			this.barItemEditRedo.ImageList = this.smBarItemImages;
			this.barItemEditRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
			this.barItemEditRedo.Text = "&Redo";
			this.barItemEditRedo.Click += new System.EventHandler(this.barItemEditRedo_Click);
			// 
			// barItemEditSelectAll
			// 
			this.barItemEditSelectAll.CategoryIndex = 3;
			this.barItemEditSelectAll.ID = "&Select All";
			this.barItemEditSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.barItemEditSelectAll.Text = "Select &All";
			this.barItemEditSelectAll.Click += new System.EventHandler(this.barItemEditSelectAll_Click);
			// 
			// barItemEditCut
			// 
			this.barItemEditCut.CategoryIndex = 3;
			this.barItemEditCut.ID = "Cut";
			this.barItemEditCut.ImageIndex = 36;
			this.barItemEditCut.ImageList = this.smBarItemImages;
			this.barItemEditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.barItemEditCut.Text = "C&ut";
			this.barItemEditCut.Click += new System.EventHandler(this.barItemEditCut_Click);
			// 
			// barItemEditCopy
			// 
			this.barItemEditCopy.CategoryIndex = 3;
			this.barItemEditCopy.ID = "Copy";
			this.barItemEditCopy.ImageIndex = 37;
			this.barItemEditCopy.ImageList = this.smBarItemImages;
			this.barItemEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.barItemEditCopy.Text = "&Copy";
			this.barItemEditCopy.Click += new System.EventHandler(this.barItemEditCopy_Click);
			// 
			// barItemEditPaste
			// 
			this.barItemEditPaste.CategoryIndex = 3;
			this.barItemEditPaste.ID = "Paste";
			this.barItemEditPaste.ImageIndex = 38;
			this.barItemEditPaste.ImageList = this.smBarItemImages;
			this.barItemEditPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.barItemEditPaste.Text = "&Paste";
			this.barItemEditPaste.Click += new System.EventHandler(this.barItemEditPaste_Click);
			// 
			// parentBarItemView
			// 
			this.parentBarItemView.CategoryIndex = 0;
			this.parentBarItemView.ID = "View";
			this.parentBarItemView.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										   this.barItemViewSymbolPalette,
																										   this.barItemViewProperties});
			this.parentBarItemView.Text = "&View";
			// 
			// barItemViewSymbolPalette
			// 
			this.barItemViewSymbolPalette.CategoryIndex = 5;
			this.barItemViewSymbolPalette.ID = "Symbol Palette";
			this.barItemViewSymbolPalette.Text = "Symbol Palette";
			this.barItemViewSymbolPalette.Click += new System.EventHandler(this.barItemViewSymbolPalette_Click);
			// 
			// barItemViewProperties
			// 
			this.barItemViewProperties.CategoryIndex = 5;
			this.barItemViewProperties.ID = "Properties";
			this.barItemViewProperties.Text = "Properties";
			this.barItemViewProperties.Click += new System.EventHandler(this.barItemViewProperties_Click);
			// 
			// parentBarItemWindow
			// 
			this.parentBarItemWindow.CategoryIndex = 0;
			this.parentBarItemWindow.ID = "Window";
			this.parentBarItemWindow.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																											 this.biTabbedMDI});
			this.parentBarItemWindow.Text = "&Window";
			// 
			// biTabbedMDI
			// 
			this.biTabbedMDI.CategoryIndex = 2;
			this.biTabbedMDI.Checked = true;
			this.biTabbedMDI.ID = "Tabbed MDI";
			this.biTabbedMDI.Text = "Tabbed MDI";
			this.biTabbedMDI.Click += new System.EventHandler(this.biTabbedMDI_Click);
			// 
			// parentBarItemHelp
			// 
			this.parentBarItemHelp.CategoryIndex = 0;
			this.parentBarItemHelp.ID = "Help";
			this.parentBarItemHelp.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										   this.barItemAbout});
			this.parentBarItemHelp.Text = "&Help";
			// 
			// barItemAbout
			// 
			this.barItemAbout.CategoryIndex = 4;
			this.barItemAbout.ID = "About";
			this.barItemAbout.Text = "About...";
			this.barItemAbout.Click += new System.EventHandler(this.barItemAbout_Click);
			// 
			// standardToolbar
			// 
			this.standardToolbar.BarName = "Standard";
			this.standardToolbar.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										 this.biFileNew,
																										 this.biFileOpen,
																										 this.biFileSave,
																										 this.biFilePrint});
			this.standardToolbar.Manager = this.mainFrameBarManager;
			// 
			// parentBarItem7
			// 
			this.parentBarItem7.CategoryIndex = 3;
			this.parentBarItem7.ID = "Zoom";
			this.parentBarItem7.ImageIndex = 8;
			this.parentBarItem7.ImageList = this.smBarItemImages;
			this.parentBarItem7.Text = "Zoom";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.IsMdiContainer = true;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Diagram Builder";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dockingManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainFrameBarManager)).EndInit();

		}
		#endregion

		#region MainForm Event Handlers

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			this.barItemViewProperties.Checked = true;
			this.barItemViewSymbolPalette.Checked = true;
		}

		#endregion

		#region Properties

		private Syncfusion.Windows.Forms.Diagram.Controls.Diagram ActiveDiagram
		{
			get
			{
				Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram = null;

				if (this.ActiveMdiChild != null)
				{
					DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
					if (diagramForm != null)
					{
						diagram = diagramForm.Diagram;
					}
				}

				return diagram;
			}
		}

		private DiagramForm ActiveDiagramForm
		{
			get
			{
				DiagramForm diagramForm = null;

				if (this.ActiveMdiChild != null)
				{
					diagramForm = this.ActiveMdiChild as DiagramForm;
				}

				return diagramForm;
			}
		}

		public PropertyEditor PropertyEditor
		{
			get
			{
				return this.propertyEditor;
			}
		}

		#endregion

		#region File Menu Event Handlers

		private void biFileAddPalette_Click(object sender, System.EventArgs e)
		{
			// Open symbol palette and add it to the symbol palette group bar
			if (openPaletteDialog.ShowDialog(this) == DialogResult.OK)
			{
				SymbolPalette curPalette = null;
				FileStream iStream = null;

				try
				{
					iStream = new FileStream(openPaletteDialog.FileName, FileMode.Open, FileAccess.Read);
				}
				catch (Exception ex)
				{
					iStream = null;
					MessageBox.Show(this, ex.Message);
				}

				if (iStream != null)
				{
					SoapFormatter formatter = new SoapFormatter();
					try
					{
						System.AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
						curPalette = (SymbolPalette) formatter.Deserialize(iStream);
						symbolPaletteGroupBar.AddPalette(curPalette);
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, ex.Message);
					}
					finally
					{
						iStream.Close();
					}
				}
			}
		}

		private void biFileNew_Click(object sender, System.EventArgs e)
		{
			// New diagram
			DiagramForm docForm = new DiagramForm();
			docForm.MdiParent = this;
			docForm.Show();
		}

		private void biFileOpen_Click(object sender, System.EventArgs e)
		{
			// Open diagram
			if (this.openDiagramDialog.ShowDialog(this) == DialogResult.OK)
			{
				DiagramForm docForm = new DiagramForm();
				docForm.OpenFile(this.openDiagramDialog.FileName);
				docForm.MdiParent = this;
				docForm.Show();
			}
		}

		private void biFileSave_Click(object sender, System.EventArgs e)
		{
			// Save diagram
			DiagramForm docForm = this.ActiveDiagramForm;
			if (docForm != null)
			{
				if (!docForm.HasFileName)
				{
					if (this.saveDiagramDialog.ShowDialog(this) == DialogResult.OK)
					{
						docForm.SaveAsFile(this.saveDiagramDialog.FileName);
					}
				}
				else
				{
					docForm.SaveFile();
				}
			}
		}

		private void biFileSaveAs_Click(object sender, System.EventArgs e)
		{
			// Save diagram
			DiagramForm docForm = this.ActiveDiagramForm;
			if (docForm != null)
			{
				if (docForm.HasFileName)
				{
					this.saveDiagramDialog.FileName = docForm.FileName;
				}
				if (this.saveDiagramDialog.ShowDialog(this) == DialogResult.OK)
				{
					docForm.SaveAsFile(this.saveDiagramDialog.FileName);
				}
			}
		}

		private void biFileExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		#endregion

		#region Edit Menu Event Handlers

		private void barItemEditCut_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
			{
				DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
				if (diagramForm != null)
				{
					diagramForm.Diagram.Controller.Cut();
				}
			}
		}

		private void barItemEditCopy_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
			{
				DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
				if (diagramForm != null)
				{
					diagramForm.Diagram.Controller.Copy();
				}
			}
		}

		private void barItemEditPaste_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
			{
				DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
				if (diagramForm != null)
				{
					diagramForm.Diagram.Controller.Paste();
				}
			}
		}

		private void barItemEditUndo_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
			{
				DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
				if (diagramForm != null)
				{
					diagramForm.Diagram.Controller.UndoCommand();
				}
			}
		}

		private void barItemEditRedo_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
			{
				DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
				if (diagramForm != null)
				{
					diagramForm.Diagram.Controller.RedoCommand();
				}
			}
		}

		private void barItemEditSelectAll_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
			{
				DiagramForm diagramForm = this.ActiveMdiChild as DiagramForm;
				if (diagramForm != null)
				{
					diagramForm.Diagram.Controller.SelectAll();
				}
			}
		}

		#endregion

		#region View Menu Event Handlers

		private void barItemViewSymbolPalette_Click(object sender, System.EventArgs e)
		{
			if (this.barItemViewSymbolPalette.Checked)
			{
				this.dockingManager.SetDockVisibility(this.symbolPaletteGroupBar, false);
				this.barItemViewSymbolPalette.Checked = false;
			}
			else
			{
				this.dockingManager.SetDockVisibility(this.symbolPaletteGroupBar, true);
				this.barItemViewSymbolPalette.Checked = true;
			}
		}

		private void barItemViewProperties_Click(object sender, System.EventArgs e)
		{
			if (this.barItemViewProperties.Checked)
			{
				this.dockingManager.SetDockVisibility(this.propertyEditor, false);
				this.barItemViewProperties.Checked = false;
			}
			else
			{
				this.dockingManager.SetDockVisibility(this.propertyEditor, true);
				this.barItemViewProperties.Checked = true;
			}
		}

		#endregion

		#region Window Menu Event Handlers

		private void biTabbedMDI_Click(object sender, System.EventArgs e)
		{
			// Toggle tabbed MDI mode
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem = sender as Syncfusion.Windows.Forms.Tools.XPMenus.BarItem;
			if (barItem != null)
			{
				if (barItem.Checked)
				{
					tabbedMDIManager.DetachFromMdiContainer(this, true);
					barItem.Checked = false;
				}
				else
				{
					tabbedMDIManager.AttachToMdiContainer(this);
					barItem.Checked = true;
				}
			}
		}

		#endregion

		#region Help Menu Event Handlers

		private void barItemAbout_Click(object sender, System.EventArgs e)
		{
			About aboutDlg = new About();
			aboutDlg.ShowDialog(this);
		}

		#endregion

		#region Docking

		private void dockingManager_DockVisibilityChanged(object sender, Syncfusion.Windows.Forms.Tools.DockVisibilityChangedEventArgs e)
		{
			if (e.Control == this.propertyEditor)
			{
				this.barItemViewProperties.Checked = this.dockingManager.GetDockVisibility(this.propertyEditor);
			}
			else if (e.Control == this.symbolPaletteGroupBar)
			{
				this.barItemViewSymbolPalette.Checked = this.dockingManager.GetDockVisibility(this.symbolPaletteGroupBar);
			}
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
	}
}
