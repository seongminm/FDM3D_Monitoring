﻿using MonitoringSensor.Services;
using MonitoringSensor.ViewModels;
using MonitoringSensor.ViewModels.Command;
using System;
using System.ComponentModel;

namespace MonitoringSensor.Views.ThirdTabView
{
    class ThirdTabViewModel : ViewModelBase
    {
        public SerialViewModel SerialViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public GetDataService getDataService;
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand Send1Command { get; set; }
        public RelayCommand Send2Command { get; set; }
        
        private string text;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }
        private string sentText;
        public string SentText
        {
            get => sentText;
            set => SetProperty(ref sentText, value);
        }
        private string textbox1;
        public string TextBox1
        {
            get => textbox1;
            set => SetProperty(ref textbox1, value);
        }
        private string textBox2;
        public string TextBox2
        {
            get => textBox2;
            set => SetProperty(ref textBox2, value);
        }

        public ThirdTabViewModel()
        {
            TimerViewModel = new TimerViewModel();
            getDataService = new GetDataService();

            SerialViewModel = new SerialViewModel(TimerViewModel, getDataService);
            getDataService.Method += DataReceived;
            ClearCommand = new RelayCommand(Clear);
            Send1Command = new RelayCommand(SendSerial1);
            Send2Command = new RelayCommand(SendSerial2);

            TextBox1 = "";
            TextBox2 = "";

        }

        private void DataReceived()
        {
            Text += getDataService.StringData + "\n";
        }

        private void SendSerial1()
        {
            if(!SerialViewModel.SerialState)
            {
                SerialViewModel.SendSerial(TextBox1);
                SentText += TextBox1 + Environment.NewLine;
            }
        }

        private void SendSerial2()
        {
            if (!SerialViewModel.SerialState)
            {
                SerialViewModel.SendSerial(TextBox2);
                SentText += TextBox2 + Environment.NewLine;
            }
             
        }

        private void Clear()
        {
            Text = "";
            SentText = "";
            TextBox1 = "";
            TextBox2 = "";
        }

       

    }
}
