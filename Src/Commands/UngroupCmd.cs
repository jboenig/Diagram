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
	/// Remove nodes from a group and add them to the diagram.
	/// </summary>
	/// <remarks>
	/// <para>The nodes to be ungrouped are specified in the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>
	/// property.</para>
	/// <para>This command removes the group from the diagram and adds
	/// all nodes in the group to the group's parent node.</para>
	/// </remarks>
	public class UngroupCmd : MultipleNodeCmd
	{
		/// <summary>
		/// 
		/// </summary>
		public UngroupCmd()
		{
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("UngroupCmd");
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
		/// 
		/// </summary>
		/// <param name="cmdTarget"></param>
		/// <returns></returns>
		public override bool Do(object cmdTarget)
		{
			bool success = false;
			ICompositeNode parentNode = null;
			Group curGroup;
			ICompositeNode curParent;
			int childIdx;
			int newIdx;
			INode curChild;

			parentNode = cmdTarget as ICompositeNode;

			if (parentNode != null)
			{
				IEnumerator nodeEnum = this.Nodes.GetEnumerator();

				while (nodeEnum.MoveNext())
				{
					curGroup = nodeEnum.Current as Group;
					if (curGroup != null)
					{
						curParent = curGroup.Parent;
						if (curParent != null)
						{
							childIdx = curParent.GetChildIndex(curGroup);
							if (childIdx >= 0)
							{
								curParent.RemoveChild(childIdx);
							}
						}

						for (childIdx = 0; childIdx < curGroup.ChildCount; childIdx++)
						{
							curChild = curGroup.GetChild(childIdx);
							newIdx = parentNode.AppendChild(curChild);
						}
					}
				}

				success = true;
			}

			return success;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override bool Undo()
		{
			return false;
		}
	}
}
