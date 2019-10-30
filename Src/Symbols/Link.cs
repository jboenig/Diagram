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
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A link is a composite node that supports labels and connections to other
	/// symbols.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A link is a special type of symbol that has two built-in ports: a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.HeadPort"/>
	/// and a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.TailPort"/>.
	/// The head and tail port define the direction of the link.
	/// </para>
	/// <para>
	/// The head and tail ports are anchored to the first child node belonging
	/// to the link (at index position 0), which is referred to as the link
	/// shape. The link shape can be any type of node, the only requirement
	/// being that it must support the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IPoints"/> interface.
	/// The tail port is anchored to the first vertex in the link shape and the
	/// head port is anchored to the last point in the link shape.
	/// </para>
	/// <para>
	/// The link shape is created by the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
	/// method, which is virtual and can overridden by derived classes. The
	/// CreateLinkShape method is passed a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.Shapes"/> enumeration,
	/// which defines a set of built-in link shapes to choose from. The
	/// CreateLinkShape method is also passed an array of points to load into
	/// the link shape. The default implementation of the CreateLinkShape method
	/// maps the <see cref="Syncfusion.Windows.Forms.Diagram.Link.Shapes"/>
	/// enumeration onto a specified type of shape node, and then creates an
	/// instance of that type and loads it with the array of points. Derived
	/// classes can override CreateLinkShape and interpret the parameters
	/// however they like, as long as it returns a valid object that supports
	/// the IPoints interface.
	/// </para>
	/// <para>
	/// Like symbols, links may contain labels. Labels are accessible through the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.Labels"/> collection.
	/// The <see cref="Syncfusion.Windows.Forms.Diagram.LinkLabel"/> class
	/// implements a special type of label that can orient itself along a
	/// line or curve by specifying the a percentage value along the link.
	/// Setting the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LinkLabel.PercentAlongLine"/>
	/// property of a LinkLabel will position the label at the tail of the link.
	/// Setting it to 100 will position the label at the head of the link. Values
	/// in between position the label somewhere between the head and tail.
	/// </para>
	/// <para>
	/// The link class implements the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IGraphEdge"/> interface, which
	/// is used in conjunction with the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IGraphNode"/> interface
	///  to navigate the node hierarchy as a graph of interconnected nodes and edges.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphEdge"/>
	/// </remarks>
	[Serializable]
	public class Link : SymbolBase, IGraphEdge, IServiceProvider
	{
		#region Enumerations

		/// <summary>
		/// Defines a set of built-in shapes that can be assigned to links.
		/// </summary>
		public enum Shapes
		{
			/// <summary>
			/// Line shape
			/// </summary>
			Line,

			/// <summary>
			/// Polyline shape
			/// </summary>
			Polyline,

			/// <summary>
			/// Orthogonal line shape
			/// </summary>
			OrthogonalLine,

			/// <summary>
			/// Arc shape
			/// </summary>
			Arc,

			/// <summary>
			/// Default shape
			/// </summary>
			Default
		};

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Calls the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
		/// method passing it
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.Shapes.Default"/>
		/// for the shape type and two points that are initialized to (0,0). An
		/// object supporting the IPoints interface is returned by the
		/// CreateLinkShape method, which is then added as the first child node
		/// in the link.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public Link()
		{
			PointF[] pts = new PointF[2] {new PointF(0,0), new PointF(0,0)};
			IPoints linkPoints = this.CreateLinkShape(Link.Shapes.Default, pts);
			if (linkPoints != null)
			{
				INode childNode = linkPoints as INode;
				if (childNode != null)
				{
					this.AppendChild(childNode);
				}
			}

			this.Ports.Add(CreateTailPort());
			this.Ports.Add(CreateHeadPort());
		}

		/// <summary>
		/// Constructs a Link object from a given set of points.
		/// </summary>
		/// <param name="pts">Points to load shape with</param>
		/// <remarks>
		/// <para>
		/// Calls the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
		/// method passing it
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.Shapes.Default"/>
		/// for the shape type and the array of points. An object supporting then
		/// IPoints interface is returned by the CreateLinkShape method, which
		/// is then added as the first child node in the link.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public Link(PointF[] pts)
		{
			IPoints linkPoints = this.CreateLinkShape(Link.Shapes.Default, pts);

			if (linkPoints != null)
			{
				INode childNode = linkPoints as INode;
				if (childNode != null)
				{
					this.AppendChild(childNode);
				}
			}

			this.Ports.Add(CreateTailPort());
			this.Ports.Add(CreateHeadPort());
		}

		/// <summary>
		/// Creates a link with a given type of shape.
		/// </summary>
		/// <param name="shapeType">Type of shape to create</param>
		/// <remarks>
		/// <para>
		/// Calls the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
		/// method passing it the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.Shapes"/>
		/// parameter and two default points. An object supporting the IPoints
		/// interface is returned by the CreateLinkShape method, which is
		/// then added as the first child node in the link.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public Link(Link.Shapes shapeType)
		{
			PointF[] pts = new PointF[2] {new PointF(0,0), new PointF(0,0)};
			IPoints linkPoints = this.CreateLinkShape(shapeType, pts);
			if (linkPoints != null)
			{
				INode childNode = linkPoints as INode;
				if (childNode != null)
				{
					this.AppendChild(childNode);
				}
			}

			this.Ports.Add(CreateTailPort());
			this.Ports.Add(CreateHeadPort());
		}

		/// <summary>
		/// Creates a link with a given type of shape and an array of points.
		/// </summary>
		/// <param name="shapeType">Type of shape to create</param>
		/// <param name="pts">Array of points to load the shape with</param>
		/// <remarks>
		/// <para>
		/// Calls the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
		/// method passing it the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.Shapes"/>
		/// parameter and the array of points. An object supporting the IPoints
		/// interface is returned by the CreateLinkShape method, which is
		/// then added as the first child node in the link.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public Link(Link.Shapes shapeType, PointF[] pts)
		{
			IPoints linkPoints = this.CreateLinkShape(shapeType, pts);

			if (linkPoints != null)
			{
				INode childNode = linkPoints as INode;
				if (childNode != null)
				{
					this.AppendChild(childNode);
				}
			}

			this.Ports.Add(CreateTailPort());
			this.Ports.Add(CreateHeadPort());
		}

		/// <summary>
		/// Constructs a link given a link shape object.
		/// </summary>
		/// <param name="linkPoints">Link shape object to attach</param>
		/// <remarks>
		/// <para>
		/// Instead of calling the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.CreateLinkShape"/>
		/// method to create the link shape, this constructor takes the link shape
		/// object as a parameter and attaches it to the link. A link shape is
		/// any object that supports the IPoints interface.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public Link(IPoints linkPoints)
		{
			if (linkPoints != null)
			{
				INode childNode = linkPoints as INode;
				if (childNode != null)
				{
					this.AppendChild(childNode);
				}
			}

			this.Ports.Add(CreateTailPort());
			this.Ports.Add(CreateHeadPort());
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy from</param>
		public Link(Link src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for symbols.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Link(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new Link(this);
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// </para>
		/// </remarks>
		object IServiceProvider.GetService(System.Type svcType)
		{
			if (svcType == typeof(IPoints))
			{
				return this.Points;
			}
			else if (svcType == typeof(IPropertyContainer))
			{
				return this;
			}
			return null;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// The port anchored to the entry point of the link.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The head port is always anchored to the last vertex in the link
		/// shape. The link shape is the first child node in the link, and it
		/// must support the <see cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// interface.
		/// </para>
		/// <para>
		/// The head and tail ports of a link define the direction of the link.
		/// The head port is the exit point for a link.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.CreateHeadPort"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.TailPort"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Port HeadPort
		{
			get
			{
				return this.Ports[1];
			}
		}

		/// <summary>
		/// The port anchored to the exit point of the link.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The tail port is always anchored to the first vertex in the link
		/// shape. The link shape is the first child node in the link, and it
		/// must support the <see cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// interface.
		/// </para>
		/// <para>
		/// The head and tail ports of a link define the direction of the link.
		/// The tail port is the entry point for a link.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.CreateTailPort"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.HeadPort"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Port TailPort
		{
			get
			{
				return this.Ports[0];
			}
		}		

		/// <summary>
		/// Shape object belonging to the link.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Gets the first child node belonging to the link and returns its
		/// IPoints interface. By definition, the first child node in a link
		/// must support the IPoints interface.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public IPoints Points
		{
			get
			{
				IPoints linkPoints = null;

				if (this.ChildCount > 0)
				{
					linkPoints = this.GetChild(0) as IPoints;
				}

				return linkPoints;
			}
		}

		/// <summary>
		/// Returns the endpoints of the link shape.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The link shape is the first child node belonging to the link. This
		/// property returns the link shape's IEndPoints interface.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IEndPoints"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public IEndPoints EndPoints
		{
			get
			{
				IEndPoints shapeEndPoints = null;

				if (this.ChildCount > 0)
				{
					shapeEndPoints = this.GetChild(0) as IEndPoints;
				}

				return shapeEndPoints;
			}
		}

		#endregion

		#region Connections

		/// <summary>
		/// Creates a new connection between the specified port and the link's
		/// head port.
		/// </summary>
		/// <param name="port">Port to connect to</param>
		/// <returns>The connection created, or null if failed</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.HeadPort"/>
		/// </remarks>
		public virtual Connection ConnectHead(Port port)
		{
			return this.Connect(this.HeadPort, port);
		}

		/// <summary>
		/// Creates a new connection between the specified port and the link's
		/// tail port.
		/// </summary>
		/// <param name="port">Port to connect to</param>
		/// <returns>The connection created, or null if failed</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.HeadPort"/>
		/// </remarks>
		public virtual Connection ConnectTail(Port port)
		{
			return this.Connect(port, this.TailPort);
		}

		/// <summary>
		/// Called after a change is made to the set of connection belonging to
		/// the link.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Docks the connection.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		protected override void OnConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs)
		{
			if (evtArgs.ChangeType == CollectionEx.ChangeType.Insert)
			{
				this.DockConnections();
			}
		}

		/// <summary>
		/// This method is called by connections when the foreign port container
		/// moves.
		/// </summary>
		/// <param name="connection">Connection that has moved</param>
		/// <remarks>
		/// Docs the connection.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection"/>
		/// </remarks>
		public override void OnConnectionMove(Connection connection)
		{
			this.DockConnections();
		}

		/// <summary>
		/// Adjusts the endpoints of the link so that they are docked with the
		/// foreign ports they are connected to.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method iterates through every connection in the link. For each
		/// connection, the location of the local port is updated to match
		/// the location of the foreign port. If the foreign port has its
		/// AttachAtPerimeter flag set to true, then some additional calculations
		/// are made in order to move the local port to the perimeter of the
		/// foreign port's container.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port.AttachAtPerimeter"/>
		/// </remarks>
		protected virtual void DockConnections()
		{
			IPoints shapePts = this.Points;
			LinkPort curLocalPort;
			Port curForeignPort;
			IPortContainer curForeignObj;
			int numPoints = 0;
			ConnectionCollection perimeterConnections = new ConnectionCollection();

			PointF ptStart;
			int ptIdx;
			PointF ptBoundary;

			if (shapePts != null)
			{
				numPoints = shapePts.PointCount;
				foreach (Connection curConnection in this.Connections)
				{
					curLocalPort = curConnection.GetLocalPort(this) as LinkPort;
					curForeignPort = curConnection.GetForeignPort(this);

					if (curLocalPort != null && curForeignPort != null)
					{
						curLocalPort.Location = curForeignPort.Location;

						if (curForeignPort.AttachAtPerimeter)
						{
							perimeterConnections.Add(curConnection);
						}
					}
				}

				foreach (Connection curConnection in perimeterConnections)
				{
					curLocalPort = curConnection.GetLocalPort(this) as LinkPort;
					curForeignPort = curConnection.GetForeignPort(this);

					if (curLocalPort != null && curForeignPort != null)
					{
						curForeignObj = curForeignPort.Container;
						if (curForeignObj != null)
						{
							ptIdx = curLocalPort.PointIndex;
							if (ptIdx > 0 && ptIdx == numPoints-1)
							{
								ptStart = shapePts.GetPoint(ptIdx-1);
							}
							else if (ptIdx == 0 && numPoints > 1)
							{
								ptStart = shapePts.GetPoint(ptIdx+1);
							}
							else
							{
								ptStart = curLocalPort.Location;
							}

							IGraphics curForeignObjGraphics = curForeignObj as IGraphics;
							if (curForeignObjGraphics != null)
							{
								if (Geometry.GetBoundaryIntercept(curForeignObjGraphics.CreateRegion(0.0f), curLocalPort.Location, ptStart, out ptBoundary))
								{
									curLocalPort.Location = ptBoundary;
								}
							}
						}
					}
				}
			}
		}

		#endregion

		#region Implementation Methods

		/// <summary>
		/// Creates a link shape matching the type specified by the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.Shapes"/>
		/// enumeration and load its with the given array of points.
		/// </summary>
		/// <param name="shapeType">Type of shape to create</param>
		/// <param name="pts">Points to initialize shape with</param>
		/// <returns>IPoints interface to the link shape object created</returns>
		/// <remarks>
		/// <para>
		/// A link shape can be any type of node that supports the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// interface. This method supports creating one of the following
		/// types of link shapes:
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Line"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.OrthogonalLine"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.PolyLine"/>, and
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Arc"/>. If
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.Shapes.Default"/>
		/// is specified, then a polyline is created.
		/// </para>
		/// <para>
		/// This method can be overridden in derived classes to support the
		/// creation of other types of link shapes.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link.Shapes"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		protected virtual IPoints CreateLinkShape(Link.Shapes shapeType, PointF[] pts)
		{
			Shape linkShape = null;

			switch (shapeType)
			{
				case Link.Shapes.Line:
					linkShape = new Line();
					break;

				case Link.Shapes.OrthogonalLine:
					linkShape = new OrthogonalLine();
					break;

				case Link.Shapes.Arc:
					linkShape = new Arc();
					break;

				default:
					linkShape = new PolyLine();
					break;
			}

			if (linkShape != null)
			{
				linkShape.SetPoints(pts);
			}

			return linkShape;
		}

		/// <summary>
		/// Called to create the link's head port.
		/// </summary>
		/// <returns>Port to be used as the head port</returns>
		/// <remarks>
		/// <para>
		/// Creates and returns a new LinkHeadPort object. This method can be
		/// overridden in derived classes in order to customize the creation
		/// of the head port.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LinkHeadPort"/>
		/// </remarks>
		protected virtual Port CreateHeadPort()
		{
			return new LinkHeadPort(this);
		}

		/// <summary>
		/// Called to create the link's tail port.
		/// </summary>
		/// <returns>Port to be used as the tail port</returns>
		/// <remarks>
		/// <para>
		/// Creates and returns a new LinkTailPort object. This method can be
		/// overridden in derived classes in order to customize the creation
		/// of the head port.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LinkTailPort"/>
		/// </remarks>
		protected virtual Port CreateTailPort()
		{
			return new LinkTailPort(this);
		}

		#endregion

		#region Graph Navigation

		/// <summary>
		/// Node connected to the tail port of the link.
		/// </summary>
		/// <remarks>
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.TailPort"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public IGraphNode FromNode
		{
			get
			{
				IGraphNode fromNode = null;
				Port tailPort = this.TailPort;

				if (tailPort != null)
				{
					ConnectionCollection tailPortConnections = new ConnectionCollection();

					if (this.GetConnectionsOnPort(tailPortConnections, tailPort) > 0)
					{
						Port foreignPort = tailPortConnections[0].GetForeignPort(this);
						if (foreignPort != null)
						{
							IPortContainer foreignObj = foreignPort.Container;
							if (foreignObj != null)
							{
								fromNode = foreignObj as IGraphNode;
							}
						}
					}
				}

				return fromNode;
			}
		}

		/// <summary>
		/// Node connected to the head port of the link.
		/// </summary>
		/// <remarks>
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Link.HeadPort"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public IGraphNode ToNode
		{
			get
			{
				IGraphNode toNode = null;
				Port headPort = this.HeadPort;

				if (headPort != null)
				{
					ConnectionCollection headPortConnections = new ConnectionCollection();

					if (this.GetConnectionsOnPort(headPortConnections, headPort) > 0)
					{
						Port foreignPort = headPortConnections[0].GetForeignPort(this);
						if (foreignPort != null)
						{
							IPortContainer foreignObj = foreignPort.Container;
							if (foreignObj != null)
							{
								toNode = foreignObj as IGraphNode;
							}
						}
					}
				}

				return toNode;
			}
		}

		/// <summary>
		/// Weight value associated with the edge.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int EdgeWeight
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Determines if this link is leaving the given node.
		/// </summary>
		/// <param name="graphNode">Node to test</param>
		/// <returns>true if edge is leaving the given node</returns>
		public bool IsNodeLeaving(IGraphNode graphNode)
		{
			Port tailPort = this.TailPort;

			if (tailPort != null)
			{
				foreach (Connection connection in this.Connections)
				{
					Port foreignPort = connection.GetForeignPort(this);
					if (foreignPort != null)
					{
						IPortContainer foreignObj = foreignPort.Container;
						if (foreignObj != null)
						{
							IGraphNode foreignGraphNode = foreignObj as IGraphNode;
							if (foreignGraphNode != null && foreignGraphNode == graphNode)
							{
								Port localPort = connection.GetLocalPort(this);
								if (localPort != null)
								{
									if (localPort == tailPort)
									{
										return true;
									}
								}
							}
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Determines if this link is entering the given node.
		/// </summary>
		/// <param name="graphNode">Node to test</param>
		/// <returns>true if edge is entering the given node</returns>
		public bool IsNodeEntering(IGraphNode graphNode)
		{
			Port headPort = this.HeadPort;

			if (headPort != null)
			{
				foreach (Connection connection in this.Connections)
				{
					Port foreignPort = connection.GetForeignPort(this);
					if (foreignPort != null)
					{
						IPortContainer foreignObj = foreignPort.Container;
						if (foreignObj != null)
						{
							IGraphNode foreignGraphNode = foreignObj as IGraphNode;
							if (foreignGraphNode != null && foreignGraphNode == graphNode)
							{
								Port localPort = connection.GetLocalPort(this);
								if (localPort != null)
								{
									if (localPort == headPort)
									{
										return true;
									}
								}
							}
						}
					}
				}
			}

			return false;
		}

		#endregion

		#region IPropertyContainer interface

		/// <summary>
		/// Sets the default property values for the link.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// link to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("FillType", Syncfusion.Windows.Forms.Diagram.FillStyle.FillType.Solid);
			this.propertyValues.Add("FillColor", Color.White);
			this.propertyValues.Add("LineColor", Color.Black);
			this.propertyValues.Add("LineWidth", 2.0f);
			this.propertyValues.Add("AllowSelect", true);
			this.propertyValues.Add("AllowVertexEdit", true);
			this.propertyValues.Add("AllowMove", false);
			this.propertyValues.Add("AllowRotate", false);
			this.propertyValues.Add("AllowResize", false);
		}

		#endregion

		#region Labels

		/// <summary>
		/// Collection of labels that belong to the link.
		/// </summary>
		[
		Browsable(true),
		Category("Appearance"),
		Description("Collection of text labels attached to the symbol"),
		Editor(typeof(LinkLabelCollectionEditor), typeof(UITypeEditor))
		]
		public override LabelCollection Labels
		{
			get
			{
				return base.Labels;
			}
		}

		/// <summary>
		/// Adds a label to a link.
		/// </summary>
		/// <param name="lbl">Label to add</param>
		public override void AddLabel(Label lbl)
		{
			lbl.Container = this;
			lbl.UpdateBounds();
			this.Labels.Add(lbl);
		}

		/// <summary>
		/// Creates a new link label and adds it to the link.
		/// </summary>
		/// <param name="txtVal">Value to assign to the label</param>
		/// <param name="pctAlongLine">Orientation value</param>
		/// <returns>The new label created</returns>
		public Label AddLabel(string txtVal, int pctAlongLine)
		{
			Label label = new LinkLabel(this, txtVal, pctAlongLine);
			label.UpdateBounds();
			this.Labels.Add(label);
			return label;
		}

		/// <summary>
		/// Adds a link label to the link.
		/// </summary>
		/// <param name="label">Label to add</param>
		public void AddLabel(LinkLabel label)
		{
			label.Container = this;
			label.UpdateBounds();
			this.Labels.Add(label);
		}

		/// <summary>
		/// Calculates the point at which the given label should be positioned.
		/// </summary>
		/// <remarks>
		/// This method is called by the label itself in order to find out where
		/// it should be positioned.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer"/>
		/// </remarks>
		public override PointF CalcLabelPosition(Label label)
		{
			PointF ptLabel = new PointF(0,0);
			bool pctAlongFound = false;

			int pctAlongLine = (int) label.GetPropertyValue("LabelPercentAlongLine");

			IPoints pts = this.Points;
			int vertexIdx1;
			int vertexIdx2;
			PointF[] ptAlongLine = new PointF[1];

			if (pts != null)
			{
				pctAlongFound = Geometry.CalcPercentageAlong(pts, pctAlongLine, out ptAlongLine[0], out vertexIdx1, out vertexIdx2);

				if (pctAlongFound)
				{
					Matrix worldXform = this.WorldTransform;
					worldXform.TransformPoints(ptAlongLine);
					ptLabel = ptAlongLine[0];
				}
			}

			if (!pctAlongFound)
			{
				// Unable to calculate label position based on percentage
				// along the line. Use the center point of the bounding box
			}

			return ptLabel;
		}

		#endregion
	}

	/// <summary>
	/// Delegate used to create links.
	/// </summary>
	public delegate Link LinkFactory(PointF[] pts);
}