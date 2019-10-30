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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for list collections that fire events before and after changes
	/// occur.
	/// </summary>
	/// <remarks>
	/// This class provides a base implementation of the IList and IEnumerable
	/// interfaces. The internal implementation of the collection is an ArrayList.
	/// </remarks>
	[Serializable()]
	public abstract class CollectionEx : IList, IEnumerable, ISerializable, ICloneable
	{
		/// <summary>
		/// Identifies a type of change made to a collection.
		/// </summary>
		public enum ChangeType
		{
			/// <summary>
			/// One or more items were inserted into the collection
			/// </summary>
			Insert,

			/// <summary>
			/// One or more items were removed from the collection
			/// </summary>
			Remove,

			/// <summary>
			/// All of the items were removed from the collection
			/// </summary>
			Clear,

			/// <summary>
			/// One of the items in the collection was modified
			/// </summary>
			Set
		}

		/// <summary>
		/// Contents of collection
		/// </summary>
		protected ArrayList items = null;

		/// <summary>
		/// Default constructor
		/// </summary>
		public CollectionEx()
		{
			this.items = new ArrayList();
		}

		/// <summary>
		/// Serialization constructor for a collection.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected CollectionEx(SerializationInfo info, StreamingContext context)
		{
			this.items = (ArrayList) info.GetValue("items", typeof(ArrayList));
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		public CollectionEx(CollectionEx src)
		{
			this.items = (ArrayList) src.items.Clone();
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns></returns>
		public abstract object Clone();

		/// <summary>
		/// The list contained by the collection.
		/// </summary>
		public IList List
		{
			get
			{
				return (IList) this;
			}
		}

		/// <summary>
		/// The raw ArrayList contained by the collection.
		/// </summary>
		/// <remarks>
		/// Making changes to the list through this property does not
		/// fire events.
		/// </remarks>
		protected ArrayList InnerList
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>
		/// Number of items in the collection
		/// </summary>
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		/// <summary>
		/// Determines if the collection is thread-safe
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return this.items.IsSynchronized;
			}
		}

		/// <summary>
		/// Object that can be used to synchronize access to the collection
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return this.items.SyncRoot;
			}
		}

		/// <summary>
		/// Copies the list or a portion of the list to an array
		/// </summary>
		/// <param name="array">Array in which to copy the items in the list</param>
		/// <param name="index">Index at which to start copying in target array</param>
		public void CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>
		/// Returns an enumerator that can be used to iterate through the items
		/// in the list.
		/// </summary>
		/// <returns>Enumerator for iterating through collection</returns>
		public IEnumerator GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		/// <summary>
		/// Gets or set the item at the specified index in the list.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The set method for this property calls OnChanging before setting the
		/// item value and then calls OnChangeComplete after setting the item
		/// value.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
		/// </remarks>
		object IList.this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				EventArgs evtArgs = this.MakeEventArgs(ChangeType.Set, value, index);
				this.OnChanging(evtArgs);
				this.items[index] = value;
				this.OnChangeComplete(evtArgs);
			}
		}

		/// <summary>
		/// Removes all items from the list.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method calls OnChanging before clearing the the collection and
		/// calls OnChangeComplete after clearing the collection.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
		/// </remarks>
		void IList.Clear()
		{
			EventArgs evtArgs = this.MakeEventArgs(ChangeType.Clear);
			this.OnChanging(evtArgs);
			this.items.Clear();
			this.OnChangeComplete(evtArgs);
		}

		/// <summary>
		/// Adds a new item to the list.
		/// </summary>
		/// <param name="value">Object to add</param>
		/// <returns>Zero-based index at which item was added</returns>
		/// <remarks>
		/// <para>
		/// This method calls OnChanging before adding the item and then calls
		/// OnChangeComplete after adding the item.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
		/// </remarks>
		int IList.Add(object value)
		{
			EventArgs evtArgs = this.MakeEventArgs(ChangeType.Insert, value);
			this.OnChanging(evtArgs);
			int index = this.items.Add(value);
			if (index >= 0)
			{
				evtArgs.Index = index;
				this.OnChangeComplete(evtArgs);
			}
			return index;
		}

		/// <summary>
		/// Inserts a new item into the list at the specified position.
		/// </summary>
		/// <param name="index">Zero-based index at which to insert the object</param>
		/// <param name="value">Object to insert</param>
		/// <remarks>
		/// <para>
		/// This method calls OnChanging before inserting the item and then calls
		/// OnChangeComplete after inserting the item.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
		/// </remarks>
		void IList.Insert(int index, object value)
		{
			EventArgs evtArgs = this.MakeEventArgs(ChangeType.Insert, value, index);
			this.OnChanging(evtArgs);
			this.items.Insert(index, value);
			this.OnChangeComplete(evtArgs);
		}

		/// <summary>
		/// Removes the given object from the list.
		/// </summary>
		/// <param name="value">Object to remove</param>
		/// <remarks>
		/// <para>
		/// This method calls OnChanging before removing the item and then calls
		/// OnChangeComplete after removing the item.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
		/// </remarks>
		void IList.Remove(object value)
		{
			int index = this.items.IndexOf(value);
			EventArgs evtArgs = this.MakeEventArgs(ChangeType.Remove, value, index);
			this.OnChanging(evtArgs);
			this.items.Remove(value);
			this.OnChangeComplete(evtArgs);
		}

		/// <summary>
		/// Indicates if the collection is read-only.
		/// </summary>
		/// <remarks>Always returns false</remarks>
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Indicates if the collection is fixed size.
		/// </summary>
		/// <remarks>Always returns true</remarks>
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Determines if the list contains the specified object.
		/// </summary>
		/// <param name="value">Object to search for</param>
		/// <returns>
		/// true if the object is contained by the collection,
		/// otherwise false
		/// </returns>
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		/// <summary>
		/// Returns the zero-based index of the specified object in the list.
		/// </summary>
		/// <param name="value">Object to search for</param>
		/// <returns>Zero-based index of item in the list or -1 if not found</returns>
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		/// <summary>
		/// Concatenates another collection with this collection.
		/// </summary>
		/// <param name="coll">
		/// Collection containing items to add to this collection
		/// </param>
		/// <remarks>
		/// <para>
		/// This method calls OnChanging before concatenating the list and
		/// then calls OnChangeComplete after concatenating the list.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
		/// </remarks>
		public void Concat(ICollection coll)
		{
			EventArgs evtArgs = this.MakeEventArgs(ChangeType.Insert, coll);
			this.OnChanging(evtArgs);
			this.items.AddRange(coll);
			this.OnChangeComplete(evtArgs);
		}

		/// <summary>
		/// Removes the item at the specified index position.
		/// </summary>
		/// <param name="index">Zero-based index of item to remove</param>
		/// <remarks>
		/// <para>
		/// This method calls OnChanging before removing the item and then calls
		/// OnChangeComplete after removing the item.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
		/// </remarks>
		public void RemoveAt(int index)
		{
			object item = null;
			if (index >= 0 && index < this.items.Count)
			{
				item = this.items[index];
				EventArgs evtArgs = this.MakeEventArgs(ChangeType.Remove, item, index);
				this.OnChanging(evtArgs);
				this.items.RemoveAt(index);
				this.OnChangeComplete(evtArgs);
			}
			else
			{
				throw new EInvalidParameter();
			}
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method calls OnChanging before clearing the the collection and
		/// calls OnChangeComplete after clearing the collection.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
		/// </remarks>
		public void Clear()
		{
			EventArgs evtArgs = this.MakeEventArgs(ChangeType.Clear);
			this.OnChanging(evtArgs);
			this.items.Clear();
			this.OnChangeComplete(evtArgs);
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
		/// Arguments for collection change events.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is the base class for the arguments passed to collection change
		/// events. The
		/// <see cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.ChangeType"/>
		/// is included in the event arguments. The index position of the element
		/// that was changed by the event is also included in the event arguments.
		/// </para>
		/// <para>
		/// Derived classes extend this base class to include an array of objects
		/// along with type-safe accessor methods. When the CollectionEx class
		/// needs to call the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
		/// and
		/// <see cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
		/// methods, it creates an EventArgs object by calling the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.MakeEventArgs"/>
		/// method. Derived classes override MakeEventArgs to create their own
		/// type-safe event argument objects.
		/// </para>
		/// </remarks>
		public abstract class EventArgs : System.EventArgs
		{
			/// <summary>
			/// Construct CollectionEx.EventArgs object of a given type.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			public EventArgs(ChangeType changeType)
			{
				this.changeType = changeType;
			}

			/// <summary>
			/// Constructs a CollectionEx.EventArgs object of a given type and
			/// assigns the collection index value.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="index">Index position at which the event occurred</param>
			public EventArgs(ChangeType changeType, int index)
			{
				this.changeType = changeType;
				this.index = index;
			}

			/// <summary>
			/// Type of change that caused the event.
			/// </summary>
			/// <remarks>
			/// <para>
			/// The reason this property exists is to reduce the number of events
			/// that collections must declare and manage. If this property did not
			/// exist, then the
			/// <see cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChanging"/>
			/// and
			/// <see cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.OnChangeComplete"/>
			/// callbacks and their corresponding events in derived classes would mushroom
			/// to around 10 callbacks and events.
			/// </para>
			/// </remarks>
			public ChangeType ChangeType
			{
				get
				{
					return this.changeType;
				}
			}

			/// <summary>
			/// Zero-based index into the collection at which the event occurred.
			/// </summary>
			public int Index
			{
				get
				{
					return this.index;
				}
				set
				{
					this.index = value;
				}
			}

			private ChangeType changeType;
			private int index = -1;
		}

		/// <summary>
		/// Creates an object derived from the CollectionEx.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		/// <remarks>
		/// <para>
		/// Derived collection classes override this method to supply their own
		/// type-safe, CollectionEx.EventArgs derived class.
		/// </para>
		/// </remarks>
		protected abstract EventArgs MakeEventArgs(ChangeType changeType);

		/// <summary>
		/// Creates an object derived from the CollectionEx.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		/// <remarks>
		/// <para>
		/// Derived collection classes override this method to supply their own
		/// type-safe, CollectionEx.EventArgs derived class.
		/// </para>
		/// </remarks>
		protected abstract EventArgs MakeEventArgs(ChangeType changeType, object obj);

		/// <summary>
		/// Creates an object derived from the CollectionEx.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <param name="index">Zero-based collection index at which the event occurred</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		/// <remarks>
		/// <para>
		/// Derived collection classes override this method to supply their own
		/// type-safe, CollectionEx.EventArgs derived class.
		/// </para>
		/// </remarks>
		protected abstract EventArgs MakeEventArgs(ChangeType changeType, object obj, int index);

		/// <summary>
		/// Creates an object derived from the CollectionEx.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="objs">Collection of objects involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		/// <remarks>
		/// <para>
		/// Derived collection classes override this method to supply their own
		/// type-safe, CollectionEx.EventArgs derived class.
		/// </para>
		/// </remarks>
		protected abstract EventArgs MakeEventArgs(ChangeType changeType, ICollection objs);

		/// <summary>
		/// Called before a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// The
		/// <see cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// parameter passed to this method is an instance of a class derived from
		/// CollectionEx.EventArgs. Derived classes are responsible for downcasting
		/// this parameter to the correct type.
		/// </para>
		/// </remarks>
		protected abstract void OnChanging(CollectionEx.EventArgs evtArgs);

		/// <summary>
		/// Called after a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// The
		/// <see cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// parameter passed to this method is an instance of a class derived from
		/// CollectionEx.EventArgs. Derived classes are responsible for downcasting
		/// this parameter to the correct type.
		/// </para>
		/// </remarks>
		protected abstract void OnChangeComplete(CollectionEx.EventArgs evtArgs);
	}
}
