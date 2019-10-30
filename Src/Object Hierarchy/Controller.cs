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
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Diagram.Interactivity;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Processes input events and translates them into actions on the diagram.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The controller is an object that is responsible for handling input in the
	/// model-view-controller architecture. The controller receives input events
	/// and translates them into commands that affect the model and view.
	/// </para>
	/// <para>
	/// The features of a controller are implemented as distinct objects called
	/// tools. A tool is an object that implements a slice of functionality for
	/// the controller. Tools can be added to the controller using the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.RegisterTool"/>
	/// method in order to customize the behavior of the controller.
	/// </para>
	/// <para>
	/// This class is an abstract base class from which concrete controller
	/// classes are derived. This class does not does not register any tools.
	/// Derived controller classes must override the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.Initialize"/>
	/// method in order to register the tools needed by that specified type
	/// of controller.
	/// </para>
	/// <para>
	/// The controller is responsible for coordinating the activation and
	/// deactivation of tools. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.ActivateTool"/>
	/// method activates a specified tool. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.DeactivateTool"/>
	/// method deactivates a specified tool. Tools can be accessed by name with the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.GetTool"/>
	/// method.
	/// </para>
	/// <para>
	/// The controller is responsible for coordinating the executing of commands.
	/// A command is typically executed by a tool when it reaches a certain
	/// state during its processing of input events. For example, the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.RectangleTool"/>
	/// executes an
	/// <see cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// command when it receives a mouse up event. The controller implements the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.ICommandDispatcher"/>
	/// interface, which defines the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.ExecuteCommand"/>
	/// method used to execute commands. The ICommandDispatcher interface also
	/// defines methods to undo commands, redo commands, and manage the undo
	/// and redo stacks.
	/// </para>
	/// <para>
	/// The controller captures all mouse events and performs hit testing on
	/// the diagram. Nodes, ports, and vertices hit by mouse movements are
	/// tracked by the controller. Tools can access this hit testing
	/// information in the controller. In other words, the controller provides
	/// basic hit testing services to tools so that each tool doesn't need to
	/// implement (and possible duplicate) that hit testing logic. The following
	/// properties and methods provide access to the hit testing state information:
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.NodesHit"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.GetSelectedNodesHit"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.ResizeHandleHitNode"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.ResizeHandleHit"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.VertexHandleHitNode"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.VertexHit"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.HandleHitType"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.HandleHitType"/>.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICommandDispatcher"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.DiagramController"/>
	/// </remarks>
	public abstract class Controller : Component, ICommandDispatcher, IMouseController, IServiceProvider
	{
		#region Constructors

		/// <summary>
		/// Constructs a Controller object
		/// </summary>
		public Controller()
		{
			this.view = null;
			this.tools = new ArrayList();
			this.toolTable = new Hashtable();
			this.selectionList = new NodeCollection();
			this.mouseEventReceivers = new ArrayList();
			this.clickEventReceivers = new ArrayList();
			this.keyboardEventReceivers = new ArrayList();
			this.maxHistory = 256;
			this.undoStack = new Stack(20);
			this.redoStack = new Stack(20);
			this.eventsEnabled = false;

			// Wire up event handlers to selection list
			this.selectionList.ChangeComplete += new NodeCollection.EventHandler(SelectionList_Changed);
		}

		/// <summary>
		/// Constructs a controller and attaches it to get specified view.
		/// </summary>
		/// <param name="view">View to attach to</param>
		public Controller(Syncfusion.Windows.Forms.Diagram.View view)
		{
			this.Initialize(view);
		}

		/// <summary>
		/// Called to release resources held by the controller.
		/// </summary>
		/// <param name="disposing">
		/// Indicates if this method is being called explicitly by a call to Dispose()
		/// or by the destructor through the garbage collector.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
				// If disposing equals true, dispose all managed 
				// and unmanaged resources.
				if (disposing)
				{
					this.DetachView();
				}
			}
			this.disposed = true;

			base.Dispose(disposing);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// The <see cref="Syncfusion.Windows.Forms.Diagram.View"/> object
		/// attached to this controller.
		/// </summary>
		/// <remarks>
		/// The View object is attached by the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.Initialize"/>
		/// method.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Syncfusion.Windows.Forms.Diagram.View View
		{
			get
			{
				return this.view;
			}
		}

		/// <summary>
		/// The <see cref="Syncfusion.Windows.Forms.Diagram.Model"/> object
		/// attached to this controller.
		/// </summary>
		/// <remarks>
		/// The Model object is attached to the Controller indirectly through
		/// the View object. 
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Syncfusion.Windows.Forms.Diagram.Model Model
		{
			get
			{
				if (this.view != null)
				{
					return this.view.Model;
				}
				return null;
			}
		}

		/// <summary>
		/// System.Windows.Forms.Control that owns the view that this
		/// controller is attached to.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual Control ParentControl
		{
			get
			{
				if (this.view != null)
				{
					return this.view.ParentControl;
				}
				return null;
			}
		}

		/// <summary>
		/// Cursor to use inside the view.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual System.Windows.Forms.Cursor Cursor
		{
			get
			{
				if (this.view != null)
				{
					return this.view.Cursor;
				}
				return null;
			}
			set
			{
				if (this.view != null)
				{
					this.view.Cursor = value;
				}
			}
		}

		/// <summary>
		/// Last known location of the mouse pointer.
		/// </summary>
		/// <remarks>
		/// This property is updated each time the controller receives a mouse event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.NodesHit"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.GetSelectedNodesHit"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.ResizeHandleHitNode"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.ResizeHandleHit"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.VertexHandleHitNode"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.VertexHit"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.HandleHitType"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public System.Drawing.Point MouseLocation
		{
			get
			{
				return this.mouseLocation;
			}
		}

		/// <summary>
		/// Indicates if event will be fired or not.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool EventsEnabled
		{
			get
			{
				return this.eventsEnabled;
			}
			set
			{
				this.eventsEnabled = value;
			}
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Initializes the controller for the given view.
		/// </summary>
		/// <param name="view">The view object to attach to the controller</param>
		/// <remarks>
		/// This method wires the controller up to the view. The controller subscribes
		/// to numerous events in the view's parent control. The controller also attaches
		/// its selection list to the view.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
		/// </remarks>
		public virtual void Initialize(Syncfusion.Windows.Forms.Diagram.View view)
		{
			this.view = view;
			this.propertyContainer = null;

			if (this.view != null)
			{
				this.view.SelectionList = this.selectionList;

				IServiceProvider svcProviderView = this.view as IServiceProvider;
				if (svcProviderView != null)
				{
					this.propertyContainer = svcProviderView.GetService(typeof(IPropertyContainer)) as IPropertyContainer;
				}
			}

			Control parentControl = this.ParentControl;
			if (parentControl != null)
			{
				// Subscribe to mouse and click events in the parent window
				parentControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
				parentControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
				parentControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
				parentControl.Click += new System.EventHandler(this.OnClick);
				parentControl.DoubleClick += new System.EventHandler(this.OnDoubleClick);
				parentControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
				parentControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
				parentControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);

#if false
                Syncfusion.Windows.Forms.ScrollControl parentScrollCtl = parentControl as Syncfusion.Windows.Forms.ScrollControl;
				if (parentScrollCtl != null)
				{
					// Parent control is a ScrollControl. Add this controller to its
					// list of IMouseController's.
					parentScrollCtl.MouseControllerDispatcher.Add(this);
				}
#endif
			}
		}

		/// <summary>
		/// Detaches the controller from the view.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Disconnects event handlers and sets the controller's view to null.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.View"/>
		/// </remarks>
		public void DetachView()
		{
			if (this.propertyContainer != null)
			{
				this.propertyContainer = null;
			}

			// Disconnect event handlers from parent control
			System.Windows.Forms.Control parentControl = this.ParentControl;
			if (parentControl != null)
			{
#if false
                Syncfusion.Windows.Forms.ScrollControl parentScrollCtl = parentControl as Syncfusion.Windows.Forms.ScrollControl;
				if (parentScrollCtl != null)
				{
					// Parent control is a ScrollControl. Remove this controller from its
					// list of IMouseController's.
					parentScrollCtl.MouseControllerDispatcher.Remove(this);
				}
#endif

				parentControl.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
				parentControl.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
				parentControl.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
				parentControl.Click -= new System.EventHandler(this.OnClick);
				parentControl.DoubleClick -= new System.EventHandler(this.OnDoubleClick);
				parentControl.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
				parentControl.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
				parentControl.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
			}

			if (this.view != null)
			{
				this.view.SelectionList = null;
				this.view = null;
			}
		}

#endregion

#region Hit Testing

		/// <summary>
		/// List of nodes that were hit during the last received mouse event.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public NodeCollection NodesHit
		{
			get
			{
				return this.nodesHit;
			}
		}

		/// <summary>
		/// Returns the subset of nodes hit that are currently selected.
		/// </summary>
		/// <param name="nodes">Collection to add nodes into</param>
		/// <returns>The number of nodes added to the collection</returns>
		/// <remarks>
		/// This function iterates through the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.NodesHit"/>
		/// collection and returns the nodes that also in the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>
		/// collection.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>
		/// </remarks>
		public int GetSelectedNodesHit(NodeCollection nodes)
		{
			int numHit = 0;

			foreach (INode curNode in this.nodesHit)
			{
				if (this.selectionList.Contains(curNode))
				{
					nodes.Add(curNode);
					numHit++;
				}
			}

			return numHit;
		}

		/// <summary>
		/// Returns the number of nodes hit that are currently selected.
		/// </summary>
		/// <returns>
		/// A count of nodes hit during the last mouse event that are in the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>
		/// </returns>
		public int GetSelectedNodesHit()
		{
			int numHit = 0;

			foreach (INode curNode in this.nodesHit)
			{
				if (this.selectionList.Contains(curNode))
				{
					numHit++;
				}
			}

			return numHit;
		}

		/// <summary>
		/// Node that owns the resize handle hit during the last mouse event.
		/// </summary>
		/// <remarks>
		/// This property is updated during each mouse event. If a resize
		/// handle is hit during the mouse event, this property contains the
		/// node that owns the handle. If a resize handle was not hit during
		/// the mouse event this property is null.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.ResizeHandleHit"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public INode ResizeHandleHitNode
		{
			get
			{
				return this.resizeHandleHitNode;
			}
		}

		/// <summary>
		/// Position of the handle hit during the last mouse event.
		/// </summary>
		/// <remarks>
		/// This property is only valid if ResizeHandleHitNode is not null.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.ResizeHandleHitNode"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public BoxPosition ResizeHandleHit
		{
			get
			{
				return this.resizeHandleHit;
			}
		}

		/// <summary>
		/// Node that owns the vertex handle hit during the last mouse event.
		/// </summary>
		/// <remarks>
		/// If a vertex was not hit during the last mouse event, this property
		/// will be null.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.VertexHit"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public INode VertexHandleHitNode
		{
			get
			{
				return this.vertexHandleHitNode;
			}
		}

		/// <summary>
		/// Zero-based index position of the vertex hit during the last mouse
		/// event.
		/// </summary>
		/// <remarks>
		/// This property is valid only if the VertexHandleHitNode property is
		/// non-null.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.VertexHandleHitNode"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int VertexHit
		{
			get
			{
				return this.vertexHit;
			}
		}

		/// <summary>
		/// Indicates the type of handle hit during the last mouse event.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SelectHandleType"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public SelectHandleType HandleHitType
		{
			get
			{
				return this.HandleHitType;
			}
		}

		/// <summary>
		/// This method gets the resize handle and vertex handle at the current
		/// mouse location.
		/// </summary>
		/// <remarks>
		/// This method checks for both resize handles and vertex handles. The point
		/// tested is
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.MouseLocation"/>.
		/// </remarks>
		private void HitTestHandles()
		{
			this.resizeHandleHitNode = this.view.GetResizeHandleAtPoint(this.mouseLocation, ref this.resizeHandleHit);
			this.vertexHandleHitNode = this.view.GetVertexHandleAtPoint(this.mouseLocation, ref this.vertexHit);
		}

		/// <summary>
		/// The select handle mode determines the type of handles to draw and hit test.
		/// </summary>
		/// <remarks>
		/// This property determines if resize handles or vertex handles will be drawn
		/// and hit tested. The value is usually Resize, except during vertex editing when
		///  the value is set to Vertex.
		/// </remarks>
		public SelectHandleType SelectHandleMode
		{
			get
			{
				IPropertyContainer propContainer = ((IServiceProvider)this).GetService(typeof(IPropertyContainer)) as IPropertyContainer;
				if (propContainer != null)
				{
					object value = propContainer.GetPropertyValue("SelectHandleMode");
					if (value != null)
					{
						return (SelectHandleType) value;
					}
				}
				return SelectHandleType.Resize;
			}
			set
			{
				bool changeVal = true;
				IPropertyContainer propContainer = ((IServiceProvider)this).GetService(typeof(IPropertyContainer)) as IPropertyContainer;
				if (propContainer != null)
				{
					object curValue = propContainer.GetPropertyValue("SelectHandleMode");
					if (curValue != null)
					{
						if ((SelectHandleType) curValue == value)
						{
							changeVal = false;
						}
					}

					if (changeVal)
					{
						propContainer.SetPropertyValue("SelectHandleMode", value);
						if (this.view != null)
						{
							this.view.Draw();
						}
					}
				}
			}
		}

#endregion

#region Tool management

		/// <summary>
		/// Adds a new tool to the controller.
		/// </summary>
		/// <param name="tool">Tool object to register</param>
		/// <returns>true if successful, otherwise false</returns>
		public bool RegisterTool(Tool tool)
		{
			bool success = false;

			if (!this.toolTable.Contains(tool.Name))
			{
				tool.Controller = this;
				this.toolTable.Add(tool.Name, tool);
				this.tools.Add(tool);

				// If the Tool implements IMouseEventReceiver then add it
				// to the list of mouse event receivers.
				IMouseEventReceiver mouseRcvr = tool as IMouseEventReceiver;
				if (mouseRcvr != null)
				{
					this.mouseEventReceivers.Add(mouseRcvr);
				}

				// If the Tool implements IClickEventReceiver then add it
				// to the list of click event receivers.
				IClickEventReceiver clickRcvr = tool as IClickEventReceiver;
				if (clickRcvr != null)
				{
					this.clickEventReceivers.Add(clickRcvr);
				}

				// If the Tool implements IKeyboardEventReceiver then add it
				// to the list of keyboard event receivers.
				IKeyboardEventReceiver keyboardRcvr = tool as IKeyboardEventReceiver;
				if (keyboardRcvr != null)
				{
					this.keyboardEventReceivers.Add(keyboardRcvr);
				}

				success = true;
			}

			return success;
		}

		/// <summary>
		/// Returns the Tool object matching the given name.
		/// </summary>
		/// <param name="toolName">Name of Tool object to return</param>
		/// <returns>Tool object matching the given name, or null if not found</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
		/// </remarks>
		public Tool GetTool(string toolName)
		{
			if (this.toolTable.Contains(toolName))
			{
				return (Tool) this.toolTable[toolName];
			}
			return null;
		}

		/// <summary>
		/// Returns an array of all Tool objects registered with the Controller.
		/// </summary>
		/// <returns>Array of registered Tool objects</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
		/// </remarks>
		public Tool[] GetAllTools()
		{
			int numTools = this.toolTable.Keys.Count;
			Tool[] tools = new Tool[numTools];
			string[] toolNames = new string[numTools];
			this.toolTable.Keys.CopyTo(toolNames, 0);
			for (int toolIdx = 0; toolIdx < numTools; toolIdx++)
			{
				tools[toolIdx] = this.toolTable[toolNames[toolIdx]] as Tool;
			}
			return tools;
		}

		/// <summary>
		/// Activates the Tool object matching the given name.
		/// </summary>
		/// <param name="toolName">Name of Tool to activate</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// This method first locates the tool matching the given name and
		/// then passes it to the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.ActivateTool"/>
		/// method.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
		/// </remarks>
		public bool ActivateTool(string toolName)
		{
			return this.ActivateTool(this.GetTool(toolName));
		}

		/// <summary>
		/// Activates the given Tool object.
		/// </summary>
		/// <param name="tool">Tool to activate</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// This method first iterates through all of the other registered
		/// tools (excluding the one to be activated) and notifies each one
		/// of the activation by calling
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Tool.ToolActivating"/>.
		/// This gives the other tools a chance to suspend themselves or do
		/// any necessary cleanup before a new tool becomes active. Next, this
		/// method activates the Tool by calling its
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Tool.Activate"/>
		/// method. Finally, it fires the Controller's ToolActivate event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
		/// </remarks>
		public bool ActivateTool(Tool tool)
		{
			bool success = false;

			if (tool != null && tool.CanActivate)
			{
				// Notify all other tools of the activation
				IEnumerator enumTools = this.tools.GetEnumerator();
				Tool curTool = null;
				while (enumTools.MoveNext())
				{
					curTool = enumTools.Current as Tool;
					if (curTool != null && curTool != tool)
					{
						curTool.ToolActivating(tool);
					}
				}

				// Activate the tool
				success = tool.Activate();

				if (success)
				{
					// Fire ToolActivate event
					if (this.eventsEnabled && this.ToolActivate != null)
					{
						this.ToolActivate(this, new ToolEventArgs(tool));
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Deactivates the given Tool object.
		/// </summary>
		/// <param name="tool">The Tool to deactivate</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// First, this method iterates through all of the other tools
		/// (excluding the one to be deactivated) and notifies them of the
		/// deactivation by calling the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Tool.ToolDeactivating"/>
		/// method. Next, this method deactivates the Tool by calling its
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Tool.Deactivate"/>
		/// method. Finally, this method fires the Controller's ToolDeactivate
		/// event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
		/// </remarks>
		public bool DeactivateTool(Tool tool)
		{
			bool success = false;

			if (tool != null && tool.Enabled && tool.Active)
			{
				// Notify all other tools of the deactivation
				IEnumerator enumTools = this.tools.GetEnumerator();
				Tool curTool = null;
				while (enumTools.MoveNext())
				{
					curTool = enumTools.Current as Tool;
					if (curTool != null && curTool != tool)
					{
						curTool.ToolDeactivating(tool);
					}
				}

				// Deactivate the tool
				success = tool.Deactivate();

				if (success)
				{
					// Fire ToolDeactivate event 
					if (this.eventsEnabled && this.ToolDeactivate != null)
					{
						this.ToolDeactivate(this, new ToolEventArgs(tool));
					}
				}
			}

			return success;
		}

		/// <summary>
		/// This class encapsulates event arguments for events fired by the
		/// controller that are caused by
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Tool"/> objects.
		/// </summary>
		public class ToolEventArgs : EventArgs
		{
			/// <summary>
			/// Constructs a ToolEventArgs object given a tool object.
			/// </summary>
			/// <param name="tool"></param>
			public ToolEventArgs(Tool tool)
			{
				this.tool = tool;
			}

			/// <summary>
			/// Returns the tool object that generated the event.
			/// </summary>
			public Tool Tool
			{
				get
				{
					return this.tool;
				}
			}

			private Tool tool;
		}

		/// <summary>
		/// Delegate used for tool events.
		/// </summary>
		public delegate void ToolEventHandler(object sender, ToolEventArgs evtArgs);

#endregion

#region Event Handlers

		/// <summary>
		/// Called when a mouse down event occurs.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// This method updates the current mouse location and then performs hit testing
		/// for handles and nodes. Then it forwards the event onto all tools that
		/// implement the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
		/// interface.
		/// </remarks>
		protected virtual void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs evtArgs)
		{
			this.mouseLocation = new Point(evtArgs.X, evtArgs.Y);

			if (this.textEdit != null)
			{
				// Mouse down events cancel text editing
				EndEditText();
			}

			if (this.view != null)
			{
				// Check to see if a handle was hit
				this.HitTestHandles();

				// Check to see if one or more nodes was hit
				this.nodesHit.Clear();
				this.view.GetNodesAtPoint(this.nodesHit, this.mouseLocation);
			}

			// Send MouseDown event to all tools that implement the
			// IMouseEventReceiver interface.
			foreach (IMouseEventReceiver rcvr in this.mouseEventReceivers)
			{
				rcvr.MouseDown(evtArgs);
			}
		}

		/// <summary>
		/// Called when a mouse move event occurs.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// This method updates the current mouse location and then performs hit testing
		/// for handles and nodes. Then it forwards the event onto all tools that
		/// implement the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
		/// interface.
		/// </remarks>
		protected virtual void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs evtArgs)
		{
			IDispatchNodeEvents curDispatcher;

			this.mouseLocation = new Point(evtArgs.X, evtArgs.Y);

			if (this.view != null)
			{
				// Check to see if a handle was hit
				this.HitTestHandles();

				// Check to see if one or more nodes was hit
				this.nodesHit.Clear();
				this.view.GetNodesAtPoint(this.nodesHit, this.mouseLocation);
			}

			// Iterate through each node hit and check to see if it is
			// in the list of hover nodes. If a node isn't already in the hover
			// node collection, then add it and dispatch a MouseEnter event to
			// the node.
			foreach (INode curNode in this.nodesHit)
			{
				if (!this.hoverNodes.Contains(curNode))
				{
					curDispatcher = curNode as IDispatchNodeEvents;
					if (curDispatcher != null)
					{
						curDispatcher.MouseEnter(new NodeMouseEventArgs(curNode));
					}
				}
			}

			// Iterate through each node in the hoverNode collections and check
			// to see if it exists in the nodesHit collection. If not, then dispatch
			// a MouseLeave event to the node.
			foreach (INode curNode in this.hoverNodes)
			{
				if (!this.nodesHit.Contains(curNode))
				{
					curDispatcher = curNode as IDispatchNodeEvents;
					if (curDispatcher != null)
					{
						curDispatcher.MouseLeave(new NodeMouseEventArgs(curNode));
					}
				}
			}

			// Load the hoverNodes collection with the nodes hit on this
			// mouse move.
			this.hoverNodes.Clear();
			this.hoverNodes.Concat(this.nodesHit);

			// Send MouseMove event to all tools that implement the
			// IMouseEventReceiver interface.
			foreach (IMouseEventReceiver rcvr in this.mouseEventReceivers)
			{
				rcvr.MouseMove(evtArgs);
			}
		}

		/// <summary>
		/// Called when a mouse up event is received.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// This method updates the current mouse location and then performs hit testing
		/// for handles and nodes. Then it forwards the event onto all tools that
		/// implement the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
		/// interface.
		/// </remarks>
		protected virtual void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs evtArgs)
		{
			if (this.view != null)
			{
				// Check to see if a handle was hit
				this.HitTestHandles();

				// Check to see if one or more nodes was hit
				this.nodesHit.Clear();
				this.view.GetNodesAtPoint(this.nodesHit, this.mouseLocation);
			}

			foreach (IMouseEventReceiver rcvr in this.mouseEventReceivers)
			{
				rcvr.MouseUp(evtArgs);
			}
		}

		/// <summary>
		/// Called when a click event is received.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Iterates through the nodes hit by the click event and notifies them
		/// of the click by calling the 
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Click"/>
		/// method. Then it iterates through all tools that implement the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IClickEventReceiver"/>
		/// interface and forwards the event onto them.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IClickEventReceiver"/>
		/// </remarks>
		protected virtual void OnClick(object sender, System.EventArgs evtArgs)
		{
			if (this.view != null)
			{
				this.nodesHit.Clear();
				int numHit = this.view.GetNodesAtPoint(this.nodesHit, this.mouseLocation);

				if (numHit > 0)
				{
					foreach (INode curNode in this.nodesHit)
					{
						IDispatchNodeEvents curEvtDispatcher = curNode as IDispatchNodeEvents;
						if (curEvtDispatcher != null)
						{
							curEvtDispatcher.Click(new NodeMouseEventArgs(curNode));
						}
					}
				}
			}

			foreach (IClickEventReceiver rcvr in this.clickEventReceivers)
			{
				rcvr.Click();
			}
		}

		/// <summary>
		/// Called when a double click event is received.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Iterates through the nodes hit by the double click event and notifies
		/// them of the click by calling the 
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.DoubleClick"/>
		/// method. Then it iterates through all tools that implement the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IClickEventReceiver"/>
		/// interface and forwards the event onto them.
		/// </para>
		/// <para>
		/// If the node clicked is a text node then it is it is placed in edit mode.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.EditText"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextNode"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IClickEventReceiver"/>
		/// </remarks>
		protected virtual void OnDoubleClick(object sender, System.EventArgs evtArgs)
		{
			if (this.view != null)
			{
				this.nodesHit.Clear();
				int numHit = this.view.GetNodesAtPoint(this.nodesHit, this.mouseLocation);

				if (numHit > 0)
				{
					foreach (INode curNode in this.nodesHit)
					{
						IDispatchNodeEvents curEvtDispatcher = curNode as IDispatchNodeEvents;
						if (curEvtDispatcher != null)
						{
							curEvtDispatcher.DoubleClick(new NodeMouseEventArgs(curNode));
						}

						TextNode textNode = curNode as TextNode;
						if (textNode != null && !textNode.ReadOnly)
						{
							this.EditText(textNode);
						}
						
						Symbol symbol = curNode as Symbol;
						if (symbol != null)
						{
							if (symbol.Labels.Count > 0)
							{
								this.EditText(symbol.Labels[0]);
							}
						}
					}
				}
			}

			foreach (IClickEventReceiver rcvr in this.clickEventReceivers)
			{
				rcvr.DoubleClick();
			}
		}

		/// <summary>
		/// Called when a key down event is received.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Iterates through all tools that implement the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IKeyboardEventReceiver"/>
		/// interface and forwards the event onto them.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IKeyboardEventReceiver"/>
		/// </remarks>
		protected virtual void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs evtArgs)
		{
			foreach (IKeyboardEventReceiver rcvr in this.keyboardEventReceivers)
			{
				rcvr.KeyDown(evtArgs);
			}
		}

		/// <summary>
		/// Called when a key up event is received.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Iterates through all tools that implement the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IKeyboardEventReceiver"/>
		/// interface and forwards the event onto them.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IKeyboardEventReceiver"/>
		/// </remarks>
		protected virtual void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs evtArgs)
		{
			foreach (IKeyboardEventReceiver rcvr in this.keyboardEventReceivers)
			{
				rcvr.KeyUp(evtArgs);
			}

			if (evtArgs.KeyCode == Keys.Escape)
			{
				foreach (Tool curTool in this.tools)
				{
					if (curTool.Active)
					{
						curTool.UserAbort();
					}
				}
			}
		}

		/// <summary>
		/// Called when a key press event is received.
		/// </summary>
		/// <param name="sender">Object sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Iterates through all tools that implement the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IKeyboardEventReceiver"/>
		/// interface and forwards the event onto them.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IKeyboardEventReceiver"/>
		/// </remarks>
		protected virtual void OnKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs evtArgs)
		{
			foreach (IKeyboardEventReceiver rcvr in this.keyboardEventReceivers)
			{
				rcvr.KeyPress(evtArgs);
			}
		}

#endregion

#region Text editing

		/// <summary>
		/// Creates an text edit control for editing the given text object.
		/// </summary>
		/// <param name="textObj">Text object to edit</param>
		/// <remarks>
		/// A TextEdit control is created and attached to the given TextBase
		/// object. The TextEdit control updates the value of the TextBase
		/// object with changes made by the user. The controller can have
		/// only one TextEdit control open at a time.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextBase"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextEdit"/>
		/// </remarks>
		public virtual void EditText(TextBase textObj)
		{
			Control parentControl = this.ParentControl;
			if (parentControl != null)
			{
				this.textEdit = new TextEdit(parentControl, textObj);
				this.textEdit.KeyUp += new KeyEventHandler(TextEdit_KeyUp);
				this.textEdit.BeginEdit();
				this.textEdit.Focus();
				parentControl.Refresh();
			}
		}

		/// <summary>
		/// Ends the current text edit and destroys the active TextEdit control.
		/// </summary>
		/// <remarks>
		/// If there is a TextEdit control active, this method saves the changes
		/// made by it to the attached text object and then destroys the TextEdit
		/// control.
		/// </remarks>
		public virtual void EndEditText()
		{
			if (this.textEdit != null)
			{
				this.textEdit.EndEdit(true);
				this.textEdit.Dispose();
				this.textEdit = null;

				if (this.view != null)
				{
					this.view.Draw();
				}
			}
		}

		private void TextEdit_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				this.EndEditText();
			}
		}

#endregion

#region Selection

		/// <summary>
		/// List of nodes that are currently selected.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public NodeCollection SelectionList
		{
			get
			{
				return this.selectionList;
			}
		}

		/// <summary>
		/// Adds all nodes in the model to the SelectionList
		/// </summary>
		public void SelectAll()
		{
			this.selectionList.Clear();
			if (this.Model != null)
			{
				this.selectionList.Concat(this.Model.Nodes);
			}
			if (this.view != null)
			{
				this.view.Draw();
			}
		}

		private void SelectionList_Changed(object sender, NodeCollection.EventArgs evtArgs)
		{
			this.OnSelectionChanged(evtArgs);
		}

		/// <summary>
		/// Called when the contents of the selection list change.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		protected virtual void OnSelectionChanged(NodeCollection.EventArgs evtArgs)
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(this, evtArgs);
			}
		}

#endregion

#region Undo/Redo

		/// <summary>
		/// Maximum number of commands that can be stored in the undo and redo stacks.
		/// </summary>
		[
		Browsable(true)
		]
		public int MaxHistory
		{
			get
			{
				return this.maxHistory;
			}
			set
			{
				this.maxHistory = value;
			}
		}

		/// <summary>
		/// Current number of commands in the undo stack.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int UndoCount
		{
			get
			{
				return this.undoStack.Count;
			}
		}

		/// <summary>
		/// Current number of commands in the redo stack.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int RedoCount
		{
			get
			{
				return this.redoStack.Count;
			}
		}

		/// <summary>
		/// Executes the given command and adds it to the undo stack.
		/// </summary>
		/// <param name="cmd">Command to execute</param>
		/// <returns>true if successful, otherwise false</returns>
		public virtual bool ExecuteCommand(ICommand cmd)
		{
			bool success = cmd.Do(this.Model);

			if (success)
			{
				if (cmd.CanUndo && this.maxHistory > 0 && this.undoStack.Count < this.maxHistory)
				{
					this.undoStack.Push(cmd);
				}
			}

			return success;
		}

		/// <summary>
		/// Execute the command on the top of the undo stack and remove it
		/// from the stack.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public virtual bool UndoCommand()
		{
			bool success = false;

			if (this.undoStack.Count > 0)
			{
				ICommand cmd = (ICommand) this.undoStack.Pop();
				if (cmd != null)
				{
					success = cmd.Undo();

					if (success)
					{
						this.redoStack.Push(cmd);
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Execute the command on the top of the redo stack and remove it
		/// from the stack.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public virtual bool RedoCommand()
		{
			bool success = false;

			if (this.redoStack.Count > 0)
			{
				ICommand cmd = (ICommand) this.redoStack.Pop();
				if (cmd != null)
				{
					success = cmd.Do(this.Model);

					if (success)
					{
						this.undoStack.Push(cmd);
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Returns a command from the undo stack.
		/// </summary>
		/// <param name="offset">Offset into the undo stack</param>
		/// <returns>Command object at the given offset in the undo stack</returns>
		public ICommand PeekUndo(int offset)
		{
			return null;
		}

		/// <summary>
		/// Returns a command from the redo stack.
		/// </summary>
		/// <param name="offset">Offset into the redo stack</param>
		/// <returns>Command object at the given offset in the redo stack</returns>
		public ICommand PeekRedo(int offset)
		{
			return null;
		}

		/// <summary>
		/// Remove all commands from the undo and redo stacks.
		/// </summary>
		public void ClearHistory()
		{
			this.undoStack.Clear();
			this.redoStack.Clear();
		}

#endregion

#region Clipboard

		/// <summary>
		/// Indicates if there are any selected nodes that can be removed from the
		/// the model.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool CanCut
		{
			get
			{
				return (this.selectionList.Count > 0);
			}
		}

		/// <summary>
		/// Remove the currently selected nodes from the diagram and move
		/// them to the clipboard.
		/// </summary>
		public void Cut()
		{
			RemoveNodesCmd removeCmd = new RemoveNodesCmd();
			removeCmd.Nodes.Concat(this.selectionList);
			NodeCollection clipboardNodes = new NodeCollection();
			clipboardNodes.Concat(this.selectionList);
			Clipboard.SetDataObject(clipboardNodes, false);
			this.ExecuteCommand(removeCmd);
		}

		/// <summary>
		/// Indicates if there are any selected nodes that can be copied to the clipboard.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool CanCopy
		{
			get
			{
				return (this.selectionList.Count > 0);
			}
		}

		/// <summary>
		/// Copy the currently selected nodes to the clipboard.
		/// </summary>
		/// <remarks>
		/// Creates a new
		/// <see cref="Syncfusion.Windows.Forms.Diagram.NodeCollection"/>
		/// and copies the selected nodes into it. It then copies the new
		/// NodeCollection to the clipboard.
		/// </remarks>
		public void Copy()
		{
			NodeCollection clipboardNodes = new NodeCollection();
			clipboardNodes.Concat(this.selectionList);
			Clipboard.SetDataObject(clipboardNodes, false);
		}

		/// <summary>
		/// Indicates if there is any data in the clipboard that can be pasted
		/// into the model.
		/// </summary>
		/// <remarks>
		/// This method checks the clipboard to see if a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.NodeCollection"/>
		/// is available.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool CanPaste
		{
			get
			{
				bool canPaste = false;

				IDataObject clipboardData = Clipboard.GetDataObject();
				if (clipboardData.GetDataPresent(typeof(NodeCollection)))
				{
					canPaste = true;
				}

				return canPaste;
			}
		}

		/// <summary>
		/// Paste the contents of the clipboard to the diagram.
		/// </summary>
		/// <remarks>
		/// If a <see cref="Syncfusion.Windows.Forms.Diagram.NodeCollection"/>
		/// is available on the clipboard, this method gets it and inserts
		/// it into the diagram by creating and executing a 
		/// <see cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
		/// command.
		/// </remarks>
		public void Paste()
		{
			InsertNodesCmd insertCmd = null;
			NodeCollection clipboardNodes = null;
			IDataObject clipboardData = Clipboard.GetDataObject();
			if (clipboardData.GetDataPresent(typeof(NodeCollection)))
			{
				clipboardNodes = (NodeCollection) clipboardData.GetData(typeof(NodeCollection));
				if (clipboardNodes != null)
				{
					insertCmd = new InsertNodesCmd();
					insertCmd.Nodes.Concat(clipboardNodes);
					this.ExecuteCommand(insertCmd);
				}
			}
		}

#endregion

#region Command wrapper methods

		/// <summary>
		/// Deletes the selected nodes from the diagram.
		/// </summary>
		/// <remarks>
		/// Differs from the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.Cut"/>
		/// method in that it doesn't copy the nodes the clipboard before
		/// removing them.
		/// </remarks>
		public void Delete()
		{
			RemoveNodesCmd removeCmd = new RemoveNodesCmd();
			removeCmd.Nodes.Concat(this.selectionList);
			this.ExecuteCommand(removeCmd);
		}

		/// <summary>
		/// Brings the selected nodes to the front of the Z-order.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// This method creates and executes a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ZOrderCmd"/> command.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ZOrderCmd"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>
		/// </remarks>
		public bool BringToFront()
		{
			bool success = false;

			if (this.selectionList != null && this.selectionList.Count > 0)
			{
				ZOrderCmd cmd = new ZOrderCmd(ZOrderUpdate.Front);
				cmd.Nodes.Concat(selectionList);
				success = this.ExecuteCommand(cmd);
			}

			return success;
		}

		/// <summary>
		/// Sends the selected nodes to the back of the Z-order.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// This method creates and executes a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ZOrderCmd"/> command.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ZOrderCmd"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>
		/// </remarks>
		public bool SendToBack()
		{
			bool success = false;

			if (this.selectionList != null && this.selectionList.Count > 0)
			{
				ZOrderCmd cmd = new ZOrderCmd(ZOrderUpdate.Back);
				cmd.Nodes.Concat(selectionList);
				success = this.ExecuteCommand(cmd);
			}

			return success;
		}

		/// <summary>
		/// Brings the selected nodes forward in the Z-order.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// This method creates and executes a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ZOrderCmd"/> command.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ZOrderCmd"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>
		/// </remarks>
		public bool BringForward()
		{
			bool success = false;

			if (this.selectionList != null && this.selectionList.Count > 0)
			{
				ZOrderCmd cmd = new ZOrderCmd(ZOrderUpdate.Forward);
				cmd.Nodes.Concat(selectionList);
				success = this.ExecuteCommand(cmd);
			}

			return success;
		}

		/// <summary>
		/// Sends the selected nodes backward in the Z-order.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// This method creates and executes a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ZOrderCmd"/> command.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ZOrderCmd"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>
		/// </remarks>
		public bool SendBackward()
		{
			bool success = false;

			if (this.selectionList != null && this.selectionList.Count > 0)
			{
				ZOrderCmd cmd = new ZOrderCmd(ZOrderUpdate.Backward);
				cmd.Nodes.Concat(selectionList);
				success = this.ExecuteCommand(cmd);
			}

			return success;
		}

#endregion

#region IMouseController interface

		/// <summary>
		/// The cursor currently assigned to the Controller.
		/// </summary>
		Cursor IMouseController.Cursor
		{
			get
			{
				if (this.view != null)
				{
					return this.view.Cursor;
				}
				return null;
			}
		}

		/// <summary>
		/// Name of the mouse controller.
		/// </summary>
		string IMouseController.Name
		{
			get
			{
				return this.GetType().ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void IMouseController.CancelMode()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mouseEventArgs"></param>
		/// <param name="controller"></param>
		/// <returns></returns>
		int IMouseController.HitTest(MouseEventArgs mouseEventArgs, IMouseController controller)
		{
			return int.MaxValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseController.MouseDown(MouseEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseController.MouseHover(MouseEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		void IMouseController.MouseHoverEnter()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseController.MouseHoverLeave(EventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseController.MouseMove(MouseEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseController.MouseUp(MouseEventArgs e)
		{
		}

#endregion

#region IServiceProvider interface

		/// <summary>
		/// Returns the specified type of service object the caller.
		/// </summary>
		/// <param name="svcType">Type of service requested</param>
		/// <returns>
		/// The object matching the service type requested or null if the
		/// service is not supported.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method is similar to COM's IUnknown::QueryInterface method,
		/// although more generic. Instead of just returning interfaces,
		/// this method can return any type of object.
		/// </para>
		/// <para>
		/// The following services are supported:
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// </para>
		/// </remarks>
		object IServiceProvider.GetService(System.Type svcType)
		{
			if (svcType == typeof(IPropertyContainer))
			{
				return propertyContainer;
			}
			return null;
		}

#endregion

#region Fields

		/// <summary>
		/// Fired when the controller activates a tool.
		/// </summary>
		public event ToolEventHandler ToolActivate;

		/// <summary>
		/// Fired when the controller deactivates a tool.
		/// </summary>
		public event ToolEventHandler ToolDeactivate;

		/// <summary>
		/// Fired when the contents of the selection list change.
		/// </summary>
		public event NodeCollection.EventHandler SelectionChanged;

		/// <summary>
		/// View attached to this controller
		/// </summary>
		private View view = null;

		/// <summary>
		/// List of tools registered with this controller
		/// </summary>
		private ArrayList tools = null;

		/// <summary>
		/// Hashtable that maps tool names onto tool objects
		/// </summary>
		private Hashtable toolTable = null;

		/// <summary>
		/// List of tools that support the IMouseEventReceiver interface
		/// </summary>
		private ArrayList mouseEventReceivers = null;

		/// <summary>
		/// List of tools that support the IClickEventReceiver interface
		/// </summary>
		protected ArrayList clickEventReceivers = null;

		/// <summary>
		/// List of tools that support the IKeyboardEventReceiver interface
		/// </summary>
		private ArrayList keyboardEventReceivers = null;

		/// <summary>
		/// List of selected nodes
		/// </summary>
		private NodeCollection selectionList = null;

		/// <summary>
		/// Maximum size of the undo and redo stacks
		/// </summary>
		private int maxHistory;

		/// <summary>
		/// Stack containing commands that can have Undo applied
		/// </summary>
		private Stack undoStack = null;

		/// <summary>
		/// Stack containing commands that can have Redo applied
		/// </summary>
		private Stack redoStack = null;

		/// <summary>
		/// Flag controlling whether events are fired by the controller or not
		/// </summary>
		private bool eventsEnabled;

		/// <summary>
		/// Last known position of the mouse pointer
		/// </summary>
		private Point mouseLocation = new Point(0,0);

		/// <summary>
		/// TextEdit control currently active for editing text objects
		/// </summary>
		private TextEdit textEdit = null;

		/// <summary>
		/// Collection of nodes hit by the most recent mouse event
		/// </summary>
		private NodeCollection nodesHit = new NodeCollection();

		/// <summary>
		/// Node that owns the resize handle hit by the most recent mouse event
		/// </summary>
		private INode resizeHandleHitNode = null;

		/// <summary>
		/// Position of resize handle hit by the most recent mouse event
		/// </summary>
		private BoxPosition resizeHandleHit;

		/// <summary>
		/// Node that owns the vertex handle hit by the most recent mouse event
		/// </summary>
		private INode vertexHandleHitNode = null;

		/// <summary>
		/// Index position of the vertex handle hit by the most recent mouse event
		/// </summary>
		private int vertexHit = -1;

		/// <summary>
		/// List of nodes in which the mouse is currently hovering
		/// </summary>
		private NodeCollection hoverNodes = new NodeCollection();

		/// <summary>
		/// Reference to the attached view's property container
		/// </summary>
		private IPropertyContainer propertyContainer = null;

		/// <summary>
		/// Indicates if the Dispose() method has been called
		/// </summary>
		private bool disposed = false;

#endregion
	}
}
