using Common.Constant;
using System;

namespace Common.Utility
{
    public class Utility_BinaryFormatter
    {
        #region Format
        public static String FormatBinary(int value, Endianess endianess = Endianess.Little, int length = 0)
        {
            string valueBinaryStr = Convert.ToString(value, 2);
            if(valueBinaryStr.Length < length)
            {// Add padding
                switch(endianess)
                {
                    case Endianess.Little:
                        valueBinaryStr.PadLeft(length);
                        break;
                    case Endianess.Big:
                        valueBinaryStr.PadRight(length);
                        break;
                }
            }
            else if(length != 0 && valueBinaryStr.Length > length)
            {
                throw new Exception("The binary value string does not fit in the given length!");
            }
            return valueBinaryStr;
        }

        public static String FormatBinary(uint value, Endianess endianess = Endianess.Little, int length = 0)
        {
            string valueBinaryStr = Convert.ToString(value, 2);
            if (valueBinaryStr.Length < length)
            {// Add padding
                switch (endianess)
                {
                    case Endianess.Little:
                        valueBinaryStr.PadLeft(length);
                        break;
                    case Endianess.Big:
                        valueBinaryStr.PadRight(length);
                        break;
                }
            }
            else if (length != 0 && valueBinaryStr.Length > length)
            {
                throw new Exception("The binary value string does not fit in the given length!");
            }
            return valueBinaryStr;
        }
        #endregion /Format
    }
}
