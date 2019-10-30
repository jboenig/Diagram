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
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interface for accessing and modifying the collection of points
	/// belonging to an object.
	/// </summary>
	public interface IPoints
	{
		/// <summary>
		/// Number of points in the object.
		/// </summary>
		int PointCount
		{
			get;
		}

		/// <summary>
		/// Minimum number of points that the object may contain.
		/// </summary>
		int MinPoints
		{
			get;
		}

		/// <summary>
		/// Maximum number of points that the object may contain.
		/// </summary>
		int MaxPoints
		{
			get;
		}

		/// <summary>
		/// Returns an array containing all of the points in the object.
		/// </summary>
		/// <returns>Array of points</returns>
		PointF[] GetPoints();

		/// <summary>
		/// Assigns the given array of points to the object.
		/// </summary>
		/// <param name="pts">Array of points to assign</param>
		void SetPoints(PointF[] pts);

		/// <summary>
		/// Returns the point at the specified index position.
		/// </summary>
		/// <param name="ptIdx">Zero-based index position</param>
		/// <returns>Point at the given position</returns>
		PointF GetPoint(int ptIdx);

		/// <summary>
		/// Assigns the value of the point at the specified index position.
		/// </summary>
		/// <param name="ptIdx">Zero-based index position of point to update</param>
		/// <param name="val">Value to assign to point</param>
		void SetPoint(int ptIdx, PointF val);

		/// <summary>
		/// Adds a point to the object.
		/// </summary>
		/// <param name="val">Point to add</param>
		void AddPoint(PointF val);

		/// <summary>
		/// Inserts a point into the object at a specified index position.
		/// </summary>
		/// <param name="ptIdx">Zero-based index position at which to insert the point</param>
		/// <param name="val">Point to index</param>
		void InsertPoint(int ptIdx, PointF val);

		/// <summary>
		/// Remove the point at the specified index position.
		/// </summary>
		/// <param name="ptIdx">Zero-based index position of the point to remove</param>
		void RemovePoint(int ptIdx);

		/// <summary>
		/// Remove all points in the object.
		/// </summary>
		void RemoveAllPoints();
	}
}
