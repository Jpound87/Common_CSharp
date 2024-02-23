using Common;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Unit.Interface
{
    [Serializable]
    public static class Scales
    {
        #region Idenity
        public const String ClassName = nameof(Scales);
        #endregion
        
        #region Scale Definition Struct
        private static readonly Dictionary<int, Enum> dictEnumerations = new Dictionary<int, Enum>();
        [Serializable]
        public struct Enum : IEquatable<Enum>
        {
            public int Enumeration { get; private set; }
            public Enum(int enumeration)
            {
                Enumeration = enumeration;
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
            public static bool operator ==(Enum thisEnum, Enum thatEnum)
            {
                return Equals(thisEnum, thatEnum);
            }
            public static bool operator !=(Enum thisEnum, Enum thatEnum)
            {
                return !Equals(thisEnum, thatEnum);
            }
        }
        #endregion

        #region Metric Scale Enumeration
        public static Enum Tera = new Enum(13);
        public static Enum Giga = new Enum(12);
        public static Enum Mega = new Enum(11);
        public static Enum Kilo = new Enum(10);
        public static Enum Hecto = new Enum(9);
        public static Enum Deca = new Enum(8);
        public static Enum Base = new Enum(7);
        public static Enum Deci = new Enum(6);
        public static Enum Centi = new Enum(5);
        public static Enum Milli = new Enum(4);
        public static Enum Micro = new Enum(3);
        public static Enum Nano = new Enum(2);
        public static Enum Pico = new Enum(1);
        public static Enum Null = new Enum(0);
        #endregion

        #region Enum Lookup
        public static bool TryGetScaleEnumFromInt(int lookupValue, out Enum unitEnum)
        {
            return dictEnumerations.TryLookup(lookupValue, out unitEnum);
        }
        #endregion

        #region Enumeration Array
        /// <summary>
        /// The set of all useful metric scale prefixes.
        /// </summary>
        /// <returns></returns>
        public static Enum[] ToArray_Full()
        {
            return new Enum[14]
            {
                Tera,
                Giga,
                Mega,
                Kilo,
                Hecto,
                Deca,
                Base,
                Deci,
                Centi,
                Milli,
                Micro,
                Nano,
                Pico,
                Null
            };
        }

        /// <summary>
        /// The set of more commonly used scale prefixes.
        /// </summary>
        /// <returns></returns>
        public static Enum[] ToArray_Common()
        {
            return new Enum[9]
            {
                Giga,
                Mega,
                Kilo,
                Base,
                Centi,
                Milli,
                Micro,
                Nano,
                Pico
            };
        }
        #endregion /Enumeration Array

        #region Static Dictionaries

        private static readonly IDictionary<Enum, Double> dictScaleFactor = new Dictionary<Enum, Double>()
        {
            { Tera, Definitions_Metric.TERA },
            { Giga, Definitions_Metric.GIGA },
            { Mega, Definitions_Metric.MEGA  },
            { Kilo, Definitions_Metric.KILO  },
            { Hecto, Definitions_Metric.HECTO },
            { Deca, Definitions_Metric.DECA  },
            { Base, Definitions_Metric.BASE },
            { Deci, Definitions_Metric.DECI },
            { Centi, Definitions_Metric.CENTI  },
            { Milli, Definitions_Metric.MILLI  },
            { Micro, Definitions_Metric.MICRO  },
            { Nano, Definitions_Metric.NANO  },
            { Pico, Definitions_Metric.PICO  },
            { Null, Definitions_Metric.NULL }
        };

        public static double GetScaleFactor(this Enum scale)
        {
            if (dictScaleFactor.TryLookup(scale, out double factor))
            {
                return factor;
            }
            return dictScaleFactor[Null];
        }

        private static readonly IDictionary<Enum, String> dictScaleName = new Dictionary<Enum, String>()
        {
            { Tera, "Tera" },
            { Giga, "Giga" },
            { Mega, "Mega" },
            { Kilo, "Kilo" },
            { Hecto, "Hecto" },
            { Deca, "Deca" },
            { Base, "-" },
            { Deci, "Deci" },
            { Centi, "Centi" },
            { Milli, "Milli" },
            { Micro, "Micro" },
            { Nano, "Nano" },
            { Pico, "Pico" },
            { Null, "-" }
        };// Cannot be reversed with '-' for null and base.

        public static string GetScaleName(this Enum scale)
        {
            if(dictScaleName.TryLookup(scale, out string name))
            {
                return name;
            }
            return dictScaleName[Null];
        }

        private static readonly IDictionary<Enum, String> dictShortScaleName = new Dictionary<Enum, String>()
        {
            { Tera, "T" },
            { Giga, "G" },
            { Mega, "M" },
            { Kilo, "k" },
            { Hecto, "h" },
            { Deca, "da" },
            { Base, String.Empty },
            { Deci, "d" },
            { Centi, "c" },
            { Milli, "m" },
            { Micro, "µ" },
            { Nano, "n" },
            { Pico, "p" },
            { Null, "Ø" }
        };

        public static string GetShortScaleName(this Enum scale)
        {
            if (dictShortScaleName.TryLookup(scale, out string name))
            {
                return name;
            }
            return dictShortScaleName[Null];
        }

        public static bool TryGetShortScaleName(Enum scale, out string name)
        {
            if (dictShortScaleName.TryLookup(scale, out name))
            {
                return true;
            }
            return false;
        }

        private static readonly IDictionary<String, Enum> dictNameShortScale = dictShortScaleName.Invert();

        public static bool TryGetScaleFromName(String name, out Enum scale)
        {
            if (dictNameShortScale.TryLookup(name, out scale))
            {
                return true;
            }
            return false;
        }

        public static bool TryScaleAdjustment(this Enum currentScale, Enum outputScale, ref String valueStr)
        {
            if (Double.TryParse(valueStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {// Check to see if this text is indeed an number.
                ScaleAdjustment(currentScale, outputScale, ref value);
                valueStr = value.ToString(CultureInfo.InvariantCulture);
                return true;
            }
            return false;
        }

        public static void ScaleAdjustment(this Enum currentScale, Enum outputScale, ref Double value)
        {
            if (currentScale.Equals(Null) || outputScale.Equals(Null))
            {
                throw new Exception("Null scales are unitless and cant be scaled");
            }
            value *= dictScaleFactor[currentScale] / dictScaleFactor[outputScale];
        }
        #endregion /Static Dictionaries
    }
}
