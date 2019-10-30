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
	/// Specialized label for links.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The position of a link label is calculated from a certain
	/// percentage along the line contained by the link. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.LinkLabel.PercentAlongLine"/>
	/// property contains the percentage value used to orient the label
	/// with respect to the link.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link"/>
	/// </remarks>
	[
	Serializable()
	]
	public class LinkLabel : Label
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LinkLabel()
		{
		}

		/// <summary>
		/// Construct a LinkLabel object given a label container.
		/// </summary>
		/// <param name="container">Container that owns the label</param>
		public LinkLabel(ILabelContainer container) : base(container)
		{
		}

		/// <summary>
		/// Construct a LinkLabel object given a label container and text value
		/// for the label.
		/// </summary>
		/// <param name="container">Container that owns the label</param>
		/// <param name="txtVal">Value to assign to the label</param>
		public LinkLabel(ILabelContainer container, string txtVal) : base(container, txtVal)
		{
		}

		/// <summary>
		/// Construct a LinkLabel object given a label container, a text value,
		/// and an orientation value.
		/// </summary>
		/// <param name="container">Container that owns the label</param>
		/// <param name="txtVal">Value to assign to the label</param>
		/// <param name="pctAlongLine">Where along the line to orient the label</param>
		public LinkLabel(ILabelContainer container, string txtVal, int pctAlongLine) : base(container, txtVal)
		{
			this.PercentAlongLine = pctAlongLine;
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src">Source object to copy</param>
		public LinkLabel(LinkLabel src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for link labels.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected LinkLabel(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new LinkLabel(this);
		}

		/// <summary>
		/// Determines where the label is oriented with respect to the link.
		/// </summary>
		/// <remarks>
		/// The value of this property is a percentage that can range from 0
		/// to 100. The position of the label is calculated as this percentage
		/// value along the line contained by the link.
		/// </remarks>
		[
		Browsable(true),
		Category("Orientation")
		]
		public int PercentAlongLine
		{
			get
			{
				object value = this.GetPropertyValue("LabelPercentAlongLine");
				if (value != null && value.GetType() == typeof(int))
				{
					return (int) value;
				}
				return 50;
			}
			set
			{
				int cookedValue = value;
				if (value < 0)
				{
					cookedValue = 0;
				}
				if (value > 100)
				{
					cookedValue = 100;
				}
				this.SetPropertyValue("LabelPercentAlongLine", cookedValue);
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
			this.propertyValues.Add("LabelPercentAlongLine", 50);
		}
	}
}