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
#if false
	/// <summary>
	/// Summary description for NodeCallback.
	/// </summary>
	interface IDispatchNodeEvents
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void PropertyChanged(PropertyEventArgs evtArgs);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void ChildAdded(NodeCollectionEventArgs evtArgs);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void ChildRemoved(NodeCollectionEventArgs evtArgs);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void BoundsChanged(BoundsEventArgs evtArgs);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void Move(MoveEventArgs evtArgs);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void Rotate(RotateEventArgs evtArgs);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void Scale(ScaleEventArgs evtArgs);
	}
#endif
}
