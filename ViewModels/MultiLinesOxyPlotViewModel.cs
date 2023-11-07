
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace MonitoringSensor.ViewModels
{
    class MultiLinesOxyPlotViewModel : ViewModelBase
    {
        public  enum SensorNum {
            A = 1,
            B,
            C,
            D,
            E,
            F,
            G,
            H
        }

        public PlotModel PlotModel { get; set; }
        private LineSeries aSensor { get; set; }
        private LineSeries bSensor { get; set; }
        private LineSeries cSensor { get; set; }
        private LineSeries dSensor { get; set; }
        private LineSeries eSensor { get; set; }
        private LineSeries fSensor { get; set; }
        private LineSeries gSensor { get; set; }
        private LineSeries hSensor { get; set; }
  
       
        private int aDataCount = 1;
        private int bDataCount = 1;
        private int cDataCount = 1;
        private int dDataCount = 1;
        private int eDataCount = 1;
        private int fDataCount = 1;
        private int gDataCount = 1;
        private int hDataCount = 1;

        public MultiLinesOxyPlotViewModel(string title, double min = 0, double max = 500)
        {
            PlotModel = new PlotModel { Title = title, TitleFontSize = 11 };
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = min, 
                Maximum = max  
            };

            PlotModel.Axes.Add(yAxis);

            aSensor = new LineSeries { Title = "A" };
            bSensor = new LineSeries { Title = "B"};
            cSensor = new LineSeries { Title = "C" };
            dSensor = new LineSeries { Title = "D" };
            eSensor = new LineSeries { Title = "E" };
            fSensor = new LineSeries { Title = "F" };
            gSensor = new LineSeries { Title = "G" };
            hSensor = new LineSeries { Title = "H" };
           


            PlotModel.Series.Add(aSensor);
            PlotModel.Series.Add(bSensor);
            PlotModel.Series.Add(cSensor);
            PlotModel.Series.Add(dSensor);
            PlotModel.Series.Add(eSensor);
            PlotModel.Series.Add(fSensor);
            PlotModel.Series.Add(gSensor);
            PlotModel.Series.Add(hSensor);
  
            PlotModel.Legends.Add(new Legend { LegendTitle = "센서"});
        }

        public void GraphUpdata(double data, bool state, int sensorNum)
        {
            switch (sensorNum)
            {
                case 1:
                    aSensor.Points.Add(new DataPoint(aDataCount, data));
                    aDataCount++;
                    break;

                case 2:
                    bSensor.Points.Add(new DataPoint(bDataCount, data));
                    bDataCount++;
                    break;

                case 3:
                    cSensor.Points.Add(new DataPoint(cDataCount, data));
                    cDataCount++;
                    break;

                case 4:
                    dSensor.Points.Add(new DataPoint(dDataCount, data));
                    dDataCount++;
                    break;

                case 5:
                    eSensor.Points.Add(new DataPoint(eDataCount, data));
                    eDataCount++;
                    break;

                case 6:
                    fSensor.Points.Add(new DataPoint(fDataCount, data));
                    fDataCount++;
                    break;

                case 7:
                    gSensor.Points.Add(new DataPoint(gDataCount, data));
                    gDataCount++;
                    break;

                case 8:
                    hSensor.Points.Add(new DataPoint(hDataCount, data));
                    hDataCount++;
                    break;
            }

            PlotModel.InvalidatePlot(state);



        }

        public void GraphClear()
        {
            aSensor.Points.Clear();
            bSensor.Points.Clear();
            cSensor.Points.Clear();
            dSensor.Points.Clear();
            eSensor.Points.Clear();
            fSensor.Points.Clear();
            gSensor.Points.Clear();
            hSensor.Points.Clear();

            aDataCount = 1;
            bDataCount = 1;
            cDataCount = 1;
            dDataCount = 1;
            eDataCount = 1;
            fDataCount = 1;
            gDataCount = 1;
            hDataCount = 1;

            PlotModel.InvalidatePlot(true);
            
        }

    }
}
