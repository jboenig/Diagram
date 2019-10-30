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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Implements an arc shape.
	/// </summary>
	[Serializable]
	public class Arc : Shape
	{
		/// <summary>
		/// 
		/// </summary>
		public Arc()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="startAngle"></param>
		/// <param name="sweepAngle"></param>
		public Arc(float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			this.grfxPath.AddArc(x, y, width, height, startAngle, sweepAngle);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcBounds"></param>
		/// <param name="startAngle"></param>
		/// <param name="sweepAngle"></param>
		public Arc(RectangleF rcBounds, float startAngle, float sweepAngle)
		{
			this.grfxPath.AddArc(rcBounds, startAngle, sweepAngle);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public Arc(Arc src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for arcs.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Arc(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new Arc(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptIdx"></param>
		/// <param name="val"></param>
		public override void SetPoint(int ptIdx, PointF val)
		{
			if (this.grfxPath == null)
			{
				throw new EInvalidOperation();
			}

			int numPts = this.grfxPath.PointCount;

			if (ptIdx != 0 && ptIdx != (numPts-1))
			{
				throw new EInvalidParameter();
			}

			PointF[] pathPts = this.grfxPath.PathPoints;

			if (pathPts[ptIdx] != val)
			{
				pathPts[ptIdx] = val;
				this.grfxPath = CreateGraphicsPath(pathPts);
				this.OnMoveVertex(new VertexEventArgs(this, ptIdx));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		protected override System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath(PointF[] pts)
		{
			GraphicsPath grfxPath = new GraphicsPath();

			if (pts == null || pts.Length < 2)
			{
				throw new EInvalidParameter();
			}

			RectangleF rcBounds;
			float startAngle;
			float sweepAngle;
			Geometry.ArcFromPoints(pts[0], pts[1], out rcBounds, out startAngle, out sweepAngle);
			grfxPath.AddArc(rcBounds, startAngle, sweepAngle);

			return grfxPath;
		}

		/// <summary>
		/// Sets the default property values for the arc.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// arc to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("AllowVertexEdit", true);
		}
	}
}
