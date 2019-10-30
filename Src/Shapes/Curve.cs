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
	/// Implements an open curve shape.
	/// </summary>
	[Serializable]
	public class Curve : Shape
	{
		/// <summary>
		/// 
		/// </summary>
		public Curve()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		public Curve(PointF[] pts)
		{
			this.grfxPath = this.CreateGraphicsPath(pts);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public Curve(Curve src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for curves.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Curve(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new Curve(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		protected override System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath(PointF[] pts)
		{
			GraphicsPath grfxPath = null;

			if (pts.Length > 0)
			{
				grfxPath = new GraphicsPath();
				grfxPath.AddCurve(pts);
			}

			return grfxPath;
		}

		/// <summary>
		/// Sets the default property values for the curve.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// curve to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("AllowVertexEdit", true);
		}
	}
}
