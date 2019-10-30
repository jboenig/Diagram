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
	/// Rotates one or more nodes by a specified angle.
	/// </summary>
	/// <remarks>
	/// The nodes to be rotated are specified in the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>
	/// property.
	/// </remarks>
	public class RotateCmd : MultipleNodeCmd
	{
		/// <summary>
		/// 
		/// </summary>
		public RotateCmd()
		{
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("RotateCmd");
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
			INode parentNode = null;
			INode curNode = null;
			ITransform objTransform = null;

			try
			{
				parentNode = (INode) cmdTarget;

				IEnumerator nodeEnum = this.Nodes.GetEnumerator();

				while (nodeEnum.MoveNext())
				{
					curNode = nodeEnum.Current as INode;
					if (curNode != null)
					{
						objTransform = curNode as ITransform;
						if (objTransform != null)
						{
							objTransform.Rotate(this.degrees);
						}
					}
				}

				success = true;
			}
			catch (Exception)
			{
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

		/// <summary>
		/// 
		/// </summary>
		public float Radians
		{
			get
			{
				return (float) (((double) this.degrees * Math.PI) / 180.0);
			}
			set
			{
				this.degrees = (float) (((double) value * 180.0) / Math.PI);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float Degrees
		{
			get
			{
				return this.degrees;
			}
			set
			{
				this.degrees = value;
			}
		}

		private float degrees = 0.0f;
	}
}
