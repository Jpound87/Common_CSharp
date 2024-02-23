using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public static class Extensions_Boolean
    {
        #region Operations

        #region Logic 

        #region Or
        public static void Choose_Or(this Control caller, MethodInvoker action_True, MethodInvoker action_False, params bool[] conditions)
        {
            bool conditions_result = false;
            switch (conditions.Length)
            {
                case 0:
                    caller.Invoke(action_False);
                    return;
                case 1:
                    conditions_result = conditions[0];
                    break;
                default:
                    conditions_result = Utilites_Search.BinarySearch_Boolean(conditions, true);
                    break;
            }
            switch (conditions_result)
            {
                case true:
                    caller.Invoke(action_True);
                    return;
                default:
                    caller.Invoke(action_False);
                    return;
            }
        }
        #endregion /Or

        #region Not
        public static Boolean Not(this Boolean @bool)
        {
            return !@bool;
        }
        #endregion /Not

        #endregion /Logic 

        #endregion /Operations

        #region Bit Conversion
        /// <summary>
        /// This method takes a boolean array and converts it to a binary string.
        /// </summary>
        /// <param name="convertArray">The boolean array to be converted</param>
        /// <returns>String representation of the given bit array</returns>
        public static String BitsToString(params Boolean[] convertArray)
        {
            StringBuilder builder = new StringBuilder();
            for (int bitIndex = convertArray.Length - 1; bitIndex >= 0; bitIndex--)
            {
                builder.Append(convertArray[bitIndex] ? "1" : "0");
            }
            return builder.ToString();
        }

        /// <summary>
        /// This method takes a boolean array and converts it to a binary string.
        /// </summary>
        /// <param name="convertArray">The boolean array to be converted</param>
        /// <returns>String representation of the given bit array</returns>
        public static String BitsToString(this Boolean[] convertArray, int size)
        {
            StringBuilder builder = new StringBuilder();
            for (int bitIndex = size - 1; bitIndex >= 0; bitIndex--)
            {
                builder.Append(convertArray[bitIndex] ? "1" : "0");
            }
            return builder.ToString();
        }

        /// <summary>
        /// This method takes a string and trys  to convert it to a boolean array. 
        /// </summary>
        /// <param name="convertStr">String to try to convert to a boolean array</param>
        /// <param name="bitArray">The ouput boolean array,
        /// filled with the boolean values represented in the string
        /// if successful, otherwise with 0 past where parse was 
        /// successful.</param>
        /// <returns>True if successful, else false</returns>
        public static Boolean TryStringToBits(this String convertStr, out Boolean[] bitArray)
        {
            bitArray = new bool[convertStr.Length];
            for (int strIndex = 0; strIndex < convertStr.Length; strIndex++)
            {
                if (convertStr[strIndex] == '1')
                {
                    bitArray[strIndex] = true;
                }
                else if (convertStr[strIndex] == '0')
                {
                    bitArray[strIndex] = false;
                }
                else return false;
            }
            return true;
        }

        public static Boolean TryIntToBits(this Int32 value, out Boolean[] bitArray)
        {
            string binary = Convert.ToString(value, 2);
            return TryStringToBits(binary, out bitArray);
        }

        public static Boolean TryUIntToBits(this UInt32 value, out Boolean[] bitArray)
        {
            string binary = Convert.ToString(value, 2);
            return TryStringToBits(binary, out bitArray);
        }
        #endregion /Bit Conversion
    }
}
