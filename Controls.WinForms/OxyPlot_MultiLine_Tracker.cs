using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Linq;

namespace Datam.WinForms
{
    public class WpbTrackerManipulator : MouseManipulator
    {
        /// <summary>
        /// The current series.
        /// </summary>
        private DataPointSeries currentSeries;

        #region Constructor
        public WpbTrackerManipulator(IPlotView plotView)
            : base(plotView)
        {
        }
        #endregion /Constructor

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.
        /// </param>
        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);
            e.Handled = true;

            currentSeries = null;
            PlotView.HideTracker();
        }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.
        /// </param>
        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);
            e.Handled = true;

            if (currentSeries == null)
            {
                PlotView.HideTracker();
                return;
            }

            var actualModel = PlotView.ActualModel;
            if (actualModel == null)
            {
                return;
            }

            if (!actualModel.PlotArea.Contains(e.Position.X, e.Position.Y))
            {
                return;
            }

            var time = currentSeries.InverseTransform(e.Position).X;
            var points = currentSeries.Points;
            DataPoint dp = points.FirstOrDefault(d => d.X >= time);
            // Exclude default DataPoint.
            // It has insignificant downside and is more performant than using First above
            // and handling exceptions.
            if (dp.X != 0 || dp.Y != 0)
            {
                int index = points.IndexOf(dp);
                IEnumerable<DataPointSeries> ss = PlotView.ActualModel.Series.Cast<DataPointSeries>();
                List<double> values = new List<double>();
                foreach (var series in ss)
                {
                    values.Add(points[index].Y);
                }

                var position = XAxis.Transform(dp.X, dp.Y, currentSeries.YAxis);
                position = new ScreenPoint(position.X, e.Position.Y);

                var result = new WpbTrackerHitResult(values.ToArray())
                {
                    Series = currentSeries,
                    DataPoint = dp,
                    Index = index,
                    Item = dp,
                    Position = position,
                    PlotModel = PlotView.ActualModel
                };
                PlotView.ShowTracker(result);
            }
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.
        /// </param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            currentSeries = PlotView?.ActualModel?.Series
                             .FirstOrDefault(s => s.IsVisible) as DataPointSeries;
            Delta(e);
        }
    }

    public class WpbTrackerHitResult : TrackerHitResult
    {
        public double[] Values { get; private set; }

        // can't use the default indexer name (Item) since the base class uses that for something else
        [System.Runtime.CompilerServices.IndexerName("ValueString")]
        public string this[int index]
        {
            get
            {
                return string.Format((index == 1 || index == 4) ?
                  "{0,7:###0   }" : "{0,7:###0.0#}", Values[index]);
            }
        }

        public WpbTrackerHitResult(double[] values)
        {
            Values = values;
        }
    }
}

