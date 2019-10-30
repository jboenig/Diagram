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
	/// Resizes one or more nodes by a specified X and Y amount.
	/// </summary>
	/// <remarks>
	/// The nodes to be resized are specified in the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>
	/// property.
	/// </remarks>
	public class ResizeCmd : MultipleNodeCmd
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ResizeCmd()
		{
		}

		/// <summary>
		/// Construct a ResizeCmd object given the offset width and height and
		/// the handle position.
		/// </summary>
		/// <param name="offsetWidth">Amount to resize along the X axis</param>
		/// <param name="offsetHeight">Amount to resize along the Y axis</param>
		/// <param name="posHandle">
		/// Position of handle on bounding box that serves as the anchor point
		/// </param>
		public ResizeCmd(float offsetWidth, float offsetHeight, BoxPosition posHandle)
		{
			this.offset.Width = offsetWidth;
			this.offset.Height = offsetHeight;
			this.posHandle = posHandle;
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("ResizeCmd");
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
			INode curNode;
			IBounds2DF curNodeBounds;

			try
			{
				parentNode = (INode) cmdTarget;

				IEnumerator nodeEnum = this.Nodes.GetEnumerator();

				while (nodeEnum.MoveNext())
				{
					curNode = nodeEnum.Current as INode;
					if (curNode != null)
					{
						curNodeBounds = curNode as IBounds2DF;

						if (curNodeBounds != null)
						{
							curNodeBounds.Bounds = CalcNewBounds(curNodeBounds);
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
		public BoxPosition Anchor
		{
			get
			{
				return this.posHandle;
			}
			set
			{
				this.posHandle = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float OffsetWidth
		{
			get
			{
				return this.offset.Width;
			}
			set
			{
				this.offset.Width = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float OffsetHeight
		{
			get
			{
				return this.offset.Height;
			}
			set
			{
				this.offset.Height = value;
			}
		}

		private RectangleF CalcNewBounds(IBounds2DF nodeBounds)
		{
			if (nodeBounds == null)
			{
				throw new EInvalidParameter();
			}

			RectangleF rcCur = nodeBounds.Bounds;
			float left;
			float top;
			float width;
			float height;
			float dx = this.offset.Width;
			float dy = this.offset.Height;

			switch (this.posHandle)
			{
				case BoxPosition.TopLeft:
					left = rcCur.Left - dx;
					top = rcCur.Top - dy;
					width = rcCur.Right - left;
					height = rcCur.Bottom - top;
					break;

				case BoxPosition.TopCenter:
					left = rcCur.Left;
					top = rcCur.Top - dy;
					width = rcCur.Width;
					height = rcCur.Bottom - top;
					break;

				case BoxPosition.TopRight:
					left = rcCur.Left;
					top = rcCur.Top - dy;
					width = rcCur.Width + dx;
					height = rcCur.Bottom - top;
					break;

				case BoxPosition.MiddleLeft:
					left = rcCur.Left - dx;
					top = rcCur.Top;
					width = rcCur.Right - left;
					height = rcCur.Height;
					break;

				case BoxPosition.MiddleRight:
					left = rcCur.Left;
					top = rcCur.Top;
					width = rcCur.Width + dx;
					height = rcCur.Height;
					break;

				case BoxPosition.BottomLeft:
					left = rcCur.Left - dx;
					top = rcCur.Top;
					width = rcCur.Right - left;
					height = rcCur.Height + dy;
					break;

				case BoxPosition.BottomCenter:
					left = rcCur.Left;
					top = rcCur.Top;
					width = rcCur.Width;
					height = rcCur.Height + dy;
					break;

				case BoxPosition.BottomRight:
					left = rcCur.Left;
					top = rcCur.Top;
					width = rcCur.Width + dx;
					height = rcCur.Height + dy;
					break;

				default:
					left = rcCur.Left;
					top = rcCur.Top;
					width = rcCur.Width;
					height = rcCur.Height;
					break;
			}

			return new RectangleF(left, top, width, height);
		}

		private BoxPosition posHandle = BoxPosition.Center;
		private SizeF offset = new SizeF(0,0);
	}
}
