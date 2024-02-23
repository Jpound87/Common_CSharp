using Common.Constant;
using Common.Extensions;
using Datam.WinForms.Interface;
using System;

namespace Datam.WinForms.Struct
{
    internal struct TimeCorrelatedData
    {
        #region Size
        public const uint DEFAULT_SIZE = 4;
        internal uint Size
        {
            get => Convert.ToUInt32(variables.Length);
            private set
            {
                if(variables.Length != value)
                {
                    IDatamVariableCaptureData[] tmpVars = new IDatamVariableCaptureData[value];
                    Array.Copy(variables, tmpVars, Math.Min(variables.Length, tmpVars.Length));
                    variables = tmpVars;
                    for (int v = 0; v < variables.Length; v++)
                    {
                        if (variables[v] == null)
                        {
                            variables[v] = new DatamVariableCaptureData();
                        }
                    }
                }
            }
        }
        private uint depth;// The number of data points.
        internal uint Depth
        {
            get => depth;
            set
            {
                if (depth != value)
                {
                    value = Math.Min(Int32.MaxValue, value);
                    AdjustCaptureDepth(value);
                    depth = value;
                }
            }
        }

        private void AdjustCaptureDepth(uint depth)
        {
            Array.Resize(ref time, (int)depth);
            for (int s = 0; s < Size; s++)
            {
                if( variables[s].Length < depth)
                {
                    variables[s].SetRawData(new double[depth]);
                }
            }
        }
        #endregion /Size

        #region Data
        private IDatamVariableCaptureData[] variables = Array.Empty<IDatamVariableCaptureData>();
        internal readonly IDatamVariableCaptureData this[uint i] => variables[i];

        public void SetVariableCaptureData(uint index, IDatamVariableCaptureData variableCaptureData)
        {
            if (index < Size)
            {
                variables[index] = variableCaptureData;
            }
            else
            {
                Size = index + 1;// Zero counts as an index ofc.
                variables[index] = variableCaptureData;
            }
        }

        public void SetRawData(uint index, Double[] rawData)
        {
            variables[index].SetRawData(rawData);
        }

        public void SetAxis(uint index, PlotAxis axis)
        {
            variables[index].SetAxis(axis);
        }

        public Double RangeMin { get; private set; } = Double.MaxValue;
        public Double RangeMax { get; private set; } = Double.MinValue;
        public void UpdateExtrema()
        {
            RangeMin = Double.MaxValue;
            RangeMax = Double.MinValue;
            for (int e = 0; e < Size; e++)
            {
                if (variables[e].Length > 0)
                {
                    if (variables[e].Valid)
                    {
                        RangeMin = Math.Min(RangeMin, variables[e].Minimum);// While we are seeing each individually,
                        RangeMax = Math.Max(RangeMax, variables[e].Maximum);// it's a good time to update these min's and max's
                    }
                }
            }
        }
        #endregion /Data

        #region Time
        private TimeScale timeScale = TimeScale.Milliseconds;// The 'previous' timescale, as it were, to compare to
        public TimeScale TimeScale // Gets current timescale or sets it to a new one
        {
            get
            {
                return timeScale;
            }
            set
            {
                for (uint c = 0; c < Depth; c++)
                {
                    timeScale.AdjustTimeScale(value, ref Time[c]);
                }
                timeScale = value;
            }
        }
        private Double[] time  = Array.Empty<Double>();
        public readonly Double[] Time
        { 
            get => time; 
        }
        /// <summary>
        /// Index is time, sub array is by var at time.
        /// </summary>
        public double TimeMax
        {
            get
            {
                if (Depth > 0)
                {
                    return Time[Depth - 1];
                }
                else return 0;
            }
        }
        public double TimeMin
        {
            get
            {
                if (Depth > 0)
                {
                    return Time[0];
                }
                else return 0;
            }
        }

        public void SetTime_Interval(Double startTimeMs, Double intervalMs)
        {
            Time[0] = startTimeMs;
            for (int d = 1; d < Depth; d++)
            {
                Time[d] = Time[d - 1] + intervalMs;
            }
        }

        public Double SetTime_Interval(uint index, Double intervalMs)
        {
            for (int timeIndex = 0; timeIndex < Depth; timeIndex++)
            {
                Time[timeIndex] = (timeIndex - index) * intervalMs;
            }
            return Time[index];
        }

        public void SetTime_Value(uint index, Double value)
        {
            Time[index] = value;
        }

        public void SetTime_Array(Double[] time)
        {
            this.time = time;
        }
        #endregion /Time

        #region Constructor
        public TimeCorrelatedData()
        {
            Size = DEFAULT_SIZE;
        }

        public TimeCorrelatedData(uint size)
        {
            Size = size;
        }
        #endregion /Constructor
    }
}
