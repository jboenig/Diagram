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
	/// Base class for objects that draw a grid onto a view.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Derived classes must override the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid.Draw"/>
	/// method in order to draw the grid.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDraw"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
	/// </remarks>
	public abstract class LayoutGrid : IDraw
	{
		/// <summary>
		/// Constructs a LayoutGrid.
		/// </summary>
		/// <param name="containerView">View in which to draw the grid</param>
		public LayoutGrid(View containerView)
		{
			this.containerView = containerView;
			this.propContainer = null;
		}

		/// <summary>
		/// Constructs a LayoutGrid.
		/// </summary>
		/// <param name="containerView">View in which to draw the grid</param>
		/// <param name="propContainer"></param>
		public LayoutGrid(View containerView, IPropertyContainer propContainer)
		{
			this.containerView = containerView;
			this.propContainer = propContainer;
		}

		/// <summary>
		/// The view this grid is attached to.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property is a reference to the view that the layout grid renders
		/// itself onto.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public View ContainerView
		{
			get
			{
				return this.containerView;
			}
			set
			{
				this.containerView = value;
			}
		}

		/// <summary>
		/// Reference to object that contains the grid property values.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The properties of the grid are stored in a separate property container.
		/// </para>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public IPropertyContainer PropertyContainer
		{
			get
			{
				return this.propContainer;
			}
			set
			{
				this.propContainer = value;
			}
		}

		/// <summary>
		/// Determines the distance between grid points.
		/// </summary>
		/// <remarks>
		/// Spacing is specified in world units.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public System.Drawing.SizeF Spacing
		{
			get
			{
				if (this.propContainer != null)
				{
					object value = this.propContainer.GetPropertyValue("GridSpacing");
					if (value != null && value.GetType() == typeof(System.Drawing.SizeF))
					{
						return (System.Drawing.SizeF) value;
					}
				}
				return new System.Drawing.SizeF(10.0f, 10.0f);
			}
			set
			{
				if (this.propContainer != null)
				{
					object curVal = this.propContainer.GetPropertyValue("GridSpacing");
					if (curVal == null || (curVal.GetType() == typeof(System.Drawing.SizeF) && (System.Drawing.SizeF) curVal != value))
					{
						this.propContainer.SetPropertyValue("GridSpacing", value);
					}
				}
			}
		}

		/// <summary>
		/// Determines the vertical distance between grid points.
		/// </summary>
		/// <remarks>
		/// Spacing is specified in world units.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Appearance"),
		Description("Determines the vertical distance between grid points")
		]
		public float VerticalSpacing
		{
			get
			{
				return this.Spacing.Height;
			}
			set
			{
				System.Drawing.SizeF spacing = this.Spacing;
				spacing.Height = value;
				this.Spacing = spacing;
			}
		}

		/// <summary>
		/// Determines the horizontal distance between grid points.
		/// </summary>
		/// <remarks>
		/// Spacing is specified in world units.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Appearance"),
		Description("Determines the horizontal distance between grid points")
		]
		public float HorizontalSpacing
		{
			get
			{
				return this.Spacing.Width;
			}
			set
			{
				System.Drawing.SizeF spacing = this.Spacing;
				spacing.Width = value;
				this.Spacing = spacing;
			}
		}

		/// <summary>
		/// Minimum spacing between grid points in device units.
		/// </summary>
		/// <remarks>
		/// This value specifies the threshold at which the grid will stop
		/// drawing itself because the spacing between grid points is too
		/// small.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int MinPixelSpacing
		{
			get
			{
				if (this.propContainer != null)
				{
					object value = this.propContainer.GetPropertyValue("MinimumGridSpacing");
					if (value != null)
					{
						return (int) value;
					}
				}
				return 4;
			}
			set
			{
				if (this.propContainer != null)
				{
					object curVal = this.propContainer.GetPropertyValue("MinimumGridSpacing");
					if (curVal == null || (curVal.GetType() == typeof(int) && (int) curVal != value))
					{
						this.propContainer.SetPropertyValue("MinimumGridSpacing", value);
					}
				}
			}
		}

		/// <summary>
		/// Controls whether or not the grid is visible.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public bool Visible
		{
			get
			{
				if (this.propContainer != null)
				{
					object value = this.propContainer.GetPropertyValue("GridVisible");
					if (value != null)
					{
						return (bool) value;
					}
				}
				return false;
			}
			set
			{
				if (this.propContainer != null)
				{
					object curVal = this.propContainer.GetPropertyValue("GridVisible");
					if (curVal == null || (curVal.GetType() == typeof(bool) && (bool) curVal != value))
					{
						this.propContainer.SetPropertyValue("GridVisible", value);
					}
				}
			}
		}

		/// <summary>
		/// Determines if the snap to grid feature is enable or disabled.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property determines how the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid.GetNearestGridPoint"/>
		/// method behaves.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid.GetNearestGridPoint"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public bool SnapToGrid
		{
			get
			{
				if (this.propContainer != null)
				{
					object value = this.propContainer.GetPropertyValue("SnapToGrid");
					if (value != null)
					{
						return (bool) value;
					}
				}
				return false;
			}
			set
			{
				if (this.propContainer != null)
				{
					object curVal = this.propContainer.GetPropertyValue("SnapToGrid");
					if (curVal == null || (curVal.GetType() == typeof(bool) && (bool) curVal != value))
					{
						this.propContainer.SetPropertyValue("SnapToGrid", value);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public System.Drawing.Color Color
		{
			get
			{
				if (this.propContainer != null)
				{
					object value = this.propContainer.GetPropertyValue("GridColor");
					if (value != null)
					{
						return (System.Drawing.Color) value;
					}
				}
				return Color.Black;
			}
			set
			{
				if (this.propContainer != null)
				{
					object curVal = this.propContainer.GetPropertyValue("GridColor");
					if (curVal == null || (curVal.GetType() == typeof(System.Drawing.Color) && (System.Drawing.Color) curVal != value))
					{
						this.propContainer.SetPropertyValue("GridColor", value);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public System.Drawing.Drawing2D.DashStyle DashStyle
		{
			get
			{
				if (this.propContainer != null)
				{
					object value = this.propContainer.GetPropertyValue("GridDashStyle");
					if (value != null)
					{
						return (System.Drawing.Drawing2D.DashStyle) value;
					}
				}
				return System.Drawing.Drawing2D.DashStyle.Dash;
			}
			set
			{
				if (this.propContainer != null)
				{
					object curVal = this.propContainer.GetPropertyValue("GridDashStyle");
					if (curVal == null || (curVal.GetType() == typeof(System.Drawing.Drawing2D.DashStyle) && (System.Drawing.Drawing2D.DashStyle) curVal != value))
					{
						this.propContainer.SetPropertyValue("GridDashStyle", value);
					}
				}
			}
		}

		/// <summary>
		/// Takes a device point and returns the point on the grid that is nearest
		/// to that point.
		/// </summary>
		/// <param name="ptDevIn">Input point</param>
		/// <returns>Point on the grid</returns>
		/// <remarks>
		/// <para>
		/// Points are in device coordinates. This method is used to when the snap
		/// to grid feature is enabled.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LayoutGrid.SnapToGrid"/>
		/// </remarks>
		public System.Drawing.Point GetNearestGridPoint(System.Drawing.Point ptDevIn)
		{
			int resX = ptDevIn.X;
			int resY = ptDevIn.Y;

			if (this.containerView != null && this.containerView.Model != null)
			{
				RectangleF rcWorld = this.containerView.Model.Bounds;
				SizeF spacing = this.Spacing;
				PointF upperLeft = new PointF(this.containerView.Model.Bounds.Location.X + spacing.Width, this.containerView.Model.Bounds.Location.Y + spacing.Height);
				upperLeft = this.containerView.ViewToDeviceF(this.containerView.WorldToView(upperLeft));
				float upperLeftX = upperLeft.X;
				float upperLeftY = upperLeft.Y;

				SizeF spacingDevice = this.containerView.ViewToDeviceF(this.containerView.WorldToView(spacing));
				float spacingWidth = spacingDevice.Width;
				float spacingHeight = spacingDevice.Height;

				int gridUnitsX = (int) Math.Round((ptDevIn.X - upperLeftX) / spacingWidth);
				int gridUnitsY = (int) Math.Round((ptDevIn.Y - upperLeftY) / spacingHeight);

				resX = (int) (upperLeft.X + ((float) gridUnitsX * spacingWidth));
				resY = (int) (upperLeft.Y + ((float) gridUnitsY * spacingWidth));
			}

			return new System.Drawing.Point(resX, resY);
		}

		/// <summary>
		/// Renders the grid onto a specified System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Drawing context object</param>
		/// <remarks>
		/// <para>
		/// This method is overridden in derived classes to render specific types of
		/// grids.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDraw"/>
		/// </remarks>
		public abstract void Draw(System.Drawing.Graphics grfx);

		/// <summary>
		/// View this grid renders to
		/// </summary>
		private View containerView = null;

		/// <summary>
		/// Property container to access for properties
		/// </summary>
		protected IPropertyContainer propContainer = null;
	}
}
