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
	/// Interactive tool for grouping the currently selected nodes in a diagram.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.GroupCmd"/>
	/// </remarks>
	public class GroupTool : Tool
	{
		/// <summary>
		/// 
		/// </summary>
		public GroupTool() : base(Resources.Strings.Toolnames.Get("GroupTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public GroupTool(string name) : base(name)
		{
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
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			GroupCmd cmd = new GroupCmd();
			cmd.Nodes.Concat(this.Controller.SelectionList);
			this.Controller.ExecuteCommand(cmd);
			this.Controller.DeactivateTool(this);
		}
	}
}
