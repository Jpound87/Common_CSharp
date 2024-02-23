using System;

namespace Common.Interface
{
    public interface ISequenceNumberGenerator : IIdentifiable
    {
        #region Accessors
        UInt64 SequenceNumber { get; }
        #endregion /Accessors
    }
}
