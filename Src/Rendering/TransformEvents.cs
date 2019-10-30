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
	/// Encapsulates arguements for move events.
	/// </summary>
	public class MoveEventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		public MoveEventArgs()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public MoveEventArgs(float dx, float dy)
		{
			this.dx = dx;
			this.dy = dy;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public MoveEventArgs(INode node, float dx, float dy)
		{
			this.nodes.Add(node);
			this.dx = dx;
			this.dy = dy;
		}

		/// <summary>
		/// 
		/// </summary>
		public NodeCollection Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float OffsetX
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
		/// 
		/// </summary>
		public float OffsetY
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

		private NodeCollection nodes = new NodeCollection();
		private float dx = 0;
		private float dy = 0;
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void MoveEventHandler(object sender, MoveEventArgs evtArgs);

	/// <summary>
	/// Encapsulates arguements for rotate events.
	/// </summary>
	public class RotateEventArgs : NodeEventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		public RotateEventArgs(INode node) : base(node)
		{
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void RotateEventHandler(object sender, RotateEventArgs evtArgs);

	/// <summary>
	/// Encapsulates arguements for scales events.
	/// </summary>
	public class ScaleEventArgs : NodeEventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		public ScaleEventArgs(INode node) : base(node)
		{
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void ScaleEventHandler(object sender, ScaleEventArgs evtArgs);
}
