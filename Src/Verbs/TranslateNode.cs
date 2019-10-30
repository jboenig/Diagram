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
	/// Translates a node.
	/// </summary>
	public class TranslateNode : IVerb
	{
		/// <summary>
		/// 
		/// </summary>
		public TranslateNode(ITransform nodeXform, float dx, float dy)
		{
			this.nodeXform = nodeXform;
			this.dx = dx;
			this.dy = dy;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		public bool Do(object target)
		{
			bool success = false;
			if (this.nodeXform != null)
			{
				this.nodeXform.Translate(this.dx, this.dy);
				success = true;
			}
			return success;
		}

		/// <summary>
		/// 
		/// </summary>
		private ITransform nodeXform = null;

		/// <summary>
		/// 
		/// </summary>
		private float dx = 0.0f;

		/// <summary>
		/// 
		/// </summary>
		private float dy = 0.0f;
	}
}
