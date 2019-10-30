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
	/// Interactive tool for adding a symbol to the diagram.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// </remarks>
	public class InsertSymbolTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public InsertSymbolTool() : base(Resources.Strings.Toolnames.Get("InsertSymbolTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public InsertSymbolTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="symbolType"></param>
		public InsertSymbolTool(string name, System.Type symbolType) : base(name)
		{
			this.symbolType = symbolType;
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool Exclusive
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Symbol to insert.
		/// </summary>
		public Syncfusion.Windows.Forms.Diagram.Symbol Symbol
		{
			get
			{
				return this.symbol;
			}
			set
			{
				this.symbol = value;
			}
		}

		/// <summary>
		/// Type of symbol to insert.
		/// </summary>
		public System.Type SymbolType
		{
			get
			{
				return this.symbolType;
			}
			set
			{
				this.symbolType = value;
				this.symbol = null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			if (this.symbol == null && this.symbolType != null)
			{
				this.symbol = Activator.CreateInstance(this.symbolType) as Symbol;
			}

			if (this.symbol == null)
			{
				// Bail out since there's no symbol to insert
				this.Controller.DeactivateTool(this);
			}
			else
			{
				this.ChangeCursor(Cursors.SizeAll);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDeactivate()
		{
			this.symbol = null;
			base.OnDeactivate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active && e.Button == MouseButtons.Left)
			{
				System.Drawing.Point devInsPt = new System.Drawing.Point(e.X, e.Y);
				System.Drawing.PointF worldInsPt = this.View.ViewToWorld(this.View.DeviceToView(devInsPt));
				worldInsPt.X = worldInsPt.X - (this.symbol.Width / 2.0f);
				worldInsPt.Y = worldInsPt.Y - (this.symbol.Height / 2.0f);

				if (this.tracker != null)
				{
					this.tracker.Erase();
					this.tracker = null;
				}

				InsertNodesCmd insCmd = new InsertNodesCmd();
				insCmd.Nodes.Add(this.symbol);
				insCmd.Location = worldInsPt;
				this.Controller.ExecuteCommand(insCmd);

				this.Controller.DeactivateTool(this);
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
					this.tracker.Move(e.X - this.startingPt.X, e.Y - this.startingPt.Y);
				}
				else
				{
					this.startingPt = new System.Drawing.Point(e.X, e.Y);
					System.Drawing.PointF worldStartingPt = this.View.ViewToWorld(this.View.DeviceToView(this.startingPt));
					RectangleF symbolBounds = this.symbol.Bounds;
					System.Drawing.PointF curCenter = Geometry.CenterPoint(symbolBounds);
					ITransform symTransform = this.symbol as ITransform;
					if (symTransform != null)
					{
						symTransform.Translate(worldStartingPt.X - curCenter.X, worldStartingPt.Y - curCenter.Y);
					}
					this.tracker = new NodeBoundsTracker(this.View, this.symbol);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
		}

		private Syncfusion.Windows.Forms.Diagram.Symbol symbol = null;
		private System.Type symbolType = null;
		private System.Drawing.Point startingPt;
		private NodeTracker tracker = null;
	}
}
