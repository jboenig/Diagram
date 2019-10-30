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
	/// Interactive tool for drawing orthogonal lines.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.OrthogonalLine"/>
	/// </remarks>
	public class OrthogonalLineTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public OrthogonalLineTool() : base(Resources.Strings.Toolnames.Get("OrthogonalLineTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public OrthogonalLineTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			ChangeCursor(Cursors.Cross);
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
					this.startingPoint = this.Controller.View.SnapPointToGrid(e.X, e.Y);
				}
				this.drawing = true;
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
				Controller.View.Draw();
				Controller.View.DrawTrackingLine(this.startingPoint, ptEnd);
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

				Point[] devicePts = { this.startingPoint, ptEnd };
				PointF[] viewPts;
				PointF[] worldPts;
				Controller.View.DeviceToView(devicePts, out viewPts);
				Controller.View.ViewToWorld(viewPts, out worldPts);

				Syncfusion.Windows.Forms.Diagram.OrthogonalLine shape = new Syncfusion.Windows.Forms.Diagram.OrthogonalLine(worldPts[0], worldPts[1]);
				InsertNodesCmd cmd = new InsertNodesCmd();
				cmd.Nodes.Add(shape);
				this.Controller.ExecuteCommand(cmd);

				this.Controller.DeactivateTool(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point startingPoint;

		/// <summary>
		/// 
		/// </summary>
		protected bool drawing = false;
	}
}
