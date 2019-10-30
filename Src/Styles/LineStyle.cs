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
	/// Encapsulates the line properties of an object.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This style is used to create pens for drawing lines. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LineStyle.CreatePen"/>
	/// create a pen from the properties contained in the line style object.
	/// </para>
	/// </remarks>
	public class LineStyle : Syncfusion.Windows.Forms.Diagram.Style
	{
		/// <summary>
		/// Constructs a line style
		/// </summary>
		/// <param name="propContainer"></param>
		public LineStyle(IPropertyContainer propContainer) :
			base(propContainer)
		{
		}

		/// <summary>
		/// Color used to draw lines.
		/// </summary>
		public System.Drawing.Color LineColor
		{
			get
			{
				object value = this.Properties.GetPropertyValue("LineColor");
				if (value == null)
				{
					return Color.Black;
				}
				return (Color) value;
			}
			set
			{
				this.Properties.SetPropertyValue("LineColor", value);
			}
		}

		/// <summary>
		/// Width of the pen in logical units.
		/// </summary>
		public float LineWidth
		{
			get
			{
				object value = this.Properties.GetPropertyValue("LineWidth");
				if (value == null)
				{
					return 0.0f;
				}
				return (float) value;
			}
			set
			{
				this.Properties.SetPropertyValue("LineWidth", value);
			}
		}

		/// <summary>
		/// Type of end cap used to draw lines.
		/// </summary>
		public LineCap EndCap
		{
			get
			{
				object value = this.Properties.GetPropertyValue("LineEndCap");
				if (value == null)
				{
					return LineCap.Flat;
				}
				return (LineCap) value;
			}
			set
			{
				this.Properties.SetPropertyValue("LineEndCap", value);
			}
		}

		/// <summary>
		/// Determines how lines are joined at corners.
		/// </summary>
		public LineJoin LineJoin
		{
			get
			{
				object value = this.Properties.GetPropertyValue("LineJoin");
				if (value == null)
				{
					return LineJoin.Bevel;
				}
				return (LineJoin) value;
			}
			set
			{
				this.Properties.SetPropertyValue("LineJoin", value);
			}
		}

		/// <summary>
		/// Miter limit value.
		/// </summary>
		public float MiterLimit
		{
			get
			{
				object value = this.Properties.GetPropertyValue("LineMiterLimit");
				if (value == null)
				{
					return 1.0f;
				}
				return (float) value;
			}
			set
			{
				this.Properties.SetPropertyValue("LineMiterLimit", value);
			}
		}

		/// <summary>
		/// Style to used for dashed lines.
		/// </summary>
		public DashStyle DashStyle
		{
			get
			{
				object value = this.Properties.GetPropertyValue("LineDashStyle");
				if (value == null)
				{
					return DashStyle.Solid;
				}
				return (DashStyle) value;
			}
			set
			{
				this.Properties.SetPropertyValue("LineDashStyle", value);
			}
		}

		/// <summary>
		/// Type of cap to use for dashed lines.
		/// </summary>
		public DashCap DashCap
		{
			get
			{
				object value = this.Properties.GetPropertyValue("LineDashCap");
				if (value == null)
				{
					return DashCap.Flat;
				}
				return (DashCap) value;
			}
			set
			{
				this.Properties.SetPropertyValue("LineDashCap", value);
			}
		}

		/// <summary>
		/// Offset of dashes in dashed lines in logical units.
		/// </summary>
		public float DashOffset
		{
			get
			{
				object value = this.Properties.GetPropertyValue("LineDashOffset");
				if (value != null)
				{
					return 3.0f;
				}
				return (float) value;
			}
			set
			{
				this.Properties.SetPropertyValue("LineDashOffset", value);
			}
		}

		/// <summary>
		/// Creates a Pen object using the properties contained by the line style.
		/// </summary>
		/// <returns>System.Drawing.Pen object</returns>
		public Pen CreatePen()
		{
			Pen pen = new Pen(this.LineColor, this.LineWidth);
			pen.EndCap = this.EndCap;
			pen.LineJoin = this.LineJoin;
			pen.MiterLimit = this.MiterLimit;
			pen.DashStyle = this.DashStyle;
			pen.DashCap = this.DashCap;
			pen.DashOffset = this.DashOffset;
			return pen;
		}
	}
}