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
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Draws grid points onto a view at specified intervals.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class renders a grid of points onto the view at intervals specified by
	/// the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid.HorizontalSpacing"/>
	/// and
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid.VerticalSpacing"/>
	/// properties.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid"/>
	/// </remarks>
	public class PointGrid : LayoutGrid
	{
		/// <summary>
		/// Constructs a PointGrid.
		/// </summary>
		/// <param name="containerView">View in which to draw the grid</param>
		public PointGrid(View containerView) : base(containerView)
		{
		}

		/// <summary>
		/// Constructs a PointGrid.
		/// </summary>
		/// <param name="containerView">View in which to draw the grid</param>
		/// <param name="propContainer"></param>
		public PointGrid(View containerView, IPropertyContainer propContainer) : base(containerView, propContainer)
		{
		}

		/// <summary>
		/// Renders the grid onto a specified System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Drawing context object</param>
		/// <remarks>
		/// <para>
		/// Sets pixel values directly on the view's drawing surface.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View.DrawingSurface"/>
		/// </remarks>
		public override void Draw(System.Drawing.Graphics grfx)
		{
			View containerView = this.ContainerView;

			if (containerView != null && containerView.Model != null && this.Visible)
			{
				RectangleF rcWorld = containerView.Model.Bounds;
				Bitmap bmpDraw = containerView.DrawingSurface;
				SizeF spacing = this.Spacing;
				int minPixelSpacing = this.MinPixelSpacing;
				System.Drawing.Color color = this.Color;
				int surfaceWidth = bmpDraw.Width;
				int surfaceHeight = bmpDraw.Height;

				PointF upperLeft = new PointF(rcWorld.Location.X + spacing.Width, rcWorld.Location.Y + spacing.Height);
				upperLeft = containerView.ViewToDeviceF(containerView.WorldToView(upperLeft));
				float upperLeftX = upperLeft.X;
				float upperLeftY = upperLeft.Y;

				SizeF spacingDevice = containerView.ViewToDeviceF(containerView.WorldToView(spacing));
				float spacingWidth = spacingDevice.Width;
				float spacingHeight = spacingDevice.Height;

				if (spacingWidth < minPixelSpacing || spacingHeight < minPixelSpacing)
				{
					return;
				}

				float containerViewRight = (float) containerView.Location.X + containerView.Size.Width;
				float containerViewBottom = (float) containerView.Location.Y + containerView.Size.Height;

				while (upperLeftX < 0 && upperLeftX < containerViewRight)
				{
					upperLeftX += spacingWidth;
				}

				while (upperLeftY < 0 && upperLeftY < containerViewBottom)
				{
					upperLeftY += spacingHeight;
				}

				PointF lowerRight = new PointF(rcWorld.Location.X + rcWorld.Width, rcWorld.Location.Y + rcWorld.Height);
				lowerRight = containerView.ViewToDeviceF(containerView.WorldToView(lowerRight));
				float lowerRightX = lowerRight.X;
				float lowerRightY = lowerRight.Y;

				if (lowerRightX > containerViewRight)
				{
					lowerRightX = containerViewRight;
				}

				if (lowerRightY > containerViewBottom)
				{
					lowerRightY = containerViewBottom;
				}

				float curX = upperLeftX;
				float curY = upperLeftY;
				int x, y;

				while (curX < lowerRightX)
				{
					while (curY < lowerRightY)
					{
						x = (int) curX;
						y = (int) curY;
						if (x < surfaceWidth && y < surfaceHeight)
						{
							bmpDraw.SetPixel(x, y, color);
						}
						curY += spacingHeight;
					}
					curX += spacingWidth;
					curY = upperLeftY;
				}
			}
		}
	}
}
