using Common.Constant;
using Parameters.Interface;
using System;

namespace Datam.WinForms.Interface
{
    public interface IDatamVariableCaptureData
    {
        #region Valid
        /// <summary>
        /// Used to set this as active.
        /// </summary>
        bool Valid { get; set; }
        #endregion /Valid

        #region Plot
        bool Visible { get; set; }
        uint Index { get; set; }
        PlotAxis Axis { get; set; }
        void SetAxis(PlotAxis axis);
        #endregion /Plot

        #region Variable
        IParameter Variable { get; set; }

        String Name { get; }
        String FullName { get; }
        String Type { get; }
        String AddressType_String { get; }
        #endregion /Variable

        #region Data
        int Length { get; }
        public Double this[uint i] { get; }

        void SetRawData(Double[] rawData);

        Double Minimum { get; }
        Double Maximum { get; }
        Double Average { get; }
        #endregion /Data

        #region Control
        void Deactivate();
        void SetVisibility(bool visible = true);
        #endregion /Control
    }
}
