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
	/// Encapsulates arguments for the origin change event of a view.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The origin of a view is the point in world space that maps to the upper-left
	/// hand corner of the view.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
	/// </remarks>
	public class ViewOriginEventArgs : System.EventArgs
	{
		/// <summary>
		/// Constructs a ViewOriginEventArgs given the original origin and new
		/// origin values.
		/// </summary>
		/// <param name="originOrig">Original origin</param>
		/// <param name="originNew">New origin</param>
		public ViewOriginEventArgs(PointF originOrig, PointF originNew)
		{
			this.originOrig = originOrig;
			this.originNew = originNew;
		}

		/// <summary>
		/// Origin value before the event occurred.
		/// </summary>
		public PointF OriginalOrigin
		{
			get
			{
				return this.originOrig;
			}
		}

		/// <summary>
		/// Origin value after the event occurred.
		/// </summary>
		public PointF NewOrigin
		{
			get
			{
				return this.originNew;
			}
		}

		/// <summary>
		/// Difference between the new origin and the original origin.
		/// </summary>
		public SizeF Offset
		{
			get
			{
				return new SizeF(this.originNew.X - this.originOrig.X, this.originNew.Y - this.originOrig.Y);
			}
		}

		private PointF originOrig;
		private PointF originNew;
	}

	/// <summary>
	/// Delegate declaration for view origin change events.
	/// </summary>
	public delegate void ViewOriginEventHandler(object sender, ViewOriginEventArgs evtArgs);

	/// <summary>
	/// Encapsulates arguments for the magnification change event of a view.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The origin of a view is the point in world space that maps to the upper-left
	/// hand corner of the view.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
	/// </remarks>
	public class ViewMagnificationEventArgs : System.EventArgs
	{
		/// <summary>
		/// Constructs a ViewMagnificationEventArgs given the original magnification and new
		/// magnification values.
		/// </summary>
		/// <param name="magnificationOrig">Original magnification</param>
		/// <param name="magnificationNew">New magnification</param>
		public ViewMagnificationEventArgs(System.Drawing.Size magnificationOrig, System.Drawing.Size magnificationNew)
		{
			this.magnificationOrig = magnificationOrig;
			this.magnificationNew = magnificationNew;
		}

		/// <summary>
		/// Magnification value before the event occurred.
		/// </summary>
		public System.Drawing.Size OriginalMagnification
		{
			get
			{
				return this.magnificationOrig;
			}
		}

		/// <summary>
		/// Magnification value after the event occurred.
		/// </summary>
		public System.Drawing.Size NewMagnification
		{
			get
			{
				return this.magnificationNew;
			}
		}

		private System.Drawing.Size magnificationOrig;
		private System.Drawing.Size magnificationNew;
	}

	/// <summary>
	/// Delegate declaration for view magnification change events.
	/// </summary>
	public delegate void ViewMagnificationEventHandler(object sender, ViewMagnificationEventArgs evtArgs);
}
