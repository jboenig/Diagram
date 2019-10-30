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
using System.Collections;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Specifies a type of select handle.
	/// </summary>
	public enum SelectHandleType
	{
		/// <summary>
		/// Resize handle
		/// </summary>
		Resize,

		/// <summary>
		/// Vertex handle
		/// </summary>
		Vertex
	}

	/// <summary>
	/// Specifies how resize handles are to be rendered.
	/// </summary>
	public enum ResizeHandleStyle
	{
		/// <summary>
		/// Render handles outside the bounds
		/// </summary>
		OutsideBounds,

		/// <summary>
		/// Render handles on the bounds
		/// </summary>
		OverlapBounds
	}
}