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
	/// Text label for a symbol.
	/// </summary>
	[
	Serializable()
	]
	public class SymbolLabel : Label
	{
		/// <summary>
		/// 
		/// </summary>
		public SymbolLabel()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		public SymbolLabel(ILabelContainer container) : base(container)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		/// <param name="txtVal"></param>
		public SymbolLabel(ILabelContainer container, string txtVal) : base(container, txtVal)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		/// <param name="txtVal"></param>
		/// <param name="anchor"></param>
		public SymbolLabel(ILabelContainer container, string txtVal, BoxPosition anchor) : base(container, txtVal)
		{
			this.Anchor = anchor;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		public SymbolLabel(SymbolLabel src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for symbol labels.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected SymbolLabel(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new SymbolLabel(this);
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Positioning"),
		Description("Specifies an anchor point on the symbol at which the label is positioned")
		]
		public BoxPosition Anchor
		{
			get
			{
				object value = this.GetPropertyValue("LabelAnchor");
				if (value != null && value.GetType() == typeof(BoxPosition))
				{
					return (BoxPosition) value;
				}
				return BoxPosition.Center;
			}
			set
			{
				this.SetPropertyValue("LabelAnchor", value);
			}
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
			this.propertyValues.Add("LabelAnchor", BoxPosition.Center);
		}
	}
}