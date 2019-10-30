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
using System.Drawing.Design;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Encapsulates the border properties of an object.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Contains properties needed to create a pen for drawing the border
	/// of an object. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.BorderStyle.CreatePen"/>
	/// method returns a pen to draw the border.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Style"/>
	/// </remarks>
	public class BorderStyle : Syncfusion.Windows.Forms.Diagram.Style
	{
		/// <summary>
		/// Construct a BorderStyle object given a property container.
		/// </summary>
		/// <param name="propContainer">Container that owns this style</param>
		public BorderStyle(IPropertyContainer propContainer) : base(propContainer)
		{
		}

		/// <summary>
		/// Flag indicating if the border is visible or not.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public bool ShowBorder
		{
			get
			{
				return (bool) this.Properties.GetPropertyValue("ShowBorder");
			}
			set
			{
				this.Properties.SetPropertyValue("ShowBorder", value);
			}
		}

		/// <summary>
		/// Color of the border.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public System.Drawing.Color BorderColor
		{
			get
			{
				return (Color) this.Properties.GetPropertyValue("BorderColor");
			}
			set
			{
				this.Properties.SetPropertyValue("BorderColor", value);
			}
		}

		/// <summary>
		/// Width of the border.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public int BorderWidth
		{
			get
			{
				return (int) this.Properties.GetPropertyValue("BorderWidth");
			}
			set
			{
				this.Properties.SetPropertyValue("BorderWidth", value);
			}
		}

		/// <summary>
		/// Creates a pen to draw the border.
		/// </summary>
		/// <returns>System.Drawing.Pen object matching the border style</returns>
		public Pen CreatePen()
		{
			if (this.ShowBorder)
			{
				return new Pen(this.BorderColor, this.BorderWidth);
			}
			return null;
		}
	}
}
