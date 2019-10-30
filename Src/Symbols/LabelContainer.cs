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
using System.Drawing.Drawing2D;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interface to objects that contain labels.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase"/>
	/// </remarks>
	public interface ILabelContainer : IServiceProvider
	{
		/// <summary>
		/// Calculates the position of the given label.
		/// </summary>
		PointF CalcLabelPosition(Label label);

		/// <summary>
		/// Calculates a world transformation matrix for a label.
		/// </summary>
		/// <param name="labelTransform">The label's own local transformation matrix</param>
		/// <returns>Matrix for transforming the label's local coordinates into world coordinates</returns>
		/// <remarks>
		/// This method is generally used to exclude the label container's
		/// transformation matrix from the label's world transformation, because
		/// scaling and rotating the label container should not affect the label.
		/// </remarks>
		Matrix GetLabelTransform(Matrix labelTransform);
	}
}
