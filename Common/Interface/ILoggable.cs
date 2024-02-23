using System;

namespace Common.Interface
{
    public interface ILoggable : IIdentifiable
    {
        #region Accessors
        String LogEntry { get; set; }
        #endregion /Accessors
    }
}
