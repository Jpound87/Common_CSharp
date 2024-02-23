using Common;
using System;
using System.Runtime.Serialization;
using Unit.Interface;

namespace Unit
{
    [Serializable]
    public class Unit_Null : Unit, IUnit
    {
        #region Scale Choices
        private static readonly IUnit countsUnit = new Unit_Null(Units.Counts);
        public static ComboBoxItem[] CountsUnitChoices
        {
            get
            {
                return countsUnit.UnitComboBoxChoices;
            }
        }

        private static readonly IUnit countsPerSecondUnit = new Unit_Null(Units.CountsPerSecond);
        public static ComboBoxItem[] CountsPerSecondUnitChoices
        {
            get
            {
                return countsPerSecondUnit.UnitComboBoxChoices;
            }
        }

        private static readonly IUnit countsPerSecond2Unit = new Unit_Null(Units.CountsPerSecond2);
        public static ComboBoxItem[] CountsPerSecond2UnitChoices
        {
            get
            {
                return countsPerSecond2Unit.UnitComboBoxChoices;
            }
        }
        #endregion

        public override ComboBoxItem[] MinSetEnumerationChoices
        {
            get
            {
                return UnitComboBoxChoices;
            }
        }


        public override ComboBoxItem[] ValueEnumerationChoices
        {
            get
            {
                return ValueComboBoxChoices;
            }
        }

        #region Unit Accessors

        private Units.Enum displayUnit;
        public override Units.Enum DisplayUnit 
        { 
            get
            {
                return displayUnit;
            }
            set
            {
                displayUnit = value;
                DecimalFormatter = Units.LookupDecimalFormatter(displayUnit);
                if (displayUnit == Units.AddressType || displayUnit == Units.Word || displayUnit == Units.Hex)
                {
                    CastToType = false;
                }
                else
                {
                    CastToType = true;
                }
            }
        }

        public override Units.Enum NativeUnit 
        {
            get
            {
                return DisplayUnit;
            }
        }

        public new string Abbreviation
        {
            get
            {
                return Units.GetShortUnitName(DisplayUnit);
            }
        }

        public new string NativeAbbreviation
        {
            get
            {
                return Units.GetShortUnitName(NativeUnit);
            }
        }
        #endregion

        #region Constructors
        public Unit_Null()
        {
            DisplayUnit = Units.Null;
            unitScale = new Scale();
            SetNativeUnitName();
            SetControlType();
            SetComboBoxChoices();
        }

        public Unit_Null(Units.Enum unit)
        {
            DisplayUnit = unit;
            unitScale = new Scale();
            SetNativeUnitName();
            SetControlType();
            SetComboBoxChoices();
        }

        public Unit_Null(Units.Enum unit, ComboBoxItem[] enumerationChoices)
        {
            DisplayUnit = unit;
            unitScale = new Scale();
            SetNativeUnitName();
            SetControlType();
            SetComboBoxChoices(enumerationChoices);
        }
        #endregion

        #region Initialization
        private void SetControlType()
        {
            if (dictUnitControlType.ContainsKey(NativeUnit))
            {
                ControlType = dictUnitControlType[NativeUnit];
            }
            else
            {
                ControlType = dictUnitControlType[Units.Null];
            }
        }

        private void SetComboBoxChoices()
        {
            UnitComboBoxChoices = new ComboBoxItem[1]
            {
                new ComboBoxItem(Abbreviation, DisplayScale)
            };
            if (NativeUnit == Units.Boolean)
            {
                ValueComboBoxChoices = new ComboBoxItem[2]
                {
                    new ComboBoxItem("True", "1"),
                    new ComboBoxItem("False", "0")
                };
            }
            else
            {
                ValueComboBoxChoices = new ComboBoxItem[0];
            }
        }

        private void SetComboBoxChoices(ComboBoxItem[] enumerationChoices)
        {
            UnitComboBoxChoices = new ComboBoxItem[1]
            {
                new ComboBoxItem(Abbreviation, DisplayScale)
            };
            ValueComboBoxChoices = enumerationChoices;
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Unit_Null(SerializationInfo info, StreamingContext context)
        {
            Deserialize(info, context);
            DisplayUnit = Units.Null;
            SetNativeUnitName();
            SetControlType();
            UnitComboBoxChoices = new ComboBoxItem[1]
            {
                new ComboBoxItem(Abbreviation, DisplayScale)
            };
        }
        #endregion

        #region Unit-Scale Adjustment
        public override bool TryScaleAdjustment(ref string valueStr)
        {
            return true;
        }
        public override bool TryUnitAdjustment(ref string valueStr)
        {
            return true;
        }
        #endregion
       
    }
}
