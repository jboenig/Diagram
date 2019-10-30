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
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for tools that draw tracking rectangles.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class provides a base implementation for tools that require the
	/// user to draw a rectangle. When a mouse down event is received, the
	/// point at which the event occurred becomes the first point in the
	/// rectangle. As the mouse moves, the rectangle is tracked. When a
	/// mouse up event is received, this class calls the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.RectangleToolBase.OnComplete"/>
	/// method which is implemented by derived classes.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// </remarks>
	public abstract class RectangleToolBase : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public RectangleToolBase(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			this.ChangeCursor(Cursors.Cross);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDeactivate()
		{
			this.drawing = false;
			base.OnDeactivate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.startingPoint = this.View.SnapPointToGrid(e.X, e.Y);
					this.drawing = true;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active && this.drawing)
			{
				System.Drawing.Point ptEnd = this.Controller.View.SnapPointToGrid(e.X, e.Y);
				this.Controller.View.Draw();
				this.Controller.View.DrawTrackingRect(this.startingPoint, ptEnd);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active && this.drawing)
			{
				System.Drawing.Point ptEnd = this.Controller.View.SnapPointToGrid(e.X, e.Y);

				System.Drawing.Rectangle rcDevice = Geometry.CreateRect(this.startingPoint, ptEnd);
				this.rect = this.Controller.View.ViewToWorld(Controller.View.DeviceToView(rcDevice));
				this.OnComplete();

				this.Controller.DeactivateTool(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected abstract void OnComplete();

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point startingPoint;

		/// <summary>
		/// 
		/// </summary>
		protected bool drawing = false;

		/// <summary>
		/// 
		/// </summary>
		protected RectangleF rect;
	}
}