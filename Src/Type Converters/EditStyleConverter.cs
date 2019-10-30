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
	/// Summary description for EditStyleConverter.
	/// </summary>
	public class EditStyleConverter : System.ComponentModel.TypeConverter
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
				EditStyle editStyle = value as EditStyle;
				if (editStyle != null)
				{
					return ("AllowMove="+editStyle.AllowMove.ToString() + ", AllowRotate="+editStyle.AllowRotate.ToString() + ", AllowResize="+editStyle.AllowResize.ToString() + "AllowSelect="+editStyle.AllowSelect.ToString() + "AllowVertexEdit="+editStyle.AllowVertexEdit.ToString());
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
			return TypeDescriptor.GetProperties(value, attrs);
		}
	}
}
