using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit.Interface
{
    public interface IScale
    {
        #region Accessors
        Scales.Enum NativeScale { get; }
        Scales.Enum DisplayScale { get; set; }

        string Name { get; }

        string ShortName { get; }

        double ScalingFactor { get; }

        void ScaleAdjustment(ref double value);

        bool TryScaleAdjustment(ref string value);
        #endregion /Accessors
    }
}
