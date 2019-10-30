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
	/// Implements a port that is rendered as a circle and crosshairs.
	/// </summary>
	[Serializable()]
	public class CirclePort : Port, IHitTestBounds, ILocalBounds2DF, ISerializable, IDeserializationCallback
	{
		/// <summary>
		/// 
		/// </summary>
		public CirclePort() : base()
		{
			this.location = new PointF(0,0);
			this.matrix = new Matrix();
			this.lineStyle = new LineStyle(this);
			this.lineStyle.LineWidth = 1.0f;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		public CirclePort(PointF location) : base()
		{
			this.location = location;
			this.matrix = new Matrix();
			this.lineStyle = new LineStyle(this);
			this.lineStyle.LineWidth = 1.0f;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="owner"></param>
		public CirclePort(Symbol owner) : base(owner)
		{
			this.location = new PointF(0,0);
			this.matrix = new Matrix();
			this.lineStyle = new LineStyle(this);
			this.lineStyle.LineWidth = 1.0f;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public CirclePort(CirclePort src) : base(src)
		{
			this.location = new PointF(src.location.X, src.location.Y);
			this.matrix = src.matrix.Clone();
			this.radius = src.radius;
			this.lineStyle = new LineStyle(this);
		}

		/// <summary>
		/// Serialization constructor for circle ports.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected CirclePort(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			float m11 = info.GetSingle("m11");
			float m12 = info.GetSingle("m12");
			float m21 = info.GetSingle("m21");
			float m22 = info.GetSingle("m22");
			float dx = info.GetSingle("dx");
			float dy = info.GetSingle("dy");
			this.matrix = new Matrix(m11, m12, m21, m22, dx, dy);
			this.location = (PointF) info.GetValue("location", typeof(PointF));
			this.radius = info.GetSingle("radius");
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new CirclePort(this);
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		TypeConverter(typeof(LineStyleConverter)),
		Category("Appearance")
		]
		public LineStyle LineStyle
		{
			get
			{
				return this.lineStyle;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override PointF Location
		{
			get
			{
				Matrix curMatrix = this.WorldTransform;
				PointF[] aPts = { this.location };
				curMatrix.TransformPoints(aPts);
				return aPts[0];
			}
			set
			{
				Matrix curMatrix = this.WorldTransform;
				PointF[] oldPts = { this.location };
				curMatrix.TransformPoints(oldPts);
				PointF[] newPts = { value };
				curMatrix.Invert();
				curMatrix.TransformPoints(newPts);
				this.location = newPts[0];
				float dx = newPts[0].X - oldPts[0].X;
				float dy = newPts[0].Y - oldPts[0].Y;
				this.OnMove(new MoveEventArgs(this, dx, dy));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		Category("General")
		]
		public override float Radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				this.radius = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override SizeF Size
		{
			get
			{
				return new SizeF(this.radius * 2, this.radius * 2);
			}
			set
			{
				float width = value.Width;
				float height = value.Height;

				if (width > height)
				{
					this.radius = width / 2.0f;
				}
				else
				{
					this.radius = height / 2.0f;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override RectangleF Bounds
		{
			get
			{
				Matrix curMatrix = this.WorldTransform;
				PointF[] aPts = { this.location };
				curMatrix.TransformPoints(aPts);
				float width = this.radius * 2;
				return new RectangleF(aPts[0].X, aPts[0].Y, width, width);
			}
			set
			{
				this.Location = value.Location;
				this.Size = value.Size;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		Category("Location")
		]
		public override float X
		{
			get
			{
				return this.Location.X;
			}
			set
			{
				this.Location = new PointF(value, this.Location.Y);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		Category("Location")
		]
		public override float Y
		{
			get
			{
				return this.Location.Y;
			}
			set
			{
				this.Location = new PointF(this.Location.X, value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override float Width
		{
			get
			{
				return this.Size.Width;
			}
			set
			{
				this.radius = value / 2.0f;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override float Height
		{
			get
			{
				return this.Size.Height;
			}
			set
			{
				this.radius = value / 2.0f;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		RectangleF ILocalBounds2DF.Bounds
		{
			get
			{
				Matrix curMatrix = Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);
				PointF[] aPts = { this.location };
				curMatrix.TransformPoints(aPts);
				Global.MatrixStack.Pop();
				float width = (radius * 2.0f);
				return new RectangleF(aPts[0].X, aPts[0].Y, width, width);
			}
			set
			{
				// TODO: does this need to be implemented?
			}
		}

		/// <summary>
		/// 
		/// </summary>
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
		/// 
		/// </summary>
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
		/// 
		/// </summary>
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
		/// 
		/// </summary>
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public override void Translate(float dx, float dy)
		{
			this.matrix.Translate(dx, dy, MatrixOrder.Append);
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override Matrix LocalTransform
		{
			get
			{
				return this.matrix;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override Matrix WorldTransform
		{
			get
			{
				Matrix worldTransform = new Matrix();

				if (this.Parent != null)
				{
					ITransform objTransformParent = this.Parent as ITransform;
					if (objTransformParent != null)
					{
						worldTransform.Multiply(objTransformParent.WorldTransform, MatrixOrder.Append);
					}
				}

				worldTransform.Multiply(this.matrix, MatrixOrder.Prepend);

				return worldTransform;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override Matrix ParentTransform
		{
			get
			{
				Matrix parentTransform = null;

				if (this.Parent != null)
				{
					ITransform objTransformParent = this.Parent as ITransform;
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptTest"></param>
		/// <param name="fSlop"></param>
		/// <returns></returns>
		bool IHitTestBounds.ContainsPoint(PointF ptTest, float fSlop)
		{
			RectangleF bounds = ((ILocalBounds2DF)this).Bounds;
			bounds.Inflate(new SizeF(fSlop, fSlop));
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
				GraphicsPath grfxPath = this.CreateGraphicsPath();
				grfxPath.Transform(xform);

				Pen pen = this.lineStyle.CreatePen();
				grfx.DrawPath(pen, grfxPath);
				pen.Dispose();
				grfxPath.Dispose();

				Global.MatrixStack.Pop();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private System.Drawing.Drawing2D.GraphicsPath CreateGraphicsPath()
		{
			GraphicsPath grfxPath = new GraphicsPath();

			RectangleF rcBounds = new RectangleF(this.location.X - this.radius,
				this.location.Y - this.radius,
				this.radius * 2,
				this.radius * 2);

			grfxPath.AddEllipse(rcBounds);
			grfxPath.CloseFigure();

			grfxPath.StartFigure();
			PointF ptLeft = new PointF(this.location.X - this.radius, this.location.Y);
			PointF ptRight = new PointF(this.location.X + this.radius, this.location.Y);
			grfxPath.AddLine(ptLeft, ptRight);
			grfxPath.CloseFigure();

			grfxPath.StartFigure();
			PointF ptTop = new PointF(this.location.X, this.location.Y - this.radius);
			PointF ptBottom = new PointF(this.location.X, this.location.Y + this.radius);
			grfxPath.AddLine(ptTop, ptBottom);
			grfxPath.CloseFigure();

			return grfxPath;
		}

		/// <summary>
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("m11", this.matrix.Elements[0]);
			info.AddValue("m12", this.matrix.Elements[1]);
			info.AddValue("m21", this.matrix.Elements[2]);
			info.AddValue("m22", this.matrix.Elements[3]);
			info.AddValue("dx", this.matrix.Elements[4]);
			info.AddValue("dy", this.matrix.Elements[5]);
			info.AddValue("location", this.location);
			info.AddValue("radius", this.radius);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.lineStyle = new LineStyle(this);
		}

		/// <summary>
		/// Called when the position of the node is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Port.Parent"/>
		/// of the move by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Move"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMove(MoveEventArgs evtArgs)
		{
			if (this.Parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.Parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Move(evtArgs);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected Matrix matrix;

		/// <summary>
		/// 
		/// </summary>
		private PointF location;

		/// <summary>
		/// 
		/// </summary>
		private float radius = 3.0f;

		/// <summary>
		/// 
		/// </summary>
		private LineStyle lineStyle = null;
	}
}
