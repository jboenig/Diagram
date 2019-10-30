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
	/// Implements a diamond decorator.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a line decorator that draws itself as a diamond.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecorator"/>
	/// </remarks>
	[Serializable]
	public class DiamondDecorator : FilledLineDecorator, ISerializable, IDeserializationCallback
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DiamondDecorator() : base()
		{
		}

		/// <summary>
		/// Constructs a diamond decorator and attaches it to the given node.
		/// </summary>
		/// <param name="line">Node to attach to</param>
		public DiamondDecorator(INode line) : base(line)
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy</param>
		public DiamondDecorator(DiamondDecorator src) : base(src)
		{
			this.diamondWidth = src.diamondWidth;
			this.diamondHeight = src.diamondHeight;
		}

		/// <summary>
		/// Serialization constructor for arrow decorators.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected DiamondDecorator(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.diamondWidth = info.GetSingle("diamondWidth");
			this.diamondHeight = info.GetSingle("diamondHeight");
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new DiamondDecorator(this);
		}

		/// <summary>
		/// Horizontal size of the diamond decorator.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default size is 16 logical units.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Appearance"),
		Description("Horizontal size of the diamond decorator")
		]
		public float DiamondWidth
		{
			get
			{
				return this.diamondWidth;
			}
			set
			{
				this.diamondWidth = value;
			}
		}

		/// <summary>
		/// Vertical size of the diamond decorator.
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
		Description("Vertical size of the diamond decorator")
		]
		public float DiamondHeight
		{
			get
			{
				return this.diamondHeight;
			}
			set
			{
				this.diamondHeight = value;
			}
		}

		/// <summary>
		/// Sets the default property values for the diamond decorator.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Sets the default fill properties of the diamond decorator to solid, black.
		/// </para>
		/// <para>
		/// This method can be called at any time to reset the properties of the
		/// diamond decorator to their default values.
		/// </para>
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("FillType", Syncfusion.Windows.Forms.Diagram.FillStyle.FillType.Solid);
			this.propertyValues.Add("FillColor", Color.Black);
		}

		/// <summary>
		/// Encapsulates the points and instructions needed to render the diamond
		/// line decorator.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The get method of this property creates a new GraphicsPath object that
		/// consists of a polygon with 4 points.
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

				PointF[] pts = new PointF[4];

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

				pts[0] = ptTo;

				double lineAngleRadians = 0.0f;
				int quadrant = 1;
				double vx = ptTo.X - ptFrom.X;
				double vy = ptFrom.Y - ptTo.Y;
				double vxIntersect = ptTo.X;
				double vyIntersect = ptTo.Y;
				double vxArrow;
				double vyArrow;
				float arrowWidth = (this.diamondWidth / 2.0f);
				float arrowHeight = this.diamondHeight;
				PointF ptLineIntersect = new PointF(0,0);
				double vxIntersect2 = ptTo.X;
				double vyIntersect2 = ptTo.Y;
				PointF ptLineIntersect2 = new PointF(0,0);

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
						vxIntersect = -((float) (System.Math.Cos(lineAngleRadians) * arrowWidth));
						vyIntersect = (float) (System.Math.Sin(lineAngleRadians) * arrowWidth);
						vxIntersect2 = -((float) (System.Math.Cos(lineAngleRadians) * this.diamondWidth));
						vyIntersect2 = (float) (System.Math.Sin(lineAngleRadians) * this.diamondWidth);
						break;

					case 2:
						vxIntersect = (float) (System.Math.Cos(lineAngleRadians) * arrowWidth);
						vyIntersect = -((float) (System.Math.Sin(lineAngleRadians) * arrowWidth));
						vxIntersect2 = (float) (System.Math.Cos(lineAngleRadians) * this.diamondWidth);
						vyIntersect2 = -((float) (System.Math.Sin(lineAngleRadians) * this.diamondWidth));
						break;

					case 3:
						vxIntersect = (float) (System.Math.Cos(lineAngleRadians) * arrowWidth);
						vyIntersect = -((float) (System.Math.Sin(lineAngleRadians) * arrowWidth));
						vxIntersect2 = (float) (System.Math.Cos(lineAngleRadians) * this.diamondWidth);
						vyIntersect2 = -((float) (System.Math.Sin(lineAngleRadians) * this.diamondWidth));
						break;

					case 4:
						vxIntersect = -((float) (System.Math.Cos(lineAngleRadians) * arrowWidth));
						vyIntersect = (float) (System.Math.Sin(lineAngleRadians) * arrowWidth);
						vxIntersect2 = -((float) (System.Math.Cos(lineAngleRadians) * this.diamondWidth));
						vyIntersect2 = (float) (System.Math.Sin(lineAngleRadians) * this.diamondWidth);
						break;
				}

				ptLineIntersect.X = ptTo.X + (float) vxIntersect;
				ptLineIntersect.Y = ptTo.Y + (float) vyIntersect;
				ptLineIntersect2.X = ptTo.X + (float) vxIntersect2;
				ptLineIntersect2.Y = ptTo.Y + (float) vyIntersect2;

				vxArrow = System.Math.Sin(lineAngleRadians) * arrowHeight;
				vyArrow = System.Math.Cos(lineAngleRadians) * arrowHeight;

				pts[1].X = ptLineIntersect.X + (float) vxArrow;
				pts[1].Y = ptLineIntersect.Y + (float) vyArrow;
				pts[2].X = ptLineIntersect2.X;
				pts[2].Y = ptLineIntersect2.Y;
				pts[3].X = ptLineIntersect.X - (float) vxArrow;
				pts[3].Y = ptLineIntersect.Y - (float) vyArrow;

				grfxPath.AddPolygon(pts);

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
			info.AddValue("diamondWidth", this.diamondWidth);
			info.AddValue("diamondHeight", this.diamondHeight);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			base.CreateStyles();
		}

		/// <summary>
		/// Horizontal size of the diamond
		/// </summary>
		private float diamondWidth = 16;

		/// <summary>
		/// Vertical size of the diamond
		/// </summary>
		private float diamondHeight = 5;
	}
}
