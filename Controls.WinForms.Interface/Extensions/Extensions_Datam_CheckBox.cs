using Parameters.Interface;
using System;
using System.Windows.Forms;

namespace Datam.WinForms.Interface.Extensions
{
    public static class Extensions_Datam_CheckBox
    {
        #region Identity
        public const String ClassName = nameof(Extensions_Datam_CheckBox);
        #endregion

        #region Update
        public static void UpdateFromParameter(this CheckBox control, IParameter paramInfo)
        {
            if (Int32.TryParse(paramInfo.Value_Cast, out int result))
            {// Check to see if this text is indeed an number.
                if (result > 0)
                {
                    control.Checked = true;
                }
                else
                {
                    control.Checked = false;
                }
            }
        }
        #endregion
    }
}
