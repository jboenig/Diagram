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
	/// Interface to objects that contain a collection of layers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface provides methods for managing nodes across a collection
	/// of layers.
	/// </para>
	/// <para>
	/// IMPORTANT NOTE: A node can belong to only one layer.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer"/>
	/// </remarks>
	public interface ILayerContainer
	{
		/// <summary>
		/// Determines if the container has a layer matching the specified name.
		/// </summary>
		/// <param name="layerName">Name of layer to search for</param>
		/// <returns>true if the layer is found, otherwise false</returns>
		bool Contains(string layerName);

		/// <summary>
		/// Finds the layer to which the specified node belongs.
		/// </summary>
		/// <param name="node">Node to search for</param>
		/// <returns>Layer containing the specified node</returns>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Layer"/>
		Layer FindNodeLayer(INode node);

		/// <summary>
		/// Property container that the all layers in the layer container inherit.
		/// </summary>
		/// <remarks>
		/// Nodes in a layer inherit properties from the layer they belong to. Layers
		/// inherit properties from the property container referenced by this property.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPropertyContainer"/>
		/// </remarks>
		IPropertyContainer PropertyContainer
		{
			get;
		}
	}
}
