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
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

using Syncfusion.Windows.Forms;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Interactive tool for editing the vertices of a shape.
	/// </summary>
	/// <remarks>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Tool"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IMouseEventReceiver"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.InsertVertexCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveVertexCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.DeleteVertexCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IPoints"/>
	/// </remarks>
	public class VertexEditTool : Tool, IMouseEventReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		public VertexEditTool() : base(Resources.Strings.Toolnames.Get("VertexEditTool"))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public VertexEditTool(string name) : base(name)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool CanActivate
		{
			get
			{
				bool value = false;

				if ((this.Enabled || this.Suspended) && !this.Active)
				{
					if (this.Controller.SelectionList.Count == 1)
					{
						INode selectedNode = this.Controller.SelectionList[0];
						IPropertyContainer propContainer = selectedNode as IPropertyContainer;
						if (propContainer != null)
						{
							object propVal = propContainer.GetPropertyValue("AllowVertexEdit");
							if (propVal != null)
							{
								value = (bool) propVal;
							}
						}
					}
				}

				return value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActivate()
		{
			base.OnActivate();

			if (this.Controller != null)
			{
				this.Controller.SelectHandleMode = SelectHandleType.Vertex;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDeactivate()
		{
			this.editMode = EditMode.None;

			if (this.Controller != null)
			{
				this.Controller.SelectHandleMode = SelectHandleType.Resize;
			}

			base.OnDeactivate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tool"></param>
		public override void ToolActivating(Tool tool)
		{
			this.Deactivate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active && e.Button == MouseButtons.Left)
			{
				if (this.Controller != null && this.View != null)
				{
					if (this.Controller.VertexHandleHitNode != null)
					{
						if (Control.ModifierKeys == Keys.Control)
						{
							this.editMode = EditMode.Deleting;
							this.node = this.Controller.VertexHandleHitNode;
							this.vertexIdx = this.Controller.VertexHit;
						}
						else
						{
							this.BeginVertexMove(this.Controller.VertexHandleHitNode,
							                     this.Controller.VertexHit,
							                     new System.Drawing.Point(e.X, e.Y));
						}
					}
					else if (this.Controller.SelectionList.Count == 1)
					{
						this.editMode = EditMode.Inserting;
						this.node = this.Controller.SelectionList[0];
						this.vertexIdx = -1;
						this.ChangeCursor(Resources.Cursors.EditVertex);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active)
			{
				if (this.Controller != null && this.View != null)
				{
					if (this.editMode == EditMode.Moving)
					{
						this.MoveVertex(new System.Drawing.Point(e.X, e.Y));
					}
					else
					{
						if (this.Controller.VertexHandleHitNode != null)
						{
							if (Control.ModifierKeys == Keys.Control)
							{
								this.ChangeCursor(Resources.Cursors.DeleteVertex);
							}
							else
							{
								this.ChangeCursor(Resources.Cursors.EditVertex);
							}
						}
						else
						{
							this.RestoreCursor();
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		void IMouseEventReceiver.MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.Active)
			{
				if (this.Controller != null && this.View != null && this.node != null)
				{
					IPoints objPoints = this.node as IPoints;

					System.Drawing.Point ptDev = new System.Drawing.Point(e.X, e.Y);

					if (this.View.Grid.SnapToGrid)
					{
						ptDev = this.View.SnapPointToGrid(ptDev);
					}

					switch (this.editMode)
					{
						case EditMode.Moving:
							this.EndVertexMove();

							System.Drawing.Size offsetDev = new System.Drawing.Size(ptDev.X - this.startingPoint.X, ptDev.Y - this.startingPoint.Y);
							System.Drawing.SizeF offsetWorld = this.View.ViewToWorld(this.View.DeviceToView(offsetDev));

							MoveVertexCmd moveVertexCmd = new MoveVertexCmd(this.node, this.vertexIdx, offsetWorld.Width, offsetWorld.Height);
							this.Controller.ExecuteCommand(moveVertexCmd);
							break;

						case EditMode.Deleting:
							if (objPoints != null && objPoints.PointCount > objPoints.MinPoints)
							{
								DeleteVertexCmd delVertexCmd = new DeleteVertexCmd(this.node, this.vertexIdx);
								this.Controller.ExecuteCommand(delVertexCmd);
							}
							break;

						case EditMode.Inserting:
							PointF ptWorld = this.View.ViewToWorld(this.View.DeviceToView(ptDev));
							InsertVertexCmd insVertexCmd = new InsertVertexCmd(this.node, ptWorld);
							this.Controller.ExecuteCommand(insVertexCmd);
							break;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="vertexIdx"></param>
		/// <param name="startingPt"></param>
		private void BeginVertexMove(INode node, int vertexIdx, System.Drawing.Point startingPt)
		{
			this.node = node;
			this.vertexIdx = vertexIdx;

			if (this.View.Grid.SnapToGrid)
			{
				this.startingPoint = this.View.SnapPointToGrid(startingPt);
			}
			else
			{
				this.startingPoint = startingPt;
			}

			this.trackingNode = null;
			this.trackingPath = null;

			if (this.Controller == null || this.View == null)
			{
				return;
			}

			if (this.node != null)
			{
				this.node = node;
				this.trackingNode = (INode) this.node.Clone();

				IPoints objPoints = this.trackingNode as IPoints;
				if (objPoints != null)
				{
					ITransform objXform = this.trackingNode as ITransform;
					PointF[] ptWorld = new PointF[1]
					{
						this.View.ViewToWorld(this.View.DeviceToView(startingPt))
					};

					if (objXform != null)
					{
						Matrix worldTransform = objXform.WorldTransform;
						worldTransform.Invert();
						worldTransform.TransformPoints(ptWorld);
					}
					objPoints.SetPoint(this.vertexIdx, ptWorld[0]);
				}

				int prevStack = Global.SelectMatrixStack(Global.TemporaryStack);
				Global.MatrixStack.Clear();

				ITransform nodeTrans = this.trackingNode as ITransform;
				if (nodeTrans != null)
				{
					Global.MatrixStack.Push(nodeTrans.ParentTransform);
				}

				IGraphics nodeGrfx = this.trackingNode as IGraphics;
				if (nodeGrfx != null)
				{
					this.trackingPath = nodeGrfx.GraphicsPath;
				}

				Global.SelectMatrixStack(prevStack);

				this.trackingRect = this.View.DrawTrackingPath(this.trackingPath);

				this.editMode = EditMode.Moving;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt"></param>
		private void MoveVertex(System.Drawing.Point pt)
		{
			if (this.Controller == null || this.View == null)
			{
				return;
			}

			if (this.editMode != EditMode.Moving || this.trackingNode == null)
			{
				return;
			}

			if (this.View.Grid.SnapToGrid)
			{
				pt = this.View.SnapPointToGrid(pt);
			}

			IPoints objPoints = this.trackingNode as IPoints;
			if (objPoints != null)
			{
				this.View.Refresh(this.trackingRect);

				ITransform objXform = this.trackingNode as ITransform;
				PointF[] ptWorld = new PointF[1]
				{
					this.View.ViewToWorld(this.View.DeviceToView(pt))
				};

				if (objXform != null)
				{
					Matrix worldTransform = objXform.WorldTransform;
					worldTransform.Invert();
					worldTransform.TransformPoints(ptWorld);
				}
				objPoints.SetPoint(this.vertexIdx, ptWorld[0]);

				int prevStack = Global.SelectMatrixStack(Global.TemporaryStack);
				Global.MatrixStack.Clear();

				ITransform nodeTrans = this.trackingNode as ITransform;
				if (nodeTrans != null)
				{
					Global.MatrixStack.Push(nodeTrans.ParentTransform);
				}

				IGraphics nodeGrfx = this.trackingNode as IGraphics;
				if (nodeGrfx != null)
				{
					this.trackingPath = nodeGrfx.GraphicsPath;
				}

				Global.SelectMatrixStack(prevStack);

				this.trackingRect = this.View.DrawTrackingPath(this.trackingPath);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void EndVertexMove()
		{
			if (this.editMode == EditMode.Moving)
			{
				this.View.Refresh(this.trackingRect);
				this.trackingNode = null;
				this.trackingPath = null;
				this.editMode = EditMode.None;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected enum EditMode
		{
			/// <summary>
			/// 
			/// </summary>
			None,

			/// <summary>
			/// 
			/// </summary>
			Inserting,

			/// <summary>
			/// 
			/// </summary>
			Moving,

			/// <summary>
			/// 
			/// </summary>
			Deleting
		}

		/// <summary>
		/// 
		/// </summary>
		private EditMode editMode = EditMode.None;

		/// <summary>
		/// 
		/// </summary>
		private INode node = null;

		/// <summary>
		/// 
		/// </summary>
		private INode trackingNode = null;

		/// <summary>
		/// 
		/// </summary>
		private System.Drawing.Point startingPoint;

		/// <summary>
		/// 
		/// </summary>
		private int vertexIdx = -1;

		/// <summary>
		/// 
		/// </summary>
		private GraphicsPath trackingPath = null;

		/// <summary>
		/// 
		/// </summary>
		private System.Drawing.Rectangle trackingRect;
	}
}
