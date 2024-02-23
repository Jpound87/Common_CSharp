using System;
using System.Windows.Forms;

namespace Common.Monitor
{
    public class Monitor_UpdatingControls : Monitor_UpdatingObjects<Control>, IDisposable, IIdentifiable
    {
        #region Identity
        new public const String ClassName = nameof(Monitor_UpdatingControls);
        public override String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Check Method
        /// <summary>
        /// This helper method checks to see if the control should be 
        /// updated this cycle. 
        /// </summary>
        /// <param name="control">Control to be checked</param>
        /// <returns></returns>
        public override bool CanScheduleUpdate(Control control)
        {
            if (!control.Focused)// && control.IsVisibleToUser())
            {
               return base.CanScheduleUpdate(control);
            }
            return false;
        }
        #endregion /Check Method
    }
}
