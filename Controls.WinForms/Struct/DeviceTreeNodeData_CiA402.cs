using Common.Constant;
using Datam.WinForms.Interface;
using Devices.Interface;
using Devices.Interface.CiA402;
using System;

namespace Datam.WinForms.Struct
{
    public struct DeviceTreeNodeData_CiA402 : IDeviceTreeNodeData_CiA402
    {
        #region Identity
        public const String StructName = nameof(DeviceTreeNodeData_CiA402);

        public String Identity
        {
            get
            {
                return StructName;
            }
        }

        public string ID
        {
            get
            {
                return DeviceData.ID;
            }
        }
        #endregion

        #region Globals

        #region Valid
        public bool Valid { get; private set; }
        #endregion

        #region Hash Code
        private readonly int hashCode;
        public override int GetHashCode()
        {
            return hashCode;
        }
        #endregion

        #region Name
        public String DeviceName
        {
            get
            {
                return DeviceData.Name;
            }
        }

        public String ManufacturerName { get; private set; }
        #endregion

        #region Node Data
        public TreeNodeType TreeNodeType
        {
            get
            {
                return TreeNodeType.Device;
            }
        }
        #endregion

        #region Communicator
        public ICommunicatorDevice Device_CiA402 { get; private set; }
        public IInformation_Communicator CommunicatorData { get; private set; }
        #endregion

        #region Device
        public IInformation_Device DeviceData_CiA402 { get; private set; }
        public IInformation_Device DeviceData => DeviceData_CiA402;
        public ICommunicatorDevice Device => Device_CiA402;
        #endregion

        #region Protocol
        public ProtocolType ProtocolType { get; set; }
        #endregion

        #endregion

        #region Constructor
        public DeviceTreeNodeData_CiA402(IInformation_Communicator communicatorInfo, ICommunicatorDevice_CiA402 device, IInformation_DeviceCiA402 deviceData_CiA402)
        {
            Device_CiA402 = device;
            DeviceData_CiA402 = deviceData_CiA402;// new Information_DeviceCiA402(device.NodeID, device.Information);
            CommunicatorData = communicatorInfo;
            ProtocolType = device.ProtocolType;
            ManufacturerName = device.DisplayName;
            Valid = true;
            hashCode = device.DisplayName.GetHashCode();
        }
        #endregion /Constructor

        #region Equality
        public override bool Equals(object other)
        {
            if (other is IDeviceTreeNodeData otherData && otherData.GetHashCode() == hashCode)
            {
                return otherData.ID == ID && otherData.DeviceName == DeviceName;
            }
            return false;
        }

        public bool Equals(IDeviceData other)
        {
            if (other is IDeviceData otherData && otherData.GetHashCode() == hashCode)
            {
                return otherData.DeviceName == DeviceName;
            }
            return false;
        }
        #endregion
    }
}
