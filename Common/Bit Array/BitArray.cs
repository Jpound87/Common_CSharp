using Common.Base;
using Common.Constant;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Bit_Array
{
    public class BitArray : Dispose_Base, IBitArray, IValidate
    {
        #region Identity
        new public const String ClassName = nameof(BitArray);
        public override String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Readonly

        #region Bits

        public UInt32[] Addressess { get; private set; }

        #endregion /Bits

        #endregion /Readonly

        #region Globals

        #region Validity
        public Boolean Valid { get; set; }
        #endregion /Validity

        #region Word

        #region Size
        private readonly UInt32 wordLength;
        public Int32 Length
        {
            get
            {
                return Convert.ToInt32(wordLength);
            }
        }

        private readonly UInt32 arrayIndexMax;
        public WordSize WordSize { get; private set; }
        #endregion /Size

        #region Endianess
        public Endianess Endianess { get; private set; }
        #endregion /Endianess

        #region Value
        private readonly Boolean[] bitArray;
        public Boolean[] Bits
        {
            get
            {
                bool[] _bitArray = new bool[bitArray.Length];
                lock (bitArray)
                {
                    switch (Endianess)
                    {
                        case Endianess.Big:
                            Array.Copy(bitArray, _bitArray, bitArray.Length);
                            break;
                        case Endianess.Little:
                            Array.Copy(bitArray.Reverse().ToArray(), _bitArray, bitArray.Length);
                            break;
                    }
                }
                return _bitArray;
            }
        }
        public UInt32 Value_Uint { get; private set; }
        private String binaryString = String.Empty;
        public String BinaryString
        {
            get
            {
                switch (Endianess)
                {
                    case Endianess.Big:
                        return BinaryString_Big;
                    case Endianess.Little:
                        return BinaryString_Little;
                    default:
                        return String.Empty.PadLeft(Length, '0');
                }
            }
            set
            {
                lock (binaryString)
                {
                    if (binaryString != value)
                    {
                        binaryString = value;
                        switch (Endianess)
                        {
                            case Endianess.Big:
                                BinaryString_Big = value;
                                BinaryString_Little = value.Reverse();
                                break;
                            case Endianess.Little:
                                BinaryString_Big = value.Reverse();
                                BinaryString_Little = value;
                                break;
                        }
                        LoadWord();
                    }
                }
            }
        }

        public String BinaryString_Big { get; private set; }
        public String BinaryString_Little { get; private set; }
   
        #endregion /Value

        #endregion /Word

        #endregion /Globals

        #region Constructor
        public BitArray(WordSize wordSize, Endianess endianess)
        {
            WordSize = wordSize;
            Endianess = endianess;
            switch (wordSize)
            {
                case WordSize.Bit_32:
                    bitArray = new bool[32];
                    Addressess = new uint[32];
                    wordLength = 32;
                    #region Bits
                    Addressess[0] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_0_LITTLE : Tokens.WORD_BIT_0_BIG;
                    Addressess[1] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_1_LITTLE : Tokens.WORD_BIT_1_BIG;
                    Addressess[2] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_2_LITTLE : Tokens.WORD_BIT_2_BIG;
                    Addressess[3] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_3_LITTLE : Tokens.WORD_BIT_3_BIG;
                    Addressess[4] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_4_LITTLE : Tokens.WORD_BIT_4_BIG;
                    Addressess[5] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_5_LITTLE : Tokens.WORD_BIT_5_BIG;
                    Addressess[6] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_6_LITTLE : Tokens.WORD_BIT_6_BIG;
                    Addressess[7] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_7_LITTLE : Tokens.WORD_BIT_7_BIG;
                    Addressess[8] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_8_LITTLE : Tokens.WORD_BIT_8_BIG;
                    Addressess[9] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_9_LITTLE : Tokens.WORD_BIT_9_BIG;
                    Addressess[10] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_10_LITTLE : Tokens.WORD_BIT_10_BIG;
                    Addressess[11] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_11_LITTLE : Tokens.WORD_BIT_11_BIG;
                    Addressess[12] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_12_LITTLE : Tokens.WORD_BIT_12_BIG;
                    Addressess[13] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_13_LITTLE : Tokens.WORD_BIT_13_BIG;
                    Addressess[14] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_14_LITTLE : Tokens.WORD_BIT_14_BIG;
                    Addressess[15] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_15_LITTLE : Tokens.WORD_BIT_15_BIG;
                    Addressess[16] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_16_LITTLE : Tokens.WORD_BIT_16_BIG;
                    Addressess[17] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_17_LITTLE : Tokens.WORD_BIT_17_BIG;
                    Addressess[18] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_18_LITTLE : Tokens.WORD_BIT_18_BIG;
                    Addressess[19] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_19_LITTLE : Tokens.WORD_BIT_19_BIG;
                    Addressess[20] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_20_LITTLE : Tokens.WORD_BIT_20_BIG;
                    Addressess[21] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_21_LITTLE : Tokens.WORD_BIT_21_BIG;
                    Addressess[22] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_22_LITTLE : Tokens.WORD_BIT_22_BIG;
                    Addressess[23] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_23_LITTLE : Tokens.WORD_BIT_23_BIG;
                    Addressess[24] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_24_LITTLE : Tokens.WORD_BIT_24_BIG;
                    Addressess[25] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_25_LITTLE : Tokens.WORD_BIT_25_BIG;
                    Addressess[26] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_26_LITTLE : Tokens.WORD_BIT_26_BIG;
                    Addressess[27] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_27_LITTLE : Tokens.WORD_BIT_27_BIG;
                    Addressess[28] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_28_LITTLE : Tokens.WORD_BIT_28_BIG;
                    Addressess[29] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_29_LITTLE : Tokens.WORD_BIT_29_BIG;
                    Addressess[30] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_30_LITTLE : Tokens.WORD_BIT_30_BIG;
                    Addressess[31] = Endianess == Endianess.Little ? Tokens.WORD_32_BIT_31_LITTLE : Tokens.WORD_BIT_31_BIG;
                    #endregion /Bits
                    break;
                case WordSize.Bit_16:
                    bitArray = new bool[16];
                    Addressess = new uint[16];
                    wordLength = 16;
                    #region Bits
                    Addressess[0] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_0_LITTLE : Tokens.WORD_BIT_0_BIG;
                    Addressess[1] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_1_LITTLE : Tokens.WORD_BIT_1_BIG;
                    Addressess[2] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_2_LITTLE : Tokens.WORD_BIT_2_BIG;
                    Addressess[3] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_3_LITTLE : Tokens.WORD_BIT_3_BIG;
                    Addressess[4] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_4_LITTLE : Tokens.WORD_BIT_4_BIG;
                    Addressess[5] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_5_LITTLE : Tokens.WORD_BIT_5_BIG;
                    Addressess[6] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_6_LITTLE : Tokens.WORD_BIT_6_BIG;
                    Addressess[7] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_7_LITTLE : Tokens.WORD_BIT_7_BIG;
                    Addressess[8] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_8_LITTLE : Tokens.WORD_BIT_8_BIG;
                    Addressess[9] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_9_LITTLE : Tokens.WORD_BIT_9_BIG;
                    Addressess[10] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_10_LITTLE : Tokens.WORD_BIT_10_BIG;
                    Addressess[11] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_11_LITTLE : Tokens.WORD_BIT_11_BIG;
                    Addressess[12] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_12_LITTLE : Tokens.WORD_BIT_12_BIG;
                    Addressess[13] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_13_LITTLE : Tokens.WORD_BIT_13_BIG;
                    Addressess[14] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_14_LITTLE : Tokens.WORD_BIT_14_BIG;
                    Addressess[15] = Endianess == Endianess.Little ? Tokens.WORD_16_BIT_15_LITTLE : Tokens.WORD_BIT_15_BIG;
                    #endregion /Bits
                    break;
                default:
                    bitArray = new bool[0];
                    Addressess = new uint[0];
                    wordLength = 0;
                    Valid = false;
                    break;
            }
            arrayIndexMax = wordLength - 1;
        }
        #endregion

        #region Load
        private void LoadWord()
        {
            lock (bitArray)
            {
                try
                {
                    if (String.IsNullOrEmpty(binaryString) || binaryString.Length != wordLength)
                    {
                        for (uint b = 0; b < wordLength; b++)
                        {
                            bitArray[b] = false;
                        }
                    }
                    else
                    {
                        for (int b = 0; b < wordLength; b++)
                        {
                            bitArray[b] = (binaryString[b] == '1');
                        }
                    }
                    Value_Uint = this.BuildBitWord(true);
                    Valid = true;
                }
                catch(IndexOutOfRangeException idex)
                {
                    throw idex;
                }
            }
        }
        #endregion /Load

        #region Methods

        #region Get

        public Boolean GetBit(UInt32 bitIndex)
        {
            lock (bitArray)
            {
                if (bitIndex < wordLength)
                {
                    return bitArray[Addressess[bitIndex]];
                }
                return false;// Well... it can't be true so....
            }
        }

        public virtual Boolean[] GetBits(params UInt32[] bitIndicies)
        {
            lock (bitArray)
            {
                bool[] bitValues = new bool[bitIndicies.Length];
                for (int b = 0; b < bitIndicies.Length; b++)
                {
                    if (b < bitArray.Length)
                    {
                        bitValues[b] = GetBit(bitIndicies[b]);
                    }
                }
                return bitValues;
            }
        }

        public virtual Boolean[] GetBits_Reverse(params UInt32[] bitIndicies)
        {
            lock (bitArray)
            {
                bool[] bitValues = new bool[bitIndicies.Length];
                for (int b = 0; b < bitIndicies.Length; b++)
                {
                    if (b < bitArray.Length)
                    {
                        bitValues[b] = GetBit(bitIndicies[b]);
                    }
                }
                return bitValues;
            }
        }
        #endregion /Get

        #region Set
        public UInt32 SetBit_Result(UInt32 bitIndex, Boolean state)
        {
            lock (bitArray)
            {
                SetBit(bitIndex, state);
                return this.BuildBitWord();
            }
        }

        /// <summary>
        /// This method will set a bit on non readonly words then update the word.
        /// If the bit was updated it will return true, otherwise false.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Boolean SetBit(UInt32 bitIndex, Boolean state)
        {
            lock (bitArray)
            {
                if (Valid)
                {
                    if (bitIndex < Length && GetBit(bitIndex) != state)
                    {
                        lock (bitArray)
                        {
                            bitArray[Addressess[bitIndex]] = state;
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        public UInt32 SetBits_Result(IDictionary<UInt32, Boolean> dictBitState)
        {
            lock (bitArray)
            {
                SetBits(dictBitState);
                return this.BuildBitWord();
            }
        }

        public void SetBits(IDictionary<UInt32, Boolean> dictBitState)
        {
            if (Valid)
            {
                lock (bitArray)
                {
                    foreach(var kvp in dictBitState)
                    {
                        if (kvp.Key < wordLength && bitArray[Addressess[kvp.Key]] != kvp.Value)
                        {
                            bitArray[Addressess[kvp.Key]] = kvp.Value;
                        }
                    }
                }
            }
        }

        public UInt32 SetBits_Reverse_Result(IDictionary<UInt32, Boolean> dictBitState)
        {
            lock (bitArray)
            {
                SetBits_Reverse(dictBitState);
                return this.BuildBitWord();
            }
        }

        public void SetBits_Reverse(IDictionary<UInt32, Boolean> dictBitState)
        {
            if (Valid)
            {
                lock (bitArray)
                {
                    Parallel.ForEach(dictBitState, (kvp) =>
                    {
                        if (kvp.Key < wordLength && bitArray[Addressess[arrayIndexMax - kvp.Key]] != kvp.Value)
                        {
                            bitArray[Addressess[arrayIndexMax - kvp.Key]] = kvp.Value;
                        }
                    });
                }
            }
        }
        #endregion /Set

        #endregion /Methods

        #region To String
        public override String ToString()
        {
            if (!String.IsNullOrEmpty(BinaryString))
            {
                return BinaryString;
            }
            return String.Empty.PadLeft(Length, '0');
        }
        #endregion To String
    }

    #region Extension Methods
    public static class BitArray_Extensions
    {
        #region Build Word
        /// <summary>
        /// This method assumes big endian
        /// </summary>
        /// <param name="wordSize"></param>
        /// <param name="bitArray"></param>
        /// <param name="arrayIndexMax"></param>
        /// <returns></returns>
        public static uint BuildBitWord(this IBitArray bitArray, bool ignoreValid = false)
        {
            uint uintWord = 0;
            if (bitArray.Valid || ignoreValid)
            {
                int wordFactor = 0;
                switch (bitArray.WordSize)
                {
                    case WordSize.Bit_32:
                        uintWord += bitArray.Bits[wordFactor] ? Tokens.U65536 : 0;
                        uintWord += bitArray.Bits[1 + wordFactor] ? Tokens.U131072 : 0;
                        uintWord += bitArray.Bits[2 + wordFactor] ? Tokens.U262144 : 0;
                        uintWord += bitArray.Bits[3 + wordFactor] ? Tokens.U524288 : 0;
                        uintWord += bitArray.Bits[4 + wordFactor] ? Tokens.U1048576 : 0;
                        uintWord += bitArray.Bits[5 + wordFactor] ? Tokens.U2097152 : 0;
                        uintWord += bitArray.Bits[6 + wordFactor] ? Tokens.U4194304 : 0;
                        uintWord += bitArray.Bits[7 + wordFactor] ? Tokens.U8388608 : 0;
                        uintWord += bitArray.Bits[8 + wordFactor] ? Tokens.U16777216 : 0;
                        uintWord += bitArray.Bits[9 + wordFactor] ? Tokens.U33554432 : 0;
                        uintWord += bitArray.Bits[10 + wordFactor] ? Tokens.U67108864 : 0;
                        uintWord += bitArray.Bits[11 + wordFactor] ? Tokens.U134217728 : 0;
                        uintWord += bitArray.Bits[12 + wordFactor] ? Tokens.U268435456 : 0;
                        uintWord += bitArray.Bits[13 + wordFactor] ? Tokens.U536870912 : 0;
                        uintWord += bitArray.Bits[14 + wordFactor] ? Tokens.U1073741824 : 0;
                        uintWord += bitArray.Bits[15 + wordFactor] ? Tokens.U2147483648 : 0;
                        wordFactor = Tokens.WORD_BIT_15_BIG;
                        goto case WordSize.Bit_16;
                    case WordSize.Bit_16:
                        uintWord += bitArray.Bits[0 + wordFactor] ? Tokens.U1 : 0;
                        uintWord += bitArray.Bits[1 + wordFactor] ? Tokens.U2 : 0;
                        uintWord += bitArray.Bits[2 + wordFactor] ? Tokens.U4 : 0;
                        uintWord += bitArray.Bits[3 + wordFactor] ? Tokens.U8 : 0;
                        uintWord += bitArray.Bits[4 + wordFactor] ? Tokens.U16 : 0;
                        uintWord += bitArray.Bits[5 + wordFactor] ? Tokens.U32 : 0;
                        uintWord += bitArray.Bits[6 + wordFactor] ? Tokens.U64 : 0;
                        uintWord += bitArray.Bits[7 + wordFactor] ? Tokens.U128 : 0;
                        uintWord += bitArray.Bits[8 + wordFactor] ? Tokens.U256 : 0;
                        uintWord += bitArray.Bits[9 + wordFactor] ? Tokens.U512 : 0;
                        uintWord += bitArray.Bits[10 + wordFactor] ? Tokens.U1024 : 0;
                        uintWord += bitArray.Bits[11 + wordFactor] ? Tokens.U2048 : 0;
                        uintWord += bitArray.Bits[12 + wordFactor] ? Tokens.U4096 : 0;
                        uintWord += bitArray.Bits[13 + wordFactor] ? Tokens.U8192 : 0;
                        uintWord += bitArray.Bits[14 + wordFactor] ? Tokens.U16384 : 0;
                        uintWord += bitArray.Bits[15 + wordFactor] ? Tokens.U32768 : 0;
                        break;
                }
            }
            return uintWord;
        }

        #endregion /Build Word
    }
    #endregion /Extension Methods

}
