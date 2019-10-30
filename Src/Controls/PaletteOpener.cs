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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Collections;

namespace Syncfusion.Windows.Forms.Diagram.Controls
{
	/// <summary>
	/// This class implements a design-time editor for opening a file containing
	/// a symbol palette.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class is used by the PaletteGroupView to allow the user to load a
	/// symbol palette from disk. This class can be used as the design-time editor
	/// for any property of type SymbolPalette using the following code:
	/// <code>
	/// [Editor(typeof(PaletteOpener), typeof(System.Drawing.Design.UITypeEditor))]
	/// </code>
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
	/// </remarks>
	public class PaletteOpener : UITypeEditor
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object EditValue(
			ITypeDescriptorContext context, IServiceProvider provider, 
			object value) 
		{
			OpenFileDialog openDlg = new OpenFileDialog();
			openDlg.Filter = "Symbol palette files (*.edp)|*.edp|All files (*.*)|*.*";
			openDlg.DefaultExt = "edp";
			openDlg.Title = "Open symbol palette";
			if (openDlg.ShowDialog() == DialogResult.OK)
			{
				value = this.typeConverter.ConvertFrom(context, null, openDlg.FileName);
			}
			return value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private System.ComponentModel.TypeConverter typeConverter = new SymbolPaletteConverter();
	}
}
