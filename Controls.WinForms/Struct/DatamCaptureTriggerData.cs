using System;

namespace Datam.WinForms.Struct
{
    internal struct DatamCaptureTriggerData
    {
        #region Set Points
        internal Double SetPoint1 { get; set; } = Double.MaxValue;
        internal Double SetPoint2 { get; set; } = Double.MinValue;
        #endregion /Set Points

        #region Globals
        /// <summary>
        /// Name of variable used for trigger.
        /// </summary>
        internal String Variable_Name;
        /// <summary>
        /// The name of the capture condition.
        /// </summary>
        internal String CaptureCondition;// TODO: update to enum
        #endregion /Globals

        #region Constructor
        public DatamCaptureTriggerData()
        {

        }
        #endregion /Constructor
    }
}
