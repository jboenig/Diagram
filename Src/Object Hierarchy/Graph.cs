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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interface to a node in a graph.
	/// </summary>
	/// <remarks>
	/// A node is an object in a graph that can have edges entering
	/// and leaving. Nodes are connected to other nodes by edges.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphEdge"/>
	/// </remarks>
	public interface IGraphNode
	{
		/// <summary>
		/// Collection of all edges entering or leaving the node.
		/// </summary>
		ICollection Edges
		{
			get;
		}

		/// <summary>
		/// Collection of edges entering the node.
		/// </summary>
		ICollection EdgesEntering
		{
			get;
		}

		/// <summary>
		/// Collection of edges leaving the node.
		/// </summary>
		ICollection EdgesLeaving
		{
			get;
		}
	}

	/// <summary>
	/// Interface to an edge in a graph.
	/// </summary>
	/// <remarks>
	/// An edge links together two nodes in a graph. It provides a path
	/// between two nodes.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphNode"/>
	/// </remarks>
	public interface IGraphEdge
	{
		/// <summary>
		/// Node connected to the tail of the edge.
		/// </summary>
		IGraphNode FromNode
		{
			get;
		}

		/// <summary>
		/// Node connected to the head of the edge.
		/// </summary>
		IGraphNode ToNode
		{
			get;
		}

		/// <summary>
		/// Weight value associated with the edge.
		/// </summary>
		int EdgeWeight
		{
			get;
		}

		/// <summary>
		/// Determines if this edge is leaving the given node.
		/// </summary>
		/// <param name="graphNode">Node to test</param>
		/// <returns>true if edge is leaving the given node</returns>
		bool IsNodeLeaving(IGraphNode graphNode);

		/// <summary>
		/// Determines if this edge is entering the given node.
		/// </summary>
		/// <param name="graphNode">Node to test</param>
		/// <returns>true if edge is entering the given node</returns>
		bool IsNodeEntering(IGraphNode graphNode);
	}
}
