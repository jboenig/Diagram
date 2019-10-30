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
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Diagram;
using Syncfusion.Runtime.InteropServices.WinAPI;

namespace Syncfusion.Windows.Forms.Diagram.Controls
{
	/// <summary>
	/// Interactive two-dimensional graphics control for diagramming,
	/// technical drawing, visualization, simulation, and technical
	/// drawing applications.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This control provides a surface for rendering and manipulating 2D
	/// shapes, symbols, text, and images. The user interface supports drag
	/// and drop, scaling, rotation, zooming, grouping, ungrouping, connection
	/// points, and many other features.
	/// </para>
	/// <para>
	/// A diagram is composed of three objects: the 
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.Model"/>,
	/// the 
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.View"/>,
	/// and the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.Controller"/>.
	/// The model-view-controller architecture provides a clear separation between
	/// data, visualization, and user interface. The Model contains the data portion
	/// of the diagram, the View is responsible for rendering the diagram, and
	/// the controller handles user interaction. The Model, View, and Controller
	/// are accessible as properties in this control and can be manipulated
	/// directly.
	/// </para>
	/// <para>
	/// Some of the methods and properties in this class are just wrappers that
	/// call identical methods in the model, view, or controller. For example,
	/// the following two lines of codes are equivalent.
	/// <code>
	///		diagram.Undo();
	///		// Same as
	///		diagram.Controller.Undo();
	/// </code>
	/// Methods that are simple wrappers are documented as such.
	/// </para>
	/// <para>
	/// Graphical objects can be added to a diagram in several ways. One way is
	/// through drag and drop. Symbols can be dragged from a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView"/>
	/// onto the diagram. Objects can also be added from the clipboard using the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.Paste"/>
	/// method. Shapes can be drawn onto the diagram by activating one of several
	/// drawing tools such as the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.RectangleTool"/>. Objects
	/// can also be created programmatically and added to the diagram using an
	/// <see cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/> or by
	/// calling the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.AppendChild"/> method.
	/// </para>
	/// <para>
	/// Activating user-interface tools is a task commonly performed by applications
	/// using this control. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.ActivateTool"/>
	/// method is used to activate tools. For example, the event handler for a
	/// toolbar button that draws a rectangle would look like this.
	/// <code>
	/// private void drawRectangle_Click(object sender, System.EventArgs e)
	/// {
	///		this.Diagram.ActivateTool("RectangleTool");
	/// }
	/// </code>
	/// </para>
	/// <para>
	/// The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.ExecuteCommand"/>
	/// method can be used to execute a command. A command is an object that encapsulates
	/// an action and parameters used to execute the action. When a command is executed,
	/// it is placed in an undo stack. Calling the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.UndoCommand"/>
	/// method removes the command on the top of the undo stack and causes an undo
	/// to occur. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.RedoCommand"/>
	/// method will redo the last command that was removed from the undo stack. The
	/// UndoCommand and RedoCommand methods are usually called in response to clicking
	/// Undo and Redo on the Edit menu.
	/// </para>
	/// <para>
	/// One advantage of the model-view-controller architecture is that the
	/// parts are interchangable. Models, views, and controllers can be swapped
	/// in and out independently. For example, the user interface of the diagram
	/// can be completely replaced by swapping in a different controller
	/// implementation. To accomplish this, you must subclass this class and
	/// override one or more of the following methods -
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.CreateModel"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.CreateView"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.CreateView"/>.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICommand"/>
	/// </remarks>
	[
	ToolboxItem(true),
	ToolboxBitmap(typeof(Diagram), "ToolboxIcons.Diagram.bmp"),
	Description("Interactive 2D graphics and diagramming")
	]
	public class Diagram : ScrollControl
	{
		#region Fields

		private Model model = null;
		private View view = null;
		private Controller controller = null;
		private LayoutManager layoutManager = null;

		private System.Drawing.Rectangle dragRect = new System.Drawing.Rectangle(0,0,0,0);

		private System.Drawing.Printing.PageSettings pageSettings = null;
		private System.Windows.Forms.ContextMenu contextMenuSave = null;

		private float scrollGranularity = 0.5f;
		private float nudgeIncrement = 1.0f;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a Diagram object.
		/// </summary>
		public Diagram()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize Essential Diagram
			Syncfusion.Windows.Forms.Diagram.Global.Initialize();

			this.pageSettings = new System.Drawing.Printing.PageSettings();

			// Create the model, view, and controller objects and initialize
			this.model = CreateModel();
			this.view = CreateView();
			this.controller = CreateController();
			this.MVCInit();

			////////////////////////////////////////////////////////////////
			// This prevents the base ScrollControl class from actually
			// changing the window origin using ScrollWindowEx() when
			// ScrollControl.ScrollWindow() is called.
			this.DisableScrollWindow = true;
			////////////////////////////////////////////////////////////////

			// Set scroll bar sizes based on the virtual size of the view
			UpdateScrollRange();

			// Enable events
			this.model.EventsEnabled = true;
			this.controller.EventsEnabled = true;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (this.model != null)
				{
					this.model.Dispose();
					this.model = null;
				}

				if (this.view != null)
				{
					this.view.Dispose();
					this.view = null;
				}

				if (this.controller != null)
				{
					this.controller.Dispose();
					this.controller = null;
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// Diagram
			// 
			this.AllowDrop = true;
			this.HScroll = true;
			this.Size = new System.Drawing.Size(464, 440);
			this.VScroll = true;

		}
		#endregion

		#region Public Properties

		/// <summary>
		/// The model contains the hierarchy of graphical nodes that are rendered
		/// onto the view and manipulated by the controller.
		/// </summary>
		/// <remarks>
		/// The model contains the data portion of a diagram. When a diagram is
		/// persisted, it is the Model that is serialized. The model is created
		/// by calling the virtual method
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.CreateModel"/>.
		/// The CreateModel method can be overridden in derived classes in order to
		/// plug custom models into the diagram.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Category("MVC"),
		TypeConverter(typeof(ModelConverter))
		]
		public Model Model
		{
			get
			{
				return this.model;
			}
		}

		/// <summary>
		/// The view is responsible for rendering the model onto a window.
		/// </summary>
		/// <remarks>
		/// A view is set inside of a window and has bounds that are measured
		/// in device coordinates. The view renders itself onto a
		/// System.Drawing.Graphics object. The view is created by calling the
		/// virtual method
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.CreateView"/>.
		/// The CreateView method can be overridden in derived classes in order to
		/// plug custom views into the diagram.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Category("MVC"),
		TypeConverter(typeof(ViewConverter))
		]
		public View View
		{
			get
			{
				return this.view;
			}
		}

		/// <summary>
		/// The controller processes input and translates it into commands and actions
		/// on the model and view.
		/// </summary>
		/// <remarks>
		/// The controller defines the user interface. It is created by calling the
		/// virtual method
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.CreateController"/>.
		/// The CreateController method can be overridden in derived classes in order to
		/// plug custom controllers into the diagram.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Category("MVC"),
		TypeConverter(typeof(ControllerConverter)),
		Description("Processes input and translates it into commands and actions. Determines interactive behavior of the diagram.")
		]
		public Controller Controller
		{
			get
			{
				return this.controller;
			}
		}

		/// <summary>
		/// Layout manager responsible for updating the layout of the diagram.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Layout"),
		Description("Layout manager object responsible for layout of nodes in the diagram")
		]
		public Syncfusion.Windows.Forms.Diagram.LayoutManager LayoutManager
		{
			get
			{
				return this.layoutManager;
			}
			set
			{
				if (this.layoutManager != value)
				{
					if (this.layoutManager != null)
					{
						this.layoutManager.Model = null;
					}

					this.layoutManager = value;

					if (this.layoutManager != null)
					{
						this.layoutManager.Model = this.Model;
					}
				}
			}
		}

		/// <summary>
		/// Determines the level of granularity for scrolling.
		/// </summary>
		/// <remarks>
		/// This value is to scale the scroll range of the scroll bars. The value
		/// of this property must be greater than 0. This value is multiplied by
		/// virtual size of the view in order to get the scroll range. For example,
		/// if the virtual size of the view is 100x50 and this property is set to
		/// 0.5f, then the horizontal scroll range is set to 0..50 and the vertical
		/// scroll range is set to 0..25.
		/// </remarks>
		public float ScrollGranularity
		{
			get
			{
				return this.scrollGranularity;
			}
			set
			{
				if (value > 0.0f)
				{
					this.scrollGranularity = value;
				}
				else
				{
					throw new EInvalidParameter();
				}
			}
		}

		/// <summary>
		/// Number of logical units to move nodes during a nudge operation.
		/// </summary>
		public float NudgeIncrement
		{
			get
			{
				return this.nudgeIncrement;
			}
			set
			{
				this.nudgeIncrement = value;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Set the ContextMenu to null.
		/// </summary>
		public void DetachContextMenu()
		{
			this.ContextMenu = null;
		}

		/// <summary>
		/// This method creates the model that is attached to the diagram.
		/// </summary>
		/// <returns>A new model object</returns>
		/// <remarks>
		/// This method can be overidden in derived classes in order to perform
		/// custom initialization of the model or to create custom models derived
		/// from the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model"/> class.
		/// </remarks>
		public virtual Model CreateModel()
		{
			return new Model();
		}

		/// <summary>
		/// Attach an existing model to the diagram.
		/// </summary>
		/// <param name="value">Model object to attach</param>
		/// <remarks>
		/// This method can be used to attach a new model object to the diagram.
		/// NOTE: Using this method will cause the contents of the previous model
		/// to be destroyed. Calling this method after your form's InitializeComponent
		/// method is called will cause design-time property values to be lost.
		/// </remarks>
		public void AttachModel(Syncfusion.Windows.Forms.Diagram.Model value)
		{
			if (this.model != value)
			{
				if (this.model != null)
				{
					this.model.BoundsChanged -= new BoundsEventHandler(OnModelBoundsChange);
				}

				this.model = value;

				if (this.model != null)
				{
					this.model.PageSettings = this.pageSettings;
					this.model.BoundsChanged += new BoundsEventHandler(OnModelBoundsChange);
					this.model.EventsEnabled = true;
				}

				if (this.view != null)
				{
					this.view.Model = this.model;
				}
			}
		}

		/// <summary>
		/// This method creates the view that is attached to the diagram.
		/// </summary>
		/// <returns>View object to attach</returns>
		/// <remarks>
		/// This method can be overidden in derived classes in order to perform
		/// custom initialization of the view or to create custom views derived
		/// from the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View"/> class.
		/// </remarks>
		public virtual View CreateView()
		{
			return new View();
		}

		/// <summary>
		/// This method creates the controller that is attached to the diagram.
		/// </summary>
		/// <returns>Controller object to attach</returns>
		/// <remarks>
		/// This method can be overidden in derived classes in order to perform
		/// custom initialization of the controller or to create custom controllers
		/// derived from the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller"/> class.
		/// </remarks>
		public virtual Controller CreateController()
		{
			Controller ctlr = new DiagramController();
			return ctlr;
		}

		/// <summary>
		/// Saves the diagram to a stream in SOAP format.
		/// </summary>
		/// <param name="strmOut">Stream to serialize the diagram into</param>
		public virtual void SaveSoap(Stream strmOut)
		{
			SoapFormatter formatter = new SoapFormatter();
			formatter.Serialize(strmOut, this.Model);
			formatter.Serialize(strmOut, this.View);
		}

		/// <summary>
		/// Saves the diagram to a file in SOAP format.
		/// </summary>
		/// <param name="fileName">Name of file to save to</param>
		public virtual void SaveSoap(string fileName)
		{
			FileStream oStream = new FileStream(fileName, FileMode.Create);
			this.SaveSoap(oStream);
			oStream.Close();
		}

		/// <summary>
		/// Loads the diagram from a stream in SOAP format.
		/// </summary>
		/// <param name="strmIn">Stream to serialize the diagram into</param>
		public virtual void LoadSoap(Stream strmIn)
		{
			this.MVCDispose();
			SoapFormatter formatter = new SoapFormatter();
			try
			{
				System.AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
				this.model = (Syncfusion.Windows.Forms.Diagram.Model) formatter.Deserialize(strmIn);
				this.view = (Syncfusion.Windows.Forms.Diagram.View) formatter.Deserialize(strmIn);
			}
			finally
			{
				System.AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
			}
			this.controller = this.CreateController();
			this.MVCInit();
		}

		/// <summary>
		/// Loads the diagram from a file in SOAP format.
		/// </summary>
		/// <param name="fileName">Name of file to load from</param>
		public virtual void LoadSoap(string fileName)
		{
			FileStream iStream = new FileStream(fileName, FileMode.Open);
			this.LoadSoap(iStream);
			iStream.Close();
		}

		/// <summary>
		/// Saves the diagram to a stream in binary format.
		/// </summary>
		/// <param name="strmOut">Stream to serialize the diagram into</param>
		public virtual void SaveBinary(Stream strmOut)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(strmOut, this.Model);
			formatter.Serialize(strmOut, this.View);
		}

		/// <summary>
		/// Saves the diagram to a file in binary format.
		/// </summary>
		/// <param name="fileName">Name of file to save to</param>
		public virtual void SaveBinary(string fileName)
		{
			FileStream oStream = new FileStream(fileName, FileMode.Create);
			this.SaveBinary(oStream);
			oStream.Close();
		}

		/// <summary>
		/// Loads the diagram from a stream in binary format.
		/// </summary>
		/// <param name="strmIn">Stream to serialize the diagram from</param>
		public virtual void LoadBinary(Stream strmIn)
		{
			this.MVCDispose();
			BinaryFormatter formatter = new BinaryFormatter();
			try
			{
				System.AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
				this.model = (Syncfusion.Windows.Forms.Diagram.Model) formatter.Deserialize(strmIn);
				this.view = (Syncfusion.Windows.Forms.Diagram.View) formatter.Deserialize(strmIn);
			}
			finally
			{
				System.AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(DiagramAssembly.AssemblyResolver);
			}
			this.controller = this.CreateController();
			this.MVCInit();
		}

		/// <summary>
		/// Loads the diagram from a file in binary format.
		/// </summary>
		/// <param name="fileName">Name of file to load from</param>
		public virtual void LoadBinary(string fileName)
		{
			FileStream iStream = new FileStream(fileName, FileMode.Open);
			this.LoadBinary(iStream);
			iStream.Close();
		}

		#endregion

		#region Events

		/// <summary>
		/// Fires when the list of selected nodes changes.
		/// </summary>
		public event NodeCollection.EventHandler SelectionChanged;

		#endregion

		#region Implementation Methods

		/// <summary>
		/// Wires up the model, view, and controller and performs necessary initialization.
		/// </summary>
		/// <remarks>
		/// This method does not create the model, view, and controller objects. It assumes
		/// that the mode, view, and controller objects have already been created.
		/// This method hooks the model to the view, and hooks the view to the
		/// controller.
		/// </remarks>
		protected virtual void MVCInit()
		{
			if (this.model != null)
			{
				this.model.PageSettings = this.pageSettings;
				this.model.BoundsChanged += new BoundsEventHandler(OnModelBoundsChange);
				this.model.ChildrenChanging += new NodeCollection.EventHandler(OnModelChildrenChanging);
				this.model.ChildrenChangeComplete += new NodeCollection.EventHandler(OnModelChildrenChangeComplete);
				this.model.PropertyChanged += new PropertyEventHandler(OnModelPropertyChanged);
			}

			if (this.view != null)
			{
				this.view.Initialize(this);
				this.view.Location = new System.Drawing.Point(0,0);
				this.view.Size = this.Size;
				this.view.Model = this.model;
				this.view.OriginChanged += new ViewOriginEventHandler(OnViewOriginChanged);
			}

			if (this.controller != null)
			{
				this.controller.Initialize(this.view);

				// Wire up event handler to listen for changes in the selection list
				NodeCollection selectionList = this.controller.SelectionList;
				if (selectionList != null)
				{
					selectionList.ChangeComplete += new NodeCollection.EventHandler(OnSelectionListChanged);
				}

				this.controller.ToolActivate += new Controller.ToolEventHandler(this.OnToolActivate);
				this.controller.ToolDeactivate += new Controller.ToolEventHandler(this.OnToolDeactivate);
			}
		}

		/// <summary>
		/// Destroys the model, view, and controller and ensures that they are completely
		/// disconnected from the diagram control.
		/// </summary>
		protected virtual void MVCDispose()
		{
			if (this.model != null)
			{
				this.model.BoundsChanged -= new BoundsEventHandler(OnModelBoundsChange);
				this.model.ChildrenChanging -= new NodeCollection.EventHandler(OnModelChildrenChanging);
				this.model.ChildrenChangeComplete -= new NodeCollection.EventHandler(OnModelChildrenChangeComplete);
				this.model.PropertyChanged -= new PropertyEventHandler(OnModelPropertyChanged);
				this.model.Dispose();
				this.model = null;
			}

			if (this.controller != null)
			{
				NodeCollection selectionList = this.controller.SelectionList;
				if (selectionList != null)
				{
					selectionList.ChangeComplete -= new NodeCollection.EventHandler(OnSelectionListChanged);
				}
				this.controller.ToolActivate -= new Controller.ToolEventHandler(this.OnToolActivate);
				this.controller.ToolDeactivate -= new Controller.ToolEventHandler(this.OnToolDeactivate);
				this.controller.Dispose();
				this.controller = null;
			}

			if (this.view != null)
			{
				this.view.OriginChanged -= new ViewOriginEventHandler(OnViewOriginChanged);
				this.view.Dispose();
				this.view = null;
			}
		}

		/// <summary>
		/// This method sets the range on the horizontal and vertical scrollbars.
		/// </summary>
		/// <remarks>
		/// The scroll range for the scrollbars is determined by the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View.VirtualSize"/> property
		/// of the view and the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.ScrollGranularity"/>
		/// property.
		/// </remarks>
		protected virtual void UpdateScrollRange()
		{
			if (this.view != null)
			{
				System.Drawing.Size szScroll = this.view.VirtualSize;
				this.HScrollBar.Minimum = 0;
				this.HScrollBar.Maximum = (int) Math.Round((double) (szScroll.Width * this.scrollGranularity));
				this.VScrollBar.Minimum = 0;
				this.VScrollBar.Maximum = (int) Math.Round((double) (szScroll.Height * this.scrollGranularity));
			}
		}

		#endregion

		/// <summary>
		/// List of currently selected nodes.
		/// </summary>
		/// <remarks>
		/// Provides access to the controller's selection list.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public NodeCollection SelectionList
		{
			get
			{
				NodeCollection selectionList = null;

				if (this.controller != null)
				{
					selectionList = this.controller.SelectionList;
				}

				return selectionList;
			}
		}

		/// <summary>
		/// Remove the currently selected nodes from the diagram and move
		/// them to the clipboard.
		/// </summary>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.Cut"/>
		/// </remarks>
		public void Cut()
		{
			if (this.controller != null)
			{
				this.controller.Cut();
			}
		}

		/// <summary>
		/// Indicates if there are any selected nodes that can be removed from the
		/// the model.
		/// </summary>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.CanCut"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool CanCut
		{
			get
			{
				if (this.controller != null)
				{
					return this.controller.CanCut;
				}
				return false;
			}
		}

		/// <summary>
		/// Copy the currently selected nodes to the clipboard.
		/// </summary>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.Copy"/>
		/// </remarks>
		public void Copy()
		{
			if (this.controller != null)
			{
				this.controller.Copy();
			}
		}

		/// <summary>
		/// Indicates if there are any selected nodes that can be copied to the clipboard.
		/// </summary>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.CanCopy"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool CanCopy
		{
			get
			{
				if (this.controller != null)
				{
					return this.controller.CanCopy;
				}
				return false;
			}
		}

		/// <summary>
		/// Paste the contents of the clipboard to the diagram.
		/// </summary>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.Paste"/>
		/// </remarks>
		public void Paste()
		{
			if (this.controller != null)
			{
				this.controller.Paste();
			}
		}

		/// <summary>
		/// Indicates if there is any data in the clipboard that can be pasted
		/// into the model.
		/// </summary>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.CanPaste"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool CanPaste
		{
			get
			{
				if (this.controller != null)
				{
					return this.controller.CanPaste;
				}
				return false;
			}
		}

		/// <summary>
		/// Indicates if there are any commands to undo.
		/// </summary>
		/// <remarks>
		/// Checks the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.UndoCount"/>
		/// to see if it is greater than 0.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool CanUndo
		{
			get
			{
				bool value = false;

				if (this.controller != null)
				{
					if (this.controller.UndoCount > 0)
					{
						value = true;
					}
				}

				return value;
			}
		}

		/// <summary>
		/// Indicates if there are any commands to redo.
		/// </summary>
		/// <remarks>
		/// Checks the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.RedoCount"/>
		/// to see if it is greater than 0.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool CanRedo
		{
			get
			{
				bool value = false;

				if (this.controller != null)
				{
					if (this.controller.RedoCount > 0)
					{
						value = true;
					}
				}

				return value;
			}
		}

		/// <summary>
		/// Adds all nodes in the model to the SelectionList
		/// </summary>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectAll"/>
		/// </remarks>
		public void SelectAll()
		{
			if (this.controller != null)
			{
				this.controller.SelectAll();
			}
		}

		/// <summary>
		/// Deletes the selected nodes from the diagram.
		/// </summary>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.Delete"/>
		/// </remarks>
		public void Delete()
		{
			if (this.controller != null)
			{
				this.controller.Delete();
			}
		}

		/// <summary>
		/// Sets the X and Y magnification (zoom) values on a scale of 1 to n.
		/// </summary>
		/// <param name="magX">Magnification percent along X axis</param>
		/// <param name="magY">Magnification percent along Y axis</param>
		/// <remarks>
		/// This method sets the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Magnification"/>
		/// property.
		/// </remarks>
		public void SetMagnification(int magX, int magY)
		{
			if (this.view != null)
			{
				this.view.Magnification = new Size(magX, magY);
				this.Invalidate();
			}
		}

		/// <summary>
		/// Activates the specified tool in the controller.
		/// </summary>
		/// <param name="toolName">Name of tool to activate</param>
		/// <returns>true if tool activated, otherwise false</returns>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.ActivateTool"/>.
		/// </remarks>
		public bool ActivateTool(string toolName)
		{
			if (this.controller == null)
			{
				throw new ArgumentException();
			}
			return this.controller.ActivateTool(toolName);
		}

		/// <summary>
		/// Executes the given command through the controller.
		/// </summary>
		/// <param name="cmd">Command to execute</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.ExecuteCommand"/>.
		/// </remarks>
		public bool ExecuteCommand(ICommand cmd)
		{
			if (this.controller == null)
			{
				throw new ArgumentException();
			}
			return this.controller.ExecuteCommand(cmd);
		}

		/// <summary>
		/// Execute the command on the top of the undo stack and remove it
		/// from the stack.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.UndoCommand"/>
		/// </remarks>
		public bool UndoCommand()
		{
			if (this.controller == null)
			{
				throw new ArgumentException();
			}
			return this.controller.UndoCommand();
		}

		/// <summary>
		/// Execute the command on the top of the redo stack and remove it
		/// from the stack.
		/// </summary>
		/// <returns>tue if successful, otherwise false</returns>
		/// <remarks>
		/// Wrapper for
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.RedoCommand"/>
		/// </remarks>
		public bool RedoCommand()
		{
			if (this.controller == null)
			{
				throw new ArgumentException();
			}
			return this.controller.RedoCommand();
		}

		/// <summary>
		/// Moves the specified nodes by a X and Y offset.
		/// </summary>
		/// <param name="nodes">Nodes to be moved</param>
		/// <param name="dx">Distance to move along the X axis</param>
		/// <param name="dy">Distance to move along the Y axis</param>
		/// <returns>true if successful, otherwise false</returns>
		public bool MoveNodes(NodeCollection nodes, float dx, float dy)
		{
			bool success = false;

			if (this.controller != null && nodes != null)
			{
				MoveCmd moveCmd = new MoveCmd(dx, dy);
				moveCmd.Nodes.Concat(nodes);
				success = this.controller.ExecuteCommand(moveCmd);
			}

			return success;
		}

		/// <summary>
		/// Nudge the selected components up by
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.NudgeIncrement"/>
		/// units.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public bool NudgeUp()
		{
			return this.MoveNodes(this.SelectionList, 0.0f, -this.nudgeIncrement);
		}

		/// <summary>
		/// Nudge the selected components down by
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.NudgeIncrement"/>
		/// units.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public bool NudgeDown()
		{
			return this.MoveNodes(this.SelectionList, 0.0f, this.nudgeIncrement);
		}

		/// <summary>
		/// Nudge the selected components left by
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.NudgeIncrement"/>
		/// units.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public bool NudgeLeft()
		{
			return this.MoveNodes(this.SelectionList, -this.nudgeIncrement, 0.0f);
		}

		/// <summary>
		/// Nudge the selected components right by
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.NudgeIncrement"/>
		/// units.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public bool NudgeRight()
		{
			return this.MoveNodes(this.SelectionList, this.nudgeIncrement, 0.0f);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="degrees"></param>
		/// <returns></returns>
		public bool Rotate(float degrees)
		{
			RotateCmd cmd = new RotateCmd();
			cmd.Nodes.Concat(this.SelectionList);
			cmd.Degrees = degrees;
			return cmd.Do(this.Model);
		}

		/// <summary>
		/// 
		/// </summary>
		public void FlipHorizontal()
		{
			ScaleCmd cmd = new ScaleCmd(1.0f, -1.0f);
			cmd.Nodes.Concat(this.SelectionList);
			cmd.Do(this.Model);
		}

		/// <summary>
		/// 
		/// </summary>
		public void FlipVertical()
		{
			ScaleCmd cmd = new ScaleCmd(-1.0f, 1.0f);
			cmd.Nodes.Concat(this.SelectionList);
			cmd.Do(this.Model);
		}

		/// <summary>
		/// 
		/// </summary>
		public void AlignLeft()
		{
			AlignCmd cmd = new AlignCmd(BoxOrientation.Left);
			cmd.Nodes.Concat(this.SelectionList);
			this.ExecuteCommand(cmd);
		}

		/// <summary>
		/// 
		/// </summary>
		public void AlignRight()
		{
			AlignCmd cmd = new AlignCmd(BoxOrientation.Right);
			cmd.Nodes.Concat(this.SelectionList);
			this.ExecuteCommand(cmd);
		}

		/// <summary>
		/// 
		/// </summary>
		public void AlignTop()
		{
			AlignCmd cmd = new AlignCmd(BoxOrientation.Top);
			cmd.Nodes.Concat(this.SelectionList);
			this.ExecuteCommand(cmd);
		}

		/// <summary>
		/// 
		/// </summary>
		public void AlignBottom()
		{
			AlignCmd cmd = new AlignCmd(BoxOrientation.Bottom);
			cmd.Nodes.Concat(this.SelectionList);
			this.ExecuteCommand(cmd);
		}

		/// <summary>
		/// 
		/// </summary>
		public void AlignMiddle()
		{
			AlignCmd cmd = new AlignCmd(BoxOrientation.Middle);
			cmd.Nodes.Concat(this.SelectionList);
			this.ExecuteCommand(cmd);
		}

		/// <summary>
		/// 
		/// </summary>
		public void AlignCenter()
		{
			AlignCmd cmd = new AlignCmd(BoxOrientation.Center);
			cmd.Nodes.Concat(this.SelectionList);
			this.ExecuteCommand(cmd);
		}

		/// <summary>
		/// 
		/// </summary>
		public void SpaceAcross()
		{
			SpacingCmd cmd = new SpacingCmd(SpacingDirection.Across);
			cmd.Nodes.Concat(this.SelectionList);
			this.ExecuteCommand(cmd);
		}

		/// <summary>
		/// 
		/// </summary>
		public void SpaceDown()
		{
			SpacingCmd cmd = new SpacingCmd(SpacingDirection.Down);
			cmd.Nodes.Concat(this.SelectionList);
			this.ExecuteCommand(cmd);
		}

		/// <summary>
		/// Displays a file open dialog that allows the user to pick an image to
		/// insert into the diagram.
		/// </summary>
		/// <remarks>
		/// This method activates either the BitmapTool or the MetafileTool,
		/// depending on the type of image passed in.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BitmapTool"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MetafileTool"/>
		/// </remarks>
		public void InsertImage()
		{
			System.Windows.Forms.OpenFileDialog openImageDlg = new System.Windows.Forms.OpenFileDialog();
			openImageDlg.Filter = "Windows Bitmaps (*.bmp)|*.bmp|JPEG files (*.jpg)|*.jpg|Graphics Interchange Format files (*.gif)|*.gif|Portable Network Graphics files (*.png)|*.png| Enhanced Metafiles (*.emf)|*.emf|All files (*.*)|*.*";
			openImageDlg.DefaultExt = "*.bmp;*.jpg;*.gif;*.png;*.emf";
			openImageDlg.Title = "Select an image file";
			if (openImageDlg.ShowDialog(this) == DialogResult.OK)
			{
				System.IO.Stream fileStrm;
				fileStrm = openImageDlg.OpenFile();
				if (fileStrm != null)
				{
					this.InsertImage(fileStrm);
					fileStrm.Close();
				}
			}
		}

		/// <summary>
		/// Inserts the given image onto the diagram.
		/// </summary>
		/// <param name="fileStrm">Binary stream containing the image</param>
		/// <remarks>
		/// This method activates either the BitmapTool or the MetafileTool,
		/// depending on the type of image passed in.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BitmapTool"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MetafileTool"/>
		/// </remarks>
		public void InsertImage(System.IO.Stream fileStrm)
		{
			if (fileStrm != null)
			{
				System.Drawing.Imaging.Metafile metafile = null;
				System.Drawing.Bitmap bitmap = null;

				try
				{
					bitmap = new System.Drawing.Bitmap(fileStrm);
				}
				catch
				{
					bitmap = null;
				}

				if (bitmap == null)
				{
					try
					{
						metafile = new System.Drawing.Imaging.Metafile(fileStrm);
					}
					catch
					{
						metafile = null;
					}
				}

				if (bitmap != null)
				{
					Clipboard.SetDataObject(bitmap, true);
					this.Controller.ActivateTool("BitmapTool");
				}
				else if (metafile != null)
				{
					Clipboard.SetDataObject(metafile, true);
					this.Controller.ActivateTool("MetafileTool");
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public System.Drawing.Printing.PageSettings PageSettings
		{
			get
			{
				return this.pageSettings;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual DiagramPrintDocument CreatePrintDocument()
		{
			DiagramPrintDocument printDoc = new DiagramPrintDocument(this.Model);

			printDoc.DefaultPageSettings = this.Model.PageSettings;

			return printDoc;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="evtArgs"></param>
		protected virtual void OnModelBoundsChange(object sender, BoundsEventArgs evtArgs)
		{
			this.UpdateScrollRange();
			this.UpdateScrollBars();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="evtArgs"></param>
		protected virtual void OnModelChildrenChanging(object sender, NodeCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="evtArgs"></param>
		protected virtual void OnModelChildrenChangeComplete(object sender, NodeCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="evtArgs"></param>
		protected virtual void OnModelPropertyChanged(object sender, PropertyEventArgs evtArgs)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="evtArgs"></param>
		protected virtual void OnViewOriginChanged(object sender, ViewOriginEventArgs evtArgs)
		{
			System.Drawing.Point newOrigin = new System.Drawing.Point(0,0);

			newOrigin.X = (int) System.Math.Round((double) (evtArgs.NewOrigin.X * this.scrollGranularity));
			newOrigin.Y = (int) System.Math.Round((double) (evtArgs.NewOrigin.Y * this.scrollGranularity));

			if (newOrigin.X >= this.HScrollBar.Minimum && newOrigin.X <= this.HScrollBar.Maximum)
			{
				this.HScrollBar.Value = newOrigin.X;
			}
			else if (newOrigin.X < this.HScrollBar.Minimum)
			{
				this.HScrollBar.Value = this.HScrollBar.Minimum;
			}
			else if (newOrigin.X > this.HScrollBar.Maximum)
			{
				this.HScrollBar.Value = this.HScrollBar.Maximum;
			}

			if (newOrigin.Y >= this.VScrollBar.Minimum && newOrigin.Y <= this.VScrollBar.Maximum)
			{
				this.VScrollBar.Value = newOrigin.Y;
			}
			else if (newOrigin.Y < this.VScrollBar.Minimum)
			{
				this.VScrollBar.Value = this.VScrollBar.Minimum;
			}
			else if (newOrigin.Y > this.VScrollBar.Maximum)
			{
				this.VScrollBar.Value = this.VScrollBar.Maximum;
			}

			this.UpdateScrollBars();
		}

		/// <summary>
		/// Called when the list of selected nodes changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="evtArgs"></param>
		protected virtual void OnSelectionListChanged(object sender, NodeCollection.EventArgs evtArgs)
		{
			NodeCollection selectionList = sender as NodeCollection;

			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(this, evtArgs);
			}
		}

		/// <summary>
		/// Called when the control needs to paint the window.
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// This method draws the view onto the System.Drawing.Graphics
		/// object provided by the System.Windows.Forms.PaintEventArgs
		/// parameter. The view is drawn by calling the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Draw"/>
		/// method.
		/// </remarks>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			this.BeginUpdate(BeginUpdateOptions.None);

			Graphics grfx = e.Graphics;

			this.FixRenderOrigin(grfx);

			if (this.view != null)
			{
				this.view.Draw(grfx);
			}

			base.OnPaint(e);

			this.EndUpdate(true);
		}

		/// <summary>
		/// Called when the control needs to clear the background.
		/// </summary>
		/// <param name="pevent">Event arguments</param>
		/// <remarks>
		/// This method does nothing. It is overridden to prevent the
		/// control from painting the background in order to eliminate
		/// flicker.
		/// </remarks>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Do not erase background in order to eliminate flicker.
		}

		/// <summary>
		/// Called when the control is resized.
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// Updates the size of the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.View"/>
		/// to match the new size of the control.
		/// </remarks>
		protected override void OnResize(System.EventArgs e)
		{
			if (this.view != null)
			{
				this.view.Size = this.Size;
			}
			base.OnResize(e);
		}

		/// <summary>
		/// Called when the horizontal scrollbar is moved.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="se">Event arguments</param>
		/// <remarks>
		/// Scrolls the window origin by the specified amount.
		/// </remarks>
		protected override void OnHScroll(object sender, ScrollEventArgs se)  
		{
			int xAmount = (int) Math.Round((double) ((float) (this.HScrollBar.Value - se.NewValue)) / this.scrollGranularity);
			System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(0, 0, this.Size.Width, this.Size.Height);
			this.ScrollWindow(xAmount, 0, bounds, bounds, false);
			base.OnHScroll(sender, se);
		}

		/// <summary>
		/// Called when the value of the horizontal scroll bar changes.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// Updates the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Origin"/>
		/// property of the view.
		/// </remarks>
		protected override void OnHScrollBarValueChanged(object sender, EventArgs e)  
		{
			if (this.view != null)
			{
				this.view.Origin = new PointF(((float)this.HScrollBar.Value / this.scrollGranularity), this.view.Origin.Y);
			}
		}

		/// <summary>
		/// Called when the vertical scrollbar is moved.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="se">Event arguments</param>
		/// <remarks>
		/// Scrolls the window origin by the specified amount.
		/// </remarks>
		protected override void OnVScroll(object sender, ScrollEventArgs se)  
		{
			int yAmount = (int) Math.Round((double) ((float) (this.VScrollBar.Value - se.NewValue)) / this.scrollGranularity);
			System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(0, 0, this.Size.Width, this.Size.Height);
			this.ScrollWindow(0, yAmount, bounds, bounds, false);
			base.OnVScroll(sender, se);
		}

		/// <summary>
		/// Called when the value of the vertical scroll bar changes.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// Updates the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Origin"/>
		/// property of the view.
		/// </remarks>
		protected override void OnVScrollBarValueChanged(object sender, EventArgs e)  
		{
			if (this.view != null)
			{
				this.view.Origin = new PointF(this.view.Origin.X, ((float) this.VScrollBar.Value / this.scrollGranularity));
			}
		}

		/// <summary>
		/// Called when a tool is activated by the controller.
		/// </summary>
		/// <param name="sender">Controller activating the tool</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// If there is a context menu associated with the diagram, this method
		/// detaches it. The reason is that the context menu can interfere with
		/// the behavior of some tools. When the tool is deactivated, the context
		/// menu is restored.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.OnToolDeactivate"/>
		/// </remarks>
		protected virtual void OnToolActivate(object sender, Controller.ToolEventArgs evtArgs)
		{
			if (this.ContextMenu != null)
			{
				this.contextMenuSave = this.ContextMenu;
				this.DetachContextMenu();
			}
		}

		/// <summary>
		/// Called when a tool is deactivated by the controller.
		/// </summary>
		/// <param name="sender">Controller deactivating the tool</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// If the context menu was disabled when the tool was activated, this
		/// method restores it.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.OnToolActivate"/>
		/// </remarks>
		protected virtual void OnToolDeactivate(object sender, Controller.ToolEventArgs evtArgs)
		{
			if (this.contextMenuSave != null)
			{
				this.ContextMenu = this.contextMenuSave;
				this.contextMenuSave = null;
			}
		}

		/// <summary>
		/// Called when the mouse enters the diagram during a drag operation.
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// Looks to see if a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.NodeCollection"/> is
		/// available in the System.Windows.Forms.IDataObject provided in the
		/// event arguments. If so, the aggregate bounds of the nodes is
		/// calculated and a tracking rectangle is created to track the nodes
		/// as they are dragged across the diagram.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.OnDragDrop"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.OnDragLeave"/>
		/// </remarks>
		protected override void OnDragEnter(System.Windows.Forms.DragEventArgs e)
		{
			IDataObject dataObj = e.Data;
			if (dataObj.GetDataPresent(typeof(NodeCollection)))
			{
				NodeCollection nodes = (NodeCollection) dataObj.GetData(typeof(NodeCollection));

				if (nodes != null && nodes.Count > 0)
				{
					RectangleF rcBounds = Geometry.GetAggregateBounds(nodes);
					SizeF szNodeWorld = rcBounds.Size;
					SizeF szNodeView = this.view.WorldToView(szNodeWorld);
					System.Drawing.Size szNodeScreen = this.view.ViewToDevice(szNodeView);

					e.Effect = DragDropEffects.Copy;

					POINT ptClient = new POINT(e.X, e.Y);
					Window.ScreenToClient(this.Handle, ref ptClient);
					ptClient.X = ptClient.X - (szNodeScreen.Width / 2);
					ptClient.Y = ptClient.Y - (szNodeScreen.Height / 2);
					StartDragRect(ptClient.X - (szNodeScreen.Width / 2),
					              ptClient.Y - (szNodeScreen.Height / 2),
					              szNodeScreen.Width, szNodeScreen.Height);
				}
			}
		}

		/// <summary>
		/// Called when objects are dropped onto the diagram.
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// Looks to see if a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.NodeCollection"/> is
		/// available in the System.Windows.Forms.IDataObject provided in the
		/// event arguments. If so, the NodeCollection is retrieved and
		/// added to the diagram using a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
		/// command.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.OnDragEnter"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.OnDragLeave"/>
		/// </remarks>
		protected override void OnDragDrop(System.Windows.Forms.DragEventArgs e)
		{
			EraseDragRect();

			IDataObject dataObj = e.Data;

			POINT ptClient = new POINT(e.X, e.Y);
			Window.ScreenToClient(this.Handle, ref ptClient);

			System.Drawing.Point ptDev = new System.Drawing.Point(ptClient.X, ptClient.Y);
			ptDev.X = ptDev.X;
			ptDev.Y = ptDev.Y;
			ptDev.X = ptDev.X - (this.dragRect.Width / 2);
			ptDev.Y = ptDev.Y - (this.dragRect.Height / 2);
			PointF worldPt = this.view.ViewToWorld(this.view.DeviceToView(ptDev));

			if (dataObj.GetDataPresent(typeof(NodeCollection)))
			{
				NodeCollection nodes = (NodeCollection) dataObj.GetData(typeof(NodeCollection));
				InsertNodesCmd insCmd = new InsertNodesCmd();
				insCmd.Nodes.Concat(nodes);
				insCmd.Location = worldPt;
				this.controller.ExecuteCommand(insCmd);
			}
		}

		/// <summary>
		/// Called when mouse leaves the diagram during a drag operation.
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// Clears the tracking rectangle.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.OnDragEnter"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram.OnDragDrop"/>
		/// </remarks>
		protected override void OnDragLeave(System.EventArgs e)
		{
			EraseDragRect();
		}

		/// <summary>
		/// Called when objects are dragged over the control.
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// Updates tracking when objects are dragged over the diagram.
		/// </remarks>
		protected override void OnDragOver(System.Windows.Forms.DragEventArgs e)
		{
			POINT ptClient = new POINT(e.X, e.Y);
			Window.ScreenToClient(this.Handle, ref ptClient);
			UpdateDragRect(ptClient.X, ptClient.Y);
		}

		private void StartDragRect(int x, int y, int width, int height)
		{
			this.dragRect.X = x;
			this.dragRect.Y = y;
			this.dragRect.Width = width;
			this.dragRect.Height = height;
			if (this.view != null)
			{
				this.view.DrawTrackingRect(this.dragRect);
			}
		}

		private void UpdateDragRect(int x, int y)
		{
			if (this.view != null)
			{
				this.view.DrawTrackingRect(this.dragRect);
			}
			this.dragRect.X = x - (this.dragRect.Width / 2);
			this.dragRect.Y = y - (this.dragRect.Height / 2);
			if (this.view != null)
			{
				this.view.DrawTrackingRect(this.dragRect);
			}
		}

		private void EraseDragRect()
		{
			if (this.view != null)
			{
				this.view.DrawTrackingRect(this.dragRect);
			}
		}
	}
}