using Common.Constant;
using Common.Extensions;
using Common.Utility;
using Devices.Interface;
using Parameters.Interface;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Datam.WinForms.Extensions
{
    public static class Extensions_Datam_TextBox
    {
        #region Scalar
        public static void UpdateScalable(this TextBox valueTextBox, ComboBox unitComboBox, IParameter parameter, bool isInteger = false)
        {
            if (isInteger)
            {// Make it so, it being the value as integer.
                if (Double.TryParse(parameter.Value_Display, out double dblValue))
                {
                    string dblStr = Convert.ToInt32(dblValue).ToString();
                    if (valueTextBox.Text != dblStr)
                    {
                        valueTextBox.Text = dblStr;
                    }
                    else
                    {
                        valueTextBox.Text = parameter.Value_Display.Split('.')[0];// Poor mans way
                    }
                }
            }
            else if (valueTextBox.Text != parameter.Value_Display)
            {
                valueTextBox.Text = parameter.Value_Display;
            }
            unitComboBox.SelectValue(parameter.Unit.Abbreviation);
        }
        #endregion /Scalar

        #region Integer
        public static void UpdateTextBox_Integer32(this TextBox control, IParameter paramInfo)
        {
            if (Int32.TryParse(paramInfo.Value_Display, out int value))
            {// Check to see if this text is indeed an number.
                control.Text = value.ToString();
            }
        }
        #endregion /Integer

        #region Float
        public static async Task<bool> UpdateTextBox_Float_Async(this ICommunicatorDevice device, TextBox textBox, IParameter paramInfo)
        {
            return textBox.UpdateTextBox_Float_Common(await device.RetrieveData_Async(paramInfo, true));
        }

        public static bool UpdateTextBox_Float(this TextBox textBox, IParameter paramInfo)
        {
            return textBox.UpdateTextBox_Float_Common(paramInfo.Value_Cast);
        }

        private static bool UpdateTextBox_Float_Common(this TextBox control, string strValue)
        {
            if (Utility_General.TryConvertFromNumberString_ToDouble(strValue, out double value, CultureInfo.InvariantCulture))
            {// Check to see if this text is indeed an number.
                control.BackColor = AM_Color.TextBox_BackColor;
                control.Text = value.ToString();
                return true;
            }
            control.BackColor = AM_Color.SendError;
            return false;
        }
        #endregion /Float

        #region BitWord
        public static bool UpdateTextBox_BitWord(this TextBox textBox, IParameter paramInfo, int size)
        {
            try
            {
                if (paramInfo.Type.IsIntType_CiA402())
                {
                    string result = Convert.ToString(Convert.ToInt32(paramInfo.Value_Raw, 16), 2).PadLeft(size, '0');
                    textBox.BackColor = AM_Color.TextBox_BackColor;
                    textBox.Text = result;
                    return true;
                }
                else if (paramInfo.Type.IsFloatType_CiA309())
                {
                    string result = Convert.ToString(Convert.ToInt32(float.Parse(paramInfo.Value_Raw)), 2).PadLeft(size, '0');
                    textBox.BackColor = AM_Color.TextBox_BackColor;
                    textBox.Text = result;
                    return true;
                }
            }
            finally
            {
                textBox.SetSizeFromContents();
            }
            textBox.BackColor = AM_Color.SendError;
            return false;
        }
        #endregion /BitWord
    }
}
