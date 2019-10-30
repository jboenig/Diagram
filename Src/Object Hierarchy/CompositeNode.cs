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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A composite node is a node that contains children.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface has methods for adding and removing child
	/// nodes.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.INode"/>
	/// </remarks>
	public interface ICompositeNode : INode
	{
		/// <summary>
		/// The number of child nodes contained by this node.
		/// </summary>
		int ChildCount
		{
			get;
		}

		/// <summary>
		/// Returns the child node at the given index position.
		/// </summary>
		/// <param name="childIndex">Zero-based index into the collection of child nodes</param>
		/// <returns>Child node at the given position or null if the index is out of range</returns>
		INode GetChild(int childIndex);

		/// <summary>
		/// Returns the child node matching the given name.
		/// </summary>
		/// <param name="childName">Name of node to return</param>
		/// <returns>Node matching the given name</returns>
		INode GetChildByName(string childName);

		/// <summary>
		/// Returns the index position of the given child node.
		/// </summary>
		/// <param name="child">Child node to query</param>
		/// <returns>Zero-based index into the collection of child nodes</returns>
		int GetChildIndex(INode child);

		/// <summary>
		/// Appends the given node to the collection of child nodes.
		/// </summary>
		/// <param name="child">Node to append</param>
		/// <returns>
		/// Zero-based index at which the node was added to the collection or -1 for failure.
		/// </returns>
		int AppendChild(INode child);

		/// <summary>
		/// Insert the given node into the collection of child nodes at a
		/// specific position.
		/// </summary>
		/// <param name="child">Node to insert</param>
		/// <param name="childIndex">Zero-based index at which to insert the node</param>
		void InsertChild(INode child, int childIndex);

		/// <summary>
		/// Removes the child node at the given position.
		/// </summary>
		/// <param name="childIndex">Zero-based index into the collection of child nodes</param>
		void RemoveChild(int childIndex);

		/// <summary>
		/// Removes all child nodes from the node.
		/// </summary>
		void RemoveAllChildren();

		/// <summary>
		/// Returns the region that the bounds of the given child node is constrained by.
		/// </summary>
		/// <param name="child">Child to get constraining region for</param>
		/// <returns>Region that constrains the bounds of the given child</returns>
		/// <remarks>
		/// <para>
		/// This method is used to limit the bounds of a child node to a specified area.
		/// The node cannot be moved, resized, or rotated beyond the edges of this region.
		/// </para>
		/// </remarks>
		System.Drawing.Region GetConstrainingRegion(INode child);

		/// <summary>
		/// Returns all children that are intersected by the given point.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children hit by the given point
		/// </param>
		/// <param name="ptWorld">Point to test</param>
		/// <returns>The number of child nodes that intersect the given point</returns>
		int GetChildrenAtPoint(NodeCollection childNodes, PointF ptWorld);

		/// <summary>
		/// Returns all children that intersect the given rectangle.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children hit by the given point
		/// </param>
		/// <param name="rcWorld">Rectangle to test</param>
		/// <returns>The number of child nodes that intersect the given rectangle</returns>
		int GetChildrenIntersecting(NodeCollection childNodes, RectangleF rcWorld);

		/// <summary>
		/// Returns all children inside the given rectangle.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children inside the specified rectangle
		/// </param>
		/// <param name="rcWorld">Rectangle to test</param>
		/// <returns>The number of child nodes added to the collection</returns>
		int GetChildrenContainedBy(NodeCollection childNodes, RectangleF rcWorld);

		/// <summary>
		/// Returns the inherited property container for the given child node.
		/// </summary>
		/// <param name="childNode">The child node making the request</param>
		/// <returns>Parent property container for the given node</returns>
		IPropertyContainer GetPropertyContainer(INode childNode);
	}
}