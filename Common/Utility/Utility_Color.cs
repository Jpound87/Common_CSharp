using System;
using System.Diagnostics;
using System.Drawing;

namespace Common.Utility
{
    public static class Utility_Color
    {
        #region Type
        public static Type Type { get; } = typeof(Color);
        #endregion Type

        #region Get
        /// <summary>
        /// This method takes in a string and returns the equivalent OxyColor. If no equivalent can be found, OxyColors.Black is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Color ColorFromName(string name)
        {
            try
            {
                return Color.FromName(name);//Convert the string to a color
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
                return Color.Black;
            }
        }
        #endregion
    }
}
