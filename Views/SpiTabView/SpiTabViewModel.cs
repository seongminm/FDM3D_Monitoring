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
        public MultiLinesOxyPlotViewModel SensorA { get; set; }
        public MultiLinesOxyPlotViewModel SensorB { get; set; }
        public MultiLinesOxyPlotViewModel SensorC { get; set; }

        public MultiLinesOxyPlotViewModel SensorD { get; set; }

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
            SensorA = new MultiLinesOxyPlotViewModel("A-Sensor");
            SensorB = new MultiLinesOxyPlotViewModel("B-Sensor");
            SensorC = new MultiLinesOxyPlotViewModel("C-Sensor");
            SensorD = new MultiLinesOxyPlotViewModel("D-Sensor");


            GraphState = true;
            GraphContent = "Stop";
            GraphCommand = new RelayCommand(GraphToggle);
            GraphClearCommand = new RelayCommand(ClearGraph);
        }

        private void ClearGraph()
        {
            SensorA.GraphClear();
            SensorB.GraphClear();
            SensorC.GraphClear();
            SensorD.GraphClear();

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

        public void GetData()
        {
            string data = SerialViewModel.GetData;
            string[] splitData = data.Split('/');

            if ((splitData.Length >= 11) && (splitData.Length <= 12))
            {

            }
            else
            {
                TimerViewModel.TimerContent = "Port 오류";
                return;
            }

            bool bl = double.TryParse(splitData[1], out double result);
            if ((double.Parse(splitData[4]) < 1000) && bl)
            {

            }
            else
            {
                TimerViewModel.TimerContent = "데이터값 오류";
                return;
            }

            switch (splitData[0]) 
            {
                case "A": SensorA.GraphUpdata(data, GraphState); break;
                case "B": SensorB.GraphUpdata(data, GraphState); break;
                case "C": SensorC.GraphUpdata(data, GraphState); break;
                case "D": SensorD.GraphUpdata(data, GraphState); break;
            }


            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yy/MM/dd HH:mm:ss");

            if (CsvViewModel.CsvState)
            {
                CsvViewModel.CsvAdd(formattedTime, data);
            }

            if (DatabaseViewModel.MysqlState)
            {
                DatabaseViewModel.DatabaseAdd(formattedTime, data, true);
            }
        }
    }
}
