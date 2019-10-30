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

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Syncfusion.Windows.Forms.Diagram;

namespace Syncfusion.Windows.Forms.Diagram.Controls
{
	/// <summary>
	/// This control displays and edits properties of objects in a diagram.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This control contains an embedded System.Windows.Forms.PropertyGrid
	/// that is used to edit objects in a diagram. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor.Diagram"/>
	/// contains a reference to a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram"/>
	/// object that the property editor is attached to. Once it is attached to
	/// a diagram, the property editor automatically displays the currently
	/// selected object in the diagram.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.Diagram"/>
	/// </remarks>
	public class PropertyEditor : System.Windows.Forms.Panel
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.PropertyGrid propGrid;
		private Diagram diagram = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PropertyEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Create embedded property grid and configure it
			this.propGrid = new PropertyGrid();
			this.Controls.Add(this.propGrid);
			this.propGrid.Dock = DockStyle.Fill;
		}

		/// <summary>
		/// Selects the list of nodes in the property grid.
		/// </summary>
		/// <param name="nodes">Nodes to display in the property editor</param>
		public void SetSelectedObjects(NodeCollection nodes)
		{
			if (nodes.Count > 0)
			{
				this.propGrid.SelectedObject = nodes[0];
			}
			else
			{
				this.propGrid.SelectedObject = null;
			}
		}

		/// <summary>
		/// Select the node in the property grid.
		/// </summary>
		/// <param name="node">Node to display in the property editor</param>
		public void SetSelectedObject(INode node)
		{
			this.propGrid.SelectedObject = node;
		}

		/// <summary>
		/// Reference to the PropertyGrid object contained by this property editor.
		/// </summary>
		public System.Windows.Forms.PropertyGrid PropertyGrid
		{
			get
			{
				return this.propGrid;
			}
		}

		/// <summary>
		/// Diagram the property editor is attached to.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property contains a reference to the Diagram that this property
		/// editor is attached to. The property editor receives events from the
		/// diagram when the current selection changes and it updates the currently
		/// displayed object in the property editor.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public Syncfusion.Windows.Forms.Diagram.Controls.Diagram Diagram
		{
			get
			{
				return this.diagram;
			}
			set
			{
				if (this.diagram != value)
				{
					if (this.diagram != null)
					{
						this.diagram.SelectionChanged -= new NodeCollection.EventHandler(Diagram_SelectionChanged);
					}

					this.diagram = value;

					if (this.diagram != null)
					{
						this.diagram.SelectionChanged += new NodeCollection.EventHandler(Diagram_SelectionChanged);

						if (!this.DesignMode)
						{
							this.SetSelectedObject(this.diagram.Model);
						}
					}
				}
			}
		}

		private void Diagram_SelectionChanged(object sender, NodeCollection.EventArgs evtArgs)
		{
			if (!this.DesignMode)
			{
				if (evtArgs.Nodes.Length == 0 || evtArgs.Nodes[0] == null)
				{
					this.propGrid.SelectedObject = this.diagram.Model;
				}
				else
				{
					this.propGrid.SelectedObject = evtArgs.Nodes[0];
				}
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PropertyEditor));
		}
		#endregion
	}
}
