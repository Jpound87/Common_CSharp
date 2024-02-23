using Common;
using OxyPlot;
using OxyPlot.Axes;
using Runtime;
using System;

namespace Datam.WinForms.Base
{
    public class PlotModel_Datam_Base_XY : PlotModel, IIdentifiable
    {
        #region Identity
        public const String ClassName = nameof(PlotModel_Datam_Base_XY);
        public String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion /Identity

        #region Constants
        private const string BLANK_X_KEY = "Blanx";
        private const string BLANK_Y_KEY = "Blankey"; // Very comforting.
        #endregion /Constants

        #region Readonly
        private readonly LinearAxis blankAxisX = new LinearAxis
        {
            Key = BLANK_X_KEY,
            Position = AxisPosition.Left,
            MajorGridlineStyle = LineStyle.Dash,
            //MajorGridlineColor = Settings_Manager.GraphGridColor,
            MajorStep = 10,
            MinorGridlineStyle = LineStyle.Dot,
            //MinorGridlineColor = Settings_Manager.GraphGridColor,
            MinorStep = 2
        };

        private readonly LinearAxis blankAxisY = new LinearAxis
        {
            Key = BLANK_Y_KEY,
            Position = AxisPosition.Bottom,
            MajorGridlineStyle = LineStyle.Dash,
            //MajorGridlineColor = Settings_Manager.GraphGridColor,
            MajorStep = 10,
            MinorGridlineStyle = LineStyle.Dot,
            //MinorGridlineColor = Settings_Manager.GraphGridColor,
            MinorStep = 2
        };

        #endregion /Readonly

        #region Color
        public OxyColor GraphGridColor
        {
            get
            {
                return PlotAreaBorderColor;
            }
            set
            {
                PlotAreaBorderColor = value;
                blankAxisX.MajorGridlineColor = value;
                blankAxisX.MinorGridlineColor = value;
                blankAxisY.MajorGridlineColor = value;
                blankAxisY.MinorGridlineColor = value;
            }
        }

        public OxyColor GraphBackColor
        {
            get
            {
                return PlotAreaBackground;
            }
            set
            {
                PlotAreaBackground = value;
            }
        }
        #endregion /Color

        #region Constructor
        public PlotModel_Datam_Base_XY()
        {
            GraphGridColor = OxyColors.Black;
            GraphBackColor = OxyColors.White;
            InitBlankModel();
        }

        public PlotModel_Datam_Base_XY(OxyColor graphGridColor, OxyColor graphBackColor)
        {
            GraphGridColor = graphGridColor;
            GraphBackColor = graphBackColor;
            InitBlankModel();
        }
        #endregion /Constructor

        #region Methods
        /// <summary>
        /// This method prepares the blank model used by the forms. 
        /// </summary>
        public void InitBlankModel()
        {
            Title = Translation_Manager.NoCapture;
            if (Axes.Count == 0)
            {
                Axes.Add(blankAxisX);
                Axes.Add(blankAxisY);
            }
        }

        ///// <summary>
        ///// This method will update the blank model colors to match the settings.
        ///// </summary>
        //private void UpdateBlankModeColors()
        //{
        //    Log_Manager.LogMethodCall(ClassName, nameof(UpdateBlankModeColors));
        //    blankModel.PlotAreaBorderColor = Settings_Manager.GraphGridColor;
        //    blankModel.PlotAreaBackground = Settings_Manager.GraphBackColor;
        //    if (blankModel.Axes.Count > 0)
        //    {
        //        OxyColor gridColor = Settings_Manager.GraphGridColor;
        //        blankModel.Axes[0].MajorGridlineColor = gridColor;
        //        blankModel.Axes[0].MinorGridlineColor = gridColor;
        //        blankModel.Axes[1].MajorGridlineColor = gridColor;
        //        blankModel.Axes[1].MinorGridlineColor = gridColor;
        //    }
        //}

        public void UpdateBlankModelGridlines(bool major = true, bool minor = false)
        {
            if (Axes.Count > 0)
            {
                Axes[0].MajorGridlineStyle = Axes[1].MajorGridlineStyle = major ? LineStyle.Dash : LineStyle.None;
                Axes[0].MinorGridlineStyle = Axes[1].MinorGridlineStyle = minor ? LineStyle.Dot : LineStyle.None;
            }
        }

        #endregion Methods
    }
}
