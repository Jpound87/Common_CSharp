using Common;
using System;
using System.Linq;
using Common.Constant;
using Common.Extensions;

namespace Device
{
    public class MovingAverage
    {
        private Int32 position;

        private Int32 _bufferSize;

        private Double recentValue;

        private Double averageValue;

        public Double Value
        {
            get
            {
                switch (FilterType)
                {
                    case FILTER_TYPE.SimpleMovingAverage:
                        return averageValue;
                    case FILTER_TYPE.ExponentialMovingAverage:
                        double factor = SmoothingFactor / (1 + _bufferSize);
                        return (recentValue * factor) + (averageValue * (1-factor));
                    case FILTER_TYPE.LinearInterpolation:
                        return 0;
                    default:
                        return 0;
                }
            }
        }

        public Double SmoothingFactor { get; set; }

        public FILTER_TYPE FilterType { get; set; }

        public Double[] PriorValues { get; private set; }

        public MovingAverage(int bufferSize)
        {
            _bufferSize = bufferSize;
            PriorValues = new double[bufferSize];
        }

        public MovingAverage(int bufferSize, int initialValue)
        {
            _bufferSize = bufferSize;
            PriorValues = new double[bufferSize];
        }

        public MovingAverage(int bufferSize, double initialValue)
        {
            _bufferSize = bufferSize;
            PriorValues = new double[bufferSize];
        }

        private void Initialize(int bufferSize, double initialValue)
        {
            PriorValues.ParallelFill(initialValue);
            SmoothingFactor = 2;
        }

        //public void InitilizeArray(int value) => priorValues = Array.Fill(priorValues, value);

        public void AddValue(int value)
        {
            AddValue(Convert.ToDouble(value));
        }

        public void AddValue(double value)
        {
            recentValue = value;
            PriorValues[position % _bufferSize] = recentValue;
            position++;
        }

        private void Average()
        {
            averageValue = PriorValues.Average();
        }
    }
}
