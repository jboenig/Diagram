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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Custom property editor for labels on a Symbol object.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Symbol"/>
	/// </remarks>
	public class SymbolLabelCollectionEditor : CollectionEditor
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="collectionType"></param>
		public SymbolLabelCollectionEditor(System.Type collectionType) : base(collectionType)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override System.Type[] CreateNewItemTypes()
		{
			return this.newItemTypes;
		}

		/// <summary>
		/// 
		/// </summary>
		private System.Type[] newItemTypes = new System.Type[] { typeof(SymbolLabel) };
	}
}
