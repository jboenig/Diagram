using System;
using System.Resources;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace Syncfusion.Windows.Forms.Diagram
{
	/// <summary>
	/// Summary description for Resources.
	/// </summary>
	public class Resources
	{
		private static System.Resources.ResourceManager textureMgr = new System.Resources.ResourceManager("Syncfusion.Windows.Forms.Diagram.Resources.Textures", typeof(Syncfusion.Windows.Forms.Diagram.Resources).Assembly);
		private static System.Resources.ResourceManager stringMgr = new System.Resources.ResourceManager("Syncfusion.Windows.Forms.Diagram.Resources.Strings", typeof(Syncfusion.Windows.Forms.Diagram.Resources).Assembly);
		private static System.Resources.ResourceManager smallImageMgr = new System.Resources.ResourceManager("Syncfusion.Windows.Forms.Diagram.Resources.SmallImages", typeof(Syncfusion.Windows.Forms.Diagram.Resources).Assembly);
		private static System.Windows.Forms.ImageList smallImages = null;

		/// <summary>
		/// 
		/// </summary>
		public static ImageList SmallImages
		{
			get
			{
				if (Resources.smallImages == null)
				{
					Resources.smallImages = new ImageList();
					Resources.smallImages.Images.Add((Image) Resources.smallImageMgr.GetObject("SmallImages.Select"));
					Resources.smallImages.Images.Add((Image) Resources.smallImageMgr.GetObject("SmallImages.Pan"));
					Resources.smallImages.Images.Add((Image) Resources.smallImageMgr.GetObject("SmallImages.Zoom"));
				}

				return Resources.smallImages;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public class Strings
		{
			/// <summary>
			/// 
			/// </summary>
			public class Toolnames
			{
				/// <summary>
				/// 
				/// </summary>
				/// <param name="toolResID"></param>
				/// <returns></returns>
				public static string Get(string toolResID)
				{
					return Resources.stringMgr.GetString("Toolnames." + toolResID);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public class CommandDescriptions
			{
				/// <summary>
				/// 
				/// </summary>
				/// <param name="cmdName"></param>
				/// <returns></returns>
				public static string Get(string cmdName)
				{
					return Resources.stringMgr.GetString("CommandDescription." + cmdName);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public class Cursors
		{
			static string cursorNS = @"Syncfusion.Windows.Forms.Diagram.Resources.";

			/// <summary>
			/// 
			/// </summary>
			static Cursor cursorPanReady = null;

			/// <summary>
			/// 
			/// </summary>
			static Cursor cursorPanning = null;

			/// <summary>
			/// 
			/// </summary>
			static Cursor cursorZoom = null;

			/// <summary>
			/// 
			/// </summary>
			static Cursor cursorConnect = null;

			/// <summary>
			/// 
			/// </summary>
			static Cursor cursorRotateReady = null;

			/// <summary>
			/// 
			/// </summary>
			static Cursor cursorRotate = null;

			/// <summary>
			/// 
			/// </summary>
			static Cursor cursorInsertVertex = null;

			/// <summary>
			/// 
			/// </summary>
			static Cursor cursorEditVertex = null;

			/// <summary>
			/// 
			/// </summary>
			static Cursor cursorDeleteVertex = null;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="cursorName"></param>
			/// <returns></returns>
			protected static Cursor GetCursor(string cursorName)
			{
				Cursor cursor = null;

				try
				{
					Type type = typeof(Syncfusion.Windows.Forms.Diagram.Resources);
					Stream stream = type.Module.Assembly.GetManifestResourceStream(cursorNS + cursorName);
					cursor = new Cursor(stream);
				}
				catch(System.Exception exception)
				{
					MessageBox.Show(exception.Message);
					throw exception;
				}  

				return cursor;
			}

			/// <summary>
			/// 
			/// </summary>
			public static Cursor PanReady
			{
				get
				{
					if (Cursors.cursorPanReady == null)
					{
						Cursors.cursorPanReady = GetCursor(@"PanReady.cur");
					}
					return Cursors.cursorPanReady;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public static Cursor Panning
			{
				get
				{
					if (Cursors.cursorPanning == null)
					{
						Cursors.cursorPanning = GetCursor(@"Panning.cur");
					}
					return Cursors.cursorPanning;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public static Cursor Zoom
			{
				get
				{
					if (Cursors.cursorZoom == null)
					{
						Cursors.cursorZoom = GetCursor(@"Zoom.cur");
					}
					return Cursors.cursorZoom;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public static Cursor Connect
			{
				get
				{
					if (Cursors.cursorConnect == null)
					{
						Cursors.cursorConnect = GetCursor(@"Connect.cur");
					}
					return Cursors.cursorConnect;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public static Cursor RotateReady
			{
				get
				{
					if (Cursors.cursorRotateReady == null)
					{
						Cursors.cursorRotateReady = GetCursor(@"RotateReady.cur");
					}
					return Cursors.cursorRotateReady;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public static Cursor Rotate
			{
				get
				{
					if (Cursors.cursorRotate == null)
					{
						Cursors.cursorRotate = GetCursor(@"Rotate.cur");
					}
					return Cursors.cursorRotate;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public static Cursor InsertVertex
			{
				get
				{
					if (Cursors.cursorInsertVertex == null)
					{
						Cursors.cursorInsertVertex = GetCursor(@"InsertVertex.cur");
					}
					return Cursors.cursorInsertVertex;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public static Cursor EditVertex
			{
				get
				{
					if (Cursors.cursorEditVertex == null)
					{
						Cursors.cursorEditVertex = GetCursor(@"EditVertex.cur");
					}
					return Cursors.cursorEditVertex;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public static Cursor DeleteVertex
			{
				get
				{
					if (Cursors.cursorDeleteVertex == null)
					{
						Cursors.cursorDeleteVertex = GetCursor(@"DeleteVertex.cur");
					}
					return Cursors.cursorDeleteVertex;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public class Textures
		{
			/// <summary>
			/// 
			/// </summary>
			public static System.Drawing.Image CheckerBoard
			{
				get
				{
					return (System.Drawing.Image) Resources.textureMgr.GetObject("CheckerBoard");
				}
			}
		}
	}
}
