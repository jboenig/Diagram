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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Removes one or more nodes from the diagram.
	/// </summary>
	/// <remarks>
	/// The nodes to be removed are specified in the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>
	/// property.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.INode"/>
	/// </remarks>
	public class RemoveNodesCmd : MultipleNodeCmd
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public RemoveNodesCmd()
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
				return Resources.Strings.CommandDescriptions.Get("RemoveNodesCmd");
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
		/// Removes the specified nodes from the diagram.
		/// </summary>
		/// <param name="cmdTarget">Parent node to remove the nodes from</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// The nodes to be removed are specified in the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>.
		/// The cmdTarget parameter must implement the ICompositeNode interface.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICompositeNode"/>
		/// </remarks>
		public override bool Do(object cmdTarget)
		{
			bool success = false;
			int childIndex;

			this.parentNode = cmdTarget as ICompositeNode;

			if (this.parentNode != null)
			{
				foreach (INode curNode in this.Nodes)
				{
					childIndex = this.parentNode.GetChildIndex(curNode);
					if (childIndex >= 0)
					{
						this.SetNodeIndex(curNode, childIndex);
						this.parentNode.RemoveChild(childIndex);
					}
				}

				success = true;
			}

			return success;
		}

		/// <summary>
		/// Adds the nodes back into the diagram that were removed by executing
		/// the command.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
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
						this.parentNode.InsertChild(curNode, childIdx);
					}
				}

				success = true;
			}

			return success;
		}

		private ICompositeNode parentNode = null;
	}
}
