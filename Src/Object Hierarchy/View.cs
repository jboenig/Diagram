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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.Serialization;

using Syncfusion.Runtime.InteropServices.WinAPI;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A view encapsulates a rectangular area inside of a control and renders
	/// a <see cref="Syncfusion.Windows.Forms.Diagram.Model"/> onto it.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A view is responsible for rendering the diagram onto a control surface
	/// (i.e. window). It contains a reference to a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model"/>, which contains the
	/// data portion of the diagram. The view renders the model onto a
	/// System.Drawing.Graphics context object belonging to the control that
	/// the view is hosted in. The view also renders other visual cues and decorations
	/// that are not persisted in the model, such as selection handles.
	/// </para>
	/// <para>
	/// The <see cref="Syncfusion.Windows.Forms.Diagram.View.ParentControl"/>
	/// property contains a reference to the control hosting the view.
	/// </para>
	/// <para>
	/// The view is responsible for conversions between world, view, and device
	/// coordinates. The model belongs to the world coordinate space. The view
	/// maps world coordinates onto view coordinates by applying its
	/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Magnification"/>
	/// and
	/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Origin"/>
	/// settings, which are used to implement zooming and scrolling. In other
	/// words, world coordinates are mapped to view coordinates by applying
	/// a transformation that translates to the origin and scales by a
	/// magnification percentage. Both world and view coordinates are stored
	/// as floating point numbers. View coordinates are mapped to device
	/// coordinates based on the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.MeasurementUnits"/>
	/// and
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.MeasurementScale"/>
	/// settings in the model and the resolution (DPI) of the output device.
	/// The default PageUnit setting is Pixel and the default PageScale setting
	/// is 1, which results in a 1-1 mapping from view to device coordinates.
	/// If the PageUnit is set to Inch, the PageScale is 0.5, and the resolution
	/// is 96 dpi then 1 logical unit in view coordinates will equal
	/// (1 * 96) * 0.5 = 48 pixels.
	/// </para>
	/// <para>
	/// The view provides methods for performing hit testing nodes, selection
	/// handles, vertices, and ports. The hit testing methods take points
	/// in device coordinates and perform the necessary conversion to world
	/// coordinates.
	/// </para>
	/// <para>
	/// The view also provides methods for drawing tracking objects. A tracking
	/// object is an outline of a rectangle or shape that is moved or tracked
	/// across the screen in response to mouse movements. The view has methods
	/// for drawing tracking outlines of rectangles, lines, polygons, curves,
	/// and System.Drawing.GraphicsPath objects. These tracking methods are
	/// typically used by user interface
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Tool"/> objects to
	/// track mouse movements.
	/// </para>
	/// <para>
	/// The view contains public methods that can be called to render and
	/// repaint onto the host control. The view uses a technique called back
	/// buffering, which divides rendering into 2 stages. First, the view
	/// renders onto a memory-based bitmap image (the back buffer). The back
	/// buffer is then painted onto the host control. This technique eliminates
	/// flicker and has the added benefit of leaving the view with an in-memory
	/// representation of the last frame it rendered. The back buffer can be
	/// accessed through the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.View.DrawingSurface"/>
	/// property.
	/// </para>
	/// <para>
	/// The <see cref="Syncfusion.Windows.Forms.Diagram.View.Draw"/> method
	/// renders the back buffer and updates the control. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Refresh"/>
	/// method takes a portion of the back buffer and draws it onto the host
	/// control without re-rendering the back buffer. This is handy for things
	/// like tracking, which temporarily clutter up the view and need to
	/// repaint it without incurring the overhead of rendering.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller"/>
	/// </remarks>
	[
	Serializable
	]
	public class View : Component, IDraw, IBounds2D, IServiceProvider, ISerializable, IDeserializationCallback
	{
		#region Constructors

		/// <summary>
		/// Default constructor for the View
		/// </summary>
		public View()
		{
			this.propertyContainer = new PropertyContainer();
			this.propertyContainer.PropertyChanged += new PropertyEventHandler(this.PropertyContainer_PropertyChanged);
			this.SetDefaultPropertyValues();
			this.bounds = System.Drawing.Rectangle.Empty;
			this.backgroundColor = Color.DarkGray;
			this.selectionList = null;
			this.trackingStyle = new TrackingStyle(this.propertyContainer);
			this.backBuffer = null;
			this.grid = this.CreateGrid();
			this.grid.ContainerView = this;
			this.magnification = new Size(100,100);
			this.dpiX = 96;
			this.dpiY = 96;
			this.handleSize = 6;
			this.handleColor = Color.White;
			this.handleAnchorColor = Color.LightGray;
			this.origin = new PointF(0,0);
			this.pageSizeKnown = false;
		}

		/// <summary>
		/// Constructs a View object for a given parent control.
		/// </summary>
		/// <param name="parentControl">Parent control (i.e. window) hosting the view</param>
		public View(Control parentControl)
		{
			this.propertyContainer = new PropertyContainer();
			this.SetDefaultPropertyValues();
			this.bounds = System.Drawing.Rectangle.Empty;
			if (parentControl != null)
			{
				this.bounds = parentControl.Bounds;
			}
			this.backgroundColor = Color.DarkGray;
			this.selectionList = null;
			this.trackingStyle = new TrackingStyle(this.propertyContainer);
			this.backBuffer = null;
			this.grid = this.CreateGrid();
			this.grid.ContainerView = this;
			this.magnification = new Size(100,100);
			this.dpiX = 96;
			this.dpiY = 96;
			this.handleSize = 6;
			this.handleColor = Color.White;
			this.handleAnchorColor = Color.LightGray;
			this.origin = new PointF(0,0);
			this.pageSizeKnown = false;

			this.Initialize(parentControl);
		}

		/// <summary>
		/// Constructs a view for a given parent control with the given dimensions.
		/// </summary>
		/// <param name="parentControl">Parent control (i.e. window) to host the view</param>
		/// <param name="top">Top of view bounds</param>
		/// <param name="left">Left of view bounds</param>
		/// <param name="width">Width of view bounds</param>
		/// <param name="height">Height of view bounds</param>
		public View(Control parentControl, int top, int left, int width, int height)
		{
			this.propertyContainer = new PropertyContainer();
			this.SetDefaultPropertyValues();
			this.bounds = new System.Drawing.Rectangle(top, left, width, height);
			this.backgroundColor = Color.DarkGray;
			this.selectionList = null;
			this.trackingStyle = new TrackingStyle(this.propertyContainer);
			this.backBuffer = null;
			this.grid = this.CreateGrid();
			this.grid.ContainerView = this;
			this.magnification = new Size(100,100);
			this.dpiX = 96;
			this.dpiY = 96;
			this.handleSize = 6;
			this.handleColor = Color.White;
			this.handleAnchorColor = Color.LightGray;
			this.origin = new PointF(0,0);
			this.pageSizeKnown = false;
			this.Initialize(parentControl);
		}

		/// <summary>
		/// Serialization constructor for views.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected View(SerializationInfo info, StreamingContext context)
		{
			this.selectionList = null;
			this.backBuffer = null;
			this.dpiX = 96;
			this.dpiY = 96;

			this.bounds = (System.Drawing.Rectangle) info.GetValue("bounds", typeof(System.Drawing.Rectangle));
			this.propertyContainer = (PropertyContainer) info.GetValue("propertyContainer", typeof(PropertyContainer));
			this.origin = (PointF) info.GetValue("origin", typeof(PointF));
			this.backgroundColor = (System.Drawing.Color) info.GetValue("backgroundColor", typeof(System.Drawing.Color));
			this.handleSize = (int) info.GetValue("handleSize", typeof(int));
			this.handleColor = (System.Drawing.Color) info.GetValue("handleColor", typeof(System.Drawing.Color));
			this.handleAnchorColor = (System.Drawing.Color) info.GetValue("handleAnchorColor", typeof(System.Drawing.Color));
			this.magnification = (System.Drawing.Size) info.GetValue("magnification", typeof(System.Drawing.Size));
		}

		/// <summary>
		/// Called to release resources held by the view.
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
					this.DetachModelEventHandlers();
					this.model = null;

					if (this.parentControl != null)
					{
						this.parentControl = null;
					}
				}
			}

			base.Dispose(disposing);
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Attaches the view to a given parent control.
		/// </summary>
		/// <param name="parentControl">Parent control hosting the view</param>
		/// <remarks>
		/// <para>
		/// Stores a reference to the parent control in the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View.ParentControl"/>
		/// property and caches the device resolution.
		/// </para>
		/// </remarks>
		public virtual void Initialize(Control parentControl)
		{
			if (this.parentControl != null)
			{
				throw new EMVCInit();
			}
			this.parentControl = parentControl;

			if (this.parentControl != null)
			{
				Graphics grfx = this.parentControl.CreateGraphics();
				this.dpiX = grfx.DpiX;
				this.dpiY = grfx.DpiX;
				grfx.Dispose();
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// The model attached to this view.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Model Model
		{
			get
			{
				return this.model;
			}
			set
			{
				if (value != this.model)
				{
					if (this.model != null)
					{
						this.DetachModelEventHandlers();
					}

					this.model = value;
					this.pageSizeKnown = false;

					if (this.model != null)
					{
						this.AttachModelEventHandlers();

						System.Drawing.Printing.PageSettings pageSettings = this.model.PageSettings;
						if (pageSettings != null)
						{
							try
							{
								this.pageSize = pageSettings.Bounds.Size;
								this.pageSizeKnown = true;
							}
							catch
							{
								this.pageSizeKnown = false;
							}
						}

						this.grid.PropertyContainer = this.propertyContainer;
					}
				}
			}
		}

		/// <summary>
		/// The parent control (window) hosting this view.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Control ParentControl
		{
			get
			{
				return this.parentControl;
			}
		}

		/// <summary>
		/// Logical origin of the view in world coordinates.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property moves the view relative to the world
		/// coordinate space. The value specifies a point in the world
		/// coordinate space that corresponds to the top left corner
		/// of the view.
		/// </para>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual PointF Origin
		{
			get
			{
				return this.origin;
			}
			set
			{
				if (this.origin != value)
				{
					PointF oldOrigin = this.origin;
					this.origin = value;
					this.OnOriginChanged(new ViewOriginEventArgs(oldOrigin, this.origin));
				}
			}
		}

		/// <summary>
		/// Returns the size of the scrollable area in device coordinates.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value returned is the width and height of the model converted to
		/// device coordinates. If there is no model attached to the view at
		/// the time of the call, then the size of the view is returned instead.
		/// </para>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual System.Drawing.Size VirtualSize
		{
			get
			{
				System.Drawing.Size szVirtual = new System.Drawing.Size(this.bounds.Width, this.bounds.Height);

				if (this.model != null)
				{
					SizeF szModel = this.model.Bounds.Size;
					szVirtual.Width = (int) Math.Round((double) szModel.Width);
					szVirtual.Height = (int) Math.Round((double) szModel.Height);
				}

				return szVirtual;
			}
		}

		/// <summary>
		/// Specifies the X and Y magnification (zoom) values on a scale of 1 to n.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This value is used to zoom the view in and out. The X and Y axes can
		/// be scaled independently. Normally, the X and Y axes will have the
		/// same magnification value.
		/// </para>
		/// <para>
		/// The value of this property along with the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Origin"/> are used to
		/// create the view transform, which is used to map world coordinates onto
		/// view coordinates.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.GetViewTransform"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.Origin"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
		]
		public Size Magnification
		{
			get
			{
				return this.magnification;
			}
			set
			{
				if (this.magnification != value)
				{
					Size oldVal = this.magnification;
					this.magnification = value;
					this.OnMagnificationChanged(new ViewMagnificationEventArgs(oldVal, value));
				}
			}
		}

		/// <summary>
		/// List of currently selected nodes.
		/// </summary>
		/// <remarks>
		/// Contains a reference to the controller's selection list. This property
		/// is assigned by the controller it attaches to the view.
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
				return this.selectionList;
			}
			set
			{
				this.selectionList = value;
			}
		}

		/// <summary>
		/// Node in the selection list that acts as the anchor.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The anchor node is always the last node in the selection list.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.SelectionList"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public INode SelectionAnchorNode
		{
			get
			{
				INode anchorNode = null;

				if (this.selectionList != null)
				{
					anchorNode = this.selectionList.Last;
				}

				return anchorNode;
			}
		}

		/// <summary>
		/// The color used to clear the view before rendering the diagram.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The background of the view is the region outside of the visible
		/// diagram.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Description("Background color of the view")
		]
		public System.Drawing.Color BackgroundColor
		{
			get
			{
				return this.backgroundColor;
			}
			set
			{
				this.backgroundColor = value;
			}
		}

		/// <summary>
		/// Size of selection handles specified in device coordinates.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Description("Size of selection handles")
		]
		public int HandleSize
		{
			get
			{
				return this.handleSize;
			}
			set
			{
				this.handleSize = value;
			}
		}

		/// <summary>
		/// Color used to drawing selection handles.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Description("Color of selection handles")
		]
		public System.Drawing.Color HandleColor
		{
			get
			{
				return this.handleColor;
			}
			set
			{
				this.handleColor = value;
			}
		}

		/// <summary>
		/// Color used for handles of anchor node.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Description("Color of selection handles for anchor node")
		]
		public System.Drawing.Color HandleAnchorColor
		{
			get
			{
				return this.handleAnchorColor;
			}
			set
			{
				this.handleAnchorColor = value;
			}
		}

		/// <summary>
		/// Determines whether resize handles or vertex handles will be drawn.
		/// </summary>
		/// <remarks>
		/// This property supports vertex editing mode.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SelectHandleType"/>
		/// </remarks>
		protected SelectHandleType SelectHandleMode
		{
			get
			{
				object value = this.propertyContainer.GetPropertyValue("SelectHandleMode");
				if (value == null)
				{
					return SelectHandleType.Resize;
				}
				return (SelectHandleType) value;
			}
		}

		/// <summary>
		/// Grid of evenly spaced points that provide a visual guide to the
		/// user.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Draws a matrix of evenly spaced points in the view and provides
		/// snap to grid calculations.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid"/>
		/// </remarks>
		[
		Browsable(true),
		TypeConverter(typeof(LayoutGridConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
		]
		public LayoutGrid Grid
		{
			get
			{
				return this.grid;
			}
		}

		/// <summary>
		/// Determines the type of grid presented in the view.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The choices are either Point or Line.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Appearance")
		]
		public TypeGrid GridType
		{
			get
			{
				object value = this.propertyContainer.GetPropertyValue("GridType");
				if (value == null)
				{
					return TypeGrid.Point;
				}
				return (TypeGrid) value;
			}
			set
			{
				this.propertyContainer.SetPropertyValue("GridType", value);
				this.grid = this.CreateGrid();
			}
		}

		/// <summary>
		/// Determines if page bounds are displayed in the view or not.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If this property is set to true, then lines are drawn in the
		/// view to mark the boundaries of pages. The size of the model
		/// and settings for the current default printer determine how
		/// many pages the diagram contains.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.PageSettings"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public bool ShowPageBounds
		{
			get
			{
				object value = propertyContainer.GetPropertyValue("ShowPageBounds");
				if (value == null)
				{
					return false;
				}
				return (bool) value;
			}
			set
			{
				propertyContainer.SetPropertyValue("ShowPageBounds", value);
			}
		}

		/// <summary>
		/// Cursor currently used in the view.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual System.Windows.Forms.Cursor Cursor
		{
			get
			{
				return this.cursor;
			}
			set
			{
				this.cursor = value;
			}
		}

		#endregion

		#region Styles

		/// <summary>
		/// Line style for tracking.
		/// </summary>
		[
		Browsable(true),
		Category("Appearance"),
		TypeConverter(typeof(TrackingStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Syncfusion.Windows.Forms.Diagram.TrackingStyle TrackingStyle
		{
			get
			{
				return this.trackingStyle;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Returns a transformation matrix that maps world coordinates to view
		/// coordinates.
		/// </summary>
		/// <returns>Transformation matrix</returns>
		/// <remarks>
		/// <para>
		/// The view transformation maps world coordinates to view coordinates.
		/// It is calculated by translating by the offset specified in the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Origin"/>
		/// property and scaling by the value in the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.View.Magnification"/>
		/// property.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.Origin"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.Magnification"/>
		/// </remarks>
		public virtual Matrix GetViewTransform()
		{
			Matrix viewTransform = new Matrix();

			viewTransform.Translate(-this.origin.X, -this.origin.Y);
			viewTransform.Scale((float) this.magnification.Width / 100.0f, (float) this.magnification.Height / 100.0f);

			return viewTransform;
		}

		/// <summary>
		/// Scrolls the view origin by a given X and Y offset.
		/// </summary>
		/// <param name="dx">X offset</param>
		/// <param name="dy">Y offset</param>
		public virtual void ScrollBy(float dx, float dy)
		{
			this.origin.X += dx;
			this.origin.Y += dy;
			this.parentControl.Invalidate();
		}

		/// <summary>
		/// Takes a device point and returns the nearest grid point.
		/// </summary>
		/// <param name="ptDevIn">Point to snap</param>
		/// <returns>Point on the grid nearest the input point</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.Grid"/>
		/// </remarks>
		public System.Drawing.Point SnapPointToGrid(System.Drawing.Point ptDevIn)
		{
			if (this.grid != null && this.grid.SnapToGrid)
			{
				return this.grid.GetNearestGridPoint(ptDevIn);
			}
			return ptDevIn;
		}

		/// <summary>
		/// Takes a device point and returns the nearest grid point.
		/// </summary>
		/// <param name="x">X coordinate of point to snap</param>
		/// <param name="y">Y coordinate of point to snap</param>
		/// <returns>Point on the grid nearest the input point</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.Grid"/>
		/// </remarks>
		public System.Drawing.Point SnapPointToGrid(int x, int y)
		{
			if (this.grid != null && this.grid.SnapToGrid)
			{
				return this.grid.GetNearestGridPoint(new System.Drawing.Point(x,y));
			}
			return new System.Drawing.Point(x,y);
		}

		#endregion

		#region Rendering

		/// <summary>
		/// Renders the view to the parent control.
		/// </summary>
		/// <remarks>
		/// This method creates a new System.Drawing.Graphics context for
		/// the
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.ParentControl"/>
		/// and renders the view onto that graphics context object.
		/// </remarks>
		public void Draw()
		{
			Graphics grfx = null;

			if (this.parentControl != null)
			{
				grfx = this.parentControl.CreateGraphics();
				this.Draw(grfx);
				grfx.Dispose();
			}
		}

		/// <summary>
		/// Renders the view onto a System.Drawing.Graphics context object.
		/// </summary>
		/// <param name="grfx">Graphics context object to render to</param>
		/// <remarks>
		/// <para>
		/// This method first renders the view onto the back buffer. It fills
		/// the buffer with the view's background color, draws the grid and
		/// page bounds, and then draws the model. Then paints the back
		/// buffer onto the graphics context.
		/// </para>
		/// </remarks>
		public virtual void Draw(System.Drawing.Graphics grfx)
		{
			Global.ViewMatrix = GetViewTransform();

			if (this.backBuffer == null)
			{
				CreateBackBuffer();
			}

			// Create graphics context for back buffer
			Graphics grfxBuffer = Graphics.FromImage(this.backBuffer);
			grfxBuffer.SetClip(this.bounds);
			grfxBuffer.Clear(this.backgroundColor);

			// Push the view transform onto the stack
			grfxBuffer.Transform = Global.ViewMatrix;

			if (this.model != null)
			{
				// Fill bounds of model with background color
				Brush fillBrush = this.model.BackgroundStyle.CreateBrush();
				RectangleF modelBounds = this.model.Bounds;
				grfxBuffer.PageUnit = this.model.MeasurementUnits;
				grfxBuffer.PageScale = this.model.MeasurementScale;
				grfxBuffer.FillRectangle(fillBrush, modelBounds);
				fillBrush.Dispose();

				// Draw the grid
				if (this.grid != null)
				{
					this.grid.Draw(grfxBuffer);
				}

				if (this.ShowPageBounds)
				{
					this.DrawPageBounds(grfxBuffer);
				}

				// Render to back buffer
				this.model.Draw(grfxBuffer);
			}

			// Switch back to pixel mode
			grfxBuffer.Transform = new Matrix();
			grfxBuffer.PageUnit = GraphicsUnit.Pixel;
			grfxBuffer.PageScale = 1.0f;

			// Draw handles around nodes in selection list
			DrawSelectionHandles(grfxBuffer);

			grfxBuffer.Dispose();

			// Blt the back buffer to the window DC
			grfx.DrawImage(this.backBuffer, 0, 0);
		}

		/// <summary>
		/// Transfers a rectangular area of the back buffer onto the parent
		/// control.
		/// </summary>
		/// <param name="rect">Bounding rectangle of area to refresh</param>
		/// <remarks>
		/// <para>
		/// This method is used to repaint an area of the parent control without
		/// rendering the back buffer. It uses the current contents of the back
		/// buffer and draws it onto a rectangular area of the parent control.
		/// </para>
		/// </remarks>
		public void Refresh(System.Drawing.Rectangle rect)
		{
			Graphics grfx = null;

			if (this.parentControl != null)
			{
				grfx = this.parentControl.CreateGraphics();

				if (this.backBuffer != null)
				{
					// Blt the back buffer to the window DC
					grfx.DrawImage(this.backBuffer, rect, rect.Left, rect.Top, rect.Width, rect.Height, GraphicsUnit.Pixel);
				}

				grfx.Dispose();
			}
		}

		/// <summary>
		/// Transfers the back buffer onto the parent control.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method is used to repaint the view onto the parent control without
		/// rendering the back buffer. It uses the current contents of the back
		/// buffer and draws it onto the parent control.
		/// </para>
		/// </remarks>
		public void Refresh()
		{
			Graphics grfx = null;

			if (this.parentControl != null)
			{
				grfx = this.parentControl.CreateGraphics();

				if (this.backBuffer != null)
				{
					// Blt the back buffer to the window DC
					grfx.DrawImage(this.backBuffer, this.bounds, this.bounds.Left, this.bounds.Top, this.bounds.Width, this.bounds.Height, GraphicsUnit.Display);
				}

				grfx.Dispose();
			}
		}

		/// <summary>
		/// The back buffer used for rendering.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The drawing surface is a bitmap on which the view renders itself
		/// before drawing it to the parent control. It is an in memory representation
		/// of the last frame rendered by the view. It can be accessed and drawn to
		/// directly.
		/// </para>
		/// <para>
		/// NOTE: The terms drawing surface and back buffer are used interchangeably.
		/// </para>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Bitmap DrawingSurface
		{
			get
			{
				if (this.backBuffer == null)
				{
					CreateBackBuffer();
				}

				return this.backBuffer;
			}
		}

		/// <summary>
		/// Draws lines on the view that indicate where page boundaries exists.
		/// </summary>
		/// <param name="grfx">Graphics context object on which to draw</param>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.ShowPageBounds"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.PageSettings"/>
		private void DrawPageBounds(System.Drawing.Graphics grfx)
		{
			bool done;
			float curX;
			float curY;
			int curCol;
			int curRow;

			if (this.pageSizeKnown && this.model != null)
			{
				RectangleF mdlWorldBounds = this.model.Bounds;
				float fMdlLeft = mdlWorldBounds.Left;
				float fMdlTop = mdlWorldBounds.Top;
				float fMdlRight = mdlWorldBounds.Right;
				float fMdlBottom = mdlWorldBounds.Bottom;

				System.Drawing.Size pageSizeDev = new System.Drawing.Size(0,0);
				pageSizeDev.Width = (int) Math.Round(((float) this.pageSize.Width * grfx.DpiX) / 100.0f);
				pageSizeDev.Height = (int) Math.Round(((float) this.pageSize.Height * grfx.DpiY) / 100.0f);
				SizeF pageSizeWorld = this.ViewToWorld(this.DeviceToView(pageSizeDev));
				float pageWidth = pageSizeWorld.Width;
				float pageHeight = pageSizeWorld.Height;

				System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.DarkBlue, 2);
				pen.DashStyle = DashStyle.Dash;
				System.Drawing.PointF pt1 = new System.Drawing.PointF(0.0f,0.0f);
				System.Drawing.PointF pt2 = new System.Drawing.PointF(0.0f,0.0f);

				// Draw vertical page bounds
				curCol = 1;
				done = false;
				pt1.Y = fMdlTop;
				pt2.Y = fMdlBottom;
				while (!done)
				{
					curX = ((float)curCol) * pageWidth;
					if (curX > fMdlRight)
					{
						done = true;
					}
					else
					{
						pt1.X = curX;
						pt2.X = curX;
						grfx.DrawLine(pen, pt1, pt2);
						curCol++;
					}
				}

				// Draw horizontal page bounds
				curRow = 1;
				done = false;
				pt1.X = fMdlLeft;
				pt2.X = fMdlRight;
				while (!done)
				{
					curY = ((float)curRow) * pageHeight;
					if (curY > fMdlBottom)
					{
						done = true;
					}
					else
					{
						pt1.Y = curY;
						pt2.Y = curY;
						grfx.DrawLine(pen, pt1, pt2);
						curRow++;
					}
				}

				pen.Dispose();
			}
		}

		#endregion

		#region Selection Handles

		/// <summary>
		/// Calculates the bounds of a handle from a given point based on the handle size.
		/// </summary>
		/// <param name="ptDevice">Point on which to center the handle bounds</param>
		/// <returns>Rectangle in view coordinates</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.HandleSize"/>
		/// </remarks>
		public virtual System.Drawing.Rectangle CalcHandleRect(Point ptDevice)
		{
			int handleOffset = (int) ((float) this.handleSize / 2.0f);
			return new System.Drawing.Rectangle(ptDevice.X - handleOffset, ptDevice.Y - handleOffset, this.handleSize, this.handleSize);
		}

		/// <summary>
		/// Calculates the bounds of a handle given a box position on a rectangle.
		/// </summary>
		/// <param name="rect">Bounding rectangle of selected object</param>
		/// <param name="pos">Position of handle on the bounding rectangle</param>
		/// <returns>Rectangle in view coordinates</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.HandleSize"/>
		/// </remarks>
		public virtual System.Drawing.Rectangle CalcHandleRect(System.Drawing.Rectangle rect, BoxPosition pos)
		{
			Point loc = rect.Location;
			Size sz = rect.Size;
			Point ptHandle;
			int handleOffset = (int) ((float) this.handleSize / 2.0f);

			switch (pos)
			{
				case BoxPosition.TopLeft:
					ptHandle = new Point(loc.X, loc.Y);
					break;

				case BoxPosition.TopCenter:
					ptHandle = new Point(loc.X + (sz.Width / 2), loc.Y);
					break;

				case BoxPosition.TopRight:
					ptHandle = new Point(loc.X + sz.Width, loc.Y);
					break;

				case BoxPosition.MiddleLeft:
					ptHandle = new Point(loc.X, loc.Y + (sz.Height / 2));
					break;

				case BoxPosition.MiddleRight:
					ptHandle = new Point(loc.X + sz.Width, loc.Y + (sz.Height / 2));
					break;

				case BoxPosition.BottomLeft:
					ptHandle = new Point(loc.X, loc.Y + sz.Height);
					break;

				case BoxPosition.BottomCenter:
					ptHandle = new Point(loc.X + (sz.Width / 2), loc.Y + sz.Height);
					break;

				case BoxPosition.BottomRight:
					ptHandle = new Point(loc.X + sz.Width, loc.Y + sz.Height);
					break;

				default:
					ptHandle = new Point(0,0);
					break;
			}

			return new System.Drawing.Rectangle(ptHandle.X - handleOffset, ptHandle.Y - handleOffset, this.handleSize, this.handleSize);
		}

		/// <summary>
		/// Draws resize handles around the given node.
		/// </summary>
		/// <param name="grfx">Graphics context on which to draw</param>
		/// <param name="node">Node to draw handles for</param>
		/// <param name="isAnchor">
		/// Flag indicating if the node is the anchor for the selection list
		/// </param>
		/// <returns>true if successful; otherwise false</returns>
		protected virtual bool DrawResizeHandles(System.Drawing.Graphics grfx, INode node, bool isAnchor)
		{
			if (node == null)
			{
				return false;
			}

			IBounds2DF objBounds = node as IBounds2DF;

			if (objBounds == null)
			{
				return false;
			}

			// Interrogate the object to find out what its capabilities are

			object propValue = null;
			ResizeHandleStyle handleStyle = ResizeHandleStyle.OverlapBounds;

			IPropertyContainer propContainer = node as IPropertyContainer;
			if (propContainer != null)
			{
				propValue = propContainer.GetPropertyValue("ResizeHandleStyle");
				if (propValue != null)
				{
					handleStyle = (ResizeHandleStyle) propValue;
				}
			}

			System.Drawing.RectangleF curHandleRect;
			Pen handlePen = new Pen(Color.Black);
			Brush handleBrush;

			if (isAnchor)
			{
				handleBrush = new SolidBrush(this.handleAnchorColor);
			}
			else
			{
				handleBrush = new SolidBrush(this.handleColor);
			}

			// Draw resize handles around the node's bounding rectangle
			RectangleF rcBounds = objBounds.Bounds;
			System.Drawing.Rectangle rcBoundsDev = this.ViewToDevice(this.WorldToView(rcBounds));

			if (handleStyle == ResizeHandleStyle.OutsideBounds)
			{
				// Inflate rectangle
				int handleOffset = (int) ((float) this.handleSize / 1.5f);
				rcBoundsDev.Inflate(handleOffset, handleOffset);

				// Draw selection bounds
				Brush brushSel = new TextureBrush(Resources.Textures.CheckerBoard);
				Pen penSel = new Pen(brushSel, this.handleSize);
				grfx.DrawRectangle(penSel, rcBoundsDev);
			}

			// Draw resize handles
			System.Array positions = Enum.GetValues(typeof(BoxPosition));
			foreach (BoxPosition curPos in positions)
			{
				if (curPos != BoxPosition.Center)
				{
					curHandleRect = CalcHandleRect(rcBoundsDev, curPos);
					grfx.FillRectangle(handleBrush, curHandleRect.X, curHandleRect.Y, curHandleRect.Width, curHandleRect.Height);
					grfx.DrawRectangle(handlePen, curHandleRect.X, curHandleRect.Y, curHandleRect.Width, curHandleRect.Height);
				}
			}

			handlePen.Dispose();
			handleBrush.Dispose();

			return true;
		}

		/// <summary>
		/// Draws vertex handles for the given node.
		/// </summary>
		/// <param name="grfx">Graphics context on which to draw</param>
		/// <param name="node">Node to draw handles for</param>
		/// <param name="isAnchor">
		/// Flag indicating if the node is the anchor for the selection list
		/// </param>
		/// <returns>true if successful; otherwise false</returns>
		protected virtual bool DrawVertexHandles(System.Drawing.Graphics grfx, INode node, bool isAnchor)
		{
			if (node == null)
			{
				return false;
			}

			IServiceProvider nodeSvcProvider = node as IServiceProvider;
			ITransform nodeXform = null;
			IPoints objPoints = null;
			object service;

			if (nodeSvcProvider != null)
			{
				service = nodeSvcProvider.GetService(typeof(IPoints));
				if (service != null)
				{
					objPoints = service as IPoints;
				}

				service = nodeSvcProvider.GetService(typeof(ITransform));
				if (service != null)
				{
					nodeXform = service as ITransform;
				}
			}

			if (objPoints == null)
			{
				return false;
			}

			Point curDevHandlePt;
			System.Drawing.RectangleF curHandleRect;
			Pen handlePen = new Pen(Color.Black);
			Brush handleBrush;

			if (isAnchor)
			{
				handleBrush = new SolidBrush(this.handleAnchorColor);
			}
			else
			{
				handleBrush = new SolidBrush(this.handleColor);
			}

			// Draw vertex handles
			PointF[] vertices = objPoints.GetPoints();
			if (vertices != null)
			{
				if (nodeXform != null)
				{
					nodeXform.WorldTransform.TransformPoints(vertices);
				}

				foreach (PointF vertex in vertices)
				{
					curDevHandlePt = ViewToDevice(WorldToView(vertex));
					curHandleRect = CalcHandleRect(curDevHandlePt);
					grfx.FillRectangle(handleBrush, curHandleRect.X, curHandleRect.Y, curHandleRect.Width, curHandleRect.Height);
					grfx.DrawRectangle(handlePen, curHandleRect.X, curHandleRect.Y, curHandleRect.Width, curHandleRect.Height);
				}
			}

			return true;
		}

		/// <summary>
		/// Draws selection handles for the nodes in the selection list.
		/// </summary>
		/// <param name="grfx">Graphics context to draw onto</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.SelectionList"/>
		/// </remarks>
		protected virtual void DrawSelectionHandles(System.Drawing.Graphics grfx)
		{
			// Draw handles around nodes in selection list
			if (this.selectionList != null)
			{
				IEnumerator enumSelection = this.selectionList.GetEnumerator();
				INode curNode = null;
				bool isAnchor = true;

				if (this.selectionList != null)
				{
					for (int nodeIdx = this.selectionList.Count-1; nodeIdx >= 0; nodeIdx--)
					{
						curNode = this.selectionList[nodeIdx];

						if (curNode != null)
						{
							// Interrogate node to find out what its capabilities are
							object propValue = null;
							bool allowVertexEdit = false;
							bool allowScale = false;

							IPropertyContainer propContainer = curNode as IPropertyContainer;
							if (propContainer != null)
							{
								propValue = propContainer.GetPropertyValue("AllowVertexEdit");
								if (propValue != null)
								{
									allowVertexEdit = (bool) propValue;
								}
								propValue = propContainer.GetPropertyValue("AllowResize");
								if (propValue != null)
								{
									allowScale = (bool) propValue;
								}
							}

							if (allowScale && allowVertexEdit)
							{
								if (this.SelectHandleMode == SelectHandleType.Resize)
								{
									if (this.DrawResizeHandles(grfx, curNode, isAnchor))
									{
										isAnchor = false;
									}
								}
								else
								{
									if (this.DrawVertexHandles(grfx, curNode, isAnchor))
									{
										isAnchor = false;
									}
								}
							}
							else if (allowScale)
							{
								if (this.DrawResizeHandles(grfx, curNode, isAnchor))
								{
									isAnchor = false;
								}
							}
							else if (allowVertexEdit)
							{
								if (this.DrawVertexHandles(grfx, curNode, isAnchor))
								{
									isAnchor = false;
								}
							}
						}
					}
				}
			}
		}

		#endregion

		#region Coordinate Conversion

		/// <summary>
		/// Converts the given point from device coordinates to view coordinates.
		/// </summary>
		/// <param name="ptDevice">Point to convert</param>
		/// <returns>Point in view coordinates</returns>
		public PointF DeviceToView(Point ptDevice)
		{
			float viewX;
			float viewY;
			float unitsPerInch = 1.0f;
			float measurementScale = 1.0f;
			float dpuX = 1.0f;
			float dpuY = 1.0f;

			if (this.model != null)
			{
				if (this.model.MeasurementUnits != GraphicsUnit.Pixel)
				{
					unitsPerInch = Measurements.UnitsPerInch(this.model.MeasurementUnits);
					dpuX = this.dpiX / unitsPerInch;
					dpuY = this.dpiX / unitsPerInch;
				}
				measurementScale = this.model.MeasurementScale;
			}

			viewX = (float) ptDevice.X / (measurementScale * dpuX);
			viewY = (float) ptDevice.Y / (measurementScale * dpuY);

			return new PointF(viewX, viewY);
		}

		/// <summary>
		/// Converts the given array of points from device coordinates to
		/// view coordinates.
		/// </summary>
		/// <param name="devicePts">Device points to convert</param>
		/// <param name="viewPts">Converted points in view coordinates</param>
		public void DeviceToView(Point[] devicePts, out PointF[] viewPts)
		{
			viewPts = new PointF[devicePts.Length];

			for (int ptIdx = 0; ptIdx < devicePts.Length; ptIdx++)
			{
				viewPts[ptIdx] = DeviceToView(devicePts[ptIdx]);
			}
		}

		/// <summary>
		/// Converts the given rectangle from device coordinates to view
		/// coordinates.
		/// </summary>
		/// <param name="rcDevice">Rectangle to convert</param>
		/// <returns>Converted rectangle in view coordinates</returns>
		public RectangleF DeviceToView(System.Drawing.Rectangle rcDevice)
		{
			Point deviceUpperLeft = new Point(rcDevice.Left, rcDevice.Top);
			Point deviceBottomRight = new Point(rcDevice.Right, rcDevice.Bottom);
			PointF viewUpperLeft = DeviceToView(deviceUpperLeft);
			PointF viewBottomRight = DeviceToView(deviceBottomRight);
			return new RectangleF(viewUpperLeft.X, viewUpperLeft.Y, viewBottomRight.X - viewUpperLeft.X, viewBottomRight.Y - viewUpperLeft.Y);
		}

		/// <summary>
		/// Converts the given size from device to view coordinates.
		/// </summary>
		/// <param name="szDevice">Size to convert</param>
		/// <returns>Converted size in view coordinates</returns>
		public SizeF DeviceToView(Size szDevice)
		{
			PointF ptView = DeviceToView(new Point(szDevice));
			return new SizeF(ptView);
		}

		/// <summary>
		/// Converts the given point from view coordinates to world coordinates.
		/// </summary>
		/// <param name="ptView">Point to convert</param>
		/// <returns>Converted point in world coordinates</returns>
		public PointF ViewToWorld(PointF ptView)
		{
			PointF[] aPts = { ptView };
			Matrix viewTransform = GetViewTransform();
			viewTransform.Invert();
			viewTransform.TransformPoints(aPts);
			return aPts[0];
		}

		/// <summary>
		/// Converts the given rectangle from view coordinates to world coordinates.
		/// </summary>
		/// <param name="rcView">Rectangle to convert</param>
		/// <returns>Converted rectangle in world coordinates</returns>
		public RectangleF ViewToWorld(RectangleF rcView)
		{
			PointF[] aPts = new PointF[2];
			aPts[0].X = rcView.Left;
			aPts[0].Y = rcView.Top;
			aPts[1].X = rcView.Right;
			aPts[1].Y = rcView.Bottom;
			Matrix viewTransform = GetViewTransform();
			viewTransform.Invert();
			viewTransform.TransformPoints(aPts);
			return new RectangleF(aPts[0].X, aPts[0].Y, aPts[1].X - aPts[0].X, aPts[1].Y - aPts[0].Y);
		}

		/// <summary>
		/// Converts the given size from view coordinates to world coordinates.
		/// </summary>
		/// <param name="szView">Size to convert</param>
		/// <returns>Converted size in world coordinates</returns>
		public SizeF ViewToWorld(SizeF szView)
		{
			PointF[] worldPts = new PointF[] { new PointF(szView.Width, szView.Height) };
			Matrix viewTransform = new Matrix();
			viewTransform.Scale((float) this.magnification.Width / 100.0f, (float) this.magnification.Height / 100.0f);
			viewTransform.Invert();
			viewTransform.TransformPoints(worldPts);
			return new SizeF(worldPts[0].X, worldPts[0].Y);
		}

		/// <summary>
		/// Converts the given array of points from view coordinates to world coordinates.
		/// </summary>
		/// <param name="viewPts">Points to convert</param>
		/// <param name="worldPts">Converted points in world coordinates</param>
		public void ViewToWorld(PointF[] viewPts, out PointF[] worldPts)
		{
			worldPts = (PointF[]) viewPts.Clone();
			Matrix viewTransform = this.GetViewTransform();
			viewTransform.Invert();
			viewTransform.TransformPoints(worldPts);
		}

		/// <summary>
		/// Converts the given rectangle from world coordinates to view coordinates.
		/// </summary>
		/// <param name="rcWorld">Rectangle to convert</param>
		/// <returns>Converted rectangle in view coordinates</returns>
		public RectangleF WorldToView(RectangleF rcWorld)
		{
			PointF[] aPts = new PointF[2];
			aPts[0].X = rcWorld.Left;
			aPts[0].Y = rcWorld.Top;
			aPts[1].X = rcWorld.Right;
			aPts[1].Y = rcWorld.Bottom;
			Matrix viewTransform = this.GetViewTransform();
			viewTransform.TransformPoints(aPts);
			return new RectangleF(aPts[0].X, aPts[0].Y, aPts[1].X - aPts[0].X, aPts[1].Y - aPts[0].Y);
		}

		/// <summary>
		/// Converts the given point from world coordinates to view coordinates.
		/// </summary>
		/// <param name="ptWorld">Point to convert</param>
		/// <returns>Converted point in view coordinates</returns>
		public PointF WorldToView(PointF ptWorld)
		{
			PointF[] aPts = { ptWorld };
			Matrix viewTransform = this.GetViewTransform();
			viewTransform.TransformPoints(aPts);
			return aPts[0];
		}

		/// <summary>
		/// Converts the given array of points from world coordinates to view coordinates.
		/// </summary>
		/// <param name="worldPts">Points to convert</param>
		/// <param name="viewPts">Converted points in view coordinates</param>
		public void WorldToView(PointF[] worldPts, out PointF[] viewPts)
		{
			viewPts = (PointF[]) worldPts.Clone();
			Matrix viewTransform = this.GetViewTransform();
			viewTransform.TransformPoints(viewPts);
		}

		/// <summary>
		/// Converts the give size from world coordinates to view coordinates.
		/// </summary>
		/// <param name="szWorld">Size to convert</param>
		/// <returns>Converted size in view coordinates</returns>
		public SizeF WorldToView(SizeF szWorld)
		{
			PointF[] viewPts = { new PointF(szWorld.Width, szWorld.Height) };
			Matrix viewTransform = new Matrix();
			viewTransform.Scale((float) this.magnification.Width / 100.0f, (float) this.magnification.Height / 100.0f);
			viewTransform.TransformPoints(viewPts);
			return new SizeF(viewPts[0].X, viewPts[0].Y);
		}

		/// <summary>
		/// Converts the given point from view coordinates to device coordinates.
		/// </summary>
		/// <param name="ptView">Point to convert</param>
		/// <returns>Converted point in device coordinates</returns>
		public System.Drawing.Point ViewToDevice(PointF ptView)
		{
			int devX;
			int devY;
			float unitsPerInch = 1.0f;
			float measurementScale = 1.0f;
			float dpuX = 1.0f;
			float dpuY = 1.0f;

			if (this.model != null)
			{
				if (this.model.MeasurementUnits != GraphicsUnit.Pixel)
				{
					unitsPerInch = Measurements.UnitsPerInch(this.model.MeasurementUnits);
					dpuX = this.dpiX / unitsPerInch;
					dpuY = this.dpiX / unitsPerInch;
				}
				measurementScale = this.model.MeasurementScale;
			}

			devX = (int) Math.Round(ptView.X * measurementScale * dpuX);
			devY = (int) Math.Round(ptView.Y * measurementScale * dpuY);

			return new Point(devX, devY);
		}

		/// <summary>
		/// Converts the give size from view coordinates to device coordinates.
		/// </summary>
		/// <param name="szView">Size to convert</param>
		/// <returns>Converted size in device coordinates</returns>
		public System.Drawing.Size ViewToDevice(SizeF szView)
		{
			int devWidth;
			int devHeight;
			float unitsPerInch = 1.0f;
			float measurementScale = 1.0f;
			float dpuX = 1.0f;
			float dpuY = 1.0f;

			if (this.model != null)
			{
				if (this.model.MeasurementUnits != GraphicsUnit.Pixel)
				{
					unitsPerInch = Measurements.UnitsPerInch(this.model.MeasurementUnits);
					dpuX = this.dpiX / unitsPerInch;
					dpuY = this.dpiX / unitsPerInch;
				}
				measurementScale = this.model.MeasurementScale;
			}

			devWidth = (int) (szView.Width * measurementScale * dpuX);
			devHeight = (int) (szView.Height * measurementScale * dpuY);

			return new System.Drawing.Size(devWidth, devHeight);
		}

		/// <summary>
		/// Converts the given point from view coordinates to device coordinates.
		/// </summary>
		/// <param name="ptView">Point to convert</param>
		/// <returns>Converted point in device coordinates</returns>
		public System.Drawing.PointF ViewToDeviceF(PointF ptView)
		{
			float devX;
			float devY;
			float unitsPerInch = 1.0f;
			float measurementScale = 1.0f;
			float dpuX = 1.0f;
			float dpuY = 1.0f;

			if (this.model != null)
			{
				if (this.model.MeasurementUnits != GraphicsUnit.Pixel)
				{
					unitsPerInch = Measurements.UnitsPerInch(this.model.MeasurementUnits);
					dpuX = this.dpiX / unitsPerInch;
					dpuY = this.dpiX / unitsPerInch;
				}
				measurementScale = this.model.MeasurementScale;
			}

			devX = (ptView.X * measurementScale * dpuX);
			devY = (ptView.Y * measurementScale * dpuY);

			return new PointF(devX, devY);
		}

		/// <summary>
		/// Converts the given rectangle from view coordinates to device coordinates.
		/// </summary>
		/// <param name="rcView">Rectangle to convert</param>
		/// <returns>Converted rectangle in device coordinates</returns>
		public System.Drawing.Rectangle ViewToDevice(RectangleF rcView)
		{
			PointF viewUpperLeft = new PointF(rcView.Left, rcView.Top);
			PointF viewBottomRight = new PointF(rcView.Right, rcView.Bottom);
			Point deviceUpperLeft = this.ViewToDevice(viewUpperLeft);
			Point deviceBottomRight = this.ViewToDevice(viewBottomRight);
			return new System.Drawing.Rectangle(deviceUpperLeft.X, deviceUpperLeft.Y, deviceBottomRight.X - deviceUpperLeft.X, deviceBottomRight.Y - deviceUpperLeft.Y);
		}

		/// <summary>
		/// Converts the given array of points from view coordinates to device coordinates.
		/// </summary>
		/// <param name="viewPts">Array of points to convert</param>
		/// <param name="devicePts">Converted points in device coordinates</param>
		public void ViewToDevice(PointF[] viewPts, out Point[] devicePts)
		{
			devicePts = new Point[viewPts.Length];
			for (int ptIdx = 0; ptIdx < viewPts.Length; ptIdx++)
			{
				devicePts[ptIdx] = this.ViewToDevice(viewPts[ptIdx]);
			}
		}

		/// <summary>
		/// Converts the given size from view coordinates to device coordinates.
		/// </summary>
		/// <param name="szView">Size to convert</param>
		/// <returns>Converted size in device coordinates</returns>
		public SizeF ViewToDeviceF(SizeF szView)
		{
			float devWidth;
			float devHeight;
			float unitsPerInch = 1.0f;
			float measurementScale = 1.0f;
			float dpuX = 1.0f;
			float dpuY = 1.0f;

			if (this.model != null)
			{
				if (this.model.MeasurementUnits != GraphicsUnit.Pixel)
				{
					unitsPerInch = Measurements.UnitsPerInch(this.model.MeasurementUnits);
					dpuX = this.dpiX / unitsPerInch;
					dpuY = this.dpiX / unitsPerInch;
				}
				measurementScale = this.model.MeasurementScale;
			}

			devWidth = (szView.Width * measurementScale * dpuX);
			devHeight = (szView.Height * measurementScale * dpuY);

			return new SizeF(devWidth, devHeight);
		}

		/// <summary>
		/// Converts the given rectangle from view coordinates to device coordinates.
		/// </summary>
		/// <param name="rcView">Rectangle to convert</param>
		/// <returns>Converted rectangle in device coordinates</returns>
		public RectangleF ViewToDeviceF(RectangleF rcView)
		{
			PointF viewUpperLeft = new PointF(rcView.Left, rcView.Top);
			PointF viewBottomRight = new PointF(rcView.Right, rcView.Bottom);
			PointF deviceUpperLeft = this.ViewToDeviceF(viewUpperLeft);
			PointF deviceBottomRight = this.ViewToDeviceF(viewBottomRight);
			return new System.Drawing.RectangleF(deviceUpperLeft.X, deviceUpperLeft.Y, deviceBottomRight.X - deviceUpperLeft.X, deviceBottomRight.Y - deviceUpperLeft.Y);
		}

		/// <summary>
		/// Converts the points in the given IPoints object from world coordinates
		/// to device coordinates.
		/// </summary>
		/// <param name="objPts">Object containing points</param>
		/// <returns>Array of device points</returns>
		/// <remarks>
		/// <para>
		/// Retrieves the points from the object using the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPoints.GetPoints"/>
		/// method and converts them from world coordinates to device
		/// coordinates.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public System.Drawing.Point[] GetDevicePoints(IPoints objPts)
		{
			System.Drawing.PointF[] viewPts = null;
			System.Drawing.Point[] devPts = null;
			System.Drawing.PointF[] worldPts = objPts.GetPoints();
			ITransform objXform = objPts as ITransform;
			if (objXform != null)
			{
				Matrix worldTransform = objXform.WorldTransform;
				worldTransform.TransformPoints(worldPts);
			}
			this.WorldToView(worldPts, out viewPts);
			this.ViewToDevice(viewPts, out devPts);
			return devPts;
		}

		#endregion

		#region Hit Testing

		/// <summary>
		/// Finds all of the nodes in the model that intersect the given point.
		/// </summary>
		/// <param name="nodes">Collection in which to add the children hit</param>
		/// <param name="ptScreen">Point to test in device coordinates</param>
		/// <returns>Number of nodes hit</returns>
		public int GetNodesAtPoint(NodeCollection nodes, Point ptScreen)
		{
			PointF ptWorld = ViewToWorld(DeviceToView(ptScreen));
			return this.model.GetChildrenAtPoint(nodes, ptWorld);
		}

		/// <summary>
		/// Finds all of the nodes in the model that intersect the given rectangle.
		/// </summary>
		/// <param name="nodes">Collection in which to add the children hit</param>
		/// <param name="rcScreen">Rectangle to test in device coordinates</param>
		/// <returns>Number of nodes hit</returns>
		public int GetNodesIntersecting(NodeCollection nodes, System.Drawing.Rectangle rcScreen)
		{
			RectangleF rcWorld = ViewToWorld(DeviceToView(rcScreen));
			return this.model.GetChildrenIntersecting(nodes, rcWorld);
		}

		/// <summary>
		/// Finds all of the nodes in the model contained by the given rectangle.
		/// </summary>
		/// <param name="nodes">Collection in which to add the children hit</param>
		/// <param name="rcScreen">Rectangle to test in device coordinates</param>
		/// <returns>Number of nodes hit</returns>
		public int GetNodesContainedBy(NodeCollection nodes, System.Drawing.Rectangle rcScreen)
		{
			RectangleF rcWorld = ViewToWorld(DeviceToView(rcScreen));
			return this.model.GetChildrenContainedBy(nodes, rcWorld);
		}

		/// <summary>
		/// Finds all of the nodes in the selection list that intersect the given
		/// point.
		/// </summary>
		/// <param name="nodes">Collection in which to add the children hit</param>
		/// <param name="ptScreen">Point to test in device coordinates</param>
		/// <returns>Number of nodes hit</returns>
		/// <remarks>
		/// <para>
		/// Only tests nodes that are currently in the selection list.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.SelectionList"/>
		/// </remarks>
		public int GetSelectedNodesAtPoint(NodeCollection nodes, Point ptScreen)
		{
			int numHit = 0;
			PointF ptWorld = ViewToWorld(DeviceToView(ptScreen));
			INode curNode;

			IEnumerator enumNodes = this.selectionList.GetEnumerator();
			while (enumNodes.MoveNext())
			{
				curNode = enumNodes.Current as INode;
				if (curNode != null)
				{
					ITransform curTransformNode = curNode as ITransform;

					if (curTransformNode != null)
					{
						Global.MatrixStack.Push(curTransformNode.ParentTransform);
					}

					IHitTestRegion rgnHitTest = curNode as IHitTestRegion;
					if (rgnHitTest != null)
					{
						if (rgnHitTest.ContainsPoint(ptWorld, 0))
						{
							nodes.Add(curNode);
							numHit++;
						}
					}
					else
					{
						IHitTestBounds boundsHitTest = curNode as IHitTestBounds;
						if (boundsHitTest != null)
						{
							if (boundsHitTest.ContainsPoint(ptWorld, 0))
							{
								nodes.Add(curNode);
								numHit++;
							}
						}
					}

					Global.MatrixStack.Clear();
				}
			}

			return numHit;
		}

		/// <summary>
		/// Looks for a resize handle at the given point.
		/// </summary>
		/// <param name="ptScreen">Point to test in device coordinates</param>
		/// <param name="handlePos">Position of the resize handle hit</param>
		/// <returns>
		/// Reference to the node that contains the handle or null if no resize
		/// handle was found at the given point
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method returns null if a resize handle is not found at the
		/// specified point. If a resize handle is found, then it returns the
		/// node that owns the resize handle and the position of the resize
		/// handle is returned in the handlePos parameter.
		/// </para>
		/// </remarks>
		public INode GetResizeHandleAtPoint(Point ptScreen, ref BoxPosition handlePos)
		{
			INode nodeHit = null;
			INode curNode = null;
			IBounds2DF curNodeBounds = null;
			RectangleF rcBounds;
			System.Drawing.Rectangle curHandleRect;

			if (this.selectionList != null)
			{
				PointF ptWorld = this.ViewToWorld(this.DeviceToView(ptScreen));
				IEnumerator enumSelection = this.selectionList.GetEnumerator();

				while (enumSelection.MoveNext() && nodeHit == null)
				{
					curNode = (INode) enumSelection.Current;

					object propValue = null;
					ResizeHandleStyle handleStyle = ResizeHandleStyle.OverlapBounds;
					bool allowResize = true;

					IPropertyContainer propContainer = curNode as IPropertyContainer;
					if (propContainer != null)
					{
						propValue = propContainer.GetPropertyValue("ResizeHandleStyle");
						if (propValue != null)
						{
							handleStyle = (ResizeHandleStyle) propValue;
						}
						propValue = propContainer.GetPropertyValue("AllowResize");
						if (propValue != null)
						{
							allowResize = (bool) propValue;
						}
					}

					if (allowResize)
					{
						curNodeBounds = curNode as IBounds2DF;
						if (curNodeBounds != null)
						{
							rcBounds = curNodeBounds.Bounds;
							System.Drawing.Rectangle rcBoundsDev = this.ViewToDevice(this.WorldToView(rcBounds));

							if (handleStyle == ResizeHandleStyle.OutsideBounds)
							{
								// Inflate rectangle
								int handleOffset = (int) ((float) this.handleSize / 1.5f);
								rcBoundsDev.Inflate(handleOffset, handleOffset);
							}

							// Test each handle
							System.Array positions = Enum.GetValues(typeof(BoxPosition));
							foreach (BoxPosition curPos in positions)
							{
								if (curPos != BoxPosition.Center)
								{
									curHandleRect = CalcHandleRect(rcBoundsDev, curPos);
									if (curHandleRect.Contains(ptScreen))
									{
										nodeHit = curNode;
										handlePos = curPos;
									}
								}
							}
						}
					}
				}
			}

			return nodeHit;
		}

		/// <summary>
		/// Looks for a vertex handle at the given point.
		/// </summary>
		/// <param name="ptScreen">Point to test in device coordinates</param>
		/// <param name="vertexIdx">Index position of the vertex hit</param>
		/// <returns>
		/// Reference to the node that contains the vertex or null if no vertex
		/// handle was found at the given point
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method returns null if a vertex handle is not found at the
		/// specified point. If a vertex handle is found, then it returns the
		/// node that owns the handle and the index position of the vertex
		/// is returned in the vertexIdx parameter.
		/// </para>
		/// </remarks>
		public INode GetVertexHandleAtPoint(Point ptScreen, ref int vertexIdx)
		{
			INode nodeHit = null;
			INode curNode = null;
			object service = null;
			IServiceProvider curSvcProvider = null;
			IPoints curNodePoints = null;
			PointF[] curPts = null;
			System.Drawing.Rectangle curHandleRect;
			ITransform curNodeXform = null;
			int curPointIdx;

			if (this.selectionList != null)
			{
				IEnumerator enumSelection = this.selectionList.GetEnumerator();

				while (enumSelection.MoveNext() && nodeHit == null)
				{
					curNode = (INode) enumSelection.Current;

					bool allowVertexEdit = false;
					IPropertyContainer propContainer = curNode as IPropertyContainer;
					if (propContainer != null)
					{
						object propValue = propContainer.GetPropertyValue("AllowVertexEdit");
						if (propValue != null)
						{
							allowVertexEdit = (bool) propValue;
						}
					}

					if (allowVertexEdit)
					{
						curSvcProvider = curNode as IServiceProvider;
						if (curSvcProvider != null)
						{
							service = curSvcProvider.GetService(typeof(IPoints));
							if (service != null)
							{
								curNodePoints = service as IPoints;
							}

							if (curNodePoints != null)
							{
								curPts = curNodePoints.GetPoints();
								curPointIdx = 0;

								if (curPts != null && curPts.Length > 0)
								{
									curNodeXform = null;
									service = curSvcProvider.GetService(typeof(ITransform));
									if (service != null)
									{
										curNodeXform = service as ITransform;
									}

									if (curNodeXform != null)
									{
										curNodeXform.WorldTransform.TransformPoints(curPts);
									}

									while (curPointIdx < curPts.Length && nodeHit == null)
									{
										PointF ptWorld = curPts[curPointIdx];
										System.Drawing.Point ptDev = this.ViewToDevice(this.WorldToView(ptWorld));
										curHandleRect = this.CalcHandleRect(ptDev);
										if (curHandleRect.Contains(ptScreen))
										{
											nodeHit = curNode;
											vertexIdx = curPointIdx;
										}
										curPointIdx++;
									}
								}
							}
						}
					}
				}
			}

			return nodeHit;
		}

		/// <summary>
		/// Looks for a port at the given point.
		/// </summary>
		/// <param name="ptScreen">Point to test in device coordinates</param>
		/// <param name="nSlop">Fudge factor for hit test</param>
		/// <returns>
		/// The port found at the given point or null if no port intersects the
		/// given point
		/// </returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		public Port GetPortAt(Point ptScreen, int nSlop)
		{
			PointF ptWorld = this.ViewToWorld(this.DeviceToView(ptScreen));
			SizeF szSlopWorld = this.ViewToWorld(this.DeviceToView(new System.Drawing.Size(nSlop,nSlop)));
			return this.model.GetPortAt(ptWorld, szSlopWorld.Width);
		}

		#endregion

		#region Tracking

		/// <summary>
		/// Draws a tracking outline a rectangle.
		/// </summary>
		/// <param name="rect">Position of rectangle to draw</param>
		public void DrawTrackingRect(System.Drawing.Rectangle rect)
		{
			if (this.parentControl != null)
			{
				IntPtr hWnd = this.parentControl.Handle;
				IntPtr hdc = Window.GetDCEx(hWnd, IntPtr.Zero, GDI.DCX_CACHE|GDI.DCX_LOCKWINDOWUPDATE|GDI.DCX_CLIPCHILDREN|GDI.DCX_CLIPSIBLINGS);
				GDI.SetROP2(hdc, GDI.R2_NOTXORPEN);
				IntPtr hPen = GDI.CreatePen(GDI.PS_DOT, 1, Macros.RGBToCOLORREF(this.trackingStyle.LineColor.ToArgb()));
				IntPtr hgdiobj = GDI.SelectObject(hdc, hPen);
				GDI.Rectangle(hdc, rect.Left, rect.Top, rect.Right, rect.Bottom);
				GDI.SelectObject(hdc, hgdiobj);
				GDI.DeleteObject(hPen);
				Window.ReleaseDC(hWnd, hdc);
			}
		}

		/// <summary>
		/// Draws a tracking outline a rectangle.
		/// </summary>
		/// <param name="pt1">First point in rectangle</param>
		/// <param name="pt2">Second point in rectangle</param>
		public void DrawTrackingRect(System.Drawing.Point pt1,  System.Drawing.Point pt2)
		{
			if (this.parentControl != null)
			{
				System.Drawing.Rectangle rect = Geometry.CreateRect(pt1, pt2);
				IntPtr hWnd = this.parentControl.Handle;
				IntPtr hdc = Window.GetDCEx(hWnd, IntPtr.Zero, GDI.DCX_CACHE|GDI.DCX_LOCKWINDOWUPDATE|GDI.DCX_CLIPCHILDREN|GDI.DCX_CLIPSIBLINGS);	//DCX_LOCKWINDOWUPDATE|DCX_CACHE
				GDI.SetROP2(hdc, GDI.R2_NOTXORPEN);
				IntPtr hPen = GDI.CreatePen(GDI.PS_DOT, 1, Macros.RGBToCOLORREF(this.trackingStyle.LineColor.ToArgb()));
				IntPtr hgdiobj = GDI.SelectObject(hdc, hPen);
				GDI.Rectangle(hdc, rect.Left, rect.Top, rect.Right, rect.Bottom);
				GDI.SelectObject(hdc, hgdiobj);
				GDI.DeleteObject(hPen);
				Window.ReleaseDC(hWnd, hdc);
			}
		}

		/// <summary>
		/// Draws a tracking outline for a rectangle at a given angle of rotation.
		/// </summary>
		/// <param name="rect">Rectangle to draw</param>
		/// <param name="angle">Angle to rotate the rectangle</param>
		public void DrawTrackingRect(System.Drawing.Rectangle rect, float angle)
		{
			if (this.parentControl != null)
			{
				Point centerPt = Geometry.CenterPoint(rect);

				Point[] rcPoints = new Point[5]
				{
					new Point(rect.Left, rect.Top),
					new Point(rect.Right, rect.Top),
					new Point(rect.Right, rect.Bottom),
					new Point(rect.Left, rect.Bottom),
					new Point(rect.Left, rect.Top)
				};

				Matrix transform = new Matrix();
				transform.Translate(-centerPt.X, -centerPt.Y, MatrixOrder.Append);
				transform.Rotate(angle, MatrixOrder.Append);
				transform.Translate(centerPt.X, centerPt.Y, MatrixOrder.Append);
				transform.TransformPoints(rcPoints);
				POINT[] pts = new POINT[5]
				{
					new POINT(rcPoints[0].X, rcPoints[0].Y),
					new POINT(rcPoints[1].X, rcPoints[1].Y),
					new POINT(rcPoints[2].X, rcPoints[2].Y),
					new POINT(rcPoints[3].X, rcPoints[3].Y),
					new POINT(rcPoints[0].X, rcPoints[0].Y),
				};

				IntPtr hWnd = this.parentControl.Handle;
				IntPtr hdc = Window.GetDCEx(hWnd, IntPtr.Zero, GDI.DCX_CACHE|GDI.DCX_LOCKWINDOWUPDATE|GDI.DCX_CLIPCHILDREN|GDI.DCX_CLIPSIBLINGS);
				GDI.SetROP2(hdc, GDI.R2_NOTXORPEN);
				IntPtr hPen = GDI.CreatePen(GDI.PS_DOT, 1, Macros.RGBToCOLORREF(this.trackingStyle.LineColor.ToArgb()));
				IntPtr hgdiobj = GDI.SelectObject(hdc, hPen);
				GDI.Polyline(hdc, pts, 5);
				GDI.SelectObject(hdc, hgdiobj);
				GDI.DeleteObject(hPen);
				Window.ReleaseDC(hWnd, hdc);
			}
		}

		/// <summary>
		/// Draws a tracking outline for a line.
		/// </summary>
		/// <param name="pt1">First point on the line</param>
		/// <param name="pt2">Second point on the line</param>
		public void DrawTrackingLine(System.Drawing.Point pt1, System.Drawing.Point pt2)
		{
			if (this.parentControl != null)
			{
				IntPtr hWnd = this.parentControl.Handle;
				IntPtr hdc = Window.GetDCEx(hWnd, IntPtr.Zero, GDI.DCX_CACHE|GDI.DCX_LOCKWINDOWUPDATE|GDI.DCX_CLIPCHILDREN|GDI.DCX_CLIPSIBLINGS);	//DCX_LOCKWINDOWUPDATE|DCX_CACHE
				GDI.SetROP2(hdc, GDI.R2_NOTXORPEN);
				IntPtr hPen = GDI.CreatePen(GDI.PS_DOT, 1, Macros.RGBToCOLORREF(this.trackingStyle.LineColor.ToArgb()));
				IntPtr hgdiobj = GDI.SelectObject(hdc, hPen);
				POINT curPt = new POINT(0,0);
				GDI.MoveToEx(hdc, pt1.X, pt1.Y, ref curPt);
				GDI.LineTo(hdc, pt2.X, pt2.Y);
				GDI.SelectObject(hdc, hgdiobj);
				GDI.DeleteObject(hPen);
				Window.ReleaseDC(hWnd, hdc);
			}
		}

		/// <summary>
		/// Draws a tracking outline for a polyline.
		/// </summary>
		/// <param name="trackingPts">Array of points in the polyline</param>
		public void DrawTrackingLines(System.Drawing.Point[] trackingPts)
		{
			if (this.parentControl != null)
			{
				IntPtr hWnd = this.parentControl.Handle;
				IntPtr hdc = Window.GetDCEx(hWnd, IntPtr.Zero, GDI.DCX_CACHE|GDI.DCX_LOCKWINDOWUPDATE|GDI.DCX_CLIPCHILDREN|GDI.DCX_CLIPSIBLINGS);	//DCX_LOCKWINDOWUPDATE|DCX_CACHE
				GDI.SetROP2(hdc, GDI.R2_NOTXORPEN);
				IntPtr hPen = GDI.CreatePen(GDI.PS_DOT, 1, Macros.RGBToCOLORREF(this.trackingStyle.LineColor.ToArgb()));
				IntPtr hgdiobj = GDI.SelectObject(hdc, hPen);
				int nPointCount = trackingPts.Length;
				POINT[] gdiPts = new POINT[nPointCount];
				for (int ptIdx = 0; ptIdx < nPointCount; ptIdx++)
				{
					gdiPts[ptIdx] = new POINT(trackingPts[ptIdx].X, trackingPts[ptIdx].Y);
				}
				GDI.Polyline(hdc, gdiPts, nPointCount);
				GDI.SelectObject(hdc, hgdiobj);
				GDI.DeleteObject(hPen);
				Window.ReleaseDC(hWnd, hdc);
			}
		}

		/// <summary>
		/// Draws a tracking outline for a polygon.
		/// </summary>
		/// <param name="trackingPts">Array of points that make up the polygon</param>
		public void DrawTrackingPolygon(System.Drawing.Point[] trackingPts)
		{
			if (this.parentControl != null)
			{
				Graphics grfx = this.parentControl.CreateGraphics();
				Pen penForeground = this.trackingStyle.CreatePen();
				grfx.DrawPolygon(penForeground, trackingPts);
				penForeground.Dispose();
				grfx.Dispose();
			}
		}

		/// <summary>
		/// Draws a tracking outline for a curve.
		/// </summary>
		/// <param name="trackingPts">Control points on the curve</param>
		public void DrawTrackingCurve(System.Drawing.Point[] trackingPts)
		{
			if (this.parentControl != null)
			{
				Graphics grfx = this.parentControl.CreateGraphics();
				Pen penForeground = this.trackingStyle.CreatePen();
				grfx.DrawCurve(penForeground, (System.Drawing.Point[]) trackingPts);
				penForeground.Dispose();
				grfx.Dispose();
			}
		}

		/// <summary>
		/// Draws a tracking outline for an arc.
		/// </summary>
		/// <param name="rcBounds">Bounds of the ellipse</param>
		/// <param name="startAngle">Start angle in the ellipse</param>
		/// <param name="sweepAngle">Sweep angle</param>
		public void DrawTrackingArc(System.Drawing.Rectangle rcBounds, float startAngle, float sweepAngle)
		{
			if (this.parentControl != null)
			{
				Graphics grfx = this.parentControl.CreateGraphics();

				Pen penForeground = this.trackingStyle.CreatePen();

				if (rcBounds.Width > 0 && rcBounds.Height > 0)
				{
					grfx.DrawArc(penForeground, rcBounds, startAngle, sweepAngle);
				}

				penForeground.Dispose();
				grfx.Dispose();
			}
		}

		/// <summary>
		/// Draws a tracking outline for a GraphicsPath at a given angle of rotation.
		/// </summary>
		/// <param name="grfxPath">GraphicsPath to draw</param>
		/// <param name="angle">Angle to rotate the object</param>
		/// <returns>
		/// Bounding rectangle in which the tracking was drawn, which can be passed to
		/// the <see cref="Syncfusion.Windows.Forms.Diagram.View.Refresh"/> method
		/// to erase the tracking.
		/// </returns>
		public System.Drawing.Rectangle DrawTrackingPath(System.Drawing.Drawing2D.GraphicsPath grfxPath, float angle)
		{
			System.Drawing.Rectangle rcClip = new System.Drawing.Rectangle(0,0,0,0);
			RectangleF pathBounds;
			PointF ptOrigin;
			Matrix pathTransform;

			if (grfxPath != null && this.parentControl != null)
			{
				Graphics grfx = this.parentControl.CreateGraphics();

				Pen penForeground = this.trackingStyle.CreatePen();

				pathBounds = grfxPath.GetBounds();
				ptOrigin = Geometry.CenterPoint(pathBounds);
				pathTransform = new Matrix();
				pathTransform.Translate(-ptOrigin.X, -ptOrigin.Y, MatrixOrder.Append);
				pathTransform.Rotate(angle, MatrixOrder.Append);
				pathTransform.Translate(ptOrigin.X, ptOrigin.Y, MatrixOrder.Append);
				pathTransform.Multiply(this.GetViewTransform(), MatrixOrder.Append);
				pathBounds = grfxPath.GetBounds(pathTransform, penForeground);
				rcClip = this.ViewToDevice(pathBounds);

				if (this.model != null)
				{
					grfx.PageUnit = this.model.MeasurementUnits;
					grfx.PageScale = this.model.MeasurementScale;
				}

				grfx.Transform = pathTransform;
				grfx.DrawPath(penForeground, grfxPath);

				penForeground.Dispose();
				grfx.Dispose();
			}

			return rcClip;
		}

		/// <summary>
		/// Draws a tracking outline for a GraphicsPath.
		/// </summary>
		/// <param name="grfxPath">GraphicsPath to draw</param>
		/// <returns>
		/// Bounding rectangle in which the tracking was drawn, which can be passed to
		/// the <see cref="Syncfusion.Windows.Forms.Diagram.View.Refresh"/> method
		/// to erase the tracking.
		/// </returns>
		public System.Drawing.Rectangle DrawTrackingPath(System.Drawing.Drawing2D.GraphicsPath grfxPath)
		{
			System.Drawing.Rectangle rcClip = new System.Drawing.Rectangle(0,0,0,0);
			RectangleF pathBounds;
			Matrix pathTransform;

			if (grfxPath != null && this.parentControl != null)
			{
				Graphics grfx = this.parentControl.CreateGraphics();

				Pen penForeground = this.trackingStyle.CreatePen();

				pathTransform = this.GetViewTransform();
				pathBounds = grfxPath.GetBounds(pathTransform, penForeground);
				rcClip = this.ViewToDevice(pathBounds);

				if (this.model != null)
				{
					grfx.PageUnit = this.model.MeasurementUnits;
					grfx.PageScale = this.model.MeasurementScale;
				}

				grfx.Transform = pathTransform;
				grfx.DrawPath(penForeground, grfxPath);

				penForeground.Dispose();
				grfx.Dispose();
			}

			return rcClip;
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
				return this.propertyContainer;
			}
			return null;
		}

		#endregion

		#region IBounds2D interface

		/// <summary>
		/// Location of the view in the parent control specified in device coordinates.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Point Location
		{
			get
			{
				return this.bounds.Location;
			}
			set
			{
				this.bounds.Location = value;
			}
		}

		/// <summary>
		/// Size of the view in device coordinates.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Size Size
		{
			get
			{
				return this.bounds.Size;
			}
			set
			{
				this.bounds.Size = value;
				CreateBackBuffer();
			}
		}

		/// <summary>
		/// X coordinate of the location.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual int X
		{
			get
			{
				return this.Location.X;
			}
		}

		/// <summary>
		/// Y coordinate of the location.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual int Y
		{
			get
			{
				return this.Location.Y;
			}
		}

		/// <summary>
		/// Width of the view in device coordinates.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual int Width
		{
			get
			{
				return this.Size.Width;
			}
		}

		/// <summary>
		/// Height of the view in device coordinates.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual int Height
		{
			get
			{
				return this.Size.Height;
			}
		}

		/// <summary>
		/// Bounds of the view in the parent control specified in device coordinates.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public System.Drawing.Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		#endregion

		#region Model Event Handlers

		/// <summary>
		/// Subscribes to events in the model.
		/// </summary>
		protected virtual void AttachModelEventHandlers()
		{
			if (this.model != null)
			{
				this.model.ChildrenChangeComplete += new NodeCollection.EventHandler(OnModelChildrenChangeComplete);
				this.model.PropertyChanged += new PropertyEventHandler(OnModelPropertyChanged);
				this.model.BoundsChanged += new BoundsEventHandler(OnModelBoundsChanged);
				this.model.Moved += new MoveEventHandler(OnModelNodeMoved);
				this.model.Rotated += new RotateEventHandler(OnModelNodeRotated);
				this.model.Scaled += new ScaleEventHandler(OnModelNodeScaled);
				this.model.InsertVertex += new VertexEventHandler(OnModelVertexEdit);
				this.model.MoveVertex += new VertexEventHandler(OnModelVertexEdit);
				this.model.DeleteVertex += new VertexEventHandler(OnModelVertexEdit);
				this.model.MeasurementUnitsChanging += new LogicalUnitsEventHandler(OnModelMeasurementUnitsChanging);
				this.model.MeasurementScaleChanging += new LogicalScaleEventHandler(OnModelMeasurementScaleChanging);
			}
		}

		/// <summary>
		/// Unsubscribes to events in the model.
		/// </summary>
		protected virtual void DetachModelEventHandlers()
		{
			if (this.model != null)
			{
				this.model.ChildrenChangeComplete -= new NodeCollection.EventHandler(OnModelChildrenChangeComplete);
				this.model.PropertyChanged -= new PropertyEventHandler(OnModelPropertyChanged);
				this.model.BoundsChanged -= new BoundsEventHandler(OnModelBoundsChanged);
				this.model.Moved -= new MoveEventHandler(OnModelNodeMoved);
				this.model.Rotated -= new RotateEventHandler(OnModelNodeRotated);
				this.model.Scaled -= new ScaleEventHandler(OnModelNodeScaled);
				this.model.InsertVertex -= new VertexEventHandler(OnModelVertexEdit);
				this.model.MoveVertex -= new VertexEventHandler(OnModelVertexEdit);
				this.model.DeleteVertex -= new VertexEventHandler(OnModelVertexEdit);
				this.model.MeasurementUnitsChanging -= new LogicalUnitsEventHandler(OnModelMeasurementUnitsChanging);
				this.model.MeasurementScaleChanging -= new LogicalScaleEventHandler(OnModelMeasurementScaleChanging);
			}
		}

		/// <summary>
		/// Called after changes are made to the heirarchy of nodes in the model.
		/// </summary>
		/// <param name="sender">The model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		protected void OnModelChildrenChangeComplete(object sender, NodeCollection.EventArgs evtArgs)
		{
			if (this.selectionList == null)
			{
				throw new EInvalidOperation();
			}

			if (evtArgs.ChangeType == CollectionEx.ChangeType.Insert)
			{
				this.selectionList.Clear();
				this.selectionList.Add(evtArgs.Node);
			}
			else if (evtArgs.ChangeType == CollectionEx.ChangeType.Remove)
			{
				this.selectionList.Clear();
			}

			this.Draw();
		}

		/// <summary>
		/// Called when a property of a node in the model changes.
		/// </summary>
		/// <param name="sender">The model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// </remarks>
		protected void OnModelPropertyChanged(object sender, PropertyEventArgs evtArgs)
		{
			this.Draw();
		}

		/// <summary>
		/// Called when the bounds of a node in the model changes.
		/// </summary>
		/// <param name="sender">The model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoundsEventArgs"/>
		/// </remarks>
		protected void OnModelBoundsChanged(object sender, BoundsEventArgs evtArgs)
		{
			this.Draw();
		}

		/// <summary>
		/// Called when a node in the model is moved.
		/// </summary>
		/// <param name="sender">The model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveEventArgs"/>
		/// </remarks>
		protected void OnModelNodeMoved(object sender, MoveEventArgs evtArgs)
		{
			this.Draw();
		}

		/// <summary>
		/// Called when a node in the model is rotated.
		/// </summary>
		/// <param name="sender">The model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RotateEventArgs"/>
		/// </remarks>
		protected void OnModelNodeRotated(object sender, RotateEventArgs evtArgs)
		{
			this.Draw();
		}

		/// <summary>
		/// Called when a node in the model is scaled.
		/// </summary>
		/// <param name="sender">The model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ScaleEventArgs"/>
		/// </remarks>
		protected void OnModelNodeScaled(object sender, ScaleEventArgs evtArgs)
		{
			this.Draw();
		}

		/// <summary>
		/// Called when a vertex in the model is modified.
		/// </summary>
		/// <param name="sender">The model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		protected void OnModelVertexEdit(object sender, VertexEventArgs evtArgs)
		{
			this.Draw();
		}

		/// <summary>
		/// Called before the logical unit of measure in the model is changed.
		/// </summary>
		/// <param name="sender">Model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Converts the origin and other values stored in world coordinates.
		/// </para>
		/// </remarks>
		protected virtual void OnModelMeasurementUnitsChanging(object sender, LogicalUnitsEventArgs evtArgs)
		{
			System.Drawing.Graphics grfx = null;
			if (this.parentControl != null)
			{
				grfx = this.parentControl.CreateGraphics();
			}

			GraphicsUnit fromUnits = evtArgs.OldUnits;
			GraphicsUnit toUnits = evtArgs.NewUnits;

			// Convert origin of the view
			System.Drawing.PointF viewOrigin = this.origin;
			viewOrigin = Measurements.Convert(fromUnits, toUnits, grfx, viewOrigin);
			this.origin = viewOrigin;

			// Convert grid spacing
			SizeF gridSpacing = (SizeF) this.propertyContainer.GetPropertyValue("GridSpacing");
			gridSpacing = Measurements.Convert(fromUnits, toUnits, grfx, gridSpacing);
			this.propertyContainer.SetPropertyValue("GridSpacing", gridSpacing);

			// Convert tracking style dash
			object objTrackingDashOffset = this.propertyContainer.GetPropertyValue("TrackingLineDashOffset");
			if (objTrackingDashOffset != null)
			{
				float trackingDashOffset = (float) objTrackingDashOffset;
				trackingDashOffset = Measurements.Convert(fromUnits, toUnits, grfx, trackingDashOffset);
				this.propertyContainer.SetPropertyValue("TrackingLineDashOffset", trackingDashOffset);
			}

			if (grfx != null)
			{
				grfx.Dispose();
			}
		}

		/// <summary>
		/// Called before the logical measurement scale in the model is changed.
		/// </summary>
		/// <param name="sender">Model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		protected void OnModelMeasurementScaleChanging(object sender, LogicalScaleEventArgs evtArgs)
		{
		}

		#endregion

		#region Property Event Handlers

		/// <summary>
		/// Event handler for change events in the property container.
		/// </summary>
		/// <param name="sender">Property container sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls OnPropertyChanged
		/// </remarks>
		private void PropertyContainer_PropertyChanged(object sender, PropertyEventArgs evtArgs)
		{
			this.OnPropertyChanged(evtArgs);
		}

		/// <summary>
		/// Called when a property in the view is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Fires the PropertyChanged event.
		/// </para>
		/// </remarks>
		protected virtual void OnPropertyChanged(PropertyEventArgs evtArgs)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, evtArgs);
			}

			this.Draw();
		}

		/// <summary>
		/// Called when the origin of the view changes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		protected virtual void OnOriginChanged(ViewOriginEventArgs evtArgs)
		{
			if (this.OriginChanged != null)
			{
				this.OriginChanged(this, evtArgs);
			}
		}

		/// <summary>
		/// Called when the magnification value of the view changes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		protected virtual void OnMagnificationChanged(ViewMagnificationEventArgs evtArgs)
		{
			if (this.MagnificationChanged != null)
			{
				this.MagnificationChanged(this, evtArgs);
			}
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("bounds", this.bounds);
			info.AddValue("propertyContainer", this.propertyContainer);
			info.AddValue("origin", this.origin);
			info.AddValue("backgroundColor", this.backgroundColor);
			info.AddValue("handleSize", this.handleSize);
			info.AddValue("handleColor", this.handleColor);
			info.AddValue("handleAnchorColor", this.handleAnchorColor);
			info.AddValue("magnification", this.magnification);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.grid = this.CreateGrid();
			this.trackingStyle = new TrackingStyle(this.propertyContainer);
		}

		#endregion

		#region Events

		/// <summary>
		/// Fired when a property in the view changes.
		/// </summary>
		public event PropertyEventHandler PropertyChanged;

		/// <summary>
		/// Fired when the <see cref="Syncfusion.Windows.Forms.Diagram.View.Origin"/>
		/// changes.
		/// </summary>
		public event ViewOriginEventHandler OriginChanged;

		/// <summary>
		/// Fired when the <see cref="Syncfusion.Windows.Forms.Diagram.View.Magnification"/>
		/// changes.
		/// </summary>
		public event ViewMagnificationEventHandler MagnificationChanged;

		#endregion

		#region Implementation

		/// <summary>
		/// Creates the back buffer.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.DrawingSurface"/>
		/// </remarks>
		protected void CreateBackBuffer()
		{
			if (this.backBuffer != null)
			{
				this.backBuffer.Dispose();
			}
			this.backBuffer = new Bitmap(this.Width, this.Height);
		}

		/// <summary>
		/// Sets the default property values for the view.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// view to their default values.
		/// </remarks>
		protected virtual void SetDefaultPropertyValues()
		{
			this.propertyContainer.SetPropertyValue("ShowPageBounds", true);
			this.propertyContainer.SetPropertyValue("GridType", TypeGrid.Point);
			this.propertyContainer.SetPropertyValue("GridVisible", true);
			this.propertyContainer.SetPropertyValue("SnapToGrid", true);
			this.propertyContainer.SetPropertyValue("GridSpacing", new SizeF(10.0f, 10.0f));
			this.propertyContainer.SetPropertyValue("GridColor", System.Drawing.Color.Black);
			this.propertyContainer.SetPropertyValue("GridDashStyle", System.Drawing.Drawing2D.DashStyle.Dash);
			this.propertyContainer.SetPropertyValue("TrackingLineColor", Color.Black);
			this.propertyContainer.SetPropertyValue("TrackingLineWidth", 0.0f);
			this.propertyContainer.SetPropertyValue("TrackingLineDashStyle", DashStyle.Dash);
			this.propertyContainer.SetPropertyValue("TrackingLineDashCap", DashCap.Flat);
			this.propertyContainer.SetPropertyValue("TrackingLineDashOffset", 4.0f);
		}

		/// <summary>
		/// Creates the layout grid that is rendered in the view.
		/// </summary>
		/// <returns>Layout grid to attach to grid</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="Syncfusion.Windows.Forms.Diagram.View.GridType"/>
		/// property is used to determine the type of grid to create. This
		/// method can be overridden in derived classes to implement new
		/// grid types.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.TypeGrid"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PointGrid"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineGrid"/>
		/// </remarks>
		protected virtual LayoutGrid CreateGrid()
		{
			LayoutGrid grid = null;

			switch (this.GridType)
			{
				case TypeGrid.Point:
					grid = new PointGrid(this);
					break;

				case TypeGrid.Line:
					grid = new LineGrid(this);
					break;
			}

			return grid;
		}

		#endregion

		#region Fields

		/// <summary>
		/// Reference to parent control (control hosting the view)
		/// </summary>
		private Control parentControl;

		/// <summary>
		/// Back buffer bitmap (drawing surface)
		/// </summary>
		private Bitmap backBuffer;

		/// <summary>
		/// Reference to the model attached to the view
		/// </summary>
		private Model model;

		/// <summary>
		/// Property container for the view's properties
		/// </summary>
		private PropertyContainer propertyContainer = null;

		/// <summary>
		/// Bounds of the view
		/// </summary>
		private System.Drawing.Rectangle bounds;

		/// <summary>
		/// Color to clear the background with
		/// </summary>
		private System.Drawing.Color backgroundColor;

		/// <summary>
		/// List of selected nodes
		/// </summary>
		private NodeCollection selectionList;

		/// <summary>
		/// Line style used to draw tracking outlines
		/// </summary>
		private TrackingStyle trackingStyle;

		/// <summary>
		/// Object that renders grid
		/// </summary>
		private LayoutGrid grid;

		/// <summary>
		/// View origin
		/// </summary>
		private PointF origin;

		/// <summary>
		/// Magnification value
		/// </summary>
		private Size magnification;

		/// <summary>
		/// Currently active cursor
		/// </summary>
		private Cursor cursor = null;

		/// <summary>
		/// Horizontal resolution of the device
		/// </summary>
		private float dpiX;

		/// <summary>
		/// Vertical resolution of the device
		/// </summary>
		private float dpiY;

		/// <summary>
		/// Size to draw selection handles (in device units)
		/// </summary>
		private int handleSize;

		/// <summary>
		/// Color to draw selection handles
		/// </summary>
		private System.Drawing.Color handleColor;

		/// <summary>
		/// Color to draw selection handles for anchor node
		/// </summary>
		private System.Drawing.Color handleAnchorColor;

		/// <summary>
		/// Page size of the default printer
		/// </summary>
		private System.Drawing.Size pageSize;

		/// <summary>
		/// Flag indicating if the page size is a known value
		/// </summary>
		private bool pageSizeKnown;

		/// <summary>
		/// Indicates if the Dispose() method has been called
		/// </summary>
		private bool disposed = false;

		#endregion

		#region Enumerations

		/// <summary>
		/// Specifies the type of grid to create.
		/// </summary>
		public enum TypeGrid
		{
			/// <summary>
			/// No grid
			/// </summary>
			None,

			/// <summary>
			/// Point grid
			/// </summary>
			Point,

			/// <summary>
			/// Line grid
			/// </summary>
			Line
		}

		#endregion
	}
}