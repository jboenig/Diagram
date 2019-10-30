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
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for symbols and links.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class provides the base implementation for both symbols and links.
	/// Symbols and links are similar in they are both composite nodes that
	/// contain ports and labels. Both implement the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/> interface,
	/// which exposes a collection of ports and supports connections to other
	/// port containers. When discussing ports and connections, symbols and links
	/// are frequently referred to as "port containers".
	/// </para>
	/// <para>
	/// One key difference between symbols and links is that links have two
	/// built-in ports: a head port and a tail port. Therefore, a link
	/// has a direction and a symbol does not.
	/// </para>
	/// <para>
	/// Another difference between symbols and links is the way in which they
	/// behave during graph navigation. Symbols implement the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IGraphNode"/> interface
	/// and links implement the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IGraphEdge"/> interface.
	/// This means that when the node hierarchy is navigated as a graph,
	/// symbols are nodes and links are edges.
	/// </para>
	/// <para>
	/// The documentation in this class uses the term "symbol" to mean both
	/// symbols and links. Refer to the documentation for the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Symbol"/>
	/// class and the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Link"/>
	/// class for more details.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICompositeNode"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IBounds2DF"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILocalBounds2DF"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphics"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ITransform"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IHitTestBounds"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IHitTestRegion"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Symbol"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link"/>
	/// </remarks>
	[Serializable]
	public abstract class SymbolBase : ICompositeNode, IDispatchNodeEvents, IBounds2DF, ILocalBounds2DF, IGraphics, ITransform, ILogicalUnitContainer, IHitTestBounds, IHitTestRegion, IPropertyContainer, IPortContainer, ILabelContainer, ISerializable, IDeserializationCallback
	{
		#region Constructors

		/// <summary>
		/// Construct a symbol
		/// </summary>
		public SymbolBase()
		{
			this.matrix = new Matrix();

			this.children = new NodeCollection();
			this.children.Changing += new NodeCollection.EventHandler(Children_Changing);
			this.children.ChangeComplete += new NodeCollection.EventHandler(Children_ChangeComplete);

			this.connections = new ConnectionCollection();
			this.connections.Changing += new ConnectionCollection.EventHandler(Connections_Changing);
			this.connections.ChangeComplete += new ConnectionCollection.EventHandler(Connections_ChangeComplete);

			this.ports = new PortCollection();
			this.ports.Changing += new PortCollection.EventHandler(Ports_Changing);
			this.ports.ChangeComplete += new PortCollection.EventHandler(Ports_ChangeComplete);

			this.centerPort = CreateCenterPort();

			this.labels = new LabelCollection();
			this.labels.Changing += new LabelCollection.EventHandler(Labels_Changing);
			this.labels.ChangeComplete += new LabelCollection.EventHandler(Labels_ChangeComplete);

			this.propertyValues = new Hashtable();
			this.editStyle = new EditStyle(this);
			this.fillStyle = new FillStyle(this);
			this.lineStyle = new LineStyle(this);
			SetDefaultPropertyValues();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy from</param>
		public SymbolBase(SymbolBase src)
		{
			this.matrix = src.matrix.Clone();

			this.children = (NodeCollection) src.children.Clone();
			this.children.Changing += new NodeCollection.EventHandler(Children_Changing);
			this.children.ChangeComplete += new NodeCollection.EventHandler(Children_ChangeComplete);

			this.connections = (ConnectionCollection) src.connections.Clone();
			this.connections.Changing += new ConnectionCollection.EventHandler(Connections_Changing);
			this.connections.ChangeComplete += new ConnectionCollection.EventHandler(Connections_ChangeComplete);

			this.ports = (PortCollection) src.ports.Clone();
			this.ports.Changing += new PortCollection.EventHandler(Ports_Changing);
			this.ports.ChangeComplete += new PortCollection.EventHandler(Ports_ChangeComplete);

			this.centerPort = (Port) src.centerPort.Clone();

			this.labels = (LabelCollection) src.labels.Clone();
			this.labels.Changing += new LabelCollection.EventHandler(Labels_Changing);
			this.labels.ChangeComplete += new LabelCollection.EventHandler(Labels_ChangeComplete);

			this.propertyValues = (Hashtable) src.propertyValues.Clone();
			this.editStyle = new EditStyle(this);
			this.fillStyle = new FillStyle(this);
			this.lineStyle = new LineStyle(this);
		}

		/// <summary>
		/// Serialization constructor for symbols.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected SymbolBase(SerializationInfo info, StreamingContext context)
		{
			this.name = info.GetString("name");
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
			this.parent = (ICompositeNode) info.GetValue("parent", typeof(ICompositeNode));
			float m11 = info.GetSingle("m11");
			float m12 = info.GetSingle("m12");
			float m21 = info.GetSingle("m21");
			float m22 = info.GetSingle("m22");
			float dx = info.GetSingle("dx");
			float dy = info.GetSingle("dy");
			this.matrix = new Matrix(m11, m12, m21, m22, dx, dy);
			this.children = (NodeCollection) info.GetValue("children", typeof(NodeCollection));
			this.ports = (PortCollection) info.GetValue("ports", typeof(PortCollection));
			this.connections = (ConnectionCollection) info.GetValue("connections", typeof(ConnectionCollection));
			this.labels = (LabelCollection) info.GetValue("labels", typeof(LabelCollection));
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public abstract object Clone();

		#endregion

		#region Public Properties

		/// <summary>
		/// Determines if the symbol is visible or hidden.
		/// </summary>
		[
		Browsable(true),
		Category("Appearance")
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
		/// Collection of child nodes belonging to the symbol.
		/// </summary>
		/// <remarks>
		/// Changes made to this collection will cause one or more of the following
		/// methods to be called:
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnChildrenChanging"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnChildrenChangeComplete"/>,
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.INode"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public NodeCollection Nodes
		{
			get
			{
				return this.children;
			}
		}

		/// <summary>
		/// Upper-left hand corner of the bounding box.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Bounds"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public PointF Location
		{
			get
			{
				return this.Bounds.Location;
			}
		}

		/// <summary>
		/// Size of the bounding box.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Bounds"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public SizeF Size
		{
			get
			{
				return this.Bounds.Size;
			}
		}

		#endregion

		#region Styles

		/// <summary>
		/// Set of properties that determine how the symbol can be manipulated.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Edit properties contain a set of flags that indicate what types of
		/// activities are valid for the symbol.
		/// </para>
		/// <para>
		/// Edit style properties that are not explicitly set by the symbol are
		/// inherited from the parent node.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.EditStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.FillStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.LineStyle"/>
		/// </remarks>
		[
		Browsable(true),
		TypeConverter(typeof(EditStyleConverter)),
		Category("Behavior"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public EditStyle EditStyle
		{
			get
			{
				return this.editStyle;
			}
		}

		/// <summary>
		/// Properties used to fill the interior of regions.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The fill style is used to create brushes for painting interior regions of
		/// shapes.
		/// </para>
		/// <para>
		/// Fill style properties that are not explicitly set by the symbol are
		/// inherited from the parent node.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.FillStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.EditStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.LineStyle"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Appearance"),
		TypeConverter(typeof(FillStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public FillStyle FillStyle
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
		/// Line style properties that are not explicitly set by the symbol are
		/// inherited from the parent node.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.FillStyle"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.EditStyle"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Appearance"),
		TypeConverter(typeof(LineStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public LineStyle LineStyle
		{
			get
			{
				return this.lineStyle;
			}
		}

		#endregion

		#region Ports

		/// <summary>
		/// Collection of ports belonging to the symbol.
		/// </summary>
		/// <remarks>
		/// Changes made to this collection will cause one or more of the following
		/// methods to be called:
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnPortsChanging"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnPortsChangeComplete"/>.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PortCollection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public PortCollection Ports
		{
			get
			{
				return this.ports;
			}
		}

		/// <summary>
		/// Built-in port at the center of the symbol.
		/// </summary>
		/// <remarks>
		/// <para>
		/// All symbols have a built-in port located at the center of the
		/// symbol.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Port CenterPort
		{
			get
			{
				return this.centerPort;
			}
		}

		/// <summary>
		/// Hit test for ports.
		/// </summary>
		/// <param name="ptWorld">Point to test</param>
		/// <param name="fSlop">Size of area around the point to test</param>
		/// <returns>
		/// The port object at the given point or null if there is no port
		/// at the given point
		/// </returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		public virtual Port GetPortAt(PointF ptWorld, float fSlop)
		{
			Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);

			Port portHit = null;
			IEnumerator enumPorts = this.ports.GetEnumerator();

			while (enumPorts.MoveNext() && portHit == null)
			{
				Port curPort = enumPorts.Current as Port;

				if (curPort.Enabled)
				{
					IHitTestBounds curPortHitTest = curPort as IHitTestBounds;

					if (curPortHitTest != null && curPortHitTest.ContainsPoint(ptWorld, fSlop))
					{
						portHit = curPort;
					}
				}
			}

			Global.MatrixStack.Pop();

			if (portHit == null && this.centerPort != null && this.centerPort.Enabled)
			{
				IHitTestBounds thisHitTest = this as IHitTestBounds;

				if (thisHitTest != null && thisHitTest.ContainsPoint(ptWorld, fSlop))
				{
					portHit = this.centerPort;
				}
			}

			return portHit;
		}

		/// <summary>
		/// Determines if ports belonging to this symbol are only visible when
		/// a user interface <see cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
		/// is creating a connection.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Behavior"),
		Description("Determines if ports belonging to this symbol are visible only when a user interface tool is creating a connection")
		]
		public bool AutoHidePorts
		{
			get
			{
				object value = this.GetPropertyValue("AutoHidePorts");
				if (value != null && value.GetType() == typeof(bool))
				{
					return (bool) value;
				}
				return false;
			}
			set
			{
				this.SetPropertyValue("AutoHidePorts", value);
			}
		}

		/// <summary>
		/// Called before a port is inserted into the symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>This method can be overridden in derived classes.</para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PortCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnPortsChanging(PortCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// Called after a port is inserted into the symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>This method can be overridden in derived classes.</para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PortCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnPortsChangeComplete(PortCollection.EventArgs evtArgs)
		{
		}

		#endregion

		#region Connections

		/// <summary>
		/// Set of connections this symbol has to other symbols.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Connections can be created and added to this collection by calling the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Connect"/>
		/// method. The
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Disconnect"/>
		/// method will remove a connection from this collection.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Connect"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Disconnect"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public ConnectionCollection Connections
		{
			get
			{
				return this.connections;
			}
		}

		/// <summary>
		/// Tests to see if a connection between the two given ports is allowed.
		/// </summary>
		/// <param name="sourcePort">Source port to test</param>
		/// <param name="targetPort">Target port to test</param>
		/// <returns>
		/// true if a connection is allowed between the two ports; otherwise false
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method gives the symbol an opportunity to reject a connection
		/// between two ports. Both port containers being connected are asked
		/// for permission to create the connection.
		/// </para>
		/// <para>
		/// The default implementation always returns true. This method can be
		/// overridden in derived classes.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		public virtual bool AcceptConnection(Port sourcePort, Port targetPort)
		{
			return true;
		}

		/// <summary>
		/// Create a new connection between the given ports.
		/// </summary>
		/// <param name="sourcePort">First port to connect</param>
		/// <param name="targetPort">Second port to connect</param>
		/// <returns>New connection object or null on failure</returns>
		/// <remarks>
		/// <para>
		/// This method first calls the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Port.AcceptConnection"/>
		/// method on each port in order to ask permission to create the
		/// connection. If both ports consent to the connection, a new
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Connection"/> object
		/// is created to bind the two ports together. Then the connection is
		/// added to the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Connections"/>
		/// collection of both port containers.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Disconnect"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Connections"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
		/// </remarks>
		public virtual Connection Connect(Port sourcePort, Port targetPort)
		{
			Connection connection = this.connections.Find(sourcePort, targetPort);

			if (connection == null)
			{
				if (sourcePort.AcceptConnection(targetPort) && targetPort.AcceptConnection(sourcePort))
				{
					connection = new Connection(sourcePort, targetPort);
					this.connections.Add(connection);
				}
			}

			return connection;
		}

		/// <summary>
		/// Removes the connection containing the specified ports from the symbol.
		/// </summary>
		/// <param name="sourcePort">Source port of connection</param>
		/// <param name="targetPort">Target port of connection</param>
		/// <remarks>
		/// If no connection is found between the two ports, then this method
		/// does nothing.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Connect"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		public virtual void Disconnect(Port sourcePort, Port targetPort)
		{
			this.connections.Remove(sourcePort, targetPort);
		}

		/// <summary>
		/// Remove all connections on the given port.
		/// </summary>
		/// <param name="port">Port to disconnect</param>
		/// <returns>The number of connections removed</returns>
		/// <remarks>
		/// This method finds all of the connections on the specified port and
		/// removes each one from the symbol.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.GetConnectionsOnPort"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Connections"/>
		/// </remarks>
		public virtual int DisconnectAll(Port port)
		{
			int numConnections = 0;
			ConnectionCollection connections = new ConnectionCollection();

			if (this.ports.Contains(port))
			{
				numConnections = this.GetConnectionsOnPort(connections, port);
				foreach (Connection connection in connections)
				{
					this.connections.Remove(connection);
				}
			}

			return numConnections;
		}

		/// <summary>
		/// Returns all connections to a specified port.
		/// </summary>
		/// <param name="connections">Collection to add the connections to</param>
		/// <param name="port">Port to search</param>
		/// <returns>Number of connections found to the port</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		public int GetConnectionsOnPort(ConnectionCollection connections, Port port)
		{
			int numFound = 0;
			IEnumerator enumConnections = this.connections.GetEnumerator();
			Connection curConnection;

			while (enumConnections.MoveNext())
			{
				curConnection = enumConnections.Current as Connection;
				if (curConnection != null)
				{
					if (curConnection.SourcePort == port || curConnection.TargetPort == port)
					{
						connections.Add(curConnection);
						numFound++;
					}
				}
			}

			return numFound;
		}

		/// <summary>
		/// Returns the port referenced by the given connection that belongs to
		/// this symbol.
		/// </summary>
		/// <param name="connection">Connection to search</param>
		/// <returns>Port belonging to this symbol</returns>
		/// <remarks>
		/// <para>
		/// A connection consists of two ports. The local port on a connection is
		/// the one that belongs to this symbol. The foreign port is the one that
		/// belongs to the other symbol.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		public Port GetLocalPortOnConnection(Connection connection)
		{
			return connection.GetLocalPort(this);
		}

		/// <summary>
		/// Returns the port referenced by the given connection that belongs to
		/// the foreign symbol.
		/// </summary>
		/// <param name="connection">Connection to search</param>
		/// <returns>Port belonging to the foreign symbol</returns>
		/// <remarks>
		/// <para>
		/// A connection consists of two ports. The local port on a connection is
		/// the one that belongs to this symbol. The foreign port is the one that
		/// belongs to the other symbol.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		public Port GetForeignPortOnConnection(Connection connection)
		{
			return connection.GetForeignPort(this);
		}

		/// <summary>
		/// Adds the specified connection to the foreign port container.
		/// </summary>
		/// <param name="connection">Connection to add</param>
		/// <remarks>
		/// <para>
		/// A connection binds together two ports on two separate
		/// port containers (symbol, link, etc). The local port container
		/// is the one that owns this collection. The foreign port container
		/// is the other one involved in the connection. Both port containers
		/// maintain a list of the connections and must be kept synchronized.
		/// This method ensures that the given connection is added to the
		/// foreign port container, if it is not already there.
		/// </para>
		/// </remarks>
		protected void ForeignContainerAdd(Connection connection)
		{
			if (connection != null)
			{
				Port foreignPort = connection.GetForeignPort(this);
				if (foreignPort != null)
				{
					IPortContainer foreignContainer = foreignPort.Container;
					if (foreignContainer != null)
					{
						if (!foreignContainer.Connections.Contains(connection))
						{
							foreignContainer.Connections.Add(connection);
						}
					}
				}
			}
		}

		/// <summary>
		/// Removes the specified connection from the foreign port container.
		/// </summary>
		/// <param name="connection">Connection to remove</param>
		/// <remarks>
		/// <para>
		/// A connection binds together two ports on two separate
		/// port containers (symbol, link, etc). The local port container
		/// is the one that owns this collection. The foreign port container
		/// is the other one involved in the connection. Both port containers
		/// maintain a list of the connections and must be kept synchronized.
		/// This method ensures that the given connection is removed from the
		/// foreign port container, if it is there.
		/// </para>
		/// </remarks>
		protected void ForeignContainerRemove(Connection connection)
		{
			if (connection != null)
			{
				Port foreignPort = connection.GetForeignPort(this);
				if (foreignPort != null)
				{
					IPortContainer foreignContainer = foreignPort.Container;
					if (foreignContainer != null)
					{
						if (foreignContainer.Connections.Contains(connection))
						{
							foreignContainer.Connections.Remove(connection);
						}
					}
				}
			}
		}

		/// <summary>
		/// Called before a change is made to the set of connection belonging to
		/// the symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>This method can be overridden in derived classes.</para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnConnectionsChanging(ConnectionCollection.EventArgs evtArgs)
		{
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
		/// Called after a change is made to the set of connection belonging to
		/// the symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>This method can be overridden in derived classes.</para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs)
		{
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
		/// This method is called by connections when the foreign port container
		/// moves.
		/// </summary>
		/// <param name="connection">Connection that has moved</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection"/>
		/// </remarks>
		public abstract void OnConnectionMove(Connection connection);

		#endregion

		#region Labels

		/// <summary>
		/// Collection of labels belonging to this symbol.
		/// </summary>
		/// <remarks>
		/// Changes made to this collection will cause one or more of the following
		/// methods to be called:
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnLabelsChanging"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnLabelsChangeComplete"/>.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LabelCollection"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label"/>
		/// </remarks>
		public virtual LabelCollection Labels
		{
			get
			{
				return this.labels;
			}
		}

		/// <summary>
		/// Adds a label to the symbol.
		/// </summary>
		/// <param name="lbl">Label to add</param>
		public abstract void AddLabel(Label lbl);

		/// <summary>
		/// Calculates the point at which the given label should be positioned.
		/// </summary>
		/// <remarks>
		/// This method is called by the label itself in order to find out where
		/// it should be positioned.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer"/>
		/// </remarks>
		public virtual PointF CalcLabelPosition(Label label)
		{
			PointF ptLabel = new PointF(0,0);

			float left = float.MaxValue;
			float top = float.MaxValue;
			float right = float.MinValue;
			float bottom = float.MinValue;
			RectangleF curBounds;

			int prevStack = Global.SelectMatrixStack(Global.TemporaryStack);
			Global.MatrixStack.Clear();
			Global.MatrixStack.Push(this.matrix);

			foreach (INode curNode in this.children)
			{
				if (curNode.GetType() != typeof(Label))
				{
					ILocalBounds2DF curNodeBounds = curNode as ILocalBounds2DF;

					if (curNodeBounds != null)
					{
						curBounds = curNodeBounds.Bounds;

						if (curBounds.Left < left)
						{
							left = curBounds.Left;
						}
						if (curBounds.Right > right)
						{
							right = curBounds.Right;
						}
						if (curBounds.Top < top)
						{
							top = curBounds.Top;
						}
						if (curBounds.Bottom > bottom)
						{
							bottom = curBounds.Bottom;
						}
					}
				}
			}

			Global.MatrixStack.Clear();
			Global.SelectMatrixStack(prevStack);

			RectangleF rcBounds = new RectangleF(left, top, right - left, bottom - top);
			BoxPosition labelAnchor = (BoxPosition) label.GetPropertyValue("LabelAnchor");
			ptLabel = Geometry.GetAnchorPoint(rcBounds, labelAnchor);

			return ptLabel;
		}

		/// <summary>
		/// Builds a world transformation matrix for a label.
		/// </summary>
		/// <param name="labelTransform">Local transformation matrix of a label</param>
		/// <returns>World transformation matrix</returns>
		/// <remarks>
		/// This method prepends the symbol's parent transformation to the incoming
		/// matrix and returns the result. The purpose of doing this is to avoid
		/// including the symbol's local transformation matrix in the label's world
		/// transformation matrix. This prevents labels from being scaled and rotated
		/// when the symbol to which they are attached is scaled or rotated.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer"/>
		/// </remarks>
		public virtual Matrix GetLabelTransform(Matrix labelTransform)
		{
			Matrix matrix = this.ParentTransform;
			matrix.Multiply(labelTransform, MatrixOrder.Prepend);
			return matrix;
		}

		/// <summary>
		/// Called before a change is made to the collection of labels belonging
		/// to the symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>This method can be overridden in derived classes.</para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LabelCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnLabelsChanging(LabelCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// Called after a change is made to the collection of labels belonging
		/// to the symbol.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>This method can be overridden in derived classes.</para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LabelCollection.EventArgs"/>
		/// </remarks>
		protected virtual void OnLabelsChangeComplete(LabelCollection.EventArgs evtArgs)
		{
		}

		#endregion

		#region IDispatchNodeEvents interface

		/// <summary>
		/// Called when a property value is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnPropertyChanged"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnChildrenChanging"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnChildrenChangeComplete"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnBoundsChanged"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnMove"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnRotate"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnScale"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnClick"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnDoubleClick"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnMouseEnter"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnMouseLeave"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnInsertVertex"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnDeleteVertex"/>
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
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnMoveVertex"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.MoveVertex(VertexEventArgs evtArgs)
		{
			this.OnMoveVertex(evtArgs);
		}

		/// <summary>
		/// Called before the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnConnectionsChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ConnectionsChanging(ConnectionCollection.EventArgs evtArgs)
		{
			this.OnConnectionsChanging(evtArgs);
		}

		/// <summary>
		/// Called after the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnConnectionsChangeComplete"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs)
		{
			this.OnConnectionsChangeComplete(evtArgs);
		}

		#endregion

		#region Node Event Callbacks

		/// <summary>
		/// Called before a change is made to the collection of child nodes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.ChildrenChanging"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnChildrenChanging(NodeCollection.EventArgs evtArgs)
		{
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
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.ChildrenChangeComplete"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnChildrenChangeComplete(NodeCollection.EventArgs evtArgs)
		{
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
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Parent"/>
		/// of the property change by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.PropertyChanged"/>
		/// method.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnPropertyChanged(PropertyEventArgs evtArgs)
		{
			if (evtArgs.PropertyName == "Name")
			{
				// Resize labels that are bound to the symbol name
				foreach (Label label in this.labels)
				{
					if (label.PropertyBinding == LabelPropertyBinding.ContainerName)
					{
						label.SizeToText(this.Size);
					}
				}
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
		/// Called when the position of the node is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Parent"/>
		/// of the move by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Move"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMove(MoveEventArgs evtArgs)
		{
			foreach (Connection conn in this.connections)
			{
				conn.SymbolMoved(this);
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
		/// Called when the node is rotated.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
		/// of the rotation by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Rotate"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RotateEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnRotate(RotateEventArgs evtArgs)
		{
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
		/// Called when the node is scaled.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
		/// of the scaling by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Scale"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ScaleEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnScale(ScaleEventArgs evtArgs)
		{
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
		/// Called when the bounds of the node change.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Parent"/>
		/// of the change in bounds by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.BoundsChanged"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoundsEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnBoundsChanged(BoundsEventArgs evtArgs)
		{
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
		/// Called when a node is clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Click"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnClick(NodeMouseEventArgs evtArgs)
		{
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
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.DoubleClick"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnDoubleClick(NodeMouseEventArgs evtArgs)
		{
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
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.MouseEnter"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMouseEnter(NodeMouseEventArgs evtArgs)
		{
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
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.MouseLeave"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMouseLeave(NodeMouseEventArgs evtArgs)
		{
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
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.InsertVertex"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnInsertVertex(VertexEventArgs evtArgs)
		{
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
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.MoveVertex"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMoveVertex(VertexEventArgs evtArgs)
		{
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
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.DeleteVertex"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnDeleteVertex(VertexEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.DeleteVertex(evtArgs);
				}
			}
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IBounds2DF"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </para>
		/// </remarks>
		object IServiceProvider.GetService(System.Type svcType)
		{
			if (svcType == typeof(IPropertyContainer))
			{
				return this;
			}
			else if (svcType == typeof(IBounds2DF))
			{
				return (IBounds2DF) this;
			}
			else if (svcType == typeof(IDispatchNodeEvents))
			{
				return (IDispatchNodeEvents) this;
			}
			return null;
		}

		#endregion

		#region ICompositeNode interface

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
		/// The root node in the node hierarchy.
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
		/// Name of the symbol.
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
				if (this.name != value)
				{
					string oldVal = this.name;
					this.name = value;
					this.OnPropertyChanged(new PropertyEventArgs(this, "Name", oldVal, this.name));
				}
			}
		}

		/// <summary>
		/// Fully qualified name of the node.
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

		/// <summary>
		/// The number of child nodes contained by this symbol.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int ChildCount
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
		public INode GetChild(int childIndex)
		{
			return this.children[childIndex];
		}

		/// <summary>
		/// Returns the child node matching the given name.
		/// </summary>
		/// <param name="childName">Name of node to return</param>
		/// <returns>Node matching the given name</returns>
		public INode GetChildByName(string childName)
		{
			INode nodeFound = null;
			IEnumerator enumChildren = this.children.GetEnumerator();
			while (nodeFound == null && enumChildren.MoveNext())
			{
				INode curNode = enumChildren.Current as INode;
				if (curNode != null)
				{
					if (curNode.Name == childName)
					{
						nodeFound = curNode;
					}
				}
			}

			return nodeFound;
		}

		/// <summary>
		/// Returns the index position of the given child node.
		/// </summary>
		/// <param name="child">Child node to query</param>
		/// <returns>Zero-based index into the collection of child nodes</returns>
		public int GetChildIndex(INode child)
		{
			return this.children.Find(child);
		}

		/// <summary>
		/// Appends the given node to the collection of child nodes.
		/// </summary>
		/// <param name="child">Node to append</param>
		/// <returns>
		/// Zero-based index at which the node was added to the collection or -1 for failure.
		/// </returns>
		public int AppendChild(INode child)
		{
			int childIndex = -1;
			
			if (child != null)
			{
				childIndex = this.children.Add(child);
				child.Parent = this;
			}

			return childIndex;
		}

		/// <summary>
		/// Insert the given node into the collection of child nodes at a
		/// specific position.
		/// </summary>
		/// <param name="child">Node to insert</param>
		/// <param name="childIndex">Zero-based index at which to insert the node</param>
		public void InsertChild(INode child, int childIndex)
		{
			this.children.Insert(childIndex, child);
			child.Parent = this;
		}

		/// <summary>
		/// Removes the child node at the given position.
		/// </summary>
		/// <param name="childIndex">Zero-based index into the collection of child nodes</param>
		public void RemoveChild(int childIndex)
		{
			INode child = this.GetChild(childIndex);

			if (child != null)
			{
				child.Parent = null;
				this.children.RemoveAt(childIndex);
			}
		}

		/// <summary>
		/// Removes all child nodes from the node.
		/// </summary>
		public void RemoveAllChildren()
		{
			foreach (INode curChild in this.children)
			{
				curChild.Parent = null;
			}
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
		/// </remarks>
		public System.Drawing.Region GetConstrainingRegion(INode child)
		{
			if (this.parent != null)
			{
				return this.parent.GetConstrainingRegion(child);
			}
			return null;
		}

		/// <summary>
		/// Returns all children that are intersected by the given point.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children hit by the given point
		/// </param>
		/// <param name="ptWorld">Point to test</param>
		/// <returns>The number of child nodes that intersect the given point</returns>
		public int GetChildrenAtPoint(NodeCollection childNodes, PointF ptWorld)
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
		public int GetChildrenIntersecting(NodeCollection childNodes, RectangleF rcWorld)
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
		public int GetChildrenContainedBy(NodeCollection childNodes, RectangleF rcWorld)
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
			return (IPropertyContainer) this;
		}

		#endregion

		#region IBounds2DF interface

		/// <summary>
		/// The symbol's bounding box.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Always returns the bounds of the symbol in world coordinates, regardless
		/// of what is on the matrix stack at the time of the call.
		/// </para>
		/// <para>
		/// The bounding box of a symbol is the union of the bounds of all of its
		/// children. This method pushes the symbol's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.WorldTransform"/>
		/// onto the matrix stack and then iterates through each child retrieving
		/// it's local bounds through the ILocalBounds2DF.
		/// </para>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public RectangleF Bounds
		{
			get
			{
				float left = float.MaxValue;
				float top = float.MaxValue;
				float right = float.MinValue;
				float bottom = float.MinValue;
				RectangleF curBounds;

				Global.MatrixStack.Clear();
				Global.MatrixStack.Push(this.WorldTransform, MatrixOrder.Prepend);

				foreach (INode curChild in this.children)
				{
					if (curChild.GetType() != typeof(Label) && (!this.AutoHidePorts || !curChild.GetType().IsSubclassOf(typeof(Port))))
					{
						ILocalBounds2DF childBounds = curChild as ILocalBounds2DF;

						if (childBounds != null)
						{
							curBounds = childBounds.Bounds;

							if (curBounds.Left < left)
							{
								left = curBounds.Left;
							}
							if (curBounds.Right > right)
							{
								right = curBounds.Right;
							}
							if (curBounds.Top < top)
							{
								top = curBounds.Top;
							}
							if (curBounds.Bottom > bottom)
							{
								bottom = curBounds.Bottom;
							}
						}
					}
				}

				Global.MatrixStack.Pop();

				return new RectangleF(left, top, right - left, bottom - top);
			}
			set
			{
				// Get the origin relative to the parent
				Global.MatrixStack.Clear();
				Global.MatrixStack.Push(this.ParentTransform);
				RectangleF bounds = this.Bounds;
				PointF curOrigin = Geometry.CenterPoint(bounds);
				// Get the new origin
				PointF newOrigin = Geometry.CenterPoint(value);
				// Calculate translation vector
				float offsetX = newOrigin.X - curOrigin.X;
				float offsetY = newOrigin.Y - curOrigin.Y;
				// Calculate scale
				float scaleX = value.Width / bounds.Width;
				float scaleY = value.Height / bounds.Height;
				// Apply transformations
				this.matrix.Translate(-curOrigin.X, -curOrigin.Y, MatrixOrder.Append);
				this.matrix.Scale(scaleX, scaleY, MatrixOrder.Append);
				this.matrix.Translate(curOrigin.X, curOrigin.Y, MatrixOrder.Append);
				this.matrix.Translate(offsetX, offsetY, MatrixOrder.Append);
				// Fire bounds change event
				this.OnBoundsChanged(new BoundsEventArgs(this, bounds, this.Bounds));
			}
		}

		/// <summary>
		/// X-coordinate of the object's location.
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
				return this.Location.X;
			}
			set
			{
				float curX = this.Bounds.Location.X;
				float dx = value - curX;
				this.Translate(dx, 0);
			}
		}

		/// <summary>
		/// Y-coordinate of the object's location.
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
				return this.Location.Y;
			}
			set
			{
				float curY = this.Bounds.Location.Y;
				float dy = value - curY;
				this.Translate(0, dy);
			}
		}

		/// <summary>
		/// Width of the object.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual float Width
		{
			get
			{
				return this.Size.Width;
			}
			set
			{
				float curWidth = this.Bounds.Size.Width;
				float sx = value / curWidth;
				this.Scale(sx, 1.0f);
			}
		}

		/// <summary>
		/// Height of the object.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual float Height
		{
			get
			{
				return this.Size.Height;
			}
			set
			{
				float curHeight = this.Bounds.Size.Height;
				float sy = value / curHeight;
				this.Scale(1.0f, sy);
			}
		}

		#endregion

		#region ILocalBounds2DF interface

		/// <summary>
		/// Bounding box of the symbol in local coordinates.
		/// </summary>
		/// <remarks>
		/// The value returned depends on the contents of the matrix stack. If the
		/// matrix stack is empty, then the value returned is in local coordinates.
		/// This method is generally used by functions that recursively traverse
		/// the node hierarchy, pushing and popping the matrix stack as they go.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Global.MatrixStack"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Bounds"/>
		/// </remarks>
		RectangleF ILocalBounds2DF.Bounds
		{
			get
			{
				float left = float.MaxValue;
				float top = float.MaxValue;
				float right = float.MinValue;
				float bottom = float.MinValue;
				RectangleF curBounds;

				Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);

				foreach (INode curChild in this.children)
				{
					if (curChild.GetType() != typeof(Label) && !curChild.GetType().IsSubclassOf(typeof(Port)))
					{
						ILocalBounds2DF childBounds = curChild as ILocalBounds2DF;

						if (childBounds != null)
						{
							curBounds = childBounds.Bounds;
							if (curBounds.Left < left)
							{
								left = curBounds.Left;
							}
							if (curBounds.Right > right)
							{
								right = curBounds.Right;
							}
							if (curBounds.Top < top)
							{
								top = curBounds.Top;
							}
							if (curBounds.Bottom > bottom)
							{
								bottom = curBounds.Bottom;
							}
						}
					}
				}

				Global.MatrixStack.Pop();

				return new RectangleF(left, top, right - left, bottom - top);
			}
			set
			{
				throw new EInvalidOperation();
			}
		}

		/// <summary>
		/// X-coordinate of the object's location.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.X
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Left;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Location = new PointF(value, rcBounds.Top);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Y-coordinate of the object's location.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.Y
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Top;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Location = new PointF(rcBounds.Left, value);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Width of the object.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.Width
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Width;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Size = new SizeF(value, rcBounds.Height);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Height of the object.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.Height
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Height;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Size = new SizeF(rcBounds.Width, value);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		#endregion

		#region IGraphics interface

		/// <summary>
		/// Renders the symbol onto a System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Graphics context to render onto</param>
		/// <remarks>
		/// Iterates through all child nodes and renders them.
		/// </remarks>
		public virtual void Draw(System.Drawing.Graphics grfx)
		{
			if (this.Visible)
			{
				Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);

				foreach (INode curChild in this.children)
				{
					IDraw drawObj = curChild as IDraw;
					if (drawObj != null)
					{
						drawObj.Draw(grfx);
					}
				}

				Global.MatrixStack.Pop();
			}
		}

		/// <summary>
		/// Encapsulates the points and instructions needed to render the symbol.
		/// </summary>
		/// <remarks>
		/// The contents of the GraphicsPath is the union of the GraphicsPath
		/// objects of all its children.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.CreateRegion"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual System.Drawing.Drawing2D.GraphicsPath GraphicsPath
		{
			get
			{
				GraphicsPath grfxPath = new GraphicsPath();

				Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);

				foreach (INode curChild in this.children)
				{
					IGraphics graphicsObj = curChild as IGraphics;
					if (graphicsObj != null)
					{
						GraphicsPath grfxChildPath = graphicsObj.GraphicsPath;
						if (grfxChildPath != null)
						{
							grfxPath.AddPath(grfxChildPath, false);
						}
					}
				}

				Global.MatrixStack.Pop();

				return grfxPath;
			}
		}

		/// <summary>
		/// Returns an object that describes the interior of the shape.
		/// </summary>
		/// <param name="padding">Amount of padding to add</param>
		/// <returns>System.Drawing.Region object</returns>
		/// <remarks>
		/// Region objects are used for hit testing and geometrical calculations.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.GraphicsPath"/>
		/// </remarks>
		public virtual System.Drawing.Region CreateRegion(float padding)
		{
#if true
			GraphicsPath grfxPath = this.GraphicsPath;

			if (grfxPath != null)
			{
				return new System.Drawing.Region(grfxPath);
			}

			return new System.Drawing.Region(this.Bounds);
#else
			Region rgn = new Region();

			Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);

			foreach (INode curChild in this.children)
			{
				IGraphics graphicsObj = curChild as IGraphics;
				if (graphicsObj != null)
				{
					Region childRgn = graphicsObj.CreateRegion(padding);
					if (childRgn != null)
					{
						rgn.Union(childRgn);
					}
				}
			}

			Global.MatrixStack.Pop();

			return rgn;
#endif
		}

		#endregion

		#region ITransform interface

		/// <summary>
		/// Moves the symbol by the given X and Y offsets.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		/// <remarks>
		/// Applies a translate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnMove"/>
		/// method is called after the change is made.
		/// </remarks>
		public virtual void Translate(float dx, float dy)
		{
			if (dx != 0 || dy != 0)
			{
				this.matrix.Translate(dx, dy, MatrixOrder.Append);
				this.OnMove(new MoveEventArgs(this, dx, dy));
			}
		}

		/// <summary>
		/// Rotates the symbol a specified number of degrees about a given
		/// anchor point.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to rotate</param>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnRotate"/>
		/// method is called after the change is made.
		/// </remarks>
		public virtual void Rotate(PointF ptAnchor, float degrees)
		{
			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Rotate(degrees, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);
		}

		/// <summary>
		/// Rotates the symbol a specified number of degrees about its center point.
		/// </summary>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnRotate"/>
		/// method is called after the change is made.
		/// </remarks>
		public virtual void Rotate(float degrees)
		{
			PointF ptOrigin = Geometry.CenterPoint(this.Bounds);
			this.matrix.Translate(-ptOrigin.X, -ptOrigin.Y, MatrixOrder.Append);
			this.matrix.Rotate(degrees, MatrixOrder.Append);
			this.matrix.Translate(ptOrigin.X, ptOrigin.Y, MatrixOrder.Append);
		}

		/// <summary>
		/// Scales the symbol by a given ratio along the X and Y axes.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to scale</param>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnScale"/>
		/// method is called after the change is made.
		/// </remarks>
		public virtual void Scale(PointF ptAnchor, float sx, float sy)
		{
			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Scale(sx, sy, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);
		}

		/// <summary>
		/// Scales the symbol about its center point by a given ratio.
		/// </summary>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnScale"/>
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
		/// Matrix containing local transformations for this symbol.
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
		/// Returns a matrix containing transformations for this symbol and all of
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

				worldTransform.Multiply(this.matrix, MatrixOrder.Prepend);

				return worldTransform;
			}
		}

		/// <summary>
		/// Returns a matrix containing the transformations of this symbol's parent.
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
			// Convert line width
			if (this.propertyValues.Contains("LineWidth"))
			{
				float lineWidth = (float) this.propertyValues["LineWidth"];
				lineWidth = Measurements.Convert(fromUnits, toUnits, grfx, lineWidth);
				this.propertyValues["LineWidth"] = lineWidth;
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

		#region IHitTestBounds interface

		/// <summary>
		/// Tests to see if the object's bounding box contains the given point.
		/// </summary>
		/// <param name="ptTest">Point to test</param>
		/// <param name="fSlop">Expands the area to be tested</param>
		/// <returns>true if the object contains the given point, otherwise false</returns>
		bool IHitTestBounds.ContainsPoint(PointF ptTest, float fSlop)
		{
			bool hit = false;

			Point pt = new Point((int) ptTest.X, (int) ptTest.Y);
			System.Drawing.RectangleF bounds = this.Bounds;
			hit = bounds.Contains(pt);

			return hit;
		}

		/// <summary>
		/// Tests to see if the object's bounding box intersects the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if an intersection occurs, otherwise false</returns>
		bool IHitTestBounds.IntersectsRect(RectangleF rcTest)
		{
			System.Drawing.RectangleF bounds = this.Bounds;
			return bounds.IntersectsWith(rcTest);
		}

		/// <summary>
		/// Tests to see if the object's bounding box contains the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if the rectangle is contained by the object, otherwise false</returns>
		bool IHitTestBounds.ContainedByRect(RectangleF rcTest)
		{
			System.Drawing.RectangleF bounds = this.Bounds;
			return rcTest.Contains(bounds);
		}

		#endregion

		#region IHitTestRegion interface

		/// <summary>
		/// Tests to see if the object's region contains the given point.
		/// </summary>
		/// <param name="ptTest">Point to test</param>
		/// <param name="fSlop">Expands the area to be tested</param>
		/// <returns>true if the object contains the given point, otherwise false</returns>
		bool IHitTestRegion.ContainsPoint(PointF ptTest, float fSlop)
		{
#if false
			bool hit = false;
			System.Drawing.Region rgn = this.CreateRegion(fSlop);
			if (rgn != null)
			{
				hit = rgn.IsVisible(ptTest);
			}
			return hit;
#else
			bool hit = false;
			IHitTestRegion curChildHitTest;

			Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);

			IEnumerator enumChildren = this.children.GetEnumerator();

			while (enumChildren.MoveNext() && !hit)
			{
				curChildHitTest = enumChildren.Current as IHitTestRegion;

				if (curChildHitTest != null)
				{
					if (curChildHitTest.ContainsPoint(ptTest, fSlop))
					{
						hit = true;
					}
				}
			}

			Global.MatrixStack.Pop();

			return hit;
#endif
		}

		/// <summary>
		/// Tests to see if the object's region intersects the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if an intersection occurs, otherwise false</returns>
		bool IHitTestRegion.IntersectsRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return bounds.IntersectsWith(rcTest);
		}

		/// <summary>
		/// Tests to see if the object's region contains the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if the rectangle is contained by the object, otherwise false</returns>
		bool IHitTestRegion.ContainedByRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return rcTest.Contains(bounds);
		}

		#endregion

		#region Implementation methods

		/// <summary>
		/// Creates the center port for the symbol.
		/// </summary>
		/// <returns>CenterPort object</returns>
		/// <remarks>
		/// Called by the constructor.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CenterPort"/>
		/// </remarks>
		protected virtual Port CreateCenterPort()
		{
			return new CenterPort(this);
		}

		#endregion

		#region IPropertyContainer interface

		/// <summary>
		/// Sets the default property values for the object.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// object to their default values.
		/// </remarks>
		public virtual void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("FillType", Syncfusion.Windows.Forms.Diagram.FillStyle.FillType.Solid);
			this.propertyValues.Add("FillColor", Color.White);
			this.propertyValues.Add("LineColor", Color.Black);
			this.propertyValues.Add("LineWidth", 2.0f);
			this.propertyValues.Add("AllowSelect", true);
			this.propertyValues.Add("AllowVertexEdit", false);
			this.propertyValues.Add("AllowMove", true);
			this.propertyValues.Add("AllowRotate", true);
			this.propertyValues.Add("AllowResize", true);
			this.propertyValues.Add("AutoHidePorts", false);
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

			if (this.propertyValues.Contains(propertyName))
			{
				return this.propertyValues[propertyName];
			}

			if (this.parent != null)
			{
				IPropertyContainer parentProps = this.parent.GetPropertyContainer(this);
				if (parentProps != null)
				{
					return parentProps.GetPropertyValue(propertyName);
				}
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
			else if (this.propertyValues.ContainsKey(propertyName))
			{
				oldVal = this.propertyValues[propertyName];
				this.propertyValues[propertyName] = val;
			}
			else
			{
				this.propertyValues.Add(propertyName, val);
			}

			this.OnPropertyChanged(new PropertyEventArgs(this, propertyName, oldVal, val));
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
			object oldVal = null;

			if (this.propertyValues.ContainsKey(propertyName))
			{
				oldVal = this.propertyValues[propertyName];
				this.propertyValues[propertyName] = val;
				this.OnPropertyChanged(new PropertyEventArgs(this, propertyName, oldVal, val));
			}
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

		#region Collection Event Handlers

		private void Children_Changing(object sender, NodeCollection.EventArgs evtArgs)
		{
			this.OnChildrenChanging(evtArgs);
		}

		private void Children_ChangeComplete(object sender, NodeCollection.EventArgs evtArgs)
		{
			CollectionEx.ChangeType changeType = evtArgs.ChangeType;

			INode node = evtArgs.Node;
			Port port = null;
			Label lbl = null;

			if (node != null)
			{
				port = node as Port;
				lbl = node as Label;
			}

			if (changeType == CollectionEx.ChangeType.Insert)
			{
				if (port != null)
				{
					if (!this.ports.Contains(port))
					{
						this.ports.Add(port);
					}
				}

				if (lbl != null)
				{
					if (!this.labels.Contains(lbl))
					{
						this.labels.Add(lbl);
					}
				}
			}
			else if (changeType == CollectionEx.ChangeType.Remove)
			{
				if (port != null)
				{
					if (this.ports.Contains(port))
					{
						this.ports.Remove(port);
					}
				}

				if (lbl != null)
				{
					if (this.labels.Contains(lbl))
					{
						this.labels.Remove(lbl);
					}
				}
			}
			else if (changeType == CollectionEx.ChangeType.Clear)
			{
				if (this.ports.Count > 0)
				{
					this.ports.Clear();
				}

				if (this.labels.Count > 0)
				{
					this.labels.Clear();
				}
			}

			this.OnChildrenChangeComplete(evtArgs);
		}

		private void Ports_Changing(object sender, PortCollection.EventArgs evtArgs)
		{
			this.OnPortsChanging(evtArgs);
		}

		private void Ports_ChangeComplete(object sender, PortCollection.EventArgs evtArgs)
		{
			CollectionEx.ChangeType changeType = evtArgs.ChangeType;

			Port port = evtArgs.Port;

			if (changeType == CollectionEx.ChangeType.Insert)
			{
				if (port != null && !this.children.Contains(port))
				{
					port.Parent = this;
					this.children.Add(port);
				}
			}
			else if (changeType == CollectionEx.ChangeType.Remove)
			{
				if (port != null && this.children.Contains(port))
				{
					this.children.Remove(port);
				}
			}

			this.OnPortsChangeComplete(evtArgs);
		}

		private void Labels_Changing(object sender, LabelCollection.EventArgs evtArgs)
		{
			this.OnLabelsChanging(evtArgs);
		}

		private void Labels_ChangeComplete(object sender, LabelCollection.EventArgs evtArgs)
		{
			CollectionEx.ChangeType changeType = evtArgs.ChangeType;

			Label label = evtArgs.Label;

			if (changeType == CollectionEx.ChangeType.Insert)
			{
				if (label != null && !this.children.Contains(label))
				{
					label.Parent = this;
					label.SizeToText(this.Size);
					this.children.Add(label);
				}
			}
			else if (changeType == CollectionEx.ChangeType.Remove)
			{
				if (label != null && this.children.Contains(label))
				{
					this.children.Remove(label);
				}
			}

			this.OnLabelsChangeComplete(evtArgs);
		}

		private void Connections_Changing(object sender, ConnectionCollection.EventArgs evtArgs)
		{
			CollectionEx.ChangeType changeType = evtArgs.ChangeType;

			if (changeType == CollectionEx.ChangeType.Clear)
			{
				IEnumerator enumItems = this.connections.GetEnumerator();
				while (enumItems.MoveNext())
				{
					Connection curConnection = enumItems.Current as Connection;
					if (curConnection != null)
					{
						this.ForeignContainerRemove(curConnection);
					}
				}
			}

			this.OnConnectionsChanging(evtArgs);
		}

		private void Connections_ChangeComplete(object sender, ConnectionCollection.EventArgs evtArgs)
		{
			CollectionEx.ChangeType changeType = evtArgs.ChangeType;

			Connection connection = evtArgs.Connection;

			if (changeType == CollectionEx.ChangeType.Insert)
			{
				if (connection != null)
				{
					this.ForeignContainerAdd(connection);
				}
			}
			else if (changeType == CollectionEx.ChangeType.Remove)
			{
				if (connection != null)
				{
					this.ForeignContainerRemove(connection);
				}
			}

			this.OnConnectionsChangeComplete(evtArgs);
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
			info.AddValue("m11", this.matrix.Elements[0]);
			info.AddValue("m12", this.matrix.Elements[1]);
			info.AddValue("m21", this.matrix.Elements[2]);
			info.AddValue("m22", this.matrix.Elements[3]);
			info.AddValue("dx", this.matrix.Elements[4]);
			info.AddValue("dy", this.matrix.Elements[5]);
			info.AddValue("children", this.children);
			info.AddValue("connections", this.connections);
			info.AddValue("ports", this.ports);
			info.AddValue("labels", this.labels);
			info.AddValue("centerPort", this.centerPort);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.editStyle = new EditStyle(this);
			this.fillStyle = new FillStyle(this);
			this.lineStyle = new LineStyle(this);
			this.children.Changing += new NodeCollection.EventHandler(Children_Changing);
			this.children.ChangeComplete += new NodeCollection.EventHandler(Children_ChangeComplete);
			this.connections.Changing += new ConnectionCollection.EventHandler(Connections_Changing);
			this.connections.ChangeComplete += new ConnectionCollection.EventHandler(Connections_ChangeComplete);
			this.ports.Changing += new PortCollection.EventHandler(Ports_Changing);
			this.ports.ChangeComplete += new PortCollection.EventHandler(Ports_ChangeComplete);
			this.labels.Changing += new LabelCollection.EventHandler(Labels_Changing);
			this.labels.ChangeComplete += new LabelCollection.EventHandler(Labels_ChangeComplete);
		}

		#endregion

		#region Fields

		/// <summary>
		/// Name of the symbol.
		/// </summary>
		private string name;

		/// <summary>
		/// Parent node.
		/// </summary>
		private ICompositeNode parent = null;

		/// <summary>
		/// Collection of child nodes.
		/// </summary>
		protected NodeCollection children = null;

		/// <summary>
		/// Local transformation matrix.
		/// </summary>
		protected Matrix matrix;

		/// <summary>
		/// Set of connections to other symbols.
		/// </summary>
		private ConnectionCollection connections = null;

		/// <summary>
		/// Collection of ports belonging to this symbol.
		/// </summary>
		private PortCollection ports = null;

		/// <summary>
		/// Collection of labels belonging to this symbol.
		/// </summary>
		private LabelCollection labels = null;

		/// <summary>
		/// Built-in center port.
		/// </summary>
		private Port centerPort = null;

		/// <summary>
		/// Edit properties.
		/// </summary>
		private EditStyle editStyle = null;

		/// <summary>
		/// Fill properties.
		/// </summary>
		private FillStyle fillStyle = null;

		/// <summary>
		/// Line drawing properties.
		/// </summary>
		private LineStyle lineStyle = null;

		/// <summary>
		/// Hashtable containing property name/value pairs.
		/// </summary>
		protected Hashtable propertyValues = null;

		#endregion
	}
}
