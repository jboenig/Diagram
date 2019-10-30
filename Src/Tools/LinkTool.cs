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
	/// Interactive tool for linking symbols together.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LinkCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveLinkCmd"/>
	/// </remarks>
	public class LinkTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public LinkTool() : base(Resources.Strings.Toolnames.Get("LinkTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public LinkTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public LinkFactory LinkFactory
		{
			set
			{
				this.linkFactory = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			this.ChangeCursor(Cursors.Cross);

			if (this.View != null)
			{
				Model mdl = this.View.Model;
				if (mdl != null)
				{
					mdl.SetPropertyValue("RevealPorts", true);
					this.View.Draw();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDeactivate()
		{
			if (this.View != null)
			{
				Model mdl = this.View.Model;
				if (mdl != null)
				{
					mdl.SetPropertyValue("RevealPorts", false);
					this.View.Draw();
				}
			}

			this.sourcePort = null;
			this.targetPort = null;
			this.tracking = false;
			this.trackingPts = null;
			this.drawing = false;
			this.points.Clear();
			base.OnDeactivate();
		}

		/// <summary>
		/// 
		/// </summary>
		public int HitTestSlop
		{
			get
			{
				return this.slop;
			}
			set
			{
				this.slop = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point ptScreen = new System.Drawing.Point(e.X, e.Y);

			if (e.Button == MouseButtons.Left)
			{
				if (this.Active)
				{
					this.drawing = true;
					this.tracking = false;
					if (this.trackingPts != null)
					{
						this.Controller.View.DrawTrackingLines(this.trackingPts);
					}
					this.trackingPts = null;
				}
				else
				{
					// Test to see if user clicked a port on a link
					if (this.HitTestLinkPorts(ptScreen))
					{
						IServiceProvider linkSvcProvider = this.selectedLink as IServiceProvider;
						if (linkSvcProvider != null)
						{
							IPoints linkPts = linkSvcProvider.GetService(typeof(IPoints)) as IPoints;
							if (linkPts != null)
							{
								this.points.Clear();
								System.Drawing.Point[] existingPts = this.View.GetDevicePoints(linkPts);
								if (existingPts != null)
								{
									for (int ptIdx = 0; ptIdx < existingPts.Length-1; ptIdx++)
									{
										this.points.Add(existingPts[ptIdx]);
									}
								}
								this.movingLinkPort = true;
								this.Controller.ActivateTool(this);
								this.tracking = true;
								this.InitTrackingPoints();
								if (this.trackingPts != null)
								{
									this.Controller.View.DrawTrackingLines(this.trackingPts);
								}
							}
						}
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
			System.Drawing.Point ptScreen = new System.Drawing.Point(e.X, e.Y);

			if (this.Active)
			{
				if (this.tracking)
				{
					// Erase existing tracking lines
					if (this.trackingPts != null)
					{
						this.Controller.View.DrawTrackingLines(this.trackingPts);
					}

					// Update last point in the array with current mouse position
					UpdateTrackingPoints(ptScreen);

					// Draw new tracking lines
					this.Controller.View.DrawTrackingLines(this.trackingPts);
				}

				// Change cursor to connect icon if mouse is over a port
				if (this.Controller.View.GetPortAt(ptScreen, this.slop) != null)
				{
					this.ChangeCursor(Resources.Cursors.Connect);
				}
				else
				{
					this.ChangeCursor(Cursors.Cross);
				}
			}
			else
			{
				// Check to see if mouse is over a port that can be moved
				if (this.HitTestLinkPorts(ptScreen))
				{
					this.ChangeCursor(Resources.Cursors.EditVertex);
				}
				else
				{
					this.RestoreCursor();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point ptScreen = new System.Drawing.Point(e.X, e.Y);

			if (this.Active)
			{
				Port portHit = this.Controller.View.GetPortAt(ptScreen, this.slop);

				if (portHit != null)
				{
					ptScreen = this.Controller.View.ViewToDevice(this.Controller.View.WorldToView(portHit.Location));

					if (this.sourcePort == null)
					{
						this.sourcePort = portHit;
					}
					else
					{
						this.targetPort = portHit;
					}
				}

				this.points.Add(ptScreen);

				if ((this.sourcePort != null && this.targetPort != null) || this.movingLinkPort)
				{
					// Source port and target port selected. Create the link.
					Point[] devicePts = (Point[]) this.points.ToArray(typeof(Point));
					PointF[] viewPts;
					PointF[] worldPts;
					this.Controller.View.DeviceToView(devicePts, out viewPts);
					this.Controller.View.ViewToWorld(viewPts, out worldPts);

					if (this.movingLinkPort)
					{
						MoveLinkCmd moveLinkCmd = new MoveLinkCmd();
						moveLinkCmd.Link = this.selectedLink;
						moveLinkCmd.LinkPort = this.portHit;
						moveLinkCmd.TargetPort = portHit;
						moveLinkCmd.Points = worldPts;
						this.Controller.ExecuteCommand(moveLinkCmd);
					}
					else
					{
						LinkCmd linkCmd = new LinkCmd(this.sourcePort, this.targetPort, worldPts);
						if (this.linkFactory != null)
						{
							linkCmd.LinkFactory = this.linkFactory;
						}
						this.Controller.ExecuteCommand(linkCmd);
					}

					this.Controller.DeactivateTool(this);
				}
				else
				{
					// Track mouse movements
					this.tracking = true;
					this.InitTrackingPoints();
					if (this.trackingPts != null)
					{
						this.Controller.View.DrawTrackingLines(this.trackingPts);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void InitTrackingPoints()
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt"></param>
		private void UpdateTrackingPoints(Point pt)
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
		/// <param name="ptScreen"></param>
		/// <returns></returns>
		private bool HitTestLinkPorts(System.Drawing.Point ptScreen)
		{
			bool hit = false;

			this.selectedLink = null;
			this.portHit = null;

			// Check to see if mouse is over a port that can be moved
			Controller controller = this.Controller;
			if (controller != null)
			{
				View view = controller.View;
				if (view != null)
				{
					if (controller.SelectionList.Count == 1)
					{
						this.selectedLink = controller.SelectionList[0] as Link;
						if (this.selectedLink != null)
						{
							PointF ptWorld = view.ViewToWorld(view.DeviceToView(ptScreen));
							int prevStack = Global.SelectMatrixStack(Global.TemporaryStack);
							Global.MatrixStack.Clear();
							Global.MatrixStack.Push(this.selectedLink.WorldTransform);
							this.portHit = this.selectedLink.GetPortAt(ptWorld, this.slop);
							Global.MatrixStack.Clear();
							Global.SelectMatrixStack(prevStack);

							if (this.portHit != null)
							{
								System.Type portType = this.portHit.GetType();

								if (portType.IsSubclassOf(typeof(LinkPort)))
								{
									hit = true;
								}
							}
						}
					}
				}
			}

			return hit;
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

		/// <summary>
		/// 
		/// </summary>
		protected bool movingLinkPort = false;

		/// <summary>
		/// 
		/// </summary>
		protected Port sourcePort = null;

		/// <summary>
		/// 
		/// </summary>
		protected Port targetPort = null;

		/// <summary>
		/// 
		/// </summary>
		protected int slop = 5;

		/// <summary>
		/// 
		/// </summary>
		private LinkFactory linkFactory = null;

		/// <summary>
		/// 
		/// </summary>
		protected Link selectedLink = null;

		/// <summary>
		/// 
		/// </summary>
		protected Port portHit = null;
	}
}
