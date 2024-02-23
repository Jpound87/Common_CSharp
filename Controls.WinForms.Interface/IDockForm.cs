using Common;
using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Datam.WinForms.Interface
{
    /// <summary>
    /// This interface contains all relevant data to control window sizing for dock forms, and for 
    /// maintaining the connection objects required to provide the window with required infomration
    /// from attached devices
    /// </summary>
    public interface IDockForm : IDockContent, IForm, IReportError
    {
        #region Identity
        /// <summary>
        /// Should contain a reference to 'this'
        /// </summary>
        Form Self { get; }
        #endregion

        #region Events
        event EventHandler SizeChanged;
        #endregion /Events

        #region Focus
        Control ActiveControl { get; }
        #endregion
         
        #region Docking
        /// <summary>
        /// This should return the DockContent floatpane
        /// </summary>
        DockPane GetFloatPane { get; }

        /// <summary>
        /// This should return the DockContent dockstate
        /// </summary>
        DockState DockState { get; }


        Panel HostPanel { get; }
        #endregion /Docking

        #region Sizing
        Size Size { get; set; }

        Size MinSize { get; }
        /// <summary>
        /// This should contain the static or dynamicly developed minimum size for the 
        /// floating style window 
        /// </summary>
        Size MinSize_Float { get; }
        #endregion /Sizing

        #region Visibility
        bool IsHidden { get; }
        void Show();
        void Show(DockPanel dockPanel, DockState dockState);
        void Hide();
        void SendToBack();
        #endregion /Visibility
    }
}
