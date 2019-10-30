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
using System.Drawing.Design;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Encapsulates the tracking properties of an object.
	/// </summary>
	public class TrackingStyle : Syncfusion.Windows.Forms.Diagram.Style
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="propContainer"></param>
		public TrackingStyle(IPropertyContainer propContainer) :
			base(propContainer)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public System.Drawing.Color LineColor
		{
			get
			{
				object value = this.Properties.GetPropertyValue("TrackingLineColor");
				if (value == null)
				{
					return Color.Black;
				}
				return (Color) value;
			}
			set
			{
				this.Properties.SetPropertyValue("TrackingLineColor", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float LineWidth
		{
			get
			{
				object value = this.Properties.GetPropertyValue("TrackingLineWidth");
				if (value == null)
				{
					return 0;
				}
				return (float) value;
			}
			set
			{
				this.Properties.SetPropertyValue("TrackingLineWidth", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public DashStyle DashStyle
		{
			get
			{
				object value = this.Properties.GetPropertyValue("TrackingLineDashStyle");
				if (value == null)
				{
					return DashStyle.Dash;
				}
				return (DashStyle) value;
			}
			set
			{
				this.Properties.SetPropertyValue("TrackingLineDashStyle", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public DashCap DashCap
		{
			get
			{
				object value = this.Properties.GetPropertyValue("TrackingLineDashCap");
				if (value == null)
				{
					return DashCap.Flat;
				}
				return (DashCap) value;
			}
			set
			{
				this.Properties.SetPropertyValue("TrackingLineDashCap", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float DashOffset
		{
			get
			{
				object value = this.Properties.GetPropertyValue("TrackingLineDashOffset");
				if (value == null)
				{
					return 1.0f;
				}
				return (float) value;
			}
			set
			{
				this.Properties.SetPropertyValue("TrackingLineDashOffset", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Pen CreatePen()
		{
			Pen pen = new Pen(this.LineColor, this.LineWidth);
			pen.EndCap = LineCap.Flat;
			pen.LineJoin = LineJoin.Bevel;
			pen.MiterLimit = 10.0f;
			pen.DashStyle = this.DashStyle;
			pen.DashCap = this.DashCap;
			pen.DashOffset = this.DashOffset;
			return pen;
		}
	}
}