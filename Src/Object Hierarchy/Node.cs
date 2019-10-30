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
	/// Interface to a node in a hierarchy or graph of objects.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A node is a named object in a hieararchical tree structure. Each node
	/// has a <see cref="Syncfusion.Windows.Forms.Diagram.INode.Name"/>
	/// and a parent. A node's name must is unique within the scope of its
	/// parent node. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.INode.FullName"/>
	/// of a node is  unique within the scope of the entire node hieararchy.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICompositeNode"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
	/// </remarks>
	public interface INode : System.IServiceProvider, System.ICloneable
	{
		/// <summary>
		/// Reference to the composite node this node is a child of.
		/// </summary>
		ICompositeNode Parent
		{
			get;
			set;
		}

		/// <summary>
		/// The root node in the node hieararchy.
		/// </summary>
		/// <remarks>
		/// The root node is found by following the chain of parent nodes until
		/// a node is found that has a null parent.
		/// </remarks>
		INode Root
		{
			get;
		}

		/// <summary>
		/// Name of the node.
		/// </summary>
		/// <remarks>
		/// Must be unique within the scope of the parent node.
		/// </remarks>
		string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Fully qualified name of the node.
		/// </summary>
		/// <remarks>
		/// The full name is the name of the node concatenated with the names
		/// of all parent nodes.
		/// </remarks>
		string FullName
		{
			get;
		}
	}
}