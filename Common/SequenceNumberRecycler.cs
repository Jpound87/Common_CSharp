using Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class SequenceNumberRecycler : ISequenceNumberGenerator
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

        #region Constants
        private const UInt64 DEFAULT_START_SEQUENCE_NUMBER = 52753;
        #endregion /Constants

        #region Readonly
        private readonly SortedSet<UInt64> SequenceNumbers = new();
        #endregion /Readonly

        #region Accessors
        private UInt64 sequenceNumber;
        private readonly UInt64 sequenceNumber_lowest;
        public UInt64 SequenceNumber
        {
            get
            {
                lock(SequenceNumbers)
                {
                    if(!SequenceNumbers.Any())
                    {// If no one has recycled anything then I suppose we need to make more garbage.
                        SequenceNumbers.Add(sequenceNumber++);
                    }
                    return SequenceNumbers.First();
                }
            }
        }
        #endregion /Accessors

        #region Constructor
        public SequenceNumberRecycler(UInt64 startingSequenceNumber = DEFAULT_START_SEQUENCE_NUMBER) 
        {
            sequenceNumber_lowest = startingSequenceNumber;
            sequenceNumber = startingSequenceNumber;
        }
        #endregion /Constructor

        #region Return
        public void Return(UInt64 returnValue)
        {
            if (returnValue < sequenceNumber_lowest)
            {
                lock (SequenceNumbers)
                {
                    SequenceNumbers.Add(returnValue);
                }
            }
        }
        public void Return(params UInt64[] returnValues)
        {
            lock (SequenceNumbers)
            {
                SequenceNumbers.UnionWith(returnValues);
            }
        }
        #endregion /Return
    }
}
