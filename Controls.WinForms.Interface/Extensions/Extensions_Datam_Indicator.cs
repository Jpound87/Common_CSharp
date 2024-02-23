using Common.Constant;
using Common.Controls;
using Parameters.Interface;
using System;

namespace Datam.WinForms.Interface.Extensions
{
    public static class Extensions_Datam_Indicator
    {
        #region Update
        public static void UpdateValue(this Indicator control, IParameter parameter)
        {
            if (Int32.TryParse(parameter.Value_Cast, out int result))
            {// Check to see if this text is indeed an number.
                bool isOn = result > 0;
                if (control.On != isOn)
                {
                    control.State = isOn ? Indicator_State.On : Indicator_State.Off;
                }
            }
        }
        #endregion /Update
    }
}
