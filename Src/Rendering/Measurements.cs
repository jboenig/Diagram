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

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Conversions between logical units of measure.
	/// </summary>
	public class Measurements
	{
		static private float screenDpiX = 96;
		static private float screenDpiY = 96;

		static private float[,] conversionRatios = new float[,]
		{
			// Display           Document           Inches           Millimeters       Points
			{  1.0f,             (300.0f/75.0f),    75.0f,           (75.0f*25.4f),    (75.0f/72.0f) },  // Display
			{  (75.0f/300.0f),   1.0f,              300.0f,          1.0f,             1.0f },           // Document
			{  (1.0f/75.0f),     (1.0f/300.0f),     1.0f,            25.4f,            72.0f },          // Inches
			{  1.0f,             1.0f,              (1.0f/25.4f),    1.0f,             (72.0f/25.4f) },  // Millimeters
			{  (72.0f/75.0f),    1.0f,              (1.0f/72.0f),    (25.4f/72.0f),    1.0f }            // Points
		};

		static private int GraphicsUnitIndex(GraphicsUnit grfxUnit)
		{
			int idx = -1;

			switch (grfxUnit)
			{
				case GraphicsUnit.Display:
					idx = 0;
					break;

				case GraphicsUnit.Document:
					idx = 1;
					break;

				case GraphicsUnit.Inch:
					idx = 2;
					break;

				case GraphicsUnit.Millimeter:
					idx = 3;
					break;

				case GraphicsUnit.Point:
					idx = 4;
					break;
			}

			return idx;
		}

		/// <summary>
		/// Gets the screen resolution and saves it for use in conversions.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Conversions that involve the Graphics.Pixel unit of measure require the
		/// DPI (dots per inch) to convert to and from real-world units of measure.
		/// This method gets the screen resolution by creating a System.Drawing.Graphics
		/// object for the screen and then it stores the X and Y DPI for later
		/// use in calculations.
		/// </para>
		/// </remarks>
		public static void InitScreenDPI()
		{
			System.Drawing.Graphics grfx = System.Drawing.Graphics.FromHwnd((IntPtr)0);
			if (grfx != null)
			{
				Measurements.screenDpiX = grfx.DpiX;
				Measurements.screenDpiY = grfx.DpiY;
			}
		}

		/// <summary>
		/// Calculate the number of units per inch for a given GraphicsUnit.
		/// </summary>
		/// <param name="unit">GraphicsUnit to test</param>
		/// <returns>Number of logical units per inch for the given GraphicsUnit</returns>
		public static float UnitsPerInch(GraphicsUnit unit)
		{
			float unitsPerInch = 1.0f;

			switch (unit)
			{
				case GraphicsUnit.Inch:
					return 1.0f;

				case GraphicsUnit.Millimeter:
					return 25.4f;

				case GraphicsUnit.Point:
					return 72.0f;

				case GraphicsUnit.Document:
					return 300.0f;

				case GraphicsUnit.Display:
					return 75.0f;
			}

			return unitsPerInch;
		}

		/// <summary>
		/// Converts the given value from one unit of measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="dpi">Device resolution (Dots Per Inch)</param>
		/// <param name="value">Value to convert</param>
		/// <returns>Converted value</returns>
		public static float Convert(GraphicsUnit fromUnits, GraphicsUnit toUnits, float dpi, float value)
		{
			float valueOut = value;
			int fromUnitsIdx;
			int toUnitsIdx;

			if (fromUnits != toUnits)
			{
				float intermediateValue;
				GraphicsUnit intermediateUnits;

				if (fromUnits == GraphicsUnit.Pixel)
				{
					// Convert incoming value to inches first
					intermediateUnits = GraphicsUnit.Inch;
					intermediateValue = value / dpi;
				}
				else
				{
					// Intermediate value is the same as incoming value
					intermediateValue = value;
					intermediateUnits = fromUnits;
				}

				if (intermediateUnits == toUnits)
				{
					// How lucky! The conversion is already done.
					valueOut = intermediateValue;
				}
				else
				{
					if (toUnits == GraphicsUnit.Pixel)
					{
						// Convert intermediate value to inches using the conversion
						// table, and then convert to pixels
						if (intermediateUnits != GraphicsUnit.Inch)
						{
							fromUnitsIdx = Measurements.GraphicsUnitIndex(intermediateUnits);
							toUnitsIdx = Measurements.GraphicsUnitIndex(GraphicsUnit.Inch);
							intermediateValue = intermediateValue * conversionRatios[fromUnitsIdx,toUnitsIdx];
						}
						valueOut = intermediateValue * dpi;
					}
					else
					{
						// Convert intermediate value to desired unit of measure using
						// the conversion table
						fromUnitsIdx = Measurements.GraphicsUnitIndex(intermediateUnits);
						toUnitsIdx = Measurements.GraphicsUnitIndex(toUnits);
						valueOut = intermediateValue * conversionRatios[fromUnitsIdx,toUnitsIdx];
					}
				}
			}

			return valueOut;
		}

		/// <summary>
		/// Converts the given value from one unit of measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="value">Value to convert</param>
		/// <returns>Converted value</returns>
		public static float Convert(GraphicsUnit fromUnits, GraphicsUnit toUnits, float value)
		{
			return Measurements.Convert(fromUnits, toUnits, Measurements.screenDpiX, value);
		}

		/// <summary>
		/// Converts the given value from one unit of measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="grfx">Graphics context object for converting device units</param>
		/// <param name="value">Value to convert</param>
		/// <returns>Converted value</returns>
		public static float Convert(GraphicsUnit fromUnits, GraphicsUnit toUnits, Graphics grfx, float value)
		{
			float dpi = Measurements.screenDpiX;
			if (grfx != null)
			{
				dpi = grfx.DpiX;
			}
			return Measurements.Convert(fromUnits, toUnits, dpi, value);
		}

		/// <summary>
		/// Converts the given PointF from one unit of measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="value">PointF to convert</param>
		/// <returns>Converted value</returns>
		public static System.Drawing.PointF Convert(GraphicsUnit fromUnits, GraphicsUnit toUnits, System.Drawing.PointF value)
		{
			float dpiX = Measurements.screenDpiX;
			float dpiY = Measurements.screenDpiY;
			float x = Measurements.Convert(fromUnits, toUnits, dpiX, value.X);
			float y = Measurements.Convert(fromUnits, toUnits, dpiY, value.Y);
			return new System.Drawing.PointF(x,y);
		}

		/// <summary>
		/// Converts the given PointF from one unit of measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="grfx">Graphics context object for converting device units</param>
		/// <param name="value">PointF to convert</param>
		/// <returns>Converted value</returns>
		public static System.Drawing.PointF Convert(GraphicsUnit fromUnits, GraphicsUnit toUnits, Graphics grfx, System.Drawing.PointF value)
		{
			float dpiX = Measurements.screenDpiX;
			float dpiY = Measurements.screenDpiY;
			if (grfx != null)
			{
				dpiX = grfx.DpiX;
				dpiY = grfx.DpiY;
			}
			float x = Measurements.Convert(fromUnits, toUnits, dpiX, value.X);
			float y = Measurements.Convert(fromUnits, toUnits, dpiY, value.Y);
			return new System.Drawing.PointF(x,y);
		}

		/// <summary>
		/// Converts the given SizeF from one unit of measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="value">SizeF to convert</param>
		/// <returns>Converted value</returns>
		public static System.Drawing.SizeF Convert(GraphicsUnit fromUnits, GraphicsUnit toUnits, System.Drawing.SizeF value)
		{
			float dpiX = Measurements.screenDpiX;
			float dpiY = Measurements.screenDpiY;
			float width = Measurements.Convert(fromUnits, toUnits, dpiX, value.Width);
			float height = Measurements.Convert(fromUnits, toUnits, dpiY, value.Height);
			return new System.Drawing.SizeF(width,height);
		}

		/// <summary>
		/// Converts the given SizeF from one unit of measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="grfx">Graphics context object for converting device units</param>
		/// <param name="value">SizeF to convert</param>
		/// <returns>Converted value</returns>
		public static System.Drawing.SizeF Convert(GraphicsUnit fromUnits, GraphicsUnit toUnits, Graphics grfx, System.Drawing.SizeF value)
		{
			float dpiX = Measurements.screenDpiX;
			float dpiY = Measurements.screenDpiY;
			if (grfx != null)
			{
				dpiX = grfx.DpiX;
				dpiY = grfx.DpiY;
			}
			float width = Measurements.Convert(fromUnits, toUnits, dpiX, value.Width);
			float height = Measurements.Convert(fromUnits, toUnits, dpiY, value.Height);
			return new System.Drawing.SizeF(width,height);
		}

		/// <summary>
		/// Converts the given RectangleF from one unit of measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="value">RectangleF to convert</param>
		/// <returns>Converted value</returns>
		public static System.Drawing.RectangleF Convert(GraphicsUnit fromUnits, GraphicsUnit toUnits, System.Drawing.RectangleF value)
		{
			float dpiX = Measurements.screenDpiX;
			float dpiY = Measurements.screenDpiY;
			float x = Measurements.Convert(fromUnits, toUnits, dpiX, value.X);
			float y = Measurements.Convert(fromUnits, toUnits, dpiY, value.Y);
			float width = Measurements.Convert(fromUnits, toUnits, dpiX, value.Width);
			float height = Measurements.Convert(fromUnits, toUnits, dpiY, value.Height);
			return new System.Drawing.RectangleF(x, y, width, height);
		}

		/// <summary>
		/// Converts the given RectangleF from one unit of measure to another.
		/// </summary>
		/// <param name="fromUnits">Units to convert from</param>
		/// <param name="toUnits">Units to convert to</param>
		/// <param name="grfx">Graphics context object for converting device units</param>
		/// <param name="value">RectangleF to convert</param>
		/// <returns>Converted value</returns>
		public static System.Drawing.RectangleF Convert(GraphicsUnit fromUnits, GraphicsUnit toUnits, Graphics grfx, System.Drawing.RectangleF value)
		{
			float dpiX = Measurements.screenDpiX;
			float dpiY = Measurements.screenDpiY;
			if (grfx != null)
			{
				dpiX = grfx.DpiX;
				dpiY = grfx.DpiY;
			}
			float x = Measurements.Convert(fromUnits, toUnits, dpiX, value.X);
			float y = Measurements.Convert(fromUnits, toUnits, dpiY, value.Y);
			float width = Measurements.Convert(fromUnits, toUnits, dpiX, value.Width);
			float height = Measurements.Convert(fromUnits, toUnits, dpiY, value.Height);
			return new System.Drawing.RectangleF(x, y, width, height);
		}
	}

	/// <summary>
	/// Interface to objects that contain logical units.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The purpose of this interface is to provide a way to convert logical units
	/// contained by an object to be converted from one unit of measure to another.
	/// </para>
	/// </remarks>
	public interface ILogicalUnitContainer
	{
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
		void ConvertLogicalUnits(GraphicsUnit fromUnits, GraphicsUnit toUnits, Graphics grfx);

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
		void ConvertLogicalScale(float fromScale, float toScale);
	}

	/// <summary>
	/// 
	/// </summary>
	public class LogicalUnitsEventArgs : System.EventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oldUnits"></param>
		/// <param name="newUnits"></param>
		public LogicalUnitsEventArgs(GraphicsUnit oldUnits, GraphicsUnit newUnits)
		{
			this.oldUnits = oldUnits;
			this.newUnits = newUnits;
		}

		/// <summary>
		/// 
		/// </summary>
		public GraphicsUnit OldUnits
		{
			get
			{
				return this.oldUnits;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public GraphicsUnit NewUnits
		{
			get
			{
				return this.newUnits;
			}
		}

		private GraphicsUnit oldUnits;
		private GraphicsUnit newUnits;
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void LogicalUnitsEventHandler(object sender, LogicalUnitsEventArgs evtArgs);

	/// <summary>
	/// 
	/// </summary>
	public class LogicalScaleEventArgs : System.EventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oldScale"></param>
		/// <param name="newScale"></param>
		public LogicalScaleEventArgs(float oldScale, float newScale)
		{
			this.oldScale = oldScale;
			this.newScale = newScale;
		}

		/// <summary>
		/// 
		/// </summary>
		public float OldScale
		{
			get
			{
				return this.oldScale;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float NewScale
		{
			get
			{
				return this.newScale;
			}
		}

		private float oldScale;
		private float newScale;
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void LogicalScaleEventHandler(object sender, LogicalScaleEventArgs evtArgs);
}
