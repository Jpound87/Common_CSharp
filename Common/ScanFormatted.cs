using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Common
{
    #region Enumerations
    // Format type specifiers
    internal enum Types
    {
        Character,
        Decimal,
        Float,
        Hexadecimal,
        Octal,
        ScanSet,
        String,
        Unsigned
    }

    // Format modifiers
    internal enum Modifiers
    {
        None,
        ShortShort,
        Short,
        Long,
        LongLong
    }
    #endregion

    #region Deleagate
    // Delegate to parse a type
    internal delegate bool ParseValue(TextParser input, FormatSpecifier spec);
    #endregion

    #region Structs

    #region Format Specifier
    // Struct to hold format specifier information
    internal struct FormatSpecifier
    {
        #region Identity
        public const String ClassName = nameof(FormatSpecifier);
        #endregion

        #region Accessors
        public Types Type { get; set; }
        public Modifiers Modifier { get; set; }
        public int Width { get; set; }
        public bool NoResult { get; set; }
        public string ScanSet { get; set; }
        public bool ScanSetExclude { get; set; }
        #endregion
    }
    #endregion

    #region Type Parser
    // Struct to associate format type with type parser
    internal struct TypeParser
    {
        public Types Type { get; set; }
        internal ParseValue Parser { get; set; }
    }
    #endregion

    #endregion /Structs

    /// <summary>
    /// Class that provides functionality of the standard C library sscanf()
    /// function.
    /// </summary>
    public class ScanFormatted
    {
        #region Identity
        public const String ClassName = nameof(ScanFormatted);
        #endregion

        #region Readonly
        // Lookup table to find parser by parser type
        internal readonly TypeParser[] typeParsers;
        #endregion

        #region Accessors
        // Holds results after calling Parse()
        public List<object> Results { get; set; }
        #endregion

        #region Constructor
        public ScanFormatted()
        {
            // Populate parser type lookup table
            typeParsers = new TypeParser[]
            {
                new TypeParser() { Type = Types.Character, Parser = Parse_Character },
                new TypeParser() { Type = Types.Decimal, Parser = Parse_Decimal },
                new TypeParser() { Type = Types.Float, Parser = Parse_Float },
                new TypeParser() { Type = Types.Hexadecimal, Parser = Parse_Hexadecimal },
                new TypeParser() { Type = Types.Octal, Parser = Parse_Octal },
                new TypeParser() { Type = Types.ScanSet, Parser = Parse_ScanSet },
                new TypeParser() { Type = Types.String, Parser = Parse_String },
                new TypeParser() { Type = Types.Unsigned, Parser = Parse_Decimal }
            };

            // Allocate results collection
            Results = new List<object>();
        }
        #endregion

        #region Parse

        #region Generic
        /// <summary>
        /// Parses the input string according to the rules in the
        /// format string. Similar to the standard C library's
        /// sscanf() function. Parsed fields are placed in the
        /// class' Results member.
        /// </summary>
        /// <param name="input">String to parse</param>
        /// <param name="format">Specifies rules for parsing input</param>
        public int Parse(string input, string format)
        {
            TextParser inp = new TextParser(input);
            TextParser fmt = new TextParser(format);
            List<object> results = new List<object>();
            FormatSpecifier spec = new FormatSpecifier();
            int count = 0;

            // Clear any previous results
            Results.Clear();

            // Process input string as indicated in format string
            while (!fmt.EndOfText && !inp.EndOfText)
            {
                if (Parse_FormatSpecifier(fmt, spec))
                {
                    // Found a format specifier
                    TypeParser parser = typeParsers.First(tp => tp.Type == spec.Type);
                    if (parser.Parser(inp, spec))
                        count++;
                    else
                        break;
                }
                else if (Char.IsWhiteSpace(fmt.Peek()))
                {
                    // Whitespace
                    inp.MovePastWhitespace();
                    fmt.MoveAhead();
                }
                else if (fmt.Peek() == inp.Peek())
                {
                    // Matching character
                    inp.MoveAhead();
                    fmt.MoveAhead();
                }
                else break;    // Break at mismatch
            }

            // Return number of fields successfully parsed
            return count;
        }
        #endregion /Generic

        #region Format Specifier
        /// <summary>
        /// Attempts to parse a field format specifier from the format string.
        /// </summary>
        internal bool Parse_FormatSpecifier(TextParser format, FormatSpecifier spec)
        {
            // Return if not a field format specifier
            if (format.Peek() != '%')
                return false;
            format.MoveAhead();

            // Return if "%%" (treat as '%' literal)
            if (format.Peek() == '%')
                return false;

            // Test for asterisk, which indicates result is not stored
            if (format.Peek() == '*')
            {
                spec.NoResult = true;
                format.MoveAhead();
            }
            else spec.NoResult = false;

            // Parse width
            int start = format.Position;
            while (Char.IsDigit(format.Peek()))
                format.MoveAhead();
            if (format.Position > start)
                spec.Width = int.Parse(format.Extract(start, format.Position));
            else
                spec.Width = 0;

            // Parse modifier
            if (format.Peek() == 'h')
            {
                format.MoveAhead();
                if (format.Peek() == 'h')
                {
                    format.MoveAhead();
                    spec.Modifier = Modifiers.ShortShort;
                }
                else spec.Modifier = Modifiers.Short;
            }
            else if (Char.ToLower(format.Peek()) == 'l')
            {
                format.MoveAhead();
                if (format.Peek() == 'l')
                {
                    format.MoveAhead();
                    spec.Modifier = Modifiers.LongLong;
                }
                else spec.Modifier = Modifiers.Long;
            }
            else spec.Modifier = Modifiers.None;

            // Parse type
            switch (format.Peek())
            {
                case 'c':
                    spec.Type = Types.Character;
                    break;
                case 'd':
                case 'i':
                    spec.Type = Types.Decimal;
                    break;
                case 'a':
                case 'A':
                case 'e':
                case 'E':
                case 'f':
                case 'F':
                case 'g':
                case 'G':
                    spec.Type = Types.Float;
                    break;
                case 'o':
                    spec.Type = Types.Octal;
                    break;
                case 's':
                    spec.Type = Types.String;
                    break;
                case 'u':
                    spec.Type = Types.Unsigned;
                    break;
                case 'x':
                case 'X':
                    spec.Type = Types.Hexadecimal;
                    break;
                case '[':
                    spec.Type = Types.ScanSet;
                    format.MoveAhead();
                    // Parse scan set characters
                    if (format.Peek() == '^')
                    {
                        spec.ScanSetExclude = true;
                        format.MoveAhead();
                    }
                    else spec.ScanSetExclude = false;
                    start = format.Position;
                    // Treat immediate ']' as literal
                    if (format.Peek() == ']')
                        format.MoveAhead();
                    format.MoveTo(']');
                    if (format.EndOfText)
                        throw new Exception("Type specifier expected character : ']'");
                    spec.ScanSet = format.Extract(start, format.Position);
                    break;
                default:
                    string msg = String.Format("Unknown format type specified : '{0}'", format.Peek());
                    throw new Exception(msg);
            }
            format.MoveAhead();
            return true;
        }
        #endregion /Format Specifier

        #region Char
        /// <summary>
        /// Parse a character field
        /// </summary>
        private bool Parse_Character(TextParser input, FormatSpecifier spec)
        {
            // Parse character(s)
            int start = input.Position;
            int count = (spec.Width > 1) ? spec.Width : 1;
            while (!input.EndOfText && count-- > 0)
                input.MoveAhead();

            // Extract token
            if (count <= 0 && input.Position > start)
            {
                if (!spec.NoResult)
                {
                    string token = input.Extract(start, input.Position);
                    if (token.Length > 1)
                        Results.Add(token.ToCharArray());
                    else
                        Results.Add(token[0]);
                }
                return true;
            }
            return false;
        }
        #endregion /Char

        #region Decimal
        /// <summary>
        /// Parse integer field
        /// </summary>
        private bool Parse_Decimal(TextParser input, FormatSpecifier spec)
        {
            int radix = 10;

            // Skip any whitespace
            input.MovePastWhitespace();

            // Parse leading sign
            int start = input.Position;
            if (input.Peek() == '+' || input.Peek() == '-')
            {
                input.MoveAhead();
            }
            else if (input.Peek() == '0')
            {
                if (Char.ToLower(input.Peek(1)) == 'x')
                {
                    radix = 16;
                    input.MoveAhead(2);
                }
                else
                {
                    radix = 8;
                    input.MoveAhead();
                }
            }

            // Parse digits
            while (IsValidDigit(input.Peek(), radix))
                input.MoveAhead();

            // Don't exceed field width
            if (spec.Width > 0)
            {
                int count = input.Position - start;
                if (spec.Width < count)
                    input.MoveAhead(spec.Width - count);
            }

            // Extract token
            if (input.Position > start)
            {
                if (!spec.NoResult)
                {
                    if (spec.Type == Types.Decimal)
                        AddSigned(input.Extract(start, input.Position), spec.Modifier, radix);
                    else
                        AddUnsigned(input.Extract(start, input.Position), spec.Modifier, radix);
                }
                return true;
            }
            return false;
        }
        #endregion /Decimal

        #region Floating Point
        /// <summary>
        /// Parse a floating-point field
        /// </summary>
        private bool Parse_Float(TextParser input, FormatSpecifier spec)
        {
            // Skip any whitespace
            input.MovePastWhitespace();

            // Parse leading sign
            int start = input.Position;
            if (input.Peek() == '+' || input.Peek() == '-')
                input.MoveAhead();

            // Parse digits
            bool hasPoint = false;
            while (Char.IsDigit(input.Peek()) || input.Peek() == '.')
            {
                if (input.Peek() == '.')
                {
                    if (hasPoint)
                        break;
                    hasPoint = true;
                }
                input.MoveAhead();
            }

            // Parse exponential notation
            if (Char.ToLower(input.Peek()) == 'e')
            {
                input.MoveAhead();
                if (input.Peek() == '+' || input.Peek() == '-')
                    input.MoveAhead();
                while (Char.IsDigit(input.Peek()))
                    input.MoveAhead();
            }

            // Don't exceed field width
            if (spec.Width > 0)
            {
                int count = input.Position - start;
                if (spec.Width < count)
                    input.MoveAhead(spec.Width - count);
            }

            // Because we parse the exponential notation before we apply
            // any field-width constraint, it becomes awkward to verify
            // we have a valid floating point token. To prevent an
            // exception, we use TryParse() here instead of Parse().

            // Extract token
            if (input.Position > start && Utility_General.TryConvertFromNumberString_ToDouble(
                    input.Extract(start, input.Position),
                    out double result,
                    CultureInfo.InvariantCulture))
            {
                if (!spec.NoResult)
                {
                    if (spec.Modifier == Modifiers.Long ||
                        spec.Modifier == Modifiers.LongLong)
                        Results.Add(result);
                    else
                        Results.Add((float)result);
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Hexidecimal

        /// <summary>
        /// Parse hexadecimal field
        /// </summary>
        internal bool Parse_Hexadecimal(TextParser input, FormatSpecifier spec)
        {
            // Skip any whitespace
            input.MovePastWhitespace();

            // Parse 0x prefix
            int start = input.Position;
            if (input.Peek() == '0' && input.Peek(1) == 'x')
                input.MoveAhead(2);

            // Parse digits
            while (IsValidDigit(input.Peek(), 16))
                input.MoveAhead();

            // Don't exceed field width
            if (spec.Width > 0)
            {
                int count = input.Position - start;
                if (spec.Width < count)
                    input.MoveAhead(spec.Width - count);
            }

            // Extract token
            if (input.Position > start)
            {
                if (!spec.NoResult)
                    AddUnsigned(input.Extract(start, input.Position), spec.Modifier, 16);
                return true;
            }
            return false;
        }
        #endregion /Hexidecimal

        #region Octal
        /// <summary>
        /// Parse an octal field
        /// </summary>
        private bool Parse_Octal(TextParser input, FormatSpecifier spec)
        {
            // Skip any whitespace
            input.MovePastWhitespace();

            // Parse digits
            int start = input.Position;
            while (IsValidDigit(input.Peek(), 8))
                input.MoveAhead();

            // Don't exceed field width
            if (spec.Width > 0)
            {
                int count = input.Position - start;
                if (spec.Width < count)
                    input.MoveAhead(spec.Width - count);
            }

            // Extract token
            if (input.Position > start)
            {
                if (!spec.NoResult)
                    AddUnsigned(input.Extract(start, input.Position), spec.Modifier, 8);
                return true;
            }
            return false;
        }
        #endregion /Octal

        #region Scan Set
        /// <summary>
        /// Parse a scan-set field
        /// </summary>
        internal bool Parse_ScanSet(TextParser input, FormatSpecifier spec)
        {
            // Parse characters
            int start = input.Position;
            if (!spec.ScanSetExclude)
            {
                while (spec.ScanSet.Contains(input.Peek()))
                    input.MoveAhead();
            }
            else
            {
                while (!input.EndOfText && !spec.ScanSet.Contains(input.Peek()))
                    input.MoveAhead();
            }

            // Don't exceed field width
            if (spec.Width > 0)
            {
                int count = input.Position - start;
                if (spec.Width < count)
                    input.MoveAhead(spec.Width - count);
            }

            // Extract token
            if (input.Position > start)
            {
                if (!spec.NoResult)
                    Results.Add(input.Extract(start, input.Position));
                return true;
            }
            return false;
        }
        #endregion /Scan Set

        #region String
        /// <summary>
        /// Parse a string field
        /// </summary>
        private bool Parse_String(TextParser input, FormatSpecifier spec)
        {
            // Skip any whitespace
            input.MovePastWhitespace();

            // Parse string characters
            int start = input.Position;
            while (!input.EndOfText && !Char.IsWhiteSpace(input.Peek()))
                input.MoveAhead();

            // Don't exceed field width
            if (spec.Width > 0)
            {
                int count = input.Position - start;
                if (spec.Width < count)
                    input.MoveAhead(spec.Width - count);
            }

            // Extract token
            if (input.Position > start)
            {
                if (!spec.NoResult)
                    Results.Add(input.Extract(start, input.Position));
                return true;
            }
            return false;
        }
        #endregion /String

        #endregion /Parse

        #region Valid Digit
        // Determines if the given digit is valid for the given radix
        private bool IsValidDigit(char c, int radix)
        {
            int i = "0123456789abcdef".IndexOf(Char.ToLower(c));
            if (i >= 0 && i < radix)
                return true;
            return false;
        }
        #endregion

        #region Signed Tagging
        // Parse signed token and add to results
        private void AddSigned(string token, Modifiers mod, int radix)
        {
            object obj;
            if (mod == Modifiers.ShortShort)
                obj = Convert.ToSByte(token, radix);
            else if (mod == Modifiers.Short)
                obj = Convert.ToInt16(token, radix);
            else if (mod == Modifiers.Long ||
                mod == Modifiers.LongLong)
                obj = Convert.ToInt64(token, radix);
            else
                obj = Convert.ToInt32(token, radix);
            Results.Add(obj);
        }

        // Parse unsigned token and add to results
        private void AddUnsigned(string token, Modifiers mod, int radix)
        {
            object obj;
            if (mod == Modifiers.ShortShort)
                obj = Convert.ToByte(token, radix);
            else if (mod == Modifiers.Short)
                obj = Convert.ToUInt16(token, radix);
            else if (mod == Modifiers.Long ||
                mod == Modifiers.LongLong)
                obj = Convert.ToUInt64(token, radix);
            else
                obj = Convert.ToUInt32(token, radix);
            Results.Add(obj);
        }
        #endregion
    }
}
