using Common.Constant;
using System;
using System.Collections.Generic;

namespace Common.Bit_Array
{
    public interface IBitArray : IValidate
    {
        #region Accessors

        #region Size
        int Length { get; }
        WordSize WordSize { get; }
        #endregion

        #region Orientation
        Endianess Endianess { get; }
        #endregion

        #region Value
        uint[] Addressess { get; }
        /// <summary>
        /// Array of the bits in the order set in the endianess property.
        /// </summary>
        bool[] Bits { get; }
        uint Value_Uint { get; }
        /// <summary>
        /// Binary string in the format as set in the endianess property.
        /// </summary>
        String BinaryString { get; set; }
        /// <summary>
        /// Binary string in big endian format.
        /// </summary>
        String BinaryString_Big { get; }
        /// <summary>
        /// Binary string in little endian format.
        /// </summary>
        String BinaryString_Little { get; }
        #endregion /Value

        #endregion /Accessors

        #region Methods

        #region Get
        bool GetBit(uint bitIndex);
        bool[] GetBits(params uint[] bitIndicies);
        bool[] GetBits_Reverse(params uint[] bitIndicies);
        #endregion

        #region Set
        uint SetBit_Result(uint bitIndex, bool state);
        bool SetBit(uint bitIndex, bool state);
        void SetBits(IDictionary<uint, bool> dictBitState);
        uint SetBits_Reverse_Result(IDictionary<uint, bool> dictBitState);
        void SetBits_Reverse(IDictionary<uint, bool> dictBitState);
        #endregion

        #region To String
        String ToString();
        #endregion

        #endregion/Methods
    }
}
