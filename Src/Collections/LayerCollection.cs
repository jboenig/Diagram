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
	/// A collection of <see cref="Syncfusion.Windows.Forms.Diagram.Layer"/> objects.
	/// </summary>
	/// <remarks>
	/// This class implements the ILayerContainer interface, which provides host
	/// services for layers.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILayerContainer"/>
	/// </remarks>
	[
	Serializable(),
	Description("Collection of layers"),
	DefaultProperty("Item")
	]
	public class LayerCollection : Syncfusion.Windows.Forms.Diagram.CollectionEx, ILayerContainer, ISerializable
	{
		/// <summary>
		/// Reference to parent property container.
		/// </summary>
		private IPropertyContainer propertyContainer = null;

		/// <summary>
		/// Construct a layer container given a specified parent property container.
		/// </summary>
		public LayerCollection(IPropertyContainer propertyContainer)
		{
			this.propertyContainer = propertyContainer;
		}

		/// <summary>
		/// Serialization constructor for a collection
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected LayerCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.propertyContainer = (IPropertyContainer) info.GetValue("propertyContainer", typeof(IPropertyContainer));
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy from</param>
		public LayerCollection(LayerCollection src) : base(src)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new LayerCollection(this);
		}

		/// <summary>
		/// Gets the layer at the specified index position.
		/// </summary>
		public Layer this[int index]
		{
			get
			{
				return (Layer) this.items[index];
			}
		}

		/// <summary>
		/// Gets the layer matching the given name.
		/// </summary>
		public Layer this[string layerName]
		{
			get
			{
				foreach (Layer layer in this.items)
				{
					if (layer.Name == layerName)
					{
						return layer;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Creates and adds a new layer with the given name.
		/// </summary>
		/// <param name="layerName">Name of layer to create</param>
		/// <returns>Layer created or null on failure</returns>
		/// <remarks>
		/// This method returns null if a layer matching the specified
		/// name already exists.
		/// </remarks>
		public Layer Add(string layerName)
		{
			Layer layer = null;

			if (!this.Contains(layerName))
			{
				layer = new Layer(this, layerName);
				this.List.Add(layer);
			}

			return layer;
		}

		/// <summary>
		/// Creates and inserts a new layer with the given name.
		/// </summary>
		/// <param name="index">Index position at which to insert the new layer</param>
		/// <param name="layerName">Name of the layer to create</param>
		/// <remarks>
		/// This method returns null if a layer matching the specified
		/// name already exists.
		/// </remarks>
		public Layer Insert(int index, string layerName)
		{
			Layer layer = null;

			if (!this.Contains(layerName))
			{
				layer = new Layer(this, layerName);
				this.List.Insert(index, layer);
			}

			return layer;
		}

		/// <summary>
		/// Removes the layer matching the specified name.
		/// </summary>
		/// <param name="layerName">Name of layer to remove</param>
		public void Remove(string layerName)
		{
			int index = this.IndexOf(layerName);
			if (index >= 0)
			{
				this.List.RemoveAt(index);
			}
		}

		/// <summary>
		/// Returns the index position of the layer matching the specified name.
		/// </summary>
		/// <param name="layerName">Name of layer to search for</param>
		/// <returns>Zero-based index position of the layer or -1 if not found</returns>
		public int IndexOf(string layerName)
		{
			int indexFound = -1;
			for (int curIdx = 0; indexFound == -1 && curIdx < this.items.Count; curIdx++)
			{
				Layer curObj = this.items[curIdx] as Layer;
				if (curObj != null && curObj.Name == layerName)
				{
					indexFound = curIdx;
				}
			}
			return indexFound;
		}

		/// <summary>
		/// Determines if the collection contains the specified layer.
		/// </summary>
		/// <param name="layerName">Name of layer to search for</param>
		/// <returns>
		/// true of the collection contains a layer matching the specified
		/// name; otherwise false
		/// </returns>
		public bool Contains(string layerName)
		{
			foreach (Layer curObj in this.items)
			{
				if (curObj.Name == layerName)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Find the layer that contains the given node.
		/// </summary>
		/// <param name="node">Node to search for in the collection of layers</param>
		/// <returns>
		/// Layer object containing the specified node or null if
		/// no layer is found that contains the node
		/// </returns>
		/// <remarks>
		/// A node can belong to only a one layer at any given time.
		/// </remarks>
		public Layer FindNodeLayer(INode node)
		{
			foreach (Layer curLayer in this.items)
			{
				if (curLayer.Contains(node))
				{
					return curLayer;
				}
			}
			return null;
		}

		/// <summary>
		/// The base property container for all layers in the collection.
		/// </summary>
		/// <remarks>
		/// All of the layers in the collection inherit properties from
		/// this property container.
		/// </remarks>
		public IPropertyContainer PropertyContainer
		{
			get
			{
				return this.propertyContainer;
			}
		}

		/// <summary>
		/// Clears out every layer in the collection.
		/// </summary>
		/// <remarks>
		/// This method removes all nodes from every layer in the collection.
		/// It does not reset layer properties.
		/// </remarks>
		public void RemoveAllMembers()
		{
			foreach (Layer curLayer in this.items)
			{
				curLayer.RemoveAll();
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
			info.AddValue("propertyContainer", this.propertyContainer);
		}

		/// <summary>
		/// Event argument class for LayerCollection events.
		/// </summary>
		public new class EventArgs : CollectionEx.EventArgs
		{
			/// <summary>
			/// Construct EventArgs given a change type.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			public EventArgs(ChangeType changeType) : base(changeType)
			{
				this.objs = new Layer[1] {null};
			}

			/// <summary>
			/// Constructs a LayerCollection.EventArgs object from a specified
			/// layer.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Layer involved in the event</param>
			public EventArgs(ChangeType changeType, object obj) : base(changeType)
			{
				Layer layer = obj as Layer;
				this.objs = new Layer[1] {layer};
			}

			/// <summary>
			/// Constructs a LayerCollection.EventArgs object from a specified
			/// layer and collection index.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="obj">Layer involved in the event</param>
			/// <param name="index">Index position at which the event occurred</param>
			public EventArgs(ChangeType changeType, object obj, int index) : base(changeType, index)
			{
				Layer layer = obj as Layer;
				this.objs = new Layer[1] {layer};
			}

			/// <summary>
			/// Constructs a LayerCollection.EventArgs object from a collection
			/// of layers.
			/// </summary>
			/// <param name="changeType">Type of change that occurred</param>
			/// <param name="objs">Collection of layers involved in the event</param>
			public EventArgs(ChangeType changeType, ICollection objs) : base(changeType)
			{
				if (objs == null || objs.Count == 0)
				{
					throw new EInvalidParameter();
				}

				int numObjs = objs.Count;
				this.objs = new Layer[numObjs];
				objs.CopyTo(this.objs, 0);
			}

			/// <summary>
			/// The layer involved in the event.
			/// </summary>
			public Layer Layer
			{
				get
				{
					return this.objs[0];
				}
			}

			/// <summary>
			/// Array of layers involved in the event.
			/// </summary>
			public Layer[] Layers
			{
				get
				{
					return this.objs;
				}
			}

			private Layer[] objs = null;
		}

		/// <summary>
		/// Creates and returns an instance of the LayerCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType)
		{
			return new LayerCollection.EventArgs(changeType);
		}

		/// <summary>
		/// Creates and returns an instance of the LayerCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj)
		{
			return new LayerCollection.EventArgs(changeType, obj);
		}

		/// <summary>
		/// Creates and returns an instance of the LayerCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="obj">Object involved in the event</param>
		/// <param name="index">Zero-based collection index at which the event occurred</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, object obj, int index)
		{
			return new LayerCollection.EventArgs(changeType, obj, index);
		}

		/// <summary>
		/// Creates and returns an instance of the LayerCollection.EventArgs class.
		/// </summary>
		/// <param name="changeType">Type of change that caused the event</param>
		/// <param name="objs">Collection of objects involved in the event</param>
		/// <returns>CollectionEx.EventArgs derived object</returns>
		protected override CollectionEx.EventArgs MakeEventArgs(ChangeType changeType, ICollection objs)
		{
			return new LayerCollection.EventArgs(changeType, objs);
		}

		/// <summary>
		/// Called before a change is made to the collection.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method downcasts the CollectionEx.EventArgs parameter to a
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LayerCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LayerCollection.Changing"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LayerCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChanging(CollectionEx.EventArgs evtArgs)
		{
			if (this.Changing != null)
			{
				LayerCollection.EventArgs typedEvtArgs = evtArgs as LayerCollection.EventArgs;
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LayerCollection.EventArgs"/>
		/// object. Then it fires the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LayerCollection.ChangeComplete"/>
		/// event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.CollectionEx.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LayerCollection.EventArgs"/>
		/// </remarks>
		protected override void OnChangeComplete(CollectionEx.EventArgs evtArgs)
		{
			if (this.ChangeComplete != null)
			{
				LayerCollection.EventArgs typedEvtArgs = evtArgs as LayerCollection.EventArgs;
				if (typedEvtArgs != null)
				{
					this.ChangeComplete(this, typedEvtArgs);
				}
			}
		}

		/// <summary>
		/// Delegate definition for LayerCollection events.
		/// </summary>
		public delegate void EventHandler(object sender, LayerCollection.EventArgs evtArgs);

		/// <summary>
		/// Fired before a change is made to the collection.
		/// </summary>
		public event LayerCollection.EventHandler Changing;

		/// <summary>
		/// Fired after a change is made to the collection.
		/// </summary>
		public event LayerCollection.EventHandler ChangeComplete;
	}
}
