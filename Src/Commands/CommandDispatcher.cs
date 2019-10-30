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
	/// Interface to command dispatcher objects.
	/// </summary>
	/// <remarks>
	/// A command dispatcher is an object that acts as a central point
	/// for executing commands and provides command history.
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICommand"/>
	/// </remarks>
	public interface ICommandDispatcher
	{
		/// <summary>
		/// Maximum number of commands that can be stored in the undo and
		/// redo buffers.
		/// </summary>
		int MaxHistory
		{
			get;
			set;
		}

		/// <summary>
		/// Current number of commands in the undo buffer.
		/// </summary>
		int UndoCount
		{
			get;
		}

		/// <summary>
		/// Current number of commands in the redo buffer.
		/// </summary>
		int RedoCount
		{
			get;
		}

		/// <summary>
		/// Executes the given command. If successful, updates the undo buffer.
		/// </summary>
		/// <param name="cmd">Command to execute</param>
		/// <returns>true if command executed successfully; otherwise false</returns>
		bool ExecuteCommand(ICommand cmd);

		/// <summary>
		/// Pops the last command off of the undo stack and calls its Undo method.
		/// </summary>
		/// <returns>true if successful; otherwise false</returns>
		bool UndoCommand();

		/// <summary>
		/// Pops the last command off of the redo stack and calls its Do method.
		/// </summary>
		/// <returns>true if successful; otherwise false</returns>
		bool RedoCommand();

		/// <summary>
		/// Returns a command from the undo buffer based on position.
		/// </summary>
		/// <returns>Command at the given offset in the undo buffer</returns>
		ICommand PeekUndo(int offset);

		/// <summary>
		/// Returns a command from the redo buffer based on position.
		/// </summary>
		/// <returns>Command at the given offset in the redo buffer</returns>
		ICommand PeekRedo(int offset);

		/// <summary>
		/// Removes all commands from the undo and redo buffers.
		/// </summary>
		void ClearHistory();
	}
}
