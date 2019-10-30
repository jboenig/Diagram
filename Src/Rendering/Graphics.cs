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
	/// Interface to an object that can draw itself and generate a
	/// GraphicsPath and Region.
	/// </summary>
	public interface IGraphics : IDraw
	{
		/// <summary>
		/// Visual representation of the object as a GraphicsPath.
		/// </summary>
		System.Drawing.Drawing2D.GraphicsPath GraphicsPath
		{
			get;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="padding"></param>
		/// <returns></returns>
		System.Drawing.Region CreateRegion(float padding);
	}
}
