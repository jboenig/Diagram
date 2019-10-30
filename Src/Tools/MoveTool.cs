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
using System.Collections;

using Syncfusion.Windows.Forms;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interactive tool for moving nodes on a diagram.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveCmd"/>
	/// </remarks>
	public class MoveTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public MoveTool() : base(Resources.Strings.Toolnames.Get("MoveTool"))
		{
			this.ready = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public MoveTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			this.ChangeCursor(Cursors.SizeAll);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDeactivate()
		{
			this.ready = false;
			base.OnDeactivate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Enabled && e.Button == MouseButtons.Left)
			{
				this.startingPoint = new System.Drawing.Point(e.X, e.Y);
				this.ready = false;

				if (this.Controller.NodesHit.Count > 0 && this.Controller.ResizeHandleHitNode == null)
				{
					this.ready = true;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active)
			{
				if (this.tracker != null)
				{
					this.tracker.Erase();
					this.tracker.Move(e.X - this.startingPoint.X, e.Y - this.startingPoint.Y);
				}
			}
			else if (this.ready)
			{
				// Track the selected nodes as they move
				this.tracker = new NodePathTracker(this.Controller.View, this.GetAllowedNodes(this.Controller.SelectionList));
				this.tracker.Erase();
				this.tracker.Move(e.X - this.startingPoint.X, e.Y - this.startingPoint.Y);
				this.Controller.ActivateTool(this);
				this.ready = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			// Always reset ready flag when a mouse up occurs to ensure that
			// only a new mouse down event will start a move
			this.ready = false;

			if (this.Active)
			{
				System.Drawing.Point upperLeft = this.tracker.UpperLeft;
				bool validMove = this.tracker.IsValidPosition;
				this.tracker.Erase();
				this.tracker = null;

				if (validMove)
				{
					// Determine the move vector and generate a move command
					Point ptEnd = new Point(e.X, e.Y);
					System.Drawing.Size offsetDev = new System.Drawing.Size(e.X - this.startingPoint.X, e.Y - this.startingPoint.Y);

					if (this.View.Grid.SnapToGrid)
					{
						System.Drawing.Point newUpperLeft = new System.Drawing.Point(upperLeft.X + offsetDev.Width, upperLeft.Y + offsetDev.Height);
						System.Drawing.Point nearestGridPoint = this.View.SnapPointToGrid(newUpperLeft);
						offsetDev.Width = nearestGridPoint.X - upperLeft.X;
						offsetDev.Height = nearestGridPoint.Y - upperLeft.Y;
					}

					System.Drawing.SizeF offsetWorld = this.View.ViewToWorld(this.View.DeviceToView(offsetDev));
					MoveCmd moveCmd = new MoveCmd(offsetWorld.Width, offsetWorld.Height);
					moveCmd.Nodes.Concat(this.GetAllowedNodes(this.Controller.SelectionList));
					this.Controller.ExecuteCommand(moveCmd);

					// Update the view
					this.Controller.View.Draw();
				}

				this.Controller.DeactivateTool(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nodesIn"></param>
		/// <returns></returns>
		protected NodeCollection GetAllowedNodes(NodeCollection nodesIn)
		{
			NodeCollection nodesOut = new NodeCollection();

			IEnumerator enumNodesIn = nodesIn.GetEnumerator();
			while (enumNodesIn.MoveNext())
			{
				bool allowed = true;

				IPropertyContainer curProps = enumNodesIn.Current as IPropertyContainer;
				if (curProps != null)
				{
					allowed = (bool) curProps.GetPropertyValue("AllowMove");
				}

				if (allowed)
				{
					nodesOut.Add(enumNodesIn.Current as INode);
				}
			}

			return nodesOut;
		}

		/// <summary>
		/// 
		/// </summary>
		protected bool moving;

		/// <summary>
		/// 
		/// </summary>
		protected bool ready;

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point startingPoint;

		/// <summary>
		/// 
		/// </summary>
		protected NodeCollection nodesHit = new NodeCollection();

		/// <summary>
		/// 
		/// </summary>
		protected NodeCollection nodesMoving = new NodeCollection();

		/// <summary>
		/// 
		/// </summary>
		protected NodeTracker tracker = null;
	}
}
