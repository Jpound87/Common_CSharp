using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Common.Utility
{
    public static class Utility_Form
    {
        #region Open Forms
        public static Form[] GetOpenFormsOfType<T>()
        {
            return GetOpenFormsOfTypeCastAs<T, Form>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type to look up.</typeparam>
        /// <typeparam name="C">Type to return if the original type can be cast to this.</typeparam>
        /// <returns></returns>
        public static C[] GetOpenFormsOfTypeCastAs<T, C>()
        {
            List<C> returnList = new List<C>();
            foreach (T lookupTypeObject in GetOpenFormsOfType_AsArray<T>())
            {
                if (lookupTypeObject is C castTypeObject)
                {
                    returnList.Add(castTypeObject);
                }
            }
            return returnList.ToArray();
        }

        public static bool IsOpenFormsOfType<T>()
        {
            return GetOpenFormsOfType_AsArray<T>().Any();
        }

        public static T[] GetOpenFormsOfType_AsArray<T>()
        {
            return Application.OpenForms.OfType<T>().ToArray();
        }
        #endregion /Open Forms

        #region Close Forms
        public static void SeekAndDestroy<T>() where T : Form
        {
            IEnumerable<T> formsOfType = Application.OpenForms.OfType<T>();
            if (formsOfType.Any())
            {
                Form form = formsOfType.First();
                form.Close();
                form.Dispose();
            }
        }
        #endregion /Close Forms
    }
}
