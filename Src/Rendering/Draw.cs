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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interface to objects that render themselves to a Graphics context.
	/// </summary>
	public interface IDraw
	{
		/// <summary>
		/// Renders the object onto a graphics context.
		/// </summary>
		/// <param name="grfx">Graphics context to render onto</param>
		void Draw(System.Drawing.Graphics grfx);
	}
}
