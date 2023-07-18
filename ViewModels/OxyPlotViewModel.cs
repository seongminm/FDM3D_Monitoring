using OxyPlot.Series;
using OxyPlot;
using System.ComponentModel;

namespace MonitoringSensor.Views
{
    class OxyPlotViewModel : INotifyPropertyChanged
    {
        private double output;
        public double Output
        {
            get { return output; }
            set
            {
                output = value;
                OnPropertyChanged(nameof(Output));
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
