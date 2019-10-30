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
using System.Drawing.Imaging;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interactive tool for inserting metafiles into a diagram.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MetafileNode"/>
	/// </remarks>
	public class MetafileTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public MetafileTool() : base(Resources.Strings.Toolnames.Get("MetafileTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public MetafileTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public System.Drawing.Imaging.Metafile Metafile
		{
			get
			{
				return this.metafile;
			}
			set
			{
				this.metafile = value;
				if (this.metafile != null)
				{
					RectangleF rcBounds = this.metafile.GetBounds(ref this.metafileUnits);
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
			if (dataObj != null && dataObj.GetDataPresent(typeof(System.Drawing.Imaging.Metafile)))
			{
				this.Metafile = (System.Drawing.Imaging.Metafile) dataObj.GetData(typeof(System.Drawing.Imaging.Metafile));
			}

			if (this.metafile != null)
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

				if (this.metafile != null)
				{
					RectangleF rcWorld = Controller.View.ViewToWorld(Controller.View.DeviceToView(rcDevice));
					MetafileNode metafileNode = new MetafileNode(this.metafile, rcWorld, GraphicsUnit.Pixel);
					InsertNodesCmd cmd = new InsertNodesCmd();
					cmd.Nodes.Add(metafileNode);
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
		private System.Drawing.Imaging.Metafile metafile = null;

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
		private System.Drawing.GraphicsUnit metafileUnits = GraphicsUnit.Pixel;
	}
}