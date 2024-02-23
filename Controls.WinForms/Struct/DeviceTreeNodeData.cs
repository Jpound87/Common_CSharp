using Common.Constant;
using Datam.WinForms.Interface;
using Devices.Interface;
using System;

namespace Datam.WinForms.Struct
{
    public struct DeviceTreeNodeData : IDeviceTreeNodeData
    {
        #region Identity
        public const String StructName = nameof(DeviceTreeNodeData);

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

        #region Node
        public TreeNodeType TreeNodeType
        {
            get
            {
                return TreeNodeType.Device;
            }
        }
        #endregion

        #region Protocol
        public ProtocolType ProtocolType { get; set; }
        #endregion

        #region Device
        public ICommunicatorDevice Device { get; private set; }
        public IInformation_Device DeviceData { get; private set; }
        #endregion

        #endregion

        #region Constructor
        public DeviceTreeNodeData(ICommunicatorDevice device, IInformation_Device deviceData)
        {
            Device = device;
            DeviceData = deviceData;// new Information_Device(device.Information);
            ManufacturerName = device.DisplayName;
            ProtocolType = ProtocolType.None;
            Valid = true;
            hashCode = DeviceData.Name.GetHashCode(); 
        }
        #endregion

        #region Equality
        public override bool Equals(object other)
        {
            if(other is IDeviceTreeNodeData otherData && otherData.GetHashCode() == hashCode)
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
