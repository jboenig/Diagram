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
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A collection of <see cref="Syncfusion.Windows.Forms.Diagram.INode"/> objects.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.INode"/>
	/// </remarks>
	[
	Serializable(),
	Description("Collection of nodes"),
	DefaultProperty("Item")
	]
	public class NodeCollection : Syncfusion.Windows.Forms.Diagram.CollectionEx, ISerializable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public NodeCollection()
		{
		}

		/// <summary>
		/// Serialization constructor for a collection
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected NodeCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy from</param>
		public NodeCollection(NodeCollection src) : base(src)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new NodeCollection(this);
		}

		/// <summary>
		/// Gets or set the node at the specified index position.
		/// </summary>
		public INode this[int index]
		{
			get
			{
				return (INode) this.items[index];
			}
			set
			{
				this.items[index] = value;
			}
		}

		/// <summary>
		/// Adds a node to the collection.
		/// </summary>
		/// <param name="obj">Node to add</param>
		/// <returns>Zero-based index position where the add occurred</returns>
		public int Add(INode obj)
		{
			return this.List.Add(obj);
		}

		/// <summary>
		/// Inserts a node into the collection at the specified index position.
		/// </summary>
		/// <param name="index">Zero-based index position at which to insert the node</param>
		/// <param name="obj">Node to insert</param>
		public void Insert(int index, INode obj)
		{
			this.List.Insert(index, obj);
		}

		/// <summary>
		/// Concatenate a collection to this collection.
		/// </summary>
		/// <param name="coll">Collection to concatenate</param>
		public void Concat(NodeCollection coll)
		{
			IEnumerator enumColl = coll.GetEnumerator();
			while (enumColl.MoveNext())
			{
				INode curObj = enumColl.Current as INode;
				if (curObj != null)
				{
					this.Add(curObj);
				}
			}
		}

		/// <summary>
		/// Removes the specified node from the collection.
		/// </summary>
		/// <param name="obj">Node to remove</param>
		public void Remove(INode obj)
		{
			this.List.Remove(obj);
		}

		/// <summary>
		/// Determines if the collection contains the specified node.
		/// </summary>
		/// <param name="obj">Node to search for</param>
		/// <returns>true if the node is found, otherwise false</returns>
		public bool Contains(INode obj)
		{
			return this.List.Contains(obj);
		}

		/// <summary>
		/// Determines if the collection contains a node matching the specified
		/// name.
		/// </summary>
		/// <param name="nodeName">Name of the node to search for</param>
		/// <returns>true if the node is found, otherwise false</returns>
		public bool Contains(string nodeName)
		{
			return (this.Find(nodeName) >= 0);
		}

		/// <summary>
		/// Returns the index position of the specified node in the collection.
		/// </summary>
		/// <param name="obj">Node to search for</param>
		/// <returns>Zero-based index position of the node, or -1 if not found</returns>
		public int Find(INode obj)
		{
			int idxFound = -1;
			int curIdx;
			INode curNode;

			for (curIdx = 0; idxFound < 0 && curIdx < this.InnerList.Count; curIdx++)
			{
				curNode = this.InnerList[curIdx] as INode;
				if (curNode == obj)
				{
					idxFound = curIdx;
				}
			}

			return idxFound;
		}

		/// <summary>
		/// Returns the index position of the node matching the specified node
		/// name.
		/// </summary>
		/// <param name="nodeName">Name of node to search for</param>
		/// <returns>Zero-based index position of the node, or -1 if not found</returns>
		public int Find(string nodeName)
		{
			int idxFound = -1;
			int curIdx;
			INode curNode;

			for (curIdx = 0; idxFound < 0 && curIdx < this.InnerList.Count; curIdx++)
			{
				curNode = this.InnerList[curIdx] as INode;
				if (curNode != null)
				{
					if (curNode.Name == nodeName)
					{
						idxFound = curIdx;
					}
				}
			}

			return idxFound;
		}

		/// <summary>
		/// Gets the first node in the collection.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public INode First
		{
			get
			{
				INode firstNode = null;

				int nNumNodes = this.Count;

				if (nNumNodes > 0)
				{
					firstNode = this[0];
				}

				return firstNode;
			}
		}

		/// <summary>
		/// Gets the last node in the collection.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public INode Last
		{
			get
			{
				INode lastNode = null;

				int nNumNodes = this.Count;

				if (nNumNodes > 0)
				{
					lastNode = this[nNumNodes-1];
				}

				return lastNode;
			}
		}

		/// <summary>
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("items", this.items);
		}

		/// <summary>
		/// Event argument class for NodeCollection events.
		/// </summary>
		public new class EventArgs : CollectionEx.EventArgs
		{
			/// <summary>
			/// Construct EventArgs given a change type.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			public EventArgs(ChangeType changeType) : base(changeType)
			{
				this.objs = new INode[1] {null};
			}

			/// <summary>
			/// Constructs a NodeCollection.EventArgs object from a specified
			/// node.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Node involved in the event</param>
			public EventArgs(ChangeType changeType, object obj) : base(changeType)
			{
				INode node = obj as INode;
				this.objs = new INode[1] {node};
			}

			/// <summary>
			/// Constructs a NodeCollection.EventArgs object from a specified
			/// node and collection index.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Node involved in the event</param>
			/// <param name="index">Index position at which the event occurred</param>
			public EventArgs(ChangeType changeType, object obj, int index) : base(changeType, index)
			{
				INode node = obj as INode;
				this.objs = new INode[1] {node};
			}

			/// <summary>
			/// Constructs a NodeCollection.EventArgs object from a collection
			/// of nodes.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="objs">Collection of nodes involved in the event</param>
			public EventArgs(ChangeType changeType, ICollection objs) : base(changeType)
			{
				if (objs == null || objs.Count == 0)
				{
					throw new EInvalidParameter();
				}

				int numObjs = objs.Count;
				this.objs = new INode[numObjs];
				objs.CopyTo(this.objs, 0);
			}

			/// <summary>
			/// The node involved in the event.
			/// </summary>
			public INode Node
			{
				get
				{
					return this.objs[0];
				}
			}

			/// <summary>
			/// Array of nodes involved in the event.
			/// </summary>
			public INode[] Nodes
			{
				get
				{
					return this.objs;
				}
			}

			private INode[] objs = null;
		}

		/// <summary>
		/// Creates and returns an instance of the NodeCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType)
		{
			return new NodeCollection.EventArgs(changeType);
		}

		/// <summary>
		/// Creates and returns an instance of the NodeCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj)
		{
			return new NodeCollection.EventArgs(changeType, obj);
		}

		/// <summary>
		/// Creates and returns an instance of the NodeCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <param name="index">Zero-based collection index at which the event occurred</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj, int index)
		{
			return new NodeCollection.EventArgs(changeType, obj, index);
		}

		/// <summary>
		/// Creates and returns an instance of the NodeCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="objs">Collection of objects involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, ICollection objs)
		{
			return new NodeCollection.EventArgs(changeType, objs);
		}

		/// <summary>
		/// Called before a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method downcasts the CollectionEx.EventArgs parameter to a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.Changing"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChanging(CollectionEx.EventArgs evtArgs)
		{
			if (this.Changing != null)
			{
				NodeCollection.EventArgs typedEvtArgs = evtArgs as NodeCollection.EventArgs;
				if (typedEvtArgs != null)
				{
					this.Changing(this, typedEvtArgs);
				}
			}
		}

		/// <summary>
		/// Called after a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method downcasts the CollectionEx.EventArgs parameter to a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.ChangeComplete"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChangeComplete(CollectionEx.EventArgs evtArgs)
		{
			if (this.ChangeComplete != null)
			{
				NodeCollection.EventArgs typedEvtArgs = evtArgs as NodeCollection.EventArgs;
				if (typedEvtArgs != null)
				{
					this.ChangeComplete(this, typedEvtArgs);
				}
			}
		}

		/// <summary>
		/// Delegate definition for NodeCollection events.
		/// </summary>
		public delegate void EventHandler(object sender, NodeCollection.EventArgs evtArgs);

		/// <summary>
		/// Fired before a change is made to the collection.
		/// </summary>
		public event NodeCollection.EventHandler Changing;

		/// <summary>
		/// Fired after a change is made to the collection.
		/// </summary>
		public event NodeCollection.EventHandler ChangeComplete;
	}
}
