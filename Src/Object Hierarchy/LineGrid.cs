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
	/// Draws grid lines onto a view at specified intervals.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class renders a grid of lines onto the view at intervals specified by
	/// the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid.HorizontalSpacing"/>
	/// and
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid.VerticalSpacing"/>
	/// properties.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid"/>
	/// </remarks>
	public class LineGrid : LayoutGrid
	{
		/// <summary>
		/// Constructs a LineGrid.
		/// </summary>
		/// <param name="containerView">View in which to draw the grid</param>
		public LineGrid(View containerView) : base(containerView)
		{
		}

		/// <summary>
		/// Constructs a LineGrid.
		/// </summary>
		/// <param name="containerView">View in which to draw the grid</param>
		/// <param name="propContainer"></param>
		public LineGrid(View containerView, IPropertyContainer propContainer) : base(containerView, propContainer)
		{
		}

		/// <summary>
		/// Renders the grid onto a specified System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Drawing context object</param>
		public override void Draw(System.Drawing.Graphics grfx)
		{
			View containerView = this.ContainerView;

			if (containerView != null && containerView.Model != null && this.Visible)
			{
				// Determine the visible rectangle in which to draw the grid
				RectangleF rcWorld = containerView.Model.Bounds;
				RectangleF rcView = containerView.ViewToWorld(containerView.Bounds);
				RectangleF rcVisible = rcView;
				rcVisible.Intersect(rcWorld);
				float visibleLeft = rcVisible.Left;
				float visibleTop = rcVisible.Top;
				float visibleRight = rcVisible.Right;
				float visibleBottom = rcVisible.Bottom;

				// Cache spacing values
				float horizontalSpacing = this.Spacing.Width;
				float verticalSpacing = this.Spacing.Height;

				// Create pen to draw lines
				Pen pen = new Pen(this.Color, 0.0f);
				pen.DashStyle = this.DashStyle;

				// Draw vertical lines

				int startingOffsetY = (int) ((rcView.Top - rcWorld.Top) / verticalSpacing) + 1;
				if (startingOffsetY <= 0)
				{
					startingOffsetY = 1;
				}

				float curY = rcWorld.Top + (startingOffsetY * verticalSpacing);
				while (curY < visibleBottom)
				{
					grfx.DrawLine(pen, visibleLeft, curY, visibleRight, curY);
					curY += verticalSpacing;
				}

				// Draw horizontal lines

				int startingOffsetX = (int) ((rcView.Left - rcWorld.Left) / horizontalSpacing) + 1;
				if (startingOffsetX <= 0)
				{
					startingOffsetX = 1;
				}

				float curX = rcWorld.Left + (startingOffsetX * horizontalSpacing);

				while (curX < visibleRight)
				{
					grfx.DrawLine(pen, curX, visibleTop, curX, visibleBottom);
					curX += horizontalSpacing;
				}

				pen.Dispose();
			}
		}
	}
}
