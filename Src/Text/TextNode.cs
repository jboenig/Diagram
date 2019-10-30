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
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Implements a node containing text.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a basic text node.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextBase"/>
	/// </remarks>
	[Serializable]
	public class TextNode : TextBase, ILocalBounds2DF, ILogicalUnitContainer, ISerializable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public TextNode()
		{
			this.text = "";
			this.bounds = new RectangleF(0, 0, 0, 0);
		}

		/// <summary>
		/// Construct a text node with a given text value.
		/// </summary>
		/// <param name="txtval">Value to assign to the Text property</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextNode.Text"/>
		/// </remarks>
		public TextNode(string txtval)
		{
			this.text = txtval;
			this.bounds = new RectangleF(0, 0, 0, 0);
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy from</param>
		public TextNode(TextNode src) : base(src)
		{
			this.text = src.text;
			this.bounds = new RectangleF(src.bounds.Location, src.bounds.Size);
		}

		/// <summary>
		/// Serialization constructor for text nodes.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected TextNode(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.text = info.GetString("text");
			this.bounds = (RectangleF) info.GetValue("bounds", typeof(RectangleF));
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new TextNode(this);
		}

		/// <summary>
		/// The text object's bounding box.
		/// </summary>
		/// <remarks>
		/// This property returns the bounds of the text object in world coordinates.
		/// </remarks>
		public override RectangleF Bounds
		{
			get
			{
				PointF[] rcPts = { new PointF(this.bounds.Left, this.bounds.Top), new PointF(this.bounds.Right, this.bounds.Bottom) };
				Matrix matrix = this.WorldTransform;
				matrix.TransformPoints(rcPts);
				return RectangleF.FromLTRB(rcPts[0].X, rcPts[0].Y, rcPts[1].X, rcPts[1].Y);
			}
			set
			{
				RectangleF oldBounds = this.Bounds;
				PointF[] rcPts = { new PointF(value.Left, value.Top), new PointF(value.Right, value.Bottom) };
				Matrix matrix = this.WorldTransform;
				matrix.Invert();
				matrix.TransformPoints(rcPts);
				this.bounds = RectangleF.FromLTRB(rcPts[0].X, rcPts[0].Y, rcPts[1].X, rcPts[1].Y);
				this.OnBoundsChanged(new BoundsEventArgs(this, oldBounds, this.Bounds));
			}
		}

		/// <summary>
		/// Bounding box of the text node in local coordinates.
		/// </summary>
		/// <remarks>
		/// The value returned depends on the contents of the matrix stack. If the
		/// matrix stack is empty, then the value returned is in local coordinates.
		/// This method is generally used by functions that recursively traverse
		/// the node hierarchy, pushing and popping the matrix stack as they go.
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Global.MatrixStack"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextNode.Bounds"/>
		/// </remarks>
		RectangleF ILocalBounds2DF.Bounds
		{
			get
			{
				PointF[] rcPts = { new PointF(this.bounds.Left, this.bounds.Top), new PointF(this.bounds.Right, this.bounds.Bottom) };
				Matrix matrix = Global.MatrixStack.Push(this.matrix);
				matrix.TransformPoints(rcPts);
				Global.MatrixStack.Pop();
				return RectangleF.FromLTRB(rcPts[0].X, rcPts[0].Y, rcPts[1].X, rcPts[1].Y);
			}
			set
			{
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

		/// <summary>
		/// Returns the bounding box of the text object in local coordinates.
		/// </summary>
		/// <returns>Bounding box for text</returns>
		/// <remarks>
		/// <para>
		/// This method returns the raw, untransformed bounding box for the
		/// text in local coordinates. No matrix transformations are applied
		/// by this method.
		/// </para>
		/// </remarks>
		public override RectangleF GetTextBox()
		{
			return this.bounds;
		}

		/// <summary>
		/// The value contained by the text object.
		/// </summary>
		public override string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (this.text != value)
				{
					string oldVal = this.text;
					this.text = value;
					this.OnPropertyChanged(new PropertyEventArgs(this, "Text", oldVal, this.text));
				}
			}
		}

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

			this.bounds = Measurements.Convert(fromUnits, toUnits, grfx, this.bounds);
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

		/// <summary>
		/// Populates a SerializationInfo with the data needed to
		/// serialize the target object.
		/// </summary>
		/// <param name="info">SerializationInfo object to populate</param>
		/// <param name="context">Destination streaming context</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("name", this.Name);
			info.AddValue("parent", this.Parent);
			info.AddValue("m11", this.matrix.Elements[0]);
			info.AddValue("m12", this.matrix.Elements[1]);
			info.AddValue("m21", this.matrix.Elements[2]);
			info.AddValue("m22", this.matrix.Elements[3]);
			info.AddValue("dx", this.matrix.Elements[4]);
			info.AddValue("dy", this.matrix.Elements[5]);
			info.AddValue("propertyValues", this.propertyValues);
			info.AddValue("bounds", this.Bounds);
			info.AddValue("text", this.text);
		}

		/// <summary>
		/// Text value of the node
		/// </summary>
		private string text;

		/// <summary>
		/// Text bounding box
		/// </summary>
		private RectangleF bounds;
	}
}