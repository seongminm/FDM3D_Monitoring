using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace MonitoringSensor.ViewModels
{
    class MultiLinesOxyPlotViewModel : ViewModelBase
    {
        public PlotModel PlotModel { get; set; }

        private LineSeries lineHumidity { get; set; }
        private LineSeries lineTemperature { get; set; }
        private LineSeries linePm1_0 { get; set; }
        private LineSeries linePm2_5 { get; set; }
        private LineSeries linePm10 { get; set; }
        private LineSeries linePid { get; set; }
        private LineSeries lineMics { get; set; }
        private LineSeries lineCjmcu { get; set; }
        private LineSeries lineMq { get; set; }
        private LineSeries lineHcho { get; set; }
       
        private int dataCount = 1;

        public MultiLinesOxyPlotViewModel(string title)
        {
            PlotModel = new PlotModel { Title = title, TitleFontSize = 11 };
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0, 
                Maximum = 100  
            };

            PlotModel.Axes.Add(yAxis);
            
            lineHumidity = new LineSeries { Title = "Hum" };
            lineTemperature = new LineSeries { Title = "Tem"};
            linePm1_0 = new LineSeries { Title = "PM1.0" };
            linePm2_5 = new LineSeries { Title = "PM2.5" };
            linePm10 = new LineSeries { Title = "PM10" };
            linePid = new LineSeries { Title = "VOC" };
            lineMics = new LineSeries { Title = "Mics" };
            lineCjmcu = new LineSeries { Title = "Cjmcu" };
            lineMq = new LineSeries { Title = "MQ" };
            lineHcho = new LineSeries { Title = "HCHO" };


            PlotModel.Series.Add(lineHumidity);
            PlotModel.Series.Add(lineTemperature);
            PlotModel.Series.Add(linePm1_0);
            PlotModel.Series.Add(linePm2_5);
            PlotModel.Series.Add(linePm10);
            PlotModel.Series.Add(linePid);
            PlotModel.Series.Add(lineMics);
            PlotModel.Series.Add(lineCjmcu);
            PlotModel.Series.Add(lineMq);
            PlotModel.Series.Add(lineHcho);

            PlotModel.Legends.Add(new Legend { LegendTitle = "그래프"});
        }

        public void GraphUpdata(string data, bool state)
        {
            string[] splitData = data.Split('/');

            lineHumidity.Points.Add(new DataPoint(dataCount, double.Parse(splitData[1])));
            lineTemperature.Points.Add(new DataPoint(dataCount, double.Parse(splitData[2])));
            linePm1_0.Points.Add(new DataPoint(dataCount, double.Parse(splitData[3])));
            linePm2_5.Points.Add(new DataPoint(dataCount, double.Parse(splitData[4])));
            linePm10.Points.Add(new DataPoint(dataCount, double.Parse(splitData[5])));
            linePid.Points.Add(new DataPoint(dataCount, double.Parse(splitData[6])));
            lineMics.Points.Add(new DataPoint(dataCount, double.Parse(splitData[7])));
            lineCjmcu.Points.Add(new DataPoint(dataCount, double.Parse(splitData[8])));
            lineMq.Points.Add(new DataPoint(dataCount, double.Parse(splitData[9])));
            lineHcho.Points.Add(new DataPoint(dataCount, double.Parse(splitData[10])));

            PlotModel.InvalidatePlot(state);
            dataCount++;
        }

        public void GraphClear()
        {
            lineHumidity.Points.Clear();
            lineTemperature.Points.Clear();
            linePm1_0.Points.Clear();
            linePm2_5.Points.Clear();
            linePm10.Points.Clear();
            linePid.Points.Clear();
            lineMics.Points.Clear();
            lineCjmcu.Points.Clear();
            lineMq.Points.Clear();
            lineHcho.Points.Clear();

            dataCount = 1;

            PlotModel.InvalidatePlot(true);
            
        }

    }
}
