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
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// A symbol is a composite node that supports labels and connections to other
	/// symbols and links.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Shapes, text, images, and other types of nodes can be aggregated together
	/// in a symbol. A symbol draws itself by drawing all of its child nodes,
	/// so its appearance is determined by union of all its children. The bounds
	/// of a symbol are also determined by the union of all its child nodes. The
	/// position and size of child nodes is always relative to the symbol.
	/// </para>
	/// <para>
	/// Symbol may contain one or more ports positioned anywhere within its bounds.
	/// A port is a point at which connections to other symbols and links can be
	/// established. NOTE: A link is a special type of symbol that contains a shape
	/// (typically a line or polyline) and two ports on each end. Much of the
	/// information about symbols is also applicable to links. A
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Connection"/> is an object that
	/// binds two ports together and establishes a connection between two symbols.
	/// Symbols that are connected co-operate by send messages to each other when
	/// certain events occur, such as movement or resizing.
	/// </para>
	/// <para>
	/// All symbols contain a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.CenterPort"/>,
	/// which is a port that is always positioned at the center of the symbol's
	/// bounds.
	/// </para>
	/// <para>
	/// Symbols can be connected to other symbols and links using the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Connect"/>
	/// method, which creates a new
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Connection"/> object and
	/// wires it up to the ports provided. The port belonging to the symbol
	/// object the Connect method was invoked on is referred to as the
	/// local port, and the port belonging to the symbol to which the
	/// connection is being made is referred to as the foreign port. In
	/// other words, the foreign port belongs to "this" symbol and the
	/// foreign port belongs to "the other" symbol. When a new connection
	/// is created, it is added to both the local symbol and the foreign
	/// symbol so that they both know about the connection. This is done
	/// automatically by the Connect method.
	/// </para>
	/// <para>
	/// Connections can be destroyed by calling the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolBase.Disconnect"/>
	/// method.
	/// </para>
	/// <para>
	/// A symbol may contain one or more labels. A
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Label"/>
	/// is a type of text object that is anchored to a symbol. A label is positioned by
	/// specifying a <see cref="Syncfusion.Windows.Forms.Diagram.BoxPosition"/>
	/// that corresponds to the bounding box of the symbol the label belongs to. The
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Label.Text"/> property
	/// of a label can either be bound to an independent value maintained by the
	/// label or it can be bound to any property in the symbol it belongs to. This
	/// is very useful for creating a label that displays the symbol name or some
	/// property of the symbol.
	/// </para>
	/// <para>
	/// Labels can be added to a symbol using the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.Symbol.AddLabel"/> method.
	/// Labels call back to the symbol through the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer"/> interface
	/// in order to position themselves relative to the symbol.
	/// </para>
	/// <para>
	/// The symbol class implements the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IGraphNode"/> interface, which
	/// is used in conjunction with the
	/// <see cref="Syncfusion.Windows.Forms.Diagram.IGraphEdge"/> interface
	///  to navigate the node hierarchy as a graph of interconnected nodes and edges.
	/// </para>
	/// <para>
	/// A symbol can be created from a <see cref="Syncfusion.Windows.Forms.Diagram.SymbolModel"/>,
	/// which is a model containing nodes and properties to be applied the symbol. The
	/// Symbol Designer application creates symbol models and stores them inside of a
	/// <see cref="Syncfusion.Windows.Forms.Diagram.SymbolPalette"/>. A symbol palette
	/// is a collection of related symbol models.
	/// </para>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolBase"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Link"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Port"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Connection"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.Label"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.SymbolModel"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphNode"/>
	/// <seealso cref="Syncfusion.Windows.Forms.Diagram.ILabelContainer"/>
	/// </remarks>
	[Serializable]
	public class Symbol : SymbolBase, IGraphNode
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Symbol()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src"></param>
		public Symbol(Symbol src) : base(src)
		{
		}

		/// <summary>
		/// Serialization constructor for symbols.
		/// </summary>
		/// <param name="info">Serialization state information</param>
		/// <param name="context">Streaming context information</param>
		protected Symbol(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>Copy of the object this method is invoked against</returns>
		public override object Clone()
		{
			return new Symbol(this);
		}

		/// <summary>
		/// Collection of labels that belong to the symbol.
		/// </summary>
		[
		Browsable(true),
		Category("Appearance"),
		Description("Collection of text labels attached to the symbol"),
		Editor(typeof(SymbolLabelCollectionEditor), typeof(UITypeEditor))
		]
		public override LabelCollection Labels
		{
			get
			{
				return base.Labels;
			}
		}

		/// <summary>
		/// Adds a label to the symbol.
		/// </summary>
		/// <param name="lbl">Label to add</param>
		public override void AddLabel(Label lbl)
		{
			SymbolLabel symLbl = lbl as SymbolLabel;
			if (symLbl == null)
			{
				throw new EInvalidParameter();
			}
			symLbl.Container = this;
			symLbl.UpdateBounds();
			this.Labels.Add(symLbl);
		}

		/// <summary>
		/// Creates a new label and adds it to the symbol.
		/// </summary>
		/// <param name="anchor">Anchor point for the label</param>
		/// <param name="txtVal">Text value to assign to the label</param>
		/// <returns>The new label that was created</returns>
		public Label AddLabel(string txtVal, BoxPosition anchor)
		{
			Label lbl = new SymbolLabel(this, txtVal, anchor);
			lbl.UpdateBounds();
			this.Labels.Add(lbl);
			return lbl;
		}

		/// <summary>
		/// Adds a label to the symbol.
		/// </summary>
		/// <param name="lbl">Label to add</param>
		public void AddLabel(SymbolLabel lbl)
		{
			lbl.Container = this;
			lbl.UpdateBounds();
			this.Labels.Add(lbl);
		}

		/// <summary>
		/// This method is called by connections when the foreign port container
		/// moves.
		/// </summary>
		/// <param name="connection">Connection that has moved</param>
		/// <remarks>
		/// <para>
		/// This method can be overridden in derived classes.
		/// </para>
		/// </remarks>
		public override void OnConnectionMove(Connection connection)
		{
		}

		/// <summary>
		/// Collection of all edges entering or leaving the symbol.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphNode"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphEdge"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public ICollection Edges
		{
			get
			{
				ArrayList edges = new ArrayList();

				foreach (Connection curConnection in this.Connections)
				{
					Port foreignPort = curConnection.GetForeignPort(this);

					if (foreignPort != null)
					{
						IPortContainer foreignObj = foreignPort.Container;
						if (foreignObj != null)
						{
							IGraphEdge edge = foreignObj as IGraphEdge;
							if (edge != null)
							{
								edges.Add(edge);
							}
						}
					}
				}

				return edges;
			}
		}

		/// <summary>
		/// Collection of edges entering the symbol.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphNode"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphEdge"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public ICollection EdgesEntering
		{
			get
			{
				ArrayList edges = new ArrayList();

				foreach (Connection curConnection in this.Connections)
				{
					Port foreignPort = curConnection.GetForeignPort(this);

					if (foreignPort != null)
					{
						IPortContainer foreignObj = foreignPort.Container;
						if (foreignObj != null)
						{
							IGraphEdge edge = foreignObj as IGraphEdge;
							if (edge != null)
							{
								if (edge.IsNodeEntering(this))
								{
									edges.Add(edge);
								}
							}
						}
					}
				}

				return edges;
			}
		}

		/// <summary>
		/// Collection of edges leaving the symbol.
		/// </summary>
		/// <remarks>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphNode"/>
		/// <seealso cref="Syncfusion.Windows.Forms.Diagram.IGraphEdge"/>
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public ICollection EdgesLeaving
		{
			get
			{
				ArrayList edges = new ArrayList();

				foreach (Connection curConnection in this.Connections)
				{
					Port foreignPort = curConnection.GetForeignPort(this);

					if (foreignPort != null)
					{
						IPortContainer foreignObj = foreignPort.Container;
						if (foreignObj != null)
						{
							IGraphEdge edge = foreignObj as IGraphEdge;
							if (edge != null)
							{
								if (edge.IsNodeLeaving(this))
								{
									edges.Add(edge);
								}
							}
						}
					}
				}

				return edges;
			}
		}
	}
}