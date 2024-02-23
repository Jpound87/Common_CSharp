using Common.Constant;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Utility
{
    #region Enumeration Arrays
    /// <summary>
    /// This class contains the relevant enumerations as arrays for iteration.
    /// </summary>
    public static class EnumArrays
    {
        #region Authorization
        public static AuthorizationLevel[] AuthorizationLevels = Enum.GetValues(typeof(AuthorizationLevel)).Cast<AuthorizationLevel>().ToArray();
        #endregion

        #region Langauges
        public static String[] GetLanguageTokens() 
        {
            List<String> languageTokenStrings = new List<string>();
            foreach(Languages value in Utility_Enums.GetEnumValuesAsArray<Languages>())
            {
                languageTokenStrings.Add(value.StringFromEnum());
            }
            return languageTokenStrings.ToArray();
        }
        #endregion
    }
    #endregion

    #region Enum Codes
    public static class EnumCodes
    {
        private static readonly TypeCode authLevel = Convert.GetTypeCode(AuthorizationLevel.All);
        public static TypeCode AuthLevel
        {
            get
            {
                return authLevel;
            }
        }


        public static T ToEnum<T>(this string data) where T : struct
        {
            if (!Enum.TryParse(data, true, out T enumVariable))
            {
                if (Enum.IsDefined(typeof(T), enumVariable))
                {
                    return enumVariable;
                }
            }

            return default;
        }

        public static T ToEnum<T>(this int data) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), data);
        }

        public static bool ToEnum<TEnum>(this int value, out TEnum enumeration)
        {
            if (typeof(TEnum).IsEnumDefined(value))
            {
                enumeration = (TEnum)(object)value;
                return true;
            }
            enumeration = default;
            return false;
        }
    }
    #endregion

    #region Enumeration Data
    public static class EnumData
    {
        #region Word Size
        public static UInt32 GetWordLength(this WordSize wordSize)
        {
            switch (wordSize)
            {
                case WordSize.Bit_1:
                    return 1;
                case WordSize.Bit_8:
                    return 8;
                case WordSize.Bit_16:
                    return 16;
                case WordSize.Bit_24:
                    return 24;
                case WordSize.Bit_32:
                    return 32;
                case WordSize.Bit_48:
                    return 48;
                case WordSize.Bit_56:
                    return 56;
                case WordSize.Bit_64:
                    return 64;
                case WordSize.Variable:
                default:
                    return 0;
            }
        }
        #endregion
    }
    #endregion

    #region Utility
    public static class Utility_Enums
    {
        #region Identity
        public const String ClassName = nameof(Utility_Enums);
        #endregion

        #region Enumeration Manipulation
        //
        public static Int32 GetCount<T>() where T : Enum
        {
            return Enum.GetNames(typeof(Themes)).Length;
        }

        public static String GetValue_String(this Enum e)
        {
            return e.GetValue_Int().ToString();
        }

        public static Int32 GetValue_Int(this Enum e)
        {
            object result = Convert.ChangeType(e, e.GetTypeCode());
            return Convert.ToInt32(result);
        }

        public static String GetValue_Int_String(this Enum e)
        {
            object result = Convert.ChangeType(e, e.GetTypeCode());
            return Convert.ToInt32(result).ToString();
        }

        public static T[] GetEnumValuesAsArray<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>() as T[];
        }
        #endregion /Enumeration Manipulation

        #region Determination
        public static T DetermineFromInt<T>(int enumValue) where T : Enum
        {
            foreach (T @enum in Enum.GetValues(typeof(T)))
            {
                if (@enum.GetValue_Int() == enumValue)
                {// If they are equal then we know its the one.
                    return @enum;
                }
            }
            return default;
        }
        #endregion /Determination
    }
    #endregion /Utility

}
