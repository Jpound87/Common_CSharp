using System;

namespace Common
{
    public struct AccessLevel_Data : IIdentifiable
    {
        #region Identity
        public const String StructName = nameof(AccessLevel_Data);
        public String Identity
        {
            get
            {
                return StructName;
            }
        }
        #endregion

        #region Readonly
        public readonly bool[] BitArray;
        #endregion /Readonly

        #region Accessors
        public bool All => true;
        public bool Safety
        {
            get
            {
                return BitArray[0];
            }
        }

        public bool Standard
        {
            get
            {
                return BitArray[1];
            }
        }

        public bool Advanced
        {
            get
            {
                return BitArray[2];
            }
        }

        public bool Authorized //Engineering Mode
        {
            get
            {
                return BitArray[3];
            }
        }
        public bool Allied //Development
        {
            get
            {
                return BitArray[4];
            }
        }
        #endregion

        #region Constructor
        public AccessLevel_Data(bool[] bitArray)
        {
            BitArray = bitArray;
        }
        #endregion /Constructor
    }
}
