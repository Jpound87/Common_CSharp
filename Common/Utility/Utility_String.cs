using Common.Extensions;
using System;

namespace Common.Utility
{
    #region String Invoke
    public delegate void Delegate_String(String invokeString);
    #endregion

    public static class Utility_String
    {
        #region Generation
        public static void GenerateNewNameNotInArray_SuffixNumeric(ref string startName, string[] existingNames, int startIndex = 0)
        {
            string newName = startName;
            int n = startIndex;
            uint suffix = 0;
            while (n < existingNames.Length)
            {
                if (existingNames[n] == newName)
                {
                    newName = $"{startName}({suffix++})";
                    n = 0;// Unfortuantely this means we must start again
                }
                else n++;
            }
            startName = newName;
        }
        #endregion /Generation

        #region Concatonate
        public static String ConcatWithSpaces(params String[] values)
        {
            return values.ConcatWithSeperator_Char();
        }
        #endregion Concatonate
    }
}
