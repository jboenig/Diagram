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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for line decorators.
	/// </summary>
	[Serializable]
	public abstract class FilledLineDecorator : LineDecorator, IDeserializationCallback
	{
		/// <summary>
		/// 
		/// </summary>
		public FilledLineDecorator()
		{
			this.fillStyle = new FillStyle(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="line"></param>
		public FilledLineDecorator(INode line) : base(line)
		{
			this.fillStyle = new FillStyle(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public FilledLineDecorator(LineDecorator src) : base(src)
		{
			this.fillStyle = new FillStyle(this);
		}

		/// <summary>
		/// Serialization constructor for line decorators.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected FilledLineDecorator(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public FillStyle FillStyle
		{
			get
			{
				return this.fillStyle;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="grfx"></param>
		public override void Draw(System.Drawing.Graphics grfx)
		{
			GraphicsPath grfxPath = this.GraphicsPath;
			if (grfxPath != null)
			{
				GraphicsPath grfxDraw = (GraphicsPath) grfxPath.Clone();
				Matrix xform = Global.MatrixStack.Peek();
				grfxDraw.Transform(xform);
				Brush brush = this.fillStyle.CreateBrush();
				grfx.FillPath(brush, grfxDraw);
				brush.Dispose();
				Pen pen = this.LineStyle.CreatePen();
				grfx.DrawPath(pen, grfxDraw);
				pen.Dispose();
			}
		}

		/// <summary>
		/// Creates style objects for this class.
		/// </summary>
		protected override void CreateStyles()
		{
			base.CreateStyles();
			this.fillStyle = new FillStyle(this);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.CreateStyles();
		}

		/// <summary>
		/// 
		/// </summary>
		private FillStyle fillStyle = null;
	}
}