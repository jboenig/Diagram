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
	/// A collection of <see cref="Syncfusion.Windows.Forms.Diagram.IVerb"/> objects.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IVerb"/>
	/// </remarks>
	[
	Serializable(),
	Description("Collection of verbs"),
	DefaultProperty("Item")
	]
	public class VerbCollection : Syncfusion.Windows.Forms.Diagram.CollectionEx, ISerializable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public VerbCollection()
		{
		}

		/// <summary>
		/// Serialization constructor for a collection
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected VerbCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy from</param>
		public VerbCollection(VerbCollection src) : base(src)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new VerbCollection(this);
		}

		/// <summary>
		/// Gets or sets the verb at the specified index position.
		/// </summary>
		public IVerb this[int index]
		{
			get
			{
				return (IVerb) this.InnerList[index];
			}
			set
			{
				this.InnerList[index] = value;
			}
		}

		/// <summary>
		/// Adds a verb to the collection.
		/// </summary>
		/// <param name="obj">Verb to add</param>
		/// <returns>Zero-based index position at which the item was added</returns>
		public int Add(IVerb obj)
		{
			return this.List.Add(obj);
		}

		/// <summary>
		/// Concatenate a collection to this collection.
		/// </summary>
		/// <param name="coll">Collection to concatenate</param>
		public void Concat(VerbCollection coll)
		{
			IEnumerator enumColl = coll.GetEnumerator();
			while (enumColl.MoveNext())
			{
				IVerb curObj = enumColl.Current as IVerb;
				if (curObj != null)
				{
					this.Add(curObj);
				}
			}
		}

		/// <summary>
		/// Removes the specified verb from the collection.
		/// </summary>
		/// <param name="obj">Verb to remove</param>
		public void Remove(IVerb obj)
		{
			this.List.Remove(obj);
		}

		/// <summary>
		/// Determines if the collection contains the specified verb.
		/// </summary>
		/// <param name="obj">Verb to search for</param>
		/// <returns>true if the verb is found, otherwise false</returns>
		public bool Contains(IVerb obj)
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
		/// Event argument class for VerbCollection events.
		/// </summary>
		public new class EventArgs : CollectionEx.EventArgs
		{
			/// <summary>
			/// Construct EventArgs given a change type.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			public EventArgs(ChangeType changeType) : base(changeType)
			{
				this.objs = new IVerb[1] {null};
			}

			/// <summary>
			/// Constructs a VerbCollection.EventArgs object from a specified
			/// verb.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Verb involved in the event</param>
			public EventArgs(ChangeType changeType, object obj) : base(changeType)
			{
				IVerb verb = obj as IVerb;
				this.objs = new IVerb[1] {verb};
			}

			/// <summary>
			/// Constructs a VerbCollection.EventArgs object from a specified
			/// verb and collection index.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Verb involved in the event</param>
			/// <param name="index">Index position at which the event occurred</param>
			public EventArgs(ChangeType changeType, object obj, int index) : base(changeType, index)
			{
				IVerb verb = obj as IVerb;
				this.objs = new IVerb[1] {verb};
			}

			/// <summary>
			/// Constructs a VerbCollection.EventArgs object from a collection
			/// of verbs.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="objs">Collection of verbs involved in the event</param>
			public EventArgs(ChangeType changeType, ICollection objs) : base(changeType)
			{
				if (objs == null || objs.Count == 0)
				{
					throw new EInvalidParameter();
				}

				int numObjs = objs.Count;
				this.objs = new IVerb[numObjs];
				objs.CopyTo(this.objs, 0);
			}

			/// <summary>
			/// The verb involved in the event.
			/// </summary>
			public IVerb Verb
			{
				get
				{
					return this.objs[0];
				}
			}

			/// <summary>
			/// Array of verbs involved in the event.
			/// </summary>
			public IVerb[] Verbs
			{
				get
				{
					return this.objs;
				}
			}

			private IVerb[] objs = null;
		}

		/// <summary>
		/// Creates and returns an instance of the VerbCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType)
		{
			return new VerbCollection.EventArgs(changeType);
		}

		/// <summary>
		/// Creates and returns an instance of the VerbCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj)
		{
			return new VerbCollection.EventArgs(changeType, obj);
		}

		/// <summary>
		/// Creates and returns an instance of the VerbCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <param name="index">Zero-based collection index at which the event occurred</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj, int index)
		{
			return new VerbCollection.EventArgs(changeType, obj, index);
		}

		/// <summary>
		/// Creates and returns an instance of the VerbCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="objs">Collection of objects involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, ICollection objs)
		{
			return new VerbCollection.EventArgs(changeType, objs);
		}

		/// <summary>
		/// Called before a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method downcasts the CollectionEx.EventArgs parameter to a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.VerbCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.VerbCollection.Changing"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VerbCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChanging(CollectionEx.EventArgs evtArgs)
		{
			if (this.Changing != null)
			{
				VerbCollection.EventArgs typedEvtArgs = evtArgs as VerbCollection.EventArgs;
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.VerbCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.VerbCollection.ChangeComplete"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VerbCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChangeComplete(CollectionEx.EventArgs evtArgs)
		{
			if (this.ChangeComplete != null)
			{
				VerbCollection.EventArgs typedEvtArgs = evtArgs as VerbCollection.EventArgs;
				if (typedEvtArgs != null)
				{
					this.ChangeComplete(this, typedEvtArgs);
				}
			}
		}

		/// <summary>
		/// Delegate definition for VerbCollection events.
		/// </summary>
		public delegate void EventHandler(object sender, VerbCollection.EventArgs evtArgs);

		/// <summary>
		/// Fired before a change is made to the collection.
		/// </summary>
		public event VerbCollection.EventHandler Changing;

		/// <summary>
		/// Fired after a change is made to the collection.
		/// </summary>
		public event VerbCollection.EventHandler ChangeComplete;
	}
}
