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
	/// Interface to nodes that can translate, scale, and rotate.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface provides methods for transforming nodes via a
	/// transformation matrix.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Global.MatrixStack"/>
	/// </remarks>
	public interface ITransform
	{
		/// <summary>
		/// Moves the node by the given X and Y offsets.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		void Translate(float dx, float dy);

		/// <summary>
		/// Rotates the node a specified number of degrees about a given
		/// anchor point.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to rotate</param>
		/// <param name="degrees">Number of degrees to rotate</param>
		void Rotate(PointF ptAnchor, float degrees);

		/// <summary>
		/// Rotates the node a specified number of degrees about its center point.
		/// </summary>
		/// <param name="degrees">Number of degrees to rotate</param>
		void Rotate(float degrees);

		/// <summary>
		/// Scales the node about a specified anchor point by a given ratio.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to scale</param>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		void Scale(PointF ptAnchor, float sx, float sy);

		/// <summary>
		/// Scales the node about its center point by a given ratio.
		/// </summary>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		void Scale(float sx, float sy);

		/// <summary>
		/// Matrix containing local transformations for this node.
		/// </summary>
		Matrix LocalTransform
		{
			get;
		}

		/// <summary>
		/// Returns a matrix containing transformations for this node and all of
		/// its ancestors.
		/// </summary>
		Matrix WorldTransform
		{
			get;
		}

		/// <summary>
		/// Returns a matrix containing the transformations of this node's parent.
		/// </summary>
		Matrix ParentTransform
		{
			get;
		}
	}
}
