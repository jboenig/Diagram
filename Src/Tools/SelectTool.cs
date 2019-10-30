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
	/// Interactive tool for changing the current selected nodes in a diagram.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>
	/// </remarks>
	public class SelectTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public SelectTool() : base(Resources.Strings.Toolnames.Get("SelectTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public SelectTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool Exclusive
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			ChangeCursor(Cursors.Arrow);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDeactivate()
		{
			this.dragging = false;
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
				this.Controller.ActivateTool(this);
				this.startingPoint = new System.Drawing.Point(e.X, e.Y);

				if (this.Controller.NodesHit.Count == 0)
				{
					this.Controller.SelectionList.Clear();
					this.dragging = true;
				}
				else
				{
					if (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control || this.Controller.SelectionList.Count == 0)
					{
						this.Controller.SelectionList.Add(this.Controller.NodesHit[0]);
						this.Controller.View.Draw();
						this.Controller.DeactivateTool(this);
					}
					else if (!this.Controller.SelectionList.Contains(this.Controller.NodesHit[0]))
					{
						this.Controller.SelectionList.Clear();
						this.Controller.SelectionList.Add(this.Controller.NodesHit[0]);
						this.Controller.View.Draw();
						this.Controller.DeactivateTool(this);
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
			if (this.Active && this.dragging)
			{
				Point ptScreen = new Point(e.X, e.Y);

				if (e.Button == MouseButtons.Left)
				{
					Controller.View.Draw();
					Controller.View.DrawTrackingRect(this.startingPoint, ptScreen);
				}
				else
				{
					this.Controller.DeactivateTool(this);
					this.dragging = false;
					this.Controller.View.Draw();
				}
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
				Point ptScreen = new Point(e.X, e.Y);

				if (this.dragging)
				{
					if (this.mode == SelectMode.Intersecting)
					{
						this.Controller.View.GetNodesIntersecting(this.Controller.SelectionList, Geometry.CreateRect(this.startingPoint, ptScreen));
					}
					else
					{
						this.Controller.View.GetNodesContainedBy(this.Controller.SelectionList, Geometry.CreateRect(this.startingPoint, ptScreen));
					}
					this.Controller.View.Draw();
				}
				else if (this.Controller.NodesHit.Count > 0)
				{
					this.Controller.SelectionList.Clear();
					this.Controller.SelectionList.Add(this.Controller.NodesHit[0]);
					this.Controller.View.Draw();
				}
				this.Controller.DeactivateTool(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public enum SelectMode
		{
			/// <summary>
			/// 
			/// </summary>
			Intersecting,
			/// <summary>
			/// 
			/// </summary>
			Containing
		}

		/// <summary>
		/// 
		/// </summary>
		protected System.Drawing.Point startingPoint;

		/// <summary>
		/// 
		/// </summary>
		protected bool dragging = false;

		/// <summary>
		/// 
		/// </summary>
		protected SelectMode mode = SelectMode.Containing;
	}
}
