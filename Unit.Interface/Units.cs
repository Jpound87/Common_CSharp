using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
//using Runtime;

namespace Unit.Interface
{
    #region Enums
    public enum ConversionTypes
    {
        //PerRated,
        MetricScale,
        Time,
        RevCount,
        Torque,
        Inertia,
        Power,
        Speed,
        Acceleration,
        None
    }
    #endregion

    [Serializable]
    public static class Units
    {
        #region Identity
        public const String ClassName = nameof(Units);
        #endregion

        #region Constants
        private const bool FIX_SCALE = true;
        private const string OriginalValue = "{0}";
        //private const string ThousandsInteger = "{0:#,0}";
        private const string OneDecimal = "{0:0.0}";
        private const string TwoDecimals = "{0:0.00}";
        //private const string ThousandsOneDecimal = "{0:#,0.0}";
        //private const string ThousandsTwoDecimals = "{0:#,0.00}";

        public const String COMPOUND_SCALE_WARNING = "Compound unit described using non base scale."; // TODO: Translation Manager?
        #endregion

        #region Readonly
        private static readonly Dictionary<int, Enum> dictEnumerations = new Dictionary<int, Enum>();
        #endregion

        #region Unit Enumeration Struct
        [Serializable]
        public struct Enum : IEquatable<Enum>
        {
            private static readonly HashSet<int> usedEnumValues = new HashSet<int>();
            public int Enumeration { get; private set; }
            public bool Compound { get; private set; }
            public string Name { get; private set; }
            public Enum(int enumeration, string name = "", bool compound = false)
            {
                Enumeration = enumeration;
                Name = name;
                Compound = compound;
                dictEnumerations.Add(enumeration, this);
            }
            public override bool Equals(object other)
            {
                if (other != null && other is Enum compareEnum)
                {
                    return Equals(compareEnum);
                }
                return false;
            }
            public bool Equals(Enum that)
            {
                if (this.Enumeration == that.Enumeration)
                {
                    return true;
                }
                return false;
            }
            public override int GetHashCode()
            {
                return Enumeration;
            }
            /// <summary>
            /// This method allows for automatic assignment of new unit enumerations.
            /// TODO: read in unit definitions?
            /// </summary>
            /// <returns></returns>
            public static Enum MakeNew(string name, bool fixScale = false)
            {
                int enumeration = name.GetHashCode();// Using this as the enumeration value allows for this to be recoverable in the event new enums are added.
                if(usedEnumValues.Contains(enumeration))
                {
                    throw new ArgumentException("Unit enumeration already taken");
                }
                return new Enum(enumeration, name, fixScale);
            }
            public static bool operator ==(Enum thisEnum, Enum thatEnum)
            {
                return Equals(thisEnum, thatEnum);
            }
            public static bool operator !=(Enum thisEnum, Enum thatEnum)
            {
                return !Equals(thisEnum, thatEnum);
            }
        }
        #endregion /Unit Enumeration Struct

        #region Unit Enmerations
        //Per Rated
        public static readonly Enum PerRated = Enum.MakeNew("PerRated");
        public static readonly HashSet<Enum> PerRatedEnums = new() { PerRated };
        //Scalar
        public static readonly Enum RevolutionsPerMinuteSecond = Enum.MakeNew("RevolutionsPerMinuteSecond", FIX_SCALE);
        public static readonly Enum RevolutionsPerSecond2 = Enum.MakeNew("RevolutionsPerSecond2", FIX_SCALE);
        public static readonly Enum RevolutionsPerMinute = Enum.MakeNew("RevolutionsPerMinute", FIX_SCALE);
        public static readonly Enum RevolutionsPerSecond = Enum.MakeNew("RevolutionsPerSecond", FIX_SCALE);
        public static readonly Enum Hertz = Enum.MakeNew("Hertz");
        public static readonly Enum MeterPerSecond2 = Enum.MakeNew("MeterPerSecond2", FIX_SCALE);
        public static readonly Enum MeterPerSecond = Enum.MakeNew("MeterPerSecond", FIX_SCALE);
        public static readonly Enum Watt = Enum.MakeNew("Watt");//NewtonMeterPerSecond is a watt
        public static readonly Enum Volt = Enum.MakeNew("Volt");
        public static readonly Enum Ampere = Enum.MakeNew("Ampere");
        public static readonly Enum Pascal = Enum.MakeNew("Pascal");
        public static readonly Enum Joule = Enum.MakeNew("Joule");//NewtonMeterPerSecond2 is a joule
        public static readonly Enum KilogramMeter2 = Enum.MakeNew("KilogramMeter2", FIX_SCALE);
        public static readonly Enum NewtonMeter = Enum.MakeNew("NewtonMeter");//
        public static readonly Enum Newton = Enum.MakeNew("Newton");
        public static readonly Enum Day = Enum.MakeNew("Day");
        public static readonly Enum Hour = Enum.MakeNew("Hour");
        public static readonly Enum Minute = Enum.MakeNew("Minute");
        public static readonly Enum Second = Enum.MakeNew("Second");
        public static readonly HashSet<Enum> TimeEnums = new()
        {
            Day, Hour, Minute, Second
        };
        public static readonly Enum Celcius = Enum.MakeNew("Celcius");
        public static readonly Enum Gram = Enum.MakeNew("Gram");
        public static readonly Enum Meter = Enum.MakeNew("Meter");
        public static readonly HashSet<Enum> ScalarEnums = new() 
        { 
            RevolutionsPerMinuteSecond, RevolutionsPerSecond2, 
            RevolutionsPerMinute, RevolutionsPerSecond,
            Hertz,
            MeterPerSecond2,
            MeterPerSecond,
            Watt,
            Volt,
            Ampere,
            Pascal,
            KilogramMeter2,
            NewtonMeter,
            Newton,
            Day, Hour, Minute, Second,
            Celcius,
            Gram,
            Meter
        };
        //Null (Not scaled)
        public static readonly Enum Null = Enum.MakeNew("Null");
        public static readonly Enum Word = Enum.MakeNew("Word");
        public static readonly Enum OrdianalCount = Enum.MakeNew("OrdianalCount");
        public static readonly Enum Counts = Enum.MakeNew("Counts");
        public static readonly Enum CountsPerSecond = Enum.MakeNew("CountsPerSecond");
        public static readonly Enum CountsPerSecond2 = Enum.MakeNew("CountsPerSecond2");
        public static readonly Enum Cardinal = Enum.MakeNew("Cardinal");
        public static readonly Enum Float = Enum.MakeNew("Float");
        public static readonly Enum Hex = Enum.MakeNew("Hex");
        public static readonly Enum String = Enum.MakeNew("String");
        public static readonly HashSet<Enum> StringEnums = new() { String };
        public static readonly Enum Domain = Enum.MakeNew("Domain");
        public static readonly HashSet<Enum> DomainEnums = new() { Domain };
        public static readonly Enum Radians = Enum.MakeNew("Radians");
        public static readonly Enum Degrees = Enum.MakeNew("Degrees");
        public static readonly Enum Percent = Enum.MakeNew("Percent");
        public static readonly Enum Enumeration = Enum.MakeNew("Enumeration");
        public static readonly Enum AddressType = Enum.MakeNew("AddressType");
        public static readonly Enum Integer = Enum.MakeNew("Integer");
        public static readonly Enum Boolean = Enum.MakeNew("Boolean");
        public static readonly HashSet<Enum> BooleanEnums = new() { Boolean };
        public static readonly HashSet<Enum> NullEnums = new() 
        {
            Null,
            Word,
            OrdianalCount,
            Counts, 
            CountsPerSecond,
            CountsPerSecond2,
            Cardinal, 
            Float,
            Hex,
            String,
            Domain,
            Radians, Degrees,
            Percent,
            Enumeration,
            AddressType,
            Integer,
            Boolean
        };
        //Non 
        public static readonly Enum Uninitialized = new Enum(int.MinValue);
        // Numeric
        public static readonly HashSet<Enum> NumericEnums = new()
        {
            RevolutionsPerMinuteSecond, RevolutionsPerSecond2,
            RevolutionsPerMinute, RevolutionsPerSecond,
            Hertz,
            MeterPerSecond2,
            MeterPerSecond,
            Watt,
            Volt,
            Ampere,
            Pascal,
            KilogramMeter2,
            NewtonMeter,
            Newton,
            Day, Hour, Minute, Second,
            Celcius,
            Gram,
            Meter,
            Word,
            OrdianalCount,
            Counts,
            CountsPerSecond,
            CountsPerSecond2,
            Cardinal,
            Float,
            Hex,
            Radians, Degrees,
            Percent,
            Integer,
            Boolean
        };
        public static readonly HashSet<Enum> IntegerEnums = new()
        {
            RevolutionsPerMinuteSecond, RevolutionsPerSecond2,
            RevolutionsPerMinute, RevolutionsPerSecond,
            Hertz,
            MeterPerSecond2,
            MeterPerSecond,
            Watt,
            Volt,
            Ampere,
            Pascal,
            KilogramMeter2,
            NewtonMeter,
            Newton,
            Day, Hour, Minute, Second,
            Celcius,
            Gram,
            Meter,
            Word,
            OrdianalCount,
            Counts,
            CountsPerSecond,
            CountsPerSecond2,
            Cardinal,
            Hex,
            Radians, Degrees,
            Percent,
            Integer,
            Boolean
        };
        public static readonly HashSet<Enum> FloatEnums = new()
        {
            RevolutionsPerMinuteSecond, RevolutionsPerSecond2,
            RevolutionsPerMinute, RevolutionsPerSecond,
            Hertz,
            MeterPerSecond2,
            MeterPerSecond,
            Watt,
            Volt,
            Ampere,
            Pascal,
            KilogramMeter2,
            NewtonMeter,
            Newton,
            Day, Hour, Minute, Second,
            Celcius,
            Gram,
            Meter,
            Word,
            CountsPerSecond,
            CountsPerSecond2,
            Float,
            Radians, Degrees,
            Percent
        };
        //Non Numeric
        public static readonly HashSet<Enum> NonNumericEnums = new()
        {
            Null,
            String,
            Domain,
            Enumeration,
            AddressType
        };
        #endregion

        #region Enum Lookup
        public static bool TryGetUnitEnumFromInt(int lookupValue, out Enum unitEnum)
        {
            return dictEnumerations.TryLookup(lookupValue, out unitEnum);
        }
        #endregion

        #region Static Dictionaries
        // No need to have this repeat these dict's for each class instance
        private static readonly IDictionary<Enum, String> dictUnitName = new Dictionary<Enum, String>()
        {
            // Per Rated
            { PerRated, "Per Thousands of Rated" },
            // Non
            { Uninitialized, "Uninitialized" },
            // Scalar
            { Meter, "Meter" },
            { Gram, "Gram" },
            { Celcius, "Celcius" },
            { Second, "Second" },
            { Minute, "Minute" },
            { Hour, "Hour" },
            { Day, "Day" },
            { Newton, "Newton" },
            { NewtonMeter, "Newton Meter" },
            { KilogramMeter2, "Kilogram per Meter²" },
            { Joule, "Joule" }, // Nm/s
            { Pascal, "Pascal" },
            { Ampere, "Ampere" },
            { Volt, "Volt" },
            { Watt, "Watt" },// Nm/s²
            { MeterPerSecond, "Meter per Second" },
            { MeterPerSecond2, "Meter per Second²" },
            { Hertz, "Hertz" },
            { RevolutionsPerSecond, "Revolutions per Second" },
            { RevolutionsPerMinute, "Revolutions per Minute" },
            { RevolutionsPerMinuteSecond, "Revolutions per Mintue per Second" },
            // Null
            { Null, "Null" },
            { Word, "Word" },
            { OrdianalCount, "Ordinal" },
            { Counts, "Counts" },
            { CountsPerSecond, "Counts per Second" },
            { CountsPerSecond2, "Counts per Second²" },
            { Float, "Float" },
            { Hex, "Hexidecimal" },
            { String, "String" },
            { Domain, "Domain" },
            { Radians, "Radians" },
            { Degrees, "Degrees" },
            { Boolean, "Boolean" },
            { Enumeration, "Enumeration" },
            { AddressType, "AddressType" },
            { Percent, "Percent" },
            { Integer, "Integer" }
        };

        public static string GetUnitName(this Enum unit)
        {
            if (dictUnitName.TryLookup(unit, out string name))
            {
                return name;
            }
            return dictUnitName[Null];
        }

        private static readonly IDictionary<String, Enum> dictNameUnit = dictUnitName.Invert();

        public static bool TryGetUnitFromName(String name, out Enum unit)
        {
            if (dictNameUnit.TryLookup(name, out unit))
            {
                return true;
            }
            return false;
        }

        private static readonly IDictionary<Enum, String> dictUnitShortName = new Dictionary<Enum, String>()
        {
            // Per Rated
            { PerRated, "‰" },
            // Non
            { Uninitialized, "?" },
            // Scalar
            { Meter, "m" },
            { Gram, "g" },
            { Celcius, "°C" },
            { Second, "s" },
            { Minute, "min" },
            { Hour, "hr" },
            { Day, "day" },
            { Newton, "N" },
            { NewtonMeter, "Nm" },
            { KilogramMeter2, "Kgm²" },
            //{ NewtonMeterPerSecond, "Nm/s" }, Watt
            //{ NewtonMeterPerSecond2, "Nm/s²" }, Joule
            { Joule, "J" },
            { Pascal, "Pa" },
            { Ampere, "A" },
            { Volt, "V" },
            { Watt, "W" },
            { MeterPerSecond, "m/s" },
            { MeterPerSecond2, "m/s²" },
            { Hertz, "Hz" },
            { RevolutionsPerSecond, "rps" },
            { RevolutionsPerMinute, "rpm" },
            { RevolutionsPerMinuteSecond, "rpm/s" },
            // Null
            { Null, string.Empty },
            { Word, "Word" },
            { OrdianalCount, "ct" },
            { Counts, "cnt" },
            { CountsPerSecond, "cnt/s" },
            { CountsPerSecond2, "cnt/s²" },
            { Float, "flt" },
            { Hex, "Hex" },
            { String, "str" },
            { Domain, "dom" },
            { Radians, "rads" },
            { Degrees, "°" },
            { Percent, "%" },
            { Enumeration, "enum" },
            { AddressType, "a/t" },
            { Integer, "int" },
            { Boolean, "bool" }
        };

        public static string GetShortUnitName(this Enum unit)
        {
            if (dictUnitShortName.TryLookup(unit, out string name))
            {
                return name;
            }
            return dictUnitShortName[Null];
        }

        public static bool TryGetShortUnitName(this Enum unit, out string name)
        {
            if (dictUnitShortName.TryLookup(unit, out name))
            {
                return true;
            }
            return false;
        }

        private static readonly IDictionary<String, Enum> dictShortNameUnit = dictUnitShortName.Invert();

        public static bool TryGetShortScaleFromName(string name, out Enum unit)
        {
            if (dictShortNameUnit.TryLookup(name, out unit))
            {
                return true;
            }
            return false;
        }

        private static readonly IDictionary<Enum, HashSet<ConversionTypes>> dictUnitConversionTypes = new Dictionary<Enum, HashSet<ConversionTypes>>()
        {
            { PerRated, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { Uninitialized, new HashSet<ConversionTypes>(){ ConversionTypes.None } },
            { Meter, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { Gram, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { Celcius, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { Second, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale, ConversionTypes.Time } },
            { Minute, new HashSet<ConversionTypes>(){ ConversionTypes.Time } },
            { Hour, new HashSet<ConversionTypes>(){ ConversionTypes.Time } },
            { Day, new HashSet<ConversionTypes>(){ ConversionTypes.Time } },
            { Newton, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { NewtonMeter, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale, ConversionTypes.Torque } },
            { KilogramMeter2, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale, ConversionTypes.Inertia } },
            //{ NewtonMeterPerSecond, new HashSet<ConversionTypes>(){ ConversionTypes.Power } }, Watt
            { Joule, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { Pascal, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { Ampere, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { Volt, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { Watt, new HashSet<ConversionTypes>(){ ConversionTypes.Power } },
            { MeterPerSecond, new HashSet<ConversionTypes>(){ ConversionTypes.Speed } },
            { MeterPerSecond2, new HashSet<ConversionTypes>(){ ConversionTypes.Acceleration } },
            { Counts, new HashSet<ConversionTypes>(){ ConversionTypes.None } },
            { CountsPerSecond, new HashSet<ConversionTypes>(){ ConversionTypes.Speed } },
            { CountsPerSecond2, new HashSet<ConversionTypes>(){ ConversionTypes.Acceleration } },
            { Hertz, new HashSet<ConversionTypes>(){ ConversionTypes.MetricScale } },
            { RevolutionsPerSecond, new HashSet<ConversionTypes>(){ ConversionTypes.Speed } },
            { RevolutionsPerMinute, new HashSet<ConversionTypes>(){ ConversionTypes.Speed } },
            { Null, new HashSet<ConversionTypes>(){ } }
        };

        public static HashSet<ConversionTypes> GetConversionTypes(this Enum unit)
        {
            if (dictUnitConversionTypes.TryLookup(unit, out HashSet<ConversionTypes> ConversionTypes))
            {
                return ConversionTypes;
            }
            return dictUnitConversionTypes[Null];
        }

        private const double countsPerRev = 65536.0;//todo: make this defined in motor parameters, b/c of encoder count?
        public static readonly IDictionary<Enum, Dictionary<Enum, Double>> dictNativeUnit_dictDisplayUnitConversionFactor =
            new Dictionary<Enum, Dictionary<Enum, Double>>()
            {
                { Second, new Dictionary<Enum, double>()
                    {
                        { Minute, 1.0/60.0 },
                        { Hour, 1.0/(60.0*60.0) },
                        { Day, 1.0/(60.0*60.0*24.0) }
                    } 
                },

                { Minute, new Dictionary<Enum, double>()
                    {
                        { Second , 60.0 },
                        { Hour, 1.0/60.0 },
                        { Day, 1.0/(60.0*24.0)}
                    } 
                },

                { Hour, new Dictionary<Enum, double>()
                    {
                        { Second, 60.0*60.0 },
                        { Minute, 60.0 },
                        { Day, 1.0/24.0}
                    } 
                },

                { Day, new Dictionary<Enum, double>()
                    {
                        { Second, 60.0*60.0*24.0},
                        { Minute, 60.0*24.0},
                        { Hour, 24.0}
                    } 
                },

                { CountsPerSecond, new Dictionary<Enum, double>()
                    {
                        { RevolutionsPerMinute, 60.0/countsPerRev},
                        { RevolutionsPerSecond, 1.0/countsPerRev}
                    } 
                },

                { CountsPerSecond2, new Dictionary<Enum, double>()
                    {
                        { RevolutionsPerSecond2, 1.0/countsPerRev}
                    } 
                },

                { RevolutionsPerSecond, new Dictionary<Enum, double>()
                    {
                        { CountsPerSecond, countsPerRev },
                        { RevolutionsPerMinute, 60.0 }
                    } 
                },

                { RevolutionsPerMinute, new Dictionary<Enum, double>()
                    {
                        { CountsPerSecond, countsPerRev/60.0},
                        { RevolutionsPerSecond, 1.0/60.0 }
                    } 
                },

                { RevolutionsPerSecond2, new Dictionary<Enum, double>()
                    {
                        { CountsPerSecond2, countsPerRev }
                    } 
                }
            };

 

        private static readonly IDictionary<Enum, String> dictUnitFormatter = new Dictionary<Enum, String>()
        {
            // Per Rated
            { PerRated, OriginalValue },// ThousandsInteger },
            // Scalar
            { Uninitialized, OriginalValue },
            { Meter, OneDecimal },//ThousandsOneDecimal },
            { Gram, OneDecimal },//ThousandsOneDecimal },
            { Celcius, OneDecimal },
            { Second, OneDecimal },//ThousandsInteger },
            { Minute, OneDecimal },//ThousandsInteger },
            { Hour, OneDecimal },//ThousandsInteger },
            { Day, OneDecimal },//ThousandsInteger },
            { Newton, OneDecimal },//ThousandsOneDecimal },
            { NewtonMeter, OneDecimal },//ThousandsOneDecimal },
            //{ NewtonMeterPerSecond, OneDecimal },//ThousandsOneDecimal }, //Watt
            { Joule, OneDecimal },//ThousandsOneDecimal },
            { Pascal, OneDecimal },//ThousandsOneDecimal },
            { Ampere, TwoDecimals },//ThousandsTwoDecimals },
            { Volt, TwoDecimals },//ThousandsTwoDecimals },
            { Watt, TwoDecimals },//ThousandsTwoDecimals },
            { MeterPerSecond, OneDecimal },//ThousandsOneDecimal },
            { MeterPerSecond2, OneDecimal },//ThousandsOneDecimal },
            { Hertz, TwoDecimals },//ThousandsTwoDecimals },
            { RevolutionsPerSecond, OneDecimal },//ThousandsOneDecimal },
            { RevolutionsPerMinute, OneDecimal },//ThousandsOneDecimal },
            // Null
            { Null, OriginalValue },
            { Word, OriginalValue },
            { OrdianalCount, OriginalValue },//ThousandsInteger},
            { Counts, OneDecimal },//ThousandsInteger },
            { CountsPerSecond, OneDecimal },//ThousandsInteger },
            { CountsPerSecond2, OneDecimal },//ThousandsInteger },
            { Float,  TwoDecimals },//ThousandsTwoDecimals },
            { String, OriginalValue },
            { Domain, OriginalValue },
            { Radians, TwoDecimals },//ThousandsTwoDecimals },
            { Degrees, TwoDecimals },//ThousandsTwoDecimals },
            { Percent, TwoDecimals },
            { Integer, OriginalValue },//ThousandsInteger },
            { Boolean, OriginalValue },
            { Enumeration, OriginalValue },
            { AddressType, OriginalValue }
        };

        public static string LookupDecimalFormatter(this Enum unit)
        {
            if (dictUnitFormatter.ContainsKey(unit))
            {
                return dictUnitFormatter[unit];
            }
            else return dictUnitFormatter[Uninitialized];
        }

        public static bool IsPerPated(this Enum unit)
        {
            if (unit == PerRated)
            {
                return true;
            }
            else return false;
        }
        #endregion

        #region Display
        /// <summary>
        /// This method will determine if the CiA402 unit type is scalable and therefore
        /// if this unit should be displayed to the user.
        /// </summary>
        /// <param name="type">DS402 type to be evaluated</param>
        /// <returns>True if numeric</returns>
        public static bool IsUnitDisplayble(this IUnit unit)
        {
            if (unit is IUnit_Scalar)
            {
                return true; // todo: check for specific types?
            }
            return false;
        }
        #endregion

        #region Static Methods
        public static bool TryUnitAdjustment(this Units.Enum currentUnit, Units.Enum outputUnit, ref string valueStr)
        {
            if (currentUnit.Equals(outputUnit))
            {
                return true;
            }
            else
            {
                if (double.TryParse(valueStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {// Check to see if this text is indeed an number.
                    UnitAdjustment(currentUnit, outputUnit, ref value);
                    valueStr = value.ToString(CultureInfo.InvariantCulture);
                    return true;
                }
            }
            return false;
        }

        public static void UnitAdjustment(this Units.Enum currentUnit, Units.Enum outputUnit, ref double value)
        {
            if (!currentUnit.Equals(outputUnit))
            {
                if (Units.dictNativeUnit_dictDisplayUnitConversionFactor.ContainsKey(currentUnit))
                {
                    Dictionary<Units.Enum, double> dictDisplayUnitConversionFactor = Units.dictNativeUnit_dictDisplayUnitConversionFactor[currentUnit];
                    if (dictDisplayUnitConversionFactor.ContainsKey(outputUnit))
                    {
                        double conversionFactor = dictDisplayUnitConversionFactor[outputUnit];
                        value *= conversionFactor;
                    }
                }
            }
        }
        #endregion
    }
}
