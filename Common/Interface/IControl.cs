using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Interface
{
    public interface IControl : IDisposable
    {
        #region Accessors
        bool Visible { get; set; }
        Size Size { get; }
        int Width { get; set; }
        int Height { get; set; }
        Control Parent { get; set; }
        Rectangle Bounds { get; set; }
        #endregion

        #region Methods
        void BringToFront();
        void SendToBack();
        #endregion
    }
}
