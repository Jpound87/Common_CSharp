using Common.Interface;
using System;

namespace Common
{
    public struct SequenceNumberGenerator : ISequenceNumberGenerator
    {
        #region Identity
        public const String StructName = nameof(SequenceNumberGenerator);

        public String Identity 
        { 
            get
            {
                return StructName;
            }
        }
        #endregion /Identity

        #region Accessors
        private UInt64 sequenceNumber;
        public UInt64 SequenceNumber
        {
            get
            {
                return sequenceNumber++;
            }
        }
        #endregion /Accessors

        #region Constructor
        public SequenceNumberGenerator(ulong SequenceStart)
        {
            sequenceNumber = SequenceStart;
        }
        #endregion /Constructor
    }
}
