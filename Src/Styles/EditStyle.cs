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
using System.Reflection;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Encapsulates the edit properties of an object.
	/// </summary>
	public class EditStyle : Syncfusion.Windows.Forms.Diagram.Style
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="propContainer"></param>
		public EditStyle(IPropertyContainer propContainer) :
			base(propContainer)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AllowSelect
		{
			get
			{
				return (bool) this.Properties.GetPropertyValue("AllowSelect");
			}
			set
			{
				this.Properties.SetPropertyValue("AllowSelect", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AllowVertexEdit
		{
			get
			{
				return (bool) this.Properties.GetPropertyValue("AllowVertexEdit");
			}
			set
			{
				this.Properties.SetPropertyValue("AllowVertexEdit", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AllowMove
		{
			get
			{
				return (bool) this.Properties.GetPropertyValue("AllowMove");
			}
			set
			{
				this.Properties.SetPropertyValue("AllowMove", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AllowRotate
		{
			get
			{
				return (bool) this.Properties.GetPropertyValue("AllowRotate");
			}
			set
			{
				this.Properties.SetPropertyValue("AllowRotate", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AllowResize
		{
			get
			{
				return (bool) this.Properties.GetPropertyValue("AllowResize");
			}
			set
			{
				this.Properties.SetPropertyValue("AllowResize", value);
			}
		}
	}
}
