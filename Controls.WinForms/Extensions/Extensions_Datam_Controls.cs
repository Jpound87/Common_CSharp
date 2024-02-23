using Common.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Datam.WinForms.Extensions
{
    public static class Extensions_Datam_Controls
    {
        #region Alter 
        public static void AlterControls(this IEnumerable<Control> controls, bool designerMode = false)
        {
#if DEBUG
            if (designerMode)
            {
                return;
            }
#endif
            FontFamily fontFamily = new FontFamily("Arial");
            foreach (Control control in controls)
            {
                control.ChangeFont(fontFamily, control.Font.Style); //Change the appearance but no properties.
                if(control is Label label)
                {
                    label.AutoEllipsis = true;
                }
            }
        }
        #endregion /Alter
    }
}
