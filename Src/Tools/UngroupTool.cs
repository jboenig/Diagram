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
	/// Interactive tool for ungrouping the currently selected group in a diagram.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller.SelectionList"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.UngroupCmd"/>
	/// </remarks>
	public class UngroupTool : Tool
	{
		/// <summary>
		/// 
		/// </summary>
		public UngroupTool() : base(Resources.Strings.Toolnames.Get("UngroupTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public UngroupTool(string name) : base(name)
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
			UngroupCmd cmd = new UngroupCmd();
			cmd.Nodes.Concat(this.Controller.SelectionList);
			cmd.Do(this.Controller.Model);
			this.Deactivate();
		}
	}
}
