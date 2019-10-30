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
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Specifies the property the label value is bound to.
	/// </summary>
	public enum LabelPropertyBinding
	{
		/// <summary>
		/// Text property of the label.
		/// </summary>
		Text,

		/// <summary>
		/// Name property in the container.
		/// </summary>
		ContainerName
	}

	/// <summary>
	/// A label is a text object that is attached to a container and
	/// is positioned relative to some control point on the container.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Typically, the container is an instance of a class derived from
	/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase"/>, which
	/// implements the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer"/>
	/// interface. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Symbol"/> and
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Link"/>
	/// classes are both derived from SymbolBase, and therefore are
	/// label containers.
	/// </para>
	/// <para>
	/// The position of a label is calculated by its container. The label
	/// calls the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer.CalcLabelPosition"/>
	/// method to request its position.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.TextBase"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer"/>
	/// </remarks>
	[
	Serializable()
	]
	public abstract class Label : TextBase, ILogicalUnitContainer, ISerializable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Label()
		{
			this.text = "";
		}

		/// <summary>
		/// Construct a label and bind it to a container.
		/// </summary>
		/// <param name="container">Container to bind the label to</param>
		public Label(ILabelContainer container)
		{
			this.Container = container;
			this.text = "";
		}

		/// <summary>
		/// Construct a label with the specified text value and bind it to a container.
		/// </summary>
		/// <param name="container">Container to bind the label to</param>
		/// <param name="txtVal">Text value to assign to the label</param>
		public Label(ILabelContainer container, string txtVal)
		{
			this.Container = container;
			this.text = txtVal;
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Label to copy values from</param>
		public Label(Label src) : base(src)
		{
			this.text = src.text;
			this.offsetX = src.offsetX;
			this.offsetY = src.offsetY;
			this.width = src.width;
			this.height = src.height;
		}

		/// <summary>
		/// Serialization constructor for labels.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Label(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (this.Parent != null)
			{
				this.container = this.Parent as ILabelContainer;
			}
			this.width = (float) info.GetDouble("width");
			this.height = (float) info.GetDouble("height");
			this.offsetX = (float) info.GetDouble("offsetX");
			this.offsetY = (float) info.GetDouble("offsetY");
			this.text = info.GetString("text");
		}

		/// <summary>
		/// Reference to the composite node this node is a child of.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override ICompositeNode Parent
		{
			get
			{
				return base.Parent;
			}
			set
			{
				ILabelContainer container = value as ILabelContainer;
				if (container == null)
				{
					throw new EInvalidParameter();
				}
				this.container = container;
				base.Parent = this.container as ICompositeNode;
			}
		}

		/// <summary>
		/// Container that the label is bound to.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The container determines where the label is positioned.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public ILabelContainer Container
		{
			get
			{
				return this.container;
			}
			set
			{
				this.container = value;
				this.Parent = null;

				if (value != null)
				{
					this.Parent = this.container as ICompositeNode;
				}
			}
		}

		/// <summary>
		/// X coordinate of the bounding box.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override float X
		{
			get
			{
				return this.Bounds.Left;
			}
			set
			{
			}
		}

		/// <summary>
		/// Y coordinate of the bounding box.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override float Y
		{
			get
			{
				return this.Bounds.Top;
			}
			set
			{
			}
		}

		/// <summary>
		/// Number of logical units to offset the label from the control point.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The offset value is added to the position calculated by the container.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Positioning")
		]
		public float OffsetX
		{
			get
			{
				return this.offsetX;
			}
			set
			{
				this.offsetX = value;
			}
		}

		/// <summary>
		/// Number of logical units to offset the label from the control point.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The offset value is added to the position calculated by the container.
		/// </para>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Positioning")
		]
		public float OffsetY
		{
			get
			{
				return this.offsetY;
			}
			set
			{
				this.offsetY = value;
			}
		}

		/// <summary>
		/// Bounding box of the label.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Attempting to set the bounds of a label throws an exception, because
		/// the bounds of a label are determined by the anchor point.
		/// </para>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override RectangleF Bounds
		{
			get
			{
				RectangleF rcBounds;

				if (this.container != null)
				{
					PointF ptLabel = this.container.CalcLabelPosition(this);

					// Center label on anchor point
					float left = (ptLabel.X - (this.width / 2.0f)) + this.offsetX;
					float top = (ptLabel.Y - (this.height / 2.0f)) + this.offsetY;
					float right = left + this.width;
					float bottom = top + this.height;

					// Transform bounding rectangle to world coordinates
					Matrix worldXform = this.container.GetLabelTransform(this.matrix);
					PointF[] boundingPts = new PointF[]
						{
							new PointF(left, top),
							new PointF(right, bottom)
						};

					worldXform.TransformPoints(boundingPts);
					rcBounds = Geometry.CreateRect(boundingPts[0], boundingPts[1]);
				}
				else
				{
					rcBounds = new RectangleF(0,0,0,0);
				}

				return rcBounds;
			}
			set
			{
				throw new EInvalidOperation();
			}
		}

		/// <summary>
		/// Width and height of the label.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override SizeF Size
		{
			get
			{
				return new SizeF(this.width, this.height);
			}
			set
			{
				this.width = value.Width;
				this.height = value.Height;
			}
		}

		/// <summary>
		/// Returns the raw bounding box for the text in local coordinates.
		/// </summary>
		/// <returns>Bounding box for text</returns>
		public override RectangleF GetTextBox()
		{
			RectangleF rcBounds;

			if (this.container != null)
			{
				PointF ptLabel = this.container.CalcLabelPosition(this);

				// Center label on anchor point
				float left = (ptLabel.X - (this.width / 2.0f)) + this.offsetX;
				float top = (ptLabel.Y - (this.height / 2.0f)) + this.offsetY;
				rcBounds = new RectangleF(left, top, this.width, this.height);
			}
			else
			{
				rcBounds = new RectangleF(0,0,0,0);
			}

			return rcBounds;
		}

		/// <summary>
		/// 
		/// </summary>
		public void UpdateBounds()
		{
		}

		/// <summary>
		/// Text value displayed by the label.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of this property depends on the value of the PropertyBinding
		/// property.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label.PropertyBinding"/>
		/// </remarks>
		[
		Browsable(true)
		]
		public override string Text
		{
			get
			{
				return (string) this.GetPropertyValue(this.PropertyBinding.ToString());
			}
			set
			{
				this.SetPropertyValue(this.PropertyBinding.ToString(), value);
			}
		}

		/// <summary>
		/// Binds the text value of the label to a property.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property allows the label to be attached to any property of the
		/// label. The default binding is to the "Text" property, which is a
		/// string property maintained by the label. Setting this property to
		/// "ContainerName" will cause the label to display the name of the
		/// container (i.e. symbol) that is hosting it.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label.Text"/>
		/// </remarks>
		[
		Browsable(true),
		Category("General"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Description("Binds the text value of the label to a property")
		]
		public LabelPropertyBinding PropertyBinding
		{
			get
			{
				object value = this.GetPropertyValue("LabelPropertyBinding");

				if (value == null)
				{
					return LabelPropertyBinding.Text;
				}

				return (LabelPropertyBinding) value;
			}
			set
			{
				this.SetPropertyValue("LabelPropertyBinding", value);
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

			this.width = Measurements.Convert(fromUnits, toUnits, grfx, this.width);
			this.height = Measurements.Convert(fromUnits, toUnits, grfx, this.height);
			this.offsetX = Measurements.Convert(fromUnits, toUnits, grfx, this.offsetX);
			this.offsetY = Measurements.Convert(fromUnits, toUnits, grfx, this.offsetY);
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
		/// 
		/// </summary>
		/// <param name="grfx"></param>
		public override void Draw(System.Drawing.Graphics grfx)
		{
			Matrix worldXform;

			if (this.container != null)
			{
				worldXform = this.container.GetLabelTransform(this.matrix);
			}
			else
			{
				worldXform = this.matrix;
			}

			RectangleF bounds = this.GetTextBox();
			Matrix prevXform = grfx.Transform;

			grfx.Transform = worldXform;
			grfx.MultiplyTransform(Global.ViewMatrix, MatrixOrder.Append);

			Brush backgroundBrush = this.BackgroundStyle.CreateBrush();
			grfx.FillRectangle(backgroundBrush, bounds.Left, bounds.Top, bounds.Width, bounds.Height);
			backgroundBrush.Dispose();

			Font font = this.FontStyle.CreateFont();
			Brush fillBrush = new SolidBrush(Color.Black);
			StringFormat fmt = GetStringFormat();
			grfx.DrawString(this.Text, font, fillBrush, bounds, fmt);
			font.Dispose();
			fillBrush.Dispose();

			if (this.BorderStyle.ShowBorder)
			{
				Pen pen = this.BorderStyle.CreatePen();
				grfx.DrawRectangle(pen, bounds.Left, bounds.Top, bounds.Width, bounds.Height);
				pen.Dispose();
			}

			grfx.Transform = prevXform;
		}

		/// <summary>
		/// Sets the default property values for the label.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// label to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			base.SetDefaultPropertyValues();
			this.propertyValues.Add("LabelPropertyBinding", LabelPropertyBinding.Text);
		}

		/// <summary>
		/// Retrieve the value of a property given its name.
		/// </summary>
		/// <param name="propertyName">Name of property to retrieve</param>
		/// <returns>Value of the named property or null if it doesn't exist</returns>
		public override object GetPropertyValue(string propertyName)
		{
			if (propertyName == "Text")
			{
				return this.text;
			}
			else if (propertyName == "ContainerName")
			{
				if (this.container != null)
				{
					IPropertyContainer propContainer = this.container.GetService(typeof(IPropertyContainer)) as IPropertyContainer;
					if (propContainer != null)
					{
						return (string) propContainer.GetPropertyValue("Name");
					}
				}
			}

			if (this.propertyValues.Contains(propertyName))
			{
				return this.propertyValues[propertyName];
			}

			if (this.Parent != null)
			{
				IPropertyContainer parentProps = this.Parent.GetPropertyContainer(this);
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
		public override void SetPropertyValue(string propertyName, object val)
		{
			if (propertyName == "Text")
			{
				this.text = (string) val;
			}
			else if (propertyName == "ContainerName")
			{
				if (this.container != null)
				{
					IPropertyContainer propContainer = this.container.GetService(typeof(IPropertyContainer)) as IPropertyContainer;
					if (propContainer != null)
					{
						propContainer.SetPropertyValue("Name", val);
					}
				}
			}
			else
			{
				if (this.propertyValues.ContainsKey(propertyName))
				{
					this.propertyValues[propertyName] = val;
				}
				else
				{
					this.propertyValues.Add(propertyName, val);
				}
			}
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
		public override void ChangePropertyValue(string propertyName, object val)
		{
			if (propertyName == "Text")
			{
				this.text = (string) val;
			}
			else if (propertyName == "ContainerName")
			{
				if (this.container != null)
				{
					IPropertyContainer propContainer = this.container.GetService(typeof(IPropertyContainer)) as IPropertyContainer;
					if (propContainer != null)
					{
						propContainer.ChangePropertyValue("Name", val);
					}
				}
			}
			else
			{
				this.propertyValues[propertyName] = val;
			}
		}

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
			info.AddValue("text", this.text);
			info.AddValue("propertyValues", this.propertyValues);
			info.AddValue("container", this.container);
			info.AddValue("width", this.width);
			info.AddValue("height", this.height);
			info.AddValue("offsetX", this.offsetX);
			info.AddValue("offsetY", this.offsetY);
		}

		/// <summary>
		/// 
		/// </summary>
		private ILabelContainer container = null;

		/// <summary>
		/// 
		/// </summary>
		private string text = "";

		/// <summary>
		/// 
		/// </summary>
		private float width = 60.0f;

		/// <summary>
		/// 
		/// </summary>
		private float height = 40.0f;

		/// <summary>
		/// 
		/// </summary>
		private float offsetX = 0.0f;

		/// <summary>
		/// 
		/// </summary>
		private float offsetY = 0.0f;
	}
}