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
	/// Interactive tool for drawing polylines.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IClickEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PolyLine"/>
	/// </remarks>
	public class PolyLineTool : Tool, IMouseEventReceiver, IClickEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public PolyLineTool() : base(Resources.Strings.Toolnames.Get("PolyLineTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public PolyLineTool(string name) : base(name)
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
			this.trackingPts = null;
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
					System.Drawing.Point pt = this.Controller.View.SnapPointToGrid(e.X, e.Y);
					this.drawing = true;
					this.tracking = false;
					if (this.trackingPts != null)
					{
						this.Controller.View.DrawTrackingLines(this.trackingPts);
					}
					this.trackingPts = null;
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
				// Erase existing tracking lines
				if (this.trackingPts != null)
				{
					this.Controller.View.DrawTrackingLines(this.trackingPts);
				}

				// Update last point in the array with current mouse position
				this.UpdateTrackingPoints(this.Controller.View.SnapPointToGrid(e.X, e.Y));

				// Draw new tracking lines
				this.Controller.View.DrawTrackingLines(this.trackingPts);
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
				System.Drawing.Point pt = this.Controller.View.SnapPointToGrid(e.X, e.Y);
				this.points.Add(pt);
				// Track mouse movements
				this.tracking = true;
				InitTrackingPoints();
				if (this.trackingPts != null)
				{
					this.Controller.View.DrawTrackingLines(this.trackingPts);
				}
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

				Syncfusion.Windows.Forms.Diagram.PolyLine polyLineShape = new Syncfusion.Windows.Forms.Diagram.PolyLine(worldPts);
				InsertNodesCmd cmd = new InsertNodesCmd();
				cmd.Nodes.Add(polyLineShape);
				this.Controller.ExecuteCommand(cmd);

				this.Controller.DeactivateTool(this);
			}
		}

		internal void InitTrackingPoints()
		{
			int numPts = this.points.Count;
			this.trackingPts = null;

			if (numPts > 1)
			{
				this.trackingPts = new Point[numPts];

				for (int ptIdx = 0; ptIdx < numPts; ptIdx++)
				{
					this.trackingPts[ptIdx] = (System.Drawing.Point) this.points[ptIdx];
				}
			}
		}

		internal void UpdateTrackingPoints(Point pt)
		{
			int numPts = this.points.Count;
			this.trackingPts = null;

			if (numPts > 0)
			{
				this.trackingPts = new Point[numPts+1];

				for (int ptIdx = 0; ptIdx < numPts; ptIdx++)
				{
					this.trackingPts[ptIdx] = (System.Drawing.Point) this.points[ptIdx];
				}
				this.trackingPts[numPts] = pt;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected ArrayList points = new ArrayList();

		/// <summary>
		/// 
		/// </summary>
		protected Point[] trackingPts;

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
