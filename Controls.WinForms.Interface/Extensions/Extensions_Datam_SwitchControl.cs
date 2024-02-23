using Common.Constant;
using Common.Controls;
using Common.Exceptions;
using Devices.Interface;
using Parameters.Interface;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Datam.WinForms.Interface.Extensions
{
    public static class Extensions_Datam_SwitchControl
    {
        #region Identity
        public const String ClassName = nameof(Extensions_Datam_SwitchControl);
        #endregion

        #region Send
    
        public static async Task<bool> SendValue_FlashResult_Async(this SwitchControl switchControl, ICommunicatorDevice device, IParameter paramInfo, CancellationToken cancellationToken)
        {
            return await switchControl.SendValue_FlashResult_Switch(Color.LightGray, device.VerifiedSendData_Async(paramInfo, switchControl.IsOn ? "1" : "0"), cancellationToken);
        }

        public static async Task<bool> SendValue_FlashResult_Switch(this SwitchControl control, Color normColor, Task<bool> taskSuccess, CancellationToken cancellationToken)
        {
            return await control.SendValue_FlashResult_Switch(normColor, await taskSuccess, cancellationToken);
        }

        public static async Task<bool> SendValue_FlashResult_Switch(this SwitchControl control, Color normColor, bool updateSuccess, CancellationToken cancellationToken)
        {
            control.BorderColor = Color.LightBlue;
            Color flashColor;
            if (updateSuccess)
            {
                flashColor = AM_Color.SendSuccess;
            }
            else
            {
                flashColor = AM_Color.SendError;
            }
            try
            {
                control.BorderColor = flashColor;
                await Task.Delay(250, cancellationToken);
                control.BorderColor = normColor;
                await Task.Delay(250, cancellationToken);
                control.BorderColor = flashColor;
                await Task.Delay(250, cancellationToken);
                control.BorderColor = normColor;
            }
            catch (Exception ex) when ((ex is Exception_ThreadExit) || (ex is TaskCanceledException))
            {
                // Do nothing as it is expected when closing forms
            }
            return updateSuccess;
        }
        #endregion /Send 

        #region Update
        public static void UpdateValue(this SwitchControl control, IParameter parameter)
        {
            if (Int32.TryParse(parameter.Value_Cast, out int result))
            {// Check to see if this text is indeed an number.
                bool isOn = result > 0;
                if (control.IsOn != isOn)
                {
                    control.IsOn = isOn;
                }
            }
        }
        #endregion /Update
    }
}
