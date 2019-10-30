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
	/// Base class for commands that operate on a single node.
	/// </summary>
	/// <remarks>
	/// The <see cref="Syncfusion.Windows.Forms.Diagram.SingleNodeCmd.Node"/>
	/// property contains the node that is affected by the command.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICommand"/>
	/// </remarks>
	public abstract class SingleNodeCmd : ICommand
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public SingleNodeCmd()
		{
			this.node = null;
		}

		/// <summary>
		/// Construct a SingleNodeCmd given a node.
		/// </summary>
		/// <param name="node"></param>
		public SingleNodeCmd(INode node)
		{
			this.node = node;
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public abstract string Description
		{
			get;
		}

		/// <summary>
		/// Indicates whether or not the command supports undo.
		/// </summary>
		public abstract bool CanUndo
		{
			get;
		}

		/// <summary>
		/// Execute the command.
		/// </summary>
		/// <param name="cmdTarget">Target of the command</param>
		/// <returns>true if successful, otherwise false</returns>
		public abstract bool Do(object cmdTarget);

		/// <summary>
		/// Reverse the affects of the command.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public abstract bool Undo();

		/// <summary>
		/// Node that the command operates on.
		/// </summary>
		public INode Node
		{
			get
			{
				return this.node;
			}
		}

		/// <summary>
		/// Zero-based index of the node within its parent container.
		/// </summary>
		/// <remarks>
		/// This property can be used by derived class to keep track of the
		/// index position of the node for the purpose of implementing Undo.
		/// </remarks>
		protected int NodeIndex
		{
			get
			{
				return this.nodeIdx;
			}
			set
			{
				this.nodeIdx = value;
			}
		}


		/// <summary>
		/// Node that the command operates on
		/// </summary>
		private INode node = null;

		/// <summary>
		/// index of the node within its parent container
		/// </summary>
		private int nodeIdx = -1;
	}

	/// <summary>
	/// Base class for commands that operate on multiple nodes.
	/// </summary>
	/// <remarks>
	/// The <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>
	/// property contains the collection of nodes that are affected by the
	/// command.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICommand"/>
	/// </remarks>
	public abstract class MultipleNodeCmd : ICommand
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public MultipleNodeCmd()
		{
			this.nodes = new NodeCollection();
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public abstract string Description
		{
			get;
		}

		/// <summary>
		/// Indicates whether or not the command supports undo.
		/// </summary>
		public abstract bool CanUndo
		{
			get;
		}

		/// <summary>
		/// Execute the command.
		/// </summary>
		/// <param name="cmdTarget">Target of the command</param>
		/// <returns>true if successful, otherwise false</returns>
		public abstract bool Do(object cmdTarget);

		/// <summary>
		/// Reverse the affects of the command.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public abstract bool Undo();

		/// <summary>
		/// Collection of nodes affected by the command.
		/// </summary>
		/// <remarks>
		/// The caller must populate this collection before executing the
		/// command.
		/// </remarks>
		public NodeCollection Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		/// <summary>
		/// Creates a hashtable for mapping nodes to their index position
		/// in a collection.
		/// </summary>
		/// <remarks>
		/// This method is called by derived classes in order to create a
		/// hashtable containing index positions of nodes in a collection.
		/// It is used to implement Undo for commands that must remember
		/// the previous position of each node before the command was
		/// executed.
		/// </remarks>
		protected virtual void CreateNodeIndexTable()
		{
			this.nodeIndexTable = new Hashtable();
		}

		/// <summary>
		/// Returns the saved index position of the given node.
		/// </summary>
		/// <param name="node">Node to get index position of</param>
		/// <returns>Index position of node or -1 if not found</returns>
		/// <remarks>
		/// Derived classes that change the position of nodes in a collection
		/// use this method to implement Undo. The position of a node can
		/// be saved prior to executing the command using the SetNodeIndex
		/// method and retrieved using this method during Undo.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.SetNodeIndex"/>
		/// </remarks>
		protected int GetNodeIndex(INode node)
		{
			if (this.nodeIndexTable == null)
			{
				throw new EInvalidOperation();
			}

			int nodeIdx = -1;

			if (this.nodeIndexTable.Contains(node))
			{
				nodeIdx = (int) this.nodeIndexTable[node];
			}

			return nodeIdx;
		}

		/// <summary>
		/// Saves the index position of the given node.
		/// </summary>
		/// <param name="node">Node to save index of</param>
		/// <param name="nodeIdx">Index position of node</param>
		/// <remarks>
		/// Derived classes that change the position of nodes in a collection
		/// use this method to implement Undo. The position of a node can
		/// be saved prior to executing the command using this method and then
		/// retrieved using the SetNodeIndex method.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.GetNodeIndex"/>
		/// </remarks>
		protected void SetNodeIndex(INode node, int nodeIdx)
		{
			if (this.nodeIndexTable == null)
			{
				throw new EInvalidOperation();
			}

			if (this.nodeIndexTable.Contains(node))
			{
				this.nodeIndexTable.Remove(node);
			}

			this.nodeIndexTable.Add(node, nodeIdx);
		}

		/// <summary>
		/// Collection of nodes affected by the command
		/// </summary>
		private NodeCollection nodes;

		/// <summary>
		/// Map of nodes to their corresponding index position
		/// within a collection
		/// </summary>
		private Hashtable nodeIndexTable = null;
	}
}
