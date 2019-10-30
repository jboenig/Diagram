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
using System.Resources;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Diagram;
using Syncfusion.Windows.Forms.Diagram.Controls;

namespace Syncfusion.SymbolDesigner
{
	/// <summary>
	/// MainForm of Essential Diagram Symbol Designer.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form, IStatusUpdate
	{
		#region Member Variables

		private System.ComponentModel.IContainer components;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar mainMenu;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem FileNew;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.ImageList smBarItemImages;
		private Syncfusion.Windows.Forms.Tools.XPMenus.MainFrameBarManager mainFrameBarManager;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem1;
		private System.Windows.Forms.Splitter splitter1;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem FileOpen;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem FileSave;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem FileSaveAs;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem FileExit;
		private System.Windows.Forms.SaveFileDialog savePaletteFileDialog;
		private System.Windows.Forms.OpenFileDialog openPaletteFileDialog;
		private Syncfusion.Windows.Forms.Tools.TabbedMDIManager tabbedMDIManager;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem EditUndo;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem EditRedo;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem EditCut;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem EditCopy;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem EditPaste;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem HelpAbout;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem1;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem3;
		private System.Windows.Forms.ImageList smallImageList1;
		private System.Windows.Forms.Panel symbolPalettePanel;
		private Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar symbolPaletteGroupBar;
		private Syncfusion.Windows.Forms.Tools.XPMenus.XPToolBar xptbSymbolPalette;
		private Syncfusion.Windows.Forms.Tools.DockingManager dockingManager;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem4;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem tabbedMDI;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem5;
		private Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor propertyEditor;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem FileClose;
		private System.Windows.Forms.OpenFileDialog openImageDialog;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItem7;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem FilePageSetup;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem FilePrint;
		private Syncfusion.Windows.Forms.Tools.XPMenus.Bar barStandard;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItemHelpContents;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem FilePrintPreview;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem EditSelectAll;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.StatusBarPanel statusBarXY;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem ViewStatusBar;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem ViewProperties;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem EditDelete;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem SymbolAdd;
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItemSymbol;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem SymbolRemove;
		private Syncfusion.SymbolDesigner.OptionsControl optionsControl;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem ViewOptions;
		private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem ViewSymbolPalette;
		private ResourceManager resStringMgr;

		private string mainFormTitle = "";
		private Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem parentBarItemEdit;
		private string paletteFileName = "";

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

			// Wire up OnIdle processing
			Application.Idle += new System.EventHandler(this.OnIdle);

			this.resStringMgr = new ResourceManager("Syncfusion.SymbolDesigner.Strings", typeof(MainForm).Assembly);
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

		#region Public Properties

		public PropertyEditor PropertyEditor
		{
			get
			{
				return this.propertyEditor;
			}
		}

		#endregion

		#region Symbol Palette I/O

		private bool PromptSavePalette()
		{
			bool shouldContinue = true;
			SymbolPalette curPalette = symbolPaletteGroupBar.CurrentPalette;

			if (curPalette != null)
			{
				if (curPalette.Modified)
				{
					string promptSaveMsg = this.resStringMgr.GetString("MsgPromptSave");
					string promptSaveCaption = this.resStringMgr.GetString("CaptionPromptSave");
					DialogResult dlgRes = MessageBox.Show(this, promptSaveMsg, promptSaveCaption, MessageBoxButtons.YesNoCancel);

					if (dlgRes == DialogResult.Yes)
					{
						if (savePaletteFileDialog.ShowDialog(this) == DialogResult.OK)
						{
							FileStream oStream = new FileStream(savePaletteFileDialog.FileName, FileMode.Create);
							SoapFormatter formatter = new SoapFormatter();
							formatter.Serialize(oStream, curPalette);
						}
					}
					else if (dlgRes == DialogResult.Cancel)
					{
						shouldContinue = false;
					}
				}
			}

			return shouldContinue;
		}

		private void ClosePalette()
		{
			symbolPaletteGroupBar.Clear();

			// Close all MDI child windows associated with the previously open palette
			int mdiChildIdx;
			for (mdiChildIdx = 0; mdiChildIdx < this.MdiChildren.GetLength(0); mdiChildIdx++)
			{
				this.MdiChildren[mdiChildIdx].Close();
			}

			this.paletteFileName = "";

			symbolPaletteGroupBar.Refresh();
		}

		#endregion

		#region MDI Management

		private Syncfusion.Windows.Forms.Diagram.Controls.Diagram ActiveDiagram
		{
			get
			{
				Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram = null;

				Form activeChild = this.ActiveMdiChild;
				if (activeChild != null)
				{
					SymbolDocument symbolDoc = activeChild as SymbolDocument;
					if (symbolDoc != null)
					{
						diagram = symbolDoc.Diagram;
					}
				}

				return diagram;
			}
		}

		#endregion

		#region MainForm Event Handlers

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			Syncfusion.Windows.Forms.Diagram.Global.Initialize();

			if (this.statusBar.Visible)
			{
				this.ViewStatusBar.Checked = true;
			}
			else
			{
				this.ViewStatusBar.Checked = false;
			}

			this.ViewProperties.Checked = true;
			this.ViewSymbolPalette.Checked = true;
			this.ViewOptions.Checked = false;
			this.mainFormTitle = this.Text;
		}

		private void MainForm_MdiChildActivate(object sender, System.EventArgs e)
		{
			Syncfusion.Windows.Forms.Diagram.Controls.Diagram activeDiagram = this.ActiveDiagram;

			this.optionsControl.Diagram = activeDiagram;

			if (activeDiagram != null)
			{
				this.symbolPaletteGroupBar.SelectSymbolModel(activeDiagram.Model.Name);
			}
		}

		#endregion

		#region File Menu Event Handlers

		private void FileNew_Click(object sender, System.EventArgs e)
		{
			this.PromptSavePalette();
			this.ClosePalette();
			symbolPaletteGroupBar.AddPalette();
			this.UpdateMainFormTitle();
		}

		private void FileOpen_Click(object sender, System.EventArgs e)
		{
			if (this.PromptSavePalette())
			{
				this.ClosePalette();

				if (openPaletteFileDialog.ShowDialog(this) == DialogResult.OK)
				{
					this.paletteFileName = openPaletteFileDialog.FileName;
					FileStream iStream = new FileStream(this.paletteFileName, FileMode.Open);
					SoapFormatter formatter = new SoapFormatter();
					SymbolPalette curPalette = null;
					try
					{
						System.AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
						curPalette = (SymbolPalette) formatter.Deserialize(iStream);
						symbolPaletteGroupBar.AddPalette(curPalette);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
					finally
					{
						System.AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
					}
					iStream.Close();
				}

				this.UpdateMainFormTitle();
			}
		}

		private void FileSave_Click(object sender, System.EventArgs e)
		{
			FileStream oStream;
			SoapFormatter formatter;
			SymbolPalette curPalette;

			if (this.paletteFileName.Length == 0)
			{
				FileSaveAs_Click(sender, e);
			}
			else
			{
				try
				{
					oStream = new FileStream(this.paletteFileName, FileMode.Create);
				}
				catch
				{
					oStream = null;
					MessageBox.Show(this, "Unable to open file " + this.paletteFileName);
				}

				if (oStream != null)
				{
					formatter = new SoapFormatter();
					formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
					curPalette = symbolPaletteGroupBar.CurrentPalette;
					formatter.Serialize(oStream, curPalette);
					oStream.Close();
				}
			}
		}

		private void FileSaveAs_Click(object sender, System.EventArgs e)
		{
			FileStream oStream;
			SoapFormatter formatter;
			SymbolPalette curPalette;

			if (savePaletteFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				this.paletteFileName = savePaletteFileDialog.FileName;
				try
				{
					oStream = new FileStream(this.paletteFileName, FileMode.Create);
				}
				catch
				{
					oStream = null;
					MessageBox.Show(this, "Unable to open file " + this.paletteFileName);
				}

				if (oStream != null)
				{
					formatter = new SoapFormatter();
					curPalette = symbolPaletteGroupBar.CurrentPalette;
					formatter.Serialize(oStream, curPalette);
					oStream.Close();
				}
			}
		}

		private void FileClose_Click(object sender, System.EventArgs e)
		{
			if (PromptSavePalette())
			{
				ClosePalette();
			}
		}

		private void FileExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void FilePrint_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveDiagram != null)
			{
				System.Drawing.Printing.PrintDocument printDoc = this.ActiveDiagram.CreatePrintDocument();
				PrintDialog printDlg = new PrintDialog();
				printDlg.Document = printDoc;
				if (printDlg.ShowDialog(this) == DialogResult.OK)
				{
					printDoc.Print();
				}
			}
		}

		private void FilePrintPreview_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveDiagram != null)
			{
				System.Drawing.Printing.PrintDocument printDoc = this.ActiveDiagram.CreatePrintDocument();
				PrintPreviewDialog printPreviewDlg = new PrintPreviewDialog();
				printPreviewDlg.Document = printDoc;
				printPreviewDlg.ShowDialog(this);
			}
		}

		private void FilePageSetup_Click(object sender, System.EventArgs e)
		{
			Syncfusion.Windows.Forms.Diagram.Controls.Diagram activeDiagram = this.ActiveDiagram;

			if (activeDiagram != null && activeDiagram.Model != null)
			{
				PageSetupDialog dlg = new PageSetupDialog();
				dlg.PageSettings = activeDiagram.Model.PageSettings;
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					activeDiagram.Model.PageSettings = dlg.PageSettings;
				}
			}
		}

		#endregion

		#region Edit Menu Event Handlers

		private void EditSelectAll_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveDiagram != null)
			{
				this.ActiveDiagram.SelectAll();
			}
		}

		private void EditDelete_Click(object sender, System.EventArgs e)
		{
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem = (Syncfusion.Windows.Forms.Tools.XPMenus.BarItem) sender;

			SymbolDocument symbolDoc = this.ActiveMdiChild as SymbolDocument;
			if (symbolDoc != null)
			{
				symbolDoc.Diagram.Delete();
			}
		}

		private void EditUndo_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveDiagram != null)
			{
				this.ActiveDiagram.UndoCommand();
			}
		}

		private void EditRedo_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveDiagram != null)
			{
				this.ActiveDiagram.RedoCommand();
			}
		}

		private void EditPaste_Click(object sender, System.EventArgs e)
		{
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem = (Syncfusion.Windows.Forms.Tools.XPMenus.BarItem) sender;

			SymbolDocument symbolDoc = this.ActiveMdiChild as SymbolDocument;
			if (symbolDoc != null)
			{
				symbolDoc.Diagram.Paste();
			}		
		}

		private void EditCopy_Click(object sender, System.EventArgs e)
		{
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem = (Syncfusion.Windows.Forms.Tools.XPMenus.BarItem) sender;

			SymbolDocument symbolDoc = this.ActiveMdiChild as SymbolDocument;
			if (symbolDoc != null)
			{
				symbolDoc.Diagram.Copy();
			}		
		}

		private void EditCut_Click(object sender, System.EventArgs e)
		{
			Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem = (Syncfusion.Windows.Forms.Tools.XPMenus.BarItem) sender;

			SymbolDocument symbolDoc = this.ActiveMdiChild as SymbolDocument;
			if (symbolDoc != null)
			{
				symbolDoc.Diagram.Cut();
			}
		}

		#endregion

		#region View Menu Event Handlers

		private void ViewStatusBar_Click(object sender, System.EventArgs e)
		{
			if (this.statusBar.Visible)
			{
				this.statusBar.Hide();
				this.ViewStatusBar.Checked = false;
			}
			else
			{
				this.ViewStatusBar.Checked = true;
				this.statusBar.Show();
			}
		}

		private void ViewProperties_Click(object sender, System.EventArgs e)
		{
			if (this.ViewProperties.Checked)
			{
				this.dockingManager.SetDockVisibility(this.propertyEditor, false);
				this.ViewProperties.Checked = false;
			}
			else
			{
				this.dockingManager.SetDockVisibility(this.propertyEditor, true);
				this.ViewProperties.Checked = true;
			}
		}

		private void ViewOptions_Click(object sender, System.EventArgs e)
		{
			if (this.ViewOptions.Checked)
			{
				this.dockingManager.SetDockVisibility(this.optionsControl, false);
				this.ViewOptions.Checked = false;
			}
			else
			{
				this.dockingManager.SetDockVisibility(this.optionsControl, true);
				this.ViewOptions.Checked = true;
			}
		}

		private void ViewSymbolPalette_Click(object sender, System.EventArgs e)
		{
			if (this.ViewSymbolPalette.Checked)
			{
				this.dockingManager.SetDockVisibility(this.symbolPalettePanel, false);
				this.ViewSymbolPalette.Checked = false;
			}
			else
			{
				this.dockingManager.SetDockVisibility(this.symbolPalettePanel, true);
				this.ViewSymbolPalette.Checked = true;
			}
		}

		#endregion

		#region Window Menu Event Handlers

		private void tabbedMDI_Click(object sender, System.EventArgs e)
		{
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

		private void HelpAbout_Click(object sender, System.EventArgs e)
		{
			About aboutFrm = new About();
			aboutFrm.ShowDialog();
		}

		#endregion

		#region Symbol Event Handlers

		private void SymbolAdd_Click(object sender, System.EventArgs e)
		{
			SymbolPalette curPalette = symbolPaletteGroupBar.CurrentPalette;
			if (curPalette != null)
			{
				SymbolModel symbolMdl = curPalette.AddSymbol("New Symbol");
				SymbolDocument formSymbolDoc = new SymbolDocument(symbolMdl);
				formSymbolDoc.MdiParent = this;
				this.propertyEditor.Diagram = formSymbolDoc.Diagram;
				formSymbolDoc.Show();
			}
		}

		private void SymbolRemove_Click(object sender, System.EventArgs e)
		{
			SymbolPalette curPalette = symbolPaletteGroupBar.CurrentPalette;
			if (curPalette != null)
			{
				curPalette.RemoveSymbol(symbolPaletteGroupBar.SelectedSymbolModel);
			}
		}

		#endregion

		#region Symbol Palette Event Handlers

		private void symbolPaletteGroupBar_SymbolModelSelected(object sender, Syncfusion.Windows.Forms.Diagram.Controls.SymbolModelEventArgs evtArgs)
		{
			IEnumerator enumMdiChildren = tabbedMDIManager.MdiChildren.GetEnumerator();
			SymbolModel selectedModel = evtArgs.Model;
			while (enumMdiChildren.MoveNext())
			{
				SymbolDocument curMdiChild = enumMdiChildren.Current as SymbolDocument;
				if (curMdiChild != null)
				{
					if (curMdiChild.Model == selectedModel)
					{
						curMdiChild.BringToFront();
						break;
					}
				}
			}
			this.propertyEditor.SetSelectedObject(selectedModel);
		}

		private void symbolPaletteGroupBar_SymbolModelDoubleClick(object sender, Syncfusion.Windows.Forms.Diagram.Controls.SymbolModelEventArgs evtArgs)
		{
			SymbolDocument formSymbolDoc = null;
			SymbolDocument curSymbolDoc = null;
			SymbolModel modelClicked = evtArgs.Model;

			// First, look for an existing MDI child window that contains the given
			// model. If one isn't found, then create a new one.
			IEnumerator enumMdiChildren = tabbedMDIManager.MdiChildren.GetEnumerator();
			while (enumMdiChildren.MoveNext() && formSymbolDoc == null)
			{
				curSymbolDoc = enumMdiChildren.Current as SymbolDocument;
				if (curSymbolDoc != null)
				{
					if (curSymbolDoc.Model == modelClicked)
					{
						formSymbolDoc = curSymbolDoc;
					}
				}
			}

			if (formSymbolDoc == null)
			{
				formSymbolDoc = new SymbolDocument(evtArgs.Model);
				formSymbolDoc.MdiParent = this;
				formSymbolDoc.Show();
			}
			else
			{
				formSymbolDoc.BringToFront();
			}

			this.propertyEditor.Diagram = formSymbolDoc.Diagram;
		}

		private void symbolPalettePanel_Resize(object sender, System.EventArgs e)
		{
			// Make sure symbol palette group bar fills the panel
			symbolPaletteGroupBar.Height = symbolPalettePanel.Height - symbolPaletteGroupBar.Location.Y;
			symbolPaletteGroupBar.Width = symbolPalettePanel.Width - symbolPaletteGroupBar.Location.X;
		}

		#endregion

		#region UI Updating

		private void OnIdle(object sender, System.EventArgs evtArgs)
		{
			SymbolPalette curSymbolPalette = this.symbolPaletteGroupBar.CurrentPalette;

			if (curSymbolPalette != null)
			{
				this.FileSave.Enabled = true;
				this.FileSaveAs.Enabled = true;
				this.FileClose.Enabled = true;
			}
			else
			{
				this.FileSave.Enabled = false;
				this.FileSaveAs.Enabled = false;
				this.FileClose.Enabled = false;
			}

			if (this.ActiveDiagram != null)
			{
				this.EditCopy.Enabled = this.ActiveDiagram.CanCopy;
				this.EditCut.Enabled = this.ActiveDiagram.CanCut;
				this.EditPaste.Enabled = this.ActiveDiagram.CanPaste;
				this.EditUndo.Enabled = this.ActiveDiagram.CanUndo;
				this.EditRedo.Enabled = this.ActiveDiagram.CanRedo;
				this.EditSelectAll.Enabled = true;
				this.FilePrint.Enabled = true;
				this.FilePrintPreview.Enabled = true;
				this.FilePageSetup.Enabled = true;
			}
			else
			{
				this.EditCopy.Enabled = false;
				this.EditCut.Enabled = false;
				this.EditPaste.Enabled = false;
				this.EditSelectAll.Enabled = false;
				this.FilePrint.Enabled = false;
				this.FilePrintPreview.Enabled = false;
				this.FilePageSetup.Enabled = false;
			}
		}

		private void UpdateMainFormTitle()
		{
			string textVal = this.mainFormTitle;
			SymbolPalette curSymbolPalette = this.symbolPaletteGroupBar.CurrentPalette;
			if (curSymbolPalette != null)
			{
				textVal = textVal + " - " + curSymbolPalette.Name;
			}
			this.Text = textVal;
		}

		#endregion

		#region Docking

		private void dockingManager_DockVisibilityChanged(object sender, Syncfusion.Windows.Forms.Tools.DockVisibilityChangedEventArgs e)
		{
			if (e.Control == this.propertyEditor)
			{
				this.ViewProperties.Checked = this.dockingManager.GetDockVisibility(this.propertyEditor);
			}
			else if (e.Control == this.optionsControl)
			{
				this.ViewOptions.Checked = this.dockingManager.GetDockVisibility(this.optionsControl);
			}
			else if (e.Control == this.symbolPalettePanel)
			{
				this.ViewSymbolPalette.Checked = this.dockingManager.GetDockVisibility(this.symbolPalettePanel);
			}
		}

		#endregion

		#region Syncfusion BarManager

		public Syncfusion.Windows.Forms.Tools.XPMenus.BarManager BarManager
		{
			get
			{
				return mainFrameBarManager;
			}
		}

		#endregion

		#region IStatusUpdate interface

		void IStatusUpdate.SetXY(float x, float y)
		{
			this.statusBarXY.Text = "(X: " + x.ToString() + ", Y: " + y.ToString() + ")";
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
			this.mainFrameBarManager = new Syncfusion.Windows.Forms.Tools.XPMenus.MainFrameBarManager(this.components, this);
			this.mainMenu = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "MainMenu");
			this.parentBarItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.FileNew = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.smallImageList1 = new System.Windows.Forms.ImageList(this.components);
			this.FileOpen = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.FileSave = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.FileSaveAs = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.FileClose = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.FilePageSetup = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.FilePrintPreview = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.FilePrint = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.FileExit = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItemEdit = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.EditUndo = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.EditRedo = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.EditCut = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.EditCopy = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.EditPaste = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItemSymbol = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.SymbolAdd = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.SymbolRemove = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItem5 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.ViewSymbolPalette = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.ViewStatusBar = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.ViewProperties = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.ViewOptions = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItem4 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.tabbedMDI = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.parentBarItem3 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.HelpAbout = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barStandard = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(this.mainFrameBarManager, "Standard");
			this.barItemHelpContents = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.smBarItemImages = new System.Windows.Forms.ImageList(this.components);
			this.EditSelectAll = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.EditDelete = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.barItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.savePaletteFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.openPaletteFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.symbolPalettePanel = new System.Windows.Forms.Panel();
			this.xptbSymbolPalette = new Syncfusion.Windows.Forms.Tools.XPMenus.XPToolBar();
			this.symbolPaletteGroupBar = new Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar();
			this.dockingManager = new Syncfusion.Windows.Forms.Tools.DockingManager(this.components);
			this.propertyEditor = new Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor();
			this.optionsControl = new Syncfusion.SymbolDesigner.OptionsControl();
			this.openImageDialog = new System.Windows.Forms.OpenFileDialog();
			this.parentBarItem7 = new Syncfusion.Windows.Forms.Tools.XPMenus.ParentBarItem();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusBarXY = new System.Windows.Forms.StatusBarPanel();
			((System.ComponentModel.ISupportInitialize)(this.mainFrameBarManager)).BeginInit();
			this.symbolPalettePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dockingManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarXY)).BeginInit();
			this.SuspendLayout();
			// 
			// mainFrameBarManager
			// 
			this.mainFrameBarManager.BarPositionInfo = ((System.IO.MemoryStream)(resources.GetObject("mainFrameBarManager.BarPositionInfo")));
			this.mainFrameBarManager.Bars.Add(this.mainMenu);
			this.mainFrameBarManager.Bars.Add(this.barStandard);
			this.mainFrameBarManager.Categories.Add("File");
			this.mainFrameBarManager.Categories.Add("Edit");
			this.mainFrameBarManager.Categories.Add("Help");
			this.mainFrameBarManager.Categories.Add("Popups");
			this.mainFrameBarManager.Categories.Add("Window");
			this.mainFrameBarManager.Categories.Add("View");
			this.mainFrameBarManager.Categories.Add("Symbol");
			this.mainFrameBarManager.CurrentBaseFormType = "System.Windows.Forms.Form";
			this.mainFrameBarManager.Form = this;
			this.mainFrameBarManager.ImageList = this.smBarItemImages;
			this.mainFrameBarManager.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																											 this.FileNew,
																											 this.FileOpen,
																											 this.FileSave,
																											 this.FileSaveAs,
																											 this.FileClose,
																											 this.FilePageSetup,
																											 this.parentBarItem1,
																											 this.FilePrintPreview,
																											 this.FilePrint,
																											 this.FileExit,
																											 this.parentBarItemEdit,
																											 this.parentBarItem4,
																											 this.parentBarItem3,
																											 this.EditUndo,
																											 this.EditRedo,
																											 this.EditCut,
																											 this.EditCopy,
																											 this.EditPaste,
																											 this.HelpAbout,
																											 this.tabbedMDI,
																											 this.parentBarItem5,
																											 this.barItemHelpContents,
																											 this.EditSelectAll,
																											 this.ViewStatusBar,
																											 this.ViewProperties,
																											 this.EditDelete,
																											 this.SymbolAdd,
																											 this.parentBarItemSymbol,
																											 this.SymbolRemove,
																											 this.ViewOptions,
																											 this.ViewSymbolPalette});
			this.mainFrameBarManager.LargeImageList = null;
			this.mainFrameBarManager.ResetCustomization = false;
			// 
			// mainMenu
			// 
			this.mainMenu.BarName = "MainMenu";
			this.mainMenu.BarStyle = ((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle)(((((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.AllowQuickCustomizing | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.IsMainMenu) 
				| Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.RotateWhenVertical) 
				| Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.Visible) 
				| Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.DrawDragBorder)));
			this.mainMenu.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																								  this.parentBarItem1,
																								  this.parentBarItemEdit,
																								  this.parentBarItemSymbol,
																								  this.parentBarItem5,
																								  this.parentBarItem4,
																								  this.parentBarItem3});
			this.mainMenu.Manager = this.mainFrameBarManager;
			// 
			// parentBarItem1
			// 
			this.parentBarItem1.CategoryIndex = 3;
			this.parentBarItem1.ID = "&File";
			this.parentBarItem1.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										this.FileNew,
																										this.FileOpen,
																										this.FileSave,
																										this.FileSaveAs,
																										this.FileClose,
																										this.FilePageSetup,
																										this.FilePrintPreview,
																										this.FilePrint,
																										this.FileExit});
			this.parentBarItem1.SeparatorIndices.AddRange(new int[] {
																		5,
																		8});
			this.parentBarItem1.Text = "&File";
			// 
			// FileNew
			// 
			this.FileNew.CategoryIndex = 0;
			this.FileNew.ID = "&New";
			this.FileNew.ImageIndex = 6;
			this.FileNew.ImageList = this.smallImageList1;
			this.FileNew.Text = "&New";
			this.FileNew.Tooltip = "New Palette";
			this.FileNew.Click += new System.EventHandler(this.FileNew_Click);
			// 
			// smallImageList1
			// 
			this.smallImageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.smallImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImageList1.ImageStream")));
			this.smallImageList1.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// FileOpen
			// 
			this.FileOpen.CategoryIndex = 0;
			this.FileOpen.ID = "&Open";
			this.FileOpen.ImageIndex = 4;
			this.FileOpen.ImageList = this.smallImageList1;
			this.FileOpen.Text = "&Open";
			this.FileOpen.Tooltip = "Open Palette";
			this.FileOpen.Click += new System.EventHandler(this.FileOpen_Click);
			// 
			// FileSave
			// 
			this.FileSave.CategoryIndex = 0;
			this.FileSave.ID = "Save";
			this.FileSave.ImageIndex = 0;
			this.FileSave.ImageList = this.smallImageList1;
			this.FileSave.Text = "Save";
			this.FileSave.Tooltip = "Save Palette";
			this.FileSave.Click += new System.EventHandler(this.FileSave_Click);
			// 
			// FileSaveAs
			// 
			this.FileSaveAs.CategoryIndex = 0;
			this.FileSaveAs.ID = "Save As";
			this.FileSaveAs.Text = "Save As";
			this.FileSaveAs.Tooltip = "Save Palette As";
			this.FileSaveAs.Click += new System.EventHandler(this.FileSaveAs_Click);
			// 
			// FileClose
			// 
			this.FileClose.CategoryIndex = 0;
			this.FileClose.ID = "&Close";
			this.FileClose.Text = "&Close";
			this.FileClose.Click += new System.EventHandler(this.FileClose_Click);
			// 
			// FilePageSetup
			// 
			this.FilePageSetup.CategoryIndex = 0;
			this.FilePageSetup.ID = "Page Setup";
			this.FilePageSetup.ImageIndex = 8;
			this.FilePageSetup.ImageList = this.smallImageList1;
			this.FilePageSetup.Text = "Page Setup";
			this.FilePageSetup.Click += new System.EventHandler(this.FilePageSetup_Click);
			// 
			// FilePrintPreview
			// 
			this.FilePrintPreview.CategoryIndex = 0;
			this.FilePrintPreview.ID = "Print Preview";
			this.FilePrintPreview.ImageIndex = 16;
			this.FilePrintPreview.ImageList = this.smallImageList1;
			this.FilePrintPreview.Text = "Print Preview";
			this.FilePrintPreview.Tooltip = "Print Preview";
			this.FilePrintPreview.Click += new System.EventHandler(this.FilePrintPreview_Click);
			// 
			// FilePrint
			// 
			this.FilePrint.CategoryIndex = 0;
			this.FilePrint.ID = "Print";
			this.FilePrint.ImageIndex = 9;
			this.FilePrint.ImageList = this.smallImageList1;
			this.FilePrint.Text = "Print";
			this.FilePrint.Click += new System.EventHandler(this.FilePrint_Click);
			// 
			// FileExit
			// 
			this.FileExit.CategoryIndex = 0;
			this.FileExit.ID = "E&xit";
			this.FileExit.Text = "E&xit";
			this.FileExit.Tooltip = "Exit Symbol Designer";
			this.FileExit.Click += new System.EventHandler(this.FileExit_Click);
			// 
			// parentBarItemEdit
			// 
			this.parentBarItemEdit.CategoryIndex = 3;
			this.parentBarItemEdit.ID = "&Edit";
			this.parentBarItemEdit.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										   this.EditUndo,
																										   this.EditRedo,
																										   this.EditSelectAll,
																										   this.EditCut,
																										   this.EditCopy,
																										   this.EditPaste});
			this.parentBarItemEdit.SeparatorIndices.AddRange(new int[] {
																		   2,
																		   3});
			this.parentBarItemEdit.Text = "&Edit";
			// 
			// EditUndo
			// 
			this.EditUndo.CategoryIndex = 1;
			this.EditUndo.Enabled = false;
			this.EditUndo.ID = "&Undo";
			this.EditUndo.ImageIndex = 13;
			this.EditUndo.ImageList = this.smallImageList1;
			this.EditUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.EditUndo.Text = "&Undo";
			this.EditUndo.Click += new System.EventHandler(this.EditUndo_Click);
			// 
			// EditRedo
			// 
			this.EditRedo.CategoryIndex = 1;
			this.EditRedo.Enabled = false;
			this.EditRedo.ID = "&Redo";
			this.EditRedo.ImageIndex = 14;
			this.EditRedo.ImageList = this.smallImageList1;
			this.EditRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
			this.EditRedo.Text = "&Redo";
			this.EditRedo.Click += new System.EventHandler(this.EditRedo_Click);
			// 
			// EditCut
			// 
			this.EditCut.CategoryIndex = 1;
			this.EditCut.Enabled = false;
			this.EditCut.ID = "Cu&t";
			this.EditCut.ImageIndex = 10;
			this.EditCut.ImageList = this.smallImageList1;
			this.EditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.EditCut.Tag = "Cut";
			this.EditCut.Text = "Cu&t";
			this.EditCut.Click += new System.EventHandler(this.EditCut_Click);
			// 
			// EditCopy
			// 
			this.EditCopy.CategoryIndex = 1;
			this.EditCopy.Enabled = false;
			this.EditCopy.ID = "&Copy";
			this.EditCopy.ImageIndex = 11;
			this.EditCopy.ImageList = this.smallImageList1;
			this.EditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.EditCopy.Tag = "Copy";
			this.EditCopy.Text = "&Copy";
			this.EditCopy.Click += new System.EventHandler(this.EditCopy_Click);
			// 
			// EditPaste
			// 
			this.EditPaste.CategoryIndex = 1;
			this.EditPaste.Enabled = false;
			this.EditPaste.ID = "&Paste";
			this.EditPaste.ImageIndex = 12;
			this.EditPaste.ImageList = this.smallImageList1;
			this.EditPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.EditPaste.Tag = "Paste";
			this.EditPaste.Text = "&Paste";
			this.EditPaste.Click += new System.EventHandler(this.EditPaste_Click);
			// 
			// parentBarItemSymbol
			// 
			this.parentBarItemSymbol.CategoryIndex = 3;
			this.parentBarItemSymbol.ID = "Symbol";
			this.parentBarItemSymbol.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																											 this.SymbolAdd,
																											 this.SymbolRemove});
			this.parentBarItemSymbol.Text = "Symbol";
			// 
			// SymbolAdd
			// 
			this.SymbolAdd.CategoryIndex = 6;
			this.SymbolAdd.ID = "Add";
			this.SymbolAdd.ImageIndex = 12;
			this.SymbolAdd.Text = "Add";
			this.SymbolAdd.Tooltip = "Add Symbol";
			this.SymbolAdd.Click += new System.EventHandler(this.SymbolAdd_Click);
			// 
			// SymbolRemove
			// 
			this.SymbolRemove.CategoryIndex = 6;
			this.SymbolRemove.ID = "Remove";
			this.SymbolRemove.ImageIndex = 43;
			this.SymbolRemove.Text = "Remove";
			this.SymbolRemove.Tooltip = "Remove Symbol";
			this.SymbolRemove.Click += new System.EventHandler(this.SymbolRemove_Click);
			// 
			// parentBarItem5
			// 
			this.parentBarItem5.CategoryIndex = 3;
			this.parentBarItem5.ID = "View";
			this.parentBarItem5.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										this.ViewSymbolPalette,
																										this.ViewStatusBar,
																										this.ViewProperties,
																										this.ViewOptions});
			this.parentBarItem5.Text = "View";
			// 
			// ViewSymbolPalette
			// 
			this.ViewSymbolPalette.CategoryIndex = 5;
			this.ViewSymbolPalette.ID = "Symbol Palette";
			this.ViewSymbolPalette.Text = "Symbol Palette";
			this.ViewSymbolPalette.Click += new System.EventHandler(this.ViewSymbolPalette_Click);
			// 
			// ViewStatusBar
			// 
			this.ViewStatusBar.CategoryIndex = 5;
			this.ViewStatusBar.ID = "Status Bar";
			this.ViewStatusBar.Text = "Status Bar";
			this.ViewStatusBar.Click += new System.EventHandler(this.ViewStatusBar_Click);
			// 
			// ViewProperties
			// 
			this.ViewProperties.CategoryIndex = 5;
			this.ViewProperties.ID = "Properties";
			this.ViewProperties.Text = "Properties";
			this.ViewProperties.Click += new System.EventHandler(this.ViewProperties_Click);
			// 
			// ViewOptions
			// 
			this.ViewOptions.CategoryIndex = 5;
			this.ViewOptions.ID = "Options";
			this.ViewOptions.Text = "Options";
			this.ViewOptions.Click += new System.EventHandler(this.ViewOptions_Click);
			// 
			// parentBarItem4
			// 
			this.parentBarItem4.CategoryIndex = 3;
			this.parentBarItem4.ID = "Window";
			this.parentBarItem4.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										this.tabbedMDI});
			this.parentBarItem4.Text = "Window";
			// 
			// tabbedMDI
			// 
			this.tabbedMDI.CategoryIndex = 4;
			this.tabbedMDI.Checked = true;
			this.tabbedMDI.ID = "Tabbed MDI";
			this.tabbedMDI.Text = "Tabbed MDI";
			this.tabbedMDI.Click += new System.EventHandler(this.tabbedMDI_Click);
			// 
			// parentBarItem3
			// 
			this.parentBarItem3.CategoryIndex = 3;
			this.parentBarItem3.ID = "Help";
			this.parentBarItem3.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										this.HelpAbout});
			this.parentBarItem3.Text = "Help";
			// 
			// HelpAbout
			// 
			this.HelpAbout.CategoryIndex = 2;
			this.HelpAbout.ID = "About...";
			this.HelpAbout.Text = "About...";
			this.HelpAbout.Click += new System.EventHandler(this.HelpAbout_Click);
			// 
			// barStandard
			// 
			this.barStandard.BarName = "Standard";
			this.barStandard.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																									 this.FileNew,
																									 this.FileOpen,
																									 this.FileSave,
																									 this.EditCut,
																									 this.EditCopy,
																									 this.EditPaste,
																									 this.EditUndo,
																									 this.EditRedo,
																									 this.FilePrint,
																									 this.barItemHelpContents});
			this.barStandard.Manager = this.mainFrameBarManager;
			// 
			// barItemHelpContents
			// 
			this.barItemHelpContents.CategoryIndex = 2;
			this.barItemHelpContents.ID = "Help Contents";
			this.barItemHelpContents.ImageIndex = 15;
			this.barItemHelpContents.ImageList = this.smallImageList1;
			this.barItemHelpContents.Text = "Help Contents";
			this.barItemHelpContents.Tooltip = "Help";
			// 
			// smBarItemImages
			// 
			this.smBarItemImages.ImageSize = new System.Drawing.Size(16, 16);
			this.smBarItemImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smBarItemImages.ImageStream")));
			this.smBarItemImages.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// EditSelectAll
			// 
			this.EditSelectAll.CategoryIndex = 1;
			this.EditSelectAll.ID = "Select All";
			this.EditSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.EditSelectAll.Text = "Select &All";
			this.EditSelectAll.Click += new System.EventHandler(this.EditSelectAll_Click);
			// 
			// EditDelete
			// 
			this.EditDelete.CategoryIndex = 1;
			this.EditDelete.ID = "&Delete";
			this.EditDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.EditDelete.Text = "&Delete";
			this.EditDelete.Click += new System.EventHandler(this.EditDelete_Click);
			// 
			// barItem1
			// 
			this.barItem1.CategoryIndex = -1;
			this.barItem1.ID = "";
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(0, 52);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 530);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// savePaletteFileDialog
			// 
			this.savePaletteFileDialog.DefaultExt = "*.edp";
			this.savePaletteFileDialog.FileName = "doc1";
			this.savePaletteFileDialog.Filter = "ED Symbol Palettes (*.edp)|*.edp|All files (*.*)|*.*";
			// 
			// openPaletteFileDialog
			// 
			this.openPaletteFileDialog.Filter = "ED Symbol Palettes (*.edp)|*.edp|All files (*.*)|*.*";
			// 
			// symbolPalettePanel
			// 
			this.symbolPalettePanel.Controls.Add(this.xptbSymbolPalette);
			this.symbolPalettePanel.Controls.Add(this.symbolPaletteGroupBar);
			this.dockingManager.SetEnableDocking(this.symbolPalettePanel, true);
			this.symbolPalettePanel.Location = new System.Drawing.Point(1, 20);
			this.symbolPalettePanel.Name = "symbolPalettePanel";
			this.symbolPalettePanel.Size = new System.Drawing.Size(169, 509);
			this.symbolPalettePanel.TabIndex = 8;
			this.symbolPalettePanel.Resize += new System.EventHandler(this.symbolPalettePanel_Resize);
			// 
			// xptbSymbolPalette
			// 
			this.xptbSymbolPalette.Bar = new Syncfusion.Windows.Forms.Tools.XPMenus.Bar(null, "", ((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle)(((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.AllowQuickCustomizing | Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.Visible) 
				| Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.DrawDragBorder))), null, null);
			this.xptbSymbolPalette.Dock = System.Windows.Forms.DockStyle.Top;
			this.xptbSymbolPalette.Items.AddRange(new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem[] {
																										   this.SymbolAdd,
																										   this.SymbolRemove});
			this.xptbSymbolPalette.Location = new System.Drawing.Point(0, 0);
			this.xptbSymbolPalette.Name = "xptbSymbolPalette";
			this.xptbSymbolPalette.Size = new System.Drawing.Size(169, 27);
			this.xptbSymbolPalette.TabIndex = 7;
			this.xptbSymbolPalette.Text = "xpToolBar1";
			// 
			// symbolPaletteGroupBar
			// 
			this.symbolPaletteGroupBar.AllowDrop = true;
			this.symbolPaletteGroupBar.EditMode = true;
			this.symbolPaletteGroupBar.Location = new System.Drawing.Point(0, 24);
			this.symbolPaletteGroupBar.Name = "symbolPaletteGroupBar";
			this.symbolPaletteGroupBar.SelectedItem = 0;
			this.symbolPaletteGroupBar.Size = new System.Drawing.Size(184, 488);
			this.symbolPaletteGroupBar.TabIndex = 6;
			this.symbolPaletteGroupBar.Text = "Symbol Palettes";
			this.symbolPaletteGroupBar.SymbolModelSelected += new Syncfusion.Windows.Forms.Diagram.Controls.SymbolModelEvent(this.symbolPaletteGroupBar_SymbolModelSelected);
			this.symbolPaletteGroupBar.SymbolModelDoubleClick += new Syncfusion.Windows.Forms.Diagram.Controls.SymbolModelEvent(this.symbolPaletteGroupBar_SymbolModelDoubleClick);
			// 
			// dockingManager
			// 
			this.dockingManager.DockLayoutStream = ((System.IO.MemoryStream)(resources.GetObject("dockingManager.DockLayoutStream")));
			this.dockingManager.HostForm = this;
			this.dockingManager.DockVisibilityChanged += new Syncfusion.Windows.Forms.Tools.DockVisibilityChangedEventHandler(this.dockingManager_DockVisibilityChanged);
			this.dockingManager.SetDockLabel(this.symbolPalettePanel, "Symbol Palette");
			this.dockingManager.SetDockLabel(this.propertyEditor, "Properties");
			this.dockingManager.SetDockLabel(this.optionsControl, "Options");
			this.dockingManager.SetFloatOnly(this.optionsControl, true);
			this.dockingManager.SetHiddenOnLoad(this.optionsControl, true);
			// 
			// propertyEditor
			// 
			this.propertyEditor.Diagram = null;
			this.dockingManager.SetEnableDocking(this.propertyEditor, true);
			this.propertyEditor.Location = new System.Drawing.Point(1, 20);
			this.propertyEditor.Name = "propertyEditor";
			this.propertyEditor.Size = new System.Drawing.Size(203, 509);
			this.propertyEditor.TabIndex = 11;
			// 
			// optionsControl
			// 
			this.optionsControl.Diagram = null;
			this.dockingManager.SetEnableDocking(this.optionsControl, true);
			this.optionsControl.Location = new System.Drawing.Point(1, 1);
			this.optionsControl.Name = "optionsControl";
			this.optionsControl.Size = new System.Drawing.Size(285, 185);
			this.optionsControl.TabIndex = 23;
			// 
			// openImageDialog
			// 
			this.openImageDialog.DefaultExt = "*.bmp;*.jpg;*.gif;*.png;*.emf";
			this.openImageDialog.Filter = "Windows Bitmaps (*.bmp)|*.bmp|JPEG files (*.jpg)|*.jpg|Graphics Interchange Forma" +
				"t files (*.gif)|*.gif|Portable Network Graphics files (*.png)|*.png| Enhanced Me" +
				"tafiles (*.emf)|*.emf|All files (*.*)|*.*";
			this.openImageDialog.Title = "Select an image file";
			// 
			// parentBarItem7
			// 
			this.parentBarItem7.CategoryIndex = -1;
			this.parentBarItem7.ID = "";
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 582);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.statusBarXY});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(712, 24);
			this.statusBar.TabIndex = 15;
			// 
			// statusBarXY
			// 
			this.statusBarXY.MinWidth = 40;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 606);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.statusBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.Name = "MainForm";
			this.Text = "Symbol Designer";
			this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.mainFrameBarManager)).EndInit();
			this.symbolPalettePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dockingManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarXY)).EndInit();
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
	}
}
