using Common.Constant;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Globalization;

namespace Common.Utility
{
    /// <summary>
    /// This is the library of static utility functions that require no outside project references.
    /// </summary>
    public static class Utility_General
    {
        #region Identity
        public const String FormName = nameof(Utility_General);
        #endregion

        #region Trignometry --
        public static double DegToRad(double angle) => Math.PI * angle / 180.0;
        #endregion

        #region Swap--
        public static void Swap(ref int x, ref int y)
        {
            if (x != y)
            {
                x ^= y;
                y ^= x;
                x ^= y;
            }
        }
        public static void Swap(ref uint x, ref uint y)
        {
            if (x != y)
            {
                x ^= y;
                y ^= x;
                x ^= y;
            }
        }
        public static void Swap(ref bool x, ref bool y)
        {
            if (x != y)
            {
                x ^= y;
                y ^= x;
                x ^= y;
            }
        }
        #endregion

        #region Maths --

        /// <summary>
        /// This method returns all permuations of the possible combinations
        /// of n items in groups of g
        /// </summary>
        /// <param name="n">number of items</param>
        /// <param name="g">number of items in a group</param>
        /// <returns></returns>
        public static int Permutation(int n, int g)
        {
            return Factorial(n) / (Factorial(n - g));
        }
        /// <summary>
        /// This method returns all combinations of the possible combinations
        /// of n items in groups of g
        /// </summary>
        /// <param name="n">number of items</param>
        /// <param name="g">number of items in a group</param>
        /// <returns></returns>
        public static int Combination(int n, int g)
        {
            if (n == 1)
            {
                return 1;
            }
            return Factorial(n) / (Factorial(n - g) * Factorial(g));
        }

        /// <summary>
        /// This method returns all combinations of the possible combinations
        /// of n items in groups of g
        /// </summary>
        /// <param name="n">number of items</param>
        /// <param name="g">number of items in a group</param>
        /// <returns></returns>
        public static uint Combination(uint n, uint g)
        {
            if (n == 1)
            {
                return 1;
            }
            return Factorial(n) / (Factorial(n - g) * Factorial(g));
        }

        /// <summary>
        /// This method will recursively solve for the factorial of the 
        /// given integer
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Factorial(int n)
        {
            if (n >= 1)
            {
                return n * Factorial(n - 1);
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// This method will recursively solve for the factorial of the 
        /// given integer
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static uint Factorial(uint n)
        {
            if (n >= 1)
            {
                return n * Factorial(n - 1);
            }
            else
            {
                return 1;
            }
        }
        public struct LinearDecompositionValues
        {
            public Matrix<Double> U { get; private set; }
            public Matrix<Double> V { get; private set; }
            public Vector<Double> SingularValues { get; private set; }

            public LinearDecompositionValues(Matrix<Double> u, Matrix<Double> v, Vector<Double> singularValues)
            {
                U = u;
                V = v;
                SingularValues = singularValues;
            }
        }

        public static LinearDecompositionValues SingularValueDecomposition(Matrix<double> matrix)
        {
            var svd = matrix.Svd();
            // Perform singular value decomposition
            return new LinearDecompositionValues(svd.U, svd.VT, svd.S);
        }

        public static Vector<Double> LinearRegression(Matrix<double> x, Vector<Double> y)
        {
            //Matrix<Double> X = Matrix<double>.Build.DenseOfArray(data);
            //Vector<Double> y = Vector<double>.Build.Dense(new[] { 3, 4, 5, 6 });
            Matrix<Double> X_transpose = x.Transpose();
            Matrix<Double> X_transpose_X = X_transpose * x;
            Matrix<Double> X_transpose_X_inverse = X_transpose_X.Inverse();
            Vector<Double> X_transpose_y = X_transpose * y;
            Vector<Double> beta = X_transpose_X_inverse * X_transpose_y;

            return beta;
        }

        /// <summary>
        /// Find the greatest common divisor of two integers.
        /// Uses Euclid's algorithm to find the greatest common divisor of two integers.
        /// See: Euclid's treatise on mathematics "The Elements"
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The greatest common divisor.</returns>
        public static int GreatestCommonDivisor(int x, int y)
        {
            while (x > 0)
            {
                if (x < y)
                {
                    Swap(ref x, ref y);
                }
                x -= y;
            }
            return y;
        }

        public static Double PercentageIncrease(double oldValue, double newValue)
        {
            return (newValue - oldValue) / oldValue;
        }

        /// <summary>
        /// This method will calculate a new average using the current average, 
        /// the number of samples in the current average, and a new value
        /// </summary>
        /// <param name="average">The current average</param>
        /// <param name="samples">Number of saples in the current average</param>
        /// <param name="value">The value to add to the average</param>
        /// <returns></returns>
        public static Double RunningAverage(double average, uint samples, double value)
        {
            //so as not to skew the average, we need to restore the weight of the average by 
            //multiplying by the number of samples
            return (value + (average * samples)) / (1 + samples); //TODO: this will run A LOT, so optimizations would be great
        }

        #endregion

        #region Plot --

        /// <summary>
        /// This method scales the max and min axis values to create 
        /// a visible edge space for the plot
        /// </summary>
        /// <param name="axisMax"></param>
        /// <param name="axisMin"></param>
        public static void AdjMaxMin(ref double axisMax, ref double axisMin, double scale)
        {
            double adj;
            if (axisMax - axisMin == 0)
            {
                if (axisMax == 0)
                {// Both must be zero. (By the identitiy property of real numbers)
                    adj = .1 * scale;
                    axisMax += adj;
                    axisMin -= adj;
                }
                else
                {// If axis max - min is zero and its not zero, they are the same number.
                    adj = Math.Abs(axisMax * scale);
                    axisMax += adj;
                    axisMin -= adj;
                }
            }
            else
            {
                adj = Math.Abs((axisMax - axisMin) * scale);
                axisMax += adj;
                axisMin -= adj;
            }
        }
        #endregion /Plot

        #region String to Numeric Conversion --

        /// <summary>
        /// Attempts conversion and captures exceptions from failed conversions.
        /// Is tolerant of leading and trailing whitespace. Fails conversion on null or empty input.
        /// </summary>
        /// <param name="inputStr">The string to be converted</param>
        /// <param name="result">Conversion result</param>
        /// <returns>True if convertable</returns>
        public static Boolean TryConvertFromHexStringToInt(String inputStr, out Int32 result)
        {
            // Must trim leading '0x' as TryParse doesn't support it
            if (inputStr != null && inputStr.ToLower().StartsWith("0x"))
            {
                inputStr = inputStr[2..];
            }
            return Int32.TryParse(inputStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Attempts conversion and captures exceptions from failed conversions.
        /// Is tolerant of leading and trailing whitespace. Fails conversion on null or empty input.
        /// </summary>
        /// <param name="inputStr">The string to be converted</param>
        /// <param name="result">Conversion result</param>
        /// <returns>True if convertable</returns>
        public static Boolean TryConvertFromHexStringToUInt(String inputStr, out UInt32 result)
        {
            // Must trim leading '0x' as TryParse doesn't support it
            if (inputStr != null && inputStr.ToLower().StartsWith("0x"))
            {
                inputStr = inputStr[2..];
            }
            return UInt32.TryParse(inputStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Attempts conversion and captures exceptions from failed conversions.
        /// Is tolerant of leading and trailing whitespace.
        /// Fails conversion on null or empty input, and on decimal points. 
        /// </summary>
        /// <param name="inputStr">The string to be converted</param>
        /// <param name="result">Conversion result</param>
        /// <returns>True if convertable</returns>
        /// <remarks>Assumes InvariantCulture</remarks>
        public static Boolean TryConvertFromWholeNumberStringToInt(String inputStr, out Int32 result)
        {
            return int.TryParse(inputStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Attempts conversion and captures exceptions from failed conversions.
        /// Is tolerant of leading and trailing whitespace.
        /// Fails conversion on null or empty input, and on decimal points. 
        /// </summary>
        /// <param name="inputStr">The string to be converted</param>
        /// <param name="result">Conversion result</param>
        /// <returns>True if convertable</returns>
        /// <remarks>Assumes InvariantCulture</remarks>
        public static Boolean TryConvertFromWholeNumberStringToUInt(String inputStr, out UInt32 result)
        {
            return UInt32.TryParse(inputStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Attempts conversion and captures exceptions from failed conversions.
        /// Is tolerant of leading and trailing whitespace. Fails conversion on null or empty input.
        /// </summary>
        /// <param name="inputStr">The string to be converted</param>
        /// <param name="result">Conversion result</param>
        /// <returns>True if convertable</returns>
        public static Boolean TryConvertFromBinaryStringToInt(String inputStr, out Int32 result)
        {// Note: cannot use uint.TryParse as it doesn't support base 2
            inputStr = inputStr.Trim();// Convert.ToXXX functions not always tolerant of whitespace
            if (String.IsNullOrWhiteSpace(inputStr))
            {// Need to manually check for null as Convert.ToXXX does not fail on null input
                result = 0;
                return false;
            }
            try
            {
                result = Convert.ToInt32(inputStr, 2);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        /// <summary>
        /// Attempts conversion and captures exceptions from failed conversions.
        /// Is tolerant of leading and trailing whitespace. Fails conversion on null or empty input.
        /// </summary>
        /// <param name="inputStr">The string to be converted</param>
        /// <param name="result">Conversion result</param>
        /// <returns>True if convertable</returns>
        public static Boolean TryConvertFromBinaryStringToUInt(String inputStr, out UInt32 result)
        {
            inputStr = inputStr.Trim();// Convert.ToXXX functions not always tolerant of whitespace
            if (String.IsNullOrWhiteSpace(inputStr))
            {// Need to manually check for null as Convert.ToXXX does not fail on null input
                result = 0;
                return false;
            }
            try
            {
                result = Convert.ToUInt32(inputStr, 2);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        /// <summary>
        /// Attempts conversion and captures exceptions from failed conversions.
        /// Is tolerant of leading and trailing whitespace. Fails conversion on null or empty input.
        /// </summary>
        /// <param name="inputStr">The string to be converted</param>
        /// <param name="result">Conversion result</param>
        /// <returns>True if convertable</returns>
        public static Boolean TryConvertToBoolean_FromBooleanString(String inputStr, out Boolean result)
        {
            return Boolean.TryParse(inputStr, out result);
        }

        /// <summary>
        /// Attempts conversion and captures exceptions from failed conversions.
        /// Is tolerant of leading and trailing whitespace. Fails conversion on null or empty input.
        /// Number strings can be an integer or real number, can use decimal points, exponential symbols,
        /// and thousands seperator.
        /// </summary>
        /// <param name="inputStr">The string to be converted</param>
        /// <param name="result">Conversion result</param>
        /// <param name="fromCulture">The culture that is used to interpret the string value
        ///     e.g. in Germany "10.000,50" would be interpretted as ten-thousand point five
        ///     e.g. in US "10,000.50" would be interpretted as ten-thousand point five
        /// </param>
        /// <returns>True if convertable</returns>
        public static Boolean TryConvertFromNumberString_ToDouble(String inputStr, out Double result, CultureInfo fromCulture)
        {
            return Double.TryParse(inputStr, NumberStyles.Float | NumberStyles.AllowThousands, fromCulture, out result);
        }

        // Like TryConvertFromNumberStringToDouble but ignores failures
        public static Double ConvertFromNumberStringToDouble(String value, CultureInfo fromCulture)
        {
            TryConvertFromNumberString_ToDouble(value, out double result, fromCulture);
            return result;
        }

        /// <summary>
        /// Attempts conversion and captures exceptions from failed conversions.
        /// Is tolerant of leading and trailing whitespace. Fails conversion on null or empty input.
        /// Number strings can be an integer or real number, can use decimal points, exponential symbols,
        /// and thousands seperator.
        /// </summary>
        /// <param name="inputStr">The string to be converted</param>
        /// <param name="fromCulture">The culture that is used to interpret the string value
        ///     e.g. in Germany "10.000,50" would be interpretted as ten-thousand point five
        ///     e.g. in US "10,000.50" would be interpretted as ten-thousand point five
        /// </param>
        /// <returns>True if convertable</returns>
        public static Boolean TryConvertFromNumberStringToDecimal(String inputStr, out Decimal result, CultureInfo fromCulture)
        {
            return decimal.TryParse(inputStr, NumberStyles.Float | NumberStyles.AllowThousands, fromCulture, out result);
        }

        /// <summary>
        /// This method is intended to remove ".00" from "X.00" which may have been appended 
        /// for user readablility. 
        /// </summary>
        private static void RemoveFloatTrailingZeros(ref String value)
        {
            if (value.Contains('.'))
            {// If its been assmued to be a decimal, modify in case its "X.00"
                value = value.TrimEnd('0');
                value = value.TrimEnd('.');
            }
        }

        #endregion /String to Numeric Conversion --

        #region Numeric String to Numeric String Conversion --
        /// <summary>
        /// This converts the exponential from the CSV file into the form of a Decimal (really a double) so that the string displays its true value.
        /// I.E. "0.0000098" instead of "9.8E-6" Because most things save them in scientific notation and it will not be able to do the maths after -EF
        /// </summary>
        /// <param name="toBeConverted"></param>
        /// <returns></returns>
        public static string Convert_FromExponent(string toBeConverted)
        {
            if (Utility_General.TryConvertFromNumberString_ToDouble(toBeConverted, out double dataDouble, CultureInfo.InvariantCulture))
            {
                return string.Format(CultureInfo.InvariantCulture, "{0: 0.############}", dataDouble);
            }
            else
            {
                //TODO log or tell user
                return string.Empty;
            }
        }
        #endregion

        #region Authorization --
        public static AuthorizationLevel DetermineAuthLevelFromInt(int authInt)
        {
            AuthorizationLevel deviceAuthLevel = AuthorizationLevel.Safety;
            foreach (AuthorizationLevel authLevel in Enum.GetValues(typeof(AuthorizationLevel)))
            {
                if ((int)authLevel == authInt)
                {// If they are equal then we know its the one.
                    return authLevel;
                }
                else if ((int)authLevel > authInt)
                {// Else we find out if its the greatest possible.
                    if ((int)authLevel <= authInt)
                    {
                        deviceAuthLevel = authLevel;
                    }
                }
            }
            return deviceAuthLevel;
        }
        #endregion

        #region Read Write Determination --
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessRight"></param>
        /// <returns></returns>
        public static bool IsWriteAccess(AccessRights accessRight)
        {
            switch (accessRight)
            {
                case AccessRights.RW:
                case AccessRights.RWR:
                case AccessRights.RWW:
                case AccessRights.WO:
                    return true;
                case AccessRights.ALLIED://Technically writable, but we dont want it to be unless they are in allied mode
                case AccessRights.CONST:
                case AccessRights.RO:
                default:
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessRight"></param>
        /// <returns></returns>
        public static bool IsReadWriteAccess(AccessRights accessRight)
        {
            switch (accessRight)
            {
                case AccessRights.RW:
                case AccessRights.RWR:
                case AccessRights.RWW:
                    return true;
                case AccessRights.WO:
                case AccessRights.ALLIED://Technically writable, but we dont want it to be unless they are in allied mode
                case AccessRights.CONST:
                case AccessRights.RO:
                default:
                    return false;
            }
        }
        #endregion

        #region Byte Manipulation --
        public static ushort IndexMask(uint dword)
        {
            return (ushort)(dword >> 16);
        }

        public static ushort LoWord(uint dword)
        {
            return (ushort)dword;
        }
        public static byte HiByte(ushort word)
        {
            return (byte)(word >> 8);
        }

        public static byte LoByte(ushort word)
        {
            return (byte)word;
        }

        public static byte HiHiByte(uint dword)
        {
            return (byte)(dword >> 24);
        }

        public static byte LoHiByte(uint dword)
        {
            return (byte)(dword >> 16);
        }

        public static byte SubIndexMask(uint dword)
        {
            return (byte)(dword >> 8);
        }

        public static byte LoLoByte(uint dword)
        {
            return (byte)(dword & 0xff);
        }
        #endregion
    }
}
