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
	/// Implements an open arrow line decorator.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a line decorator that draws itself as an open
	/// arrow.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecorator"/>
	/// </remarks>
	[Serializable]
	public class OpenArrowDecorator : LineDecorator, ISerializable, IDeserializationCallback
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public OpenArrowDecorator() : base()
		{
		}

		/// <summary>
		/// Constructs an open arrow line decorator and attaches it to the given node.
		/// </summary>
		/// <param name="line">Node to attach to</param>
		public OpenArrowDecorator(INode line) : base(line)
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy</param>
		public OpenArrowDecorator(OpenArrowDecorator src) : base(src)
		{
			this.arrowWidth = src.arrowWidth;
			this.arrowHeight = src.arrowHeight;
		}

		/// <summary>
		/// Serialization constructor for arrow decorators.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected OpenArrowDecorator(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.arrowWidth = info.GetSingle("arrowWidth");
			this.arrowHeight = info.GetSingle("arrowHeight");
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new OpenArrowDecorator(this);
		}

		/// <summary>
		/// Horizontal size of the arrow decorator.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default size is 9 logical units.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Appearance"),
		Description("Horizontal size of the arrow decorator")
		]
		public float ArrowWidth
		{
			get
			{
				return this.arrowWidth;
			}
			set
			{
				this.arrowWidth = value;
			}
		}

		/// <summary>
		/// Vertical size of the arrow decorator.
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
		Description("Vertical size of the arrow decorator")
		]
		public float ArrowHeight
		{
			get
			{
				return this.arrowHeight;
			}
			set
			{
				this.arrowHeight = value;
			}
		}

		/// <summary>
		/// Encapsulates the points and instructions needed to render the open arrow
		/// line decorator.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The get method of this property creates a new GraphicsPath object that
		/// consists of a polyline with 3 points. The second point in the polyline
		/// is always identical to (overlaps) the endpoint that the decorator is
		/// attached to.
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

				double lineAngleRadians = 0.0f;
				int quadrant = 1;
				double vx = ptTo.X - ptFrom.X;
				double vy = ptFrom.Y - ptTo.Y;
				double vxIntersect = ptTo.X;
				double vyIntersect = ptTo.Y;
				double vxArrow;
				double vyArrow;
				PointF ptLineIntersect = new PointF(0,0);

				lineAngleRadians = Math.Atan(vy/vx);

				if (vx < 0 && vy > 0)
				{
					quadrant = 2;
				}
				else if (vx < 0 && vy < 0)
				{
					quadrant = 3;
				}
				else if (vx > 0 && vy < 0)
				{
					quadrant = 4;
				}

				switch (quadrant)
				{
					case 1:
						vxIntersect = -((float) (System.Math.Cos(lineAngleRadians) * this.arrowWidth));
						vyIntersect = (float) (System.Math.Sin(lineAngleRadians) * this.arrowWidth);
						break;

					case 2:
						vxIntersect = (float) (System.Math.Cos(lineAngleRadians) * this.arrowWidth);
						vyIntersect = -((float) (System.Math.Sin(lineAngleRadians) * this.arrowWidth));
						break;

					case 3:
						vxIntersect = (float) (System.Math.Cos(lineAngleRadians) * this.arrowWidth);
						vyIntersect = -((float) (System.Math.Sin(lineAngleRadians) * this.arrowWidth));
						break;

					case 4:
						vxIntersect = -((float) (System.Math.Cos(lineAngleRadians) * this.arrowWidth));
						vyIntersect = (float) (System.Math.Sin(lineAngleRadians) * this.arrowWidth);
						break;
				}

				ptLineIntersect.X = ptTo.X + (float) vxIntersect;
				ptLineIntersect.Y = ptTo.Y + (float) vyIntersect;
				vxArrow = System.Math.Sin(lineAngleRadians) * this.arrowHeight;
				vyArrow = System.Math.Cos(lineAngleRadians) * this.arrowHeight;
				pts[0].X = ptLineIntersect.X + (float) vxArrow;
				pts[0].Y = ptLineIntersect.Y + (float) vyArrow;
				pts[1] = ptTo;
				pts[2].X = ptLineIntersect.X - (float) vxArrow;
				pts[2].Y = ptLineIntersect.Y - (float) vyArrow;

				grfxPath.AddLines(pts);

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
			info.AddValue("arrowWidth", this.arrowWidth);
			info.AddValue("arrowHeight", this.arrowHeight);
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
		/// Horizontal size of the arrow
		/// </summary>
		private float arrowWidth = 9;

		/// <summary>
		/// Vertical size of the arrow
		/// </summary>
		private float arrowHeight = 5;
	}
}
