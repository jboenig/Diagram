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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for image nodes.
	/// </summary>
	[Serializable]
	public abstract class ImageNode : INode, IBounds2DF, IDraw, ITransform, IHitTestBounds, ILogicalUnitContainer, IPropertyContainer, ISerializable, IDeserializationCallback
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ImageNode()
		{
			this.propertyValues = new Hashtable();
			SetDefaultPropertyValues();
			this.lineStyle = new LineStyle(this);
			this.bounds = new RectangleF(0,0,0,0);
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="src">ImageNode to copy</param>
		public ImageNode(ImageNode src)
		{
			this.propertyValues = (Hashtable) src.propertyValues.Clone();
			this.lineStyle = new LineStyle(this);
			this.bounds = src.Bounds;
		}

		/// <summary>
		/// Serialization constructor for images.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected ImageNode(SerializationInfo info, StreamingContext context)
		{
			this.name = info.GetString("name");
			this.parent = (ICompositeNode) info.GetValue("parent", typeof(ICompositeNode));
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
			this.bounds = (RectangleF) info.GetValue("bounds", typeof(RectangleF));
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public abstract object Clone();

		/// <summary>
		/// Reference to the Image object contained by this node.
		/// </summary>
		public abstract System.Drawing.Image Image
		{
			get;
		}

		/// <summary>
		/// 
		/// </summary>
		public abstract System.Drawing.GraphicsUnit GraphicsUnit
		{
			get;
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
		///   N/A
		/// </para>
		/// </remarks>
		public object GetService(System.Type svcType)
		{
			return null;
		}

		/// <summary>
		/// Reference to the composite node this node is a child of.
		/// </summary>
		[Browsable(false)]
		public ICompositeNode Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		/// <summary>
		/// The root node in the node hieararchy.
		/// </summary>
		/// <remarks>
		/// The root node is found by following the chain of parent nodes until
		/// a node is found that has a null parent.
		/// </remarks>
		[Browsable(false)]
		public INode Root
		{
			get
			{
				if (this.parent == null)
				{
					return this;
				}
				return this.parent.Root;
			}
		}

		/// <summary>
		/// Name of the node.
		/// </summary>
		/// <remarks>
		/// Must be unique within the scope of the parent node.
		/// </remarks>
		[
		Browsable(true),
		Category("General"),
		Description("Unique name of image node")
		]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>
		/// Fully qualified name of the node.
		/// </summary>
		/// <remarks>
		/// The full name is the name of the node concatenated with the names
		/// of all parent nodes.
		/// </remarks>
		[
		Browsable(true),
		Category("General"),
		Description("Fully-qualified name of image node")
		]
		public string FullName
		{
			get
			{
				if (this.parent == null)
				{
					return this.name;
				}
				return this.parent.FullName + "." + this.name;
			}
		}

		/// <summary>
		/// The image's bounding box.
		/// </summary>
		[Browsable(false)]
		public virtual RectangleF Bounds
		{
			get
			{
				PointF[] boundingPts = new PointF[] { new PointF(this.bounds.Left, this.bounds.Top), new PointF(this.bounds.Right, this.bounds.Bottom) };
				Matrix worldMatrix = Global.MatrixStack.Peek();
				if (worldMatrix != null)
				{
					worldMatrix.TransformPoints(boundingPts);
				}

				return Geometry.CreateRect(boundingPts);
			}
			set
			{
				RectangleF oldBounds = this.bounds;
				this.bounds = value;
				this.OnBoundsChanged(new BoundsEventArgs(this, oldBounds, this.Bounds));
			}
		}

		/// <summary>
		/// X-coordinate of the image's location.
		/// </summary>
		[
		Browsable(true),
		Category("Bounds")
		]
		public virtual float X
		{
			get
			{
				return this.Bounds.Left;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Location = new PointF(value, rcBounds.Top);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Y-coordinate of the image's location.
		/// </summary>
		[
		Browsable(true),
		Category("Bounds")
		]
		public virtual float Y
		{
			get
			{
				return this.Bounds.Top;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Location = new PointF(rcBounds.Left, value);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Width of the image's bounding box.
		/// </summary>
		[
		Browsable(true),
		Category("Bounds")
		]
		public virtual float Width
		{
			get
			{
				return this.Bounds.Width;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Size = new SizeF(value, rcBounds.Height);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Height of the image's bounding box.
		/// </summary>
		[
		Browsable(true),
		Category("Bounds")
		]
		public virtual float Height
		{
			get
			{
				return this.Bounds.Height;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Size = new SizeF(rcBounds.Width, value);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Renders the image onto a System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Graphics context to render onto</param>
		public virtual void Draw(System.Drawing.Graphics grfx)
		{
			Matrix prevXform = grfx.Transform;
			grfx.Transform = Global.MatrixStack.Peek();
			grfx.MultiplyTransform(Global.ViewMatrix, MatrixOrder.Append);

			Pen pen = this.lineStyle.CreatePen();
			System.Drawing.Image img = this.Image;
			if (img != null)
			{
				grfx.DrawImage(img, this.bounds);
			}
			else
			{
				grfx.DrawRectangle(pen, this.bounds.Left, this.bounds.Top, this.bounds.Width, this.bounds.Height);
			}
			pen.Dispose();

			grfx.Transform = prevXform;
		}

		// Begin interface (IHitTestBounds)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptTest"></param>
		/// <param name="fSlop"></param>
		/// <returns></returns>
		bool IHitTestBounds.ContainsPoint(PointF ptTest, float fSlop)
		{
			RectangleF bounds = this.Bounds;
			return bounds.Contains(ptTest);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcTest"></param>
		/// <returns></returns>
		bool IHitTestBounds.IntersectsRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return bounds.IntersectsWith(rcTest);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcTest"></param>
		/// <returns></returns>
		bool IHitTestBounds.ContainedByRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return rcTest.Contains(bounds);
		}

		#region ILogicalUnitContainer interface

		/// <summary>
		/// Converts the logical values contained by the object from one unit of
		/// measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="grfx">Graphics context object for converting device units</param>
		/// <remarks>
		/// <para>
		/// This method converts all logical unit values contained by the object from
		/// one unit of measure to another.
		/// </para>
		/// </remarks>
		void ILogicalUnitContainer.ConvertLogicalUnits(GraphicsUnit fromUnits, GraphicsUnit toUnits, Graphics grfx)
		{
			// Convert line width
			if (this.propertyValues.Contains("LineWidth"))
			{
				float lineWidth = (float) this.propertyValues["LineWidth"];
				lineWidth = Measurements.Convert(fromUnits, toUnits, grfx, lineWidth);
				this.propertyValues["LineWidth"] = lineWidth;
			}

			// Convert bounds
			this.bounds = Measurements.Convert(fromUnits, toUnits, grfx, this.bounds);
		}

		/// <summary>
		/// Converts the logical values contained by the object from one scale to
		/// another.
		/// </summary>
		/// <param name="fromScale">Scale to convert from</param>
		/// <param name="toScale">Scale to convert to</param>
		/// <remarks>
		/// <para>
		/// This method scales all logical unit values contained by the object.
		/// </para>
		/// </remarks>
		void ILogicalUnitContainer.ConvertLogicalScale(float fromScale, float toScale)
		{
		}

		#endregion

		// Begin interface (IPropertyContainer)

		/// <summary>
		/// Sets the default property values for the image.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// image to their default values.
		/// </remarks>
		public virtual void SetDefaultPropertyValues()
		{
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

			IPropertyContainer parentProps = this.parent as IPropertyContainer;
			if (parentProps != null)
			{
				return parentProps.GetPropertyValue(propertyName);
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

			this.OnPropertyChanged(new PropertyEventArgs(this, propertyName, oldVal, val));
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
			object oldVal = null;

			if (this.propertyValues.ContainsKey(propertyName))
			{
				oldVal = this.propertyValues[propertyName];
				this.propertyValues[propertyName] = val;
				this.OnPropertyChanged(new PropertyEventArgs(this, propertyName, oldVal, val));
			}
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
		/// Called when a property is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ImageNode.Parent"/>
		/// of the property change by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.PropertyChanged"/>
		/// method.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnPropertyChanged(PropertyEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.PropertyChanged(evtArgs);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		void ITransform.Translate(float dx, float dy)
		{
			this.bounds.Offset(dx, dy);
		}

		/// <summary>
		/// Currently not implemented for bitmaps
		/// </summary>
		/// <param name="ptAnchor"></param>
		/// <param name="degrees"></param>
		void ITransform.Rotate(PointF ptAnchor, float degrees)
		{
			throw new EInvalidOperation();
		}

		/// <summary>
		/// Currently not implemented for bitmaps
		/// </summary>
		/// <param name="degrees"></param>
		void ITransform.Rotate(float degrees)
		{
			throw new EInvalidOperation();
		}

		/// <summary>
		/// Currently not implemented for bitmaps
		/// </summary>
		/// <param name="ptAnchor"></param>
		/// <param name="sx"></param>
		/// <param name="sy"></param>
		void ITransform.Scale(PointF ptAnchor, float sx, float sy)
		{
			throw new EInvalidOperation();
		}

		/// <summary>
		/// Scales the node about its center point by a given ratio.
		/// </summary>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		void ITransform.Scale(float sx, float sy)
		{
			throw new EInvalidOperation();
		}

		/// <summary>
		/// 
		/// </summary>
		Matrix ITransform.LocalTransform
		{
			get
			{
				return new Matrix();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		Matrix ITransform.WorldTransform
		{
			get
			{
				Matrix worldTransform = new Matrix();

				if (this.parent != null)
				{
					ITransform objTransformParent = this.parent as ITransform;
					if (objTransformParent != null)
					{
						worldTransform.Multiply(objTransformParent.WorldTransform, MatrixOrder.Append);
					}
				}

				return worldTransform;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		Matrix ITransform.ParentTransform
		{
			get
			{
				Matrix parentTransform = null;

				if (this.parent != null)
				{
					ITransform objTransformParent = this.parent as ITransform;
					if (objTransformParent != null)
					{
						parentTransform = objTransformParent.WorldTransform;
					}
				}

				if (parentTransform == null)
				{
					parentTransform = new Matrix();
				}

				return parentTransform;
			}
		}

		/// <summary>
		/// Called when the bounds of the node change.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.ImageNode.Parent"/>
		/// of the change in bounds by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.BoundsChanged"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoundsEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnBoundsChanged(BoundsEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.BoundsChanged(evtArgs);
				}
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
			info.AddValue("name", this.name);
			info.AddValue("parent", this.parent);
			info.AddValue("propertyValues", this.propertyValues);
			info.AddValue("bounds", this.bounds);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.lineStyle = new LineStyle(this);
		}

		/// <summary>
		/// Name of the image node
		/// </summary>
		private string name = null;

		/// <summary>
		/// Reference to the parent node
		/// </summary>
		private ICompositeNode parent = null;

		/// <summary>
		/// Hashtable containing property/value pairs
		/// </summary>
		protected Hashtable propertyValues = null;

		/// <summary>
		/// Properties for drawing lines
		/// </summary>
		protected LineStyle lineStyle = null;

		/// <summary>
		/// Bounds of the image
		/// </summary>
		private RectangleF bounds;
	}
}