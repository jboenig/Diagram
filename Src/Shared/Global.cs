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
using System.Drawing.Drawing2D;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// This class contains static methods and data that are used globally.
	/// </summary>
	public class Global
	{
		/// <summary>
		/// Flag indicating if initialization of the library is complete.
		/// </summary>
		public static bool InitDone = false;

		/// <summary>
		/// Current matrix stack.
		/// </summary>
		public static MatrixStack MatrixStack;

		/// <summary>
		/// Total number of available matrix stacks.
		/// </summary>
		public static int NumMatrixStacks = 2;

		/// <summary>
		/// Array of matrix stacks.
		/// </summary>
		private static MatrixStack[] MatrixStacks = null;

		/// <summary>
		/// Index used to select the rendering stack.
		/// </summary>
		public static int RenderingStack = 0;

		/// <summary>
		/// Index used to select the temporary stack.
		/// </summary>
		public static int TemporaryStack = 1;

		/// <summary>
		/// Index of the currently selected stack.
		/// </summary>
		private static int CurrentStack = 0;

		/// <summary>
		/// The view matrix.
		/// </summary>
		public static Matrix ViewMatrix;

		/// <summary>
		/// Initializes the library.
		/// </summary>
		public static void Initialize()
		{
			if (!InitDone)
			{
				MatrixStacks = new MatrixStack[NumMatrixStacks];
				for (int stackIdx = 0; stackIdx < NumMatrixStacks; stackIdx++)
				{
					MatrixStacks[stackIdx] = new MatrixStack();
				}
				SelectMatrixStack(0);
				ViewMatrix = new Matrix();
				InitDone = true;
			}
		}

		/// <summary>
		/// Selects a specific matrix to be the current stack.
		/// </summary>
		/// <param name="stackID">Index of stack to select</param>
		/// <returns>Previous stack index</returns>
		public static int SelectMatrixStack(int stackID)
		{
			int prevStack = -1;

			if (stackID < NumMatrixStacks)
			{
				prevStack = CurrentStack;
				CurrentStack = stackID;
				MatrixStack = MatrixStacks[CurrentStack];
			}

			return prevStack;
		}
	}
}
