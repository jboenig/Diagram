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
	/// Creates a Link object and connects it to two symbols.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This command creates a new link and adds it to the diagram. It then
	/// creates a connection between the tail port of the link and the port
	/// specified by the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LinkCmd.SourcePort"/>
	/// property. Next, it creates a connection between the head port of
	/// the link and the port specified by the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LinkCmd.TargetPort"/>
	/// property. The SourcePort and TargetPort are allowed to be null. If
	/// the SourcePort is null, then no connection will be created for the
	/// tail port of the link. If the TargetPort is null, then no connection
	/// will be created for the head port of the link.
	/// </para>
	/// <para>
	/// Creation of the link can be controller by setting the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LinkCmd.LinkFactory"/>
	/// property, which is a delegate that is invoked when the command
	/// needs to create the link. The LinkFactory can be used to create
	/// links containing different shapes and properties. It can also be
	/// used to create objects derived from the Link class. The default
	/// LinkFactory creates a an object of type
	/// Syncfusion.Windows.Forms.Diagram.Link containing a polyline.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link"/>
	/// </remarks>
	public class LinkCmd : ICommand
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LinkCmd()
		{
			this.linkFactory = new LinkFactory(DefaultLinkFactory);
		}

		/// <summary>
		/// Construct a LinkCmd object given two ports and an array of points.
		/// </summary>
		/// <param name="sourcePort">Port to connect to the tail of the link</param>
		/// <param name="targetPort">Port to connect to the head of the link</param>
		/// <param name="pts">Array of points to create the link with</param>
		public LinkCmd(Port sourcePort, Port targetPort, PointF[] pts)
		{
			this.linkFactory = new LinkFactory(DefaultLinkFactory);
			this.SourcePort = sourcePort;
			this.TargetPort = targetPort;
			this.Points = pts;
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public string Description
		{
			get
			{
				return Resources.Strings.CommandDescriptions.Get("LinkCmd");
			}
		}

		/// <summary>
		/// Indicates whether or not the command supports undo.
		/// </summary>
		public bool CanUndo
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Creates a Link object and connects the tail port and head
		/// port of the link.
		/// </summary>
		/// <param name="cmdTarget">Parent node to add the link to</param>
		/// <returns>true if successful, otherwise false</returns>
		/// <remarks>
		/// If the <see cref="Syncfusion.Windows.Forms.Diagram.LinkCmd.Link"/>
		/// property is null when this method is called, then a new Link object
		/// is created by invoking the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LinkCmd.LinkFactory"/>
		/// delegate. The Link object is then added to the node passed in the
		/// cmdTarget parameter. Next, the tail of the link is connected to the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LinkCmd.SourcePort"/>
		/// and the head of the link is connected to the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LinkCmd.TargetPort"/>.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LinkCmd.LinkFactory"/>
		/// </remarks>
		public bool Do(object cmdTarget)
		{
			bool success = false;
			ICompositeNode parentNode = null;
			int childIdx;

			if (this.link == null)
			{
				this.link = this.linkFactory(this.points);
			}

			if (this.link != null)
			{
				parentNode = cmdTarget as ICompositeNode;

				if (parentNode != null)
				{
					childIdx = parentNode.AppendChild(this.link);

					if (this.sourcePort != null)
					{
						this.link.ConnectTail(this.sourcePort);
					}

					if (this.targetPort != null)
					{
						this.link.ConnectHead(this.targetPort);
					}

					success = true;
				}
			}

			return success;
		}

		/// <summary>
		/// Removes the Link object from the diagram and disconnects it
		/// from the symbols.
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public bool Undo()
		{
			return false;
		}

		/// <summary>
		/// Port on symbol to connect the tail port of the link to.
		/// </summary>
		public Port SourcePort
		{
			get
			{
				return this.sourcePort;
			}
			set
			{
				this.sourcePort = value;
			}
		}

		/// <summary>
		/// Port on symbol to connect the head port of the link to.
		/// </summary>
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
		/// The Link object to add to the diagram.
		/// </summary>
		/// <remarks>
		/// If this property is null when the command is executed, then the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.LinkCmd.LinkFactory"/>
		/// is used to create a Link object.
		/// </remarks>
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
		/// Delegate that is called to create the Link object.
		/// </summary>
		/// <remarks>
		/// This property allows the caller to plugin a function that creates
		/// and configures the Link.
		/// </remarks>
		public LinkFactory LinkFactory
		{
			set
			{
				if (value != null)
				{
					this.linkFactory = value;
				}
			}
		}

		/// <summary>
		/// Creates a Link object and loads it with the array of points.
		/// </summary>
		/// <param name="pts">Array of points to add to link</param>
		/// <returns>Link object</returns>
		/// <remarks>
		/// The Link object created contains a PolyLine shape.
		/// </remarks>
		protected Link DefaultLinkFactory(PointF[] pts)
		{
			return new Link(pts);
		}

		/// <summary>
		/// Array of points to add to the Link object.
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

		private LinkFactory linkFactory = null;
		private Port sourcePort = null;
		private Port targetPort = null;
		private Link link = null;
		private PointF[] points = null;
	}
}
