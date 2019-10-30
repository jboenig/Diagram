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
	/// Interface to objects that contain ports and connections.
	/// </summary>
	public interface IPortContainer : IServiceProvider
	{
		/// <summary>
		/// 
		/// </summary>
		PortCollection Ports
		{
			get;
		}

		/// <summary>
		/// 
		/// </summary>
		ConnectionCollection Connections
		{
			get;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourcePort"></param>
		/// <param name="targetPort"></param>
		/// <returns></returns>
		bool AcceptConnection(Port sourcePort, Port targetPort);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourcePort"></param>
		/// <param name="targetPort"></param>
		/// <returns></returns>
		Connection Connect(Port sourcePort, Port targetPort);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourcePort"></param>
		/// <param name="targetPort"></param>
		void Disconnect(Port sourcePort, Port targetPort);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connections"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		int GetConnectionsOnPort(ConnectionCollection connections, Port port);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		Port GetLocalPortOnConnection(Connection connection);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		Port GetForeignPortOnConnection(Connection connection);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptWorld"></param>
		/// <param name="fSlop"></param>
		/// <returns></returns>
		Port GetPortAt(PointF ptWorld, float fSlop);

		/// <summary>
		/// 
		/// </summary>
		bool AutoHidePorts
		{
			get;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connection"></param>
		void OnConnectionMove(Connection connection);
	}
}
