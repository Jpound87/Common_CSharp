using Common.Constant;
using System;
using System.Windows.Forms;

namespace Common.Extensions
{
    public static class Extensions_NumericUpDown
    {
        #region Get
        public static String GetValue_AsString(this NumericUpDown numericUpDown, bool throwException = false)
        {
            if (numericUpDown != null )
            {
                try
                {
                    return numericUpDown.Value.ToString();
                }
                catch
                {
                    if (throwException)
                    {
                        throw;
                    }
                }
            }
            return Tokens.Alert;
        }
        #endregion /Get
    }
}
