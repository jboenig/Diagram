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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Inserts a vertex into a node.
	/// </summary>
	/// <remarks>
	/// The <see cref="Syncfusion.Windows.Forms.Diagram.SingleNodeCmd.Node"/>
	/// property contains the node that the vertex will be added to. The
	/// node must support the IPoints interface.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
	/// </remarks>
	public class InsertVertexCmd : SingleNodeCmd
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public InsertVertexCmd()
		{
			this.point = new PointF(0,0);
		}

		/// <summary>
		/// Constructs an InsertVertexCmd object given a node.
		/// </summary>
		/// <param name="node">Node receiving the new vertex</param>
		public InsertVertexCmd(INode node) : base(node)
		{
			this.point = new PointF(0,0);
		}

		/// <summary>
		/// Constructs an InsertVertexCmd object given a node and a vertex.
		/// </summary>
		/// <param name="node">Node receiving the new vertex</param>
		/// <param name="vertexIdx">Index position of vertex to insert</param>
		/// <param name="point">Value of vertex to insert</param>
		public InsertVertexCmd(INode node, int vertexIdx, PointF point) : base(node)
		{
			this.vertexIdx = vertexIdx;
			this.point = point;
		}

		/// <summary>
		/// Constructs an InsertVertexCmd object given a node and a vertex.
		/// </summary>
		/// <param name="node">Node receiving the new vertex</param>
		/// <param name="point">Value of vertex to insert</param>
		public InsertVertexCmd(INode node, PointF point) : base(node)
		{
			this.vertexIdx = -1;
			this.point = point;
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("InsertVertexCmd");
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
		/// Inserts the specified vertex into the node.
		/// </summary>
		/// <param name="cmdTarget">Unused</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
		/// </remarks>
		public override bool Do(object cmdTarget)
		{
			bool success = false;

			if (this.Node != null)
			{
				IPoints objPoints = this.Node as IPoints;
				if (objPoints != null)
				{
					if (this.vertexIdx >= 0 && this.vertexIdx <= objPoints.PointCount)
					{
						try
						{
							objPoints.InsertPoint(this.vertexIdx, this.point);
							success = true;
						}
						catch
						{
							success = false;
						}
					}
					else if (this.vertexIdx == -1)
					{
						try
						{
							objPoints.AddPoint(this.point);
							this.vertexIdx = objPoints.PointCount - 1;
							success = true;
						}
						catch
						{
							success = false;
						}
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Removes the vertex that was added by the Execute method.
		/// </summary>
		/// <returns></returns>
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
						objPoints.RemovePoint(this.vertexIdx);
						success = true;
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Zero-based index position of the vertex inserted.
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
		/// Value of the vertex to insert.
		/// </summary>
		public PointF Point
		{
			get
			{
				return this.point;
			}
			set
			{
				this.point = value;
			}
		}

		/// <summary>
		/// Index of the vertex inserted
		/// </summary>
		private int vertexIdx = -1;

		/// <summary>
		/// Value of the vertex
		/// </summary>
		private PointF point;
	}
}
