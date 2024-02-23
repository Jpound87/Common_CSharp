using System;

namespace Common
{
    public interface IReportError// ITattle is the name I want to call this.
    {
        #region Accessors
        bool ErrorOccured { get; }
        String LastError { get; }
        DateTime LastErrorTime { get; }
        #endregion /Accessors
    }
}
