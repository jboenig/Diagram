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
using System.Windows.Forms;

using Syncfusion.Windows.Forms.Diagram;

namespace Syncfusion.Windows.Forms.Diagram.Controls
{
	/// <summary>
	/// Event argument class for events associated with symbol models loaded
	/// in a PaletteGroupBar object.
	/// </summary>
	public class SymbolModelEventArgs : EventArgs
	{
		/// <summary>
		/// Constructs a SymbolModelEventArgs object from a SymbolModel
		/// </summary>
		/// <param name="symbolMdl"></param>
		public SymbolModelEventArgs(SymbolModel symbolMdl)
		{
			this.symbolMdl = symbolMdl;
		}

		/// <summary>
		/// The SymbolModel that generated the event.
		/// </summary>
		public SymbolModel Model
		{
			get
			{
				return this.symbolMdl;
			}
		}

		private SymbolModel symbolMdl;
	}

	/// <summary>
	/// Delegate for events associated with symbol models loaded
	/// in a PaletteGroupBar object.
	/// </summary>
	public delegate void SymbolModelEvent(object sender, SymbolModelEventArgs evtArgs);
}
