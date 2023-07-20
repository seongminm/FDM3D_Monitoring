﻿using MonitoringSensor.Services;
using MonitoringSensor.ViewModels;
using MonitoringSensor.ViewModels.Command;
using System;
using System.ComponentModel;

namespace MonitoringSensor.Views.FourthTabView
{
    class FourthTabViewModel : INotifyPropertyChanged
    {
        public UdpViewModel UdpViewModel { get; set; }
        public TimerViewModel TimerViewModel { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand Send1Command { get; set; }
        public RelayCommand Send2Command { get; set; }

        public GetDataService getDataService;

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        private string sentText;
        public string SentText
        {
            get { return sentText; }
            set
            {
                sentText = value;
                OnPropertyChanged(nameof(SentText));
            }
        }
        private string textbox1;
        public string TextBox1
        {
            get { return textbox1; }
            set
            {
                textbox1 = value;
                OnPropertyChanged(nameof(TextBox1));
            }
        }
        private string textBox2;
        public string TextBox2
        {
            get { return textBox2; }
            set
            {
                textBox2 = value;
                OnPropertyChanged(nameof(TextBox2));
            }
        }
        public FourthTabViewModel()
        {
            TimerViewModel = new TimerViewModel();
            getDataService = new GetDataService();

            UdpViewModel = new UdpViewModel(TimerViewModel, getDataService);
            getDataService.Method += DataReceived;

            ClearCommand = new RelayCommand(Clear);
            Send1Command = new RelayCommand(SendUdp1);
            Send2Command = new RelayCommand(SendUdp2);

            TextBox1 = "";
            TextBox2 = "";
        }

        private void SendUdp2()
        {
            if (!UdpViewModel.UdpState)
            {
                UdpViewModel.SendUdp(TextBox2);
                SentText += TextBox2 + Environment.NewLine;
            }
        }

        private void SendUdp1()
        {
            if (!UdpViewModel.UdpState)
            {
                UdpViewModel.SendUdp(TextBox1);
                SentText += TextBox1 + Environment.NewLine;
            }
        }

        private void Clear()
        {
            Text = "";
            SentText = "";
            TextBox1 = "";
            TextBox2 = "";
        }

        private void DataReceived()
        {
            Text += getDataService.StringData + Environment.NewLine;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
