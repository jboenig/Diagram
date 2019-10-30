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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Moves a vertex on a node by a specified X and Y offset.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
	/// </remarks>
	public class MoveVertexCmd : SingleNodeCmd
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public MoveVertexCmd()
		{
		}

		/// <summary>
		/// Constructs a MoveVertexCmd object given a node.
		/// </summary>
		/// <param name="node">Node affected by command</param>
		public MoveVertexCmd(INode node) : base(node)
		{
		}

		/// <summary>
		/// Constructs a MoveVertexCmd object given a node, a vertex, and
		/// offset values.
		/// </summary>
		/// <param name="node">Node affected by command</param>
		/// <param name="vertexIdx">Zero-based index of vertex to move</param>
		/// <param name="dx">Distance to move vertex in X direction</param>
		/// <param name="dy">Distance to move vertex in Y direction</param>
		public MoveVertexCmd(INode node, int vertexIdx, float dx, float dy) : base(node)
		{
			this.vertexIdx = vertexIdx;
			this.dx = dx;
			this.dy = dy;
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("MoveVertexCmd");
			}
		}

		/// <summary>
		/// Indicates whether or not the command supports undo.
		/// </summary>
		public override bool CanUndo
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Moves the vertex of the node by the specified amount.
		/// </summary>
		/// <param name="cmdTarget">Unused</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// The node must support the IPoints interface. The IPoints.SetPoint
		/// method is used to move the point by the specified offset.
		/// </remarks>
		public override bool Do(object cmdTarget)
		{
			bool success = false;

			if (this.Node != null)
			{
				IPoints objPoints = this.Node as IPoints;
				if (objPoints != null)
				{
					ITransform objTransform = this.Node as ITransform;
					Matrix worldXform = null;
					Matrix invWorldXform = null;

					if (objTransform != null)
					{
						worldXform = objTransform.WorldTransform;
						invWorldXform = worldXform.Clone();
						invWorldXform.Invert();
					}

					if (this.vertexIdx >= 0 && this.vertexIdx < objPoints.PointCount)
					{
						PointF[] pt = new PointF[1]
						{
							objPoints.GetPoint(this.vertexIdx)
						};

						if (worldXform != null)
						{
							worldXform.TransformPoints(pt);
						}

						pt[0].X += this.dx;
						pt[0].Y += this.dy;

						if (invWorldXform != null)
						{
							invWorldXform.TransformPoints(pt);
						}

						objPoints.SetPoint(this.vertexIdx, pt[0]);

						success = true;
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Moves the vertex back to the position it was in before the
		/// command was executed.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public override bool Undo()
		{
			bool success = false;

			if (this.Node != null)
			{
				IPoints objPoints = this.Node as IPoints;
				if (objPoints != null)
				{
					if (this.vertexIdx >= 0 && this.vertexIdx < objPoints.PointCount)
					{
						PointF pt = objPoints.GetPoint(this.vertexIdx);
						pt.X -= this.dx;
						pt.Y -= this.dy;
						objPoints.SetPoint(this.vertexIdx, pt);
						success = true;
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Zero-based index of vertex to move.
		/// </summary>
		public int Vertex
		{
			get
			{
				return this.vertexIdx;
			}
			set
			{
				this.vertexIdx = value;
			}
		}

		/// <summary>
		/// Distance to move vertex in the X direction.
		/// </summary>
		public float X
		{
			get
			{
				return this.dx;
			}
			set
			{
				this.dx = value;
			}
		}

		/// <summary>
		/// Distance to move vertex in the Y direction.
		/// </summary>
		public float Y
		{
			get
			{
				return this.dy;
			}
			set
			{
				this.dy = value;
			}
		}

		private int vertexIdx = -1;
		private float dx = 0;
		private float dy = 0;
	}
}
