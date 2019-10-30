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
	/// Implementation of rounded rectangle shape.
	/// </summary>
	[Serializable]
	public class RoundRect : FilledShape
	{
		/// <summary>
		/// 
		/// </summary>
		public RoundRect()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public RoundRect(float x, float y, float width, float height)
		{
			this.rcBounds = new RectangleF(x, y, width, height);
			this.grfxPath = this.CreateGraphicsPath(this.rcBounds);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcBounds"></param>
		public RoundRect(RectangleF rcBounds)
		{
			this.rcBounds = rcBounds;
			this.grfxPath = this.CreateGraphicsPath(this.rcBounds);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public RoundRect(RoundRect src) : base(src)
		{
			this.rcBounds = new RectangleF(src.rcBounds.Location, src.rcBounds.Size);
			this.curvePct = src.curvePct;
		}

		/// <summary>
		/// Serialization constructor for rounded rectangles.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected RoundRect(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new RoundRect(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		protected override System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath(PointF[] pts)
		{
			GraphicsPath grfxPath = null;
			if (pts.Length >= 2)
			{
				RectangleF rcBounds = Geometry.CreateRect(pts[0], pts[1]);
				grfxPath = this.CreateGraphicsPath(rcBounds);
			}
			return grfxPath;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcBounds"></param>
		/// <returns></returns>
		protected System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath(RectangleF rcBounds)
		{
			GraphicsPath grfxPath = new GraphicsPath();

			float x = rcBounds.Left;
			float y = rcBounds.Top;
			float width = rcBounds.Width;
			float height = rcBounds.Height;
			float offsetCurve = ((width + height) / 2.0f) * (this.curvePct / 100.0f);
			float offsetHalf = offsetCurve / 2.0f;

			System.Drawing.PointF ptUpperLeftStart = new System.Drawing.PointF(rcBounds.Left, rcBounds.Top + offsetCurve);
			System.Drawing.PointF ptUpperLeftCtl1 = new System.Drawing.PointF(rcBounds.Left, rcBounds.Top + offsetHalf);
			System.Drawing.PointF ptUpperLeftCtl2 = new System.Drawing.PointF(rcBounds.Left + offsetHalf, rcBounds.Top);
			System.Drawing.PointF ptUpperLeftEnd = new System.Drawing.PointF(rcBounds.Left + offsetCurve, rcBounds.Top);
			grfxPath.AddBezier(ptUpperLeftStart, ptUpperLeftCtl1, ptUpperLeftCtl2, ptUpperLeftEnd);

			System.Drawing.PointF ptUpperRightStart = new System.Drawing.PointF(rcBounds.Right - offsetCurve, rcBounds.Top);
			System.Drawing.PointF ptUpperRightCtl1 = new System.Drawing.PointF(rcBounds.Right - offsetHalf, rcBounds.Top);
			System.Drawing.PointF ptUpperRightCtl2 = new System.Drawing.PointF(rcBounds.Right, rcBounds.Top + offsetHalf);
			System.Drawing.PointF ptUpperRightEnd = new System.Drawing.PointF(rcBounds.Right, rcBounds.Top + offsetCurve);
			grfxPath.AddBezier(ptUpperRightStart, ptUpperRightCtl1, ptUpperRightCtl2, ptUpperRightEnd);

			System.Drawing.PointF ptLowerRightStart = new System.Drawing.PointF(rcBounds.Right, rcBounds.Bottom - offsetCurve);
			System.Drawing.PointF ptLowerRightCtl1 = new System.Drawing.PointF(rcBounds.Right, rcBounds.Bottom - offsetHalf);
			System.Drawing.PointF ptLowerRightCtl2 = new System.Drawing.PointF(rcBounds.Right - offsetHalf, rcBounds.Bottom);
			System.Drawing.PointF ptLowerRightEnd = new System.Drawing.PointF(rcBounds.Right - offsetCurve, rcBounds.Bottom);
			grfxPath.AddBezier(ptLowerRightStart, ptLowerRightCtl1, ptLowerRightCtl2, ptLowerRightEnd);

			System.Drawing.PointF ptLowerLeftStart = new System.Drawing.PointF(rcBounds.Left + offsetCurve, rcBounds.Bottom);
			System.Drawing.PointF ptLowerLeftCtl1 = new System.Drawing.PointF(rcBounds.Left + offsetHalf, rcBounds.Bottom);
			System.Drawing.PointF ptLowerLeftCtl2 = new System.Drawing.PointF(rcBounds.Left, rcBounds.Bottom - offsetHalf);
			System.Drawing.PointF ptLowerLeftEnd = new System.Drawing.PointF(rcBounds.Left, rcBounds.Bottom - offsetCurve);
			grfxPath.AddBezier(ptLowerLeftStart, ptLowerLeftCtl1, ptLowerLeftCtl2, ptLowerLeftEnd);
			grfxPath.AddLine(ptLowerLeftEnd, ptUpperLeftStart);

			return grfxPath;
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		Category("Appearance"),
		Description("Percentage of the width and height of the rectangle that included in the curves")
		]
		public float CurvePercent
		{
			get
			{
				return this.curvePct;
			}
			set
			{
				if (value != this.curvePct)
				{
					this.curvePct = value;
					this.grfxPath = this.CreateGraphicsPath(this.rcBounds);
				}
			}
		}

		/// <summary>
		/// Sets the default property values for the rounded rectangle.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// shape to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("AllowVertexEdit", false);
		}

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.RectangleF rcBounds;

		/// <summary>
		/// 
		/// </summary>
		protected float curvePct = 15.0f;
	}
}
