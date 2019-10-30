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
	/// Interactive tool for inserting text objects into a diagram.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextNode"/>
	/// </remarks>
	public class TextTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public TextTool() : base(Resources.Strings.Toolnames.Get("TextTool"))
		{
			this.drawing = false;
			this.defaultText = "";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public TextTool(string name) : base(name)
		{
		}

		/// <summary>
		/// Default text value assigned to new text nodes created by this tool.
		/// </summary>
		public string DefaultText
		{
			get
			{
				return this.defaultText;
			}
			set
			{
				this.defaultText = value;
			}
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
					this.startingPoint = new System.Drawing.Point(e.X, e.Y);
					this.drawing = true;
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
				this.Controller.View.Draw();
				this.Controller.View.DrawTrackingRect(this.startingPoint, new System.Drawing.Point(e.X, e.Y));
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
				System.Drawing.Point ptEnd = new System.Drawing.Point(e.X, e.Y);

				int left = this.startingPoint.X;
				int top = this.startingPoint.Y;
				int width = e.X - left;
				int height = e.Y - top;

				if (width <= 0)
				{
					left = e.X;
					width = this.startingPoint.X - left;
				}

				if (height <= 0)
				{
					top = e.Y;
					height = this.startingPoint.Y - top;
				}

				Syncfusion.Windows.Forms.Diagram.TextNode textNode = new Syncfusion.Windows.Forms.Diagram.TextNode(this.defaultText);
				textNode.Location = new PointF(left, top);
				textNode.Size = new SizeF(width, height);

				InsertNodesCmd cmd = new InsertNodesCmd();
				cmd.Nodes.Add(textNode);
				this.Controller.ExecuteCommand(cmd);

				this.Controller.EditText(textNode);

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
		private bool drawing;

		/// <summary>
		/// 
		/// </summary>
		private string defaultText;
	}
}
