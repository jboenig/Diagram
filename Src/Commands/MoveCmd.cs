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
	/// Moves one or more nodes by a specified X and Y offset.
	/// </summary>
	public class MoveCmd : MultipleNodeCmd
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public MoveCmd()
		{
		}

		/// <summary>
		/// Constructs a MoveCmd object given an X and Y offset.
		/// </summary>
		/// <param name="dx">Distance to move the nodes in the X direction</param>
		/// <param name="dy">Distance to move the nodes in the Y direction</param>
		public MoveCmd(float dx, float dy)
		{
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
				return Resources.Strings.CommandDescriptions.Get("MoveCmd");
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
		/// Moves the collection of nodes by the offsets specified in the
		/// X and Y properties.
		/// </summary>
		/// <param name="cmdTarget">Unused</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// The node must support the ITransform interface. The
		/// ITransform.Translate method is called on each node.
		/// </remarks>
		public override bool Do(object cmdTarget)
		{
#if true
			ITransform curNodeTransform;
#else
			IBounds2DF curNodeBounds;
#endif

			foreach (INode curNode in this.Nodes)
			{
#if true
				curNodeTransform = curNode as ITransform;
				if (curNodeTransform != null)
				{
					try
					{
						curNodeTransform.Translate(this.dx, this.dy);
					}
					catch (EBoundaryConstraint)
					{
					}
				}
#else
				curNodeBounds = curNode as IBounds2DF;

				if (curNodeBounds != null)
				{
					curNodeBounds.Bounds = CalcNewBounds(curNodeBounds);
				}
#endif
			}

			return true;
		}

		/// <summary>
		/// Moves the nodes back to their original positions.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public override bool Undo()
		{
			ITransform curNodeTransform;

			foreach (INode curNode in this.Nodes)
			{
				curNodeTransform = curNode as ITransform;
				if (curNodeTransform != null)
				{
					curNodeTransform.Translate(-this.dx, -this.dy);
				}
			}
			return true;
		}

		/// <summary>
		/// Distance to move the nodes in the X direction.
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
		/// Distance to move the nodes in the Y direction.
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

		private RectangleF CalcNewBounds(IBounds2DF nodeBounds)
		{
			if (nodeBounds == null)
			{
				throw new EInvalidParameter();
			}

			RectangleF rcCur = nodeBounds.Bounds;
			float left = rcCur.Left + this.dx;
			float top = rcCur.Top + this.dy;
			float width = rcCur.Width;
			float height = rcCur.Height;

			return new RectangleF(left, top, width, height);
		}

		private float dx = 0;
		private float dy = 0;
	}
}
