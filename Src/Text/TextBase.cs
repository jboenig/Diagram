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
using System.ComponentModel;

using Syncfusion.Runtime.InteropServices.WinAPI;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Base class for text nodes and labels.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is an abstract base class that implements a text node.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextEdit"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextNode"/>
	/// </remarks>
	[Serializable]
	public abstract class TextBase : INode, IBounds2DF, IDraw, ITransform, IHitTestBounds, ILogicalUnitContainer, IPropertyContainer, ISerializable, IDeserializationCallback, IDispatchNodeEvents
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TextBase()
		{
			this.matrix = new Matrix();
			this.propertyValues = new Hashtable();
			SetDefaultPropertyValues();
			this.borderStyle = new BorderStyle(this);
			this.backgroundStyle = new BackgroundStyle(this);
			this.fillStyle = new FillStyle(this);
			this.editStyle = new EditStyle(this);
			this.fontStyle = new FontStyle(this);
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy from</param>
		public TextBase(TextBase src)
		{
			this.name = src.name;
			this.matrix = src.matrix.Clone();
			this.propertyValues = (Hashtable) src.propertyValues.Clone();
			this.borderStyle = new BorderStyle(this);
			this.backgroundStyle = new BackgroundStyle(this);
			this.fillStyle = new FillStyle(this);
			this.editStyle = new EditStyle(this);
			this.fontStyle = new FontStyle(this);
		}

		/// <summary>
		/// Serialization constructor for text objects.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected TextBase(SerializationInfo info, StreamingContext context)
		{
			this.name = info.GetString("name");
			this.parent = (ICompositeNode) info.GetValue("parent", typeof(ICompositeNode));
			float m11 = info.GetSingle("m11");
			float m12 = info.GetSingle("m12");
			float m21 = info.GetSingle("m21");
			float m22 = info.GetSingle("m22");
			float dx = info.GetSingle("dx");
			float dy = info.GetSingle("dy");
			this.matrix = new Matrix(m11, m12, m21, m22, dx, dy);
			this.propertyValues = (Hashtable) info.GetValue("propertyValues", typeof(Hashtable));
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public abstract object Clone();

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

		#region INode interface

		/// <summary>
		/// Reference to the composite node this node is a child of.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual ICompositeNode Parent
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
		/// The root node in the node hieararchy.
		/// </summary>
		/// <remarks>
		/// The root node is found by following the chain of parent nodes until
		/// a node is found that has a null parent.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
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
		/// 
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("General"),
		Description("Name of the text node")
		]
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
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
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

		#endregion

		#region IBounds2DF interface

		/// <summary>
		/// The text object's bounding box.
		/// </summary>
		/// <remarks>
		/// Always returns the bounds of the text object in world coordinates,
		/// regardless of what is on the matrix stack at the time of the call.
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public abstract RectangleF Bounds
		{
			get;
			set;
		}

		/// <summary>
		/// X-coordinate of the text object's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Bounds"),
		Description("X coordinate of the bounding box")
		]
		public virtual float X
		{
			get
			{
				return this.Bounds.Left;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Location = new PointF(value, rcBounds.Top);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Y-coordinate of the text object's location.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Bounds"),
		Description("Y coordinate of the bounding box")
		]
		public virtual float Y
		{
			get
			{
				return this.Bounds.Top;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Location = new PointF(rcBounds.Left, value);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Width of the text object.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Bounds"),
		Description("Width of the bounding box")
		]
		public virtual float Width
		{
			get
			{
				return this.Bounds.Width;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Size = new SizeF(value, rcBounds.Height);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Height of the text object.
		/// </summary>
		/// <remarks>
		/// Specified in world coordinates.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Bounds"),
		Description("Height of the bounding box")
		]
		public virtual float Height
		{
			get
			{
				return this.Bounds.Height;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Size = new SizeF(rcBounds.Width, value);
				this.Bounds = rcBounds;
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Determines the horizontal alignment of the text.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property is used by the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.GetStringFormat"/>
		/// method to generate a System.Drawing.StringFormat object. This property
		/// corresponds to the Alignment property in the System.Drawing.StringFormat
		/// class.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextBase.GetStringFormat"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Appearance"),
		Description("Horizontal alignment of the text")
		]
		public StringAlignment HorizontalAlignment
		{
			get
			{
				return (StringAlignment) this.GetPropertyValue("HorizontalAlignment");
			}
			set
			{
				this.SetPropertyValue("HorizontalAlignment", value);
			}
		}

		/// <summary>
		/// Determines the vertical alignment of the text.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property is used by the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.GetStringFormat"/>
		/// method to generate a System.Drawing.StringFormat object.  This property
		/// corresponds to the LineAlignment property in the System.Drawing.StringFormat
		/// class.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextBase.GetStringFormat"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Appearance"),
		Description("Vertical alignment of the text")
		]
		public StringAlignment VerticalAlignment
		{
			get
			{
				return (StringAlignment) this.GetPropertyValue("VerticalAlignment");
			}
			set
			{
				this.SetPropertyValue("VerticalAlignment", value);
			}
		}

		/// <summary>
		/// Indicates if text should be wrapped when it exceeds the width of
		/// the bounding box.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Formatting"),
		Description("Indicates if text should be wrapped when it exceeds the width the bounding box")
		]
		public bool WrapText
		{
			get
			{
				return (bool) this.GetPropertyValue("WrapText");
			}
			set
			{
				this.SetPropertyValue("WrapText", value);
			}
		}

		/// <summary>
		/// Specifies that text is right to left.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Formatting"),
		Description("Specifies that text is right to left")
		]
		public bool DirectionRightToLeft
		{
			get
			{
				return (bool) this.GetPropertyValue("DirectionRightToLeft");
			}
			set
			{
				this.SetPropertyValue("DirectionRightToLeft", value);
			}
		}

		/// <summary>
		/// Specifies that text is vertical.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Formatting"),
		Description("Specifies that text is vertical")
		]
		public bool DirectionVertical
		{
			get
			{
				return (bool) this.GetPropertyValue("DirectionVertical");
			}
			set
			{
				this.SetPropertyValue("DirectionVertical", value);
			}
		}

		/// <summary>
		/// Specifies that no part of any glyph overhangs the bounding rectangle.
		/// </summary>
		/// <remarks>
		/// <para>
		/// By default some glyphs overhang the rectangle slightly where necessary to
		/// appear at the edge visually. For example when an italic lowercase letter
		/// f in a font such as Garamond is aligned at the far left of a rectangle,
		/// the lower part of the f will reach slightly further left than the left
		/// edge of the rectangle. Setting this flag will ensure no painting outside
		/// the rectangle but will cause the aligned edges of adjacent lines of text
		/// to appear uneven.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Formatting"),
		Description("Specifies that no part of any glyph overhangs the bounding rectangle")
		]
		public bool FitBlackBox
		{
			get
			{
				return (bool) this.GetPropertyValue("FitBlackBox");
			}
			set
			{
				this.SetPropertyValue("FitBlackBox", value);
			}
		}

		/// <summary>
		/// Only entire lines are laid out in the formatting rectangle.
		/// </summary>
		/// <remarks>
		/// <para>
		/// By default layout continues until the end of the text, or until no
		/// more lines are visible as a result of clipping, whichever comes first.
		/// Note that the default settings allow the last line to be partially
		/// obscured by a formatting rectangle that is not a whole multiple of
		/// the line height. To ensure that only whole lines are seen, specify
		/// this value and be careful to provide a formatting rectangle at least
		/// as tall as the height of one line.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Formatting"),
		Description("Only entire lines are laid out in the formatting rectangle")
		]
		public bool LineLimit
		{
			get
			{
				return (bool) this.GetPropertyValue("LineLimit");
			}
			set
			{
				this.SetPropertyValue("LineLimit", value);
			}
		}

		/// <summary>
		/// Include space at the end of each line in calculations that measure
		/// the size of the text.
		/// </summary>
		/// <remarks>
		/// <para>
		/// By default the boundary rectangle returned by the MeasureString
		/// method excludes the space at the end of each line. Set this flag
		/// to include that space in measurement.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Formatting"),
		Description("Include space at the end of each line in calculations that measure the size of the text")
		]
		public bool MeasureTrailingSpaces
		{
			get
			{
				return (bool) this.GetPropertyValue("MeasureTrailingSpaces");
			}
			set
			{
				this.SetPropertyValue("MeasureTrailingSpaces", value);
			}
		}

		/// <summary>
		/// Overhanging parts of glyphs, and unwrapped text reaching outside the
		/// formatting rectangle are allowed to show.
		/// </summary>
		/// <remarks>
		/// <para>
		/// By default all text and glyph parts reaching outside the formatting
		/// rectangle are clipped.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Formatting"),
		Description("Overhanging parts of glyphs, and unwrapped text reaching outside the formatting rectangle are allowed to show")
		]
		public bool NoClip
		{
			get
			{
				return (bool) this.GetPropertyValue("NoClip");
			}
			set
			{
				this.SetPropertyValue("NoClip", value);
			}
		}

		/// <summary>
		/// Flags used to format the text.
		/// </summary>
		/// <remarks>
		/// <para>
		/// See System.Drawing.StringFormatFlags for more details.
		/// </para>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public StringFormatFlags FormatFlags
		{
			get
			{
				StringFormatFlags fmtFlags = 0;
				if (!this.WrapText)
				{
					fmtFlags = fmtFlags | StringFormatFlags.NoWrap;
				}
				if (this.DirectionRightToLeft)
				{
					fmtFlags = fmtFlags | StringFormatFlags.DirectionRightToLeft;
				}
				if (this.DirectionVertical)
				{
					fmtFlags = fmtFlags | StringFormatFlags.DirectionVertical;
				}
				if (this.FitBlackBox)
				{
					fmtFlags = fmtFlags | StringFormatFlags.FitBlackBox;
				}
				if (this.LineLimit)
				{
					fmtFlags = fmtFlags | StringFormatFlags.LineLimit;
				}
				if (this.MeasureTrailingSpaces)
				{
					fmtFlags = fmtFlags | StringFormatFlags.MeasureTrailingSpaces;
				}
				if (this.NoClip)
				{
					fmtFlags = fmtFlags | StringFormatFlags.NoClip;
				}
				return fmtFlags;
			}
		}

		/// <summary>
		/// The value contained by the text object.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Derived classes override this property in order to supply the
		/// text value in an implementation specific way.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("General"),
		Description("Text value to be displayed")
		]
		public abstract string Text
		{
			get;
			set;
		}

		/// <summary>
		/// Flag indicating if the text object is visible or not.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("General"),
		Description("Determines if the text object is visible or not")
		]
		public bool Visible
		{
			get
			{
				return (bool) this.GetPropertyValue("Visible");
			}
			set
			{
				this.SetPropertyValue("Visible", value);
			}
		}

		/// <summary>
		/// Flag indicating if the text object is read-only or not.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("General"),
		Description("Flag indicating if the text object is read-only or not")
		]
		public bool ReadOnly
		{
			get
			{
				return (bool) this.GetPropertyValue("ReadOnly");
			}
			set
			{
				this.SetPropertyValue("ReadOnly", value);
			}
		}

		/// <summary>
		/// Upper-left hand corner of the bounding box.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public PointF Location
		{
			get
			{
				return this.Bounds.Location;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Location = new PointF(value.X, value.Y);
				this.Bounds = rcBounds;
			}
		}

		/// <summary>
		/// Size of the bounding box.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual SizeF Size
		{
			get
			{
				return this.Bounds.Size;
			}
			set
			{
				RectangleF rcBounds = this.Bounds;
				rcBounds.Size = new SizeF(value.Width, value.Height);
				this.Bounds = rcBounds;
			}
		}

		#endregion

		#region Styles

		/// <summary>
		/// Properties for drawing the border.
		/// </summary>
		[
		Browsable(true),
		TypeConverter(typeof(BorderStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Appearance"),
		Description("Properties for drawing the border")
		]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
		}

		/// <summary>
		/// Properties for filling the background.
		/// </summary>
		[
		Browsable(true),
		TypeConverter(typeof(BackgroundStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Appearance"),
		Description("Properties for filling the background")
		]
		public BackgroundStyle BackgroundStyle
		{
			get
			{
				return this.backgroundStyle;
			}
		}

		/// <summary>
		/// Properties for filling the text.
		/// </summary>
		[
		Browsable(true),
		TypeConverter(typeof(FillStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Appearance"),
		Description("Properties for filling the text")
		]
		public FillStyle FillStyle
		{
			get
			{
				return this.fillStyle;
			}
		}

		/// <summary>
		/// Properties for determining how the text object can be edited.
		/// </summary>
		[
		Browsable(true),
		TypeConverter(typeof(EditStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Behavior"),
		Description("Properties for determining how the text object can be edited")
		]
		public EditStyle EditStyle
		{
			get
			{
				return this.editStyle;
			}
		}

		/// <summary>
		/// Determines the font used to draw the text.
		/// </summary>
		[
		Browsable(true),
		TypeConverter(typeof(FontStyleConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Appearance"),
		Description("Determines the font used to draw the text")
		]
		public FontStyle FontStyle
		{
			get
			{
				return this.fontStyle;
			}
		}

		#endregion

		#region Public Methods

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
		public abstract RectangleF GetTextBox();

		/// <summary>
		/// Creates a StringFormat object that encapsulates the properties of
		/// the text object.
		/// </summary>
		/// <returns>System.Drawing.StringFormat object</returns>
		/// <remarks>
		/// <para>
		/// The System.Drawing.StringFormat object returned by this method is
		/// used to draw the text using the System.Drawing.Graphics.DrawString
		/// method.
		/// </para>
		/// </remarks>
		public virtual StringFormat GetStringFormat()
		{
			StringFormat fmt = new StringFormat();
			fmt.Alignment = this.HorizontalAlignment;
			fmt.LineAlignment = this.VerticalAlignment;
			fmt.FormatFlags = this.FormatFlags;
			return fmt;
		}

		/// <summary>
		/// Adjusts the size of the bounding box to fit the text.
		/// </summary>
		/// <param name="grfx">Graphics context used to measure the text</param>
		/// <param name="layoutArea">Maximum layout size of the string</param>
		/// <returns>New size of the bounding box</returns>
		/// <remarks>
		/// <para>
		/// Uses the System.Drawing.Graphics.MeasureString method to calculate the
		/// size of the bounding box based on the font and text value.
		/// </para>
		/// </remarks>
		public SizeF SizeToText(Graphics grfx, SizeF layoutArea)
		{
			Font font = this.fontStyle.CreateFont();
			SizeF szText = grfx.MeasureString(this.Text, font, layoutArea, this.GetStringFormat());
			font.Dispose();
			this.Size = szText;
			return szText;
		}

		/// <summary>
		/// Adjusts the size of the bounding box to fit the text.
		/// </summary>
		/// <param name="layoutArea">Maximum layout size of the string</param>
		/// <returns>New size of the bounding box</returns>
		/// <remarks>
		/// <para>
		/// Uses the System.Drawing.Graphics.MeasureString method to calculate the
		/// size of the bounding box based on the font and text value.
		/// </para>
		/// </remarks>
		public SizeF SizeToText(SizeF layoutArea)
		{
			Graphics grfx = Graphics.FromHdc(Window.GetDCEx(IntPtr.Zero, IntPtr.Zero, GDI.DCX_CACHE|GDI.DCX_LOCKWINDOWUPDATE|GDI.DCX_CLIPCHILDREN|GDI.DCX_CLIPSIBLINGS));
			return this.SizeToText(grfx, layoutArea);
		}

		#endregion

		#region IDraw interface

		/// <summary>
		/// Renders the text object to a graphics context.
		/// </summary>
		/// <param name="grfx">Graphics context object to render to</param>
		public virtual void Draw(System.Drawing.Graphics grfx)
		{
			RectangleF bounds = this.GetTextBox();

			Matrix worldXform = Global.MatrixStack.Push(this.matrix, MatrixOrder.Prepend);
			grfx.Transform = worldXform;
			grfx.MultiplyTransform(Global.ViewMatrix, MatrixOrder.Append);

			Brush backgroundBrush = this.backgroundStyle.CreateBrush();
			grfx.FillRectangle(backgroundBrush, bounds.Left, bounds.Top, bounds.Width, bounds.Height);
			backgroundBrush.Dispose();

			Font font = this.fontStyle.CreateFont();
			Brush fillBrush = this.fillStyle.CreateBrush();
			StringFormat fmt = GetStringFormat();
			grfx.DrawString(this.Text, font, fillBrush, bounds, fmt);
			font.Dispose();
			fillBrush.Dispose();

			if (this.borderStyle.ShowBorder)
			{
				Pen pen = this.borderStyle.CreatePen();
				grfx.DrawRectangle(pen, bounds.Left, bounds.Top, bounds.Width, bounds.Height);
				pen.Dispose();
			}

			Global.MatrixStack.Pop();
		}

		#endregion

		#region ITransform interface

		/// <summary>
		/// Moves the text object by the given X and Y offsets.
		/// </summary>
		/// <param name="dx">Distance to move along X axis</param>
		/// <param name="dy">Distance to move along Y axis</param>
		/// <remarks>
		/// Applies a translate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.OnMove"/>
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
		/// Rotates the text object a specified number of degrees about a given
		/// anchor point.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to rotate</param>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.OnRotate"/>
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
		/// Rotates the text object a specified number of degrees about its center point.
		/// </summary>
		/// <param name="degrees">Number of degrees to rotate</param>
		/// <remarks>
		/// Applies a rotate operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.OnRotate"/>
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
		/// Scales the text object by a given ratio along the X and Y axes.
		/// </summary>
		/// <param name="ptAnchor">Fixed point about which to scale</param>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.LocalTransform"/>
		/// matrix. The <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.OnScale"/>
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
		/// Scales the text object about its center point by a given ratio.
		/// </summary>
		/// <param name="sx">Scaling ratio for X axis</param>
		/// <param name="sy">Scaling ratio for Y axis</param>
		/// <remarks>
		/// Applies a scale operation to the the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.TextBase.LocalTransform"/>
		/// matrix. The OnScale method is called after the change is made.
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
		/// Matrix containing transformations for this node.
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

		#region IHitTestBounds interface

		/// <summary>
		/// Tests to see if the object's bounding box contains the given point.
		/// </summary>
		/// <param name="ptTest">Point to test</param>
		/// <param name="fSlop">Expands the area to be tested</param>
		/// <returns>true if the object contains the given point, otherwise false</returns>
		bool IHitTestBounds.ContainsPoint(PointF ptTest, float fSlop)
		{
			RectangleF bounds = this.Bounds;
			return bounds.Contains(ptTest);
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

		#region IPropertyContainer interface

		/// <summary>
		/// Sets the default property values for the text object.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// text object to their default values.
		/// </remarks>
		public virtual void SetDefaultPropertyValues()
		{
			this.propertyValues.Add("FillType", Syncfusion.Windows.Forms.Diagram.FillStyle.FillType.Solid);
			this.propertyValues.Add("FillColor", Color.Black);
			this.propertyValues.Add("BackgroundType", Syncfusion.Windows.Forms.Diagram.FillStyle.FillType.Solid);
			this.propertyValues.Add("BackgroundColor", Color.White);
			this.propertyValues.Add("BorderColor", Color.Black);
			this.propertyValues.Add("BorderWidth", 1);
			this.propertyValues.Add("ShowBorder", false);
			this.propertyValues.Add("AllowSelect", true);
			this.propertyValues.Add("AllowVertexEdit", false);
			this.propertyValues.Add("AllowMove", true);
			this.propertyValues.Add("AllowRotate", true);
			this.propertyValues.Add("AllowResize", true);
			this.propertyValues.Add("ReadOnly", false);
			this.propertyValues.Add("FontFamily", "Times New Roman");
			this.propertyValues.Add("FontStyle", System.Drawing.FontStyle.Regular);
			this.propertyValues.Add("FontSize", 12.0f);
			this.propertyValues.Add("FontUnit", GraphicsUnit.Point);
			this.propertyValues.Add("HorizontalAlignment", StringAlignment.Near);
			this.propertyValues.Add("VerticalAlignment", StringAlignment.Near);
			this.propertyValues.Add("WrapText", true);
			this.propertyValues.Add("DirectionRightToLeft", false);
			this.propertyValues.Add("DirectionVertical", false);
			this.propertyValues.Add("FitBlackBox", false);
			this.propertyValues.Add("LineLimit", false);
			this.propertyValues.Add("MeasureTrailingSpaces", false);
			this.propertyValues.Add("NoClip", false);
			this.propertyValues.Add("Visible", true);
		}

		/// <summary>
		/// Retrieve the value of a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to retrieve</param>
		/// <returns>Value of the named property or null if it doesn't exist</returns>
		public virtual object GetPropertyValue(string propertyName)
		{
			if (propertyName == "Text")
			{
				return this.Text;
			}
			else if (propertyName == "Name")
			{
				return this.Name;
			}

			if (this.propertyValues.Contains(propertyName))
			{
				return this.propertyValues[propertyName];
			}

			if (this.parent != null)
			{
				IPropertyContainer parentProps = this.parent.GetPropertyContainer(this);
				if (parentProps != null)
				{
					return parentProps.GetPropertyValue(propertyName);
				}
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

			if (propertyName == "Text")
			{
				oldVal = this.Text;
				this.Text = (string) val;
			}

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
			if (propertyName == "Text")
			{
				this.Text = (string) val;
			}

			this.propertyValues[propertyName] = val;
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
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.PropertyChanged(PropertyEventArgs evtArgs)
		{
			this.OnPropertyChanged(evtArgs);
		}

		/// <summary>
		/// Called before the collection of child nodes is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ChildrenChanging(NodeCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// Called after the collection of child nodes is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeCollection.EventArgs"/>
		/// </remarks>
		void IDispatchNodeEvents.ChildrenChangeComplete(NodeCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.BoundsChanged(BoundsEventArgs evtArgs)
		{
			this.OnBoundsChanged(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.Move(MoveEventArgs evtArgs)
		{
			this.OnMove(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.Rotate(RotateEventArgs evtArgs)
		{
			this.OnRotate(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.Scale(ScaleEventArgs evtArgs)
		{
			this.OnScale(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.Click(NodeMouseEventArgs evtArgs)
		{
			this.OnClick(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.DoubleClick(NodeMouseEventArgs evtArgs)
		{
			this.OnDoubleClick(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.MouseEnter(NodeMouseEventArgs evtArgs)
		{
			this.OnMouseEnter(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.MouseLeave(NodeMouseEventArgs evtArgs)
		{
			this.OnMouseLeave(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.InsertVertex(VertexEventArgs evtArgs)
		{
			this.OnInsertVertex(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.MoveVertex(VertexEventArgs evtArgs)
		{
			this.OnMoveVertex(evtArgs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evtArgs"></param>
		void IDispatchNodeEvents.DeleteVertex(VertexEventArgs evtArgs)
		{
			this.OnDeleteVertex(evtArgs);
		}

		/// <summary>
		/// Called before the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		void IDispatchNodeEvents.ConnectionsChanging(ConnectionCollection.EventArgs evtArgs)
		{
		}

		/// <summary>
		/// Called after the connection list of a symbol is modified.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		void IDispatchNodeEvents.ConnectionsChangeComplete(ConnectionCollection.EventArgs evtArgs)
		{
		}

		#endregion

		#region Node Event Callbacks

		/// <summary>
		/// Called when a property is changed.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// This method notifies the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
		/// of the property change by calling the parent node's
		/// <see cref="Syncfusion.Windows.Forms.Diagram.IDispatchNodeEvents.PropertyChanged"/>
		/// method.
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
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
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Shape.Parent"/>
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
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
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
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
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
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
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
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.NodeMouseEventArgs"/>
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
		/// Called when a vertex is inserted into a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
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
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
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
		/// Called when a vertex is deleted from a node.
		/// </summary>
		/// <param name="evtArgs">Event arguments</param>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.VertexEventArgs"/>
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
			info.AddValue("parent", this.parent);
			info.AddValue("m11", this.matrix.Elements[0]);
			info.AddValue("m12", this.matrix.Elements[1]);
			info.AddValue("m21", this.matrix.Elements[2]);
			info.AddValue("m22", this.matrix.Elements[3]);
			info.AddValue("dx", this.matrix.Elements[4]);
			info.AddValue("dy", this.matrix.Elements[5]);
			info.AddValue("propertyValues", this.propertyValues);
		}

		/// <summary>
		/// Called when deserialization is complete.
		/// </summary>
		/// <param name="sender">Object performing the deserialization</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.borderStyle = new BorderStyle(this);
			this.backgroundStyle = new BackgroundStyle(this);
			this.fillStyle = new FillStyle(this);
			this.editStyle = new EditStyle(this);
			this.fontStyle = new FontStyle(this);
		}

		#endregion

		#region Fields

		/// <summary>
		/// Name of the text object
		/// </summary>
		private string name = "";

		/// <summary>
		/// Reference to the parent node
		/// </summary>
		private ICompositeNode parent;

		/// <summary>
		/// Local transformation matrix
		/// </summary>
		protected Matrix matrix;

		/// <summary>
		/// Hashtable containing property name/value pairs
		/// </summary>
		protected Hashtable propertyValues = null;

		/// <summary>
		/// Border properties
		/// </summary>
		private BorderStyle borderStyle = null;

		/// <summary>
		/// Background properties
		/// </summary>
		private BackgroundStyle backgroundStyle = null;

		/// <summary>
		/// Fill (brush) properties
		/// </summary>
		private FillStyle fillStyle = null;

		/// <summary>
		/// Edit properties
		/// </summary>
		private EditStyle editStyle = null;

		/// <summary>
		/// Font properties
		/// </summary>
		private FontStyle fontStyle = null;

		#endregion
	}
}