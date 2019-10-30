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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Used to specify a logical end point of a line.
	/// </summary>
	public enum LineEndPoint
	{
		/// <summary>
		/// First endpoint
		/// </summary>
		First,
		/// <summary>
		/// Last endpoint
		/// </summary>
		Last
	}

	/// <summary>
	/// Specifies one of 9 relative positions on a box.
	/// </summary>
	public enum BoxPosition
	{
		/// <summary>
		/// Top left point of the box
		/// </summary>
		TopLeft,
		/// <summary>
		/// Top center point of the box
		/// </summary>
		TopCenter,
		/// <summary>
		/// Top right point of the box
		/// </summary>
		TopRight,
		/// <summary>
		/// Middle left point of the box
		/// </summary>
		MiddleLeft,
		/// <summary>
		/// Center of the box
		/// </summary>
		Center,
		/// <summary>
		/// Middle left point of the box
		/// </summary>
		MiddleRight,
		/// <summary>
		/// Bottom left point of the box
		/// </summary>
		BottomLeft,
		/// <summary>
		/// Bottom center point of the box
		/// </summary>
		BottomCenter,
		/// <summary>
		/// Bottom right point of the box
		/// </summary>
		BottomRight
	}

	/// <summary>
	/// The BoxOrientation specifies a vertical or horizontal line passing
	/// through one of the four edges or through the center of a box.
	/// </summary>
	public enum BoxOrientation
	{
		/// <summary>
		/// Left edge of the box
		/// </summary>
		Left,
		/// <summary>
		/// Top edge of the box
		/// </summary>
		Top,
		/// <summary>
		/// Right edge of the box
		/// </summary>
		Right,
		/// <summary>
		/// Bottom edge of the box
		/// </summary>
		Bottom,
		/// <summary>
		/// Horizontal center
		/// </summary>
		Middle,
		/// <summary>
		/// Vertical center
		/// </summary>
		Center
	}

	/// <summary>
	/// Specifies a compass heading.
	/// </summary>
	/// <remarks>
	/// This enumeration is used to specify the direction of a vector. A vector
	/// can be generated given one point and a CompassHeading.
	/// </remarks>
	public enum CompassHeading
	{
		/// <summary>
		/// North (up)
		/// </summary>
		North,

		/// <summary>
		/// South (down)
		/// </summary>
		South,

		/// <summary>
		/// East (right)
		/// </summary>
		East,

		/// <summary>
		/// West (left)
		/// </summary>
		West,

		/// <summary>
		/// Northwest (Up and left)
		/// </summary>
		Northwest,

		/// <summary>
		/// Northeast (Up and right)
		/// </summary>
		Northeast,

		/// <summary>
		/// Southwest (Down and left)
		/// </summary>
		Southwest,

		/// <summary>
		/// Southeast (Down and right)
		/// </summary>
		Southeast
	}

	/// <summary>
	/// Contains static declarations for functions and constants used for
	/// performing calculations on points, lines, and rectangles.
	/// </summary>
	public class Geometry
	{
		/// <summary>
		/// Angle of quadrant 1 for a unit circle in radians.
		/// </summary>
		public static double RadiansQuadrant1 = (Math.PI / 2.0);

		/// <summary>
		/// Angle of quadrant 2 for a unit circle in radians.
		/// </summary>
		public static double RadiansQuadrant2 = Math.PI;

		/// <summary>
		/// Angle of quadrant 3 for a unit circle in radians.
		/// </summary>
		public static double RadiansQuadrant3 = (3.0 * Math.PI) / 2.0;

		/// <summary>
		/// Angle of quadrant 4 for a unit circle in radians.
		/// </summary>
		public static double RadiansQuadrant4 = (2.0 * Math.PI);

		/// <summary>
		/// Create a rectangle from 2 points.
		/// </summary>
		/// <param name="pt1">First point</param>
		/// <param name="pt2">Second point</param>
		/// <returns>Rectangle</returns>
		public static System.Drawing.Rectangle CreateRect(Point pt1, Point pt2)
		{
			int swapVal;
			System.Drawing.Point ptUpperLeft = pt1;
			System.Drawing.Point ptLowerRight = pt2;
			if (ptUpperLeft.X > ptLowerRight.X)
			{
				swapVal = ptUpperLeft.X;
				ptUpperLeft.X = ptLowerRight.X;
				ptLowerRight.X = swapVal;
			}
			if (ptUpperLeft.Y > ptLowerRight.Y)
			{
				swapVal = ptUpperLeft.Y;
				ptUpperLeft.Y = ptLowerRight.Y;
				ptLowerRight.Y = swapVal;
			}
			return new System.Drawing.Rectangle(ptUpperLeft.X, ptUpperLeft.Y, ptLowerRight.X - ptUpperLeft.X, ptLowerRight.Y - ptUpperLeft.Y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static System.Drawing.RectangleF CreateRect(PointF pt1, PointF pt2)
		{
			float swapVal;
			PointF ptUpperLeft = pt1;
			PointF ptLowerRight = pt2;
			if (ptUpperLeft.X > ptLowerRight.X)
			{
				swapVal = ptUpperLeft.X;
				ptUpperLeft.X = ptLowerRight.X;
				ptLowerRight.X = swapVal;
			}
			if (ptUpperLeft.Y > ptLowerRight.Y)
			{
				swapVal = ptUpperLeft.Y;
				ptUpperLeft.Y = ptLowerRight.Y;
				ptLowerRight.Y = swapVal;
			}
			return new System.Drawing.RectangleF(ptUpperLeft.X, ptUpperLeft.Y, ptLowerRight.X - ptUpperLeft.X, ptLowerRight.Y - ptUpperLeft.Y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static System.Drawing.PointF CenterPoint(RectangleF rect)
		{
			float left = rect.Left;
			float right = rect.Right;
			float top = rect.Top;
			float bottom = rect.Bottom;

			float x;
			float y;

			x = (left + right) / 2.0f;
			y = (top + bottom) / 2.0f;

			return new PointF(x, y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static System.Drawing.Point CenterPoint(System.Drawing.Rectangle rect)
		{
			int left = rect.Left;
			int right = rect.Right;
			int top = rect.Top;
			int bottom = rect.Bottom;

			int x;
			int y;

			x = (left + right) / 2;
			y = (top + bottom) / 2;

			return new Point(x, y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="anchor"></param>
		/// <returns></returns>
		public static System.Drawing.PointF GetAnchorPoint(RectangleF rect, BoxPosition anchor)
		{
			float left = rect.Left;
			float right = rect.Right;
			float top = rect.Top;
			float bottom = rect.Bottom;

			float x = 0;
			float y = 0;

			switch (anchor)
			{
				case BoxPosition.TopLeft:
					x = left;
					y = top;
					break;

				case BoxPosition.TopCenter:
					x = (left + right) / 2;
					y = top;
					break;

				case BoxPosition.TopRight:
					x = right;
					y = top;
					break;

				case BoxPosition.MiddleLeft:
					x = left;
					y = (top + bottom) / 2;
					break;

				case BoxPosition.Center:
					x = (left + right) / 2;
					y = (top + bottom) / 2;
					break;

				case BoxPosition.MiddleRight:
					x = right;
					y = (top + bottom) / 2;
					break;

				case BoxPosition.BottomLeft:
					x = left;
					y = bottom;
					break;

				case BoxPosition.BottomCenter:
					x = (left + right) / 2;
					y = bottom;
					break;

				case BoxPosition.BottomRight:
					x = right;
					y = bottom;
					break;
			}

			return new PointF(x, y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static System.Drawing.RectangleF CreateRect(PointF[] pts)
		{
			float left = float.MaxValue;
			float top = float.MaxValue;
			float right = float.MinValue;
			float bottom = float.MinValue;

			int numPts = pts.Length;
			float curX, curY;
			for (int ptIdx = 0; ptIdx < numPts; ptIdx++)
			{
				curX = pts[ptIdx].X;
				curY = pts[ptIdx].Y;
				if (curX < left)
				{
					left = curX;
				}
				if (curX > right)
				{
					right = curX;
				}
				if (curY < top)
				{
					top = curY;
				}
				if (curY > bottom)
				{
					bottom = curY;
				}
			}

			return new System.Drawing.RectangleF(left, top, right - left, bottom - top);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <param name="rcBounds"></param>
		/// <param name="startAngle"></param>
		/// <param name="sweepAngle"></param>
		public static void ArcFromPoints(System.Drawing.Point pt1,
			System.Drawing.Point pt2,
			out System.Drawing.Rectangle rcBounds,
			out float startAngle,
			out float sweepAngle)
		{
			rcBounds = new System.Drawing.Rectangle(0,0,0,0);
			int diffX = pt2.X - pt1.X;
			int diffY = pt2.Y - pt1.Y;
			int width = diffX * 2;
			int height = diffY * 2;
			if (width > 0 && height > 0)
			{
				rcBounds.Location = new System.Drawing.Point(pt1.X - diffX, pt1.Y);
				rcBounds.Size = new System.Drawing.Size(width, height);
				startAngle = 0;
				sweepAngle = -90;
			}
			else if (width > 0 && height <= 0)
			{
				rcBounds.Location = new System.Drawing.Point(pt1.X, pt2.Y);
				rcBounds.Size = new System.Drawing.Size(width, -height);
				startAngle = 180;
				sweepAngle = 90;
			}
			else if (width <= 0 && height > 0)
			{
				rcBounds.Location = new System.Drawing.Point(pt2.X + diffX, pt1.Y - diffY);
				rcBounds.Size = new System.Drawing.Size(-width, height);
				startAngle = 0;
				sweepAngle = 90;
			}
			else
			{
				rcBounds.Location = new System.Drawing.Point(pt2.X, pt2.Y + diffY);
				rcBounds.Size = new System.Drawing.Size(-width, -height);
				startAngle = 180;
				sweepAngle = -90;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <param name="rcBounds"></param>
		/// <param name="startAngle"></param>
		/// <param name="sweepAngle"></param>
		public static void ArcFromPoints(System.Drawing.PointF pt1,
			System.Drawing.PointF pt2,
			out System.Drawing.RectangleF rcBounds,
			out float startAngle,
			out float sweepAngle)
		{
			rcBounds = new System.Drawing.RectangleF(0.0f,0.0f,0.0f,0.0f);
			float diffX = pt2.X - pt1.X;
			float diffY = pt2.Y - pt1.Y;
			float width = diffX * 2;
			float height = diffY * 2;
			if (width > 0.0f && height > 0.0f)
			{
				rcBounds.Location = new System.Drawing.PointF(pt1.X - diffX, pt1.Y);
				rcBounds.Size = new System.Drawing.SizeF(width, height);
				startAngle = 0.0f;
				sweepAngle = -90.0f;
			}
			else if (width > 0.0f && height <= 0.0f)
			{
				rcBounds.Location = new System.Drawing.PointF(pt1.X, pt2.Y);
				rcBounds.Size = new System.Drawing.SizeF(width, -height);
				startAngle = 180.0f;
				sweepAngle = 90.0f;
			}
			else if (width <= 0.0f && height > 0.0f)
			{
				rcBounds.Location = new System.Drawing.PointF(pt2.X + diffX, pt1.Y - diffY);
				rcBounds.Size = new System.Drawing.SizeF(-width, height);
				startAngle = 0.0f;
				sweepAngle = 90.0f;
			}
			else
			{
				rcBounds.Location = new System.Drawing.PointF(pt2.X, pt2.Y + diffY);
				rcBounds.Size = new System.Drawing.SizeF(-width, -height);
				startAngle = 180.0f;
				sweepAngle = -90.0f;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static double PointDistance(PointF pt1, PointF pt2)
		{
			double d = 0;
			double dx = pt2.X - pt1.X;
			double dy = pt2.Y - pt1.Y;
			double t = (dx*dx) + (dy*dy);
			if (t > 0)
			{
				d = Math.Sqrt(t);
			}
			return d;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static double LineSlope(PointF pt1, PointF pt2)
		{
			double m = 0;
			double dx = pt2.X - pt1.X;
			double dy = pt2.Y - pt1.Y;
			if (dx != 0)
			{
				m = dy / dx;
			}
			else
			{
				throw new ESlopeUndefined();
			}
			return m;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static double LineSlope(System.Drawing.Point pt1, System.Drawing.Point pt2)
		{
			double m = 0;
			double dx = (double) pt2.X - (double) pt1.X;
			double dy = (double) pt2.Y - (double) pt1.Y;
			if (dx != 0)
			{
				m = dy / dx;
			}
			else
			{
				throw new ESlopeUndefined();
			}
			return m;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <param name="rect"></param>
		/// <param name="ptsIntersect"></param>
		/// <returns></returns>
		public static int GetLineIntersect(PointF pt1, PointF pt2, RectangleF rect, out PointF[] ptsIntersect)
		{
			int intersectCount = 0;
			ptsIntersect = null;

			bool isVerticalLine = false;
			double m = 0;
			float xIntercept;
			float yIntercept;
			PointF[] ptsFound = new PointF[4];
			float rcTop = rect.Top;
			float rcBottom = rect.Bottom;
			float rcLeft = rect.Left;
			float rcRight = rect.Right;

			if ((pt2.X - pt1.X) != 0)
			{
				m = LineSlope(pt1, pt2);
			}
			else
			{
				isVerticalLine = true;
			}

			// Test top side

			if ((pt1.Y <= rcTop && pt2.Y >= rcTop) || (pt2.Y <= rcTop && pt1.Y >= rcTop))
			{
				if (isVerticalLine)
				{
					xIntercept = pt1.X;
				}
				else
				{
					xIntercept = (float)((rcTop + (m * pt1.X - pt1.Y)) / m);
				}

				if (xIntercept >= rcLeft && xIntercept <= rcRight)
				{
					ptsFound[intersectCount] = new PointF(xIntercept, rcTop);
					intersectCount++;
				}
			}

			// Test bottom side

			if ((pt1.Y <= rcBottom && pt2.Y >= rcBottom) || (pt2.Y <= rcBottom && pt1.Y >= rcBottom))
			{
				if (isVerticalLine)
				{
					xIntercept = pt1.X;
				}
				else
				{
					xIntercept = (float)((rcBottom + (m * pt1.X - pt1.Y)) / m);
				}

				if (xIntercept >= rcLeft && xIntercept <= rcRight)
				{
					ptsFound[intersectCount] = new PointF(xIntercept, rcBottom);
					intersectCount++;
				}
			}

			// Test left side

			if (!isVerticalLine && (pt1.X <= rcLeft && pt2.X >= rcLeft) || (pt2.X <= rcLeft && pt1.X >= rcLeft))
			{
				yIntercept = (float)(m * (rcLeft - pt1.X) + pt1.Y);

				if (yIntercept >= rcTop && yIntercept <= rcBottom)
				{
					ptsFound[intersectCount] = new PointF(rcLeft, yIntercept);
					intersectCount++;
				}
			}

			// Test right side

			if (!isVerticalLine && (pt1.X <= rcRight && pt2.X >= rcRight) || (pt2.X <= rcRight && pt1.X >= rcRight))
			{
				yIntercept = (float)(m * (rcRight - pt1.X) + pt1.Y);

				if (yIntercept >= rcTop && yIntercept <= rcBottom)
				{
					ptsFound[intersectCount] = new PointF(rcRight, yIntercept);
					intersectCount++;
				}
			}

			if (intersectCount > 0)
			{
				ptsIntersect = new PointF[intersectCount];
				for (int ptIdx = 0; ptIdx < intersectCount; ptIdx++)
				{
					ptsIntersect[ptIdx] = ptsFound[ptIdx];
				}
			}

			return intersectCount;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt0"></param>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static int CounterClockwise(PointF pt0, PointF pt1, PointF pt2)
		{
			float dx1 = pt1.X - pt0.X;
			float dx2 = pt2.X - pt0.X;
			float dy1 = pt1.Y - pt0.Y;
			float dy2 = pt2.Y - pt0.Y;
			return ((dx1*dy2 > dy1*dx2) ? 1 : -1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt0"></param>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <param name="pt3"></param>
		/// <returns></returns>
		public static bool LinesIntersect(PointF pt0, PointF pt1, PointF pt2, PointF pt3)
		{
			return (((Geometry.CounterClockwise(pt0,pt1,pt2) * Geometry.CounterClockwise(pt0,pt1,pt3)) <= 0) &&
			        ((Geometry.CounterClockwise(pt2,pt3,pt0) * Geometry.CounterClockwise(pt2,pt3,pt1)  <= 0)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static double LineAngle(PointF pt1, PointF pt2)
		{
			double radians;
			double vx = pt2.X - pt1.X;
			double vy = pt2.Y - pt1.Y;

			radians = Math.Atan(vy/vx);

			if (vx < 0 && vy > 0)
			{
				// quandrant 2
				radians = RadiansQuadrant2 + radians;
			}
			else if (vx < 0 && vy < 0)
			{
				// quandrant 3
				radians = RadiansQuadrant2 + radians;
			}
			else if (vx > 0 && vy < 0)
			{
				// quandrant 4
				radians = RadiansQuadrant4 + radians;
			}
			return radians;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		public static double LineAngle(System.Drawing.Point pt1, System.Drawing.Point pt2)
		{
			double radians;
			double vx = (double) pt2.X - pt1.X;
			double vy = (double) pt2.Y - pt1.Y;

			radians = Math.Atan(vy/vx);

			if (vx < 0 && vy > 0)
			{
				// quandrant 2
				radians = RadiansQuadrant2 + radians;
			}
			else if (vx < 0 && vy < 0)
			{
				// quandrant 3
				radians = RadiansQuadrant2 + radians;
			}
			else if (vx > 0 && vy < 0)
			{
				// quandrant 4
				radians = RadiansQuadrant4 + radians;
			}
			return radians;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptA"></param>
		/// <param name="ptO"></param>
		/// <param name="ptB"></param>
		/// <returns></returns>
		public static double LineAngle(System.Drawing.Point ptA, System.Drawing.Point ptO, System.Drawing.Point ptB)
		{
			double m1;
			double m2;

			try
			{
				m1 = LineSlope(ptO, ptA);
			}
			catch (ESlopeUndefined)
			{
				m1 = 1; // TODO: this is a temporary hack
			}

			try
			{
				m2 = LineSlope(ptO, ptB);
			}
			catch (ESlopeUndefined)
			{
				m2 = 1; // TODO: this is a temporary hack
			}

			double d = 1 + (m1 * m2);

			if (d == 0.0)
			{
				throw new EInvalidOperation();
			}

			return Math.Atan((m2 - m1) / d);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="radians"></param>
		/// <returns></returns>
		public static int AngleQuadrant(double radians)
		{
			if (radians <= RadiansQuadrant1)
			{
				return 1;
			}
			else if (radians < RadiansQuadrant2)
			{
				return 2;
			}
			else if (radians < RadiansQuadrant3)
			{
				return 3;
			}
			else if (radians < RadiansQuadrant4)
			{
				return 4;
			}
			return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rgn"></param>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <param name="ptIntercept"></param>
		/// <returns></returns>
		public static bool GetBoundaryIntercept(System.Drawing.Region rgn, PointF pt1, PointF pt2, out PointF ptIntercept)
		{
			bool hasIntercept = false;
			ptIntercept = new PointF(0,0);

			bool pt1InRegion = rgn.IsVisible(pt1);
			bool pt2InRegion = rgn.IsVisible(pt2);

			if ((pt1InRegion && !pt2InRegion) || (pt2InRegion && !pt1InRegion))
			{
				PointF ptInside;
				PointF ptOutside;

				if (pt1InRegion)
				{
					ptInside = pt1;
					ptOutside = pt2;
				}
				else
				{
					ptInside = pt2;
					ptOutside = pt1;
				}

				RectangleF[] rcScans = rgn.GetRegionScans(new Matrix());
				RectangleF rcCur;
				PointF[] ptsIntersect;
				int numIntersects;
				double minDist = double.MaxValue;
				double curDist;
				PointF curPt;
				int rcIdx;
				int ptIdx;

				for (rcIdx = 0; rcIdx < rcScans.Length; rcIdx++)
				{
					rcCur = rcScans[rcIdx];
					numIntersects = Geometry.GetLineIntersect(ptOutside, ptInside, rcCur, out ptsIntersect);

					for (ptIdx = 0; ptIdx < numIntersects; ptIdx++)
					{
						curPt = ptsIntersect[ptIdx];
						curDist = Geometry.PointDistance(ptOutside, curPt);
						if (curDist < minDist)
						{
							ptIntercept = curPt;
							minDist = curDist;
							hasIntercept = true;
						}
					}
				}
			}

			return hasIntercept;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect1"></param>
		/// <param name="rect2"></param>
		/// <returns></returns>
		public static RectangleF Union(RectangleF rect1, RectangleF rect2)
		{
			float left;
			float top;
			float right;
			float bottom;

			if (rect1.Left < rect2.Left)
			{
				left = rect1.Left;
			}
			else
			{
				left = rect2.Left;
			}

			if (rect1.Right > rect2.Right)
			{
				right = rect1.Right;
			}
			else
			{
				right = rect2.Right;
			}

			if (rect1.Top < rect2.Top)
			{
				top = rect1.Top;
			}
			else
			{
				top = rect2.Top;
			}

			if (rect1.Bottom > rect2.Bottom)
			{
				bottom = rect1.Bottom;
			}
			else
			{
				bottom = rect2.Bottom;
			}

			return new RectangleF(left, top, right-left, bottom-top);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect1"></param>
		/// <param name="rect2"></param>
		/// <returns></returns>
		public static System.Drawing.Rectangle Union(System.Drawing.Rectangle rect1, System.Drawing.Rectangle rect2)
		{
			int left;
			int top;
			int right;
			int bottom;

			if (rect1.Left < rect2.Left)
			{
				left = rect1.Left;
			}
			else
			{
				left = rect2.Left;
			}

			if (rect1.Right > rect2.Right)
			{
				right = rect1.Right;
			}
			else
			{
				right = rect2.Right;
			}

			if (rect1.Top < rect2.Top)
			{
				top = rect1.Top;
			}
			else
			{
				top = rect2.Top;
			}

			if (rect1.Bottom > rect2.Bottom)
			{
				bottom = rect1.Bottom;
			}
			else
			{
				bottom = rect2.Bottom;
			}

			return new System.Drawing.Rectangle(left, top, right-left, bottom-top);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nodes"></param>
		/// <returns></returns>
		public static System.Drawing.RectangleF GetAggregateBounds(NodeCollection nodes)
		{
			RectangleF rcBounds = new RectangleF(0,0,0,0);
			bool firstNode = true;

			foreach (INode node in nodes)
			{
				IBounds2DF nodeBounds = node as IBounds2DF;
				if (nodeBounds != null)
				{
					if (firstNode)
					{
						rcBounds = nodeBounds.Bounds;
						firstNode = false;
					}
					else
					{
						rcBounds = Geometry.Union(rcBounds, nodeBounds.Bounds);
					}
				}
			}

			return rcBounds;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static double CalcLineLength(IPoints pts)
		{
			if (pts == null)
			{
				throw new EInvalidParameter();
			}

			double length = 0.0f;
			int vertexIdx1 = 0;
			int vertexIdx2 = 1;
			PointF vertex1;
			PointF vertex2;
			int numPts = pts.PointCount;

			while (vertexIdx2 < numPts)
			{
				vertex1 = pts.GetPoint(vertexIdx1);
				vertex2 = pts.GetPoint(vertexIdx2);
				length += Geometry.PointDistance(vertex1, vertex2);
				vertexIdx1++;
				vertexIdx2++;
			}

			return length;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="percent"></param>
		/// <param name="ptReturn"></param>
		/// <param name="vertexIdx1"></param>
		/// <param name="vertexIdx2"></param>
		/// <returns></returns>
		public static bool CalcPercentageAlong(IPoints pts, int percent, out System.Drawing.PointF ptReturn, out int vertexIdx1, out int vertexIdx2)
		{
			bool found = false;

			if (pts == null)
			{
				throw new EInvalidParameter();
			}

			if (percent < 0 || percent > 100)
			{
				throw new EInvalidParameter();
			}

			int numPts = pts.PointCount;
			double totalLength = Geometry.CalcLineLength(pts);
			double lengthAlong = totalLength * (percent / 100.0f);
			double lengthSoFar = 0.0f;
			PointF vertex1;
			PointF vertex2;
			double curSegmentLength;
			double curSegmentPct;

			vertexIdx1 = 0;
			vertexIdx2 = 1;
			ptReturn = new PointF(0,0);

			while (vertexIdx2 < numPts && !found)
			{
				vertex1 = pts.GetPoint(vertexIdx1);
				vertex2 = pts.GetPoint(vertexIdx2);
				curSegmentLength = Geometry.PointDistance(vertex1, vertex2);
				if (curSegmentLength > 0.0f && (lengthSoFar + curSegmentLength) >= lengthAlong)
				{
					// calculate percentage along this segment
					curSegmentPct = (lengthAlong - lengthSoFar) * 100.0f / curSegmentLength;

					// find the point on the segment
					double ptX = vertex1.X + (curSegmentPct * (vertex2.X - vertex1.X) / 100.0f);
					double ptY = vertex1.Y + (curSegmentPct * (vertex2.Y - vertex1.Y) / 100.0f);
					ptReturn = new PointF((float) ptX, (float) ptY);

					// Exit loop and return
					found = true;
				}
				else
				{
					vertexIdx1++;
					vertexIdx2++;
				}
			}

			return found;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="heading"></param>
		/// <returns></returns>
		public static System.Drawing.Size CompassHeadingToVector(CompassHeading heading)
		{
			System.Drawing.Size szVector = new System.Drawing.Size(0,0);

			switch (heading)
			{
				case CompassHeading.East:
					szVector.Width = 1;
					break;

				case CompassHeading.West:
					szVector.Width = -1;
					break;

				case CompassHeading.North:
					szVector.Height = -1;
					break;

				case CompassHeading.South:
					szVector.Height = 1;
					break;

				case CompassHeading.Northeast:
					szVector.Height = -1;
					szVector.Width = 1;
					break;

				case CompassHeading.Northwest:
					szVector.Height = -1;
					szVector.Width = -1;
					break;

				case CompassHeading.Southeast:
					szVector.Height = 1;
					szVector.Width = 1;
					break;

				case CompassHeading.Southwest:
					szVector.Height = 1;
					szVector.Width = -1;
					break;
			}

			return szVector;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static bool IsOrthogonalLine(PointF[] pts)
		{
			bool orthogonal = true;

			if (pts != null && pts.Length > 0)
			{
				PointF ptPrev = pts[0];
				PointF ptCur;

				for (int ptIdx = 1; orthogonal && ptIdx < pts.Length; ptIdx++)
				{
					ptCur = pts[ptIdx];

					if (ptCur.X != ptPrev.X && ptCur.Y != ptPrev.Y)
					{
						orthogonal = false;
					}
					else
					{
						ptPrev = ptCur;
					}
				}
			}

			return orthogonal;
		}

		private static BoxPosition[,] orthogonalCtlPts = new BoxPosition[,]
		{
			{ BoxPosition.TopLeft,    BoxPosition.TopLeft,    BoxPosition.TopCenter,    BoxPosition.TopRight,    BoxPosition.TopRight },
			{ BoxPosition.TopLeft,    BoxPosition.TopLeft,    BoxPosition.TopCenter,    BoxPosition.TopRight,    BoxPosition.TopRight },
			{ BoxPosition.MiddleLeft, BoxPosition.MiddleLeft, BoxPosition.Center,       BoxPosition.MiddleRight, BoxPosition.MiddleRight },
			{ BoxPosition.BottomLeft, BoxPosition.BottomLeft, BoxPosition.BottomCenter, BoxPosition.BottomRight, BoxPosition.BottomRight },
			{ BoxPosition.BottomLeft, BoxPosition.BottomLeft, BoxPosition.BottomCenter, BoxPosition.BottomRight, BoxPosition.BottomRight }
		};

		/// <summary>
		/// Returns a point orthogonal to the two endpoints of the line.
		/// </summary>
		/// <param name="ptBegin"></param>
		/// <param name="ptEnd"></param>
		/// <param name="row">Row number of point to calculate</param>
		/// <param name="col">Column number of point to calculate</param>
		/// <param name="padLeft"></param>
		/// <param name="padRight"></param>
		/// <param name="padTop"></param>
		/// <param name="padBottom"></param>
		/// <returns>A logical point that is orthogonal to the two line endpoints</returns>
		/// <remarks>
		/// The row and column are used to index a 5x5 matrix of points that
		/// surround the two endpoints. All of the points in the matrix are orthogonal
		/// to the endpoints. The matrix can be thought of as two rectangles and a
		/// point in the center. The outer rectangle contains 16 points and the inner
		/// rectangle contains 8 points (16+8+1=25). The endpoints always lie in
		/// either the 2nd or 4th column and the 2nd or 4th row. In other words, the
		/// endpoints are always two of the four corners of the inner rectangle. The
		/// outer rectangle is calculated by inflating the inner rectangle using the
		/// padding value passed in.		
		/// </remarks>
		public static PointF GetOrthogonalPoint(PointF ptBegin, PointF ptEnd, int row, int col, float padLeft, float padRight, float padTop, float padBottom)
		{
			PointF orthogonalPt = new PointF(0,0);

			RectangleF rcInner = Geometry.CreateRect(ptBegin, ptEnd);
			float outerLeft = rcInner.Left - padLeft;
			float outerTop = rcInner.Top - padTop;
			float outerWidth = rcInner.Width + padLeft + padRight;
			float outerHeight = rcInner.Height + padTop + padBottom;
			RectangleF rcOuter = new RectangleF(outerLeft, outerTop, outerWidth, outerHeight);
			RectangleF rcOuterHorz = new RectangleF(outerLeft, rcInner.Top, outerWidth, rcInner.Height);
			RectangleF rcOuterVert = new RectangleF(rcInner.Left, outerTop, rcInner.Width, outerHeight);
			BoxPosition ctlPt = Geometry.orthogonalCtlPts[row,col];

			if ((row < 1 || row > 3) && (col < 1 || col > 3))
			{
				// One of the outer corners
				orthogonalPt = Geometry.GetAnchorPoint(rcOuter, ctlPt);
			}
			else if (row < 1 || row > 3)
			{
				// Top or bottom row
				orthogonalPt = Geometry.GetAnchorPoint(rcOuterVert, ctlPt);
			}
			else if (col < 1 || col > 3)
			{
				// Left or right column
				orthogonalPt = Geometry.GetAnchorPoint(rcOuterHorz, ctlPt);
			}
			else
			{
				// Inner rectangle
				orthogonalPt = Geometry.GetAnchorPoint(rcInner, ctlPt);
			}

			return orthogonalPt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptEnd1"></param>
		/// <param name="endPt1Heading"></param>
		/// <param name="ptEnd2"></param>
		/// <param name="endPt2Heading"></param>
		/// <param name="padLeft"></param>
		/// <param name="padRight"></param>
		/// <param name="padTop"></param>
		/// <param name="padBottom"></param>
		/// <returns></returns>
		public static PointF[] CalcOrthogonalPoints(PointF ptEnd1, CompassHeading endPt1Heading, PointF ptEnd2, CompassHeading endPt2Heading, float padLeft, float padRight, float padTop, float padBottom)
		{
			System.Collections.ArrayList ptsOut = new System.Collections.ArrayList();

			System.Drawing.Size frontHeadingVector = Geometry.CompassHeadingToVector(endPt1Heading);
			System.Drawing.Size backHeadingVector = Geometry.CompassHeadingToVector(endPt2Heading);

			// Maximum of 6 points from a 25x25 grid
			int[] row = new int[] {2, -1, -1, -1, -1, 2};
			int[] col = new int[] {2, -1, -1, -1, -1, 2};
			int front = 0;
			int back = 5;

			// Determine the row and column in the grid
			// for each endpoint
			if (ptEnd1.X < ptEnd2.X)
			{
				col[front] = 1;
				col[back] = 3;
			}
			else if (ptEnd1.X > ptEnd2.X)
			{
				col[front] = 3;
				col[back] = 1;
			}
			else
			{
				col[front] = col[front] + (1 * frontHeadingVector.Width);
				col[back] = col[back] + (1 * backHeadingVector.Width);
			}

			if (ptEnd1.Y < ptEnd2.Y)
			{
				row[front] = 1;
				row[back] = 3;
			}
			else if (ptEnd1.Y > ptEnd2.Y)
			{
				row[front] = 3;
				row[back] = 1;
			}
			else
			{
				row[front] = row[front] + (1 * frontHeadingVector.Height);
				row[back] = row[back] + (1 * backHeadingVector.Height);
			}

			row[front+1] = row[front] + (1 * frontHeadingVector.Height);
			col[front+1] = col[front] + (1 * frontHeadingVector.Width);
			front++;

			row[back-1] = row[back] + (1 * backHeadingVector.Height);
			col[back-1] = col[back] + (1 * backHeadingVector.Width);
			back--;

			bool hasMoved = true;
			bool isOrthogonal = (row[front] == row[back]) || (col[front] == col[back]);

			while (!isOrthogonal && front <= back)
			{
				// Determine if vectors will intersect
				PointF ptFront0 = new PointF(col[front], row[front]);
				PointF ptFront1 = new PointF(col[front] + (10 * frontHeadingVector.Width), row[front] + (10 * frontHeadingVector.Height));
				PointF ptBack0 = new PointF(col[back], row[back]);
				PointF ptBack1 = new PointF(col[back] + (10 * backHeadingVector.Width), row[back] + (10 * backHeadingVector.Height));
				bool vectorsIntersect = Geometry.LinesIntersect(ptFront0, ptFront1, ptBack0, ptBack1);

				// Determine if vectors are pointed towards each other
				SizeF szShift = frontHeadingVector + backHeadingVector;
				bool vectorsOpposite = (szShift.Width == 0 && szShift.Height == 0);
				bool vectorsSame = (frontHeadingVector == backHeadingVector);

				if (!hasMoved || (!vectorsIntersect && !vectorsSame))
				{
					if (vectorsOpposite || vectorsSame)
					{
						int turnDirection = 1;  // 1 == counterclockwise, -1 = clockwise

						// Change direction so that vectors are at right angles

						if (frontHeadingVector.Width > 0)
						{
							// Pointing right
							if (row[front] < row[back])
							{
								turnDirection = 1;
							}
							else
							{
								turnDirection = -1;
							}
						}
						else if (frontHeadingVector.Width < 0)
						{
							// Pointing left
							if (row[front] < row[back])
							{
								turnDirection = -1;
							}
							else
							{
								turnDirection = 1;
							}
						}
						else if (frontHeadingVector.Height > 0)
						{
							// Pointing down
							if (col[front] < col[back])
							{
								turnDirection = 1;
							}
							else
							{
								turnDirection = -1;
							}
						}
						else if (frontHeadingVector.Height < 0)
						{
							// Pointing up
							if (col[front] < col[back])
							{
								turnDirection = -1;
							}
							else
							{
								turnDirection = 1;
							}
						}

						System.Drawing.Size szTmp = frontHeadingVector;
						frontHeadingVector.Width = (szTmp.Height * turnDirection);
						frontHeadingVector.Height = (szTmp.Width * turnDirection);
					}
					else
					{
						frontHeadingVector = backHeadingVector;
					}

					// Go to next front point
					row[front+1] = row[front];
					col[front+1] = col[front];
					front++;
				}

				hasMoved = false;

				// Calculate next front grid point and test to see if
				// the line segments are orthogonal yet.
				int rowFront = row[front] + (1 * frontHeadingVector.Height);
				rowFront = (rowFront < 4) ? (rowFront) : (4);
				rowFront = (rowFront > 0) ? (rowFront) : (0);
				if (rowFront != row[front])
				{
					hasMoved = true;
					row[front] = rowFront;
				}

				int colFront = col[front] + (1 * frontHeadingVector.Width);
				colFront = (colFront < 4) ? (colFront) : (4);
				colFront = (colFront > 0) ? (colFront) : (0);
				if (colFront != col[front])
				{
					hasMoved = true;
					col[front] = colFront;
				}

				isOrthogonal = (row[front] == row[back]) || (col[front] == col[back]);
			}

			int numEmptySlots = back - front - 1;
			numEmptySlots = (numEmptySlots > 0) ? (numEmptySlots) : (0);
			int numPoints = 6 - numEmptySlots;

			// First point is always endpoint 1
			ptsOut.Add(ptEnd1);

			// Compress array and elimate unused points

			while ((front < back) && (back < 6))
			{
				if (row[front] == -1 || col[front] == -1)
				{
					row[front] = row[back];
					col[front] = col[back];
					row[back] = -1;
					col[back] = -1;
					back++;
				}
				front++;
			}

			// Look at the other four points and see if they should
			// be added.
			for (int addIdx = 1; addIdx < numPoints - 1; addIdx++)
			{
				if ((row[addIdx-1] != row[addIdx+1]) && (col[addIdx-1] != col[addIdx+1]))
				{
					PointF ptAdd = Geometry.GetOrthogonalPoint(ptEnd1, ptEnd2, row[addIdx], col[addIdx], padLeft, padRight, padTop, padBottom);

					bool found = false;
					for (int ptIdx = 0; !found && ptIdx < ptsOut.Count; ptIdx++)
					{
						found = (ptAdd == (PointF) ptsOut[ptIdx]);
					}

					if (!found)
					{
						ptsOut.Add(ptAdd);
					}
				}
			}

			// Last point is always endpoint 2
			ptsOut.Add(ptEnd2);

			return (PointF[]) ptsOut.ToArray(typeof(PointF));
		}

		/// <summary>
		/// Calculate the optimal directions for the two specified points.
		/// </summary>
		/// <param name="pt1">First point in the line</param>
		/// <param name="pt2">Second point in the line</param>
		/// <param name="pt1Heading">Heading calculated for the first point</param>
		/// <param name="pt2Heading">Heading calculated for the second point</param>
		/// <remarks>
		/// This method is used for orthogonal lines. Given two points, this method
		/// determines the compass headings for the two points. The compass heading
		/// for each point determines which direction the line will attach to the
		/// point.
		/// </remarks>
		public static void CalcEndpointDirections(PointF pt1, PointF pt2, out CompassHeading pt1Heading, out CompassHeading pt2Heading)
		{
			float horzDiff = pt1.X - pt2.Y;
			float vertDiff = pt1.X - pt2.Y;

			if (Math.Abs(horzDiff) > Math.Abs(vertDiff))
			{
				// Endpoints are further apart horizontally than vertically so make
				// the line segments attached to the endpoints horizontal.

				if (horzDiff > 0)
				{
					pt1Heading = CompassHeading.East;
					pt2Heading = CompassHeading.West;
				}
				else
				{
					pt1Heading = CompassHeading.West;
					pt2Heading = CompassHeading.East;
				}
			}
			else
			{
				// Endpoints are further apart vertically than horizontally so make
				// the line segments attached to the endpoints vertical.

				if (vertDiff > 0)
				{
					pt1Heading = CompassHeading.South;
					pt2Heading = CompassHeading.North;
				}
				else
				{
					pt1Heading = CompassHeading.North;
					pt2Heading = CompassHeading.South;
				}
			}
		}
	}
}
