using System;

namespace Common.Interface
{
    #region ISequencedMessage Interface
    public interface ISequencedMessage
    {
        UInt64 SequenceNumber { get; }
        String Message { get; }
    }
    #endregion /ISequencedMessage Interface
}
