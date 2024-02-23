using OxyPlot;

namespace Datam.WinForms.Utility
{
    public static class Utility_Datam_OxyPlot
    {
        #region Marker Type

        #region Array
        /// <summary>
        /// This array contains the a chosen distribution of the marker
        /// types for Oxyplot
        /// </summary>
        public static readonly MarkerType[] MarkerTypes = new MarkerType[6]
        {
            #region Selected Marker Types
            MarkerType.Circle,
            MarkerType.Square,
            MarkerType.Diamond,
            MarkerType.Triangle,
            MarkerType.Star,
            MarkerType.Cross
            #endregion /Selected Marker Types
        };

        #endregion /Array

        #endregion /Marker Type
    }
}
