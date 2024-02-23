using Connections.Extensions;
using Connections.USB.Extensions;
using Connections.USB.Interface;
using System;
using System.Management;

namespace Connections.USB
{
    /// <summary>
    /// 
    /// </summary>
    public struct PortData_USB : IPortData_USB
    {
        #region Accessors
        public bool Valid { get; private set; }
        private readonly String comPort;
        public String ComPort 
        { 
            get
            {
                return comPort;
            }
        }
        public String DeviceID { get; private set; }
        public String Service { get; private set; }
        #endregion /Accessors

        #region Constructor
        public PortData_USB(ManagementBaseObject mbo)
        {
            Valid = mbo.CheckDeviceUSB(out comPort);
            DeviceID = mbo.GetDeviceID();
            Service = mbo.GetService();
        }

        public PortData_USB(String comPort, ManagementBaseObject mbo)
        {
            Valid = true;// We assume....
            this.comPort = comPort;
            DeviceID = mbo.GetDeviceID();
            Service = mbo.GetService();
        }
        #endregion /Constructor
    }
}
