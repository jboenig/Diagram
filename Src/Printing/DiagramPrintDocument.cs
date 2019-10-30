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
using System.Drawing.Printing;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Print document object for a diagram.
	/// </summary>
	public class DiagramPrintDocument : PrintDocument
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="printObj"></param>
		public DiagramPrintDocument(IPrint printObj)
		{
			this.printObj = printObj;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected override void OnPrintPage(PrintPageEventArgs evtArgs)
		{
			if (this.printObj != null)
			{
				System.Drawing.Graphics grfx = evtArgs.Graphics;

				if (grfx.PageUnit == GraphicsUnit.Pixel)
				{
					// Scale from screen resolution to printer resolution???
				}
				this.printObj.PrintPage(evtArgs);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected override void OnQueryPageSettings(QueryPageSettingsEventArgs evtArgs)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		private IPrint printObj = null;
	}
}
