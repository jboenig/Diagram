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
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Implements orthogonal lines.
	/// </summary>
	/// <remarks>
	/// <para>
	/// An orthogonal line is a series of line segments that are
	/// joined together with right (90 degree) angles. All of the
	/// line segments in an orthogonal line are parallel to either
	/// the X or Y axis of the world coordinate space.
	/// </para>
	/// <para>
	/// When a vertex is changed, added, or removed from the orthogonal
	/// line, the remaining vertices are recalculated to keep the line
	/// orthogonal.
	/// </para>
	/// </remarks>
	[Serializable]
	public class OrthogonalLine : PolyLine
	{
		private const float DefaultPadding = 20.0f;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public OrthogonalLine()
		{
		}

		/// <summary>
		/// Constructs an orthogonal line given two points.
		/// </summary>
		/// <param name="pt1">First point</param>
		/// <param name="pt2">Second point</param>
		public OrthogonalLine(PointF pt1, PointF pt2)
		{
			CompassHeading headingPt1;
			CompassHeading headingPt2;
			Geometry.CalcEndpointDirections(pt1, pt2, out headingPt1, out headingPt2);

			float padLeft = this.PaddingLeft;
			float padRight = this.PaddingRight;
			float padTop = this.PaddingTop;
			float padBottom = this.PaddingBottom;

			PointF[] pts = Geometry.CalcOrthogonalPoints(pt1, headingPt1, pt2, headingPt2, padLeft, padRight, padTop, padBottom);

			if (pts != null)
			{
				this.grfxPath = this.CreateGraphicsPath(pts);
			}

			// Assign values directly to propertyValues hashtable to avoid
			// firing property change event, which would cause the points
			// to be recalculated.
			this.propertyValues["HeadingEndPoint1"] = headingPt1;
			this.propertyValues["HeadingEndPoint2"] = headingPt2;
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Object to copy</param>
		public OrthogonalLine(OrthogonalLine src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for orthogonal lines.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected OrthogonalLine(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new OrthogonalLine(this);
		}

		/// <summary>
		/// Determines if the compass headings of the endpoints are automatically
		/// calculated.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool AutomaticHeadings
		{
			get
			{
				object value = this.GetPropertyValue("AutomaticHeadings");
				if (value != null)
				{
					return (bool) value;
				}
				return false;
			}
			set
			{
				this.SetPropertyValue("AutomaticHeadings", value);
			}
		}

		/// <summary>
		/// Compass heading of the first endpoint.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The compass heading can be North, South, East, or West. The compass
		/// headings of the first and last endpoints determine how many segments
		/// are needed to join the two endpoints using orthogonal lines.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public CompassHeading HeadingEndPoint1
		{
			get
			{
				object value = this.GetPropertyValue("HeadingEndPoint1");
				if (value != null)
				{
					return (CompassHeading) value;
				}
				return CompassHeading.South;
			}
			set
			{
				this.SetPropertyValue("HeadingEndPoint1", value);
			}
		}

		/// <summary>
		/// Compass heading of the second endpoint.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The compass heading can be North, South, East, or West. The compass
		/// headings of the first and last endpoints determine how many segments
		/// are needed to join the two endpoints using orthogonal lines.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public CompassHeading HeadingEndPoint2
		{
			get
			{
				object value = this.GetPropertyValue("HeadingEndPoint2");
				if (value != null)
				{
					return (CompassHeading) value;
				}
				return CompassHeading.South;
			}
			set
			{
				this.SetPropertyValue("HeadingEndPoint2", value);
			}
		}

		/// <summary>
		/// Length of line segments heading west from the left side of the
		/// line's bounding box.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public float PaddingLeft
		{
			get
			{
				object value = this.GetPropertyValue("PaddingLeft");
				if (value != null)
				{
					return (float) value;
				}
				return OrthogonalLine.DefaultPadding;
			}
			set
			{
				this.SetPropertyValue("PaddingLeft", value);
			}
		}

		/// <summary>
		/// Length of line segments heading north from the top side of the
		/// line's bounding box.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public float PaddingTop
		{
			get
			{
				object value = this.GetPropertyValue("PaddingTop");
				if (value != null)
				{
					return (float) value;
				}
				return OrthogonalLine.DefaultPadding;
			}
			set
			{
				this.SetPropertyValue("PaddingTop", value);
			}
		}

		/// <summary>
		/// Length of line segments heading east from the right side of the
		/// line's bounding box.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public float PaddingRight
		{
			get
			{
				object value = this.GetPropertyValue("PaddingRight");
				if (value != null)
				{
					return (float) value;
				}
				return OrthogonalLine.DefaultPadding;
			}
			set
			{
				this.SetPropertyValue("PaddingRight", value);
			}
		}

		/// <summary>
		/// Length of line segments heading south from the bottom side of the
		/// line's bounding box.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public float PaddingBottom
		{
			get
			{
				object value = this.GetPropertyValue("PaddingBottom");
				if (value != null)
				{
					return (float) value;
				}
				return OrthogonalLine.DefaultPadding;
			}
			set
			{
				this.SetPropertyValue("PaddingBottom", value);
			}
		}

		/// <summary>
		/// Minimum number of points this shape can have.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override int MinPoints
		{
			get
			{
				return 2;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		public override void SetPoints(PointF[] pts)
		{
			if (pts == null)
			{
				this.grfxPath.Dispose();
				this.grfxPath = null;
				return;
			}

			if (pts.Length < 2)
			{
				throw new EInvalidParameter();
			}

			PointF[] orthogonalPts = this.MakeOrthogonal(pts);

			if (orthogonalPts != null)
			{
				this.grfxPath = this.CreateGraphicsPath(orthogonalPts);
			}
			else
			{
				this.grfxPath = this.CreateGraphicsPath(pts);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptIdx"></param>
		/// <param name="val"></param>
		public override void SetPoint(int ptIdx, PointF val)
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

					PointF[] orthogonalPts = this.MakeOrthogonal(pathPts);

					if (orthogonalPts != null)
					{
						this.grfxPath = CreateGraphicsPath(orthogonalPts);
					}
					else
					{
						this.grfxPath = CreateGraphicsPath(pathPts);
					}
				}
				this.OnMoveVertex(new VertexEventArgs(this, ptIdx));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		public override void AddPoint(PointF val)
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

			PointF[] orthogonalPts = this.MakeOrthogonal(newPts);

			if (orthogonalPts != null)
			{
				this.grfxPath = CreateGraphicsPath(orthogonalPts);
			}
			else
			{
				this.grfxPath = CreateGraphicsPath(newPts);
			}

			this.OnInsertVertex(new VertexEventArgs(this, numPts));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptIdx"></param>
		/// <param name="val"></param>
		public override void InsertPoint(int ptIdx, PointF val)
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


			PointF[] orthogonalPts = this.MakeOrthogonal(newPts);

			if (orthogonalPts != null)
			{
				this.grfxPath = CreateGraphicsPath(orthogonalPts);
			}
			else
			{
				this.grfxPath = CreateGraphicsPath(newPts);
			}

			this.OnInsertVertex(new VertexEventArgs(this, ptIdx));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptIdx"></param>
		public override void RemovePoint(int ptIdx)
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

			PointF[] orthogonalPts = this.MakeOrthogonal(newPts);

			if (orthogonalPts != null)
			{
				this.grfxPath = CreateGraphicsPath(orthogonalPts);
			}
			else
			{
				this.grfxPath = CreateGraphicsPath(newPts);
			}

			this.OnDeleteVertex(new VertexEventArgs(this, ptIdx));
		}

		/// <summary>
		/// Sets the default property values for the line.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// line to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("AutomaticHeadings", true);
			this.propertyValues.Add("HeadingEndPoint1", CompassHeading.East);
			this.propertyValues.Add("HeadingEndPoint2", CompassHeading.West);
			this.propertyValues.Add("PaddingLeft", OrthogonalLine.DefaultPadding);
			this.propertyValues.Add("PaddingTop", OrthogonalLine.DefaultPadding);
			this.propertyValues.Add("PaddingRight", OrthogonalLine.DefaultPadding);
			this.propertyValues.Add("PaddingBottom", OrthogonalLine.DefaultPadding);
		}

		/// <summary>
		/// Called when a property is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
		/// of the property change by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.PropertyChanged"/>
		/// method.
		/// </para>
		/// <para>
		/// This method also recalculates the points in the orthogonal line if one of
		/// the following properties was changed:
		/// <see cref="Syncfusion.Windows.Forms.Diagram.OrthogonalLine.HeadingEndPoint1"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.OrthogonalLine.HeadingEndPoint2"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.OrthogonalLine.PaddingLeft"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.OrthogonalLine.PaddingTop"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.OrthogonalLine.PaddingRight"/>,
		/// <see cref="Syncfusion.Windows.Forms.Diagram.OrthogonalLine.PaddingBottom"/>.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected override void OnPropertyChanged(PropertyEventArgs evtArgs)
		{
			switch (evtArgs.PropertyName)
			{
				case "HeadingEndPoint1":
				case "HeadingEndPoint2":
				case "PaddingLeft":
				case "PaddingTop":
				case "PaddingRight":
				case "PaddingBottom":
					this.MakeOrthogonal(this.HeadingEndPoint1, this.HeadingEndPoint2, this.PaddingLeft, this.PaddingRight, this.PaddingTop, this.PaddingBottom);
					break;
			}

			base.OnPropertyChanged(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		protected PointF[] MakeOrthogonal(PointF[] pts)
		{
			if (pts == null)
			{
				return null;
			}

			int numPts = pts.Length;
			bool headingsChanged = false;
			PointF[] orthogonalPts = null;

			if (this.AutomaticHeadings)
			{
				CompassHeading headingPt1 = (CompassHeading) this.propertyValues["HeadingEndPoint1"];
				CompassHeading headingPt2 = (CompassHeading) this.propertyValues["HeadingEndPoint2"];
				CompassHeading newHeadingPt1;
				CompassHeading newHeadingPt2;

				Geometry.CalcEndpointDirections(pts[0], pts[numPts-1], out newHeadingPt1, out newHeadingPt2);

				if (newHeadingPt1 != headingPt1)
				{
					this.propertyValues["HeadingEndPoint1"] = newHeadingPt1;
					headingsChanged = true;
				}

				if (newHeadingPt2 != headingPt2)
				{
					this.propertyValues["HeadingEndPoint2"] = newHeadingPt2;
					headingsChanged = true;
				}
			}

			if (headingsChanged)
			{
				orthogonalPts = Geometry.CalcOrthogonalPoints(pts[0],
					this.HeadingEndPoint1,
					pts[numPts-1],
					this.HeadingEndPoint2,
					this.PaddingLeft,
					this.PaddingRight,
					this.PaddingTop,
					this.PaddingBottom);
			}
			else if (!Geometry.IsOrthogonalLine(pts))
			{
				orthogonalPts = Geometry.CalcOrthogonalPoints(pts[0],
					this.HeadingEndPoint1,
					pts[numPts-1],
					this.HeadingEndPoint2,
					this.PaddingLeft,
					this.PaddingRight,
					this.PaddingTop,
					this.PaddingBottom);
			}
			else
			{
				orthogonalPts = pts;
			}

			return orthogonalPts;
		}
	}
}
