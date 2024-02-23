using Common;
using Runtime;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unit.Interface;

namespace Unit
{
    [Serializable]
    public class Unit_Scalar : Unit, IUnit_Scalar
    {
        #region Identity
        private const String ClassName = nameof(Unit_Scalar);
        #endregion
        
        #region Scale Choices
        private static readonly IUnit secondsUnit = new Unit_Scalar(Units.Second, Scales.Micro);
        public static ComboBoxItem[] SecondsUnitChoices
        {
            get
            {
                return secondsUnit.UnitComboBoxChoices;
            }
        }
        #endregion /Scale Choices

        #region Display Control Type
        public override ControlType ControlType
        {
            get
            {
                return ControlType.Numeric_Scalar;
            }
        }
        #endregion

        #region Unit Accessors
        public override bool CastToType
        {
            get
            {
                return true;
            }
        }

        public bool Compound
        {
            get
            {
                return NativeUnit.Compound;
            }
        }

        private Units.Enum displayUnit;
        public override Units.Enum DisplayUnit
        {
            get
            {
                return displayUnit;
            }
            set
            {
                if(value!=NativeUnit)
                {
                    DisplayScale = Scales.Base;
                }
                displayUnit = value;
                DecimalFormatter = displayUnit.LookupDecimalFormatter();
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
                DisplayUnit = NativeUnit;// Return to the native unit for ALL scale changes
                unitScale.DisplayScale = value;
            }
        }
        #endregion

        #region Constructors

        public Unit_Scalar(Units.Enum nativeUnit, Scales.Enum initScale)
        {
            if (nativeUnit.Compound && initScale != Scales.Base)
            {
                Log_Manager.LogWarning(ClassName, Units.COMPOUND_SCALE_WARNING);
#if DEBUG
                throw new NotImplementedException(Units.COMPOUND_SCALE_WARNING);
#endif
            }
            unitScale = new Scale(initScale);
            NativeUnit = nativeUnit;
            DisplayUnit = nativeUnit;
            
            SetNativeUnitName();
            PopulateUnitScaleChoices();
        }

        public Unit_Scalar(Units.Enum nativeUnit, Scales.Enum nativeScale, Scales.Enum displayScale)
        {
            if (nativeUnit.Compound)
            {
                Log_Manager.LogWarning(ClassName, Units.COMPOUND_SCALE_WARNING);
#if DEBUG
                throw new NotImplementedException(Units.COMPOUND_SCALE_WARNING);
#endif
            }
            unitScale = new Scale(nativeScale, displayScale);
            NativeUnit = nativeUnit;
            DisplayUnit = nativeUnit;
            
            SetNativeUnitName();
            PopulateUnitScaleChoices();
        }

        public Unit_Scalar(Units.Enum nativeUnit, Units.Enum displayUnit, Scales.Enum initScale)
        {
            if (displayUnit.Compound && initScale != Scales.Base)
            {
                Log_Manager.LogWarning(ClassName, Units.COMPOUND_SCALE_WARNING);
#if DEBUG
                throw new NotImplementedException(Units.COMPOUND_SCALE_WARNING);
#endif
            }
            unitScale = new Scale(initScale);
            NativeUnit = nativeUnit;
            DisplayUnit = displayUnit;
            
            SetNativeUnitName();
            PopulateUnitScaleChoices();
        }

        public Unit_Scalar(Units.Enum nativeUnit, Units.Enum displayUnit, Scales.Enum nativeScale, Scales.Enum displayScale)
        {
            if (displayUnit.Compound && displayScale != Scales.Base)
            {
                Log_Manager.LogWarning(ClassName, Units.COMPOUND_SCALE_WARNING);
#if DEBUG
                throw new NotImplementedException(Units.COMPOUND_SCALE_WARNING);
#endif
            }
            unitScale = new Scale(nativeScale, displayScale);
            NativeUnit = nativeUnit;
            DisplayUnit = displayUnit;
            
            SetNativeUnitName();
            PopulateUnitScaleChoices();
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Unit_Scalar(SerializationInfo info, StreamingContext context)
        {
            Deserialize(info, context);
            SetNativeUnitName();
            PopulateUnitScaleChoices();
        }
        #endregion

        #region Initilization


        private void PopulateUnitScaleChoices()
        {
            string displayName;
            List<ComboBoxItem> enumerationChoiceList = new List<ComboBoxItem>();
            List<ComboBoxItem> minSetEnumerationChoiceList = new List<ComboBoxItem>();
            HashSet<ConversionTypes> RelevantConversionTypes = Units.GetConversionTypes(NativeUnit);
            if (!Compound && RelevantConversionTypes.Contains(ConversionTypes.MetricScale))
            {
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
            }
            else
            {// If no metric conversion 
                if (!ComboBoxItem.ListContainsValue(enumerationChoiceList, DisplayUnit))
                {
                    enumerationChoiceList.Add(new ComboBoxItem(Units.GetShortUnitName(DisplayUnit), DisplayUnit));
                    minSetEnumerationChoiceList.Add(new ComboBoxItem(Units.GetShortUnitName(DisplayUnit), DisplayUnit));
                }
            }

            if (RelevantConversionTypes.Contains(ConversionTypes.Time) ||
                RelevantConversionTypes.Contains(ConversionTypes.Speed))
            {
                if (Units.dictNativeUnit_dictDisplayUnitConversionFactor.ContainsKey(NativeUnit))
                {
                    foreach (Units.Enum metricUnit in Units.dictNativeUnit_dictDisplayUnitConversionFactor[NativeUnit].Keys)
                    {
                        if (Units.TryGetShortUnitName(metricUnit, out displayName))
                        {
                            enumerationChoiceList.Add(new ComboBoxItem(displayName, metricUnit));
                            if (metricUnit != Units.Day && metricUnit != Units.Hour)
                            {
                                minSetEnumerationChoiceList.Add(new ComboBoxItem(displayName, metricUnit));
                            }
                        }
                    }
                }
            }

            // This will always add the native unit no matter what, well... unless its programmed wrong.
            if (!DisplayUnit.Equals(NativeUnit))
            {// We need to add the native unit to the list
                if (!ComboBoxItem.ListContainsValue(enumerationChoiceList, NativeUnit))
                {// If its not already there
                    enumerationChoiceList.Add(new ComboBoxItem(NativeUnitName, NativeUnit));
                    minSetEnumerationChoiceList.Add(new ComboBoxItem(NativeUnitName, NativeUnit));
                }
            }
            UnitComboBoxChoices = enumerationChoiceList.ToArray();
            MinSetEnumerationChoices = minSetEnumerationChoiceList.ToArray();

        }
        #endregion

        #region Unit-Scale Adjustment
        public override bool TryScaleAdjustment(ref string valueStr)
        {
            return unitScale.TryScaleAdjustment(ref valueStr);
        }

        public override bool TryUnitAdjustment(ref string valueStr)
        {
            return NativeUnit.TryUnitAdjustment(DisplayUnit, ref valueStr);
        }

        public static bool TryUnitAdjustment(Units.Enum nativeUnit, Scales.Enum displayUnit, ref string valueStr)
        {
            return TryUnitAdjustment(nativeUnit, displayUnit, ref valueStr);
        }
        #endregion /Unit-Scale Adjustment
    }
}
