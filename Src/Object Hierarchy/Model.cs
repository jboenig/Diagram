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
using System.Drawing.Printing;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A model is a collection of nodes that are rendered onto a view and
	/// manipulated by a controller.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A model is the data portion of a diagram. It is the root node in a hierarchy
	/// that is rendered onto a view. This class implements the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.ICompositeNode"/> interface,
	/// provides methods for accessing, adding, and removing child nodes. This
	/// includes the following methods:
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.ChildCount"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.GetChild"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.AppendChild"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.InsertChild"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.RemoveChild"/>. Child
	/// nodes in the model can also be accessed through the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.Nodes"/> property.
	/// </para>
	/// <para>
	/// A model maintains a collection of layers in the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.Layers"/>
	/// property. A <see cref="Syncfusion.Windows.Forms.Diagram.Layer"/> is a
	/// collection of nodes that share a common set of default properties and the
	/// same Z-order relative to other layers. A model always contains at least one
	/// layer. Each node in the model belongs to one and only one layer. The model
	/// renders itself onto the view by iterating through the layers and rendering
	/// each one. Each layer is responsible for rendering the nodes belonging to
	/// it. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LayerCollection.PropertyContainer"/>
	/// of the <see cref="Syncfusion.Windows.Forms.Diagram.Model.Layers"/>
	/// collection is a reference back to the model, which allows the layers to
	/// inherit properties from the model. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.DefaultLayer"/>
	/// property determines which layer a node is assigned to when it is added
	/// to the model.
	/// </para>
	/// <para>
	/// A model contains document-level settings such as
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.PageSettings"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.MeasurementUnits"/>, and
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.MeasurementScale"/>.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICompositeNode"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IBounds2DF"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDraw"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPrint"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
	/// </remarks>
	[Serializable]
	public class Model : Component, ICompositeNode, IBounds2DF, IDraw, ITransform, IPropertyContainer, ILogicalUnitContainer, IPrint, IDispatchNodeEvents, ISerializable, IDeserializationCallback
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Model()
		{
			this.name = "Model";

			float width = Measurements.Convert(GraphicsUnit.Inch, GraphicsUnit.Pixel, 96.0f, 8.5f);
			float height = Measurements.Convert(GraphicsUnit.Inch, GraphicsUnit.Pixel, 96.0f, 11f);
			this.bounds = new RectangleF(0, 0, width, height);

			this.boundaryConstraintsEnabled = true;
			this.rgnConstraint = new System.Drawing.Region(this.bounds);

			this.children = new NodeCollection();
			this.children.Changing += new NodeCollection.EventHandler(Children_Changing);
			this.children.ChangeComplete += new NodeCollection.EventHandler(Children_ChangeComplete);

			this.layers = new LayerCollection(this);
			this.defaultLayer = this.layers.Add("Default");
			this.nameTable = new Hashtable();
			this.eventsEnabled = false;
			this.measurementUnits = GraphicsUnit.Pixel;
			this.measurementScale = 1;
			this.matrix = new Matrix();
			this.isModified = false;
			this.propertyValues = new Hashtable();
			this.fillStyle = new FillStyle(this);
			this.lineStyle = new LineStyle(this);
			this.backgroundStyle = new BackgroundStyle(this);

			((IPropertyContainer)this).SetDefaultPropertyValues();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source model to copy</param>
		public Model(Model src)
		{
			this.name = src.name;
			this.bounds = src.bounds;

			this.boundaryConstraintsEnabled = src.boundaryConstraintsEnabled;
			this.rgnConstraint = (System.Drawing.Region) src.rgnConstraint.Clone();

			this.children = (NodeCollection) src.children.Clone();
			this.children.Changing += new NodeCollection.EventHandler(Children_Changing);
			this.children.ChangeComplete += new NodeCollection.EventHandler(Children_ChangeComplete);

			this.layers = (LayerCollection) src.layers.Clone();
			this.defaultLayer = this.layers[src.DefaultLayer.Name];

			this.nameTable = (Hashtable) src.nameTable.Clone();
			this.measurementUnits = src.measurementUnits;
			this.measurementScale = src.measurementScale;
			this.matrix = src.matrix.Clone();
			this.isModified = false;
			this.propertyValues = (Hashtable) src.propertyValues.Clone();
			this.fillStyle = new FillStyle(this);
			this.lineStyle = new LineStyle(this);
			this.backgroundStyle = new BackgroundStyle(this);
		}

		/// <summary>
		/// Serialization constructor for models.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Model(SerializationInfo info, StreamingContext context)
		{
			this.name = info.GetString("name");
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
			this.parent = (ICompositeNode) info.GetValue("parent", typeof(ICompositeNode));
			this.bounds = (RectangleF) info.GetValue("bounds", typeof(RectangleF));
			float m11 = info.GetSingle("m11");
			float m12 = info.GetSingle("m12");
			float m21 = info.GetSingle("m21");
			float m22 = info.GetSingle("m22");
			float dx = info.GetSingle("dx");
			float dy = info.GetSingle("dy");
			this.matrix = new Matrix(m11, m12, m21, m22, dx, dy);
			this.children = (NodeCollection) info.GetValue("children", typeof(NodeCollection));
			this.layers = (LayerCollection) info.GetValue("layers", typeof(LayerCollection));
			this.defaultLayer = (Layer) info.GetValue("defaultLayer", typeof(Layer));
			this.measurementUnits = (GraphicsUnit) info.GetValue("measurementUnits", typeof(GraphicsUnit));
			this.measurementScale = (float) info.GetValue("measurementScale", typeof(float));
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public virtual object Clone()
		{
			return new Model(this);
		}

		/// <summary>
		/// Called to release resources held by the model.
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
				}
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Collection of child nodes belonging to model.
		/// </summary>
		/// <remarks>
		/// Changes made to this collection will cause one or more of the following
		/// methods to be called:
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnChildrenChanging"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnChildrenChangeComplete"/>.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.INode"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual NodeCollection Nodes
		{
			get
			{
				return this.children;
			}
		}

		/// <summary>
		/// Collection of layers in the model.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A Layer is a collection of nodes that share a common set of default properties
		/// and the same Z-order relative to other layers. A model always contains at least
		/// one layer. Each node in the model belongs to one and only one layer. The model
		/// renders itself onto the view by iterating through this collection and rendering
		/// each layer. Each layer is responsible for rendering the nodes belonging to
		/// it. The
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LayerCollection.PropertyContainer"/>
		/// of this collection is a reference back to the model, which allows the layers to
		/// inherit properties from the model. The DefaultLayer property determines which
		/// layer a node is assigned to when it is added to the model.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LayerCollection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.DefaultLayer"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Layers"),
		Description("Collection of layers in the model")
		]
		public virtual LayerCollection Layers
		{
			get
			{
				return this.layers;
			}
		}

		/// <summary>
		/// Determines which layer nodes are assigned to when they are added to the model.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.Layers"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual Layer DefaultLayer
		{
			get
			{
				return this.defaultLayer;
			}
		}

		/// <summary>
		/// Name of the default layer.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.Layers"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.DefaultLayer"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Layers"),
		Description("Name of the default layer")
		]
		public virtual string DefaultLayerName
		{
			get
			{
				if (this.defaultLayer != null)
				{
					return this.defaultLayer.Name;
				}
				return null;
			}
			set
			{
				Layer layer = this.Layers[value];
				if (layer != null)
				{
					this.defaultLayer = layer;
				}
				else
				{
					throw new EInvalidParameter();
				}
			}
		}

		/// <summary>
		/// Flag indicating if the model has been modified.
		/// </summary>
		/// <remarks>
		/// This flag is set to true when any property of the model changes.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual bool Modified
		{
			get
			{
				return this.isModified;
			}
			set
			{
				this.isModified = value;
			}
		}

		/// <summary>
		/// Unit of measure used for world coordinates.
		/// </summary>
		[
		Browsable(true),
		Category("Logical Units")
		]
		public virtual GraphicsUnit MeasurementUnits
		{
			get
			{
				return this.measurementUnits;
			}
			set
			{
				if (value != GraphicsUnit.World)
				{
					if (this.measurementUnits != value)
					{
						GraphicsUnit oldVal = this.measurementUnits;
						this.OnMeasurementUnitsChanging(new LogicalUnitsEventArgs(oldVal, value));
						this.measurementUnits = value;
						this.OnPropertyChanged(new PropertyEventArgs(this, "MeasurementUnits", oldVal, this.measurementUnits));
					}
				}
			}
		}

		/// <summary>
		/// Scaling ratio between world coordinates and view coordinates.
		/// </summary>
		[
		Browsable(true),
		Category("Logical Units")
		]
		public virtual float MeasurementScale
		{
			get
			{
				return this.measurementScale;
			}
			set
			{
				if (this.measurementScale != value)
				{
					float oldVal = this.measurementScale;
					this.OnMeasurementScaleChanging(new LogicalScaleEventArgs(oldVal, value));
					this.measurementScale = value;
					this.OnPropertyChanged(new PropertyEventArgs(this, "MeasurementScale", oldVal, this.measurementScale));
				}
			}
		}

		/// <summary>
		/// Indicates if boundary constraints are enabled or not.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property is true by default. If this property is true, then all nodes
		/// in the model are constrained to the bounds of the model. In other words,
		/// child nodes cannot move, resize, or rotate to a position that leaves the
		/// bounds of the model.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.GetConstrainingRegion"/>
		/// </remarks>
		public bool BoundaryConstraintsEnabled
		{
			get
			{
				return this.boundaryConstraintsEnabled;
			}
			set
			{
				if (this.boundaryConstraintsEnabled != value)
				{
					this.boundaryConstraintsEnabled = value;

					if (this.rgnConstraint != null)
					{
						this.rgnConstraint.Dispose();
						this.rgnConstraint = null;
					}

					if (this.boundaryConstraintsEnabled)
					{
						this.rgnConstraint = new System.Drawing.Region(this.bounds);
					}
				}
			}
		}

		/// <summary>
		/// Determines whether the model fires events or not.
		/// </summary>
		/// <remarks>
		/// This property is useful for temporarily disabling events during
		/// time critical operations.
		/// </remarks>
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

		#region Styles

		/// <summary>
		/// Properties used to fill the interior of regions.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The fill style is used to create brushes for painting interior regions of
		/// shapes.
		/// </para>
		/// <para>
		/// The fill style properties in the model are inherited by child nodes. If a
		/// child node does not have a value assigned to a given fill style property,
		/// then the value assigned to the model is used.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.FillStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.LineStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.BackgroundStyle"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Appearance"),
		TypeConverter(typeof(FillStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual FillStyle FillStyle
		{
			get
			{
				return this.fillStyle;
			}
		}

		/// <summary>
		/// Properties used for drawing lines.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The line style is used to create pens for painting drawing lines.
		/// </para>
		/// <para>
		/// The line style properties in the model are inherited by child nodes. If a
		/// child node does not have a value assigned to a given line style property,
		/// then the value assigned to the model is used.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.FillStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.BackgroundStyle"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Appearance"),
		TypeConverter(typeof(LineStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual LineStyle LineStyle
		{
			get
			{
				return this.lineStyle;
			}
		}

		/// <summary>
		/// Properties for filling the background of the model.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The background style properties determine how the background of the
		/// model will be filled. The BackgroundStyle class is identical to the
		/// FillStyle class, with the except that the underlying property names
		/// used in the property container are different in order to avoid
		/// ambiguity. The FillStyle is used to provide default properties to
		/// child nodes. The BackgroundStyle is used by the model in its
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.Draw"/> method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BackgroundStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.FillStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.LineStyle"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Appearance"),
		TypeConverter(typeof(BackgroundStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual BackgroundStyle BackgroundStyle
		{
			get
			{
				return this.backgroundStyle;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Performs hit testing for ports.
		/// </summary>
		/// <param name="ptWorld">Point to hit test in world coordinates</param>
		/// <param name="fSlop">Amount to expand hit test point</param>
		/// <returns>
		/// Port that intersects the given point; null if no port is found at the point
		/// </returns>
		public Port GetPortAt(PointF ptWorld, float fSlop)
		{
			Port portHit = null;

			int prevStack = Global.SelectMatrixStack(Global.RenderingStack);
			Global.MatrixStack.Clear();
			Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);

			IEnumerator enumChildren = this.children.GetEnumerator();
			while (enumChildren.MoveNext() && portHit == null)
			{
				IPortContainer portContainer = enumChildren.Current as IPortContainer;
				if (portContainer != null)
				{
					portHit = portContainer.GetPortAt(ptWorld, fSlop);
				}
			}

			Global.MatrixStack.Pop();
			Global.SelectMatrixStack(prevStack);

			return portHit;
		}

		#endregion

		#region Printing

		/// <summary>
		/// Page settings to use when creating a print document for the model.
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
			set
			{
				this.pageSettings = value;
			}
		}

		/// <summary>
		/// Prints a page to the specified output device.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		void IPrint.PrintPage(PrintPageEventArgs evtArgs)
		{
			System.Drawing.Graphics grfx = evtArgs.Graphics;
			this.Draw(grfx);
		}

		#endregion

		#region IPropertyContainer interface

		/// <summary>
		/// Sets the default property values for the model.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// model to their default values.
		/// </remarks>
		public virtual void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("FillType", Syncfusion.Windows.Forms.Diagram.FillStyle.FillType.Solid);
			this.propertyValues.Add("FillColor", Color.LightGray);
			this.propertyValues.Add("GradientStartColor", Color.LightGray);
			this.propertyValues.Add("GradientBounds", new RectangleF(0,0,100,100));
			this.propertyValues.Add("GradientAngle", 90.0f);
			this.propertyValues.Add("FillTexture", null);
			this.propertyValues.Add("FillTextureWrapMode", System.Drawing.Drawing2D.WrapMode.Tile);
			this.propertyValues.Add("LineColor", Color.Black);
			this.propertyValues.Add("LineWidth", 2.0f);
			this.propertyValues.Add("LineEndCap", LineCap.Flat);
			this.propertyValues.Add("LineJoin", LineJoin.Bevel);
			this.propertyValues.Add("LineMiterLimit", 10.0f);
			this.propertyValues.Add("LineDashStyle", DashStyle.Solid);
			this.propertyValues.Add("LineDashCap", DashCap.Flat);
			this.propertyValues.Add("LineDashOffset", 0.0f);
			this.propertyValues.Add("AllowSelect", true);
			this.propertyValues.Add("AllowVertexEdit", false);
			this.propertyValues.Add("AllowMove", true);
			this.propertyValues.Add("AllowRotate", true);
			this.propertyValues.Add("AllowResize", true);
			this.propertyValues.Add("Visible", true);
			this.propertyValues.Add("Enabled", true);
			this.propertyValues.Add("BackgroundType", Syncfusion.Windows.Forms.Diagram.FillStyle.FillType.Solid);
			this.propertyValues.Add("BackgroundColor", Color.White);
			this.propertyValues.Add("RevealPorts", false);
			this.propertyValues.Add("LineHitTestPadding", 6.0f);
		}

		/// <summary>
		/// Retrieve the value of a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to retrieve</param>
		/// <returns>Value of the named property or null if it doesn't exist</returns>
		public virtual object GetPropertyValue(string propertyName)
		{
			if (propertyName == "Name")
			{
				return this.name;
			}
			else if (propertyName == "MeasurementUnits")
			{
				return this.measurementUnits;
			}
			else if (propertyName == "MeasurementScale")
			{
				return this.measurementScale;
			}

			if (this.propertyValues.Contains(propertyName))
			{
				return this.propertyValues[propertyName];
			}

			IPropertyContainer parentProps = this.parent as IPropertyContainer;
			if (parentProps != null)
			{
				return parentProps.GetPropertyValue(propertyName);
			}

			return null;
		}

		/// <summary>
		/// Assign a value to a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to set</param>
		/// <param name="val">Value to assign to property</param>
		/// <remarks>
		/// This method will add the property to the container if it doesn't
		/// already exist.
		/// </remarks>
		public virtual void SetPropertyValue(string propertyName, object val)
		{
			object oldVal = null;

			if (propertyName == "Name")
			{
				this.name = (string) val;
			}
			else if (propertyName == "MeasurementUnits")
			{
				oldVal = this.measurementUnits;
				((ILogicalUnitContainer) this).ConvertLogicalUnits((GraphicsUnit) oldVal, (GraphicsUnit) val, null);
				this.measurementUnits = (GraphicsUnit) val;
			}
			else if (propertyName == "MeasurementScale")
			{
				oldVal = this.measurementScale;
				((ILogicalUnitContainer) this).ConvertLogicalScale((float) oldVal, (float) val);
				this.measurementScale = (float) val;
			}
			else
			{
				if (this.propertyValues.ContainsKey(propertyName))
				{
					oldVal = this.propertyValues[propertyName];
					this.propertyValues[propertyName] = val;
				}
				else
				{
					this.propertyValues.Add(propertyName, val);
				}
			}

			this.OnPropertyChanged(new PropertyEventArgs(this, propertyName, oldVal, val));
			this.Modified = true;
		}

		/// <summary>
		/// Assign a value to a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to change</param>
		/// <param name="val">Value to assign to property</param>
		/// <remarks>
		/// This method only modifies property values that already exist
		/// in the container. If the property does not exist, this method fails.
		/// </remarks>
		public virtual void ChangePropertyValue(string propertyName, object val)
		{
			if (propertyName == "Name")
			{
				this.name = (string) val;
			}
			else if (propertyName == "MeasurementUnits")
			{
				this.measurementUnits = (GraphicsUnit) val;
			}
			else if (propertyName == "MeasurementScale")
			{
				this.measurementScale = (float) val;
			}

			this.propertyValues[propertyName] = val;

			this.Modified = true;
		}

		/// <summary>
		/// Removes the specified property.
		/// </summary>
		/// <param name="propertyName">Name of property to remove</param>
		public virtual void RemoveProperty(string propertyName)
		{
			if (this.propertyValues.ContainsKey(propertyName))
			{
				this.propertyValues.Remove(propertyName);
				this.Modified = true;
			}
		}

		/// <summary>
		/// Returns an array containing the names of all properties in the container.
		/// </summary>
		/// <returns>String array containing property names</returns>
		public virtual string[] GetPropertyNames()
		{
			string[] propertyNames = new string[this.propertyValues.Keys.Count];
			this.propertyValues.Keys.CopyTo(propertyNames, 0);
			return propertyNames;
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// </para>
		/// </remarks>
		object IServiceProvider.GetService(System.Type svcType)
		{
			if (svcType == typeof(IDispatchNodeEvents))
			{
				return this;
			}
			else if (svcType == typeof(IPropertyContainer))
			{
				return this;
			}
			else if (svcType == typeof(ILogicalUnitContainer))
			{
				return this;
			}
			return null;
		}

		#endregion

		#region ICompositeNode interface

		/// <summary>
		/// Reference to the composite node this node is a child of.
		/// </summary>
		/// <remarks>
		/// If the model is the root node in the hierarchy, this property will
		/// be null. The model may also belong to another composite node, in which
		/// case the property will reference that composite node.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual ICompositeNode Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		/// <summary>
		/// The root node in the node hieararchy.
		/// </summary>
		/// <remarks>
		/// The root node is found by following the chain of parent nodes until
		/// a node is found that has a null parent.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual INode Root
		{
			get
			{
				if (this.parent == null)
				{
					return this;
				}
				return this.parent.Root;
			}
		}


		/// <summary>
		/// Name of the model.
		/// </summary>
		/// <remarks>
		/// Must be unique within the scope of the parent node.
		/// </remarks>
		[
		Browsable(true),
		Category("General"),
		Description("Name of the model")
		]
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					object oldName = this.name;

					this.name = value;

					this.OnPropertyChanged(new PropertyEventArgs(this, "Name", oldName, this.name));
				}
			}
		}

		/// <summary>
		/// Fully qualified name of the model.
		/// </summary>
		/// <remarks>
		/// The full name is the name of the node concatenated with the names
		/// of all parent nodes.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual string FullName
		{
			get
			{
				if (this.parent == null)
				{
					return this.name;
				}
				return this.parent.FullName + "." + this.name;
			}
		}

		/// <summary>
		/// The number of child nodes contained by this model.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual int ChildCount
		{
			get
			{
				return this.children.Count;
			}
		}

		/// <summary>
		/// Returns the child node at the given index position.
		/// </summary>
		/// <param name="childIndex">Zero-based index into the collection of child nodes</param>
		/// <returns>Child node at the given position or null if the index is out of range</returns>
		public virtual INode GetChild(int childIndex)
		{
			return this.children[childIndex];
		}

		/// <summary>
		/// Returns the child node matching the given name.
		/// </summary>
		/// <param name="childName">Name of node to return</param>
		/// <returns>Node matching the given name</returns>
		public virtual INode GetChildByName(string childName)
		{
			if (this.nameTable.Contains(childName))
			{
				return (INode) this.nameTable[childName];
			}
			return null;
		}

		/// <summary>
		/// Returns the index position of the given child node.
		/// </summary>
		/// <param name="child">Child node to query</param>
		/// <returns>Zero-based index into the collection of child nodes</returns>
		public virtual int GetChildIndex(INode child)
		{
			return this.children.Find(child);
		}

		/// <summary>
		/// Appends the given node to the model.
		/// </summary>
		/// <param name="child">Node to append</param>
		/// <returns>
		/// Zero-based index at which the node was added to the collection or -1 for failure.
		/// </returns>
		public virtual int AppendChild(INode child)
		{
			int childIdx = -1;
			if (!this.children.Contains(child))
			{
				childIdx = this.children.Add(child);
			}
			return childIdx;
		}

		/// <summary>
		/// Insert the given node into the model at a specific position.
		/// </summary>
		/// <param name="child">Node to insert</param>
		/// <param name="childIndex">Zero-based index at which to insert the node</param>
		public virtual void InsertChild(INode child, int childIndex)
		{
			this.children.Insert(childIndex, child);
		}

		/// <summary>
		/// Removes the child node at the given position.
		/// </summary>
		/// <param name="childIndex">Zero-based index into the collection of child nodes</param>
		public virtual void RemoveChild(int childIndex)
		{
			this.children.RemoveAt(childIndex);
		}

		/// <summary>
		/// Removes all child nodes from the node.
		/// </summary>
		public virtual void RemoveAllChildren()
		{
			this.children.Clear();
		}

		/// <summary>
		/// Returns the region that the bounds of the given child node is constrained by.
		/// </summary>
		/// <param name="child">Child to get constraining region for</param>
		/// <returns>Region that constrains the bounds of the given child</returns>
		/// <remarks>
		/// <para>
		/// This method is used to limit the bounds of a child node to a specified area.
		/// The node cannot be moved, resized, or rotated beyond the edges of this region.
		/// </para>
		/// <para>
		/// If the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.BoundaryConstraintsEnabled"/>
		/// property is set to true, this method returns a region that matches the bounds
		/// of the model. If BoundaryConstraintsEnabled is set to false, then this
		/// method returns null. The child parameter is basically ignored, since the
		/// implementation is the same for all child nodes. Derived model classes can
		/// take advantage of the child parameter to set constraints on a node by node
		/// basis.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model.BoundaryConstraintsEnabled"/>
		/// </remarks>
		public virtual System.Drawing.Region GetConstrainingRegion(INode child)
		{
			return this.rgnConstraint;
		}

		/// <summary>
		/// Returns all children that are intersected by the given point.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children hit by the given point
		/// </param>
		/// <param name="ptWorld">Point to test</param>
		/// <returns>The number of child nodes that intersect the given point</returns>
		public virtual int GetChildrenAtPoint(NodeCollection childNodes, PointF ptWorld)
		{
			int numFound = 0;

			Global.MatrixStack.Push(this.matrix);

			foreach (INode curChild in this.children)
			{
				if (curChild != null)
				{
					IHitTestRegion rgnHitTest = curChild as IHitTestRegion;
					if (rgnHitTest != null)
					{
						if (rgnHitTest.ContainsPoint(ptWorld, 0))
						{
							if (childNodes != null)
							{
								childNodes.Insert(0, curChild);
							}
							numFound++;
						}
					}
					else
					{
						IHitTestBounds boundsHitTest = curChild as IHitTestBounds;
						if (boundsHitTest != null)
						{
							if (boundsHitTest.ContainsPoint(ptWorld, 0))
							{
								if (childNodes != null)
								{
									childNodes.Insert(0, curChild);
								}
								numFound++;
							}
						}
					}
				}
			}

			Global.MatrixStack.Pop();

			return numFound;
		}

		/// <summary>
		/// Returns all children that intersect the given rectangle.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children hit by the given point
		/// </param>
		/// <param name="rcWorld">Rectangle to test</param>
		/// <returns>The number of child nodes that intersect the given rectangle</returns>
		public virtual int GetChildrenIntersecting(NodeCollection childNodes, RectangleF rcWorld)
		{
			int numFound = 0;

			Global.MatrixStack.Push(this.matrix);

			foreach (INode curNode in this.children)
			{
				IHitTestBounds hitTestObj = curNode as IHitTestBounds;
				if (hitTestObj != null)
				{
					if (hitTestObj.IntersectsRect(rcWorld))
					{
						childNodes.Insert(0, curNode);
						numFound++;
					}
				}
			}

			Global.MatrixStack.Pop();

			return numFound;
		}

		/// <summary>
		/// Returns all children inside the given rectangle.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children inside the specified rectangle
		/// </param>
		/// <param name="rcWorld">Rectangle to test</param>
		/// <returns>The number of child nodes added to the collection</returns>
		public virtual int GetChildrenContainedBy(NodeCollection childNodes, RectangleF rcWorld)
		{
			int numFound = 0;

			Global.MatrixStack.Push(this.matrix);

			foreach (INode curNode in this.children)
			{
				IHitTestBounds hitTestObj = curNode as IHitTestBounds;
				if (hitTestObj != null)
				{
					if (hitTestObj.ContainedByRect(rcWorld))
					{
						childNodes.Add(curNode);
						numFound++;
					}
				}
			}
			
			Global.MatrixStack.Pop();

			return numFound;
		}

		/// <summary>
		/// Returns the inherited property container for the given child node.
		/// </summary>
		/// <param name="childNode">The child node making the request</param>
		/// <returns>Parent property container for the given node</returns>
		public virtual IPropertyContainer GetPropertyContainer(INode childNode)
		{
			IPropertyContainer propContainer = null;

			Layer nodeLayer = this.layers.FindNodeLayer(childNode);
			if (nodeLayer != null)
			{
				propContainer = nodeLayer as IPropertyContainer;
			}
			else
			{
				propContainer = this;
			}

			return propContainer;
		}

		#endregion

		#region IBounds2DF interface

		/// <summary>
		/// The model's bounding box.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public RectangleF Bounds
		{
			get
			{
				return this.bounds;
			}
			set
			{
				RectangleF oldBounds = this.bounds;
				this.bounds = value;

				// Destroy constraining region
				if (this.rgnConstraint != null)
				{
					this.rgnConstraint.Dispose();
					this.rgnConstraint = null;
				}

				if (this.boundaryConstraintsEnabled)
				{
					this.rgnConstraint = new System.Drawing.Region(this.bounds);
				}

				this.OnBoundsChanged(new BoundsEventArgs(this, oldBounds, this.bounds));
			}
		}

		/// <summary>
		/// X-coordinate of the model's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual float X
		{
			get
			{
				return this.Bounds.Location.X;
			}
			set
			{
			}
		}

		/// <summary>
		/// Y-coordinate of the model's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual float Y
		{
			get
			{
				return this.Bounds.Location.Y;
			}
			set
			{
			}
		}

		/// <summary>
		/// Width of the model's bounding box.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		Category("Bounds"),
		Description("Width of the model in logical units")
		]
		public virtual float Width
		{
			get
			{
				return this.Bounds.Size.Width;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Size = new SizeF(value, rcBounds.Height);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Height of the model's bounding box.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		Category("Bounds"),
		Description("Height of the model in logical units")
		]
		public virtual float Height
		{
			get
			{
				return this.Bounds.Size.Height;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Size = new SizeF(rcBounds.Width, value);
				this.Bounds = rcBounds;
			}
		}

		#endregion

		#region IDraw interface

		/// <summary>
		/// Renders the model onto the given System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Graphics context object to render onto</param>
		/// <remarks>
		/// <para>
		/// The model initializes the given System.Drawing.Graphics object with
		/// the its
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.LocalTransform"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.MeasurementUnits"/>, and
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.MeasurementScale"/>.
		/// Then it iterates through each layer in its
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.Layers"/>
		/// collections and draws each one.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDraw"/>
		/// </remarks>
		public void Draw(System.Drawing.Graphics grfx)
		{
			Global.SelectMatrixStack(Global.RenderingStack);

			GraphicsState grfxState = grfx.Save();

			grfx.MultiplyTransform(Global.MatrixStack.Push(this.matrix), MatrixOrder.Prepend);

			grfx.PageUnit = this.measurementUnits;
			grfx.PageScale = this.measurementScale;

			foreach (Layer layer in this.layers)
			{
				layer.Draw(grfx);
			}

			grfx.Transform = Global.MatrixStack.Pop();
			grfx.Restore(grfxState);
		}

		#endregion

		#region ITransform interface

		/// <summary>
		/// Moves the model by the given X and Y offsets.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		/// <remarks>
		/// Applies a translate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnMove"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Translate(float dx, float dy)
		{
			if (dx != 0.0f || dy != 0.0f)
			{
				this.matrix.Translate(dx, dy, MatrixOrder.Append);
				this.OnMove(new MoveEventArgs(this, dx, dy));
			}
		}

		/// <summary>
		/// Rotates the model a specified number of degrees about a given
		/// anchor point.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to rotate</param>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnRotate"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Rotate(PointF ptAnchor, float degrees)
		{
			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Rotate(degrees, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);
			this.OnRotate(new RotateEventArgs(this));
		}

		/// <summary>
		/// Rotates the model a specified number of degrees about its center point.
		/// </summary>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnRotate"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Rotate(float degrees)
		{
			PointF ptOrigin = Geometry.CenterPoint(this.Bounds);
			this.matrix.Translate(-ptOrigin.X, -ptOrigin.Y, MatrixOrder.Append);
			this.matrix.Rotate(degrees, MatrixOrder.Append);
			this.matrix.Translate(ptOrigin.X, ptOrigin.Y, MatrixOrder.Append);
			this.OnRotate(new RotateEventArgs(this));
		}

		/// <summary>
		/// Scales the model by a given ratio along the X and Y axes.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to scale</param>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnScale"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Scale(PointF ptAnchor, float sx, float sy)
		{
			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Scale(sx, sy, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);
			this.OnScale(new ScaleEventArgs(this));
		}

		/// <summary>
		/// Scales the model about its center point by a given ratio.
		/// </summary>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnScale"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Scale(float sx, float sy)
		{
			PointF ptAnchor = Geometry.CenterPoint(this.Bounds);
			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Scale(sx, sy, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);
			this.OnScale(new ScaleEventArgs(this));
		}

		/// <summary>
		/// Matrix containing local transformations for this node.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Matrix LocalTransform
		{
			get
			{
				return this.matrix;
			}
		}

		/// <summary>
		/// Returns a matrix containing transformations for this node and all of
		/// its ancestors.
		/// </summary>
		/// <remarks>
		/// Chains up the node hierarchy and builds a transformation matrix containing
		/// all transformations that apply to this node in the world coordinate space.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Matrix WorldTransform
		{
			get
			{
				Matrix worldTransform = new Matrix();

				if (this.parent != null)
				{
					ITransform objTransformParent = this.parent as ITransform;
					if (objTransformParent != null)
					{
						worldTransform.Multiply(objTransformParent.WorldTransform, MatrixOrder.Append);
					}
				}

				worldTransform.Multiply(this.matrix, MatrixOrder.Append);

				return worldTransform;
			}
		}

		/// <summary>
		/// Returns a matrix containing the transformations of this node's parent.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Matrix ParentTransform
		{
			get
			{
				Matrix parentTransform = null;

				if (this.parent != null)
				{
					ITransform objTransformParent = this.parent as ITransform;
					if (objTransformParent != null)
					{
						parentTransform = objTransformParent.WorldTransform;
					}
				}

				if (parentTransform == null)
				{
					parentTransform = new Matrix();
				}

				return parentTransform;
			}
		}

		#endregion

		#region ILogicalUnitContainer interface

		/// <summary>
		/// Converts the logical values contained by the object from one unit of
		/// measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="grfx">Graphics context object for converting device units</param>
		/// <remarks>
		/// <para>
		/// This method converts all logical unit values contained by the object from
		/// one unit of measure to another.
		/// </para>
		/// </remarks>
		void ILogicalUnitContainer.ConvertLogicalUnits(GraphicsUnit fromUnits, GraphicsUnit toUnits, Graphics grfx)
		{
			// Convert bounds of the model
			System.Drawing.RectangleF modelBounds = this.bounds;
			modelBounds = Measurements.Convert(fromUnits, toUnits, grfx, modelBounds);
			this.bounds = modelBounds;

			// Convert line width
			if (this.propertyValues.Contains("LineWidth"))
			{
				float lineWidth = (float) this.propertyValues["LineWidth"];
				lineWidth = Measurements.Convert(fromUnits, toUnits, grfx, lineWidth);
				this.propertyValues["LineWidth"] = lineWidth;
			}

			// Convert the gradient bounds
			if (this.propertyValues.Contains("GradientBounds"))
			{
				System.Drawing.RectangleF gradientBounds = (System.Drawing.RectangleF) this.propertyValues["GradientBounds"];
				gradientBounds = Measurements.Convert(fromUnits, toUnits, grfx, gradientBounds);
				this.propertyValues["GradientBounds"] = gradientBounds;
			}

			// Convert line hit test padding
			if (this.propertyValues.Contains("LineHitTestPadding"))
			{
				float lineHitTestPadding = (float) this.GetPropertyValue("LineHitTestPadding");
				lineHitTestPadding = Measurements.Convert(fromUnits, toUnits, grfx, lineHitTestPadding);
				this.SetPropertyValue("LineHitTestPadding", lineHitTestPadding);
			}

			// Iterate through children and convert them
			foreach (INode curChild in this.children)
			{
				ILogicalUnitContainer logUnitContainer = curChild as ILogicalUnitContainer;
				if (logUnitContainer != null)
				{
					logUnitContainer.ConvertLogicalUnits(fromUnits, toUnits, grfx);
				}
			}
		}

		/// <summary>
		/// Converts the logical values contained by the object from one scale to
		/// another.
		/// </summary>
		/// <param name="fromScale">Scale to convert from</param>
		/// <param name="toScale">Scale to convert to</param>
		/// <remarks>
		/// <para>
		/// This method scales all logical unit values contained by the object.
		/// </para>
		/// </remarks>
		void ILogicalUnitContainer.ConvertLogicalScale(float fromScale, float toScale)
		{
		}

		#endregion

		#region Event Callbacks

		/// <summary>
		/// Called before a change is made to the collection of child nodes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.ChildrenChanging"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnChildrenChanging(NodeCollection.EventArgs evtArgs)
		{
			if (this.eventsEnabled && this.ChildrenChanging != null)
			{
				this.ChildrenChanging(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.ChildrenChanging(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called after a change is made to the collection of child nodes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.ChildrenChangeComplete"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnChildrenChangeComplete(NodeCollection.EventArgs evtArgs)
		{
			if (this.eventsEnabled && this.ChildrenChangeComplete != null)
			{
				this.ChildrenChangeComplete(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.ChildrenChangeComplete(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a property is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.PropertyChanged"/>
		/// event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// </remarks>
		protected virtual void OnPropertyChanged(PropertyEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.PropertyChanged != null)
			{
				this.PropertyChanged(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.PropertyChanged(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the bounds of a node change.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.BoundsChanged"/>
		/// event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoundsEventArgs"/>
		/// </para>
		/// </remarks>
		protected virtual void OnBoundsChanged(BoundsEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.BoundsChanged != null)
			{
				this.BoundsChanged(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.BoundsChanged(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the position of a node is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.Moved"/>
		/// event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveEventArgs"/>
		/// </para>
		/// </remarks>
		protected virtual void OnMove(MoveEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.Moved != null)
			{
				this.Moved(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Move(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a node is rotated.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.Rotated"/>
		/// event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RotateEventArgs"/>
		/// </para>
		/// </remarks>
		protected virtual void OnRotate(RotateEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.Rotated != null)
			{
				this.Rotated(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Rotate(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a node is scaled.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Model.Scaled"/>
		/// event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ScaleEventArgs"/>
		/// </para>
		/// </remarks>
		protected virtual void OnScale(ScaleEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.Scaled != null)
			{
				this.Scaled(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Scale(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a node is clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the <see cref="Syncfusion.Windows.Forms.Diagram.Model.Click"/> event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		protected virtual void OnClick(NodeMouseEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.Click != null)
			{
				this.Click(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Click(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a node is double clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the <see cref="Syncfusion.Windows.Forms.Diagram.Model.DoubleClick"/> event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		protected virtual void OnDoubleClick(NodeMouseEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.DoubleClick != null)
			{
				this.DoubleClick(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.DoubleClick(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the mouse enters a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the <see cref="Syncfusion.Windows.Forms.Diagram.Model.MouseEnter"/> event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		protected virtual void OnMouseEnter(NodeMouseEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.MouseEnter != null)
			{
				this.MouseEnter(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.MouseEnter(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the mouse leaves a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the <see cref="Syncfusion.Windows.Forms.Diagram.Model.MouseLeave"/> event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		protected virtual void OnMouseLeave(NodeMouseEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.MouseLeave != null)
			{
				this.MouseLeave(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.MouseLeave(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a vertex is inserted.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the <see cref="Syncfusion.Windows.Forms.Diagram.Model.InsertVertex"/> event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		protected virtual void OnInsertVertex(VertexEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.InsertVertex != null)
			{
				this.InsertVertex(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.InsertVertex(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a vertex is moved.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the <see cref="Syncfusion.Windows.Forms.Diagram.Model.MoveVertex"/> event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		protected virtual void OnMoveVertex(VertexEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.MoveVertex != null)
			{
				this.MoveVertex(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.MoveVertex(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a vertex is deleted.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the <see cref="Syncfusion.Windows.Forms.Diagram.Model.DeleteVertex"/> event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		protected virtual void OnDeleteVertex(VertexEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.DeleteVertex != null)
			{
				this.DeleteVertex(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.DeleteVertex(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called before a change is made to the list of connections of a symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the <see cref="Syncfusion.Windows.Forms.Diagram.Model.ConnectionsChanging"/> event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnConnectionsChanging(ConnectionCollection.EventArgs evtArgs)
		{
			if (this.eventsEnabled && this.ConnectionsChanging != null)
			{
				this.ConnectionsChanging(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.ConnectionsChanging(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called after a change is made to the list of connections of a symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Fires the <see cref="Syncfusion.Windows.Forms.Diagram.Model.ConnectionsChangeComplete"/> event.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs)
		{
			if (this.eventsEnabled && this.ConnectionsChangeComplete != null)
			{
				this.ConnectionsChangeComplete(this, evtArgs);
			}

			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.ConnectionsChangeComplete(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called before the logical unit of measure is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		protected virtual void OnMeasurementUnitsChanging(LogicalUnitsEventArgs evtArgs)
		{
			// Convert values contained by the model and all children from
			// the old unit of measure to the new unit of measure
			((ILogicalUnitContainer)this).ConvertLogicalUnits(evtArgs.OldUnits, evtArgs.NewUnits, null);

			// Fire event
			if (this.MeasurementUnitsChanging != null)
			{
				this.MeasurementUnitsChanging(this, evtArgs);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected virtual void OnMeasurementScaleChanging(LogicalScaleEventArgs evtArgs)
		{
			// Convert values contained by the model and all children from
			// the old unit of measure to the new unit of measure
			((ILogicalUnitContainer)this).ConvertLogicalScale(evtArgs.OldScale, evtArgs.NewScale);

			// Fire event
			if (this.MeasurementScaleChanging != null)
			{
				this.MeasurementScaleChanging(this, evtArgs);
			}
		}

		#endregion

		#region IDispatchNodeEvents interface

		/// <summary>
		/// Called when a property value is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnPropertyChanged"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.PropertyChanged(PropertyEventArgs evtArgs)
		{
			this.OnPropertyChanged(evtArgs);
		}

		/// <summary>
		/// Called before the collection of child nodes is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnChildrenChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ChildrenChanging(NodeCollection.EventArgs evtArgs)
		{
			this.OnChildrenChanging(evtArgs);
		}

		/// <summary>
		/// Called after the collection of child nodes is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnChildrenChangeComplete"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ChildrenChangeComplete(NodeCollection.EventArgs evtArgs)
		{
			this.OnChildrenChangeComplete(evtArgs);
		}

		/// <summary>
		/// Called when the bounds of a node changes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnBoundsChanged"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoundsEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.BoundsChanged(BoundsEventArgs evtArgs)
		{
			this.OnBoundsChanged(evtArgs);
		}

		/// <summary>
		/// Called when a node is moved.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnMove"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.Move(MoveEventArgs evtArgs)
		{
			this.OnMove(evtArgs);
		}

		/// <summary>
		/// Called when a node is rotated.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnRotate"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RotateEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.Rotate(RotateEventArgs evtArgs)
		{
			this.OnRotate(evtArgs);
		}

		/// <summary>
		/// Called when a node is scaled.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnScale"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ScaleEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.Scale(ScaleEventArgs evtArgs)
		{
			this.OnScale(evtArgs);
		}

		/// <summary>
		/// Called when a node is clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnClick"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.Click(NodeMouseEventArgs evtArgs)
		{
			this.OnClick(evtArgs);
		}

		/// <summary>
		/// Called when a node is double clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnDoubleClick"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.DoubleClick(NodeMouseEventArgs evtArgs)
		{
			this.OnDoubleClick(evtArgs);
		}

		/// <summary>
		/// Called when the mouse enters a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnMouseEnter"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.MouseEnter(NodeMouseEventArgs evtArgs)
		{
			this.OnMouseEnter(evtArgs);
		}

		/// <summary>
		/// Called when the mouse leaves a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnMouseLeave"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.MouseLeave(NodeMouseEventArgs evtArgs)
		{
			this.OnMouseLeave(evtArgs);
		}

		/// <summary>
		/// Called when a vertex is inserted into a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnInsertVertex"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.InsertVertex(VertexEventArgs evtArgs)
		{
			this.OnInsertVertex(evtArgs);
		}

		/// <summary>
		/// Called when a vertex is deleted from a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnDeleteVertex"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.DeleteVertex(VertexEventArgs evtArgs)
		{
			this.OnDeleteVertex(evtArgs);
		}

		/// <summary>
		/// Called when a vertex is moved.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnMoveVertex"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.MoveVertex(VertexEventArgs evtArgs)
		{
			this.OnMoveVertex(evtArgs);
		}

		/// <summary>
		/// Called before a change is made to the connection list of a symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnConnectionsChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ConnectionsChanging(ConnectionCollection.EventArgs evtArgs)
		{
			this.OnConnectionsChanging(evtArgs);
		}

		/// <summary>
		/// Called after a change is made to the connection list of a symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Model.OnConnectionsChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs)
		{
			this.OnConnectionsChangeComplete(evtArgs);
		}

		#endregion

		#region Collection Event Handlers

		private void Children_Changing(object sender, NodeCollection.EventArgs evtArgs)
		{
			CollectionEx.ChangeType changeType = evtArgs.ChangeType;

			// Get node from event arguments
			INode obj = evtArgs.Node;

			if (changeType == CollectionEx.ChangeType.Insert)
			{
				// Generate unique name for node
				string nodeName;
				if (GenerateUniqueNodeName(obj, out nodeName))
				{
					obj.Name = nodeName;
				}
			}

			this.OnChildrenChanging(evtArgs);
		}

		private void Children_ChangeComplete(object sender, NodeCollection.EventArgs evtArgs)
		{
			CollectionEx.ChangeType changeType = evtArgs.ChangeType;

			// Get node from event arguments
			INode obj = evtArgs.Node;

			if (changeType == CollectionEx.ChangeType.Insert)
			{
				// Make this object the parent of the new child
				obj.Parent = this;

				// Update the name table
				this.nameTable.Add(obj.Name, obj);

				// Add to the default layer
				this.defaultLayer.Add(obj);

				// Set the modified flag for the model
				this.Modified = true;

			}
			else if (changeType == CollectionEx.ChangeType.Remove)
			{
				// Update the name table
				this.nameTable.Remove(obj.Name);

				// Disconnect the node from parent
				obj.Parent = null;

				// Remove node from the layer it belongs to
				Layer layer = this.layers.FindNodeLayer(obj);
				if (layer != null)
				{
					layer.Remove(obj);
				}

				// Set the modified flag for the model
				this.Modified = true;
			}
			else if (changeType == CollectionEx.ChangeType.Clear)
			{
				this.Modified = true;
				this.nameTable.Clear();
				this.layers.RemoveAllMembers();
			}

			this.OnChildrenChangeComplete(evtArgs);
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
			info.AddValue("name", this.name);
			info.AddValue("propertyValues", this.propertyValues);
			info.AddValue("parent", this.parent);
			info.AddValue("bounds", this.bounds);
			info.AddValue("m11", this.matrix.Elements[0]);
			info.AddValue("m12", this.matrix.Elements[1]);
			info.AddValue("m21", this.matrix.Elements[2]);
			info.AddValue("m22", this.matrix.Elements[3]);
			info.AddValue("dx", this.matrix.Elements[4]);
			info.AddValue("dy", this.matrix.Elements[5]);
			info.AddValue("children", this.children);
			info.AddValue("layers", this.layers);
			info.AddValue("defaultLayer", this.defaultLayer);
			info.AddValue("measurementUnits", this.measurementUnits);
			info.AddValue("measurementScale", this.measurementScale);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.nameTable = new Hashtable();
			this.fillStyle = new FillStyle(this);
			this.lineStyle = new LineStyle(this);
			this.backgroundStyle = new BackgroundStyle(this);
			this.eventsEnabled = false;
			this.children.Changing += new NodeCollection.EventHandler(Children_Changing);
			this.children.ChangeComplete += new NodeCollection.EventHandler(Children_ChangeComplete);
			this.eventsEnabled = true;
			this.isModified = false;
		}

		#endregion

		#region Implementation Methods

		/// <summary>
		/// Called to generate a unique name when inserting a new node.
		/// </summary>
		/// <param name="obj">Node to generate unique name for</param>
		/// <param name="nodeName">Node name generated</param>
		/// <returns>
		/// true if a new name was generated; false if the name is already unique
		/// </returns>
		/// <remarks>
		/// <para>
		/// The implementation of this method first checks to see if the name is
		/// already unique. If it is, then it returns false to the caller and
		/// the nodeName parameter contains the original name. If the node's name
		/// is not unique within the model, then this method adds a numeric suffix
		/// to the name and continues to increment it in a loop until the name
		/// is unique. If the nodeName output parameter contains a value other
		/// than the original node name, then this method returns true.
		/// </para>
		/// <para>
		/// This method can be overriden in derived classes in order to customize or
		/// replace the algorithm for generating unique names.
		/// </para>
		/// </remarks>
		protected virtual bool GenerateUniqueNodeName(INode obj, out string nodeName)
		{
			string baseNodeName = obj.Name;
			bool nameChanged = false;

			if (baseNodeName == null || baseNodeName.Length == 0)
			{
				baseNodeName = obj.GetType().Name;
				nameChanged = true;
			}

			uint nameSuffix = 1;
			nodeName = baseNodeName;
			while (this.nameTable.Contains(nodeName))
			{
				if (nameSuffix == 0xFFFFFFFF)
				{
					throw new EDuplicateName();
				}
				else
				{
					nodeName = baseNodeName + nameSuffix.ToString();
					nameChanged = true;
					nameSuffix++;
				}
			}

			return nameChanged;
		}

		#endregion

		#region Events

		/// <summary>
		/// Fired before the collection of child nodes is changed.
		/// </summary>
		public event NodeCollection.EventHandler ChildrenChanging;

		/// <summary>
		/// Fired after the collection of child nodes is changed.
		/// </summary>
		public event NodeCollection.EventHandler ChildrenChangeComplete;

		/// <summary>
		/// Fired when a property of the model or a child node is changed.
		/// </summary>
		public event PropertyEventHandler PropertyChanged;

		/// <summary>
		/// Fired when the bounds of the model or a child node change.
		/// </summary>
		public event BoundsEventHandler BoundsChanged;

		/// <summary>
		/// Fired when the logical unit of measure changes.
		/// </summary>
		public event LogicalUnitsEventHandler MeasurementUnitsChanging;

		/// <summary>
		/// Fired when the scale of logical units changes.
		/// </summary>
		public event LogicalScaleEventHandler MeasurementScaleChanging;

		/// <summary>
		/// Fired when a child node is moved.
		/// </summary>
		public event MoveEventHandler Moved;

		/// <summary>
		/// Fired when a child node is rotate.
		/// </summary>
		public event RotateEventHandler Rotated;

		/// <summary>
		/// Fired when a child node is scaled.
		/// </summary>
		public event ScaleEventHandler Scaled;

		/// <summary>
		/// Fired when a child node is single clicked.
		/// </summary>
		public event NodeMouseEventHandler Click;

		/// <summary>
		/// Fired when a child node is double clicked.
		/// </summary>
		public event NodeMouseEventHandler DoubleClick;

		/// <summary>
		/// Fired when the mouse enters a child node.
		/// </summary>
		public event NodeMouseEventHandler MouseEnter;

		/// <summary>
		/// Fired when the mouse leaves a child node.
		/// </summary>
		public event NodeMouseEventHandler MouseLeave;

		/// <summary>
		/// Fired when a vertex is inserted into a child node.
		/// </summary>
		public event VertexEventHandler InsertVertex;

		/// <summary>
		/// Fired when a vertex belonging to a child node is moved.
		/// </summary>
		public event VertexEventHandler MoveVertex;

		/// <summary>
		/// Fired when a vertex belonging to a child node is deleted.
		/// </summary>
		public event VertexEventHandler DeleteVertex;

		/// <summary>
		/// Fired before a change is made to the connection list of a symbol.
		/// </summary>
		public event ConnectionCollection.EventHandler ConnectionsChanging;

		/// <summary>
		/// Fired after a change is made to the connection list of a symbol.
		/// </summary>
		public event ConnectionCollection.EventHandler ConnectionsChangeComplete;

		#endregion

		#region Fields

		/// <summary>
		/// Name of the model.
		/// </summary>
		private string name;

		/// <summary>
		/// Reference to parent node.
		/// </summary>
		private ICompositeNode parent = null;

		/// <summary>
		/// Logical bounds of the model.
		/// </summary>
		private RectangleF bounds;

		/// <summary>
		/// Local transformation matrix.
		/// </summary>
		protected Matrix matrix = null;

		/// <summary>
		/// Collection of child nodes belonging to the model.
		/// </summary>
		protected NodeCollection children = null;

		/// <summary>
		/// Collection of layers in the model.
		/// </summary>
		private LayerCollection layers = null;

		/// <summary>
		/// Layer to which new nodes are assigned to.
		/// </summary>
		private Layer defaultLayer = null;

		/// <summary>
		/// Maps node names to node objects.
		/// </summary>
		private Hashtable nameTable = null;

		/// <summary>
		/// Collection of property name and value pairs.
		/// </summary>
		protected Hashtable propertyValues = null;

		/// <summary>
		/// Properties for filling shapes in the model.
		/// </summary>
		private FillStyle fillStyle = null;

		/// <summary>
		/// Properties for drawing lines in the model.
		/// </summary>
		private LineStyle lineStyle = null;

		/// <summary>
		/// Properties for filling the background of the model.
		/// </summary>
		private BackgroundStyle backgroundStyle = null;

		/// <summary>
		/// Unit of measure used for world coordinates.
		/// </summary>
		private GraphicsUnit measurementUnits = GraphicsUnit.Pixel;

		/// <summary>
		/// Scaling ratio between world coordinates and view coordinates.
		/// </summary>
		private float measurementScale = 1.0f;

		/// <summary>
		/// Determines if events will be fired or not.
		/// </summary>
		private bool eventsEnabled = false;

		/// <summary>
		/// Flag to indicate if the model has been modified.
		/// </summary>
		private bool isModified = false;

		/// <summary>
		/// Page settings used for printing.
		/// </summary>
		private System.Drawing.Printing.PageSettings pageSettings = null;

		/// <summary>
		/// Indicates if boundary constraints are enabled or not
		/// </summary>
		private bool boundaryConstraintsEnabled = true;

		/// <summary>
		/// Region that child nodes are constrained to
		/// </summary>
		private System.Drawing.Region rgnConstraint = null;

		/// <summary>
		/// Indicates if the Dispose() method has been called
		/// </summary>
		private bool disposed = false;

		#endregion
	}
}
