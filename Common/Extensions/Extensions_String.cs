using Common.Constant;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common.Extensions
{
    #region Enumerations

    #region Decimal Mark
    public enum DecimalMark : byte
    {
        Comma,
        Period,
        Unknown
    }
    #endregion

    #region SignificantFigures
    public enum SignificantFigures : byte
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }
    #endregion

    #endregion

    public static class Extensions_String
    {
        #region Enumeration 
        public static String StringFromEnum(this Enum e)
        {
            return Convert.ToChar(e).ToString();
        }
        #endregion

        #region Trim
        /// <summary>
        /// Optimized trim -> faster than .NET Trim by more than 2x.
        /// TODO: Test?
        /// </summary>
        /// <param name="str">String from which to remove null charaters.</param>
        /// <returns>String with null terminal charaters removed.</returns>
        public static String Optimized_TrimNull(this String str)
        {
            int removeLength = 0;// Incremented if a char we want to remove is seen at the end
            for (int i = str.Length - 1; i >= 0; i--)
            {
                char c = str[i];
                if (c == Tokens.TERMINAL_CHAR || c == Tokens._s_)
                {
                    removeLength++;
                }
                else
                {
                    break;// Found a good char, so we stop
                }
            }
            if (removeLength > 0)
            {
                return str.Substring(0, str.Length - removeLength);// Removes the length containing what we don't want
            }
            return str;
        }
        #endregion /Trim

        #region Case

        #region Camel
        public static String ToCamelCase(this String formattableString, String prefix = "", String suffix = "")
        {
            formattableString += suffix; //To be camel cased, but also in case it's 'oops all suffix'
            if (String.IsNullOrEmpty(formattableString))
            {
                return String.Empty;
            }
            StringBuilder ouputString = new StringBuilder();
            bool lastNull;
            String atString;
            String[] splitFormattableString = formattableString.Split(Tokens._s_);
            int sfs;
            if(String.IsNullOrWhiteSpace(prefix))
            {
                sfs = 1;
                ouputString.Append(splitFormattableString[0].ToLower());
            }
            else
            {
                sfs = 0;
                ouputString.Append(prefix.ToLower());
            }
            for (; sfs < splitFormattableString.Length; sfs++)
            {
                atString = splitFormattableString[sfs].Trim();
                lastNull = String.IsNullOrWhiteSpace(atString);
                if (!lastNull)
                {
                    if (atString.Length > 1)
                    {// rn we only care about more than one char words.
                        ouputString.Append($"{Char.ToUpper(atString[0])}{atString.Substring(1).ToLower()}");
                    }
                }
            }
            return ouputString.ToString();
        }
        #endregion /Camel

        #region Title
        public static String ToTitleCase(this String formattableString)
        {
            if(String.IsNullOrEmpty(formattableString))
            {
                return String.Empty;
            }
            StringBuilder outputString = new StringBuilder();
            bool lastNull = true;
            String atString;
            String[] splitFormattableString = formattableString.Split(Tokens._s_);
            for(int sfs = 0; sfs < splitFormattableString.Length; sfs++)
            {
                if (!lastNull)
                {// We need a space between. (these bitter lies.. TODO: is that the lyrics to that Dave Mattherws song??)
                    outputString.Append(Tokens._S_);
                }
                atString = splitFormattableString[sfs].Trim();
                lastNull = String.IsNullOrEmpty(atString);
                if (!lastNull)
                {
                    //if(atString.Length > 1)
                    //{// rn we only care about more than one char words.
                        outputString.Append($"{Char.ToUpper(atString[0])}{atString.Substring(1).ToLower()}");
                    //}
                }
            }
            return outputString.ToString();
        }
        #endregion

        #endregion /Case

        #region Array
        public static String[] MakeStringArray(int size)
        {
            return new String[size];
        }

        public static String[] MakeStringArray(params String[] entries)
        {
            return entries;
        }
        #endregion

        #region Concatonate
        public static String ConcatWithSeperator_Char(this String[] values, Char seperator = Tokens._s_)
        {
            StringBuilder builder = new StringBuilder();
            if (values != null)
            {
                builder.Append(values[0]);
                if (values.Length > 1)
                {
                    for (int v = 1; v < values.Length; v++)
                    {
                        builder.Append(seperator);
                        builder.Append(values[v]);
                    }
                }
            }
            return builder.ToString();
        }

        public static String ConcatWithSeperator_String(this String[] values, String seperator = Tokens._S_)
        {
            StringBuilder builder = new StringBuilder();
            if (values != null)
            {
                builder.Append(values[0]);
                if (values.Length > 1)
                {
                    for (int v = 1; v < values.Length; v++)
                    {
                        builder.Append(seperator);
                        builder.Append(values[v]);
                    }
                }
            }
            return builder.ToString();
        }
        #endregion

        #region Matching
        public static bool IsMatch(params String[] strings)
        {
            if(strings != null && strings.Count() > 1)
            {
                string first = strings.First();
                for(int s = 1; s < strings.Count(); s++)
                {
                    if(!IsMatch(first, strings[s]))
                    {
                        return false; // It only takes that one.
                    }
                }
                return true;
            }
            else
            {// Nothing to compare.
                return true;
            }
        }

        public static bool IsMatch(this String str1, String str2)
        {
            return IsMatch_Lower(str1, str2);
        }

        public static bool IsMatch_Lower(this String str1, String str2)
        {
            return str1.Trim().ToLower() == str2.Trim().ToLower();
        }
        #endregion

        #region Quotation Marks

        #region Add
        public static String AddRequiredQuotes(this String str)
        {
            if (str != null && str.Length > 0)
            {
                if (str[0] != '"')
                {
                    str = $"{Tokens.FullQuotation}{str}";
                }
                if (str[str.Length - 1] != '"')
                {
                    str = $"{str}{Tokens.FullQuotation}";
                }
            }
            else
            {
                str = "\"\"";
            }
            return str;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Remove the quotes, as the raven said, make them nevermore.
        /// </summary>
        /// <param name="str">The string from which to remove the quotes.</param>
        /// <returns>String without quotes at the terminal points.</returns>
        public static void StripQuotes(ref String str)
        {
            str = str.StripQuotes();
        }

        /// <summary>
        /// Remove the quotes, as the raven said, make them nevermore.
        /// </summary>
        /// <param name="str">The string from which to remove the quotes.</param>
        /// <returns>String without quotes at the terminal points.</returns>
        public static String StripQuotes(this String str)
        {
            return str.Trim('"');
        }
        #endregion

        #endregion /Quotation Marks

        #region Reverse
        public static String Reverse(this String s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        #endregion

        #region Character Swap
        public static String Swap(this String str, char char1, char char2)
        {
            return new String(str.Select(c => c == char1 ? char2 : (c == char2 ? char1 : c)).ToArray());
        }
        #endregion

        #region Numeric Formatting

        #region Kilo, Mega, Billion (KMB)
        /// <summary>
        /// This string formatter converts the given double to a string with 
        /// larger numbers using K for thousand, M for millions, and B for billions.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToNumericStringFormat_KMB(double num)
        {
            try
            {
                if (num < 1E3)
                {
                    return String.Format("{0:F1}", num);
                }
                else if (num >= 1E3 && num < 1E6)
                {
                    return String.Format("{0:F1}K", num / 1E3);
                }
                else if (num >= 1E6 && num < 1E9)
                {
                    return String.Format("{0:F1}M", num / 1E6);
                }
                else if (num >= 1E9)
                {
                    return String.Format("{0:F1}B", num / 1E9);
                }
                else
                {
                    return String.Format("{0:F1}", num);
                }
            }
            catch
            {
                return num.ToString();
            }
        }
        #endregion

        #region Decimal Mark
        public static void ConvertToPeriodDecimal(ref String value)
        {
            value = ConvertToPeriodDecimal(value);
        }

        public static String ConvertToPeriodDecimal(String value)
        {
            DecimalMark decimalMark = DetermineDecimalMark(value);
            switch (decimalMark)
            {
                case DecimalMark.Comma:
                    return value.Swap(',', '.');
                case DecimalMark.Period:
                case DecimalMark.Unknown:
                default:
                    return value;
            }
        }

        public static String ConvertDecimalMark(String value, DecimalMark desiredDecimalMark)
        {
            DecimalMark decimalMark = DetermineDecimalMark(value);
            if (desiredDecimalMark != decimalMark && decimalMark != DecimalMark.Unknown)
            {// We need to change the mark, if they are not the same.... they must be opposite.
                return value.Swap(',', '.');
            }
            return value;
        }

        public static DecimalMark DetermineDecimalMark(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value[0] == '.')
                {
                    return DecimalMark.Period;
                }
                if (value[0] == ',')
                {
                    return DecimalMark.Comma;
                }
                bool containsPeriod = value.Contains('.');
                bool containsComma = value.Contains(',');
                if (!(containsPeriod || containsComma))
                {// No conversion required
                    return DecimalMark.Unknown;
                }
                string[] allCommaSplits = value.Split(',');
                if (allCommaSplits.Length > 2)
                {//We have more than one comma, so this is 'Decimal Point' or a bad value (we dont handle bad here)
                    return DecimalMark.Period;
                }
                string[] allPeriodSplits = value.Split('.');
                if (allPeriodSplits.Length > 2)
                {//We have more than one decimal, so this is 'Comma Point' or a bad value (we dont handle bad here)
                    return DecimalMark.Comma;
                }
                if (containsPeriod && containsComma)
                {
                    if (allCommaSplits.Last().Contains('.'))
                    {// We have commas leading decimals, so this is 'Decimal Point' or a bad value (we dont handle bad here)
                        return DecimalMark.Period;
                    }
                    else if (allCommaSplits[0].Contains('.'))
                    {// We have the decimals leading commas so this is 'Decimal Comma' or a bad value (we dont handle bad here) 
                        return DecimalMark.Comma;
                    }
                }
                if (containsComma)
                {
                    if (allCommaSplits[0].Length < 2)
                    {
                        bool isPeriodDecimal = true;
                        for (int i = 1; i < allCommaSplits.Length - 1; i++)
                        {
                            if (allCommaSplits.Length != 3)
                            {
                                isPeriodDecimal = false;
                                break;
                            }
                        }
                        if (isPeriodDecimal)
                        {
                            return DecimalMark.Period;// we have proper formatting for decimal periods
                        }
                    }
                    return DecimalMark.Comma;// We have more than 3 chars between commas so this is 'Decimal Comma' or a bad value (we dont break bad here) 
                }
                if (containsPeriod)
                {
                    if (allPeriodSplits[0].Length < 2)
                    {
                        bool isCommaDecimal = true;
                        for (int i = 1; i < allPeriodSplits.Length - 1; i++)
                        {
                            if (allPeriodSplits.Length != 3)
                            {
                                isCommaDecimal = false;
                                break;
                            }
                        }
                        if (isCommaDecimal)
                        {
                            return DecimalMark.Comma;// we have proper formatting for decimal periods
                        }
                    }
                    return DecimalMark.Period;// We have more than 3 chars between commas so this is 'Decimal Comma' or a bad value (we dont f wit bad here) 
                }
            }
            return DecimalMark.Unknown;
        }


        #endregion /Decimal Mark

        #region Check
        public static bool IsNumeric_Int(this String intString)
        {
            return int.TryParse(intString, out _);
        }

        public static bool IsNumeric_UInt(this String uintString)
        {
            return uint.TryParse(uintString, out _);
        }
        #endregion

        #endregion /Numeric Formatting

        #region Measurement
        /// <summary>
        /// Wrapper for TextRenderer class MeasureText method
        /// </summary>
        /// <param name="text">Text to measure.</param>
        /// <param name="font">Font of the text to measure.</param>
        /// <returns>Size of the text.</returns>
        public static Size MeasureText(this String text, Font font)
        {
            return TextRenderer.MeasureText(text, font);
        }

        /// <summary>
        /// Wrapper for TextRenderer class MeasureText method
        /// </summary>
        /// <param name="text">Text to measure.</param>
        /// <param name="font">Font of the text to measure.</param>
        /// <returns>Height of the text.</returns>
        public static int MeasureText_Height(this String text, Font font, int padding = 0)
        {
            return TextRenderer.MeasureText(text, font).Height + padding;
        }

        /// <summary>
        /// Wrapper for TextRenderer class MeasureText method
        /// </summary>
        /// <param name="text">Text to measure.</param>
        /// <param name="font">Font of the text to measure.</param>
        /// <returns>Width of the text.</returns>
        public static int MeasureText_Width(this String text, Font font, int padding = 0)
        {
            return TextRenderer.MeasureText(text, font).Width + padding;
        }

        public static int GetLongestWidthSequence_SpaceDelimited(this String text, Font font, int padding = 0)
        {
            return text.GetLongestWidthSequence_CharDelimited(font, Tokens._s_, padding);
        }

        /// <summary>
        /// This method will return the longest char string delimeted by the given delimeter in the given string using the given font.
        /// It will return the integer length in pixels with the given padding added. 
        /// </summary>
        /// <param name="text">Text to delimit and measure non delimiter string sequences.</param>
        /// <param name="font">Font used to draw the given string.</param>
        /// <param name="delimiter">Char used to seperate sequences in the given string.</param>
        /// <param name="padding">Padding to add to the result.</param>
        /// <returns></returns>
        public static int GetLongestWidthSequence_CharDelimited(this String text, Font font, Char delimiter, int padding = 0 )
        {
            if(String.IsNullOrEmpty(text))
            {
                return padding;
            }
            string delimiterStr = delimiter.ToString();
            int maxLength = 0;
            string[] subStrings = text.Split(delimiter);
            for (int ss = 0; ss < subStrings.Length; ss++)
            {
                maxLength = Math.Max(maxLength, subStrings[ss].Replace(delimiterStr, String.Empty).MeasureText_Width(font, padding));
            }
            return maxLength + padding;
        }
        #endregion /Measurement

        #region Number to String
        /// <summary>
        /// This method attempts to take an unsigned integer and convert it to
        /// a string in the given format, it will return its suceess or failure as a boolean.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="format">Format to use in string formatter.</param>
        /// <param name="result">The result of the conversion, empty string if unsucessful.</param>
        /// <returns>True if successful, else false.</returns>
        public static bool TryConvertUintToString(uint value, string format, out string result)
        {
            try
            {
                result = value.ToString(format, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                result = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// This method takes an unsigned integer and converts it to
        /// a string in the given format
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static String ConvertUintToString(uint value, string format)
        {
            try
            {
                return value.ToString(format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// This method attempts to take an unsigned integer and convert it to
        /// a string in the given format, it will return its suceess or failure as a boolean.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="format">Format to use in string formatter.</param>
        /// <param name="result">The result of the conversion, empty string if unsucessful.</param>
        /// <returns>True if successful, else false.</returns>
        public static bool TryConvertIntToString(int value, string format, out string result)
        {
            try
            {
                result = value.ToString(format, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                result = string.Empty;
                return false;
            }
        }


        /// <summary>
        /// This method takes an integer and converts it to
        /// a string in the given format
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static String ConvertIntToString(int value, string format) //TODO, constants for named formats
        {
            try
            {
                return value.ToString(format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion /Number to String

        #region Format
        /// <summary>
        /// This method formats the gauge output appropriately to display a bool
        /// </summary>
        public static String FormatOutput_Bool(bool boolValue)
        {
            return boolValue ? Tokens.TRUE : Tokens.FALSE;
        }

        /// <summary>
        /// This method formats the gauge output appropriately to display a double
        /// </summary>
        public static String FormatOutput_Double(String strValue, SignificantFigures significantFigures = SignificantFigures.Two)
        {
            if (Double.TryParse(strValue, out double value))
            {
                if (strValue.Contains('.'))
                {// We want a leading zero
                    return String.Format(CultureInfo.InvariantCulture, "{0:0.000}", value);
                }
                else
                {// We want at least one decimal value, even if zero

                    return FormatOutput_Double_Common(value, significantFigures);
                }
            }
            return Tokens._0;
        }

        /// <summary>
        /// This method formats the gauge output appropriately to display a double
        /// </summary>
        public static String FormatOutput_Double(Double value, SignificantFigures significantFigures = SignificantFigures.Two)
        {
            return FormatOutput_Double_Common(value, significantFigures);
        }

        private static String FormatOutput_Double_Common(Double value, SignificantFigures significantFigures = SignificantFigures.Two)
        {
            switch(significantFigures)
            {
                case SignificantFigures.None:
                    return String.Format(CultureInfo.InvariantCulture, "{0:0}", value); 
                case SignificantFigures.One:
                    return String.Format(CultureInfo.InvariantCulture, "{0:0}.0", value); 
                case SignificantFigures.Two:
                    return String.Format(CultureInfo.InvariantCulture, "{0:0}.00", value); 
                case SignificantFigures.Three:
                    return String.Format(CultureInfo.InvariantCulture, "{0:0}.000", value); 
                case SignificantFigures.Four:
                    return String.Format(CultureInfo.InvariantCulture, "{0:0}.0000", value);
                default:
                    return value.ToString();
            }
        }

        #endregion
    }
}
