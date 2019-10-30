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
using System.Diagnostics;

using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Diagram;

namespace Syncfusion.Windows.Forms.Diagram.Controls
{
	/// <summary>
	/// Displays a collection of symbol palettes the symbol models they contain
	/// in a GroupBar control.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class is derived from Syncfusion.Windows.Forms.Tools.GroupBar and
	/// provides an implementation for displaying symbol palettes and the symbol
	/// models they contain. Each symbol palette corresponds to a single panel
	/// (i.e. GroupView) inside of the GroupBar. Each entry in a panel corresponds
	/// to a symbol model inside of a symbol palette.
	/// </para>
	/// <para>
	/// The user interface looks and behaves like an Outlook bar. Each symbol
	/// palette is a list of symbols that have an icon and a label. If the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar.EditMode"/>
	/// flag is false, the PaletteGroupBar allows symbols to be dragged from
	/// this control onto diagrams.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView"/>
	/// </remarks>
	public class PaletteGroupBar : GroupBar
	{
		private System.ComponentModel.IContainer components = null;
		private bool editMode = false;

		/// <summary>
		/// Construct a PaletteGroupBar
		/// </summary>
		public PaletteGroupBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
		/// Determines whether or not the symbols can be dragged from the palette
		/// onto a diagram.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When this property is set to true, the palette is being edited and symbols
		/// cannot be dragged onto a diagram. The Symbol Designer sets this flag to true.
		/// The typical setting for an application is false, which means that the palette
		/// is not being editing and symbols can be dragged onto a diagram.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		Category("Behavior"),
		Description("Determines whether or not symbols can be dragged from the palette onto a diagram")
		]
		public bool EditMode
		{
			get
			{
				return this.editMode;
			}
			set
			{
				this.editMode = value;
			}
		}

		/// <summary>
		/// Removes all symbol palettes from the GroupBar.
		/// </summary>
		public void Clear()
		{
			while (this.GroupBarItems.Count > 0)
			{
				this.GroupBarItems.Remove(this.GroupBarItems[0]);
			}
		}

		/// <summary>
		/// Adds an existing symbol palette to the GroupBar.
		/// </summary>
		/// <param name="palette">Symbol palette to add</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// </remarks>
		public void AddPalette(SymbolPalette palette)
		{
			GroupBarItem paletteBarItem = new GroupBarItem();
			paletteBarItem.Text = palette.Name;
			paletteBarItem.Tag = palette;

			PaletteGroupView paletteView = new PaletteGroupView();
			paletteView.EditMode = this.editMode;
			paletteView.ButtonView = true;
			paletteView.BackColor = Color.Ivory;
			paletteView.SymbolModelSelected += new SymbolModelEvent(OnSymbolModelSelected);
			paletteView.SymbolModelDoubleClick += new SymbolModelEvent(OnSymbolModelDoubleClick);

			paletteView.LoadPalette(palette);
			paletteBarItem.Client = paletteView;
			this.GroupBarItems.Add(paletteBarItem);
		}

		/// <summary>
		/// Adds a new symbol palette to the GroupBar with the given name.
		/// </summary>
		/// <param name="paletteName">Name of symbol palette to create</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// </remarks>
		public SymbolPalette AddPalette(string paletteName)
		{
			SymbolPalette palette = new SymbolPalette();
			palette.Name = paletteName;
			AddPalette(palette);
			return palette;
		}

		/// <summary>
		/// Adds a new symbol palette to the GroupBar after prompting the user for
		/// the name of the new symbol palette to create.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controls.PaletteAddDlg"/>
		/// </remarks>
		public SymbolPalette AddPalette()
		{
			SymbolPalette palette = null;
			PaletteAddDlg dlg = new PaletteAddDlg();
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				palette = AddPalette(dlg.PaletteName);
			}
			return palette;
		}

		/// <summary>
		/// The currently selected symbol palette
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public SymbolPalette CurrentPalette
		{
			get
			{
				SymbolPalette curPalette = null;
				if (this.SelectedItem >= 0 && this.SelectedItem < this.GroupBarItems.Count)
				{
					curPalette = this.GroupBarItems[this.SelectedItem].Tag as SymbolPalette;
				}
				return curPalette;
			}
		}

		/// <summary>
		/// The number of symbol palettes loaded in the GroupBar control
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int PaletteCount
		{
			get
			{
				return this.GroupBarItems.Count;
			}
		}

		/// <summary>
		/// Returns the symbol palette at the given index.
		/// </summary>
		/// <param name="paletteIdx">Zero-based index into the collection of symbol palettes loaded into the GroupBar control</param>
		/// <returns>SymbolPalette object or null if paletteIdx parameter is out of range</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>
		/// </remarks>
		public SymbolPalette GetPalette(int paletteIdx)
		{
			SymbolPalette palette = null;

			if (paletteIdx < GroupBarItems.Count)
			{
				palette = GroupBarItems[paletteIdx].Tag as SymbolPalette;
			}

			return palette;
		}

		/// <summary>
		/// Returns the currently selected symbol model
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public SymbolModel SelectedSymbolModel
		{
			get
			{
				SymbolModel symbolMdl = null;

				if (this.SelectedItem < this.GroupBarItems.Count)
				{
					PaletteGroupView curGroupView = this.GroupBarItems[this.SelectedItem].Client as PaletteGroupView;
					if (curGroupView != null)
					{
						symbolMdl = curGroupView.SelectedSymbolModel;
					}
				}

				return symbolMdl;
			}
		}

		/// <summary>
		/// Set the selected symbol model to the one matching the given symbol model name.
		/// </summary>
		/// <param name="symbolName">Name of symbol model to select</param>
		public bool SelectSymbolModel(string symbolName)
		{
			bool found = false;

			if (this.SelectedItem < this.GroupBarItems.Count)
			{
				PaletteGroupView curGroupView = this.GroupBarItems[this.SelectedItem].Client as PaletteGroupView;
				if (curGroupView != null)
				{
					found = curGroupView.SelectSymbolModel(symbolName);
				}
			}

			return found;
		}

		/// <summary>
		/// Fired when the user selects a symbol model icon in a PaletteGroupBar component.
		/// </summary>
		public event SymbolModelEvent SymbolModelSelected;

		/// <summary>
		/// Fired when the user double clicks a symbol model icon in a PaletteGroupBar component.
		/// </summary>
		public event SymbolModelEvent SymbolModelDoubleClick;

		private void OnSymbolModelSelected(object sender, SymbolModelEventArgs evtArgs)
		{
			if (this.SymbolModelSelected != null)
			{
				this.SymbolModelSelected(sender, evtArgs);
			}
		}

		/// <summary>
		/// Internal event handler that is wired to the GroupView.DoubleClick event.
		/// </summary>
		/// <param name="sender">Object that sent the event</param>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// This event handler fires the SymbolModelDoubleClick event.
		/// </remarks>
		private void OnSymbolModelDoubleClick(object sender, SymbolModelEventArgs evtArgs)
		{
			if (this.SymbolModelDoubleClick != null)
			{
				this.SymbolModelDoubleClick(sender, evtArgs);
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
