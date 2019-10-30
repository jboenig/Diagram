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
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A connection is an object that binds two ports together.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A connection consists of two ports: a source port and a target port.
	/// The distinction between the source port and target port is not
	/// really signficant, because a connection doesn't have a direction.
	/// The names source and target could just as easily be Port1 and
	/// Port2. The two ports involved in the connection can be accessed
	/// through the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Connection.SourcePort"/>
	/// and
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Connection.TargetPort"/>
	/// properties.
	/// </para>
	/// <para>
	/// Each port is owned by an object that implements the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
	/// interface, which will typically be either a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Symbol"/>
	/// or a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Link"/>. A port
	/// container has methods for accessing the ports and connections
	/// it contains, and it has methods that are called by connections
	/// in order to notify the port container of events that affect
	/// the connection. For example, the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IPortContainer.OnConnectionMove"/>
	/// method is called by a connection to notify a port container that
	/// the other port container it is connected to has moved.
	/// </para>
	/// <para>
	/// This class contains methods to help navigate from one port container
	/// to another. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Connection.GetLocalPort"/>
	/// method takes a port container as a parameter and returns the either
	/// the <see cref="Syncfusion.Windows.Forms.Diagram.Connection.SourcePort"/>
	/// or the <see cref="Syncfusion.Windows.Forms.Diagram.Connection.TargetPort"/>,
	/// depending on which one belongs to the given container. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Connection.GetForeignPort"/>
	/// method does the reverse. It returns the port that does not belong to the
	/// given port container.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPortContainer"/>
	/// </remarks>
	[Serializable()]
	public class Connection : ISerializable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Connection()
		{
		}

		/// <summary>
		/// Constructs a connection from two ports.
		/// </summary>
		/// <param name="sourcePort">Source port</param>
		/// <param name="targetPort">Target port</param>
		public Connection(Port sourcePort, Port targetPort)
		{
			if (sourcePort == null || targetPort == null)
			{
				throw new EInvalidParameter();
			}

			this.SourcePort = sourcePort;
			this.TargetPort = targetPort;
		}

		/// <summary>
		/// Serialization constructor for connections.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Connection(SerializationInfo info, StreamingContext context)
		{
			this.sourcePort = (Port) info.GetValue("sourcePort", typeof(Port));
			this.targetPort = (Port) info.GetValue("targetPort", typeof(Port));
		}

		/// <summary>
		/// Source port involved in the connection.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		public Port SourcePort
		{
			get
			{
				return this.sourcePort;
			}
			set
			{
				Port sourcePort = value;

				if (this.sourcePort != sourcePort)
				{
					if (sourcePort != null && this.targetPort != null)
					{
						if (sourcePort.Container == this.targetPort.Container)
						{
							throw new EInvalidConnection();
						}
					}

					this.sourcePort = sourcePort;
				}
			}
		}

		/// <summary>
		/// Target port involved in the connection.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
		/// </remarks>
		public Port TargetPort
		{
			get
			{
				return this.targetPort;
			}
			set
			{
				Port targetPort = value;

				if (this.targetPort != targetPort)
				{
					if (targetPort != null && this.sourcePort != null)
					{
						if (targetPort.Container == this.sourcePort.Container)
						{
							throw new EInvalidConnection();
						}
					}

					this.targetPort = targetPort;
				}
			}
		}

		/// <summary>
		/// Takes a port container as a parameter and returns the either
		/// the SourcePort or TargetPort, depending on which one belongs to the
		/// given container.
		/// </summary>
		/// <param name="container">Port container to test</param>
		/// <returns>Port belonging to the given port container</returns>
		/// <remarks>
		/// <para>
		/// If the SourcePort belongs to the given port container, then it is
		/// the port returned to the caller. If the TargetPort belongs to the
		/// given port container, then it is the port returned to the caller.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection.SourcePort"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection.TargetPort"/>
		/// </remarks>
		public Port GetLocalPort(IPortContainer container)
		{
			if (this.sourcePort.Container == container)
			{
				return this.sourcePort;
			}
			else if (this.targetPort.Container == container)
			{
				return this.targetPort;
			}
			return null;
		}

		/// <summary>
		/// Takes a port container as a parameter and returns the either
		/// the SourcePort or TargetPort, depending on which one does
		/// not belong to the given container.
		/// </summary>
		/// <param name="container">Port container to test</param>
		/// <returns>Port not belonging to the given port container</returns>
		/// <remarks>
		/// <para>
		/// If the SourcePort belongs to the given port container, then the
		/// TargetPort is returned. If the TargetPort belongs to the
		/// given port container, then the SourcePort is returned.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection.SourcePort"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection.TargetPort"/>
		/// </remarks>
		public Port GetForeignPort(IPortContainer container)
		{
			if (this.sourcePort.Container == container)
			{
				return this.targetPort;
			}
			else if (this.targetPort.Container == container)
			{
				return this.sourcePort;
			}
			return null;
		}

		/// <summary>
		/// Notifies the connection that one of the port containers has moved.
		/// </summary>
		/// <param name="container">Container that moved</param>
		/// <remarks>
		/// This method calls the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IPortContainer.OnConnectionMove"/>
		/// method on the foreign port container. The foreign port container is the
		/// port container that is not the one passed in as a parameter.
		/// </remarks>
		public void SymbolMoved(IPortContainer container)
		{
			Port receiverPort = this.GetForeignPort(container);
			IPortContainer receiver;

			if (receiverPort != null)
			{
				receiver = receiverPort.Container;

				if (receiver != null)
				{
					receiver.OnConnectionMove(this);
				}
			}
		}

		#region Serialization

		/// <summary>
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("sourcePort", this.sourcePort);
			info.AddValue("targetPort", this.targetPort);
		}

		#endregion

		/// <summary>
		/// First port in the connection.
		/// </summary>
		private Port sourcePort = null;

		/// <summary>
		/// Second port in the connection.
		/// </summary>
		private Port targetPort = null;
	}

}
