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
	/// Deletes a vertex from a node.
	/// </summary>
	/// <remarks>
	/// This command deletes a vertex from the given node. The node must
	/// support the IPoints interface.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
	/// </remarks>
	public class DeleteVertexCmd : SingleNodeCmd
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DeleteVertexCmd()
		{
		}

		/// <summary>
		/// Constructs a DeleteVertexCmd given a node.
		/// </summary>
		/// <param name="node">Node to apply command to</param>
		public DeleteVertexCmd(INode node) : base(node)
		{
		}

		/// <summary>
		/// Constructs a DeleteVertexCmd given a node and a vertex index.
		/// </summary>
		/// <param name="node">Node to apply command to</param>
		/// <param name="vertexIdx">Zero-based index of vertex to delete</param>
		public DeleteVertexCmd(INode node, int vertexIdx) : base(node)
		{
			this.vertexIdx = vertexIdx;
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("DeleteVertexCmd");
			}
		}

		/// <summary>
		/// Indicates whether or not the command supports undo.
		/// </summary>
		public override bool CanUndo
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Deletes the specified vertex from the node.
		/// </summary>
		/// <param name="cmdTarget">Unused</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// This method gets the IPoints interface from the node and
		/// calls <see cref="Syncfusion.Windows.Forms.Diagram.IPoints.RemovePoint"/>
		/// to remove the vertex.
		/// </remarks>
		public override bool Do(object cmdTarget)
		{
			bool success = false;

			if (this.Node != null)
			{
				IPoints objPoints = this.Node as IPoints;
				if (objPoints != null)
				{
					if (this.vertexIdx >= 0 && this.vertexIdx < objPoints.PointCount)
					{
						this.ptValue = objPoints.GetPoint(this.vertexIdx);
						objPoints.RemovePoint(this.vertexIdx);
						success = true;
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Reverses a delete vertex command.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// This method inserts the vertex back into the object.
		/// </remarks>
		public override bool Undo()
		{
			bool success = false;

			if (this.Node != null)
			{
				IPoints objPoints = this.Node as IPoints;
				if (objPoints != null)
				{
					if (this.vertexIdx >= 0 && this.vertexIdx <= objPoints.PointCount)
					{
						objPoints.InsertPoint(this.vertexIdx, this.ptValue);
						success = true;
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Zero-based index of vertex to delete.
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
		/// Zero-based index of vertex to delete.
		/// </summary>
		private int vertexIdx = -1;

		/// <summary>
		/// Value of vertex deleted - saved for undo
		/// </summary>
		private PointF ptValue;
	}
}
