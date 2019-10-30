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
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A shape is a leaf node that encapsulates a drawing primitive such as
	/// a rectangle, line, ellipse, or polygon.
	/// is a drawing primitive such as encapsulates a graphics object that is represented by a
	/// GDI+ GraphicsPath.
	/// </summary>
	/// <remarks>
	/// The Shape class implements the <see cref="Syncfusion.Windows.Forms.Diagram.IGraphics"/>
	/// interface and contains a GDI+ GraphicsPath object. This class is an abstract
	/// base class from which specific shapes are implemented. The contained GraphicsPath
	/// object is contains provides rendering and hit testing for the shape.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphics"/>
	/// </remarks>
	[Serializable]
	public abstract class Shape : INode, IBounds2DF, ILocalBounds2DF, IGraphics, ITransform, IPoints, ILogicalUnitContainer, IHitTestBounds, IHitTestRegion, IPropertyContainer, ISerializable, IDeserializationCallback, IDispatchNodeEvents
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public Shape()
		{
			this.grfxPath = new System.Drawing.Drawing2D.GraphicsPath();
			this.matrix = new Matrix();
			this.propertyValues = new Hashtable();
			this.SetDefaultPropertyValues();
			this.CreateStyles();
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="src">Object to copy from</param>
		public Shape(Shape src)
		{
			this.name = src.name;
			this.grfxPath = (GraphicsPath) src.grfxPath.Clone();
			this.matrix = src.matrix.Clone();
			this.propertyValues = (Hashtable) src.propertyValues.Clone();
			this.CreateStyles();
		}

		/// <summary>
		/// Serialization constructor for shapes.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Shape(SerializationInfo info, StreamingContext context)
		{
			this.name = info.GetString("name");
			this.parent = (ICompositeNode) info.GetValue("parent", typeof(ICompositeNode));
			PointF[] pathPts = (PointF[]) info.GetValue("pathPoints", typeof(PointF[]));
			byte[] pathTypes = (byte[]) info.GetValue("pathTypes", typeof(byte[]));
			this.grfxPath = new System.Drawing.Drawing2D.GraphicsPath(pathPts, pathTypes);
			float m11 = info.GetSingle("m11");
			float m12 = info.GetSingle("m12");
			float m21 = info.GetSingle("m21");
			float m22 = info.GetSingle("m22");
			float dx = info.GetSingle("dx");
			float dy = info.GetSingle("dy");
			this.matrix = new Matrix(m11, m12, m21, m22, dx, dy);
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
			this.CreateStyles();
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public abstract object Clone();

		#endregion

		#region Public Properties

		/// <summary>
		/// Determines if the shape is visible or not.
		/// </summary>
		[
		Browsable(true),
		Category("Appearance")
		]
		public bool Visible
		{
			get
			{
				object value = this.GetPropertyValue("Visible");
				if (value == null)
				{
					return true;
				}
				return (bool) value;
			}
			set
			{
				this.SetPropertyValue("Visible", value);
			}
		}

		#endregion

		#region IServiceProvider interface

		/// <summary>
		/// Returns the specified type of service object the caller.
		/// </summary>
		/// <param name="svcType">Type of service requested</param>
		/// <returns>
		/// The object matching the service type requested or null if the
		/// service is not supported.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method is similar to COM's IUnknown::QueryInterface method,
		/// although more generic. Instead of just returning interfaces,
		/// this method can return any type of object.
		/// </para>
		/// <para>
		/// The following services are supported:
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IBounds2DF"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </para>
		/// </remarks>
		object IServiceProvider.GetService(System.Type svcType)
		{
			if (svcType == typeof(IDispatchNodeEvents))
			{
				return (IDispatchNodeEvents) this;
			}
			else if (svcType == typeof(IPropertyContainer))
			{
				return (IPropertyContainer) this;
			}
			else if (svcType == typeof(IBounds2DF))
			{
				return (IBounds2DF) this;
			}
			else if (svcType == typeof(IPoints))
			{
				return (IPoints) this;
			}
			else if (svcType == typeof(ITransform))
			{
				return (ITransform) this;
			}

			return null;
		}

		#endregion

		#region INode interface

		/// <summary>
		/// Reference to the composite node this node is a child of.
		/// </summary>
		[Browsable(false)]
		public ICompositeNode Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		/// <summary>
		/// The root node in the node hieararchy.
		/// </summary>
		/// <remarks>
		/// The root node is found by following the chain of parent nodes until
		/// a node is found that has a null parent.
		/// </remarks>
		[Browsable(false)]
		public INode Root
		{
			get
			{
				if (this.parent == null)
				{
					return this;
				}
				return this.parent.Root;
			}
		}

		/// <summary>
		/// Name of the node.
		/// </summary>
		/// <remarks>
		/// Must be unique within the scope of the parent node.
		/// </remarks>
		[
		Browsable(true),
		Category("General"),
		Description("Unique name of shape")
		]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>
		/// Fully qualified name of the node.
		/// </summary>
		/// <remarks>
		/// The full name is the name of the node concatenated with the names
		/// of all parent nodes.
		/// </remarks>
		[
		Browsable(true),
		Category("General"),
		Description("Fully-qualified name of shape")
		]
		public string FullName
		{
			get
			{
				if (this.parent == null)
				{
					return this.name;
				}
				return this.parent.FullName + "." + this.name;
			}
		}

		#endregion

		#region IBounds2DF interface

		/// <summary>
		/// The shape's bounding box.
		/// </summary>
		/// <remarks>
		/// Always returns the bounds of the shape in world coordinates, regardless
		/// of what is on the matrix stack at the time of the call.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual RectangleF Bounds
		{
			get
			{
				RectangleF bounds;
				if (this.grfxPath != null)
				{
					bounds = this.grfxPath.GetBounds(this.WorldTransform);
				}
				else
				{
					bounds = new RectangleF(0,0,0,0);
				}
				return bounds;
			}
			set
			{
				if (this.grfxPath != null)
				{
					// Get the origin relative to the parent
					RectangleF oldBounds = this.Bounds;
					PointF curOrigin = Geometry.CenterPoint(oldBounds);

					// Get the new origin
					PointF newOrigin = Geometry.CenterPoint(value);

					// Calculate translation vector
					float offsetX = newOrigin.X - curOrigin.X;
					float offsetY = newOrigin.Y - curOrigin.Y;

					// Calculate scale
					float scaleX = value.Width / oldBounds.Width;
					float scaleY = value.Height / oldBounds.Height;

					// Save backup of current matrix
					Matrix origMatrix = this.matrix.Clone();

					// Apply transformations
					this.matrix.Translate(-curOrigin.X, -curOrigin.Y, MatrixOrder.Append);
					this.matrix.Scale(scaleX, scaleY, MatrixOrder.Append);
					this.matrix.Translate(curOrigin.X, curOrigin.Y, MatrixOrder.Append);
					this.matrix.Translate(offsetX, offsetY, MatrixOrder.Append);

					// Test to see if the new bounds fall within the constraining region
					// defined by the parent node
					if (!this.CheckConstrainingRegion())
					{
						this.matrix = origMatrix;
						throw new EBoundaryConstraint(this);
					}
					origMatrix.Dispose();

					this.OnBoundsChanged(new BoundsEventArgs(this, oldBounds, this.Bounds));
				}
			}
		}

		/// <summary>
		/// X-coordinate of the shape's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		Category("Bounds")
		]
		public virtual float X
		{
			get
			{
				return this.Bounds.Left;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Location = new PointF(value, rcBounds.Top);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Y-coordinate of the shape's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		Category("Bounds")
		]
		public virtual float Y
		{
			get
			{
				return this.Bounds.Top;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Location = new PointF(rcBounds.Left, value);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Width of the shape.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		Category("Bounds")
		]
		public virtual float Width
		{
			get
			{
				return this.Bounds.Width;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Size = new SizeF(value, rcBounds.Height);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Height of the shape.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		Category("Bounds")
		]
		public virtual float Height
		{
			get
			{
				return this.Bounds.Height;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Size = new SizeF(rcBounds.Width, value);
				this.Bounds = rcBounds;
			}
		}

		#endregion

		#region ILocalBounds2DF interface

		/// <summary>
		/// Bounding box of the shape in local coordinates.
		/// </summary>
		/// <remarks>
		/// The value returned depends on the contents of the matrix stack. If the
		/// matrix stack is empty, then the value returned is in local coordinates.
		/// This method is generally used by functions that recursively traverse
		/// the node hierarchy, pushing and popping the matrix stack as they go.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Global.MatrixStack"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Shape.Bounds"/>
		/// </remarks>
		RectangleF ILocalBounds2DF.Bounds
		{
			get
			{
				RectangleF bounds;
				if (this.grfxPath != null)
				{
					Matrix xform = Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);
					bounds = this.grfxPath.GetBounds(xform);
					Global.MatrixStack.Pop();
				}
				else
				{
					bounds = new RectangleF(0,0,0,0);
				}
				return bounds;
			}
			set
			{
				if (this.grfxPath != null)
				{
					// Get the origin relative to the parent
					Global.MatrixStack.Clear();
					Global.MatrixStack.Push(this.ParentTransform);
					RectangleF bounds = this.Bounds;
					Global.MatrixStack.Pop();
					PointF curOrigin = Geometry.CenterPoint(bounds);

					// Get the new origin
					PointF newOrigin = Geometry.CenterPoint(value);

					// Calculate translation vector
					float offsetX = newOrigin.X - curOrigin.X;
					float offsetY = newOrigin.Y - curOrigin.Y;

					// Calculate scale
					float scaleX = value.Width / bounds.Width;
					float scaleY = value.Height / bounds.Height;

					// Apply transformations
					this.matrix.Translate(-curOrigin.X, -curOrigin.Y, MatrixOrder.Append);
					this.matrix.Scale(scaleX, scaleY, MatrixOrder.Append);
					this.matrix.Translate(curOrigin.X, curOrigin.Y, MatrixOrder.Append);
					this.matrix.Translate(offsetX, offsetY, MatrixOrder.Append);

					this.OnBoundsChanged(new BoundsEventArgs(this, bounds, this.Bounds));
				}
			}
		}

		/// <summary>
		/// X-coordinate of the object's location.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.X
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Left;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Location = new PointF(value, rcBounds.Top);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Y-coordinate of the object's location.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.Y
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Top;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Location = new PointF(rcBounds.Left, value);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Width of the object.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.Width
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Width;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Size = new SizeF(value, rcBounds.Height);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Height of the object.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.Height
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Height;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Size = new SizeF(rcBounds.Width, value);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		#endregion

		#region IGraphics interface

		/// <summary>
		/// Renders the shape onto a System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Graphics context to render onto</param>
		public virtual void Draw(System.Drawing.Graphics grfx)
		{
			if (this.Visible)
			{
				grfx.Transform = Global.ViewMatrix;
				Matrix xform = Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);
				GraphicsPath grfxPath = (GraphicsPath) this.grfxPath.Clone();
				grfxPath.Transform(xform);

				Pen pen = this.lineStyle.CreatePen();
				grfx.DrawPath(pen, grfxPath);
				grfxPath.Dispose();
				pen.Dispose();

				Global.MatrixStack.Pop();
			}
		}

		/// <summary>
		/// Encapsulates the points and instructions needed to render the shape.
		/// </summary>
		/// <remarks>
		/// The contents of the GraphicsPath is determined by derived classes,
		/// and depends on the type of shape.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Shape.CreateRegion"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual System.Drawing.Drawing2D.GraphicsPath GraphicsPath
		{
			get
			{
				System.Drawing.Drawing2D.GraphicsPath grfxPath = null;
				Matrix xform;

				if (this.grfxPath != null)
				{
					grfxPath = (System.Drawing.Drawing2D.GraphicsPath) this.grfxPath.Clone();
					xform = Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);
					grfxPath.Transform(xform);
					Global.MatrixStack.Pop();
				}

				return grfxPath;
			}
		}

		/// <summary>
		/// Returns an object that describes the interior of the shape.
		/// </summary>
		/// <param name="padding">Amount of padding to add</param>
		/// <returns>System.Drawing.Region object</returns>
		/// <remarks>
		/// Region objects are used for hit testing and geometrical calculations.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Shape.GraphicsPath"/>
		/// </remarks>
		public virtual System.Drawing.Region CreateRegion(float padding)
		{
			System.Drawing.Region rgn = null;
			GraphicsPath grfxPath = this.GraphicsPath;
			GraphicsPath paddedGrfxPath = null;

			if (grfxPath != null)
			{
				if (padding > 0.0f)
				{
					paddedGrfxPath = (GraphicsPath) grfxPath.Clone();
					System.Drawing.Pen pen = new System.Drawing.Pen(Color.Black, padding);
					paddedGrfxPath.Widen(pen);
					pen.Dispose();
					rgn = new System.Drawing.Region(paddedGrfxPath);
				}
				else
				{
					rgn = new System.Drawing.Region(grfxPath);
				}
			}
			else
			{
				rgn = new System.Drawing.Region(this.Bounds);
			}

			return rgn;
		}

		#endregion

		#region ITransform interface

		/// <summary>
		/// Moves the shape by the given X and Y offsets.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		/// <remarks>
		/// Applies a translate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Shape.OnMove"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Translate(float dx, float dy)
		{
			if (dx != 0.0f || dy != 0.0f)
			{
				Matrix origMatrix = (Matrix) this.matrix.Clone();

				this.matrix.Translate(dx, dy, MatrixOrder.Append);

				if (!this.CheckConstrainingRegion())
				{
					this.matrix = origMatrix;
					throw new EBoundaryConstraint(this);
				}
				origMatrix.Dispose();

				this.OnMove(new MoveEventArgs(this, dx, dy));
			}
		}

		/// <summary>
		/// Rotates the shape a specified number of degrees about a given
		/// anchor point.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to rotate</param>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Shape.OnRotate"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Rotate(PointF ptAnchor, float degrees)
		{
			Matrix origMatrix = (Matrix) this.matrix.Clone();

			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Rotate(degrees, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);

			if (!this.CheckConstrainingRegion())
			{
				this.matrix = origMatrix;
				throw new EBoundaryConstraint(this);
			}
			origMatrix.Dispose();

			this.OnRotate(new RotateEventArgs(this));
		}

		/// <summary>
		/// Rotates the shape a specified number of degrees about its center point.
		/// </summary>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Shape.OnRotate"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Rotate(float degrees)
		{
			Matrix origMatrix = (Matrix) this.matrix.Clone();

			PointF ptOrigin = Geometry.CenterPoint(this.Bounds);
			this.matrix.Translate(-ptOrigin.X, -ptOrigin.Y, MatrixOrder.Append);
			this.matrix.Rotate(degrees, MatrixOrder.Append);
			this.matrix.Translate(ptOrigin.X, ptOrigin.Y, MatrixOrder.Append);

			if (!this.CheckConstrainingRegion())
			{
				this.matrix = origMatrix;
				throw new EBoundaryConstraint(this);
			}
			origMatrix.Dispose();

			this.OnRotate(new RotateEventArgs(this));
		}

		/// <summary>
		/// Scales the shape by a given ratio along the X and Y axes.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to scale</param>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Shape.OnScale"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Scale(PointF ptAnchor, float sx, float sy)
		{
			Matrix origMatrix = (Matrix) this.matrix.Clone();

			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Scale(sx, sy, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);

			if (!this.CheckConstrainingRegion())
			{
				this.matrix = origMatrix;
				throw new EBoundaryConstraint(this);
			}
			origMatrix.Dispose();

			this.OnScale(new ScaleEventArgs(this));
		}

		/// <summary>
		/// Scales the shape about its center point by a given ratio.
		/// </summary>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Shape.OnScale"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Scale(float sx, float sy)
		{
			Matrix origMatrix = (Matrix) this.matrix.Clone();

			PointF ptAnchor = Geometry.CenterPoint(this.Bounds);
			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Scale(sx, sy, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);

			if (!this.CheckConstrainingRegion())
			{
				this.matrix = origMatrix;
				throw new EBoundaryConstraint(this);
			}
			origMatrix.Dispose();

			this.OnScale(new ScaleEventArgs(this));
		}

		/// <summary>
		/// Matrix containing transformations for this node.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Matrix LocalTransform
		{
			get
			{
				return this.matrix;
			}
		}

		/// <summary>
		/// Returns a matrix containing transformations for this node and all of
		/// its ancestors.
		/// </summary>
		/// <remarks>
		/// Chains up the node hierarchy and builds a transformation matrix containing
		/// all transformations that apply to this node in the world coordinate space.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Matrix WorldTransform
		{
			get
			{
				Matrix worldTransform = new Matrix();

				if (this.parent != null)
				{
					ITransform objTransformParent = this.parent as ITransform;
					if (objTransformParent != null)
					{
						worldTransform.Multiply(objTransformParent.WorldTransform, MatrixOrder.Append);
					}
				}

				worldTransform.Multiply(this.matrix, MatrixOrder.Append);

				return worldTransform;
			}
		}

		/// <summary>
		/// Returns a matrix containing the transformations of this node's parent.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Matrix ParentTransform
		{
			get
			{
				Matrix parentTransform = null;

				if (this.parent != null)
				{
					ITransform objTransformParent = this.parent as ITransform;
					if (objTransformParent != null)
					{
						parentTransform = objTransformParent.WorldTransform;
					}
				}

				if (parentTransform == null)
				{
					parentTransform = new Matrix();
				}

				return parentTransform;
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

		#region Styles

		/// <summary>
		/// Line drawing properties for this node.
		/// </summary>
		/// <remarks>
		/// The line style determines the configuration of the pen used to
		/// render lines.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LineStyle"/>
		/// </remarks>
		[
		Browsable(true),
		TypeConverter(typeof(LineStyleConverter)),
		Category("Appearance"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public LineStyle LineStyle
		{
			get
			{
				return this.lineStyle;
			}
		}

		/// <summary>
		/// Edit properties for this node.
		/// </summary>
		/// <remarks>
		/// Edit properties determine how this node can be edited.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.EditStyle"/>
		/// </remarks>
		[
		Browsable(true),
		TypeConverter(typeof(EditStyleConverter)),
		Category("Behavior"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public EditStyle EditStyle
		{
			get
			{
				return this.editStyle;
			}
		}

		#endregion

		#region IPoints interface

		/// <summary>
		/// Number of vertices contained by the shape.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual int PointCount
		{
			get
			{
				if (this.grfxPath != null)
				{
					return this.grfxPath.PointCount;
				}
				return 0;
			}
		}

		/// <summary>
		/// Minimum number of vertices this shape may contain.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual int MinPoints
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Maxiumum number of vertices this shape may contain.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual int MaxPoints
		{
			get
			{
				return System.Int32.MaxValue;
			}
		}

		/// <summary>
		/// Returns an array containing all vertices belonging to this shape.
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// The points returned are in local coordinates. They are not
		/// transformed in any way.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public virtual PointF[] GetPoints()
		{
			if (this.grfxPath != null)
			{
				return this.grfxPath.PathPoints;
			}
			return null;
		}

		/// <summary>
		/// Assigns an array of points to the shape.
		/// </summary>
		/// <param name="pts">Points to assign to the shape</param>
		/// <remarks>
		/// The array passed in cannot be null and the number of points it contains
		/// must be greater than or equal to
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.MinPoints"/>.
		/// The points are passed into the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.CreateGraphicsPath"/>
		/// method in order to create the GraphicsPath for the shape.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Shape.GraphicsPath"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Shape.CreateGraphicsPath"/>
		/// </remarks>
		public virtual void SetPoints(PointF[] pts)
		{
			if (pts == null || pts.Length < this.MinPoints)
			{
				throw new EInvalidParameter();
			}

			this.grfxPath = CreateGraphicsPath(pts);
		}

		/// <summary>
		/// Returns the vertex at the specified index position.
		/// </summary>
		/// <param name="ptIdx">Zero-based index position of the point to retrieve</param>
		/// <returns>Point at given offset</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public virtual PointF GetPoint(int ptIdx)
		{
			if (this.grfxPath == null)
			{
				throw new EInvalidOperation();
			}
			return this.grfxPath.PathPoints[ptIdx];
		}

		/// <summary>
		/// Assigns the value of the vertex at the specified index position.
		/// </summary>
		/// <param name="ptIdx">Zero-based index position of the point to update</param>
		/// <param name="val">Value to assign to the vertex</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public virtual void SetPoint(int ptIdx, PointF val)
		{
			if (this.grfxPath == null)
			{
				throw new EInvalidOperation();
			}

			int numPts = this.grfxPath.PointCount;

			if (ptIdx < 0 || ptIdx >= numPts)
			{
				throw new EInvalidParameter();
			}

			PointF[] pathPts = this.grfxPath.PathPoints;

			if (pathPts[ptIdx] != val)
			{
				if (ptIdx >=0 && ptIdx < numPts)
				{
					pathPts[ptIdx] = val;
					this.grfxPath = CreateGraphicsPath(pathPts);
				}
				this.OnMoveVertex(new VertexEventArgs(this, ptIdx));
			}
		}

		/// <summary>
		/// Adds a vertex to the shape.
		/// </summary>
		/// <param name="val">Value of the point to add</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public virtual void AddPoint(PointF val)
		{
			if (this.grfxPath == null)
			{
				throw new EInvalidOperation();
			}

			int numPts = this.grfxPath.PointCount;

			if (numPts >= this.MaxPoints)
			{
				throw new EInvalidOperation();
			}

			PointF[] curPts = this.grfxPath.PathPoints;
			PointF[] newPts = new PointF[numPts+1];
			for (int ptIdx = 0; ptIdx < numPts; ptIdx++)
			{
				newPts[ptIdx] = curPts[ptIdx];
			}
			newPts[numPts] = val;
			this.grfxPath = CreateGraphicsPath(newPts);
			this.OnInsertVertex(new VertexEventArgs(this, numPts));
		}

		/// <summary>
		/// Inserts a vertex into the shape at the given index position.
		/// </summary>
		/// <param name="ptIdx">Zero-based index at which to insert the vertex</param>
		/// <param name="val">Value of the vertex to insert</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public virtual void InsertPoint(int ptIdx, PointF val)
		{
			if (this.grfxPath == null)
			{
				throw new EInvalidOperation();
			}
			int numNewPts = this.grfxPath.PointCount + 1;

			if (numNewPts >= this.MaxPoints)
			{
				throw new EInvalidOperation();
			}

			if (ptIdx < 0 || ptIdx >= numNewPts)
			{
				throw new EInvalidParameter();
			}

			PointF[] curPts = this.grfxPath.PathPoints;
			PointF[] newPts = new PointF[numNewPts];
			int idxOldPts = 0;
			int idxNewPts = 0;
			while (idxNewPts < numNewPts)
			{
				if (idxNewPts == ptIdx)
				{
					newPts[idxNewPts] = val;
				}
				else
				{
					newPts[idxNewPts] = curPts[idxOldPts];
					idxOldPts++;
				}
				idxNewPts++;
			}
			this.grfxPath = CreateGraphicsPath(newPts);
			this.OnInsertVertex(new VertexEventArgs(this, ptIdx));
		}

		/// <summary>
		/// Removes a vertex from the shape.
		/// </summary>
		/// <param name="ptIdx">Zero-based index position of the vertex to remove</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public virtual void RemovePoint(int ptIdx)
		{
			if (this.grfxPath == null)
			{
				throw new EInvalidOperation();
			}
			int numPts = this.grfxPath.PointCount;

			if (ptIdx < 0 || ptIdx >= numPts)
			{
				throw new EInvalidParameter();
			}

			if (numPts <= this.MinPoints)
			{
				throw new EInvalidOperation();
			}

			PointF[] curPts = this.grfxPath.PathPoints;
			PointF[] newPts = new PointF[numPts-1];
			int idxOldPts = 0;
			int idxNewPts = 0;
			while (idxOldPts < numPts)
			{
				if (idxOldPts != ptIdx)
				{
					newPts[idxNewPts] = curPts[idxOldPts];
					idxNewPts++;
				}
				idxOldPts++;
			}
			this.grfxPath = CreateGraphicsPath(newPts);
			this.OnDeleteVertex(new VertexEventArgs(this, ptIdx));
		}

		/// <summary>
		/// Removes all vertices from the shape.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public virtual void RemoveAllPoints()
		{
			if (this.grfxPath != null)
			{
				this.grfxPath.Reset();
			}
		}

		#endregion

		#region IHitTestBounds interface

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptTest"></param>
		/// <param name="fSlop"></param>
		/// <returns></returns>
		bool IHitTestBounds.ContainsPoint(PointF ptTest, float fSlop)
		{
			RectangleF bounds = this.Bounds;
			return bounds.Contains(ptTest);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcTest"></param>
		/// <returns></returns>
		bool IHitTestBounds.IntersectsRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return bounds.IntersectsWith(rcTest);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcTest"></param>
		/// <returns></returns>
		bool IHitTestBounds.ContainedByRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return rcTest.Contains(bounds);
		}

		#endregion

		#region IHitTestRegion interface

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptTest"></param>
		/// <param name="fSlop"></param>
		/// <returns></returns>
		bool IHitTestRegion.ContainsPoint(PointF ptTest, float fSlop)
		{
			bool hit = false;
			System.Drawing.Region rgn = this.CreateRegion(fSlop);
			if (rgn != null)
			{
				hit = rgn.IsVisible(ptTest);
			}
			return hit;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcTest"></param>
		/// <returns></returns>
		bool IHitTestRegion.IntersectsRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return bounds.IntersectsWith(rcTest);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcTest"></param>
		/// <returns></returns>
		bool IHitTestRegion.ContainedByRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return rcTest.Contains(bounds);
		}

		#endregion

		#region IPropertyContainer interface

		/// <summary>
		/// Sets the default property values for the shape.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// shape to their default values.
		/// </remarks>
		public virtual void SetDefaultPropertyValues()
		{
		}

		/// <summary>
		/// Retrieve the value of a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to retrieve</param>
		/// <returns>Value of the named property or null if it doesn't exist</returns>
		public virtual object GetPropertyValue(string propertyName)
		{
			if (this.propertyValues.Contains(propertyName))
			{
				return this.propertyValues[propertyName];
			}

			if (this.parent != null)
			{
				IPropertyContainer parentProps = this.parent.GetPropertyContainer(this);
				if (parentProps != null)
				{
					return parentProps.GetPropertyValue(propertyName);
				}
			}

			return null;
		}

		/// <summary>
		/// Assign a value to a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to set</param>
		/// <param name="val">Value to assign to property</param>
		/// <remarks>
		/// This method will add the property to the container if it doesn't
		/// already exist.
		/// </remarks>
		public virtual void SetPropertyValue(string propertyName, object val)
		{
			object oldVal = null;

			if (this.propertyValues.ContainsKey(propertyName))
			{
				oldVal = this.propertyValues[propertyName];
				this.propertyValues[propertyName] = val;
			}
			else
			{
				this.propertyValues.Add(propertyName, val);
			}

			this.OnPropertyChanged(new PropertyEventArgs(this, propertyName, oldVal, val));
		}

		/// <summary>
		/// Assign a value to a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to change</param>
		/// <param name="val">Value to assign to property</param>
		/// <remarks>
		/// This method only modifies property values that already exist
		/// in the container. If the property does not exist, this method fails.
		/// </remarks>
		public virtual void ChangePropertyValue(string propertyName, object val)
		{
			object oldVal = null;

			if (this.propertyValues.ContainsKey(propertyName))
			{
				oldVal = this.propertyValues[propertyName];
				this.propertyValues[propertyName] = val;
				this.OnPropertyChanged(new PropertyEventArgs(this, propertyName, oldVal, val));
			}
		}

		/// <summary>
		/// Removes the specified property.
		/// </summary>
		/// <param name="propertyName">Name of property to remove</param>
		public virtual void RemoveProperty(string propertyName)
		{
			if (this.propertyValues.ContainsKey(propertyName))
			{
				this.propertyValues.Remove(propertyName);
			}
		}

		/// <summary>
		/// Returns an array containing the names of all properties in the container.
		/// </summary>
		/// <returns>String array containing property names</returns>
		public virtual string[] GetPropertyNames()
		{
			string[] propertyNames = new string[this.propertyValues.Keys.Count];
			this.propertyValues.Keys.CopyTo(propertyNames, 0);
			return propertyNames;
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("name", this.name);
			info.AddValue("parent", this.parent);
			info.AddValue("pathPoints", this.grfxPath.PathPoints);
			info.AddValue("pathTypes", this.grfxPath.PathTypes);
			info.AddValue("m11", this.matrix.Elements[0]);
			info.AddValue("m12", this.matrix.Elements[1]);
			info.AddValue("m21", this.matrix.Elements[2]);
			info.AddValue("m22", this.matrix.Elements[3]);
			info.AddValue("dx", this.matrix.Elements[4]);
			info.AddValue("dy", this.matrix.Elements[5]);
			info.AddValue("propertyValues", this.propertyValues);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.lineStyle = new LineStyle(this);
		}

		#endregion

		#region IDispatchNodeEvents interface

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.PropertyChanged(PropertyEventArgs evtArgs)
		{
			this.OnPropertyChanged(evtArgs);
		}

		/// <summary>
		/// Called before the collection of child nodes is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ChildrenChanging(NodeCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// Called after the collection of child nodes is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ChildrenChangeComplete(NodeCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.BoundsChanged(BoundsEventArgs evtArgs)
		{
			this.OnBoundsChanged(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.Move(MoveEventArgs evtArgs)
		{
			this.OnMove(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.Rotate(RotateEventArgs evtArgs)
		{
			this.OnRotate(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.Scale(ScaleEventArgs evtArgs)
		{
			this.OnScale(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.Click(NodeMouseEventArgs evtArgs)
		{
			this.OnClick(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.DoubleClick(NodeMouseEventArgs evtArgs)
		{
			this.OnDoubleClick(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.MouseEnter(NodeMouseEventArgs evtArgs)
		{
			this.OnMouseEnter(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.MouseLeave(NodeMouseEventArgs evtArgs)
		{
			this.OnMouseLeave(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.InsertVertex(VertexEventArgs evtArgs)
		{
			this.OnInsertVertex(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.MoveVertex(VertexEventArgs evtArgs)
		{
			this.OnMoveVertex(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.DeleteVertex(VertexEventArgs evtArgs)
		{
			this.OnDeleteVertex(evtArgs);
		}

		/// <summary>
		/// Called before the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		void IDispatchNodeEvents.ConnectionsChanging(ConnectionCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// Called after the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		void IDispatchNodeEvents.ConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs)
		{
		}

		#endregion

		#region Node Event Callbacks

		/// <summary>
		/// Called when a property is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
		/// of the property change by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.PropertyChanged"/>
		/// method.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnPropertyChanged(PropertyEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.PropertyChanged(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the position of the node is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
		/// of the move by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Move"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMove(MoveEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Move(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the node is rotated.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
		/// of the rotation by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Rotate"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RotateEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnRotate(RotateEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Rotate(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the node is scaled.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
		/// of the scaling by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Scale"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ScaleEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnScale(ScaleEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Scale(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the bounds of the node change.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
		/// of the change in bounds by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.BoundsChanged"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoundsEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnBoundsChanged(BoundsEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.BoundsChanged(evtArgs);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected virtual void OnClick(NodeMouseEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Click(evtArgs);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected virtual void OnDoubleClick(NodeMouseEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.DoubleClick(evtArgs);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected virtual void OnMouseEnter(NodeMouseEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.MouseEnter(evtArgs);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected virtual void OnMouseLeave(NodeMouseEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.MouseLeave(evtArgs);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected virtual void OnInsertVertex(VertexEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.InsertVertex(evtArgs);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected virtual void OnMoveVertex(VertexEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.MoveVertex(evtArgs);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		protected virtual void OnDeleteVertex(VertexEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.DeleteVertex(evtArgs);
				}
			}
		}

		#endregion

		#region Implementation methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		protected virtual System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath(PointF[] pts)
		{
			return null;
		}

		/// <summary>
		/// Tests to see if the current position of the shape falls within the
		/// constraining region specified by the parent node.
		/// </summary>
		/// <returns>true if position is valid; otherwise false</returns>
		protected virtual bool CheckConstrainingRegion()
		{
			if (this.parent != null)
			{
				System.Drawing.Region rgnConstraint = this.parent.GetConstrainingRegion(this);
				if (rgnConstraint != null)
				{
					PointF[] pts = this.GetPoints();
					Matrix worldTransform = this.WorldTransform;
					worldTransform.TransformPoints(pts);

					foreach (PointF curPt in pts)
					{
						if (!rgnConstraint.IsVisible(curPt))
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Creates style objects for this class.
		/// </summary>
		protected virtual void CreateStyles()
		{
			this.lineStyle = new LineStyle(this);
			this.editStyle = new EditStyle(this);
		}

		#endregion

		#region Fields

		/// <summary>
		/// Name of the shape
		/// </summary>
		private string name;

		/// <summary>
		/// Reference to parent node
		/// </summary>
		private ICompositeNode parent;

		/// <summary>
		/// Transformation matrix
		/// </summary>
		protected Matrix matrix = null;

		/// <summary>
		/// GraphicsPath containing points and instructions for rendering the shape
		/// </summary>
		protected GraphicsPath grfxPath = null;

		/// <summary>
		/// Hashtable containing property name/value pairs
		/// </summary>
		protected Hashtable propertyValues = null;

		/// <summary>
		/// Properties for creating pens to draw lines
		/// </summary>
		private LineStyle lineStyle = null;

		/// <summary>
		/// Properties for determining edit capabilities
		/// </summary>
		private EditStyle editStyle = null;

		#endregion
	}
}