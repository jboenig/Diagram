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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A collection of <see cref="Syncfusion.Windows.Forms.Diagram.Tool"/> objects.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// </remarks>
	public class ToolCollection : Syncfusion.Windows.Forms.Diagram.CollectionEx
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ToolCollection()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy from</param>
		public ToolCollection(ToolCollection src) : base(src)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new ToolCollection(this);
		}

		/// <summary>
		/// Gets the Tool at the specified index position.
		/// </summary>
		public Tool this[int index]
		{
			get
			{
				return (Tool) InnerList[index];
			}
		}

		/// <summary>
		/// Adds a tool to the collection.
		/// </summary>
		/// <param name="obj">Tool to add</param>
		/// <returns>Zero-based index position at which the item was added</returns>
		public int Add(Tool obj)
		{
			return List.Add(obj);
		}

		/// <summary>
		/// Inserts a tool into the collection at the specified index position.
		/// </summary>
		/// <param name="index">Index position at which to add the tool</param>
		/// <param name="obj">Tool to add</param>
		public void Insert(int index, Tool obj)
		{
			List.Insert(index, obj);
		}

		/// <summary>
		/// Remove the specified tool from the collection.
		/// </summary>
		/// <param name="obj">Tool to remove</param>
		public void Remove(Tool obj)
		{
			List.Remove(obj);
		}

		/// <summary>
		/// Returns the index position of the specified tool in the collection.
		/// </summary>
		/// <param name="obj">Tool to search for</param>
		/// <returns>Zero-based index position of the item or -1 if not found</returns>
		public int IndexOf(Tool obj)
		{
			int idx = 0;
			IEnumerator enumTools = InnerList.GetEnumerator();
			while (enumTools.MoveNext())
			{
				if (enumTools.Current == obj)
				{
					return idx;
				}
				idx++;
			}
			return -1;
		}

		/// <summary>
		/// Event argument class for ToolCollection events.
		/// </summary>
		public new class EventArgs : CollectionEx.EventArgs
		{
			/// <summary>
			/// Construct EventArgs given a change type.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			public EventArgs(ChangeType changeType) : base(changeType)
			{
				this.objs = new Tool[1] {null};
			}

			/// <summary>
			/// Constructs a ToolCollection.EventArgs object from a specified
			/// tool.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Tool involved in the event</param>
			public EventArgs(ChangeType changeType, object obj) : base(changeType)
			{
				Tool tool = obj as Tool;
				this.objs = new Tool[1] {tool};
			}

			/// <summary>
			/// Constructs a ToolCollection.EventArgs object from a specified
			/// tool and collection index.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Tool involved in the event</param>
			/// <param name="index">Index position at which the event occurred</param>
			public EventArgs(ChangeType changeType, object obj, int index) : base(changeType, index)
			{
				Tool tool = obj as Tool;
				this.objs = new Tool[1] {tool};
			}

			/// <summary>
			/// Constructs a ToolCollection.EventArgs object from a collection
			/// of tools.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="objs">Collection of tools involved in the event</param>
			public EventArgs(ChangeType changeType, ICollection objs) : base(changeType)
			{
				if (objs == null || objs.Count == 0)
				{
					throw new EInvalidParameter();
				}

				int numObjs = objs.Count;
				this.objs = new Tool[numObjs];
				objs.CopyTo(this.objs, 0);
			}

			/// <summary>
			/// The tool involved in the event.
			/// </summary>
			public Tool Tool
			{
				get
				{
					return this.objs[0];
				}
			}

			/// <summary>
			/// Array of tools involved in the event.
			/// </summary>
			public Tool[] Tools
			{
				get
				{
					return this.objs;
				}
			}

			private Tool[] objs = null;
		}

		/// <summary>
		/// Creates and returns an instance of the ToolCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType)
		{
			return new ToolCollection.EventArgs(changeType);
		}

		/// <summary>
		/// Creates and returns an instance of the ToolCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj)
		{
			return new ToolCollection.EventArgs(changeType, obj);
		}

		/// <summary>
		/// Creates and returns an instance of the ToolCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <param name="index">Zero-based collection index at which the event occurred</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj, int index)
		{
			return new ToolCollection.EventArgs(changeType, obj, index);
		}

		/// <summary>
		/// Creates and returns an instance of the ToolCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="objs">Collection of objects involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, ICollection objs)
		{
			return new ToolCollection.EventArgs(changeType, objs);
		}

		/// <summary>
		/// Called before a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method does nothing because the ToolCollection class
		/// does not fire events.
		/// </para>
		/// </remarks>
		protected override void OnChanging(CollectionEx.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// Called after a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method does nothing because the ToolCollection class
		/// does not fire events.
		/// </para>
		/// </remarks>
		protected override void OnChangeComplete(CollectionEx.EventArgs evtArgs)
		{
		}
	}
}
