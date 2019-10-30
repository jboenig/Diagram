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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// List of commands that are bundled into a single command.
	/// </summary>
	public class MacroCmd : ICommand
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public MacroCmd()
		{
		}

		/// <summary>
		/// Short, user-friendly description of the command.
		/// </summary>
		public string Description
		{
			get
			{
				return "MacroCommand";
			}
		}

		/// <summary>
		/// Indicates whether or not the command supports undo.
		/// </summary>
		public bool CanUndo
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Executes all commands in the macro.
		/// </summary>
		/// <param name="cmdTarget">Command target object</param>
		/// <returns>true if successful; otherwise false</returns>
		public bool Do(object cmdTarget)
		{
			bool success = true;

			foreach (ICommand cmd in this.cmds)
			{
				success = cmd.Do(cmdTarget);

				if (!success)
				{
					break;
				}
			}

			return success;
		}

		/// <summary>
		/// Reverses the command.
		/// </summary>
		/// <returns>true if successful; otherwise false</returns>
		public bool Undo()
		{
			bool success = true;

			foreach (ICommand cmd in this.cmds)
			{
				success = cmd.Undo();
				
				if (!success)
				{
					break;
				}
			}

			return success;
		}

		/// <summary>
		/// Add a command to the macro.
		/// </summary>
		/// <param name="cmd">Command to add</param>
		public void AddCommand(ICommand cmd)
		{
			this.cmds.Add(cmd);
		}

		/// <summary>
		/// Removes all commands from the macro.
		/// </summary>
		public void Clear()
		{
			this.cmds.Clear();
		}

		/// <summary>
		/// List of commands to execute
		/// </summary>
		protected ArrayList cmds = new ArrayList();
	}
}
