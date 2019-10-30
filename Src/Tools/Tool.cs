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
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for user interface tools.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A tool is an object that receives input from a controller and
	/// implements a piece of functionality or feature. Tools are helper
	/// objects that plug into a controller. Tools are attached to a
	/// controller using the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.RegisterTool"/>
	/// method. Each tool has a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Tool.Name"/>, which
	/// must be unique within a controller. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.GetTool"/>
	/// method in the controller can be used to lookup a tool by
	/// name.
	/// </para>
	/// <para>
	/// Activation and deactivation of tools is coordinated by the controller.
	/// The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Controller.ActivateTool"/>
	/// method activates a given tool. The controller notifies all other
	/// tools about the activation. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Tool.ToolActivating"/>
	/// method is called by the controller when another tool is being
	/// activated, which gives the tool the opportunity to either suspend or
	/// cancel what it is doing. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Tool.ToolDeactivating"/>
	/// method is called by the controller when another tool is being
	/// deactivated, which gives the tool the opportunity to resume what
	/// it was doing prior to being suspended.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Controller"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.View"/>
	/// </remarks>
	public abstract class Tool
	{
		/// <summary>
		/// Construct a tool with a given name
		/// </summary>
		/// <param name="name"></param>
		public Tool(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Name of the tool
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>
		/// Controller that the tool is plugged into
		/// </summary>
		public Controller Controller
		{
			get
			{
				return this.controller;
			}
			set
			{
				this.controller = value;
				OnControllerChange();
			}
		}

		/// <summary>
		/// View attached to the controller that the tool is plugged into
		/// </summary>
		public View View
		{
			get
			{
				if (this.controller != null)
				{
					return this.controller.View;
				}
				return null;
			}
		}

		/// <summary>
		/// Enables and disables the tool
		/// </summary>
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (!value && this.active)
				{
					this.Deactivate();
				}

				this.enabled = value;
			}
		}

		/// <summary>
		/// Indicates if the tool is active
		/// </summary>
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		/// <summary>
		/// Indicates if the tool is currently suspended.
		/// </summary>
		public bool Suspended
		{
			get
			{
				return this.suspended;
			}
		}

		/// <summary>
		/// Indicates if the tool runs in exclusive mode when activated
		/// </summary>
		public virtual bool Exclusive
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Indicates if the tool can activate
		/// </summary>
		public virtual bool CanActivate
		{
			get
			{
				return ((this.enabled || this.suspended) && !this.active);
			}
		}

		/// <summary>
		/// Activates the tool
		/// </summary>
		/// <returns>true if tool is active</returns>
		public bool Activate()
		{
			// Resume if currently suspended
			if (this.suspended)
			{
				this.Resume();
			}

			if (this.enabled && !this.active)
			{
				this.prevCursor = Cursor.Current;
				this.active = true;
				OnActivate();
			}

			return (this.Active);
		}

		/// <summary>
		/// Deactivates the tool
		/// </summary>
		/// <returns>true if successful, otherwise false</returns>
		public bool Deactivate()
		{
			bool success = false;

			if (this.Active)
			{
				OnDeactivate();
				this.active = false;
				RestoreCursor();
				success = true;
			}

			return success;
		}

		/// <summary>
		/// Puts the tool into a suspended state
		/// </summary>
		public virtual void Suspend()
		{
			if (!this.suspended && this.enabled)
			{
				this.Enabled = false;
				this.suspended = true;
			}
		}

		/// <summary>
		/// Removes the tool from a suspended state
		/// </summary>
		public virtual void Resume()
		{
			if (this.suspended)
			{
				this.suspended = false;
				this.Enabled = true;
			}
		}

		/// <summary>
		/// Called when the user presses the ESC key.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method is virtual and can be overriden in derived classes. The default
		/// implementation provided by this class deactivates the tool if it is active
		/// and in exclusive mode.
		/// </para>
		/// </remarks>
		public virtual void UserAbort()
		{
			if (this.controller != null && this.active && this.Exclusive)
			{
				this.controller.DeactivateTool(this);
			}
		}

		/// <summary>
		/// Called when another tool is activating
		/// </summary>
		/// <param name="tool">The tool that is being activated</param>
		/// <remarks>
		/// Gives the tool the opportunity to suspend or deactivate itself
		/// </remarks>
		public virtual void ToolActivating(Tool tool)
		{
			if (tool.Exclusive)
			{
				this.Suspend();
			}
			else if (this.Active)
			{
				this.Deactivate();
			}
		}

		/// <summary>
		/// Called when another tool is in the process of being deactivated
		/// </summary>
		/// <param name="tool">The tool that is being deactivated</param>
		/// <remarks>
		/// Gives the tool the opportunity to resume after being suspended
		/// </remarks>
		public virtual void ToolDeactivating(Tool tool)
		{
			if (tool.Exclusive)
			{
				this.Resume();
			}
		}

		/// <summary>
		/// Called when a controller is attached to the tool
		/// </summary>
		protected virtual void OnControllerChange()
		{
		}

		/// <summary>
		/// Saves the current cursor
		/// </summary>
		protected void SaveCursor()
		{
			this.prevCursor = Cursor.Current;
		}

		/// <summary>
		/// Changes the current cursor
		/// </summary>
		/// <param name="newCursor">Cursor to use</param>
		protected void ChangeCursor(Cursor newCursor)
		{
			if (this.controller != null)
			{
				if (this.prevCursor == null)
				{
					this.prevCursor = Cursor.Current;
				}

				this.controller.Cursor = newCursor;
			}
		}

		/// <summary>
		/// Restores the saved cursor
		/// </summary>
		protected void RestoreCursor()
		{
			if (this.prevCursor != null)
			{
				if (this.controller != null)
				{
					this.controller.Cursor = this.prevCursor;
				}
				this.prevCursor = null;
			}
		}

		/// <summary>
		/// Indicates if the tool has changed the cursor
		/// </summary>
		protected bool CursorChanged
		{
			get
			{
				return (this.prevCursor != null);
			}
		}

		/// <summary>
		/// Called when the tool is activated
		/// </summary>
		protected virtual void OnActivate()
		{
		}

		/// <summary>
		/// Called when the tool is deactivated
		/// </summary>
		protected virtual void OnDeactivate()
		{
		}

		/// <summary>
		/// Name of the tool
		/// </summary>
		private string name = null;

		/// <summary>
		/// Controller the tool is plugged into
		/// </summary>
		private Controller controller = null;

		/// <summary>
		/// Flag indicating if the tool is enabled or disabled
		/// </summary>
		private bool enabled = true;

		/// <summary>
		/// Flag indicating if the tool is active or inactive
		/// </summary>
		private bool active = false;

		/// <summary>
		/// Flag indicating if the tool is in a suspended state
		/// </summary>
		private bool suspended = false;

		/// <summary>
		/// Cursor that is saved and restored
		/// </summary>
		private Cursor prevCursor = null;
	}
}