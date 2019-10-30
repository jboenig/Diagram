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
	/// List of <see cref="Syncfusion.Windows.Forms.Diagram.Connection"/> objects.
	/// </summary>
	/// <remarks>
	/// A ConnectionCollection can be attached to an object that implements the
	/// IPortContainer interface. If it is, the ConnectionCollection automatically
	/// synchronizes the connections in foreign symbols when connections are
	/// added or removed.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection"/>
	/// </remarks>
	[
	Serializable(),
	Description("Set of connections in a port container"),
	DefaultProperty("Item")
	]
	public class ConnectionCollection : Syncfusion.Windows.Forms.Diagram.CollectionEx, ISerializable
	{
		/// <summary>
		/// Construct a ConnectionCollection object.
		/// </summary>
		public ConnectionCollection()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Object to copy</param>
		public ConnectionCollection(ConnectionCollection src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for a collection
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected ConnectionCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new ConnectionCollection(this);
		}

		/// <summary>
		/// Gets or sets the item at the specified zero-based index
		/// in the collection.
		/// </summary>
		public Connection this[int index]
		{
			get
			{
				return (Connection) this.InnerList[index];
			}
		}

		/// <summary>
		/// Add a connection to this collection.
		/// </summary>
		/// <param name="obj">Connection to add</param>
		/// <returns>Zero-based index at which the item was added</returns>
		public int Add(Connection obj)
		{
			return this.List.Add(obj);
		}

		/// <summary>
		/// Concatenate a collection to this collection.
		/// </summary>
		/// <param name="coll">Collection to append</param>
		public void Concat(ConnectionCollection coll)
		{
			IEnumerator enumColl = coll.GetEnumerator();
			while (enumColl.MoveNext())
			{
				Connection curObj = enumColl.Current as Connection;
				if (curObj != null)
				{
					this.Add(curObj);
				}
			}
		}

		/// <summary>
		/// Removes the specified connection from the collection.
		/// </summary>
		/// <param name="obj">Connection to remove</param>
		public void Remove(Connection obj)
		{
			this.List.Remove(obj);
		}

		/// <summary>
		/// Remove a connection from the collection given a source and target port.
		/// </summary>
		/// <param name="sourcePort">Source port of connection to remove</param>
		/// <param name="targetPort">Target port of connection to remove</param>
		/// <returns>The connection removed or null if not found</returns>
		/// <remarks>
		/// This method searches the collection for a connection matching the given
		/// source and target ports. If one is found, it is removed and returned to
		/// the caller.
		/// </remarks>
		public Connection Remove(Port sourcePort, Port targetPort)
		{
			Connection connection = this.Find(sourcePort, targetPort);

			if (connection != null)
			{
				this.Remove(connection);
			}

			return connection;
 		}

		/// <summary>
		/// Determine if the specified connection exists in this collection.
		/// </summary>
		/// <param name="obj">Object to search for</param>
		/// <returns>true if the item is found, otherwise false</returns>
		public bool Contains(Connection obj)
		{
			return this.List.Contains(obj);
		}

		/// <summary>
		/// Searches the collection for a connection matching the given
		/// source and target ports.
		/// </summary>
		/// <param name="sourcePort">Source port of connection</param>
		/// <param name="targetPort">Target port of connection</param>
		/// <returns>
		/// Connection object matching the given source and target ports or
		/// null if one does not exist in the collection
		/// </returns>
		public Connection Find(Port sourcePort, Port targetPort)
		{
			Connection obj = null;
			Connection cur = null;
			IEnumerator enumColl = this.GetEnumerator();

			while (obj == null && enumColl.MoveNext())
			{
				cur = enumColl.Current as Connection;
				if (cur != null)
				{
					if (cur.SourcePort == sourcePort && cur.TargetPort == targetPort)
					{
						obj = cur;
					}
				}
			}

			return obj;
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
		/// Event argument class for ConnectionCollection events.
		/// </summary>
		public new class EventArgs : CollectionEx.EventArgs
		{
			/// <summary>
			/// Construct EventArgs given a change type.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			public EventArgs(ChangeType changeType) : base(changeType)
			{
				this.objs = new Connection[1] {null};
			}

			/// <summary>
			/// Constructs a ConnectionCollection.EventArgs object from a specified
			/// connection.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Connection involved in the event</param>
			public EventArgs(ChangeType changeType, object obj) : base(changeType)
			{
				Connection connection = obj as Connection;
				this.objs = new Connection[1] {connection};
			}

			/// <summary>
			/// Constructs a ConnectionCollection.EventArgs object from a specified
			/// connection and collection index.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Connection involved in the event</param>
			/// <param name="index">Index position at which the event occurred</param>
			public EventArgs(ChangeType changeType, object obj, int index) : base(changeType, index)
			{
				Connection connection = obj as Connection;
				this.objs = new Connection[1] {connection};
			}

			/// <summary>
			/// Constructs a ConnectionCollection.EventArgs object from a collection
			/// of connections.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="objs">Collection of connections involved in the event</param>
			public EventArgs(ChangeType changeType, ICollection objs) : base(changeType)
			{
				if (objs == null || objs.Count == 0)
				{
					throw new EInvalidParameter();
				}

				int numObjs = objs.Count;
				this.objs = new Connection[numObjs];
				objs.CopyTo(this.objs, 0);
			}

			/// <summary>
			/// The connection involved in the event.
			/// </summary>
			public Connection Connection
			{
				get
				{
					return this.objs[0];
				}
			}

			/// <summary>
			/// Array of connections involved in the event.
			/// </summary>
			public Connection[] Connections
			{
				get
				{
					return this.objs;
				}
			}

			private Connection[] objs = null;
		}

		/// <summary>
		/// Creates and returns an instance of the ConnectionCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType)
		{
			return new ConnectionCollection.EventArgs(changeType);
		}

		/// <summary>
		/// Creates and returns an instance of the ConnectionCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj)
		{
			return new ConnectionCollection.EventArgs(changeType, obj);
		}

		/// <summary>
		/// Creates and returns an instance of the ConnectionCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <param name="index">Zero-based collection index at which the event occurred</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj, int index)
		{
			return new ConnectionCollection.EventArgs(changeType, obj, index);
		}

		/// <summary>
		/// Creates and returns an instance of the ConnectionCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="objs">Collection of objects involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, ICollection objs)
		{
			return new ConnectionCollection.EventArgs(changeType, objs);
		}

		/// <summary>
		/// Called before a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method downcasts the CollectionEx.EventArgs parameter to a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.Changing"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChanging(CollectionEx.EventArgs evtArgs)
		{
			if (this.Changing != null)
			{
				ConnectionCollection.EventArgs typedEvtArgs = evtArgs as ConnectionCollection.EventArgs;
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.ChangeComplete"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChangeComplete(CollectionEx.EventArgs evtArgs)
		{
			if (this.ChangeComplete != null)
			{
				ConnectionCollection.EventArgs typedEvtArgs = evtArgs as ConnectionCollection.EventArgs;
				if (typedEvtArgs != null)
				{
					this.ChangeComplete(this, typedEvtArgs);
				}
			}
		}

		/// <summary>
		/// Delegate definition for ConnectionCollection events.
		/// </summary>
		public delegate void EventHandler(object sender, ConnectionCollection.EventArgs evtArgs);

		/// <summary>
		/// Fired before a change is made to the collection.
		/// </summary>
		public event ConnectionCollection.EventHandler Changing;

		/// <summary>
		/// Fired after a change is made to the collection.
		/// </summary>
		public event ConnectionCollection.EventHandler ChangeComplete;
	}
}
