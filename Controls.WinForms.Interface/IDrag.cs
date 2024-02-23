using Parameters.Interface;
using System;

namespace Datam.WinForms.Interface
{
    public interface IDrag
    {
        #region Events
        event EventHandler Click_Metadata;
        #endregion /Events

        #region Accessors
        Boolean InDrag { get; set; }
        IParameter Parameter { get; set; }//TODO: IDrag with Parameter
        #endregion /Accessors

        #region Methods
        void SelectControl();
        #endregion /Methods
    }
}
