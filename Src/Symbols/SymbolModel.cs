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
using System.Drawing.Design;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Remoting;
using System.ComponentModel;
using System.Reflection;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Design-time representation of a symbol.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class encapsulates the data used to create a symbol. It is a model
	/// that contains nodes and properties that are used to create and initialize
	/// symbols. A symbol model can contain shapes, text, images, and other types
	/// of nodes which are copied into new symbols created from it.
	/// </para>
	/// <para>
	/// The <see cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.CreateSymbol"/>
	/// method is called to create instances of symbols from the symbol model. The
	/// runtime type created by CreateSymbol is determined by the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInAssembly"/>
	/// and
	/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInClass"/>
	/// properties, which are set by default to create
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Symbol"/>
	/// objects. The PlugInAssembly and PlugInClass properties allow developers to
	/// plugin their own derived implementations of the Symbol class.
	/// </para>
	/// <para>
	/// After the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.CreateSymbol"/>
	/// method instantiates the symbol specified by the PlugInAssembly and
	/// PlugInClass properties, it populates with the nodes and properties
	/// contained by the symbol model.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Symbol"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Model"/>
	/// </remarks>
	[Serializable]
	public class SymbolModel : Model, ISerializable
	{
		/// <summary>
		/// Constructs a symbol model
		/// </summary>
		public SymbolModel()
		{
			this.pluginAssembly = "Syncfusion.Diagram";
			this.pluginClass = "Syncfusion.Windows.Forms.Diagram.Symbol";
			this.labels = new LabelCollection();
		}

		/// <summary>
		/// Serialization constructor for symbol models.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected SymbolModel(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.pluginAssembly = info.GetString("pluginAssembly");
			this.pluginClass = info.GetString("pluginClass");
			this.smallIcon = (System.Drawing.Image) info.GetValue("smallIcon", typeof(System.Drawing.Image));
			this.largeIcon = (System.Drawing.Image) info.GetValue("largeIcon", typeof(System.Drawing.Image));
			this.labels = (LabelCollection) info.GetValue("labels", typeof(LabelCollection));
			this.centerPortEnabled = info.GetBoolean("centerPortEnabled");
		}

		/// <summary>
		/// Creates a new instance of the symbol described by the symbol model.
		/// </summary>
		/// <returns>Instance of a symbol</returns>
		/// <remarks>
		/// <para>
		/// Creates an instance of the class specified by the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInAssembly"/>
		/// and
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInClass"/>
		/// properties. By default, the PlugInAssembly and PlugInClass properties are
		/// set to create objects of type
		/// <see cref="Syncfusion.Windows.Forms.Diagram.Symbol"/>.
		/// </para>
		/// <para>
		/// Once the symbol object is created, this method loads it with the child
		/// nodes, labels, and properties contained by the symbol model.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Symbol"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInAssembly"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInClass"/>
		/// </remarks>
		public Symbol CreateSymbol()
		{
			Symbol sym = null;
			int childIdx;

			// Create the new symbol. Use activator to create the new symbol if
			// plugin info is specified. Otherwise, create an instance of Symbol
			// the base Symbol class.
			if (this.pluginAssembly != null && this.pluginAssembly.Length > 0 &&
				this.pluginClass != null && this.pluginClass.Length > 0)
			{
				System.Type symbolType = null;

				if (this.pluginAssembly.StartsWith(DiagramAssembly.Name))
				{
					symbolType = DiagramAssembly.Assembly.GetType(this.pluginClass, false);
				}
				else
				{
					Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
					for (int assemblyIdx = 0; assemblyIdx < assemblies.Length; assemblyIdx++)
					{
						if (assemblies[assemblyIdx].GetName().Name == this.pluginAssembly)
						{
							symbolType = assemblies[assemblyIdx].GetType(this.pluginClass, false);
						}
					}
				}

				if (symbolType != null)
				{
					sym = Activator.CreateInstance(symbolType) as Symbol;
				}
			}
			else
			{
				sym = new Symbol();
			}

			if (sym != null)
			{
				// Bestow upon the new symbol all properties of the symbol model that
				// are valid for the symbol.
				IDictionaryEnumerator enumProps = this.propertyValues.GetEnumerator();
				while (enumProps.MoveNext())
				{
					string propName = (string) enumProps.Key;
					object propVal = enumProps.Value;

					try
					{
						// Apply property value to symbol
						sym.ChangePropertyValue(propName, propVal);
					}
					catch
					{
						// Property doesn't exist in the symbol. Just skip it.
					}
				}

				// Iterate through all children in the model and append each
				// child to the new symbol.
				foreach (INode curChild in this.children)
				{
					object curChildClone = curChild.Clone();
					if (curChildClone != null)
					{
						INode curCloneNode = curChildClone as INode;
						if (curCloneNode != null)
						{
							childIdx = sym.AppendChild(curCloneNode);
						}
					}
				}

				// Add labels to the symbol
				foreach (Label lbl in this.Labels)
				{
					object curLblClone = lbl.Clone();
					if (curLblClone != null)
					{
						sym.AddLabel((Label) curLblClone);
					}
				}

				// Configure the symbol's center port
				sym.CenterPort.Enabled = this.centerPortEnabled;
			}

			return sym;
		}

		/// <summary>
		/// Background style of the symbol model.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Inherited from the base class and overridden here in order to hide it
		/// during design-time.
		/// </para>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override BackgroundStyle BackgroundStyle
		{
			get
			{
				return base.BackgroundStyle;
			}
		}

		/// <summary>
		/// Name of the assembly containing the implementation of the class used
		/// to create run-time instances of the symbol.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Set to Syncfusion.Diagram by default. Used in conjunction with the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInClass"/>
		/// property to create symbol objects.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInClass"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.CreateSymbol"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Subclassing"),
		Description("Name of the assembly containing the class instantiated at runtime for this symbol")
		]
		public string PlugInAssembly
		{
			get
			{
				return this.pluginAssembly;
			}
			set
			{
				this.pluginAssembly = value;
			}
		}

		/// <summary>
		/// Name of the class used to create run-time instances of the symbol.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Set to <see cref="Syncfusion.Windows.Forms.Diagram.Symbol"/> by default.
		/// Used in conjunction with the
		/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInAssembly"/>
		/// property to create symbol objects.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.PlugInAssembly"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.CreateSymbol"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Subclassing"),
		Description("Name of the class instantiated at runtime for this symbol")
		]
		public string PlugInClass
		{
			get
			{
				return this.pluginClass;
			}
			set
			{
				this.pluginClass = value;
			}
		}

		/// <summary>
		/// Labels to add to symbols.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The collection of labels that will be added to symbols created by
		/// this symbol model.
		/// </para>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Appearance"),
		Description("Collection of text labels attached to the symbol"),
		Editor(typeof(SymbolLabelCollectionEditor), typeof(UITypeEditor))
		]
		public LabelCollection Labels
		{
			get
			{
				return this.labels;
			}
		}

		/// <summary>
		/// A 16x16 image used to represent the symbol as an icon.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.LargeIcon"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Appearance"),
		Description("16 x 16 image used to represent the symbol")
		]
		public System.Drawing.Image SmallIcon
		{
			get
			{
				return this.smallIcon;
			}
			set
			{
				object oldVal = this.smallIcon;
				this.smallIcon = value;
				this.OnPropertyChanged(new PropertyEventArgs(this, "SmallIcon", oldVal, this.smallIcon));
			}
		}

		/// <summary>
		/// A 32x32 image used to represent the symbol as an icon.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel.SmallIcon"/>
		/// </remarks>
		[
		Browsable(true),
		Category("Appearance"),
		Description("32 x 32 image used to represent the symbol")
		]
		public System.Drawing.Image LargeIcon
		{
			get
			{
				return this.largeIcon;
			}
			set
			{
				object oldVal = this.largeIcon;
				this.largeIcon = value;
				this.OnPropertyChanged(new PropertyEventArgs(this, "LargeIcon", oldVal, this.largeIcon));
			}
		}

		/// <summary>
		/// Boolean flag that determines if ports are shown only when a user-interface
		/// tool creating connections is active.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.LinkTool"/>
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Behavior"),
		Description("Determines if ports are visible when they are not being used by an interactive tool to create connections")
		]
		public bool AutoHidePorts
		{
			get
			{
				object value = this.GetPropertyValue("AutoHidePorts");

				if (value != null && value.GetType() == typeof(bool))
				{
					return (bool) value;
				}

				return false;
			}
			set
			{
				this.SetPropertyValue("AutoHidePorts", value);
			}
		}

		/// <summary>
		/// Boolean flag that determines if the center port of the symbol will be
		/// enabled or disabled.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Category("Behavior"),
		Description("Determines if center port is enabled or disabled")
		]
		public bool CenterPortEnabled
		{
			get
			{
				return this.centerPortEnabled;
			}
			set
			{
				this.centerPortEnabled = value;
			}
		}

		/// <summary>
		/// Collection of layers in the model.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property is inherited from the base class and is overriden in this
		/// class in order to hide it at design-time.
		/// </para>
		/// </remarks>
		[
		Browsable(false)
		]
		public override LayerCollection Layers
		{
			get
			{
				return base.Layers;
			}
		}

		/// <summary>
		/// Name of the default layer.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property is inherited from the base class and is overriden in this
		/// class in order to hide it at design-time.
		/// </para>
		/// </remarks>
		[
		Browsable(false)
		]
		public override string DefaultLayerName
		{
			get
			{
				return base.DefaultLayerName;
			}
			set
			{
				base.DefaultLayerName = value;
			}
		}

		/// <summary>
		/// Sets the default property values for the model.
		/// </summary>
		/// <remarks>
		/// This method can be called at any time to reset the properties of the
		/// model to their default values.
		/// </remarks>
		public override void SetDefaultPropertyValues()
		{
			base.SetDefaultPropertyValues();
			this.propertyValues.Add("AutoHidePorts", false);
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
			info.AddValue("propertyValues", this.propertyValues);
			info.AddValue("parent", this.Parent);
			info.AddValue("bounds", this.Bounds);
			info.AddValue("m11", this.matrix.Elements[0]);
			info.AddValue("m12", this.matrix.Elements[1]);
			info.AddValue("m21", this.matrix.Elements[2]);
			info.AddValue("m22", this.matrix.Elements[3]);
			info.AddValue("dx", this.matrix.Elements[4]);
			info.AddValue("dy", this.matrix.Elements[5]);
			info.AddValue("children", this.children);
			info.AddValue("layers", this.Layers);
			info.AddValue("defaultLayer", this.DefaultLayer);
			info.AddValue("measurementUnits", this.MeasurementUnits);
			info.AddValue("measurementScale", this.MeasurementScale);
			info.AddValue("pluginAssembly", this.pluginAssembly);
			info.AddValue("pluginClass", this.pluginClass);
			info.AddValue("smallIcon", this.smallIcon);
			info.AddValue("largeIcon", this.largeIcon);
			info.AddValue("labels", this.labels);
			info.AddValue("centerPortEnabled", this.centerPortEnabled);
		}

		/// <summary>
		/// Name of assembly containing the implementation of the symbol class
		/// </summary>
		private string pluginAssembly;

		/// <summary>
		/// Name of class that implements the symbol
		/// </summary>
		private string pluginClass;

		/// <summary>
		/// 16x16 icon image
		/// </summary>
		private System.Drawing.Image smallIcon = null;

		/// <summary>
		/// 32x32 icon image
		/// </summary>
		private System.Drawing.Image largeIcon = null;

		/// <summary>
		/// Collection of labels contained by the symbol
		/// </summary>
		private LabelCollection labels = null;

		/// <summary>
		/// Flag indicating if center port of symbol is enabled or disabled
		/// </summary>
		private bool centerPortEnabled = true;
	}
}
