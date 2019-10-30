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
using System.Collections;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// This command adds one or more nodes into a target node.
	/// </summary>
	/// <remarks>
	/// The nodes to be added are specified by the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>
	/// property. The parent node to which the nodes are added is specified by
	/// the cmdTarget parameter of the Execute method. If the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.InsertNodesCmd.Location"/>
	/// property is set, the nodes will be moved to the specified location.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd"/>
	/// </remarks>
	public class InsertNodesCmd : MultipleNodeCmd
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public InsertNodesCmd()
		{
			this.CreateNodeIndexTable();
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("InsertNodesCmd");
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
		/// Inserts the nodes into the given parent node.
		/// </summary>
		/// <param name="cmdTarget">Parent node to add the nodes to</param>
		/// <returns>true if successful, otherwise false</returns>
		public override bool Do(object cmdTarget)
		{
			bool success = false;
			IBounds2DF nodeBounds;
			int childIdx;

			this.parentNode = cmdTarget as ICompositeNode;

			if (this.parentNode != null)
			{
				foreach (INode curNode in this.Nodes)
				{
					if (locationSet)
					{
						nodeBounds = curNode as IBounds2DF;
						RectangleF rcBounds;

						if (nodeBounds != null)
						{
							rcBounds = nodeBounds.Bounds;
							rcBounds.Location = this.location;
							nodeBounds.Bounds = rcBounds;
						}
					}

					childIdx = this.parentNode.AppendChild(curNode);

					if (childIdx >= 0)
					{
						this.SetNodeIndex(curNode, childIdx);
					}
				}

				success = true;
			}

			return success;
		}

		/// <summary>
		/// Reverses the affects of executing an insert nodes command.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// Removes the nodes from the parent they were added to by the
		/// Execute method.
		/// </remarks>
		public override bool Undo()
		{
			bool success = false;

			if (this.parentNode != null)
			{
				foreach (INode curNode in this.Nodes)
				{
					int childIdx = this.GetNodeIndex(curNode);
					if (childIdx >= 0)
					{
						this.parentNode.RemoveChild(childIdx);
					}
				}

				success = true;
			}

			return success;
		}

		/// <summary>
		/// The location at which to add the nodes.
		/// </summary>
		/// <remarks>
		/// If this property is set, the nodes are moved to this location
		/// after they are added to the parent. If this property is not set,
		/// then the location of the nodes is unaffected.
		/// </remarks>
		public PointF Location
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
				locationSet = true;
			}
		}

		private ICompositeNode parentNode = null;
		private PointF location = new PointF(0,0);
		private bool locationSet = false;
	}
}
