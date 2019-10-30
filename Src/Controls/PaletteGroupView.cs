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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using System.CodeDom;

using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Diagram;

namespace Syncfusion.Windows.Forms.Diagram.Controls
{
	/// <summary>
	/// Displays the symbol models belonging to a symbol palette in a GroupView.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class is derived from the Syncfusion.Windows.Forms.Tools.GroupView
	/// class. A GroupView is a control that contains a list of icons and labels
	/// that can be hosted in a GroupBar (Outlook bar).
	/// </para>
	/// <para>
	/// This class provides an implementation that displays a list of symbol models
	/// that belong to a given symbol palette. Symbol models can be dragged from
	/// this control onto diagrams.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel"/>
	/// </remarks>
	[
		DesignerSerializer(typeof(PaletteGroupViewCodeDOMSerializer), typeof(CodeDomSerializer))
	]
	public class PaletteGroupView : GroupView
	{
		private System.ComponentModel.IContainer components;
		private SymbolPalette palette = null;
		private bool beginDrag = false;
		private System.Windows.Forms.ImageList smImages;
		private System.Windows.Forms.ImageList lgImages;
		private bool editMode = false;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PaletteGroupView()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PaletteGroupView));
			this.smImages = new System.Windows.Forms.ImageList(this.components);
			this.lgImages = new System.Windows.Forms.ImageList(this.components);
			// 
			// ctrlToolTip
			// 
			this.ctrlToolTip.ClientSize = new System.Drawing.Size(300, 300);
			this.ctrlToolTip.Location = new System.Drawing.Point(132, 174);
			this.ctrlToolTip.Name = "ctrlToolTip";
			// 
			// smImages
			// 
			this.smImages.ImageSize = new System.Drawing.Size(16, 16);
			this.smImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smImages.ImageStream")));
			this.smImages.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// lgImages
			// 
			this.lgImages.ImageSize = new System.Drawing.Size(32, 32);
			this.lgImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("lgImages.ImageStream")));
			this.lgImages.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// PaletteGroupView
			// 
			this.LargeImageList = this.lgImages;
			this.SmallImageList = this.smImages;

		}
		#endregion

		/// <summary>
		/// Determines whether or not the symbols can be dragged from the palette
		/// onto a diagram.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When this property is set to true, the palette is being edited and symbols
		/// cannot be dragged onto a diagram. The Symbol Designer sets this flag to true.
		/// The typical setting for an application is false, which means that the palette
		/// is not being editing and symbols can be dragged onto a diagram.
		/// </para>
		/// </remarks>
		[
		Browsable(true)
		]
		public bool EditMode
		{
			get
			{
				return this.editMode;
			}
			set
			{
				this.editMode = value;
			}
		}

		/// <summary>
		/// Reference to the symbol palette displayed by this control.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Editor(typeof(PaletteOpener), typeof(System.Drawing.Design.UITypeEditor)),
		TypeConverter(typeof(SymbolPaletteConverter))
		]
		public SymbolPalette Palette
		{
			get
			{
				return this.palette;
			}
			set
			{
				this.palette = value;
			}
		}

		/// <summary>
		/// Loads a symbol palette from a file.
		/// </summary>
		/// <param name="filename">Name of file to load</param>
		/// <returns>true if successful; otherwise false</returns>
		/// <remarks>
		/// Deserializes a symbol palette from disk and loads it into this
		/// control.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// </remarks>
		public bool LoadPalette(string filename)
		{
			bool success = false;
			SymbolPalette palette = null;
			FileStream iStream = null;

			if (File.Exists(filename))
			{
				try
				{
					System.AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
					iStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
					SoapFormatter formatter = new SoapFormatter();
					palette = (SymbolPalette) formatter.Deserialize(iStream);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					success = false;
					palette = null;
				}
				finally
				{
					iStream.Close();
					System.AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
				}
			}

			if (palette != null)
			{
				success = this.LoadPalette(palette);
			}

			return success;
		}

		/// <summary>
		/// Loads the given symbol palette into this control.
		/// </summary>
		/// <param name="palette">Palette to load</param>
		/// <returns>true if successful; otherwise false</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// </remarks>
		public bool LoadPalette(SymbolPalette palette)
		{
			if (palette == null)
			{
				return false;
			}

			bool success = true;

			int symbolMdlIdx;
			SymbolModel symModel = null;
			GroupViewItem gvItem = null;

			for (symbolMdlIdx = 0; symbolMdlIdx < palette.ChildCount; symbolMdlIdx++)
			{
				symModel = palette.GetChild(symbolMdlIdx) as SymbolModel;
				if (symModel != null)
				{
					int imageIdx = 0;
					int lgImageIdx = 0;
					int smImageIdx = 0;

					if (symModel.SmallIcon != null)
					{
						smImageIdx = this.AddSmallIcon(symModel.SmallIcon);
					}

					if (symModel.LargeIcon != null)
					{
						lgImageIdx = this.AddLargeIcon(symModel.LargeIcon);
					}

					if (this.SmallImageView)
					{
						imageIdx = smImageIdx;
					}
					else
					{
						imageIdx = lgImageIdx;
					}

					gvItem = new GroupViewPaletteItem(symModel, imageIdx);
					this.GroupViewItems.Add(gvItem);
				}
			}

			palette.ChildrenChangeComplete += new NodeCollection.EventHandler(OnPalette_ChildrenChangeComplete);
			palette.EventsEnabled = true;
			this.palette = palette;

			return success;
		}

		/// <summary>
		/// Loads a symbol palette from a resource file.
		/// </summary>
		/// <param name="assembly">Assembly containing the symbol palette</param>
		/// <param name="baseName">Base name of resource</param>
		/// <param name="resName">Name of resource</param>
		/// <returns>true if successful; otherwise false</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// </remarks>
		public bool LoadPalette(System.Reflection.Assembly assembly, string baseName, string resName)
		{
			bool success = false;
			SymbolPalette palette = null;
			System.Resources.ResourceManager resMgr = new System.Resources.ResourceManager(baseName, assembly);
			object resObj = resMgr.GetObject(resName);
			if (resObj != null && resObj.GetType() == typeof(byte[]))
			{
				System.IO.MemoryStream strmRes = new MemoryStream((byte[]) resObj);
				BinaryFormatter formatter = new BinaryFormatter();
				try
				{
					System.AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
					palette = (SymbolPalette) formatter.Deserialize(strmRes);
				}
				finally
				{
					strmRes.Close();
					System.AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
				}

				if (palette != null)
				{
					success = this.LoadPalette(palette);
				}
			}
			return success;
		}

		/// <summary>
		/// Loads a symbol palette from memory.
		/// </summary>
		/// <param name="strmData">Array of bytes containing serialized symbol palette</param>
		/// <returns>true if successful; otherwise false</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// </remarks>
		public bool LoadPalette(byte[] strmData)
		{
			bool success = false;
			SymbolPalette palette = null;

			System.IO.MemoryStream strmRes = new MemoryStream((byte[]) strmData);
			BinaryFormatter formatter = new BinaryFormatter();
			try
			{
				System.AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
				palette = (SymbolPalette) formatter.Deserialize(strmRes);
			}
			finally
			{
				strmRes.Close();
				System.AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
			}

			if (palette != null)
			{
				success = this.LoadPalette(palette);
			}

			return success;
		}

		/// <summary>
		/// Returns the currently selected symbol model
		/// </summary>
		/// <remarks>
		/// Symbol models are the items displayed by this control. This
		/// property returns the one that is currently selected.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public SymbolModel SelectedSymbolModel
		{
			get
			{
				SymbolModel symbolMdl = null;

				if (this.SelectedItem >= 0 && this.SelectedItem < this.GroupViewItems.Count)
				{
					symbolMdl = this.GroupViewItems[this.SelectedItem].Tag as SymbolModel;
				}

				return symbolMdl;
			}
		}

		/// <summary>
		/// Set the selected symbol model to the one matching the given symbol
		/// model name.
		/// </summary>
		/// <param name="symbolName">Name of symbol model to select</param>
		public bool SelectSymbolModel(string symbolName)
		{
			bool found = false;

			for (int gvIdx = 0; gvIdx < this.GroupViewItems.Count; gvIdx++)
			{
				GroupViewItem gvItem = this.GroupViewItems[gvIdx];

				if (gvItem != null)
				{
					if (gvItem.Tag != null)
					{
						SymbolModel curSymbolMdl = gvItem.Tag as SymbolModel;
						if (curSymbolMdl != null)
						{
							if (curSymbolMdl.Name == symbolName)
							{
								found = true;
								this.SelectedItem = gvIdx;
								break;
							}
						}
					}
				}
			}

			return found;
		}

		/// <summary>
		/// Adds a large icon to the image list.
		/// </summary>
		/// <param name="lgIcon">Icon to add</param>
		/// <returns>Index position at which the icon was added</returns>
		/// <remarks>
		/// <para>
		/// This control maintains two image lists - one for large icons and one
		/// for small icons.
		/// </para>
		/// </remarks>
		public int AddLargeIcon(System.Drawing.Image lgIcon)
		{
			this.lgImages.Images.Add(lgIcon);
			return (this.lgImages.Images.Count - 1);
		}

		/// <summary>
		/// Adds a small icon to the image list.
		/// </summary>
		/// <param name="smIcon">Icon to add</param>
		/// <returns>Index position at which the icon was added</returns>
		/// <remarks>
		/// <para>
		/// This control maintains two image lists - one for large icons and one
		/// for small icons.
		/// </para>
		/// </remarks>
		public int AddSmallIcon(System.Drawing.Image smIcon)
		{
			this.smImages.Images.Add(smIcon);
			return (this.smImages.Images.Count - 1);
		}

		/// <summary>
		/// Called when an item is selected.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView.SymbolModelSelected"/>
		/// event.
		/// </remarks>
		protected override void OnGroupViewItemSelected(System.EventArgs evtArgs)
		{
			base.OnGroupViewItemSelected(evtArgs);

			if (this.SymbolModelSelected != null)
			{
				this.SymbolModelSelected(this, new SymbolModelEventArgs(this.SelectedSymbolModel));
			}
		}

		/// <summary>
		/// Called when an item is double clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView.SymbolModelDoubleClick"/>
		/// event.
		/// </remarks>
		protected override void OnDoubleClick(System.EventArgs evtArgs)
		{
			base.OnDoubleClick(evtArgs);

			if (this.SymbolModelDoubleClick != null)
			{
				this.SymbolModelDoubleClick(this, new SymbolModelEventArgs(this.SelectedSymbolModel));
			}
		}

		/// <summary>
		/// Called when a mouse down event is received.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// If <see cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView.EditMode"/>
		/// is enabled, then this method starts a drag and drop operation.
		/// </para>
		/// </remarks>
		protected override void OnMouseDown(MouseEventArgs evtArgs)
		{
			base.OnMouseDown(evtArgs);

			if (!this.editMode)
			{
				this.beginDrag = true;
			}
		}

		/// <summary>
		/// Called when a mouse move event is received.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// If a drag and drop operation has been started with a mouse down, this
		/// method calls the System.Windows.Forms.Control.DoDragDrop method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView.OnMouseDown"/>
		/// </remarks>
		protected override void OnMouseMove(MouseEventArgs evtArgs)
		{
			base.OnMouseMove(evtArgs);

			if (this.beginDrag)
			{
				SymbolModel symModel = this.SelectedSymbolModel;
				NodeCollection nodes = new NodeCollection();
				nodes.Add(symModel.CreateSymbol());
				DragDropEffects dde = DoDragDrop(nodes, DragDropEffects.Copy);
				this.beginDrag = false;
			}
		}

		/// <summary>
		/// Called when a mouse up event is received.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		protected override void OnMouseUp(MouseEventArgs evtArgs)
		{
			base.OnMouseUp(evtArgs);
			this.beginDrag = false;
		}

		private void OnPalette_ChildrenChangeComplete(object sender, NodeCollection.EventArgs nodeEvtArgs)
		{
			if (nodeEvtArgs.ChangeType == CollectionEx.ChangeType.Insert)
			{
				SymbolModel symModel = nodeEvtArgs.Node as SymbolModel;

				if (symModel != null)
				{
					GroupViewItem gvItem = new GroupViewPaletteItem(symModel);
					this.GroupViewItems.Add(gvItem);
				}
			}
			else if (nodeEvtArgs.ChangeType == CollectionEx.ChangeType.Remove)
			{
				if (nodeEvtArgs.Node == this.SelectedSymbolModel)
				{
					this.SelectedItem = -1;
				}

				int gviIdx = 0;
				bool found = false;

				while (gviIdx < this.GroupViewItems.Count && !found)
				{
					if (this.GroupViewItems[gviIdx].Tag == nodeEvtArgs.Node)
					{
						found = true;
					}
					else
					{
						gviIdx++;
					}
				}

				if (found)
				{
					this.GroupViewItems.RemoveAt(gviIdx);
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// GroupViewItem derived class representing a symbol model in a group view.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel"/>
		/// </remarks>
		public class GroupViewPaletteItem : GroupViewItem
		{
			/// <summary>
			/// Construct a GroupViewPaletteItem given a symbol model
			/// </summary>
			/// <param name="symModel">Symbol model to display in the group view</param>
			public GroupViewPaletteItem(SymbolModel symModel) : base(symModel.Name, 0)
			{
				this.Tag = symModel;
				symModel.PropertyChanged += new PropertyEventHandler(this.OnPalettePropertyChanged);
			}

			/// <summary>
			/// Construct a GroupViewPaletteItem given a symbol model
			/// </summary>
			/// <param name="symModel">Symbol model to display in the group view</param>
			/// <param name="imageIdx"></param>
			public GroupViewPaletteItem(SymbolModel symModel, int imageIdx) : base(symModel.Name, imageIdx)
			{
				this.Tag = symModel;
				symModel.PropertyChanged += new PropertyEventHandler(this.OnPalettePropertyChanged);
			}

			private void OnPalettePropertyChanged(object sender, PropertyEventArgs evtArgs)
			{
				if (this.Tag == evtArgs.Node)
				{
					this.Text = evtArgs.Node.Name;

					PaletteGroupView groupView = this.GroupView as PaletteGroupView;
					SymbolModel symModel = evtArgs.Node as SymbolModel;
					if (groupView != null && symModel != null)
					{
						if (evtArgs.PropertyName == "LargeIcon" && !groupView.SmallImageView)
						{
							if (symModel.LargeIcon != null)
							{
								this.ImageIndex = groupView.AddLargeIcon(symModel.LargeIcon);
							}
							else
							{
								this.ImageIndex = 0;
							}
						}
						else if (evtArgs.PropertyName == "SmallIcon" && groupView.SmallImageView)
						{
							if (symModel.SmallIcon != null)
							{
								this.ImageIndex = groupView.AddLargeIcon(symModel.SmallIcon);
							}
							else
							{
								this.ImageIndex = 0;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Fired when the user selects a symbol model icon in a PaletteGroupBar component.
		/// </summary>
		public event SymbolModelEvent SymbolModelSelected;

		/// <summary>
		/// Fired when the user double clicks a symbol model icon in a PaletteGroupBar component.
		/// </summary>
		public event SymbolModelEvent SymbolModelDoubleClick;
	}

	/// <summary>
	/// Serializes a PaletteGroupView to the code DOM
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView"/>
	/// </remarks>
	public class PaletteGroupViewCodeDOMSerializer : CodeDomSerializer
	{
		/// <summary>
		/// Serializes a PaletteGroupView object to the code DOM.
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			object retVal = value;
			CodeDomSerializer baseSerializer = (CodeDomSerializer) manager.GetSerializer(typeof(GroupView), typeof(CodeDomSerializer));
			if (baseSerializer != null)
			{
				retVal = baseSerializer.Serialize(manager, value);
				if (retVal != null)
				{
					System.CodeDom.CodeStatementCollection stmts = retVal as System.CodeDom.CodeStatementCollection;
					if (stmts != null)
					{
						PaletteGroupView palGrpVw = value as PaletteGroupView;
						if (palGrpVw != null)
						{
							SymbolPalette pal = palGrpVw.Palette;
							if (pal != null)
							{
								// Serialize the palette into the resource file

								BinaryFormatter formatter = new BinaryFormatter();
								System.IO.MemoryStream oStream = new System.IO.MemoryStream();
								formatter.Serialize(oStream, pal);
								string resName = palGrpVw.Name + "." + "Palette";
								this.SerializeResource(manager, resName, oStream.GetBuffer());
								oStream.Close();

								// Add statements to CodeDom to load the palette from
								// the resource file at run-time

								string formClassName = "";
								IDesignerHost designerHost = manager.GetService(typeof(IDesignerHost)) as IDesignerHost;
								if (designerHost != null)
								{
									formClassName = designerHost.RootComponentClassName;
									CodePropertyReferenceExpression exprRefGroupView = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), palGrpVw.Name);
									CodeExpression[] loadPalParams = new CodeExpression[]
									{
										new CodePropertyReferenceExpression(new CodeTypeOfExpression(formClassName), "Assembly"),
										new CodePrimitiveExpression(formClassName),
										new CodePrimitiveExpression(resName)
									};
									CodeMethodInvokeExpression stmtLoadPalette = new CodeMethodInvokeExpression(exprRefGroupView, "LoadPalette", loadPalParams);
									stmts.Add(stmtLoadPalette);
								}
							}
						}
					}
				}
			}
			return retVal;
		}

		/// <summary>
		/// Deserializes a PaletteGroupView object from the code DOM.
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="codeObject"></param>
		/// <returns></returns>
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			object retVal = null;

			// Get base class serializer
			CodeDomSerializer baseSerializer = (CodeDomSerializer) manager.GetSerializer(typeof(GroupView), typeof(CodeDomSerializer));

			if (baseSerializer != null)
			{
				// Deserialize palette group view using base class serializer
				retVal = baseSerializer.Deserialize(manager, codeObject);

				if (retVal != null)
				{
					// Downcast to PaletteGroupView
					PaletteGroupView palGrpVw = retVal as PaletteGroupView;

					if (palGrpVw != null)
					{
						// Load symbol palette from the resource file and attach
						// to the palette group view
						string resName = palGrpVw.Name + "." + "Palette";
						System.ComponentModel.Design.IResourceService resSvc = manager.GetService(typeof(System.ComponentModel.Design.IResourceService)) as System.ComponentModel.Design.IResourceService;
						if (resSvc != null)
						{
							System.Resources.IResourceReader resReader = resSvc.GetResourceReader(System.Globalization.CultureInfo.InvariantCulture);
							if (resReader != null)
							{
								// Iterate through the resource file using the resource
								// reader and find the byte stream for the palette
								byte[] strmData = null;
								bool resFound = false;
								IDictionaryEnumerator resEnum = resReader.GetEnumerator();
								while (resEnum.MoveNext() && !resFound)
								{
									if ((string) resEnum.Key == resName)
									{
										strmData = (byte[]) resEnum.Value;
										resFound = true;
									}
								}

								if (resFound && strmData != null)
								{
									palGrpVw.LoadPalette(strmData);
								}
							}
						}
					}
				}
			}

			return retVal;
		}
	}
}
