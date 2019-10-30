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
	/// Sets the bounds of a node.
	/// </summary>
	public class SetBounds : IVerb
	{
		/// <summary>
		/// 
		/// </summary>
		public SetBounds(IBounds2DF boundsObj, RectangleF boundingRect)
		{
			this.boundsObj = boundsObj;
			this.boundingRect = boundingRect;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		public bool Do(object target)
		{
			bool success = false;
			if (this.boundsObj != null)
			{
				this.boundsObj.Bounds = boundingRect;
				success = true;
			}
			return success;
		}

		/// <summary>
		/// 
		/// </summary>
		private IBounds2DF boundsObj = null;

		/// <summary>
		/// 
		/// </summary>
		private RectangleF boundingRect;
	}
}
