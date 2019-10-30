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
//     Created: April 2003
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interactive tool for inserting bitmaps into a diagram.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BitmapNode"/>
	/// </remarks>
	public class BitmapTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public BitmapTool() : base(Resources.Strings.Toolnames.Get("BitmapTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public BitmapTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public System.Drawing.Bitmap Bitmap
		{
			get
			{
				return this.bitmap;
			}
			set
			{
				this.bitmap = value;
				if (this.bitmap != null)
				{
					RectangleF rcBounds = this.bitmap.GetBounds(ref this.pageUnit);
					this.trackingRect = new System.Drawing.Rectangle((int) rcBounds.Left, (int) rcBounds.Top, (int) rcBounds.Width, (int) rcBounds.Height);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool DragSize
		{
			get
			{
				return this.dragSize;
			}
			set
			{
				this.dragSize = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			IDataObject dataObj = Clipboard.GetDataObject();
			if (dataObj != null && dataObj.GetDataPresent(typeof(System.Drawing.Bitmap)))
			{
				this.Bitmap = (System.Drawing.Bitmap) dataObj.GetData(typeof(System.Drawing.Bitmap));
			}

			if (this.bitmap != null)
			{
				if (this.dragSize)
				{
					ChangeCursor(Cursors.Cross);
				}
				else
				{
					ChangeCursor(Cursors.SizeAll);
					this.drawing = true;
				}
				this.eraseTracking = false;
			}
			else
			{
				this.Controller.DeactivateTool(this);
			}
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
					if (this.dragSize && !this.drawing)
					{
						this.startingPoint = new System.Drawing.Point(e.X, e.Y);
						this.drawing = true;
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
			if (this.Active && this.drawing)
			{
				System.Drawing.Point ptCur = new System.Drawing.Point(e.X, e.Y);

				if (this.eraseTracking)
				{
					this.Controller.View.DrawTrackingRect(this.trackingRect);
				}

				if (this.dragSize)
				{
					this.trackingRect = Geometry.CreateRect(this.startingPoint, ptCur);
				}
				else
				{
					int trackingWidth = this.trackingRect.Width;
					int trackingHeight = this.trackingRect.Height;
					int trackingLeft = ptCur.X - (trackingWidth / 2);
					int trackingTop = ptCur.Y - (trackingHeight / 2);
					this.trackingRect = new System.Drawing.Rectangle(trackingLeft, trackingTop, trackingWidth, trackingHeight);
				}

				this.Controller.View.DrawTrackingRect(this.trackingRect);
				this.eraseTracking = true;
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
				System.Drawing.Point ptCur = new System.Drawing.Point(e.X, e.Y);
				System.Drawing.Rectangle rcDevice;
				
				if (this.dragSize)
				{
					rcDevice = Geometry.CreateRect(this.startingPoint, ptCur);
				}
				else
				{
					int trackingWidth = this.trackingRect.Width;
					int trackingHeight = this.trackingRect.Height;
					int trackingLeft = ptCur.X - (trackingWidth / 2);
					int trackingTop = ptCur.Y - (trackingHeight / 2);
					rcDevice = new System.Drawing.Rectangle(trackingLeft, trackingTop, trackingWidth, trackingHeight);
				}

				if (this.Bitmap != null)
				{
					RectangleF rcWorld = Controller.View.ViewToWorld(Controller.View.DeviceToView(rcDevice));
					BitmapNode BitmapNode = new BitmapNode(this.bitmap, rcWorld);
					InsertNodesCmd cmd = new InsertNodesCmd();
					cmd.Nodes.Add(BitmapNode);
					this.Controller.ExecuteCommand(cmd);
				}

				this.Controller.DeactivateTool(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private System.Drawing.Point startingPoint;

		/// <summary>
		/// 
		/// </summary>
		private bool drawing = false;

		/// <summary>
		/// 
		/// </summary>
		private bool dragSize = false;

		/// <summary>
		/// 
		/// </summary>
		private System.Drawing.Bitmap bitmap = null;

		/// <summary>
		/// 
		/// </summary>
		private System.Drawing.Rectangle trackingRect;

		/// <summary>
		/// 
		/// </summary>
		private bool eraseTracking = false;

		/// <summary>
		/// 
		/// </summary>
		private GraphicsUnit pageUnit = GraphicsUnit.Pixel;
	}
}