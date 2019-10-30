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
using System.Windows.Forms;
using System.Drawing;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// TextBox control for editing text objects.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a text box control that is used for editing
	/// text nodes derived from
	/// <see cref="Syncfusion.Windows.Forms.Diagram.TextBase"/>.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextBase"/>
	/// </remarks>
	public class TextEdit : System.Windows.Forms.TextBox
	{
		/// <summary>
		/// Constructs a TextEdit given a parent control and text node.
		/// </summary>
		/// <param name="parentControl">Parent control</param>
		/// <param name="textObj">Text node to edit</param>
		public TextEdit(Control parentControl, TextBase textObj)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.textObj = textObj;
			this.Parent = parentControl;
		}

		/// <summary>
		/// Loads and positions the text edit control and goes into edit mode.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method loads the control with the text value and properties of the
		/// attached text node. It also positions the control to corresponding to
		/// bounds of the text node. Then it hides the text node and makes the
		/// control visible.
		/// </para>
		/// </remarks>
		public void BeginEdit()
		{
			if (this.textObj == null)
			{
				throw new EInvalidOperation();
			}

			// Hide the text node
			this.textObjVisible = this.textObj.Visible;
			this.textObj.Visible = false;

			// Turn off resize handles
			this.textObjAllowResize = this.textObj.EditStyle.AllowResize;
			this.textObj.EditStyle.AllowResize = false;

			// Load control with the text value of the attached node
			this.Text = this.textObj.Text;

			// Set the bounds of the control to match the node
			RectangleF rcTextBounds = this.textObj.Bounds;
			this.Location = new Point((int) Math.Round(rcTextBounds.Left), (int) Math.Round(rcTextBounds.Top));
			this.Size = new Size((int) Math.Round(rcTextBounds.Width), (int) Math.Round(rcTextBounds.Height));

			// If text node wraps text, then enable multi-line support in the control
			this.Multiline = this.textObj.WrapText;

			// Load the correct font into the control
			this.Font = this.textObj.FontStyle.CreateFont();

			// Show and enable the control
			this.Visible = true;
			this.Enabled = true;
		}

		/// <summary>
		/// Saves the changes made in the control to the attached text node and
		/// ends edit mode.
		/// </summary>
		/// <param name="saveChanges">Flag indicating if changes should be saved</param>
		public void EndEdit(bool saveChanges)
		{
			this.Enabled = false;
			this.Visible = false;

			if (this.textObj != null)
			{
				if (saveChanges)
				{
					this.textObj.Text = this.Text;
				}
				this.textObj.Visible = this.textObjVisible;
				this.textObj.EditStyle.AllowResize = this.textObjAllowResize;
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// TextEdit
			// 
			this.AutoSize = false;
			this.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Enabled = false;
			this.Size = new System.Drawing.Size(100, 20);
			this.Visible = false;

		}
		#endregion

		private TextBase textObj = null;
		private bool textObjVisible = true;
		private bool textObjAllowResize = true;
	}
}
