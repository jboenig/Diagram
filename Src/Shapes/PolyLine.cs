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
using System.Drawing.Design;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Implementation of polyline shapes.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A polyline is a collection of points that form a series of connected
	/// line segments.
	/// </para>
	/// </remarks>
	[Serializable]
	public class PolyLine : Shape, IEndPoints, IHitTestRegion, ISerializable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public PolyLine()
		{
		}

		/// <summary>
		/// Constructs a polyline given an array of points.
		/// </summary>
		/// <param name="pts">Points to add to polyline</param>
		public PolyLine(PointF[] pts)
		{
			this.grfxPath = this.CreateGraphicsPath(pts);
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Object to copy</param>
		public PolyLine(PolyLine src) : base(src)
		{
			((IEndPoints)this).FirstEndPoint = src.firstEndPoint;
			((IEndPoints)this).LastEndPoint = src.lastEndPoint;
		}

		/// <summary>
		/// Serialization constructor for lines.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected PolyLine(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.firstEndPoint = (LineDecorator) info.GetValue("firstEndPoint", typeof(LineDecorator));
			this.lastEndPoint = (LineDecorator) info.GetValue("lastEndPoint", typeof(LineDecorator));
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new PolyLine(this);
		}

		/// <summary>
		/// Line decorator object for the first end point.
		/// </summary>
		[
		Browsable(true),
		Editor(typeof(LineEndpointEditor), typeof(UITypeEditor)),
		TypeConverter(typeof(LineDecoratorConverter))
		]
		public LineDecorator FirstEndPoint
		{
			get
			{
				return this.firstEndPoint;
			}
			set
			{
				if (value != this.firstEndPoint)
				{
					object oldVal = this.firstEndPoint;

					if (this.firstEndPoint != null)
					{
						this.firstEndPoint.Line = null;
					}

					this.firstEndPoint = value;

					if (this.firstEndPoint != null)
					{
						this.firstEndPoint.Line = this;
						this.firstEndPoint.EndPointType = LineEndPoint.First;
					}

					this.OnPropertyChanged(new PropertyEventArgs(this, "FirstEndPoint", oldVal, this.firstEndPoint));
				}
			}
		}

		/// <summary>
		/// Line decorator object for the last end point.
		/// </summary>
		[
		Browsable(true),
		Editor(typeof(LineEndpointEditor), typeof(UITypeEditor)),
		TypeConverter(typeof(LineDecoratorConverter))
		]
		public LineDecorator LastEndPoint
		{
			get
			{
				return this.lastEndPoint;
			}
			set
			{
				if (value != this.lastEndPoint)
				{
					object oldVal = this.lastEndPoint;

					if (this.lastEndPoint != null)
					{
						this.lastEndPoint.Line = null;
					}

					this.lastEndPoint = value;

					if (this.lastEndPoint != null)
					{
						this.lastEndPoint.Line = this;
						this.lastEndPoint.EndPointType = LineEndPoint.Last;
					}

					this.OnPropertyChanged(new PropertyEventArgs(this, "LastEndPoint", oldVal, this.lastEndPoint));
				}
			}
		}

		/// <summary>
		/// Determines if the polyline is orthogonal or not.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property tests the points in the polyline each time it is
		/// accessed and checks to see if they form an orthogonal line.
		/// </para>
		/// </remarks>
		public bool IsOrthogonal
		{
			get
			{
				PointF[] pts = this.GetPoints();
				return Geometry.IsOrthogonalLine(pts);
			}
		}

		/// <summary>
		/// Re-arranges the points in the polyline to form an orthogonal line.
		/// </summary>
		public void MakeOrthogonal()
		{
			PointF[] pts = this.GetPoints();

			if (pts != null && pts.Length > 1)
			{
				PointF endPt1 = pts[0];
				PointF endPt2 = pts[pts.Length-1];

				CompassHeading endPt1Heading;
				CompassHeading endPt2Heading;
				Geometry.CalcEndpointDirections(endPt1, endPt2, out endPt1Heading, out endPt2Heading);

				float padLeft = 1.0f;
				float padRight = 1.0f;
				float padTop = 1.0f;
				float padBottom = 1.0f;

				PointF[] orthogonalPts = Geometry.CalcOrthogonalPoints(endPt1, endPt1Heading, endPt2, endPt2Heading, padLeft, padRight, padTop, padBottom);

				if (orthogonalPts != null)
				{
					this.SetPoints(orthogonalPts);
				}
			}
		}

		/// <summary>
		/// Re-arranges the points in the polyline to form an orthogonal line.
		/// </summary>
		/// <param name="padLeft"></param>
		/// <param name="padRight"></param>
		/// <param name="padTop"></param>
		/// <param name="padBottom"></param>
		public void MakeOrthogonal(float padLeft, float padRight, float padTop, float padBottom)
		{
			PointF[] pts = this.GetPoints();

			if (pts != null && pts.Length > 1)
			{
				PointF endPt1 = pts[0];
				PointF endPt2 = pts[pts.Length-1];

				CompassHeading endPt1Heading;
				CompassHeading endPt2Heading;
				Geometry.CalcEndpointDirections(endPt1, endPt2, out endPt1Heading, out endPt2Heading);

				PointF[] orthogonalPts = Geometry.CalcOrthogonalPoints(endPt1, endPt1Heading, endPt2, endPt2Heading, padLeft, padRight, padTop, padBottom);

				if (orthogonalPts != null)
				{
					this.SetPoints(orthogonalPts);
				}
			}
		}


		/// <summary>
		/// Re-arranges the points in the polyline to form an orthogonal line.
		/// </summary>
		/// <param name="endPt1Heading"></param>
		/// <param name="endPt2Heading"></param>
		/// <param name="padLeft"></param>
		/// <param name="padRight"></param>
		/// <param name="padTop"></param>
		/// <param name="padBottom"></param>
		public void MakeOrthogonal(CompassHeading endPt1Heading, CompassHeading endPt2Heading, float padLeft, float padRight, float padTop, float padBottom)
		{
			PointF[] pts = this.GetPoints();

			if (pts != null && pts.Length > 1)
			{
				PointF endPt1 = pts[0];
				PointF endPt2 = pts[pts.Length-1];

				PointF[] orthogonalPts = Geometry.CalcOrthogonalPoints(endPt1, endPt1Heading, endPt2, endPt2Heading, padLeft, padRight, padTop, padBottom);

				if (orthogonalPts != null)
				{
					this.SetPoints(orthogonalPts);
				}
			}
		}

		/// <summary>
		/// Creates the graphics path for the shape given an array of points.
		/// </summary>
		/// <param name="pts">Array of points used to construct the graphics path</param>
		/// <returns>Graphics path containing a line</returns>
		protected override System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath(PointF[] pts)
		{
			PointF[] ptTest;
			GraphicsPath grfxPath = new GraphicsPath();
			if (pts.Length > 0)
			{
				grfxPath.AddLines(pts);
				ptTest = grfxPath.PathPoints;
			}
			return grfxPath;
		}

		/// <summary>
		/// Renders the line to a graphics context object.
		/// </summary>
		/// <param name="grfx">Graphics context object to render onto</param>
		public override void Draw(System.Drawing.Graphics grfx)
		{
			if (this.Visible)
			{
				base.Draw(grfx);
				
				if (this.firstEndPoint != null)
				{
					this.firstEndPoint.Draw(grfx);
				}

				if (this.lastEndPoint != null)
				{
					this.lastEndPoint.Draw(grfx);
				}
			}
		}

		/// <summary>
		/// Padding around the line used for hit testing.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Specified in logical units.
		/// </para>
		/// </remarks>
		[
		Browsable(true)
		]
		public float LineHitTestPadding
		{
			get
			{
				return (float) this.GetPropertyValue("LineHitTestPadding");
			}
			set
			{
				this.SetPropertyValue("LineHitTestPadding", value);
			}
		}

		/// <summary>
		/// Sets the default property values for the polyline.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// polyline to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("LineColor", Color.Black);
			this.propertyValues.Add("LineWidth", 2.0f);
			this.propertyValues.Add("AllowSelect", true);
			this.propertyValues.Add("AllowVertexEdit", true);
			this.propertyValues.Add("AllowMove", true);
			this.propertyValues.Add("AllowRotate", true);
			this.propertyValues.Add("AllowResize", true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptTest"></param>
		/// <param name="fSlop"></param>
		/// <returns></returns>
		bool IHitTestRegion.ContainsPoint(PointF ptTest, float fSlop)
		{
			bool hit = false;
			System.Drawing.Region rgn = this.CreateRegion(fSlop + this.LineHitTestPadding);
			if (rgn != null)
			{
				hit = rgn.IsVisible(ptTest);
			}
			return hit;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcTest"></param>
		/// <returns></returns>
		bool IHitTestRegion.IntersectsRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return bounds.IntersectsWith(rcTest);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcTest"></param>
		/// <returns></returns>
		bool IHitTestRegion.ContainedByRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return rcTest.Contains(bounds);
		}

		/// <summary>
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("name", this.Name);
			info.AddValue("parent", this.Parent);
			info.AddValue("pathPoints", this.grfxPath.PathPoints);
			info.AddValue("pathTypes", this.grfxPath.PathTypes);
			info.AddValue("m11", this.matrix.Elements[0]);
			info.AddValue("m12", this.matrix.Elements[1]);
			info.AddValue("m21", this.matrix.Elements[2]);
			info.AddValue("m22", this.matrix.Elements[3]);
			info.AddValue("dx", this.matrix.Elements[4]);
			info.AddValue("dy", this.matrix.Elements[5]);
			info.AddValue("propertyValues", this.propertyValues);
			info.AddValue("firstEndPoint", this.firstEndPoint);
			info.AddValue("lastEndPoint", this.lastEndPoint);
		}

		/// <summary>
		/// LineDecorator attached to first point in line
		/// </summary>
		private LineDecorator firstEndPoint = null;

		/// <summary>
		/// LineDecorator attached to last point in line
		/// </summary>
		private LineDecorator lastEndPoint = null;
	}
}