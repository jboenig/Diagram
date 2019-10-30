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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Specifies relative Z-order movement
	/// </summary>
	public enum ZOrderUpdate
	{
		/// <summary>
		/// 
		/// </summary>
		Front,

		/// <summary>
		/// 
		/// </summary>
		Back,

		/// <summary>
		/// 
		/// </summary>
		Forward,

		/// <summary>
		/// 
		/// </summary>
		Backward
	}

	/// <summary>
	/// Change the Z-order of one or more nodes.
	/// </summary>
	/// <remarks>
	/// <para>Z-order determines the order in which nodes are rendered
	/// and affects how nodes overlap.</para>
	/// <para>The nodes to be ungrouped are specified in the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>
	/// property.</para>
	/// </remarks>
	public class ZOrderCmd : MultipleNodeCmd
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="updType"></param>
		public ZOrderCmd(ZOrderUpdate updType)
		{
			this.updType = updType;
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("ZOrderCmd");
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

			ICompositeNode parentNode = cmdTarget as ICompositeNode;

			if (parentNode != null)
			{
				foreach (INode curNode in this.Nodes)
				{
					int childIdx = parentNode.GetChildIndex(curNode);
					int newIdx;
					int childCount;

					if (childIdx >= 0)
					{
						parentNode.RemoveChild(childIdx);
					}

					switch (this.updType)
					{
						case ZOrderUpdate.Front:
							parentNode.AppendChild(curNode);
							break;

						case ZOrderUpdate.Back:
							parentNode.InsertChild(curNode, 0);
							break;

						case ZOrderUpdate.Forward:
							childCount = parentNode.ChildCount;
							newIdx = (childIdx < childCount) ? (childIdx+1) : (childCount);
							parentNode.InsertChild(curNode, newIdx);
							break;

						case ZOrderUpdate.Backward:
							newIdx = (childIdx > 0) ? (childIdx-1) : (0);
							parentNode.InsertChild(curNode, newIdx);
							break;
					}
				}
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

		private ZOrderUpdate updType;
	}
}
