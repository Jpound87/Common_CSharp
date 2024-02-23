using Common.Constant;
using System.Drawing;
using System.Windows.Forms;

namespace Datam.WinForms.Extensions
{
    public static class Extentions_Datam_CheckBox
    {
        #region Toggle
        /// <summary>
        /// This helper method is designed to appropriately color 
        /// the checkbox control to it's 'checked' state.
        /// </summary>
        /// <param name="chkDigital">The checkbox to color by its 'checked' state</param>
        public static void ToggleDigital(this CheckBox chkDigital)
        {
            if (chkDigital.Checked)
            {// High
                chkDigital.Text = Tokens.HIGH;
                chkDigital.BackColor = AM_Color.HighOn;
                chkDigital.ForeColor = Color.White;
            }
            else
            {// Low

                chkDigital.Text = Tokens.LOW;
                chkDigital.BackColor = AM_Color.LowOff;
                chkDigital.ForeColor = Color.White;
            }
        }
        #endregion /Toggle
    }
}
