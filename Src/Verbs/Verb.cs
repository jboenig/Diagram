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
	/// Interface to verb objects.
	/// </summary>
	/// <remarks>
	/// A verb is an object that performs an action.
	/// </remarks>
	public interface IVerb
	{
		/// <summary>
		/// Performs the action defined by the verb.
		/// </summary>
		/// <param name="target">Object that is acted upon (noun)</param>
		/// <returns>true if successful, otherwise false</returns>
		bool Do(object target);
	}
}
