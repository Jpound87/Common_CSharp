using Common.Extensions;
using Parameters.Interface;
using System;
using System.Windows.Forms;

namespace Datam.WinForms.Interface.Extensions
{
    public static class Extensions_Datam_ComboBox
    {
        #region Identity
        public const String ClassName = nameof(Extensions_Datam_ComboBox);
        #endregion

        #region Unit
        public static void SetUp_AsUnit(this ComboBox comboBox, IParameter paramInfo)
        {
            comboBox.Items.AddRange(paramInfo.Unit.MinSetEnumerationChoices);
            if (comboBox.Items.Count < 2)
            {
                comboBox.Enabled = false;
            }
            comboBox.TrySelect_Name(paramInfo.Unit.Abbreviation);
        }
        #endregion /Unit
    }
}
