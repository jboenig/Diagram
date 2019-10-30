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
	/// Implements an ellipse shape.
	/// </summary>
	[Serializable]
	public class Ellipse : FilledShape
	{
		/// <summary>
		/// 
		/// </summary>
		public Ellipse()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Ellipse(float x, float y, float width, float height)
		{
			this.grfxPath = new GraphicsPath();
			this.grfxPath.AddEllipse(x, y, width, height);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcBounds"></param>
		public Ellipse(RectangleF rcBounds)
		{
			this.grfxPath = new GraphicsPath();
			this.grfxPath.AddEllipse(rcBounds);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public Ellipse(Ellipse src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for ellipses.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Ellipse(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new Ellipse(this);
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
				grfxPath = new GraphicsPath();
				RectangleF rcBounds = Geometry.CreateRect(pts[0], pts[1]);
				grfxPath.AddEllipse(rcBounds);
			}
			return grfxPath;
		}
	}
}