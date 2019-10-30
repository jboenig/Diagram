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
	/// Encapsulates the font properties of an object.
	/// </summary>
	public class FontStyle : Syncfusion.Windows.Forms.Diagram.Style
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="propContainer"></param>
		public FontStyle(IPropertyContainer propContainer) :
			base(propContainer)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true)
		]
		public string Family
		{
			get
			{
				object value = this.Properties.GetPropertyValue("FontFamily");

				if (value == null)
				{
					return "Times New Roman";
				}

				return (string) value;
			}
			set
			{
				this.Properties.SetPropertyValue("FontFamily", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true)
		]
		public string Name
		{
			get
			{
				object value = this.Properties.GetPropertyValue("FontName");

				if (value == null)
				{
					return null;
				}

				return (string) value;
			}
			set
			{
				this.Properties.SetPropertyValue("FontName", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true)
		]
		public System.Drawing.FontStyle Style
		{
			get
			{
				object value = this.Properties.GetPropertyValue("FontStyle");

				if (value == null)
				{
					return System.Drawing.FontStyle.Regular;
				}

				return (System.Drawing.FontStyle) this.Properties.GetPropertyValue("FontStyle");
			}
			set
			{
				this.Properties.SetPropertyValue("FontStyle", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true)
		]
		public float Size
		{
			get
			{
				object value = this.Properties.GetPropertyValue("FontSize");

				if (value == null)
				{
					return 8.0f;
				}

				return (float) value;
			}
			set
			{
				this.Properties.SetPropertyValue("FontSize", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true)
		]
		public GraphicsUnit Unit
		{
			get
			{
				object value = this.Properties.GetPropertyValue("FontUnit");

				if (value == null)
				{
					return GraphicsUnit.Point;
				}

				return (GraphicsUnit) value;
			}
			set
			{
				this.Properties.SetPropertyValue("FontUnit", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Font CreateFont()
		{
			return new Font(this.Family, this.Size, this.Style, this.Unit);
		}
	}
}
