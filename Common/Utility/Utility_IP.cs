using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Common.Utility
{
    #region Valid IP Struct
    /// <summary>
    /// This struct allows for the scanner to seach thru 
    /// the resulting list of found devices easily
    /// </summary>
    public struct Valid_IP
    {
        #region Identity
        public const String StructName = nameof(Valid_IP);
        public String Identity
        {
            get
            {
                return StructName;
            }
        }
        #endregion /Identity

        #region Readonly
        private readonly bool _isValid;
        private readonly IPAddress _IP;
        #endregion / Readonly

        #region Accessors
        public bool IsValid
        {
            get { return _isValid; }
        }
        public IPAddress IP
        {
            get { return _IP; }
        }
        public bool IsDrive { get; set; }
        #endregion /Accessors

        #region Constructor
        public Valid_IP(bool isValid, IPAddress IP)
        {
            _isValid = isValid;
            _IP = IP;
            IsDrive = false;
        }
        #endregion
    }
    #endregion

    public static class Utility_IP
    {
        #region Identity
        public const String className = nameof(Utility_IP);
        #endregion

        #region Network Discovery
        /// <summary>
        /// This method quieries the OS to find the available network adapters  
        /// </summary>
        /// <returns>An array of ComboboxItems containing the found network adapters</returns>
        public static IPAddress[] FindAdaptorIPs()
        {
            HashSet<IPAddress> Adaptors = new HashSet<IPAddress>();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation IP in nic.GetIPProperties().UnicastAddresses)
                    {
                        Adaptors.Add(IP.Address.MapToIPv4());
                    }
                }
            }
            return Adaptors.ToArray();
        }
        #endregion
    }
}
