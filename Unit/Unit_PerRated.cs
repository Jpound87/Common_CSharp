using Common;
using Parameters.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Unit.Interface;

namespace Unit
{
    [Serializable]
    public class Unit_PerRated : Unit, IUnit
    {
        #region Cast
        public override bool CastToType
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Control Type
        public override ControlType ControlType
        {
            get
            {
                return ControlType.Numeric_Scalar;
            }
        }
        #endregion

        #region Scale Accessors
        public override Scales.Enum DisplayScale
        {
            get
            {
                return unitScale.DisplayScale;
            }
            set
            {
                DisplayUnit = PerRatedParameter.Unit.NativeUnit; ;// Return to the native unit for ALL scale changes
                unitScale.DisplayScale = value;
            }
        }
        #endregion

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
            }
        }
        #endregion

        #region Name 

        private string perRatedUnitName;

        #endregion

        #region Constructor
        public Unit_PerRated(IParameter ratedValueParamInfo)
        {
            NativeUnit = Units.PerRated;
            unitScale = new Scale(ratedValueParamInfo.Unit.DisplayScale);
            DisplayUnit = ratedValueParamInfo.Unit.DisplayUnit;
            PerRatedParameter = ratedValueParamInfo;
            SetNativeUnitName();
            DefinePerRatedName();
            PopulateUnitScaleChoices();
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Unit_PerRated(SerializationInfo info, StreamingContext context)
        {
            Deserialize(info, context);
            SetNativeUnitName();
            DefinePerRatedName();// Must occur before PopulateUnitScaleChoices 
            PopulateUnitScaleChoices();
        }
        #endregion

        #region Serialization Methods
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        #endregion

        #region Initialization


        private void DefinePerRatedName()
        {
            perRatedUnitName = string.Format("{0}{1}", PerRatedParameter.Unit.Abbreviation, Units.GetShortUnitName(Units.PerRated));
        }

      
        private void PopulateUnitScaleChoices()
        {
            string displayName;
            List<ComboBoxItem> enumerationChoiceList = new List<ComboBoxItem>();
            List<ComboBoxItem> minSetEnumerationChoiceList = new List<ComboBoxItem>();
            int scaleInt;
            int scaleDisplay = DisplayScale.Enumeration;
            int nativeScaleInt = NativeScale.Enumeration;
            int minScaleRangeInt = Math.Max(nativeScaleInt - 2, 1);// Needed to keep null from being an option
            int maxScaleRangeInt = nativeScaleInt + 2;
            int minOutlier = Math.Max(nativeScaleInt - 4, 1);// Needed to keep null from being an option
            int maxOutLier = nativeScaleInt + 4;
            foreach (Scales.Enum scale in Scales.ToArray_Full())
            {
                if (!scale.Equals(Scales.Null))
                {// Only scaleable options
                    if (Scales.TryGetShortScaleName(scale, out string shortScaleName))
                    {
                        scaleInt = scale.Enumeration;
                        if ((scaleInt >= minScaleRangeInt && scaleInt <= maxScaleRangeInt) || scaleInt == minOutlier || scaleInt == maxOutLier || scaleInt == scaleDisplay)
                        {// This provides a smaller range of possibilities wrt the native choice
                            if (!ComboBoxItem.ListContainsValue(enumerationChoiceList, scale))
                            {
                                displayName = string.Format("{0}{1}", shortScaleName, Units.GetShortUnitName(DisplayUnit));
                                enumerationChoiceList.Add(new ComboBoxItem(displayName, scale));
                            }
                        }
                        if (scale.Equals(NativeScale) || scale.Equals(DisplayScale))
                        {
                            if (!ComboBoxItem.ListContainsValue(minSetEnumerationChoiceList, scale))
                            {
                                displayName = string.Format("{0}{1}", shortScaleName, Units.GetShortUnitName(DisplayUnit));
                                minSetEnumerationChoiceList.Add(new ComboBoxItem(displayName, scale));
                            }
                        }
                    }
                }
            }
            if (!ComboBoxItem.ListContainsValue(enumerationChoiceList, Scales.Base))
            {// If its not already there, we want the base unit (TODO: add in the appropriate order)
                if (Scales.TryGetShortScaleName(Scales.Base, out string shortScaleName))
                {
                    displayName = string.Format("{0}{1}", shortScaleName, Units.GetShortUnitName(DisplayUnit));
                    enumerationChoiceList.Add(new ComboBoxItem(displayName, Scales.Base));
                    minSetEnumerationChoiceList.Add(new ComboBoxItem(displayName, Scales.Base));
                }
            }
            enumerationChoiceList.Add(new ComboBoxItem(perRatedUnitName, Units.PerRated));
            minSetEnumerationChoiceList.Add(new ComboBoxItem(perRatedUnitName, Units.PerRated));
            // This will always add the native unit no matter what, well... unless its programmed wrong.
            if (!DisplayUnit.Equals(PerRatedParameter.Unit.NativeUnit))
            {// We need to add the native unit to the list
                if (!ComboBoxItem.ListContainsValue(enumerationChoiceList, PerRatedParameter.Unit.NativeUnit))
                {// If its not already there
                    enumerationChoiceList.Add(new ComboBoxItem(PerRatedParameter.Unit.NativeUnitName, PerRatedParameter.Unit.NativeUnit));
                    minSetEnumerationChoiceList.Add(new ComboBoxItem(PerRatedParameter.Unit.NativeUnitName, PerRatedParameter.Unit.NativeUnit));
                }
            }
            UnitComboBoxChoices = enumerationChoiceList.ToArray();
            MinSetEnumerationChoices = minSetEnumerationChoiceList.ToArray();
        }
        #endregion

        #region Unit-Scale Adjustment
        private bool TryPerRatedAdjustment(ref string valueStr)
        {
            if (double.TryParse(PerRatedParameter.Value_Cast, NumberStyles.Float, CultureInfo.InvariantCulture, out double maxRated))
            {//find out rated torque
                Scales.ScaleAdjustment(PerRatedParameter.NativeScale, DisplayScale, ref maxRated);
                double thousantdhOfRated = maxRated / 1000.0;
                if (double.TryParse(valueStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {// Check to see if this text is indeed an number.
                    value *= thousantdhOfRated;
                    valueStr = value.ToString(CultureInfo.InvariantCulture);
                    return true;
                }
            }
            return false;
        }

        public override bool TryScaleAdjustment(ref string valueStr)
        {
            if (!DisplayUnit.Equals(Units.PerRated))
            {
                if (TryPerRatedAdjustment(ref valueStr))
                {
                    //bool one = unitScale.TryScaleAdjustment(ref valueStr);
                    return unitScale.TryScaleAdjustment(ref valueStr);
                }
                return false;
            }
            return true;
        }


        public override bool TryUnitAdjustment(ref string valueStr)
        {
            return true;// for now, no unit adjustment on per rated//UnitScalar.TryUnitAdjustment(NativeUnit, DisplayUnit, ref valueStr);
        }
        #endregion

        #region Per Rated    
        public readonly IParameter PerRatedParameter;
        public static bool TryPerRatedAdjustement(Unit_PerRated unitPerRated, ref double value)
        {
            if(!unitPerRated.DisplayUnit.Equals(Units.PerRated))
            {
                if (double.TryParse(unitPerRated.PerRatedParameter.Value_Cast, NumberStyles.Float, CultureInfo.InvariantCulture, out double maxRated))
                {//find out rated torque
                    //double thousantdhOfRated =  maxRated / 1000.0;
                    value *= 1000000;
                    value /= maxRated;
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
