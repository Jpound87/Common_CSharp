using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Base
{ 
    [ToolboxItem(false)]
    public class SplitterBase : Control
    {
        #region Windows
        protected override void WndProc(ref Message m)
        {
            // eat the WM_MOUSEACTIVATE message
            if (m.Msg == (int)Win32.Msgs.WM_MOUSEACTIVATE)
                return;

            base.WndProc(ref m);
        }
        #endregion

        #region Accessors
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set
            {
                SuspendLayout();
                base.Dock = value;

                if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                    Width = SplitterSize;
                else if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
                    Height = SplitterSize;
                else
                    Bounds = Rectangle.Empty;

                if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                    Cursor = Cursors.VSplit;
                else if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
                    Cursor = Cursors.HSplit;
                else
                    Cursor = Cursors.Default;

                ResumeLayout();
            }
        }

        protected virtual int SplitterSize
        {
            get { return 0; }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            StartDrag();
        }
        #endregion

        #region Constructor
        public SplitterBase()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
        #endregion

        #region Methods

        protected virtual void StartDrag()
        {
        }
        #endregion
    }
}
