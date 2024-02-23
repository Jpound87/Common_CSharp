using OxyPlot;
using System;
using System.Drawing;

namespace Common.Constant
{
    public static class AM_Color
    {
        #region Identity
        public const String ClassName = nameof(AM_Color);
        #endregion

        #region Text 
        public static readonly Color Text_White = Color.AntiqueWhite;
        public static readonly Color Text_Black = Color.Black;
        public static readonly Color Text_Saved = Color.Indigo;
        #endregion

        #region TU Main Colors
        public static readonly Color Allied = Color.Blue;
        public static readonly Color Hei = Color.DarkOrange;
        public static readonly Color Ormec = Color.LightGoldenrodYellow;
        #endregion /TU Main Colors

        #region Form Colors
        /// <summary>
        /// Used to denote this color hasn't not been set, since the use of this color is extremely unlikely.
        /// </summary>
        public static readonly Color Form_Invalid = Color.FromArgb(7, 7, 7);
        public static readonly Color Form_BackColor = SystemColors.Control;
        public static readonly Color Form_ForeColor = SystemColors.ControlDark;
        #endregion

        #region State Color

        #region Binary State
        public static readonly Color Disable = Color.Red;
        public static readonly Color Enable = Color.LimeGreen;
        public static readonly Color LowOff = Color.LightGray;//CAH - Added
        public static readonly Color LowOn = Color.SteelBlue;//CAH - Added
        public static readonly Color HighOff = Color.LightGray;
        public static readonly Color HighOn = Color.MediumTurquoise;
        #endregion

        #region Send State
        public static readonly Color SendError = Color.OrangeRed;
        public static readonly Color DecodeError = Color.OrangeRed;
        public static readonly Color SendSuccess = Color.LightGreen;
        #endregion

        #region Status Monitor
        public static readonly Color TextStatusOff = Color.Black;
        public static readonly Color StatusOff = Color.SteelBlue;
        public static readonly Color TextMotionActive = Color.LightGreen;
        public static readonly Color MotionActive = Color.Green;
        public static readonly Color TextDataLinkUp = Color.DarkBlue;
        public static readonly Color DataLinkUp = Color.Aqua;
        public static readonly Color TextDataLinkDown = Color.Lavender;
        public static readonly Color DataLinkDown = Color.Plum;
        public static readonly Color TextFaulted = Color.Yellow;
        public static readonly Color Faulted = Color.Red;
        public static readonly Color TextWarning = Color.Red;
        public static readonly Color Warning = Color.Yellow;
        public static readonly Color TextBrakeOverride = Color.Gold;
        public static readonly Color BrakeOverride = Color.Maroon;
        public static readonly Color TextBrakeOn = Color.White;
        public static readonly Color BrakeOn = Color.Red;
        public static readonly Color TextBrakeOff = Color.LightGreen;
        public static readonly Color BrakeOff = Color.Green;
        public static readonly Color TextRemoted = Color.LightCyan;
        public static readonly Color Remoted = Color.DarkViolet;
        public static readonly Color DownedDataLink = Color.IndianRed;
        #endregion /Status Monitor

        #endregion /State Color

        #region Controls

        #region Button
        public static readonly Color Button_BackColor = SystemColors.ControlDark;
        #endregion

        #region CheckBox
        public static readonly Color CheckBox_BackColor = SystemColors.Control;
        #endregion

        #region ComboBox
        public static readonly Color ComboBox_BackColor = SystemColors.Control;
        public static readonly Color ComboBox_Disabled = SystemColors.ControlDark;
        #endregion /ComboBox

        #region DataGrid
        public static readonly Color DataGrid_White = Color.White;
        public static readonly Color DataGrid_Highlight = Color.LightGoldenrodYellow;
        #endregion

        #region GroupBox
        public static readonly Color GroupBox_ForeColor = SystemColors.ControlText;
        #endregion

        #region Label
        public static readonly Color Label_BackColor = SystemColors.Control;
        #endregion

        #region NumericUpDown
        public static readonly Color NumericUpDown_BackColor = SystemColors.Window;
        #endregion

        #region TextBox
        public static readonly Color TextBox_BackColor = SystemColors.Window;
        #endregion

        #endregion /Controls

        #region Indicators

        #region Fault Reset Indicator
        public static readonly Color BlinkActivatedBack = Color.CadetBlue;
        public static readonly Color BlinkActivatedFore = Color.AliceBlue;
        #endregion

        #region Quick Stop Indicator
        public static readonly Color QuickStopActivatedBack = Color.LightGoldenrodYellow;
        public static readonly Color QuickStopActivatedFore = Color.DarkOrange;
        #endregion

        #region Halt Indicator
        public static readonly Color HaltActivatedBack = Color.OrangeRed;
        public static readonly Color HaltActivatedFore = Color.White;
        #endregion

        #endregion

        #region Motion Control 

        #region Position Graph
        public static readonly OxyColor VelocityColor = OxyColors.SlateBlue;
        public static readonly OxyColor VelocityGridlineColor = OxyColors.DarkSlateBlue;
        public static readonly OxyColor PositionColor = OxyColors.Magenta;
        public static readonly OxyColor PositionGridlineColor = OxyColors.DarkMagenta;
        public static readonly OxyColor TimeGridlineColor = OxyColor.FromArgb(255, 102, 102, 0);
        #endregion

        #endregion

        #region Data Capture
        public static readonly OxyColor GraphParam1Color;
        public static readonly OxyColor GraphParam2Color;
        public static readonly OxyColor GraphParam3Color;
        public static readonly OxyColor GraphParam4Color;
        public static readonly OxyColor GraphBackColor;
        public static readonly OxyColor GraphGridColor;
        #endregion

        #region Authorization 
        public static readonly Color NoneAuthColor = Color.Red;
        public static readonly Color AlliedAuthColor = Color.CornflowerBlue;
        public static readonly Color AuthorizedAuthColor = Color.Gold;
        public static readonly Color ElevatedAuthColor = Color.Orange;
        public static readonly Color StandardAuthColor = Color.LightGreen;
        public static readonly Color SafetyAuthColor = Color.Gray;
        public static readonly Color AllAuthColor = SystemColors.Control;
        #endregion
    }
}
