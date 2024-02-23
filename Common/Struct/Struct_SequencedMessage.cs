using Common.Interface;
using System;

namespace Common.Struct
{
    public struct Struct_SequencedMessage : ISequencedMessage
    {
        #region Identity
        public const String ClassName = nameof(Struct_SequencedMessage);
        #endregion /Identity

        #region Accessors
        public UInt64 SequenceNumber { get; set; }
        public String Message { get; set; }
        #endregion /Accessors

        #region Constructor
        public Struct_SequencedMessage(UInt64 sequenceNumber, String message)
        {
            SequenceNumber = sequenceNumber;
            Message = message;
        }
        #endregion /Constructor
    }
}
