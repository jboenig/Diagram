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
	/// A collection of <see cref="Syncfusion.Windows.Forms.Diagram.Port"/> objects.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
	/// </remarks>
	[
	Serializable(),
	Description("Collection of ports in a port collection"),
	DefaultProperty("Item")
	]
	public class PortCollection : Syncfusion.Windows.Forms.Diagram.CollectionEx, ISerializable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public PortCollection()
		{
		}

		/// <summary>
		/// Serialization constructor for a collection
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected PortCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy from</param>
		public PortCollection(PortCollection src) : base(src)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new PortCollection(this);
		}

		/// <summary>
		/// Gets and sets the port at the specified index position.
		/// </summary>
		public Port this[int index]
		{
			get
			{
				return (Port) this.InnerList[index];
			}
			set
			{
				this.InnerList[index] = value;
			}
		}

		/// <summary>
		/// Add a port to the collection.
		/// </summary>
		/// <param name="obj">Port to add</param>
		/// <returns>Zero-based index position at which the port was added</returns>
		public int Add(Port obj)
		{
			return this.List.Add(obj);
		}

		/// <summary>
		/// Insert a port into the collection at the specified index position.
		/// </summary>
		/// <param name="index">Zero-based index position at which to insert</param>
		/// <param name="obj">Port to insert</param>
		public void Insert(int index, Port obj)
		{
			this.List.Insert(index, obj);
		}

		/// <summary>
		/// Concatenate a collection to this collection.
		/// </summary>
		/// <param name="coll">Collection to concatenate</param>
		public void Concat(PortCollection coll)
		{
			IEnumerator enumColl = coll.GetEnumerator();
			while (enumColl.MoveNext())
			{
				Port curObj = enumColl.Current as Port;
				if (curObj != null)
				{
					this.Add(curObj);
				}
			}
		}

		/// <summary>
		/// Remove the specified port from the collection.
		/// </summary>
		/// <param name="obj">Port to remove</param>
		public void Remove(Port obj)
		{
			this.List.Remove(obj);
		}

		/// <summary>
		/// Determine if the collection contains the specified port.
		/// </summary>
		/// <param name="obj">Port to search for</param>
		/// <returns>true if the collection contains the port, otherwise false</returns>
		public bool Contains(Port obj)
		{
			return this.List.Contains(obj);
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
		/// Event argument class for PortCollection events.
		/// </summary>
		public new class EventArgs : CollectionEx.EventArgs
		{
			/// <summary>
			/// Construct EventArgs given a change type.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			public EventArgs(ChangeType changeType) : base(changeType)
			{
				this.objs = new Port[1] {null};
			}

			/// <summary>
			/// Constructs a PortCollection.EventArgs object from a specified
			/// port.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Port involved in the event</param>
			public EventArgs(ChangeType changeType, object obj) : base(changeType)
			{
				Port port = obj as Port;
				this.objs = new Port[1] {port};
			}

			/// <summary>
			/// Constructs a PortCollection.EventArgs object from a specified
			/// port and collection index.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Port involved in the event</param>
			/// <param name="index">Index position at which the event occurred</param>
			public EventArgs(ChangeType changeType, object obj, int index) : base(changeType, index)
			{
				Port port = obj as Port;
				this.objs = new Port[1] {port};
			}

			/// <summary>
			/// Constructs a PortCollection.EventArgs object from a collection
			/// of ports.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="objs">Collection of ports involved in the event</param>
			public EventArgs(ChangeType changeType, ICollection objs) : base(changeType)
			{
				if (objs == null || objs.Count == 0)
				{
					throw new EInvalidParameter();
				}

				int numObjs = objs.Count;
				this.objs = new Port[numObjs];
				objs.CopyTo(this.objs, 0);
			}

			/// <summary>
			/// The port involved in the event.
			/// </summary>
			public Port Port
			{
				get
				{
					return this.objs[0];
				}
			}

			/// <summary>
			/// Array of ports involved in the event.
			/// </summary>
			public Port[] Ports
			{
				get
				{
					return this.objs;
				}
			}

			private Port[] objs = null;
		}

		/// <summary>
		/// Creates and returns an instance of the PortCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType)
		{
			return new PortCollection.EventArgs(changeType);
		}

		/// <summary>
		/// Creates and returns an instance of the PortCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj)
		{
			return new PortCollection.EventArgs(changeType, obj);
		}

		/// <summary>
		/// Creates and returns an instance of the PortCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <param name="index">Zero-based collection index at which the event occurred</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj, int index)
		{
			return new PortCollection.EventArgs(changeType, obj, index);
		}

		/// <summary>
		/// Creates and returns an instance of the PortCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="objs">Collection of objects involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, ICollection objs)
		{
			return new PortCollection.EventArgs(changeType, objs);
		}

		/// <summary>
		/// Called before a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method downcasts the CollectionEx.EventArgs parameter to a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.PortCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.PortCollection.Changing"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PortCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChanging(CollectionEx.EventArgs evtArgs)
		{
			if (this.Changing != null)
			{
				PortCollection.EventArgs typedEvtArgs = evtArgs as PortCollection.EventArgs;
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.PortCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.PortCollection.ChangeComplete"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PortCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChangeComplete(CollectionEx.EventArgs evtArgs)
		{
			if (this.ChangeComplete != null)
			{
				PortCollection.EventArgs typedEvtArgs = evtArgs as PortCollection.EventArgs;
				if (typedEvtArgs != null)
				{
					this.ChangeComplete(this, typedEvtArgs);
				}
			}
		}

		/// <summary>
		/// Delegate definition for PortCollection events.
		/// </summary>
		public delegate void EventHandler(object sender, PortCollection.EventArgs evtArgs);

		/// <summary>
		/// Fired before a change is made to the collection.
		/// </summary>
		public event PortCollection.EventHandler Changing;

		/// <summary>
		/// Fired after a change is made to the collection.
		/// </summary>
		public event PortCollection.EventHandler ChangeComplete;
	}
}
