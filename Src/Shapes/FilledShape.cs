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
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for filled shapes.
	/// </summary>
	public class FilledShape : Shape, ILogicalUnitContainer, IDeserializationCallback
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public FilledShape()
		{
			this.fillStyle = new FillStyle(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public FilledShape(FilledShape src) : base(src)
		{
			this.fillStyle = new FillStyle(this);
		}

		/// <summary>
		/// Serialization constructor for filled shapes.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected FilledShape(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.fillStyle = new FillStyle(this);
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new FilledShape(this);
		}

		#endregion

		#region Styles

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		TypeConverter(typeof(FillStyleConverter)),
		Category("Appearance"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public FillStyle FillStyle
		{
			get
			{
				return this.fillStyle;
			}
		}

		#endregion

		#region IBounds2DF interface

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public override RectangleF Bounds
		{
			get
			{
				return base.Bounds;
			}
			set
			{
				base.Bounds = value;
				RectangleF gradientBounds = new RectangleF(0,0,value.Width, value.Height);
				this.fillStyle.GradientBounds = gradientBounds;
			}
		}

		#endregion

		#region ILogicalUnitContainer interface

		/// <summary>
		/// Converts the logical values contained by the object from one unit of
		/// measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="grfx">Graphics context object for converting device units</param>
		/// <remarks>
		/// <para>
		/// This method converts all logical unit values contained by the object from
		/// one unit of measure to another.
		/// </para>
		/// </remarks>
		void ILogicalUnitContainer.ConvertLogicalUnits(GraphicsUnit fromUnits, GraphicsUnit toUnits, Graphics grfx)
		{
			// Convert line width
			if (this.propertyValues.Contains("LineWidth"))
			{
				float lineWidth = (float) this.propertyValues["LineWidth"];
				lineWidth = Measurements.Convert(fromUnits, toUnits, grfx, lineWidth);
				this.propertyValues["LineWidth"] = lineWidth;
			}

			// Convert the gradient bounds
			if (this.propertyValues.Contains("GradientBounds"))
			{
				System.Drawing.RectangleF gradientBounds = (System.Drawing.RectangleF) this.propertyValues["GradientBounds"];
				gradientBounds = Measurements.Convert(fromUnits, toUnits, grfx, gradientBounds);
				this.propertyValues["GradientBounds"] = gradientBounds;
			}

			// Convert points
			System.Drawing.PointF[] pts = this.GetPoints();
			for (int ptIdx = 0; ptIdx < pts.Length; ptIdx++)
			{
				pts[ptIdx] = Measurements.Convert(fromUnits, toUnits, grfx, pts[ptIdx]);
			}
			this.SetPoints(pts);
		}

		/// <summary>
		/// Converts the logical values contained by the object from one scale to
		/// another.
		/// </summary>
		/// <param name="fromScale">Scale to convert from</param>
		/// <param name="toScale">Scale to convert to</param>
		/// <remarks>
		/// <para>
		/// This method scales all logical unit values contained by the object.
		/// </para>
		/// </remarks>
		void ILogicalUnitContainer.ConvertLogicalScale(float fromScale, float toScale)
		{
		}

		#endregion

		#region IGraphics interface

		/// <summary>
		/// 
		/// </summary>
		/// <param name="grfx"></param>
		public override void Draw(System.Drawing.Graphics grfx)
		{
			if (this.Visible)
			{
				grfx.Transform = Global.ViewMatrix;
				Matrix xform = Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);
				GraphicsPath grfxPath = (GraphicsPath) this.grfxPath.Clone();
				grfxPath.Transform(xform);

				RectangleF fillBounds = this.grfxPath.GetBounds(xform);
				Brush brush = this.fillStyle.CreateBrush(fillBounds);
				grfx.FillPath(brush, grfxPath);
				brush.Dispose();

				Pen pen = this.LineStyle.CreatePen();
				grfx.DrawPath(pen, grfxPath);
				grfxPath.Dispose();
				pen.Dispose();

				Global.MatrixStack.Pop();
			}
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.CreateStyles();
		}

		#endregion

		#region Implementation methods

		/// <summary>
		/// Creates style objects for this class.
		/// </summary>
		protected override void CreateStyles()
		{
			base.CreateStyles();
			this.fillStyle = new FillStyle(this);
		}

		#endregion

		#region Fields

		/// <summary>
		/// 
		/// </summary>
		private FillStyle fillStyle = null;

		#endregion
	}
}
