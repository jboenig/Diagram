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
	/// Summary description for ENoImplementation.
	/// </summary>
	public class ENoImplementation : System.Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public ENoImplementation()
		{
		}
	}

	/// <summary>
	/// Summary description for EDuplicateName.
	/// </summary>
	public class EDuplicateName : System.Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public EDuplicateName()
		{
		}
	}

	/// <summary>
	/// Summary description for EMVCInit.
	/// </summary>
	public class EMVCInit : System.Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public EMVCInit()
		{
		}
	}

	/// <summary>
	/// Summary description for EInvalidOperation.
	/// </summary>
	public class EInvalidOperation : System.Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public EInvalidOperation()
		{
		}
	}

	/// <summary>
	/// Summary description for EInvalidParameter.
	/// </summary>
	public class EInvalidParameter : System.Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public EInvalidParameter()
		{
		}
	}

	/// <summary>
	/// Summary description for EInvalidConnection.
	/// </summary>
	public class EInvalidConnection : System.Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public EInvalidConnection()
		{
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class ESlopeUndefined : System.Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public ESlopeUndefined()
		{
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class EBoundaryConstraint : System.Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public EBoundaryConstraint(INode node)
		{
			this.node = node;
		}

		/// <summary>
		/// 
		/// </summary>
		public INode Node
		{
			get
			{
				return this.node;
			}
		}

		private INode node = null;
	}
}