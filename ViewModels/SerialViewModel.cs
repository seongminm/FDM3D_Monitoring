using MonitoringSensor.Services;
using MonitoringSensor.ViewModels.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MonitoringSensor.ViewModels
{
    class SerialViewModel : ViewModelBase
    {
        private RelayCommand serialCommand;
        public RelayCommand SerialCommand
        {
            get => serialCommand;
            set => SetProperty(ref serialCommand, value);
           
        }
        public RelayCommand SerialPortCommand { get; set; }
        private string serialContent;
        public string SerialContent
        {
            get => serialContent;
            set => SetProperty(ref serialContent, value);
        }
        private bool serialState;
        public bool SerialState
        {
            get => serialState;
            set => SetProperty(ref serialState, value);
        }
        private string selectedSerialPort;
        public string SelectedSerialPort
        {
            get => selectedSerialPort;
            set => SetProperty(ref selectedSerialPort, value);
        }

        private string selectedSerialBaudRate;
        public string SelectedSerialBaudRate
        {
            get => selectedSerialBaudRate;
            set => SetProperty(ref selectedSerialBaudRate, value);
        }
        private List<string> serialPorts;
        public List<string> SerialPorts
        {
            get => serialPorts;
            set => SetProperty(ref serialPorts, value);

        }
        public List<int> SerialBaudRate { get; set; }


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
            SerialPortCommand = new RelayCommand(LoadSerial);

        }

        private void LoadSerial()
        {
            serialService.LoadSerialPorts();
            SerialPorts = serialService.SerialPorts;
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

        public void SendSerial(string message)
        {
            if(serialService.isOpen())
            {
                serialService.SendSerial(message);
            }
        }

    }
}
