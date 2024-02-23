using Common.Constant;
using System;
using System.Management;

namespace Connections.Extensions
{
    public static class Extensions_Connection
    {
        #region Get

        #region Device ID
        public static String GetDeviceID(this ManagementBaseObject mbo)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            return mbo.GetPropertyValue(Tokens.DEVICE_ID)?.ToString();
#pragma warning restore CA1416 // Validate platform compatibility
        }
        #endregion /Device ID

        #region Service
        public static String GetService(this ManagementBaseObject mbo)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            return mbo.GetPropertyValue(Tokens.DEVICE_ID)?.ToString();
#pragma warning restore CA1416 // Validate platform compatibility
        }
        #endregion /Service

        #endregion /Get
    }
}
