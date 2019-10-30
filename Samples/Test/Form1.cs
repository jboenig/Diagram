using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Test
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram1;
		private Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar paletteGroupBar1;
		private Syncfusion.Windows.Forms.Tools.GroupBarItem groupBarItem1;
		private Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView paletteGroupView1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.diagram1 = new Syncfusion.Windows.Forms.Diagram.Controls.Diagram();
			this.paletteGroupBar1 = new Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar();
			this.paletteGroupView1 = new Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView();
			this.groupBarItem1 = new Syncfusion.Windows.Forms.Tools.GroupBarItem();
			this.paletteGroupBar1.SuspendLayout();
			this.SuspendLayout();
			// 
			// diagram1
			// 
			this.diagram1.AllowDrop = true;
			// 
			// diagram1.Controller
			// 
			this.diagram1.Controller.MaxHistory = 256;
			this.diagram1.Controller.SelectHandleMode = Syncfusion.Windows.Forms.Diagram.SelectHandleType.Resize;
			this.diagram1.HScroll = true;
			this.diagram1.LayoutManager = null;
			this.diagram1.Location = new System.Drawing.Point(152, 8);
			// 
			// diagram1.Model
			// 
			this.diagram1.Model.BoundaryConstraintsEnabled = true;
			this.diagram1.Model.Height = 1056F;
			this.diagram1.Model.MeasurementScale = 1F;
			this.diagram1.Model.MeasurementUnits = System.Drawing.GraphicsUnit.Pixel;
			this.diagram1.Model.Name = "Model";
			this.diagram1.Model.Width = 816F;
			this.diagram1.Name = "diagram1";
			this.diagram1.NudgeIncrement = 1F;
			this.diagram1.ScrollGranularity = 0.5F;
			this.diagram1.Size = new System.Drawing.Size(440, 480);
			this.diagram1.TabIndex = 0;
			this.diagram1.Text = "diagram1";
			// 
			// diagram1.View
			// 
			this.diagram1.View.BackgroundColor = System.Drawing.Color.DarkGray;
			this.diagram1.View.Grid.Color = System.Drawing.Color.IndianRed;
			this.diagram1.View.Grid.HorizontalSpacing = 10F;
			this.diagram1.View.Grid.SnapToGrid = true;
			this.diagram1.View.Grid.VerticalSpacing = 10F;
			this.diagram1.View.Grid.Visible = true;
			this.diagram1.View.HandleAnchorColor = System.Drawing.Color.LightGray;
			this.diagram1.View.HandleColor = System.Drawing.Color.White;
			this.diagram1.View.HandleSize = 6;
			this.diagram1.View.ShowPageBounds = true;
			this.diagram1.VScroll = true;
			// 
			// paletteGroupBar1
			// 
			this.paletteGroupBar1.AllowDrop = true;
			this.paletteGroupBar1.Controls.Add(this.paletteGroupView1);
			this.paletteGroupBar1.EditMode = false;
			this.paletteGroupBar1.GroupBarItems.AddRange(new Syncfusion.Windows.Forms.Tools.GroupBarItem[] {
																											   this.groupBarItem1});
			this.paletteGroupBar1.Location = new System.Drawing.Point(8, 16);
			this.paletteGroupBar1.Name = "paletteGroupBar1";
			this.paletteGroupBar1.SelectedItem = 0;
			this.paletteGroupBar1.Size = new System.Drawing.Size(136, 368);
			this.paletteGroupBar1.TabIndex = 1;
			this.paletteGroupBar1.Text = "paletteGroupBar1";
			// 
			// paletteGroupView1
			// 
			this.paletteGroupView1.EditMode = false;
			this.paletteGroupView1.Location = new System.Drawing.Point(2, 23);
			this.paletteGroupView1.Name = "paletteGroupView1";
			this.paletteGroupView1.SelectedItem = 0;
			this.paletteGroupView1.Size = new System.Drawing.Size(132, 343);
			this.paletteGroupView1.TabIndex = 0;
			this.paletteGroupView1.Text = "paletteGroupView1";
			this.paletteGroupView1.LoadPalette(typeof(Test.Form1).Assembly, "Test.Form1", "paletteGroupView1.Palette");
			// 
			// groupBarItem1
			// 
			this.groupBarItem1.Client = this.paletteGroupView1;
			this.groupBarItem1.Text = "GroupBarItem0";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(600, 494);
			this.Controls.Add(this.paletteGroupBar1);
			this.Controls.Add(this.diagram1);
			this.Name = "Form1";
			this.Text = "Dude";
			this.paletteGroupBar1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
	}
}
