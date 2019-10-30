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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Property editor for line endpoints.
	/// </summary>
	/// <remarks>
	/// This custom property editor displays a combobox that allows
	/// the user to select from a list of endpoints.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineDecoratorListBox"/>
	/// </remarks>
	public class LineEndpointEditor : UITypeEditor
	{
		private IWindowsFormsEditorService edSvc = null;
		private Line paintLine = new Line();

		/// <summary>
		/// 
		/// </summary>
		public LineEndpointEditor()
		{
			this.paintLine.LineStyle.LineWidth = 1.0f;
			this.paintLine.LineStyle.LineColor = Color.Black;
			this.paintLine.LineStyle.DashStyle = DashStyle.Solid;
			this.paintLine.LineStyle.EndCap = LineCap.Flat;
			this.paintLine.LineStyle.LineJoin = LineJoin.Bevel;
			this.paintLine.LineStyle.MiterLimit = 10.0f;
			this.paintLine.LineStyle.DashStyle = DashStyle.Solid;
			this.paintLine.LineStyle.DashCap = DashCap.Flat;
			this.paintLine.LineStyle.DashOffset = 0.0f;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			this.edSvc = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
			if (this.edSvc != null)
			{
				LineDecoratorListBox decoratorCtl = new LineDecoratorListBox();
				decoratorCtl.AddDecorator(typeof(FilledArrowDecorator));
				decoratorCtl.AddDecorator(typeof(OpenArrowDecorator));
				decoratorCtl.AddDecorator(typeof(CircleDecorator));
				decoratorCtl.AddDecorator(typeof(DiamondDecorator));
				decoratorCtl.SelectedIndexChanged += new System.EventHandler(OnValueSelected);
				if (value != null)
				{
					decoratorCtl.SelectedLineDecorator = value.GetType();
				}
				edSvc.DropDownControl(decoratorCtl);

				if (decoratorCtl != null)
				{
					System.Type decoratorType = decoratorCtl.SelectedLineDecorator;
					if (decoratorType == null)
					{
						value = null;
					}
					else
					{
						value = Activator.CreateInstance(decoratorType);
					}
				}
			}
			return value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public override void PaintValue(PaintValueEventArgs e)
		{
			object value = e.Value;
			if (value != null)
			{
				LineDecorator lineDecorator = value as LineDecorator;

				if (lineDecorator != null)
				{
					float lineY = e.Bounds.Top + (e.Bounds.Height / 2.0f);
					PointF[] pts = new PointF[2];
					pts[0] = new PointF(e.Bounds.Left, lineY);
					pts[1] = new PointF(e.Bounds.Left + (e.Bounds.Width / 2), lineY);
					this.paintLine.SetPoints(pts);
					LineDecorator paintDecorator = (LineDecorator) lineDecorator.Clone();
					if (paintDecorator != null)
					{
						FilledLineDecorator filledPaintDecorator = paintDecorator as FilledLineDecorator;
						if (filledPaintDecorator != null)
						{
							filledPaintDecorator.FillStyle.Type = FillStyle.FillType.Solid;
							filledPaintDecorator.FillStyle.Color = Color.Black;
						}
						this.paintLine.LastEndPoint = paintDecorator;
						int prevStack = Global.SelectMatrixStack(Global.RenderingStack);
						Global.MatrixStack.Clear();
						Global.MatrixStack.Push(e.Graphics.Transform);
						this.paintLine.Draw(e.Graphics);
						Global.MatrixStack.Pop();
						Global.SelectMatrixStack(prevStack);
					}
				}
			}
		}

		private void OnValueSelected(object sender, System.EventArgs evtArgs)
		{
			this.edSvc.CloseDropDown();
		}
	}
}
