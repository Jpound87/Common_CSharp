using Common.Constant;
using System;
using System.Drawing;

namespace Common
{
    public struct TextStyle
    {
        #region Identity
        public const String StructName = nameof(TextStyle); 
        #endregion

        #region Constants
        public static readonly TextStyle NEW_LINE_TEXTSTYLE = new TextStyle(Tokens.NEW_LINE_STRING);
        public static readonly TextStyle NEW_LINE_TEXTSTYLE_2 = new TextStyle(Tokens.NEW_LINE_STRING_2);
        #endregion

        #region Globals
        public PrintType PrintType { get; set; }
        public String Text { get; set; }
        public FontStyle FontStyle { get; set; }
        #endregion

        #region Constructor
        public TextStyle(String text)
        {
            Text = text;
            FontStyle = FontStyle.Regular;
            PrintType = PrintType.Regular;
        }

        public TextStyle(String text, FontStyle fontStyle)
        {
            Text = text;
            FontStyle = fontStyle;
            PrintType = PrintType.Regular;
        }

        public TextStyle(String text, PrintType printType)
        {
            Text = text;
            FontStyle = FontStyle.Regular;
            PrintType = printType;
        }

        public TextStyle(String text, FontStyle fontStyle, PrintType printType)
        {
            Text = text;
            FontStyle = fontStyle;
            PrintType = printType;
        }
        #endregion
    }
}
