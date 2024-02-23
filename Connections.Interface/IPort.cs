using System;

namespace Connections.Interface
{
    public interface IPort : IDisposable
    {
        #region Events
        event EventHandler<IEventArgs_Request> RequestSent;
        #endregion

        #region Accessors
        bool IsOpen { get; }
        #endregion

        #region Methods
        void Write(IPortWriteParams portWriteParams);
        int Read(IPortReadParams portReadParams);
        void Close();
        #endregion
    }
}
