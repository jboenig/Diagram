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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Syncfusion.Windows.Forms.Diagram;

namespace Syncfusion.Windows.Forms.Diagram.Controls
{
	/// <summary>
	/// Summary description for SymbolModelProperties.
	/// </summary>
	public class SymbolModelProperties : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label label2;
		private Syncfusion.Windows.Forms.Tools.ButtonEdit btnEditIcon;
		private Syncfusion.Windows.Forms.Tools.ButtonEditChildButton btnBrowse;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbPlugInAssembly;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbPlugInClass;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 
		/// </summary>
		public SymbolModelProperties()
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
		/// <param name="symbolMdl"></param>
		public void SetValues(SymbolModel symbolMdl)
		{
			tbName.Text = symbolMdl.Name;
			tbPlugInAssembly.Text = symbolMdl.PlugInAssembly;
			tbPlugInClass.Text = symbolMdl.PlugInClass;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="symbolMdl"></param>
		public void GetValues(SymbolModel symbolMdl)
		{
			symbolMdl.Name = tbName.Text;
			symbolMdl.PlugInAssembly = tbPlugInAssembly.Text;
			symbolMdl.PlugInClass = tbPlugInClass.Text;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SymbolModelProperties));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnEditIcon = new Syncfusion.Windows.Forms.Tools.ButtonEdit();
			this.btnBrowse = new Syncfusion.Windows.Forms.Tools.ButtonEditChildButton();
			this.label3 = new System.Windows.Forms.Label();
			this.tbPlugInAssembly = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbPlugInClass = new System.Windows.Forms.TextBox();
			this.btnEditIcon.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(120, 168);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 24);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(200, 168);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(48, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(128, 24);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(200, 20);
			this.tbName.TabIndex = 3;
			this.tbName.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(48, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Icon:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnEditIcon
			// 
			this.btnEditIcon.Buttons.Add(this.btnBrowse);
			this.btnEditIcon.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.btnBrowse,
																					  this.btnEditIcon.TextBox});
			this.btnEditIcon.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.btnEditIcon.Location = new System.Drawing.Point(128, 48);
			this.btnEditIcon.Name = "btnEditIcon";
			this.btnEditIcon.SelectionLength = 0;
			this.btnEditIcon.SelectionStart = 0;
			this.btnEditIcon.ShowTextBox = true;
			this.btnEditIcon.Size = new System.Drawing.Size(200, 22);
			this.btnEditIcon.TabIndex = 5;
			this.btnEditIcon.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			// 
			// btnEditIcon.TextBox
			// 
			this.btnEditIcon.TextBox.AutoSize = false;
			this.btnEditIcon.TextBox.Location = new System.Drawing.Point(1, 2);
			this.btnEditIcon.TextBox.Size = new System.Drawing.Size(182, 18);
			this.btnEditIcon.TextBox.TabIndex = 0;
			// 
			// btnBrowse
			// 
			this.btnBrowse.BackColor = System.Drawing.SystemColors.Control;
			this.btnBrowse.ButtonAlign = Syncfusion.Windows.Forms.Tools.ButtonAlignment.Right;
			this.btnBrowse.ButtonEditParent = this.btnEditIcon;
			this.btnBrowse.ButtonType = Syncfusion.Windows.Forms.Tools.ButtonTypes.Browse;
			this.btnBrowse.Image = ((System.Drawing.Bitmap)(resources.GetObject("btnBrowse.Image")));
			this.btnBrowse.Location = new System.Drawing.Point(184, 2);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.PreferredWidth = 16;
			this.btnBrowse.Size = new System.Drawing.Size(16, 18);
			this.btnBrowse.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "Plug In Assembly:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbPlugInAssembly
			// 
			this.tbPlugInAssembly.Location = new System.Drawing.Point(128, 72);
			this.tbPlugInAssembly.Name = "tbPlugInAssembly";
			this.tbPlugInAssembly.Size = new System.Drawing.Size(200, 20);
			this.tbPlugInAssembly.TabIndex = 7;
			this.tbPlugInAssembly.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 8;
			this.label4.Text = "Plug In Class:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbPlugInClass
			// 
			this.tbPlugInClass.Location = new System.Drawing.Point(128, 96);
			this.tbPlugInClass.Name = "tbPlugInClass";
			this.tbPlugInClass.Size = new System.Drawing.Size(200, 20);
			this.tbPlugInClass.TabIndex = 9;
			this.tbPlugInClass.Text = "";
			// 
			// SymbolModelProperties
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(362, 208);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tbPlugInClass,
																		  this.label4,
																		  this.tbPlugInAssembly,
																		  this.label3,
																		  this.btnEditIcon,
																		  this.label2,
																		  this.tbName,
																		  this.label1,
																		  this.btnCancel,
																		  this.btnOK});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SymbolModelProperties";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Symbol Model Properties";
			this.btnEditIcon.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
