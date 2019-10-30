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
	/// Align two or more nodes along a specified vertical or horizontal edge.
	/// </summary>
	/// <remarks>
	/// The first node in the command's node collection is always the anchor
	/// node. The anchor node is never moved by this command. There must be
	/// at least two nodes in the node collection for this command to work.
	/// A <see cref="Syncfusion.Windows.Forms.Diagram.BoxOrientation"/> value
	/// specifies which vertical or horizontal edge of the bounding box the
	/// nodes will be aligned with. For example, if BoxOrientation.Middle is
	/// specified then the nodes will be aligned along the horizontal line
	/// passing through the center of the anchor node.
	/// </remarks>
	public class AlignCmd : MultipleNodeCmd
	{
		/// <summary>
		/// Construct an AlignCmd object given a box orientation value.
		/// </summary>
		/// <param name="orientation">
		/// Specifies which edge of the bounding boxes will be aligned
		/// </param>
		public AlignCmd(BoxOrientation orientation)
		{
			this.orientation = orientation;
		}

		/// <summary>
		/// Specifies the vertical or horizontal edge along which the nodes
		/// will be aligned.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoxOrientation"/>
		/// </remarks>
		public BoxOrientation Orientation
		{
			get
			{
				return this.orientation;
			}
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("AlignCmd");
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
		/// Executes the alignment command.
		/// </summary>
		/// <param name="cmdTarget">Unused</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// This method iterates through each node and aligns them to the
		/// anchor node, which is always the first node in the list. The nodes
		/// are aligned either vertically or horizontally depending on the value
		/// of the <see cref="Syncfusion.Windows.Forms.Diagram.AlignCmd.Orientation"/> property.
		/// </remarks>
		public override bool Do(object cmdTarget)
		{
			IBounds2DF anchorBounds = null;
			RectangleF rcAnchor = new RectangleF(0,0,0,0);
			PointF ptAnchorCenter = new PointF(0,0);
			IBounds2DF curBounds = null;
			RectangleF rcCur;
			PointF ptCurCenter;

			this.undoVerbs.Clear();

			foreach (INode curNode in this.Nodes)
			{
				if (anchorBounds == null)
				{
					anchorBounds = curNode as IBounds2DF;
					if (anchorBounds != null)
					{
						rcAnchor = anchorBounds.Bounds;
						ptAnchorCenter = Geometry.CenterPoint(rcAnchor);
					}
				}
				else
				{
					curBounds = curNode as IBounds2DF;

					if (curBounds != null)
					{
						rcCur = curBounds.Bounds;

						this.undoVerbs.Add(new SetBounds(curBounds, rcCur));
						
						float offsetX = 0.0f;
						float offsetY = 0.0f;

						switch (this.orientation)
						{
							case BoxOrientation.Left:
								offsetX = rcAnchor.Left - rcCur.Left;
								break;

							case BoxOrientation.Right:
								offsetX = rcAnchor.Right - rcCur.Right;
								break;

							case BoxOrientation.Top:
								offsetY = rcAnchor.Top - rcCur.Top;
								break;

							case BoxOrientation.Bottom:
								offsetY = rcAnchor.Bottom - rcCur.Bottom;
								break;

							case BoxOrientation.Middle:
								ptCurCenter = Geometry.CenterPoint(rcCur);
								offsetY = ptAnchorCenter.Y - ptCurCenter.Y;
								break;

							case BoxOrientation.Center:
								ptCurCenter = Geometry.CenterPoint(rcCur);
								offsetX = ptAnchorCenter.X - ptCurCenter.X;
								break;
						}

						rcCur.Offset(offsetX, offsetY);
						curBounds.Bounds = rcCur;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Reverse the alignment command.
		/// </summary>
		/// <returns>Always returns true</returns>
		/// <remarks>
		/// This method restores the nodes affected by the align command
		/// to their original positions.
		/// </remarks>
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
		/// Specifies the horizontal or vertical edge used for alignment
		/// </summary>
		private BoxOrientation orientation;

		/// <summary>
		/// Collection of verbs used to undo the command
		/// </summary>
		private VerbCollection undoVerbs = new VerbCollection();
	}
}
