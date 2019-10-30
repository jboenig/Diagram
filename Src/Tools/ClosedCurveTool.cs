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
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interactive tool for drawing closed curves.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IClickEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ClosedCurve"/>
	/// </remarks>
	public class ClosedCurveTool : Tool, IMouseEventReceiver, IClickEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public ClosedCurveTool() : base(Resources.Strings.Toolnames.Get("ClosedCurveTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ClosedCurveTool(string name) : base(name)
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
			this.tracking = false;
			this.drawing = false;
			this.points.Clear();
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
					System.Drawing.Point pt = new System.Drawing.Point(e.X, e.Y);
					this.drawing = true;
					this.tracking = false;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active && this.tracking)
			{
				int numPts = this.points.Count;
				Point[] trackingPts = new Point[numPts + 1];
				for (int ptIdx = 0; ptIdx < numPts; ptIdx++)
				{
					trackingPts[ptIdx] = (System.Drawing.Point) this.points[ptIdx];
				}
				trackingPts[numPts] = new System.Drawing.Point(e.X, e.Y);
				Controller.View.Draw();
				Controller.View.DrawTrackingCurve(trackingPts);
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
				this.points.Add(new System.Drawing.Point(e.X, e.Y));
				this.tracking = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void IClickEventReceiver.Click()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		void IClickEventReceiver.DoubleClick()
		{
			if (this.Active && this.drawing)
			{
				Point[] devicePts = (Point[]) this.points.ToArray(typeof(Point));
				PointF[] viewPts;
				PointF[] worldPts;
				Controller.View.DeviceToView(devicePts, out viewPts);
				Controller.View.ViewToWorld(viewPts, out worldPts);

				Syncfusion.Windows.Forms.Diagram.ClosedCurve shape = new Syncfusion.Windows.Forms.Diagram.ClosedCurve(worldPts);
				InsertNodesCmd cmd = new InsertNodesCmd();
				cmd.Nodes.Add(shape);
				this.Controller.ExecuteCommand(cmd);

				this.Controller.DeactivateTool(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected ArrayList points = new ArrayList();

		/// <summary>
		/// 
		/// </summary>
		protected bool drawing = false;

		/// <summary>
		/// 
		/// </summary>
		protected bool tracking = false;
	}
}
