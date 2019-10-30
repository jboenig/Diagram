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
using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Summary description for LineDecoratorConverter.
	/// </summary>
	public class LineDecoratorConverter : System.ComponentModel.TypeConverter
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
				if (value == null)
				{
					return "(none)";
				}
				else
				{
					LineDecorator lineDecorator = value as LineDecorator;
					if (lineDecorator != null)
					{
						return lineDecorator.GetType().Name;
					}
				}
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
			return false;
		}

#if false
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
			return TypeDescriptor.GetProperties(value, attrs);
		}
#endif
	}
}
