using Common.Constant;
using Connections.USB.Interface;
using System;
using System.Management;

namespace Connections.USB.Extensions
{
    public static class Extensions_USB
    {
        #region Identity
        private const string FormName = nameof(Extensions_USB);
        #endregion

        #region Check
        public static bool CheckDeviceUSB(this ManagementBaseObject mbo, out String comPort)
        {
            comPort = mbo.GetPropertyValue(Tokens.NAME)?.ToString();
            try
            {
                if (comPort != null)
                {
                    if (comPort.Contains(Tokens.COM) && comPort.Contains('(') && comPort.Contains(')'))
                    {// Its a device such as we are looking for, specifically a com port.
                        comPort = comPort.Split('(')[1].Split(')')[0];
                        return true;
                    }
                }
            }
            catch
            {

            }
            return false;
        }
        #endregion

        #region Create
        public static bool TryCreatePortData_USB(this ManagementBaseObject mbo, out IPortData_USB portData)
        {
            if(mbo.CheckDeviceUSB(out String comPort))
            {
                portData = new PortData_USB(comPort, mbo);
                return true;
            }
            portData = default;
            return false;
        }
        #endregion
    }
}
