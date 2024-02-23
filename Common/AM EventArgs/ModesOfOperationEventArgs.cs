using Common.Constant;
using System;

namespace Common.AM_EventArgs
{
    public class ModesOfOperationEventArgs : EventArgs
    {
        #region Accessors
        public OperatingMode OperationMode { get; private set; }
        #endregion

        #region Constructor
        public ModesOfOperationEventArgs(OperatingMode operationMode)
        {
            OperationMode = operationMode;
        }
        #endregion
    }
}
