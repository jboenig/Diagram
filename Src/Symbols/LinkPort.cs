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
	/// Base class for head and tail ports of a link.
	/// </summary>
	public abstract class LinkPort : Port, IHitTestBounds, ISerializable
	{
		/// <summary>
		/// 
		/// </summary>
		public LinkPort()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="link"></param>
		public LinkPort(Link link) : base(link)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public LinkPort(LinkPort src) : base(src)
		{
			this.radius = src.radius;
		}

		/// <summary>
		/// Serialization constructor for link ports.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected LinkPort(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.radius = info.GetSingle("Radius");
		}

		/// <summary>
		/// 
		/// </summary>
		public abstract int PointIndex
		{
			get;
		}

		/// <summary>
		/// 
		/// </summary>
		public override PointF Location
		{
			get
			{
				Link link = this.Container as Link;
				if (link == null)
				{
					throw new EInvalidOperation();
				}

				IPoints linkPoints = link.Points;
				if (linkPoints == null)
				{
					throw new EInvalidOperation();
				}

				int ptIdx = this.PointIndex;

				if (ptIdx < 0)
				{
					throw new EInvalidOperation();
				}

				return linkPoints.GetPoint(ptIdx);
			}
			set
			{
				PointF loc = value;

				Link link = this.Container as Link;
				if (link == null)
				{
					throw new EInvalidOperation();
				}

				IPoints linkPoints = link.Points;
				if (linkPoints == null)
				{
					throw new EInvalidOperation();
				}

				int ptIdx = this.PointIndex;

				if (ptIdx < 0)
				{
					throw new EInvalidOperation();
				}

				linkPoints.SetPoint(ptIdx, loc);
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
		/// Not allowed for link ports.
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="grfx"></param>
		public override void Draw(System.Drawing.Graphics grfx)
		{
			// Link ports are always invisible
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
			info.AddValue("Radius", this.radius);
		}

		/// <summary>
		/// 
		/// </summary>
		private float radius = 3.0f;
	}

	/// <summary>
	/// Summary description for LinkHeadPort.
	/// </summary>
	[Serializable()]
	public class LinkHeadPort : LinkPort
	{
		/// <summary>
		/// 
		/// </summary>
		public LinkHeadPort()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="link"></param>
		public LinkHeadPort(Link link) : base(link)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public LinkHeadPort(LinkHeadPort src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for link ports.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected LinkHeadPort(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new LinkHeadPort(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public override int PointIndex
		{
			get
			{
				int ptIdx = -1;

				Link link = this.Container as Link;
				if (link != null)
				{
					IPoints linkPoints = link.Points;
					if (linkPoints != null)
					{
						ptIdx = linkPoints.PointCount - 1;
					}
				}

				return ptIdx;
			}
		}
	}

	/// <summary>
	/// Summary description for LinkTailPort.
	/// </summary>
	[Serializable()]
	public class LinkTailPort : LinkPort
	{
		/// <summary>
		/// 
		/// </summary>
		public LinkTailPort()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="link"></param>
		public LinkTailPort(Link link) : base(link)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public LinkTailPort(LinkTailPort src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for link ports.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected LinkTailPort(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new LinkTailPort(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public override int PointIndex
		{
			get
			{
				int ptIdx = -1;

				Link link = this.Container as Link;
				if (link != null)
				{
					IPoints linkPoints = link.Points;
					if (linkPoints != null)
					{
						if (linkPoints.PointCount > 0)
						{
							ptIdx = 0;
						}
					}
				}

				return ptIdx;
			}
		}
	}
}
