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
	/// Interface for performing hit testing on the bounding box of an object.
	/// </summary>
	public interface IHitTestBounds
	{
		/// <summary>
		/// Tests to see if the object's bounding box contains the given point.
		/// </summary>
		/// <param name="ptTest">Point to test</param>
		/// <param name="fSlop">Expands the area to be tested</param>
		/// <returns>true if the object contains the given point, otherwise false</returns>
		bool ContainsPoint(PointF ptTest, float fSlop);

		/// <summary>
		/// Tests to see if the object's bounding box intersects the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if an intersection occurs, otherwise false</returns>
		bool IntersectsRect(RectangleF rcTest);

		/// <summary>
		/// Tests to see if the object's bounding box contains the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if the rectangle is contained by the object, otherwise false</returns>
		bool ContainedByRect(RectangleF rcTest);
	}

	/// <summary>
	/// Interface for performing hit testing on the region of an object.
	/// </summary>
	public interface IHitTestRegion
	{
		/// <summary>
		/// Tests to see if the object's region contains the given point.
		/// </summary>
		/// <param name="ptTest">Point to test</param>
		/// <param name="fSlop">Expands the area to be tested</param>
		/// <returns>true if the object contains the given point, otherwise false</returns>
		bool ContainsPoint(PointF ptTest, float fSlop);

		/// <summary>
		/// Tests to see if the object's region intersects the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if an intersection occurs, otherwise false</returns>
		bool IntersectsRect(RectangleF rcTest);

		/// <summary>
		/// Tests to see if the object's region contains the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if the rectangle is contained by the object, otherwise false</returns>
		bool ContainedByRect(RectangleF rcTest);
	}
}
