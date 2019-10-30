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
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for node tracker classes.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class defines an abstract interface for tracking one or more
	/// nodes as they are being dragged. Node trackers typically draw the
	/// outline of the nodes using dashed lines.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeBoundsTracker"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodePathTracker"/>
	/// </remarks>
	public abstract class NodeTracker
	{
		/// <summary>
		/// Construct a node tracker and attach it to the given view.
		/// </summary>
		/// <param name="view">View to attach to</param>
		public NodeTracker(View view)
		{
			this.view = view;
		}

		/// <summary>
		/// Moves the nodes by a given X and Y offset.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		public abstract void Move(int dx, int dy);

		/// <summary>
		/// Erases the current tracking outline from the view.
		/// </summary>
		public abstract void Erase();

		/// <summary>
		/// Returns the most recent tracking position of the nodes.
		/// </summary>
		/// <remarks>
		/// This point represents the upper-left hand corner of a
		/// bounding rectangle that is the union of the nodes being
		/// tracked.
		/// </remarks>
		public System.Drawing.Point UpperLeft
		{
			get
			{
				return this.ptUpperLeft;
			}
		}

		/// <summary>
		/// Indicates if the current position is valid within the constraining region.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This flag is true if all nodes are in a valid position. It is false if
		/// any node violates its constraining region.
		/// </para>
		/// </remarks>
		public bool IsValidPosition
		{
			get
			{
				return this.validPosition;
			}
		}

		/// <summary>
		/// View to draw on
		/// </summary>
		protected View view = null;

		/// <summary>
		/// Upper-left hand corner of the tracking position
		/// </summary>
		protected System.Drawing.Point ptUpperLeft;

		/// <summary>
		/// Indicates if the current position is valid
		/// </summary>
		protected bool validPosition = true;
	}

	/// <summary>
	/// Tracks the rectangular bounds of a collection of nodes.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a node tracker the draws the outline of the
	/// rectangular bounds of a collection of nodes.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeTracker"/>
	/// </remarks>
	public class NodeBoundsTracker : NodeTracker
	{
		/// <summary>
		/// Constructs NodeTracker from a given view and node collection.
		/// </summary>
		/// <param name="view">View to render onto</param>
		/// <param name="nodes">Collection of nodes to track</param>
		public NodeBoundsTracker(View view, NodeCollection nodes) : base(view)
		{
			// Load array of bounding rectangles
			this.ptUpperLeft = new Point(int.MaxValue, int.MaxValue);
			System.Drawing.Rectangle curBounds;
			int nodeIdx = 0;
			this.nodeBounds = new System.Drawing.Rectangle[nodes.Count];

			foreach (INode curNode in nodes)
			{
				IBounds2DF nodeBounds = curNode as IBounds2DF;
				ITransform nodeTrans = curNode as ITransform;

				// Push the parent transforms for the current node onto the stack
				if (nodeTrans != null)
				{
					Global.MatrixStack.Push(nodeTrans.ParentTransform);
				}

				// Get the bounds of the current node in world coordinates
				if (nodeBounds != null)
				{
					curBounds = this.view.ViewToDevice(this.view.WorldToView(nodeBounds.Bounds));

					if (curBounds.Left < this.ptUpperLeft.X)
					{
						this.ptUpperLeft.X = curBounds.Left;
					}

					if (curBounds.Top < this.ptUpperLeft.Y)
					{
						this.ptUpperLeft.Y = curBounds.Top;
					}
				}
				else
				{
					curBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
				}

				// Reset matrix stack
				Global.MatrixStack.Clear();

				// Add bounds to array
				this.nodeBounds[nodeIdx++] = curBounds;
			}
		}

		/// <summary>
		/// Constructs NodeTracker from a given view and node collection.
		/// </summary>
		/// <param name="view">View to render onto</param>
		/// <param name="node">Node to track</param>
		public NodeBoundsTracker(View view, INode node) : base(view)
		{
			// Load array of bounding rectangles
			this.ptUpperLeft = new Point(int.MaxValue, int.MaxValue);
			System.Drawing.Rectangle curBounds;
			int nodeIdx = 0;
			this.nodeBounds = new System.Drawing.Rectangle[1];

			IBounds2DF nodeBounds = node as IBounds2DF;
			ITransform nodeTrans = node as ITransform;

			// Push the parent transforms for the current node onto the stack
			if (nodeTrans != null)
			{
				Global.MatrixStack.Push(nodeTrans.ParentTransform);
			}

			// Get the bounds of the current node in world coordinates
			if (nodeBounds != null)
			{
				curBounds = this.view.ViewToDevice(this.view.WorldToView(nodeBounds.Bounds));

				if (curBounds.Left < this.ptUpperLeft.X)
				{
					this.ptUpperLeft.X = curBounds.Left;
				}

				if (curBounds.Top < this.ptUpperLeft.Y)
				{
					this.ptUpperLeft.Y = curBounds.Top;
				}
			}
			else
			{
				curBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
			}

			// Reset matrix stack
			Global.MatrixStack.Clear();

			// Add bounds to array
			this.nodeBounds[nodeIdx++] = curBounds;
		}

		/// <summary>
		/// Moves the nodes by a given X and Y offset.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		public override void Move(int dx, int dy)
		{
			int dxActual = dx;
			int dyActual = dy;

			if (this.view.Grid.SnapToGrid)
			{
				System.Drawing.Point newUpperLeft = new System.Drawing.Point(this.ptUpperLeft.X + dx, this.ptUpperLeft.Y + dy);
				System.Drawing.Point nearestGridPoint = this.view.SnapPointToGrid(newUpperLeft);
				dxActual = nearestGridPoint.X - this.ptUpperLeft.X;
				dyActual = nearestGridPoint.Y - this.ptUpperLeft.Y;
			}

			foreach (System.Drawing.Rectangle rcBounds in this.nodeBounds)
			{
				rcBounds.Offset(dxActual, dyActual);
				this.view.DrawTrackingRect(rcBounds);
				this.trackingRects.Add(rcBounds);
			}
		}

		/// <summary>
		/// Erases the current tracking outline from the view.
		/// </summary>
		public override void Erase()
		{
			IEnumerator enumRects = this.trackingRects.GetEnumerator();
			System.Drawing.Rectangle rect;

			while (enumRects.MoveNext())
			{
				rect = (System.Drawing.Rectangle) enumRects.Current;
				this.view.DrawTrackingRect(rect);
			}

			this.trackingRects.Clear();
		}

		/// <summary>
		/// Bounding rectangles of the nodes being tracked
		/// </summary>
		protected System.Drawing.Rectangle[] nodeBounds;

		/// <summary>
		/// Rectangles that need to be erased
		/// </summary>
		protected ArrayList trackingRects = new ArrayList();
	}

	/// <summary>
	/// Tracks the graphics paths of a collection of nodes.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a node tracker the draws the outline of the
	/// regions of a collection of nodes. In other words, this class tracks
	/// the actual outline of the object - not just the bounding boxes.
	/// A System.Drawing.Drawing2D.GraphicsPath object is created for each
	/// node by calling the node's
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IGraphics.GraphicsPath"/>
	/// method. Nodes that do not support the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IGraphics"/>
	/// interface are tracked using their bounding boxes.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeTracker"/>
	/// </remarks>
	public class NodePathTracker : NodeTracker
	{
		/// <summary>
		/// Construct a NodePathTracker given a view and a node.
		/// </summary>
		/// <param name="view">View to attach to</param>
		/// <param name="node">Node to track</param>
		public NodePathTracker(View view, INode node) : base(view)
		{
			// Load array of bounding rectangles
			this.trackingPath = new GraphicsPath();
			this.ptUpperLeft = new Point(int.MaxValue, int.MaxValue);
			System.Drawing.Rectangle curBounds;

			if (node != null)
			{
				IGraphics graphicsObj = node as IGraphics;
				IBounds2DF nodeBounds = node as IBounds2DF;
				ITransform nodeTrans = node as ITransform;
				ICompositeNode nodeParent = node.Parent;

				// Push the parent transforms for the current node onto the stack
				if (nodeTrans != null)
				{
					Global.MatrixStack.Push(nodeTrans.ParentTransform);
				}

				// Get the bounds of the current node in world coordinates
				if (nodeBounds != null)
				{
					curBounds = this.view.ViewToDevice(this.view.WorldToView(nodeBounds.Bounds));

					if (curBounds.Left < this.ptUpperLeft.X)
					{
						this.ptUpperLeft.X = curBounds.Left;
					}

					if (curBounds.Top < this.ptUpperLeft.Y)
					{
						this.ptUpperLeft.Y = curBounds.Top;
					}
				}
				else
				{
					curBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
				}

				if (graphicsObj != null)
				{
					GraphicsPath grfxPath = graphicsObj.GraphicsPath;
					if (grfxPath != null)
					{
						this.trackingPath.AddPath(grfxPath, false);
					}
				}
				else if (nodeBounds != null)
				{
					this.trackingPath.AddRectangle(curBounds);
				}

				// Get the constraining region for the current node and add it
				// to the constraining region for the tracker
				if (nodeParent != null)
				{
					this.rgnConstraint = nodeParent.GetConstrainingRegion(node);
				}

				// Reset matrix stack
				Global.MatrixStack.Clear();
			}
		}

		/// <summary>
		/// Constructs a NodePathTracker object given a view and a collection
		/// of nodes to track.
		/// </summary>
		/// <param name="view">View to attach to</param>
		/// <param name="nodes">Collection of nodes to track</param>
		public NodePathTracker(View view, NodeCollection nodes) : base(view)
		{
			// Load array of bounding rectangles
			this.trackingPath = new GraphicsPath();
			this.ptUpperLeft = new Point(int.MaxValue, int.MaxValue);
			System.Drawing.Rectangle curBounds;

			foreach (INode curNode in nodes)
			{
				IGraphics graphicsObj = curNode as IGraphics;
				IBounds2DF nodeBounds = curNode as IBounds2DF;
				ITransform nodeTrans = curNode as ITransform;
				ICompositeNode nodeParent = curNode.Parent;

				// Push the parent transforms for the current node onto the stack
				if (nodeTrans != null)
				{
					Global.MatrixStack.Push(nodeTrans.ParentTransform);
				}

				// Get the bounds of the current node in world coordinates
				if (nodeBounds != null)
				{
					curBounds = this.view.ViewToDevice(this.view.WorldToView(nodeBounds.Bounds));

					if (curBounds.Left < this.ptUpperLeft.X)
					{
						this.ptUpperLeft.X = curBounds.Left;
					}

					if (curBounds.Top < this.ptUpperLeft.Y)
					{
						this.ptUpperLeft.Y = curBounds.Top;
					}
				}
				else
				{
					curBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
				}

				if (graphicsObj != null)
				{
					GraphicsPath grfxPath = graphicsObj.GraphicsPath;
					if (grfxPath != null)
					{
						this.trackingPath.AddPath(grfxPath, false);
					}
				}
				else if (nodeBounds != null)
				{
					this.trackingPath.AddRectangle(curBounds);
				}

				// Get the constraining region for the current node and add it
				// to the constraining region for the tracker
				Region curRgnConstraint = null;
				if (nodeParent != null)
				{
					curRgnConstraint = nodeParent.GetConstrainingRegion(curNode);
					if (curRgnConstraint != null)
					{
						if (this.rgnConstraint == null)
						{
							this.rgnConstraint = curRgnConstraint.Clone();
						}
						else
						{
							this.rgnConstraint.Union(curRgnConstraint);
						}
					}
				}

				// Reset matrix stack
				Global.MatrixStack.Clear();
			}
		}

		/// <summary>
		/// Moves the nodes by a given X and Y offset.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		public override void Move(int dx, int dy)
		{
			System.Drawing.Size szDevice = new System.Drawing.Size(dx, dy);
			System.Drawing.SizeF szView;
			System.Drawing.SizeF szWorld;

			if (this.view.Grid.SnapToGrid)
			{
				System.Drawing.Point newUpperLeft = new System.Drawing.Point(this.ptUpperLeft.X + dx, this.ptUpperLeft.Y + dy);
				System.Drawing.Point nearestGridPoint = this.view.SnapPointToGrid(newUpperLeft);
				szDevice.Width = nearestGridPoint.X - this.ptUpperLeft.X;
				szDevice.Height = nearestGridPoint.Y - this.ptUpperLeft.Y;
			}

			szView = this.view.DeviceToView(szDevice);
			szWorld = this.view.ViewToWorld(szView);

			GraphicsPath grfxPath = (GraphicsPath) this.trackingPath.Clone();
			this.trackingXform.Reset();
			this.trackingXform.Translate(szWorld.Width, szWorld.Height, MatrixOrder.Append);
			grfxPath.Transform(this.trackingXform);
			System.Drawing.Rectangle rect = this.view.DrawTrackingPath(grfxPath);
			this.rectErase = Geometry.Union(rect, this.rectErase);

			// If there is a constraining region, test all points in the graphics
			// path to determine if the current position is valid or not

			this.validPosition = true;

			if (this.rgnConstraint != null)
			{
				// Test each point to see if they fall within the constraining region
				foreach (PointF curPt in grfxPath.PathPoints)
				{
					if (!this.rgnConstraint.IsVisible(curPt))
					{
						this.validPosition = false;
						break;
					}
				}

				if (this.validPosition)
				{
					// Position is valid. Restore cursor if necessary.
					if (this.prevCursor != null)
					{
						this.view.Cursor = this.prevCursor;
						this.prevCursor = null;
					}
				}
				else
				{
					// Position is not valid. Set cursor to No icon (circle + slash)
					if (this.prevCursor == null)
					{
						this.prevCursor = this.view.Cursor;
						this.view.Cursor = Cursors.No;
					}
				}
			}
		}

		/// <summary>
		/// Erases the current tracking outline from the view.
		/// </summary>
		public override void Erase()
		{
			if (!this.rectErase.IsEmpty)
			{
				this.view.Refresh(this.rectErase);
				this.rectErase = new System.Drawing.Rectangle(0,0,0,0);
			}
		}

		/// <summary>
		/// GraphicsPath to draw onto the view
		/// </summary>
		protected GraphicsPath trackingPath;

		/// <summary>
		/// Region that the nodes are constrained to
		/// </summary>
		protected Region rgnConstraint = null;

		/// <summary>
		/// Matrix used to transform the the tracking path
		/// </summary>
		protected Matrix trackingXform = new Matrix();

		/// <summary>
		/// Rectangle to erase
		/// </summary>
		protected System.Drawing.Rectangle rectErase = new System.Drawing.Rectangle(0,0,0,0);

		/// <summary>
		/// 
		/// </summary>
		protected Cursor prevCursor = null;
	}
}
