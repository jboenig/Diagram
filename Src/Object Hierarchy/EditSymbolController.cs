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
	/// Implements a controller used for editing symbols.
	/// </summary>
	/// <remarks>
	/// This controller is tailored to editing symbols in the Symbol Designer
	/// utility. It registers the following tools:
	/// <see cref="Syncfusion.Windows.Forms.Diagram.MoveTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.ResizeTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LineTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.PolyLineTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.RectangleTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.ArcTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.PolygonTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.EllipseTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.CurveTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.ClosedCurveTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.PortTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.TextTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.ZoomTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.PanTool"/>,
	/// <see cref="Syncfusion.Windows.Forms.Diagram.SelectTool"/>
	/// </remarks>
	[Serializable]
	public class SymbolEditController : Controller
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SymbolEditController()
		{
		}

		/// <summary>
		/// Constructs a DiagramController and attaches it to get specified view.
		/// </summary>
		/// <param name="view"></param>
		public SymbolEditController(Syncfusion.Windows.Forms.Diagram.View view)
		{
			Initialize(view);
		}

		/// <summary>
		/// Initializes the controller for the given view.
		/// </summary>
		/// <param name="view">The view object to attach to the controller</param>
		public override void Initialize(Syncfusion.Windows.Forms.Diagram.View view)
		{
			base.Initialize(view);

			RegisterTool(new MoveTool());
			RegisterTool(new ResizeTool());
			RegisterTool(new LineTool());
			RegisterTool(new PolyLineTool());
			RegisterTool(new RectangleTool());
			RegisterTool(new ArcTool());
			RegisterTool(new PolygonTool());
			RegisterTool(new EllipseTool());
			RegisterTool(new CurveTool());
			RegisterTool(new ClosedCurveTool());
			RegisterTool(new PortTool());
			RegisterTool(new TextTool());
			RegisterTool(new ZoomTool());
			RegisterTool(new PanTool());
			RegisterTool(new SelectTool());
		}	
	}
}