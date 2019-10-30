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
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Implements a port that is always oriented to the center of a symbol and
	/// is always invisible.
	/// </summary>
	[Serializable()]
	public class CenterPort : Port, ILocalBounds2DF, IHitTestBounds, ISerializable
	{
		/// <summary>
		/// 
		/// </summary>
		public CenterPort()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		public CenterPort(IPortContainer container) : base(container)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public CenterPort(CenterPort src) : base(src)
		{
			this.radius = src.radius;
		}

		/// <summary>
		/// Serialization constructor for center ports.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected CenterPort(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.radius = info.GetSingle("radius");
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new CenterPort(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public override PointF Location
		{
			get
			{
				IPortContainer container = this.Container;
				if (container != null)
				{
					IBounds2DF containerBounds = container as IBounds2DF;
					if (containerBounds != null)
					{
						return Geometry.CenterPoint(containerBounds.Bounds);
					}
				}
				return new PointF(0,0);
			}
			set
			{
				throw new EInvalidOperation();
			}
		}

		/// <summary>
		/// 
		/// </summary>
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
		public override SizeF Size
		{
			get
			{
				return new SizeF(this.radius * 2, this.radius * 2);
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override RectangleF Bounds
		{
			get
			{
				return new RectangleF(this.Location, this.Size);
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override float X
		{
			get
			{
				return this.Location.X;
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override float Y
		{
			get
			{
				return this.Location.Y;
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override float Width
		{
			get
			{
				return this.Size.Width;
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override float Height
		{
			get
			{
				return this.Size.Height;
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		RectangleF ILocalBounds2DF.Bounds
		{
			get
			{
				return new RectangleF(0,0,0,0);
			}
			set
			{
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
		/// Not allowed for center ports.
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public override void Translate(float dx, float dy)
		{
			throw new EInvalidOperation();
		}

		/// <summary>
		/// 
		/// </summary>
		public override Matrix LocalTransform
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override Matrix WorldTransform
		{
			get
			{
				return new Matrix();
			}
		}

		/// <summary>
		/// 
		/// </summary>
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
			// Center port is always invisible
		}

		/// <summary>
		/// Sets the default property values for the port.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// port to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			base.SetDefaultPropertyValues();
			SetPropertyValue("AttachAtPerimeter", true);
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
			info.AddValue("radius", this.radius);
		}

		/// <summary>
		/// 
		/// </summary>
		private float radius = 3.0f;
	}
}
