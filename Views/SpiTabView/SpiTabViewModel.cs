using MonitoringSensor.Services;
using MonitoringSensor.ViewModels;
using MonitoringSensor.ViewModels.Command;
using System;

namespace MonitoringSensor.Views.SpiTabView
{
    class SpiTabViewModel : ViewModelBase, IGetDataService
    {
        public SerialViewModel SerialViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public CsvViewModel CsvViewModel { get; set; }
        public DatabaseViewModel DatabaseViewModel { get; set; }
        public MultiLinesOxyPlotViewModel Humidity { get; set; }
        public MultiLinesOxyPlotViewModel Temperature { get; set; }
        public MultiLinesOxyPlotViewModel Pm1_0 { get; set; }
        public MultiLinesOxyPlotViewModel Pm2_5 { get; set; }
        public MultiLinesOxyPlotViewModel Pm10 { get; set; }
        public MultiLinesOxyPlotViewModel Voc { get; set; }
        public MultiLinesOxyPlotViewModel Mics { get; set; }
        public MultiLinesOxyPlotViewModel Cjmcu { get; set; }
        public MultiLinesOxyPlotViewModel Mq { get; set; }
        public MultiLinesOxyPlotViewModel Hcho { get; set; }

        private bool graphState;
        public bool GraphState
        {
            get => graphState;
            set => SetProperty(ref graphState, value);
        }

        private string graphContent;
        public string GraphContent
        {
            get => graphContent;
            set => SetProperty(ref graphContent, value);
        }

        public RelayCommand GraphCommand { get; set; }
        public RelayCommand GraphClearCommand { get; set; }


        public SpiTabViewModel()
        {
            TimerViewModel = new TimerViewModel();

            string line = $"{"Time"},{"Sensor"},{"Humidity"},{"Temperature"}, {"PM1.0"}, {"PM2.5"}, {"PM10"}, {"VOC"}, {"MICS"}, {"CJMCU"}, {"MQ"}, {"HCHO"}";

            CsvViewModel = new CsvViewModel(line);
            DatabaseViewModel = new DatabaseViewModel(true);

            SerialViewModel = new SerialViewModel(TimerViewModel, this);
            Humidity = new MultiLinesOxyPlotViewModel("Humidity", 0, 101);
            Temperature = new MultiLinesOxyPlotViewModel("Temperature",0 ,81);
            Pm1_0 = new MultiLinesOxyPlotViewModel("PM1.0");
            Pm2_5 = new MultiLinesOxyPlotViewModel("PM2.5");
            Pm10 = new MultiLinesOxyPlotViewModel("PM10");
            Voc = new MultiLinesOxyPlotViewModel("VOC", 0, 21);
            Mics = new MultiLinesOxyPlotViewModel("Mics", 0, 6);
            Cjmcu = new MultiLinesOxyPlotViewModel("CJMCU", 0, 6);
            Mq = new MultiLinesOxyPlotViewModel("MQ" ,0 ,6);
            Hcho = new MultiLinesOxyPlotViewModel("HCHO", 0, 3);


            GraphState = true;
            GraphContent = "Stop";
            GraphCommand = new RelayCommand(GraphToggle);
            GraphClearCommand = new RelayCommand(ClearGraph);
        }

        private void ClearGraph()
        {
            Humidity.GraphClear();
            Temperature.GraphClear();
            Pm1_0.GraphClear();
            Pm2_5.GraphClear();
            Pm10.GraphClear();
            Voc.GraphClear();
            Cjmcu.GraphClear();
            Mq.GraphClear();
            Hcho.GraphClear();
        }

        private void GraphToggle()
        {
            if (GraphState)
            {
                GraphState = !GraphState;
                GraphContent = "Live";
            }
            else
            {
                GraphState = !GraphState;
                GraphContent = "Stop";
            }
        }

        public void GetData(string readData)
        {
            string[] splitData = readData.Split('/');
           
            if ((splitData.Length >= 11) && (splitData.Length <= 12))
            {

            }
            else
            {
                TimerViewModel.TimerContent = "Port 오류";
                return;
            }


            double[] doubleSplitdata = new double[splitData.Length];
            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    doubleSplitdata[i] = double.Parse(splitData[i]);

                } catch (Exception e) 
                {
                    TimerViewModel.TimerContent = "???";
                    return;
                }
                
            }

            if (doubleSplitdata[4] < 1000)
            {

            }
            else
            {
                TimerViewModel.TimerContent = "데이터값 오류";
                return;
            }


            int sensorNum = 0;
            
            switch (splitData[0]) 
            {
                case "A":
                    sensorNum = (int)MultiLinesOxyPlotViewModel.SensorNum.A;
                    break;
                case "B":
                    sensorNum = (int)MultiLinesOxyPlotViewModel.SensorNum.B;
                    break;
                case "C":
                    sensorNum = (int)MultiLinesOxyPlotViewModel.SensorNum.C;
                    break;
                case "D":
                    sensorNum = (int)MultiLinesOxyPlotViewModel.SensorNum.D;
                    break;
                case "E":
                    sensorNum = (int)MultiLinesOxyPlotViewModel.SensorNum.E;
                    break;
                case "F":
                    sensorNum = (int)MultiLinesOxyPlotViewModel.SensorNum.F;
                    break;
                case "G":
                    sensorNum = (int)MultiLinesOxyPlotViewModel.SensorNum.G;
                    break;
                case "H":
                    sensorNum = (int)MultiLinesOxyPlotViewModel.SensorNum.H;
                    break;
            }

            Humidity.GraphUpdata(doubleSplitdata[1], GraphState, sensorNum);
            Temperature.GraphUpdata(doubleSplitdata[2], GraphState, sensorNum);
            Pm1_0.GraphUpdata(doubleSplitdata[3], GraphState, sensorNum);
            Pm2_5.GraphUpdata(doubleSplitdata[4], GraphState, sensorNum);
            Pm10.GraphUpdata(doubleSplitdata[5], GraphState, sensorNum);
            Voc.GraphUpdata(doubleSplitdata[6] * 0.001, GraphState, sensorNum);
            Mics.GraphUpdata(doubleSplitdata[7], GraphState, sensorNum);
            Cjmcu.GraphUpdata(doubleSplitdata[8], GraphState, sensorNum);
            Mq.GraphUpdata(doubleSplitdata[9], GraphState, sensorNum);
            Hcho.GraphUpdata(doubleSplitdata[10], GraphState, sensorNum);

            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yy/MM/dd HH:mm:ss");

            if (CsvViewModel.CsvState)
            {
                CsvViewModel.CsvAdd(formattedTime, readData);
            }

            if (DatabaseViewModel.MysqlState)
            {
                DatabaseViewModel.DatabaseAdd(formattedTime, readData, true);
            }
        }
    }
}
