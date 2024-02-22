using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Interface
{
    public class FixedCenterLinearAxis : LinearAxis
    {
        public bool Centering 
        {
            get
            {
                return !IsPanEnabled;
            }
        }
        public double CenterAt { get; private set; }

        public FixedCenterLinearAxis() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="centerAt">The focus of the centering</param>
        public FixedCenterLinearAxis(double centerAt) : base()
        {
            CenterAt = centerAt;
        }


        /// <summary>
        /// This copy constructor allows the former linrar axis to be turned into a centered one. 
        /// Should be used to load old plots.
        /// </summary>
        /// <param name="oldLinearAxis">The axis to be converted</param>
        /// <param name="centerAt">The focus of the centering</param>
        public FixedCenterLinearAxis(LinearAxis oldLinearAxis, double centerAt) : base()
        {
            //copy constructor
            PropertyInfo[] destinationProperties = this.GetType().GetProperties();
            foreach (PropertyInfo destinationPi in destinationProperties)
            {
                PropertyInfo sourcePi = oldLinearAxis.GetType().GetProperty(destinationPi.Name);
                if (destinationPi.CanWrite)
                {
                    destinationPi.SetValue(this, sourcePi.GetValue(oldLinearAxis, null), null);
                }
            }
            CenterAt = centerAt;
        }

        public override void ZoomAt(double factor, double x)
        {
            if (Centering)
            {// The user wants the centering feature
                base.ZoomAt(factor, CenterAt);
            }
            else
            {
                base.ZoomAt(factor, x);
            }
        }
    }
}
