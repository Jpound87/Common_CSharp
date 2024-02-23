using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Struct
{
    public struct Extrema : IValidate
    {
        #region Identity
        public const String StructName = nameof(Extrema);
        #endregion /Identity

        #region Accessors
        public bool Valid
        {
            get
            {
                return Maximum > Minimum;
            }
        }
        public Double Minimum { get; internal set; } = Double.MaxValue;
        public Double Maximum { get; internal set; } = Double.MinValue;
        public Double Range
        {
            get
            {
                return Maximum - Minimum;
            }
        }
        public Double Average
        {
            get
            {
                return (Maximum + Minimum) / 2;
            }
        }
        #endregion /Accessors

        #region Constructor
        public Extrema(Double[] values)
        {
            if (values.Any())
            {
                Minimum = values.Min();
                Maximum = values.Max();
            }
        }
        public Extrema(Double min, Double max)
        {
            Minimum = min;
            Maximum = max;
        }
        #endregion /Constructor

        #region Update 
        public void Update(Double[] values)
        {
            Update(values.Min(), values.Max());
        }
        public void Update(Double min, Double max)
        {
            Minimum = min;
            Maximum = max;
        }
        #endregion /Update 
    }
}
