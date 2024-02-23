using Connections.Interface;

namespace Connections.USB
{
    /// <summary>
    /// This struct is a container for the data being recieved from the USB port.
    /// </summary>
    public struct PortReadParams_USB : IPortReadParams
    {
        #region Accessors
        public byte[] Buffer { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
        #endregion

        #region Constructor
        public PortReadParams_USB(byte[] buffer, int offset, int count)
        {
            Buffer = buffer;
            Offset = offset;
            Count = count;
        }
        #endregion

        #region Static Methods
        public static PortReadParams_USB Create(byte[] buffer, int offset, int count)
        {
            return new PortReadParams_USB(buffer, offset, count);
        }
        #endregion
    }
}
