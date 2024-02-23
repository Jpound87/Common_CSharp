using Common.Constant;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Common.Extensions
{
    #region Enumerations

    #region Message Type
    public enum MessageType
    {
        Invalid,
        Command,
        Parameter_Read,
        Parameter_Write
    }
    #endregion

    #region Parameter Type
    /// <summary>
    /// Enumeration of the DS402 defined parameter types.
    /// </summary>
    public enum ParameterType
    {
        //M - Mandatory, O - Optional by CANopen spec
        Null = 0x0000,      //
        Bool = 0x0001,      //M
        Int8 = 0x0002,      //M
        Int16 = 0x0003,     //M
        Int24 = 0x0010,     //O
        Int32 = 0x0004,     //M
        INT40 = 0x0012,     //O
        Int48 = 0x0013,     //O
        Int56 = 0x0014,     //O
        Int64 = 0x0015,     //O
        UInt8 = 0x0005,     //M
        UInt16 = 0x0006,    //M
        UInt24 = 0x0016,    //O
        UInt32 = 0x0007,    //M
        UINT40 = 0x0018,    //O
        UInt48 = 0x0019,    //O
        UInt56 = 0x001A,    //O
        UInt64 = 0x001B,    //O
        Real32 = 0x0008,    //O
        Real64 = 0x0011,    //O
        String = 0x0009,    //O
        OctetString = 0x000A, //O 
        Unicode = 0x000B,   //O 
        Domain = 0x000F,    //O
        DateTime = 0x000C,  //O
        TimeSpan = 0x000D   //O
    }
    #endregion /Parameter Type

    #endregion /Enumerations

    public static class Extensions_CiA309_3
    {
        #region Numeric

        #region General
        /// <summary>
        /// This method will determine if the DS402 type
        /// of the parameter is considered numeric
        /// </summary>
        /// <param name="type">DS402 type to be evaluated</param>
        /// <returns>True if numeric</returns>
        public static bool IsNumericType_CiA402(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                case ParameterType.Null:
                    return false;
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                case ParameterType.Real32:
                case ParameterType.Real64:
                    return true;
                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return false;
                default:
                    throw new ArgumentException($"CiA402 type {nameof(type)} not known");
            }
        }
        #endregion /General

        #region Integer
        /// <summary>
        /// This method will determine if the DS402 type
        /// of the parameter is considered numeric
        /// </summary>
        /// <param name="type">DS402 type to be evaluated</param>
        /// <returns>True if numeric</returns>
        public static bool IsIntType_CiA402(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                case ParameterType.Null:
                    return false;
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                    return true;
                case ParameterType.Real32:
                case ParameterType.Real64:

                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return false;
                default:
                    throw new ArgumentException($"CiA402 type {nameof(type)} not known");
            }
        }

        public static bool IsSignedIntType_CiA402(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                case ParameterType.Null:
                    return false;
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                    return true;
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                    return false;
                case ParameterType.Real32:
                case ParameterType.Real64:

                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return false;
                default:
                    throw new ArgumentException($"CiA402 type {nameof(type)} not known");
            }
        }

        public static bool IsUnsignedIntType_CiA402(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                case ParameterType.Null:
                    return false;
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                    return false;
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                    return true;
                case ParameterType.Real32:
                case ParameterType.Real64:

                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return false;
                default:
                    throw new ArgumentException($"CiA402 type {nameof(type)} not known");
            }
        }
        #endregion

        #region Floating Point
        /// <summary>
        /// This method will determine if the DS402 type
        /// of the parameter is considered numeric
        /// </summary>
        /// <param name="type">DS402 type to be evaluated</param>
        /// <returns>True if numeric</returns>
        public static bool IsFloatType_CiA309(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                case ParameterType.Null:
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                    return false;
                case ParameterType.Real32:
                case ParameterType.Real64:
                    return true;
                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return false;
                default:
                    throw new ArgumentException($"CiA402 type {nameof(type)} not known");
            }
        }
        #endregion

        #region Numeric Null

        /// <summary>
        /// This method will determine if the DS402 type
        /// of the parameter is considered numeric
        /// </summary>
        /// <param name="type">DS402 type to be evaluated</param>
        /// <returns>True if numeric</returns>
        public static bool IsNumericWithNullType_CiA402(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                    return false;
                case ParameterType.Null:
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                case ParameterType.Real32:
                case ParameterType.Real64:
                    return true;
                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return false;
                default:
                    throw new ArgumentException($"CiA402 type {nameof(type)} not known");
            }
        }

        #endregion

        #endregion /Numeric

        #region String

        public static bool IsStringType_CiA402(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                case ParameterType.Null:
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                case ParameterType.Real32:
                case ParameterType.Real64:
                    return false;
                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                    return true;
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return false;
                default:
                    throw new ArgumentException($"CiA402 type {nameof(type)} not known");
            }
        }
        #endregion

        #region Null
        /// <summary>
        /// This method will determine if the DS402 type
        /// of the parameter is considered numeric
        /// </summary>
        /// <param name="type">DS402 type to be evaluated</param>
        /// <returns>True if numeric</returns>
        public static bool IsNullType_CiA402(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                    return false;
                case ParameterType.Null:
                    return true;
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                case ParameterType.Real32:
                case ParameterType.Real64:
                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return false;
                default:
                    throw new ArgumentException($"CiA402 type {nameof(type)} not known");
            }
        }
        #endregion

        #region Word Size
        /// <summary>
        /// This method will determine if the DS402 type
        /// of the parameter is considered numeric
        /// </summary>
        /// <param name="type">DS402 type to be evaluated</param>
        /// <returns>True if numeric</returns>
        public static WordSize LookupWordSize_CiA402(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                case ParameterType.Null:
                    return WordSize.Bit_1;
                case ParameterType.Int8:
                    return WordSize.Bit_8;
                case ParameterType.Int16:
                    return WordSize.Bit_16;
                case ParameterType.Int24:
                    return WordSize.Bit_24;
                case ParameterType.Int32:
                    return WordSize.Bit_32;
                case ParameterType.Int48:
                    return WordSize.Bit_48;
                case ParameterType.Int56:
                    return WordSize.Bit_56;
                case ParameterType.Int64:
                    return WordSize.Bit_64;
                case ParameterType.UInt8:
                    return WordSize.Bit_8;
                case ParameterType.UInt16:
                    return WordSize.Bit_16;
                case ParameterType.UInt24:
                    return WordSize.Bit_24;
                case ParameterType.UInt32:
                    return WordSize.Bit_32;
                case ParameterType.UInt48:
                    return WordSize.Bit_48;
                case ParameterType.UInt56:
                    return WordSize.Bit_56;
                case ParameterType.UInt64:
                    return WordSize.Bit_64;
                case ParameterType.Real32:
                    return WordSize.Bit_32;
                case ParameterType.Real64:
                    return WordSize.Bit_64;
                default:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return WordSize.Variable;
            }
        }
        #endregion /Word Size

        #region Annotation

        #region Decimal
        /// <summary>
        /// This method will determine if the DS402 type
        /// of the parameter is considered numeric
        /// </summary>
        /// <param name="type">DS402 type to be evaluated</param>
        /// <returns>True if numeric</returns>
        public static bool DisplayDecimalForType(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Bool:
                case ParameterType.Null:
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                    return false;
                case ParameterType.Real32:
                case ParameterType.Real64:
                    return true;
                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    return false;
                default:
                    throw new ArgumentException($"CiA402 type {nameof(type)} not known");
            }
        }
        #endregion /Decimal

        #endregion /Annotation

        #region Type References
        private static readonly Dictionary<String, ParameterType> dictDs309TypeStr_ParamTypeEnum = new Dictionary<String, ParameterType>()
        {
            { Tokens.BOOL_309_3, ParameterType.Bool },
            { Tokens.INT8_309_3, ParameterType.Int8 },
            { Tokens.INT16_309_3, ParameterType.Int16 },
            { Tokens.INT24_309_3, ParameterType.Int24 },
            { Tokens.INT32_309_3, ParameterType.Int32 },
            { Tokens.INT40_309_3, ParameterType.INT40 },
            { Tokens.INT48_309_3, ParameterType.Int48 },
            { Tokens.INT56_309_3, ParameterType.Int56 },
            { Tokens.INT64_309_3, ParameterType.Int64 },
            { Tokens.UINT8_309_3, ParameterType.UInt8 },
            { Tokens.UINT16_309_3, ParameterType.UInt16 },
            { Tokens.UINT24_309_3, ParameterType.UInt24 },
            { Tokens.UINT32_309_3, ParameterType.UInt32 },
            { Tokens.UINT40_309_3, ParameterType.UINT40 },
            { Tokens.UINT48_309_3, ParameterType.UInt48 },
            { Tokens.UINT56_309_3, ParameterType.UInt56 },
            { Tokens.UINT64_309_3, ParameterType.UInt64 },
            { Tokens.REAL32_309_3, ParameterType.Real32 },
            { Tokens.REAL64_309_3, ParameterType.Real64 },
            { Tokens.STRING_309_3, ParameterType.String },
            { Tokens.OCTET_STRING_309_3, ParameterType.OctetString },
            { Tokens.UNICODE_309_3, ParameterType.Unicode },
            { Tokens.DOMAIN_309_3, ParameterType.Domain },
            { Tokens.DATE_TIME_309_3, ParameterType.DateTime },
            { Tokens.TIMESPAN_309_3, ParameterType.TimeSpan }
        };

        private static readonly Dictionary<ParameterType, String> dictParamTypeEnum_Ds309Str = new Dictionary<ParameterType, String>()
        {
            { ParameterType.Bool, Tokens.BOOL_309_3  },
            { ParameterType.Int8 , Tokens.INT8_309_3 },
            { ParameterType.Int16, Tokens.INT16_309_3 },
            { ParameterType.Int24, Tokens.INT24_309_3 },
            { ParameterType.Int32, Tokens.INT32_309_3 },
            { ParameterType.INT40, Tokens.INT40_309_3 },
            { ParameterType.Int48, Tokens.INT48_309_3 },
            { ParameterType.Int56, Tokens.INT56_309_3 },
            { ParameterType.Int64, Tokens.INT64_309_3 },
            { ParameterType.UInt8, Tokens.UINT8_309_3 },
            { ParameterType.UInt16, Tokens.UINT16_309_3 },
            { ParameterType.UInt24, Tokens.UINT24_309_3 },
            { ParameterType.UInt32, Tokens.UINT32_309_3 },
            { ParameterType.UINT40, Tokens.UINT40_309_3 },
            { ParameterType.UInt48, Tokens.UINT48_309_3 },
            { ParameterType.UInt56, Tokens.UINT56_309_3 },
            { ParameterType.UInt64, Tokens.UINT64_309_3 },
            { ParameterType.Real32, Tokens.REAL32_309_3 },
            { ParameterType.Real64, Tokens.REAL64_309_3 },
            { ParameterType.String, Tokens.STRING_309_3 },
            { ParameterType.OctetString, Tokens.OCTET_STRING_309_3 },
            { ParameterType.Unicode, Tokens.UNICODE_309_3 },
            { ParameterType.Domain, Tokens.DOMAIN_309_3 },
            { ParameterType.DateTime , Tokens.DATE_TIME_309_3 },
            { ParameterType.TimeSpan, Tokens.TIMESPAN_309_3}
        };

        private static readonly Dictionary<String, ParameterType> DictParamTypeStr_ParamTypeEnum = new Dictionary<String, ParameterType>()
        {
            {"0x0000", ParameterType.Null },     //M
            {"0x0001", ParameterType.Bool },     //M
            {"0x0002", ParameterType.Int8 },      //M
            {"0x0003", ParameterType.Int16 },     //M
            {"0x0010", ParameterType.Int24 },     //O
            {"0x0004", ParameterType.Int32 },     //M
            {"0x0012", ParameterType.INT40 },     //O
            {"0x0013", ParameterType.Int48 },     //O
            {"0x0014", ParameterType.Int56 },     //O
            {"0x0015", ParameterType.Int64 },     //O
            {"0x0005", ParameterType.UInt8 },     //M
            {"0x0006", ParameterType.UInt16 },    //M
            {"0x0016", ParameterType.UInt24 },    //O
            {"0x0007", ParameterType.UInt32 },    //M
            {"0x0018", ParameterType.UINT40 },    //O
            {"0x0019", ParameterType.UInt48 },    //O
            {"0x001A", ParameterType.UInt56 },    //O
            {"0x001a", ParameterType.UInt56 },    //O
            {"0x001B", ParameterType.UInt64 },    //O
            {"0x001b", ParameterType.UInt64 },    //O
            {"0x0008", ParameterType.Real32 },    //O
            {"0x0011", ParameterType.Real64 },    //O
            {"0x0009", ParameterType.String },    //O
            {"0x000A", ParameterType.OctetString }, //O 
            {"0x000a", ParameterType.OctetString }, //O 
            {"0x000B", ParameterType.Unicode },   //O
            {"0x000b", ParameterType.Unicode },   //O
            {"0x000F", ParameterType.Domain },    //O
            {"0x000f", ParameterType.Domain },    //O
            {"0x000C", ParameterType.DateTime },  //O
            {"0x000c", ParameterType.DateTime },  //O
            {"0x000D", ParameterType.TimeSpan },  //O
            {"0x000d", ParameterType.TimeSpan }   //O
        };

        public static readonly Dictionary<ParameterType, String> DictParamTypeEnum_ParamTypeStr = new Dictionary<ParameterType, String>()
        {
            {ParameterType.Null, "0x0000" },      //M
            {ParameterType.Bool, "0x0001" },      //M
            {ParameterType.Int8, "0x0002" },      //M
            {ParameterType.Int16,"0x0003"  },     //M
            {ParameterType.Int24, "0x0010" },     //O
            {ParameterType.Int32, "0x0004" },     //M
            {ParameterType.INT40, "0x0012" },     //O
            {ParameterType.Int48, "0x0013" },     //O
            {ParameterType.Int56, "0x0014" },     //O
            {ParameterType.Int64, "0x0015" },     //O
            {ParameterType.UInt8, "0x0005" },     //M
            {ParameterType.UInt16, "0x0006" },    //M
            {ParameterType.UInt24, "0x0016" },    //O
            {ParameterType.UInt32, "0x0007" },    //M
            {ParameterType.UINT40, "0x0018" },    //O
            {ParameterType.UInt48, "0x0019" },    //O
            {ParameterType.UInt56, "0x001a" },    //O
            {ParameterType.UInt64, "0x001b" },    //O
            {ParameterType.Real32, "0x0008" },    //O
            {ParameterType.Real64, "0x0011" },    //O
            {ParameterType.String, "0x0009" },    //O
            {ParameterType.OctetString, "0x000a" }, //O 
            {ParameterType.Unicode, "0x000b" },   //O
            {ParameterType.Domain, "0x000f" },    //O
            {ParameterType.DateTime, "0x000c" },  //O
            {ParameterType.TimeSpan, "0x000d" }   //O
        };

        private static readonly Dictionary<ParameterType, String> dictParamTypeEnum_ParamNameStr = new Dictionary<ParameterType, String>()
        {
            { ParameterType.Null, "Null"  },
            { ParameterType.Bool, "Bool"  },
            { ParameterType.Int8 , "Int 8" },
            { ParameterType.Int16, "Int 16" },
            { ParameterType.Int24, "Int 24" },
            { ParameterType.Int32, "Int 32" },
            { ParameterType.INT40, "Int 40" },
            { ParameterType.Int48, "Int 48" },
            { ParameterType.Int56, "Int 56" },
            { ParameterType.Int64, "Int 64" },
            { ParameterType.UInt8, "UInt 8" },
            { ParameterType.UInt16, "UInt 16" },
            { ParameterType.UInt24, "UInt 24" },
            { ParameterType.UInt32, "UInt 32" },
            { ParameterType.UINT40, "UInt 40" },
            { ParameterType.UInt48, "UInt 48" },
            { ParameterType.UInt56, "UInt 56" },
            { ParameterType.UInt64, "UInt 64" },
            { ParameterType.Real32, "Float 32" },
            { ParameterType.Real64 , "Float 64" },
            { ParameterType.String, "String" },
            { ParameterType.OctetString, "String_O" },
            { ParameterType.Unicode, "String_U" },
            { ParameterType.Domain, "Domain" },
            { ParameterType.DateTime , "Time" },
            { ParameterType.TimeSpan, "TimeDiff" }
        };

        private static readonly Dictionary<String, ParameterType> dictParamNameStr_ParamTypeEnum = new Dictionary<String, ParameterType>()
        {
            { "Null", ParameterType.Null  },
            { "Bool", ParameterType.Bool  },
            { "Int 8", ParameterType.Int8  },
            { "Int 16", ParameterType.Int16 },
            { "Int 24", ParameterType.Int24 },
            { "Int 32", ParameterType.Int32 },
            { "Int 40", ParameterType.INT40 },
            { "Int 48", ParameterType.Int48 },
            { "Int 56", ParameterType.Int56 },
            { "Int 64", ParameterType.Int64 },
            { "UInt 8", ParameterType.UInt8 },
            { "UInt 16", ParameterType.UInt16 },
            { "UInt 24", ParameterType.UInt24 },
            { "UInt 32", ParameterType.UInt32 },
            { "UInt 40", ParameterType.UINT40 },
            { "UInt 48", ParameterType.UInt48 },
            { "UInt 56", ParameterType.UInt56 },
            { "UInt 64", ParameterType.UInt64 },
            { "Float 32", ParameterType.Real32 },
            { "Float 64", ParameterType.Real64 },
            { "String", ParameterType.String },
            { "String_O", ParameterType.OctetString },
            { "String_U", ParameterType.Unicode },
            { "Domain", ParameterType.Domain },
            { "Time", ParameterType.DateTime },
            { "TimeDiff", ParameterType.TimeSpan }
        };


        public static bool TryEncodeTypeStrToParamType(string paramTypeStr, out ParameterType parameterType)
        {
            return DictParamTypeStr_ParamTypeEnum.TryLookup(paramTypeStr, out parameterType);
        }

        public static bool TryEncodeTypeNameStrToParamType(string paramTypeNameStr, out ParameterType parameterType)
        {
            return dictParamNameStr_ParamTypeEnum.TryLookup(paramTypeNameStr, out parameterType);
        }

        public static bool TryDecodeParamTypeToTypeNameStr(this ParameterType parameterType, out string paramTypeNameStr)
        {
            return dictParamTypeEnum_ParamNameStr.TryLookup(parameterType, out paramTypeNameStr);
        }

        public static bool TryEncodeDs309TypeStrToParamType(string paramTypeD3s309Str, out ParameterType paramType)
        {
            return dictDs309TypeStr_ParamTypeEnum.TryLookup(paramTypeD3s309Str, out paramType);
        }

        /// <summary>
        /// This method will take a paramater type variable and try to tonvert it to its CiA 309-3
        /// standard string representation.
        /// </summary>
        /// <param name="parameterType"></param>
        /// <param name="paramTypeD3s309Str"></param>
        /// <returns></returns>
        public static bool TryDecodeParamTypeToDs309TypeStr(this ParameterType parameterType, out string paramTypeD3s309Str)
        {
            return dictParamTypeEnum_Ds309Str.TryLookup(parameterType, out paramTypeD3s309Str);
        }

        /// <summary>
        /// This method will return true if the type encoded in the given string has desernable information,
        /// otherwise it returns false
        /// </summary>
        /// <param name="strType">The string to be decoded</param>
        /// <param name="type">The ParameterType encoded in the string</param>
        /// <returns></returns>
        public static bool TryExtractTypeStr(string strType, out ParameterType type)
        {
            strType = strType.ToUpper();
            switch (strType)
            {
                case Tokens._00:
                    type = ParameterType.Null;
                    return true;  //
                case Tokens._01:
                    type = ParameterType.Bool;     //M
                    return true;
                case Tokens._02:
                    type = ParameterType.Int8;     //M
                    return true;
                case Tokens._03:
                    type = ParameterType.Int16;    //M
                    return true;
                case Tokens._10:
                    type = ParameterType.Int24;    //O
                    return true;
                case Tokens._04:
                    type = ParameterType.Int32;    //M
                    return true;
                case Tokens._12:
                    type = ParameterType.INT40;    //O
                    return true;
                case Tokens._13:
                    type = ParameterType.Int48;    //O
                    return true;
                case Tokens._14:
                    type = ParameterType.Int56;     //O
                    return true;
                case Tokens._15:
                    type = ParameterType.Int64;    //O
                    return true;
                case Tokens._05:
                    type = ParameterType.UInt8;    //M
                    return true;
                case Tokens._06:
                    type = ParameterType.UInt16;    //M
                    return true;
                case Tokens._16:
                    type = ParameterType.UInt24;    //O
                    return true;
                case Tokens._07:
                    type = ParameterType.UInt32;    //M
                    return true;
                case Tokens._18:
                    type = ParameterType.UINT40;    //O
                    return true;
                case Tokens._19:
                    type = ParameterType.UInt48;   //O
                    return true;
                case Tokens._1A:
                    type = ParameterType.UInt56;    //O
                    return true;
                case Tokens._1B:
                    type = ParameterType.UInt64;    //O
                    return true;
                case Tokens._08:
                    type = ParameterType.Real32;   //O
                    return true;
                case Tokens._11:
                    type = ParameterType.Real64;  //O
                    return true;
                case Tokens._09:
                    type = ParameterType.String;   //O
                    return true;
                case Tokens._0A:
                    type = ParameterType.OctetString; //O 
                    return true;
                case Tokens._0B:
                    type = ParameterType.Unicode;   //O 
                    return true;
                case Tokens._0F:// Big oof
                    type = ParameterType.Domain;    //O
                    return true;
                case Tokens._0C:
                    type = ParameterType.DateTime; //O
                    return true;
                case Tokens._0D:
                    type = ParameterType.TimeSpan;  //O
                    return true;
                default:
                    type = ParameterType.Null;
                    return false;
            }
        }
        #endregion /Type References

        #region 309-3 Type Formatting
        public static String ReturnValueTypeFormatted(this ParameterType type, string valueStr)
        {
            if (valueStr != null)
            {
                try
                {
                    if (valueStr.Contains(Tokens.ERROR))
                    {
                        //skip for now
                    }
                    else if (type.IsNumericType_CiA402() && valueStr.Contains(Tokens.Ox))
                    {//its hex encoded

                        // TODO: This function is being called multiple times with hex encoded and non-hex encoded valueStr for
                        // for each numerical parameter. This seems strange. If the incomming raw parameter value is hex encoded then
                        // there would be no need to call this function again with the decimal encoded numerical value.
                        // I suspect this is why the check for valueStr.Contains(Tokens.Ox) exists above in order to ignore the decimal encoded values.
                        switch (type)
                        {
                            case ParameterType.Int8:
                            case ParameterType.Int16:
                            case ParameterType.Int24:
                            case ParameterType.Int32:
                                valueStr = Convert.ToInt32(valueStr, 16).ToString(CultureInfo.InvariantCulture);
                                break;

                            case ParameterType.Int48:
                            case ParameterType.Int56:
                            case ParameterType.Int64:
                                valueStr = Convert.ToInt64(valueStr, 16).ToString(CultureInfo.InvariantCulture);
                                break;
                            case ParameterType.UInt8:
                            case ParameterType.UInt16:
                            case ParameterType.UInt24:
                            case ParameterType.UInt32:
                                valueStr = Convert.ToUInt32(valueStr, 16).ToString(CultureInfo.InvariantCulture);
                                break;
                            case ParameterType.UInt48:
                            case ParameterType.UInt56:
                            case ParameterType.UInt64:
                                valueStr = Convert.ToUInt64(valueStr, 16).ToString(CultureInfo.InvariantCulture);
                                break;

                            case ParameterType.Real32:
                            case ParameterType.Real64:
                                valueStr = Convert.ToByte(valueStr, 16).ToString(CultureInfo.InvariantCulture);
                                break;
                        }
                    }
                    return valueStr;//output 
                }
                catch (Exception)
                {
                    return String.Empty;//output 
                }
            }
            return String.Empty;//output 
        }
        #endregion /309-3 Type Formatting
    }
}
