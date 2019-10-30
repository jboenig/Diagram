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
	/// Interactive tool for zooming in and out of a diagram's view.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Left click increases magnification by 25%. Right click decreases
	/// magnification by 25%.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.Magnification"/>
	/// </remarks>
	public class ZoomTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public ZoomTool() : base(Resources.Strings.Toolnames.Get("ZoomTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ZoomTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			ChangeCursor(Resources.Cursors.Zoom);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tool"></param>
		public override void ToolActivating(Tool tool)
		{
			if (this.Active)
			{
				this.Deactivate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active && this.Controller.View != null)
			{
				if (e.Button == MouseButtons.Left)
				{
					Size szMag = new Size(this.Controller.View.Magnification.Width + 25, this.Controller.View.Magnification.Height + 25);
					this.Controller.View.Magnification = szMag;
					this.Controller.View.Draw();
				}
				else if (e.Button == MouseButtons.Right)
				{
					Size szMag = new Size(this.Controller.View.Magnification.Width - 25, this.Controller.View.Magnification.Height - 25);
					this.Controller.View.Magnification = szMag;
					this.Controller.View.Draw();
				}
			}
		}
	}
}
