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
using System.Windows.Forms;

using Syncfusion.Windows.Forms.Diagram;
using Syncfusion.Windows.Forms.Diagram.Controls;

namespace Syncfusion.SymbolDesigner
{
	/// <summary>
	/// Summary description for OptionsControl.
	/// </summary>
	public class OptionsControl : System.Windows.Forms.UserControl
	{
		private Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram = null;
		private bool controlsLocked = false;
		private bool diagramLocked = false;

		private System.Windows.Forms.TabControl tabControlOptions;
		private System.Windows.Forms.TabPage tabPageGrid;
		private System.Windows.Forms.GroupBox groupBoxSpacing;
		private System.Windows.Forms.NumericUpDown verticalGridSpacingCtl;
		private System.Windows.Forms.NumericUpDown horizontalGridSpacingCtl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBoxSnapToGrid;
		private System.Windows.Forms.CheckBox checkBoxGridVisible;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private System.Windows.Forms.CheckBox checkBoxShowPageBounds;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OptionsControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (this.diagram != null)
				{
					this.diagram.View.PropertyChanged -= new PropertyEventHandler(View_PropertyChanged);
				}

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// 
		/// </summary>
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
						this.diagram.View.PropertyChanged -= new PropertyEventHandler(View_PropertyChanged);
					}

					this.diagram = value;

					if (this.diagram != null)
					{
						this.UpdateControls();
						this.diagram.View.PropertyChanged += new PropertyEventHandler(View_PropertyChanged);
					}
				}
			}
		}

		private void View_PropertyChanged(object sender, PropertyEventArgs evtArgs)
		{
			this.UpdateControls();
		}

		private void UpdateControls()
		{
			this.diagramLocked = true;

			if (!this.controlsLocked && this.diagram != null)
			{
				this.checkBoxGridVisible.Checked = this.Diagram.View.Grid.Visible;
				this.checkBoxSnapToGrid.Checked = this.Diagram.View.Grid.SnapToGrid;
				this.horizontalGridSpacingCtl.Value = System.Convert.ToDecimal(this.diagram.View.Grid.HorizontalSpacing);
				this.verticalGridSpacingCtl.Value = System.Convert.ToDecimal(this.diagram.View.Grid.VerticalSpacing);
				this.checkBoxShowPageBounds.Checked = this.Diagram.View.ShowPageBounds;
			}

			this.diagramLocked = false;
		}

		private void UpdateDiagram()
		{
			this.controlsLocked = true;

			if (!this.diagramLocked && this.diagram != null)
			{
				this.Diagram.View.Grid.Visible = this.checkBoxGridVisible.Checked;
				this.Diagram.View.Grid.SnapToGrid = this.checkBoxSnapToGrid.Checked;
				this.Diagram.View.Grid.HorizontalSpacing = System.Convert.ToSingle(this.horizontalGridSpacingCtl.Value);
				this.Diagram.View.Grid.VerticalSpacing = System.Convert.ToSingle(this.verticalGridSpacingCtl.Value);
				this.Diagram.View.ShowPageBounds = this.checkBoxShowPageBounds.Checked;
			}

			this.controlsLocked = false;
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControlOptions = new System.Windows.Forms.TabControl();
			this.tabPageGrid = new System.Windows.Forms.TabPage();
			this.checkBoxSnapToGrid = new System.Windows.Forms.CheckBox();
			this.checkBoxGridVisible = new System.Windows.Forms.CheckBox();
			this.groupBoxSpacing = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.horizontalGridSpacingCtl = new System.Windows.Forms.NumericUpDown();
			this.verticalGridSpacingCtl = new System.Windows.Forms.NumericUpDown();
			this.tabPageGeneral = new System.Windows.Forms.TabPage();
			this.checkBoxShowPageBounds = new System.Windows.Forms.CheckBox();
			this.tabControlOptions.SuspendLayout();
			this.tabPageGrid.SuspendLayout();
			this.groupBoxSpacing.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.horizontalGridSpacingCtl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.verticalGridSpacingCtl)).BeginInit();
			this.tabPageGeneral.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControlOptions
			// 
			this.tabControlOptions.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControlOptions.Controls.Add(this.tabPageGeneral);
			this.tabControlOptions.Controls.Add(this.tabPageGrid);
			this.tabControlOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlOptions.Location = new System.Drawing.Point(0, 0);
			this.tabControlOptions.Name = "tabControlOptions";
			this.tabControlOptions.SelectedIndex = 0;
			this.tabControlOptions.Size = new System.Drawing.Size(296, 208);
			this.tabControlOptions.TabIndex = 0;
			// 
			// tabPageGrid
			// 
			this.tabPageGrid.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageGrid.Controls.Add(this.groupBoxSpacing);
			this.tabPageGrid.Controls.Add(this.checkBoxGridVisible);
			this.tabPageGrid.Controls.Add(this.checkBoxSnapToGrid);
			this.tabPageGrid.Location = new System.Drawing.Point(4, 4);
			this.tabPageGrid.Name = "tabPageGrid";
			this.tabPageGrid.Size = new System.Drawing.Size(288, 182);
			this.tabPageGrid.TabIndex = 0;
			this.tabPageGrid.Text = "Grid";
			// 
			// checkBoxSnapToGrid
			// 
			this.checkBoxSnapToGrid.BackColor = System.Drawing.Color.Transparent;
			this.checkBoxSnapToGrid.Location = new System.Drawing.Point(152, 16);
			this.checkBoxSnapToGrid.Name = "checkBoxSnapToGrid";
			this.checkBoxSnapToGrid.Size = new System.Drawing.Size(88, 16);
			this.checkBoxSnapToGrid.TabIndex = 4;
			this.checkBoxSnapToGrid.Text = "Snap to Grid";
			this.checkBoxSnapToGrid.CheckedChanged += new System.EventHandler(this.ValueChanged);
			// 
			// checkBoxGridVisible
			// 
			this.checkBoxGridVisible.BackColor = System.Drawing.Color.Transparent;
			this.checkBoxGridVisible.Location = new System.Drawing.Point(24, 16);
			this.checkBoxGridVisible.Name = "checkBoxGridVisible";
			this.checkBoxGridVisible.Size = new System.Drawing.Size(88, 16);
			this.checkBoxGridVisible.TabIndex = 3;
			this.checkBoxGridVisible.Text = "Grid Visible";
			this.checkBoxGridVisible.CheckedChanged += new System.EventHandler(this.ValueChanged);
			// 
			// groupBoxSpacing
			// 
			this.groupBoxSpacing.BackColor = System.Drawing.Color.Transparent;
			this.groupBoxSpacing.Controls.Add(this.label2);
			this.groupBoxSpacing.Controls.Add(this.label1);
			this.groupBoxSpacing.Controls.Add(this.horizontalGridSpacingCtl);
			this.groupBoxSpacing.Controls.Add(this.verticalGridSpacingCtl);
			this.groupBoxSpacing.Location = new System.Drawing.Point(16, 48);
			this.groupBoxSpacing.Name = "groupBoxSpacing";
			this.groupBoxSpacing.Size = new System.Drawing.Size(232, 88);
			this.groupBoxSpacing.TabIndex = 2;
			this.groupBoxSpacing.TabStop = false;
			this.groupBoxSpacing.Text = "Grid Spacing";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(40, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Vertical:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(48, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Horizontal:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// horizontalGridSpacingCtl
			// 
			this.horizontalGridSpacingCtl.DecimalPlaces = 2;
			this.horizontalGridSpacingCtl.Location = new System.Drawing.Point(128, 24);
			this.horizontalGridSpacingCtl.Name = "horizontalGridSpacingCtl";
			this.horizontalGridSpacingCtl.Size = new System.Drawing.Size(56, 20);
			this.horizontalGridSpacingCtl.TabIndex = 3;
			this.horizontalGridSpacingCtl.ValueChanged += new System.EventHandler(this.ValueChanged);
			// 
			// verticalGridSpacingCtl
			// 
			this.verticalGridSpacingCtl.DecimalPlaces = 2;
			this.verticalGridSpacingCtl.Location = new System.Drawing.Point(128, 48);
			this.verticalGridSpacingCtl.Name = "verticalGridSpacingCtl";
			this.verticalGridSpacingCtl.Size = new System.Drawing.Size(56, 20);
			this.verticalGridSpacingCtl.TabIndex = 2;
			this.verticalGridSpacingCtl.ValueChanged += new System.EventHandler(this.ValueChanged);
			// 
			// tabPageGeneral
			// 
			this.tabPageGeneral.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageGeneral.Controls.Add(this.checkBoxShowPageBounds);
			this.tabPageGeneral.Location = new System.Drawing.Point(4, 4);
			this.tabPageGeneral.Name = "tabPageGeneral";
			this.tabPageGeneral.Size = new System.Drawing.Size(288, 182);
			this.tabPageGeneral.TabIndex = 1;
			this.tabPageGeneral.Text = "General";
			// 
			// checkBoxShowPageBounds
			// 
			this.checkBoxShowPageBounds.BackColor = System.Drawing.Color.Transparent;
			this.checkBoxShowPageBounds.Location = new System.Drawing.Point(16, 16);
			this.checkBoxShowPageBounds.Name = "checkBoxShowPageBounds";
			this.checkBoxShowPageBounds.Size = new System.Drawing.Size(136, 16);
			this.checkBoxShowPageBounds.TabIndex = 4;
			this.checkBoxShowPageBounds.Text = "Show Page Bounds";
			this.checkBoxShowPageBounds.CheckedChanged += new System.EventHandler(this.ValueChanged);
			// 
			// OptionsControl
			// 
			this.Controls.Add(this.tabControlOptions);
			this.Name = "OptionsControl";
			this.Size = new System.Drawing.Size(296, 208);
			this.tabControlOptions.ResumeLayout(false);
			this.tabPageGrid.ResumeLayout(false);
			this.groupBoxSpacing.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.horizontalGridSpacingCtl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.verticalGridSpacingCtl)).EndInit();
			this.tabPageGeneral.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ValueChanged(object sender, System.EventArgs e)
		{
			this.UpdateDiagram();
		}
	}
}
