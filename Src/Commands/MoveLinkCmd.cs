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
	/// Moves either the head port or tail port of a link and optionally
	/// connects it to another symbol.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link"/>
	/// </remarks>
	public class MoveLinkCmd : SingleNodeCmd
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public MoveLinkCmd()
		{
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public override string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("MoveLinkCmd");
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
		/// Moves the port on the link and optionally reconnects to a new
		/// port.
		/// </summary>
		/// <param name="cmdTarget">Unused</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// First, any existing connections on the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.MoveLinkCmd.LinkPort"/>
		/// are removed. Next, if the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.MoveLinkCmd.Points"/>
		/// property is not null it is used to update link's vertices. And
		/// finally, the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.MoveLinkCmd.LinkPort"/>
		/// is connected to the 
		/// <see cref="Syncfusion.Windows.Forms.Diagram.MoveLinkCmd.TargetPort"/>.
		/// </remarks>
		public override bool Do(object cmdTarget)
		{
			bool success = false;

			if (this.link != null && this.linkPort != null)
			{
				this.link.DisconnectAll(this.linkPort);

				if (this.points != null)
				{
					IServiceProvider linkSvcProvider = this.link as IServiceProvider;
					if (linkSvcProvider != null)
					{
						IPoints ptsObj = linkSvcProvider.GetService(typeof(IPoints)) as IPoints;
						if (ptsObj != null)
						{
							ptsObj.SetPoints(this.points);
						}
					}
				}

				if (this.targetPort != null)
				{
					this.link.Connect(this.linkPort, this.targetPort);
				}
			}

			return success;
		}

		/// <summary>
		/// Restores the Link object to the state it was in before the
		/// command was executed.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public override bool Undo()
		{
			return false;
		}

		/// <summary>
		/// Link to be updated.
		/// </summary>
		public Link Link
		{
			get
			{
				return this.link;
			}
			set
			{
				this.link = value;
			}
		}

		/// <summary>
		/// Port on the link that will be moved.
		/// </summary>
		public Port LinkPort
		{
			get
			{
				return this.linkPort;
			}
			set
			{
				this.linkPort = value;
			}
		}

		/// <summary>
		/// Port to connect the LinkPort to.
		/// </summary>
		/// <remarks>
		/// If this property is null, no connection is made on the LinkPort.
		/// </remarks>
		public Port TargetPort
		{
			get
			{
				return this.targetPort;
			}
			set
			{
				this.targetPort = value;
			}
		}

		/// <summary>
		/// Points to assign to the link.
		/// </summary>
		public PointF[] Points
		{
			get
			{
				return this.points;
			}
			set
			{
				this.points = value;
			}
		}

		private Link link = null;
		private Port linkPort = null;
		private Port targetPort = null;
		private PointF[] points = null;
	}
}
