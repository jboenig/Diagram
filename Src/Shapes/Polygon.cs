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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Implementation of polygon shapes.
	/// </summary>
	[Serializable]
	public class Polygon : FilledShape
	{
		/// <summary>
		/// 
		/// </summary>
		public Polygon()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		public Polygon(PointF[] pts)
		{
			this.grfxPath = CreateGraphicsPath(pts);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public Polygon(Polygon src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for polygons.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Polygon(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new Polygon(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		protected override System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath(PointF[] pts)
		{
			GraphicsPath grfxPath = new GraphicsPath();
			if (pts.Length > 0)
			{
				grfxPath.AddPolygon(pts);
			}
			return grfxPath;
		}

		/// <summary>
		/// Sets the default property values for the polygon.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// polygon to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			this.SetPropertyValue("AllowVertexEdit", true);
		}

		/// <summary>
		/// 
		/// </summary>
		public override int MinPoints
		{
			get
			{
				return 3;
			}
		}
	}
}
