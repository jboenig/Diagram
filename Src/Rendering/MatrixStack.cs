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
using System.Drawing.Drawing2D;
using System.Collections;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// This class implements a stack collection for Matrix objects.
	/// </summary>
	public class MatrixStack
	{
		/// <summary>
		/// Constructs a matrix stack.
		/// </summary>
		public MatrixStack()
		{
			this.stack = new System.Collections.Stack(12);
		}

		/// <summary>
		/// Push the given matrix onto the stack.
		/// </summary>
		/// <param name="m">Matrix to push onto the stack</param>
		/// <returns>Result matrix</returns>
		/// <remarks>
		/// The matrix is passed in is combined with the matrix on the
		/// top of the stack to produce the return value. The matrices are
		/// combined using matrix multiplication, where the incoming matrix
		/// is appended to the matrix on the top of the stack. If the
		/// stack is empty, then the matrix passed in is returned.
		/// </remarks>
		public Matrix Push(Matrix m)
		{
			Matrix resMatrix;

			if (m != null)
			{
				if (this.stack.Count > 0)
				{
					resMatrix = ((Matrix) this.stack.Peek()).Clone();
					resMatrix.Multiply(m, MatrixOrder.Append);
				}
				else
				{
					resMatrix = m.Clone();
				}

				this.stack.Push(resMatrix);
			}
			else
			{
				resMatrix = ((Matrix) this.stack.Peek()).Clone();
			}

			return resMatrix;
		}

		/// <summary>
		/// Push the given matrix onto the stack given a matrix order to
		/// produce the result.
		/// </summary>
		/// <param name="m">Matrix to push onto the stack</param>
		/// <param name="order">
		/// Order in which multiplication will take place when generating
		/// the result matrix
		/// </param>
		/// <returns>Result matrix</returns>
		public Matrix Push(Matrix m, MatrixOrder order)
		{
			Matrix resMatrix;

			if (m != null)
			{
				if (this.stack.Count > 0)
				{
					resMatrix = ((Matrix) this.stack.Peek()).Clone();
					resMatrix.Multiply(m, order);
				}
				else
				{
					resMatrix = m.Clone();
				}

				this.stack.Push(resMatrix);
			}
			else
			{
				resMatrix = ((Matrix) this.stack.Peek()).Clone();
			}

			return resMatrix;
		}

		/// <summary>
		/// Return the matrix on the top of the stack.
		/// </summary>
		/// <returns>Matrix on top of the stack</returns>
		public Matrix Peek()
		{
			if (this.stack.Count > 0)
			{
				return (Matrix) this.stack.Peek();
			}
			return new Matrix();
		}

		/// <summary>
		/// Removes the matrix on the top from the stack and returns it.
		/// </summary>
		/// <returns>Matrix popped from the stack</returns>
		public Matrix Pop()
		{
			if (this.stack.Count > 0)
			{
				return (Matrix) this.stack.Pop();
			}
			return new Matrix();
		}

		/// <summary>
		/// Remove all matrix objects from the stack.
		/// </summary>
		public void Clear()
		{
			this.stack.Clear();
		}

		private System.Collections.Stack stack;
	}
}