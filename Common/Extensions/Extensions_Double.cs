using Common.Struct;
using Common.Utility;
using System;
using System.Linq;

namespace Common.Extensions
{
    public static class Extensions_Double
    {
        #region Extrema
        /// <summary>
        /// This method will determine the max and min in an array.
        /// </summary>
        /// <param name="values">Array in which to find the max and min</param>
        /// <returns>Tuple whose first item is the range minimum and the second is range maximum</returns>
        public static Extrema FindExtrema(this Double[] values, out Int32 majorGrid, Double windowFactor = .2)
        {
            Extrema extrema = new Extrema(values);
            Utility_Extrema.AdjustExtremaToWindow(ref extrema, windowFactor);
            majorGrid = Convert.ToInt32(Math.Max(1, Math.Floor(extrema.Average) / 3));
            return extrema;
        }

        public static Boolean TryFindExtremaSeperateValues_LoopInvariant(this Double[] inputValues, out Double[] finalValues)
        {
            int length = inputValues.Length;
            int sameCount = 0;
            double lastValue = inputValues.Last();
            finalValues = new double[length];
            return TryFindExtremaSeperateValues_LoopInvariant(inputValues, ref lastValue, ref length, ref sameCount, ref finalValues);
        }

        public static Boolean TryFindExtremaSeperateValues_LoopInvariant(this Double[] inputValues, ref Double lastValue, ref Int32 index, ref Int32 sameCount, ref Double[] finalValues)
        {
            index--;
            if (inputValues[index] == lastValue)
            {
                sameCount++;
            }
            else if (inputValues[index] > lastValue)
            {// The base assumption is that the data is ordered and increasing, so this is not possible.
                finalValues = inputValues;
                return false;
            }
            else if (sameCount == 1)
            {
                finalValues[index] = inputValues[index];
                sameCount = 1;
                lastValue = inputValues[index];
            }
            else
            {
                double step = (lastValue + inputValues[index]) / sameCount;
                sameCount = 1;
                lastValue = inputValues[index];
                for (int fv = index + 1; fv <= index + sameCount; fv++)
                {
                    finalValues[fv] = lastValue + (step * (fv - index));
                }
            }
            if (index > 0)
            {
                return TryFindExtremaSeperateValues_LoopInvariant(inputValues, ref lastValue, ref index, ref sameCount, ref finalValues);
            }
            return true;
        }
        #endregion /Extrema
    }
}
