using Common.Utility;
using System;

namespace Connection.USB
{
    #region Conversion Utility
    public static class CovnersionRJCP
    {
        public static RJCP.IO.Ports.SerialDataReceivedEventArgs ConvertSerialDataReceivedEventArgs(this System.IO.Ports.SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            int serialDataInt = serialDataReceivedEventArgs.EventType.GetValue_Int();
            foreach (RJCP.IO.Ports.SerialData serialDataValue in Enum.GetValues(typeof(System.IO.Ports.SerialData)))
            {
                if (Convert.ToInt32(serialDataValue) == serialDataInt)
                {// If they are equal then we know its the one.
                    return new RJCP.IO.Ports.SerialDataReceivedEventArgs(serialDataValue);
                }
            }
            return default;
        }
    }
    #endregion
}
