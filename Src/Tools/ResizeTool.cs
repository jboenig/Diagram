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
	/// Interactive tool for resizing nodes.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ResizeCmd"/>
	/// </remarks>
	public class ResizeTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public ResizeTool() : base(Resources.Strings.Toolnames.Get("ResizeTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ResizeTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			base.OnActivate();
			this.SetCursor(this.handleHit);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			IBounds2DF boundsObj = null;

			if (this.Enabled && e.Button == MouseButtons.Left)
			{
				System.Drawing.Point ptCur = new System.Drawing.Point(e.X, e.Y);
				this.nodeHit = this.Controller.ResizeHandleHitNode;
				this.handleHit = this.Controller.ResizeHandleHit;

				if (this.nodeHit != null)
				{
					boundsObj = this.nodeHit as IBounds2DF;
					if (boundsObj != null)
					{
						this.startBounds = Controller.View.ViewToDevice(Controller.View.WorldToView(boundsObj.Bounds));
						this.curBounds = ResizeTool.MoveAnchorPoint(this.startBounds, this.handleHit, ptCur);
						Controller.View.DrawTrackingRect(this.curBounds);
					}

					this.Controller.ActivateTool(this);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Enabled)
			{
				System.Drawing.Point ptCur;

				if (this.Active)
				{
					ptCur = this.Controller.View.SnapPointToGrid(e.X, e.Y);
					this.Controller.View.DrawTrackingRect(this.curBounds);
					this.curBounds = MoveAnchorPoint(this.startBounds, this.handleHit, ptCur);
					this.Controller.View.DrawTrackingRect(this.curBounds);
				}
				else
				{
					ptCur = new System.Drawing.Point(e.X, e.Y);
					this.nodeHit = this.Controller.ResizeHandleHitNode;
					this.handleHit = this.Controller.ResizeHandleHit;

					if (this.nodeHit != null)
					{
						this.SetCursor(this.handleHit);
					}
					else
					{
						this.RestoreCursor();
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
			if (this.Enabled && this.Active)
			{
				this.Controller.View.DrawTrackingRect(this.curBounds);

				Point ptCur = this.Controller.View.SnapPointToGrid(e.X, e.Y);
				this.curBounds = MoveAnchorPoint(this.startBounds, this.handleHit, ptCur);

				System.Drawing.Size szDev = new System.Drawing.Size(this.curBounds.Width - this.startBounds.Width, this.curBounds.Height - this.startBounds.Height);
				SizeF szWorld = this.Controller.View.ViewToWorld(this.Controller.View.DeviceToView(szDev));

				ResizeCmd resizeCmd = new ResizeCmd(szWorld.Width, szWorld.Height, this.handleHit);
				resizeCmd.Nodes.Concat(this.Controller.SelectionList);
				this.Controller.ExecuteCommand(resizeCmd);

				this.Controller.DeactivateTool(this);
				this.Controller.Cursor = Cursors.Default;
			}
		}

		private static System.Drawing.Rectangle MoveAnchorPoint(System.Drawing.Rectangle rect, BoxPosition posAnchor, Point ptAnchor)
		{
			int left = 0;
			int top = 0;
			int width = 0;
			int height = 0;

			switch (posAnchor)
			{
				case BoxPosition.TopLeft:
					left = ptAnchor.X;
					top = ptAnchor.Y;
					width = rect.Right - left;
					height = rect.Bottom - top;
					break;

				case BoxPosition.TopCenter:
					left = rect.Left;
					top = ptAnchor.Y;
					width = rect.Width;
					height = rect.Bottom - top;
					break;

				case BoxPosition.TopRight:
					left = rect.Left;
					top = ptAnchor.Y;
					width = ptAnchor.X - rect.Left;
					height = rect.Bottom - top;
					break;

				case BoxPosition.MiddleLeft:
					left = ptAnchor.X;
					top = rect.Top;
					width = rect.Right - left;
					height = rect.Height;
					break;

				case BoxPosition.MiddleRight:
					left = rect.Left;
					top = rect.Top;
					width = ptAnchor.X - left;
					height = rect.Height;
					break;

				case BoxPosition.BottomRight:
					left = rect.Left;
					top = rect.Top;
					width = ptAnchor.X - left;
					height = ptAnchor.Y - top;
					break;

				case BoxPosition.BottomCenter:
					left = rect.Left;
					top = rect.Top;
					width = rect.Width;
					height = ptAnchor.Y - top;
					break;

				case BoxPosition.BottomLeft:
					left = ptAnchor.X;
					top = rect.Top;
					width = rect.Right - left;
					height = ptAnchor.Y - top;
					break;
			}

			return new System.Drawing.Rectangle(left, top, width, height);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handlePos"></param>
		private void SetCursor(BoxPosition handlePos)
		{
			switch (handlePos)
			{
				case BoxPosition.TopCenter:
				case BoxPosition.BottomCenter:
					ChangeCursor(Cursors.SizeNS);
					break;

				case BoxPosition.MiddleLeft:
				case BoxPosition.MiddleRight:
					ChangeCursor(Cursors.SizeWE);
					break;

				case BoxPosition.TopLeft:
				case BoxPosition.BottomRight:
					ChangeCursor(Cursors.SizeNWSE);
					break;

				case BoxPosition.TopRight:
				case BoxPosition.BottomLeft:
					ChangeCursor(Cursors.SizeNESW);
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
#if false
		protected ISelectable nodeHit = null;
#else
		protected INode nodeHit = null;
#endif

		/// <summary>
		/// 
		/// </summary>
		protected BoxPosition handleHit;

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Rectangle startBounds;

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Rectangle curBounds;
	}
}
