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
	/// This interface is implemented by tools that want to receive
	/// click and double-click events from the controller.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// </remarks>
	public interface IClickEventReceiver
	{
		/// <summary>
		/// Called when a click event occurs.
		/// </summary>
		void Click();

		/// <summary>
		/// Called when a double-click event occurs.
		/// </summary>
		void DoubleClick();
	}
}
