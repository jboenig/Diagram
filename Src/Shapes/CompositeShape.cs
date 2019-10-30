using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Syncfusion.Windows.Forms.Canvas
{
	/// <summary>
	/// Summary description for CompositeShape.
	/// </summary>
	[Serializable]
	public class CompositeShape : Shape, ISerializable
	{
		public CompositeShape()
		{
		}

		protected CompositeShape(SerializationInfo info, StreamingContext context)
		{
			m_name = info.GetString("m_name");
			m_parent = (INode) info.GetValue("m_parent", typeof(INode));
			m_matrix = new Matrix();
			m_matrix.Elements[0] = info.GetSingle("m11");
			m_matrix.Elements[1] = info.GetSingle("m12");
			m_matrix.Elements[2] = info.GetSingle("m21");
			m_matrix.Elements[3] = info.GetSingle("m22");
			m_matrix.Elements[4] = info.GetSingle("dx");
			m_matrix.Elements[5] = info.GetSingle("dy");
			m_lineStyle = (LineStyle) info.GetValue("m_lineStyle", typeof(LineStyle));
		}

		public override PointF Location
		{
			get
			{
				return this.Bounds.Location;
			}
			set
			{
			}
		}

		public override SizeF Size
		{
			get
			{
				return this.Bounds.Size;
			}
			set
			{
			}
		}

		public override RectangleF Bounds
		{
			get
			{
				Matrix curMatrix = Global.MatrixStack.Push(m_matrix);
				//PointF[] aPts = { m_rect.Location, new PointF(m_rect.Right, m_rect.Bottom) };
				//curMatrix.TransformPoints(aPts);
				Global.MatrixStack.Pop();
				//return new RectangleF(aPts[0].X, aPts[0].Y, aPts[1].X - aPts[0].X, aPts[1].Y - aPts[0].Y);
				return new RectangleF(0,0,0,0);
			}
		}

		public LineStyle LineStyle
		{
			get
			{
				return m_lineStyle;
			}
		}

		public override void Draw(System.Drawing.Graphics grfx)
		{
			grfx.Transform = Global.MatrixStack.Push(m_matrix);

			Pen pen = m_lineStyle.CreatePen();
			grfx.DrawPath(pen, m_graphicsPath);
			pen.Dispose();

			grfx.Transform = Global.MatrixStack.Pop();
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("m_name", m_name);
			info.AddValue("m_parent", m_parent);
			info.AddValue("m11", m_matrix.Elements[0]);
			info.AddValue("m12", m_matrix.Elements[1]);
			info.AddValue("m21", m_matrix.Elements[2]);
			info.AddValue("m22", m_matrix.Elements[3]);
			info.AddValue("dx", m_matrix.Elements[4]);
			info.AddValue("dy", m_matrix.Elements[5]);
			info.AddValue("m_lineStyle", m_lineStyle);
		}

		protected GraphicsPath m_graphicsPath = new GraphicsPath();
		protected LineStyle m_lineStyle = new LineStyle();
	}
}
