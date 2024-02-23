using Common.Extensions;
using Common.Utility;
using Parameters.Interface;
using System;
using System.Windows.Forms;

namespace Datam.WinForms.Extensions
{
    public static class Extensions_Datam_ComboBox
    {
        #region ComboBox
        public static void UpdateComboBox_SelectEnumeration_Integer32(this ComboBox cboEnum, IParameter paramInfo)
        {
            string valueStr = paramInfo.Value_Display;
            if (Utility_General.TryConvertFromWholeNumberStringToInt(valueStr, out _))
            {// Check to see if this text is indeed an number.
                if (!cboEnum.TrySelect_Enumeration(valueStr))//We've already verified its an int
                {// If there is no equivalent enum, we fail by putting the value
                    cboEnum.Text = String.Empty;//$"Option: {value.ToString(CultureInfo.InvariantCulture)}";
                }
            }
            else
            {// Todo: log error and string
                cboEnum.Text = "Not Defined";
            }
        }
        #endregion /ComboBox
    }
}
