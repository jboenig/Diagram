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
	/// Used to indicate the direction for a spacing command.
	/// </summary>
	public enum SpacingDirection
	{
		/// <summary>
		/// Position nodes across the page.
		/// </summary>
		Across,

		/// <summary>
		/// Position nodes down the page.
		/// </summary>
		Down
	}

	/// <summary>
	/// Evenly space two or more nodes either down or across the page.
	/// </summary>
	/// <remarks>
	/// The nodes to be spaced are specified in the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.MultipleNodeCmd.Nodes"/>
	/// property.
	/// </remarks>
	public class SpacingCmd : MultipleNodeCmd
	{
		/// <summary>
		/// 
		/// </summary>
		public SpacingCmd(SpacingDirection dir)
		{
			this.dir = dir;
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("SpacingCmd");
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
		/// 
		/// </summary>
		/// <param name="cmdTarget"></param>
		/// <returns></returns>
		public override bool Do(object cmdTarget)
		{
			bool success = false;

			IBounds2DF curBounds = null;
			RectangleF rcCur;
			PointF ptCurCenter;
			float left = float.MaxValue;
			float right = float.MinValue;
			float top = float.MaxValue;
			float bottom = float.MinValue;
			float spacing = 0.0f;
			float curPos = 0.0f;
			float offset;
			ITransform curTransform = null;

			this.undoVerbs.Clear();

			int numNodes = this.Nodes.Count;

			if (numNodes >= 3)
			{
				foreach (INode curNode in this.Nodes)
				{
					curBounds = curNode as IBounds2DF;

					if (curBounds != null)
					{
						rcCur = curBounds.Bounds;
						ptCurCenter = Geometry.CenterPoint(rcCur);

						if (ptCurCenter.X < left)
						{
							left = ptCurCenter.X;
						}

						if (ptCurCenter.X > right)
						{
							right = ptCurCenter.X;
						}

						if (ptCurCenter.Y < top)
						{
							top = ptCurCenter.Y;
						}

						if (ptCurCenter.Y > bottom)
						{
							bottom = ptCurCenter.Y;
						}
					}
				}

				if (this.dir == SpacingDirection.Down)
				{
					spacing = (bottom - top) / (float) numNodes;
					curPos = top;
					foreach (INode curNode in this.Nodes)
					{
						curBounds = curNode as IBounds2DF;
						curTransform = curNode as ITransform;

						if (curBounds != null && curTransform != null)
						{
							rcCur = curBounds.Bounds;
							ptCurCenter = Geometry.CenterPoint(rcCur);
							offset = curPos - ptCurCenter.Y;
							curTransform.Translate(0.0f, offset);
							this.undoVerbs.Add(new TranslateNode(curTransform, 0.0f, -offset));
						}
						curPos += spacing;
					}
				}
				else
				{
					spacing = (right - left) / (float) numNodes;
					curPos = left;
					foreach (INode curNode in this.Nodes)
					{
						curBounds = curNode as IBounds2DF;
						curTransform = curNode as ITransform;

						if (curBounds != null && curTransform != null)
						{
							rcCur = curBounds.Bounds;
							ptCurCenter = Geometry.CenterPoint(rcCur);
							offset = curPos - ptCurCenter.X;
							curTransform.Translate(offset, 0.0f);
							this.undoVerbs.Add(new TranslateNode(curTransform, -offset, 0.0f));
						}
						curPos += spacing;
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
			foreach (IVerb undoVerb in this.undoVerbs)
			{
				undoVerb.Do(null);
			}

			this.undoVerbs.Clear();

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		private SpacingDirection dir;

		/// <summary>
		/// 
		/// </summary>
		private VerbCollection undoVerbs = new VerbCollection();
	}
}
