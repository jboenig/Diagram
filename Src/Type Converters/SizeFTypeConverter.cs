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
using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Summary description for SizeFConverter.
	/// </summary>
	public class SizeFConverter : System.ComponentModel.TypeConverter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) 
		{  
			if (destinationType == typeof(string)) 
			{
				SizeF sz = (SizeF) value;
				return (sz.Width + "," + sz.Height);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			System.Attribute[] attrs = new System.Attribute[]
			{
				new System.ComponentModel.BrowsableAttribute(true)
			};
			PropertyDescriptorCollection propDesc = TypeDescriptor.GetProperties(value, attrs);

			return propDesc;
		}
	}
}
