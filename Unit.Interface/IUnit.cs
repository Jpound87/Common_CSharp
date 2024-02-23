using Common;
using System;
using System.Runtime.Serialization;

namespace Unit.Interface
{
    #region Control Type Enumeration
    public enum ControlType : byte
    { 
        None,
        BitWord_ReadWrite,
        BitWord_ReadOnly,
        Text,
        List, // Also enumerations
        //RadioButton,
        //Slider,
        Button,
        Numeric_Scalar,// Non scalar is text
        Other//,
        //ScaledComboBox
    }
    #endregion

    public interface IUnit : ISerializable
    {
        #region Formatting
        bool CastToType { get; }
        String DecimalFormatter { get; }
        #endregion /Formatting

        #region Display
        String Abbreviation { get; }
        String NativeAbbreviation { get; }
        String NativeUnitName { get; }
        ControlType ControlType { get; }
        #endregion

        #region Unit
        Units.Enum DisplayUnit { get; set; } 
        Units.Enum NativeUnit { get; }
        #endregion

        #region Scale
        Scales.Enum NativeScale { get; }
        Scales.Enum DisplayScale { get; set; }
        #endregion

        #region Combo Box Choices
        ComboBoxItem[] UnitComboBoxChoices { get; }
        ComboBoxItem[] MinSetEnumerationChoices { get; }
        ComboBoxItem[] ValueComboBoxChoices { get; }
        #endregion

        #region Adjustment Methods
        bool TryScaleAdjustment(ref String valueStr);
        bool TryUnitAdjustment(ref String valueStr);
        #endregion
    }
}
