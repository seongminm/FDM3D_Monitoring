﻿using MonitoringSensor.Services;
using MonitoringSensor.ViewModels.Command;
using MonitoringSensor.ViewModels;
using System;

namespace MonitoringSensor.Views.FirstTabView
{
    internal class FirstTapViewModel : ViewModelBase, IGetDataService
    {
        public SerialViewModel SerialViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public OxyPlotViewModel Humidity { get; set; }
        public OxyPlotViewModel Temperature { get; set; }
        public OxyPlotViewModel Pm1_0 { get; set; }
        public OxyPlotViewModel Pm2_5 { get; set; }
        public OxyPlotViewModel Pm10 { get; set; }
        public OxyPlotViewModel Pid { get; set; }
        public OxyPlotViewModel Mics { get; set; }
        public OxyPlotViewModel Cjmcu { get; set; }
        public OxyPlotViewModel Mq { get; set; }
        public OxyPlotViewModel Hcho { get; set; }

        public CsvViewModel CsvViewModel { get; set; }
        public DatabaseViewModel DatabaseViewModel { get; set; }



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

        private double dataCount = 1;

        public FirstTapViewModel()
        {
            TimerViewModel = new TimerViewModel();

            Humidity = new OxyPlotViewModel();
            Temperature = new OxyPlotViewModel();
            Pm1_0 = new OxyPlotViewModel();
            Pm2_5 = new OxyPlotViewModel();
            Pm10 = new OxyPlotViewModel();
            Pid = new OxyPlotViewModel();
            Mics = new OxyPlotViewModel();
            Cjmcu = new OxyPlotViewModel();
            Mq = new OxyPlotViewModel();
            Hcho = new OxyPlotViewModel();

            string line = $"{"Time"},{"Humidity"},{"Temperature"}, {"PM1.0"}, {"PM2.5"}, {"PM10"}, {"VOC"}, {"MICS"}, {"CJMCU"}, {"MQ"}, {"HCHO"}";

            CsvViewModel = new CsvViewModel(line);
            DatabaseViewModel = new DatabaseViewModel();

            SerialViewModel = new SerialViewModel(TimerViewModel, this);

            GraphState = true;
            GraphContent = "Stop";
            GraphCommand = new RelayCommand(GraphToggle);
            GraphClearCommand = new RelayCommand(ClearGraph);

        }



        private void ClearGraph()
        {
            dataCount = 1;
            Humidity.GrahpClear();
            Temperature.GrahpClear();
            Pm1_0.GrahpClear();
            Pm2_5.GrahpClear();
            Pm10.GrahpClear();
            Pid.GrahpClear();
            Mics.GrahpClear();
            Cjmcu.GrahpClear();
            Mq.GrahpClear();
            Hcho.GrahpClear();
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

            if ((splitData.Length >= 10) && (splitData.Length <= 11))
            {

            }
            else
            {
                TimerViewModel.TimerContent = "Port 오류";
                return;
            }

            bool bl = double.TryParse(splitData[0], out double result);
            if ((double.Parse(splitData[3]) < 1000) && bl)
            {

            }
            else
            {
                TimerViewModel.TimerContent = "데이터값 오류";
                return;
            }

            Humidity.Output = double.Parse(splitData[0]);
            Humidity.GrpahUpdate(dataCount, GraphState);
            Temperature.Output = double.Parse(splitData[1]);
            Temperature.GrpahUpdate(dataCount, GraphState);
            Pm1_0.Output = double.Parse(splitData[2]);
            Pm1_0.GrpahUpdate(dataCount, GraphState);
            Pm2_5.Output = double.Parse(splitData[3]);
            Pm2_5.GrpahUpdate(dataCount, GraphState);
            Pm10.Output = double.Parse(splitData[4]);
            Pm10.GrpahUpdate(dataCount, GraphState);
            Pid.Output = double.Parse(splitData[5]);
            Pid.GrpahUpdate(dataCount, GraphState);
            Mics.Output = double.Parse(splitData[6]);
            Mics.GrpahUpdate(dataCount, GraphState);
            Cjmcu.Output = double.Parse(splitData[7]);
            Cjmcu.GrpahUpdate(dataCount, GraphState);
            Mq.Output = double.Parse(splitData[8]);
            Mq.GrpahUpdate(dataCount, GraphState);
            Hcho.Output = double.Parse(splitData[9]);
            Hcho.GrpahUpdate(dataCount, GraphState);
            dataCount++;

            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yy/MM/dd HH:mm:ss");

            if (CsvViewModel.CsvState)
            {
                CsvViewModel.CsvAdd(formattedTime, readData);
            }

            if (DatabaseViewModel.MysqlState)
            {
                DatabaseViewModel.DatabaseAdd(formattedTime, readData);
            }
        }

    }
}
   