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
	/// A collection of <see cref="Syncfusion.Windows.Forms.Diagram.Label"/> objects.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label"/>
	/// </remarks>
	[
	Serializable(),
	Description("Collection of labels in a symbol"),
	DefaultProperty("Item")
	]
	public class LabelCollection : Syncfusion.Windows.Forms.Diagram.CollectionEx, ISerializable
	{
		/// <summary>
		/// Construct a LabelCollection object.
		/// </summary>
		public LabelCollection()
		{
		}

		/// <summary>
		/// Serialization constructor for a collection
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected LabelCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="src">Object to copy</param>
		public LabelCollection(LabelCollection src) : base(src)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new LabelCollection(this);
		}

		/// <summary>
		/// Gets or sets the item at the specified zero-based index
		/// in the collection.
		/// </summary>
		public Label this[int index]
		{
			get
			{
				return (Label) this.InnerList[index];
			}
			set
			{
				this.InnerList[index] = value;
			}
		}

		/// <summary>
		/// Adds a label to the collection.
		/// </summary>
		/// <param name="obj">Label to add</param>
		/// <returns>Zero-based index of item added</returns>
		public int Add(Label obj)
		{
			return this.List.Add(obj);
		}

		/// <summary>
		/// Inserts a label into the collection.
		/// </summary>
		/// <param name="index">Zero-based index at which to add the label</param>
		/// <param name="obj">Label to add</param>
		public void Insert(int index, Label obj)
		{
			this.List.Insert(index, obj);
		}

		/// <summary>
		/// Concatenate a collection to this collection.
		/// </summary>
		/// <param name="coll">Collection to concatenate</param>
		public void Concat(LabelCollection coll)
		{
			IEnumerator enumColl = coll.GetEnumerator();
			while (enumColl.MoveNext())
			{
				Label curObj = enumColl.Current as Label;
				if (curObj != null)
				{
					this.Add(curObj);
				}
			}
		}

		/// <summary>
		/// Remove the specified label from the collection.
		/// </summary>
		/// <param name="obj">Label to remove from the collection</param>
		public void Remove(Label obj)
		{
			this.List.Remove(obj);
		}

		/// <summary>
		/// Determines if the collection contains the specified label.
		/// </summary>
		/// <param name="obj">Label to search for</param>
		/// <returns>true if found, otherwise false</returns>
		public bool Contains(Label obj)
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
		/// Event argument class for LabelCollection events.
		/// </summary>
		public new class EventArgs : CollectionEx.EventArgs
		{
			/// <summary>
			/// Construct EventArgs given a change type.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			public EventArgs(ChangeType changeType) : base(changeType)
			{
				this.objs = new Label[1] {null};
			}

			/// <summary>
			/// Constructs a LabelCollection.EventArgs object from a specified
			/// label.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Label involved in the event</param>
			public EventArgs(ChangeType changeType, object obj) : base(changeType)
			{
				Label label = obj as Label;
				this.objs = new Label[1] {label};
			}

			/// <summary>
			/// Constructs a LabelCollection.EventArgs object from a specified
			/// label and collection index.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Label involved in the event</param>
			/// <param name="index">Index position at which the event occurred</param>
			public EventArgs(ChangeType changeType, object obj, int index) : base(changeType, index)
			{
				Label label = obj as Label;
				this.objs = new Label[1] {label};
			}

			/// <summary>
			/// Constructs a LabelCollection.EventArgs object from a collection
			/// of labels.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="objs">Collection of labels involved in the event</param>
			public EventArgs(ChangeType changeType, ICollection objs) : base(changeType)
			{
				if (objs == null || objs.Count == 0)
				{
					throw new EInvalidParameter();
				}

				int numObjs = objs.Count;
				this.objs = new Label[numObjs];
				objs.CopyTo(this.objs, 0);
			}

			/// <summary>
			/// The label involved in the event.
			/// </summary>
			public Label Label
			{
				get
				{
					return this.objs[0];
				}
			}

			/// <summary>
			/// Array of labels involved in the event.
			/// </summary>
			public Label[] Labels
			{
				get
				{
					return this.objs;
				}
			}

			private Label[] objs = null;
		}

		/// <summary>
		/// Creates and returns an instance of the LabelCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType)
		{
			return new LabelCollection.EventArgs(changeType);
		}

		/// <summary>
		/// Creates and returns an instance of the LabelCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj)
		{
			return new LabelCollection.EventArgs(changeType, obj);
		}

		/// <summary>
		/// Creates and returns an instance of the LabelCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <param name="index">Zero-based collection index at which the event occurred</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj, int index)
		{
			return new LabelCollection.EventArgs(changeType, obj, index);
		}

		/// <summary>
		/// Creates and returns an instance of the LabelCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="objs">Collection of objects involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, ICollection objs)
		{
			return new LabelCollection.EventArgs(changeType, objs);
		}

		/// <summary>
		/// Called before a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method downcasts the CollectionEx.EventArgs parameter to a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LabelCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LabelCollection.Changing"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LabelCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChanging(CollectionEx.EventArgs evtArgs)
		{
			if (this.Changing != null)
			{
				LabelCollection.EventArgs typedEvtArgs = evtArgs as LabelCollection.EventArgs;
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LabelCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LabelCollection.ChangeComplete"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LabelCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChangeComplete(CollectionEx.EventArgs evtArgs)
		{
			if (this.ChangeComplete != null)
			{
				LabelCollection.EventArgs typedEvtArgs = evtArgs as LabelCollection.EventArgs;
				if (typedEvtArgs != null)
				{
					this.ChangeComplete(this, typedEvtArgs);
				}
			}
		}

		/// <summary>
		/// Delegate definition for LabelCollection events.
		/// </summary>
		public delegate void EventHandler(object sender, LabelCollection.EventArgs evtArgs);

		/// <summary>
		/// Fired before a change is made to the collection.
		/// </summary>
		public event LabelCollection.EventHandler Changing;

		/// <summary>
		/// Fired after a change is made to the collection.
		/// </summary>
		public event LabelCollection.EventHandler ChangeComplete;
	}
}
