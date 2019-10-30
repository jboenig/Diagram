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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Summary description for RoundRectTool.
	/// </summary>
	public class RoundRectTool : RectangleToolBase
	{
		/// <summary>
		/// Interactive tool for drawing rounded rectangles.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RectangleToolBase"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RoundRect"/>
		/// </remarks>
		public RoundRectTool() : base(Resources.Strings.Toolnames.Get("RoundRectTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public RoundRectTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnComplete()
		{
			Syncfusion.Windows.Forms.Diagram.RoundRect shape = new Syncfusion.Windows.Forms.Diagram.RoundRect(this.rect);
			InsertNodesCmd cmd = new InsertNodesCmd();
			cmd.Nodes.Add(shape);
			this.Controller.ExecuteCommand(cmd);
		}
	}
}
