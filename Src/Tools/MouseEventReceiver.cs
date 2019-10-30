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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interface implemented by tools that want to receive mouse events.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// </remarks>
	public interface IMouseEventReceiver
	{
		/// <summary>
		/// Called when a mouse down event occurs.
		/// </summary>
		/// <param name="e">Mouse event arguments</param>
		void MouseDown(System.Windows.Forms.MouseEventArgs e);

		/// <summary>
		/// Called when a mouse move event occurs.
		/// </summary>
		/// <param name="e">Mouse event arguments</param>
		void MouseMove(System.Windows.Forms.MouseEventArgs e);

		/// <summary>
		/// Called when a mouse up event occurs.
		/// </summary>
		/// <param name="e">Mouse event arguments</param>
		void MouseUp(System.Windows.Forms.MouseEventArgs e);
	}
}
