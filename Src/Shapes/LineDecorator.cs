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
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for line decorators.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A line decorator is an object that adorns an endpoint of a line or other shape.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.FilledLineDecorator"/>
	/// </remarks>
	[Serializable]
	public abstract class LineDecorator : IGraphics, IHitTestRegion, IPropertyContainer, ISerializable, IDeserializationCallback, ICloneable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LineDecorator()
		{
			this.line = null;
			this.linePoints = null;
			this.propertyValues = new Hashtable();
			this.SetDefaultPropertyValues();
			this.CreateStyles();
		}

		/// <summary>
		/// Constructs a line decorator and attaches it to the given node.
		/// </summary>
		/// <param name="line">Node to attach to</param>
		public LineDecorator(INode line)
		{
			this.line = line;
			this.linePoints = this.line as IPoints;
			this.propertyValues = new Hashtable();
			this.SetDefaultPropertyValues();
			this.CreateStyles();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy</param>
		public LineDecorator(LineDecorator src)
		{
			this.line = null;
			this.linePoints = null;
			this.propertyValues = (Hashtable) src.propertyValues.Clone();
			this.CreateStyles();
		}

		/// <summary>
		/// Serialization constructor for line decorators.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected LineDecorator(SerializationInfo info, StreamingContext context)
		{
			this.endPointType = (LineEndPoint) info.GetValue("endPointType", typeof(LineEndPoint));
			this.line = (INode) info.GetValue("line", typeof(INode));
			this.linePoints = (IPoints) info.GetValue("linePoints", typeof(IPoints));
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public abstract object Clone();

		/// <summary>
		/// Reference to the node the decorator is attached to.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This can be any type of node that supports the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPoints"/> interface.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public INode Line
		{
			get
			{
				return this.line;
			}
			set
			{
				this.line = value;

				if (this.line != null)
				{
					this.linePoints = this.line as IPoints;
				}
				else
				{
					this.linePoints = null;
				}
			}
		}

		/// <summary>
		/// Determines which endpoint of the attached node to decorate.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Determines if the decorator is attached to the first or last point in
		/// the owner node.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecorator.Line"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public LineEndPoint EndPointType
		{
			get
			{
				return this.endPointType;
			}
			set
			{
				this.endPointType = value;
			}
		}

		/// <summary>
		/// Returns the current location of the owner's endpoint.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Retrieves either the first or last point from the attached
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecorator.Line"/>
		/// adorned by this decorator. The
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LineDecorator.EndPointType"/>
		/// property determines if the first or last point is returned.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecorator.Line"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public PointF EndPointLocation
		{
			get
			{
				PointF ptLoc;
				int numPts = 0;

				if (this.linePoints == null)
				{
					throw new EInvalidOperation();
				}

				numPts = this.linePoints.PointCount;

				if (numPts < 1)
				{
					throw new EInvalidOperation();
				}

				if (this.endPointType == LineEndPoint.First)
				{
					ptLoc = this.linePoints.GetPoint(0);
				}
				else
				{
					ptLoc = this.linePoints.GetPoint(numPts-1);
				}

				return ptLoc;
			}
		}

		/// <summary>
		/// Line drawing properties for this node.
		/// </summary>
		/// <remarks>
		/// The line style determines the configuration of the pen used to
		/// render lines.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineStyle"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public LineStyle LineStyle
		{
			get
			{
				return this.lineStyle;
			}
		}

		/// <summary>
		/// Renders the line decorator onto a System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Graphics context to render onto</param>
		public virtual void Draw(System.Drawing.Graphics grfx)
		{
			GraphicsPath grfxPath = this.GraphicsPath;
			if (grfxPath != null)
			{
				GraphicsPath grfxDraw = (GraphicsPath) grfxPath.Clone();
				Matrix xform = Global.MatrixStack.Peek();
				grfxDraw.Transform(xform);
				Pen pen = this.lineStyle.CreatePen();
				grfx.DrawPath(pen, grfxDraw);
				pen.Dispose();
			}
		}

		/// <summary>
		/// Encapsulates the points and instructions needed to render the line decorator.
		/// </summary>
		/// <remarks>
		/// The get method for this property is overridden by derived classes.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecorator.CreateRegion"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract System.Drawing.Drawing2D.GraphicsPath GraphicsPath
		{
			get;
		}

		/// <summary>
		/// Returns an object that describes the interior of the line decorator.
		/// </summary>
		/// <param name="padding">Amount of padding to add</param>
		/// <returns>System.Drawing.Region object</returns>
		/// <remarks>
		/// Region objects are used for hit testing and geometrical calculations.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecorator.GraphicsPath"/>
		/// </remarks>
		public virtual System.Drawing.Region CreateRegion(float padding)
		{
			GraphicsPath grfxPath = this.GraphicsPath;

			if (grfxPath != null)
			{
				return new System.Drawing.Region(grfxPath);
			}

			return null;
		}

		/// <summary>
		/// Tests the region of the line decorator to determine if the given point is
		/// contained by the region.
		/// </summary>
		/// <param name="ptTest">Point to test</param>
		/// <param name="fSlop">Amount of padding to add for hit test</param>
		/// <returns>true if the point is within the region; otherwise false</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecorator.CreateRegion"/>
		/// </remarks>
		bool IHitTestRegion.ContainsPoint(PointF ptTest, float fSlop)
		{
			bool hit = false;
			System.Drawing.Region rgn = this.CreateRegion(fSlop);
			if (rgn != null)
			{
				hit = rgn.IsVisible(ptTest);
			}
			return hit;
		}

		/// <summary>
		/// Tests the region of the line decorator to determine if the given rectangle
		/// touches the region.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if the rectangle touches the region; otherwise false</returns>
		bool IHitTestRegion.IntersectsRect(RectangleF rcTest)
		{
			bool hit = false;
			GraphicsPath grfxPath = this.GraphicsPath;

			if (grfxPath != null)
			{
				RectangleF bounds = grfxPath.GetBounds();
				hit = rcTest.Contains(bounds);
			}
			return hit;
		}

		/// <summary>
		/// Tests the region of the line decorator to determine if the region is
		/// entirely contained by the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if the rectangle contains the region; otherwise false</returns>
		bool IHitTestRegion.ContainedByRect(RectangleF rcTest)
		{
			bool hit = false;
			GraphicsPath grfxPath = this.GraphicsPath;

			if (grfxPath != null)
			{
				RectangleF bounds = grfxPath.GetBounds();
				hit = rcTest.Contains(bounds);
			}
			return hit;
		}

		/// <summary>
		/// Sets the default property values for the line decorator.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// line decorator to their default values.
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

			IPropertyContainer parentProps = this.line as IPropertyContainer;
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
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("endPointType", this.endPointType);
			info.AddValue("line", this.line);
			info.AddValue("linePoints", this.linePoints);
			info.AddValue("propertyValues", this.propertyValues);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.CreateStyles();
		}

		/// <summary>
		/// Creates style objects for this class.
		/// </summary>
		protected virtual void CreateStyles()
		{
			this.lineStyle = new LineStyle(this);
		}

		/// <summary>
		/// 
		/// </summary>
		private LineEndPoint endPointType = LineEndPoint.Last;

		/// <summary>
		/// 
		/// </summary>
		private INode line = null;

		/// <summary>
		/// 
		/// </summary>
		protected IPoints linePoints = null;

		/// <summary>
		/// 
		/// </summary>
		protected Hashtable propertyValues = null;

		/// <summary>
		/// 
		/// </summary>
		private LineStyle lineStyle = null;
	}
}