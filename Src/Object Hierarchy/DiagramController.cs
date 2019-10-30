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
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Implements a generic diagram controller containing a default set
	/// of tools.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a concrete controller with a generic
	/// diagramming user interface. It overrides the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.Initialize"/>
	/// method and registers all available tools in the Syncfusion.Diagram.dll
	/// assembly.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// </remarks>
	[Serializable]
	//Licensing-DONT REMOVE//[LicenseProviderAttribute("Syncfusion.Licensing.FusionLicenseProvider, _FusionLic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=632609b4d040f6b4")]
	public class DiagramController : Controller
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DiagramController()
		{//[EvalPlaceholder]//
		}

		/// <summary>
		/// Constructs a DiagramController and attaches it to get specified view.
		/// </summary>
		/// <param name="view"></param>
		public DiagramController(Syncfusion.Windows.Forms.Diagram.View view) : base(view)
		{//[EvalPlaceholder]//
		}

		/// <summary>
		/// Initializes the controller for the given view.
		/// </summary>
		/// <param name="view">The view object to attach to the controller</param>
		/// <remarks>
		/// This method initializes the controller and registers all of the
		/// tools in the Essential Diagram library.
		/// </remarks>
		public override void Initialize(Syncfusion.Windows.Forms.Diagram.View view)
		{
			base.Initialize(view);

			RegisterTool(new MoveTool());
			RegisterTool(new ResizeTool());
			RegisterTool(new RotateTool());
			RegisterTool(new LineTool());
			RegisterTool(new OrthogonalLineTool());
			RegisterTool(new PolyLineTool());
			RegisterTool(new RectangleTool());
			RegisterTool(new RoundRectTool());
			RegisterTool(new ArcTool());
			RegisterTool(new PolygonTool());
			RegisterTool(new EllipseTool());
			RegisterTool(new CurveTool());
			RegisterTool(new ClosedCurveTool());
			RegisterTool(new PortTool());
			RegisterTool(new LinkTool());
			RegisterTool(new TextTool());
			RegisterTool(new ZoomTool());
			RegisterTool(new PanTool());
			RegisterTool(new GroupTool());
			RegisterTool(new UngroupTool());
			RegisterTool(new SelectTool());
			RegisterTool(new MetafileTool());
			RegisterTool(new BitmapTool());
			RegisterTool(new VertexEditTool());
		}	

		public override System.ComponentModel.ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				//Licensing-DONT REMOVE//if(this.DesignMode == true)LicenseManager.Validate(typeof(DiagramController));
			}
		}
	}
}