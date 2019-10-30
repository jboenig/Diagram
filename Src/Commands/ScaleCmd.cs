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
	/// Scale one or more nodes by a specified X and Y ratio.
	/// </summary>
	/// <remarks>
	/// The nodes to be scaled are specified in the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>
	/// property.
	/// </remarks>
	public class ScaleCmd : MultipleNodeCmd
	{
		/// <summary>
		/// 
		/// </summary>
		public ScaleCmd()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="sy"></param>
		public ScaleCmd(float sx, float sy)
		{
			this.sx = sx;
			this.sy = sy;
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("ScaleCmd");
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
		public float ScaleX
		{
			get
			{
				return this.sx;
			}
			set
			{
				this.sx = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float ScaleY
		{
			get
			{
				return this.sy;
			}
			set
			{
				this.sy = value;
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
			INode parentNode = null;
			IBounds2DF nodeBounds = null;
			ITransform curNodeXForm;
			PointF ptAnchor;

			parentNode = (INode) cmdTarget;

			foreach (INode curNode in this.Nodes)
			{
				nodeBounds = curNode as IBounds2DF;
				curNodeXForm = curNode as ITransform;

				if (nodeBounds != null && curNodeXForm != null)
				{
					ptAnchor = Geometry.CenterPoint(nodeBounds.Bounds);
					curNodeXForm.Scale(ptAnchor, this.sx, this.sy);
				}
			}

			success = true;

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

		/// <summary>
		/// 
		/// </summary>
		private float sx = 1.0f;

		/// <summary>
		/// 
		/// </summary>
		private float sy = 1.0f;
	}
}
