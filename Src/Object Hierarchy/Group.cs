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
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A group is a node that acts as a transparent container for other nodes.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A group is a composite node that controls a set of child nodes. The
	/// bounding rectangle of a group is the union of the bounds of its
	/// children. The group is renders itself by iterating through its children
	/// and rendering them. Child nodes cannot be selected or manipulated
	/// individually.
	/// </para>
	/// <para>
	/// Members of the group are added and removed through the ICompositeNode
	/// interface.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.GroupCmd"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ICompositeNode"/>
	/// </remarks>
	[Serializable]
	public class Group : ICompositeNode, IDispatchNodeEvents, IBounds2DF, ILocalBounds2DF, IGraphics, ILogicalUnitContainer, IPropertyContainer, ITransform, IHitTestBounds, ISerializable, IDeserializationCallback
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Group()
		{
			this.matrix = new Matrix();
			this.children = new NodeCollection();
			this.children.Changing += new NodeCollection.EventHandler(Children_Changing);
			this.children.ChangeComplete += new NodeCollection.EventHandler(Children_ChangeComplete);
			this.propertyValues = new Hashtable();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src"></param>
		public Group(Group src)
		{
			this.name = src.name;
			this.matrix = src.matrix.Clone();
			this.children = (NodeCollection) src.children.Clone();
			this.children.Changing += new NodeCollection.EventHandler(Children_Changing);
			this.children.ChangeComplete += new NodeCollection.EventHandler(Children_ChangeComplete);
			this.propertyValues = (Hashtable) src.propertyValues.Clone();
		}

		/// <summary>
		/// Serialization constructor for symbols.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Group(SerializationInfo info, StreamingContext context)
		{
			this.name = info.GetString("name");
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
			this.parent = (ICompositeNode) info.GetValue("parent", typeof(ICompositeNode));
			float m11 = info.GetSingle("m11");
			float m12 = info.GetSingle("m12");
			float m21 = info.GetSingle("m21");
			float m22 = info.GetSingle("m22");
			float dx = info.GetSingle("dx");
			float dy = info.GetSingle("dy");
			this.matrix = new Matrix(m11, m12, m21, m22, dx, dy);
			this.children = (NodeCollection) info.GetValue("children", typeof(NodeCollection));
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public object Clone()
		{
			return new Group(this);
		}

		#endregion

		#region IServiceProvider interface

		/// <summary>
		/// Returns the specified type of service object the caller.
		/// </summary>
		/// <param name="svcType">Type of service requested</param>
		/// <returns>
		/// The object matching the service type requested or null if the
		/// service is not supported.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method is similar to COM's IUnknown::QueryInterface method,
		/// although more generic. Instead of just returning interfaces,
		/// this method can return any type of object.
		/// </para>
		/// <para>
		/// The following services are supported:
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IBounds2DF"/>
		/// </para>
		/// </remarks>
		object IServiceProvider.GetService(System.Type svcType)
		{
			if (svcType == typeof(IBounds2DF))
			{
				return (IBounds2DF) this;
			}
			return null;
		}

		#endregion

		#region ICompositeNode interface

		/// <summary>
		/// Reference to the composite node this node is a child of.
		/// </summary>
		public ICompositeNode Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		/// <summary>
		/// The root node in the node hierarchy.
		/// </summary>
		/// <remarks>
		/// The root node is found by following the chain of parent nodes until
		/// a node is found that has a null parent.
		/// </remarks>
		public INode Root
		{
			get
			{
				if (this.parent == null)
				{
					return this;
				}
				return this.parent.Root;
			}
		}

		/// <summary>
		/// Name of the group.
		/// </summary>
		/// <remarks>
		/// Must be unique within the scope of the parent node.
		/// </remarks>
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
		/// Fully qualified name of the node.
		/// </summary>
		/// <remarks>
		/// The full name is the name of the node concatenated with the names
		/// of all parent nodes.
		/// </remarks>
		public string FullName
		{
			get
			{
				if (this.parent == null)
				{
					return this.name;
				}
				return this.parent.FullName + "." + this.name;
			}
		}

		/// <summary>
		/// The number of child nodes contained by the group.
		/// </summary>
		public int ChildCount
		{
			get
			{
				return this.children.Count;
			}
		}

		/// <summary>
		/// Returns the child node at the given index position.
		/// </summary>
		/// <param name="childIndex">Zero-based index into the collection of child nodes</param>
		/// <returns>Child node at the given position or null if the index is out of range</returns>
		public INode GetChild(int childIndex)
		{
			return (INode) this.children[childIndex];
		}

		/// <summary>
		/// Returns the child node matching the given name.
		/// </summary>
		/// <param name="childName">Name of node to return</param>
		/// <returns>Node matching the given name</returns>
		public INode GetChildByName(string childName)
		{
			INode nodeFound = null;
			IEnumerator enumChildren = this.children.GetEnumerator();
			while (nodeFound == null && enumChildren.MoveNext())
			{
				INode curNode = enumChildren.Current as INode;
				if (curNode != null)
				{
					if (curNode.Name == childName)
					{
						nodeFound = curNode;
					}
				}
			}

			return nodeFound;
		}

		/// <summary>
		/// Returns the index position of the given child node.
		/// </summary>
		/// <param name="child">Child node to query</param>
		/// <returns>Zero-based index into the collection of child nodes</returns>
		public int GetChildIndex(INode child)
		{
			return this.children.Find(child);
		}

		/// <summary>
		/// Appends the given node to the collection of child nodes.
		/// </summary>
		/// <param name="child">Node to append</param>
		/// <returns>
		/// Zero-based index at which the node was added to the collection or -1 for failure.
		/// </returns>
		public int AppendChild(INode child)
		{
			int childIndex = this.children.Add(child);
			child.Parent = this;
			return childIndex;
		}

		/// <summary>
		/// Insert the given node into the collection of child nodes at a
		/// specific position.
		/// </summary>
		/// <param name="child">Node to insert</param>
		/// <param name="childIndex">Zero-based index at which to insert the node</param>
		public void InsertChild(INode child, int childIndex)
		{
			this.children.Insert(childIndex, child);
			child.Parent = this;
		}

		/// <summary>
		/// Removes the child node at the given position.
		/// </summary>
		/// <param name="childIndex">Zero-based index into the collection of child nodes</param>
		public void RemoveChild(int childIndex)
		{
			INode child = this.GetChild(childIndex);
			if (child != null)
			{
				child.Parent = null;
				this.children.RemoveAt(childIndex);
			}
		}

		/// <summary>
		/// Removes all child nodes from the node.
		/// </summary>
		public void RemoveAllChildren()
		{
			IEnumerator enumChildren = this.children.GetEnumerator();
			while (enumChildren.MoveNext())
			{
				INode curChild = enumChildren.Current as INode;
				if (curChild != null)
				{
					curChild.Parent = null;
				}
			}
			this.children.Clear();
		}

		/// <summary>
		/// Returns the region that the bounds of the given child node is constrained by.
		/// </summary>
		/// <param name="child">Child to get constraining region for</param>
		/// <returns>Region that constrains the bounds of the given child</returns>
		/// <remarks>
		/// <para>
		/// This method is used to limit the bounds of a child node to a specified area.
		/// The node cannot be moved, resized, or rotated beyond the edges of this region.
		/// </para>
		/// </remarks>
		public System.Drawing.Region GetConstrainingRegion(INode child)
		{
			if (this.parent != null)
			{
				return this.parent.GetConstrainingRegion(child);
			}
			return null;
		}

		/// <summary>
		/// Returns all children that are intersected by the given point.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children hit by the given point
		/// </param>
		/// <param name="ptWorld">Point to test</param>
		/// <returns>The number of child nodes that intersect the given point</returns>
		public int GetChildrenAtPoint(NodeCollection childNodes, PointF ptWorld)
		{
			int numFound = 0;

			Global.MatrixStack.Push(this.matrix);

			foreach (INode curChild in this.children)
			{
				if (curChild != null)
				{
					IHitTestRegion rgnHitTest = curChild as IHitTestRegion;
					if (rgnHitTest != null)
					{
						if (rgnHitTest.ContainsPoint(ptWorld, 0))
						{
							if (childNodes != null)
							{
								childNodes.Insert(0, curChild);
							}
							numFound++;
						}
					}
					else
					{
						IHitTestBounds boundsHitTest = curChild as IHitTestBounds;
						if (boundsHitTest != null)
						{
							if (boundsHitTest.ContainsPoint(ptWorld, 0))
							{
								if (childNodes != null)
								{
									childNodes.Insert(0, curChild);
								}
								numFound++;
							}
						}
					}
				}
			}

			Global.MatrixStack.Pop();

			return numFound;
		}

		/// <summary>
		/// Returns all children that intersect the given rectangle.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children hit by the given point
		/// </param>
		/// <param name="rcWorld">Rectangle to test</param>
		/// <returns>The number of child nodes that intersect the given rectangle</returns>
		public int GetChildrenIntersecting(NodeCollection childNodes, RectangleF rcWorld)
		{
			int numFound = 0;

			Global.MatrixStack.Push(this.matrix);

			foreach (INode curNode in this.children)
			{
				IHitTestBounds hitTestObj = curNode as IHitTestBounds;
				if (hitTestObj != null)
				{
					if (hitTestObj.IntersectsRect(rcWorld))
					{
						childNodes.Insert(0, curNode);
						numFound++;
					}
				}
			}

			Global.MatrixStack.Pop();

			return numFound;
		}

		/// <summary>
		/// Returns all children inside the given rectangle.
		/// </summary>
		/// <param name="childNodes">
		/// Collection in which to add the children inside the specified rectangle
		/// </param>
		/// <param name="rcWorld">Rectangle to test</param>
		/// <returns>The number of child nodes added to the collection</returns>
		public int GetChildrenContainedBy(NodeCollection childNodes, RectangleF rcWorld)
		{
			int numFound = 0;

			Global.MatrixStack.Push(this.matrix);

			foreach (INode curNode in this.children)
			{
				IHitTestBounds hitTestObj = curNode as IHitTestBounds;
				if (hitTestObj != null)
				{
					if (hitTestObj.ContainedByRect(rcWorld))
					{
						childNodes.Add(curNode);
						numFound++;
					}
				}
			}
			
			Global.MatrixStack.Pop();

			return numFound;
		}

		/// <summary>
		/// Returns the inherited property container for the given child node.
		/// </summary>
		/// <param name="childNode">The child node making the request</param>
		/// <returns>Parent property container for the given node</returns>
		public virtual IPropertyContainer GetPropertyContainer(INode childNode)
		{
			return (IPropertyContainer) this;
		}

		#endregion

		#region IBounds2DF interface

		/// <summary>
		/// The group's bounding box.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Always returns the bounds of the group in world coordinates, regardless
		/// of what is on the matrix stack at the time of the call.
		/// </para>
		/// <para>
		/// The bounding box of a group is the union of the bounds of all of its
		/// children. This method pushes the group's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.WorldTransform"/>
		/// onto the matrix stack and then iterates through each child retrieving
		/// it's local bounds through the ILocalBounds2DF.
		/// </para>
		/// </remarks>
		public RectangleF Bounds
		{
			get
			{
				float left = float.MaxValue;
				float top = float.MaxValue;
				float right = float.MinValue;
				float bottom = float.MinValue;
				RectangleF curBounds;

				Global.MatrixStack.Clear();
				Global.MatrixStack.Push(this.WorldTransform, MatrixOrder.Prepend);

				foreach (INode curChild in this.children)
				{
					ILocalBounds2DF childBounds = curChild as ILocalBounds2DF;

					if (childBounds != null)
					{
						curBounds = childBounds.Bounds;

						if (curBounds.Left < left)
						{
							left = curBounds.Left;
						}
						if (curBounds.Right > right)
						{
							right = curBounds.Right;
						}
						if (curBounds.Top < top)
						{
							top = curBounds.Top;
						}
						if (curBounds.Bottom > bottom)
						{
							bottom = curBounds.Bottom;
						}
					}
				}

				Global.MatrixStack.Pop();

				return new RectangleF(left, top, right - left, bottom - top);
			}
			set
			{
				// Get the origin relative to the parent
				RectangleF oldBounds = this.Bounds;
				PointF curOrigin = Geometry.CenterPoint(oldBounds);
				// Get the new origin
				PointF newOrigin = Geometry.CenterPoint(value);
				// Calculate translation vector
				float offsetX = newOrigin.X - curOrigin.X;
				float offsetY = newOrigin.Y - curOrigin.Y;
				// Calculate scale
				float scaleX = value.Width / oldBounds.Width;
				float scaleY = value.Height / oldBounds.Height;
				// Apply transformations
				this.matrix.Translate(-curOrigin.X, -curOrigin.Y, MatrixOrder.Append);
				this.matrix.Scale(scaleX, scaleY, MatrixOrder.Append);
				this.matrix.Translate(curOrigin.X, curOrigin.Y, MatrixOrder.Append);
				this.matrix.Translate(offsetX, offsetY, MatrixOrder.Append);

				this.OnBoundsChanged(new BoundsEventArgs(this, oldBounds, this.Bounds));
			}
		}

		/// <summary>
		/// X-coordinate of the object's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		public virtual float X
		{
			get
			{
				return this.Bounds.Location.X;
			}
			set
			{
				float curX = this.Bounds.Location.X;
				float dx = value - curX;
				this.Translate(dx, 0);
			}
		}

		/// <summary>
		/// Y-coordinate of the object's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		public virtual float Y
		{
			get
			{
				return this.Bounds.Location.Y;
			}
			set
			{
				float curY = this.Bounds.Location.Y;
				float dy = value - curY;
				this.Translate(0, dy);
			}
		}

		/// <summary>
		/// Width of the object.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		public virtual float Width
		{
			get
			{
				return this.Bounds.Size.Width;
			}
			set
			{
				float curWidth = this.Bounds.Size.Width;
				float sx = value / curWidth;
				this.Scale(sx, 1.0f);
			}
		}

		/// <summary>
		/// Height of the object.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		public virtual float Height
		{
			get
			{
				return this.Bounds.Size.Height;
			}
			set
			{
				float curHeight = this.Bounds.Size.Height;
				float sy = value / curHeight;
				this.Scale(1.0f, sy);
			}
		}

		#endregion

		#region ILocalBounds2DF interface

		/// <summary>
		/// Bounding box of the group in local coordinates.
		/// </summary>
		/// <remarks>
		/// The value returned depends on the contents of the matrix stack. If the
		/// matrix stack is empty, then the value returned is in local coordinates.
		/// This method is generally used by functions that recursively traverse
		/// the node hierarchy, pushing and popping the matrix stack as they go.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Global.MatrixStack"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Group.Bounds"/>
		/// </remarks>
		RectangleF ILocalBounds2DF.Bounds
		{
			get
			{
				float left = float.MaxValue;
				float top = float.MaxValue;
				float right = float.MinValue;
				float bottom = float.MinValue;
				RectangleF curBounds;

				Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);

				foreach (INode curChild in this.children)
				{
					if (curChild.GetType() != typeof(Label) && !curChild.GetType().IsSubclassOf(typeof(Port)))
					{
						ILocalBounds2DF childBounds = curChild as ILocalBounds2DF;

						if (childBounds != null)
						{
							curBounds = childBounds.Bounds;

							if (curBounds.Left < left)
							{
								left = curBounds.Left;
							}
							if (curBounds.Right > right)
							{
								right = curBounds.Right;
							}
							if (curBounds.Top < top)
							{
								top = curBounds.Top;
							}
							if (curBounds.Bottom > bottom)
							{
								bottom = curBounds.Bottom;
							}
						}
					}
				}

				Global.MatrixStack.Pop();

				return new RectangleF(left, top, right - left, bottom - top);
			}
			set
			{
				throw new EInvalidOperation();
			}
		}

		/// <summary>
		/// X-coordinate of the object's location.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.X
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Left;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Location = new PointF(value, rcBounds.Top);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Y-coordinate of the object's location.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.Y
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Top;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Location = new PointF(rcBounds.Left, value);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Width of the object.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.Width
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Width;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Size = new SizeF(value, rcBounds.Height);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Height of the object.
		/// </summary>
		/// <remarks>
		/// Specified in local coordinates.
		/// </remarks>
		float ILocalBounds2DF.Height
		{
			get
			{
				return ((ILocalBounds2DF)this).Bounds.Height;
			}
			set
			{
				RectangleF rcBounds = ((ILocalBounds2DF)this).Bounds;
				rcBounds.Size = new SizeF(rcBounds.Width, value);
				((ILocalBounds2DF)this).Bounds = rcBounds;
			}
		}

		#endregion

		#region IGraphics interface

		/// <summary>
		/// Renders the group onto a System.Drawing.Graphics object.
		/// </summary>
		/// <param name="grfx">Graphics context to render onto</param>
		/// <remarks>
		/// Iterates through all child nodes and renders them.
		/// </remarks>
		public void Draw(System.Drawing.Graphics grfx)
		{
			grfx.Transform = Global.MatrixStack.Push(this.matrix);

			IEnumerator enumChildren = this.children.GetEnumerator();
			while (enumChildren.MoveNext())
			{
				IDraw drawObj = enumChildren.Current as IDraw;
				if (drawObj != null)
				{
					drawObj.Draw(grfx);
				}
			}

			grfx.Transform = Global.MatrixStack.Pop();
		}

		/// <summary>
		/// Encapsulates the points and instructions needed to render the group.
		/// </summary>
		/// <remarks>
		/// The contents of the GraphicsPath is the union of the GraphicsPath
		/// objects of all its children.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Group.CreateRegion"/>
		/// </remarks>
		[Browsable(false)]
		public virtual System.Drawing.Drawing2D.GraphicsPath GraphicsPath
		{
			get
			{
				GraphicsPath grfxPath = new GraphicsPath();

				Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);

				foreach (INode curChild in this.children)
				{
					IGraphics graphicsObj = curChild as IGraphics;
					if (graphicsObj != null)
					{
						GraphicsPath grfxChildPath = graphicsObj.GraphicsPath;
						if (grfxChildPath != null)
						{
							grfxPath.AddPath(grfxChildPath, false);
						}
					}
				}

				Global.MatrixStack.Pop();

				return grfxPath;
			}
		}

		/// <summary>
		/// Returns an object that describes the interior of the shape.
		/// </summary>
		/// <param name="padding">Amount of padding to add</param>
		/// <returns>System.Drawing.Region object</returns>
		/// <remarks>
		/// Region objects are used for hit testing and geometrical calculations.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Group.GraphicsPath"/>
		/// </remarks>
		public virtual System.Drawing.Region CreateRegion(float padding)
		{
			GraphicsPath grfxPath = this.GraphicsPath;

			if (grfxPath != null)
			{
				grfxPath.Transform(this.matrix);
				return new System.Drawing.Region(grfxPath);
			}

			return new System.Drawing.Region(this.Bounds);
		}

		#endregion

		#region ITransform interface

		/// <summary>
		/// Moves the group by the given X and Y offsets.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		/// <remarks>
		/// Applies a translate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnMove"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Translate(float dx, float dy)
		{
			if (dx != 0.0f || dy != 0.0f)
			{
				this.matrix.Translate(dx, dy, MatrixOrder.Append);
				this.OnMove(new MoveEventArgs(this, dx, dy));
			}
		}

		/// <summary>
		/// Rotates the group a specified number of degrees about a given
		/// anchor point.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to rotate</param>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnRotate"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Rotate(PointF ptAnchor, float degrees)
		{
			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Rotate(degrees, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);
			this.OnRotate(new RotateEventArgs(this));
		}

		/// <summary>
		/// Rotates the group a specified number of degrees about its center point.
		/// </summary>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnRotate"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Rotate(float degrees)
		{
			PointF ptOrigin = Geometry.CenterPoint(this.Bounds);
			this.matrix.Translate(-ptOrigin.X, -ptOrigin.Y, MatrixOrder.Append);
			this.matrix.Rotate(degrees, MatrixOrder.Append);
			this.matrix.Translate(ptOrigin.X, ptOrigin.Y, MatrixOrder.Append);
			this.OnRotate(new RotateEventArgs(this));
		}

		/// <summary>
		/// Scales the group by a given ratio along the X and Y axes.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to scale</param>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnScale"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Scale(PointF ptAnchor, float sx, float sy)
		{
			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Scale(sx, sy, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);
			this.OnScale(new ScaleEventArgs(this));
		}

		/// <summary>
		/// Scales the group about its center point by a given ratio.
		/// </summary>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnScale"/>
		/// method is called after the change is made.
		/// </remarks>
		public void Scale(float sx, float sy)
		{
			PointF ptAnchor = Geometry.CenterPoint(this.Bounds);
			this.matrix.Translate(-ptAnchor.X, -ptAnchor.Y, MatrixOrder.Append);
			this.matrix.Scale(sx, sy, MatrixOrder.Append);
			this.matrix.Translate(ptAnchor.X, ptAnchor.Y, MatrixOrder.Append);
			this.OnScale(new ScaleEventArgs(this));
		}

		/// <summary>
		/// Matrix containing local transformations for this node.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Matrix LocalTransform
		{
			get
			{
				return this.matrix;
			}
		}

		/// <summary>
		/// Returns a matrix containing transformations for this node and all of
		/// its ancestors.
		/// </summary>
		/// <remarks>
		/// Chains up the node hierarchy and builds a transformation matrix containing
		/// all transformations that apply to this node in the world coordinate space.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Matrix WorldTransform
		{
			get
			{
				Matrix worldTransform = new Matrix();

				if (this.parent != null)
				{
					ITransform objTransformParent = this.parent as ITransform;
					if (objTransformParent != null)
					{
						worldTransform.Multiply(objTransformParent.WorldTransform, MatrixOrder.Append);
					}
				}

				worldTransform.Multiply(this.matrix, MatrixOrder.Append);

				return worldTransform;
			}
		}

		/// <summary>
		/// Returns a matrix containing the transformations of this node's parent.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Matrix ParentTransform
		{
			get
			{
				Matrix parentTransform = null;

				if (this.parent != null)
				{
					ITransform objTransformParent = this.parent as ITransform;
					if (objTransformParent != null)
					{
						parentTransform = objTransformParent.WorldTransform;
					}
				}

				if (parentTransform == null)
				{
					parentTransform = new Matrix();
				}

				return parentTransform;
			}
		}

		#endregion

		#region ILogicalUnitContainer interface

		/// <summary>
		/// Converts the logical values contained by the object from one unit of
		/// measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="grfx">Graphics context object for converting device units</param>
		/// <remarks>
		/// <para>
		/// This method converts all logical unit values contained by the object from
		/// one unit of measure to another.
		/// </para>
		/// </remarks>
		void ILogicalUnitContainer.ConvertLogicalUnits(GraphicsUnit fromUnits, GraphicsUnit toUnits, Graphics grfx)
		{
			// Convert line width
			if (this.propertyValues.Contains("LineWidth"))
			{
				float lineWidth = (float) this.propertyValues["LineWidth"];
				lineWidth = Measurements.Convert(fromUnits, toUnits, grfx, lineWidth);
				this.propertyValues["LineWidth"] = lineWidth;
			}

			// Iterate through children and convert them
			foreach (INode curChild in this.children)
			{
				ILogicalUnitContainer logUnitContainer = curChild as ILogicalUnitContainer;
				if (logUnitContainer != null)
				{
					logUnitContainer.ConvertLogicalUnits(fromUnits, toUnits, grfx);
				}
			}
		}

		/// <summary>
		/// Converts the logical values contained by the object from one scale to
		/// another.
		/// </summary>
		/// <param name="fromScale">Scale to convert from</param>
		/// <param name="toScale">Scale to convert to</param>
		/// <remarks>
		/// <para>
		/// This method scales all logical unit values contained by the object.
		/// </para>
		/// </remarks>
		void ILogicalUnitContainer.ConvertLogicalScale(float fromScale, float toScale)
		{
		}

		#endregion

		#region IHitTestBounds interface

		/// <summary>
		/// Tests to see if the object's bounding box contains the given point.
		/// </summary>
		/// <param name="ptTest">Point to test</param>
		/// <param name="fSlop">Expands the area to be tested</param>
		/// <returns>true if the object contains the given point, otherwise false</returns>
		bool IHitTestBounds.ContainsPoint(PointF ptTest, float fSlop)
		{
			bool hit = false;

			Point pt = new Point((int) ptTest.X, (int) ptTest.Y);
			System.Drawing.RectangleF bounds = this.Bounds;
			hit = bounds.Contains(pt);

			return hit;
		}

		/// <summary>
		/// Tests to see if the object's bounding box intersects the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if an intersection occurs, otherwise false</returns>
		bool IHitTestBounds.IntersectsRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return bounds.IntersectsWith(rcTest);
		}

		/// <summary>
		/// Tests to see if the object's bounding box contains the given rectangle.
		/// </summary>
		/// <param name="rcTest">Rectangle to test</param>
		/// <returns>true if the rectangle is contained by the object, otherwise false</returns>
		bool IHitTestBounds.ContainedByRect(RectangleF rcTest)
		{
			RectangleF bounds = this.Bounds;
			return rcTest.Contains(bounds);
		}

		#endregion

		#region IPropertyContainer interface

		/// <summary>
		/// Sets the default property values for the group.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// group to their default values.
		/// </remarks>
		public virtual void SetDefaultPropertyValues()
		{
		}

		/// <summary>
		/// Retrieve the value of a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to retrieve</param>
		/// <returns>Value of the named property or null if it doesn't exist</returns>
		public virtual object GetPropertyValue(string propertyName)
		{
			if (this.propertyValues.Contains(propertyName))
			{
				return this.propertyValues[propertyName];
			}

			IPropertyContainer parentProps = this.parent as IPropertyContainer;
			if (parentProps != null)
			{
				return parentProps.GetPropertyValue(propertyName);
			}

			return null;
		}

		/// <summary>
		/// Assign a value to a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to set</param>
		/// <param name="val">Value to assign to property</param>
		/// <remarks>
		/// This method will add the property to the container if it doesn't
		/// already exist.
		/// </remarks>
		public virtual void SetPropertyValue(string propertyName, object val)
		{
			object oldVal = null;

			if (this.propertyValues.ContainsKey(propertyName))
			{
				oldVal = this.propertyValues[propertyName];
				this.propertyValues[propertyName] = val;
			}
			else
			{
				this.propertyValues.Add(propertyName, val);
			}

			this.OnPropertyChanged(new PropertyEventArgs(this, propertyName, oldVal, val));
		}

		/// <summary>
		/// Assign a value to a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to change</param>
		/// <param name="val">Value to assign to property</param>
		/// <remarks>
		/// This method only modifies property values that already exist
		/// in the container. If the property does not exist, this method fails.
		/// </remarks>
		public virtual void ChangePropertyValue(string propertyName, object val)
		{
			object oldVal = null;

			if (this.propertyValues.ContainsKey(propertyName))
			{
				oldVal = this.propertyValues[propertyName];
				this.propertyValues[propertyName] = val;
				this.OnPropertyChanged(new PropertyEventArgs(this, propertyName, oldVal, val));
			}
		}

		/// <summary>
		/// Removes the specified property.
		/// </summary>
		/// <param name="propertyName">Name of property to remove</param>
		public virtual void RemoveProperty(string propertyName)
		{
			if (this.propertyValues.ContainsKey(propertyName))
			{
				this.propertyValues.Remove(propertyName);
			}
		}

		/// <summary>
		/// Returns an array containing the names of all properties in the container.
		/// </summary>
		/// <returns>String array containing property names</returns>
		public virtual string[] GetPropertyNames()
		{
			string[] propertyNames = new string[this.propertyValues.Keys.Count];
			this.propertyValues.Keys.CopyTo(propertyNames, 0);
			return propertyNames;
		}

		#endregion

		#region IDispatchNodeEvents interface

		/// <summary>
		/// Called when a property value is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnPropertyChanged"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.PropertyChanged(PropertyEventArgs evtArgs)
		{
			this.OnPropertyChanged(evtArgs);
		}

		/// <summary>
		/// Called before the collection of child nodes is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnChildrenChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ChildrenChanging(NodeCollection.EventArgs evtArgs)
		{
			this.OnChildrenChanging(evtArgs);
		}

		/// <summary>
		/// Called after the collection of child nodes is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnChildrenChangeComplete"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ChildrenChangeComplete(NodeCollection.EventArgs evtArgs)
		{
			this.OnChildrenChangeComplete(evtArgs);
		}

		/// <summary>
		/// Called when the bounds of a node changes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnBoundsChanged"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoundsEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.BoundsChanged(BoundsEventArgs evtArgs)
		{
			this.OnBoundsChanged(evtArgs);
		}

		/// <summary>
		/// Called when a node is moved.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnMove"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.Move(MoveEventArgs evtArgs)
		{
			this.OnMove(evtArgs);
		}

		/// <summary>
		/// Called when a node is rotated.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnRotate"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RotateEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.Rotate(RotateEventArgs evtArgs)
		{
			this.OnRotate(evtArgs);
		}

		/// <summary>
		/// Called when a node is scaled.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnScale"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ScaleEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.Scale(ScaleEventArgs evtArgs)
		{
			this.OnScale(evtArgs);
		}

		/// <summary>
		/// Called when a node is clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnClick"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.Click(NodeMouseEventArgs evtArgs)
		{
			this.OnClick(evtArgs);
		}

		/// <summary>
		/// Called when a node is double clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnDoubleClick"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.DoubleClick(NodeMouseEventArgs evtArgs)
		{
			this.OnDoubleClick(evtArgs);
		}

		/// <summary>
		/// Called when the mouse enters a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnMouseEnter"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.MouseEnter(NodeMouseEventArgs evtArgs)
		{
			this.OnMouseEnter(evtArgs);
		}

		/// <summary>
		/// Called when the mouse leaves a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnMouseLeave"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.MouseLeave(NodeMouseEventArgs evtArgs)
		{
			this.OnMouseLeave(evtArgs);
		}

		/// <summary>
		/// Called when a vertex is inserted into a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnInsertVertex"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.InsertVertex(VertexEventArgs evtArgs)
		{
			this.OnInsertVertex(evtArgs);
		}

		/// <summary>
		/// Called when a vertex is deleted from a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnDeleteVertex"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.DeleteVertex(VertexEventArgs evtArgs)
		{
			this.OnDeleteVertex(evtArgs);
		}

		/// <summary>
		/// Called when a vertex is moved.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.Group.OnMoveVertex"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.MoveVertex(VertexEventArgs evtArgs)
		{
			this.OnMoveVertex(evtArgs);
		}

		/// <summary>
		/// Called before the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnConnectionsChanging"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ConnectionsChanging(ConnectionCollection.EventArgs evtArgs)
		{
			this.OnConnectionsChanging(evtArgs);
		}

		/// <summary>
		/// Called after the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// Calls <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.OnConnectionsChangeComplete"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs)
		{
			this.OnConnectionsChangeComplete(evtArgs);
		}

		#endregion

		#region Node Event Callbacks

		/// <summary>
		/// Called before a change is made to the collection of child nodes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.ChildrenChanging"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnChildrenChanging(NodeCollection.EventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.ChildrenChanging(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called after a change is made to the collection of child nodes.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.ChildrenChangeComplete"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnChildrenChangeComplete(NodeCollection.EventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.ChildrenChangeComplete(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a property is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.Parent"/>
		/// of the property change by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.PropertyChanged"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.PropertyEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnPropertyChanged(PropertyEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.PropertyChanged(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the position of the node is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.Parent"/>
		/// of the move by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Move"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.MoveEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMove(MoveEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Move(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the node is rotated.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.Parent"/>
		/// of the rotation by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Rotate"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.RotateEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnRotate(RotateEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Rotate(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the node is scaled.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.Parent"/>
		/// of the scaling by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Scale"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ScaleEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnScale(ScaleEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Scale(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the bounds of the node change.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Group.Parent"/>
		/// of the change in bounds by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.BoundsChanged"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.BoundsEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnBoundsChanged(BoundsEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.BoundsChanged(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a node is clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.Click"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnClick(NodeMouseEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.Click(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a node is double clicked.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.DoubleClick"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnDoubleClick(NodeMouseEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.DoubleClick(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the mouse enters a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.MouseEnter"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMouseEnter(NodeMouseEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.MouseEnter(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when the mouse leaves a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.MouseLeave"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMouseLeave(NodeMouseEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.MouseLeave(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a vertex is inserted.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.InsertVertex"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnInsertVertex(VertexEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.InsertVertex(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a vertex is moved.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.MoveVertex"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnMoveVertex(VertexEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.MoveVertex(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called when a vertex is deleted.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.DeleteVertex"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnDeleteVertex(VertexEventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.DeleteVertex(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called before the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.ConnectionsChanging"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnConnectionsChanging(ConnectionCollection.EventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.ConnectionsChanging(evtArgs);
				}
			}
		}

		/// <summary>
		/// Called after the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <para>
		/// Forwards the event notification to the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.ConnectionsChangeComplete"/>
		/// method.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ConnectionCollection.EventArgs"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents"/>
		/// </remarks>
		protected virtual void OnConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs)
		{
			if (this.parent != null)
			{
				IDispatchNodeEvents evtDispatcher = this.parent.GetService(typeof(IDispatchNodeEvents)) as IDispatchNodeEvents;
				if (evtDispatcher != null)
				{
					evtDispatcher.ConnectionsChangeComplete(evtArgs);
				}
			}
		}

		#endregion

		#region Collection Event Handlers

		private void Children_Changing(object sender, NodeCollection.EventArgs evtArgs)
		{
			this.OnChildrenChanging(evtArgs);
		}

		private void Children_ChangeComplete(object sender, NodeCollection.EventArgs evtArgs)
		{
			this.OnChildrenChangeComplete(evtArgs);
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("name", this.name);
			info.AddValue("propertyValues", this.propertyValues);
			info.AddValue("parent", this.parent);
			info.AddValue("m11", this.matrix.Elements[0]);
			info.AddValue("m12", this.matrix.Elements[1]);
			info.AddValue("m21", this.matrix.Elements[2]);
			info.AddValue("m22", this.matrix.Elements[3]);
			info.AddValue("dx", this.matrix.Elements[4]);
			info.AddValue("dy", this.matrix.Elements[5]);
			info.AddValue("children", this.children);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.children.Changing += new NodeCollection.EventHandler(Children_Changing);
			this.children.ChangeComplete += new NodeCollection.EventHandler(Children_ChangeComplete);
		}

		#endregion

		#region Fields

		/// <summary>
		/// Name of the group.
		/// </summary>
		private string name;

		/// <summary>
		/// Reference to the parent node.
		/// </summary>
		private ICompositeNode parent;

		/// <summary>
		/// Collection of child nodes.
		/// </summary>
		protected NodeCollection children;

		/// <summary>
		/// Local transformation matrix.
		/// </summary>
		protected Matrix matrix;

		/// <summary>
		/// Hashtable containing property values belonging to the group.
		/// </summary>
		protected Hashtable propertyValues = null;

		#endregion
	}
}
