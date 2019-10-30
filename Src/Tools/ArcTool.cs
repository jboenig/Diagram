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
	/// Interactive tool for drawing arcs.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Arc"/>
	/// </remarks>
	public class ArcTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public ArcTool() : base(Resources.Strings.Toolnames.Get("ArcTool"))
		{
			this.drawing = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ArcTool(string name) : base(name)
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
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.startingPt = this.Controller.View.SnapPointToGrid(e.X, e.Y);
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
			System.Drawing.Point pt = this.Controller.View.SnapPointToGrid(e.X, e.Y);

			if (this.Active && this.drawing)
			{
				System.Drawing.Rectangle rcBounds;
				float startAngle;
				float sweepAngle;
				Geometry.ArcFromPoints(this.startingPt, pt, out rcBounds, out startAngle, out sweepAngle);
				//Controller.View.DrawTrackingRect(rcBounds);
				Controller.View.Draw();
				Controller.View.DrawTrackingArc(rcBounds, startAngle, sweepAngle);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = this.Controller.View.SnapPointToGrid(e.X, e.Y);

			if (this.Active && this.drawing)
			{
				this.drawing = false;

				System.Drawing.Rectangle rcBounds;
				float startAngle;
				float sweepAngle;

				Geometry.ArcFromPoints(this.startingPt, pt, out rcBounds, out startAngle, out sweepAngle);

				RectangleF rcWorld = Controller.View.ViewToWorld(Controller.View.DeviceToView(rcBounds));
				Syncfusion.Windows.Forms.Diagram.Arc shape = new Syncfusion.Windows.Forms.Diagram.Arc(rcWorld, startAngle, sweepAngle);
				InsertNodesCmd cmd = new InsertNodesCmd();
				cmd.Nodes.Add(shape);
				this.Controller.ExecuteCommand(cmd);

				this.Controller.DeactivateTool(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point startingPt;

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point endingPt;

		/// <summary>
		/// 
		/// </summary>
		protected bool drawing;
	}
}