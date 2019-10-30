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

using Syncfusion.Windows.Forms.Diagram;

namespace Syncfusion.Windows.Forms.Diagram.Controls
{
	/// <summary>
	/// Type converter for a SymbolPalette.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
	/// </remarks>
	public class SymbolPaletteConverter : System.ComponentModel.TypeConverter
	{
		/// <summary>
		/// Converts a SymbolPalette to other data types.
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
				if (value != null)
				{
					SymbolPalette symPalette = value as SymbolPalette;
					if (symPalette != null)
					{
						return symPalette.Name;
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Converts a SymbolPalette from other types.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value != null && value.GetType() == typeof(string))
			{
				string fileName = (string) value;
				PaletteGroupView palGrpVw = context.Instance as PaletteGroupView;
				if (palGrpVw != null)
				{
					if (palGrpVw.LoadPalette(fileName))
					{
						return palGrpVw.Palette;
					}
					return null;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
