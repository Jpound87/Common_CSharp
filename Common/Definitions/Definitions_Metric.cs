using System;

namespace Common
{
    #region Metric Scale Enumeration
    public class Definitions_Metric
    {
        public static readonly double TERA = Math.Pow(10.0, 12.0);
        public static readonly double GIGA = Math.Pow(10.0, 9.0);
        public static readonly double MEGA = Math.Pow(10.0, 6.0);
        public static readonly double KILO = Math.Pow(10.0, 3.0);
        public static readonly double HECTO = Math.Pow(10.0, 2.0);
        public static readonly double DECA = Math.Pow(10.0, 1.0);
        public static readonly double BASE = 1.0;
        public static readonly double DECI = Math.Pow(10.0, -1.0);
        public static readonly double CENTI = Math.Pow(10.0, -2.0);
        public static readonly double MILLI = Math.Pow(10.0, -3.0);
        public static readonly double MICRO = Math.Pow(10.0, -6.0);
        public static readonly double NANO = Math.Pow(10.0, -9.0);
        public static readonly double PICO = Math.Pow(10.0, -12.0);
        /// <summary>
        /// Null units have 'unit scaling' ergo x1
        /// </summary>
        public static readonly int NULL = 1;
    }
    #endregion /Metric Scale Enumeration
}
