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
	/// Interactive tool for drawing rectangles.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RectangleToolBase"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Rectangle"/>
	/// </remarks>
	public class RectangleTool : RectangleToolBase
	{
		/// <summary>
		/// 
		/// </summary>
		public RectangleTool() : base(Resources.Strings.Toolnames.Get("RectangleTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public RectangleTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnComplete()
		{
			Syncfusion.Windows.Forms.Diagram.Rectangle rectNode = new Syncfusion.Windows.Forms.Diagram.Rectangle(this.rect);
			InsertNodesCmd cmd = new InsertNodesCmd();
			cmd.Nodes.Add(rectNode);
			this.Controller.ExecuteCommand(cmd);
		}
	}
}