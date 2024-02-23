using Common;
using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace Datam.WinForms.Interface
{
    /// <summary>
    /// This interface contains all relevant data for allowing the editing of a User Interface
    /// from one language to another.
    /// </summary>
    public interface IForm : IIdentifiable, IDisposable
    {
        #region Events
        event ControlEventHandler ControlAdded;
        event EventHandler Click;
        #endregion /Events

        #region Accessors
        String Name { get; }
        Boolean Visible { get; set; }
        ControlCollection Controls { get; }
        Control ActiveControl { get; set; }
        Boolean DoubleBuffered { get; }
        Boolean InvokeRequired { get; }
        Boolean IsHandleCreated { get; }
        Size Size { get; }
        Size ClientSize { get; }
        Cursor Cursor { get; set; }
        #endregion /Accessors

        #region Methods

        #region Child
        Boolean Focus();
        void CreateHandle();
        Object Invoke(Delegate method);
        Boolean IsDesignerHosted(Control control);
        void PerformLayout();
        #endregion /Child

        #region Base
        void OnLoad_Base(EventArgs e);
        void OnShown_Base(EventArgs e);
        void OnSizeChanged_Base(EventArgs e);
        void SetVisibleCore_Base(Boolean visible);
        void OnHandleCreated_Base(EventArgs e);
        void Dispose_Base();
        void Dispose_Base(Boolean disposing);
        #endregion /Base

        #endregion /Methods
    }
}