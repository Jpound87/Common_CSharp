namespace Connections.Interface
{
    public interface ISequencedTransmission
    {
        #region Sequence Number
        ulong SequenceNumber { get; }
        #endregion /Sequence Number

        #region Set
        void SetSequenceNumber(ulong sequenceNumber);
        #endregion /Set
    }
}
