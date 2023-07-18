using MonitoringSensor.Services;
using MonitoringSensor.ViewModels.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MonitoringSensor.ViewModels
{
    class SerialViewModel : INotifyPropertyChanged
    {
        private RelayCommand serialCommand;
        public RelayCommand SerialCommand
        {
            get { return serialCommand; }
            set
            {
                serialCommand = value;
                OnPropertyChanged(nameof(SerialCommand));
            }
        }

        private string _serialContent;
        public string SerialContent
        {
            get { return _serialContent; }
            set
            {
                _serialContent = value;
                OnPropertyChanged(nameof(SerialContent));
            }
        }
        private bool _serialState;
        public bool SerialState
        {
            get { return _serialState; }
            set
            {
                _serialState = value;
                OnPropertyChanged(nameof(SerialState));
            }
        }
        private string _selectedSerialPort;
        public string SelectedSerialPort
        {
            get { return _selectedSerialPort; }
            set
            {
                _selectedSerialPort = value;
                OnPropertyChanged(nameof(SelectedSerialPort));
            }
        }

        private string _selectedSerialBaudRate;
        public string SelectedSerialBaudRate
        {
            get { return _selectedSerialBaudRate; }
            set
            {
                _selectedSerialBaudRate = value;
                OnPropertyChanged(nameof(SelectedSerialBaudRate));
            }
        }
        public ObservableCollection<string> SerialPorts { get; set; }
        public ObservableCollection<int> SerialBaudRate { get; set; }


        private SerialService serialService;
        private GetDataService getDataService;

        TimerViewModel timerViewModel;

        public SerialViewModel(TimerViewModel timerViewModel, GetDataService getDataService)
        {

            this.getDataService = getDataService;
            this.timerViewModel = timerViewModel;

            serialService = new SerialService(this.getDataService);
            SerialCommand = new RelayCommand(OpenSerial);
            SerialContent = "Open";
            SerialState = true;
            SerialPorts = serialService.SerialPorts;
            SerialBaudRate = serialService.SerialBaudRate;

        }

        private void OpenSerial()
        {

            serialService.OpenSerial(SelectedSerialPort, SelectedSerialBaudRate);
            if (serialService.isOpen())
            {
                SerialCommand = new RelayCommand(CloseSerial);
                SerialContent = "Close";
                SerialState = false;
                timerViewModel.Start();
            }
        }

        public void CloseSerial()
        {
            serialService.CloseSerial();
            if (!serialService.isOpen())
            {
                SerialCommand = new RelayCommand(OpenSerial);
                SerialContent = "Open";
                SerialState = true;
                timerViewModel.Stop();
            }
        }

        public void NullMemeory()
        {
            if (!SerialState)
            {
                serialService.CloseSerial();
            }
            serialService = null;
            timerViewModel = null;
            getDataService = null;
            SerialCommand = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
