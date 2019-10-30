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
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A symbol palette is a collection of related SymbolModel objects used
	/// to add symbols to a diagram.
	/// </summary>
	/// <remarks>
	/// This class is a special type of Model object that contains only
	/// SymbolModel objects. SymbolPalettes are serializable and can be
	/// saved to a file and reloaded. This class is used in conjuction
	/// with the PaletteGroupBar control, which displays the contents
	/// a SymbolPalette and allows the user to drag and drop symbols
	/// onto a diagram.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel"/>
	/// </remarks>
	[
		Serializable
	]
	public class SymbolPalette : Model
	{
		/// <summary>
		/// 
		/// </summary>
		public SymbolPalette()
		{
			this.Name = "SymbolPalette";
		}

		/// <summary>
		/// Serialization constructor for symbol palettes.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected SymbolPalette(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="symbolName"></param>
		/// <returns></returns>
		public SymbolModel AddSymbol(string symbolName)
		{
			SymbolModel symbolMdl = new SymbolModel();
			symbolMdl.Name = symbolName;
			AppendChild(symbolMdl);
			return symbolMdl;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="symbolMdl"></param>
		public void RemoveSymbol(SymbolModel symbolMdl)
		{
			int symbolIdx = this.GetChildIndex(symbolMdl);
			if (symbolIdx >= 0)
			{
				this.RemoveChild(symbolIdx);
			}
		}
	}
}
