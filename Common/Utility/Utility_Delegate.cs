using System;
using System.Windows.Forms;

namespace Common.Utility
{
    public static class Utilities_Delegate
    {
        #region Invoke
        public static void Invoke_MethodInvoker(this Control control, MethodInvoker invoker, bool waitOnHandle = true)
        {
            try
            {
                if (control != null && (control.IsHandleCreated || !waitOnHandle))
                {
                    lock (control)
                    {
                        control?.Invoke(invoker);// The controls both have the same invocation requirements. 
                    }
                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
        }

        public static void Invoke_Action(this Control control, Action action, bool waitOnHandle = true)
        {
            try
            {
                if (control != null && (control.IsHandleCreated || !waitOnHandle))
                {
                    lock (control)
                    {
                        control?.Invoke(action);// The controls both have the same invocation requirements. 
                    }
                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
        }

        public static void Invoke_Action(this Control control, Action<String> action, String value, bool waitOnHandle = true)
        {
            try
            {
                if (control != null && (control.IsHandleCreated || !waitOnHandle))
                {
                    lock (control)
                    {
                        control?.Invoke(action, value);// The controls both have the same invocation requirements. 
                    }
                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
        }
        #endregion
    }
}
