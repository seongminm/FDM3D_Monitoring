using OxyPlot.Series;
using OxyPlot;
using System.ComponentModel;
using MonitoringSensor.ViewModels;

namespace MonitoringSensor.Views
{
    class OxyPlotViewModel : ViewModelBase
    {
        private double output;
        public double Output
        {
            get => output;
            set => SetProperty(ref output, value);
        }

        public PlotModel PlotModel { get; set; }

        private LineSeries linePlotModel;


        public OxyPlotViewModel()
        {
            PlotModel = new PlotModel();
            linePlotModel = new LineSeries();

            PlotModel.Series.Add(linePlotModel);

        }

        public void GrpahUpdate(double x, bool state)
        {
            linePlotModel.Points.Add(new DataPoint(x, output));
            PlotModel.InvalidatePlot(state);
        }


        public void GrahpClear()
        {
            linePlotModel.Points.Clear();
            PlotModel.InvalidatePlot(true);
            Output = 0;
        }

       
    }
}
