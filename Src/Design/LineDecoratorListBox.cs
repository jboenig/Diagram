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
using System.Drawing.Drawing2D;
using System.Collections;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// This class implements a ListBox that contains a list of line
	/// decorators that the user can choose from.
	/// </summary>
	/// <remarks>
	/// The items in the list are an actual image of the endpoints that
	/// the user can choose. Each item in the list is associated with
	/// a System.Type, which must be a LineDecorator derived class.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineEndpointEditor"/>
	/// </remarks>
	public class LineDecoratorListBox : System.Windows.Forms.ListBox
	{
		private System.ComponentModel.IContainer components = null;
		private Line itemLine = new Line();

		private class DecoratorEntry
		{
			public DecoratorEntry(System.Type decoratorType)
			{
				this.decoratorType = decoratorType;
			}

			public override string ToString()
			{
				return decoratorType.Name;
			}

			public System.Type DecoratorType
			{
				get
				{
					return this.decoratorType;
				}
			}

			private System.Type decoratorType = null;
		}

		/// <summary>
		/// 
		/// </summary>
		public LineDecoratorListBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.itemLine.LineStyle.LineWidth = 1.0f;
			this.itemLine.LineStyle.LineColor = Color.Black;
			this.itemLine.LineStyle.DashStyle = DashStyle.Solid;
			this.itemLine.LineStyle.EndCap = LineCap.Flat;
			this.itemLine.LineStyle.LineJoin = LineJoin.Bevel;
			this.itemLine.LineStyle.MiterLimit = 10.0f;
			this.itemLine.LineStyle.DashStyle = DashStyle.Solid;
			this.itemLine.LineStyle.DashCap = DashCap.Flat;
			this.itemLine.LineStyle.DashOffset = 0.0f;

			this.DrawMode = DrawMode.OwnerDrawFixed;
			this.Items.Add("(None)");
			this.SelectedIndex = 0;
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
		public System.Type SelectedLineDecorator
		{
			get
			{
				if (this.SelectedIndex == 0)
				{
					return null;
				}
				object selectedItem = this.SelectedItem;
				if (selectedItem != null)
				{
					if (selectedItem.GetType() == typeof(DecoratorEntry))
					{
						return ((DecoratorEntry) selectedItem).DecoratorType;
					}
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.SelectedIndex = 0;
				}
				else
				{
					for (int itemIdx = 0; itemIdx < this.Items.Count; itemIdx++)
					{
						DecoratorEntry curEntry = this.Items[itemIdx] as DecoratorEntry;
						if (curEntry != null && curEntry.DecoratorType == value)
						{
							this.SelectedIndex = itemIdx;
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="decoratorType"></param>
		public void AddDecorator(System.Type decoratorType)
		{
			if (decoratorType.IsSubclassOf(typeof(LineDecorator)))
			{
				this.Items.Add(new DecoratorEntry(decoratorType));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			object item = this.Items[e.Index];
			if (item != null)
			{
				e.DrawBackground();
				e.DrawFocusRectangle();

				Font font = new Font("Arial", 8);

				if (item.GetType() == typeof(string))
				{
					e.Graphics.DrawString((string)item, font, System.Drawing.Brushes.Black, e.Bounds, StringFormat.GenericDefault);
				}
				else if (item.GetType() == typeof(DecoratorEntry))
				{
					float lineY = e.Bounds.Top + (e.Bounds.Height / 2.0f);
					PointF[] pts = new PointF[2];
					pts[0] = new PointF(e.Bounds.Left + 10, lineY);
					pts[1] = new PointF(e.Bounds.Right - 10, lineY);
					this.itemLine.SetPoints(pts);
					System.Type decoratorType = ((DecoratorEntry) item).DecoratorType;
					LineDecorator decorator = (LineDecorator) Activator.CreateInstance(decoratorType);
					if (decorator != null)
					{
						FilledLineDecorator filledDecorator = decorator as FilledLineDecorator;
						if (filledDecorator != null)
						{
							filledDecorator.FillStyle.Type = FillStyle.FillType.Solid;
							filledDecorator.FillStyle.Color = Color.Black;
						}
						this.itemLine.LastEndPoint = decorator;
						this.itemLine.Draw(e.Graphics);
					}
				}
			}
			base.OnDrawItem(e);
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
