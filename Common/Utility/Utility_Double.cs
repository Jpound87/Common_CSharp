using System;
using System.Linq;

namespace Common.Utility
{
    public static class Utility_Double
    {
        #region Maths

        #region Average
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Double Average(params double[] values)
        {
            return values.Average();
        }
        #endregion

        #region Percent Diference
        /// <summary>
        /// Calculates the percent difference between the two inputs. 
        /// </summary>
        /// <param name="value1">First input value.</param>
        /// <param name="value2">Second input value.</param>
        /// <returns>Number between 0 and 1 representing the percent difference.</returns>
        public static Double PercentDifference(Double value1, Double value2)
        {
            if (value1 == value2)
            {
                return 0;
            }
            double denominator = (value1 + value2);
            if (denominator == 0)
            {
                return 1;
            }
            denominator /= 2;
            return Math.Abs(value1 - value2) / denominator;
        }
        #endregion /Percent Diference

        #endregion /Maths
    }
}
