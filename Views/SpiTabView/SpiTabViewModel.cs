using MonitoringSensor.Services;
using MonitoringSensor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringSensor.Views.SpiTabView
{
    class SpiTabViewModel : ViewModelBase
    {
        public SerialViewModel SerialViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public CsvViewModel CsvViewModel { get; set; }
        public DatabaseViewModel DatabaseViewModel { get; set; }
        public GetDataService getDataService;


        private double dataCount = 1;

        public SpiTabViewModel()
        {
            TimerViewModel = new TimerViewModel();
            getDataService = new GetDataService();

            string line = $"{"Time"},{"Type"},{"Humidity"},{"Temperature"}, {"PM1.0"}, {"PM2.5"}, {"PM10"}, {"VOC"}, {"MICS"}, {"CJMCU"}, {"MQ"}, {"HCHO"}";

            CsvViewModel = new CsvViewModel(line);
            DatabaseViewModel = new DatabaseViewModel();

            SerialViewModel = new SerialViewModel(TimerViewModel, getDataService);
            getDataService.Method += DataReceived;

        }

        private void DataReceived()
        {

            string data = getDataService.StringData;
            string[] splitData = data.Split('/');

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

         

            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yy/MM/dd HH:mm:ss");

            if (CsvViewModel.CsvState)
            {
                CsvViewModel.CsvAdd(formattedTime, data);
            }

            if (DatabaseViewModel.MysqlState)
            {
                DatabaseViewModel.DatabaseAdd(formattedTime, data);
            }


        }

    }
}
