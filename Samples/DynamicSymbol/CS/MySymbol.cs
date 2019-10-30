using System;
using System.Drawing;
using Syncfusion.Windows.Forms.Diagram;

namespace DynamicSymbol
{
	/// <summary>
	/// Summary description for MySymbol.
	/// </summary>
	public class MySymbol : Symbol
	{
		private Syncfusion.Windows.Forms.Diagram.Rectangle outerRect = null;
		private Ellipse innerEllipse = null;
		private int curEllipseColor = 0;

		static System.Drawing.Color[] ellipseColors =
		{
			Color.LightBlue,
			Color.Silver,
			Color.Yellow,
			Color.MidnightBlue
		};

		public MySymbol()
		{
			//
			// Add child nodes to the symbol programmatically
			//

			// Add an outer rectangle
			this.outerRect = new Syncfusion.Windows.Forms.Diagram.Rectangle(0, 0, 120, 80);
			this.outerRect.FillStyle.Color = Color.Khaki;
			this.AppendChild(outerRect);

			// Add an inner ellipse
			this.innerEllipse = new Ellipse(10, 10, 100, 60);
			this.innerEllipse.FillStyle.Color = ellipseColors[this.curEllipseColor];
			this.AppendChild(innerEllipse);

			this.AddLabel("Hello world", BoxPosition.Center);
		}

		protected override void OnMouseEnter(NodeMouseEventArgs evtArgs)
		{
			this.outerRect.FillStyle.Color = Color.Green;
		}

		protected override void OnMouseLeave(NodeMouseEventArgs evtArgs)
		{
			this.outerRect.FillStyle.Color = Color.Khaki;
		}

		protected override void OnClick(NodeMouseEventArgs evtArgs)
		{
			if (this.curEllipseColor == ellipseColors.Length-1)
			{
				this.curEllipseColor = 0;
			}
			else
			{
				this.curEllipseColor++;
			}
			this.innerEllipse.FillStyle.Color = ellipseColors[this.curEllipseColor];
		}
	}
}
