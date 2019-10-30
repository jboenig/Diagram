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
using System.Windows.Forms;
using System.Drawing;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interactive tool for panning (scrolling) the view of a diagram.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.Origin"/>
	/// </remarks>
	public class PanTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public PanTool() : base(Resources.Strings.Toolnames.Get("PanTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public PanTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			ChangeCursor(Resources.Cursors.PanReady);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tool"></param>
		public override void ToolActivating(Tool tool)
		{
			if (this.Active)
			{
				this.Controller.DeactivateTool(this);
			}
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
					this.startingPoint = new System.Drawing.Point(e.X, e.Y);
					this.originStart = this.Controller.View.Origin;
					ChangeCursor(Resources.Cursors.Panning);
					this.panning = true;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active && this.panning)
			{
				Size szOffset = new Size(this.startingPoint.X - e.X, this.startingPoint.Y - e.Y);
				SizeF szLogOffset = this.Controller.View.ViewToWorld(this.Controller.View.DeviceToView(szOffset));
				PointF newOrigin = new PointF(this.originStart.X + szLogOffset.Width, this.originStart.Y + szLogOffset.Height);
				this.Controller.View.Origin = newOrigin;
				this.Controller.View.Draw();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active)
			{
				this.ChangeCursor(Resources.Cursors.PanReady);

				this.panning = false;

				Size szOffset = new Size(this.startingPoint.X - e.X, this.startingPoint.Y - e.Y);
				SizeF szLogOffset = this.Controller.View.ViewToWorld(this.Controller.View.DeviceToView(szOffset));
				PointF newOrigin = new PointF(this.originStart.X + szLogOffset.Width, this.originStart.Y + szLogOffset.Height);
				this.Controller.View.Origin = newOrigin;
				this.Controller.View.Draw();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected bool panning = false;

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point startingPoint;

		/// <summary>
		/// 
		/// </summary>
		protected PointF originStart;
	}
}
