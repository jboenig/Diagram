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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A style is an object that encapsulates one or more properties
	/// in a property container.
	/// </summary>
	/// <remarks>
	/// Style objects provide a wrapper for a collection of properties in
	/// a property container. Getting and setting properties through a style
	/// object is easier than doing it through the IPropertyContainer interface
	/// because the style object provides type-safe wrappers for each property.
	/// The properties exposed through a style object are also browsable in
	/// the property grid.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
	/// </remarks>
	public abstract class Style
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="propContainer"></param>
		public Style(IPropertyContainer propContainer)
		{
			this.propContainer = propContainer;
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public IPropertyContainer Properties
		{
			get
			{
				return this.propContainer;
			}
			set
			{
				this.propContainer = value;
			}
		}

		private IPropertyContainer propContainer = null;
	}
}
