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

namespace Syncfusion.Windows.Forms.Diagram
{
	#region Generic Node Event

	/// <summary>
	/// Arguments for node events.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.INode"/>
	/// </remarks>
	public class NodeEventArgs : System.EventArgs
	{
		/// <summary>
		/// Constructs node event arguments given a node.
		/// </summary>
		/// <param name="node">Node involved in the event</param>
		public NodeEventArgs(INode node)
		{
			this.node = node;
		}

		/// <summary>
		/// Node the event applies to.
		/// </summary>
		public INode Node
		{
			get
			{
				return this.node;
			}
		}

		private INode node;
	}

	/// <summary>
	/// Delegate for node events.
	/// </summary>
	public delegate void NodeEventHandler(object sender, NodeEventArgs evtArgs);

	#endregion

	#region Node Bounds Event

	/// <summary>
	/// Arguments for bounds events.
	/// </summary>
	/// <remarks>
	/// A bounds event notifies event subscribers that the bounding box of a
	/// node has changed.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeEventArgs"/>
	/// </remarks>
	public class BoundsEventArgs : NodeEventArgs
	{
		/// <summary>
		/// Construct a BoundsEventArgs object given the node, old bounds, and new bounds.
		/// </summary>
		/// <param name="node">Node the event applies to</param>
		/// <param name="oldBounds">Bounds before the event occurred</param>
		/// <param name="newBounds">Bounds after the event occurred</param>
		public BoundsEventArgs(INode node, RectangleF oldBounds, RectangleF newBounds) : base(node)
		{
			this.oldBounds = oldBounds;
			this.newBounds = newBounds;
		}

		/// <summary>
		/// Bounds before the event occurred.
		/// </summary>
		public RectangleF OldBounds
		{
			get
			{
				return this.oldBounds;
			}
		}

		/// <summary>
		/// Bounds after the event occurred.
		/// </summary>
		public RectangleF NewBounds
		{
			get
			{
				return this.newBounds;
			}
		}

		private RectangleF oldBounds;
		private RectangleF newBounds;
	}

	/// <summary>
	/// Delegate for node bounds events.
	/// </summary>
	public delegate void BoundsEventHandler(object sender, BoundsEventArgs evtArgs);

	#endregion

	#region Node Mouse Events

	/// <summary>
	/// Arguments for node mouse events.
	/// </summary>
	/// <remarks>
	/// A mouse event notifies event subscribers of mouse events that affect a
	/// node.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeEventArgs"/>
	/// </remarks>
	public class NodeMouseEventArgs : NodeEventArgs
	{
		/// <summary>
		/// Construct a NodeMouseEventArgs object given a node.
		/// </summary>
		/// <param name="node">Node the event applies to</param>
		public NodeMouseEventArgs(INode node) : base(node)
		{
		}
	}

	/// <summary>
	/// Delegate for node mouse events.
	/// </summary>
	public delegate void NodeMouseEventHandler(object sender, NodeMouseEventArgs evtArgs);

	#endregion

	#region Vertex Events

	/// <summary>
	/// Arguments for vertex events.
	/// </summary>
	/// <remarks>
	/// A vertex notifies event subscribers that a vertex in a node has been modified,
	/// added, or removed.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeEventArgs"/>
	/// </remarks>
	public class VertexEventArgs : NodeEventArgs
	{
		/// <summary>
		/// Constructs a VertexEventArgs object given a node and a vertex index.
		/// </summary>
		/// <param name="node">Node the event applies to</param>
		/// <param name="vertexIdx">Zero-based index position of the vertex affected</param>
		public VertexEventArgs(INode node, int vertexIdx) : base(node)
		{
			this.vertexIdx = vertexIdx;
		}

		/// <summary>
		/// Zero-based index position of the vertex affected.
		/// </summary>
		public int Vertex
		{
			get
			{
				return this.vertexIdx;
			}
		}

		private int vertexIdx = -1;
	}

	/// <summary>
	/// Delegate for vertex events.
	/// </summary>
	public delegate void VertexEventHandler(object sender, VertexEventArgs evtArgs);

	#endregion

	#region Property Events

	/// <summary>
	/// Arguments for property events.
	/// </summary>
	/// <remarks>
	/// A property event notifies event subscribers of changes to property in
	/// a node.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeEventArgs"/>
	/// </remarks>
	public class PropertyEventArgs : NodeEventArgs
	{
		/// <summary>
		/// Constructs a PropertyEventArgs object given a node, property name,
		/// old property value, and new property value.
		/// </summary>
		/// <param name="node">Node the event applies to</param>
		/// <param name="propertyName">Name of the property the event applies to</param>
		/// <param name="oldVal">Value of the property before the event occurred</param>
		/// <param name="newVal">Value of the property after the event occurred</param>
		public PropertyEventArgs(INode node, string propertyName, object oldVal, object newVal) :
			base(node)
		{
			this.propertyName = propertyName;
			this.oldVal = oldVal;
			this.newVal = newVal;
		}

		/// <summary>
		/// Name of the property the event applies to.
		/// </summary>
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		/// <summary>
		/// Value of the property before the event occurred.
		/// </summary>
		public object OldValue
		{
			get
			{
				return this.oldVal;
			}
		}

		/// <summary>
		/// Value of the property after the event occurred.
		/// </summary>
		public object NewValue
		{
			get
			{
				return this.newVal;
			}
		}

		private string propertyName = null;
		private object oldVal = null;
		private object newVal = null;
	}

	/// <summary>
	/// Delegate for property events.
	/// </summary>
	public delegate void PropertyEventHandler(object sender, PropertyEventArgs evtArgs);

	#endregion

	#region IDispatchNodeEvents interface

	/// <summary>
	/// Provides nodes with a mechanism for forwarding events up the node hierarchy.
	/// </summary>
	/// <remarks>
	/// Nodes that implement this interface can be notified of events and will
	/// respond by either forwarding the event up the node hierarchy or handling the
	/// event.
	/// </remarks>
	public interface IDispatchNodeEvents
	{
		/// <summary>
		/// Called when a property value is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// </remarks>
		void PropertyChanged(PropertyEventArgs evtArgs);

		/// <summary>
		/// Called before a change is made to the collection of child nodes belonging
		/// to a composite node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void ChildrenChanging(NodeCollection.EventArgs evtArgs);

		/// <summary>
		/// Called after a change is made to the collection of child nodes belonging
		/// to a composite node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void ChildrenChangeComplete(NodeCollection.EventArgs evtArgs);

		/// <summary>
		/// Called when the bounds of a node changes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoundsEventArgs"/>
		/// </remarks>
		void BoundsChanged(BoundsEventArgs evtArgs);

		/// <summary>
		/// Called when a node is moved.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveEventArgs"/>
		/// </remarks>
		void Move(MoveEventArgs evtArgs);

		/// <summary>
		/// Called when a node is rotated.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RotateEventArgs"/>
		/// </remarks>
		void Rotate(RotateEventArgs evtArgs);

		/// <summary>
		/// Called when a node is scaled.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ScaleEventArgs"/>
		/// </remarks>
		void Scale(ScaleEventArgs evtArgs);

		/// <summary>
		/// Called when a node is clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void Click(NodeMouseEventArgs evtArgs);

		/// <summary>
		/// Called when a node is double clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void DoubleClick(NodeMouseEventArgs evtArgs);

		/// <summary>
		/// Called when the mouse enters a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void MouseEnter(NodeMouseEventArgs evtArgs);

		/// <summary>
		/// Called when the mouse leaves a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void MouseLeave(NodeMouseEventArgs evtArgs);

		/// <summary>
		/// Called when a vertex is inserted into a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void InsertVertex(VertexEventArgs evtArgs);

		/// <summary>
		/// Called when a vertex is deleted from a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void DeleteVertex(VertexEventArgs evtArgs);

		/// <summary>
		/// Called when a vertex is moved.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void MoveVertex(VertexEventArgs evtArgs);

		/// <summary>
		/// Called before the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		void ConnectionsChanging(ConnectionCollection.EventArgs evtArgs);

		/// <summary>
		/// Called after the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		void ConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs);
	}

	#endregion
}
