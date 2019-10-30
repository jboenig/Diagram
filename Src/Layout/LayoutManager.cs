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
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for layout managers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class provides the basic plumbing for layout managers. A layout
	/// manager is an object that controls the positioning of nodes in a model.
	/// Each layout manager object is attached to a single model by the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LayoutManager.Model"/>
	/// property.
	/// </para>
	/// <para>
	/// Layout managers can operate one of two modes: manual or
	/// auto-update. When the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LayoutManager.AutoLayout"/>
	/// flag is set to true, the layout manager responds to events in the model
	/// by automatically repositioning nodes in the model. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LayoutManager.UpdateLayout"/>
	/// method can be called at any time to reposition the nodes in the model.
	/// If the AutoLayout flag is false, then UpdateLayout must be called manually
	/// in order to update the layout of nodes in the model.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model"/>
	/// </remarks>
	public abstract class LayoutManager : Component
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public LayoutManager()
		{
		}

		/// <summary>
		/// Construct a layout manager given a model.
		/// </summary>
		/// <param name="model"></param>
		public LayoutManager(Model model)
		{
			this.Model = model;
		}

		/// <summary>
		/// The model attached to this layout manager.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The model referenced by this property is updated when the
		/// UpdateLayout method is called.
		/// </para>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Model Model
		{
			get
			{
				return this.mdl;
			}
			set
			{
				if (this.mdl != value)
				{
					if (this.mdl != null)
					{
						this.mdl.ChildrenChangeComplete -= new NodeCollection.EventHandler(OnNodeCollectionChanged);
						this.mdl.BoundsChanged -= new BoundsEventHandler(OnNodeBoundsChanged);
						this.mdl.Moved -= new MoveEventHandler(OnNodeMoved);
						this.mdl.Rotated -= new RotateEventHandler(OnNodeRotated);
						this.mdl.Scaled -= new ScaleEventHandler(OnNodeScaled);
						this.mdl.ConnectionsChangeComplete -= new ConnectionCollection.EventHandler(OnConnectionsChanged);
					}

					this.mdl = value;

					if (this.mdl != null)
					{
						this.mdl.ChildrenChangeComplete += new NodeCollection.EventHandler(OnNodeCollectionChanged);
						this.mdl.BoundsChanged += new BoundsEventHandler(OnNodeBoundsChanged);
						this.mdl.Moved += new MoveEventHandler(OnNodeMoved);
						this.mdl.Rotated += new RotateEventHandler(OnNodeRotated);
						this.mdl.Scaled += new ScaleEventHandler(OnNodeScaled);
						this.mdl.ConnectionsChangeComplete += new ConnectionCollection.EventHandler(OnConnectionsChanged);
					}
				}
			}
		}

		/// <summary>
		/// Determines if layout manager automatically updates the layout of the model.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Behavior"),
		Description("Determines if layout manager automatically updates the layout of the model")
		]
		public bool AutoLayout
		{
			get
			{
				return this.autoLayoutFlag;
			}
			set
			{
				this.autoLayoutFlag = value;
			}
		}

		/// <summary>
		/// Updates the layout of the nodes in the model.
		/// </summary>
		/// <param name="contextInfo">Provides context infomation to help with updating the layout</param>
		/// <returns>true if changes were made; otherwise false</returns>
		public abstract bool UpdateLayout(object contextInfo);

		/// <summary>
		/// Called when nodes are added or removed from the model.
		/// </summary>
		/// <param name="sender">Model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		protected virtual void OnNodeCollectionChanged(object sender, NodeCollection.EventArgs evtArgs)
		{
			if (this.autoLayoutFlag)
			{
				this.UpdateLayout(null);
			}
		}

		/// <summary>
		/// Called when the bounds of a node changes in the model.
		/// </summary>
		/// <param name="sender">Model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		protected virtual void OnNodeBoundsChanged(object sender, BoundsEventArgs evtArgs)
		{
			if (this.autoLayoutFlag)
			{
				this.UpdateLayout(null);
			}
		}

		/// <summary>
		/// Called when one or more nodes are moved.
		/// </summary>
		/// <param name="sender">Model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		protected virtual void OnNodeMoved(object sender, MoveEventArgs evtArgs)
		{
			if (this.autoLayoutFlag)
			{
				this.UpdateLayout(null);
			}
		}

		/// <summary>
		/// Called when one or more nodes are scaled.
		/// </summary>
		/// <param name="sender">Model sending the event</param>
		/// <param name="evtArgs">Event arguments</param>
		protected virtual void OnNodeScaled(object sender, ScaleEventArgs evtArgs)
		{
			if (this.autoLayoutFlag)
			{
				this.UpdateLayout(null);
			}
		}

		/// <summary>
		/// Called when one or more nodes in the model are scaled.
		/// </summary>
		/// <param name="sender">Model sending the event</param>
		/// <param name="evtArgs"></param>
		protected virtual void OnNodeRotated(object sender, RotateEventArgs evtArgs)
		{
			if (this.autoLayoutFlag)
			{
				this.UpdateLayout(null);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="evtArgs"></param>
		protected virtual void OnConnectionsChanged(object sender, ConnectionCollection.EventArgs evtArgs)
		{
			if (this.autoLayoutFlag)
			{
				this.UpdateLayout(null);
			}
		}

		/// <summary>
		/// Reference to model to layout
		/// </summary>
		protected Model mdl = null;

		/// <summary>
		/// Flag indicating if layout is to be updated automatically
		/// </summary>
		protected bool autoLayoutFlag = false;
	}
}
