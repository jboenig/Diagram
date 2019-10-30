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
	/// This command creates a <see cref="Syncfusion.Windows.Forms.Diagram.Group"/>
	/// node and populates it with the list of nodes attached to the command.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Group"/>
	/// </remarks>
	public class GroupCmd : MultipleNodeCmd
	{
		/// <summary>
		/// Group created when the command is executed
		/// </summary>
		protected Group group = null;

		/// <summary>
		/// Composite node the group is added to
		/// </summary>
		protected ICompositeNode groupParent = null;

		/// <summary>
		/// Contains map of children to their previous parents - for
		/// implementing undo
		/// </summary>
		protected Hashtable parentMap = new Hashtable();

		/// <summary>
		/// Default constructor.
		/// </summary>
		public GroupCmd()
		{
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("GroupCmd");
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
		/// Creates a group and populates with the list of nodes attached to
		/// the command.
		/// </summary>
		/// <param name="cmdTarget">Parent node that will contain the new group</param>
		/// <returns>true if successful, otherwise false</returns>
		public override bool Do(object cmdTarget)
		{
			bool success = false;

			ICompositeNode curParent;
			int childIdx;

			this.group = new Group();
			this.parentMap.Clear();

			this.groupParent = cmdTarget as ICompositeNode;

			if (this.groupParent != null)
			{
				foreach (INode curNode in this.Nodes)
				{
					curParent = curNode.Parent;
					if (curParent != null)
					{
						childIdx = curParent.GetChildIndex(curNode);
						if (childIdx >= 0)
						{
							this.parentMap.Add(curNode, curParent);
							curParent.RemoveChild(childIdx);
						}
					}
					childIdx = this.group.AppendChild(curNode);
				}

				childIdx = this.groupParent.AppendChild(this.group);

				success = true;
			}

			return success;
		}

		/// <summary>
		/// Destroys the group created by executing the command and restores
		/// the nodes to their original parent.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public override bool Undo()
		{
			bool success = false;

			if (this.group != null && this.groupParent != null)
			{
				NodeCollection groupMembers = new NodeCollection();

				for (int childIdx = 0; childIdx < this.group.ChildCount; childIdx++)
				{
					INode curChild = this.group.GetChild(childIdx);
					if (curChild != null)
					{
						groupMembers.Add(curChild);
					}
				}

				int groupChildIdx = this.groupParent.GetChildIndex(this.group);
				if (groupChildIdx >= 0)
				{
					this.groupParent.RemoveChild(groupChildIdx);
				}

				this.group.RemoveAllChildren();
				this.group = null;

				foreach (INode curChild in groupMembers)
				{
					ICompositeNode curParent = null;
					if (this.parentMap.Contains(curChild))
					{
						curParent = (ICompositeNode) this.parentMap[curChild];
						curParent.AppendChild(curChild);
					}
				}

				success = true;
			}

			return success;
		}
	}
}
