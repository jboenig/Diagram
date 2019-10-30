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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A layer is a collection of nodes that share a common set of default properties
	/// and the same Z-order relative to other layers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A layer contains zero or more nodes and is responsible for rendering
	/// those nodes. Since the nodes in a layer are rendered as a group, their
	/// Z-order is the same relative to other layers in the diagram. For example,
	/// if layer A has a higher Z-order than layer B, then all nodes in layer B
	/// will be rendered behind those in layer A. If the Visible flag on a layer
	/// is set to false, none of the nodes in the layer will be rendered.
	/// </para>
	/// <para>
	/// The nodes in the layer can inherit properties from the layer. If a
	/// property is not explicitly set in a node, the node inherits the property
	/// from the layer. If the layer does not have the property set, then the
	/// layer chains up to the model to get the property. This allows all nodes
	/// in a layer to share the same defaults.
	/// </para>
	/// <para>
	/// Nodes are added to the layer using the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.Add"/> method and
	/// removed using the <see cref="Syncfusion.Windows.Forms.Diagram.Layer.Remove"/>
	/// method. A node can only belong to one layer at a time. When a node is added
	/// to a layer, the layer's
	/// <see  cref="Syncfusion.Windows.Forms.Diagram.Layer.Container"/> is
	/// notified so that it can remove it from any other layers that it might
	/// already belong to.
	/// </para>
	/// <para>
	/// All nodes in the layer can be hidden by setting the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.Visible"/> flag
	/// to false.
	/// </para>
	/// <para>
	/// The Z-order of nodes within a layer can be changed using the following
	/// methods: <see cref="Syncfusion.Windows.Forms.Diagram.Layer.SetZOrder"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.BringForward"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.BringToFront"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.SendBackward"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.SendToBack"/>.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILayerContainer"/>
	/// </remarks>
	[Serializable]
	public class Layer : IServiceProvider, ICollection, IEnumerable, IDraw, IPropertyContainer, ISerializable, IDeserializationCallback
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public Layer()
		{
			this.container = null;
			this.propertyValues = new Hashtable();
			this.SetDefaultPropertyValues();
			this.members = new ArrayList();
			this.drawObjects = new ArrayList();
		}

		/// <summary>
		/// Constructs a Layer and attaches it to the specified layer container.
		/// </summary>
		/// <param name="container">Container to attach the layer to</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILayerContainer"/>
		/// </remarks>
		public Layer(ILayerContainer container)
		{
			this.container = container;
			this.propertyValues = new Hashtable();
			this.SetDefaultPropertyValues();
			this.members = new ArrayList();
			this.drawObjects = new ArrayList();
		}

		/// <summary>
		/// Constructs a Layer with a specified name and attaches it to the
		/// given layer container.
		/// </summary>
		/// <param name="container">Container to attach the layer to</param>
		/// <param name="layerName">Name to give the layer</param>
		public Layer(ILayerContainer container, string layerName)
		{
			this.container = container;
			this.propertyValues = new Hashtable();
			this.SetDefaultPropertyValues();
			this.Name = layerName;
			this.members = new ArrayList();
			this.drawObjects = new ArrayList();
		}

		/// <summary>
		/// Serialization constructor for layers.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Layer(SerializationInfo info, StreamingContext context)
		{
			this.container = (ILayerContainer) info.GetValue("container", typeof(ILayerContainer));
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
			this.members = (ArrayList) info.GetValue("members", typeof(ArrayList));
			this.drawObjects = new ArrayList();
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnMembershipChange();
		}

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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// </para>
		/// </remarks>
		object IServiceProvider.GetService(System.Type svcType)
		{
			if (svcType == typeof(IPropertyContainer))
			{
				return (IPropertyContainer) this;
			}
			return null;
		}

		/// <summary>
		/// The number of nodes in the layer.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int Count
		{
			get
			{
				return this.members.Count;
			}
		}

		/// <summary>
		/// Determines if the collection is thread-safe
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool IsSynchronized
		{
			get
			{
				return this.members.IsSynchronized;
			}
		}

		/// <summary>
		/// Object that can be used to synchronize access to the collection
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public object SyncRoot
		{
			get
			{
				return this.members.SyncRoot;
			}
		}

		/// <summary>
		/// Copies the list or a portion of the list to an array
		/// </summary>
		/// <param name="array">Array in which to copy the items in the list</param>
		/// <param name="index">Index at which to start copying in target array</param>
		public void CopyTo(Array array, int index)
		{
			this.members.CopyTo(array, index);
		}

		/// <summary>
		/// Returns an enumerator that can be used to iterate through the nodes
		/// in the layer.
		/// </summary>
		/// <returns>Enumerator for iterating through layer</returns>
		public IEnumerator GetEnumerator()
		{
			return this.members.GetEnumerator();
		}

		/// <summary>
		/// Determines if the specified node belongs to the layer.
		/// </summary>
		/// <param name="node">Node to search for</param>
		/// <returns>
		/// true if the node belongs to the layer, otherwise false
		/// </returns>
		public bool Contains(INode node)
		{
			return this.members.Contains(node);
		}

		/// <summary>
		/// Object that contains the layer.
		/// </summary>
		/// <remarks>
		/// Each layer has a reference to a container object for the purpose of
		/// synchronizing changes in layers. This is necesssary because a node
		/// can only belong to a single layer at a time. When a node is added to
		/// one layer, it must be removed from any other layer it might already
		/// belong to.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILayerContainer"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LayerCollection"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public ILayerContainer Container
		{
			get
			{
				return this.container;
			}
			set
			{
				this.container = value;
			}
		}

		/// <summary>
		/// Name of the layer.
		/// </summary>
		[
		Browsable(true)
		]
		public string Name
		{
			get
			{
				object value = this.GetPropertyValue("Name");
				if (value != null)
				{
					return (string) value;
				}
				return null;
			}
			set
			{
				object curVal = this.GetPropertyValue("Name");
				if (curVal == null || ((string) curVal) != value)
				{
					if (this.container == null || !this.container.Contains(value))
					{
						this.SetPropertyValue("Name", value);
					}
				}
			}
		}

		/// <summary>
		/// Adds a node the layer.
		/// </summary>
		/// <param name="node">Node to add</param>
		/// <remarks>
		/// <para>
		/// If the node already belongs to this layer, this method does nothing.
		/// Before adding the node to this layer, this method queries the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.Container"/>
		/// object to see if the node already belongs to another layer. If it
		/// does, then this method removes the node from that layer.
		/// </para>
		/// <para>
		/// The node is then added to this layer and the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// method is called.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer.Remove"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// </remarks>
		public void Add(INode node)
		{
			if (!this.members.Contains(node))
			{
				if (this.container != null)
				{
					Layer curLayer = this.container.FindNodeLayer(node);
					if (curLayer != null)
					{
						curLayer.Remove(node);
					}
				}
				this.members.Add(node);
				this.OnMembershipChange();
			}
		}

		/// <summary>
		/// Removes the specified node from this layer.
		/// </summary>
		/// <param name="node">Node to remove</param>
		/// <remarks>
		/// <para>
		/// If the node does not belong to this layer, this method does nothing.
		/// If the node is found and removed from this layer, the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// method is called.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer.Add"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// </remarks>
		public void Remove(INode node)
		{
			int index = this.members.IndexOf(node);
			if (index >= 0)
			{
				this.members.RemoveAt(index);
				this.OnMembershipChange();
			}
		}

		/// <summary>
		/// Removes all nodes from this layer.
		/// </summary>
		public void RemoveAll()
		{
			this.members.Clear();
			this.drawObjects.Clear();
		}

		/// <summary>
		/// Sets the Z-order of the given node within the layer.
		/// </summary>
		/// <param name="node">Node to set Z-order for</param>
		/// <param name="index">Zero-based z-order value</param>
		/// <remarks>
		/// <para>
		/// If the specified node doesn't belong to this layer,
		/// this method does nothing. If the node is found, it is
		/// removed and re-inserted at the given index position and
		/// the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// method is called.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// </remarks>
		public void SetZOrder(INode node, int index)
		{
			if (index >= 0 && index < this.members.Count)
			{
				int curIndex = this.members.IndexOf(node);
				if (curIndex >= 0 && curIndex != index)
				{
					this.members.RemoveAt(curIndex);
					this.members.Insert(index, node);
					this.OnMembershipChange();
				}
			}
		}

		/// <summary>
		/// Moves the specified node in the layer forward in the Z-order.
		/// </summary>
		/// <param name="node">Node to move forward</param>
		/// <remarks>
		/// <para>
		/// If the specified node doesn't belong to this layer,
		/// this method does nothing. If the node is found, it is
		/// removed and re-inserted at 1 minus its previous index and
		/// the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// method is called.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// </remarks>
		public void BringForward(INode node)
		{
			int index = this.members.IndexOf(node);
			if (index > 0)
			{
				this.members.RemoveAt(index);
				this.members.Insert(index-1, node);
				this.OnMembershipChange();
			}
		}

		/// <summary>
		/// Sends the specified node in the layer back in the Z-order.
		/// </summary>
		/// <param name="node">Node to move backward</param>
		/// <remarks>
		/// <para>
		/// If the specified node doesn't belong to this layer,
		/// this method does nothing. If the node is found, it is
		/// removed and re-inserted at 1 plus its previous index and
		/// the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// method is called.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// </remarks>
		public void SendBackward(INode node)
		{
			int index = this.members.IndexOf(node);
			if (index >= 0 && index < (this.members.Count-1))
			{
				this.members.RemoveAt(index);
				this.members.Insert(index+1, node);
				this.OnMembershipChange();
			}
		}

		/// <summary>
		/// Brings the specified node in the layer to the front of the Z-order.
		/// </summary>
		/// <param name="node">Node to bring to the front</param>
		/// <remarks>
		/// <para>
		/// If the specified node doesn't belong to this layer,
		/// this method does nothing. If the node is found, it is
		/// removed and re-inserted at index 0 and the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// method is called.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// </remarks>
		public void BringToFront(INode node)
		{
			int index = this.members.IndexOf(node);
			if (index > 0 && index < this.members.Count)
			{
				this.members.RemoveAt(index);
				this.members.Insert(0, node);
				this.OnMembershipChange();
			}
		}

		/// <summary>
		/// Sends the specified node in the layer to the back of the Z-order.
		/// </summary>
		/// <param name="node">Node to send to the back</param>
		/// <remarks>
		/// <para>
		/// If the specified node doesn't belong to this layer,
		/// this method does nothing. If the node is found, it is
		/// removed and re-inserted at the end of the list and the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// method is called.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer.OnMembershipChange"/>
		/// </remarks>
		public void SendToBack(INode node)
		{
			int index = this.members.IndexOf(node);
			if (index > 0 && index < this.members.Count)
			{
				this.members.RemoveAt(index);
				this.members.Add(node);
				this.OnMembershipChange();
			}
		}

		/// <summary>
		/// Indicates if the layer is visible.
		/// </summary>
		/// <remarks>
		/// If this flag is set to false, none of the nodes belonging to the layer
		/// will be rendered.
		/// </remarks>
		[
		Browsable(true)
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
		/// Indicates if the layer is enabled.
		/// </summary>
		[
		Browsable(true)
		]
		public bool Enabled
		{
			get
			{
				return (bool) this.GetPropertyValue("Enabled");
			}
			set
			{
				this.SetPropertyValue("Enabled", value);
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
			info.AddValue("container", this.container);
			info.AddValue("propertyValues", this.propertyValues);
			info.AddValue("members", this.members);
		}

		/// <summary>
		/// Renders the layer onto a System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Graphics context to draw onto</param>
		/// <remarks>
		/// Iterates through each node belonging to the layer and draws it
		/// onto the System.Drawing.Graphics object. If the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Layer.Visible"/>
		/// flag is set to false, this method does nothing.
		/// </remarks>
		public void Draw(Graphics grfx)
		{
			if (this.Visible)
			{
				foreach (IDraw drawObj in this.drawObjects)
				{
					drawObj.Draw(grfx);
				}
			}
		}

		// Begin interface (IPropertyContainer)

		/// <summary>
		/// Sets the default property values for the layer.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// layer to their default values.
		/// </remarks>
		public virtual void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("Name", "Layer");
			this.propertyValues.Add("Visible", true);
			this.propertyValues.Add("Enabled", true);
		}

		/// <summary>
		/// Retrieve the value of a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to retrieve</param>
		/// <returns>Value of the named property or null if it doesn't exist</returns>
		public virtual object GetPropertyValue(string propertyName)
		{
			if (this.propertyValues.Contains(propertyName))
			{
				return this.propertyValues[propertyName];
			}

			if (this.container != null)
			{
				IPropertyContainer containerProps = this.container.PropertyContainer;
				if (containerProps != null)
				{
					return containerProps.GetPropertyValue(propertyName);
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
			if (this.propertyValues.ContainsKey(propertyName))
			{
				this.propertyValues[propertyName] = val;
			}
			else
			{
				this.propertyValues.Add(propertyName, val);
			}
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
			this.propertyValues[propertyName] = val;
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

		/// <summary>
		/// Called when the list of member nodes in the layer changes.
		/// </summary>
		protected virtual void OnMembershipChange()
		{
			this.drawObjects.Clear();
			foreach (INode curNode in this.members)
			{
				IDraw curDrawObj = curNode as IDraw;
				if (curDrawObj != null)
				{
					this.drawObjects.Add(curDrawObj);
				}
			}
		}

		private ILayerContainer container = null;
		private ArrayList members = null;
		private ArrayList drawObjects = null;
		private Hashtable propertyValues = null;
	}
}
