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
	/// Late-binding interface for accessing dynamic properties.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface provides access to the properties of an object. There
	/// are several advantages to using this interface. Code accessing the
	/// properties of an object do not need to be coupled to the object type;
	/// it only requires this interface. Objects that support this interface
	/// have flexibility in how it is implemented. For example, a class can
	/// support both static (compile-time) properties and dynamic (run-time)
	/// properties through this interface. Property inheritance is also
	/// an important feature made possible by this interface.
	/// </para>
	/// </remarks>
	public interface IPropertyContainer
	{
		/// <summary>
		/// Sets the default property values for the container.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// container to their default values.
		/// </remarks>
		void SetDefaultPropertyValues();

		/// <summary>
		/// Retrieve the value of a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to retrieve</param>
		/// <returns>Value of the named property or null if it doesn't exist</returns>
		object GetPropertyValue(string propertyName);

		/// <summary>
		/// Assign a value to a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to set</param>
		/// <param name="val">Value to assign to property</param>
		/// <remarks>
		/// This method will add the property to the container if it doesn't
		/// already exist.
		/// </remarks>
		void SetPropertyValue(string propertyName, object val);

		/// <summary>
		/// Assign a value to a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to change</param>
		/// <param name="val">Value to assign to property</param>
		/// <remarks>
		/// This method only modifies property values that already exist
		/// in the container. If the property does not exist, this method fails.
		/// </remarks>
		void ChangePropertyValue(string propertyName, object val);

		/// <summary>
		/// Removes the specified property.
		/// </summary>
		/// <param name="propertyName">Name of property to remove</param>
		void RemoveProperty(string propertyName);

		/// <summary>
		/// Returns an array containing the names of all properties in the container.
		/// </summary>
		/// <returns>String array containing property names</returns>
		string[] GetPropertyNames();
	}

	/// <summary>
	/// Default implementation of the IPropertyContainer interface.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
	/// </remarks>
	[Serializable()]
	public class PropertyContainer : IPropertyContainer, ISerializable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public PropertyContainer()
		{
			this.SetDefaultPropertyValues();
		}

		/// <summary>
		/// Serialization constructor for property containers.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected PropertyContainer(SerializationInfo info, StreamingContext context)
		{
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
		}

		/// <summary>
		/// Sets the default property values for the property container.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// property container to their default values.
		/// </remarks>
		public virtual void SetDefaultPropertyValues()
		{
		}

		/// <summary>
		/// Retrieve the value of a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to retrieve</param>
		/// <returns>Value of the named property or null if it doesn't exist</returns>
		public object GetPropertyValue(string propertyName)
		{
			if (this.propertyValues.Contains(propertyName))
			{
				return this.propertyValues[propertyName];
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
		public void SetPropertyValue(string propertyName, object val)
		{
			object oldVal = null;

			if (this.propertyValues.ContainsKey(propertyName))
			{
				oldVal = this.propertyValues[propertyName];
				this.propertyValues[propertyName] = val;
			}
			else
			{
				this.propertyValues.Add(propertyName, val);
			}

			this.OnPropertyChanged(new PropertyEventArgs(null, propertyName, oldVal, val));
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
		public void ChangePropertyValue(string propertyName, object val)
		{
			object oldVal = null;

			if (this.propertyValues.ContainsKey(propertyName))
			{
				oldVal = this.propertyValues[propertyName];
				this.propertyValues[propertyName] = val;
				this.OnPropertyChanged(new PropertyEventArgs(null, propertyName, oldVal, val));
			}
		}

		/// <summary>
		/// Removes the specified property.
		/// </summary>
		/// <param name="propertyName">Name of property to remove</param>
		public void RemoveProperty(string propertyName)
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
		public string[] GetPropertyNames()
		{
			string[] propertyNames = new string[this.propertyValues.Keys.Count];
			this.propertyValues.Keys.CopyTo(propertyNames, 0);
			return propertyNames;
		}

		/// <summary>
		/// Determines whether the model fires events or not.
		/// </summary>
		/// <remarks>
		/// This property is useful for temporarily disabling events during
		/// time critical operations.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool EventsEnabled
		{
			get
			{
				return this.eventsEnabled;
			}
			set
			{
				this.eventsEnabled = value;
			}
		}

		/// <summary>
		/// Called when a property is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Fires the PropertyChanged event.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// </remarks>
		protected virtual void OnPropertyChanged(PropertyEventArgs evtArgs)
		{
			if (this.eventsEnabled && this.PropertyChanged != null)
			{
				this.PropertyChanged(this, evtArgs);
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
			info.AddValue("propertyValues", this.propertyValues);
		}

		/// <summary>
		/// Fired when a property in the container is changed.
		/// </summary>
		public event PropertyEventHandler PropertyChanged;

		/// <summary>
		/// Hashtable containing property name/value pairs
		/// </summary>
		private Hashtable propertyValues = new Hashtable();

		/// <summary>
		/// Determines if events will be fired or not.
		/// </summary>
		private bool eventsEnabled = true;
	}
}
