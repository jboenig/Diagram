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
using System.Drawing.Printing;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// This interface is implemented by classes that can print.
	/// </summary>
	public interface IPrint
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void PrintPage(PrintPageEventArgs evtArgs);
	}
}
