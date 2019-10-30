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
	/// This interface is implemented by command objects.
	/// </summary>
	/// <remarks>
	/// A command object encapsulates an action and the data required to
	/// perform the action. A command is executed using the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IVerb.Do"/> and can
	/// be reversed using the <see cref="Syncfusion.Windows.Forms.Diagram.ICommand.Undo"/>
	/// method.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IVerb"/>
	/// </remarks>
	public interface ICommand : IVerb
	{
		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		string Description
		{
			get;
		}

		/// <summary>
		/// Indicates whether or not the command supports undo.
		/// </summary>
		bool CanUndo
		{
			get;
		}

		/// <summary>
		/// Reverses the command.
		/// </summary>
		/// <returns>true if successful; otherwise false</returns>
		bool Undo();
	}
}
