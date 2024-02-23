using System;
using System.Globalization;

namespace AM_WinForms.Datam
{
    /// <summary>
    /// This class holds utility functions for use in different forms
    /// </summary>
    public static class Functions
    {
        #region String Formating --
        /// <summary>
        /// This method converts an Uint16 to a bitwise string.
        /// </summary>
        /// <param name="value">The uint16 value to convert.</param>
        /// <returns>a string representation of the given uint16.</returns>
        public static string ConvertUintTo16BitBinaryString(uint value)
        {
            return ConvertUintToXBitBinaryString(value, 16);
        }

        public static string ConvertUintToXBitBinaryString(uint value, int length)
        {
            try
            {
                string binValue = Convert.ToString(value, 2);
                if (binValue.Length < length)
                {
                    return binValue.PadLeft(length, '0');
                }
                else if (binValue.Length > length)
                {
                    return binValue.Substring(binValue.Length - length, length);
                }
                return binValue;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region Function Generation --
        /// <summary>
        /// This method generates a sinusoidal waveform with the min and max values
        /// provided in the number of samples given
        /// </summary>
        /// <param name="amplitude">The amplitude of the waveform</param>
        /// <param name="sampleSize">The number of samples in the waveform</param>
        /// <returns></returns>
        public static string[] GenerateDoubleSinusoid(double amplitude, uint sampleSize)
        {
            double radianStep = 2 * Math.PI / sampleSize;
            double atRadian = 0;
            string[] result = new string[sampleSize];
            //result[sampleSize] = "0";
            for (int atSample = 0; atSample < sampleSize; atSample++)
            {
                result[atSample] = (Math.Sin(atRadian) * amplitude).ToString(CultureInfo.InvariantCulture);
                atRadian += radianStep;
            }
            return result;
        }

        /// <summary>
        /// This method generates a sinusoidal waveform with the min and max values
        /// provided in the number of samples given
        /// </summary>
        /// <param name="amplitude">The amplitude of the waveform</param>
        /// <param name="sampleSize">The number of samples in the waveform</param>
        /// <returns></returns>
        public static string[] GenerateDoubleCosine(double amplitude, uint sampleSize)
        {
            double radianStep = 2 * Math.PI / sampleSize;
            double atRadian = 0;
            string[] result = new string[sampleSize];
            //result[sampleSize] = "0";
            for (int atSample = 0; atSample < sampleSize; atSample++)
            {
                result[atSample] = (Math.Cos(atRadian) * amplitude).ToString(CultureInfo.InvariantCulture);
                atRadian += radianStep;
            }
            return result;
        }

        /// <summary>
        /// This generates a square wave with a positive and negative value that of the amplitude requested.
        /// </summary>
        /// <param name="amplitude"></param>
        /// <param name="sampleSize"></param>
        /// <returns></returns>
        public static string[] GenerateDoubleSquare(double amplitude, uint sampleSize)
        {

            string[] result = new string[sampleSize];
            //result[sampleSize] = "0";
            for (int atSample = 0; atSample < sampleSize; atSample++)
            {
                if (atSample < sampleSize / 2)
                {
                    result[atSample] = amplitude.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    result[atSample] = (amplitude * -1).ToString(CultureInfo.InvariantCulture);
                }
            }
            return result;
        }

        /// <summary>
        /// This method generates a ramp waveform with the min and max values
        /// provided in the number of samples given
        /// </summary>
        /// <param name="amplitude">The amplitude of the waveform</param>
        /// <param name="sampleSize">The number of samples in the waveform</param>
        /// <returns></returns>
        public static string[] GenerateDoubleRamp(double amplitude, uint sampleSize)
        {
            amplitude = Math.Abs(amplitude);//so the min/max works out
            double ampStep = 4 * amplitude / sampleSize;
            double atApmlitude = 0;
            string[] result = new string[sampleSize + 1];
            result[0] = "0";
            result[sampleSize] = "0";
            byte state = 0;
            for (uint atSample = 1; atSample < sampleSize; atSample++)
            {
                switch (state)
                {
                    case 0://ramp up 
                        atApmlitude += ampStep;
                        result[atSample] = atApmlitude.ToString("D7");
                        if (atApmlitude >= amplitude)
                        {
                            state = 1;
                        }
                        break;
                    case 1://ramp down
                        atApmlitude -= ampStep;
                        result[atSample] = atApmlitude.ToString("D7");
                        if (atApmlitude <= -amplitude)
                        {
                            state = 0;
                        }
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// This method generates a sinusoidal waveform with the min and max values
        /// provided in the number of samples given
        /// </summary>
        /// <param name="amplitude">The amplitude of the waveform</param>
        /// <param name="sampleSize">The number of samples in the waveform</param>
        /// <returns></returns>
        public static string[] GenerateIntegerSinusoid(double amplitude, uint sampleSize)
        {
            double radianStep = 2 * Math.PI / sampleSize;
            double atRadian = 0;
            string[] result = new string[sampleSize + 1];
            result[sampleSize] = "0";
            for (uint atSample = 0; atSample < sampleSize; atSample++)
            {
                result[atSample] = ((int)Math.Ceiling(Math.Sin(atRadian) * amplitude)).ToString(CultureInfo.InvariantCulture);
                atRadian += radianStep;
            }
            return result;
        }

        /// <summary>
        /// This method generates a ramp waveform with the min and max values
        /// provided in the number of samples given
        /// </summary>
        /// <param name="amplitude">The amplitude of the waveform</param>
        /// <param name="sampleSize">The number of samples in the waveform</param>
        /// <returns></returns>
        public static string[] GenerateIntegerRamp(double amplitude, uint sampleSize)
        {
            amplitude = Math.Abs(amplitude);//so the min/max works out
            double ampStep = 4 * amplitude / sampleSize;
            double atApmlitude = 0;
            string[] result = new string[sampleSize + 1];
            result[0] = "0";
            result[sampleSize] = "0";
            byte state = 0;
            for (uint atSample = 1; atSample < sampleSize; atSample++)
            {
                switch (state)
                {
                    case 0://ramp up 
                        atApmlitude += ampStep;
                        result[atSample] = ((int)Math.Ceiling(atApmlitude)).ToString(CultureInfo.InvariantCulture);
                        if (atApmlitude >= amplitude)
                        {
                            state = 1;
                        }
                        break;
                    case 1://ramp down
                        atApmlitude -= ampStep;
                        result[atSample] = ((int)Math.Ceiling(atApmlitude)).ToString(CultureInfo.InvariantCulture);
                        if (atApmlitude <= -amplitude)
                        {
                            state = 0;
                        }
                        break;
                }
            }
            return result;
        }
        #endregion
    }
}
