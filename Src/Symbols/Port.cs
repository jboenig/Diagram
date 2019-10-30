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
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A port is a point on a symbol on which connections can be established.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Each port is owned by a port container and may have zero or more connections
	/// hooked up to it. A port container is an object that implements
	/// the <see cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
	/// interface. The port container is typically either a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Symbol"/> or a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Link"/>.
	/// </para>
	/// <para>
	/// This is an abstract base class from which different implementations are
	/// derived. Positioning and visual representation of a port is determined by
	/// classes derived from this one. For example, a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.CirclePort"/> renders itself as
	/// a circle containing crosshairs and can be positioned anywhere within the bounds
	/// of the port container. A
	/// <see cref="Syncfusion.Windows.Forms.Diagram.CenterPort"/> is
	/// invisible and always positions itself at the center of its port
	/// container.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CenterPort"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CirclePort"/>
	/// </remarks>
	[Serializable()]
	public abstract class Port : INode, IBounds2DF, IDraw, ITransform, IPropertyContainer, ISerializable
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Port()
		{
			this.propertyValues = new Hashtable();
			SetDefaultPropertyValues();
		}

		/// <summary>
		/// Constructs a Port given a port container.
		/// </summary>
		/// <param name="container">Port container that owns the port</param>
		public Port(IPortContainer container)
		{
			this.Container = container;
			this.propertyValues = new Hashtable();
			SetDefaultPropertyValues();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy</param>
		public Port(Port src)
		{
			this.name = src.name;
			this.propertyValues = (Hashtable) src.propertyValues.Clone();
		}

		/// <summary>
		/// Serialization constructor for ports.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Port(SerializationInfo info, StreamingContext context)
		{
			this.name = info.GetString("name");
			this.parent = (ICompositeNode) info.GetValue("parent", typeof(ICompositeNode));
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public abstract object Clone();

		#endregion

		#region Public Properties

		/// <summary>
		/// Container that owns this port.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The parent node of a port must support the IPortContainer interface.
		/// The get method returns the port's parent node downcast to the
		/// IPortContainer interface. The set method upcasts the value passed in
		/// to the ICompositeNode interface and assigns it to the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Port.Parent"/>
		/// property.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port.Parent"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICompositeNode"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public IPortContainer Container
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent as IPortContainer;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.parent = value as ICompositeNode;
				}
			}
		}

		/// <summary>
		/// Location of the port.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port.Bounds"/>
		/// </remarks>
		[
		Browsable(false),
		Category("Bounds")
		]
		public abstract PointF Location
		{
			get;
			set;
		}

		/// <summary>
		/// Size of the port.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port.Bounds"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract SizeF Size
		{
			get;
			set;
		}

		/// <summary>
		/// Radius of the port.
		/// </summary>
		public abstract float Radius
		{
			get;
			set;
		}

		/// <summary>
		/// Determines if the port is visible.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// </remarks>
		[
		Browsable(true),
		Category("General")
		]
		public bool Visible
		{
			get
			{
				return (bool) this.GetPropertyValue("Visible");
			}
			set
			{
				this.SetPropertyValue("Visible", value);
			}
		}

		/// <summary>
		/// Determines if the port is enabled or disabled.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Behavior")
		]
		public bool Enabled
		{
			get
			{
				return (bool) this.GetPropertyValue("Enabled");
			}
			set
			{
				this.SetPropertyValue("Enabled", value);
			}
		}

		/// <summary>
		/// Determines if ports connecting to this port must remain at the
		/// perimeter of the port container.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// </remarks>
		public bool AttachAtPerimeter
		{
			get
			{
				return (bool) GetPropertyValue("AttachAtPerimeter");
			}
			set
			{
				SetPropertyValue("AttachAtPerimeter", value);
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Tests to see if a connection to the given port is allowed.
		/// </summary>
		/// <param name="port">Port to test</param>
		/// <returns>
		/// true if a connection is allowed between this port and the specified port;
		/// otherwise false
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method calls the port container's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPortContainer.AcceptConnection"/>
		/// method, passing as parameters this port and the parameter to this
		/// method.
		/// </para>
		/// </remarks>
		public bool AcceptConnection(Port port)
		{
			bool accept = false;
			IPortContainer container = this.Container;
			if (container != null)
			{
				return container.AcceptConnection(this, port);
			}
			return accept;
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IBounds2DF"/>
		/// </para>
		/// </remarks>
		object IServiceProvider.GetService(System.Type svcType)
		{
			if (svcType == typeof(IBounds2DF))
			{
				return (IBounds2DF) this;
			}
			return null;
		}

		#endregion

		#region INode interface

		/// <summary>
		/// Reference to the composite node this node is a child of.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public ICompositeNode Parent
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
		public INode Root
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
		/// Name of the port.
		/// </summary>
		/// <remarks>
		/// Must be unique within the scope of the parent node.
		/// </remarks>
		[
		Browsable(true)
		]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>
		/// Fully qualified name of the port.
		/// </summary>
		/// <remarks>
		/// The full name is the name of the node concatenated with the names
		/// of all parent nodes.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public string FullName
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

		#endregion

		#region IBounds2DF interface

		/// <summary>
		/// The port's bounding box.
		/// </summary>
		/// <remarks>
		/// Always returns the bounds of the port in world coordinates, regardless
		/// of what is on the matrix stack at the time of the call.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract RectangleF Bounds
		{
			get;
			set;
		}

		/// <summary>
		/// X-coordinate of the port's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract float X
		{
			get;
			set;
		}

		/// <summary>
		/// Y-coordinate of the port's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract float Y
		{
			get;
			set;
		}

		/// <summary>
		/// Width of the port.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract float Width
		{
			get;
			set;
		}

		/// <summary>
		/// Height of the port.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract float Height
		{
			get;
			set;
		}

		#endregion

		#region IDraw interface

		/// <summary>
		/// Draws the port onto a System.Drawing.Graphics device context.
		/// </summary>
		/// <param name="gc">Graphics context to draw onto</param>
		public abstract void Draw(System.Drawing.Graphics gc);

		#endregion

		#region ITransform interface

		/// <summary>
		/// Moves the port by the given X and Y offsets.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		public abstract void Translate(float dx, float dy);

		/// <summary>
		/// Not allowed for ports.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to rotate</param>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// <para>Throws an exception. Rotation is not allowed on ports.</para>
		/// </remarks>
		public void Rotate(PointF ptAnchor, float degrees)
		{
			throw new EInvalidOperation();
		}

		/// <summary>
		/// Not allowed for ports.
		/// </summary>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// <para>Throws an exception. Rotation is not allowed on ports.</para>
		/// </remarks>
		public void Rotate(float degrees)
		{
			throw new EInvalidOperation();
		}

		/// <summary>
		/// Not allowed for ports.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to scale</param>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// <para>Throws an exception. Scaling is not allowed on ports.</para>
		/// </remarks>
		public void Scale(PointF ptAnchor, float sx, float sy)
		{
			throw new EInvalidOperation();
		}

		/// <summary>
		/// Not allowed for ports.
		/// </summary>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// <para>Throws an exception. Scaling is not allowed on ports.</para>
		/// </remarks>
		public void Scale(float sx, float sy)
		{
			throw new EInvalidOperation();
		}

		/// <summary>
		/// Matrix containing transformations for this port.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract Matrix LocalTransform
		{
			get;
		}

		/// <summary>
		/// Returns a matrix containing transformations for this port and all of
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
		public abstract Matrix WorldTransform
		{
			get;
		}

		/// <summary>
		/// Returns a matrix containing the transformations of this ports's parent.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract Matrix ParentTransform
		{
			get;
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("name", this.name);
			info.AddValue("parent", this.parent);
			info.AddValue("propertyValues", this.propertyValues);
		}

		#endregion

		#region IPropertyContainer interface

		/// <summary>
		/// Sets the default property values for the port.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// port to their default values.
		/// </remarks>
		public virtual void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("Visible", true);
			this.propertyValues.Add("Enabled", true);
			this.propertyValues.Add("FillType", Syncfusion.Windows.Forms.Diagram.FillStyle.FillType.Solid);
			this.propertyValues.Add("FillColor", Color.White);
			this.propertyValues.Add("LineColor", Color.Black);
			this.propertyValues.Add("LineWidth", 2.0f);
			this.propertyValues.Add("AllowSelect", true);
			this.propertyValues.Add("AllowVertexEdit", false);
			this.propertyValues.Add("AllowMove", true);
			this.propertyValues.Add("AllowRotate", false);
			this.propertyValues.Add("AllowResize", false);
			this.propertyValues.Add("AttachAtPerimeter", false);
		}

		/// <summary>
		/// Retrieve the value of a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to retrieve</param>
		/// <returns>Value of the named property or null if it doesn't exist</returns>
		public virtual object GetPropertyValue(string propertyName)
		{
			object value = null;
			IPortContainer portContainer = this.Container;

			IPropertyContainer parentProps = null;
			if (this.parent != null)
			{
				parentProps = this.parent.GetPropertyContainer(this);
			}

			if (this.propertyValues.Contains(propertyName))
			{
				value = this.propertyValues[propertyName];
			}

			if (value == null && parentProps != null)
			{
				value = parentProps.GetPropertyValue(propertyName);
			}

			if (propertyName == "Visible" && parentProps != null)
			{
				// Visibility may depend on whether the AutoHidePorts property
				// is set to true in the symbol symbol

				bool visible = true;

				if (value != null && value.GetType() == typeof(bool))
				{
					visible = (bool) value;
				}

				if (visible && portContainer != null && portContainer.AutoHidePorts)
				{
					object revealPorts = parentProps.GetPropertyValue("RevealPorts");
					if (revealPorts != null && revealPorts.GetType() == typeof(bool))
					{
						value = (bool) revealPorts;
					}
				}
			}

			return value;
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
			if (this.propertyValues.ContainsKey(propertyName))
			{
				this.propertyValues[propertyName] = val;
			}
			else
			{
				this.propertyValues.Add(propertyName, val);
			}
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
			this.propertyValues[propertyName] = val;
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

		#region Fields

		/// <summary>
		/// Name of the port
		/// </summary>
		private string name;

		/// <summary>
		/// Parent node.
		/// </summary>
		private ICompositeNode parent = null;

		/// <summary>
		/// Hash table containing property name/value pairs.
		/// </summary>
		protected Hashtable propertyValues = null;

		#endregion
	}
}