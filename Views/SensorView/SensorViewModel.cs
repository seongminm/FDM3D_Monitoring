using MonitoringSensor.Services;
using MonitoringSensor.ViewModels;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringSensor.Views.SensorView
{
    internal class SensorViewModel : ViewModelBase, IGetDataService
    {
        public SerialViewModel SerialViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public CsvViewModel CsvViewModel { get; set; }
        public DatabaseViewModel DatabaseViewModel { get; set; }

        private double temperature;
        public double Temperature
        {
            get => temperature;
            set => SetProperty(ref temperature, value);
        }

        private double humidity;
        public double Humidity
        {
            get => humidity;
            set => SetProperty(ref humidity, value);
        }

        private double pm1_0;
        public double Pm1_0
        {
            get => pm1_0;
            set => SetProperty(ref pm1_0, value);
        }

        private double pm2_5;
        public double Pm2_5
        {
            get => pm2_5;
            set => SetProperty(ref pm2_5, value);
        }

        private double pm10;
        public double Pm10
        {
            get => pm10;
            set => SetProperty(ref pm10, value);
        }

        private double voc;
        public double Voc
        {
            get => voc;
            set => SetProperty(ref voc, value);
        }

        private double mics;
        public double Mics
        {
            get => mics;
            set => SetProperty(ref mics, value);
        }

        private double cjmcu;
        public double Cjmcu
        {
            get => cjmcu;
            set => SetProperty(ref cjmcu, value);
        }

        private double mq;
        public double Mq
        {
            get => mq;
            set => SetProperty(ref mq, value);
        }

        private double hcho;
        public double Hcho
        {
            get => hcho;
            set => SetProperty(ref hcho, value);
        }

       


        public SensorViewModel()
        {
            TimerViewModel = new TimerViewModel();

            string line = $"{"Time"},{"Humidity"},{"Temperature"}, {"PM1.0"}, {"PM2.5"}, {"PM10"}, {"VOC"}, {"MICS"}, {"CJMCU"}, {"MQ"}, {"HCHO"}";

            CsvViewModel = new CsvViewModel(line);
            DatabaseViewModel = new DatabaseViewModel();

            SerialViewModel = new SerialViewModel(TimerViewModel, this);
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

            Humidity = double.Parse(splitData[0]);
            Temperature = double.Parse(splitData[1]);
            Pm1_0 = double.Parse(splitData[2]);
            Pm2_5 = double.Parse(splitData[3]);
            Pm10 = double.Parse(splitData[4]);
            Voc = double.Parse(splitData[5]);
            Mics = double.Parse(splitData[6]);
            Cjmcu = double.Parse(splitData[7]);
            Mq = double.Parse(splitData[8]);
            Hcho = double.Parse(splitData[9]);
           

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
