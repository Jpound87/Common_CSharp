using System;
using Unit.Interface;

namespace Unit
{
    [Serializable]
    public class Scale : IScale
    {
        #region Globals
        public Scales.Enum NativeScale { get; private set; }

        private Scales.Enum displayScale;
        public Scales.Enum DisplayScale
        {
            get
            {
                return displayScale;
            }
            set
            {
                displayScale = value;
                Name = Scales.GetScaleName(displayScale);
                ShortName = Scales.GetShortScaleName(displayScale);
                ScalingFactor = Scales.GetScaleFactor(displayScale);
            }
        }

        public string Name { get; private set; }

        public string ShortName { get; private set; }

        public double ScalingFactor { get; private set; }
        #endregion

        #region Constructor
        public Scale()
        {
            NativeScale = Scales.Null;
            DisplayScale = Scales.Null;
        }

        public Scale(Scales.Enum initNativeScale)
        {
            NativeScale = initNativeScale;
            DisplayScale = initNativeScale;
        }

        public Scale(Scales.Enum initNativeScale, Scales.Enum initDisplayScale)
        {
            NativeScale = initNativeScale;
            DisplayScale = initDisplayScale;
        }
        #endregion /Constructors

        #region Method
        public void ScaleAdjustment(ref double value)
        {
            Scales.ScaleAdjustment(NativeScale, displayScale, ref value);
        }

        public bool TryScaleAdjustment(ref string valueStr)
        {
            return Scales.TryScaleAdjustment(NativeScale, displayScale, ref valueStr);
        }
        #endregion /Method
    }
}
