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
	/// Interface to objects with 2-dimensional, integer bounds
	/// </summary>
	public interface IBounds2D
	{
		/// <summary>
		/// (X,Y) location of the object
		/// </summary>
		Point Location
		{
			get;
			set;
		}

		/// <summary>
		/// Width and height of the object
		/// </summary>
		Size Size
		{
			get;
			set;
		}

		/// <summary>
		/// X-coordinate of the object's location
		/// </summary>
		int X
		{
			get;
		}

		/// <summary>
		/// Y-coordinate of the object's location
		/// </summary>
		int Y
		{
			get;
		}

		/// <summary>
		/// Width of the object
		/// </summary>
		int Width
		{
			get;
		}

		/// <summary>
		/// Height of the object
		/// </summary>
		int Height
		{
			get;
		}

		/// <summary>
		/// Bounds of the object
		/// </summary>
		System.Drawing.Rectangle Bounds
		{
			get;
		}
	}

	/// <summary>
	/// Interface to a objects with 2-dimensional, floating point bounds
	/// </summary>
	public interface IBounds2DF
	{
		/// <summary>
		/// Position and size of the object.
		/// </summary>
		RectangleF Bounds
		{
			get;
			set;
		}

		/// <summary>
		/// X-coordinate of the location.
		/// </summary>
		float X
		{
			get;
			set;
		}

		/// <summary>
		/// Y-coordinate of the location.
		/// </summary>
		float Y
		{
			get;
			set;
		}

		/// <summary>
		/// Width of the object.
		/// </summary>
		float Width
		{
			get;
			set;
		}

		/// <summary>
		/// Height of the object.
		/// </summary>
		float Height
		{
			get;
			set;
		}
	}

	/// <summary>
	/// Interface to a objects with 2-dimensional, floating point bounds
	/// </summary>
	public interface ILocalBounds2DF
	{
		/// <summary>
		/// Position and size of the object.
		/// </summary>
		RectangleF Bounds
		{
			get;
			set;
		}

		/// <summary>
		/// X-coordinate of the location.
		/// </summary>
		float X
		{
			get;
			set;
		}

		/// <summary>
		/// Y-coordinate of the location.
		/// </summary>
		float Y
		{
			get;
			set;
		}

		/// <summary>
		/// Width of the object.
		/// </summary>
		float Width
		{
			get;
			set;
		}

		/// <summary>
		/// Height of the object.
		/// </summary>
		float Height
		{
			get;
			set;
		}
	}
}
