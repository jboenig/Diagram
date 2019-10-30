using System;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.Diagram.Interactivity
{
    public interface IMouseController
    {
        /// <summary>
        /// The cursor currently assigned to the Controller.
        /// </summary>
        Cursor Cursor
        {
            get;
        }

        /// <summary>
        /// Name of the mouse controller.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        void CancelMode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouseEventArgs"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        int HitTest(MouseEventArgs mouseEventArgs, IMouseController controller);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void MouseDown(MouseEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void MouseHover(MouseEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        void MouseHoverEnter();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void MouseHoverLeave(EventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void MouseMove(MouseEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void MouseUp(MouseEventArgs e);
    }
}
