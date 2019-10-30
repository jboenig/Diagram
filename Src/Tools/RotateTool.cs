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
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interactive tool for rotating nodes.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RotateCmd"/>
	/// </remarks>
	public class RotateTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public RotateTool() : base(Resources.Strings.Toolnames.Get("RotateTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public RotateTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			ChangeCursor(Resources.Cursors.RotateReady);
			base.OnActivate();
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDeactivate()
		{
			this.tracking = false;
			base.OnDeactivate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point ptCur = new System.Drawing.Point(e.X, e.Y);
			IBounds2DF nodeBounds = null;
			IGraphics nodeGrfx = null;
			ITransform nodeXform = null;
			PointF worldPt;

			if (this.Enabled && this.Active && e.Button == MouseButtons.Left)
			{
				this.nodeHit = this.HitTest(ptCur);

				if (this.nodeHit != null && this.IsNodeAllowed(this.nodeHit))
				{
					ChangeCursor(Resources.Cursors.Rotate);
					this.startPoint = ptCur;
					this.currentPoint = this.startPoint;

					nodeBounds = this.nodeHit as IBounds2DF;
					nodeXform = this.nodeHit as ITransform;
					nodeGrfx = this.nodeHit as IGraphics;

					if (nodeGrfx != null)
					{
						this.grfxPath = (GraphicsPath) nodeGrfx.GraphicsPath.Clone();
						if (nodeXform != null)
						{
							this.grfxPath.Transform(nodeXform.WorldTransform);
						}
					}

					if (nodeBounds != null)
					{
						Global.MatrixStack.Clear();
						if (nodeXform != null)
						{
							Global.MatrixStack.Push(nodeXform.ParentTransform);
						}
						worldPt = Geometry.CenterPoint(nodeBounds.Bounds);
						Global.MatrixStack.Clear();

						this.origin = this.Controller.View.ViewToDevice(this.Controller.View.WorldToView(worldPt));
						this.Controller.View.DrawTrackingLine(this.origin, this.startPoint);
						this.trackingRect = this.Controller.View.DrawTrackingPath(this.grfxPath, this.CurrentAngle);
						this.tracking = true;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point ptCur = new System.Drawing.Point(e.X, e.Y);

			if (this.Enabled && this.Active)
			{
				if (this.tracking)
				{
					// Erase tracking
					this.Controller.View.DrawTrackingLine(this.origin, this.currentPoint);
					this.Controller.View.Refresh(this.trackingRect);
					// Update current point
					this.currentPoint = ptCur;
					// Redraw tracking
					this.Controller.View.DrawTrackingLine(this.origin, this.currentPoint);
					this.trackingRect = this.Controller.View.DrawTrackingPath(this.grfxPath, this.CurrentAngle);
				}
				else
				{
					// Not tracking yet. See if mouse is over a handle.
					this.nodeHit = this.HitTest(ptCur);

					if (this.nodeHit != null && this.IsNodeAllowed(this.nodeHit))
					{
						this.ChangeCursor(Resources.Cursors.Rotate);
					}
					else
					{
						this.ChangeCursor(Resources.Cursors.RotateReady);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			Point ptCur = new Point(e.X, e.Y);

			if (this.Enabled && this.Active)
			{
				if (this.tracking)
				{
					// Erase tracking
					this.Controller.View.DrawTrackingLine(this.origin, this.currentPoint);
					this.Controller.View.Refresh(this.trackingRect);
				}

				// Execute rotate command
				if (this.nodeHit != null && this.IsNodeAllowed(this.nodeHit))
				{
					RotateCmd rotateCmd = new RotateCmd();
					rotateCmd.Nodes.Add(this.nodeHit);
					rotateCmd.Degrees = this.CurrentAngle;
					this.Controller.ExecuteCommand(rotateCmd);
				}

				this.Controller.DeactivateTool(this);

				this.Controller.View.Draw();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		private INode HitTest(System.Drawing.Point pt)
		{
			INode nodeHit = null;
			NodeCollection nodesHit = new NodeCollection();
			if (this.Controller.View.GetSelectedNodesAtPoint(nodesHit, pt) > 0)
			{
				nodeHit = nodesHit[0];
			}
			return nodeHit;
		}

		/// <summary>
		/// 
		/// </summary>
		private float CurrentAngle
		{
			get
			{
				double radians = Geometry.LineAngle(this.startPoint, this.origin, this.currentPoint);
				return (float) ((radians * 180.0) / Math.PI);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		protected bool IsNodeAllowed(INode node)
		{
			bool allowed = true;

			IPropertyContainer curProps = node as IPropertyContainer;
			if (curProps != null)
			{
				allowed = (bool) curProps.GetPropertyValue("AllowRotate");
			}

			return allowed;
		}

		/// <summary>
		/// 
		/// </summary>
		protected bool tracking = false;

		/// <summary>
		/// 
		/// </summary>
		protected INode nodeHit = null;

		/// <summary>
		/// 
		/// </summary>
		protected BoxPosition posHandle;

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point origin;

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point startPoint;

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Rectangle trackingRect;

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point currentPoint;

		/// <summary>
		/// 
		/// </summary>
		protected GraphicsPath grfxPath = null;
	}
}
