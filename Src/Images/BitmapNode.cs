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
using System.Drawing.Imaging;
using System.IO;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Stores and renders a bitmap image.
	/// </summary>
	public class BitmapNode : ImageNode
	{
		/// <summary>
		/// 
		/// </summary>
		public BitmapNode()
		{
			this.bitmap = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public BitmapNode(System.IO.Stream stream)
		{
			this.bitmap = new System.Drawing.Bitmap(stream);
			this.Bounds = this.bitmap.GetBounds(ref this.grfxUnit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		public BitmapNode(string filename)
		{
			this.bitmap = new System.Drawing.Bitmap(filename);
			this.Bounds = this.bitmap.GetBounds(ref this.grfxUnit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public BitmapNode(System.Drawing.Bitmap src)
		{
			this.bitmap = src;
			this.Bounds = this.bitmap.GetBounds(ref this.grfxUnit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		/// <param name="rcBounds"></param>
		public BitmapNode(System.Drawing.Bitmap src, RectangleF rcBounds)
		{
			this.bitmap = src;
			this.Bounds = rcBounds;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public BitmapNode(BitmapNode src) : base(src)
		{
			this.bitmap = (System.Drawing.Bitmap) src.bitmap.Clone();
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new BitmapNode(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public override System.Drawing.Image Image
		{
			get
			{
				return this.bitmap;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override System.Drawing.GraphicsUnit GraphicsUnit
		{
			get
			{
				return this.grfxUnit;
			}
		}

		private System.Drawing.Bitmap bitmap = null;
		private System.Drawing.GraphicsUnit grfxUnit = GraphicsUnit.Pixel;
	}
}
