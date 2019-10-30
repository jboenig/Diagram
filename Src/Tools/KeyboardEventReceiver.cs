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
	/// Interface implemented by tools that want to receive keyboard events.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// </remarks>
	public interface IKeyboardEventReceiver
	{
		/// <summary>
		/// Called when a key down event occurs.
		/// </summary>
		/// <param name="e">Key event arguments</param>
		void KeyDown(System.Windows.Forms.KeyEventArgs e);

		/// <summary>
		/// Called when a key up event occurs.
		/// </summary>
		/// <param name="e">Key event arguments</param>
		void KeyUp(System.Windows.Forms.KeyEventArgs e);

		/// <summary>
		/// Called when a key is pressed.
		/// </summary>
		/// <param name="e">Key press event arguments</param>
		void KeyPress(System.Windows.Forms.KeyPressEventArgs e);
	}
}
