using Common.Extensions;
using Common.Struct;
using System;

namespace Common.Utility
{
    public static class Utility_Extrema
    {
        #region Find
        public static Boolean TryFindExtremaSeperateValues(ref Double[] values, out Int32 majorGrid, out Extrema extrema, Double windowFactor = .2)
        {
            if (values == null)
            {// These conditions are not applicable.
                majorGrid = 0;
                extrema = new Extrema();
                return false;
            }
            extrema = values.FindExtrema(out majorGrid, windowFactor);
            if (extrema.Valid)
            {// The base assumption is that the data is ordered and increasing, so this is not possible.
                return false;
            }
            if (values.Length <= 1)
            {// We can do this super easy.
                return true;
            }
            if (!values.TryFindExtremaSeperateValues_LoopInvariant(out Double[] finalValues))
            {
                extrema = finalValues.FindExtrema(out majorGrid, windowFactor);
                return false;
            }
            AdjustExtremaToWindow(ref extrema, windowFactor);
            majorGrid = Convert.ToInt32(Math.Max(1, (Math.Floor(extrema.Average) / 3)));
            return true;
        }
        #endregion /Find

        #region Compare
        /// <summary>
        /// This method will comapre extrema and return the most fitting values.
        /// </summary>
        /// <param name="values">Array in which to find the max and min</param>
        /// <returns>Tuple whose first item is the range minimum and the second is range maximum</returns>
        public static Extrema CompareExtrema(params Extrema[] extremas)
        {
            Extrema mostExtrema = extremas[0];
            if (extremas.Length > 1)
            {
                foreach (Extrema extrema in extremas)
                {
                    mostExtrema.Maximum = Math.Max(mostExtrema.Maximum, extrema.Maximum);
                    mostExtrema.Minimum = Math.Min(mostExtrema.Minimum, extrema.Minimum);
                }
            }
            return mostExtrema;
        }
        #endregion / Compare

        #region Adjust
        public static void AdjustExtremaToWindow(ref Extrema extrema, Double windowFactor = .2)
        {
            bool maxZero = extrema.Maximum == 0;
            bool minZero = extrema.Minimum == 0;
            if (maxZero && minZero)
            {// No point, return.
                extrema.Maximum = windowFactor;
                extrema.Minimum = -windowFactor;
                return;
            }
            double windowFactorMax = 1 + windowFactor;
            double windowFactorMin = 1 - windowFactor;
            if (maxZero)
            {// Min must be below zero
                double max = extrema.Minimum;
                extrema.Minimum *= windowFactorMin;
                extrema.Maximum = extrema.Minimum - max;
                return;
            }
            else if (minZero)
            {// Max must be above zero
                double min = extrema.Maximum;
                extrema.Maximum *= windowFactorMax;
                extrema.Minimum = -(extrema.Maximum - min);
                return;
            }
            bool maxBelowZero = extrema.Maximum < 0;
            if (maxBelowZero)
            {
                extrema.Maximum *= windowFactorMin;
                extrema.Minimum *= windowFactorMax;
            }
            else
            {
                extrema.Maximum *= windowFactorMax;
                extrema.Minimum *= windowFactorMin;
            }
        }
        #endregion /Adjust
    }
}
