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
	/// Implements a circle line decorator.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a line decorator that draws itself as a circle.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecorator"/>
	/// </remarks>
	[Serializable]
	public class CircleDecorator : FilledLineDecorator, ISerializable, IDeserializationCallback
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public CircleDecorator() : base()
		{
		}

		/// <summary>
		/// Constructs an open arrow line decorator and attaches it to the given node.
		/// </summary>
		/// <param name="line">Node to attach to</param>
		public CircleDecorator(INode line) : base(line)
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy</param>
		public CircleDecorator(CircleDecorator src) : base(src)
		{
			this.radius = src.radius;
		}

		/// <summary>
		/// Serialization constructor for arrow decorators.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected CircleDecorator(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.radius = info.GetSingle("radius");
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new CircleDecorator(this);
		}

		/// <summary>
		/// Radius of the circle decorator.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default size is 5 logical units.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Appearance"),
		Description("Radius of the circle decorator")
		]
		public float Radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				this.radius = value;
			}
		}

		/// <summary>
		/// Encapsulates the points and instructions needed to render the open arrow
		/// line decorator.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The get method of this property creates a new GraphicsPath object that
		/// consists of an ellipse. The center of the ellipse corresponds to the
		/// the endpoint that the decorator is attached to.
		/// </para>
		/// </remarks>
		public override System.Drawing.Drawing2D.GraphicsPath GraphicsPath
		{
			get
			{
				GraphicsPath grfxPath = new GraphicsPath();

				if (this.linePoints == null)
				{
					throw new EInvalidOperation();
				}

				int numPts = this.linePoints.PointCount;

				if (numPts < 2)
				{
					throw new EInvalidOperation();
				}

				PointF[] pts = new PointF[3];

				PointF ptFrom;
				PointF ptTo;

				if (this.EndPointType == LineEndPoint.First)
				{
					ptFrom = this.linePoints.GetPoint(1);
					ptTo = this.linePoints.GetPoint(0);
				}
				else
				{
					ptFrom = this.linePoints.GetPoint(numPts-2);
					ptTo = this.linePoints.GetPoint(numPts-1);
				}

				float circleX = ptTo.X - this.radius;
				float circleY = ptTo.Y - this.radius;
				float circleWidth = this.radius * 2.0f;
				float circleHeight = this.radius * 2.0f;
				RectangleF circleBounds = new RectangleF(circleX, circleY, circleWidth, circleHeight);
				grfxPath.AddEllipse(circleBounds);

				return grfxPath;
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
			info.AddValue("endPointType", this.EndPointType);
			info.AddValue("line", this.Line);
			info.AddValue("linePoints", this.linePoints);
			info.AddValue("propertyValues", this.propertyValues);
			info.AddValue("radius", this.radius);
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
		/// Radius of the circle
		/// </summary>
		private float radius = 5;
	}
}
