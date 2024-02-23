using System;

namespace Common.Utility
{
    public class Utility_Random
    {
        #region Generator
        public static Random Random { get; } = new Random();
        #endregion /Generator

        #region Double
        public static String GetRandomDouble_String()
        {
            return GetRandomDouble().ToString();
        }

        public static Double GetRandomDouble()
        {
            lock (Random)
            {
                return Random.NextDouble();
            }
        }
        #endregion /Double

        #region Integer
        public static String GetRandomInteger_String(int rangeMinimum = int.MinValue, int rangeMaximum = int.MaxValue)
        {
            return GetRandomInteger(rangeMinimum, rangeMaximum).ToString();
        }

        public static int GetRandomInteger(int rangeMinimum = int.MinValue, int rangeMaximum = int.MaxValue)
        {
            lock (Random)
            {
                return Random.Next(rangeMinimum, rangeMaximum);
            }
        }
        #endregion /Integer
    }
}
