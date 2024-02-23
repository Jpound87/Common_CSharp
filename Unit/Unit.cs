using Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unit.Interface;

namespace Unit
{
    [Serializable]
    public class Unit : IUnit 
    {
        #region Static Control Type Dictionary
        protected static readonly Dictionary<Units.Enum, ControlType> dictUnitControlType = new Dictionary<Units.Enum, ControlType>()
        {
            {Units.AddressType, ControlType.Text},
            {Units.OrdianalCount, ControlType.Text},
            {Units.Domain, ControlType.Text},
            {Units.Radians, ControlType.Text},
            {Units.Degrees, ControlType.Text},
            {Units.Percent, ControlType.Text},
            {Units.Float, ControlType.Text},
            {Units.Integer, ControlType.Text},
            {Units.Hex, ControlType.Text},
            {Units.String, ControlType.Text},
            {Units.Word, ControlType.Text},
            {Units.Boolean, ControlType.List},
            {Units.Enumeration, ControlType.List},
            {Units.Null, ControlType.None}
        };
        #endregion

        #region Unit Combo Box
        public ComboBoxItem[] UnitComboBoxChoices { get; protected set; }
        public virtual ComboBoxItem[] MinSetEnumerationChoices { get; protected set; }
        #endregion

        #region Value Combo Box
        public ComboBoxItem[] ValueComboBoxChoices { get; protected set; }
        public virtual ComboBoxItem[] ValueEnumerationChoices { get; protected set; }
        #endregion

        #region Unit Accessors
        public virtual bool CastToType { get; protected set; }
        public virtual ControlType ControlType { get; protected set; }
        #endregion

        #region Units
        public virtual Units.Enum DisplayUnit { get; set; }
        public virtual Units.Enum NativeUnit { get; protected set; }
        #endregion /Units

        #region Scale Accessors
        protected IScale unitScale;

        public Scales.Enum NativeScale
        {
            get
            {
                return unitScale.NativeScale;
            }
        }

        public virtual Scales.Enum DisplayScale { get; set; }
        #endregion

        #region Name
        public string Abbreviation
        {
            get
            {
                return Scales.GetShortScaleName(DisplayScale) + Units.GetShortUnitName(DisplayUnit);
            }
        }

        public string NativeAbbreviation
        {
            get
            {
                return nativeShortScaleName + Units.GetShortUnitName(NativeUnit);
            }
        }

        public string NativeUnitName { get; protected set; }

        protected string nativeShortUnitName;

        protected string nativeShortScaleName;

        public string DecimalFormatter { get; protected set; }
        #endregion

        #region Constructor
        protected Unit()
        {

        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Unit(SerializationInfo info, StreamingContext context)
        {
            Deserialize(info, context);
        }
        #endregion /Constructor

        #region Serialization Methods
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");
            info.AddValue("NativeScaleInt", NativeScale.Enumeration);
            info.AddValue("NativeUnitInt", NativeUnit.Enumeration);
            info.AddValue("DisplayScaleInt", DisplayScale.Enumeration);
            info.AddValue("DisplayUnitInt", DisplayUnit.Enumeration);
        }

        protected virtual void Deserialize(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");
            //Scale
            Scales.Enum nativeScale = Scales.Base;
            Scales.Enum displayScale = Scales.Base;
            if (info.GetValue("NativeScaleInt", typeof(int)) is int nativeScaleInt)
            {
                if (Scales.TryGetScaleEnumFromInt(nativeScaleInt, out nativeScale))
                {

                }
            }
            if (info.GetValue("DisplayScaleInt", typeof(int)) is int displayScaleInt)
            {
                if (Scales.TryGetScaleEnumFromInt(displayScaleInt, out displayScale))
                {

                }
            }
            unitScale = new Scale(nativeScale, displayScale);

            //Unit
            if (info.GetValue("NativeUnitInt", typeof(int)) is int nativeUnitInt)
            {
                if (Units.TryGetUnitEnumFromInt(nativeUnitInt, out Units.Enum nativeUnit))
                {
                    NativeUnit = nativeUnit;
                }
            }
            if (info.GetValue("DisplayUnitInt", typeof(int)) is int displayUnitInt)
            {
                if (Units.TryGetUnitEnumFromInt(displayUnitInt, out Units.Enum displayUnit))
                {
                    DisplayUnit = displayUnit;
                }
            }
        }
        #endregion /Serialization Methods

        #region Set Name
        protected void SetNativeUnitName()
        {
            nativeShortUnitName = Units.GetShortUnitName(DisplayUnit);
            nativeShortScaleName = Scales.GetShortScaleName(NativeScale);
            NativeUnitName = string.Format("{0}{1}", nativeShortScaleName, nativeShortUnitName);
        }
        #endregion

        #region Adjustment

        #region Scale 
        public virtual bool TryScaleAdjustment(ref string valueStr)
        {
            throw new NotImplementedException("This method is ment to be overridden.");
        }
        #endregion

        #region Unit Adjustment
        public virtual bool TryUnitAdjustment(ref string valueStr)
        {
            throw new NotImplementedException("This method is ment to be overridden.");
        }
        #endregion

        #endregion /Adjustment
    }
}
