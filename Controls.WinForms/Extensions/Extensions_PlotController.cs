using OxyPlot;

namespace Datam.WinForms.Extensions
{
    public static class Extensions_PlotController
    {
        public static void AttacheNewPlotController(this IPlotController plotController)
        {
            plotController.UnbindMouseDown(OxyMouseButton.Left);
            plotController.UnbindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Control);
            plotController.UnbindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Shift);

            plotController.BindMouseDown(OxyMouseButton.Left, new DelegatePlotCommand<OxyMouseDownEventArgs>(
                         (view, controller, args) =>
                            controller.AddMouseManipulator(view, new WpbTrackerManipulator(view), args)));
        }
    }
}
