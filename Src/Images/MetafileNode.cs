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
	/// Stores and renders an enhanced metafile image.
	/// </summary>
	public class MetafileNode : ImageNode
	{
		/// <summary>
		/// 
		/// </summary>
		public MetafileNode()
		{
			this.metafile = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public MetafileNode(System.IO.Stream stream)
		{
			this.metafile = new System.Drawing.Imaging.Metafile(stream);
			this.Bounds = this.metafile.GetBounds(ref this.grfxUnit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		public MetafileNode(string filename)
		{
			this.metafile = new System.Drawing.Imaging.Metafile(filename);
			this.Bounds = this.metafile.GetBounds(ref this.grfxUnit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="metafile"></param>
		public MetafileNode(System.Drawing.Imaging.Metafile metafile)
		{
			this.metafile = metafile;
			this.Bounds = this.metafile.GetBounds(ref this.grfxUnit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="metafile"></param>
		/// <param name="bounds"></param>
		/// <param name="grfxUnit"></param>
		public MetafileNode(System.Drawing.Imaging.Metafile metafile, RectangleF bounds, System.Drawing.GraphicsUnit grfxUnit)
		{
			this.metafile = metafile;
			this.Bounds = bounds;
			this.grfxUnit = grfxUnit;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public MetafileNode(MetafileNode src) : base(src)
		{
			this.metafile = (System.Drawing.Imaging.Metafile) src.metafile.Clone();
			this.grfxUnit = src.grfxUnit;
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new MetafileNode(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public override System.Drawing.Image Image
		{
			get
			{
				return this.metafile;
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

		private System.Drawing.Imaging.Metafile metafile = null;
		private System.Drawing.GraphicsUnit grfxUnit = GraphicsUnit.Pixel;
	}
}
