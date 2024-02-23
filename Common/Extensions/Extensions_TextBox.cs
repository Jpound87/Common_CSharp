using Common.Constant;
using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Common.Extensions
{
    public static class Extensions_TextBox
    {
        #region Identity
        public const String ClassName = nameof(Extensions_TextBox);
        #endregion

        #region TextBox

        #region Resize
        public static void Resizer(this TextBox textBox, Size minSize)
        {
            Resizer(textBox, minSize.Width, minSize.Height);
        }

        public static void Resizer(this TextBox textBox, int minWidth = 30, int minHeight = 30)
        {
            Size textSize = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            if (textSize.Width >= minWidth)
            {
                textBox.Width = Math.Max(textSize.Width, textBox.MaximumSize.Width);
            }
            else
            {
                textBox.Width = minWidth;
            }
            if (textSize.Height >= minHeight)
            {
                textBox.Width = Math.Max(textSize.Height, textBox.MaximumSize.Height);
            }
            else
            {
                textBox.Width = minWidth;
            }
        }

        public static void Resizer_Width(this TextBox[] textBoxes)
        {
            int maxWidth = 0;
            foreach (TextBox textBox in textBoxes)
            {
                maxWidth = Math.Max(TextRenderer.MeasureText(textBox.Text, textBox.Font).Width, maxWidth);
            }
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Width = maxWidth;
            }
        }

        public static void Resizer_Width(this TextBox textBox, int minWidth = 30)
        {
            Size textSize = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            if (textSize.Width >= minWidth)
            {
                textBox.Width = textSize.Width;
            }
            else
            {
                textBox.Width = minWidth;
            }
        }

        public static void Resizer_Height(this TextBox textBox, int minHeight = 30)
        {
            Size textSize = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            if (textSize.Height >= minHeight)
            {
                textBox.Height = Math.Max(textSize.Height, textBox.MaximumSize.Height);
            }
            else
            {
                textBox.Height = minHeight;
            }
        }
        #endregion /Resize

        #region Remove Text
        public static void RemoveSpaceFromLive(this TextBox textbox)
        {
            textbox.Text = textbox.Text.Replace(" ", "");
            textbox.SelectionStart = textbox.Text.Length;
            textbox.SelectionLength = 0;
        }
        #endregion

        #region Text Measurement
        public static bool IsTextOverflowing_Width(this TextBox textBox)
        {
            return TextRenderer.MeasureText(textBox.Text, textBox.Font).Width > textBox.Width;
        }

        public static bool IsTextOverflowing_Width(this TextBox textBox, out int widthFactor)
        {
            int textWidth = TextRenderer.MeasureText(textBox.Text, textBox.Font).Width;
            if (textWidth > textBox.Width)
            {
                widthFactor = textWidth - textBox.Width;
                return true;
            }
            widthFactor = textBox.Width - textWidth;
            return false;
        }

        public static bool IsTextOverflowing_Height(this TextBox textBox)
        {
            return TextRenderer.MeasureText(textBox.Text, textBox.Font).Height > textBox.Height;
        }

        public static void IsTextOverflowing(this TextBox textBox, out bool width, out bool height)
        {
            Size textSize = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            width = textSize.Width > textBox.Width;
            height = textSize.Height > textBox.Height;
        }

        public static void GetTextOverflowSize(this TextBox textBox, out int width, out int height)
        {
            Size textbox_size = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            width = Math.Max(0, textBox.Width - textbox_size.Width);
            height = Math.Max(0, textBox.Height - textbox_size.Height);
        }
        #endregion / Text Measurement

        #region Size

        #region Set
        /// <summary>
        /// This method takes a combobox and adjusts its drop down 
        /// box to be wide enough to display the elements in it.
        /// </summary>
        /// <param name="textBox">The ComboBox to resize</param>
        public static void SetSizeFromContents(this TextBox textBox)
        {
            try
            {
                textBox.SuspendLayout();
                int currItemWidth = Extensions_String.MeasureText(textBox.Text, textBox.Font).Width;
                int boxWidth = currItemWidth + 17;
                if (boxWidth > textBox.Width)
                {
                    textBox.Width = boxWidth;
                }
            }
            finally
            {
                textBox.ResumeLayout();
            }
        }
        #endregion /Set

        #endregion /Size

        #region Check Text
        public static bool CheckInvalidText<T>(this TextBox textBox, ParameterType type, Color originalColor, Color invalidColor, out T value)
        {
            bool success = false;
            string strValue = textBox.Text.Trim();
            Extensions_String.ConvertToPeriodDecimal(ref strValue);
            if (String.IsNullOrEmpty(strValue))
            {
                textBox.BackColor = originalColor;
                value = default;
                return true;
            }
            switch (type)
            {
                case ParameterType.Bool:
                    success = Boolean.TryParse(strValue, out bool boolValue);
                    if (boolValue is T typeBoolValue)
                    {
                        value = typeBoolValue;
                    }
                    else
                    {
                        value = default;
                    }
                    break;
                case ParameterType.Int8:
                case ParameterType.Int16:
                case ParameterType.Int24:
                case ParameterType.Int32:
                case ParameterType.Int48:
                case ParameterType.Int56:
                case ParameterType.Int64:
                    success = Int32.TryParse(strValue, out int intValue);
                    if (intValue is T typeIntValue)
                    {
                        value = typeIntValue;
                    }
                    else
                    {
                        value = default;
                    }
                    break;
                case ParameterType.UInt8:
                case ParameterType.UInt16:
                case ParameterType.UInt24:
                case ParameterType.UInt32:
                case ParameterType.UInt48:
                case ParameterType.UInt56:
                case ParameterType.UInt64:
                    success = UInt32.TryParse(strValue, out uint uintValue);
                    if (uintValue is T typeUintValue)
                    {
                        value = typeUintValue;
                    }
                    else
                    {
                        value = default;
                    }
                    break;
                case ParameterType.Real32:
                case ParameterType.Real64:
                    success = Double.TryParse(strValue, out double doubleValue);
                    if (doubleValue is T typeDoubleValue)
                    {
                        value = typeDoubleValue;
                    }
                    else
                    {
                        value = default;
                    }
                    break;
                case ParameterType.String:
                case ParameterType.OctetString:
                case ParameterType.Unicode:
                case ParameterType.Domain:
                case ParameterType.DateTime:
                case ParameterType.TimeSpan:
                    if (strValue is T typeValue)
                    {
                        value = typeValue;
                    }
                    else
                    {
                        value = default;
                    }
                    break;
                default:
                    value = default;
                    return false;
            }
            if (success)
            {
                textBox.BackColor = originalColor;
            }
            else
            {
                textBox.BackColor = invalidColor;
            }
            return success;
        }
        #endregion /Check Text

        #region Scientific Notation
        /// <summary>
        /// This helper method is intended to be used in conjunction with the
        /// format output methods too handle the case where the number is too 
        /// long to be displayed and needs to be converted to scientific 
        /// notation in order to lower character count
        /// </summary>
        /// <param name="givenValue">String containing the output value adjusted to fit on the gauge display</param>
        /// <returns></returns>
        public static void ConvertToScientificNotation(this TextBox textBox)
        {
            double givenValueDouble = Convert.ToDouble(textBox.Text);//allows for scientific notion 
            string outputValue = string.Format(Tokens.EXPONENT_CONVERSION_STRING, givenValueDouble);
            if (outputValue.Contains(Tokens.EXPONENT_STRING))
            {
                string[] outputValueParts = outputValue.Split('E');
                StringBuilder significand = new StringBuilder(outputValueParts[0]);
                string exponent = outputValueParts[1];
                while (textBox.IsTextOverflowing_Width())
                {// While the text is too big to fit in the reduced size box, remove sig figs.
                    int at = significand.Length - 1; // Never do math twice!
                    if (significand[at] == '.')
                    {// We are coming out of decimals
                        significand.Remove(at--, 1);// Remove the decimal, then decriment at.
                    }
                    if (int.Parse(significand[at].ToString(CultureInfo.InvariantCulture)) >= 5)
                    {// We need to round up the following number.
                        int adj = 2;// Decimal and character to get rid of..
                        if (significand[0] == '-')
                        {
                            adj = 3;// Count neg sign as well..
                        }
                        significand = new StringBuilder(Math.Round(double.Parse(significand.ToString()), at - adj).ToString(CultureInfo.InvariantCulture));// C# magic                                                                                                                             
                        textBox.Text = string.Concat(significand.ToString(), 'E', exponent);
                    }
                    else
                    {
                        textBox.Text = string.Concat(significand.Remove(at, 1).ToString(), 'E', exponent);
                    }
                }
            }
        }
        #endregion /Scientific Notation

        #endregion /TextBox

        #region RichTextBox
        public static void TextboxResizer(this RichTextBox textBox)
        {
            TextboxResizer(textBox, false, textBox.MinimumSize.Width, 0, false, textBox.MinimumSize.Height, 0);
        }

        public static void TextboxResizer(this RichTextBox textBox, int padLines, bool lockWidth = true)
        {
            TextboxResizer(textBox, lockWidth, textBox.MinimumSize.Width, 0, false, textBox.MinimumSize.Height, padLines);
        }

        public static void TextboxResizer(this RichTextBox textBox, Size minSize)
        {
            TextboxResizer(textBox, false, minSize.Width, 0, false, minSize.Height, 0);
        }

        public static void TextboxResizer(this RichTextBox textBox, Size minSize, int padLines, bool lockWidth = true)
        {
            TextboxResizer(textBox, lockWidth, minSize.Width, 0, false, minSize.Height, padLines);
        }

        public static void TextboxResizer
        (
            this RichTextBox textBox, 
            bool lockWidth = false, int minWidth = 30, int padWidthChars = 0,
            bool lockHeight = false, int minHeight = 30, int padHeightLines = 0
        )
        {
            if (!lockWidth && textBox.Width <= minWidth)
            {// This is asserted. 
                textBox.Width = minWidth;
            }
            if (!lockHeight && textBox.Height <= minHeight)
            {// This is asserted. 
                textBox.Height = minWidth;
            }
            Size charSize = TextRenderer.MeasureText(Tokens.SINGLE_CHAR_STRING, textBox.Font);
            Size textSize = new Size();
            foreach (string line in textBox.Lines)
            {
                textSize.Width = Math.Max(textSize.Width, TextRenderer.MeasureText(line, textBox.Font).Width);
                textSize.Height += charSize.Height;
            }
            
            if (IsTextOverflowing(textBox.Size, textSize, out bool width, out bool height))
            {
                
                padWidthChars *= charSize.Width;
                padHeightLines *= charSize.Height;

                textSize.Width += padWidthChars;
                textSize.Height += padHeightLines;

                Size parentSize = textBox.Parent.Size;
                if (!lockWidth && width)
                {
                    parentSize.Width -= (textBox.Margin.Left + textBox.Margin.Right);
                    //if (textBox.MaximumSize.Width > 0)
                    //{
                    //    parentSize.Width = Math.Min(textBox.MaximumSize.Width, parentSize.Width);
                    //} // This is automatic is it not?
                    textBox.Width = Math.Min(textSize.Width, parentSize.Width);
                }
                if (!lockHeight && height)
                {
                    parentSize.Height -= (textBox.Margin.Top + textBox.Margin.Bottom);
                    //if (textBox.MaximumSize.Height > 0)
                    //{
                    //    parentSize.Height = Math.Min(textBox.MaximumSize.Height, parentSize.Height);
                    //}// This is automatic is it not?
                    textBox.Height = Math.Min(textSize.Height, parentSize.Height);
                }
            }
        }

        public static bool IsTextOverflowing_Width(this RichTextBox textBox)
        {
            return TextRenderer.MeasureText(textBox.Text, textBox.Font).Width < textBox.Width;
        }

        public static bool IsTextOverflowing_Height(this RichTextBox textBox)
        {
            return TextRenderer.MeasureText(textBox.Text, textBox.Font).Height < textBox.Height;
        }

        public static bool IsTextOverflowing(this RichTextBox textBox, out bool width, out bool height)
        {
            Size textSize = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            return IsTextOverflowing(textBox.Size, textSize, out width, out height);
        }

        private static bool IsTextOverflowing(Size textBoxSize, Size textSize, out bool width, out bool height)
        {
            width = textSize.Width > textBoxSize.Width;
            height = textSize.Height > textBoxSize.Height;
            return width || height;
        }

        public static void GetTextOverflowSize(this RichTextBox textBox, out int width, out int height)
        {
            Size textbox_size = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            width = Math.Max(0, textBox.Width - textbox_size.Width);
            height = Math.Max(0, textBox.Height - textbox_size.Height);
        }


        #endregion
    }
}
