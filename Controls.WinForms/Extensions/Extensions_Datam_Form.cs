using Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AM_WinForms.Datam.Extensions
{
    public static class Extensions_Datam_Form
    {
        #region Identity
        public const String ClassName = nameof(Extensions_Datam_Form);
        #endregion

        #region Methods
        public static void GetAndDisplayForm<T>(this T form) where T : Form
        {
            Log_Manager.LogMethodCall(ClassName, nameof(GetAndDisplayForm));
            IEnumerable<T> formsOfType = Application.OpenForms.OfType<T>();
            if (formsOfType.Any())
            {
                form = formsOfType.First();
            }
            form.BringToFront();
            form.Show();
        }

        public static void SeekAndDestroy<T>() where T : Form
        {
            Log_Manager.LogMethodCall(ClassName, nameof(SeekAndDestroy));
            IEnumerable<T> formsOfType = Application.OpenForms.OfType<T>();
            if (formsOfType.Any())
            {
                Form form = formsOfType.First();
                form.Close();
            }
        }

        public static bool IsForm<T>() where T : Form
        {
            Log_Manager.LogMethodCall(ClassName, nameof(SeekAndDestroy));
            IEnumerable<T> formsOfType = Application.OpenForms.OfType<T>();
            return formsOfType.Any();
        }

        public static void GetAndDisplayDialog<T>(this T form, Form owner = null) where T : Form
        {
            Log_Manager.LogMethodCall(ClassName, nameof(GetAndDisplayDialog));
            IEnumerable<T> formsOfType = Application.OpenForms.OfType<T>();
            if (formsOfType.Any())
            {
                try
                {
                    form = formsOfType.First();
                    if (form.Visible != true)
                    {
                        if (owner == null)
                        {
                            form.ShowDialog();
                        }
                        else
                        {
                            form.ShowDialog(owner);
                        }
                    }
                    form.Invoke(new MethodInvoker(form.BringToFront));
                }
                catch(InvalidOperationException)
                {
                    form?.Invoke(new MethodInvoker(form.Close));
                }
                catch(Exception ex)
                {
                    Log_Manager.LogAssert(ClassName, ex.Message);
                }
            }
            else
            {
                form.ShowDialog();
            }
        }
        #endregion
    }
}
