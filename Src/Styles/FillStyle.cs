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
using System.Drawing.Design;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A FillStyle is a collection of properties that define a brush used for
	/// fill operations during rendering.
	/// </summary>
	public class FillStyle : Syncfusion.Windows.Forms.Diagram.Style
	{
		/// <summary>
		/// Types of brushes that can be created by a FillStyle.
		/// </summary>
		public enum FillType
		{
			/// <summary>
			/// Solid brush
			/// </summary>
			Solid,
			/// <summary>
			/// Text brush
			/// </summary>
			Texture,
			/// <summary>
			/// Linear gradient brush
			/// </summary>
			LinearGradient
		}

		/// <summary>
		/// Constructs a FillStyle object.
		/// </summary>
		/// <param name="propContainer">Property container to bind to</param>
		public FillStyle(IPropertyContainer propContainer) :
			base(propContainer)
		{
		}

		/// <summary>
		/// Color to use for the brush.
		/// </summary>
		/// <remarks>
		/// NOTE: If <see cref="Syncfusion.Windows.Forms.Diagram.FillStyle.Type"/> is
		/// set to FillType.LinearGradient, then this is the ending color for the
		/// gradient.
		/// </remarks>
		[
		Browsable(true)
		]
		public Color Color
		{
			get
			{
				object value = this.Properties.GetPropertyValue("FillColor");
				if (value == null)
				{
					return Color.Transparent;
				}
				return (Color) value;
			}
			set
			{
				this.Properties.SetPropertyValue("FillColor", value);

				if (this.Type == FillType.Solid)
				{
					this.Properties.SetPropertyValue("GradientStartColor", value);
				}
			}
		}

		/// <summary>
		/// Color to use at the beginning of a gradient, if
		/// <see cref="Syncfusion.Windows.Forms.Diagram.FillStyle.Type"/> is set to
		/// FillType.LinearGradient.
		/// </summary>
		[
		Browsable(true)
		]
		public Color GradientStartColor
		{
			get
			{
				object value = this.Properties.GetPropertyValue("GradientStartColor");
				if (value == null)
				{
					return this.Color;
				}
				return (Color) value;
			}
			set
			{
				this.Properties.SetPropertyValue("GradientStartColor", value);

				if (this.Type == FillType.Solid)
				{
					this.Properties.SetPropertyValue("FillColor", value);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		RefreshProperties(RefreshProperties.All)
		]
		public System.Drawing.Image Texture
		{
			get
			{
				object value = this.Properties.GetPropertyValue("FillTexture");
				if (value != null)
				{
					return (System.Drawing.Image) value;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Type = FillType.Texture;
				}
				this.Properties.SetPropertyValue("FillTexture", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true)
		]
		public System.Drawing.Drawing2D.WrapMode TextureWrapMode
		{
			get
			{
				object value = this.Properties.GetPropertyValue("FillTextureWrapMode");
				if (value != null)
				{
					return (System.Drawing.Drawing2D.WrapMode) value;
				}
				return System.Drawing.Drawing2D.WrapMode.Tile;
			}
			set
			{
				this.Properties.SetPropertyValue("FillTextureWrapMode", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false)
		]
		public RectangleF GradientBounds
		{
			get
			{
				return (RectangleF) this.Properties.GetPropertyValue("GradientBounds");
			}
			set
			{
				this.Properties.SetPropertyValue("GradientBounds", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true)
		]
		public float GradientAngle
		{
			get
			{
				object value = this.Properties.GetPropertyValue("GradientAngle");
				if (value == null)
				{
					return 0.0f;
				}
				return (float) value;
			}
			set
			{
				this.Properties.SetPropertyValue("GradientAngle", value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		RefreshProperties(RefreshProperties.All)
		]
		public FillType Type
		{
			get
			{
				object value = this.Properties.GetPropertyValue("FillType");

				if (value == null)
				{
					return FillType.Solid;
				}

				return (FillType) value;
			}
			set
			{
				this.Properties.SetPropertyValue("FillType", value);

				if (value == FillType.Solid)
				{
					this.GradientStartColor = this.Color;
					this.Texture = null;
				}

				if (value == FillType.LinearGradient)
				{
					this.Texture = null;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Brush CreateBrush()
		{
			Brush br = null;
			System.Drawing.Image texture = null;
			TextureBrush textureBrush = null;

			try
			{
				switch (this.Type)
				{
					case FillType.Solid:
						br = new SolidBrush(this.Color);
						break;

					case FillType.LinearGradient:
						br = new LinearGradientBrush(this.GradientBounds, this.GradientStartColor, this.Color, this.GradientAngle, true);
						break;

					case FillType.Texture:
						texture = this.Texture;
						if (texture != null)
						{
							textureBrush = new TextureBrush(this.Texture);
							textureBrush.WrapMode = this.TextureWrapMode;
							br = textureBrush;
						}
						else
						{
							br = new SolidBrush(this.Color);
						}
						break;
				}
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(e.Message);
				br = new SolidBrush(this.Color);
			}

			return br;
		}

		/// <summary>
		/// Creates a new brush based on the properties contained by
		/// the FillStyle object.
		/// </summary>
		/// <param name="fillBounds">
		/// Bounds of the object to be filled (needed for gradient
		/// brushes only)
		/// </param>
		/// <returns></returns>
		public Brush CreateBrush(RectangleF fillBounds)
		{
			Brush br = null;
			System.Drawing.Image texture = null;
			TextureBrush textureBrush = null;

			try
			{
				switch (this.Type)
				{
					case FillType.Solid:
						br = new SolidBrush(this.Color);
						break;

					case FillType.LinearGradient:
						br = new LinearGradientBrush(fillBounds, this.GradientStartColor, this.Color, this.GradientAngle, true);
						break;

					case FillType.Texture:
						texture = this.Texture;
						if (texture != null)
						{
							textureBrush = new TextureBrush(this.Texture);
							textureBrush.WrapMode = this.TextureWrapMode;
							br = textureBrush;
						}
						else
						{
							br = new SolidBrush(this.Color);
						}
						break;
				}
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(e.Message);
				br = new SolidBrush(this.Color);
			}

			return br;
		}
	}
}
